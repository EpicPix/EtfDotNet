namespace EtfDotNet.Types;

public record EtfSmallInteger(byte Value) : EtfType
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