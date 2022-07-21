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

    // ReSharper disable RedundantCast
    // ReSharper disable HeapView.BoxingAllocation
    public static T? ToObject<T>(EtfContainer container)
    {
        if (typeof(EtfContainer).IsAssignableFrom(typeof(T)))
        {
            return container.As<T>();
        }
        if (container.Type == EtfConstants.AtomExt)
        {
            var name = container.ToAtom();
            if (name is "true" or "false")
            {
                return (name == "true").As<T>();
            }
            if (name is "nil")
            {
                if (Nullable.GetUnderlyingType(typeof(T)) != null || !typeof(T).IsValueType)
                {
                    return default;
                }
                throw new EtfException("Cannot convert non-nullable object to null");
            }
            return name.As<T>();
        }
        if (container.Type == EtfConstants.BinaryExt)
        {
            if (typeof(string).IsAssignableFrom(typeof(T)))
            {
                return Encoding.UTF8.GetString((ArraySegment<byte>) container).As<T>();
            }
            if (typeof(byte[]).IsAssignableFrom(typeof(T)))
            {
                return ((ArraySegment<byte>) container).ToArray().As<T>();
            }
            if (typeof(ArraySegment<byte>).IsAssignableFrom(typeof(T)))
            {
                return ((ArraySegment<byte>) container).As<T>();
            }
            throw new EtfException($"Cannot convert BinaryExt to {typeof(T)}");
        }
        if (container.Type == EtfConstants.StringExt)
        {
            if (typeof(string).IsAssignableFrom(typeof(T)))
            {
                return ((string) container).As<T>();
            }
            throw new EtfException($"Cannot convert StringExt to {typeof(T)}");
        }
        if (container.Type == EtfConstants.SmallIntegerExt)
        {
            byte data = container;
            if (typeof(byte)      .IsAssignableFrom(typeof(T))) return ((byte)       data).As<T>();
            if (typeof(short)     .IsAssignableFrom(typeof(T))) return ((short)      data).As<T>();
            if (typeof(ushort)    .IsAssignableFrom(typeof(T))) return ((ushort)     data).As<T>();
            if (typeof(int)       .IsAssignableFrom(typeof(T))) return ((int)        data).As<T>();
            if (typeof(uint)      .IsAssignableFrom(typeof(T))) return ((uint)       data).As<T>();
            if (typeof(long)      .IsAssignableFrom(typeof(T))) return ((long)       data).As<T>();
            if (typeof(ulong)     .IsAssignableFrom(typeof(T))) return ((ulong)      data).As<T>();
            if (typeof(BigInteger).IsAssignableFrom(typeof(T))) return ((BigInteger) data).As<T>();
            if (typeof(string)    .IsAssignableFrom(typeof(T))) return data.ToString()    .As<T>();
            throw new EtfException($"Cannot convert SmallIntegerExt to {typeof(T)}");
        }
        if (container.Type == EtfConstants.IntegerExt)
        {
            int data = container;
            if (typeof(int)       .IsAssignableFrom(typeof(T))) return ((int)        data).As<T>();
            if (typeof(uint)      .IsAssignableFrom(typeof(T))) return ((uint)       data).As<T>();
            if (typeof(long)      .IsAssignableFrom(typeof(T))) return ((long)       data).As<T>();
            if (typeof(ulong)     .IsAssignableFrom(typeof(T))) return ((ulong)      data).As<T>();
            if (typeof(BigInteger).IsAssignableFrom(typeof(T))) return ((BigInteger) data).As<T>();
            if (typeof(string)    .IsAssignableFrom(typeof(T))) return data.ToString()    .As<T>();
            throw new EtfException($"Cannot convert IntegerExt to {typeof(T)}");
        }
        if (container.Type == EtfConstants.SmallBigExt)
        {
            BigInteger data = container;
            if (typeof(BigInteger).IsAssignableFrom(typeof(T))) return ((BigInteger) data).As<T>();
            if (typeof(string)    .IsAssignableFrom(typeof(T))) return data.ToString()    .As<T>();
            throw new EtfException($"Cannot convert SmallBigExt to {typeof(T)}");
        }
        if (container.Type == EtfConstants.NewFloatExt)
        {
            double data = container;
            if (typeof(double).IsAssignableFrom(typeof(T)))
            {
                return data.As<T>();
            }
            throw new EtfException($"Cannot convert NewFloatExt to {typeof(T)}");
        }
        throw new NotImplementedException("TODO");
    }
}

internal static class ObjectExtensions
{

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    internal static T As<T>(this object obj)
    {
        return (T) obj;
    }
}