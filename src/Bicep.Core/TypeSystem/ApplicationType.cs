// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Resources;

namespace Bicep.Core.TypeSystem
{
    public class ApplicationType : TypeSymbol
    {
        internal const string FullyQualifiedTypeName = "Microsoft.CustomProviders/resourceProviders/Applications@2018-09-01-preview";
        internal static readonly ResourceTypeReference ResourceType = ResourceTypeReference.Parse(FullyQualifiedTypeName);

        public static readonly ApplicationType Instance = new ApplicationType(new NamedObjectType(
            name: FullyQualifiedTypeName,
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant),
                new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required | TypePropertyFlags.DeployTimeConstant),
                new TypeProperty("type", new StringLiteralType(ResourceType.FullyQualifiedType), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant),
                new TypeProperty("apiVersion", new StringLiteralType(ResourceType.ApiVersion), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant),
                new TypeProperty("dependsOn", new TypedArrayType(LanguageConstants.ResourceRef, TypeSymbolValidationFlags.Default), TypePropertyFlags.WriteOnly),
                new TypeProperty("tags", LanguageConstants.Tags),
                new TypeProperty("properties", LanguageConstants.Object),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None));

        public ApplicationType(ITypeReference body)
            : base(FullyQualifiedTypeName)
        {
            Body = body;
        }

        public ITypeReference Body { get; }

        public override TypeKind TypeKind => TypeKind.Application;

        public ResourceTypeReference TypeReference => ResourceType;
    }
}