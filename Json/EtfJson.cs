using System.Text.Json;
using System.Text.Json.Nodes;
using EtfDotNet.Types;

namespace EtfDotNet.Json;

public static class EtfJson
{
    public static JsonNode ConvertEtfToJson(EtfType type)
    {
        var node = EtfToJsonConverter.Convert(type);
        if (node is null)
        {
            throw new JsonException("Node is null");
        }
        return node;
    }
}