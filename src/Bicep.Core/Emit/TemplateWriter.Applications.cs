// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Azure.Deployments.Expression.Expressions;
using Bicep.Core.Extensions;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using Bicep.Core.TypeSystem;
using Newtonsoft.Json;

namespace Bicep.Core.Emit
{
    public partial class TemplateWriter
    {
        private void EmitResource(ProjectedResource resource)
        {
            // Write the application
            writer.WriteStartObject();

            this.emitter.EmitProperty("type", resource.ResourceType.FullyQualifiedType);
            this.emitter.EmitProperty("apiVersion", resource.ResourceType.ApiVersion);

            // rewrite the name property - it needs to be prefixed with "oam/"
            this.emitter.EmitProperty("name", () => this.emitter.EmitResourceName(resource.Name));
            if (resource.Kind != null)
            {
                this.emitter.EmitProperty("kind", resource.Kind);
            }
            this.emitter.EmitObjectProperties(resource.Body, resource.PropertiesToOmit);

            // dependsOn is currently not allowed as a top-level resource property in bicep
            // we will need to revisit this and probably merge the two if we decide to allow it

            this.EmitDependsOn(resource);

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
    }
}