// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using Bicep.Core.Resources;

namespace Bicep.Core.TypeSystem.Radius
{
    internal static class KnownTypes
    {
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
                name: "radius.dev/Applications@v1alpha1",
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

            return new ResourceType(ResourceTypeReference.Parse("radius.dev/Applications@v1alpha1"), ResourceScope.ResourceGroup, bodyType, provider);
        }

        public static ResourceType MakeDeployment(IResourceTypeProvider provider)
        {
            var componentNameProperty = new TypeProperty("componentName", LanguageConstants.String, TypePropertyFlags.None);
            var componentEntryType = new ObjectType(
                name: "components",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new []
                {
                    componentNameProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var componentsProperty = new TypeProperty(
                "components",
                new TypedArrayType(componentEntryType, TypeSymbolValidationFlags.Default),
                TypePropertyFlags.Required);

            var propertiesType = new ObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    componentsProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            var bodyType = new ObjectType(
                name: "radius.dev/Applications/Deployments@v1alpha1",
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

            return new ResourceType(ResourceTypeReference.Parse("radius.dev/Applications/Deployments@v1alpha1"), ResourceScope.ResourceGroup, bodyType, provider);
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
                MakeRabbitMQ(),
            };

            var bodyType = new DiscriminatedObjectType(
                name: "radius.dev/Applications/Components@v1alpha1",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                discriminatorKey: "kind",
                members);

            return new ResourceType(ResourceTypeReference.Parse("radius.dev/Applications/Components@v1alpha1"), ResourceScope.ResourceGroup, bodyType, provider);
        }

        public static ObjectType MakeContainer()
        {
            var kind = "radius.dev/Container@v1alpha1";

            var envType = new ObjectType(
                name: "env",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: LanguageConstants.String,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var envProperty = new TypeProperty("env", envType, TypePropertyFlags.None);

            var imageProperty = new TypeProperty("image", LanguageConstants.String, TypePropertyFlags.Required);
            var containerType = new ObjectType(
                "container",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    imageProperty,
                    envProperty,
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
            var kind = "dapr.io/StateStore@v1alpha1";
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

            var bindings = new Dictionary<string, ITypeReference>()
            {
                { "default", CommonBindings.BindingDaprStateStore },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeDaprPubSubTopic()
        {
            var kind = "dapr.io/PubSubTopic@v1alpha1";
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

            var bindings = new Dictionary<string, ITypeReference>()
            {
                { "default", CommonBindings.BindingDaprPubSubTopic },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeServiceBusQueue()
        {
            var kind = "azure.com/ServiceBusQueue@v1alpha1";
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

            var bindings = new Dictionary<string, ITypeReference>()
            {
                { "default", CommonBindings.BindingServiceBusQueue }
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeRedis()
        {
            var kind = "redislabs.com/Redis@v1alpha1";
            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, ITypeReference>()
            {
                { "default", CommonBindings.BindingRedis },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeRabbitMQ()
        {
            var kind = "rabbitmq.com/MessageQueue@v1alpha1";
            var configType = new ObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var bindings = new Dictionary<string, ITypeReference>()
            {
                { "default", CommonBindings.BindingRabbitMQ },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeCosmosDBMongo()
        {
            var kind = "azure.com/CosmosDBMongo@v1alpha1";
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

            var bindings = new Dictionary<string, ITypeReference>()
            {
                { "mongo", CommonBindings.BindingMongo },
                { "cosmos", CommonBindings.BindingCosmosDBMongo },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeCosmosDBSQL()
        {
            var kind = "azure.com/CosmosDBSQL@v1alpha1";
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

            var bindings = new Dictionary<string, ITypeReference>()
            {
                { "sql", CommonBindings.BindingSQL },
                { "cosmos", CommonBindings.BindingCosmosDBSQL },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeKeyVault()
        {
            var kind = "azure.com/KeyVault@v1alpha1";
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

            var bindings = new Dictionary<string, ITypeReference>()
            {
                { "default", CommonBindings.BindingKeyVault },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        public static ObjectType MakeMongoDB()
        {
            var kind = "mongodb.com/Mongo@v1alpha1";
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

            var bindings = new Dictionary<string, ITypeReference>()
            {
                { "mongo", CommonBindings.BindingMongo },
            };

            return MakeComponentType(kind, config: configType, bindings: bindings);
        }

        private static ObjectType MakeComponentType(
            string kind,
            ObjectType? run = null,
            ObjectType? config = null,
            Dictionary<string, ITypeReference>? bindings = null)
        {
            if (run == null && config == null)
            {
                throw new InvalidOperationException("either run or config must be specified");
            }

            var properties = new List<TypeProperty>()
            {
                CommonBindings.MakeBindingsProperty(bindings),
                CommonProperties.Traits,
                CommonProperties.Scopes,
            };

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
                additionalPropertiesFlags: TypePropertyFlags.None);
        }
    }
}
