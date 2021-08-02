// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;

namespace Bicep.Core.TypeSystem.Radius.V3
{
    public class ThreePartType : IEquatable<ThreePartType>
    {
        public ThreePartType(string? @namespace, string type, string category)
        {
            this.Namespace = @namespace;
            this.Type = type;
            this.Category = category;
        }

        public string? Namespace { get; }

        public string Type { get; }


        public string Category { get; }

        public string FormatKind() => Namespace == null ? Type : $"{Namespace}/{Type}";

        public string FormatType(string parent)
        {
            return $"{parent}/{(Namespace == null ? "" : (Namespace + "."))}{Type}{Category}";
        }

        public string FormatTypeAndVersion(string parent, string version)
        {
            return $"{parent}/{(Namespace == null ? "" : (Namespace + "."))}{Type}{Category}@{version}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Namespace, Type, Category);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as ThreePartType);
        }

        public bool Equals(ThreePartType other)
        {
            return other != null && this.Namespace == other.Namespace && this.Type == other.Type && other.Category == this.Category;
        }
    }
}
