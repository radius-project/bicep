// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using Bicep.Core.TypeSystem;
using Bicep.Core.TypeSystem.Radius;

namespace Bicep.Core.Emit
{
    public class ResourceRewriter
    {
        private static readonly AncestorItem RadiusCustomProvider = new AncestorItem("radius", ResourceTypeReference.Parse("Microsoft.CustomProviders/resourceProviders@2018-09-01-preview"));

        public static ImmutableArray<ResourceItem> Transform(
            SemanticModel semanticModel,
            ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<ResourceDependency>> resourceDependencies)
        {
            var rewriter = new ResourceRewriter(semanticModel, resourceDependencies);
            var items = rewriter.TransformResources(semanticModel.Root.GetAllResourceDeclarations().ToArray());
            return items.ToImmutableArray();
        }

        public ResourceRewriter(SemanticModel semanticModel, ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<ResourceDependency>> resourceDependencies)
        {
            this.SemanticModel = semanticModel;
            this.ResourceDependencies = resourceDependencies;
        }

        public SemanticModel SemanticModel { get; }

        public ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<ResourceDependency>> ResourceDependencies { get; }

        private IEnumerable<ResourceItem> TransformResources(ResourceSymbol[] resources)
        {
            foreach (var resource in resources)
            {
                yield return CreateItem(resource);
            }

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

        private static bool IsRadiusType(ResourceSymbol resource)
        {
            var type = resource.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
            return type.Namespace == "radius.dev";
        }

        private static bool IsRadiusApplicationType(ResourceSymbol resource)
        {
            var type = resource.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
            return type.Namespace == "radius.dev" && type.TypesString == "Applications";
        }

        private static bool IsRadiusComponentType(ResourceSymbol resource)
        {
            var type = resource.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
            return type.Namespace == "radius.dev" && type.TypesString == "Applications/Components";
        }

        private static bool IsRadiusDeploymentType(ResourceSymbol resource)
        {
            var type = resource.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
            return type.Namespace == "radius.dev" && type.TypesString == "Applications/Deployments";
        }

        private ResourceItem CreateItem(ResourceSymbol resource)
        {
            if (IsRadiusApplicationType(resource))
            {
                return CreateApplicationItem(resource);
            }

            if (IsRadiusComponentType(resource))
            {
                return CreateComponentItem(resource);
            }

            if (IsRadiusDeploymentType(resource))
            {
                return CreateDeploymentItem(resource);
            }

            return CreateDefaultItem(resource);
        }

        private ResourceItem CreateApplicationItem(ResourceSymbol resource)
        {
            var (body, conditions, loops) = UnwrapLoopsAndConditions(resource);
            var ancestors = GetAncestors(resource);
            var dependencies = GetDependencies(resource, body);
            var scope = GetScope(resource);

            ancestors = ancestors.Insert(0, RadiusCustomProvider);

            return new ResourceItem(
                symbol: resource,
                name: resource.Name,
                type: ResourceTypeReference.Parse(RadiusResources.ApplicationType + "@" + RadiusResources.ApiVersion),
                ancestors: ancestors,
                dependencies: dependencies,
                isExistingResource: resource.DeclaringResource.IsExistingResource(),
                body: body,
                conditions: conditions,
                loops: loops,
                scope: scope);
        }

        private ResourceItem CreateComponentItem(ResourceSymbol resource)
        {
            var (body, conditions, loops) = UnwrapLoopsAndConditions(resource);
            var ancestors = GetAncestors(resource);
            var dependencies = GetDependencies(resource, body);
            var scope = GetScope(resource);

            ancestors = ancestors.Insert(0, RadiusCustomProvider);

            return new ResourceItem(
                symbol: resource,
                name: resource.Name,
                type: ResourceTypeReference.Parse(RadiusResources.ComponentType + "@" + RadiusResources.ApiVersion),
                ancestors: ancestors,
                dependencies: dependencies,
                isExistingResource: resource.DeclaringResource.IsExistingResource(),
                body: body,
                conditions: conditions,
                loops: loops,
                scope: scope);
        }

        private ResourceItem CreateDeploymentItem(ResourceSymbol resource)
        {
            var (body, conditions, loops) = UnwrapLoopsAndConditions(resource);
            var ancestors = GetAncestors(resource);
            var dependencies = GetDependencies(resource, body);
            var scope = GetScope(resource);

            ancestors = ancestors.Insert(0, RadiusCustomProvider);

            return new ResourceItem(
                symbol: resource,
                name: resource.Name,
                type: ResourceTypeReference.Parse(RadiusResources.DeploymentType + "@" + RadiusResources.ApiVersion),
                ancestors: ancestors,
                dependencies: dependencies,
                isExistingResource: resource.DeclaringResource.IsExistingResource(),
                body: body,
                conditions: conditions,
                loops: loops,
                scope: scope);
        }

        private ResourceItem CreateDefaultItem(ResourceSymbol resource)
        {
            var type = resource.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
            var (body, conditions, loops) = UnwrapLoopsAndConditions(resource);
            var ancestors = GetAncestors(resource);
            var dependencies = GetDependencies(resource, body);
            var scope = GetScope(resource);
            return new ResourceItem(
                symbol: resource,
                name: resource.Name,
                type: type,
                ancestors: ancestors,
                dependencies: dependencies,
                isExistingResource: resource.DeclaringResource.IsExistingResource(),
                body: body,
                conditions: conditions,
                loops: loops,
                scope: scope);
        }

        private ResourceItem CreateSynthesizedDeployment(ResourceSymbol application, ResourceSymbol[] components)
        {
            var references = components.OrderBy(c => c.Name).Select(c => c.UnsafeGetBodyProperty("name").Value).ToArray();

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

            var ancestors = ImmutableArray.Create<AncestorItem>(RadiusCustomProvider, new AncestorItem(application, null));

            var dependencies = ImmutableArray.Create(new DependencyItem(application, newContext: null!));
            dependencies = dependencies.AddRange(components.OrderBy(c => c.Name).Select(c => new DependencyItem(c, newContext: null!)));

            return new ResourceItem(
                symbol: null,
                name: "default_deployment",
                type: ResourceTypeReference.Parse(RadiusResources.DeploymentType + "@" + RadiusResources.ApiVersion),
                ancestors: ancestors,
                dependencies: dependencies,
                isExistingResource: false,
                body: body,
                conditions: ImmutableArray<SyntaxBase>.Empty,
                loops: ImmutableArray<LoopItem>.Empty,
                scope: null);
        }

        private ImmutableArray<AncestorItem> GetAncestors(ResourceSymbol resource)
        {
            var ancestors = this.SemanticModel.ResourceAncestors.GetAncestors(resource);
            return ancestors.Select(a => new AncestorItem(a.Resource, a.IndexExpression)).ToImmutableArray();
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

        private long? GetBatchSize(StatementSyntax decoratedSyntax)
        {
            var evaluated = this.EvaluateDecorators(decoratedSyntax, SyntaxFactory.CreateObject(Enumerable.Empty<ObjectPropertySyntax>()), LanguageConstants.Array);
            var batchSizeProperty = evaluated.SafeGetPropertyByName("batchSize");

            return batchSizeProperty switch
            {
                ObjectPropertySyntax { Value: IntegerLiteralSyntax integerLiteral } => integerLiteral.Value,
                _ => null
            };
        }

        private (ObjectSyntax body, ImmutableArray<SyntaxBase> conditions, ImmutableArray<LoopItem> loops) UnwrapLoopsAndConditions(ResourceSymbol resource)
        {
            // Note: conditions STACK with nesting.
            //
            // Children inherit the conditions of their parents, etc. This avoids a problem
            // where we emit a dependsOn to something that's not in the template, or not
            // being evaulated in the template.
            var conditions = ImmutableArray.CreateBuilder<SyntaxBase>();
            var loops = ImmutableArray.CreateBuilder<LoopItem>();

            var ancestors = this.SemanticModel.ResourceAncestors.GetAncestors(resource);
            foreach (var ancestor in ancestors)
            {
                if (ancestor.AncestorType == ResourceAncestorGraph.ResourceAncestorType.Nested &&
                    ancestor.Resource.DeclaringResource.Value is IfConditionSyntax ifCondition)
                {
                    conditions.Add(ifCondition.ConditionExpression);
                }

                if (ancestor.AncestorType == ResourceAncestorGraph.ResourceAncestorType.Nested &&
                    ancestor.Resource.DeclaringResource.Value is ForSyntax @for)
                {
                    var batchSize = GetBatchSize(resource.DeclaringResource);
                    loops.Add(new LoopItem(ancestor.Resource.Name, @for, null, batchSize));
                }
            }

            // Unwrap the 'real' resource body if there's a condition
            var body = resource.DeclaringResource.Value;
            switch (body)
            {
                case IfConditionSyntax ifCondition:
                    body = ifCondition.Body;
                    conditions.Add(ifCondition.ConditionExpression);
                    break;

                case ForSyntax @for:
                    var batchSize = GetBatchSize(resource.DeclaringResource);
                    loops.Add(new LoopItem(resource.Name, @for, null, batchSize));
                    if (@for.Body is IfConditionSyntax loopFilter)
                    {
                        body = loopFilter.Body;
                        conditions.Add(loopFilter.ConditionExpression);
                    }
                    else
                    {
                        body = @for.Body;
                    }

                    break;
            }


            return ((ObjectSyntax)body, conditions.ToImmutable(), loops.ToImmutable());
        }

        private ScopeItem? GetScope(ResourceSymbol resource)
        {
            if (this.SemanticModel.EmitLimitationInfo.ResourceScopeData.TryGetValue(resource, out var scopeData) && scopeData.ResourceScopeSymbol is { } scopeResource)
            {
                return new ScopeItem(scopeResource, scopeData.IndexExpression);
            }

            return null;
        }

        private ImmutableArray<DependencyItem> GetDependencies(ResourceSymbol symbol, SyntaxBase newContext)
        {
            var dependencies = this.ResourceDependencies[symbol];
            if (!dependencies.Any())
            {
                return ImmutableArray<DependencyItem>.Empty;
            }

            var results = ImmutableArray.CreateBuilder<DependencyItem>();

            // need to put dependencies in a deterministic order to generate a deterministic template
            foreach (var dependency in dependencies.OrderBy(x => x.Resource.Name))
            {
                switch (dependency.Resource)
                {
                    case ResourceSymbol resourceDependency:
                        if (resourceDependency.IsCollection && dependency.IndexExpression == null)
                        {
                            // dependency is on the entire resource collection
                            // write the name of the resource collection as the dependency
                            results.Add(new DependencyItem(resourceDependency, newContext));
                            break;
                        }

                        results.Add(new DependencyItem(
                            resourceDependency,
                            dependency.IndexExpression,
                            newContext,

                            // Quirk to avoid updating baselines in our fork
                            resourceDependency.DeclaringResource.IsExistingResource()));
                        break;

                    case ModuleSymbol moduleDependency:
                        if (moduleDependency.IsCollection && dependency.IndexExpression == null)
                        {
                            // dependency is on the entire module collection
                            // write the name of the module collection as the dependency
                            results.Add(new DependencyItem(moduleDependency, newContext));
                            break;
                        }

                        results.Add(new DependencyItem(moduleDependency, dependency.IndexExpression, newContext));
                        break;

                    default:
                        throw new InvalidOperationException($"Found dependency '{dependency.Resource.Name}' of unexpected type {dependency.GetType()}");
                }
            }

            return results.ToImmutable();
        }

        private Dictionary<ResourceSymbol, (ResourceSymbol[] components, ResourceSymbol[] deployments)> GroupByApplication(ResourceSymbol[] resources)
        {
            var componentParents = new Dictionary<ResourceSymbol, List<ResourceSymbol>>();
            foreach (var component in resources.Where(IsRadiusComponentType))
            {
                var ancestors = this.SemanticModel.ResourceAncestors.GetAncestors(component);
                if (ancestors.Length != 1)
                {
                    throw new InvalidOperationException("Expected the component to have an ancestor");
                }

                if (!IsRadiusApplicationType(ancestors[0].Resource))
                {
                    throw new InvalidOperationException("Expected the component to have an application as ancestor");
                }

                if (!componentParents.TryGetValue(ancestors[0].Resource, out var application))
                {
                    application = new List<ResourceSymbol>();
                    componentParents.Add(ancestors[0].Resource, application);
                }

                application.Add(component);
            }

            var deploymentParents = new Dictionary<ResourceSymbol, List<ResourceSymbol>>();
            foreach (var deployment in resources.Where(IsRadiusDeploymentType))
            {
                var ancestors = this.SemanticModel.ResourceAncestors.GetAncestors(deployment);
                if (ancestors.Length != 1)
                {
                    throw new InvalidOperationException("Expected the deployment to have an ancestor");
                }

                if (!IsRadiusApplicationType(ancestors[0].Resource))
                {
                    throw new InvalidOperationException("Expected the deployment to have an application as ancestor");
                }

                if (!deploymentParents.TryGetValue(ancestors[0].Resource, out var application))
                {
                    application = new List<ResourceSymbol>();
                    deploymentParents.Add(ancestors[0].Resource, application);
                }

                application.Add(deployment);
            }

            return componentParents.ToDictionary(kvp => kvp.Key, kvp =>
            {
                componentParents.TryGetValue(kvp.Key, out var components);
                deploymentParents.TryGetValue(kvp.Key, out var deployments);

                return (components?.ToArray() ?? Array.Empty<ResourceSymbol>(), deployments?.ToArray() ?? Array.Empty<ResourceSymbol>());
            });
        }

        private bool DeploymentIncludesComponent(ResourceSymbol deployment, ResourceSymbol component)
        {
            // for now just track dependencies
            var dependencies = this.ResourceDependencies[deployment];
            return dependencies.Any(d => d.Resource == component);
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
        public AncestorItem(ResourceSymbol ancestorSymbol, SyntaxBase? indexExpression)
        {
            this.ParentSymbol = ancestorSymbol;
            this.IndexExpression = indexExpression;
            this.TypeReference = ancestorSymbol.TryGetResourceTypeReference() ?? throw new InvalidOperationException("expected a valid resource type.");
        }

        public AncestorItem(string resourceName, ResourceTypeReference typeReference)
        {
            this.ParentName = resourceName;
            this.TypeReference = typeReference;
        }

        public SyntaxBase? IndexExpression { get; }

        public ResourceSymbol? ParentSymbol { get; }

        public string? ParentName { get; }

        public ResourceTypeReference TypeReference { get; }
    }

    public class DependencyItem
    {
        public DependencyItem(DeclaredSymbol symbol, SyntaxBase newContext, bool existing = false)
        {
            this.Symbol = symbol;
            this.NewContext = newContext;
            this.Existing = existing;
        }

        public DependencyItem(DeclaredSymbol symbol, SyntaxBase? indexExpression, SyntaxBase newContext, bool existing = false)
        {
            this.Symbol = symbol;
            this.IndexExpression = indexExpression;
            this.NewContext = newContext;
            this.Existing = existing;
        }

        public DeclaredSymbol Symbol { get; }

        public SyntaxBase? IndexExpression { get; }

        public SyntaxBase NewContext { get; }

        public bool Existing { get; }
    }
}
