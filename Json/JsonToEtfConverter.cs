using System.Text;
using System.Text.Json.Nodes;
using EtfDotNet.Types;

namespace EtfDotNet.Json;

internal static class JsonToEtfConverter
{

    public static EtfType Convert(JsonNode? type)
    {
        if (type is null)
        {
            return new EtfAtom("nil");
        }
        if (type is JsonValue val)
        {
            if (val.TryGetValue<bool>(out var vbool))
            {
                return vbool ? new EtfAtom("true") : new EtfAtom("false");
            }
            if (val.TryGetValue<string>(out var vstring))
            {
                return new EtfBinary(Encoding.UTF8.GetBytes(vstring));
            }
            var v = val.GetValue<object>();
            if(long.TryParse(v.ToString(), out var vlong))
            {
                if (vlong >= byte.MinValue && vlong <= byte.MaxValue)
                {
                    return new EtfSmallInteger((byte) vlong);
                }
                if (vlong >= int.MinValue && vlong <= int.MaxValue)
                {
                    return new EtfInteger((int) vlong);
                }
                return new EtfBig(vlong);
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
                map[new EtfBinary(Encoding.UTF8.GetBytes(k))] = Convert(v);
            }
            return map;
        }
        throw new EtfException($"Unsupported Json node: {type}");
    }
}