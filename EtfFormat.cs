namespace EtfDotNet;

public static class EtfFormat
{
    
    public static EtfContainer Unpack(EtfMemory input)
    {
        if (input.ReadConstant() != EtfConstants.VersionNumber)
        {
            throw new EtfException("Invalid version number");
        }
        return EtfDecoder.DecodeType(input);
    }
    
    public static EtfContainer Unpack(byte[] input)
    {
        return Unpack(EtfMemory.FromArray(input));
    }

    public static void Pack(EtfContainer etfType, EtfMemory output)
    {
        output.WriteConstant(EtfConstants.VersionNumber);
        EtfEncoder.EncodeType(etfType, output);
    }

    public static byte[] Pack(EtfContainer etfType)
    {
        var mem = new byte[GetPackedSize(etfType)];
        Pack(etfType, EtfMemory.FromArray(mem));
        return mem;
    }

    public static int GetPackedSize(EtfContainer container)
    {
        return EtfEncoder.CalculateTypeSize(container) + 1;
    }
}