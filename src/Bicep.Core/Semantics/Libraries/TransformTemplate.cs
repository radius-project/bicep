// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Emit;
using Bicep.Core.Resources;
using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics.Libraries
{
    public class TransformTemplate
    {
        public TransformTemplate(
            DeclaredSymbol? declaration, 
            ResourceTypeReference resourceType, 
            CompoundName name,
            string? kind,
            ObjectSyntax input,
            Func<ObjectSyntax, ObjectSyntax> body,
            IEnumerable<string> propertiesToOmit,
            IEnumerable<ResourceReference> implicitDependencies)
        {
            Declaration = declaration;
            ResourceType = resourceType;
            Name = name;
            Kind = kind;
            Input = input;
            Body = body;
            PropertiesToOmit = propertiesToOmit.ToImmutableHashSet();
            ImplicitDependencies = implicitDependencies.ToImmutableArray();
        }

        public Func<ObjectSyntax, ObjectSyntax> Body { get; }

        public DeclaredSymbol? Declaration { get; }
        
        
        public ObjectSyntax Input { get; }

        public CompoundName Name { get; }

        public string? Kind { get; }

        public ImmutableArray<ResourceReference> ImplicitDependencies { get; }

        public ImmutableHashSet<string> PropertiesToOmit { get; }

        public ResourceTypeReference ResourceType { get; }
    }
}