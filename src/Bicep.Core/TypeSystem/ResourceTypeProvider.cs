// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using Bicep.Core.Resources;
using System.Collections.Immutable;
using Bicep.Core.Semantics.Metadata;

namespace Bicep.Core.TypeSystem
{

    public abstract class ResourceTypeProvider : IResourceTypeProvider
    {
        private class ResourceTypeCache
        {
            private class KeyComparer : IEqualityComparer<(ResourceTypeGenerationFlags flags, ResourceTypeReference type)>
            {
                public static IEqualityComparer<(ResourceTypeGenerationFlags flags, ResourceTypeReference type)> Instance { get; }
                    = new KeyComparer();

                public bool Equals((ResourceTypeGenerationFlags flags, ResourceTypeReference type) x, (ResourceTypeGenerationFlags flags, ResourceTypeReference type) y)
                    => x.flags == y.flags &&
                        ResourceTypeReferenceComparer.Instance.Equals(x.type, y.type);

                public int GetHashCode((ResourceTypeGenerationFlags flags, ResourceTypeReference type) x)
                    => x.flags.GetHashCode() ^
                        ResourceTypeReferenceComparer.Instance.GetHashCode(x.type);
            }

            private readonly IDictionary<(ResourceTypeGenerationFlags flags, ResourceTypeReference type), ResourceType> cache
                = new Dictionary<(ResourceTypeGenerationFlags flags, ResourceTypeReference type), ResourceType>(KeyComparer.Instance);

            public ResourceType GetOrAdd(ResourceTypeGenerationFlags flags, ResourceTypeReference typeReference, Func<ResourceType> buildFunc)
            {
                var cacheKey = (flags, typeReference);
                if (!cache.TryGetValue(cacheKey, out var value))
                {
                    value = buildFunc();
                    cache[cacheKey] = value;
                }

                return value;
            }
        }

        private IResourceTypeLoader? loader;
        private ImmutableHashSet<ResourceTypeReference>? types;
        private readonly ResourceTypeCache cache = new ResourceTypeCache();

        protected void Initialize(IResourceTypeLoader loader)
        {
            this.loader = loader;
            this.types = loader.GetAvailableTypes().ToImmutableHashSet(ResourceTypeReferenceComparer.Instance);
        }

        private ResourceType GenerateResourceType(ResourceTypeReference typeReference)
        {
            if (types!.Contains(typeReference))
            {
                return this.loader!.LoadType(typeReference);
            }

            throw new InvalidOperationException($"type {typeReference} is not supported");
        }

        public ResourceType GetType(ResourceTypeReference typeReference, ResourceTypeGenerationFlags flags)
        {
            if (!HasType(typeReference))
            {
                return null!; // YOLO!
            }

            // It's important to cache this result because GenerateResourceType is an expensive operation
            return cache.GetOrAdd(flags, typeReference, () =>
            {
                var resourceType = GenerateResourceType(typeReference);
                return Az.AzResourceTypeProvider.SetBicepResourceProperties(resourceType, flags);
            });
        }

        public bool HasType(ResourceTypeReference typeReference)
            => types!.Contains(typeReference);

        public IEnumerable<ResourceTypeReference> GetAvailableTypes()
            => types!;

        public virtual ResourceMetadata CreateMetadata(ResourceMetadata input)
        {
            return input;
        }
    }
}
