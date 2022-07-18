using System.Diagnostics.Contracts;

namespace EtfDotNet.Types;

public class EtfMap : List<(EtfContainer, EtfContainer)>, IEtfType, IEtfComplex
{
    [Pure]
    public int GetSize()
    {
        int size = 4; // uint length (count)
        foreach (var container in this)
        {
            size += container.Item1.GetByteSize();
            size += container.Item2.GetByteSize();
        }
        return size;
    }

    public int GetSerializedSize()
    {
        int size = 4; // uint length (count)
        foreach (var container in this)
        {
            size += container.Item1.GetSerializedByteSize();
            size += container.Item2.GetSerializedByteSize();
        }
        return size;
    }

    public void Serialize(EtfMemory memory)
    {
        memory.WriteUInt((uint)Count);
        foreach (var container in this)
        {
            EtfEncoder.EncodeType(container.Item1, memory);
            EtfEncoder.EncodeType(container.Item2, memory);
        }
    }
    
    public void Dispose()
    {
        foreach (var container in this)
        {
            container.Item1.Dispose();
            container.Item2.Dispose();
        }
    }
}