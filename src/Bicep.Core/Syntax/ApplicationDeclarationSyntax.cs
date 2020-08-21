// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Navigation;
using Bicep.Core.Parsing;

namespace Bicep.Core.Syntax
{
    public class ApplicationDeclarationSyntax : SyntaxBase, INamedDeclarationSyntax
    {
        public ApplicationDeclarationSyntax(Token keyword, IdentifierSyntax name, SyntaxBase assignment, SyntaxBase? ifCondition, SyntaxBase body)
        {
            AssertKeyword(keyword, nameof(keyword), LanguageConstants.ApplicationKeyword);
            AssertSyntaxType(name, nameof(name), typeof(IdentifierSyntax));
            AssertTokenType(keyword, nameof(keyword), TokenType.Identifier);
            AssertSyntaxType(assignment, nameof(assignment), typeof(Token), typeof(SkippedTriviaSyntax));
            AssertTokenType(assignment as Token, nameof(assignment), TokenType.Assignment);
            AssertSyntaxType(ifCondition, nameof(ifCondition), typeof(SkippedTriviaSyntax), typeof(IfConditionSyntax));
            AssertSyntaxType(body, nameof(body), typeof(SkippedTriviaSyntax), typeof(ObjectSyntax));

            this.Keyword = keyword;
            this.Name = name;
            this.Assignment = assignment;
            this.IfCondition = ifCondition;
            this.Body = body;
        }

        public Token Keyword { get; }

        public IdentifierSyntax Name { get; }

        public SyntaxBase Assignment { get; }

        public SyntaxBase? IfCondition { get; }

        public SyntaxBase Body { get; }

        public override void Accept(ISyntaxVisitor visitor) => visitor.VisitApplicationDeclarationSyntax(this);

        public override TextSpan Span => TextSpan.Between(Keyword, Body);
    }
}