namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public EtfTuple AsTuple()
    {
        if (Type == EtfConstants.LargeTupleExt)
        {
            return (EtfTuple) _complexData!;
        }
        EnforceIsType(EtfConstants.SmallTupleExt);
        return (EtfTuple)_complexData!;
    }
    
    public static implicit operator EtfContainer(EtfTuple tuple)
    {
        if (tuple.Count > 255)
        {
            return AsContainer(tuple, EtfConstants.LargeTupleExt);
        }
        return AsContainer(tuple, EtfConstants.SmallTupleExt);
    }
}