// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Diagnostics;
using Bicep.Core.Emit;
using Bicep.Core.Parsing;
using Bicep.Core.Syntax;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics
{
    public partial class SemanticModel
    {
        private static ImmutableHashSet<string> ApplicationPropertiesToOmit = new [] {
            "dependsOn",
            "name",
        }.ToImmutableHashSet();

        private static ImmutableHashSet<string> ComponentPropertiesToOmit = new [] {
            "dependsOn",
            "name",
            "application",
        }.ToImmutableHashSet();

        private static ImmutableHashSet<string> DeploymentPropertiesToOmit = new [] {
            "dependsOn",
            "name",
            "application",
        }.ToImmutableHashSet();

        private (ImmutableArray<ProjectedResource> resources, ImmutableArray<Diagnostic> diagnostics) ComputeProjectedResources()
        {
            var diagnotics = ImmutableArray.CreateBuilder<Diagnostic>();

            var visitor = new ApplicationResourceVisitor();
            visitor.Visit(this.Root);
            diagnotics.AddRange(visitor.Diagnostics);

            var resources = new List<ProjectedResource>();
            foreach (var kvp in visitor.Applications)
            {
                var name = new CompoundName(new[]
                {
                    new CompoundName.Segment("radius"),
                    new CompoundName.Segment(kvp.Key),
                });

                var application = new ProjectedResource(
                    kvp.Value, 
                    EmitHelpers.GetTypeReference(kvp.Value), 
                    name, 
                    null,
                    (ObjectSyntax)kvp.Value.DeclaringApplication.Body,
                    ApplicationPropertiesToOmit,
                    Array.Empty<ResourceReference>());
                resources.Add(application);
            }

            var components = new List<ComponentResource>();
            foreach (var kvp in visitor.Components)
            {
                var name = new CompoundName(new[]
                {
                    new CompoundName.Segment("radius"),
                    new CompoundName.Segment(kvp.Key.Item1),
                    new CompoundName.Segment(kvp.Key.Item2),
                });

                var component = new ProjectedResource(
                    kvp.Value,
                    EmitHelpers.GetTypeReference(kvp.Value),
                    name,
                    ((StringSyntax)kvp.Value.DeclaringComponent.Type).TryGetLiteralValue(),
                    (ObjectSyntax)kvp.Value.DeclaringComponent.Body,
                    ComponentPropertiesToOmit, 
                    new []
                    {
                        new ResourceReference(new CompoundName(new[]{new CompoundName.Segment("radius"), new CompoundName.Segment(kvp.Key.Item1),}), ApplicationType.ResourceType),
                    });
                resources.Add(component);
            }

            var deployments = new List<DeploymentResource>();
            foreach (var kvp in visitor.Deployments)
            {
                var name = new CompoundName(new[]
                {
                    new CompoundName.Segment("radius"),
                    new CompoundName.Segment(kvp.Key.Item1),
                    new CompoundName.Segment(kvp.Key.Item2),
                });

                var deployment = new ProjectedResource(
                    kvp.Value,
                    EmitHelpers.GetTypeReference(kvp.Value),
                    name,
                    null,
                    (ObjectSyntax)kvp.Value.DeclaringDeployment.Body,
                    DeploymentPropertiesToOmit, 
                    new []
                    {
                        new ResourceReference(new CompoundName(new[]{new CompoundName.Segment("radius"), new CompoundName.Segment(kvp.Key.Item1),}), ApplicationType.ResourceType),
                    });
                resources.Add(deployment);
            }

            if (visitor.Instances.Count > 0)
            {
                foreach (var group in visitor.Instances.GroupBy(kvp => kvp.Key.Item1))
                {
                    var references = new List<ResourceReference>();
                    foreach (var kvp in group)
                    {
                        var component = new ProjectedResource(
                            kvp.Value,
                            EmitHelpers.GetTypeReference(kvp.Value),
                            new CompoundName(new[]
                            {
                                new CompoundName.Segment("radius"),
                                new CompoundName.Segment(kvp.Key.Item1),
                                new CompoundName.Segment(kvp.Key.Item2),
                            }),
                            ((StringSyntax)kvp.Value.DeclaringInstance.Type).TryGetLiteralValue(),
                            (ObjectSyntax)kvp.Value.DeclaringInstance.Body,
                            ComponentPropertiesToOmit, 
                            new []
                            {
                                new ResourceReference(new CompoundName(new[]{new CompoundName.Segment("radius"), new CompoundName.Segment(kvp.Key.Item1),}), ApplicationType.ResourceType),
                            });
                        resources.Add(component);
                        references.Add(new ResourceReference(component.Name, component.ResourceType));
                    }

                    references.Add(new ResourceReference(new CompoundName(new[]{new CompoundName.Segment("radius"), new CompoundName.Segment(group.Key),}), ApplicationType.ResourceType));
                    var body = SynthesizeDeployment(group);
                    var deployment = new ProjectedResource(
                        null,
                        DeploymentType.ResourceType,
                        new CompoundName(new[]
                        {
                            new CompoundName.Segment("radius"),
                            new CompoundName.Segment(group.Key),
                            new CompoundName.Segment("implicit"),
                        }),
                        null,
                        body,
                        DeploymentPropertiesToOmit, 
                        references);
                    resources.Add(deployment);
                }
            }

            return (resources.ToImmutableArray(), diagnotics.ToImmutableArray());
        }

        public ImmutableArray<ProjectedResource> GetProjectedResources()
        {
            var (resources, _) = ComputeProjectedResources();
            return resources;
        }

        private ObjectSyntax SynthesizeDeployment(IGrouping<SyntaxBase, KeyValuePair<(SyntaxBase, SyntaxBase), InstanceSymbol>> group)
        {
            var componentsArrayItems = new List<SyntaxBase>();
            foreach (var kvp in group)
            {
                var componentNameProperty = new ObjectPropertySyntax(
                    key: new StringSyntax(
                        stringTokens: new[]
                        {
                            new Token(TokenType.StringComplete, new TextSpan(0, 0), "componentName", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>())
                        },
                        expressions: Array.Empty<SyntaxBase>(),
                        segmentValues: new[] { "componentName", }),
                    colon: new Token(TokenType.Colon, new TextSpan(0, 0), ":", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                    value: kvp.Key.Item2);

                var itemValue = new ObjectSyntax(
                    openBrace: new Token(TokenType.LeftBrace, new TextSpan(0, 0), "{", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                    children: new[] { componentNameProperty, },
                    closeBrace: new Token(TokenType.RightBrace, new TextSpan(0, 0), "}", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()));

                var item = new ArrayItemSyntax(itemValue);
                componentsArrayItems.Add(item);
            }

            var componentsValue = new ArraySyntax(
                openBracket: new Token(TokenType.LeftSquare, new TextSpan(0, 0), "[", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                children: componentsArrayItems,
                closeBracket: new Token(TokenType.RightSquare, new TextSpan(0, 0), "]", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()));

            var componentsProperty = new ObjectPropertySyntax(
                key: new StringSyntax(
                    stringTokens: new[]
                    {
                        new Token(TokenType.StringComplete, new TextSpan(0, 0), "components", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>())
                    },
                    expressions: Array.Empty<SyntaxBase>(),
                    segmentValues: new[] { "components", }),
                colon: new Token(TokenType.Colon, new TextSpan(0, 0), ":", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                value: componentsValue);

            var propertiesValue = new ObjectSyntax(
                openBrace: new Token(TokenType.LeftBrace, new TextSpan(0, 0), "{", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                children: new[] { componentsProperty, },
                closeBrace: new Token(TokenType.RightBrace, new TextSpan(0, 0), "}", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()));

            var propertiesProperty = new ObjectPropertySyntax(
                key: new StringSyntax(
                    stringTokens: new[]
                    {
                        new Token(TokenType.StringComplete, new TextSpan(0, 0), "properties", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>())
                    },
                    expressions: Array.Empty<SyntaxBase>(),
                    segmentValues: new[] { "properties", }),
                colon: new Token(TokenType.Colon, new TextSpan(0, 0), ":", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                value: propertiesValue);

            var body = new ObjectSyntax(
                openBrace: new Token(TokenType.LeftBrace, new TextSpan(0, 0), "{", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()),
                children: new[] { propertiesProperty, },
                closeBrace: new Token(TokenType.RightBrace, new TextSpan(0, 0), "}", Array.Empty<SyntaxTrivia>(), Array.Empty<SyntaxTrivia>()));

            return body;
        }
    }
}