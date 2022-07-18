namespace EtfDotNet.Types;

public class EtfAtom : EtfType
{
    public readonly string Name;
    
    public EtfAtom(string name)
    {
        Name = name;
    }
}