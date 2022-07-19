using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class BigContainerTests
{
    
    [Fact]
    public void EtfToBinaryTest()
    {
        using var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.BinaryExt, 0, 0, 0, 4, 0, 1, 2, 3}));
        Assert.Equal(EtfConstants.BinaryExt, atom.Type);
        Assert.True(((ArraySegment<byte>) atom).SequenceEqual(new byte[]{0, 1, 2, 3}));
    }

    [Fact]
    public void BinaryToEtfTest()
    {
        using var atom = (EtfContainer) new byte[] {0, 1, 2, 3, 4, 5, 6, 7};
        Assert.Equal(EtfConstants.BinaryExt, atom.Type);
        Assert.True(((ArraySegment<byte>) atom).SequenceEqual(new byte[]{0, 1, 2, 3, 4, 5, 6, 7}));
        Assert.Equal(13, EtfEncoder.CalculateTypeSize(atom));
        var arr = new byte[13];
        EtfEncoder.EncodeType(atom, EtfMemory.FromArray(arr));
        Assert.True(arr.SequenceEqual(new byte[]{(byte) EtfConstants.BinaryExt, 0, 0, 0, 8, 0, 1, 2, 3, 4, 5, 6, 7}));
    }

    [Fact]
    public void BinarySizeTest()
    {
        Assert.Equal(9, EtfFormat.GetPackedSize(new byte[] {0, 1, 2}));
        Assert.Equal(14, EtfFormat.GetPackedSize(new byte[] {0, 1, 2, 3, 4, 5, 6, 7}));
        Assert.Equal(17, EtfFormat.GetPackedSize(new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10}));
    }
    
}