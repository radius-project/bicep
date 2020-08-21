// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics
{
    public class ImportSymbol : DeclaredSymbol
    {
        public ImportSymbol(ISymbolContext context, string name, SyntaxBase declaringSyntax, IdentifierSyntax nameSyntax) 
            : base(context, name, declaringSyntax, nameSyntax)
        {
        }

        public override void Accept(SymbolVisitor visitor)
        {
            visitor.VisitImportSymbol(this);
        }

        public override SymbolKind Kind => SymbolKind.Import;
    }
}