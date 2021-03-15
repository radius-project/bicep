// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Bicep.Core.Syntax;

namespace Bicep.Core.IR
{
    public class PropertyModel
    {
        public PropertyModel(string name, ValueModel value)
        {
            this.Name = new ValueJTokenModel(name);
            this.Value = value;
        }

        public PropertyModel(SyntaxBase name, ValueModel value)
        {
            this.Name = new ValueSyntaxModel(name);
            this.Value = value;
        }

        public ValueModel Name { get; }

        public ValueModel Value { get; }
    }
}