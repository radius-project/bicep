// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Diagnostics.CodeAnalysis;
using Bicep.Core.Diagnostics;
using Bicep.Core.IntegrationTests.Extensibility;
using Bicep.Core.UnitTests;
using Bicep.Core.UnitTests.Assertions;
using Bicep.Core.UnitTests.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Bicep.Core.IntegrationTests
{
    [TestClass]
    public class ExtensibilityTests
    {
        [NotNull]
        public TestContext? TestContext { get; set; }

        private CompilationHelper.CompilationHelperContext GetCompilationContext()
        {
            var features = BicepTestConstants.CreateFeaturesProvider(TestContext, importsEnabled: true);
            var resourceTypeLoader = BicepTestConstants.AzResourceTypeLoader;
            var namespaceProvider = new ExtensibilityNamespaceProvider(resourceTypeLoader, features);

            return new(
                AzResourceTypeLoader: resourceTypeLoader,
                Features: features,
                NamespaceProvider: namespaceProvider);
        }

        [TestMethod]
        public void Storage_import_bad_config_is_blocked()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import storage as stg {
  madeUpProperty: 'asdf'
}
");
            result.ExcludingLinterDiagnostics().Should().HaveDiagnostics(new[] {
                ("BCP035", DiagnosticLevel.Error, "The specified \"object\" declaration is missing the following required properties: \"connectionString\"."),
                ("BCP037", DiagnosticLevel.Error, "The property \"madeUpProperty\" is not allowed on objects of type \"configuration\". Permissible properties include \"connectionString\"."),
            });
        }

        [TestMethod]
        public void Storage_import_can_be_duplicated()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import storage as stg1 {
  connectionString: 'connectionString1'
}

import storage as stg2 {
  connectionString: 'connectionString2'
}
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
        }

        [TestMethod]
        public void Storage_import_basic_test()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import storage as stg {
  connectionString: 'asdf'
}

resource container 'container' = {
  name: 'myblob'
}

resource blob 'blob' = {
  name: 'myblob'
  containerName: container.name
  base64Content: base64('sadfasdfd')
}
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
        }

        [TestMethod]
        public void Storage_import_basic_test_loops_and_referencing()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import storage as stg {
  connectionString: 'asdf'
}

resource container 'container' = {
  name: 'myblob'
}

resource blobs 'blob' = [for i in range(0, 10): {
  name: 'myblob-${i}.txt'
  containerName: container.name
  base64Content: base64('Hello blob ${i}!')
}]

resource blobs2 'blob' = [for i in range(10, 10): {
  name: blobs[i - 10].name
  containerName: container.name
  base64Content: base64('Hello blob ${i}!')
}]

output sourceContainerName string = container.name
output sourceContainerNameSquare string = container['name']
output miscBlobContainerName string = blobs[13 % 10].containerName
output containerName string = blobs[5].containerName
output base64Content string = blobs[3]['base64Content']
");
            result.Should().NotHaveAnyDiagnostics();
            result.Template.Should().HaveValueAtPath("$.outputs['sourceContainerName'].value", "[reference('container').name]");
            result.Template.Should().HaveValueAtPath("$.outputs['sourceContainerNameSquare'].value", "[reference('container').name]");
            result.Template.Should().HaveValueAtPath("$.outputs['miscBlobContainerName'].value", "[reference(format('blobs[{0}]', mod(13, 10))).containerName]");
            result.Template.Should().HaveValueAtPath("$.outputs['containerName'].value", "[reference(format('blobs[{0}]', 5)).containerName]");
            result.Template.Should().HaveValueAtPath("$.outputs['base64Content'].value", "[reference(format('blobs[{0}]', 3)).base64Content]");
        }

        [TestMethod]
        public void Aad_import_basic_test_loops_and_referencing()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import aad as aad
param numApps int

resource myApp 'application' = {
  uniqueName: 'foo'
}

resource myAppsLoop 'application' = [for i in range(0, numApps): {
  uniqueName: '${myApp.appId}-bar-${i}'
}]

output myAppId string = myApp.appId
output myAppId2 string = myApp['appId']
output myAppsLoopId string = myAppsLoop[13 % numApps].appId
output myAppsLoopId2 string = myAppsLoop[3]['appId']
");
            result.Should().NotHaveAnyDiagnostics();
            result.Template.Should().HaveValueAtPath("$.outputs['myAppId'].value", "[reference('myApp').appId]");
            result.Template.Should().HaveValueAtPath("$.outputs['myAppId2'].value", "[reference('myApp').appId]");
            result.Template.Should().HaveValueAtPath("$.outputs['myAppsLoopId'].value", "[reference(format('myAppsLoop[{0}]', mod(13, parameters('numApps')))).appId]");
            result.Template.Should().HaveValueAtPath("$.outputs['myAppsLoopId2'].value", "[reference(format('myAppsLoop[{0}]', 3)).appId]");
        }

        [TestMethod]
        public void Aad_import_existing_requires_uniqueName()
        {
            // we've accidentally used 'name' even though this resource type doesn't support it
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import aad as aad

resource myApp 'application' existing = {
  name: 'foo'
}
");

            result.Should().NotGenerateATemplate();
            result.Should().HaveDiagnostics(new[] {
                ("BCP035", DiagnosticLevel.Error, "The specified \"resource\" declaration is missing the following required properties: \"uniqueName\"."),
                ("BCP037", DiagnosticLevel.Error, "The property \"name\" is not allowed on objects of type \"application\". Permissible properties include \"uniqueName\". If this is an inaccuracy in the documentation, please report it to the Bicep Team."),
            });

            // oops! let's change it to 'uniqueName'
            result = CompilationHelper.Compile(GetCompilationContext(), @"
import aad as aad

resource myApp 'application' existing = {
  uniqueName: 'foo'
}
");

            result.Should().GenerateATemplate();
            result.Should().NotHaveAnyDiagnostics();
        }

        [TestMethod]
        public void Kubernetes_import_existing_warns_with_readonly_fields()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import kubernetes as kubernetes {
  namespace: 'default'
  kubeConfig: ''
}
resource service 'core/Service@v1' existing = {
  metadata: {
    name: 'existing-service'
    namespace: 'default'
    labels: {
      format: 'k8s-extension'
    }
    annotations: {
      foo: 'bar'
    }
  }
}
");

            result.Should().GenerateATemplate();
            result.ExcludingLinterDiagnostics().Should().HaveDiagnostics(new[]
            {
                ("BCP073", DiagnosticLevel.Warning, "The property \"labels\" is read-only. Expressions cannot be assigned to read-only properties. If this is an inaccuracy in the documentation, please report it to the Bicep Team."),
                ("BCP073", DiagnosticLevel.Warning, "The property \"annotations\" is read-only. Expressions cannot be assigned to read-only properties. If this is an inaccuracy in the documentation, please report it to the Bicep Team."),
            });
        }

        [TestMethod]
        public void Kubernetes_import_existing_resources()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import kubernetes as kubernetes {
  namespace: 'default'
  kubeConfig: ''
}
resource service 'core/Service@v1' existing = {
  metadata: {
    name: 'existing-service'
    namespace: 'default'
  }
}
resource secret 'core/Secret@v1' existing = {
  metadata: {
    name: 'existing-secret'
    namespace: 'default'
  }
}
resource configmap 'core/ConfigMap@v1' existing = {
  metadata: {
    name: 'existing-configmap'
    namespace: 'default'
  }
}
");

            result.Should().GenerateATemplate();
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
        }

        [TestMethod]
        public void Kubernetes_import_existing_connectionstring_test()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import kubernetes as kubernetes {
  namespace: 'default'
  kubeConfig: ''
}
resource redisService 'core/Service@v1' existing = {
  metadata: {
    name: 'redis-service'
    namespace: 'default'
  }
}
resource redisSecret 'core/Secret@v1' existing = {
  metadata: {
    name: 'redis-secret'
    namespace: 'default'
  }
}
resource secret 'core/Secret@v1' = {
  metadata: {
    name: 'conn-secret'
    namespace: 'default'
    labels: {
      format: 'k8s-extension'
    }
  }
  stringData: {
    connectionString: '${redisService.metadata.name}.${redisService.metadata.namespace}.svc.cluster.local,password=${base64ToString(redisSecret.data.redisPassword)}'
  }
}
");

            result.Should().GenerateATemplate();
            result.Should().NotHaveAnyDiagnostics();
        }


        [TestMethod]
        public void Storage_import_basic_test_with_qualified_type()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import storage as stg {
  connectionString: 'asdf'
}

resource container 'stg:container' = {
  name: 'myblob'
}

resource blob 'stg:blob' = {
  name: 'myblob'
  containerName: container.name
  base64Content: base64('sadfasdfd')
}
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
        }

        [TestMethod]
        public void Invalid_namespace_qualifier_returns_error()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import storage as stg {
  connectionString: 'asdf'
}

resource container 'foo:container' = {
  name: 'myblob'
}

resource blob 'bar:blob' = {
  name: 'myblob'
  containerName: container.name
  base64Content: base64('sadfasdfd')
}
");

            result.ExcludingLinterDiagnostics().Should().HaveDiagnostics(new[] {
                ("BCP208", DiagnosticLevel.Error, "The specified namespace \"foo\" is not recognized. Specify a resource reference using one of the following namespaces: \"az\", \"stg\", \"sys\"."),
                ("BCP208", DiagnosticLevel.Error, "The specified namespace \"bar\" is not recognized. Specify a resource reference using one of the following namespaces: \"az\", \"stg\", \"sys\"."),
            });
        }

        [TestMethod]
        public void Radius_function_call_mongo()
        {
          var result = CompilationHelper.Compile(GetCompilationContext(), @"
import radius as radius

resource mongo 'Applications.Connector/mongoDatabases@2022-03-15-privatepreview' = {
  name: 'my-mongo'
  location: 'global'
  properties: {
    environment: 'test'
  }
}

resource container 'Applications.Core/containers@2022-03-15-privatepreview' = {
  name: 'mycontainer'
  location: 'global'
  properties: {
    application: 'myapp'
    connections: {
      redis: {
        source: 'foo'
      }
    }
    container: {
      image: 'test'
      env: {
        DBCONNECTION: mongo.connectionString()
        DBCONNECTION2: mongo.username()
        DBCONNECTION3: mongo.password()
      }
    }
  }
}
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
            var text = result.Template!.ToString();
            result.Template.Should().HaveValueAtPath("$.resources.container.properties.properties.container.env.DBCONNECTION", "[listSecrets('mongo', '2022-03-15-privatepreview').connectionString]");
            result.Template.Should().HaveValueAtPath("$.resources.container.properties.properties.container.env.DBCONNECTION2", "[listSecrets('mongo', '2022-03-15-privatepreview').username]");
            result.Template.Should().HaveValueAtPath("$.resources.container.properties.properties.container.env.DBCONNECTION3", "[listSecrets('mongo', '2022-03-15-privatepreview').password]");
        }

        [TestMethod]
        public void Radius_function_call_redis()
        {
          var result = CompilationHelper.Compile(GetCompilationContext(), @"
import radius as radius

resource redis 'Applications.Connector/redisCaches@2022-03-15-privatepreview' = {
  name: 'my-redis'
  location: 'global'
  properties: {
    environment: 'test'
  }
}

resource container 'Applications.Core/containers@2022-03-15-privatepreview' = {
  name: 'mycontainer'
  location: 'global'
  properties: {
    application: 'myapp'
    connections: {
      redis: {
        source: 'foo'
      }
    }
    container: {
      image: 'test'
      env: {
        DBCONNECTION: redis.connectionString()
        DBCONNECTION2: redis.password()
      }
    }
  }
}
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
            var text = result.Template!.ToString();
            result.Template.Should().HaveValueAtPath("$.resources.container.properties.properties.container.env.DBCONNECTION", "[listSecrets('redis', '2022-03-15-privatepreview').connectionString]");
            result.Template.Should().HaveValueAtPath("$.resources.container.properties.properties.container.env.DBCONNECTION2", "[listSecrets('redis', '2022-03-15-privatepreview').password]");
        }

        [TestMethod]
        public void Radius_function_call_rabbitmq()
        {
          var result = CompilationHelper.Compile(GetCompilationContext(), @"
import radius as radius

resource rabbitmq 'Applications.Connector/rabbitMQMessageQueues@2022-03-15-privatepreview' = {
  name: 'my-rabbitmq'
  location: 'global'
  properties: {
    environment: 'test'
    queue: 'my-queue'
  }
}

resource container 'Applications.Core/containers@2022-03-15-privatepreview' = {
  name: 'mycontainer'
  location: 'global'
  properties: {
    application: 'myapp'
    connections: {
      rabbitmq: {
        source: 'foo'
      }
    }
    container: {
      image: 'test'
      env: {
        DBCONNECTION: rabbitmq.connectionString()
      }
    }
  }
}
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
            var text = result.Template!.ToString();
            result.Template.Should().HaveValueAtPath("$.resources.container.properties.properties.container.env.DBCONNECTION", "[listSecrets('rabbitmq', '2022-03-15-privatepreview').connectionString]");
        }

        [TestMethod]
        public void Radius_function_call_extender()
        {
          var result = CompilationHelper.Compile(GetCompilationContext(), @"
import radius as radius

resource twilio 'Applications.Connector/extenders@2022-03-15-privatepreview' = {
  name: 'my-extender'
  location: 'global'
  properties: {
    environment: 'test'
    secrets: {
      accountSid: 'sid'
      authToken: 'token'
    }
  }
}

resource container 'Applications.Core/containers@2022-03-15-privatepreview' = {
  name: 'mycontainer'
  location: 'global'
  properties: {
    application: 'myapp'
    connections: {
      rabbitmq: {
        source: 'foo'
      }
    }
    container: {
      image: 'test'
      env: {
        'TWILIO_SID': twilio.secrets('accountSid')
        'TWILIO_ACCOUNT': twilio.secrets('authToken')
      }
    }
  }
}
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
            var text = result.Template!.ToString();
            result.Template.Should().HaveValueAtPath(
                "$.resources.container.properties.properties.container.env.TWILIO_SID",
                "[listSecrets('twilio', '2022-03-15-privatepreview').accountSid]");
            result.Template.Should().HaveValueAtPath(
                "$.resources.container.properties.properties.container.env.TWILIO_ACCOUNT",
                "[listSecrets('twilio', '2022-03-15-privatepreview').authToken]");

        }


        [TestMethod]
        public void Radius_azure_reference()
        {
          var result = CompilationHelper.Compile(GetCompilationContext(), @"
import radius as radius

resource mongo 'Applications.Connector/mongoDatabases@2022-03-15-privatepreview' = {
  name: 'my-mongo'
  location: 'global'
  properties: {
    environment: 'test'
  }
}

resource account 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: 'coolaccount'
  location: 'global'
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    customDomain: {
        name: mongo.connectionString()
    }
  }
}

output connectionString string = mongo.connectionString()
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
            var text = result.Template!.ToString();
            result.Template.Should().HaveValueAtPath(
                "$.resources.account.properties.customDomain.name",
                "[listSecrets('mongo', '2022-03-15-privatepreview').connectionString]");
        }

        [TestMethod]
        public void Kubernetes_Radius_Reference()
        {
          var result = CompilationHelper.Compile(GetCompilationContext(), @"
param kubeConfig string

import radius as radius
import kubernetes as kubernetes {
  kubeConfig: kubeConfig
  namespace: 'default'
}

resource mongo 'Applications.Connector/mongoDatabases@2022-03-15-privatepreview' = {
  name: 'my-mongo'
  location: 'global'
  properties: {
    environment: 'test'
  }
}

resource secret 'core/Secret@v1' = {
  metadata: {
    name: 'redis-conn'
    namespace: 'default'
    labels: {
      format: 'k8s-extension'
    }
  }

  stringData: {
    connectionString: '${mongo.connectionString()}'
  }
}

output connectionString string = mongo.connectionString()
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
            var text = result.Template!.ToString();
            result.Template.Should().HaveValueAtPath(
                "$.resources.secret.properties.stringData.connectionString",
                "[format('{0}', listSecrets('mongo', '2022-03-15-privatepreview').connectionString)]");
            result.Template.Should().HaveValueAtPath(
                "$.outputs.connectionString.value",
                "[listSecrets('mongo', '2022-03-15-privatepreview').connectionString]");
        }

        [TestMethod]
        public void Child_resource_with_parent_namespace_mismatch_returns_error()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(), @"
import storage as stg {
  connectionString: 'asdf'
}

resource parent 'az:Microsoft.Storage/storageAccounts@2020-01-01' existing = {
  name: 'stgParent'

  resource container 'stg:container' = {
    name: 'myblob'
  }
}
");

            result.ExcludingLinterDiagnostics().Should().HaveDiagnostics(new[] {
                ("BCP081", DiagnosticLevel.Warning, "Resource type \"Microsoft.Storage/storageAccounts@2020-01-01\" does not have types available."),
                ("BCP210", DiagnosticLevel.Error, "Resource type belonging to namespace \"stg\" cannot have a parent resource type belonging to different namespace \"az\"."),
            });
        }

        [TestMethod]
        [Ignore("This test has hardcoded hash values and is not reliable as a unit test.")]
        public void Storage_import_end_to_end_test()
        {
            var result = CompilationHelper.Compile(GetCompilationContext(),
                ("main.bicep", @"
param accountName string

resource stgAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: toLower(accountName)
  location: resourceGroup().location
  kind: 'Storage'
  sku: {
    name: 'Standard_LRS'
  }
}

var connectionString = 'DefaultEndpointsProtocol=https;AccountName=${stgAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${stgAccount.listKeys().keys[0].value}'

module website './website.bicep' = {
  name: 'website'
  params: {
    connectionString: connectionString
  }
}
"),
                ("website.bicep", @"
@secure()
param connectionString string

import storage as stg {
  connectionString: connectionString
}

resource container 'container' = {
  name: 'bicep'
}

resource blob 'blob' = {
  name: 'blob.txt'
  containerName: container.name
  base64Content: base64(loadTextContent('blob.txt'))
}
"),
                ("blob.txt", @"
Hello from Bicep!"));

            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
            result.Template.Should().DeepEqual(JToken.Parse(@"{
  ""$schema"": ""https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#"",
  ""languageVersion"": ""1.9-experimental"",
  ""contentVersion"": ""1.0.0.0"",
  ""metadata"": {
    ""_generator"": {
      ""name"": ""bicep"",
      ""version"": ""dev"",
      ""templateHash"": ""102018899127935696""
    }
  },
  ""parameters"": {
    ""accountName"": {
      ""type"": ""string""
    }
  },
  ""resources"": {
    ""stgAccount"": {
      ""type"": ""Microsoft.Storage/storageAccounts"",
      ""apiVersion"": ""2019-06-01"",
      ""name"": ""[toLower(parameters('accountName'))]"",
      ""location"": ""[resourceGroup().location]"",
      ""kind"": ""Storage"",
      ""sku"": {
        ""name"": ""Standard_LRS""
      }
    },
    ""website"": {
      ""type"": ""Microsoft.Resources/deployments"",
      ""apiVersion"": ""2020-10-01"",
      ""name"": ""website"",
      ""properties"": {
        ""expressionEvaluationOptions"": {
          ""scope"": ""inner""
        },
        ""mode"": ""Incremental"",
        ""parameters"": {
          ""connectionString"": {
            ""value"": ""[format('DefaultEndpointsProtocol=https;AccountName={0};EndpointSuffix={1};AccountKey={2}', resourceInfo('stgAccount').name, environment().suffixes.storage, listKeys(resourceId('Microsoft.Storage/storageAccounts', toLower(parameters('accountName'))), '2019-06-01').keys[0].value)]""
          }
        },
        ""template"": {
          ""$schema"": ""https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#"",
          ""languageVersion"": ""1.9-experimental"",
          ""contentVersion"": ""1.0.0.0"",
          ""metadata"": {
            ""_generator"": {
              ""name"": ""bicep"",
              ""version"": ""dev"",
              ""templateHash"": ""16652946131126277045""
            }
          },
          ""parameters"": {
            ""connectionString"": {
              ""type"": ""secureString""
            }
          },
          ""variables"": {
            ""$fxv#0"": ""\nHello from Bicep!""
          },
          ""imports"": {
            ""stg"": {
              ""provider"": ""AzureStorage"",
              ""version"": ""1.0"",
              ""config"": {
                ""connectionString"": ""[parameters('connectionString')]""
              }
            }
          },
          ""resources"": {
            ""container"": {
              ""import"": ""stg"",
              ""type"": ""container"",
              ""properties"": {
                ""name"": ""bicep""
              }
            },
            ""blob"": {
              ""import"": ""stg"",
              ""type"": ""blob"",
              ""properties"": {
                ""name"": ""blob.txt"",
                ""containerName"": ""[reference('container').name]"",
                ""base64Content"": ""[base64(variables('$fxv#0'))]""
              },
              ""dependsOn"": [
                ""container""
              ]
            }
          }
        }
      },
      ""dependsOn"": [
        ""stgAccount""
      ]
    }
  }
}"));
        }


        [TestMethod]
        public void Aws_types_can_compile()
        {
          var result = CompilationHelper.Compile(GetCompilationContext(), @"
import aws as aws

resource stream 'AWS.Kinesis/Stream@default' = {
  name: 'stream'
  properties: {
    ShardCount: 3
  }
}

output id string = stream.id
output arn string = stream.properties.Arn
");
            result.ExcludingLinterDiagnostics().Should().NotHaveAnyDiagnostics();
            var text = result.Template!.ToString();
            result.Template.Should().HaveValueAtPath("$.outputs.id.value", "[reference('stream').id]");
            result.Template.Should().HaveValueAtPath("$.outputs.arn.value", "[reference('stream').properties.Arn]");
        }
    }
}
