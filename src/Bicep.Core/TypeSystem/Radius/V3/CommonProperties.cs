// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.TypeSystem.Radius.V3
{
    internal static class CommonProperties
    {
        public static readonly TypeProperty Id = new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Name = new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty ApiVersion = new TypeProperty("apiVersion", new StringLiteralType(RadiusResources.ResourceApiVersion), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty DependsOn = new TypeProperty("dependsOn", new TypedArrayType(LanguageConstants.ResourceRef, TypeSymbolValidationFlags.Default), TypePropertyFlags.WriteOnly);

        public static readonly TypeProperty Tags = new TypeProperty("tags", new TypedArrayType(LanguageConstants.String, TypeSymbolValidationFlags.Default), TypePropertyFlags.DeployTimeConstant);

        public static readonly TypeProperty Traits = new TypeProperty("traits", CommonTraits.TraitArrayType);
    }
}
