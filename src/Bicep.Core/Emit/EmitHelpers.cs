// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Emit
{
    public static class EmitHelpers
    {
        /// <summary>
        /// Gets the resource type reference from a resource symbol, assuming it has already been type-checked.
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <exception cref="ArgumentException">If the symbol is not for a valid resource type.</exception>
        public static ResourceTypeReference GetTypeReference(DeclaredSymbol symbol)
        {
            // TODO: come up with safety mechanism to ensure type checking has already occurred
            if (symbol.Type is ResourceType resourceType)
            {
                return resourceType.TypeReference;
            }

            if (symbol.Type is ApplicationType applicationType)
            {
                return applicationType.TypeReference;
            }

            if (symbol.Type is ComponentType componentType)
            {
                return ComponentType.ResourceType;
            }

            if (symbol.Type is DeploymentType deploymentType)
            {
                return deploymentType.TypeReference;
            }

            if (symbol.Type is InstanceType instanceType)
            {
                return ComponentType.ResourceType;
            }

            // throw here because the semantic model should be completely valid at this point
            // (it's a code defect if it some errors were not emitted)
            throw new ArgumentException($"Symbol does not have a valid resource type (found {symbol.Type.Name})");
        }
    }
}
