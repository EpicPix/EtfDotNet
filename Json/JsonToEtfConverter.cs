namespace EtfDotNet.Json;

internal static class JsonToEtfConverter
{

    public static EtfContainer Convert(JsonNode? type)
    {
        if (type is null)
        {
            return "nil";
        }
        if (type is JsonValue val)
        {
            if (val.TryGetValue<bool>(out var vbool))
            {
                return vbool ? "true" : "false";
            }
            if (val.TryGetValue<string>(out var vstring))
            {
                return Encoding.UTF8.GetBytes(vstring);
            }
            if (val.TryGetValue<double>(out var vdouble))
            {
                return vdouble;
            }
            var v = val.GetValue<object>();
            if(long.TryParse(v.ToString(), out var vlong))
            {
                if (vlong >= byte.MinValue && vlong <= byte.MaxValue)
                {
                    return (byte) vlong;
                }
                if (vlong >= int.MinValue && vlong <= int.MaxValue)
                {
                    return (int) vlong;
                }
                return (BigInteger)vlong;
            }
            throw new EtfException($"Unknown Json value type: {v.GetType()}");
        }
        if (type is JsonArray arr)
        {
            var list = new EtfList();
            foreach (var value in arr)
            {
                list.Add(Convert(value));
            }
            return list;
        }
        if (type is JsonObject obj)
        {
            var map = new EtfMap();
            foreach (var (k, v) in obj)
            {
                map.Add((k, Convert(v)));
            }
            return map;
        }
        throw new EtfException($"Unsupported Json node: {type}");
    }
}