namespace EtfDotNet.Types;

public record struct EtfSmallInteger(byte Value) : IEtfType
{
    public static implicit operator EtfSmallInteger(byte v)
    {
        return new EtfSmallInteger(v);
    }
    
    public static implicit operator byte(EtfSmallInteger v)
    {
        return v.Value;
    }
}