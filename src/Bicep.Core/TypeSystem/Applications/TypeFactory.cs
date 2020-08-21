// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using static Bicep.Core.TypeSystem.Applications.CommonProperties;

namespace Bicep.Core.TypeSystem.Applications
{
    internal static class TypeFactory
    {
        public static ComponentType CreateComponentType(ComponentTypeReference typeReference, NamedObjectType body)
        {
            return new ComponentType(typeReference, body);
        }

        public static InstanceType CreateInstanceType(ComponentTypeReference typeReference, NamedObjectType body)
        {
            return new InstanceType(typeReference, body);
        }
    }
}