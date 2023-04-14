// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using Azure.Bicep.Types;
using Azure.Bicep.Types.Radius;
using Bicep.Core.Resources;

namespace Bicep.Core.TypeSystem.Radius
{
    public class RadiusResourceTypeLoader
    {
        private readonly ITypeLoader typeLoader;
        private readonly RadiusResourceTypeFactory resourceTypeFactory;
        private readonly ImmutableDictionary<ResourceTypeReference, TypeLocation> availableTypes;

        public RadiusResourceTypeLoader()
        {
            this.typeLoader = new RadiusTypeLoader();
            this.resourceTypeFactory = new RadiusResourceTypeFactory();
            var indexedTypes = typeLoader.LoadTypeIndex();
            this.availableTypes = indexedTypes.Resources.ToImmutableDictionary(
                kvp => ResourceTypeReference.Parse(kvp.Key),
                kvp => kvp.Value,
                ResourceTypeReferenceComparer.Instance);
        }

        public IEnumerable<ResourceTypeReference> GetAvailableTypes()
            => availableTypes.Keys;

        public ResourceTypeComponents LoadType(ResourceTypeReference reference)
        {
            var typeLocation = availableTypes[reference];

            var serializedResourceType = typeLoader.LoadResourceType(typeLocation);
            return resourceTypeFactory.GetResourceType(serializedResourceType);
        }
    }
}
