// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Emit;
using Bicep.Core.Resources;
using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics
{
    public class TransformedResource
    {
        public TransformedResource(
            DeclaredSymbol? declaration, 
            ResourceTypeReference resourceType, 
            CompoundName name,
            string? kind,
            ObjectSyntax body,
            IEnumerable<string> propertiesToOmit,
            IEnumerable<ResourceReference> implicitDependencies)
        {
            Declaration = declaration;
            ResourceType = resourceType;
            Name = name;
            Kind = kind;
            Body = body;
            PropertiesToOmit = propertiesToOmit.ToImmutableHashSet();
            ImplicitDependencies = implicitDependencies.ToImmutableArray();
        }

        public ObjectSyntax Body { get; }

        public DeclaredSymbol? Declaration { get; }

        public CompoundName Name { get; }

        public string? Kind { get; }

        public ImmutableArray<ResourceReference> ImplicitDependencies { get; }

        public ImmutableHashSet<string> PropertiesToOmit { get; }

        public ResourceTypeReference ResourceType { get; }
    }
}