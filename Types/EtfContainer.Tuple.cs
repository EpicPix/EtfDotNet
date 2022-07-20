using System.Text;

namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public EtfTuple AsTuple()
    {
        EnforceIsType(EtfConstants.SmallTupleExt);
        return (EtfTuple)_complexData!;
    }
    
    public static implicit operator EtfContainer(EtfTuple tuple)
    {
        if (tuple.Count > 255)
        {
            throw new EtfException("LargeTupleExt not implemented yet");
        }
        return AsContainer(tuple, EtfConstants.SmallTupleExt);
    }
}