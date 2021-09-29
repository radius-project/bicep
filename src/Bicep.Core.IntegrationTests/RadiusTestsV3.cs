// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bicep.Core.Emit;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;
using Bicep.Core.TypeSystem.Radius;
using Bicep.Core.UnitTests;
using Bicep.Core.UnitTests.Assertions;
using Bicep.Core.UnitTests.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using static Bicep.Core.UnitTests.Utils.CompilationHelper;

namespace Bicep.Core.IntegrationTests
{
    [TestClass]
    public class RadiusTestsV3
    {
        [TestMethod]
        public void Application_with_components_can_be_compiled()
        {
            var context = new CompilationHelperContext(new RadiusTypeProvider(), BicepTestConstants.Features);
            var (template, diagnostics, compilation) = Compile(context, @"
resource app 'radius.dev/Application@v1alpha3' = {
  name: 'app'

  resource frontend 'ContainerComponent' = {
    name: 'frontend'
    properties: {
      container: {
        image: 'rynowak/frontend:latest'
      }
    }
  }

  resource backend 'ContainerComponent' = {
    name: 'backend'
    properties: {
      container: {
        image: 'rynowak/backend:latest'
      }
    }
  }
}
");

            var model = compilation.GetEntrypointSemanticModel();
            model.GetAllDiagnostics().Should().BeEmpty();

            var applicationSymbol = model.AllResources.Should().ContainSingle(r => r.Symbol.Name == "app").Subject.Symbol;
            var applicationType = applicationSymbol.TryGetResourceTypeReference();
            applicationType.Should().NotBeNull();
            applicationType.Should().BeEquivalentTo(ResourceTypeReference.Parse("radius.dev/Application@v1alpha3"));

            var frontendSymbol = model.AllResources.Should().ContainSingle(r => r.Symbol.Name == "frontend").Subject.Symbol;
            var frontendType = frontendSymbol.TryGetResourceTypeReference();
            frontendType.Should().NotBeNull();
            frontendType.Should().BeEquivalentTo(ResourceTypeReference.Parse("radius.dev/Application/ContainerComponent@v1alpha3"));

            var backendSymbol = model.AllResources.Should().ContainSingle(r => r.Symbol.Name == "backend").Subject.Symbol;
            var backendType = backendSymbol.TryGetResourceTypeReference();
            backendType.Should().NotBeNull();
            backendType.Should().BeEquivalentTo(ResourceTypeReference.Parse("radius.dev/Application/ContainerComponent@v1alpha3"));

            template.Should().HaveValueAtPath("$.resources[0]", new JObject()
            {
                ["type"] = new JValue("Microsoft.CustomProviders/resourceProviders/Application/ContainerComponent"),
                ["apiVersion"] = new JValue("2018-09-01-preview"),
                ["name"] = new JValue("[format('{0}/{1}/{2}', 'radiusv3', 'app', 'frontend')]"),
                ["properties"] = new JObject()
                {
                    ["container"] = new JObject()
                    {
                        ["image"] = new JValue("rynowak/frontend:latest")
                    }
                },
                ["dependsOn"] = new JArray()
                {
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application', 'radiusv3', 'app')]")
                },
            });

            template.Should().HaveValueAtPath("$.resources[1]", new JObject()
            {
                ["type"] = new JValue("Microsoft.CustomProviders/resourceProviders/Application/ContainerComponent"),
                ["apiVersion"] = new JValue("2018-09-01-preview"),
                ["name"] = new JValue("[format('{0}/{1}/{2}', 'radiusv3', 'app', 'backend')]"),
                ["properties"] = new JObject()
                {
                    ["container"] = new JObject()
                    {
                        ["image"] = new JValue("rynowak/backend:latest")
                    }
                },
                ["dependsOn"] = new JArray()
                {
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application', 'radiusv3', 'app')]")
                },
            });

            template.Should().HaveValueAtPath("$.resources[2]", new JObject()
            {
                ["type"] = new JValue("Microsoft.CustomProviders/resourceProviders/Application"),
                ["apiVersion"] = new JValue("2018-09-01-preview"),
                ["name"] = new JValue("[format('{0}/{1}', 'radiusv3', 'app')]"),
            });
        }

        [TestMethod]
        public void Programmatic_secrets_work()
        {
            var context = new CompilationHelperContext(new RadiusTypeProvider(), BicepTestConstants.Features);
            var (template, diagnostics, compilation) = Compile(context, @"
resource app 'radius.dev/Application@v1alpha3' = {
  name: 'app'

  resource db 'mongodb.com.MongoComponent' = {
    name: 'db'
    properties: {
      managed: true
    }
  }

  resource backend 'ContainerComponent' = {
    name: 'backend'
    properties: {
      container: {
        image: 'rynowak/backend:latest'
        env: {
          CONNECTION_STRING: db.connectionString()
        }
      }
    }
  }
}

output connectionString string = app::db.connectionString()
");

            var model = compilation.GetEntrypointSemanticModel();
            model.GetAllDiagnostics().Should().BeEmpty();

            var applicationSymbol = model.AllResources.Should().ContainSingle(r => r.Symbol.Name == "app").Subject.Symbol;
            var applicationType = applicationSymbol.TryGetResourceTypeReference();
            applicationType.Should().NotBeNull();
            applicationType.Should().BeEquivalentTo(ResourceTypeReference.Parse("radius.dev/Application@v1alpha3"));

            template.Should().HaveValueAtPath("$.resources[1]", new JObject()
            {
                ["type"] = new JValue("Microsoft.CustomProviders/resourceProviders/Application/ContainerComponent"),
                ["apiVersion"] = new JValue("2018-09-01-preview"),
                ["name"] = new JValue("[format('{0}/{1}/{2}', 'radiusv3', 'app', 'backend')]"),
                ["properties"] = new JObject()
                {
                    ["container"] = new JObject()
                    {
                        ["image"] = new JValue("rynowak/backend:latest"),
                        ["env"] = new JObject()
                        {
                            ["CONNECTION_STRING"] = new JValue("[listSecrets(resourceId('Microsoft.CustomProviders/resourceProviders', 'radiusv3'), '2018-09-01-preview', createObject('targetId', resourceId('Microsoft.CustomProviders/resourceProviders/Application/mongodb.com.MongoComponent', 'radiusv3', 'app', 'db'))).connectionString]"),
                        }
                    }
                },
                ["dependsOn"] = new JArray()
                {
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application', 'radiusv3', 'app')]"),
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application/mongodb.com.MongoComponent', 'radiusv3', 'app', 'db')]"),
                },
            });

            template.Should().HaveValueAtPath("$.resources[2]", new JObject()
            {
                ["type"] = new JValue("Microsoft.CustomProviders/resourceProviders/Application"),
                ["apiVersion"] = new JValue("2018-09-01-preview"),
                ["name"] = new JValue("[format('{0}/{1}', 'radiusv3', 'app')]"),
            });

            template.Should().HaveValueAtPath("$.outputs.connectionString", new JObject()
            {
                ["type"] = new JValue("string"),
                ["value"] = new JValue("[listSecrets(resourceId('Microsoft.CustomProviders/resourceProviders', 'radiusv3'), '2018-09-01-preview', createObject('targetId', resourceId('Microsoft.CustomProviders/resourceProviders/Application/mongodb.com.MongoComponent', 'radiusv3', 'app', 'db'))).connectionString]"),
            });
        }
    }
}
