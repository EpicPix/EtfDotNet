namespace EtfDotNet.Types;

public record EtfString(string Value) : EtfType
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