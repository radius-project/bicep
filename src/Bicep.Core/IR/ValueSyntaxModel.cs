// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Syntax;

namespace Bicep.Core.IR
{
    public class ValueSyntaxModel : ValueModel
    {
        public ValueSyntaxModel(SyntaxBase value)
        {
            this.Value = value;

        }
        
        public SyntaxBase Value { get; }
    }
}