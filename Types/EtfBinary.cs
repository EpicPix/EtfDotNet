using System.Collections.ObjectModel;

namespace EtfDotNet.Types;

public struct EtfBinary : IEtfType
{
    public readonly ReadOnlyMemory<byte> Bytes;
    
    public EtfBinary(byte[] bytes)
    {
        Bytes = new ReadOnlyMemory<byte>(bytes);
    }
}