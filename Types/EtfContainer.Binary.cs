using System.Text;

namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public static implicit operator EtfContainer(ArraySegment<byte> v)
    {
        var container = Make(v.Count, EtfConstants.BinaryExt);
        EtfMemory.FromArray(container.ContainedData).Write(v);
        return container;
    }
    
    public static implicit operator ArraySegment<byte>(EtfContainer v)
    {
        v.EnforceIsType(EtfConstants.BinaryExt);
        return v.ContainedData;
    }
}