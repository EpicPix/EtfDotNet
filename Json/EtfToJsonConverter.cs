using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EtfDotNet.Types;

namespace EtfDotNet.Json;

internal static class EtfToJsonConverter
{
    public static JsonNode? Convert(EtfType type)
    {
        if (type is EtfMap map)
        {
            var obj = new JsonObject();
            foreach (var (k, v) in map.Entries())
            {
                var key = Convert(k);
                var keyName = key!.AsValue().GetValue<string>();
                obj[keyName] = Convert(v);
            }
            return obj;
        }
        if (type is EtfList list)
        {
            var arr = new JsonArray();
            foreach (var v in list)
            {
                arr.Add(Convert(v));
            }
            return arr;
        }
        if (type is EtfAtom atom)
        {
            return JsonValue.Create(atom.Name);
        }
        if (type is EtfInteger integer)
        {
            return JsonValue.Create(integer.Value);
        }
        if (type is EtfString str)
        {
            return JsonValue.Create(str.Value);
        }
        if (type is EtfBig big)
        {
            return JsonValue.Create(big.Number.ToString());
        }
        if (type is EtfBinary binary)
        {
            var bytes = binary.Bytes.ToArray();
            return JsonValue.Create(Encoding.Latin1.GetString(bytes));
        }
        if (type is EtfSmallInteger smallInteger)
        {
            return JsonValue.Create(smallInteger);
        }
        throw new NotImplementedException();
    }
}