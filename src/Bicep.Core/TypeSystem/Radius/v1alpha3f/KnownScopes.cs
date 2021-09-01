// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;

namespace Bicep.Core.TypeSystem.Radiusv1alpha3f
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
                Type = new ThreePartType("dapr.io", "Dapr"),
                Properties =
                {
                    new TypeProperty("features", featuresType, TypePropertyFlags.None),
                },
                Routes =
                {
                    new ThreePartType("dapr.io", "Invoke"),
                },
            };
        }

        public static ScopeData MakeNetworkScope()
        {
            return new ScopeData()
            {
                Type = new ThreePartType(null, "Network"),
                Routes =
                {
                    new ThreePartType(null, "Http"),
                    new ThreePartType(null, "Grpc"),
                },
            };
        }
    }
}
