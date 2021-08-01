// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bicep.Core.TypeSystem.Radius
{
    public static class CommonBindings
    {
        public static NamedObjectType BindingHttp = new NamedObjectType(
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

        public static NamedObjectType BindingDaprInvoke = new NamedObjectType(
            "binding: dapr.io/Invoke",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("dapr.io/Invoke"), TypePropertyFlags.Required),
                new TypeProperty("appId", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static NamedObjectType BindingDaprPubSubTopic = new NamedObjectType(
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

        public static NamedObjectType BindingDaprStateStore = new NamedObjectType(
            "binding: dapr.io/StateStore",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("dapr.io/StateStore"), TypePropertyFlags.Required),
                new TypeProperty("stateStoreName", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static NamedObjectType BindingRedis = new NamedObjectType(
            "binding: redislabs.com/Redis",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("redislabs.com/Redis"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static NamedObjectType BindingMongo = new NamedObjectType(
            "binding: mongo.com/MongoDB",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("mongo.com/MongoDB"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static NamedObjectType BindingCosmosDBMongo = new NamedObjectType(
            "binding: azure.com/CosmosDBMongo",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("azure.com/CosmosDBMongo"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static NamedObjectType BindingSQL = new NamedObjectType(
            "binding: microsoft.com/SQL",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("microsoft.com/SQL"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static NamedObjectType BindingCosmosDBSQL = new NamedObjectType(
            "binding: azure.com/CosmosDBSQL",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("azure.com/CosmosDBSQL"), TypePropertyFlags.Required),
                new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static NamedObjectType BindingKeyVault = new NamedObjectType(
            "binding: azure.com/KeyVault",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("kind", new StringLiteralType("azure.com/KeyVault"), TypePropertyFlags.Required),
                new TypeProperty("uri", LanguageConstants.String, TypePropertyFlags.None),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static NamedObjectType BindingServiceBusQueue = new NamedObjectType(
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

            var bindingsType = new NamedObjectType(
                "bindings",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: properties,
                additionalPropertiesType: CommonBindings.BindingType,
                additionalPropertiesFlags: TypePropertyFlags.None);

            return new TypeProperty("bindings", bindingsType, TypePropertyFlags.None);
        }
    }
}
