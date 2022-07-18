namespace EtfDotNet;

public interface IEtfComplex : IDisposable
{
    public int GetSize();
    public void Serialize(EtfMemory memory);
}