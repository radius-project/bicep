// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using Azure.Bicep.Types;
using Azure.Bicep.Types.Aws;
using Bicep.Core.Resources;

namespace Bicep.Core.TypeSystem.Aws
{
    public class AwsResourceTypeLoader
    {
        private readonly ITypeLoader typeLoader;
        private readonly AwsResourceTypeFactory resourceTypeFactory;
        private readonly ImmutableDictionary<ResourceTypeReference, TypeLocation> availableTypes;

        public AwsResourceTypeLoader()
        {
            this.typeLoader = new AwsTypeLoader();
            this.resourceTypeFactory = new AwsResourceTypeFactory();
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
