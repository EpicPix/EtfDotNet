using System.Buffers;
using System.Text;
using EtfDotNet.Types;

namespace EtfDotNet;

internal static class EtfEncoder
{
    public static void EncodeType(EtfContainer container, EtfMemory output)
    {
        output.WriteConstant(container.Type);
        var buf = container.Serialize(out var ret);
        output.Write(buf);
        if (ret)
        {
            buf.ReturnShared();
        }
    }
}