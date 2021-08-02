// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bicep.Core.TypeSystem.Radiusv1alpha3u
{
    public static class CommonBindings
    {
        public static readonly BindingData BindingDataHttp = new BindingData()
        {
            Kind = "http",
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
            Kind = "grpc",
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
            Kind = "dapr.io/Invoke",
            IsRoute = true,
            Values =
            {
                "appId",
            },
        };

        public static readonly BindingData BindingDataDaprPubSubTopic = new BindingData()
        {
            Kind = "dapr.io/PubSubTopic",
            Values =
            {
                "pubSubName",
                "topic",
            },
        };

        public static readonly BindingData BindingDataDaprStateStore = new BindingData()
        {
            Kind = "dapr.io/StateStore",
            Values =
            {
                "stateStoreName",
            },
        };

        public static readonly BindingData BindingDataRedis = new BindingData()
        {
            Kind = "redislabs.com/Redis",
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataMongo = new BindingData()
        {
            Kind = "mongo.com/MongoDB",
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataCosmosMongo = new BindingData()
        {
            Kind = "azure.com/CosmosDBMongo",
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataCosmosSQL= new BindingData()
        {
            Kind = "azure.com/CosmosDBSQL",
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataSQL= new BindingData()
        {
            Kind = "microsoft.com/SQL",
            Values =
            {
                "connectionString",
            },
        };

        public static readonly BindingData BindingDataKeyVault = new BindingData()
        {
            Kind = "azure.com/KeyVault",
            Values =
            {
                "uri",
            },
        };

        public static readonly BindingData BindingDataServiceBusQueue = new BindingData()
        {
            Kind = "azure.com/ServiceBusQueue",
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

            public string Kind { get; set; } = default!;

            public List<TypeProperty> Properties { get; } = new List<TypeProperty>();

            public List<string> Values { get; } = new List<string>();
        }

        public static readonly DiscriminatedObjectType BindingResourceBodyType = new DiscriminatedObjectType(
            "binding",
            TypeSymbolValidationFlags.Default,
            "kind",
            RouteBindingData.Select(b => MakeV3BindingBodyType(b)));

        private static ObjectType MakeV3BindingBodyType(BindingData data)
        {
            var propertiesType = new ObjectType(
                name: $"binding properties: {data.Kind}",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: data.Properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            return new ObjectType(
                $"binding: {data.Kind}",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new []
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    new TypeProperty("kind", new StringLiteralType(data.Kind), TypePropertyFlags.Required),
                    new TypeProperty("properties", propertiesType, propertiesType.Properties.Any(p => p.Value.Flags.HasFlag(TypePropertyFlags.Required)) ? TypePropertyFlags.Required : TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static TypeProperty MakeBindingsProperty(Dictionary<string, BindingData>? builtIn)
        {
            var properties = builtIn?.Select(kvp =>
            {
                var bindingType = new ObjectType(
                    name: $"binding properties: {kvp.Value.Kind}",
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
