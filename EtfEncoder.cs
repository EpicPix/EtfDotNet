using System.Buffers;
using System.Diagnostics.Contracts;
using System.Text;
using EtfDotNet.Types;

namespace EtfDotNet;

public static class EtfEncoder
{

    public static void EncodeType(EtfContainer container, EtfMemory output)
    {
        var type = container.Type;
        ArraySegment<byte> buf = container.ContainedData;
        switch (type)
        {
            // SmallBigExt
            case EtfConstants.SmallBigExt:
                output.WriteConstant(type);
                output.WriteByte((byte)(buf.Count - 1));
                output.Write(buf);
                break;
            // ushort-length types
            case EtfConstants.StringExt or EtfConstants.AtomExt:
                 output.WriteConstant(type);
                 output.WriteUShort((ushort)buf.Count);
                 output.Write(buf);
                 break;
            // uint-length types
            case EtfConstants.BinaryExt:
                output.WriteConstant(type);
                output.WriteUInt((uint)buf.Count);
                output.Write(buf);
                break;
             // raw byte types
             case EtfConstants.IntegerExt or 
                 EtfConstants.SmallIntegerExt:
                 output.WriteConstant(type);
                 output.Write(buf);
                 break;
             // complex types
            case EtfConstants.MapExt or EtfConstants.ListExt:
                if (type == EtfConstants.ListExt && container.AsList().Count == 0)
                {
                    output.WriteConstant(EtfConstants.NilExt);
                    break;
                }
                output.WriteConstant(type);
                buf = container.Serialize(out var ret);
                output.Write(buf);
                if(ret) buf.ReturnShared();
                break;
            // nil type
            case EtfConstants.NilExt:
                output.WriteConstant(type);
                break;
             default:
                 throw new EtfException($"Unknown type {type}");
        }
    }

    [Pure]
    public static int CalculateTypeSize(EtfContainer container)
    {
        var type = container.Type;
        int length = 0;
        ArraySegment<byte> buf = container.ContainedData;
        switch (type)
        {
            // SmallBigExt
            case EtfConstants.SmallBigExt:
                length = 1 + 1 + buf.Count;
                break;
            // ushort-length types
            case EtfConstants.StringExt or EtfConstants.AtomExt:
                length = 1 + 2 + buf.Count;
                break;
            // uint-length types
            case EtfConstants.BinaryExt:
                length = 1 + 4 + buf.Count;
                break;
            // raw byte types
            case EtfConstants.IntegerExt or 
                EtfConstants.SmallIntegerExt:
                length = 1 + buf.Count;
                break;
            // complex types
            case EtfConstants.MapExt or EtfConstants.ListExt:
                if (type == EtfConstants.ListExt && container.AsList().Count == 0)
                {
                    length = 1;
                    break;
                }
                length = 1 + container._complexData!.GetSerializedSize();
                break;
            // nil type
            case EtfConstants.NilExt:
                length = 1;
                break;
            default:
                throw new EtfException($"Unknown type {type}");
        }

        return length;
    }
}