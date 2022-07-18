namespace EtfDotNet.Types;

public class EtfList : List<EtfContainer>, IEtfType, IEtfComplex
{
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