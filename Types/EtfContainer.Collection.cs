namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public EtfList AsList()
    {
        EnforceIsType(EtfConstants.ListExt);
        return ((EtfList)_complexData!);
    }
    public EtfMap AsMap()
    {
        EnforceIsType(EtfConstants.MapExt);
        return ((EtfMap)_complexData!);
    }
    
    public static implicit operator EtfContainer(EtfList list)
    {
        return AsContainer(list, EtfConstants.ListExt);
    }
    public static implicit operator EtfContainer(EtfMap map)
    {
        return AsContainer(map, EtfConstants.MapExt);
    }
}