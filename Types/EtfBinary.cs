using System.Collections.ObjectModel;

namespace EtfDotNet.Types;

public struct EtfBinary : EtfType
{
    public readonly ReadOnlyMemory<byte> Bytes;
    
    public EtfBinary(byte[] bytes)
    {
        Bytes = new ReadOnlyMemory<byte>(bytes);
    }
}