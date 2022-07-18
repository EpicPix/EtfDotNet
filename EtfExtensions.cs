using System.Buffers;

namespace EtfDotNet;

internal static class EtfExtensions
{

    public static EtfConstants ReadConstant(this EtfMemory stream)
    {
        return (EtfConstants) stream.ReadByte();
    }

    public static ushort ReadUShort(this EtfMemory stream)
    {
        var value1 = (byte) stream.ReadByte();
        var value2 = (byte) stream.ReadByte();
        return (ushort) ((value1 << 8) | value2);
    }

    public static uint ReadUInt(this EtfMemory stream)
    {
        var value1 = (byte) stream.ReadByte();
        var value2 = (byte) stream.ReadByte();
        var value3 = (byte) stream.ReadByte();
        var value4 = (byte) stream.ReadByte();
        return (uint) ((value1 << 24) | (value2 << 16) | (value3 << 8) | value4);
    }
    
    public static uint ReadUInt(this ArraySegment<byte> buf)
    {
        var value1 = buf[0];
        var value2 = buf[1];
        var value3 = buf[2];
        var value4 = buf[3];
        return (uint) ((value1 << 24) | (value2 << 16) | (value3 << 8) | value4);
    }

    public static ulong ReadULong(this EtfMemory stream)
    {
        var value1 = stream.ReadUInt();
        var value2 = stream.ReadUInt();
        return ((ulong) value1 << 32) | value2;
    }
    
    public static void WriteConstant(this EtfMemory stream, EtfConstants constant)
    {
        stream.WriteByte((byte) constant);
    }

    public static void WriteUShort(this EtfMemory stream, ushort value)
    {
        stream.WriteByte((byte) (value >> 8));
        stream.WriteByte((byte) value);
    }

    public static void WriteUInt(this EtfMemory stream, uint value)
    {
        stream.WriteByte((byte) (value >> 24));
        stream.WriteByte((byte) (value >> 16));
        stream.WriteByte((byte) (value >> 8));
        stream.WriteByte((byte) value);
    }
    
    public static void WriteUInt(this ArraySegment<byte> buf, uint value)
    {
        buf[0] = (byte) (value >> 24);
        buf[1] = (byte) (value >> 16);
        buf[2] = (byte) (value >> 8);
        buf[3] = (byte) value;
    }

    public static void WriteULong(this EtfMemory stream, ulong value)
    {
        stream.WriteUInt((uint) (value >> 32));
        stream.WriteUInt((uint) value);
    }

    public static void ReturnShared(this ArraySegment<byte> arr)
    {
        ArrayPool<byte>.Shared.Return(arr.Array);
    }
}