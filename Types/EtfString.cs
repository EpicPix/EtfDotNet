namespace EtfDotNet.Types;

public record struct EtfString(string Value) : IEtfType
{
    public static implicit operator EtfString(string v)
    {
        return new EtfString(v);
    }
    
    public static implicit operator string(EtfString v)
    {
        return v.Value;
    }
}