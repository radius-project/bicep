// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.TypeSystem.Applications;

namespace Bicep.Core.TypeSystem
{
    public class InstanceType : TypeSymbol
    {
        public InstanceType(ComponentTypeReference typeReference, ITypeReference body)
            : base(ComponentType.FullyQualifiedTypeName)
        {
            TypeReference = typeReference;
            Body = body;
        }

        public ITypeReference Body { get; }

        public override TypeKind TypeKind => TypeKind.Instance;

        public ComponentTypeReference TypeReference { get; }
    }
}