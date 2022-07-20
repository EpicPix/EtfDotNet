using System.Collections;

namespace EtfDotNet.Types;

public class EtfTuple : IReadOnlyList<EtfContainer>, IEtfComplex
{
    private readonly EtfContainer[] _array;

    public int Count => _array.Length;

    public EtfContainer this[int index] {
        get => _array[index];
        set => _array[index] = value;
    }

    public EtfContainer this[uint index] {
        get => _array[index];
        set => _array[index] = value;
    }

    public EtfTuple(uint length)
    {
        _array = new EtfContainer[length];
    }
    
    public int GetSize()
    {
        int size = _array.Length > 255 ? 4 : 1; // uint length (count)
        foreach (var container in _array)
        {
            size += container.GetSize();
        }
        return size;
    }
    
    public int GetSerializedSize()
    {
        int size = _array.Length > 255 ? 4 : 1; // uint length (count)
        foreach (var container in _array)
        {
            size += EtfEncoder.CalculateTypeSize(container);
        }
        return size;
    }

    public void Serialize(EtfMemory memory)
    {
        if (_array.Length > 255)
        {
            memory.WriteUInt((uint) Count);
        } else
        {
            memory.WriteByte((byte) Count);
        }
        foreach (var container in _array)
        {
            EtfEncoder.EncodeType(container, memory);
        }
    }

    public void Dispose()
    {
        foreach (var container in _array)
        {
            container.Dispose();
        }
    }

    public IEnumerator<EtfContainer> GetEnumerator()
    {
        return ((IEnumerable<EtfContainer>) _array).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}