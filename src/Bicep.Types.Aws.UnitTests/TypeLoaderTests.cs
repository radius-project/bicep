// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;

namespace Azure.Bicep.Types.Aws.UnitTests
{
    [TestClass]
    public class K8sTypeLoaderTests
    {
        [TestMethod]
        public void K8sTypeLoader_can_load_all_types_without_throwing()
        {
            var typeLoader = new AwsTypeLoader();
            var index = typeLoader.LoadTypeIndex();

            foreach (var kvp in index.Resources)
            {
                var resourceType = typeLoader.LoadResourceType(kvp.Value);
            }

            foreach (var (resourceType, functionsByApiVersion) in index.Functions)
            {
                foreach (var (apiVersion, functions) in functionsByApiVersion)
                {
                    foreach (var functionLocation in functions)
                    {
                        var resourceFunctionType = typeLoader.LoadResourceFunctionType(functionLocation);
                    }
                }
            }
        }

        [TestMethod]
        public void TypeLoader_type_keys_are_insensitively_unique()
        {
            var typeLoader = new AwsTypeLoader();
            var index = typeLoader.LoadTypeIndex();

            index.Resources.Keys.Select(x => x.ToLowerInvariant()).Should().OnlyHaveUniqueItems();
            index.Functions.Keys.Select(x => x.ToLowerInvariant()).Should().OnlyHaveUniqueItems();
            foreach (var functionsByApiVersion in index.Functions.Values)
            {
                functionsByApiVersion.Keys.Select(x => x.ToLowerInvariant()).Should().OnlyHaveUniqueItems();
            }
        }
    }
}