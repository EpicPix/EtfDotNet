using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class AtomContainerTests
{

    [Fact]
    public void EtfToAtomTest()
    {
        using var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.AtomExt, 0, 4, (byte) 't', (byte) 'e', (byte) 's', (byte) 't'}));
        Assert.Equal(EtfConstants.AtomExt, atom.Type);
        Assert.Equal("test", atom.ToAtom());
    }

    [Fact]
    public void AtomToEtfTest()
    {
        using var atom = (EtfContainer) "test";
        Assert.Equal(EtfConstants.AtomExt, atom.Type);
        Assert.Equal("test", atom.ToAtom());
        Assert.Equal(7, EtfEncoder.CalculateTypeSize(atom));
        var arr = new byte[7];
        EtfEncoder.EncodeType(atom, EtfMemory.FromArray(arr));
        Assert.True(arr.SequenceEqual(new byte[]{(byte) EtfConstants.AtomExt, 0, 4, (byte) 't', (byte) 'e', (byte) 's', (byte) 't'}));
    }

    [Fact]
    public void ContainerToAtomTest()
    {
        using EtfContainer val = "test";
        Assert.Equal(EtfConstants.AtomExt, val.Type);
        string got = val;
        Assert.Equal("test", got);
    }

    [Fact]
    public void AtomLengthTest()
    {
        Assert.Throws<EtfException>(() => EtfContainer.FromAtom("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"));
    }

    [Fact]
    public void AtomSizeTest()
    {
        Assert.Equal(4, EtfFormat.GetPackedSize(""));
        Assert.Equal(8, EtfFormat.GetPackedSize("test"));
        Assert.Equal(16, EtfFormat.GetPackedSize("testtesttest"));
    }
    
}