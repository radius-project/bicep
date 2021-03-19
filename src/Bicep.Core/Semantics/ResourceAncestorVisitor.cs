// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Bicep.Core.Semantics.Metadata;
using Bicep.Core.Syntax;
using static Bicep.Core.Semantics.ResourceAncestorGraph;

namespace Bicep.Core.Semantics
{
    public sealed class ResourceAncestorVisitor : SyntaxVisitor
    {
        private readonly SemanticModel semanticModel;
        private readonly ImmutableDictionary<ResourceSymbol, ResourceAncestor>.Builder ancestry;

        public ResourceAncestorVisitor(SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
            this.ancestry = ImmutableDictionary.CreateBuilder<ResourceSymbol, ResourceAncestor>();
        }

        public ImmutableDictionary<ResourceSymbol, ResourceAncestor> Ancestry
            => this.ancestry.ToImmutableDictionary();

        public override void VisitResourceDeclarationSyntax(ResourceDeclarationSyntax syntax)
        {
            // Skip analysis for ErrorSymbol and similar cases, these are invalid cases, and won't be emitted.
            if (semanticModel.GetSymbolInfo(syntax) is not ResourceSymbol resourceSymbol)
            {
                base.VisitResourceDeclarationSyntax(syntax);
                return;
            }

            if (semanticModel.Binder.GetNearestAncestor<ResourceDeclarationSyntax>(syntax) is {} nestedParentSyntax)
            {
                // nested resource parent syntax
                if (semanticModel.GetSymbolInfo(nestedParentSyntax) is ResourceSymbol parentResource)
                {
                    this.ancestry.Add(resourceSymbol, new ResourceAncestor(ResourceAncestorType.Nested, parentResource, null));
                }
            }
            else if (resourceSymbol.SafeGetBodyPropertyValue(LanguageConstants.ResourceParentPropertyName) is {} referenceParentSyntax)
            {
                SyntaxBase? indexExpression = null;
                if (referenceParentSyntax is ArrayAccessSyntax arrayAccess)
                {
                    referenceParentSyntax = arrayAccess.BaseExpression;
                    indexExpression = arrayAccess.IndexExpression;
                }

                // parent property reference syntax
                if (semanticModel.GetSymbolInfo(referenceParentSyntax) is ResourceSymbol parentResource)
                {
                    this.ancestry.Add(resourceSymbol, new ResourceAncestor(ResourceAncestorType.ParentProperty, parentResource, indexExpression));
                }
            }

            base.VisitResourceDeclarationSyntax(syntax);
            return;
        }
    }
}
