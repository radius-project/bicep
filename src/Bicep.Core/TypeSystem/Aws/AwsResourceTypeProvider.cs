// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Resources;
using System.Collections.Immutable;
using System.Collections.Concurrent;

namespace Bicep.Core.TypeSystem.Aws
{
    public class AwsResourceTypeProvider : IResourceTypeProvider
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

            private readonly ConcurrentDictionary<(ResourceTypeGenerationFlags flags, ResourceTypeReference type), ResourceTypeComponents> cache
                = new ConcurrentDictionary<(ResourceTypeGenerationFlags flags, ResourceTypeReference type), ResourceTypeComponents>(KeyComparer.Instance);

            public ResourceTypeComponents GetOrAdd(ResourceTypeGenerationFlags flags, ResourceTypeReference typeReference, Func<ResourceTypeComponents> buildFunc)
            {
                var cacheKey = (flags, typeReference);

                return cache.GetOrAdd(cacheKey, cacheKey => buildFunc());
            }
        }

        private const string ResourceNamePropertyName = "name";
        private const string ResourceAliasPropertyName = "alias";

        private static readonly TypeSymbol Tags = new ObjectType(nameof(Tags), TypeSymbolValidationFlags.Default, Enumerable.Empty<TypeProperty>(), LanguageConstants.String, TypePropertyFlags.None);

        private readonly AwsResourceTypeLoader resourceTypeLoader;
        private readonly ImmutableHashSet<ResourceTypeReference> availableResourceTypes;
        private readonly ResourceTypeCache definedTypeCache;
        private readonly ResourceTypeCache generatedTypeCache;

        public static readonly ImmutableHashSet<string> UniqueIdentifierProperties = new[]
        {
            ResourceNamePropertyName,
        }.ToImmutableHashSet();

        public AwsResourceTypeProvider(AwsResourceTypeLoader resourceTypeLoader)
        {
            this.resourceTypeLoader = resourceTypeLoader;
            this.availableResourceTypes = resourceTypeLoader.GetAvailableTypes().ToImmutableHashSet(ResourceTypeReferenceComparer.Instance);
            this.definedTypeCache = new ResourceTypeCache();
            this.generatedTypeCache = new ResourceTypeCache();
        }

        private static ResourceTypeComponents SetBicepResourceProperties(ResourceTypeComponents resourceType, ResourceTypeGenerationFlags flags)
        {
            var bodyType = resourceType.Body.Type;

            switch (bodyType)
            {
                case ObjectType bodyObjectType:
                    if (bodyObjectType.Properties.TryGetValue(ResourceNamePropertyName, out var nameProperty) &&
                        nameProperty.TypeReference.Type is not PrimitiveType { Name: LanguageConstants.TypeNameString })
                    {
                        // The 'name' property doesn't support fixed value names (e.g. we're in a top-level child resource declaration).
                        // Best we can do is return a regular 'string' field for it as we have no good way to reliably evaluate complex expressions (e.g. to check whether it terminates with '/<constantType>').
                        // Keep it simple for now - we eventually plan to phase out the 'top-level child' syntax.
                        bodyObjectType = new ObjectType(
                            bodyObjectType.Name,
                            bodyObjectType.ValidationFlags,
                            bodyObjectType.Properties.SetItem(ResourceNamePropertyName, new TypeProperty(nameProperty.Name, LanguageConstants.String, nameProperty.Flags)).Values,
                            bodyObjectType.AdditionalPropertiesType,
                            bodyObjectType.AdditionalPropertiesFlags,
                            bodyObjectType.MethodResolver.CopyToObject);

                        bodyType = SetBicepResourceProperties(bodyObjectType, resourceType.ValidParentScopes, resourceType.TypeReference, flags);
                        break;
                    }

                    bodyType = SetBicepResourceProperties(bodyObjectType, resourceType.ValidParentScopes, resourceType.TypeReference, flags);
                    break;
                default:
                    // we exhaustively test deserialization of every resource type during CI, and this happens in a deterministic fashion,
                    // so this exception should never occur in the released product
                    throw new ArgumentException($"Resource {resourceType.TypeReference.FormatName()} has unexpected body type {bodyType.GetType()}");
            }
            return resourceType with { Body = bodyType };
        }

        private static ObjectType SetBicepResourceProperties(ObjectType objectType, ResourceScope validParentScopes, ResourceTypeReference typeReference, ResourceTypeGenerationFlags flags)
        {
            // Local function.
            static TypeProperty UpdateFlags(TypeProperty typeProperty, TypePropertyFlags flags) =>
                new(typeProperty.Name, typeProperty.TypeReference, flags, typeProperty.Description);

            var properties = objectType.Properties;
            var isExistingResource = flags.HasFlag(ResourceTypeGenerationFlags.ExistingResource);


            properties = properties.SetItem("id", new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.SystemProperty, "The resource id"));
            properties = properties.SetItem("type", new TypeProperty("type", new StringLiteralType(typeReference.FormatType()), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.SystemProperty, "The resource type"));
            properties = properties.SetItem("apiVersion", new TypeProperty("apiVersion", new StringLiteralType(typeReference.ApiVersion!), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.SystemProperty, "The resource api version"));

            if (isExistingResource)
            {
            }
            else
            {
                // TODO: remove 'dependsOn' from the type library
                properties = properties.SetItem(LanguageConstants.ResourceDependsOnPropertyName, new TypeProperty(LanguageConstants.ResourceDependsOnPropertyName, LanguageConstants.ResourceOrResourceCollectionRefArray, TypePropertyFlags.WriteOnly | TypePropertyFlags.DisallowAny));
            }

            // add the loop variant flag to the name property (if it exists)
            if (properties.TryGetValue(ResourceNamePropertyName, out var nameProperty) && properties.TryGetValue(ResourceAliasPropertyName, out var aliasProperty))
            {
                // TODO apply this to all unique properties
                properties = properties.SetItem(ResourceNamePropertyName, UpdateFlags(nameProperty, nameProperty.Flags | TypePropertyFlags.LoopVariant));
                properties = properties.SetItem(ResourceAliasPropertyName, UpdateFlags(aliasProperty, aliasProperty.Flags | TypePropertyFlags.Identifier | TypePropertyFlags.Required | TypePropertyFlags.LoopVariant));

                var functions = Array.Empty<Semantics.FunctionOverload>();
                return new ObjectType(
                    objectType.Name,
                    objectType.ValidationFlags,
                    isExistingResource ? HandleExistingResource(properties.Values) : properties.Values,
                    objectType.AdditionalPropertiesType,
                    isExistingResource ? ConvertToReadOnly(objectType.AdditionalPropertiesFlags) : objectType.AdditionalPropertiesFlags,
                    functions: functions);
            }

            // Get the type here
            // Applications.Datastores/mongoDatabases

            return new ObjectType(
                objectType.Name,
                objectType.ValidationFlags,
                isExistingResource ? HandleExistingResource(properties.Values) : properties.Values,
                objectType.AdditionalPropertiesType,
                isExistingResource ? ConvertToReadOnly(objectType.AdditionalPropertiesFlags) : objectType.AdditionalPropertiesFlags,
                functions: null);
        }

        private static IEnumerable<TypeProperty> HandleExistingResource(IEnumerable<TypeProperty> properties)
        {
            foreach (var property in properties)
            {
                // We want to mark only identifier properties are required, everything else should be optional and readonly
                if (property.TypeReference.Type is ObjectType curObj)
                {
                    var visited = HandleExistingResource(curObj.Properties.Values);
                    var propsWithIdentifier = visited.Where(p => p.Flags.HasFlag(TypePropertyFlags.Identifier)).Count();
                    var curPropFlags = propsWithIdentifier > 0 ? MakeRequired(curObj.AdditionalPropertiesFlags) : curObj.AdditionalPropertiesFlags;

                    yield return new TypeProperty(property.Name, new ObjectType(curObj.Name, curObj.ValidationFlags, visited, curObj.AdditionalPropertiesType), curPropFlags);
                }
                else
                {
                    yield return new TypeProperty(property.Name, property.TypeReference, HandleIdentifierProperty(property.Flags));
                }
            }
        }

        private static TypePropertyFlags HandleIdentifierProperty(TypePropertyFlags typePropertyFlags)
        {
            if (typePropertyFlags.HasFlag(TypePropertyFlags.Identifier)) 
            {
                return MakeRequired(typePropertyFlags);
            }
            else
            {
                return ConvertToReadOnly(typePropertyFlags);
            }
        }

        private static TypePropertyFlags MakeRequired(TypePropertyFlags typePropertyFlags)
            => (typePropertyFlags | TypePropertyFlags.Required) & ~TypePropertyFlags.ReadOnly;

        private static TypePropertyFlags ConvertToReadOnly(TypePropertyFlags typePropertyFlags)
            => (typePropertyFlags | TypePropertyFlags.ReadOnly) & ~TypePropertyFlags.Required;

        public ResourceType? TryGetDefinedType(NamespaceType declaringNamespace, ResourceTypeReference typeReference, ResourceTypeGenerationFlags flags)
        {
            if (!HasDefinedType(typeReference))
            {
                return null;
            }

            // It's important to cache this result because generating the resource type is an expensive operation
            var resourceType = definedTypeCache.GetOrAdd(flags, typeReference, () =>
            {
                var resourceType = this.resourceTypeLoader.LoadType(typeReference);

                return SetBicepResourceProperties(resourceType, flags);
            });

            return new(
                declaringNamespace,
                resourceType.TypeReference,
                resourceType.ValidParentScopes,
                resourceType.ReadOnlyScopes,
                resourceType.Flags,
                resourceType.Body,
                UniqueIdentifierProperties);
        }

        public ResourceType? TryGenerateFallbackType(NamespaceType declaringNamespace, ResourceTypeReference typeReference, ResourceTypeGenerationFlags flags)
            => null;

        public bool HasDefinedType(ResourceTypeReference typeReference)
            => availableResourceTypes.Contains(typeReference);

        public IEnumerable<ResourceTypeReference> GetAvailableTypes()
            => availableResourceTypes;
    }
}
