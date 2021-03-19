// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Bicep.Core.Semantics;

namespace Bicep.Core.TypeSystem.Radius
{
    public class RadiusArmNamespace
    {
        public const string BuiltInName = "Radius";

        public static NamespaceSettings Settings { get; } = new(
            IsSingleton: true,
            BicepProviderName: BuiltInName,
            ConfigurationType: null,
            ArmTemplateProviderName: "AzureResourceManager",
            ArmTemplateProviderVersion: "1.0",
            Transformer: RadiusTypeProvider.CreateMetadata);

        public static NamespaceType Create(string aliasName)
        {
            return new NamespaceType(
                aliasName: aliasName,
                settings: Settings,
                properties: Array.Empty<TypeProperty>(),
                functionOverloads: Array.Empty<FunctionOverload>(),
                bannedFunctions: Array.Empty<BannedFunction>(),
                decorators: Array.Empty<Decorator>(),
                new RadiusTypeProvider());
        }
    }
}
