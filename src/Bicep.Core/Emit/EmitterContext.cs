// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.DataFlow;
using Bicep.Core.Semantics;

namespace Bicep.Core.Emit
{
    public class EmitterContext
    {
        private ImmutableDictionary<ResourceSymbol, ResourceItem> itemsBySymbol;

        public EmitterContext(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;
            this.DataFlowAnalyzer = new(semanticModel);
            this.VariablesToInline = InlineDependencyVisitor.GetVariablesToInline(semanticModel);
            this.ResourceDependencies = ResourceDependencyVisitor.GetResourceDependencies(semanticModel);

            this.ResourceItems = ResourceRewriter.Transform(SemanticModel, ResourceDependencies);

            this.itemsBySymbol = this.ResourceItems.Where(item => item.Symbol != null).ToImmutableDictionary(item => item.Symbol!);
        }

        public SemanticModel SemanticModel { get; }

        public DataFlowAnalyzer DataFlowAnalyzer { get; }

        public ImmutableHashSet<VariableSymbol> VariablesToInline { get; }

        public ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<ResourceDependency>> ResourceDependencies { get; }

        public ImmutableArray<ResourceItem> ResourceItems { get; }

        public ImmutableDictionary<ModuleSymbol, ScopeHelper.ScopeData> ModuleScopeData => SemanticModel.EmitLimitationInfo.ModuleScopeData;

        public ImmutableDictionary<ResourceSymbol, ScopeHelper.ScopeData> ResourceScopeData => SemanticModel.EmitLimitationInfo.ResourceScopeData;

        public ResourceItem GetResourceItem(ResourceSymbol resource) => this.itemsBySymbol[resource];
    }
}
