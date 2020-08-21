// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics
{
    public class InstanceSymbol : DeclaredSymbol
    {
        public InstanceSymbol(ISymbolContext context, string name, InstanceDeclarationSyntax declaringSyntax, ApplicationSymbol? parent)
            : base(context, name, declaringSyntax, declaringSyntax.Name)
        {
            this.Parent = parent;
        }

        public InstanceDeclarationSyntax DeclaringInstance => (InstanceDeclarationSyntax)this.DeclaringSyntax;

        public SyntaxBase Body => DeclaringInstance.Body;

        public override void Accept(SymbolVisitor visitor) => visitor.VisitInstanceSymbol(this);

        public override SymbolKind Kind => SymbolKind.Instance;

        public override IEnumerable<Symbol> Descendants
        {
            get
            {
                yield return this.Type;
            }
        }

        public ApplicationSymbol? Parent { get; }

        public string? GetKindForDisplay()
        {
            if (DeclaringInstance.Type is StringSyntax str && str.TryGetLiteralValue() is string literal)
            {
                return literal;
            }

            return "unknown kind";
        }
    }
}
