// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics;
using Bicep.Core.Diagnostics;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;

namespace Bicep.Core.Emit
{
    public sealed class ForSyntaxValidatorVisitor : SyntaxVisitor
    {
        private readonly IDiagnosticWriter diagnosticWriter;

        private readonly SemanticModel semanticModel;

        private readonly Stack<LoopValidationItem> loopParents;

        private ForSyntaxValidatorVisitor(SemanticModel semanticModel, IDiagnosticWriter diagnosticWriter)
        {
            this.semanticModel = semanticModel;
            this.diagnosticWriter = diagnosticWriter;

            this.loopParents = new Stack<LoopValidationItem>();
        }

        public static void Validate(SemanticModel semanticModel, IDiagnosticWriter diagnosticWriter)
        {
            var visitor = new ForSyntaxValidatorVisitor(semanticModel, diagnosticWriter);
            
            // visiting writes diagnostics in some cases
            visitor.Visit(semanticModel.SyntaxTree.ProgramSyntax);
        }

        public override void VisitForSyntax(ForSyntax syntax)
        {
            var item = CreateValidationItem(syntax);

            if (!item.IsValidParent)
            {
                // this loop was used incorrectly
                this.diagnosticWriter.Write(DiagnosticBuilder.ForPosition(syntax.ForKeyword).LoopsNotSupported());
            }
            else if (item.PropertyLoopCount > 1)
            {
                // too many property loops
                this.diagnosticWriter.Write(DiagnosticBuilder.ForPosition(syntax.ForKeyword).TooManyPropertyLoops());
            }

            // push the parent to the stack
            this.loopParents.Push(item);

            // visit children
            base.VisitForSyntax(syntax);

            // pop the parent
            var lastPopped = this.loopParents.Pop();
            Debug.Assert(ReferenceEquals(lastPopped, item), "ReferenceEquals(lastPopped, item)");
        }

        private LoopValidationItem CreateValidationItem(ForSyntax syntax)
        {
            var (lastParentValid, lastPropertyLoopCount) = this.loopParents.TryPeek(out var lastStatus)
                ? (lastStatus.IsValidParent, lastStatus.PropertyLoopCount)
                : (true, 0);

            var parent = this.semanticModel.SyntaxTree.Hierarchy.GetParent(syntax);

            // keep the cases in sync with the error message in the default case
            switch (parent)
            {
                // loops are allowed in top-level module/resource values
                case ResourceDeclarationSyntax resource when ReferenceEquals(resource.Value, syntax):
                case ModuleDeclarationSyntax module when ReferenceEquals(module.Value, syntax):
                    return new LoopValidationItem(parent, lastParentValid, lastPropertyLoopCount);

                // loops are generally allowed in property values
                case ObjectPropertySyntax property when ReferenceEquals(property.Value, syntax):
                    return new LoopValidationItem(parent, lastParentValid, lastPropertyLoopCount + 1);

                default:
                    return new LoopValidationItem(parent, false, lastPropertyLoopCount);
            }
        }

        private class LoopValidationItem
        {
            public LoopValidationItem(SyntaxBase? parent, bool isValidParent, int propertyLoopCount)
            {
                this.Parent = parent;
                this.IsValidParent = isValidParent;
                this.PropertyLoopCount = propertyLoopCount;
            }

            public SyntaxBase? Parent { get; }

            public bool IsValidParent { get; }

            public int PropertyLoopCount { get; }
        }
    }
}
