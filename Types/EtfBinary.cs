using System.Collections.ObjectModel;

namespace EtfDotNet.Types;

public class EtfBinary : EtfType
{

    public readonly ReadOnlyCollection<byte> Bytes;
    
    public EtfBinary(byte[] bytes)
    {
        Bytes = Array.AsReadOnly(bytes);
    }

}