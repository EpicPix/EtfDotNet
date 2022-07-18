namespace EtfDotNet.Types;

public record struct EtfAtom(string Name) : EtfType
{
    public static implicit operator EtfAtom(string str)
    {
        return new EtfAtom(str);
    }
}