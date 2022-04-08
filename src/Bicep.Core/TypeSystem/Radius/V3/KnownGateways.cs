// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;

namespace Bicep.Core.TypeSystem.Radius.V3
{
    public static class KnownGateways
    {

        public class GatewayData
        {
            public ThreePartType Type { get; set; } = default!;

            public List<TypeProperty> Properties { get; } = new List<TypeProperty>();
        }

        public static GatewayData MakeGateway() {
            var internalProperty = new TypeProperty("internal", LanguageConstants.Bool, TypePropertyFlags.None, "Set gateway to internal-only to use as a proxy. Defaults to false (expose to internet).");

            var hostnameType = new ObjectType(
                name: "hostname",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("prefix", LanguageConstants.String, TypePropertyFlags.None, "Specify a prefix for the hostname: myhostname.myapp.<PUBLIC HOSTNAME or IP>.nip.io"),
                    new TypeProperty("fullyQualifiedHostname", LanguageConstants.String, TypePropertyFlags.None, "Specify a fully-qualified domain name: myapp.mydomain.com. Mutually exclusive with 'prefix'.")
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null); ;

            var hostnameProperty = new TypeProperty("hostname", hostnameType, TypePropertyFlags.None, "Declare hostname information for the gateway. Leaving the hostname empty auto-assigns one: mygateway.myapp.<PUBLIC HOSTNAME or IP>.nip.io.");

            var routeType = new ObjectType(
                name: "routes",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new[]
                {
                    new TypeProperty("path", LanguageConstants.String, TypePropertyFlags.Required, "The path to the service, for example, /myservice."),
                    new TypeProperty("destination", LanguageConstants.String, TypePropertyFlags.Required, "The HttpRoute source, for example, myservice_route.id."),
                    new TypeProperty("replacePrefix", LanguageConstants.String, TypePropertyFlags.None, "Optionally update the prefix when sending the request to the service."),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var routesType = new ObjectType(name: "routes",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: routeType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var routesProperty = new TypeProperty("routes", routesType, TypePropertyFlags.Required, @"Specify the routes for the gateway.
            
Routes define the connections between services in the application.

Routes can only be publicly accessible when declared in this array.
            
```bicep
routes: [
  {
    path: '/servicea'
    destination: service_a_route.id
    replacePrefix: '/'
  }
]
```
");

            var listenerType = new ObjectType(
                name: $"listener",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new []
                {
                    new TypeProperty("port", LanguageConstants.Int, TypePropertyFlags.Required, "Specify listening ports for the gateway."),
                    new TypeProperty("protocol", LanguageConstants.String, TypePropertyFlags.Required, "Specifies the protocol to listen on."),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var listenersType = new ObjectType(name: "listeners",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: listenerType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var listenersProperty = new TypeProperty("listeners", listenersType, TypePropertyFlags.None, @"Specify the listeners for the gateway.
            
            Listeners define the endpoints that are bound to this Gateway's address.
            
            For example, the following code defines the listener to bind to port 80 and 443.
            
            ```bicep
            listeners: {
                http: {
                    port: 80
                    protocol: http
                }
                https: {
                    port: 443
                    protocol: https
                }
            }");

            return new GatewayData() {
                Type = new ThreePartType(null, "", RadiusResources.CategoryGateway),
                Properties = {
                    internalProperty,
                    hostnameProperty,
                    routesProperty
                }
            };
        }
    }
}