namespace EtfDotNet.Poco;

public class EtfNameAttribute : Attribute
{
    public EtfNameAttribute(string serializedName)
    {
        SerializedName = serializedName;
    }

    public string SerializedName { get; set; }
}