using EtfDotNet.Types;

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

    public static void Pack(EtfContainer etfType, EtfMemory output)
    {
        output.WriteConstant(EtfConstants.VersionNumber);
        EtfEncoder.EncodeType(etfType, output);
    }

    public static int GetPackedSize(EtfContainer container)
    {
        return container.GetSerializedByteSize() + 1;
    }
}