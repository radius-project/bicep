// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Azure.Deployments.Expression.Expressions;
using Bicep.Core.IR;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using Bicep.Core.TypeSystem;
using Newtonsoft.Json;

namespace Bicep.Core.Emit
{
    // TODO: Are there discrepancies between parameter, variable, and output names between bicep and ARM?
    public partial class TemplateWriter
    {
        public const string NestedDeploymentResourceType = "Microsoft.Resources/deployments";
        public const string NestedDeploymentResourceApiVersion = "2019-10-01";

        private static ImmutableHashSet<string> ResourcePropertiesToOmit = new [] {
            LanguageConstants.ResourceScopePropertyName,
            LanguageConstants.ResourceDependsOnPropertyName,
        }.ToImmutableHashSet();

        private static ImmutableHashSet<string> ModulePropertiesToOmit = new [] {
            LanguageConstants.ModuleParamsPropertyName,
            LanguageConstants.ResourceScopePropertyName,
            LanguageConstants.ResourceDependsOnPropertyName,
        }.ToImmutableHashSet();

        private static SemanticModel GetModuleSemanticModel(ModuleSymbol moduleSymbol)
        {
            if (!moduleSymbol.TryGetSemanticModel(out var moduleSemanticModel, out _))
            {
                // this should have already been checked during type assignment
                throw new InvalidOperationException($"Unable to find referenced compilation for module {moduleSymbol.Name}");
            }
            
            return moduleSemanticModel;
        }

        private readonly JsonTextWriter writer;
        private readonly EmitterContext context;
        private readonly ExpressionEmitter emitter;
        private readonly TemplateModel model;

        public TemplateWriter(JsonTextWriter writer, TemplateModel model)
        {
            this.writer = writer;
            this.model = model;
            this.context = new EmitterContext(model.SemanticModel);
            this.emitter = new ExpressionEmitter(writer, context);
        }

        public void Write()
        {
            writer.WriteStartObject();

            this.emitter.EmitProperty("$schema", this.model.Schema);

            this.emitter.EmitProperty("contentVersion", this.model.ContentVersion);

            this.EmitParametersIfPresent();

            writer.WritePropertyName("functions");
            writer.WriteStartArray();
            foreach (var function in this.model.Functions)
            {
                throw new NotImplementedException("No support for functions yet");
            }
            writer.WriteEndArray();

            this.EmitVariablesIfPresent();

            this.EmitResources();

            this.EmitOutputsIfPresent();

            writer.WriteEndObject();
        }

        private void EmitParametersIfPresent()
        {
            if (this.model.Parameters.Length == 0)
            {
                return;
            }

            writer.WritePropertyName("parameters");
            writer.WriteStartObject();

            foreach (var parameterModel in this.model.Parameters)
            {
                writer.WritePropertyName(parameterModel.Name);
                this.EmitParameter(parameterModel);
            }

            writer.WriteEndObject();
        }

        private void EmitParameter(ParameterModel parameterModel)
        {
            writer.WriteStartObject();

            foreach (var property in parameterModel.Properties)
            {
                this.emitter.EmitProperty(property);
            }

            writer.WriteEndObject();
        }

        private void EmitVariablesIfPresent()
        {
            if (this.model.Variables.Length == 0)
            {
                return;
            }

            writer.WritePropertyName("variables");
            writer.WriteStartObject();

            foreach (var variableModel in this.model.Variables)
            {
                writer.WritePropertyName(variableModel.Name);
                this.EmitVariable(variableModel);
            }

            writer.WriteEndObject();
        }

        private void EmitVariable(VariableModel variableModel)
        {
            this.emitter.EmitExpression(variableModel.Value);
        }

        private void EmitResources()
        {
            writer.WritePropertyName("resources");
            writer.WriteStartArray();

            foreach (var resourceSymbol in this.context.SemanticModel.Root.ResourceDeclarations)
            {
                this.EmitResource(resourceSymbol);
            }

            foreach (var applicationResource in this.context.ProjectedResources)
            {
                this.EmitResource(applicationResource);
            }

            foreach (var moduleSymbol in this.context.SemanticModel.Root.ModuleDeclarations)
            {
                this.EmitModule(moduleSymbol);
            }

            writer.WriteEndArray();
        }

        private void EmitResource(ResourceModel resourceModel)
        {
            writer.WriteStartObject();

            if (resourceModel.Conditions.Length == 1)
            {
                this.emitter.EmitProperty("condition", resourceModel.Conditions[0]);
            }

            this.emitter.EmitProperty("type", resourceModel.Type);
            this.emitter.EmitProperty("apiVersion", resourceModel.ApiVersion);

            if (resourceModel.Scope is LanguageExpression scope)
            {
                this.emitter.EmitProperty("scope", scope);
            }

            foreach (var property in resourceModel.Properties)
            {
                this.emitter.EmitProperty(property);
            }

            // dependsOn is currently not allowed as a top-level resource property in bicep
            // we will need to revisit this and probably merge the two if we decide to allow it
            if (resourceModel.DependsOn.Length > 0)
            {
                writer.WritePropertyName("dependsOn");
                writer.WriteStartArray();

                foreach (var dependency in resourceModel.DependsOn)
                {
                    this.emitter.EmitExpression(dependency);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }

        private void EmitResource(ResourceSymbol resourceSymbol)
        {
            writer.WriteStartObject();

            var typeReference = EmitHelpers.GetTypeReference(resourceSymbol);
            if (resourceSymbol.DeclaringResource.IfCondition is IfConditionSyntax ifCondition)
            {
                this.emitter.EmitProperty("condition", ifCondition.ConditionExpression);
            }

            this.emitter.EmitProperty("type", typeReference.FullyQualifiedType);
            this.emitter.EmitProperty("apiVersion", typeReference.ApiVersion);
            if (context.SemanticModel.EmitLimitationInfo.ResoureScopeData[resourceSymbol] is ResourceSymbol scopeResource)
            {
                this.emitter.EmitProperty("scope", () => this.emitter.EmitUnqualifiedResourceId(scopeResource));
            }
            this.emitter.EmitObjectProperties((ObjectSyntax)resourceSymbol.DeclaringResource.Body, ResourcePropertiesToOmit);

            // dependsOn is currently not allowed as a top-level resource property in bicep
            // we will need to revisit this and probably merge the two if we decide to allow it
            this.EmitDependsOn(resourceSymbol);

            writer.WriteEndObject();
        }

        private void EmitModuleParameters(ModuleSymbol moduleSymbol)
        {
            var paramsValue = moduleSymbol.SafeGetBodyPropertyValue(LanguageConstants.ModuleParamsPropertyName);
            if (paramsValue is not ObjectSyntax paramsObjectSyntax)
            {
                // 'params' is optional if the module has no required params
                return;
            }

            writer.WritePropertyName("parameters");

            writer.WriteStartObject();

            foreach (var propertySyntax in paramsObjectSyntax.Properties)
            {
                if (!(propertySyntax.TryGetKeyText() is string keyName))
                {
                    // should have been caught by earlier validation
                    throw new ArgumentException("Disallowed interpolation in module parameter");
                }

                writer.WritePropertyName(keyName);
                {
                    writer.WriteStartObject();
                    this.emitter.EmitProperty("value", propertySyntax.Value);
                    writer.WriteEndObject();
                }
            }

            writer.WriteEndObject();
        }

        private void EmitModule(ModuleSymbol moduleSymbol)
        {
            writer.WriteStartObject();

            if (moduleSymbol.DeclaringModule.IfCondition is IfConditionSyntax ifCondition)
            {
                this.emitter.EmitProperty("condition", ifCondition.ConditionExpression);
            }

            this.emitter.EmitProperty("type", NestedDeploymentResourceType);
            this.emitter.EmitProperty("apiVersion", NestedDeploymentResourceApiVersion);

            // emit all properties apart from 'params'. In practice, this currrently only allows 'name', but we may choose to allow other top-level resource properties in future.
            // params requires special handling (see below).
            this.emitter.EmitObjectProperties((ObjectSyntax)moduleSymbol.DeclaringModule.Body, ModulePropertiesToOmit);


            var scopeData = context.ModuleScopeData[moduleSymbol];
            ScopeHelper.EmitModuleScopeProperties(context.SemanticModel.TargetScope, scopeData, emitter);

            if (scopeData.RequestedScope != ResourceScopeType.ResourceGroupScope)
            {
                // if we're deploying to a scope other than resource group, we need to supply a location
                if (this.context.SemanticModel.TargetScope == ResourceScopeType.ResourceGroupScope)
                {
                    // the deployment() object at resource group scope does not contain a property named 'location', so we have to use resourceGroup().location
                    this.emitter.EmitProperty("location", new FunctionExpression(
                        "resourceGroup",
                        new LanguageExpression[] { },
                        new LanguageExpression[] { new JTokenExpression("location") }));
                }
                else
                {
                    // at all other scopes we can just use deployment().location
                    this.emitter.EmitProperty("location", new FunctionExpression(
                        "deployment",
                        new LanguageExpression[] { },
                        new LanguageExpression[] { new JTokenExpression("location") }));
                }
            }

            writer.WritePropertyName("properties");
            {
                writer.WriteStartObject();

                writer.WritePropertyName("expressionEvaluationOptions");
                {
                    writer.WriteStartObject();
                    this.emitter.EmitProperty("scope", "inner");
                    writer.WriteEndObject();
                }

                this.emitter.EmitProperty("mode", "Incremental");

                EmitModuleParameters(moduleSymbol);

                writer.WritePropertyName("template");
                {
                    var moduleSemanticModel = GetModuleSemanticModel(moduleSymbol);
                    var moduleWriter = new TemplateWriter(writer, ModelBuilder.Build(moduleSemanticModel));
                    moduleWriter.Write();
                }

                writer.WriteEndObject();
            }

            this.EmitDependsOn(moduleSymbol);

            writer.WriteEndObject();
        }

        private void EmitDependsOn(ProjectedResource resource)
        {
            var dependencies = new List<ResourceDependency>();
            dependencies.AddRange(resource.ImplicitDependencies.Select(d => new ResourceDependency(d)));

            if(resource.Declaration is DeclaredSymbol symbol)
            {
                dependencies.AddRange(context.ResourceDependencies[symbol].Select(d => new ResourceDependency(d)));
            }

            if (!dependencies.Any())
            {
                return;
            }

            writer.WritePropertyName("dependsOn");
            writer.WriteStartArray();
            emitter.EmitResourceIdReferences(dependencies);
            writer.WriteEndArray();
        }

        private void EmitDependsOn(DeclaredSymbol declaredSymbol)
        {
            var dependencies = context.ResourceDependencies[declaredSymbol];
            if (!dependencies.Any())
            {
                return;
            }

            writer.WritePropertyName("dependsOn");
            writer.WriteStartArray();
            // need to put dependencies in a deterministic order to generate a deterministic template
            foreach (var dependency in dependencies.OrderBy(x => x.Name))
            {
                switch (dependency)
                {
                    case ResourceSymbol resourceDependency:
                        emitter.EmitResourceIdReference(resourceDependency);
                        break;
                    case ApplicationSymbol applicationDependency:
                        emitter.EmitResourceIdReference(applicationDependency);
                        break;
                    case ComponentSymbol componentDependency:
                        emitter.EmitResourceIdReference(componentDependency);
                        break;
                    case DeploymentSymbol deploymentDependency:
                        emitter.EmitResourceIdReference(deploymentDependency);
                        break;
                    case ModuleSymbol moduleDependency:
                        emitter.EmitResourceIdReference(moduleDependency);
                        break;
                    default:
                        throw new InvalidOperationException($"Found dependency '{dependency.Name}' of unexpected type {dependency.GetType()}");
                }
            }
            writer.WriteEndArray();
        }

        private void EmitOutputsIfPresent()
        {
            if (this.model.Outputs.Length == 0)
            {
                return;
            }

            writer.WritePropertyName("outputs");
            writer.WriteStartObject();

            foreach (var outputModel in this.model.Outputs)
            {
                writer.WritePropertyName(outputModel.Name);
                this.EmitOutput(outputModel);
            }

            writer.WriteEndObject();
        }

        private void EmitOutput(OutputModel outputSymbol)
        {
            writer.WriteStartObject();

            this.emitter.EmitProperty("type", outputSymbol.Type);
            this.emitter.EmitProperty("value", outputSymbol.Value);

            writer.WriteEndObject();
        }
    }
}

