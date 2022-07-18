using System.Text;

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
}