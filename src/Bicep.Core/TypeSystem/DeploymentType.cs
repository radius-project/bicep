// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Resources;

namespace Bicep.Core.TypeSystem
{
    public class DeploymentType : TypeSymbol
    {
        internal const string FullyQualifiedTypeName = "Microsoft.CustomProviders/resourceProviders/Applications/Deployments@2018-09-01-preview";
        internal static readonly ResourceTypeReference ResourceType = ResourceTypeReference.Parse(FullyQualifiedTypeName);

        public static DeploymentType Instance => new DeploymentType(new NamedObjectType(
            name: FullyQualifiedTypeName,
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties:   new []
            {
                new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant),
                new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required | TypePropertyFlags.DeployTimeConstant),
                new TypeProperty("type", new StringLiteralType(ResourceType.FullyQualifiedType), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant),
                new TypeProperty("apiVersion", new StringLiteralType(ResourceType.ApiVersion), TypePropertyFlags.ReadOnly | TypePropertyFlags.DeployTimeConstant),
                new TypeProperty("dependsOn", new TypedArrayType(LanguageConstants.ResourceRef, TypeSymbolValidationFlags.Default), TypePropertyFlags.WriteOnly),
                new TypeProperty("tags", LanguageConstants.Tags),
                new TypeProperty("application", LanguageConstants.String),
                new TypeProperty("properties", LanguageConstants.Object),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None));

        public DeploymentType(ITypeReference body)
            : base(FullyQualifiedTypeName)
        {
            Body = body;
        }

        public ITypeReference Body { get; }

        public override TypeKind TypeKind => TypeKind.Deployment;

        public ResourceTypeReference TypeReference => ResourceType;
    }
}