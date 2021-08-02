// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Resources;
using Bicep.Core.Semantics.Metadata;

using KnownTypesV1 = Bicep.Core.TypeSystem.Radius.KnownTypes;
using KnownTypesv1alpha3u = Bicep.Core.TypeSystem.Radiusv1alpha3u.KnownTypes;
using KnownTypesv1alpha3a = Bicep.Core.TypeSystem.Radiusv1alpha3a.KnownTypes;

namespace Bicep.Core.TypeSystem.Radius
{
    public class RadiusTypeProvider : ResourceTypeProvider
    {
        public static IResourceTypeProvider MakeComposite(IResourceTypeProvider primary)
        {
            return new CompositeResourceTypeProvider(new []{ new RadiusTypeProvider(), primary, });
        }

        public RadiusTypeProvider()
        {
            Initialize(new Loader(this));
        }

        public override ResourceMetadata CreateMetadata(ResourceMetadata input)
        {
            switch (input.TypeReference.FullyQualifiedType)
            {
                case RadiusResources.ApplicationResourceType:
                {
                    // We need to synthesize a 'parent' to represent the custom provider
                    var parent = new ResourceMetadataParent("radius");

                    return new ResourceMetadata(
                        input.Type,
                        ResourceTypeReference.Parse($"{RadiusResources.ApplicationCRPType}@{RadiusResources.CRPApiVersion}"),
                        input.DeclaringSyntax,
                        input.NameSyntax,
                        input.Symbol,
                        parent,
                        input.Dependencies,
                        input.ScopeSyntax,
                        input.IsExistingResource);
                }

                case RadiusResources.ComponentResourceType:
                {
                    return new ResourceMetadata(
                        input.Type,
                        ResourceTypeReference.Parse($"{RadiusResources.ComponentCRPType}@{RadiusResources.CRPApiVersion}"),
                        input.DeclaringSyntax,
                        input.NameSyntax,
                        input.Symbol,
                        input.Parent,
                        input.Dependencies,
                        input.ScopeSyntax,
                        input.IsExistingResource);
                }

                case RadiusResources.DeploymentResourceType:
                {
                    return new ResourceMetadata(
                        input.Type,
                        ResourceTypeReference.Parse($"{RadiusResources.DeploymentCRPType}@{RadiusResources.CRPApiVersion}"),
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
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.ApplicationResourceType}@{RadiusResources.ResourceApiVersion}"), KnownTypesV1.MakeApplication(provider));
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.ComponentResourceType}@{RadiusResources.ResourceApiVersion}"), KnownTypesV1.MakeComponent(provider));
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.DeploymentResourceType}@{RadiusResources.ResourceApiVersion}"), KnownTypesV1.MakeDeployment(provider));

                types.Add(ResourceTypeReference.Parse($"{RadiusResources.ApplicationResourceType}@v1alpha3u"), KnownTypesv1alpha3u.MakeApplication(provider));
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.BindingResourceType}@v1alpha3u"), KnownTypesv1alpha3u.MakeTopLevelBinding(provider));
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.ComponentResourceType}@v1alpha3u"), KnownTypesv1alpha3u.MakeComponent(provider));
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.ScopeResourceType}@v1alpha3u"), KnownTypesv1alpha3u.MakeScope(provider));
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.ScopeBindingResourceType}@v1alpha3u"), KnownTypesv1alpha3u.MakeScopeBinding(provider));

                foreach (var type in KnownTypesv1alpha3a.MakeResourceTypes(provider))
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
