// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Bicep.Core.UnitTests.Assertions;
using Bicep.Core.UnitTests.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bicep.Core.UnitTests.TypeSystem.Aws
{
    [TestClass]
    public class AWSResourceTypeProviderTests
    {
        private static ServiceBuilder Services => new ServiceBuilder();

        [TestMethod]
        public void AWSResourceTypeProvider_nowarn_for_existing_with_properties()
        {
            var compilation = Services.BuildCompilation(@"
import aws as aws

resource s3 'AWS.S3/Bucket@default' existing = {
  name: 'foo'
  properties: {
    BucketName: 'my-bucket-asdfasdfdfzzaasda2afq1'
    AccessControl: 'PublicRead'
  }
}

output foo string = s3.name

");

            var diag = compilation.GetEntrypointSemanticModel().GetAllDiagnostics();

            compilation.Should().NotHaveAnyDiagnostics();
        }

        [TestMethod]
        public void AWSResourceTypeProvider_nowarn_for_existing_no_name()
        {
            var compilation = Services.BuildCompilation(@"
import aws as aws

resource s3 'AWS.S3/Bucket@default' existing = {
  properties: {
    BucketName: 'my-bucket-asdfasdfdfzzaasda2afq1'
    AccessControl: 'PublicRead'
  }
}

output foo string = s3.properties.BucketName
");

            var diag = compilation.GetEntrypointSemanticModel().GetAllDiagnostics();

            compilation.Should().NotHaveAnyDiagnostics();
        }


        [TestMethod]
        public void AWSResourceTypeProvider_nowarn_for_existing_without_properties()
        {
            var compilation = Services.BuildCompilation(@"
import aws as aws
param eksClusterName string

resource eksCluster 'AWS.EKS/Cluster@default' existing = {
  name: eksClusterName
}

output foo string = eksCluster.name
");

            var diag = compilation.GetEntrypointSemanticModel().GetAllDiagnostics();

            compilation.Should().NotHaveAnyDiagnostics();
        }

        [TestMethod]
        public void AWSResourceTypeProvider_nowarn_without_name()
        {
            var compilation = Services.BuildCompilation(@"
import aws as aws

resource s3 'AWS.S3/Bucket@default' = {
  properties: {
    BucketName: 'bucket'
  }
}

output foo string = s3.properties.BucketName
");

            var diag = compilation.GetEntrypointSemanticModel().GetAllDiagnostics();

            compilation.Should().NotHaveAnyDiagnostics();
        }
    }
}
