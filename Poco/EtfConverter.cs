using System.Collections;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using EtfDotNet.Types;

namespace EtfDotNet.Poco;

public class EtfConverter
{
    public static EtfContainer ToEtf<T>(T value)
    {
        if (value is null)
        {
            return EtfContainer.FromAtom("nil");
        }

        if (value is bool v_bool)
        {
            return v_bool ? EtfContainer.FromAtom("true") : EtfContainer.FromAtom("false");
        }
        if (value is int v_int)
        {
            return v_int;
        }
        if (value is BigInteger v_bigi)
        {
            return v_bigi;
        }
        if (value is string v_str)
        {
            return v_str;
        }
        if (value is byte v_byte)
        {
            return v_byte;
        }
        if (value is double v_dbl)
        {
            return v_dbl;
        }

        Type type = value.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>) || value is IDictionary)
        {
            var v_dict = value as IDictionary;
            var map = new EtfMap();
            foreach (DictionaryEntry entry in v_dict)
            {
                map.Add((ToEtf(entry.Key), ToEtf(entry.Value)));
            }

            return map;
        }
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || value is IEnumerable)
        {
            var v_enu = value as IEnumerable;
            var list = new EtfList();
            foreach (var item in v_enu)
            {
                list.Add(ToEtf(item));
            }
            return list;
        }
        if (value is ITuple v_tup)
        {
            var tup = new EtfTuple((uint)v_tup.Length);
            for (int i = 0; i < v_tup.Length; i++)
            {
                tup[i] = ToEtf(v_tup[i]);
            }
            return tup;
        }
        // complex object map type
        var etfMap = new EtfMap();
        var props = type.GetProperties();
        foreach (var propertyInfo in props)
        {
            var val = SerializeMember(propertyInfo, propertyInfo.GetValue(value)!);
            if(val.HasValue) etfMap.Add(val.Value);
        }
        var fields = type.GetFields();
        foreach (var fieldInfo in fields)
        {
            var val = SerializeMember(fieldInfo, fieldInfo.GetValue(value)!);
            if(val.HasValue) etfMap.Add(val.Value);
        }

        return etfMap;
    }

    private static (EtfContainer, EtfContainer)? SerializeMember(MemberInfo info, object value)
    {
        if (info.GetCustomAttribute<EtfIgnoreAttribute>() is not null) return null;
        string name = info.Name;
        var etfName = info.GetCustomAttribute<EtfNameAttribute>();
        if (etfName is not null)
        {
            name = etfName.SerializedName;
        }

        return (name, ToEtf(value));
    }

    public static T? ToObject<T>(EtfContainer container)
    {
        return (T?) ToObject(container, typeof(T));
    }

    // ReSharper disable RedundantCast
    // ReSharper disable HeapView.BoxingAllocation
    public static object? ToObject(EtfContainer container, Type t)
    {
        if (typeof(EtfContainer).IsAssignableFrom(t))
        {
            return container.As(t);
        }
        if (container.Type == EtfConstants.AtomExt)
        {
            var name = container.ToAtom();
            if (name is "true" or "false")
            {
                return (name == "true").As(t);
            }
            if (name is "nil")
            {
                if (Nullable.GetUnderlyingType(t) != null || !t.IsValueType)
                {
                    return null;
                }
                throw new EtfException("Cannot convert non-nullable object to null");
            }
            return name.As(t);
        }
        if (container.Type == EtfConstants.BinaryExt)
        {
            if (typeof(string).IsAssignableFrom(t))
            {
                return Encoding.UTF8.GetString((ArraySegment<byte>) container).As(t);
            }
            if (typeof(byte[]).IsAssignableFrom(t))
            {
                return ((ArraySegment<byte>) container).ToArray().As(t);
            }
            if (typeof(ArraySegment<byte>).IsAssignableFrom(t))
            {
                return ((ArraySegment<byte>) container).As(t);
            }
            throw new EtfException($"Cannot convert BinaryExt to {t}");
        }
        if (container.Type == EtfConstants.StringExt)
        {
            if (typeof(string).IsAssignableFrom(t))
            {
                return ((string) container).As(t);
            }
            throw new EtfException($"Cannot convert StringExt to {t}");
        }
        if (container.Type == EtfConstants.SmallIntegerExt)
        {
            byte data = container;
            if (typeof(byte)      .IsAssignableFrom(t)) return ((byte)       data).As(t);
            if (typeof(short)     .IsAssignableFrom(t)) return ((short)      data).As(t);
            if (typeof(ushort)    .IsAssignableFrom(t)) return ((ushort)     data).As(t);
            if (typeof(int)       .IsAssignableFrom(t)) return ((int)        data).As(t);
            if (typeof(uint)      .IsAssignableFrom(t)) return ((uint)       data).As(t);
            if (typeof(long)      .IsAssignableFrom(t)) return ((long)       data).As(t);
            if (typeof(ulong)     .IsAssignableFrom(t)) return ((ulong)      data).As(t);
            if (typeof(BigInteger).IsAssignableFrom(t)) return ((BigInteger) data).As(t);
            if (typeof(string)    .IsAssignableFrom(t)) return data.ToString()    .As(t);
            throw new EtfException($"Cannot convert SmallIntegerExt to {t}");
        }
        if (container.Type == EtfConstants.IntegerExt)
        {
            int data = container;
            if (typeof(int)       .IsAssignableFrom(t)) return ((int)        data).As(t);
            if (typeof(uint)      .IsAssignableFrom(t)) return ((uint)       data).As(t);
            if (typeof(long)      .IsAssignableFrom(t)) return ((long)       data).As(t);
            if (typeof(ulong)     .IsAssignableFrom(t)) return ((ulong)      data).As(t);
            if (typeof(BigInteger).IsAssignableFrom(t)) return ((BigInteger) data).As(t);
            if (typeof(string)    .IsAssignableFrom(t)) return data.ToString()    .As(t);
            throw new EtfException($"Cannot convert IntegerExt to {t}");
        }
        if (container.Type == EtfConstants.SmallBigExt)
        {
            BigInteger data = container;
            if (typeof(BigInteger).IsAssignableFrom(t)) return ((BigInteger) data).As(t);
            if (typeof(string)    .IsAssignableFrom(t)) return data.ToString()    .As(t);
            throw new EtfException($"Cannot convert SmallBigExt to {t}");
        }
        if (container.Type == EtfConstants.NewFloatExt)
        {
            double data = container;
            if (typeof(double).IsAssignableFrom(t))
            {
                return data.As(t);
            }
            throw new EtfException($"Cannot convert NewFloatExt to {t}");
        }
        if (container.Type is EtfConstants.SmallTupleExt or EtfConstants.LargeTupleExt)
        {
            return ToTuple(container.AsTuple(), t);
        }
        throw new NotImplementedException("TODO: Add: NilExt ListExt MapExt");
    }

    internal static long GetTupleLength(Type t)
    {
        if (!typeof(ITuple).IsAssignableFrom(t)) return 1;
        var amt = t.GenericTypeArguments.LongLength;
        if (amt == 8)
        {
            return 7 + GetTupleLength(t.GenericTypeArguments[7]);
        }
        return amt;
    }

    internal static ITuple CreateTuple(Type t, object? value1 = null, object? value2 = null, object? value3 = null, object? value4 = null, object? value5 = null, object? value6 = null, object? value7 = null, object? value8 = null)
    {
        var length = GetTupleLength(t);
        if (length == 1)
        {
            return (ITuple) Activator.CreateInstance(t, value1)!;
        }
        throw new NotImplementedException();
    }

    internal static ITuple ToTuple(EtfTuple tuple, Type t)
    {
        if (!typeof(ITuple).IsAssignableFrom(t))
        {
            throw new EtfException($"Cannot convert EtfTuple to {t}, expected an ITuple or a subclass of it");
        }
        var len = GetTupleLength(t);
        if (len != tuple.Length)
        {
            throw new EtfException($"Tuple lengths are not the same, expected {tuple.Length} got {len}");
        }
        // some debug code
        // Console.WriteLine(GetTupleLength(t));
        // Console.WriteLine(CreateTuple(t, ToObject(tuple[0], t.GenericTypeArguments[0])));
        throw new NotImplementedException();
    }
}

internal static class ObjectExtensions
{

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    internal static object As(this object obj, Type type)
    {
        try
        {
            return Convert.ChangeType(obj, type);
        } catch (FormatException)
        {
            throw new InvalidCastException($"Unable to cast object of type '{obj.GetType()}' to type '{type}'.");
        }
    }
}