// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Bicep.Core.Semantics;

namespace Bicep.Core.IR
{
    public class ModuleModel
    {
        public ModuleModel(SemanticModel semanticModel, ModuleSymbol symbol)
        {
            this.SemanticModel = semanticModel;
            this.Symbol = symbol;
        }

        public SemanticModel SemanticModel { get; }

        public ModuleSymbol Symbol { get; }
    }
}