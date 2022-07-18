using System.Text;

namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public static implicit operator EtfContainer(byte v)
    {
        var container = Make(1, EtfConstants.SmallIntegerExt);
        EtfMemory.FromArray(container.ContainedData).WriteByte(v);
        return container;
    }
    
    public static implicit operator byte(EtfContainer v)
    {
        v.EnforceIsType(EtfConstants.SmallIntegerExt);
        return (byte)EtfMemory.FromArray(v.ContainedData).ReadByte();
    }
}