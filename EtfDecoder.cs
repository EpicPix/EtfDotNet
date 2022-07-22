namespace EtfDotNet;

public static class EtfDecoder
{
    public static EtfContainer DecodeType(EtfMemory input)
    {
        var typeId = input.ReadConstant();
        if (typeId == EtfConstants.NewFloatExt)
        {
            return input.ReadContainer(8, typeId);
        }
        if (typeId == EtfConstants.SmallIntegerExt)
        {
            return input.ReadContainer(1, typeId);
        }
        if (typeId == EtfConstants.IntegerExt)
        {
            return input.ReadContainer(4, typeId);
        }
        if (typeId == EtfConstants.BinaryExt)
        {
            var len = input.ReadUInt();
            return input.ReadContainer((int)len, typeId);
        }
        if (typeId == EtfConstants.SmallBigExt)
        {
            var len = input.ReadByte();
            return input.ReadContainer(len + 1, typeId);
        }
        if (typeId == EtfConstants.StringExt)
        {
            var len = input.ReadUShort();
            return input.ReadContainer(len, typeId);
        }
        if (typeId == EtfConstants.AtomExt)
        {
            var len = input.ReadUShort();
            return input.ReadContainer(len, typeId);
        }
        if (typeId == EtfConstants.SmallAtomExt)
        {
            var len = input.ReadByte();
            return input.ReadContainer(len, EtfConstants.AtomExt);
        }
        if (typeId == EtfConstants.SmallTupleExt)
        {
            return DecodeTuple(input, typeId, (uint) input.ReadByte());
        }
        if (typeId == EtfConstants.LargeTupleExt)
        {
            return DecodeTuple(input, typeId, input.ReadUInt());
        }
        if (typeId == EtfConstants.NilExt)
        {
            return EtfContainer.Nil;
        }
        if (typeId == EtfConstants.ListExt)
        {
            return DecodeList(input);
        }
        if (typeId == EtfConstants.MapExt)
        {
            return DecodeMap(input);
        }
        throw new EtfException($"Unknown type {typeId}");
    }

    public static EtfContainer DecodeTuple(EtfMemory input, EtfConstants typeId, uint length)
    {
        var tuple = new EtfTuple(length);
        for (var i = 0u; i < length; i++)
        {
            tuple[i] = DecodeType(input);
        }
        return EtfContainer.AsContainer(tuple, typeId);
    }

    public static EtfContainer DecodeList(EtfMemory input)
    {
        var length = input.ReadUInt();
        var list = new EtfList();
        for (var i = 0u; i < length; i++)
        {
            list.Add(DecodeType(input));
        }
        if (input.ReadConstant() != EtfConstants.NilExt)
        {
            throw new EtfException("Expected NilExt");
        }
        return EtfContainer.AsContainer(list, EtfConstants.ListExt);
    }

    public static EtfContainer DecodeMap(EtfMemory input)
    {
        var kvLength = input.ReadUInt();
        var map = new EtfMap();
        for (var i = 0u; i < kvLength; i++)
        {
            var key = DecodeType(input);
            var value = DecodeType(input);
            map.Add((key, value));
        }
        return EtfContainer.AsContainer(map, EtfConstants.MapExt);
    }
}