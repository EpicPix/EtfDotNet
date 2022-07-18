namespace EtfDotNet.Types;

public class EtfList : List<EtfContainer>, IEtfType, IEtfComplex
{
    public int GetSize()
    {
        int size = 5; // uint length + 1 EtfConstant
        foreach (var container in this)
        {
            size += container.GetByteSize();
        }
        return size;
    }

    public void Serialize(EtfMemory memory)
    {
        memory.WriteUInt((uint)Count);
        foreach (var container in this)
        {
            var buf = container.Serialize(out var ret);
            memory.Write(buf);
            if(ret) buf.ReturnShared();
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