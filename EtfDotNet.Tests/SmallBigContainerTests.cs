using System.Numerics;
using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class SmallBigContainerTests
{
    [Fact]
    public void EtfToSmallBigPositiveTest()
    {
        var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.SmallBigExt, 1, 0, 129}));
        Assert.Equal(EtfConstants.SmallBigExt, atom.Type);
        Assert.Equal(new BigInteger(129), (BigInteger) atom);
    }
    
    [Fact]
    public void EtfToSmallBigNegativeTest()
    {
        var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.SmallBigExt, 1, 1, 129}));
        Assert.Equal(EtfConstants.SmallBigExt, atom.Type);
        Assert.Equal(new BigInteger(-129), (BigInteger) atom);
    }
    
    [Fact]
    public void EtfToSmallBigPositiveMultiByteTest()
    {
        var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.SmallBigExt, 3, 0, 129, 16, 1}));
        Assert.Equal(EtfConstants.SmallBigExt, atom.Type);
        Assert.Equal(new BigInteger(69761), (BigInteger) atom);
    }
    
    [Fact]
    public void EtfToSmallBigNegativeMultiByteTest()
    {
        var atom = EtfDecoder.DecodeType(EtfMemory.FromArray(new byte[]{(byte) EtfConstants.SmallBigExt, 3, 1, 129, 16, 1}));
        Assert.Equal(EtfConstants.SmallBigExt, atom.Type);
        Assert.Equal(new BigInteger(-69761), (BigInteger) atom);
    }
    
    
    [Fact]
    public void SmallBigToEtfPositiveTest()
    {
        var atom = (EtfContainer) new BigInteger(129);
        Assert.Equal(EtfConstants.SmallBigExt, atom.Type);
        Assert.Equal(new BigInteger(129), (BigInteger) atom);
        Assert.Equal(4, EtfEncoder.CalculateTypeSize(atom));
        var arr = new byte[4];
        EtfEncoder.EncodeType(atom, EtfMemory.FromArray(arr));
        Assert.True(arr.SequenceEqual(new byte[]{(byte) EtfConstants.SmallBigExt, 1, 0, 129}));
    }
    
    [Fact]
    public void SmallBigToEtfNegativeTest()
    {
        var atom = (EtfContainer) new BigInteger(-129);
        Assert.Equal(EtfConstants.SmallBigExt, atom.Type);
        Assert.Equal(new BigInteger(-129), (BigInteger) atom);
        Assert.Equal(4, EtfEncoder.CalculateTypeSize(atom));
        var arr = new byte[4];
        EtfEncoder.EncodeType(atom, EtfMemory.FromArray(arr));
        Assert.True(arr.SequenceEqual(new byte[]{(byte) EtfConstants.SmallBigExt, 1, 1, 129}));
    }
    
    [Fact]
    public void SmallBigToEtfPositiveMultiByteTest()
    {
        var atom = (EtfContainer) new BigInteger(69761);
        Assert.Equal(EtfConstants.SmallBigExt, atom.Type);
        Assert.Equal(new BigInteger(69761), (BigInteger) atom);
        Assert.Equal(6, EtfEncoder.CalculateTypeSize(atom));
        var arr = new byte[6];
        EtfEncoder.EncodeType(atom, EtfMemory.FromArray(arr));
        Assert.True(arr.SequenceEqual(new byte[]{(byte) EtfConstants.SmallBigExt, 3, 0, 129, 16, 1}));
    }
    
    [Fact]
    public void SmallBigToEtfNegativeMultiByteTest()
    {
        var atom = (EtfContainer) new BigInteger(-69761);
        Assert.Equal(EtfConstants.SmallBigExt, atom.Type);
        Assert.Equal(new BigInteger(-69761), (BigInteger) atom);
        Assert.Equal(6, EtfEncoder.CalculateTypeSize(atom));
        var arr = new byte[6];
        EtfEncoder.EncodeType(atom, EtfMemory.FromArray(arr));
        Assert.True(arr.SequenceEqual(new byte[]{(byte) EtfConstants.SmallBigExt, 3, 1, 129, 16, 1}));
    }

    [Fact]
    public void SmallBigSizeTest()
    {
        Assert.Equal(5, EtfFormat.GetPackedSize((BigInteger) 1));
        Assert.Equal(6, EtfFormat.GetPackedSize((BigInteger) 256));
        Assert.Equal(6, EtfFormat.GetPackedSize((BigInteger) 2048));
        Assert.Equal(5, EtfFormat.GetPackedSize((BigInteger) (-1)));
        Assert.Equal(6, EtfFormat.GetPackedSize((BigInteger) (-256)));
        Assert.Equal(6, EtfFormat.GetPackedSize((BigInteger) (-2048)));
    }
    
}