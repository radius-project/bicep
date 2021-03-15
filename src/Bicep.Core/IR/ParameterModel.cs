// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;
using Bicep.Core.Semantics;

namespace Bicep.Core.IR
{
    public class ParameterModel
    {
        public ParameterModel(ParameterSymbol symbol, ImmutableArray<PropertyModel> properties)
        {
            this.Symbol = symbol;
            this.Properties = properties;
        }

        public string Name => this.Symbol.Name;

        public ImmutableArray<PropertyModel> Properties { get; }

        public ParameterSymbol Symbol { get; }
    }
}