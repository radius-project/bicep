// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Syntax;

namespace Bicep.Core.Emit
{
    public class CompoundName
    {
        public CompoundName(IEnumerable<Segment> segments)
        {
            Segments = segments.ToImmutableArray();

            if (Segments.Length == 0)
            {
                throw new ArgumentException("A compound name must have at least 1 segment.", nameof(segments));
            }
        }

        public ImmutableArray<Segment> Segments { get; }

        public readonly struct Segment
        {
            public readonly string? Literal;
            public readonly SyntaxBase? Expression;

            public Segment(string literal)
            {
                Literal = literal;
                Expression = null;
            }

            public Segment(SyntaxBase expression)
            {
                Expression = expression;
                Literal = null;
            }
        }
    }
}