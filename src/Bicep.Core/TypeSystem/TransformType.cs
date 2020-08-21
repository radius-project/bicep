// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.TypeSystem
{
    public class TransformType : TypeSymbol
    {
        public TransformType(string name, ITypeReference inputType, ITypeReference outputType) 
            : base(name)
        {
            InputType = inputType;
            OutputType = outputType;
        }
        
        public ITypeReference InputType { get;  }
        
        public ITypeReference OutputType { get; }

        public override TypeKind TypeKind { get; }
    }
}