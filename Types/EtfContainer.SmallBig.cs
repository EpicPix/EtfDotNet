using System.Numerics;
using System.Text;

namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public static implicit operator EtfContainer(BigInteger big)
    {
        var sign = big.Sign == decimal.MinusOne;
        var bytes = sign ? (-big).ToByteArray(true) : big.ToByteArray(true);
        if (bytes.Length > 255)
        {
            throw new EtfException("Cannot encode number with more than 255 bytes");
        }
        var container = Make(1 + bytes.Length, EtfConstants.SmallBigExt);
        var mem = EtfMemory.FromArray(container.ContainedData);
        mem.WriteByte((byte)(sign ? 1 : 0));
        mem.Write(bytes);
        return container;
    }
    
    public static implicit operator BigInteger(EtfContainer v)
    {
        v.EnforceIsType(EtfConstants.SmallBigExt);
        var sign = v.ContainedData[0];
        var num = new BigInteger(v.ContainedData.Slice(1), isUnsigned: true);
        if (sign == 1) num = -num;
        return num;
    }
}