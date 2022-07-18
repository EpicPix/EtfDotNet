namespace EtfDotNet.Types;

public class EtfMap : EtfType
{
    private readonly Dictionary<EtfType, EtfType> _dict = new();
    
    public EtfType this[EtfType at]
    {
        get => _dict[at];
        set => _dict[at] = value;
    }
    
    public EtfType this[EtfAtom at]
    {
        get => _dict[at];
        set => _dict[at] = value;
    }

    public List<(EtfType, EtfType)> Entries()
    {
        var entries = new List<(EtfType, EtfType)>();
        foreach (var (k, v) in _dict)
        {
            entries.Add((k, v));
        }
        return entries;
    }
}