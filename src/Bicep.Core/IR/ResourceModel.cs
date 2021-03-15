// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;
using Azure.Deployments.Expression.Expressions;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;

namespace Bicep.Core.IR
{
    public class ResourceModel
    {
        public ResourceModel(
            ResourceSymbol symbol,
            ResourceTypeReference typeReference,
            ImmutableArray<PropertyModel> properties,
            ImmutableArray<ValueModel> conditions,
            ImmutableArray<ValueModel> dependsOn,
            LanguageExpression? scope)
        {
            this.Symbol = symbol;
            this.TypeReference = typeReference;
            this.Properties = properties;
            this.Conditions = conditions;
            this.DependsOn = dependsOn;
            this.Scope = scope;
        }

        public string ApiVersion => this.TypeReference.ApiVersion;
        
        public string Type => this.TypeReference.FullyQualifiedType;

        public ImmutableArray<ValueModel> Conditions { get; }

        public ImmutableArray<ValueModel> DependsOn { get; }

        public ImmutableArray<PropertyModel> Properties { get; }

        public LanguageExpression? Scope { get; }

        public ResourceSymbol Symbol { get; }

        public ResourceTypeReference TypeReference { get; }
    }
}