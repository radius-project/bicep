// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Text.Json;

namespace Azure.Bicep.Types.Radius.Index
{
    public static class TypeIndexer
    {
        private static readonly JsonSerializerOptions SerializeOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public static TypeIndex DeserializeIndex(string content)
        {
            return JsonSerializer.Deserialize<TypeIndex>(content, SerializeOptions) ?? throw new JsonException("Failed to deserialize index");
        }
    }
}