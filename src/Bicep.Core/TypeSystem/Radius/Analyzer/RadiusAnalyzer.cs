// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Analyzers;
using Bicep.Core.Analyzers.Interfaces;
using Bicep.Core.CodeAction;
using Bicep.Core.Diagnostics;
using Bicep.Core.Semantics;
using Bicep.Core.Semantics.Metadata;
using Bicep.Core.Syntax;
using Microsoft.Extensions.Configuration;

namespace Bicep.Core.TypeSystem.Radius.Analyzer
{
    public class RadiusAnalyzer : IBicepAnalyzer
    {
        private readonly IBicepAnalyzerRule[] rules;

        public RadiusAnalyzer()
        {
            this.rules = new[]
            {
                new IncomingConnectionRule(),
            };
        }

        public IEnumerable<IDiagnostic> Analyze(SemanticModel model)
        {
            var diagnostics = new List<IDiagnostic>();
            foreach (var rule in this.rules)
            {
                diagnostics.AddRange(rule.Analyze(model));
            }

            return diagnostics;
        }

        public IEnumerable<IBicepAnalyzerRule> GetRuleSet()
        {
            return this.rules;
        }

        private class IncomingConnectionRule : IBicepAnalyzerRule
        {
            public string AnalyzerName => "Radius Analyzer";

            public string Code => "RAD9000";

            public string Description => "Add a connection when referencing a binding or route";

            public DiagnosticLevel DiagnosticLevel => DiagnosticLevel.Info;

            public DiagnosticLabel? DiagnosticLabel => null;

            public Uri? Uri => new Uri("https://zombo.com");

            public IEnumerable<IDiagnostic> Analyze(SemanticModel model)
            {
                var diagnostics = new List<AnalyzerFixableDiagnostic>();
                foreach (var resource in model.AllResources)
                {
                    if (resource.TypeReference.Namespace == "radius.dev" &&
                        resource.TypeReference.Types[0] == "Application" &&
                        resource.TypeReference.ApiVersion == "v1alpha3f")
                    {
                        VisitResource(diagnostics, model, resource);
                    }
                }

                return diagnostics;
            }

            public void Configure(IConfigurationRoot config)
            {
            }

            private void VisitResource(List<AnalyzerFixableDiagnostic> diagnostics, SemanticModel model, ResourceMetadata resource)
            {
                var isConnectionsPropertyDefined = false;
                if (resource.Symbol.SafeGetBodyPropertyValue("properties") is ObjectSyntax propertiesValue &&
                    model.GetDeclaredType(propertiesValue) is ObjectType propertiesType &&
                    propertiesType.Properties.TryGetValue("config", out var obj) && obj.TypeReference.Type is ObjectType configPropertyType &&
                    configPropertyType.Properties.TryGetValue("connections", out var connectionsProperty))
                {
                    isConnectionsPropertyDefined = true;
                }

                if (!isConnectionsPropertyDefined)
                {
                    return; // Not applicable
                }

                // Now we need to walk over the resource body and look for references to bindings & routes.
                var references = new List<ResourceSymbol>();

                var visitor = new Visitor(model);
                visitor.Visit(resource.DeclaringSyntax);

                // It's ok if this is null, we're using it to search for existing bindings
                ObjectSyntax? connectionsPropertySyntax = null;
                if ((resource.Symbol.SafeGetBodyPropertyValue("properties") as ObjectSyntax)?.SafeGetPropertyByNameRecursive(new []{ "config", "connections", }) is ObjectPropertySyntax temp &&
                    temp.Value is ObjectSyntax temp2)
                {
                    connectionsPropertySyntax = temp2;
                }

                foreach (var result in visitor.Results)
                {
                    var match = connectionsPropertySyntax != null && connectionsPropertySyntax.Properties.Any(p =>
                    {
                        return
                            p.Value is ObjectSyntax temp3 &&
                            temp3.SafeGetPropertyByName("source") is {} temp4 &&
                            temp4.Value is PropertyAccessSyntax temp5 &&
                            temp5.PropertyName.IdentifierName == "id" &&
                            temp5.BaseExpression is VariableAccessSyntax temp6 &&
                            model.GetSymbolInfo(temp6) == result.resourceSymbol;
                    });

                    if (!match)
                    {
                        diagnostics.Add(new AnalyzerFixableDiagnostic(
                            this.AnalyzerName,
                            result.syntax.Span,
                            this.DiagnosticLevel,
                            this.Code,
                            $"The route {result.resourceSymbol.Name} is not referenced as a connection of component {resource.Symbol.Name}.",
                            this.Uri,
                            this.DiagnosticLabel,
                            new []{ CreateAddConnectionCodeFix(resource, result.resourceSymbol, result.resourceType, result.syntax), }));
                    }
                }
            }

            private CodeFix CreateAddConnectionCodeFix(ResourceMetadata target, ResourceSymbol sourceSymbol, ResourceType sourceType, InstanceFunctionCallSyntax functionCall)
            {
                // We have to handle the case where neither config nor connections is defined. We'll just default to replacing the whole 'properties' property.
                // We'll walk down the chain to .properties.config.connections and then back up.
                var body = target.DeclaringSyntax.GetBody();
                var properties = body.SafeGetPropertyByName("properties")?.Value as ObjectSyntax ?? SyntaxFactory.CreateObject(Array.Empty<ObjectPropertySyntax>());
                var config = properties.SafeGetPropertyByName("config")?.Value as ObjectSyntax ?? SyntaxFactory.CreateObject(Array.Empty<ObjectPropertySyntax>());
                var connections = config.SafeGetPropertyByName("connections")?.Value as ObjectSyntax ?? SyntaxFactory.CreateObject(Array.Empty<ObjectPropertySyntax>());

                var kind = sourceType.TypeReference.Types[sourceType.TypeReference.Types.Length - 1];
                if (kind.EndsWith("Route"))
                {
                    kind = kind.Substring(0, kind.Length - "Route".Length);
                }

                var @namespace = (string?)null;
                var parts = kind.Split(".");
                if (parts.Length > 1)
                {
                    @namespace = string.Join(".", parts.Take(parts.Length - 1));
                    kind = parts[parts.Length - 1];
                }

                // TODO handle conflicts lololololol
                var name = sourceSymbol.Name;
                if (name.EndsWith(kind))
                {
                    name = name.Substring(0, name.Length - kind.Length);
                }

                kind = @namespace == null ? kind : $"{@namespace}/{kind}";

                connections = connections.MergeProperty(name, SyntaxFactory.CreateObject(new []
                {
                    SyntaxFactory.CreateObjectProperty("kind", SyntaxFactory.CreateStringLiteral(kind)),
                    SyntaxFactory.CreateObjectProperty("source", new PropertyAccessSyntax(functionCall.BaseExpression, SyntaxFactory.DotToken, SyntaxFactory.CreateIdentifier("id"))),
                }));
                config = config.MergeProperty("connections", connections);
                properties = properties.MergeProperty("config", config);

                var replacement = CodeReplacement.FromSyntax(body.SafeGetPropertyByName("properties")!.Value.Span, properties);
                return new CodeFix($"Add connection to {sourceSymbol.Name}", isPreferred: true, replacement);
            }

            private class Visitor : SyntaxVisitor
            {
                private readonly SemanticModel model;

                public Visitor(SemanticModel model)
                {
                    this.model = model;
                }

                public List<(ResourceType resourceType, ResourceSymbol resourceSymbol, InstanceFunctionCallSyntax syntax)> Results { get; } = new List<(ResourceType resourceType, ResourceSymbol resourceSymbol, InstanceFunctionCallSyntax syntax)>();

                public override void VisitInstanceFunctionCallSyntax(InstanceFunctionCallSyntax syntax)
                {
                    var type = this.model.GetTypeInfo(syntax.BaseExpression);
                    var symbol = this.model.GetSymbolInfo(syntax.BaseExpression);
                    if (type is ResourceType resourceType &&
                        symbol is ResourceSymbol resourceSymbol &&
                        resourceType.TypeReference.Namespace == "radius.dev" &&
                        resourceType.TypeReference.Types[0] == "Application" &&
                        resourceType.TypeReference.ApiVersion == "v1alpha3f")
                    {
                        Results.Add((resourceType, resourceSymbol, syntax));
                    }
                }
            }
        }
    }
}
