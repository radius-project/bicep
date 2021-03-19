// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.TypeSystem
{
    public class ExpressionType : TypeSymbol
    {
        public ExpressionType(string name, ITypeReference returnType)
            : base(name)
        {
            this.ReturnType = returnType;
        }

        public ITypeReference ReturnType { get; }

        public override TypeKind TypeKind => throw new System.NotImplementedException();
    }
}
