// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;
using Bicep.Core.TypeSystem;
using Bicep.Core.TypeSystem.Radius;

namespace Bicep.Core.Semantics.Namespaces
{
    public static class RadiusNamespaceType
    {
        public const string BuiltInName = "radius";

        private static readonly IResourceTypeProvider TypeProvider = new RadiusResourceTypeProvider(new RadiusResourceTypeLoader());

        public static NamespaceSettings Settings { get; } = new(
            IsSingleton: false,
            BicepProviderName: BuiltInName,
            ConfigurationType: GetConfigurationType(),
            ArmTemplateProviderName: "Radius",
            ArmTemplateProviderVersion: "1.0");

        private static ObjectType GetConfigurationType()
        {
            return new ObjectType("configuration", TypeSymbolValidationFlags.Default, new TypeProperty[]
            {
            }, null);
        }

        public static NamespaceType Create(string aliasName)
        {
            return new NamespaceType(
                aliasName,
                Settings,
                ImmutableArray<TypeProperty>.Empty,
                ImmutableArray<FunctionOverload>.Empty,
                ImmutableArray<BannedFunction>.Empty,
                ImmutableArray<Decorator>.Empty,
                TypeProvider);
        }
    }
}
