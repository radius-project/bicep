// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Bicep.Core.Diagnostics;

namespace Bicep.Core.Semantics
{
    public partial class SemanticModel
    {
        public (ImmutableArray<TransformedResource> resources, ImmutableArray<Diagnostic> diagnostics) EvaluateTransforms()
        {
            foreach (var transform in this.Root.TransformDeclarations)
            {
                var template = this.LibraryManager.GetTransformTemplate(transform);
                if (template is null)
                {
                    // TODO: diagnostic
                }
                
                r
            }
        }
    }
}