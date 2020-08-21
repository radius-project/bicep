// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Bicep.Core.Extensions;

namespace Bicep.Core.TypeSystem.Applications
{
    public class ComponentTypeReference : IEquatable<ComponentTypeReference>
    {
        private static readonly Regex ResourceTypePattern = new Regex(@"^(?<namespace>[a-z0-9][a-z0-9\.]*)(/(?<type>[a-z0-9]+))+@(?<version>[a-z0-9][a-z0-9\.]*)$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public ComponentTypeReference(string @namespace, IEnumerable<string> types, string apiVersion)
        {
            if (String.IsNullOrWhiteSpace(@namespace))
            {
                throw new ArgumentException("Namespace must not be null, empty or whitespace.");
            }

            if (String.IsNullOrWhiteSpace(apiVersion))
            {
                throw new ArgumentException("API Version must not be null, empty or whitespace.");
            }

            this.Namespace = @namespace;
            this.Types = types.ToImmutableArray();
            if (this.Types.Length <= 0)
            {
                throw new ArgumentException("At least one type must be specified.");
            }

            this.ApiVersion = apiVersion;
        }

        public string Namespace { get; }

        public ImmutableArray<string> Types { get; }

        public string ApiVersion { get; }

        public string FullyQualifiedType => $"{this.Namespace}/{this.Types.ConcatString("/")}";

        public string FormatName()
            => $"{this.FullyQualifiedType}@{this.ApiVersion}";

        public static ComponentTypeReference? TryParse(string componentType)
        {
            var match = ResourceTypePattern.Match(componentType);
            if (match.Success == false)
            {
                return null;
            }

            var ns = match.Groups["namespace"].Value;
            var types = match.Groups["type"].Captures.Cast<Capture>().Select(c => c.Value);
            var version = match.Groups["version"].Value;

            return new ComponentTypeReference(ns, types, version);
        }

        public static ComponentTypeReference Parse(string componentType)
            => TryParse(componentType) ?? throw new ArgumentException($"Unable to parse '{componentType}'", nameof(componentType));

        public bool Equals(ComponentTypeReference other)
        {
            return
                other != null &&
                StringComparer.OrdinalIgnoreCase.Equals(this.Namespace, other.Namespace) &&
                this.Types.Length == other.Types.Length &&
                Enumerable.SequenceEqual(this.Types, other.Types, StringComparer.OrdinalIgnoreCase) &&
                StringComparer.OrdinalIgnoreCase.Equals(this.ApiVersion, other.ApiVersion);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as ComponentTypeReference);
        }

        public override int GetHashCode()
        {
            return 
                StringComparer.OrdinalIgnoreCase.GetHashCode(this.Namespace) ^
                Enumerable.Select(this.Types, x => StringComparer.OrdinalIgnoreCase.GetHashCode(x)).Aggregate((a, b) => a ^ b) ^
                StringComparer.OrdinalIgnoreCase.GetHashCode(this.ApiVersion);
        }
    }
}
