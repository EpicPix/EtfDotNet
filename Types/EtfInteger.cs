namespace EtfDotNet.Types;

public record EtfInteger(int Value) : EtfType
{
    public static implicit operator EtfInteger(int v)
    {
        return new EtfInteger(v);
    }
    
    public static implicit operator int(EtfInteger v)
    {
        return v.Value;
    }
}