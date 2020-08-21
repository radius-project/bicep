// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.TypeSystem.Applications
{
    public interface IComponentTypeProvider
    {
        bool HasType(ComponentTypeReference reference);
        NamedObjectType GetBodyType(ComponentTypeReference reference);
    }
}