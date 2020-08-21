// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Emit
{
    public class ComponentResource
    {
        public ComponentResource(DeclaredSymbol declaration, CompoundName name)
        {
            Declaration = declaration;
            Name = name;

            ResourceType = EmitHelpers.GetTypeReference(declaration);
        }

        public DeclaredSymbol Declaration { get; }

        public CompoundName Name { get;  }

        public ResourceTypeReference ResourceType { get; }
    }
}