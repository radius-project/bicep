// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Immutable;
using Azure.Deployments.Expression.Expressions;
using Bicep.Core.Emit;
using Bicep.Core.Extensions;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.IR
{
    public static class ModelBuilder
    {
        public static TemplateModel Build(SemanticModel semanticModel)
        {
            var context = new EmitterContext(semanticModel);
            var builder = new Builder(semanticModel, context, new ExpressionConverter(context));

            var functions = ImmutableArray.CreateBuilder<FunctionModel>();
            var modules = ImmutableArray.CreateBuilder<ModuleModel>();
            var parameters = ImmutableArray.CreateBuilder<ParameterModel>();
            var outputs = ImmutableArray.CreateBuilder<OutputModel>();
            var resources = ImmutableArray.CreateBuilder<ResourceModel>();
            var variables = ImmutableArray.CreateBuilder<VariableModel>();

            foreach (var outputSymbol in semanticModel.Root.OutputDeclarations)
            {
                outputs.Add(builder.CreateOutput(outputSymbol));
            }

            foreach (var parameterSymbol in semanticModel.Root.ParameterDeclarations)
            {
                parameters.Add(builder.CreateParameter(parameterSymbol));
            }

            foreach (var variableSymbol in semanticModel.Root.VariableDeclarations)
            {
                if (context.VariablesToInline.Contains(variableSymbol))
                {
                    continue;
                }

                variables.Add(builder.CreateVariable(variableSymbol));
            }

            foreach (var resourceSymbol in semanticModel.Root.ResourceDeclarations)
            {

            }

            return new TemplateModel(
                semanticModel,
                functions.ToImmutable(),
                modules.ToImmutable(),
                outputs.ToImmutable(),
                parameters.ToImmutable(),
                resources.ToImmutable(),
                variables.ToImmutable());
        }

        private class Builder
        {
            private static readonly ImmutableArray<string> ParameterModifierPropertiesToEmitDirectly = new[]
            {
                "minValue",
                "maxValue",
                "minLength",
                "maxLength",
                "metadata"
            }.ToImmutableArray();

            private static ImmutableHashSet<string> ResourcePropertiesToOmit = new [] {
                LanguageConstants.ResourceScopePropertyName,
                LanguageConstants.ResourceDependsOnPropertyName,
            }.ToImmutableHashSet();

            private readonly EmitterContext context;
            private readonly ExpressionConverter converter;
            private readonly SemanticModel model;

            public Builder(SemanticModel model, EmitterContext context, ExpressionConverter converter)
            {
                this.model = model;
                this.context = context;
                this.converter = converter;
            }

            public ParameterModel CreateParameter(ParameterSymbol parameterSymbol)
            {
                // local function
                bool IsSecure(SyntaxBase? value) => value is BooleanLiteralSyntax boolLiteral && boolLiteral.Value;

                if (!(SyntaxHelper.TryGetPrimitiveType(parameterSymbol.DeclaringParameter) is TypeSymbol primitiveType))
                {
                    // this should have been caught by the type checker long ago
                    throw new ArgumentException($"Unable to find primitive type for parameter {parameterSymbol.Name}");
                }

                var properties = ImmutableArray.CreateBuilder<PropertyModel>();

                switch (parameterSymbol.Modifier)
                {
                    case null:
                    {
                        var type = GetParameterTemplateTypeName(primitiveType, secure: false);
                        properties.Add(new PropertyModel("type", new ValueJTokenModel(type)));
                        break;
                    }

                    case ParameterDefaultValueSyntax defaultValueSyntax:
                    {
                        var type = GetParameterTemplateTypeName(primitiveType, secure: false);
                        properties.Add(new PropertyModel("type", new ValueJTokenModel(type)));
                        properties.Add(new PropertyModel("defaultValue", new ValueSyntaxModel(defaultValueSyntax.DefaultValue)));
                        break;
                    }

                    case ObjectSyntax modifierSyntax:
                    {
                        // this would throw on duplicate properties in the object node - we are relying on emitter checking for errors at the beginning
                        var parameterProperties = modifierSyntax.ToKnownPropertyValueDictionary();

                        var type = GetParameterTemplateTypeName(primitiveType, IsSecure(parameterProperties.TryGetValue("secure")));
                        properties.Add(new PropertyModel("type", new ValueJTokenModel(type)));

                        // relying on validation here as well (not all of the properties are valid in all contexts)
                        foreach (string modifierPropertyName in ParameterModifierPropertiesToEmitDirectly)
                        {
                            if (parameterProperties.TryGetValue(modifierPropertyName) is SyntaxBase value)
                            {
                                properties.Add(new PropertyModel(modifierPropertyName, new ValueSyntaxModel(value)));
                            }
                        }

                        if (parameterProperties.TryGetValue(LanguageConstants.ParameterDefaultPropertyName) is SyntaxBase defaultValue)
                        {
                            properties.Add(new PropertyModel("defaultValue", new ValueSyntaxModel(defaultValue)));
                        }

                        if (parameterProperties.TryGetValue(LanguageConstants.ParameterAllowedPropertyName) is SyntaxBase allowedValues)
                        {
                            properties.Add(new PropertyModel("allowedValues", new ValueSyntaxModel(allowedValues)));
                        }

                        break;
                    }
                }

                return new ParameterModel(parameterSymbol, properties.ToImmutable());
            }

            private string GetParameterTemplateTypeName(TypeSymbol type, bool secure)
            {
                if (secure)
                {
                    if (ReferenceEquals(type, LanguageConstants.String))
                    {
                        return "secureString";
                    }

                    if (ReferenceEquals(type, LanguageConstants.Object))
                    {
                        return "secureObject";
                    }
                }

                return type.Name;
            }

            public VariableModel CreateVariable(VariableSymbol variableSymbol)
            {
                // TODO: When we have expressions, only expressions without runtime functions can be emitted this way. Everything else will need to be inlined.
                return new VariableModel(variableSymbol, new ValueSyntaxModel(variableSymbol.Value));
            }

            public OutputModel CreateOutput(OutputSymbol outputSymbol)
            {
                return new OutputModel(outputSymbol, new ValueSyntaxModel(outputSymbol.Value));
            }

            public ResourceModel CreateResource(ResourceSymbol resourceSymbol)
            {
                var conditions = ImmutableArray.CreateBuilder<ValueModel>();
                if (resourceSymbol.DeclaringResource.IfCondition is IfConditionSyntax ifCondition)
                {
                    conditions.Add(new ValueSyntaxModel(ifCondition.ConditionExpression));
                }

                LanguageExpression? scope = null;
                if (model.EmitLimitationInfo.ResoureScopeData[resourceSymbol] is ResourceSymbol scopeResource)
                {
                    scope = this.converter.GetUnqualifiedResourceId(scopeResource);
                }

                var properties = ImmutableArray.CreateBuilder<PropertyModel>();
                foreach (var propertySyntax in ((ObjectSyntax)resourceSymbol.DeclaringResource.Body).Properties)
                {
                    if (propertySyntax.TryGetKeyText() is string keyName)
                    {
                        if (ResourcePropertiesToOmit.Contains(keyName) == true)
                        {
                            continue;
                        }
                    }

                    properties.Add(new PropertyModel(propertySyntax.Key, new ValueSyntaxModel(propertySyntax.Value)));
                }

                var dependsOn = ImmutableArray.CreateBuilder<ValueModel>();
                foreach (var dependency in this.context.ResourceDependencies[resourceSymbol])
                {
                    var resourceId = this.converter.GetResourceIdExpression(dependency);
                    dependsOn.Add(new ValueExpressionModel(resourceId));
                }

                return new ResourceModel(
                    resourceSymbol,
                    EmitHelpers.GetTypeReference(resourceSymbol),
                    properties.ToImmutable(),
                    conditions.ToImmutable(),
                    dependsOn.ToImmutable(),
                    scope);
            }
        }
    }
}