// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics
{
    public class ApplicationSymbol : DeclaredSymbol
    {
        public ApplicationSymbol(ISymbolContext context, string name, ApplicationDeclarationSyntax declaringSyntax)
            : base(context, name, declaringSyntax, declaringSyntax.Name)
        {
        }

        public ApplicationDeclarationSyntax DeclaringApplication => (ApplicationDeclarationSyntax)base.DeclaringSyntax;

        public SyntaxBase Body => DeclaringApplication.Body;

        public override void Accept(SymbolVisitor visitor) => visitor.VisitApplicationSymbol(this);

        public override SymbolKind Kind => SymbolKind.Application;

        public override IEnumerable<Symbol> Descendants
        {
            get
            {
                yield return this.Type;
            }
        }
    }
}
