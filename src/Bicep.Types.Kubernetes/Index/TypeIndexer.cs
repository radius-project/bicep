// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;

namespace Bicep.Types.Kubernetes.Index
{
    public class TypeIndex
    {
        public TypeIndex(IReadOnlyDictionary<string, TypeLocation> types)
        {
            Types = types;
        }

        public IReadOnlyDictionary<string, TypeLocation> Types { get; }
    }
}
