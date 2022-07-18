using System.Diagnostics.Contracts;

namespace EtfDotNet.Types;

public class EtfMap : List<(EtfContainer, EtfContainer)>, IEtfType, IEtfComplex
{
    [Pure]
    public int GetSize()
    {
        int size = 5; // uint length + 1 EtfConstant
        foreach (var container in this)
        {
            size += container.Item1.GetByteSize();
            size += container.Item2.GetByteSize();
        }
        return size;
    }
    
    public void Serialize(EtfMemory memory)
    {
        memory.WriteUInt((uint)Count);
        foreach (var container in this)
        {
            var buf = container.Item1.Serialize(out var ret);
            memory.Write(buf);
            if(ret) buf.ReturnShared();
            var buf2 = container.Item2.Serialize(out var ret2);
            memory.Write(buf2);
            if(ret2) buf2.ReturnShared();
        }
    }
}