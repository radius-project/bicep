// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Resources;
using Bicep.Core.Semantics.Metadata;

using RadiusV3 = Bicep.Core.TypeSystem.Radius.V3;

namespace Bicep.Core.TypeSystem.Radius
{
    public class RadiusTypeProvider : ResourceTypeProvider
    {
        public RadiusTypeProvider()
        {
            Initialize(new Loader(this));
        }

        public ResourceTypeReference GetApplicationCRPType()
        {
            return ResourceTypeReference.Parse($"{RadiusV3.RadiusResources.ApplicationCRPType}@{RadiusV3.RadiusResources.CRPApiVersion}");
        }

        public ResourceTypeReference GetResourceCRPType(ResourceTypeReference input)
        {
            return ResourceTypeReference.Parse($"{string.Format(RadiusV3.RadiusResources.ApplicationChildCRPTypeFormat, input.TypeSegments[input.TypeSegments.Length - 1])}@{RadiusV3.RadiusResources.CRPApiVersion}");
        }

        private class Loader : Az.IAzResourceTypeLoader
        {
            private readonly ImmutableDictionary<ResourceTypeReference, ResourceTypeComponents> types;

            public Loader(IResourceTypeProvider provider)
            {
                var types = ImmutableDictionary.CreateBuilder<ResourceTypeReference, ResourceTypeComponents>(ResourceTypeReferenceComparer.Instance);

                // AppModel v3 types
                foreach (var type in RadiusV3.KnownTypes.MakeResourceTypes())
                {
                    types.Add(type.TypeReference, type);
                }

                this.types = types.ToImmutable();
            }

            public IEnumerable<ResourceTypeReference> GetAvailableTypes() => types.Keys;

            public ResourceTypeComponents LoadType(ResourceTypeReference reference) => types[reference];
        }
    }
}
