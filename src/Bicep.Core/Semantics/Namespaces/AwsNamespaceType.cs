// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;
using Bicep.Core.TypeSystem;
using Bicep.Core.TypeSystem.Aws;

namespace Bicep.Core.Semantics.Namespaces
{
    public static class AwsNamespaceType
    {
        public const string BuiltInName = "aws";

        private static readonly IResourceTypeProvider TypeProvider = new AwsResourceTypeProvider(new AwsResourceTypeLoader());

        public static NamespaceSettings Settings { get; } = new(
            IsSingleton: false,
            BicepProviderName: BuiltInName,
            ConfigurationType: GetConfigurationType(),
            ArmTemplateProviderName: "AWS",
            ArmTemplateProviderVersion: "0.1");

        private static ObjectType GetConfigurationType()
        {
            return new ObjectType("configuration", TypeSymbolValidationFlags.Default, new TypeProperty[]
            {
                new TypeProperty("account", LanguageConstants.String),
                new TypeProperty("region", LanguageConstants.String),
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
