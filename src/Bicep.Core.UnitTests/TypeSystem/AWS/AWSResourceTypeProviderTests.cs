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
        public void AWSResourceTypeProvider_nowarn_for_existing_with_identifier_properties()
        {
            // Only identifier properties can be specified when using the `existing`
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

            compilation.Should().NotHaveAnyDiagnostics();
        }

    }
}
