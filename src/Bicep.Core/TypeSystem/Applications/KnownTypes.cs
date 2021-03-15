// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;

namespace Bicep.Core.TypeSystem.Applications
{
    internal static class KnownTypes
    {
        public static NamedObjectType MakeContainer()
        {
            var imageProperty = new TypeProperty("image", LanguageConstants.String, TypePropertyFlags.Required);
            var containerType = new NamedObjectType(
                "container",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    imageProperty
                },
                additionalPropertiesType: LanguageConstants.Any,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var containerProperty = new TypeProperty("container", containerType, TypePropertyFlags.Required);

            var runType = new NamedObjectType(
                "run",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    containerProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var runProperty = new TypeProperty("run", runType, TypePropertyFlags.Required);

            var buildProperty = new TypeProperty("build", MakeBuildSectionType(), TypePropertyFlags.None);

            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    buildProperty,
                    runProperty,
                    CommonProperties.ComponentDependsOn,
                    CommonProperties.Provides,
                    CommonProperties.Traits,
                    CommonProperties.Scopes,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            return new NamedObjectType(
                name: ComponentType.FullyQualifiedTypeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    CommonProperties.Application,
                    CommonProperties.Kind,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeFunction()
        {
            var codeProperty = new TypeProperty("code", LanguageConstants.Object);

            var runType = new NamedObjectType(
                "run",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    codeProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var runProperty = new TypeProperty("run", runType);
            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    runProperty,
                    CommonProperties.ComponentDependsOn,
                    CommonProperties.Provides,
                    CommonProperties.Traits,
                    CommonProperties.Scopes,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            return new NamedObjectType(
                name: ComponentType.FullyQualifiedTypeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    CommonProperties.Application,
                    CommonProperties.Kind,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeWebApp()
        {
            var codeProperty = new TypeProperty("code", LanguageConstants.Object);

            var runType = new NamedObjectType(
                "run",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    codeProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var runProperty = new TypeProperty("run", runType);

            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    runProperty,
                    CommonProperties.ComponentDependsOn,
                    CommonProperties.Provides,
                    CommonProperties.Traits,
                    CommonProperties.Scopes,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            return new NamedObjectType(
                name: ComponentType.FullyQualifiedTypeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    CommonProperties.Application,
                    CommonProperties.Kind,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeDaprComponent()
        {
            var configProperty = new TypeProperty("config", LanguageConstants.Object);

            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    configProperty,
                    CommonProperties.ComponentDependsOn,
                    CommonProperties.Provides,
                    CommonProperties.Traits,
                    CommonProperties.Scopes,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            return new NamedObjectType(
                name: ComponentType.FullyQualifiedTypeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    CommonProperties.Application,
                    CommonProperties.Kind,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeDaprStateStore()
        {
            var configProperty = new TypeProperty("config", LanguageConstants.Object);

            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    configProperty,
                    CommonProperties.ComponentDependsOn,
                    CommonProperties.Provides,
                    CommonProperties.Traits,
                    CommonProperties.Scopes,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            return new NamedObjectType(
                name: ComponentType.FullyQualifiedTypeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    CommonProperties.Application,
                    CommonProperties.Kind,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeContainerizedWorkload()
        {
            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Run,
                    CommonProperties.ComponentDependsOn,
                    CommonProperties.Provides,
                    CommonProperties.Traits,
                    CommonProperties.Scopes,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            return new NamedObjectType(
                name: ComponentType.FullyQualifiedTypeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    CommonProperties.Application,
                    CommonProperties.Kind,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeGeneric()
        {
            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Config,
                    CommonProperties.Run,
                    CommonProperties.ComponentDependsOn,
                    CommonProperties.Provides,
                    CommonProperties.Traits,
                    CommonProperties.Scopes,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            return new NamedObjectType(
                name: ComponentType.FullyQualifiedTypeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    CommonProperties.Application,
                    CommonProperties.Kind,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static TypeSymbol MakeBuildSectionType()
        {
            var dotnetBuilderType = new NamedObjectType(
                "dotnet builder",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new TypeProperty[]
                {
                    new TypeProperty("builder", new StringLiteralType("dotnet"), TypePropertyFlags.Required | TypePropertyFlags.Constant),
                    new TypeProperty("projectFile", LanguageConstants.String, TypePropertyFlags.Required | TypePropertyFlags.Constant),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var buildType = new DiscriminatedObjectType(
                name: "build",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                discriminatorKey: "builder",
                unionMembers: new []
                {
                    dotnetBuilderType,
                });
            return buildType;
        }
    }
}