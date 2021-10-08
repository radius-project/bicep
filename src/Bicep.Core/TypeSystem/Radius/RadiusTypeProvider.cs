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
        public static IResourceTypeProvider MakeComposite(IResourceTypeProvider primary)
        {
            return new CompositeResourceTypeProvider(new[] { new RadiusTypeProvider(), primary, });
        }

        public RadiusTypeProvider()
        {
            Initialize(new Loader(this));
        }

        public override ResourceMetadata CreateMetadata(ResourceMetadata input)
        {
            if (input.TypeReference.ApiVersion == "v1alpha3")
            {
                if (input.TypeReference.FullyQualifiedType == RadiusV3.RadiusResources.ApplicationResourceType)
                {
                    // We need to synthesize a 'parent' to represent the custom provider
                    var parent = new ResourceMetadataParent(RadiusV3.RadiusResources.ProviderCRPName);

                    return new ResourceMetadata(
                        input.Type,
                        ResourceTypeReference.Parse($"{RadiusV3.RadiusResources.ApplicationCRPType}@{RadiusV3.RadiusResources.CRPApiVersion}"),
                        input.DeclaringSyntax,
                        input.NameSyntax,
                        input.Symbol,
                        parent,
                        input.Dependencies,
                        input.ScopeSyntax,
                        input.IsExistingResource);
                }
                else
                {
                    return new ResourceMetadata(
                        input.Type,
                        ResourceTypeReference.Parse($"{string.Format(RadiusV3.RadiusResources.ApplicationChildCRPTypeFormat, input.Type.TypeReference.Types[input.Type.TypeReference.Types.Length - 1])}@{RadiusV3.RadiusResources.CRPApiVersion}"),
                        input.DeclaringSyntax,
                        input.NameSyntax,
                        input.Symbol,
                        input.Parent,
                        input.Dependencies,
                        input.ScopeSyntax,
                        input.IsExistingResource);
                }
            }

            return base.CreateMetadata(input);
        }

        private class Loader : IResourceTypeLoader
        {
            private readonly ImmutableDictionary<ResourceTypeReference, ResourceType> types;

            public Loader(IResourceTypeProvider provider)
            {
                var types = ImmutableDictionary.CreateBuilder<ResourceTypeReference, ResourceType>(ResourceTypeReferenceComparer.Instance);

                // AppModel v3 types
                foreach (var type in RadiusV3.KnownTypes.MakeResourceTypes(provider))
                {
                    types.Add(type.TypeReference, type);
                }

                this.types = types.ToImmutable();
            }

            public IEnumerable<ResourceTypeReference> GetAvailableTypes() => types.Keys;

            public ResourceType LoadType(ResourceTypeReference reference) => types[reference];
        }
    }
}
