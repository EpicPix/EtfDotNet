namespace EtfDotNet;

public static class EtfFormat
{
    public static EtfType Unpack(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return Unpack(stream);
    }

    public static byte[] Pack(EtfType type)
    {
        using var stream = new MemoryStream();
        Pack(type, stream);
        return stream.ToArray();
    }
    
    
    public static EtfType Unpack(Stream input)
    {
        throw new NotImplementedException();
    }

    public static void Pack(EtfType type, Stream output)
    {
        throw new NotImplementedException();
    }
}