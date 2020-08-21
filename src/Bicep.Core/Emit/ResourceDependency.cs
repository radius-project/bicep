// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;

namespace Bicep.Core.Emit
{
    public readonly struct ResourceDependency
    {
        public readonly ResourceReference? Reference;
        public readonly DeclaredSymbol? Symbol;

        public ResourceDependency(ResourceReference reference)
        {
            Reference = reference;
            Symbol = null;
        }

        public ResourceDependency(DeclaredSymbol symbol)
        {
            Symbol = symbol;
            Reference = null;
        }
    }
}