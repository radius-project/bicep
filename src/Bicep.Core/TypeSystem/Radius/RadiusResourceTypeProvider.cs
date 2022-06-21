// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Resources;
using System.Collections.Immutable;
using System.Collections.Concurrent;
using Bicep.Core.Syntax;
using Bicep.Core.Semantics;

namespace Bicep.Core.TypeSystem.Radius
{
    public class RadiusResourceTypeProvider : IResourceTypeProvider
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

        private static Dictionary<string, Func<string, string, string, IEnumerable<Semantics.FunctionOverload>>> FunctionTable = new()
        {
            {
                "Applications.Connector", (string apiVersion, string type, string name) => new []
                {
                    new Semantics.FunctionOverloadBuilder("connectionString")
                        .WithReturnType(LanguageConstants.String)
                        .WithEvaluator(Eval(apiVersion, type, name))
                        .Build(),
                }
            },
        };


        // private static SyntaxBase EvaluateSecret(CommonBindings.BindingValue value, FunctionCallSyntaxBase functionCall, Symbol symbol, TypeSymbol typeSymbol)
        // {
        //     // POST /subus...../resurceProviders/radiusV3/listSecrets
        //     // {
        //     //    targetId: ....
        //     // }
        //     //
        //     // A function like foo.connectionString() is replaced with code like:
        //     // listSecrets(resourceId('Microsoft.CustomProviders/resourceProviders', 'radiusv3'), '2018-09-01-preview', { 'targetID': resourceId(...) }).connectionString
        //     //
        //     // - The former resourceId is the ID of the CustomRP - this is a limitation we have to live with
        //     // - The latter resourceId is the ID of the Radius resource being accessed.

        //     var instance = (InstanceFunctionCallSyntax)functionCall;

        //     var customProviderResourceIdArgumentExpression = SyntaxFactory.CreateFunctionCall(
        //         "resourceId",
        //         SyntaxFactory.CreateStringLiteral(RadiusResources.ProviderCRPType),
        //         SyntaxFactory.CreateStringLiteral(RadiusResources.ProviderCRPName));


        //     var targetResourceIdExpression = SyntaxFactory.CreatePropertyAccess(instance.BaseExpression, "id");
        //     var customActionDataArgumentExpression = SyntaxFactory.CreateObject(new[]
        //     {
        //         SyntaxFactory.CreateObjectProperty("targetId", targetResourceIdExpression),
        //     });

        //     var functionCallExpression = SyntaxFactory.CreateFunctionCall(
        //         "listSecrets",
        //         customProviderResourceIdArgumentExpression,
        //         SyntaxFactory.CreateStringLiteral(RadiusResources.CRPApiVersion),
        //         customActionDataArgumentExpression);


        //     return SyntaxFactory.CreatePropertyAccess(functionCallExpression, value.Name);
        // }

        private static Semantics.FunctionOverload.EvaluatorDelegate Eval(string apiVersion, string type, string name)
        {
            return (FunctionCallSyntaxBase functionCall, Symbol symbol, TypeSymbol typeSymbol, FunctionVariable? functionVariable,  object? functionResultValue) =>
            {
                var instance = (InstanceFunctionCallSyntax)functionCall;
                var functionSymbol = (FunctionSymbol)symbol;

                var customProviderResourceIdArgumentExpression = SyntaxFactory.CreateFunctionCall(
                    "resourceId",
                    SyntaxFactory.CreateStringLiteral(type),
                    SyntaxFactory.CreateStringLiteral(name));

                var targetResourceIdExpression = SyntaxFactory.CreatePropertyAccess(instance.BaseExpression, "id");
                var customActionDataArgumentExpression = SyntaxFactory.CreateObject(new[]
                {
                    SyntaxFactory.CreateObjectProperty("targetId", targetResourceIdExpression),
                });
                // listSecrets(resourceId('Microsoft.CustomProviders/resourceProviders', 'radiusv3'), '2018-09-01-preview', { 'targetID': resourceId(...) }).connectionString
                // "[listSecrets(reference('mongo'), '2022-03-15-privatepreview')]"
                // listSecrets()
                // (mongo.listSecrets('2022-03-15-privatepreview', {...}).connectionString
                // return new PropertyAccessSyntax(baseExpression, SyntaxFactory.DotToken, SyntaxFactory.CreateIdentifier(propertyName));

                // [listSecrets(resourceId('Microsoft.CustomProviders/resourceProviders', 'radiusv3'), '2018-09-01-preview', createObject('targetId', resourceId('Microsoft.CustomProviders/resourceProviders/Application/mongodb.com.MongoDBComponent', 'radiusv3', 'app', 'db'))).connectionString]
                var listSecretsFunc = SyntaxFactory.CreateFunctionCall(
                    "listSecrets",
                    customProviderResourceIdArgumentExpression,
                    SyntaxFactory.CreateStringLiteral(apiVersion),
                    customActionDataArgumentExpression);

                return SyntaxFactory.CreatePropertyAccess(listSecretsFunc, "connectionString");

/*propertyacces
    (instancefunctioncall(
        'mongo'
        'listSecrets'

    ))
*/
            };
        }

        public const string ResourceNamePropertyName = "name";

        public static readonly TypeSymbol Tags = new ObjectType(nameof(Tags), TypeSymbolValidationFlags.Default, Enumerable.Empty<TypeProperty>(), LanguageConstants.String, TypePropertyFlags.None);

        private readonly RadiusResourceTypeLoader resourceTypeLoader;
        private readonly ImmutableHashSet<ResourceTypeReference> availableResourceTypes;
        private readonly ResourceTypeCache definedTypeCache;
        private readonly ResourceTypeCache generatedTypeCache;

        public static readonly ImmutableHashSet<string> UniqueIdentifierProperties = new[]
        {
            ResourceNamePropertyName,
        }.ToImmutableHashSet();

        public RadiusResourceTypeProvider(RadiusResourceTypeLoader resourceTypeLoader)
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

            return new ResourceTypeComponents(resourceType.TypeReference, resourceType.ValidParentScopes, bodyType);
        }

        private static ObjectType SetBicepResourceProperties(ObjectType objectType, ResourceScope validParentScopes, ResourceTypeReference typeReference, ResourceTypeGenerationFlags flags)
        {
            // Local function.
            static TypeProperty UpdateFlags(TypeProperty typeProperty, TypePropertyFlags flags) =>
                new(typeProperty.Name, typeProperty.TypeReference, flags, typeProperty.Description);

            var properties = objectType.Properties;
            var isExistingResource = flags.HasFlag(ResourceTypeGenerationFlags.ExistingResource);

            if (isExistingResource)
            {
            }
            else
            {
                // TODO: remove 'dependsOn' from the type library
                properties = properties.SetItem(LanguageConstants.ResourceDependsOnPropertyName, new TypeProperty(LanguageConstants.ResourceDependsOnPropertyName, LanguageConstants.ResourceOrResourceCollectionRefArray, TypePropertyFlags.WriteOnly | TypePropertyFlags.DisallowAny));
            }

            // add the loop variant flag to the name prope rty (if it exists)
            if (properties.TryGetValue(ResourceNamePropertyName, out var nameProperty))
            {
                // TODO apply this to all unique properties
                properties = properties.SetItem(ResourceNamePropertyName, UpdateFlags(nameProperty, nameProperty.Flags | TypePropertyFlags.LoopVariant));
                var type = objectType.Name.Split("/");

                FunctionTable.TryGetValue(type[0], out var functionBuilder);
                var functions = functionBuilder?.Invoke(typeReference.ApiVersion!, typeReference.FormatType(), nameProperty.Name) ?? Array.Empty<Semantics.FunctionOverload>();
                return new ObjectType(
                    objectType.Name,
                    objectType.ValidationFlags,
                    isExistingResource ? ConvertToReadOnly(properties.Values) : properties.Values,
                    objectType.AdditionalPropertiesType,
                    isExistingResource ? ConvertToReadOnly(objectType.AdditionalPropertiesFlags) : objectType.AdditionalPropertiesFlags,
                    functions: functions);
            }

            // Get the type here
            // Applications.Connector/mongoDatabase

            return new ObjectType(
                objectType.Name,
                objectType.ValidationFlags,
                isExistingResource ? ConvertToReadOnly(properties.Values) : properties.Values,
                objectType.AdditionalPropertiesType,
                isExistingResource ? ConvertToReadOnly(objectType.AdditionalPropertiesFlags) : objectType.AdditionalPropertiesFlags,
                functions: null);
        }

        private static IEnumerable<TypeProperty> ConvertToReadOnly(IEnumerable<TypeProperty> properties)
        {
            foreach (var property in properties)
            {
                // "name", "scope" & "parent" can be set for existing resources - everything else should be read-only
                if (UniqueIdentifierProperties.Contains(property.Name))
                {
                    yield return property;
                }
                else
                {
                    yield return new TypeProperty(property.Name, property.TypeReference, ConvertToReadOnly(property.Flags));
                }
            }
        }

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

            return new(declaringNamespace, resourceType.TypeReference, resourceType.ValidParentScopes, resourceType.Body, UniqueIdentifierProperties);
        }

        public ResourceType? TryGenerateFallbackType(NamespaceType declaringNamespace, ResourceTypeReference typeReference, ResourceTypeGenerationFlags flags)
            => null;

        public bool HasDefinedType(ResourceTypeReference typeReference)
            => availableResourceTypes.Contains(typeReference);

        public IEnumerable<ResourceTypeReference> GetAvailableTypes()
            => availableResourceTypes;
    }
}
