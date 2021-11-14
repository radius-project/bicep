// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.


using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Bicep.Core.TypeSystem.Radius.V3
{
    public static class CommonBindings
    {

        private static ObjectType PathProperty = new ObjectType(
            "path",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new[]
            {
                new TypeProperty("value", LanguageConstants.String, TypePropertyFlags.Required, description: "Specifies the path to match the incoming request."),
                new TypeProperty(
                    "type",
                    new UnionType("type", ImmutableArray.Create<ITypeReference>(new StringLiteralType("prefix"), new StringLiteralType("exact"))),
                    TypePropertyFlags.None, description: "Specifies the type of matching to match the path on. Supported values: 'prefix', 'exact'"),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        private static ObjectType RuleProperty = new ObjectType(
            "rule",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new[]
            {
                new TypeProperty("method", LanguageConstants.String, TypePropertyFlags.None, description: "Specifies the method to match on the incoming request."),
                // path needs to be an object
                new TypeProperty("path", PathProperty, TypePropertyFlags.None, description: "Specifies the path to match on the incoming request."),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        private static ObjectType RulesType = new ObjectType(
                name: "rules",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: RuleProperty,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
        private static TypeProperty GatewayProperty = new TypeProperty("gateway", new ObjectType(
            "gateway",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new[]
            {
                new TypeProperty("hostname", LanguageConstants.String, TypePropertyFlags.None, description: "Specifies the hostname to match. Use '*' to match all hostnames."),
                new TypeProperty("source", LanguageConstants.String, TypePropertyFlags.None, "The gateway which this HttpRoute belongs to."),
                new TypeProperty("rules", RulesType, TypePropertyFlags.None, description: "Specifies the path to match on the incoming request.")
            },
            additionalPropertiesType: RulesType,
            additionalPropertiesFlags: TypePropertyFlags.None),
            TypePropertyFlags.None,
            description: "Specifies a gateway for public access to the route from outside the network.");

        public static readonly BindingData BindingDataHttp = new BindingData()
        {
            Type = new ThreePartType(null, "Http", RadiusResources.CategoryRoute),
            Properties =
            {
                new TypeProperty("url", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("scheme", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("host", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("port", LanguageConstants.Int, TypePropertyFlags.None),
                GatewayProperty,
            },
            Values =
            {
                new BindingValue("url"),
                new BindingValue("scheme"),
                new BindingValue("host"),
                new BindingValue("port"),
            },
        };

        public static readonly BindingData BindingDataGrpc = new BindingData()
        {
            Type = new ThreePartType(null, "Grpc", RadiusResources.CategoryRoute),
            Properties =
            {
                new TypeProperty("url", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("scheme", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("host", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("port", LanguageConstants.Int, TypePropertyFlags.None),
                GatewayProperty,
            },
            Values =
            {
                new BindingValue("url"),
                new BindingValue("scheme"),
                new BindingValue("host"),
                new BindingValue("port"),
            },
        };

        public static readonly BindingData BindingDataDaprHttp = new BindingData()
        {
            Type = new ThreePartType("dapr.io", "DaprHttp", RadiusResources.CategoryRoute),
            Properties =
            {
                new TypeProperty(
                    "appId",
                     LanguageConstants.String,
                     TypePropertyFlags.None,
                     description: "The Dapr appId to use for the route. Will default to the route name if not specified. If the component providing this route also specifies an appId then the values must match."),
            },
            Values =
            {
                new BindingValue("appId"),
            },
        };

        public static readonly BindingData BindingDataDaprPubSubTopic = new BindingData()
        {
            Type = new ThreePartType("dapr.io", "PubSubTopic", RadiusResources.CategoryBinding),
            Properties =
            {
                new TypeProperty("pubSubName", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("topic", LanguageConstants.String, TypePropertyFlags.ReadOnly),
            },
            Values =
            {
                new BindingValue("pubSubName"),
                new BindingValue("topic"),
            },
        };

        public static readonly BindingData BindingDataDaprStateStore = new BindingData()
        {
            Type = new ThreePartType("dapr.io", "StateStore", RadiusResources.CategoryBinding),
            Properties =
            {
                new TypeProperty("stateStoreName", LanguageConstants.String, TypePropertyFlags.ReadOnly),
            },
            Values =
            {
                new BindingValue("stateStoreName"),
            },
        };

        public static readonly BindingData BindingDataRedis = new BindingData()
        {
            Type = new ThreePartType("redislabs.com", "Redis", RadiusResources.CategoryBinding),
            Properties =
            {
                new TypeProperty("host", LanguageConstants.String, TypePropertyFlags.None),
                new TypeProperty("port", LanguageConstants.Int, TypePropertyFlags.None),
                new TypeProperty("username", LanguageConstants.String, TypePropertyFlags.None),
                // The secrets section allows usage of binding expression to specify
                // custom secrets.
                //
                // TODO: It is slightly confusing to set `redis.secrets.connectionString`, and read
                //       from `redis.connectionString`. However, it is backward compatible with the
                //       existing model. This will be a point we need to discuss when
                //       check-pointing the end-to-end for RedisComponent.
                new TypeProperty(
                    "secrets",
                    new ObjectType(
                        "secrets",
                        validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                        properties: new []
                        {
                            // TODO: it is unclear that we will need both `connectionString` and `password`.
                            //       For now we will go ahead having both, but it is good to raise this point
                            //       during our discussion.
                            new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.WriteOnly),
                            new TypeProperty("password", LanguageConstants.String, TypePropertyFlags.WriteOnly),
                        },
                        additionalPropertiesType: null,
                        additionalPropertiesFlags: TypePropertyFlags.None),
                TypePropertyFlags.None),
            },
            Values =
            {
                new BindingValue("host"),
                new BindingValue("port"),
                new BindingValue("username"),
                new BindingValue("connectionString", secret: true),
                new BindingValue("password", secret: true),
            },
        };

        public static readonly BindingData BindingDataRabbitMQ = new BindingData()
        {
            Type = new ThreePartType("rabbitmq.com", "MessageQueue", RadiusResources.CategoryBinding),
            Values =
            {
                new BindingValue("queue"),
                new BindingValue("connectionString", secret: true),
            },
        };

        public static readonly BindingData BindingDataMongo = new BindingData()
        {
            Type = new ThreePartType("mongo.com", "MongoDB", RadiusResources.CategoryBinding),
            Properties =
            {
                new TypeProperty(
                    "secrets",
                    new ObjectType(
                        "secrets",
                        validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                        properties: new []
                        {
                            new TypeProperty("connectionString", LanguageConstants.String, TypePropertyFlags.WriteOnly),
                        },
                        additionalPropertiesType: null,
                        additionalPropertiesFlags: TypePropertyFlags.None),
                TypePropertyFlags.None),
            },
            Values =
            {
                new BindingValue("connectionString", secret: true),
            },
        };

        public static readonly BindingData BindingDataSQL = new BindingData()
        {
            Type = new ThreePartType("microsoft.com", "SQL", RadiusResources.CategoryBinding),
            Properties =
            {
                new TypeProperty("database", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("server", LanguageConstants.String, TypePropertyFlags.ReadOnly),
            },
            Values =
            {
                new BindingValue("database"),
                new BindingValue("server"),
            },
        };

        public static readonly BindingData BindingDataKeyVault = new BindingData()
        {
            Type = new ThreePartType("azure.com", "KeyVault", RadiusResources.CategoryBinding),
            Properties =
            {
                new TypeProperty("uri", LanguageConstants.String, TypePropertyFlags.ReadOnly),
            },
            Values =
            {
                new BindingValue("uri"),
            },
        };

        public static readonly BindingData BindingDataServiceBusQueue = new BindingData()
        {
            Type = new ThreePartType("azure.com", "ServiceBusQueue", RadiusResources.CategoryBinding),
            Properties =
            {
                new TypeProperty("namespace", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("namespaceConnectionString", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                new TypeProperty("queueConnectionString", LanguageConstants.String, TypePropertyFlags.ReadOnly),
            },
            Values =
            {
                new BindingValue("namespaceConnectionString"),
                new BindingValue("queueConnectionString"),
                new BindingValue("namespace"),
                new BindingValue("queue"),
            },
        };

        public static readonly BindingData[] AllBindingData = new BindingData[]
        {
            BindingDataDaprHttp,
            BindingDataDaprPubSubTopic,
            BindingDataDaprStateStore,
            // Intentionally Hidden: BindingDataGrpc,
            BindingDataHttp,
            BindingDataKeyVault,
            BindingDataMongo,
            BindingDataRedis,
            BindingDataRabbitMQ,
            BindingDataServiceBusQueue,
            BindingDataSQL,
        };

        public static readonly IEnumerable<BindingData> RouteBindingData = AllBindingData.Where(b => b.IsRoute);

        private static readonly IEnumerable<BindingData> NonRouteBindingData = AllBindingData.Where(b => !b.IsRoute);

        public class BindingData
        {
            public bool IsRoute => Type.Category == RadiusResources.CategoryRoute;

            public ThreePartType Type { get; set; } = default!;

            public List<TypeProperty> Properties { get; } = new List<TypeProperty>();

            public List<BindingValue> Values { get; } = new List<BindingValue>();
        }

        public class BindingValue
        {
            public BindingValue(string name, ITypeReference? type = null, bool secret = false)
            {
                this.Name = name;
                this.Secret = secret;
                this.Type = type ?? LanguageConstants.String;
            }

            public string Name { get; }

            public bool Secret { get; }

            public ITypeReference Type { get; }
        }
    }
}
