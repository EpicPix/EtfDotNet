namespace EtfDotNet.Types;

public record struct EtfAtom(string Name) : IEtfType
{
    public static implicit operator EtfAtom(string str)
    {
        return new EtfAtom(str);
    }
}