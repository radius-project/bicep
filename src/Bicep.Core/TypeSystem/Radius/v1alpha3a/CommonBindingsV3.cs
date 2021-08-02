// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bicep.Core.TypeSystem.Radiusv1alpha3a
{
    public static class CommonBindings
    {
        public static readonly BindingData BindingDataHttp = new BindingData()
        {
            Type = new ThreePartType(null, "Http"),
            IsRoute = true,
            Properties =
            {
                new TypeProperty("port", LanguageConstants.Int, TypePropertyFlags.WriteOnly),
            },
            Values =
            {
                "url",
                "scheme",
                "host",
                "port"
            },
        };

        public static readonly BindingData BindingDataGrpc = new BindingData()
        {
            Type = new ThreePartType(null, "Grpc"),
            IsRoute = true,
            Properties =
            {
                new TypeProperty("port", LanguageConstants.Int, TypePropertyFlags.WriteOnly),
            },
            Values =
            {
                "url",
                "scheme",
                "host",
                "port"
            },
        };

        public static readonly BindingData BindingDataDaprInvoke = new BindingData()
        {
            Type = new ThreePartType("dapr.io", "Invoke"),
            IsRoute = true,
            Values =
            {
                "appId",
            },
        };

        public static readonly BindingData BindingDataDaprPubSubTopic = new BindingData()
        {
            Type = new ThreePartType("dapr.io", "PubSubTopic"),
            Values =
            {
                "pubSubName",
                "topic",
            },
        };

        public static readonly BindingData BindingDataDaprStateStore = new BindingData()
        {
            Type = new ThreePartType("dapr.io", "StateStore"),
            Values =
            {
                "stateStoreName",
            },
        };

        public static readonly BindingData BindingDataRedis = new BindingData()
        {
            Type = new ThreePartType("redislabs.com", "Redis"),
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataMongo = new BindingData()
        {
            Type = new ThreePartType("mongo.com", "MongoDB"),
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataCosmosMongo = new BindingData()
        {
            Type = new ThreePartType("azure.com", "CosmosDBMongo"),
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataCosmosSQL= new BindingData()
        {
            Type = new ThreePartType("azure.com", "CosmosDBSQL"),
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataSQL= new BindingData()
        {
            Type = new ThreePartType("microsoft.com", "SQL"),
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataKeyVault = new BindingData()
        {
            Type = new ThreePartType("azure.com", "KeyVault"),
            Values =
            {
                "uri",
            },
        };

        public static readonly BindingData BindingDataServiceBusQueue = new BindingData()
        {
            Type = new ThreePartType("azure.com", "ServiceBusQueue"),
            Values =
            {
                "connectionString",
                "namespace",
                "queue",
            },
        };

        public static readonly BindingData[] AllBindingData = new BindingData[]
        {
            BindingDataCosmosMongo,
            BindingDataCosmosSQL,
            BindingDataDaprInvoke,
            BindingDataDaprPubSubTopic,
            BindingDataDaprStateStore,
            BindingDataGrpc,
            BindingDataHttp,
            BindingDataKeyVault,
            BindingDataMongo,
            BindingDataRedis,
            BindingDataServiceBusQueue,
            BindingDataSQL,
        };

        public static readonly IEnumerable<BindingData> RouteBindingData = AllBindingData.Where(b => b.IsRoute);

        private static readonly IEnumerable<BindingData> NonRouteBindingData = AllBindingData.Where(b => !b.IsRoute);

        public class BindingData
        {
            public bool IsRoute { get; set; }

            public ThreePartType Type { get; set; } = default!;

            public List<TypeProperty> Properties { get; } = new List<TypeProperty>();

            public List<string> Values { get; } = new List<string>();
        }

        public static TypeProperty MakeBindingsProperty(Dictionary<string, BindingData>? builtIn)
        {
            var properties = builtIn?.Select(kvp =>
            {
                var bindingType = new ObjectType(
                    name: $"binding properties: {kvp.Value.Type.FormatKind()}",
                    validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                    properties: new []
                    {
                        new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                    },
                    additionalPropertiesType: null,
                    additionalPropertiesFlags: TypePropertyFlags.None,
                    functions: null);

                return new TypeProperty(kvp.Key, bindingType, TypePropertyFlags.None);
            }).ToArray() ?? Array.Empty<TypeProperty>();

            var bindingsType = new ObjectType(
                "bindings",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            return new TypeProperty("bindings", bindingsType, TypePropertyFlags.ReadOnly);
        }
    }
}
