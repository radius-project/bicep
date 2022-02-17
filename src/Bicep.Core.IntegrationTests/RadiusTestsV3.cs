// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Bicep.Core.Resources;
using Bicep.Core.Semantics.Metadata;
using Bicep.Core.UnitTests.Assertions;
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
            var context = new CompilationHelperContext();
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

            var applicationSymbol = model.AllResources.OfType<DeclaredResourceMetadata>().Should().ContainSingle(r => r.Symbol.Name == "app").Subject.Symbol;
            var applicationType = applicationSymbol.TryGetResourceTypeReference();
            applicationType.Should().NotBeNull();
            applicationType.Should().BeEquivalentTo(ResourceTypeReference.Parse("radius.dev/Application@v1alpha3"));

            var frontendSymbol = model.AllResources.OfType<DeclaredResourceMetadata>().Should().ContainSingle(r => r.Symbol.Name == "frontend").Subject.Symbol;
            var frontendType = frontendSymbol.TryGetResourceTypeReference();
            frontendType.Should().NotBeNull();
            frontendType.Should().BeEquivalentTo(ResourceTypeReference.Parse("radius.dev/Application/ContainerComponent@v1alpha3"));

            var backendSymbol = model.AllResources.OfType<DeclaredResourceMetadata>().Should().ContainSingle(r => r.Symbol.Name == "backend").Subject.Symbol;
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
        public void Dependency_on_component_from_component_can_work()
        {
            var context = new CompilationHelperContext();
            var (template, diagnostics, compilation) = Compile(context, @"
resource app 'radius.dev/Application@v1alpha3' = {
  name: 'app'

  resource route 'HttpRoute' = {
    name: 'route'
  }

  resource backend 'ContainerComponent' = {
    name: 'backend'
    properties: {
      container: {
        image: 'rynowak/backend:latest'
        ports: {
          web: {
            containerPort: 3000
            provides: route.id
          }
        }
      }
    }
  }
}
");

            var model = compilation.GetEntrypointSemanticModel();
            model.GetAllDiagnostics().Should().BeEmpty();

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
                        ["ports"] = new JObject()
                        {
                            ["web"] = new JObject()
                            {
                                ["containerPort"] = new JValue(3000),
                                ["provides"] = new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application/HttpRoute', 'radiusv3', 'app', 'route')]"),
                            },
                        },
                    },
                },
                ["dependsOn"] = new JArray()
                {
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application', 'radiusv3', 'app')]"),
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application/HttpRoute', 'radiusv3', 'app', 'route')]"),
                },
            });
        }

        [TestMethod]
        public void Dependency_on_component_can_work()
        {
            var context = new CompilationHelperContext();
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
}

resource storageAccounts 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: 'mystorage'
  location: resourceGroup().location
  kind: 'StorageV2'
  sku: {
    name: 'Premium_LRS'
  }
  properties: {
    customDomain: {
      name: app::frontend.properties.container.image
    }
  }
}
");

            var model = compilation.GetEntrypointSemanticModel();
            model.GetAllDiagnostics().Should().BeEmpty();

            template.Should().HaveValueAtPath("$.resources[2]", new JObject()
            {
                ["type"] = new JValue("Microsoft.Storage/storageAccounts"),
                ["apiVersion"] = new JValue("2021-02-01"),
                ["name"] = new JValue("mystorage"),
                ["location"] = new JValue("[resourceGroup().location]"),
                ["kind"] = new JValue("StorageV2"),
                ["sku"] = new JObject()
                {
                    ["name"] = new JValue("Premium_LRS"),
                },
                ["properties"] = new JObject()
                {
                    ["customDomain"] = new JObject()
                    {
                        ["name"] = new JValue("[reference(resourceId('Microsoft.CustomProviders/resourceProviders/Application/ContainerComponent', 'radiusv3', 'app', 'frontend')).container.image]"),
                    },
                },
                ["dependsOn"] = new JArray()
                {
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application/ContainerComponent', 'radiusv3', 'app', 'frontend')]"),
                },
            });
        }

        [TestMethod]
        public void Programmatic_secrets_work()
        {
            var context = new CompilationHelperContext();
            var (template, diagnostics, compilation) = Compile(context, @"
resource app 'radius.dev/Application@v1alpha3' = {
  name: 'app'

  resource db 'mongodb.com.MongoDBComponent' = {
    name: 'db'
    properties: {
      resource: 'test'
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

            var applicationSymbol = model.AllResources.OfType<DeclaredResourceMetadata>().Should().ContainSingle(r => r.Symbol.Name == "app").Subject.Symbol;
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
                            ["CONNECTION_STRING"] = new JValue("[listSecrets(resourceId('Microsoft.CustomProviders/resourceProviders', 'radiusv3'), '2018-09-01-preview', createObject('targetId', resourceId('Microsoft.CustomProviders/resourceProviders/Application/mongodb.com.MongoDBComponent', 'radiusv3', 'app', 'db'))).connectionString]"),
                        }
                    }
                },
                ["dependsOn"] = new JArray()
                {
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application', 'radiusv3', 'app')]"),
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application/mongodb.com.MongoDBComponent', 'radiusv3', 'app', 'db')]"),
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
                ["value"] = new JValue("[listSecrets(resourceId('Microsoft.CustomProviders/resourceProviders', 'radiusv3'), '2018-09-01-preview', createObject('targetId', resourceId('Microsoft.CustomProviders/resourceProviders/Application/mongodb.com.MongoDBComponent', 'radiusv3', 'app', 'db'))).connectionString]"),
            });
        }

        [TestMethod]
        public void Programmatic_secrets_work_with_parameter()
        {
            var context = new CompilationHelperContext();
            var (template, diagnostics, compilation) = Compile(context, @"
param db resource 'radius.dev/Application/mongodb.com.MongoDBComponent@v1alpha3'

resource app 'radius.dev/Application@v1alpha3' existing = {
  name: 'app'

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
");

            var model = compilation.GetEntrypointSemanticModel();
            model.GetAllDiagnostics().Should().BeEmpty();

            var applicationSymbol = model.AllResources.OfType<DeclaredResourceMetadata>().Should().ContainSingle(r => r.Symbol.Name == "app").Subject.Symbol;
            var applicationType = applicationSymbol.TryGetResourceTypeReference();
            applicationType.Should().NotBeNull();
            applicationType.Should().BeEquivalentTo(ResourceTypeReference.Parse("radius.dev/Application@v1alpha3"));

            template.Should().HaveValueAtPath("$.resources[0]", new JObject()
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
                            ["CONNECTION_STRING"] = new JValue("[listSecrets(resourceId('Microsoft.CustomProviders/resourceProviders', 'radiusv3'), '2018-09-01-preview', createObject('targetId', parameters('db'))).connectionString]"),
                        }
                    }
                },
                ["dependsOn"] = new JArray()
                {
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application', 'radiusv3', 'app')]"),
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application/mongodb.com.MongoDBComponent', 'radiusv3', 'app', 'db')]"),
                },
            });
        }

        [TestMethod]
        public void Application_with_executable_can_be_compiled()
        {
            var context = new CompilationHelperContext();
            var (template, diagnostics, compilation) = Compile(context, @"
resource app 'radius.dev/Application@v1alpha3' = {
  name: 'radapp1'

  resource service 'ExecutableComponent' = {
    name: 'frontend'

    properties: {
        executable: 'npx'
        args: [
          'ts-node'
          'index.ts'
        ]
        workingDirectory: '/path/to/app'
    }
  }
}
");
            var model = compilation.GetEntrypointSemanticModel();
            model.GetAllDiagnostics().Should().BeEmpty();

            var applicationSymbol = model.AllResources.OfType<DeclaredResourceMetadata>().Should().ContainSingle(r => r.Symbol.Name == "app").Subject.Symbol;
            var applicationType = applicationSymbol.TryGetResourceTypeReference();
            applicationType.Should().NotBeNull();
            applicationType.Should().BeEquivalentTo(ResourceTypeReference.Parse("radius.dev/Application@v1alpha3"));

            var frontendSymbol = model.AllResources.OfType<DeclaredResourceMetadata>().Should().ContainSingle(r => r.Symbol.Name == "service").Subject.Symbol;
            var frontendType = frontendSymbol.TryGetResourceTypeReference();
            frontendType.Should().NotBeNull();
            frontendType.Should().BeEquivalentTo(ResourceTypeReference.Parse("radius.dev/Application/ExecutableComponent@v1alpha3"));

            template.Should().HaveValueAtPath("$.resources[0]", new JObject()
            {
                ["type"] = new JValue("Microsoft.CustomProviders/resourceProviders/Application/ExecutableComponent"),
                ["apiVersion"] = new JValue("2018-09-01-preview"),
                ["name"] = new JValue("[format('{0}/{1}/{2}', 'radiusv3', 'radapp1', 'frontend')]"),
                ["properties"] = new JObject()
                {
                    ["executable"] = new JValue("npx"),
                    ["args"] = new JArray()
                    {
                        new JValue("ts-node"),
                        new JValue("index.ts")
                    },
                    ["workingDirectory"] = new JValue("/path/to/app")
                },
                ["dependsOn"] = new JArray()
                {
                    new JValue("[resourceId('Microsoft.CustomProviders/resourceProviders/Application', 'radiusv3', 'radapp1')]")
                }
            });
        }
    }
}
