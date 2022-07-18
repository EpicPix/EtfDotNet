using System.Text;

namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public static implicit operator EtfContainer(byte v)
    {
        var container = Make(1, EtfConstants.SmallIntegerExt);
        container.ContainedData[0] = v;
        return container;
    }
    
    public static implicit operator byte(EtfContainer v)
    {
        v.EnforceIsType(EtfConstants.SmallIntegerExt);
        return v.ContainedData[0];
    }
}