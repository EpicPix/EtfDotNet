using System.Numerics;

namespace EtfDotNet.Types;

public class EtfBig : EtfType
{
    public readonly BigInteger Number;

    public EtfBig(BigInteger number)
    {
        Number = number;
    }
}