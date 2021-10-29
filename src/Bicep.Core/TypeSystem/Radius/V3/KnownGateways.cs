// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Linq;

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
            var members = new List<ObjectType>();

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
                    listenersProperty
                }
            };
        }
    }
}