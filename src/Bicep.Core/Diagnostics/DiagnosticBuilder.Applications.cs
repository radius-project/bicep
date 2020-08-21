// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.TypeSystem.Applications;

namespace Bicep.Core.Diagnostics
{
    public static partial class DiagnosticBuilder
    {
        public partial class DiagnosticBuilderInternal
        {
            public Diagnostic ComponentMissingApplicationProperty(string name) => new Diagnostic(
                TextSpan,
                DiagnosticLevel.Warning,
                "BCP996",
                $"Component \"{name}\" must have an \"application\" property.");

            public Diagnostic DeploymentMissingApplicationProperty(string name) => new Diagnostic(
                TextSpan,
                DiagnosticLevel.Warning,
                "BCP997",
                $"Deployment \"{name}\" must have an \"application\" property.");

            public Diagnostic InstanceMissingApplicationProperty(string name) => new Diagnostic(
                TextSpan,
                DiagnosticLevel.Warning,
                "BCP998",
                $"Instance \"{name}\" must have an \"application\" property.");

            public Diagnostic ResourceTypesUnavailable(ComponentTypeReference componentTypeReference) => new Diagnostic(
                TextSpan,
                DiagnosticLevel.Warning,
                "BCP999",
                $"Component type \"{componentTypeReference.FormatName()}\" does not have types available.");
        }
    }
}