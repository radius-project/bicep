// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;

namespace Bicep.Core.Emit
{
    public class DeploymentResource
    {
        public DeploymentResource(DeploymentSymbol declaration, CompoundName name)
        {
            Declaration = declaration;
            Body = (ObjectSyntax)declaration.DeclaringDeployment.Body;
            Name = name;

            ResourceType = EmitHelpers.GetTypeReference(declaration);
        }

        public DeploymentResource(ObjectSyntax body, ResourceTypeReference resourceType, CompoundName name)
        {
            Body = body;
            ResourceType = resourceType;
            Name = name;
        }

        public DeclaredSymbol? Declaration { get; }

        public ObjectSyntax Body { get; }

        public CompoundName Name { get; }

        public ResourceTypeReference ResourceType { get; }
    }
}