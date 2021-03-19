// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.TypeSystem.Radius
{
    public static class RadiusResources
    {
        public static readonly string CRPApiVersion = "2018-09-01-preview";

        public static readonly string ProviderCRPType = "Microsoft.CustomProviders/resourceProviders";

        public static readonly string ApplicationCRPType = "Microsoft.CustomProviders/resourceProviders/Applications";

        public static readonly string ComponentCRPType = "Microsoft.CustomProviders/resourceProviders/Applications/Components";

        public static readonly string DeploymentCRPType = "Microsoft.CustomProviders/resourceProviders/Applications/Deployments";

        public static readonly string ResourceApiVersion = "v1alpha1";

        public const string ApplicationResourceType = "radius.dev/Applications";

        public const string ComponentResourceType = "radius.dev/Applications/Components";

        public const string DeploymentResourceType = "radius.dev/Applications/Deployments";
    }
}
