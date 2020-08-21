// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.TypeSystem.Applications
{
    internal static class CommonProperties
    {
        public static readonly TypeProperty Id = new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Name = new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Type = new TypeProperty("type", new StringLiteralType(ComponentType.ResourceType.FullyQualifiedType), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty ApiVersion = new TypeProperty("apiVersion", new StringLiteralType(ComponentType.ResourceType.ApiVersion), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty DependsOn = new TypeProperty("dependsOn", new TypedArrayType(LanguageConstants.ResourceRef, TypeSymbolValidationFlags.Default), TypePropertyFlags.WriteOnly);

        public static readonly TypeProperty Tags = new TypeProperty("tags", LanguageConstants.Tags);

        public static readonly TypeProperty Application = new TypeProperty("application", LanguageConstants.String);

        public static readonly TypeProperty Kind = new TypeProperty("kind", LanguageConstants.String, TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Config = new TypeProperty("config", LanguageConstants.Any);

        public static readonly TypeProperty Run = new TypeProperty("run", LanguageConstants.Any);

        public static readonly TypeProperty ComponentDependsOn = new TypeProperty("dependsOn", LanguageConstants.Array);

        private static readonly NamedObjectType ComponentProvidesObjectType = new NamedObjectType(
                "provides",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required),
                    new TypeProperty("kind", LanguageConstants.String, TypePropertyFlags.Required),
                    new TypeProperty("containerPort", LanguageConstants.Int, TypePropertyFlags.None),
                },
                additionalPropertiesType: LanguageConstants.Any,
                additionalPropertiesFlags: TypePropertyFlags.None);

        public static readonly TypedArrayType ComponentProvidesArrayType = new TypedArrayType(ComponentProvidesObjectType, TypeSymbolValidationFlags.WarnOnTypeMismatch);

        public static readonly TypeProperty Provides = new TypeProperty("provides", ComponentProvidesArrayType);

        public static readonly TypeProperty Traits = new TypeProperty("traits", CommonTraits.TraitArrayType);

        public static readonly TypeProperty Scopes = new TypeProperty("scopes", LanguageConstants.Array);
    }
}