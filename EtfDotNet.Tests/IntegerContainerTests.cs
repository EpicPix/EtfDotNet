using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class IntegerContainerTests
{
    [Fact]
    public void EtfToIntegerTest()
    {
        var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.IntegerExt, 0x10, 0x20, 0x30, 0x40}));
        Assert.Equal(EtfConstants.IntegerExt, atom.Type);
        Assert.Equal(0x10203040, (int) atom);
    }

    [Fact]
    public void IntegerToEtfTest()
    {
        var atom = (EtfContainer) 0x51827364;
        Assert.Equal(EtfConstants.IntegerExt, atom.Type);
        Assert.Equal(0x51827364, (int) atom);
        Assert.Equal(5, EtfEncoder.CalculateTypeSize(atom));
        var arr = new byte[5];
        EtfEncoder.EncodeType(atom, EtfMemory.FromArray(arr));
        Assert.True(arr.SequenceEqual(new byte[]{(byte) EtfConstants.IntegerExt, 0x51, 0x82, 0x73, 0x64}));
    }

    [Fact]
    public void IntegerSizeTest()
    {
        Assert.Equal(6, EtfFormat.GetPackedSize(1));
        Assert.Equal(6, EtfFormat.GetPackedSize(1000));
        Assert.Equal(6, EtfFormat.GetPackedSize(1000000));
    }
    
}