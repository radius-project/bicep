// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bicep.Core.TypeSystem.Radius
{
    public static class CommonBindings
    {
        public static ObjectType BindingHttp = new ObjectType(
            "binding: http",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("http"), TypePropertyFlags.Required),
                new TypeProperty("scheme", LanguageConstants.String, TypePropertyFlags.None),
                new TypeProperty("host", LanguageConstants.String, TypePropertyFlags.None),
                new TypeProperty("port", LanguageConstants.Int, TypePropertyFlags.None),
                new TypeProperty("targetPort", LanguageConstants.Int, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingDaprInvoke = new ObjectType(
            "binding: dapr.io/Invoke",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("dapr.io/Invoke"), TypePropertyFlags.Required),
                new TypeProperty("appId", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingDaprPubSubTopic = new ObjectType(
            "binding: dapr.io/PubSubTopic",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("dapr.io/PubSubTopic"), TypePropertyFlags.Required),
                new TypeProperty("pubSubName", LanguageConstants.String, TypePropertyFlags.None),
                new TypeProperty("topic", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingDaprStateStore = new ObjectType(
            "binding: dapr.io/StateStore",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("dapr.io/StateStore"), TypePropertyFlags.Required),
                new TypeProperty("stateStoreName", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingRedis = new ObjectType(
            "binding: redislabs.com/Redis",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("redislabs.com/Redis"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingRabbitMQ = new ObjectType(
            "binding: rabbitmq.com/MessageQueue",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("rabbitmq.com/MessageQueue"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
                new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.Required),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingMongo = new ObjectType(
            "binding: mongo.com/MongoDB",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("mongo.com/MongoDB"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingCosmosDBMongo = new ObjectType(
            "binding: azure.com/CosmosDBMongo",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("azure.com/CosmosDBMongo"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingSQL = new ObjectType(
            "binding: microsoft.com/SQL",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("microsoft.com/SQL"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingCosmosDBSQL = new ObjectType(
            "binding: azure.com/CosmosDBSQL",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("azure.com/CosmosDBSQL"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingKeyVault = new ObjectType(
            "binding: azure.com/KeyVault",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("azure.com/KeyVault"), TypePropertyFlags.Required),
                new TypeProperty("uri", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static ObjectType BindingServiceBusQueue = new ObjectType(
            "binding: azure.com/ServiceBusQueue",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("azure.com/ServiceBusQueue"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
                new TypeProperty("namespace", LanguageConstants.String, TypePropertyFlags.None),
                new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static readonly DiscriminatedObjectType BindingType = new DiscriminatedObjectType(
            "binding",
            TypeSymbolValidationFlags.Default,
            "kind", new ITypeReference[]
            {
                BindingHttp,
                BindingDaprInvoke,
                BindingDaprPubSubTopic,
                BindingDaprStateStore,
                BindingKeyVault,
                BindingMongo,
                BindingCosmosDBMongo,
                BindingSQL,
                BindingCosmosDBSQL,
                BindingRedis,
            });

        public static TypeProperty MakeBindingsProperty(Dictionary<string, ITypeReference>? builtIn)
        {
            var properties = builtIn?.Select(kvp =>
            {
                return new TypeProperty(kvp.Key, kvp.Value, TypePropertyFlags.None);
            }).ToArray() ?? Array.Empty<TypeProperty>();

            var bindingsType = new ObjectType(
                "bindings",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: properties,
                additionalPropertiesType: CommonBindings.BindingType,
                additionalPropertiesFlags: TypePropertyFlags.None);

            return new TypeProperty("bindings", bindingsType, TypePropertyFlags.None);
        }
    }
}
