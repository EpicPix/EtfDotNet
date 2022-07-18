using EtfDotNet.Types;

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
        if (input.ReadConstant() != EtfConstants.VersionNumber)
        {
            throw new EtfException("Invalid version number");
        }
        return EtfDecoder.DecodeType(input);
    }

    public static void Pack(EtfType type, Stream output)
    {
        output.WriteConstant(EtfConstants.VersionNumber);
        EtfEncoder.EncodeType(output, type);
    }
    
}