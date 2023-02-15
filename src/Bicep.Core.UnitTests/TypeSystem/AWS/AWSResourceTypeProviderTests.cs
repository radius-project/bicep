// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Bicep.Core.UnitTests.Assertions;
using Bicep.Core.UnitTests.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bicep.Core.Diagnostics;
namespace Bicep.Core.UnitTests.TypeSystem.Aws
{
    [TestClass]
    public class AWSResourceTypeProviderTests
    {
        private static ServiceBuilder Services => new ServiceBuilder();

        [TestMethod]
        public void AWSResourceTypeProvider_nowarn_for_existing_with_identifier_properties()
        {
            // Only identifier properties can be specified when using the `existing`
            var compilation = Services.BuildCompilation(@"
import aws as aws

resource s3 'AWS.S3/Bucket@default' existing = {
  alias: 'my-bucket-alias'
  properties: {
    BucketName: 'my-bucket-asdfasdfdfzzaasda2afq1'
  }
}

output foo string = s3.name

");

            var diag = compilation.GetEntrypointSemanticModel().GetAllDiagnostics();

            compilation.Should().NotHaveAnyDiagnostics();
        }

        [TestMethod]
        public void AWSResourceTypeProvider_warn_for_existing_with_non_identifier_properties()
        {
            // Only identifier properties can be specified when using the `existing`
            var compilation = Services.BuildCompilation(@"
import aws as aws

resource s3 'AWS.S3/Bucket@default' existing = {
  alias: 'my-bucket-alias'
  properties: {
    BucketName: 'my-bucket-asdfasdfdfzzaasda2afq1'
    AccessControl: 'PublicRead'
  }
}

output foo string = s3.name

");

            var diag = compilation.GetEntrypointSemanticModel().GetAllDiagnostics();

            compilation.Should().HaveDiagnostics(new[]
            {
                ("BCP073", DiagnosticLevel.Warning, "The property \"AccessControl\" is read-only. Expressions cannot be assigned to read-only properties. If this is an inaccuracy in the documentation, please report it to the Bicep Team.")
            });
        }

        [TestMethod]
        public void AWSResourceTypeProvider_error_without_alias()
        {
            var compilation = Services.BuildCompilation(@"
import aws as aws

resource s3 'AWS.S3/Bucket@default' existing = {
  properties: {
    BucketName: 'my-bucket-asdfasdfdfzzaasda2afq1'
  }
}

output foo string = s3.name

");

            var diag = compilation.GetEntrypointSemanticModel().GetAllDiagnostics();

            compilation.Should().HaveDiagnostics(new[]
            {
                ("BCP035", DiagnosticLevel.Warning, "The specified \"resource\" declaration is missing the following required properties: \"alias\". If this is an inaccuracy in the documentation, please report it to the Bicep Team.")
            });
        }

        [TestMethod]
        public void AWSResourceTypeProvider_noerror_existing_resource()
        {
            var compilation = Services.BuildCompilation(@"
import aws as aws

resource eksCluster 'AWS.EKS/Cluster@default' existing = {
  alias: 'test-eks-cluster'
  properties: {
    Name: 'test-eks-cluster'
  }
}

output foo string = eksCluster.properties.Name

");

            var diag = compilation.GetEntrypointSemanticModel().GetAllDiagnostics();

            compilation.Should().NotHaveAnyDiagnostics();
        }
    }
}
