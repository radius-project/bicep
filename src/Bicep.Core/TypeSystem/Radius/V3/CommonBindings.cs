// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.


using System.Collections.Generic;
using System.Linq;

namespace Bicep.Core.TypeSystem.Radius.V3
{
    public static class CommonBindings
    {
        private static TypeProperty GatewayProperty = new TypeProperty("gateway", new ObjectType(
            "gateway",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new[]
            {
                new TypeProperty("hostname", LanguageConstants.String, TypePropertyFlags.Required, description: "Specifies the hostname to match. Use '*' to match all hostnames."),
            },
            additionalPropertiesType: null,
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

        public static readonly BindingData BindingDataDaprInvoke = new BindingData()
        {
            Type = new ThreePartType("dapr.io", "Invoke", RadiusResources.CategoryRoute),
            Properties =
            {
                new TypeProperty("appId", LanguageConstants.String, TypePropertyFlags.ReadOnly),
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
            Values =
            {
                new BindingValue("connectionString", secret: true),
                new BindingValue("primaryKey", secret: true),
                new BindingValue("secondaryKey", secret: true),
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
            Values =
            {
                new BindingValue("connectionString", secret: true),
            },
        };

        public static readonly BindingData BindingDataSQL= new BindingData()
        {
            Type = new ThreePartType("microsoft.com", "SQL", RadiusResources.CategoryBinding),
            Values =
            {
                new BindingValue("connectionString", secret: true),
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
            },
            Values =
            {
                new BindingValue("connectionString", secret: true),
                new BindingValue("namespace"),
                new BindingValue("queue"),
            },
        };

        public static readonly BindingData[] AllBindingData = new BindingData[]
        {
            BindingDataDaprInvoke,
            BindingDataDaprPubSubTopic,
            BindingDataDaprStateStore,
            BindingDataGrpc,
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
