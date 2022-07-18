using System.Text;

namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public static implicit operator EtfContainer(int v)
    {
        var container = Make(4, EtfConstants.IntegerExt);
        EtfMemory.FromArray(container.ContainedData).WriteUInt((uint)v);
        return container;
    }
    
    public static implicit operator int(EtfContainer v)
    {
        v.EnforceIsType(EtfConstants.IntegerExt);
        return (int)EtfMemory.FromArray(v.ContainedData).ReadUInt();
    }
}