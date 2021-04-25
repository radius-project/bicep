// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;

namespace Bicep.Core.TypeSystem.Radius
{
    internal static class KnownTypes
    {
        public static NamedObjectType MakeApplication()
        {
            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.None);

            return new NamedObjectType(
                name: "radius.dev/Applications@v1alpha1",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeDeployment()
        {
            var componentNameProperty = new TypeProperty("componentName", LanguageConstants.String, TypePropertyFlags.None);
            var componentEntryType = new NamedObjectType(
                name: "components",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new []
                {
                    componentNameProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var componentsProperty = new TypeProperty(
                "components",
                new TypedArrayType(componentEntryType, TypeSymbolValidationFlags.Default),
                TypePropertyFlags.Required);

            var propertiesType = new NamedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    componentsProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            return new NamedObjectType(
                name: "radius.dev/Applications/Deployments@v1alpha1",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static DiscriminatedObjectType MakeComponent()
        {
            var members = new ITypeReference[]
            {
                MakeContainer(),
                MakeFunction(),
                MakeWebApp(),
                MakeDaprComponent(),
                MakeDaprStateStore(),
                MakeServiceBusQueue(),
                MakeCosmosDocumentDb(),
                MakeKeyVault(),
            };

            var type = new DiscriminatedObjectType(
                name: "radius.dev/Applications/Components@v1alpha1",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                discriminatorKey: "kind",
                members);
            return type;
        }

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

            var kindProperty = new TypeProperty("kind", new StringLiteralType("radius.dev/Container@v1alpha1"), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new NamedObjectType(
                name: "radius.dev/Container@v1alpha1",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    kindProperty,
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

            var kindProperty = new TypeProperty("kind", new StringLiteralType("azure.com/Function@v1alpha1"), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new NamedObjectType(
                name: "azure.com/Function@v1alpha1",
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
                    kindProperty,
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

            var kindProperty = new TypeProperty("kind", new StringLiteralType("azure.com/WebApp@v1alpha1"), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new NamedObjectType(
                name: "azure.com/WebApp@v1alpha1",
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
                    kindProperty,
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

            var kindProperty = new TypeProperty("kind", new StringLiteralType("dapr.io/Component@v1alpha1"), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new NamedObjectType(
                name: "dapr.io/Component@v1alpha1",
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
                    kindProperty,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeDaprStateStore()
        {
            var configKindType = UnionType.Create(new StringLiteralType("state.azure.tablestorage"), new StringLiteralType("any"));

            var configType = new NamedObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("kind", configKindType, TypePropertyFlags.Required),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.Required),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var configProperty = new TypeProperty("config", configType, TypePropertyFlags.Required);

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

            var kindProperty = new TypeProperty("kind", new StringLiteralType("dapr.io/StateStore@v1alpha1"), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new NamedObjectType(
                name: "dapr.io/StateStore@v1alpha1",
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
                    kindProperty,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeServiceBusQueue()
        {
            var configType = new NamedObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.Required),
                    new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.Required),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var configProperty = new TypeProperty("config", configType, TypePropertyFlags.Required);

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

            var kindProperty = new TypeProperty("kind", new StringLiteralType("azure.com/ServiceBusQueue@v1alpha1"), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new NamedObjectType(
                name: "azure.com/ServiceBusQueue@v1alpha1",
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
                    kindProperty,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeCosmosDocumentDb()
        {
            var configType = new NamedObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.Required),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var configProperty = new TypeProperty("config", configType, TypePropertyFlags.Required);

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

            var kindProperty = new TypeProperty("kind", new StringLiteralType("azure.com/CosmosDocumentDb@v1alpha1"), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new NamedObjectType(
                name: "azure.com/CosmosDocumentDb@v1alpha1",
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
                    kindProperty,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
        }

        public static NamedObjectType MakeKeyVault()
        {
            var configType = new NamedObjectType(
                "config",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.Required),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var configProperty = new TypeProperty("config", configType, TypePropertyFlags.Required);

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

            var kindProperty = new TypeProperty("kind", new StringLiteralType("azure.com/KeyVault@v1alpha1"), TypePropertyFlags.Required | TypePropertyFlags.Constant);

            return new NamedObjectType(
                name: "azure.com/KeyVault@v1alpha1",
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
                    kindProperty,
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
