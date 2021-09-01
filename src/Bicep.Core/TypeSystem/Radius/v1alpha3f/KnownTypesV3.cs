// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;

namespace Bicep.Core.TypeSystem.Radiusv1alpha3f
{
    internal static class KnownTypes
    {
        private const string Version = "v1alpha3f";

        public static IEnumerable<ResourceType> MakeResourceTypes(IResourceTypeProvider provider)
        {
            var types = new List<ResourceType>();
            types.Add(MakeApplication(provider));
            types.AddRange(MakeComponents(provider));
            types.AddRange(MakeRoutes(provider));
            types.AddRange(MakeScopes(provider));
            types.AddRange(MakeScopeRoutes(provider));

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
                name: $"radius.dev/Application@{Version}",
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

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse($"radius.dev/Application@{Version}"),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        public static IEnumerable<ResourceType> MakeComponents(IResourceTypeProvider provider)
        {
            var components = new KnownComponents.ComponentData[]
            {
                KnownComponents.MakeContainer(),
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

        public static IEnumerable<ResourceType> MakeScopeRoutes(IResourceTypeProvider provider)
        {
            var scopes = new KnownScopes.ScopeData[]
            {
                KnownScopes.MakeDaprScope(),
                KnownScopes.MakeNetworkScope(),
            };

            var routes = CommonBindings.RouteBindingData;

            return scopes
                .SelectMany(s => routes.Where(r => s.Routes.Any(sr => r.Type.Equals(sr))).Select(r => new { scope = s, route = r, }))
                .Select(sr => MakeScopeRouteType(provider, sr.scope, sr.route));
        }

        private static ResourceType MakeComponentType(IResourceTypeProvider provider, KnownComponents.ComponentData component)
        {
            var properties = new List<TypeProperty>()
            {
                CommonProperties.Traits,
                CommonProperties.Scopes,
            };

            if (component.Bindings.Count > 0)
            {
                properties.Add(MakeBindingsProperty(component.Bindings));
            }

            if (component.Config.Count > 0)
            {
                var configType = new ObjectType(
                    "config",
                    validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                    properties: component.Config,
                    additionalPropertiesType: null,
                    additionalPropertiesFlags: TypePropertyFlags.None);
                properties.Add(new TypeProperty("config", configType, component.Config.Any(p => p.Flags.HasFlag(TypePropertyFlags.Required)) ? TypePropertyFlags.Required : TypePropertyFlags.None));
            }

            if (component.Run.Count > 0)
            {
                var runType = new ObjectType(
                    "run",
                    validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                    properties: component.Run,
                    additionalPropertiesType: null,
                    additionalPropertiesFlags: TypePropertyFlags.None);
                properties.Add(new TypeProperty("run", runType, component.Run.Any(p => p.Flags.HasFlag(TypePropertyFlags.Required)) ? TypePropertyFlags.Required : TypePropertyFlags.None));
            }

            var propertiesType = new ObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            var bodyType = new ObjectType(
                name: component.Type.FormatType("radius.dev/Application", "Component", Version),
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
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: Array.Empty<FunctionOverload>());

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse(component.Type.FormatType("radius.dev/Application", "Component", Version)),
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
            functions.AddRange(route.Values.Select(v => MakeFunction(v)));

            var propertiesType = new ObjectType(
                name: "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: route.Properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var bodyType = new ObjectType(
                route.Type.FormatType("radius.dev/Application", "Route", Version),
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new []
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    new TypeProperty("properties", propertiesType, propertiesType.Properties.Any(p => p.Value.Flags.HasFlag(TypePropertyFlags.Required)) ? TypePropertyFlags.Required : TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: functions);

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse(route.Type.FormatType("radius.dev/Application", "Route", Version)),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        private static ResourceType MakeScopeRouteType(IResourceTypeProvider provider, KnownScopes.ScopeData scope, CommonBindings.BindingData route)
        {
            var functions = new List<FunctionOverload>();
            functions.AddRange(route.Values.Select(v => MakeFunction(v)));

            var propertiesType = new ObjectType(
                name: "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: route.Properties,
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var segment = scope.Type.Namespace == null ? scope.Type.Type : $"{scope.Type.Namespace}.{scope.Type.Type}";
            var bodyType = new ObjectType(
                route.Type.FormatType($"radius.dev/Application/{segment}Scope", "Route", Version),
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new []
                {
                    CommonProperties.Id,
                    CommonProperties.Name,
                    CommonProperties.Type,
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    new TypeProperty("properties", propertiesType, propertiesType.Properties.Any(p => p.Value.Flags.HasFlag(TypePropertyFlags.Required)) ? TypePropertyFlags.Required : TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: functions);

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse(route.Type.FormatType($"radius.dev/Application/{segment}Scope", "Route", Version)),
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
                name: scope.Type.FormatType("radius.dev/Application", "Scope", Version),
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
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: Array.Empty<FunctionOverload>());

            return new ResourceType(
                typeReference: ResourceTypeReference.Parse(scope.Type.FormatType("radius.dev/Application", "Scope", Version)),
                validParentScopes: ResourceScope.ResourceGroup,
                body: bodyType,
                provider: provider);
        }

        private static TypeProperty MakeBindingsProperty(Dictionary<string, CommonBindings.BindingData> builtIn)
        {
            var properties = builtIn?.Select(kvp =>
            {
                var functions = new List<FunctionOverload>();
                functions.AddRange(kvp.Value.Values.Select(v => MakeFunction(v)));

                var bindingType = new ObjectType(
                    name: $"binding properties: {kvp.Value.Type.FormatKind()}",
                    validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                    properties: new []
                    {
                        new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.ReadOnly),
                    },
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

        private static ObjectType BindingValue = new ObjectType(
            name: "binding value",
            validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
            properties: new []
            {
                new TypeProperty("id", LanguageConstants.String, TypePropertyFlags.Required, "The id of the binding"),
                new TypeProperty("valueName", LanguageConstants.String, TypePropertyFlags.Required, "The name of the value to bind"),
                new TypeProperty("format", LanguageConstants.String, TypePropertyFlags.None, "An optional format"),
            },
            additionalPropertiesType: null,
            additionalPropertiesFlags: TypePropertyFlags.None,
            functions: new []
            {
                new FunctionOverloadBuilder("currentValue")
                    .WithDescription("Gets the current value of the binding")
                    .WithFlags(FunctionFlags.RequiresInlining)
                    .WithReturnType(LanguageConstants.SecureString)
                    .Build(),
            });

        private static FunctionOverload MakeFunction(string value)
        {
            return new FunctionOverloadBuilder(value)
                .WithDescription($"Creates a binding to the {value} value")
                .WithEvaluator(EvaluateBinding)
                .WithFlags(FunctionFlags.RequiresInlining)
                .WithReturnType(BindingValue)
                .Build();
        }

        private static SyntaxBase EvaluateBinding(FunctionCallSyntaxBase functionCall, Symbol symbol, TypeSymbol typeSymbol)
        {
            throw null!;
        }
    }
}
