// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;
using Bicep.Core.Semantics;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.IR
{
    public class TemplateModel
    {
        public TemplateModel(
            SemanticModel semanticModel,
            ImmutableArray<FunctionModel> functions,
            ImmutableArray<ModuleModel> modules,
            ImmutableArray<OutputModel> outputs,
            ImmutableArray<ParameterModel> parameters,
            ImmutableArray<ResourceModel> resources,
            ImmutableArray<VariableModel> variables)
        {
            this.SemanticModel = semanticModel;
            this.Functions = functions;
            this.Modules = modules;
            this.Outputs = outputs;
            this.Parameters = parameters;
            this.Resources = resources;
            this.Variables = variables;
        }

        public SemanticModel SemanticModel { get; }

        public string ContentVersion => "1.0.0.0";

        public ImmutableArray<FunctionModel> Functions { get; }

        public ImmutableArray<ModuleModel> Modules { get; }

        public ImmutableArray<OutputModel> Outputs { get; }

        public ImmutableArray<ParameterModel> Parameters { get; }

        public ImmutableArray<ResourceModel> Resources { get; }

        public ImmutableArray<VariableModel> Variables { get; }

        public string Schema => GetSchema(this.SemanticModel.TargetScope);

        private static string GetSchema(ResourceScopeType targetScope)
        {
            if (targetScope.HasFlag(ResourceScopeType.TenantScope))
            {
                return "https://schema.management.azure.com/schemas/2019-08-01/tenantDeploymentTemplate.json#";
            }

            if (targetScope.HasFlag(ResourceScopeType.ManagementGroupScope))
            {
                return "https://schema.management.azure.com/schemas/2019-08-01/managementGroupDeploymentTemplate.json#";
            }

            if (targetScope.HasFlag(ResourceScopeType.SubscriptionScope))
            {
                return "https://schema.management.azure.com/schemas/2018-05-01/subscriptionDeploymentTemplate.json#";
            }

            return "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#";
        }
    }
}