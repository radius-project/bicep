// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Parsing;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.Semantics.Metadata;
using Bicep.Core.Syntax;
using Bicep.Core.TypeSystem;
using Bicep.Core.TypeSystem.Radius;

namespace Bicep.Core.Emit
{
    public class ResourceRewriter
    {
        private static readonly AncestorItem RadiusCustomProvider = new AncestorItem("radius", ResourceTypeReference.Parse("Microsoft.CustomProviders/resourceProviders@2018-09-01-preview"));

        public static ImmutableArray<ResourceMetadata> Transform(
            SemanticModel semanticModel,
            ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<ResourceDependency>> resourceDependencies)
        {
            var rewriter = new ResourceRewriter(semanticModel, resourceDependencies);
            var items = rewriter.TransformResources(semanticModel.AllResources.ToArray());

            return items.ToImmutableArray();
        }

        public ResourceRewriter(SemanticModel semanticModel, ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<ResourceDependency>> resourceDependencies)
        {
            this.SemanticModel = semanticModel;
            this.ResourceDependencies = resourceDependencies;
        }

        public SemanticModel SemanticModel { get; }

        public ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<ResourceDependency>> ResourceDependencies { get; }

        private IEnumerable<ResourceMetadata> TransformResources(ResourceMetadata[] resources)
        {
            var applications = GroupByApplication(resources);
            foreach (var (application, (components, deployments)) in applications)
            {
                var orphans = components.Where(c => !deployments.Any(d => DeploymentIncludesComponent(d, c))).ToArray();
                if (orphans.Length > 0)
                {
                    yield return CreateSynthesizedDeployment(application, orphans);
                }
            }
        }

        private static bool IsRadiusType(ResourceMetadata resource)
        {
            var type = resource.Symbol.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
            return type.Namespace == "radius.dev";
        }

        private static bool IsRadiusApplicationType(ResourceMetadata resource)
        {
            var type = resource.Symbol.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
            return type.Namespace == "radius.dev" && type.TypesString == "Applications";
        }

        private static bool IsRadiusComponentType(ResourceMetadata resource)
        {
            var type = resource.Symbol.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
            return type.Namespace == "radius.dev" && type.TypesString == "Applications/Components";
        }

        private static bool IsRadiusDeploymentType(ResourceMetadata resource)
        {
            var type = resource.Symbol.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
            return type.Namespace == "radius.dev" && type.TypesString == "Applications/Deployments";
        }

        private ResourceMetadata CreateSynthesizedDeployment(ResourceMetadata application, ResourceMetadata[] components)
        {
            var references = components.OrderBy(c => c.Symbol.Name).Select(c => c.Symbol.UnsafeGetBodyProperty("name").Value).ToArray();

            var body = SyntaxFactory.CreateObject(new []
                {
                    SyntaxFactory.CreateObjectProperty(
                        "name",
                        SyntaxFactory.CreateStringLiteral("default")),
                    SyntaxFactory.CreateObjectProperty(
                        "properties",
                        SyntaxFactory.CreateObject(new []
                        {
                            SyntaxFactory.CreateObjectProperty(
                                "components",
                                SyntaxFactory.CreateArray(references.Select(reference =>
                                {
                                    return SyntaxFactory.CreateObject(new []
                                    {
                                        SyntaxFactory.CreateObjectProperty("componentName", reference),
                                    });
                                }).ToArray())),
                        }))
                });

            var declaration = new ResourceDeclarationSyntax(
                Array.Empty<SyntaxBase>(),
                 new Token(TokenType.Identifier, new TextSpan(0, 0), "resource", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                 new IdentifierSyntax(new Token(TokenType.Identifier, new TextSpan(0, 0), "generated_deployment", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>())),
                 SyntaxFactory.CreateString(new[]{"Deployments@v1alpha1"}, Array.Empty<SyntaxBase>()),
                 existingKeyword: null,
                 assignment: new Token(TokenType.Assignment, new TextSpan(0, 0), "=", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                 body);

            var ancestors = ImmutableArray.Create<AncestorItem>(RadiusCustomProvider, new AncestorItem(application, null));

            var dependencies = ImmutableArray.Create(new DependencyItem(application, newContext: null!));
            dependencies = dependencies.AddRange(components.OrderBy(c => c.Symbol.Name).Select(c => new DependencyItem(c, newContext: null!)));

            return new ResourceMetadata(
                type: application.Type.Provider!.GetType(ResourceTypeReference.Parse("radius.dev/Applications/Deployments@v1alpha1"), ResourceTypeGenerationFlags.None),
                typeReference: ResourceTypeReference.Parse($"{RadiusResources.DeploymentCRPType}@{RadiusResources.CRPApiVersion}"),
                declaringSyntax: declaration,
                nameSyntax: body.SafeGetPropertyByName("name")!.Value,
                symbol: null!,
                parent: new ResourceMetadataParent(application, null, isNested: true),
                dependencies: components.Concat(new []{ application, }).Select(d => new ResourceDependencyMetadata(d.Symbol, indexExpression: null)),
                scopeSyntax: null,
                isExistingResource: false);
        }

        private ObjectSyntax EvaluateDecorators(StatementSyntax statement, ObjectSyntax input, TypeSymbol targetType)
        {
            var result = input;
            foreach (var decoratorSyntax in statement.Decorators.Reverse())
            {
                var symbol = this.SemanticModel.GetSymbolInfo(decoratorSyntax.Expression);

                if (symbol is FunctionSymbol decoratorSymbol)
                {
                    var argumentTypes = decoratorSyntax.Arguments
                        .Select(argument => this.SemanticModel.TypeManager.GetTypeInfo(argument))
                        .ToArray();

                    // There should be exact one matching decorator since there's no errors.
                    Decorator decorator = this.SemanticModel.Root.ImportedNamespaces
                        .SelectMany(ns => ns.Value.Type.DecoratorResolver.GetMatches(decoratorSymbol, argumentTypes))
                        .Single();

                    result = decorator.Evaluate(decoratorSyntax, targetType, result);
                }
            }

            return result;
        }


        private Dictionary<ResourceMetadata, (ResourceMetadata[] components, ResourceMetadata[] deployments)> GroupByApplication(ResourceMetadata[] resources)
        {
            var componentParents = new Dictionary<ResourceMetadata, List<ResourceMetadata>>();
            foreach (var component in resources.Where(IsRadiusComponentType))
            {
                var ancestors = this.SemanticModel.ResourceAncestors.GetAncestors(component.Symbol).Select(a => this.SemanticModel.ResourceMetadata.TryLookup(a.Resource.DeclaringSyntax)!).ToArray();
                if (ancestors.Length != 1)
                {
                    throw new InvalidOperationException("Expected the component to have an ancestor");
                }

                if (!IsRadiusApplicationType(ancestors[0]))
                {
                    throw new InvalidOperationException("Expected the component to have an application as ancestor");
                }

                if (!componentParents.TryGetValue(ancestors[0], out var application))
                {
                    application = new List<ResourceMetadata>();
                    componentParents.Add(ancestors[0], application);
                }

                application.Add(component);
            }

            var deploymentParents = new Dictionary<ResourceMetadata, List<ResourceMetadata>>();
            foreach (var deployment in resources.Where(IsRadiusDeploymentType))
            {
                var ancestors = this.SemanticModel.ResourceAncestors.GetAncestors(deployment.Symbol).Select(a => this.SemanticModel.ResourceMetadata.TryLookup(a.Resource.DeclaringSyntax)!).ToArray();
                if (ancestors.Length != 1)
                {
                    throw new InvalidOperationException("Expected the deployment to have an ancestor");
                }

                if (!IsRadiusApplicationType(ancestors[0]))
                {
                    throw new InvalidOperationException("Expected the deployment to have an application as ancestor");
                }

                if (!deploymentParents.TryGetValue(ancestors[0], out var application))
                {
                    application = new List<ResourceMetadata>();
                    deploymentParents.Add(ancestors[0], application);
                }

                application.Add(deployment);
            }

            return componentParents.ToDictionary(kvp => kvp.Key, kvp =>
            {
                componentParents.TryGetValue(kvp.Key, out var components);
                deploymentParents.TryGetValue(kvp.Key, out var deployments);

                return (components?.ToArray() ?? Array.Empty<ResourceMetadata>(), deployments?.ToArray() ?? Array.Empty<ResourceMetadata>());
            });
        }

        private bool DeploymentIncludesComponent(ResourceMetadata deployment, ResourceMetadata component)
        {
            // for now just track dependencies
            var dependencies = this.ResourceDependencies[deployment.Symbol];
            return dependencies.Any(d => d.Resource == component.Symbol);
        }
    }


    public class ResourceItem
    {
        public ResourceItem(
            ResourceSymbol? symbol,
            string name,
            ResourceTypeReference type,
            ImmutableArray<AncestorItem> ancestors,
            ImmutableArray<DependencyItem> dependencies,
            bool isExistingResource,
            ObjectSyntax body,
            ImmutableArray<SyntaxBase> conditions,
            ImmutableArray<LoopItem> loops,
            ScopeItem? scope)
        {
            this.Symbol = symbol;
            this.Name = name;
            this.Type = type;
            this.Ancestors = ancestors;
            this.Dependencies = dependencies;
            this.IsExistingResource = isExistingResource;
            this.Body = body;
            this.Conditions = conditions;
            this.Loops = loops;
            this.Scope = scope;
        }

        public ResourceSymbol? Symbol { get; }

        public string Name { get; }

        public ResourceTypeReference Type { get; }

        public ImmutableArray<AncestorItem> Ancestors { get; }

        public ImmutableArray<DependencyItem> Dependencies { get; }

        public bool IsExistingResource { get; }

        public ObjectSyntax Body { get; }

        public ImmutableArray<SyntaxBase> Conditions { get; }

        public ImmutableArray<LoopItem> Loops { get; }

        public ScopeItem? Scope { get; }
    }

    public class LoopItem
    {
        public LoopItem(string name, ForSyntax @for, SyntaxBase? input, long? batchSize)
        {
            this.Name = name;
            this.For = @for;
            this.Input = input;
            this.BatchSize = batchSize;
        }

        public string Name { get; }

        public ForSyntax For { get; }

        public SyntaxBase? Input { get; }

        public long? BatchSize { get; }
    }

    public class ScopeItem
    {
        public ScopeItem(ResourceSymbol scopeResource, SyntaxBase? indexExpression)
        {
            this.ScopeResource = scopeResource;
            this.IndexExpression = indexExpression;
        }

        public ResourceSymbol ScopeResource { get; }

        public SyntaxBase? IndexExpression { get; }
    }

    public class AncestorItem
    {
        public AncestorItem(ResourceMetadata ancestor, SyntaxBase? indexExpression)
        {
            this.Ancestor = ancestor;
            this.IndexExpression = indexExpression;
            this.TypeReference = ancestor.Symbol.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
        }

        public AncestorItem(string ancestorName, ResourceTypeReference typeReference)
        {
            this.AncestorName = ancestorName;
            this.TypeReference = typeReference;
        }

        public SyntaxBase? IndexExpression { get; }

        public ResourceMetadata? Ancestor { get; }

        public string? AncestorName { get; }

        public ResourceTypeReference TypeReference { get; }
    }

    public class DependencyItem
    {
        public DependencyItem(ResourceMetadata resource, SyntaxBase newContext, bool existing = false)
        {
            this.Resource = resource;
            this.NewContext = newContext;
            this.Existing = existing;
        }

        public DependencyItem(ResourceMetadata resource, SyntaxBase? indexExpression, SyntaxBase newContext, bool existing = false)
        {
            this.Resource = resource;
            this.IndexExpression = indexExpression;
            this.NewContext = newContext;
            this.Existing = existing;
        }

        public ResourceMetadata Resource { get; }

        public SyntaxBase? IndexExpression { get; }

        public SyntaxBase NewContext { get; }

        public bool Existing { get; }
    }
}
