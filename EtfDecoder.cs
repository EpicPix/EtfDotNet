using System.Text;
using EtfDotNet.Types;

namespace EtfDotNet;

internal static class EtfDecoder
{
    public static EtfType DecodeType(Stream input)
    {
        var typeId = input.ReadConstant();
        if (typeId == EtfConstants.AtomExt)
        {
            return DecodeAtom(input);
        }
        if (typeId == EtfConstants.MapExt)
        {
            return DecodeMap(input);
        }
        throw new EtfException($"Unknown type {typeId}");
    }

    public static EtfAtom DecodeAtom(Stream input)
    {
        var len = input.ReadUShort();
        var latin1Text = new byte[len];
        if (input.Read(latin1Text) != len)
        {
            throw new IOException("Not everything has been read from the stream");
        }
        return new EtfAtom(Encoding.Latin1.GetString(latin1Text));
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