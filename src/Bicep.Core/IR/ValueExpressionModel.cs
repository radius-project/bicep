// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Deployments.Expression.Expressions;
using Bicep.Core.Syntax;

namespace Bicep.Core.IR
{
    public class ValueExpressionModel : ValueModel
    {
        public ValueExpressionModel(LanguageExpression value)
        {
            this.Value = value;

        }
        
        public LanguageExpression Value { get; }
    }
}