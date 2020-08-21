// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics.Libraries
{
    public class DefaultLibraryManager : LibraryManager
    {
        private readonly IResourceTypeProvider resourceTypeProvider;

        public DefaultLibraryManager(IResourceTypeProvider resourceTypeProvider)
        {
            this.resourceTypeProvider = resourceTypeProvider;
        }
    }
}