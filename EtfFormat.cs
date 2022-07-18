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
        return DecodeType(input);
    }

    public static void Pack(EtfType type, Stream output)
    {
        output.WriteConstant(EtfConstants.VersionNumber);
        EncodeType(type, output);
    }

    private static EtfType DecodeType(Stream input)
    {
        var typeId = input.ReadConstant();
        switch (typeId)
        {
            case EtfConstants.MapExt:
                throw new NotImplementedException();
            default:
                throw new EtfException($"Unknown type {typeId}");
        }
    }

    private static void EncodeType(EtfType type, Stream output)
    {
        if (type is EtfMap map)
        {
            output.WriteConstant(EtfConstants.MapExt);
            throw new NotImplementedException();
        }
        throw new EtfException($"Unknown type {type}");
    }
    
}