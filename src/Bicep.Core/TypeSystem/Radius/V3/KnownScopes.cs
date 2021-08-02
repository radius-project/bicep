// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;

namespace Bicep.Core.TypeSystem.Radius.V3
{
    public static class KnownScopes
    {
        public class ScopeData
        {
            public ThreePartType Type { get; set; } = default!;

            public List<TypeProperty> Properties { get; } = new List<TypeProperty>();

            public List<ThreePartType> Routes { get; } = new List<ThreePartType>();
        }

        public static ScopeData MakeDaprScope()
        {
            var featuresType = new ObjectType(
                name: "features",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: LanguageConstants.Bool,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            return new ScopeData()
            {
                Type = new ThreePartType("dapr.io", "Dapr", RadiusResources.CategoryScope),
                Properties =
                {
                    new TypeProperty("features", featuresType, TypePropertyFlags.None),
                },
                Routes =
                {
                    new ThreePartType("dapr.io", "Invoke", RadiusResources.CategoryRoute),
                },
            };
        }

        public static ScopeData MakeNetworkScope()
        {
            return new ScopeData()
            {
                Type = new ThreePartType(null, "Network", RadiusResources.CategoryScope),
                Routes =
                {
                    new ThreePartType(null, "Http", RadiusResources.CategoryRoute),
                    new ThreePartType(null, "Grpc", RadiusResources.CategoryRoute),
                },
            };
        }
    }
}
