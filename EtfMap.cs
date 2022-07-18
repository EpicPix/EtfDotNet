namespace EtfDotNet;

public class EtfMap : EtfType
{
    private readonly Dictionary<EtfType, EtfType> _dict = new();
    
    public EtfType this[EtfType at]
    {
        get => _dict[at];
        set => _dict[at] = value;
    }
    
}