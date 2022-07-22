namespace EtfDotNet.Types;

public partial struct EtfContainer
{
    public static EtfContainer FromAtom(string value)
    {
        var len = Encoding.Latin1.GetByteCount(value);
        if (len > 255)
        {
            throw new EtfException("Currently cannot encode atom with >255 bytes");
        }
        var container = Make(len, EtfConstants.AtomExt);
        Encoding.Latin1.GetBytes(value, container.ContainedData);
        return container;
    }

    public string ToAtom()
    {
        EnforceIsType(EtfConstants.AtomExt);
        return Encoding.Latin1.GetString(ContainedData);
    }
}