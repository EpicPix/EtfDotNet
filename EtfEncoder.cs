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
        output.WriteUShort((ushort) bytes.Length);
        output.Write(bytes);
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