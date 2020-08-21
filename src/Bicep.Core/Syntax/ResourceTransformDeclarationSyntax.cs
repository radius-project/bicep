// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Navigation;
using Bicep.Core.Parsing;
using Bicep.Core.Resources;
using Bicep.Core.Semantics.Libraries;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Syntax
{
    public class ResourceTransformDeclarationSyntax : SyntaxBase, INamedDeclarationSyntax
    {
        public ResourceTransformDeclarationSyntax(IdentifierSyntax transform, IdentifierSyntax name, SyntaxBase? type, SyntaxBase assignment, SyntaxBase? ifCondition, SyntaxBase body)
        {
            AssertSyntaxType(name, nameof(name), typeof(IdentifierSyntax));
            AssertSyntaxType(type, nameof(type), typeof(StringSyntax), typeof(SkippedTriviaSyntax));
            AssertSyntaxType(transform, nameof(transform), typeof(IdentifierSyntax));
            AssertSyntaxType(assignment, nameof(assignment), typeof(Token), typeof(SkippedTriviaSyntax));
            AssertTokenType(assignment as Token, nameof(assignment), TokenType.Assignment);
            AssertSyntaxType(ifCondition, nameof(ifCondition), typeof(SkippedTriviaSyntax), typeof(IfConditionSyntax));
            AssertSyntaxType(body, nameof(body), typeof(SkippedTriviaSyntax), typeof(ObjectSyntax));

            this.Transform = transform;
            this.Name = name;
            this.Type = type;
            this.Assignment = assignment;
            this.IfCondition = ifCondition;
            this.Body = body;
        }

        public IdentifierSyntax Transform { get; }

        public IdentifierSyntax Name { get; }

        public SyntaxBase? Type { get; }

        public SyntaxBase Assignment { get; }

        public SyntaxBase? IfCondition { get; }

        public SyntaxBase Body { get; }

        public override void Accept(ISyntaxVisitor visitor) => visitor.VisitResourceTransformDeclarationSyntax(this);

        public override TextSpan Span => TextSpan.Between(Transform, Body);

        Token IDeclarationSyntax.Keyword => (Transform.Child as Token)!;

        public TypeSymbol GetDeclaredInputType(LibraryManager libraryManager)
        {
            throw null!;
        }

        public TypeSymbol GetDeclaredOutputType(LibraryManager libraryManager, IResourceTypeProvider resourceTypeProvider)
        {
            throw null!;
        }
    }
}
