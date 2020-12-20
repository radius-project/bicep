// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Navigation;
using Bicep.Core.Parsing;

namespace Bicep.Core.Syntax
{
    public class FunctionCallSyntax : ExpressionSyntax, ISymbolReference
    {
        public FunctionCallSyntax(IdentifierSyntax name, Token openParen, IEnumerable<SyntaxBase> children, Token closeParen)
        {
            AssertTokenType(openParen, nameof(openParen), TokenType.LeftParen);
            AssertTokenType(closeParen, nameof(closeParen), TokenType.RightParen);

            this.Name = name;
            this.OpenParen = openParen;
            this.Children = children.ToImmutableArray();
            this.CloseParen = closeParen;

            this.Arguments = this.Children.OfType<FunctionArgumentSyntax>().ToImmutableArray();
        }

        public IdentifierSyntax Name { get; }

        public Token OpenParen { get; }

        public ImmutableArray<SyntaxBase> Children { get; }

        public Token CloseParen { get; }

        public override void Accept(ISyntaxVisitor visitor) => visitor.VisitFunctionCallSyntax(this);

        public override TextSpan Span => TextSpan.Between(Name, CloseParen);

        public ImmutableArray<FunctionArgumentSyntax> Arguments { get; }
    }
}
