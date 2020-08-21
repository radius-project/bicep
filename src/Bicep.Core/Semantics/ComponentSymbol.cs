// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics
{
    public class ComponentSymbol : DeclaredSymbol
    {
        public ComponentSymbol(ISymbolContext context, string name, ComponentDeclarationSyntax declaringSyntax, ApplicationSymbol? parent)
            : base(context, name, declaringSyntax, declaringSyntax.Name)
        {
            Parent = parent;
        }

        public ApplicationSymbol? Parent { get; }

        public ComponentDeclarationSyntax DeclaringComponent => (ComponentDeclarationSyntax)this.DeclaringSyntax;

        public SyntaxBase Body => DeclaringComponent.Body;

        public override void Accept(SymbolVisitor visitor) => visitor.VisitComponentSymbol(this);

        public override SymbolKind Kind => SymbolKind.Component;

        public override IEnumerable<Symbol> Descendants
        {
            get
            {
                yield return this.Type;
            }
        }

        public string? GetKindForDisplay()
        {
            if (DeclaringComponent.Type is StringSyntax str && str.TryGetLiteralValue() is string literal)
            {
                return literal;
            }

            return "unknown kind";
        }
    }
}
