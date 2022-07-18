using System.Numerics;

namespace EtfDotNet.Types;

public struct EtfBig : IEtfType
{
    public readonly BigInteger Number;

    public EtfBig(BigInteger number)
    {
        Number = number;
    }
}