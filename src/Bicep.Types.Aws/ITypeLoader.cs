// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Azure.Bicep.Types.Aws.Index;
using Azure.Bicep.Types.Concrete;

namespace Azure.Bicep.Types.Aws
{
    public interface ITypeLoader
    {
        ResourceType LoadResourceType(TypeLocation location);

        TypeIndex GetIndexedTypes();
    }
}
