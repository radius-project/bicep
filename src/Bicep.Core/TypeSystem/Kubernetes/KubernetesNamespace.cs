// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Bicep.Core.Semantics;

namespace Bicep.Core.TypeSystem.Kubernetes
{
    public class KubernetesNamespace
    {
        public const string BuiltInName = "kubernetes";

        public static NamespaceSettings Settings { get; } = new(
            IsSingleton: true,
            BicepProviderName: BuiltInName,
            ConfigurationType: null,
            ArmTemplateProviderName: "Kubernetes",
            ArmTemplateProviderVersion: "0.1");

        public static NamespaceType Create(string aliasName)
        {
            return new NamespaceType(
                aliasName: aliasName,
                settings: Settings,
                properties: Array.Empty<TypeProperty>(),
                functionOverloads: Array.Empty<FunctionOverload>(),
                bannedFunctions: Array.Empty<BannedFunction>(),
                decorators: Array.Empty<Decorator>(),
                new KubernetesTypeProvider());
        }
    }
}
