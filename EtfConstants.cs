namespace EtfDotNet;

public enum EtfConstants : byte
{
    VersionNumber = 131,
    
    NewFloatExt =  70,
    SmallIntegerExt = 97,
    IntegerExt = 98,
    AtomExt = 100,
    SmallTupleExt = 104,
    LargeTupleExt = 105,
    NilExt = 106,
    StringExt = 107,
    ListExt = 108,
    BinaryExt = 109,
    SmallBigExt = 110,
    SmallAtomExt = 115,
    MapExt = 116,
}