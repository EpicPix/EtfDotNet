using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EtfDotNet.Types;

namespace EtfDotNet.Json;

internal static class EtfToJsonConverter
{
    public static JsonNode? Convert(EtfContainer type)
    {
        if (type.Type == EtfConstants.MapExt)
        {
            var obj = new JsonObject();
            foreach (var (k, v) in type.AsMap())
            {
                var key = Convert(k);
                var keyName = key!.AsValue().GetValue<string>();
                obj[keyName] = Convert(v);
            }
            return obj;
        }
        if (type.Type == EtfConstants.ListExt)
        {
            var arr = new JsonArray();
            foreach (var v in type.AsList())
            {
                arr.Add(Convert(v));
            }
            return arr;
        }
        if (type.Type == EtfConstants.NilExt)
        {
            return new JsonArray();;
        }
        if (type.Type == EtfConstants.AtomExt)
        {
            var atom = type.ToAtom();
            return atom switch {
                "false" => JsonValue.Create(false),
                "true" => JsonValue.Create(true),
                "nil" => null,
                _ => JsonValue.Create(atom)
            };
        }
        if (type.Type == EtfConstants.IntegerExt)
        {
            return JsonValue.Create((int)type);
        }
        if (type.Type == EtfConstants.StringExt)
        {
            return JsonValue.Create((string)type);
        }
        // if (type is EtfBig big)
        // {
        //     return JsonValue.Create(big.Number.ToString());
        // }
        if (type.Type == EtfConstants.BinaryExt)
        {
            return JsonValue.Create(Encoding.Latin1.GetString(type.ContainedData));
        }
        if (type.Type == EtfConstants.SmallIntegerExt)
        {
            return JsonValue.Create((byte)type);
        }
        throw new EtfException($"Unknown EtfType: {type.Type}");
    }
}