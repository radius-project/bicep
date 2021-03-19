// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Resources;
using Bicep.Core.Semantics.Metadata;

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
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.ApplicationResourceType}@{RadiusResources.ResourceApiVersion}"), KnownTypes.MakeApplication(provider));
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.ComponentResourceType}@{RadiusResources.ResourceApiVersion}"), KnownTypes.MakeComponent(provider));
                types.Add(ResourceTypeReference.Parse($"{RadiusResources.DeploymentResourceType}@{RadiusResources.ResourceApiVersion}"), KnownTypes.MakeDeployment(provider));
                this.types = types.ToImmutable();
            }

            public IEnumerable<ResourceTypeReference> GetAvailableTypes() => types.Keys;

            public ResourceType LoadType(ResourceTypeReference reference) => types[reference];
        }
    }
}
