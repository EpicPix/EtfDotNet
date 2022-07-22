namespace EtfDotNet.Json;

public static class EtfJson
{
    public static JsonNode ConvertEtfToJson(EtfContainer type)
    {
        var node = EtfToJsonConverter.Convert(type);
        if (node is null)
        {
            throw new JsonException("Node is null");
        }
        return node;
    }
    
    public static EtfContainer ConvertJsonToEtf(JsonNode node)
    {
        var type = JsonToEtfConverter.Convert(node);
        return type;
    }
}