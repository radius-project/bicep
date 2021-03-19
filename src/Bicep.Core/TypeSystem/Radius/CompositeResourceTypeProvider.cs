// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Resources;
using Bicep.Core.Semantics.Metadata;

namespace Bicep.Core.TypeSystem.Radius
{
    public class CompositeResourceTypeProvider : IResourceTypeProvider
    {
        private readonly IResourceTypeProvider[] providers;

        public CompositeResourceTypeProvider(IResourceTypeProvider[] providers)
        {
            this.providers = providers;
        }

        public ResourceMetadata CreateMetadata(ResourceMetadata input)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ResourceTypeReference> GetAvailableTypes()
        {
            return this.providers.SelectMany(p => p.GetAvailableTypes());
        }

        public ResourceType GetType(ResourceTypeReference reference, ResourceTypeGenerationFlags flags)
        {
            for (var i = 0; i < this.providers.Length; i++)
            {
                var provider = this.providers[i];
                var type = provider.GetType(reference, flags);
                if (type != null)
                {
                    return type;
                }
            }

            throw new InvalidOperationException($"no provider found the type {reference.FullyQualifiedType}");
        }

        public bool HasType(ResourceTypeReference typeReference)
        {
            for (var i = 0; i < this.providers.Length; i++)
            {
                var provider = this.providers[i];
                var hasType = provider.HasType(typeReference);
                if (hasType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
