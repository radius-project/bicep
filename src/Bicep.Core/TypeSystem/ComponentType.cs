// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Bicep.Core.Resources;
using Bicep.Core.TypeSystem.Applications;

namespace Bicep.Core.TypeSystem
{
    public class ComponentType : TypeSymbol
    {
        internal const string FullyQualifiedTypeName = "Microsoft.CustomProviders/resourceProviders/Applications/Components@2018-09-01-preview";
        internal static readonly ResourceTypeReference ResourceType = ResourceTypeReference.Parse(FullyQualifiedTypeName);

        public ComponentType(ComponentTypeReference typeReference, ITypeReference body)
            : base(FullyQualifiedTypeName)
        {
            TypeReference = typeReference;
            Body = body;
        }

        public ITypeReference Body { get; }

        public override TypeKind TypeKind => TypeKind.Component;

        public ComponentTypeReference TypeReference { get; }
    }
}