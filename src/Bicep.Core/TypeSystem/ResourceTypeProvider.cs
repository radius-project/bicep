// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using Bicep.Core.Resources;
using System.Collections.Immutable;

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

        private Az.IAzResourceTypeLoader? loader;
        private ImmutableHashSet<ResourceTypeReference>? types;
        private readonly ResourceTypeCache cache = new ResourceTypeCache();

        protected void Initialize(Az.IAzResourceTypeLoader loader)
        {
            this.loader = loader;
            this.types = loader.GetAvailableTypes().ToImmutableHashSet(ResourceTypeReferenceComparer.Instance);
        }

        public IEnumerable<ResourceTypeReference> GetAvailableTypes()
        {
            return this.types!;
        }

        public ResourceType? TryGetDefinedType(NamespaceType declaringNamespace, ResourceTypeReference reference, ResourceTypeGenerationFlags flags)
        {
            if (!HasDefinedType(reference))
            {
                return null;
            }

            // It's important to cache this result because GenerateResourceType is an expensive operation
            return cache.GetOrAdd(flags, reference, () =>
            {
                var components = this.loader!.LoadType(reference);

                components = this.RewriteType(components, flags);
                return new ResourceType(declaringNamespace, components.TypeReference, components.ValidParentScopes, components.Body, ImmutableHashSet.Create<string>("name"));
            });
        }

        protected virtual ResourceTypeComponents RewriteType(ResourceTypeComponents resourceType, ResourceTypeGenerationFlags flags)
        {
            return Az.AzResourceTypeProvider.SetBicepResourceProperties(resourceType, flags, isExtensibility: true);
        }

        public bool HasDefinedType(ResourceTypeReference typeReference)
        {
            return types!.Contains(typeReference);
        }

        public ResourceType? TryGenerateFallbackType(NamespaceType declaringNamespace, ResourceTypeReference reference, ResourceTypeGenerationFlags flags)
        {
            return null;
        }
    }
}
