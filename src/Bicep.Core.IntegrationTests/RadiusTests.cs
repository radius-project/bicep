// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bicep.Core.Emit;
using Bicep.Core.Semantics;
using Bicep.Core.TypeSystem.Applications;
using Bicep.Core.UnitTests.Assertions;
using Bicep.Core.UnitTests.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Bicep.Core.IntegrationTests
{
    [TestClass]
    public class RadiusTests
    {
        [TestMethod]
        public void Application_with_instances_can_be_compiled()
        {
            var text = @"
application app = {
  name: 'app'

  instance frontend 'radius.dev/Container@v1alpha1' = {
    name: 'frontend'
    properties: {
      run: {
        container: {
          image: 'rynowak/frontend:latest'
        }
      }
    }
  }

  instance backend 'radius.dev/Container@v1alpha1' = {
    name: 'backend'
    properties: {
      run: {
        container: {
          image: 'rynowak/backend:latest'
        }
      }
    }
  }
}
";

            var compilation = new Compilation(TestResourceTypeProvider.Create(), new ComponentTypeProvider(), SyntaxFactory.CreateFromText(text));
            compilation.GetEntrypointSemanticModel().GetAllDiagnostics().Should().BeEmpty();

            var (result, template) = Emit(compilation);
            result.Diagnostics.Should().BeEmpty();

            var resources = GetResources(template!);
            resources.Should().HaveCount(4);

            var application = resources.Where(r => r.IsApplicationType()).Should().ContainSingle().Subject;
            application.Name.Value<string>().Should().BeEquivalentTo("[format('{0}/{1}', 'radius', 'app')]");
            application.DependsOn!.Should().BeNull();

            var frontend = resources.Where(r => r.IsComponentType() && r.NameContainsSegment("frontend")).Should().ContainSingle().Subject;
            frontend.Name.Value<string>().Should().BeEquivalentTo("[format('{0}/{1}/{2}', 'radius', 'app', 'frontend')]");
            frontend.DependsOn.Should().SatisfyRespectively(token =>
            {
                token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications', split(format('{0}/{1}', 'radius', 'app'), '/')[0], split(format('{0}/{1}', 'radius', 'app'), '/')[1])]");
            });

            var backend = resources.Where(r => r.IsComponentType() && r.NameContainsSegment("backend")).Should().ContainSingle().Subject;
            backend.Name.Value<string>().Should().BeEquivalentTo("[format('{0}/{1}/{2}', 'radius', 'app', 'backend')]");
            backend.DependsOn.Should().SatisfyRespectively(token =>
            {
                token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications', split(format('{0}/{1}', 'radius', 'app'), '/')[0], split(format('{0}/{1}', 'radius', 'app'), '/')[1])]");
            });

            var deployment = resources.Where(r => r.IsDeploymentType()).Should().ContainSingle().Subject;
            deployment.Name.Value<string>().Should().BeEquivalentTo("[format('{0}/{1}/{2}', 'radius', 'app', 'implicit')]");
            deployment.DependsOn.Should().SatisfyRespectively(
                token =>
                {
                    token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications', split(format('{0}/{1}', 'radius', 'app'), '/')[0], split(format('{0}/{1}', 'radius', 'app'), '/')[1])]");
                },
                token =>
                {
                    token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications/Components', split(format('{0}/{1}/{2}', 'radius', 'app', 'backend'), '/')[0], split(format('{0}/{1}/{2}', 'radius', 'app', 'backend'), '/')[1], split(format('{0}/{1}/{2}', 'radius', 'app', 'backend'), '/')[2])]");
                },
                token =>
                {
                    token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications/Components', split(format('{0}/{1}/{2}', 'radius', 'app', 'frontend'), '/')[0], split(format('{0}/{1}/{2}', 'radius', 'app', 'frontend'), '/')[1], split(format('{0}/{1}/{2}', 'radius', 'app', 'frontend'), '/')[2])]");
                });
            
            var components = deployment.Node
                .Property("properties")!.Should().BeOfType<JProperty>().Subject.Value.As<JObject>()
                .Property("components")!.Should().BeOfType<JProperty>().Subject.Value.As<JArray>();
            components.Should().SatisfyRespectively(
                token =>
                {
                    var value = token.Should().BeOfType<JObject>().Subject.Property("componentName")!.Should().BeOfType<JProperty>().Subject.Value;
                    value.Value<string>().Should().BeEquivalentTo("frontend");
                },
                token =>
                {
                    var value = token.Should().BeOfType<JObject>().Subject.Property("componentName")!.Should().BeOfType<JProperty>().Subject.Value;
                    value.Value<string>().Should().BeEquivalentTo("backend");
                });
        }

        [TestMethod]
        public void Application_with_components_and_deployment_can_be_compiled()
        {
            var text = @"
application app = {
  name: 'app'

  component frontend 'radius.dev/Container@v1alpha1' = {
    name: 'frontend'
    properties: {
      run: {
        container: {
          image: 'rynowak/frontend:latest'
        }
      }
    }
  }

  component backend 'radius.dev/Container@v1alpha1' = {
    name: 'backend'
    properties: {
      run: {
        container: {
          image: 'rynowak/backend:latest'
        }
      }
    }
  }

  deployment deploy = {
    name: 'default'
    properties: {
      components: [
        {
          componentName: frontend.name
        }
        {
          componentName: backend.name
        }
      ]
    }
  }
}
";

            var compilation = new Compilation(TestResourceTypeProvider.Create(), new ComponentTypeProvider(), SyntaxFactory.CreateFromText(text));
            compilation.GetEntrypointSemanticModel().GetAllDiagnostics().Should().BeEmpty();

            var (result, template) = Emit(compilation);
            result.Diagnostics.Should().BeEmpty();

            var resources = GetResources(template!);
            resources.Should().HaveCount(4);

            var application = resources.Where(r => r.IsApplicationType()).Should().ContainSingle().Subject;
            application.Name.Value<string>().Should().BeEquivalentTo("[format('{0}/{1}', 'radius', 'app')]");
            application.DependsOn!.Should().BeNull();

            var frontend = resources.Where(r => r.IsComponentType() && r.NameContainsSegment("frontend")).Should().ContainSingle().Subject;
            frontend.Name.Value<string>().Should().BeEquivalentTo("[format('{0}/{1}/{2}', 'radius', 'app', 'frontend')]");
            frontend.DependsOn.Should().SatisfyRespectively(token =>
            {
                token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications', split(format('{0}/{1}', 'radius', 'app'), '/')[0], split(format('{0}/{1}', 'radius', 'app'), '/')[1])]");
            });

            var backend = resources.Where(r => r.IsComponentType() && r.NameContainsSegment("backend")).Should().ContainSingle().Subject;
            backend.Name.Value<string>().Should().BeEquivalentTo("[format('{0}/{1}/{2}', 'radius', 'app', 'backend')]");
            backend.DependsOn.Should().SatisfyRespectively(token =>
            {
                token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications', split(format('{0}/{1}', 'radius', 'app'), '/')[0], split(format('{0}/{1}', 'radius', 'app'), '/')[1])]");
            });

            var deployment = resources.Where(r => r.IsDeploymentType()).Should().ContainSingle().Subject;
            deployment.Name.Value<string>().Should().BeEquivalentTo("[format('{0}/{1}/{2}', 'radius', 'app', 'default')]");
            deployment.DependsOn.Should().SatisfyRespectively(
                token =>
                {
                    token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications', split(format('{0}/{1}', 'radius', 'app'), '/')[0], split(format('{0}/{1}', 'radius', 'app'), '/')[1])]");
                },
                token =>
                {
                    token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications/Components', split(format('radius/{0}/{1}', 'app', 'backend'), '/')[0], split(format('radius/{0}/{1}', 'app', 'backend'), '/')[1], split(format('radius/{0}/{1}', 'app', 'backend'), '/')[2])]");
                },
                token =>
                {
                    token.Value<string>().Should().BeEquivalentTo("[resourceId('Microsoft.CustomProviders/resourceProviders/Applications/Components', split(format('radius/{0}/{1}', 'app', 'frontend'), '/')[0], split(format('radius/{0}/{1}', 'app', 'frontend'), '/')[1], split(format('radius/{0}/{1}', 'app', 'frontend'), '/')[2])]");
                });
            
            var components = deployment.Node
                .Property("properties")!.Should().BeOfType<JProperty>().Subject.Value.As<JObject>()
                .Property("components")!.Should().BeOfType<JProperty>().Subject.Value.As<JArray>();
            components.Should().SatisfyRespectively(
                token =>
                {
                    var value = token.Should().BeOfType<JObject>().Subject.Property("componentName")!.Should().BeOfType<JProperty>().Subject.Value;
                    value.Value<string>().Should().BeEquivalentTo("[format('radius/{0}/{1}', 'app', 'frontend')]");
                },
                token =>
                {
                    var value = token.Should().BeOfType<JObject>().Subject.Property("componentName")!.Should().BeOfType<JProperty>().Subject.Value;
                    value.Value<string>().Should().BeEquivalentTo("[format('radius/{0}/{1}', 'app', 'backend')]");
                });
        }

        private (EmitResult result, JObject? template) Emit(Compilation compilation)
        {
            using var stream = new MemoryStream();
            var emitter = new TemplateEmitter(compilation.GetEntrypointSemanticModel());
            var result = emitter.Emit(stream);

            if (result.Status == EmitStatus.Failed)
            {
                return (result, (JObject?)null);
            }
            else
            {
                stream.Seek(0L, SeekOrigin.Begin);
                var text = Encoding.UTF8.GetString(stream.GetBuffer());
                var template = JObject.Parse(text);
                return (result, (JObject?)template);
            }
        }

        private Resource[] GetResources(JObject template)
        {
            var resourcesProperty = template.Property("resources")!.Should().BeOfType<JProperty>().Subject;
            var resourcesArray = resourcesProperty.Value.Should().BeOfType<JArray>().Subject;

            var resources = new List<Resource>();
            foreach (var resource in resourcesArray)
            {
                resources.Add(new Resource(resource.Should().BeOfType<JObject>().Subject));
            }

            return resources.ToArray();
        }

        private class Resource
        {
            public Resource(JObject node)
            {
                this.Node = node;
            }

            public JObject Node { get; }

            public JValue Name => Node.Property("name")!.Should().BeOfType<JProperty>().Subject.Value.Should().BeOfType<JValue>().Subject;

            public JValue Type => Node.Property("type")!.Should().BeOfType<JProperty>().Subject.Value.Should().BeOfType<JValue>().Subject;

            public JValue apiVersion => Node.Property("apiVersion")!.Should().BeOfType<JProperty>().Subject.Value.Should().BeOfType<JValue>().Subject;

            public JArray? DependsOn => Node.Property("dependsOn")?.Should().BeOfType<JProperty>().Subject.Value.Should().BeOfType<JArray>().Subject;

            public bool IsApplicationType()
            {
                return Type.Value<string>() == "Microsoft.CustomProviders/resourceProviders/Applications";
            }

            public bool IsComponentType()
            {
                return Type.Value<string>() == "Microsoft.CustomProviders/resourceProviders/Applications/Components";
            }

            public bool IsDeploymentType()
            {
                return Type.Value<string>() == "Microsoft.CustomProviders/resourceProviders/Applications/Deployments";
            }

            public bool NameContainsSegment(string segment)
            {
                return Name.Value<string>().Split(new[]{'\'', '/'}).Any(s => s.Equals(segment));
            }
        }
    }
}