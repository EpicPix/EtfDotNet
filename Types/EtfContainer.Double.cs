using System.Text;

namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public static implicit operator EtfContainer(double v)
    {
        var container = Make(8, EtfConstants.NewFloatExt);
        container.ContainedData.WriteULong(BitConverter.DoubleToUInt64Bits(v));
        return container;
    }
    
    public static implicit operator double(EtfContainer v)
    {
        v.EnforceIsType(EtfConstants.NewFloatExt);
        return BitConverter.UInt64BitsToDouble(v.ContainedData.ReadULong());
    }
}