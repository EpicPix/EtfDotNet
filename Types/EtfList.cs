using EtfDotNet.Poco;

namespace EtfDotNet.Types;

public class EtfList : List<EtfContainer>, IEtfComplex
{
    public EtfList()
    {
    }

    public EtfList(IEnumerable<EtfContainer> collection) : base(collection)
    {
    }
    public EtfList(int capacity) : base(capacity)
    {
    }

    public static EtfList From<T>(IEnumerable<T> collection)
    {
        return new EtfList(collection.Select(EtfConverter.ToEtf));
    }

    public int GetSize()
    {
        int size = 5; // uint length + 1 EtfConstant
        foreach (var container in this)
        {
            size += container.GetSize();
        }
        return size;
    }
    
    public int GetSerializedSize()
    {
        int size = 5; // uint length (count) + 1 EtfConstant
        foreach (var container in this)
        {
            size += EtfEncoder.CalculateTypeSize(container);
        }
        return size;
    }

    public void Serialize(EtfMemory memory)
    {
        memory.WriteUInt((uint)Count);
        foreach (var container in this)
        {
            EtfEncoder.EncodeType(container, memory);
        }
        memory.WriteConstant(EtfConstants.NilExt);
    }

    public void Dispose()
    {
        foreach (var container in this)
        {
            container.Dispose();
        }
    }
}