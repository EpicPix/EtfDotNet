using System.Text;
using EtfDotNet.Types;

namespace EtfDotNet;

internal static class EtfEncoder
{
    public static void EncodeType(Stream output, EtfType type)
    {
        if (type is EtfAtom atom)
        {
            output.WriteConstant(EtfConstants.AtomExt);
            EncodeAtom(output, atom);
            return;
        }
        if (type is EtfList list)
        {
            output.WriteConstant(EtfConstants.ListExt);
            EncodeList(output, list);
            return;
        }
        if (type is EtfBig big)
        {
            var sign = big.Number.Sign == decimal.One;
            var bytes = sign ? (-big.Number).ToByteArray() : big.Number.ToByteArray();
            if (bytes.Length > 255)
            {
                throw new EtfException("Cannot encode number with more than 255 bytes");
            }
            output.WriteConstant(EtfConstants.SmallBigExt);
            output.WriteByte((byte) (sign ? 1 : 0));
            output.WriteByte((byte) bytes.Length);
            output.Write(bytes);
            return;
        }
        if (type is EtfMap map)
        {
            output.WriteConstant(EtfConstants.MapExt);
            EncodeMap(output, map);
            return;
        }
        throw new EtfException($"Unknown type {type}");
    }

    private static void EncodeAtom(Stream output, EtfAtom atom)
    {
        var bytes = Encoding.Latin1.GetBytes(atom.Name);
        if (bytes.Length > 255)
        {
            throw new EtfException("Currently cannot encode atom with >255 bytes");
        }
        output.WriteUShort((ushort) bytes.Length);
        output.Write(bytes);
    }
    
    private static void EncodeList(Stream output, EtfList list)
    {
        output.WriteUInt((uint) list.Count);
        foreach(var value in list)
        {
            EncodeType(output, value);
        }
        output.WriteConstant(EtfConstants.NilExt);
    }
    
    private static void EncodeMap(Stream output, EtfMap map)
    {
        var entries = map.Entries();
        output.WriteUInt((uint) entries.Count);
        foreach(var (k, v) in entries)
        {
            EncodeType(output, k);
            EncodeType(output, v);
        }
    }
    
}