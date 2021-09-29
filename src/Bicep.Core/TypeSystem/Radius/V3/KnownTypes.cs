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
                KnownComponents.MakeDaprStateStore(),
                KnownComponents.MakeDaprPubSubTopic(),
                KnownComponents.MakeMongoDB(),
                KnownComponents.MakeCosmosDBMongo(),
                KnownComponents.MakeCosmosDBSQL(),
                KnownComponents.MakeKeyVault(),
                KnownComponents.MakeServiceBusQueue(),
                KnownComponents.MakeRedis(),
                KnownComponents.MakeVolume(),
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

            var functions = new List<FunctionOverload>();
            if (component.Binding is {})
            {
                properties.AddRange(component.Binding.Properties.Where(p => !properties.Any(pp => p.Name == pp.Name)));
                functions.AddRange(component.Binding.Values.Where(v => v.Secret).Select(v => MakeSecretAccessorFunction(v)));
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
                functions: functions);

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

        private static FunctionOverload MakeSecretAccessorFunction(CommonBindings.BindingValue value)
        {
            return new FunctionOverloadBuilder(value.Name)
                .WithDescription($"Provides access to the {value.Name} value.")
                .WithEvaluator((function, symbol, type) => EvaluateSecret(value, function, symbol, type))
                .WithFlags(FunctionFlags.RequiresInlining)
                .WithReturnType(value.Type.Type)
                .Build();
        }

        private static SyntaxBase EvaluateSecret(CommonBindings.BindingValue value, FunctionCallSyntaxBase functionCall, Symbol symbol, TypeSymbol typeSymbol)
        {
            // POST /subus...../resurceProviders/radiusV3/listSecrets
            // {
            //    targetId: ....
            // }
            //
            // A function like foo.connectionString() is replaced with code like:
            // listSecrets(resourceId('Microsoft.CustomProviders/resourceProviders', 'radiusv3'), '2018-09-01-preview', { 'targetID': resourceId(...) }).connectionString
            //
            // - The former resourceId is the ID of the CustomRP - this is a limitation we have to live with
            // - The latter resourceId is the ID of the Radius resource being accessed.

            var instance = (InstanceFunctionCallSyntax)functionCall;

            var customProviderResourceIdArgumentExpression = SyntaxFactory.CreateFunctionCall(
                "resourceId",
                SyntaxFactory.CreateStringLiteral(RadiusResources.ProviderCRPType),
                SyntaxFactory.CreateStringLiteral(RadiusResources.ProviderCRPName));


            var targetResourceIdExpression = SyntaxFactory.CreatePropertyAccess(instance.BaseExpression, "id");
            var customActionDataArgumentExpression = SyntaxFactory.CreateObject(new[]
            {
                SyntaxFactory.CreateObjectProperty("targetId", targetResourceIdExpression),
            });

            var functionCallExpression = SyntaxFactory.CreateFunctionCall(
                "listSecrets",
                customProviderResourceIdArgumentExpression,
                SyntaxFactory.CreateStringLiteral(RadiusResources.CRPApiVersion),
                customActionDataArgumentExpression);


            return SyntaxFactory.CreatePropertyAccess(functionCallExpression, value.Name);
        }
    }
}
