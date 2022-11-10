// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Configuration;
using Bicep.Core.Diagnostics;
using Bicep.Core.Semantics;
using Bicep.Core.TypeSystem;
using Bicep.Core.TypeSystem.Aws;
using Bicep.Core.TypeSystem.Az;
using Bicep.Core.UnitTests.Assertions;
using Bicep.Core.UnitTests.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bicep.Core.Extensions;
using Moq;
using Bicep.Core.FileSystem;
using Bicep.Core.Semantics.Namespaces;
using System.Reflection;
using Bicep.Core.Resources;

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
    }
}
