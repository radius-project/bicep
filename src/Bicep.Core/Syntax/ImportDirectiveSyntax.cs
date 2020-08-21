// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Navigation;
using Bicep.Core.Parsing;

namespace Bicep.Core.Syntax
{
    public class ImportDirectiveSyntax : SyntaxBase, IDeclarationSyntax
    {
        public ImportDirectiveSyntax(Token keyword, IdentifierSyntax name)
        {
            AssertKeyword(keyword, nameof(keyword), LanguageConstants.ImportKeyword);
            AssertSyntaxType(name, nameof(name), typeof(IdentifierSyntax));

            this.Keyword = keyword;
            this.Name = name;
        }

        public Token Keyword { get; }

        public IdentifierSyntax Name { get; }

        public override void Accept(ISyntaxVisitor visitor) => visitor.VisitImportDirectiveSyntax(this);

        public override TextSpan Span => TextSpan.Between(Name, Name);
    }
}
