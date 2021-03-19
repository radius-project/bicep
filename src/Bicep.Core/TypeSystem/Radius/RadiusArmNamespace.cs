// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.Semantics.Metadata;
using RadiusV3 = Bicep.Core.TypeSystem.Radius.V3;

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
            ExcludeFromCompletion: true);

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

        public static ResourceTypeReference? TryConvertRadiusType(ResourceMetadata resource)
        {
            if (resource.TypeReference.FormatType() == RadiusV3.RadiusResources.ApplicationResourceType)
            {
                return GetApplicationCRPType();
            }
            else if (resource.TypeReference.FormatType().StartsWith(RadiusV3.RadiusResources.ApplicationResourceType))
            {
                return GetResourceCRPType(resource.TypeReference);
            }

            return null;
        }

        public static ResourceTypeReference GetApplicationCRPType()
        {
            return ResourceTypeReference.Parse($"{RadiusV3.RadiusResources.ApplicationCRPType}@{RadiusV3.RadiusResources.CRPApiVersion}");
        }

        public static ResourceTypeReference GetResourceCRPType(ResourceTypeReference input)
        {
            return ResourceTypeReference.Parse($"{string.Format(RadiusV3.RadiusResources.ApplicationChildCRPTypeFormat, input.TypeSegments[input.TypeSegments.Length - 1])}@{RadiusV3.RadiusResources.CRPApiVersion}");
        }
    }
}
