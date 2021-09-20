// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// For now there's some dead-code here to deal with the idea of 'late-binding' to values
// #define RADIUS_LATE_BINDING
using System;
using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;

namespace Bicep.Core.TypeSystem.Radius.V3
{
    internal static class KnownTypes
    {
        private const string Version = RadiusResources.ResourceApiVersion;

        public static IEnumerable<ResourceType> MakeResourceTypes(IResourceTypeProvider provider)
        {
            var types = new List<ResourceType>();
            types.Add(MakeApplication(provider));
            types.AddRange(MakeComponents(provider));
            types.AddRange(MakeRoutes(provider));
            types.AddRange(MakeScopes(provider));

            return types;
        }

        public static ResourceType MakeApplication(IResourceTypeProvider provider)
        {
            var propertiesType = new ObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.None);

            var bodyType = new ObjectType(
                name: $"{RadiusResources.ApplicationResourceType}@{Version}",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    new TypeProperty("type", new StringLiteralType(RadiusResources.ApplicationResourceType), TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.ReadOnly),
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse($"{RadiusResources.ApplicationResourceType}@{Version}"),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        public static IEnumerable<ResourceType> MakeComponents(IResourceTypeProvider provider)
        {
            var components = new KnownComponents.ComponentData[]
            {
                KnownComponents.MakeContainer(),
                KnownComponents.MakeExecutable(),
                KnownComponents.MakeDaprStateStore(),
                KnownComponents.MakeDaprPubSubTopic(),
                KnownComponents.MakeMongoDB(),
                KnownComponents.MakeCosmosDBMongo(),
                KnownComponents.MakeCosmosDBSQL(),
                KnownComponents.MakeKeyVault(),
                KnownComponents.MakeServiceBusQueue(),
                KnownComponents.MakeRedis(),
            };

            return components.Select(s => MakeComponentType(provider, s));
        }

        public static IEnumerable<ResourceType> MakeScopes(IResourceTypeProvider provider)
        {
            var scopes = new KnownScopes.ScopeData[]
            {
                KnownScopes.MakeDaprScope(),
                KnownScopes.MakeNetworkScope(),
            };

            return scopes.Select(s => MakeScopeType(provider, s));
        }

        private static ResourceType MakeComponentType(IResourceTypeProvider provider, KnownComponents.ComponentData component)
        {
            var properties = new List<TypeProperty>()
            {
                // All components support traits
                CommonProperties.Traits,
            };

            properties.AddRange(component.Properties);
            if (component.Binding is {})
            {
                properties.AddRange(component.Binding.Properties.Where(p => !properties.Any(pp => p.Name == pp.Name)));
            }

            var propertiesType = new ObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            var bodyType = new ObjectType(
                name: component.Type.FormatTypeAndVersion(RadiusResources.ApplicationResourceType, Version),
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    // Top level properties are predefined
                    CommonProperties.Id,
                    CommonProperties.Name,
                    new TypeProperty("type", new StringLiteralType(component.Type.FormatType(RadiusResources.ApplicationResourceType)), TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.ReadOnly),
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: Array.Empty<FunctionOverload>());

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse(component.Type.FormatTypeAndVersion(RadiusResources.ApplicationResourceType, Version)),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        public static IEnumerable<ResourceType> MakeRoutes(IResourceTypeProvider provider)
        {
            var routes = CommonBindings.RouteBindingData;

            return routes.Select(r => MakeRouteType(provider, r));
        }

        private static ResourceType MakeRouteType(IResourceTypeProvider provider, CommonBindings.BindingData route)
        {
            var functions = new List<FunctionOverload>();
#if RADIUS_LATE_BINDING
            functions.AddRange(route.Values.Where(v => !v.Secret).Select(v => MakeFunction(route, v.Name)));
#endif
            var propertiesType = new ObjectType(
                name: "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: route.Properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var bodyType = new ObjectType(
                route.Type.FormatTypeAndVersion(RadiusResources.ApplicationResourceType, Version),
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new []
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    new TypeProperty("type", new StringLiteralType(route.Type.FormatType(RadiusResources.ApplicationResourceType)), TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.ReadOnly),
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    new TypeProperty("properties", propertiesType, propertiesType.Properties.Any(p => p.Value.Flags.HasFlag(TypePropertyFlags.Required)) ? TypePropertyFlags.Required : TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: functions);

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse(route.Type.FormatTypeAndVersion(RadiusResources.ApplicationResourceType, Version)),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        private static ResourceType MakeScopeType(IResourceTypeProvider provider, KnownScopes.ScopeData scope)
        {
            var propertiesType = new ObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: scope.Properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, scope.Properties.Any(p => p.Flags.HasFlag(TypePropertyFlags.Required)) ? TypePropertyFlags.Required : TypePropertyFlags.None);

            var bodyType = new ObjectType(
                name: scope.Type.FormatTypeAndVersion(RadiusResources.ApplicationResourceType, Version),
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    new TypeProperty("type", new StringLiteralType(scope.Type.FormatType(RadiusResources.ApplicationResourceType)), TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.ReadOnly),
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: Array.Empty<FunctionOverload>());

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse(scope.Type.FormatTypeAndVersion(RadiusResources.ApplicationResourceType, Version)),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        private static TypeProperty MakeBindingsProperty(Dictionary<string, CommonBindings.BindingData> builtIn)
        {
            var properties = builtIn?.Select(kvp =>
            {
                var functions = new List<FunctionOverload>();

#if RADIUS_LATE_BINDING
                functions.AddRange(kvp.Value.Values.Select(v => MakeFunction(kvp.Value, v.Name)));
#endif

                // Every binding needs to have an ID property so we can reference it.
                var properties = new List<TypeProperty>()
                {
                    new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                };
                // Computed values are accessible via readonly properties if they are not secrets.
                properties.AddRange(kvp.Value.Properties);

                var bindingType = new ObjectType(
                    name: $"binding properties: {kvp.Value.Type.FormatKind()}",
                    validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                    properties: properties,
                    additionalPropertiesType: null,
                    additionalPropertiesFlags: TypePropertyFlags.None,
                    functions: functions);

                return new TypeProperty(kvp.Key, bindingType, TypePropertyFlags.None);
            }).ToArray() ?? Array.Empty<TypeProperty>();

            var bindingsType = new ObjectType(
                "bindings",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);

            return new TypeProperty("bindings", bindingsType, TypePropertyFlags.ReadOnly);
        }

#if RADIUS_LATE_BINDING
        private static ObjectType BindingValue = new ObjectType(
            name: "binding value",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("source", LanguageConstants.String, TypePropertyFlags.Required, "The source of the binding"),
                new TypeProperty("kind", LanguageConstants.String, TypePropertyFlags.Required, "The kind of binding"),
                new TypeProperty("valueName", LanguageConstants.String, TypePropertyFlags.Required, "The name of the value to bind"),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None,
            functions: Array.Empty<FunctionOverload>());

        private static FunctionOverload MakeFunction(CommonBindings.BindingData binding, string value)
        {
            return new FunctionOverloadBuilder("bind" + value[0].ToString().ToUpper() + value.Substring(1))
                .WithDescription($"Creates a binding to the {value} value")
                .WithEvaluator((function, symbol, type) => EvaluateBinding(binding, value, function, symbol, type))
                .WithFlags(FunctionFlags.RequiresInlining)
                .WithReturnType(BindingValue)
                .Build();
        }

        private static SyntaxBase EvaluateBinding(CommonBindings.BindingData binding, string value, FunctionCallSyntaxBase functionCall, Symbol symbol, TypeSymbol typeSymbol)
        {
            // A function like foo.bindUrl() is replaced with code like:
            // {
            //   source: foo.id
            //   kind: 'Http'
            //   valueName: 'url'
            // }

            if (functionCall is InstanceFunctionCallSyntax instance)
            {
                return SyntaxFactory.CreateObject(new []
                {
                    SyntaxFactory.CreateObjectProperty("source",  SyntaxFactory.CreatePropertyAccess(instance.BaseExpression, "id")),
                    SyntaxFactory.CreateObjectProperty("kind", SyntaxFactory.CreateStringLiteral(binding.Type.FormatKind())),
                    SyntaxFactory.CreateObjectProperty("valueName", SyntaxFactory.CreateStringLiteral(value))
                });
            }

            throw new InvalidOperationException("this should not be found for a static function");
        }
#endif
    }
}
