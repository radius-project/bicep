// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
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
            Initialize(new Loader());
        }

        protected override ResourceTypeComponents RewriteType(ResourceTypeComponents resourceType, ResourceTypeGenerationFlags flags)
        {
            var body = (TypeSymbol)resourceType.Body;

            // Mark name as an identifier
            body = RewriteObjectType(body, "metadata", input =>
            {
                return RewriteObjectTypeProperty(input, "name", input =>
                {
                    return UpdateFlags(input, input.Flags | TypePropertyFlags.LoopVariant);
                });
            });

            if (flags.HasFlag(ResourceTypeGenerationFlags.ExistingResource))
            {
                body = RewriteObjectType(body, input =>
                {
                    if (input.Name == "metadata")
                    {
                        // For an existing resource allow metadata.name & metadata.namespace to be set
                        return RewriteObjectTypeProperty(input, input =>
                        {
                            if (input.Name == "name" || input.Name == "namespace")
                            {
                                return input;
                            }

                            return UpdateFlags(input, input.Flags | TypePropertyFlags.ReadOnly);
                        });
                    }
                    else
                    {
                        // For an existing resource set everything except metadata to be readonly.
                        return UpdateFlags(input, input.Flags | TypePropertyFlags.ReadOnly);
                    }
                });
            }

            return new ResourceTypeComponents(resourceType.TypeReference, resourceType.ValidParentScopes, body);
        }

        private static TypeSymbol RewriteObjectType(TypeSymbol type, Func<TypeProperty, TypeProperty> rewriter)
            => RewriteObjectType(type, null, rewriter);

        private static TypeSymbol RewriteObjectType(TypeSymbol type, string? name, Func<TypeProperty, TypeProperty> rewriter)
        {
            if (type is ObjectType objectType)
            {
                var properties = objectType.Properties;
                foreach (var key in properties.Keys)
                {
                    if (name == null || key == name)
                    {
                        properties = properties.SetItem(key, rewriter(properties[key]));
                    }
                }

                return new ObjectType(objectType.Name, objectType.ValidationFlags, properties.Values, objectType.AdditionalPropertiesType, objectType.AdditionalPropertiesFlags, objectType.MethodResolver.CopyToObject);
            }

            return type;
        }

        private static TypeProperty RewriteObjectTypeProperty(TypeProperty property, Func<TypeProperty, TypeProperty> rewriter)
        {
            var propertyType = RewriteObjectType(property.TypeReference.Type, rewriter);
            return new TypeProperty(property.Name, propertyType, property.Flags, property.Description);
        }

        private static TypeProperty RewriteObjectTypeProperty(TypeProperty property, string name, Func<TypeProperty, TypeProperty> rewriter)
        {
            var propertyType = RewriteObjectType(property.TypeReference.Type, name, rewriter);
            return new TypeProperty(property.Name, propertyType, property.Flags, property.Description);
        }

        private static TypeProperty UpdateFlags(TypeProperty typeProperty, TypePropertyFlags flags) =>
                new(typeProperty.Name, typeProperty.TypeReference, flags, typeProperty.Description);

        private class Loader : Az.IAzResourceTypeLoader
        {
            private readonly ITypeLoader typeLoader;
            private readonly KubernetesResourceTypeFactory resourceTypeFactory;
            private readonly ImmutableDictionary<ResourceTypeReference, TypeLocation> availableTypes;

            public Loader()
            {
                this.typeLoader = new TypeLoader();
                this.resourceTypeFactory = new KubernetesResourceTypeFactory();
                this.availableTypes = typeLoader.GetIndexedTypes().Types.ToImmutableDictionary(
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
}
