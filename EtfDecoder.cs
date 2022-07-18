using EtfDotNet.Types;

namespace EtfDotNet;

internal static class EtfDecoder
{
    public static EtfType DecodeType(Stream input)
    {
        var typeId = input.ReadConstant();
        if (typeId == EtfConstants.MapExt)
        {
            return DecodeMap(input);
        }
        throw new EtfException($"Unknown type {typeId}");
    }

    public static EtfMap DecodeMap(Stream input)
    {
        var kvLength = input.ReadUInt();
        var map = new EtfMap();
        for (var i = 0u; i < kvLength; i++)
        {
            var key = DecodeType(input);
            var value = DecodeType(input);
            map[key] = value;
        }
        return map;
    }
}