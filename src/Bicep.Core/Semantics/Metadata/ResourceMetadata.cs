// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Emit;
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
            IEnumerable<ResourceDependency> dependencies,
            SyntaxBase? scopeSyntax,
            bool isExistingResource)
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
        }

        public ResourceTypeReference TypeReference { get; }

        public ResourceSymbol Symbol { get; }

        public ResourceType Type { get; }

        public ResourceMetadataParent? Parent { get; }

        public ImmutableArray<ResourceDependency> Dependencies { get; }

        public ResourceDeclarationSyntax DeclaringSyntax { get; }

        public SyntaxBase NameSyntax { get; }

        public SyntaxBase? ScopeSyntax { get; }

        public bool IsExistingResource { get; }
    }
}
