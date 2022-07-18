using System.Text;

namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public static implicit operator EtfContainer(string v)
    {
        var count = Encoding.Latin1.GetByteCount(v);
        if (count <= 255)
        {
            return FromAtom(v);
        }
        var container = Make(count, EtfConstants.StringExt);
        Encoding.Latin1.GetBytes(v, container.ContainedData);
        return container;
    }
    
    public static implicit operator string(EtfContainer v)
    {
        if (v.Type == EtfConstants.AtomExt)
        {
            return ToAtom(v);
        }
        v.EnforceIsType(EtfConstants.StringExt);
        return Encoding.Latin1.GetString(v.ContainedData);
    }
}