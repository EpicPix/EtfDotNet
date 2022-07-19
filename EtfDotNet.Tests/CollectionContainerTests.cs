using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class CollectionContainerTests
{

    [Fact]
    public void WrongTypeToListTest()
    {
        Assert.Throws<InvalidCastException>(() => EtfContainer.Nil.AsList());
    }

    [Fact]
    public void CorrectTypeToListTest()
    {
        var start = new EtfList();
        using EtfContainer list = start;
        Assert.Equal(start, list.AsList());
    }

    [Fact]
    public void WrongTypeToMapTest()
    {
        Assert.Throws<InvalidCastException>(() => EtfContainer.Nil.AsMap());
    }

    [Fact]
    public void CorrectTypeToMapTest()
    {
        var start = new EtfMap();
        using EtfContainer list = start;
        Assert.Equal(start, list.AsMap());
    }
    
}