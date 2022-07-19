using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class UnknownTypeTest
{
    private readonly EtfContainer UnknownType = EtfContainer.AsContainer(ArraySegment<byte>.Empty, (EtfConstants) 255);
    
    [Fact]
    public void UnpackInvalidTypeTest()
    {
        Assert.Throws<EtfException>(() => EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[] { 255 })));
    }
    
    [Fact]
    public void PackUnknownTypeTest()
    {
        Assert.Throws<EtfException>(() => EtfFormat.Pack(UnknownType));
    }
    
    [Fact]
    public void EncodeUnknownTypeTest()
    {
        Assert.Throws<EtfException>(() => EtfEncoder.EncodeType(UnknownType, EtfMemory.FromArray(new byte[8])));
    }
    
    [Fact]
    public void GetLengthOfUnknownTypeTest()
    {
        Assert.Throws<EtfException>(() => EtfFormat.GetPackedSize(UnknownType));
    }
    
}