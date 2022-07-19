using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class ListContainerTests
{

    [Fact]
    public void EtfToListTest()
    {
        using var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.ListExt, 0, 0, 0, 0, (byte) EtfConstants.NilExt}));
        Assert.Equal(EtfConstants.ListExt, atom.Type);
        var list = atom.AsList();
        Assert.Empty(list);
    }

    [Fact]
    public void EtfToListWithNilTest()
    {
        using var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.ListExt, 0, 0, 0, 1, (byte) EtfConstants.NilExt, (byte) EtfConstants.NilExt}));
        Assert.Equal(EtfConstants.ListExt, atom.Type);
        var list = atom.AsList();
        Assert.Single(list);
        Assert.Equal(EtfConstants.NilExt, list[0].Type);
        Assert.Equal(EtfContainer.Nil, list[0]);
    }
    
}