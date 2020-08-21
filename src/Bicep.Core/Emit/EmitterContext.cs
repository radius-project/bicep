// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;
using Bicep.Core.Semantics;

namespace Bicep.Core.Emit
{
    public class EmitterContext
    {
        public EmitterContext(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;
            this.VariablesToInline = InlineDependencyVisitor.GetVariablesToInline(semanticModel);
            this.ResourceDependencies = ResourceDependencyVisitor.GetResourceDependencies(semanticModel);
            this.ProjectedResources = semanticModel.GetProjectedResources();
        }

        public SemanticModel SemanticModel { get; }

        public ImmutableHashSet<VariableSymbol> VariablesToInline { get; }

        public ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<DeclaredSymbol>> ResourceDependencies { get; }

        public ImmutableDictionary<ModuleSymbol, ScopeHelper.ScopeData> ModuleScopeData => SemanticModel.EmitLimitationInfo.ModuleScopeData;

        public ImmutableDictionary<ResourceSymbol, ResourceSymbol?> ResoureScopeData => SemanticModel.EmitLimitationInfo.ResoureScopeData;
        public ImmutableArray<ProjectedResource> ProjectedResources { get; }
    }
}
