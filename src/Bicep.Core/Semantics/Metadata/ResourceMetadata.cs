// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Resources;
using Bicep.Core.Syntax;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics.Metadata
{
    public class ResourceMetadata
    {
        public ResourceMetadata(
            ResourceType type,
            ResourceTypeReference typeReference,
            ResourceDeclarationSyntax declaringSyntax,
            SyntaxBase nameSyntax,
            ResourceSymbol symbol,
            ResourceMetadataParent? parent,
            IEnumerable<ResourceDependencyMetadata> dependencies,
            SyntaxBase? scopeSyntax,
            bool isExistingResource,
            string? provider)
        {
            // TODO: turn this into a record when the target framework supports it
            Type = type;
            TypeReference = typeReference;
            DeclaringSyntax = declaringSyntax;
            NameSyntax = nameSyntax;
            Symbol = symbol;
            Parent = parent;
            Dependencies = dependencies.ToImmutableArray();
            ScopeSyntax = scopeSyntax;
            IsExistingResource = isExistingResource;
            Provider = provider;
        }

        public ResourceSymbol Symbol { get; }

        public ResourceTypeReference TypeReference { get; }

        public ResourceType Type { get; }

        public ResourceMetadataParent? Parent { get; }

        public ImmutableArray<ResourceDependencyMetadata> Dependencies { get; }

        public ResourceDeclarationSyntax DeclaringSyntax { get; }

        public SyntaxBase NameSyntax { get; }

        public SyntaxBase? ScopeSyntax { get; }

        public bool IsExistingResource { get; }

        public string? Provider { get; }

        public bool IsExtensionResource => Provider is not null;
    }

    public class ResourceDependencyMetadata
    {
        public ResourceDependencyMetadata(DeclaredSymbol symbol, SyntaxBase? indexExpression)
        {
            Symbol = symbol;
            IndexExpression = indexExpression;
        }

        public DeclaredSymbol Symbol { get; }

        public SyntaxBase? IndexExpression { get; }
    }
}
