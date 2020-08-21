// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Bicep.Core.Resources;

namespace Bicep.Core.Emit
{
    public readonly struct ResourceReference
    {
        public readonly CompoundName Name { get; }

        public readonly ResourceTypeReference ResourceType { get; }

        public ResourceReference(CompoundName name, ResourceTypeReference resourceType)
        {
            Name = name;
            ResourceType = resourceType;
        }
    }
}