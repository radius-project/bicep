// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Bicep.Core.Diagnostics;
using Bicep.Core.Syntax;
using Bicep.Core.Workspaces;

namespace Bicep.Core.Semantics
{
    public class ParamsSemanticModel
    {
        public BicepParamFile bicepParamFile { get; }

        public ParamBinder paramBinder {get; }

        // private readonly ParamBinder paramBinder; 
        public ParamsSemanticModel(BicepParamFile bicepParamFile)
        {
            this.bicepParamFile = bicepParamFile;
            this.paramBinder = new(bicepParamFile); 
        }

        public IEnumerable<IDiagnostic> GetDiagnostics()
        => bicepParamFile.ProgramSyntax.GetParseDiagnostics();
    }
}
