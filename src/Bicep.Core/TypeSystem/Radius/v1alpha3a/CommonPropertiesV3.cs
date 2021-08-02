// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bicep.Core.TypeSystem.Radiusv1alpha3a
{
    internal static class CommonProperties
    {
        public static readonly TypeProperty Id = new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Name = new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty OptionalName = new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Default = new TypeProperty("default", LanguageConstants.Bool, TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Type = new TypeProperty("type", new StringLiteralType(Bicep.Core.TypeSystem.Radius.RadiusResources.ComponentResourceType), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty ApiVersion = new TypeProperty("apiVersion", new StringLiteralType(Bicep.Core.TypeSystem.Radius.RadiusResources.ResourceApiVersion), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty DependsOn = new TypeProperty("dependsOn", new TypedArrayType(LanguageConstants.ResourceRef, TypeSymbolValidationFlags.Default), TypePropertyFlags.WriteOnly);

        public static readonly TypeProperty Tags = new TypeProperty("tags", LanguageConstants.Tags);

        public static readonly TypeProperty ComponentUses = new TypeProperty(
            "uses",
            new TypedArrayType(MakeComponentUseType(), TypeSymbolValidationFlags.WarnOnTypeMismatch));

        public static readonly TypeProperty Traits = new TypeProperty("traits", CommonTraits.TraitArrayType);

        public static readonly TypeProperty Scopes = new TypeProperty("scopes", LanguageConstants.Array);

        private static ITypeReference MakeComponentUseType()
        {
            var bindings = CommonBindings.AllBindingData
                .ToDictionary(b => b.Type.FormatKind(), b => b.Values.ToArray());

            var members = new List<ObjectType>();
            foreach (var kvp in bindings)
            {
                var keyType = UnionType.Create(kvp.Value.Select(key => new StringLiteralType(key)));
                var envType = new ObjectType(
                    "env",
                    validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                    properties: Array.Empty<TypeProperty>(),
                    additionalPropertiesType: keyType,
                    additionalPropertiesFlags: TypePropertyFlags.None);

                var secretsKeysType = new ObjectType(
                    "keys",
                    validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                    properties: Array.Empty<TypeProperty>(),
                    additionalPropertiesType: keyType,
                    additionalPropertiesFlags: TypePropertyFlags.None);

                var secretsType = new ObjectType(
                    "secrets",
                    validationFlags: TypeSymbolValidationFlags.Default,
                    properties: new []
                    {
                        new TypeProperty("store", LanguageConstants.String, TypePropertyFlags.Required),
                        new TypeProperty("keys", secretsKeysType, TypePropertyFlags.Required),
                    },
                    additionalPropertiesType: null,
                    additionalPropertiesFlags: TypePropertyFlags.None);

                var member = new ObjectType(
                    name: $"use {kvp.Key}",
                    validationFlags: TypeSymbolValidationFlags.Default,
                    properties: new TypeProperty[]
                    {
                        new TypeProperty("kind", new StringLiteralType(kvp.Key), TypePropertyFlags.Required | TypePropertyFlags.Constant),
                        new TypeProperty("binding", LanguageConstants.String, TypePropertyFlags.None),
                        new TypeProperty("env", envType, TypePropertyFlags.None),
                        new TypeProperty("secrets", secretsType, TypePropertyFlags.None),
                    },
                    additionalPropertiesType: null,
                    additionalPropertiesFlags: TypePropertyFlags.None,
                    functions: null);
                members.Add(member);
            }

            return new DiscriminatedObjectType(
                name: "use binding",
                TypeSymbolValidationFlags.WarnOnTypeMismatch,
                "kind",
                members);
        }
    }
}
