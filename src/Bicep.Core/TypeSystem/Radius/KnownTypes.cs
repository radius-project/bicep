// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;

namespace Bicep.Core.TypeSystem.Radius
{
    internal static class KnownTypes
    {
        public static NamedObjectType MakeApplication()
        {
            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.None);

            return new NamedObjectType(
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
        }

        public static NamedObjectType MakeDeployment()
        {
            var componentNameProperty = new TypeProperty("componentName", LanguageConstants.String, TypePropertyFlags.None);
            var componentEntryType = new NamedObjectType(
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

            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    componentsProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            return new NamedObjectType(
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
        }

        public static DiscriminatedObjectType MakeComponent()
        {
            var members = new ITypeReference[]
            {
                MakeContainer(),
                MakeDaprStateStore(),
                MakeDaprPubSubTopic(),
                MakeCosmosDBMongo(),
                MakeCosmosDBSQL(),
                MakeKeyVault(),
                MakeServiceBusQueue(),
                MakeRedis(),
            };

            var type = new DiscriminatedObjectType(
                name: "radius.dev/Applications/Components@v1alpha1",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                discriminatorKey: "kind",
                members);
            return type;
        }

        public static NamedObjectType MakeContainer()
        {
            var kind = "radius.dev/Container@v1alpha1";

            var imageProperty = new TypeProperty("image", LanguageConstants.String, TypePropertyFlags.Required);
            var containerType = new NamedObjectType(
                "container",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    imageProperty
                },
                additionalPropertiesType: LanguageConstants.Any,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var containerProperty = new TypeProperty("container", containerType, TypePropertyFlags.Required);

            var runType = new NamedObjectType(
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

        public static NamedObjectType MakeDaprStateStore()
        {
            var kind = "dapr.io/StateStore@v1alpha1";
            var configKindType = UnionType.Create(
                new StringLiteralType("state.azure.tablestorage"),
                new StringLiteralType("state.sqlserver"),
                new StringLiteralType("any"));

            var configType = new NamedObjectType(
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

        public static NamedObjectType MakeDaprPubSubTopic()
        {
            var kind = "dapr.io/PubSubTopic@v1alpha1";
            var configKindType = UnionType.Create(new StringLiteralType("pubsub.azure.servicebus"), new StringLiteralType("any"));

            var configType = new NamedObjectType(
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

        public static NamedObjectType MakeServiceBusQueue()
        {
            var kind = "azure.com/ServiceBusQueue@v1alpha1";
            var configType = new NamedObjectType(
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

        public static NamedObjectType MakeRedis()
        {
            var kind = "redislabs.com/Redis@v1alpha1";
            var configType = new NamedObjectType(
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

        public static NamedObjectType MakeCosmosDBMongo()
        {
            var kind = "azure.com/CosmosDBMongo@v1alpha1";
            var configType = new NamedObjectType(
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

        public static NamedObjectType MakeCosmosDBSQL()
        {
            var kind = "azure.com/CosmosDBSQL@v1alpha1";
            var configType = new NamedObjectType(
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

        public static NamedObjectType MakeKeyVault()
        {
            var kind = "azure.com/KeyVault@v1alpha1";
            var configType = new NamedObjectType(
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

        private static NamedObjectType MakeComponentType(
            string kind,
            NamedObjectType? run = null,
            NamedObjectType? config = null,
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

            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            var kindProperty = new TypeProperty("kind", new StringLiteralType(kind), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new NamedObjectType(
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
