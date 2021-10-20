// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Diagnostics.CodeAnalysis;
using Bicep.Core.Resources;
using Bicep.Core.UnitTests;
using Bicep.Core.UnitTests.Assertions;
using Bicep.Core.UnitTests.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Bicep.Core.IntegrationTests
{
    [TestClass]
    public class KubernetesTests
    {
        [NotNull]
        public TestContext? TestContext { get; set; }

        private CompilationHelper.CompilationHelperContext GetCompilationContext()
        {
            var features = BicepTestConstants.CreateFeaturesProvider(TestContext, importsEnabled: true);
            return new(Features: features);
        }

        [TestMethod]
        public void ConfigMap_can_be_compiled()
        {
            var text = @"
import kubernetes from kubernetes

resource map 'kubernetes.core/ConfigMap@v1' = {
    metadata: {
        name: 'test-map'
    }
    data: {
        key: 'value'
    }
}
";

            var compilation = CompilationHelper.Compile(GetCompilationContext(), text);
            var model = compilation.Compilation.GetEntrypointSemanticModel();
            compilation.Diagnostics.Should().BeEmpty();

            var configMapSymbol = model.AllResources.Should().ContainSingle(r => r.Symbol.Name == "map").Subject.Symbol;
            configMapSymbol.TryGetResourceTypeReference().Should().BeEquivalentTo(ResourceTypeReference.Parse("kubernetes.core/ConfigMap@v1"));

            var template = compilation.Template!;

            var resources = template.SelectToken("resources").Should().BeAssignableTo<JArray>().Which.Should().HaveCount(1);
            template.Should().HaveValueAtPath("resources[0]", new JObject()
            {
                ["type"] = new JValue("kubernetes.core/ConfigMap@v1"),
                ["name"] = new JValue("map"),
                ["import"] = new JObject()
                {
                    ["provider"] = new JValue("kubernetes"),
                },
                ["properties"] = new JObject()
                {
                    ["metadata"] = new JObject()
                    {
                        ["name"] = new JValue("test-map")
                    },
                    ["data"] = new JObject()
                    {
                        ["key"] = new JValue("value")
                    }
                },
            });
        }

        [TestMethod]
        public void ConfigMap_can_be_referenced()
        {
            var text = @"
import kubernetes from kubernetes

resource map 'kubernetes.core/ConfigMap@v1' = {
    metadata: {
        name: 'test-map'
    }
    data: {
        key: 'value'
    }
}

resource secret 'kubernetes.core/Secret@v1' = {
    metadata: {
        name: 'test-secret'
    }
    type: 'Opaque'
    stringData: {
        key: map.data.key
    }
}
";

            var compilation = CompilationHelper.Compile(GetCompilationContext(), text);
            var model = compilation.Compilation.GetEntrypointSemanticModel();
            compilation.Diagnostics.Should().BeEmpty();

            var configMapSymbol = model.AllResources.Should().ContainSingle(r => r.Symbol.Name == "map").Subject.Symbol;
            configMapSymbol.TryGetResourceTypeReference().Should().BeEquivalentTo(ResourceTypeReference.Parse("kubernetes.core/ConfigMap@v1"));

            var secretSymbol = model.AllResources.Should().ContainSingle(r => r.Symbol.Name == "secret").Subject.Symbol;
            secretSymbol.TryGetResourceTypeReference().Should().BeEquivalentTo(ResourceTypeReference.Parse("kubernetes.core/Secret@v1"));

            var template = compilation.Template!;

            var resources = template.SelectToken("resources").Should().BeAssignableTo<JArray>().Which.Should().HaveCount(2);
            template.Should().HaveValueAtPath("resources[0]", new JObject()
            {
                ["type"] = new JValue("kubernetes.core/ConfigMap@v1"),
                ["name"] = new JValue("map"),
                ["import"] = new JObject()
                {
                    ["provider"] = new JValue("kubernetes"),
                },
                ["properties"] = new JObject()
                {
                    ["metadata"] = new JObject()
                    {
                        ["name"] = new JValue("test-map")
                    },
                    ["data"] = new JObject()
                    {
                        ["key"] = new JValue("value")
                    }
                },
            });
            template.Should().HaveValueAtPath("resources[1]", new JObject()
            {
                ["type"] = new JValue("kubernetes.core/Secret@v1"),
                ["name"] = new JValue("secret"),
                ["import"] = new JObject()
                {
                    ["provider"] = new JValue("kubernetes"),
                },
                ["properties"] = new JObject()
                {
                    ["metadata"] = new JObject()
                    {
                        ["name"] = new JValue("test-secret")
                    },
                    ["type"] = new JValue("Opaque"),
                    ["stringData"] = new JObject()
                    {
                        ["key"] = new JValue("[reference(resourceId('kubernetes.core/ConfigMap', 'test-map'), 'v1', 'full').properties.data.key]")
                    }
                },
                ["dependsOn"] = new JArray()
                {
                    new JValue("[resourceId('kubernetes.core/ConfigMap', 'test-map')]"),
                },
            });
        }

        [TestMethod]
        public void ConfigMap_can_be_referenced_existing()
        {
            var text = @"
import kubernetes from kubernetes

resource map 'kubernetes.core/ConfigMap@v1' existing = {
    metadata: {
        name: 'test-map'
    }
}

resource secret 'kubernetes.core/Secret@v1' = {
    metadata: {
        name: 'test-secret'
    }
    type: 'Opaque'
    stringData: {
        key: map.data.key
    }
}
";

            var compilation = CompilationHelper.Compile(GetCompilationContext(), text);
            var model = compilation.Compilation.GetEntrypointSemanticModel();
            compilation.Diagnostics.Should().BeEmpty();

            var configMapSymbol = model.AllResources.Should().ContainSingle(r => r.Symbol.Name == "map").Subject.Symbol;
            configMapSymbol.TryGetResourceTypeReference().Should().BeEquivalentTo(ResourceTypeReference.Parse("kubernetes.core/ConfigMap@v1"));

            var secretSymbol = model.AllResources.Should().ContainSingle(r => r.Symbol.Name == "secret").Subject.Symbol;
            secretSymbol.TryGetResourceTypeReference().Should().BeEquivalentTo(ResourceTypeReference.Parse("kubernetes.core/Secret@v1"));

            var template = compilation.Template!;

            var resources = template.SelectToken("resources").Should().BeAssignableTo<JArray>().Which.Should().HaveCount(1);
            template.Should().HaveValueAtPath("resources[0]", new JObject()
            {
                ["type"] = new JValue("kubernetes.core/Secret@v1"),
                ["name"] = new JValue("secret"),
                ["import"] = new JObject()
                {
                    ["provider"] = new JValue("kubernetes"),
                },
                ["properties"] = new JObject()
                {
                    ["metadata"] = new JObject()
                    {
                        ["name"] = new JValue("test-secret")
                    },
                    ["type"] = new JValue("Opaque"),
                    ["stringData"] = new JObject()
                    {
                        ["key"] = new JValue("[reference(resourceId('kubernetes.core/ConfigMap', 'test-map'), 'v1', 'full').properties.data.key]")
                    }
                },
            });
        }
    }
}
