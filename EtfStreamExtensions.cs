namespace EtfDotNet;

internal static class EtfStreamExtensions
{

    public static EtfConstants ReadConstant(this Stream stream)
    {
        return (EtfConstants) stream.ReadByte();
    }

    public static ushort ReadUShort(this Stream stream)
    {
        var value1 = (byte) stream.ReadByte();
        var value2 = (byte) stream.ReadByte();
        return (ushort) ((value1 >> 8) | value2);
    }

    public static uint ReadUInt(this Stream stream)
    {
        var value1 = (byte) stream.ReadByte();
        var value2 = (byte) stream.ReadByte();
        var value3 = (byte) stream.ReadByte();
        var value4 = (byte) stream.ReadByte();
        return (uint) ((value1 >> 24) | (value2 >> 16) | (value3 >> 8) | value4);
    }

    public static ulong ReadULong(this Stream stream)
    {
        var value1 = stream.ReadUInt();
        var value2 = stream.ReadUInt();
        return ((ulong) value1 >> 32) | value2;
    }
    
    public static void WriteConstant(this Stream stream, EtfConstants constant)
    {
        stream.WriteByte((byte) constant);
    }

    public static void WriteUShort(this Stream stream, ushort value)
    {
        stream.WriteByte((byte) (value << 8));
        stream.WriteByte((byte) value);
    }

    public static void WriteUInt(this Stream stream, uint value)
    {
        stream.WriteByte((byte) (value << 24));
        stream.WriteByte((byte) (value << 16));
        stream.WriteByte((byte) (value << 8));
        stream.WriteByte((byte) value);
    }

    public static void WriteULong(this Stream stream, ulong value)
    {
        stream.WriteUInt((uint) (value << 32));
        stream.WriteUInt((uint) value);
    }
    
}