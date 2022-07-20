using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class NewFloatExtTests
{
    [Fact]
    public void EtfToDoubleTest()
    {
        using var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.NewFloatExt, 0x40, 0x59, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}));
        Assert.Equal(EtfConstants.NewFloatExt, atom.Type);
        Assert.Equal(100d, (double) atom);
    }

    [Fact]
    public void DoubleToEtfTest()
    {
        
        using var atom = (EtfContainer) 100d;
        Assert.Equal(EtfConstants.NewFloatExt, atom.Type);
        Assert.Equal(100d, (double) atom);
        Assert.Equal(9, EtfEncoder.CalculateTypeSize(atom));
        var arr = new byte[9];
        EtfEncoder.EncodeType(atom, EtfMemory.FromArray(arr));
        Assert.True(arr.SequenceEqual(new byte[]{(byte) EtfConstants.NewFloatExt, 0x40, 0x59, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}));
    }

    [Fact]
    public void DoubleSizeTest()
    {
        Assert.Equal(10, EtfFormat.GetPackedSize(1d));
        Assert.Equal(10, EtfFormat.GetPackedSize(1000d));
        Assert.Equal(10, EtfFormat.GetPackedSize(1000000d));
    }
    
}