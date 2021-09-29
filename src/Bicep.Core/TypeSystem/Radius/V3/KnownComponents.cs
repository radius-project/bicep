// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bicep.Core.TypeSystem.Radius.V3
{
    public static class KnownComponents
    {
        public class ComponentData
        {
            public ThreePartType Type { get; set; } = default!;

            public CommonBindings.BindingData? Binding { get; set; }

            public List<TypeProperty> Properties { get; } = new List<TypeProperty>();
        }

        public static ComponentData MakeContainer()
        {
            var members = new List<ObjectType>();

            var connectionType = new DiscriminatedObjectType(
                name: "connection",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: CommonBindings.AllBindingData.Select(b =>
                {
                    return new ObjectType(
                        name: $"connection {b.Type.FormatKind()}",
                        validationFlags: TypeSymbolValidationFlags.Default,
                        properties: new []
                        {
                            new TypeProperty("kind", new StringLiteralType(b.Type.FormatKind()), TypePropertyFlags.Required, "The kind of connection"),
                            new TypeProperty("source", LanguageConstants.String, TypePropertyFlags.Required, "The source of the connection"),
                        },
                        additionalPropertiesType: null,
                        additionalPropertiesFlags: TypePropertyFlags.None,
                        functions: null);
                }));

            var connectionsType = new ObjectType(
                name: "connections",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: connectionType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var connectionsProperty = new TypeProperty("connections", connectionsType, TypePropertyFlags.None, "Specify named connections for the component");

            var envItemType = LanguageConstants.LooseString;

            var envType = new ObjectType(
                name: "env",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: envItemType,
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

            var headersType = new ObjectType(
                name: "headers",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: LanguageConstants.String,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var httpGet = new ObjectType(
                name: "httpGet",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("containerPort", LanguageConstants.Int, TypePropertyFlags.Required, "The listening port number"),
                    new TypeProperty("path", LanguageConstants.String, TypePropertyFlags.Required, "The route to make the HTTP request on"),
                    new TypeProperty("headers", headersType, TypePropertyFlags.None, "Custom HTTP headers to add to the get request"),
                    new TypeProperty("kind", new StringLiteralType("httpGet"), TypePropertyFlags.Required, "Health probe kind"),
                    new TypeProperty("initialDelaySeconds", LanguageConstants.Int, TypePropertyFlags.None, "Initial delay in seconds before probing for readiness/liveness"),
                    new TypeProperty("failureThreshold", LanguageConstants.Int, TypePropertyFlags.None, "Threshold number of times the probe fails after which a failure would be reported"),
                    new TypeProperty("periodSeconds", LanguageConstants.Int, TypePropertyFlags.None, "Interval for the readiness/liveness probe in seconds"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var tcp = new ObjectType(
                name: "tcp",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("containerPort", LanguageConstants.Int, TypePropertyFlags.Required, "The listening port number"),
                    new TypeProperty("kind", new StringLiteralType("tcp"), TypePropertyFlags.Required, "Health probe kind"),
                    new TypeProperty("initialDelaySeconds", LanguageConstants.Int, TypePropertyFlags.None, "Initial delay in seconds before probing for readiness/liveness"),
                    new TypeProperty("failureThreshold", LanguageConstants.Int, TypePropertyFlags.None, "Threshold number of times the probe fails after which a failure would be reported"),
                    new TypeProperty("periodSeconds", LanguageConstants.Int, TypePropertyFlags.None, "Interval for the readiness/liveness probe in seconds"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var exec = new ObjectType(
                name: "exec",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("command", LanguageConstants.String, TypePropertyFlags.Required, "Command to execute to probe readiness/liveness"),
                    new TypeProperty("kind", new StringLiteralType("exec"), TypePropertyFlags.Required, "Health probe kind"),
                    new TypeProperty("initialDelaySeconds", LanguageConstants.Int, TypePropertyFlags.None, "Initial delay in seconds before probing for readiness/liveness"),
                    new TypeProperty("failureThreshold", LanguageConstants.Int, TypePropertyFlags.None, "Threshold number of times the probe fails after which a failure would be reported"),
                    new TypeProperty("periodSeconds", LanguageConstants.Int, TypePropertyFlags.None, "Interval for the readiness/liveness probe in seconds"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var healthProbeType = new DiscriminatedObjectType(
                name: "healthProbe",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: new ITypeReference[]{httpGet, tcp, exec}
                );
            
            var readinessProperty = new TypeProperty("readinessProbe", healthProbeType, TypePropertyFlags.None, "Readiness health probe");
            var livessProperty = new TypeProperty("livenessProbe", healthProbeType, TypePropertyFlags.None, "Liveness health probe");

            var imageProperty = new TypeProperty("image", LanguageConstants.String, TypePropertyFlags.Required);
            var containerType = new ObjectType(
                "container",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    imageProperty,
                    envProperty,
                    portsProperty,
                    readinessProperty,
                    livessProperty
                },
                additionalPropertiesType: LanguageConstants.Any,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var containerProperty = new TypeProperty("container", containerType, TypePropertyFlags.Required);

            return new ComponentData()
            {
                Type = new ThreePartType(null, "Container", RadiusResources.CategoryComponent),
                Properties =
                {
                    connectionsProperty,
                    containerProperty
                },
            };
        }

        public static ComponentData MakeDaprStateStore()
        {
            var configKindType = UnionType.Create(
                new StringLiteralType("state.azure.tablestorage"),
                new StringLiteralType("state.sqlserver"),
                new StringLiteralType("any"));

            return new ComponentData()
            {
                Type = new ThreePartType("dapr.io", "StateStore", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataDaprStateStore,
                Properties =
                {
                    new TypeProperty("kind", configKindType, TypePropertyFlags.Required),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeDaprPubSubTopic()
        {
            var configKindType = UnionType.Create(new StringLiteralType("pubsub.azure.servicebus"), new StringLiteralType("any"));

            return new ComponentData()
            {
                Type = new ThreePartType("dapr.io", "PubSubTopic", RadiusResources.CategoryComponent),
                Binding =  CommonBindings.BindingDataDaprPubSubTopic,
                Properties =
                {
                    new TypeProperty("kind", configKindType, TypePropertyFlags.Required),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                    new TypeProperty("topic", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeServiceBusQueue()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("azure.com", "ServiceBusQueue", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataServiceBusQueue,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                    new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeRedis()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("redislabs.com", "Redis", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataRedis,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeRabbitMQ()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("rabbitmq.com", "MessageQueue", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataRabbitMQ,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeCosmosDBMongo()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("azure.com", "CosmosDBMongo", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataMongo,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeCosmosDBSQL()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("azure.com", "CosmosDBSQL", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataSQL,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeKeyVault()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("azure.com", "KeyVault", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataKeyVault,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeMongoDB()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("mongodb.com", "Mongo", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataMongo,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }
    }
}
