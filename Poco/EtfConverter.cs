using System.Collections;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using EtfDotNet.Types;
using TB.ComponentModel;

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

    // ReSharper disable HeapView.BoxingAllocation
    public static object? ToObject(EtfContainer container, Type t)
    {
        if (container.IsConvertibleTo(t))
        {
            return container.To(t);
        }
        if (container.Type == EtfConstants.AtomExt)
        {
            var name = container.ToAtom();
            return name switch {
                "true" => true,
                "false" => false,
                "nil" => null,
                _ => name.To(t)
            };
        }
        if (container.Type == EtfConstants.BinaryExt)
        {
            if (typeof(string).IsAssignableFrom(t))
            {
                return Encoding.UTF8.GetString((ArraySegment<byte>) container);
            }
            if (typeof(byte[]).IsAssignableFrom(t))
            {
                return ((ArraySegment<byte>) container).ToArray();
            }
            if (typeof(ArraySegment<byte>).IsAssignableFrom(t))
            {
                return (ArraySegment<byte>) container;
            }
            throw new EtfException($"Cannot convert BinaryExt to {t}");
        }
        if (container.Type == EtfConstants.StringExt)
        {
            return ((string) container).To(t);
        }
        if (container.Type == EtfConstants.SmallIntegerExt)
        {
            return ((byte) container).To(t);
        }
        if (container.Type == EtfConstants.IntegerExt)
        {
            return ((int) container).To(t);
        }
        if (container.Type == EtfConstants.SmallBigExt)
        {
            return ((BigInteger) container).To(t);
        }
        if (container.Type == EtfConstants.NewFloatExt)
        {
            return ((double) container).To(t);
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

    internal static ITuple CreateTuple(Type t, object?[] values)
    {
        var typeArguments = t.GenericTypeArguments;
        
        object?[] vals;
        if (values.Length >= 8)
        {
            vals = values[..8];
            vals[7] = CreateTuple(typeArguments[7], values[7..]);
        }
        else
        {
            vals = values;
        }

        return (ITuple)Activator.CreateInstance(t, vals);

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
        var values = new object?[tuple.Length];
        for (var i = 0; i < values.Length; i++)
        {
            var genericIndex = i;
            var generic = t.GenericTypeArguments[genericIndex < 8 ? genericIndex : 7];
            while (genericIndex >= 7)
            {
                genericIndex -= 7;
                generic = generic.GenericTypeArguments[genericIndex < 8 ? genericIndex : 7];
            }
            values[i] = ToObject(tuple[i], generic);
        }

        return CreateTuple(t, values);
    }
}