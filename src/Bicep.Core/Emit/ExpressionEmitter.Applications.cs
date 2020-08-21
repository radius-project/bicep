// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Semantics;

namespace Bicep.Core.Emit
{
    public partial class ExpressionEmitter
    {
        public void EmitResourceIdReference(ApplicationSymbol applicationSymbol)
        {
            var resourceIdExpression = converter.GetResourceIdExpression(applicationSymbol);
            var serialized = ExpressionSerializer.SerializeExpression(resourceIdExpression);

            writer.WriteValue(serialized);
        }

        public void EmitResourceIdReference(ComponentSymbol componentSymbol)
        {
            var resourceIdExpression = converter.GetResourceIdExpression(componentSymbol);
            var serialized = ExpressionSerializer.SerializeExpression(resourceIdExpression);

            writer.WriteValue(serialized);
        }

        public void EmitResourceIdReference(DeploymentSymbol deploymentSymbol)
        {
            var resourceIdExpression = converter.GetResourceIdExpression(deploymentSymbol);
            var serialized = ExpressionSerializer.SerializeExpression(resourceIdExpression);

            writer.WriteValue(serialized);
        }

        public void EmitResourceName(CompoundName name)
        {
            var resourceNameExpression = converter.GetResourceNameExpression(name);
            var serialized = ExpressionSerializer.SerializeExpression(resourceNameExpression);

            writer.WriteValue(serialized);
        }

        public void EmitResourceName(ApplicationSymbol applicationSymbol)
        {
            var resourceIdExpression = converter.GetResourceNameExpression(applicationSymbol);
            var serialized = ExpressionSerializer.SerializeExpression(resourceIdExpression);

            writer.WriteValue(serialized);
        }

        public void EmitResourceName(ComponentSymbol componentSymbol)
        {
            var resourceIdExpression = converter.GetResourceNameExpression(componentSymbol);
            var serialized = ExpressionSerializer.SerializeExpression(resourceIdExpression);

            writer.WriteValue(serialized);
        }

        public void EmitResourceName(DeploymentSymbol deploymentSymbol)
        {
            var resourceIdExpression = converter.GetResourceNameExpression(deploymentSymbol);
            var serialized = ExpressionSerializer.SerializeExpression(resourceIdExpression);

            writer.WriteValue(serialized);
        }

        public void EmitResourceIdReferences(IEnumerable<ResourceDependency> resources)
        {
            // need to put dependencies in a deterministic order to generate a deterministic template

            // TODO - ordering isn't deterministic
            foreach (var resourceId in resources.Select(d => GetResourceId(d)).OrderBy(x => x))
            {
                writer.WriteValue(resourceId);
            }

            string GetResourceId(ResourceDependency dependency)
            {
                if (dependency.Reference is ResourceReference reference)
                {
                    var resourceIdExpression = converter.GetResourceIdExpression(reference);
                    var serialized = ExpressionSerializer.SerializeExpression(resourceIdExpression);
                    return serialized;
                }
                else if (dependency.Symbol is DeclaredSymbol symbol)
                {
                    var resourceIdExpression = converter.GetResourceIdExpression(symbol);
                    var serialized = ExpressionSerializer.SerializeExpression(resourceIdExpression);
                    return serialized;
                }

                throw new InvalidOperationException("Unreachable.");
            }
        }
    }
}