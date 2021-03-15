// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Deployments.Expression.Expressions;

namespace Bicep.Core.IR
{
    public class ValueJTokenModel : ValueModel
    {
        public ValueJTokenModel(string value)
        {
            this.Value = new JTokenExpression(value);
        }

        public JTokenExpression Value { get; }
    }
}