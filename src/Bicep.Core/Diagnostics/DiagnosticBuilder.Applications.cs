// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.TypeSystem.Applications;

namespace Bicep.Core.Diagnostics
{
    public static partial class DiagnosticBuilder
    {
        public partial class DiagnosticBuilderInternal
        {
            public ErrorDiagnostic ImportDirectiveMissingName() => new ErrorDiagnostic(
                TextSpan,
                "BCP998",
                "Expected an import identifier at this location.");

            public ErrorDiagnostic ExpectedIdentifier() => new ErrorDiagnostic(
                TextSpan,
                "BCP999",
                "Expected an identifier or keyword at this location.");
        }
    }
}