// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;

namespace Azure.Bicep.Types.Radius.Index
{
    public class TypeIndex
    {
        public TypeIndex(IReadOnlyDictionary<string, TypeLocation> resources)
        {
            Resources = resources;
        }

        public IReadOnlyDictionary<string, TypeLocation> Resources { get; }
    }
}
