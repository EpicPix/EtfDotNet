using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EtfDotNet;
using EtfDotNet.Json;
using EtfDotNet.Poco;
using EtfDotNet.Types;

// // Console.WriteLine(EtfSerializer.Deserialize<long>(345));
//
// // var test = new EtfMap()
// // {
// //     {"its not a property", "abc"}
// // };
// var tuple = new EtfTuple(16);
// tuple[0] = "a";
// tuple[1] = "b";
// tuple[2] = "c";
// tuple[3] = "d";
// tuple[4] = "e";
// tuple[5] = "f";
// tuple[6] = "g";
// tuple[7] = "h";
// tuple[8] = "i";
// tuple[9] = "j";
// tuple[10] = "k";
// tuple[11] = "l";
// tuple[12] = "m";
// tuple[13] = "n";
// tuple[14] = "o";
// tuple[15] = "p";
// var test = (EtfContainer) tuple;
// // var t = EtfSerializer.Deserialize<(string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string)>(test);
// // Console.WriteLine($"{t} / {t.GetType()}");
//
// var map = new EtfMap();
// map.Add("test", "abc");
// map.Add("other", "def");
// map.Add("v", "ghi");
// map.Add("nope", "jkl");
// var custom = EtfSerializer.Deserialize<IDictionary<string, string>>(map);
// Console.WriteLine("-----");
// Console.WriteLine(custom.GetType());
// Console.WriteLine(custom["test"]);
// Console.WriteLine(custom["v"]);
// Console.WriteLine(custom["nope"]);
// Console.WriteLine("=====");
// var e = custom.GetEnumerator();
// while(e.MoveNext()) {
//     Console.WriteLine($"{e.Current.Key} : {e.Current.Value}");
// }
// Console.WriteLine("-----");
//
// // Console.WriteLine(clz.ThisIsAField);
//
// // Console.WriteLine(EtfJson.ConvertEtfToJson(EtfSerializer.Serialize(new CustomClass())));
//
// class CustomClass
// {
//     public string test;
//     [EtfName("other")] public string v;
//     [EtfIgnore] public string nope = "not set";
// }


var res = EtfSerializer.Deserialize<bool>(EtfContainer.FromAtom("true"));

Fixture fixture = new Fixture();

var obj = fixture.Create<CustomClass2>();

var serialized = EtfSerializer.Serialize(obj);

Console.WriteLine(EtfJson.ConvertEtfToJson(serialized));

var deserialized = EtfSerializer.Deserialize<CustomClass2>(serialized);

Console.WriteLine(JsonSerializer.Serialize(deserialized, new JsonSerializerOptions(){WriteIndented = true, IncludeFields = true}));

class CustomClass2
{
    public string test;
    public Dictionary<string, long> niceDictionary { get; set; }
    public Dictionary<string, int> nicerDictionary { get; set; }
    public Dictionary<string, DateTime> nicestDictionary { get; set; }
    public Dictionary<string, byte[]> betterDictionary { get; set; }
    [EtfName("betterDictionary")] public int notevenadictionary { get; set; }
    public Dictionary<string, (int, long, bool, string, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int)> bestDictionary { get; set; }
    [EtfName("other")] public string v;
    [EtfIgnore] public string nope = "not set";
}

// var data = new byte[]
// {
//     131,116,0,0,0,4,100,0,1,100,116,0,0,0,18,109,0,0,0,11,97,116,116,97,99,104,109,101,110,116,115,106,109,0,0,0,6,97,117,116,104,111,114,116,0,0,0,6,109,0,0,0,6,97,118,97,116,97,114,109,0,0,0,32,51,53,55,55,50,48,49,53,49,57,50,102,57,100,52,98,97,48,97,48,52,101,49,99,56,51,98,55,52,48,98,51,109,0,0,0,17,97,118,97,116,97,114,95,100,101,99,111,114,97,116,105,111,110,100,0,3,110,105,108,109,0,0,0,13,100,105,115,99,114,105,109,105,110,97,116,111,114,109,0,0,0,4,53,57,53,57,109,0,0,0,2,105,100,110,8,0,4,0,4,94,97,5,159,4,109,0,0,0,12,112,117,98,108,105,99,95,102,108,97,103,115,97,64,109,0,0,0,8,117,115,101,114,110,97,109,101,109,0,0,0,7,69,112,105,99,80,105,120,109,0,0,0,10,99,104,97,110,110,101,108,95,105,100,110,8,0,70,128,130,125,167,187,218,12,109,0,0,0,10,99,111,109,112,111,110,101,110,116,115,106,109,0,0,0,7,99,111,110,116,101,110,116,109,0,0,0,12,97,98,99,46,46,32,49,32,116,101,115,116,109,0,0,0,16,101,100,105,116,101,100,95,116,105,109,101,115,116,97,109,112,100,0,3,110,105,108,109,0,0,0,6,101,109,98,101,100,115,106,109,0,0,0,5,102,108,97,103,115,97,0,109,0,0,0,2,105,100,110,8,0,50,64,4,13,50,220,219,13,109,0,0,0,16,109,101,110,116,105,111,110,95,101,118,101,114,121,111,110,101,100,0,5,102,97,108,115,101,109,0,0,0,13,109,101,110,116,105,111,110,95,114,111,108,101,115,106,109,0,0,0,8,109,101,110,116,105,111,110,115,106,109,0,0,0,5,110,111,110,99,101,109,0,0,0,18,57,57,56,54,51,51,56,52,57,50,53,52,53,55,54,49,50,56,109,0,0,0,6,112,105,110,110,101,100,100,0,5,102,97,108,115,101,109,0,0,0,18,114,101,102,101,114,101,110,99,101,100,95,109,101,115,115,97,103,101,100,0,3,110,105,108,109,0,0,0,9,116,105,109,101,115,116,97,109,112,109,0,0,0,32,50,48,50,50,45,48,55,45,49,56,84,49,54,58,53,52,58,50,51,46,53,52,48,48,48,48,43,48,48,58,48,48,109,0,0,0,3,116,116,115,100,0,5,102,97,108,115,101,109,0,0,0,4,116,121,112,101,97,0,100,0,2,111,112,97,0,100,0,1,115,97,6,100,0,1,116,100,0,14,77,69,83,83,65,71,69,95,67,82,69,65,84,69
// };
//
// var container = EtfFormat.Unpack(EtfMemory.FromArray(data));
//
// var dArr = new byte[EtfFormat.GetPackedSize(container)];
//
// EtfFormat.Pack(container, EtfMemory.FromArray(dArr));
//
// Console.WriteLine(string.Join(",", dArr));
//
// Console.WriteLine($"Original {data.Length}, Packed {data.Length}. Original == Packed {data.SequenceEqual(dArr)}");
//
// Console.WriteLine(container.GetSerializedSize());
//
//
//
// EtfContainer testContainer = new EtfMap()
// {
//     {"a", 1},
//     {"b", new EtfMap(){
//         {"asdfa", 2}
//     }}
// };
//
// Console.WriteLine(EtfJson.ConvertEtfToJson(testContainer));

// BenchmarkRunner.Run<EtfBenchmark>();

[MemoryDiagnoser]
public class EtfBenchmark
{
    private byte[] data = {
        131,116,0,0,0,4,100,0,1,100,116,0,0,0,18,109,0,0,0,11,97,116,116,97,99,104,109,101,110,116,115,106,109,0,0,0,6,97,117,116,104,111,114,116,0,0,0,6,109,0,0,0,6,97,118,97,116,97,114,109,0,0,0,32,51,53,55,55,50,48,49,53,49,57,50,102,57,100,52,98,97,48,97,48,52,101,49,99,56,51,98,55,52,48,98,51,109,0,0,0,17,97,118,97,116,97,114,95,100,101,99,111,114,97,116,105,111,110,100,0,3,110,105,108,109,0,0,0,13,100,105,115,99,114,105,109,105,110,97,116,111,114,109,0,0,0,4,53,57,53,57,109,0,0,0,2,105,100,110,8,0,4,0,4,94,97,5,159,4,109,0,0,0,12,112,117,98,108,105,99,95,102,108,97,103,115,97,64,109,0,0,0,8,117,115,101,114,110,97,109,101,109,0,0,0,7,69,112,105,99,80,105,120,109,0,0,0,10,99,104,97,110,110,101,108,95,105,100,110,8,0,70,128,130,125,167,187,218,12,109,0,0,0,10,99,111,109,112,111,110,101,110,116,115,106,109,0,0,0,7,99,111,110,116,101,110,116,109,0,0,0,12,97,98,99,46,46,32,49,32,116,101,115,116,109,0,0,0,16,101,100,105,116,101,100,95,116,105,109,101,115,116,97,109,112,100,0,3,110,105,108,109,0,0,0,6,101,109,98,101,100,115,106,109,0,0,0,5,102,108,97,103,115,97,0,109,0,0,0,2,105,100,110,8,0,50,64,4,13,50,220,219,13,109,0,0,0,16,109,101,110,116,105,111,110,95,101,118,101,114,121,111,110,101,100,0,5,102,97,108,115,101,109,0,0,0,13,109,101,110,116,105,111,110,95,114,111,108,101,115,106,109,0,0,0,8,109,101,110,116,105,111,110,115,106,109,0,0,0,5,110,111,110,99,101,109,0,0,0,18,57,57,56,54,51,51,56,52,57,50,53,52,53,55,54,49,50,56,109,0,0,0,6,112,105,110,110,101,100,100,0,5,102,97,108,115,101,109,0,0,0,18,114,101,102,101,114,101,110,99,101,100,95,109,101,115,115,97,103,101,100,0,3,110,105,108,109,0,0,0,9,116,105,109,101,115,116,97,109,112,109,0,0,0,32,50,48,50,50,45,48,55,45,49,56,84,49,54,58,53,52,58,50,51,46,53,52,48,48,48,48,43,48,48,58,48,48,109,0,0,0,3,116,116,115,100,0,5,102,97,108,115,101,109,0,0,0,4,116,121,112,101,97,0,100,0,2,111,112,97,0,100,0,1,115,97,6,100,0,1,116,100,0,14,77,69,83,83,65,71,69,95,67,82,69,65,84,69
    };

    private byte[] outData;

    private EtfContainer _rData;

    [GlobalSetup]
    public void Setup()
    {
        _rData = EtfFormat.Unpack(EtfMemory.FromArray(data));
        outData = new byte[data.Length];
    }

    [Benchmark]
    public int Deserialize()
    {
        using var unpacked = EtfFormat.Unpack(EtfMemory.FromArray(data));
        return unpacked.GetHashCode();
    }

    [Benchmark]
    public int Serialize()
    {
        EtfFormat.Pack(_rData, EtfMemory.FromArray(outData));
        return outData[0];
    }
}
