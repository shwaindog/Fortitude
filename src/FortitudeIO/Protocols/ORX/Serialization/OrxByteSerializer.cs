#region

using System.Reflection;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization;

public interface ISerializer
{
    Type SerializesType { get; }
}

public class OrxByteSerializer<Tm> : IOrxSerializer where Tm : class, new()
{
    private readonly ITypeSerializer[] serializers;
    private readonly Dictionary<string, ITypeSerializer?> visitedSerializers = new();

    public OrxByteSerializer() : this(new Dictionary<string, ISerializer?>()) { }

    private OrxByteSerializer(Dictionary<string, ISerializer?> rootAllSerializers)
    {
        foreach (var kv in OrxMandatoryField.FindAll(typeof(Tm)))
        {
            var pi = kv.Value;
            if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                MandatoryBasicTypes(pi, visitedSerializers, rootAllSerializers);
            else
                MandatoryArrayTypes(pi, visitedSerializers, rootAllSerializers);
        }

        foreach (var kv in OrxOptionalField.FindAll(typeof(Tm)))
        {
            var id = kv.Key;
            var pi = kv.Value;
            if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                OptionalBasicTypes(pi, id, visitedSerializers, rootAllSerializers);
            else
                OptionalArrayTypes(pi, id, visitedSerializers, rootAllSerializers);
        }

        // serializers = currObjSerializers.ToArray();
        serializers = visitedSerializers.Values.Where(v => v != null).OfType<ITypeSerializer>().ToArray();
    }

    public unsafe int Serialize(object message, byte[] buffer, int msgOffset, int headerOffset)
    {
        fixed (byte* fptr = buffer)
        {
            var pureDataSize = Serialize(message, fptr + msgOffset + headerOffset, fptr + msgOffset
                , fptr + buffer.Length);
            if (headerOffset == OrxMessageHeader.HeaderSize)
            {
                var msgSize = (ushort)(pureDataSize + headerOffset);
                var msgSizePtr = fptr + OrxMessageHeader.MessageSizeOffset;
                StreamByteOps.ToBytes(ref msgSizePtr, msgSize);
            }

            return pureDataSize;
        }
    }

    public unsafe int Serialize(object message, byte* ptr, byte* msgStart, byte* endPtr)
    {
        var msg = (Tm)message;
        var dataStart = ptr;
        for (var i = 0; i < serializers.Length; i++)
            if (!serializers[i].Serialize(msg, ref ptr, msgStart, endPtr))
                return 0;
        return (int)(ptr - dataStart);
    }

    #region Inner members

    private interface ITypeSerializer : ISerializer
    {
        unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr);
    }

    private string PropertyInfoFullPath(PropertyInfo pi) => $"{pi.DeclaringType!.FullName}.{pi.Name}";

    private void OptionalArrayTypes(PropertyInfo pi, ushort id, Dictionary<string, ITypeSerializer?> currObjSerializers,
        Dictionary<string, ISerializer?> rootAllSerializers)
    {
        var piPath = PropertyInfoFullPath(pi);
        if (!currObjSerializers.ContainsKey(piPath) && rootAllSerializers.TryGetValue(piPath, out var serializer))
        {
            currObjSerializers[piPath] = serializer as ITypeSerializer;
            return;
        }

        if (pi.PropertyType == typeof(bool[]))
            currObjSerializers[piPath] = new BoolArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<bool>))
            currObjSerializers[piPath] = new BoolListSerializer(pi, id);
        else if (pi.PropertyType == typeof(byte[]))
            currObjSerializers[piPath] = new ByteArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<byte>))
            currObjSerializers[piPath] = new ByteListSerializer(pi, id);
        else if (pi.PropertyType == typeof(short[]))
            currObjSerializers[piPath] = new ShortArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<short>))
            currObjSerializers[piPath] = new ShortListSerializer(pi, id);
        else if (pi.PropertyType == typeof(ushort[]))
            currObjSerializers[piPath] = new UShortArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<ushort>))
            currObjSerializers[piPath] = new UShortListSerializer(pi, id);
        else if (pi.PropertyType == typeof(int[]))
            currObjSerializers[piPath] = new IntArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<int>))
            currObjSerializers[piPath] = new IntListSerializer(pi, id);
        else if (pi.PropertyType == typeof(uint[]))
            currObjSerializers[piPath] = new UIntArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<uint>))
            currObjSerializers[piPath] = new UIntListSerializer(pi, id);
        else if (pi.PropertyType == typeof(long[]))
            currObjSerializers[piPath] = new LongArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<long>))
            currObjSerializers[piPath] = new LongListSerializer(pi, id);
        else if (pi.PropertyType == typeof(decimal[]))
            currObjSerializers[piPath] = new DecimalArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<decimal>))
            currObjSerializers[piPath] = new DecimalListSerializer(pi, id);
        else if (pi.PropertyType == typeof(string[]))
            currObjSerializers[piPath] = new StringArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<string>))
            currObjSerializers[piPath] = new StringListSerializer(pi, id);
        else if (pi.PropertyType == typeof(MutableString[]))
            currObjSerializers[piPath] = new MutableStringArraySerializer(pi, id);
        else if (pi.PropertyType == typeof(List<MutableString>))
            currObjSerializers[piPath] = new MutableStringListSerializer(pi, id);
        else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                 pi.PropertyType.GenericTypeArguments[0].IsClass)
            currObjSerializers[piPath] = (ITypeSerializer)Activator.CreateInstance(
                typeof(ObjectListSerializer<>).MakeGenericType(
                    typeof(Tm),
                    pi.PropertyType.GenericTypeArguments[0]), pi, id)!;
        // ReSharper disable once PossibleNullReferenceException
        else if (pi.PropertyType.GetElementType()!.IsClass && pi.PropertyType.GetArrayRank() == 1)
            currObjSerializers[piPath] = (ITypeSerializer)Activator.CreateInstance(typeof(ObjectArraySerializer<>)
                .MakeGenericType(typeof(Tm), pi.PropertyType.GetElementType()!), pi, id)!;
        else
            throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
        rootAllSerializers[piPath] = currObjSerializers[piPath]!;
    }

    private void OptionalBasicTypes(PropertyInfo pi, ushort id, Dictionary<string, ITypeSerializer?> currObjSerializers
        , Dictionary<string, ISerializer?> rootAllSerializers)
    {
        var piPath = PropertyInfoFullPath(pi);
        if (!currObjSerializers.ContainsKey(piPath) && rootAllSerializers.TryGetValue(piPath, out var serializer))
        {
            currObjSerializers[piPath] = serializer as ITypeSerializer;
            return;
        }

        if (pi.PropertyType == typeof(bool))
        {
            currObjSerializers[piPath] = new BoolSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(byte) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(byte)))
        {
            currObjSerializers[piPath] = new ByteSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(short) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(short)))
        {
            currObjSerializers[piPath] = new ShortSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(ushort) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(ushort)))
        {
            currObjSerializers[piPath] = new UShortSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(int) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(int)))
        {
            currObjSerializers[piPath] = new IntSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(uint) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(uint)))
        {
            currObjSerializers[piPath] = new UIntSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(decimal))
        {
            currObjSerializers[piPath] = new DecimalSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(long))
        {
            currObjSerializers[piPath] = new LongSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(DateTime))
        {
            currObjSerializers[piPath] = new DateTimeSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(string))
        {
            currObjSerializers[piPath] = new StringSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(MutableString))
        {
            currObjSerializers[piPath] = new MutableStringSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(Dictionary<string, string>))
        {
            currObjSerializers[piPath] = new MapSerializer(pi, id);
        }
        else if (pi.PropertyType == typeof(Tm))
        {
            rootAllSerializers[piPath] = null!; // prevent re-entry
            var customAttributes = (OrxOptionalField?)pi
                .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
            if (customAttributes != null)
                currObjSerializers[piPath] = new SelfSerializer(pi, id, customAttributes.Mapping,
                    this, rootAllSerializers);
        }
        else if (pi.PropertyType.IsClass)
        {
            var customAttributes = (OrxOptionalField?)pi
                .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
            if ((customAttributes != null && pi.PropertyType.IsAbstract) ||
                pi.PropertyType.GetConstructor(Type.EmptyTypes) == null ||
                (customAttributes?.Mapping?.Count ?? 0) > 0)
                currObjSerializers[piPath] = (ITypeSerializer)Activator.CreateInstance(typeof(ObjectSerializer<>)
                        .MakeGenericType(typeof(Tm), typeof(object)), pi, id,
                    rootAllSerializers, customAttributes?.Mapping)!;
            else
                currObjSerializers[piPath] = (ITypeSerializer)Activator.CreateInstance(typeof(ObjectSerializer<>)
                    .MakeGenericType(typeof(Tm), pi.PropertyType), pi, id, rootAllSerializers)!;
        }
        else
        {
            throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
        }

        rootAllSerializers[piPath] = currObjSerializers[piPath]!;
    }

    private void MandatoryArrayTypes(PropertyInfo pi, Dictionary<string, ITypeSerializer?> currObjSerializers
        , Dictionary<string, ISerializer?> rootAllSerializers)
    {
        var piPath = PropertyInfoFullPath(pi);
        if (!currObjSerializers.ContainsKey(piPath) && rootAllSerializers.TryGetValue(piPath, out var serializer))
        {
            currObjSerializers[piPath] = serializer as ITypeSerializer;
            return;
        }

        if (pi.PropertyType == typeof(bool[]))
            currObjSerializers[piPath] = new BoolArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<bool>))
            currObjSerializers[piPath] = new BoolListSerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(byte[]))
            currObjSerializers[piPath] = new ByteArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<byte>))
            currObjSerializers[piPath] = new ByteListSerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(short[]))
            currObjSerializers[piPath] = new ShortArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<short>))
            currObjSerializers[piPath] = new ShortListSerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(ushort[]))
            currObjSerializers[piPath] = new UShortArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<ushort>))
            currObjSerializers[piPath] = new UShortListSerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(int[]))
            currObjSerializers[piPath] = new IntArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<int>))
            currObjSerializers[piPath] = new IntListSerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(uint[]))
            currObjSerializers[piPath] = new UIntArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<uint>))
            currObjSerializers[piPath] = new UIntListSerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(long[]))
            currObjSerializers[piPath] = new LongArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<long>))
            currObjSerializers[piPath] = new LongListSerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(decimal[]))
            currObjSerializers[piPath] = new DecimalArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<decimal>))
            currObjSerializers[piPath] = new DecimalListSerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(string[]))
            currObjSerializers[piPath] = new StringArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<string>))
            currObjSerializers[piPath] = new StringListSerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(MutableString[]))
            currObjSerializers[piPath] = new MutableStringArraySerializer(pi, ushort.MaxValue);
        else if (pi.PropertyType == typeof(List<MutableString>))
            currObjSerializers[piPath] = new MutableStringListSerializer(pi, ushort.MaxValue);
        // ReSharper disable once PossibleNullReferenceException
        else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                 pi.PropertyType.GenericTypeArguments[0].IsClass)
            currObjSerializers[piPath] = (ITypeSerializer)Activator.CreateInstance(
                typeof(ObjectListSerializer<>).MakeGenericType(
                    typeof(Tm),
                    pi.PropertyType.GenericTypeArguments[0]), pi, ushort.MaxValue)!;
        // ReSharper disable once PossibleNullReferenceException
        else if (pi.PropertyType.GetElementType()!.IsClass && pi.PropertyType.GetArrayRank() == 1)
            currObjSerializers[piPath] = (ITypeSerializer)Activator.CreateInstance(
                typeof(ObjectArraySerializer<>).MakeGenericType(
                    typeof(Tm),
                    pi.PropertyType.GetElementType()!), pi, ushort.MaxValue)!;
        else
            throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
        rootAllSerializers[piPath] = currObjSerializers[piPath]!;
    }


    private void MandatoryBasicTypes(PropertyInfo pi, Dictionary<string, ITypeSerializer?> currObjSerializers
        , Dictionary<string, ISerializer?> rootAllSerializers)
    {
        var piPath = PropertyInfoFullPath(pi);
        if (!currObjSerializers.ContainsKey(piPath) && rootAllSerializers.TryGetValue(piPath, out var serializer))
        {
            currObjSerializers[piPath] = serializer as ITypeSerializer;
            return;
        }

        if (pi.PropertyType == typeof(bool))
        {
            currObjSerializers[piPath] = new BoolSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(byte) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(byte)))
        {
            currObjSerializers[piPath] = new ByteSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(short) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(short)))
        {
            currObjSerializers[piPath] = new ShortSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(ushort) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(ushort)))
        {
            currObjSerializers[piPath] = new UShortSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(int) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(int)))
        {
            currObjSerializers[piPath] = new IntSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(uint) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(uint)))
        {
            currObjSerializers[piPath] = new UIntSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(decimal))
        {
            currObjSerializers[piPath] = new DecimalSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(long))
        {
            currObjSerializers[piPath] = new LongSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(DateTime))
        {
            currObjSerializers[piPath] = new DateTimeSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(string))
        {
            currObjSerializers[piPath] = new StringSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(MutableString))
        {
            currObjSerializers[piPath] = new MutableStringSerializer(pi, ushort.MaxValue);
        }
        else if (pi.PropertyType == typeof(Tm))
        {
            rootAllSerializers[piPath] = null!; // prevent re-entry
            var customAttributes = (OrxMandatoryField?)pi
                .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
            if (customAttributes != null)
                currObjSerializers[piPath] = new SelfSerializer(pi, ushort.MaxValue, customAttributes.Mapping,
                    this, rootAllSerializers);
        }
        else if (pi.PropertyType.IsClass)
        {
            var customAttributes = (OrxMandatoryField?)pi
                .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
            if ((customAttributes != null && pi.PropertyType.IsAbstract) ||
                pi.PropertyType.GetConstructor(Type.EmptyTypes) == null ||
                (customAttributes?.Mapping?.Count ?? 0) > 0)
                currObjSerializers[piPath] = (ITypeSerializer)Activator.CreateInstance(typeof(ObjectSerializer<>)
                        .MakeGenericType(typeof(Tm), typeof(object)), pi, ushort.MaxValue,
                    rootAllSerializers, customAttributes?.Mapping)!;
            else
                currObjSerializers[piPath] = (ITypeSerializer)Activator.CreateInstance(typeof(ObjectSerializer<>)
                    .MakeGenericType(typeof(Tm), pi.PropertyType), pi, ushort.MaxValue, rootAllSerializers)!;
        }
        else
        {
            throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
        }

        rootAllSerializers[piPath] = currObjSerializers[piPath]!;
    }

    private abstract class Serializer<TP> : ITypeSerializer
    {
        protected readonly Func<Tm, TP?> Get;
        protected readonly ushort Id;

        protected Serializer(PropertyInfo property, ushort id)
        {
            Id = id;
            Get = (Func<Tm, TP>)Delegate.CreateDelegate(typeof(Func<Tm, TP>), property.GetGetMethod(true)!);
        }

        protected bool IsOptional => Id != ushort.MaxValue;

        public Type SerializesType => typeof(TP);

        public abstract unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr);
    }

    private class ObjectSerializer<TO> : Serializer<object> where TO : class, new()
    {
        private readonly Dictionary<Type, IOrxSerializer> serializerLookup;
        protected OrxByteSerializer<TO> ItemSerializer;
        private Dictionary<ushort, Type>? mapping;

        public ObjectSerializer(PropertyInfo property, ushort id, Dictionary<string, ISerializer?> allSerializers) :
            base(property, id)
        {
            serializerLookup = new Dictionary<Type, IOrxSerializer>();
            mapping = new Dictionary<ushort, Type>();
            ItemSerializer = new OrxByteSerializer<TO>(allSerializers);
        }

        // ReSharper disable once UnusedMember.Local
        public ObjectSerializer(PropertyInfo property, ushort id, Dictionary<string, ISerializer?> allSerializers
            , Dictionary<ushort, Type>? mapping)
            : this(property, id, allSerializers)
        {
            ImportMapping(mapping);
        }

        protected void ImportMapping(Dictionary<ushort, Type>? typeLookupMapping)
        {
            mapping = typeLookupMapping;
            if (typeLookupMapping != null)
                foreach (var keyValuePair in typeLookupMapping)
                {
                    var mappedSerializer = (IOrxSerializer)Activator.CreateInstance(typeof(OrxByteSerializer<>)
                        .MakeGenericType(keyValuePair.Value))!;
                    serializerLookup.Add(keyValuePair.Value, mappedSerializer);
                }
        }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var obj = Get(message);
            if (obj == null && IsOptional) return true;
            if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
            StreamByteOps.ToBytes(ref ptr, Id);
            var sizePtr = ptr;
            ptr += 2;
            if (obj == null)
            {
                StreamByteOps.ToBytes(ref sizePtr, (ushort)0);
                return true;
            }

            int size;
            if (mapping != null && mapping.Count > 0)
            {
                var subClassKey = mapping.FirstOrDefault(kvp => kvp.Value == obj.GetType()).Key;
                if (subClassKey == 0) return false;
                StreamByteOps.ToBytes(ref ptr, subClassKey);
                var mappedSerializer = serializerLookup[obj.GetType()];
                if ((size = mappedSerializer.Serialize(obj, ptr, msgStart, endPtr)) == 0) return false;
            }
            else if ((size = ItemSerializer.Serialize(obj, ptr, msgStart, endPtr)) == 0)
            {
                return false;
            }

            StreamByteOps.ToBytes(ref sizePtr, (ushort)size);
            ptr += size;

            return true;
        }
    }

    private class SelfSerializer : ObjectSerializer<Tm>
    {
        public SelfSerializer(PropertyInfo property, ushort id, Dictionary<ushort, Type> mapping,
            OrxByteSerializer<Tm> serializer, Dictionary<string, ISerializer?> allSerializers)
            : base(property, id, allSerializers)
        {
            ItemSerializer = serializer;
            ImportMapping(mapping);
        }
    }

    #region Collections

    private sealed class BoolArraySerializer : Serializer<bool[]>
    {
        public BoolArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Length ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Length ?? 0));
                for (var i = 0; i < (array?.Length ?? 0); i++)
                {
                    *ptr = array![i] ? (byte)0xFF : (byte)00;
                    ptr++;
                }

                return true;
            }

            return false;
        }
    }

    private sealed class BoolListSerializer : Serializer<List<bool>>
    {
        public BoolListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Count ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Count ?? 0));
                for (var i = 0; i < (array?.Count ?? 0); i++)
                {
                    *ptr = array![i] ? (byte)0xFF : (byte)00;
                    ptr++;
                }

                return true;
            }

            return false;
        }
    }

    private sealed class ByteArraySerializer : Serializer<byte[]>
    {
        public ByteArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Length ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Length ?? 0));
                for (var i = 0; i < (array?.Length ?? 0); i++)
                {
                    *ptr = array![i];
                    ptr++;
                }

                return true;
            }

            return false;
        }
    }

    private sealed class ByteListSerializer : Serializer<List<byte>>
    {
        public ByteListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Count ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Count ?? 0));
                for (var i = 0; i < (array?.Count ?? 0); i++)
                {
                    *ptr = array![i];
                    ptr++;
                }

                return true;
            }

            return false;
        }
    }

    private sealed class ShortArraySerializer : Serializer<short[]>
    {
        public ShortArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Length ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Length ?? 0));
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class ShortListSerializer : Serializer<List<short>>
    {
        public ShortListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Count ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Count ?? 0));
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class UShortArraySerializer : Serializer<ushort[]>
    {
        public UShortArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Length ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Length ?? 0));
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class UShortListSerializer : Serializer<List<ushort>>
    {
        public UShortListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Count ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Count ?? 0));
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class IntArraySerializer : Serializer<int[]>
    {
        public IntArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Length ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Length ?? 0));
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class IntListSerializer : Serializer<List<int>>
    {
        public IntListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Count ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Count ?? 0));
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class UIntArraySerializer : Serializer<uint[]>
    {
        public UIntArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Length ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Length ?? 0));
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class UIntListSerializer : Serializer<List<uint>>
    {
        public UIntListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Count ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Count ?? 0));
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class LongArraySerializer : Serializer<long[]>
    {
        public LongArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Length ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Length ?? 0));
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class LongListSerializer : Serializer<List<long>>
    {
        public LongListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Count ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Count ?? 0));
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i]);
                return true;
            }

            return false;
        }
    }

    private sealed class DecimalArraySerializer : Serializer<decimal[]>
    {
        public DecimalArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz));
            }

            if (ptr + OrxConstants.UInt16Sz + (array?.Length ?? 0) * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)(array?.Length ?? 0));
                for (var i = 0; i < (array?.Length ?? 0); i++)
                {
                    var elementDecimal = array![i];
                    var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(elementDecimal)[3])[2];
                    var roundingNoDecimal = (long)((decimal)Math.Pow(10, decimalPlaces) * elementDecimal);
                    StreamByteOps.ToBytes(ref ptr, decimalPlaces);
                    StreamByteOps.ToBytes(ref ptr, roundingNoDecimal);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class DecimalListSerializer : Serializer<List<decimal>>
    {
        public DecimalListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            if (Id != ushort.MaxValue)
            {
                if (array == null) return true;
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
            }

            var arrayCount = array?.Count ?? 0;
            if (ptr + OrxConstants.UInt16Sz + arrayCount * OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, (ushort)arrayCount);
                for (var i = 0; i < arrayCount; i++)
                {
                    var elementDecimal = array![i];
                    var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(elementDecimal)[3])[2];
                    var roundingNoDecimal = (long)((decimal)Math.Pow(10, decimalPlaces) * elementDecimal);
                    StreamByteOps.ToBytes(ref ptr, decimalPlaces);
                    StreamByteOps.ToBytes(ref ptr, roundingNoDecimal);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class StringArraySerializer : Serializer<string[]>
    {
        public StringArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            var sizeOfArray = ptr;

            if (Id != ushort.MaxValue)
            {
                if (ptr + 256 > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray = ptr;
                ptr += 2;
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                var arrayLength = array?.Length ?? 0;
                StreamByteOps.ToBytes(ref ptr, (ushort)arrayLength);
                for (var i = 0; i < arrayLength; i++)
                    if (ptr + arrayLength * 256 <= endPtr)
                        StreamByteOps.ToBytesWithSizeHeader(ref ptr, array![i], 256);
                if (Id != ushort.MaxValue)
                    StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray - OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class StringListSerializer : Serializer<List<string>>
    {
        public StringListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            var sizeOfArray = ptr;

            if (Id != ushort.MaxValue)
            {
                if (ptr + 256 > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray = ptr;
                ptr += 2;
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                var arrayCount = array?.Count ?? 0;
                StreamByteOps.ToBytes(ref ptr, (ushort)arrayCount);
                for (var i = 0; i < arrayCount; i++)
                    if (ptr + arrayCount * 256 <= endPtr)
                        StreamByteOps.ToBytesWithSizeHeader(ref ptr, array![i], 256);
                if (Id != ushort.MaxValue)
                    StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray - OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class MutableStringArraySerializer : Serializer<MutableString[]>
    {
        public MutableStringArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            var sizeOfArray = ptr;

            if (Id != ushort.MaxValue)
            {
                if (ptr + 1024 > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray = ptr;
                ptr += 2;
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                var arrayLength = array?.Length ?? 0;
                StreamByteOps.ToBytes(ref ptr, (ushort)arrayLength);
                for (var i = 0; i < arrayLength; i++)
                    if (ptr + arrayLength * 1024 <= endPtr)
                        StreamByteOps.ToBytesWithSizeHeader(ref ptr, array![i], 1024);
                if (Id != ushort.MaxValue)
                    StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray - OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class MutableStringListSerializer : Serializer<List<MutableString>>
    {
        public MutableStringListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            var sizeOfArray = ptr;

            if (Id != ushort.MaxValue)
            {
                if (ptr + 1024 > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray = ptr;
                ptr += 2;
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                var arrayCount = array?.Count ?? 0;
                StreamByteOps.ToBytes(ref ptr, (ushort)arrayCount);
                for (var i = 0; i < arrayCount; i++)
                    if (ptr + arrayCount * 1024 <= endPtr)
                        StreamByteOps.ToBytesWithSizeHeader(ref ptr, array![i], 1024);
                if (Id != ushort.MaxValue)
                    StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray - OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class MapSerializer : Serializer<Dictionary<string, string>>
    {
        public MapSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var dictionary = Get(message);
            var dic = dictionary;

            byte* sizePtr = null;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizePtr = ptr;
                ptr += OrxConstants.UInt16Sz;
            }

            if (ptr + OrxConstants.UInt32Sz > endPtr) return false;
            var dicCount = dic?.Count ?? 0;
            StreamByteOps.ToBytes(ref ptr, (uint)dicCount);

            int size = OrxConstants.UInt32Sz;
            if (dictionary != null)
                foreach (var kv in dictionary)
                {
                    var recLen = (kv.Key.Length + kv.Value.Length + 2) * OrxConstants.UInt8Sz;
                    if (ptr + recLen > endPtr) return false;
                    size += recLen;

                    StreamByteOps.ToBytesWithSizeHeader(ref ptr, kv.Key, ushort.MaxValue);
                    StreamByteOps.ToBytesWithSizeHeader(ref ptr, kv.Value, ushort.MaxValue);
                }

            if (sizePtr != null) StreamByteOps.ToBytes(ref sizePtr, (ushort)size);

            return true;
        }
    }

    private sealed class ObjectArraySerializer<To> : Serializer<To[]> where To : class, new()
    {
        private readonly OrxByteSerializer<To> itemSerializer = new();

        public ObjectArraySerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var array = Get(message);
            var wasNull = array == null;
            array ??= Array.Empty<To>();

            if (ptr + (2 + array.Length) * OrxConstants.UInt16Sz > endPtr) return false;
            StreamByteOps.ToBytes(ref ptr, Id);
            var objectsListSize = ptr;
            ptr += OrxConstants.UInt16Sz;
            if (wasNull)
                StreamByteOps.ToBytes(ref ptr, ushort.MaxValue);
            else
                StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
            var objectListStart = ptr;
            for (var i = 0; i < array.Length; i++)
            {
                int size;
                if ((size = itemSerializer.Serialize(array[i], ptr + OrxConstants.UInt16Sz, msgStart, endPtr)) == 0)
                    return false;
                StreamByteOps.ToBytes(ref ptr, (ushort)size);
                ptr += size;
            }

            StreamByteOps.ToBytes(ref objectsListSize, (ushort)(ptr - objectListStart));

            return true;
        }
    }

    private sealed class ObjectListSerializer<To> : Serializer<List<To>> where To : class, new()
    {
        private readonly OrxByteSerializer<To> itemSerializer = new();

        public ObjectListSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var list = Get(message);
            var wasNull = list == null;
            list ??= new List<To>();
            if (ptr + (2 + list.Count) * OrxConstants.UInt16Sz > endPtr) return false;
            StreamByteOps.ToBytes(ref ptr, Id);
            var objectsListSize = ptr;
            ptr += OrxConstants.UInt16Sz;
            if (wasNull)
                StreamByteOps.ToBytes(ref ptr, ushort.MaxValue);
            else
                StreamByteOps.ToBytes(ref ptr, (ushort)list.Count);
            var objectListStart = ptr;
            for (var i = 0; i < list.Count; i++)
            {
                int size;
                if ((size = itemSerializer.Serialize(list[i], ptr + OrxConstants.UInt16Sz, msgStart, endPtr)) == 0)
                    return false;
                StreamByteOps.ToBytes(ref ptr, (ushort)size);
                ptr += size;
            }

            StreamByteOps.ToBytes(ref objectsListSize, (ushort)(ptr - objectListStart));

            return true;
        }
    }

    #endregion

    #region Basic types

    private sealed class BoolSerializer : Serializer<bool>
    {
        public BoolSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt8Sz);
            }

            if (ptr + OrxConstants.UInt8Sz <= endPtr)
            {
                *ptr++ = Get(message) ? (byte)1 : (byte)0;
                return true;
            }

            return false;
        }
    }

    private sealed class ByteSerializer : Serializer<byte>
    {
        public ByteSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt8Sz);
            }

            if (ptr + OrxConstants.UInt8Sz <= endPtr)
            {
                *ptr++ = Get(message);
                return true;
            }

            return false;
        }
    }

    private sealed class UShortSerializer : Serializer<ushort>
    {
        public UShortSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt16Sz);
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, Get(message));
                return true;
            }

            return false;
        }
    }

    private sealed class ShortSerializer : Serializer<short>
    {
        public ShortSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt16Sz);
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, Get(message));
                return true;
            }

            return false;
        }
    }

    private sealed class UIntSerializer : Serializer<uint>
    {
        public UIntSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt32Sz);
            }

            if (ptr + OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, Get(message));
                return true;
            }

            return false;
        }
    }

    private sealed class IntSerializer : Serializer<int>
    {
        public IntSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt32Sz);
            }

            if (ptr + OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, Get(message));
                return true;
            }

            return false;
        }
    }

    private sealed class StringSerializer : Serializer<string>
    {
        public StringSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var str = Get(message);
            if (str == null) return true;
            var sizeOfArray = ptr;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray = ptr;
                ptr += 2;
            }

            if (ptr + str.Length * OrxConstants.UInt8Sz + 1 <= endPtr)
            {
                StreamByteOps.ToBytesWithSizeHeader(ref ptr, str, ushort.MaxValue);
                if (Id != ushort.MaxValue)
                    StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray + OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class MutableStringSerializer : Serializer<MutableString>
    {
        public MutableStringSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var str = Get(message);
            if (str == null) return true;
            var sizeOfArray = ptr;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray = ptr;
                ptr += 2;
            }

            if (ptr + str.Length * OrxConstants.UInt8Sz + 1 <= endPtr)
            {
                StreamByteOps.ToBytesWithSizeHeader(ref ptr, str, ushort.MaxValue);
                if (Id != ushort.MaxValue)
                    StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray + OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class DecimalSerializer : Serializer<decimal>
    {
        public DecimalSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt8Sz + OrxConstants.UInt32Sz));
            }

            if (ptr + OrxConstants.UInt8Sz + OrxConstants.UInt32Sz <= endPtr)
            {
                var value = Get(message);
                var factor = OrxScaling.GetScalingFactor(value);
                *ptr++ = factor;
                StreamByteOps.ToBytes(ref ptr, OrxScaling.Scale(value, factor));
                return true;
            }

            return false;
        }
    }

    private sealed class LongSerializer : Serializer<long>
    {
        public LongSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            if (Id != ushort.MaxValue)
            {
                if (ptr + OrxConstants.UInt16Sz + 2 * OrxConstants.UInt64Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt64Sz);
            }

            if (ptr + OrxConstants.UInt64Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, Get(message));
                return true;
            }

            return false;
        }
    }

    private sealed class DateTimeSerializer : Serializer<DateTime>
    {
        public DateTimeSerializer(PropertyInfo property, ushort id)
            : base(property, id) { }

        public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
        {
            var dateTime = Get(message);
            if (dateTime == DateTimeConstants.UnixEpoch && IsOptional) return true;
            if (IsOptional)
            {
                if (ptr + OrxConstants.UInt16Sz + 2 * OrxConstants.UInt64Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt64Sz);
            }

            if (ptr + OrxConstants.UInt64Sz <= endPtr)
            {
                var timeAsNanosSinceUnixEpoch = (dateTime.Ticks - DateTimeConstants.UnixEpochTicks) * 100;
                StreamByteOps.ToBytes(ref ptr, timeAsNanosSinceUnixEpoch);
                return true;
            }

            return false;
        }
    }

    #endregion

    #endregion
}
