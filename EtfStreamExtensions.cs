namespace EtfDotNet;

internal static class EtfStreamExtensions
{

    public static EtfConstants ReadConstant(this Stream stream)
    {
        return (EtfConstants) stream.ReadByte();
    }
    
    public static void WriteConstant(this Stream stream, EtfConstants constant)
    {
        stream.WriteByte((byte) constant);
    }
    
}