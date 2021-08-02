// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.TypeSystem.Radius;

namespace Bicep.Core.TypeSystem.Radiusv1alpha3u
{
    internal static class KnownTypes
    {
        private const string Version = "v1alpha3u";

        public static ResourceType MakeApplication(IResourceTypeProvider provider)
        {
            var propertiesType = new ObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.None);

            var bodyType = new ObjectType(
                name: $"{RadiusResources.ApplicationResourceType}@{Version}",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse($"{RadiusResources.ApplicationResourceType}@{Version}"),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        public static ResourceType MakeScope(IResourceTypeProvider provider)
        {
            var members = new ITypeReference[]
            {
                MakeDaprScope(),
                MakeNetworkScope(),
            };

            var bodyType = new DiscriminatedObjectType(
                name: $"{RadiusResources.ScopeResourceType}@{Version}",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                discriminatorKey: "kind",
                members);

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse($"{RadiusResources.ScopeResourceType}@{Version}"),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        public static ResourceType MakeComponent(IResourceTypeProvider provider)
        {
            var members = new ITypeReference[]
            {
                MakeContainer(),
                MakeDaprStateStore(),
                MakeDaprPubSubTopic(),
                MakeMongoDB(),
                MakeCosmosDBMongo(),
                MakeCosmosDBSQL(),
                MakeKeyVault(),
                MakeServiceBusQueue(),
                MakeRedis(),
            };

            var bodyType = new DiscriminatedObjectType(
                name: $"{RadiusResources.ComponentResourceType}@{Version}",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                discriminatorKey: "kind",
                members);

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse($"{RadiusResources.ComponentResourceType}@{Version}"),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        public static ResourceType MakeTopLevelBinding(IResourceTypeProvider provider)
        {
            return new ResourceType(
                typeReference: ResourceTypeReference.Parse($"{RadiusResources.BindingResourceType}@{Version}"),
                validParentScopes: ResourceScope.ResourceGroup,
                body: CommonBindings.BindingResourceBodyType,
                provider: provider);
        }

        public static ResourceType MakeScopeBinding(IResourceTypeProvider provider)
        {
            return new ResourceType(
                typeReference: ResourceTypeReference.Parse($"{RadiusResources.ScopeBindingResourceType}@{Version}"),
                validParentScopes: ResourceScope.ResourceGroup,
                body: CommonBindings.BindingResourceBodyType,
                provider: provider);
        }

        public static ObjectType MakeContainer()
        {
            var kind = $"radius.dev/Container@{Version}";

            var envType = new ObjectType(
                name: "env",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: LanguageConstants.String,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var envProperty = new TypeProperty("env", envType, TypePropertyFlags.None);

            var portType = new ObjectType(
                name: "port",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("containerPort", LanguageConstants.Int, TypePropertyFlags.Required),
                    new TypeProperty("protocol", UnionType.Create(new StringLiteralType("TCP"), new StringLiteralType("UDP")), TypePropertyFlags.None),
                    new TypeProperty("provides", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var portsType = new ObjectType(
                name: "ports",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: portType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var portsProperty = new TypeProperty("ports", portsType, TypePropertyFlags.None);

            var imageProperty = new TypeProperty("image", LanguageConstants.String, TypePropertyFlags.Required);
            var containerType = new ObjectType(
                "container",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    imageProperty,
                    envProperty,
                    portsProperty,
                },
                additionalPropertiesType: LanguageConstants.Any,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var containerProperty = new TypeProperty("container", containerType, TypePropertyFlags.Required);

            var runType = new ObjectType(
                "run",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    containerProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var runProperty = new TypeProperty("run", runType, TypePropertyFlags.Required);

            return MakeComponentType(kind, run: runType);
        }

        public static ObjectType MakeDaprStateStore()
        {
            var kind = $"dapr.io/StateStore@{Version}";
            var configKindType = UnionType.Create(
                new StringLiteralType("state.azure.tablestorage"),
                new StringLiteralType("state.sqlserver"),
                new StringLiteralType("any"));

            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("kind", configKindType, TypePropertyFlags.Required),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, CommonBindings.BindingData>()
            {
                { "default", CommonBindings.BindingDataDaprStateStore },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeDaprPubSubTopic()
        {
            var kind = $"dapr.io/PubSubTopic@{Version}";
            var configKindType = UnionType.Create(new StringLiteralType("pubsub.azure.servicebus"), new StringLiteralType("any"));

            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("kind", configKindType, TypePropertyFlags.Required),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                    new TypeProperty("topic", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, CommonBindings.BindingData>()
            {
                { "default", CommonBindings.BindingDataDaprPubSubTopic },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeServiceBusQueue()
        {
            var kind = $"azure.com/ServiceBusQueue@{Version}";
            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                    new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, CommonBindings.BindingData>()
            {
                { "default", CommonBindings.BindingDataServiceBusQueue }
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeRedis()
        {
            var kind = $"redislabs.com/Redis@{Version}";
            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, CommonBindings.BindingData>()
            {
                { "default", CommonBindings.BindingDataRedis },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeCosmosDBMongo()
        {
            var kind = $"azure.com/CosmosDBMongo@{Version}";
            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, CommonBindings.BindingData>()
            {
                { "mongo", CommonBindings.BindingDataMongo },
                { "cosmos", CommonBindings.BindingDataCosmosMongo },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeCosmosDBSQL()
        {
            var kind = $"azure.com/CosmosDBSQL@{Version}";
            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, CommonBindings.BindingData>()
            {
                { "sql", CommonBindings.BindingDataSQL },
                { "cosmos", CommonBindings.BindingDataCosmosSQL },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeKeyVault()
        {
            var kind = $"azure.com/KeyVault@{Version}";
            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, CommonBindings.BindingData>()
            {
                { "default", CommonBindings.BindingDataKeyVault },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeMongoDB()
        {
            var kind = $"mongodb.com/Mongo@{Version}";
            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, CommonBindings.BindingData>()
            {
                { "mongo", CommonBindings.BindingDataMongo },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeNetworkScope()
        {
            var kind = $"radius.dev/Network@{Version}";
            var properties = new List<TypeProperty>()
            {
                new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
            };

            return MakeScopeType(kind, properties);
        }

        public static ObjectType MakeDaprScope()
        {
            var featuresType = new ObjectType(
                name: "features",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: LanguageConstants.Bool,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: Array.Empty<FunctionOverload>());

            var kind = $"dapr.io/Dapr@{Version}";
            var properties = new List<TypeProperty>()
            {
                new TypeProperty("features", featuresType, TypePropertyFlags.None),
            };

            return MakeScopeType(kind, properties);
        }

        private static ObjectType MakeScopeType(string kind, IEnumerable<TypeProperty> properties)
        {
            var propertiesType = new ObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.None);

            var kindProperty = new TypeProperty("kind", new StringLiteralType(kind), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new ObjectType(
                name: kind,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.OptionalName,
                    CommonProperties.Default,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    kindProperty,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: Array.Empty<FunctionOverload>());
        }

        private static ObjectType MakeComponentType(
            string kind,
            ObjectType? run = null,
            ObjectType? config = null,
            Dictionary<string, CommonBindings.BindingData>? bindings = null)
        {
            if (run == null && config == null)
            {
                throw new InvalidOperationException("either run or config must be specified");
            }

            var properties = new List<TypeProperty>()
            {
                CommonProperties.Traits,
                CommonProperties.Scopes,
            };

            if (bindings != null)
            {
                properties.Add(CommonBindings.MakeBindingsProperty(bindings));
            }

            if (run != null)
            {
                properties.Add(new TypeProperty("run", run, TypePropertyFlags.Required));
                properties.Add(CommonProperties.ComponentUses);
            }

            if (config != null)
            {
                properties.Add(new TypeProperty("config", config, TypePropertyFlags.Required));
            }

            var propertiesType = new ObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            var kindProperty = new TypeProperty("kind", new StringLiteralType(kind), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new ObjectType(
                name: kind,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    kindProperty,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: Array.Empty<FunctionOverload>());
        }
    }
}
