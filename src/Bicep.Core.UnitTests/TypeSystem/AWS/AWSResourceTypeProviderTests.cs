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
        public void AWSResourceTypeProvider_warn_when_multiple_identifiers_exist_but_not_all_are_specified()
        {
            // All identifier properties must be specified when using the `existing`
            var compilation = Services.BuildCompilation(@"
import aws as aws

resource t 'AWS.Redshift/EndpointAuthorization@default' existing = {
  properties: {
    // this manifest is missing missing Account (which is an Identifier prop)
    ClusterIdentifier: ''
  }
}

output foo string = t.name

");

            var diag = compilation.GetEntrypointSemanticModel().GetAllDiagnostics();

            compilation.Should().HaveDiagnostics(new[]
            {
                ("BCP035", DiagnosticLevel.Warning, "The specified \"object\" declaration is missing the following required properties: \"Account\". If this is an inaccuracy in the documentation, please report it to the Bicep Team.")
            });
        }

    }
}
