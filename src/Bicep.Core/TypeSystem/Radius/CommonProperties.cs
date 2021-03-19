// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;

namespace Bicep.Core.TypeSystem.Radius
{
    internal static class CommonProperties
    {
        public static readonly TypeProperty Id = new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Name = new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Type = new TypeProperty("type", new StringLiteralType(RadiusResources.ComponentCRPType), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty ApiVersion = new TypeProperty("apiVersion", new StringLiteralType(RadiusResources.CRPApiVersion), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty DependsOn = new TypeProperty("dependsOn", new TypedArrayType(LanguageConstants.ResourceRef, TypeSymbolValidationFlags.Default), TypePropertyFlags.WriteOnly);

        public static readonly TypeProperty Tags = new TypeProperty("tags", LanguageConstants.Tags);

        public static readonly TypeProperty Application = new TypeProperty("application", LanguageConstants.String);

        public static readonly TypeProperty Config = new TypeProperty("config", LanguageConstants.Any);

        public static readonly TypeProperty Run = new TypeProperty("run", LanguageConstants.Any);

        public static readonly TypeSymbol EnvVarType = UnionType.Create(LanguageConstants.LooseString, LanguageConstants.Bool, LanguageConstants.Int, LanguageConstants.Null);

        public static readonly TypeSymbol SecretType = UnionType.Create(LanguageConstants.LooseString, LanguageConstants.Bool, LanguageConstants.Int, LanguageConstants.Null);

        public static readonly ObjectType UsesSecretsKeysType = new ObjectType(
            "keys",
            validationFlags: TypeSymbolValidationFlags.Default,
            properties: Array.Empty<TypeProperty>(),
            additionalPropertiesType: new ExpressionType("secret expression", SecretType),
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static readonly ObjectType UsesSecretsType = new ObjectType(
            "secrets",
            validationFlags: TypeSymbolValidationFlags.Default,
            properties: new []
            {
                new TypeProperty("store", new ExpressionType("binding expression", CommonBindings.BindingType), TypePropertyFlags.Required),
                new TypeProperty("keys", UsesSecretsKeysType, TypePropertyFlags.Required),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static readonly ObjectType UsesEnvType = new ObjectType(
            "env",
            validationFlags: TypeSymbolValidationFlags.Default,
            properties: Array.Empty<TypeProperty>(),
            additionalPropertiesType: new ExpressionType("environment variable expression", EnvVarType),
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static readonly ObjectType ComponentUsesType = new ObjectType(
            "uses",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("binding", new ExpressionType("binding expression", CommonBindings.BindingType), TypePropertyFlags.None),
                new TypeProperty("env", UsesEnvType, TypePropertyFlags.None),
                new TypeProperty("secrets", UsesSecretsType, TypePropertyFlags.None)
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static readonly TypeProperty ComponentUses = new TypeProperty("uses", new TypedArrayType(ComponentUsesType, TypeSymbolValidationFlags.WarnOnTypeMismatch));

        public static readonly ObjectType ComponentBindingsType = new ObjectType(
            "bindings",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: Array.Empty<TypeProperty>(),
            additionalPropertiesType: CommonBindings.BindingType,
            additionalPropertiesFlags: TypePropertyFlags.None);

        public static readonly TypeProperty Bindings = new TypeProperty("bindings", ComponentBindingsType);

        public static readonly TypeProperty Traits = new TypeProperty("traits", CommonTraits.TraitArrayType);

        public static readonly TypeProperty Scopes = new TypeProperty("scopes", LanguageConstants.Array);
    }
}
