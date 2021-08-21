// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Resources;
using Bicep.Core.Semantics.Metadata;
using Bicep.Types.Kubernetes;

namespace Bicep.Core.TypeSystem.Kubernetes
{
    public class KubernetesTypeProvider : ResourceTypeProvider
    {
        public KubernetesTypeProvider()
        {
            Initialize(new Loader(this));
        }

        public override ResourceMetadata CreateMetadata(ResourceMetadata input)
        {
            return new ResourceMetadata(
                input.Type,
                input.TypeReference,
                input.DeclaringSyntax,
                input.NameSyntax,
                input.Symbol,
                input.Parent,
                input.Dependencies,
                input.ScopeSyntax,
                input.IsExistingResource,
                provider: "kubernetes");
        }

        private class Loader : IResourceTypeLoader
        {
            private readonly ITypeLoader typeLoader;
            private readonly KubernetesResourceTypeFactory resourceTypeFactory;
            private readonly ImmutableDictionary<ResourceTypeReference, TypeLocation> availableTypes;

            public Loader(IResourceTypeProvider provider)
            {
                this.typeLoader = new TypeLoader();
                this.resourceTypeFactory = new KubernetesResourceTypeFactory(provider);
                this.availableTypes = typeLoader.GetIndexedTypes().Types.ToImmutableDictionary(
                    kvp => ResourceTypeReference.Parse(kvp.Key),
                    kvp => kvp.Value,
                    ResourceTypeReferenceComparer.Instance);
            }

            public IEnumerable<ResourceTypeReference> GetAvailableTypes()
                => availableTypes.Keys;

            public ResourceType LoadType(ResourceTypeReference reference)
            {
                var typeLocation = availableTypes[reference];

                var serializedResourceType = typeLoader.LoadResourceType(typeLocation);
                return resourceTypeFactory.GetResourceType(serializedResourceType);
            }
        }
    }
}
