// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Resources;

namespace Bicep.Core.TypeSystem.Radius
{
    public class RadiusTypeProvider : IResourceTypeProvider
    {
        private static readonly ImmutableHashSet<string> WritableExistingResourceProperties = new []
        {
            LanguageConstants.ResourceNamePropertyName,
            LanguageConstants.ResourceScopePropertyName,
            LanguageConstants.ResourceParentPropertyName,
        }.ToImmutableHashSet();

        private readonly ImmutableDictionary<ResourceTypeReference, (ResourceType normal, ResourceType existing)> types;

        public static IResourceTypeProvider MakeComposite(IResourceTypeProvider primary)
        {
            return new CompositeResourceTypeProvider(new []{ new RadiusTypeProvider(), primary, });
        }

        public RadiusTypeProvider()
        {
            var types = ImmutableDictionary.CreateBuilder<ResourceTypeReference, (ResourceType normal, ResourceType existing)>(ResourceTypeReferenceComparer.Instance);
            types.Add(Make(ResourceTypeReference.Parse("radius.dev/Applications@v1alpha1"), KnownTypes.MakeApplication()));
            types.Add(Make(ResourceTypeReference.Parse("radius.dev/Applications/Components@v1alpha1"), KnownTypes.MakeComponent()));
            types.Add(Make(ResourceTypeReference.Parse("radius.dev/Applications/Deployments@v1alpha1"), KnownTypes.MakeDeployment()));
            this.types = types.ToImmutable();
        }

        public IEnumerable<ResourceTypeReference> GetAvailableTypes()
        {
            return this.types.Keys;
        }

        public ResourceType GetType(ResourceTypeReference reference, bool isExistingResource)
        {
            if (this.types.TryGetValue(reference, out var tuple))
            {
                return isExistingResource ? tuple.existing : tuple.normal;
            }

            return null!;
        }

        public bool HasType(ResourceTypeReference typeReference)
        {
            return this.types.ContainsKey(typeReference);
        }

        private static KeyValuePair<ResourceTypeReference, (ResourceType normal, ResourceType existing)> Make(ResourceTypeReference reference, ITypeReference type)
        {
            var normal = new ResourceType(reference, ResourceScope.ResourceGroup, type);
            var existing = new ResourceType(reference, ResourceScope.ResourceGroup, type);
            return new KeyValuePair<ResourceTypeReference, (ResourceType normal, ResourceType existing)>(reference, (normal, existing));
        }

        public static ResourceType SetBicepResourceProperties(ResourceType resourceType, bool isExistingResource)
        {
            var bodyType = resourceType.Body.Type;

            switch (bodyType)
            {
                case ObjectType bodyObjectType:
                    bodyType = SetBicepResourceProperties(bodyObjectType, resourceType.ValidParentScopes, resourceType.TypeReference, isExistingResource);
                    break;
                case DiscriminatedObjectType bodyDiscriminatedType:
                    var bodyTypes = bodyDiscriminatedType.UnionMembersByKey.Values.ToList()
                        .Select(x => x.Type as ObjectType ?? throw new ArgumentException($"Resource {resourceType.Name} has unexpected body type {bodyType.GetType()}"));
                    bodyTypes = bodyTypes.Select(x => SetBicepResourceProperties(x, resourceType.ValidParentScopes, resourceType.TypeReference, isExistingResource));
                    bodyType = new DiscriminatedObjectType(
                        bodyDiscriminatedType.Name,
                        bodyDiscriminatedType.ValidationFlags,
                        bodyDiscriminatedType.DiscriminatorKey,
                        bodyTypes);
                    break;
                default:
                    // we exhaustively test deserialization of every resource type during CI, and this happens in a deterministic fashion,
                    // so this exception should never occur in the released product
                    throw new ArgumentException($"Resource {resourceType.Name} has unexpected body type {bodyType.GetType()}");
            }

            return new ResourceType(resourceType.TypeReference, resourceType.ValidParentScopes, bodyType);
        }

        private static ObjectType SetBicepResourceProperties(ObjectType objectType, ResourceScope validParentScopes, ResourceTypeReference typeReference, bool isExistingResource)
        {
            var properties = objectType.Properties;

            var scopePropertyFlags = TypePropertyFlags.WriteOnly | TypePropertyFlags.DeployTimeConstant;
            if (validParentScopes == ResourceScope.Resource)
            {
                // resource can only be deployed as an extension resource - scope should be required
                scopePropertyFlags |= TypePropertyFlags.Required;
            }

            if (isExistingResource)
            {
                // we can refer to a resource at any scope if it is an existing resource not being deployed by this file
                var scopeReference = LanguageConstants.CreateResourceScopeReference(validParentScopes);
                properties = properties.SetItem(LanguageConstants.ResourceScopePropertyName, new TypeProperty(LanguageConstants.ResourceScopePropertyName, scopeReference, scopePropertyFlags));
            }
            else
            {
                // TODO: remove 'dependsOn' from the type library
                properties = properties.SetItem(LanguageConstants.ResourceDependsOnPropertyName, new TypeProperty(LanguageConstants.ResourceDependsOnPropertyName, LanguageConstants.ResourceOrResourceCollectionRefArray, TypePropertyFlags.WriteOnly));

                // we only support scope for extension resources (or resources where the scope is unknown and thus may be an extension resource)
                if (validParentScopes.HasFlag(ResourceScope.Resource))
                {
                    var scopeReference = LanguageConstants.CreateResourceScopeReference(ResourceScope.Resource);
                    properties = properties.SetItem(LanguageConstants.ResourceScopePropertyName, new TypeProperty(LanguageConstants.ResourceScopePropertyName, scopeReference, scopePropertyFlags));
                }
            }

            // add the 'parent' property for child resource types
            if (!typeReference.IsRootType)
            {
                var parentType = LanguageConstants.CreateResourceScopeReference(ResourceScope.Resource);
                var parentFlags = TypePropertyFlags.WriteOnly | TypePropertyFlags.DeployTimeConstant;

                properties = properties.SetItem(LanguageConstants.ResourceParentPropertyName, new TypeProperty(LanguageConstants.ResourceParentPropertyName, parentType, parentFlags));
            }

            return new NamedObjectType(
                objectType.Name,
                objectType.ValidationFlags,
                isExistingResource ? ConvertToReadOnly(properties.Values) : properties.Values,
                objectType.AdditionalPropertiesType,
                isExistingResource ? ConvertToReadOnly(objectType.AdditionalPropertiesFlags) : objectType.AdditionalPropertiesFlags);
        }

        private static TypePropertyFlags ConvertToReadOnly(TypePropertyFlags typePropertyFlags)
            => (typePropertyFlags | TypePropertyFlags.ReadOnly) & ~TypePropertyFlags.Required;

        private static IEnumerable<TypeProperty> ConvertToReadOnly(IEnumerable<TypeProperty> properties)
        {
            foreach (var property in properties)
            {
                // "name", "scope" & "parent" can be set for existing resources - everything else should be read-only
                if (WritableExistingResourceProperties.Contains(property.Name))
                {
                    yield return property;
                }
                else
                {
                    yield return new TypeProperty(property.Name, property.TypeReference, ConvertToReadOnly(property.Flags));
                }
            }
        }
    }
}
