// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Bicep.Types.Concrete;
using Bicep.Types.Kubernetes.Index;

namespace Bicep.Types.Kubernetes
{
    public interface ITypeLoader
    {
        ResourceType LoadResourceType(TypeLocation location);

        TypeIndex GetIndexedTypes();
    }
}
