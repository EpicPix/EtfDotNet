namespace EtfDotNet.Types;

public partial struct EtfContainer : IDisposable
{
    public static readonly EtfContainer Nil = AsContainer(ArraySegment<byte>.Empty, EtfConstants.NilExt);
    private bool _returnToPool;
    public ArraySegment<byte> ContainedData;
    public EtfConstants Type;
    internal IEtfComplex? _complexData;

    public static EtfContainer Make(int length, EtfConstants type)
    {
        return new EtfContainer()
        {
            ContainedData = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(length), 0, length),
            Type = type,
            _returnToPool = true
        };
    }

    public static EtfContainer AsContainer(ArraySegment<byte> data, EtfConstants type)
    {
        var container = new EtfContainer()
        {
            Type = type,
            ContainedData = data,
            _returnToPool = false
        };
        return container;
    }

    public static EtfContainer AsContainer(IEtfComplex complexData, EtfConstants type)
    {
        var container = new EtfContainer()
        {
            Type = type,
            _complexData = complexData,
            _returnToPool = false,
        };
        return container;
    }
    
    public void Dispose()
    {
        _complexData?.Dispose();
        if (!_returnToPool) return;
        ContainedData.ReturnShared();
    }

    [Pure]
    public int GetSize()
    {
        if (_complexData is not null)
        {
            return _complexData.GetSize();
        }

        return ContainedData.Count;
    }

    [Pure]
    public int GetSerializedSize()
    {
        return EtfEncoder.CalculateTypeSize(this);
    }

    [Pure]
    public ArraySegment<byte> Serialize(out bool shouldReturnToSharedPool)
    {
        if (_complexData is not null)
        {
            shouldReturnToSharedPool = true;
            var size = _complexData.GetSerializedSize();
            var arr = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(size), 0, size);
            var mem = EtfMemory.FromArray(arr);
            _complexData.Serialize(mem);
            return arr;
        }

        shouldReturnToSharedPool = false;
        return ContainedData;
    }

    public void EnforceIsType(EtfConstants type)
    {
        if (Type != type)
            throw new InvalidCastException($"The EtfContainer is of type {Type} and not {type}");
    }
}