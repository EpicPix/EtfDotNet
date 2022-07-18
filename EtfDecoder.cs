using System.Numerics;
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
        if (typeId == EtfConstants.ListExt)
        {
            return DecodeList(input);
        }
        if (typeId == EtfConstants.SmallBigExt)
        {
            return DecodeSmallBig(input);
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

    public static EtfList DecodeList(Stream input)
    {
        var length = input.ReadUInt();
        var list = new EtfList();
        for (var i = 0u; i < length; i++)
        {
            list.Add(DecodeType(input));
        }
        if (input.ReadConstant() != EtfConstants.NilExt)
        {
            throw new EtfException("Expected NilExt");
        }
        return list;
    }

    public static EtfBig DecodeSmallBig(Stream input)
    {
        var len = input.ReadByte();
        var sign = input.ReadByte();
        var num = new BigInteger();
        for (var i = 0; i < len; i++)
        {
            num += input.ReadByte() * BigInteger.Pow(256, i);
        }
        if (sign == 1) num = -num;
        return new EtfBig(num);
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