// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Bicep.Core.Semantics;

namespace Bicep.Core.IR
{
    public class VariableModel
    {
        public VariableModel(VariableSymbol symbol, ValueModel value)
        {
            this.Symbol = symbol;
            this.Value = value;
        }

        public string Name => Symbol.Name;

        public VariableSymbol Symbol { get; }

        public ValueModel Value { get; }
    }
}