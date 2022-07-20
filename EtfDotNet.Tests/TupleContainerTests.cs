using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class TupleContainerTests
{

    [Fact]
    public void WrongTypeToTupleTest()
    {
        Assert.Throws<InvalidCastException>(() => EtfContainer.Nil.AsTuple());
    }

    [Fact]
    public void CorrectTypeToTupleTest()
    {
        var start = new EtfTuple(0);
        using EtfContainer list = start;
        Assert.Equal(start, list.AsTuple());
    }
    
    [Fact]
    public void EtfToTupleEmptyTest()
    {
        using var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.SmallTupleExt, 0x00}));
        Assert.Equal(EtfConstants.SmallTupleExt, atom.Type);
        var tuple = atom.AsTuple();
        Assert.Empty(tuple);
    }
    
    [Fact]
    public void EtfToTupleOneTest()
    {
        using var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.SmallTupleExt, 0x01, (byte) EtfConstants.NilExt}));
        Assert.Equal(EtfConstants.SmallTupleExt, atom.Type);
        var tuple = atom.AsTuple();
        Assert.Single(tuple);
        Assert.Equal(EtfConstants.NilExt, tuple[0].Type);
    }
    
    [Fact]
    public void EtfToTupleLargeOneTest()
    {
        using var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.LargeTupleExt, 0x00, 0x00, 0x00, 0x01, (byte) EtfConstants.NilExt}));
        Assert.Equal(EtfConstants.LargeTupleExt, atom.Type);
        var tuple = atom.AsTuple();
        Assert.Single(tuple);
        Assert.Equal(EtfConstants.NilExt, tuple[0].Type);
    }

    [Fact]
    public void TupleSizeTest()
    {
        //                                                                                                           count   nil atom
        Assert.Equal(3, EtfFormat.GetPackedSize(new EtfTuple(0)));      // 1 + 1 + 1 +   0 * (1 + 2 + 3)
        Assert.Equal(9, EtfFormat.GetPackedSize(new EtfTuple(1)));      // 1 + 1 + 1 +   1 * (1 + 2 + 3)
        Assert.Equal(1533, EtfFormat.GetPackedSize(new EtfTuple(255))); // 1 + 1 + 1 + 255 * (1 + 2 + 3)
        Assert.Equal(1542, EtfFormat.GetPackedSize(new EtfTuple(256))); // 1 + 1 + 4 + 256 * (1 + 2 + 3)
    }
    
}