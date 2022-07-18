namespace EtfDotNet;

public interface IEtfComplex : IDisposable
{
    public int GetSize();
    public int GetSerializedSize();
    public void Serialize(EtfMemory memory);
}