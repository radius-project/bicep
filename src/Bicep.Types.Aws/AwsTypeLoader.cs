// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.IO;
using System.IO.Compression;

namespace Azure.Bicep.Types.Aws;

public class AwsTypeLoader : TypeLoader
{

    protected override Stream GetContentStreamAtPath(string path)
    {
        var fileStream = typeof(AwsTypeLoader).Assembly.GetManifestResourceStream($"{path}.deflated");
        if (fileStream is null)
        {
            throw new ArgumentException($"Unable to locate manifest resource at path {path}", nameof(path));
        }

        return new DeflateStream(fileStream, CompressionMode.Decompress);
    }
}
