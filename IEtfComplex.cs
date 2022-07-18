namespace EtfDotNet;

public interface IEtfComplex
{
    public int GetSize();
    public void Serialize(EtfMemory memory);
}