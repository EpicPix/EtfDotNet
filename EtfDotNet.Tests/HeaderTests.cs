using EtfDotNet.Types;
using Xunit;

namespace EtfDotNet.Tests;

public class HeaderTests
{
    [Fact]
    public void UnpackBadVersionTest()
    {
        Assert.Throws<EtfException>(() => EtfFormat.Unpack(new byte[] { 0, (byte) EtfConstants.NilExt }));
    }
    
    [Fact]
    public void UnpackValidVersionTest()
    {
        EtfFormat.Unpack(new[] { (byte) EtfConstants.VersionNumber, (byte) EtfConstants.NilExt });
    }
    
    [Fact]
    public void PackVersionTest()
    {
        var arr = EtfFormat.Pack(EtfContainer.Nil);
        Assert.Equal((byte) EtfConstants.VersionNumber, arr[0]);
    }
}