// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Emit;
using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics.Metadata
{
    public class ResourceMetadataCache : SyntaxMetadataCacheBase<ResourceMetadata?>
    {
        private readonly SemanticModel semanticModel;
        private readonly ConcurrentDictionary<ResourceSymbol, ResourceMetadata> symbolLookup;
        private readonly Lazy<ImmutableDictionary<ResourceDeclarationSyntax, ResourceSymbol>> resourceSymbols;
        private readonly Lazy<ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<ResourceDependency>>> resourceDependencies;

        public ResourceMetadataCache(SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
            this.symbolLookup = new();
            this.resourceSymbols = new(() => ResourceSymbolVisitor.GetAllResources(semanticModel.Root)
                .ToImmutableDictionary(x => x.DeclaringResource));

            this.resourceDependencies = new(() => ResourceDependencyVisitor.GetResourceDependencies(semanticModel));
        }

        protected override ResourceMetadata? Calculate(SyntaxBase syntax)
        {
            switch (syntax)
            {
                case ResourceAccessSyntax _:
                case VariableAccessSyntax _:
                    {
                        var symbol = semanticModel.GetSymbolInfo(syntax);
                        if (symbol is DeclaredSymbol declaredSymbol)
                        {
                            return this.TryLookup(declaredSymbol.DeclaringSyntax);
                        }

                        break;
                    }
                case ResourceDeclarationSyntax resourceDeclarationSyntax:
                    {
                        var metadata = CalculateResourceMetadata(resourceDeclarationSyntax);
                        if (metadata?.Type.Provider is {} provider)
                        {
                            return provider.CreateMetadata(metadata);
                        }

                        return metadata;
                    }
            }

            return null;
        }

        private ResourceMetadata? CalculateResourceMetadata(ResourceDeclarationSyntax resourceDeclarationSyntax)
        {
            // Skip analysis for ErrorSymbol and similar cases, these are invalid cases, and won't be emitted.
            if (!resourceSymbols.Value.TryGetValue(resourceDeclarationSyntax, out var symbol) ||
                symbol.TryGetResourceType() is not { } resourceType ||
                symbol.SafeGetBodyPropertyValue(TypeSystem.TypePropertyFlags.Identifier) is not { } nameSyntax)
            {
                return null;
            }

            this.resourceDependencies.Value.TryGetValue(symbol, out var dependencies);
            dependencies ??= ImmutableHashSet<ResourceDependency>.Empty;

            if (semanticModel.Binder.GetNearestAncestor<ResourceDeclarationSyntax>(resourceDeclarationSyntax) is { } nestedParentSyntax)
            {
                // nested resource parent syntax
                if (TryLookup(nestedParentSyntax) is { } parentMetadata)
                {
                    return new(
                        resourceType,
                        resourceType.TypeReference,
                        resourceDeclarationSyntax,
                        nameSyntax,
                        symbol,
                        new(parentMetadata, null, true),
                        dependencies.Select(d => new ResourceDependencyMetadata(d.Resource, d.IndexExpression)),
                        symbol.SafeGetBodyPropertyValue(LanguageConstants.ResourceScopePropertyName),
                        symbol.DeclaringResource.IsExistingResource(),
                        provider: null);
                }
            }
            else if (symbol.SafeGetBodyPropertyValue(LanguageConstants.ResourceParentPropertyName) is { } referenceParentSyntax)
            {
                SyntaxBase? indexExpression = null;
                if (referenceParentSyntax is ArrayAccessSyntax arrayAccess)
                {
                    referenceParentSyntax = arrayAccess.BaseExpression;
                    indexExpression = arrayAccess.IndexExpression;
                }

                // parent property reference syntax
                if (TryLookup(referenceParentSyntax) is { } parentMetadata)
                {
                    return new(
                        resourceType,
                        resourceType.TypeReference,
                        resourceDeclarationSyntax,
                        nameSyntax,
                        symbol,
                        new(parentMetadata, indexExpression, false),
                        dependencies.Select(d => new ResourceDependencyMetadata(d.Resource, d.IndexExpression)),
                        symbol.SafeGetBodyPropertyValue(LanguageConstants.ResourceScopePropertyName),
                        symbol.DeclaringResource.IsExistingResource(),
                        provider: null);
                }
            }
            else
            {
                return new(
                    resourceType,
                    resourceType.TypeReference,
                    resourceDeclarationSyntax,
                    nameSyntax,
                    symbol,
                    null,
                    dependencies.Select(d => new ResourceDependencyMetadata(d.Resource, d.IndexExpression)),
                    symbol.SafeGetBodyPropertyValue(LanguageConstants.ResourceScopePropertyName),
                    symbol.DeclaringResource.IsExistingResource(),
                    provider: null);
            }

            return null;
        }
    }
}
