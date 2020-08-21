// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.Semantics.Libraries
{
    public abstract class LibraryManager
    {
        public abstract TransformTemplate GetTransformTemplate(TransformSymbol symbol);
    }
}