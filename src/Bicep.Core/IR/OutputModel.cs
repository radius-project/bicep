// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Bicep.Core.Semantics;

namespace Bicep.Core.IR
{
    public class OutputModel
    {
        public OutputModel(OutputSymbol symbol, ValueModel value)
        {
            this.Symbol = symbol;
            this.Value = value;
        }

        public string Name => Symbol.Name;

        public string Type => Symbol.Type.Name;

        public OutputSymbol Symbol { get; }

        public ValueModel Value { get; }
    }
}