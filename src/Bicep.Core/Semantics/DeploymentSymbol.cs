// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics
{
    public class DeploymentSymbol : DeclaredSymbol
    {
        public DeploymentSymbol(ISymbolContext context, string name, DeploymentDeclarationSyntax declaringSyntax, ApplicationSymbol? parent)
            : base(context, name, declaringSyntax, declaringSyntax.Name)
        {
            Parent = parent;
        }

        public DeploymentDeclarationSyntax DeclaringDeployment => (DeploymentDeclarationSyntax)base.DeclaringSyntax;

        public SyntaxBase Body => DeclaringDeployment.Body;

        public override IEnumerable<Symbol> Descendants
        {
            get
            {
                yield return this.Type;
            }
        }

        public ApplicationSymbol? Parent { get; }

        public override void Accept(SymbolVisitor visitor) => visitor.VisitDeploymentSymbol(this);

        public override SymbolKind Kind => SymbolKind.Deployment;
    }
}
