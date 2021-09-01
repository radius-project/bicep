// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;

namespace Bicep.Core.TypeSystem.Radiusv1alpha3f
{
    public class ThreePartType : IEquatable<ThreePartType>
    {
        public ThreePartType(string? @namespace, string type)
        {
            this.Namespace = @namespace;
            this.Type = type;
        }

        public string? Namespace { get; }

        public string Type { get; }

        public string FormatKind() => Namespace == null ? Type : $"{Namespace}/{Type}";

        public string FormatType(string parent, string category, string version)
        {
            return $"{parent}/{(Namespace == null ? "" : (Namespace + "."))}{Type}{category}@{version}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Namespace, Type);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as ThreePartType);
        }

        public bool Equals(ThreePartType other)
        {
            return other != null && this.Namespace == other.Namespace && this.Type == other.Type;
        }
    }
}
