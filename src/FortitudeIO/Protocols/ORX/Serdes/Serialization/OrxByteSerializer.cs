#region

using System.Reflection;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Serialization;

public interface ISerializer
{
    Type SerializesType { get; }
}

public class OrxByteSerializer<TMsg> : IOrxSerializer where TMsg : class, new()
{
    private readonly ITypeSerializer[]                    serializers;
    private readonly Dictionary<string, ITypeSerializer?> visitedSerializers = new();

    public OrxByteSerializer() : this(new Dictionary<string, ISerializer?>()) { }

    private OrxByteSerializer(Dictionary<string, ISerializer?> rootAllSerializers)
    {
        foreach (var kv in OrxMandatoryField.FindAll(typeof(TMsg)))
        {
            var pi = kv.Value;
            if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                MandatoryBasicTypes(pi, visitedSerializers, rootAllSerializers);
            else
                MandatoryArrayTypes(pi, visitedSerializers, rootAllSerializers);
        }

        foreach (var kv in OrxOptionalField.FindAll(typeof(TMsg)))
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

    public unsafe int Serialize(object message, byte* ptr, byte* endPtr)
    {
        var msg       = (TMsg)message;
        var dataStart = ptr;
        for (var i = 0; i < serializers.Length; i++)
            if (!serializers[i].Serialize(msg, ref ptr, endPtr))
                return 0;
        return (int)(ptr - dataStart);
    }

    public unsafe int Serialize(object message, IBuffer buffer, int headerOffset)
    {
        using var fixedBuffer  = buffer;
        var       fptr         = buffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var       noHeaderSize = Serialize(message, fptr + headerOffset, fptr + buffer.RemainingStorage);
        if (headerOffset == OrxMessageHeader.HeaderSize)
        {
            var msgSize    = (uint)(noHeaderSize + headerOffset);
            var msgSizePtr = fptr + OrxMessageHeader.MessageSizeOffset;
            StreamByteOps.ToBytes(ref msgSizePtr, msgSize);
        }

        return noHeaderSize;
    }

    #region Inner members

    private interface ITypeSerializer : ISerializer
    {
        unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr);
    }

    private string PropertyInfoFullPath(PropertyInfo pi) => $"{pi.DeclaringType!.FullName}.{pi.Name}";

    private void OptionalArrayTypes
    (PropertyInfo pi, ushort id, Dictionary<string, ITypeSerializer?> currObjSerializers,
        Dictionary<string, ISerializer?> rootAllSerializers)
    {
        if (interrogatingTypes.Contains(pi.PropertyType)) return;
        var piPath = PropertyInfoFullPath(pi);
        currObjSerializers[piPath] = ResolveArrayTypes(pi, id, currObjSerializers, rootAllSerializers);
        rootAllSerializers[piPath] = currObjSerializers[piPath]!;
    }

    private ITypeSerializer? ResolveArrayTypes
    (PropertyInfo pi, ushort id, Dictionary<string, ITypeSerializer?> currObjSerializers,
        Dictionary<string, ISerializer?> rootAllSerializers)
    {
        var piPath = PropertyInfoFullPath(pi);
        if (!currObjSerializers.ContainsKey(piPath) && rootAllSerializers.TryGetValue(piPath, out var serializer))
        {
            return serializer as ITypeSerializer; // can be temporary null placeholder
        }

        ITypeSerializer? foundSerializer =
            pi.PropertyType switch
            {
                var t when t.IsBoolArray()            => new BoolArraySerializer(pi, id)
              , var t when t.IsNullableBoolArray()    => new NullableBoolArraySerializer(pi, id)
              , var t when t.IsBoolList()             => new BoolListSerializer(pi, id)
              , var t when t.IsNullableBoolList()     => new NullableBoolListSerializer(pi, id)
              , var t when t.IsByteArray()            => new ByteArraySerializer(pi, id)
              , var t when t.IsNullableByteArray()    => new NullableByteArraySerializer(pi, id)
              , var t when t.IsByteList()             => new ByteListSerializer(pi, id)
              , var t when t.IsNullableByteList()     => new NullableByteListSerializer(pi, id)
              , var t when t.IsShortArray()           => new ShortArraySerializer(pi, id)
              , var t when t.IsNullableShortArray()   => new NullableShortArraySerializer(pi, id)
              , var t when t.IsShortList()            => new ShortListSerializer(pi, id)
              , var t when t.IsNullableShortList()    => new NullableShortListSerializer(pi, id)
              , var t when t.IsUShortArray()          => new UShortArraySerializer(pi, id)
              , var t when t.IsNullableUShortArray()  => new NullableUShortArraySerializer(pi, id)
              , var t when t.IsUShortList()           => new UShortListSerializer(pi, id)
              , var t when t.IsNullableUShortList()   => new NullableUShortListSerializer(pi, id)
              , var t when t.IsIntArray()             => new IntArraySerializer(pi, id)
              , var t when t.IsNullableIntArray()     => new NullableIntArraySerializer(pi, id)
              , var t when t.IsIntList()              => new IntListSerializer(pi, id)
              , var t when t.IsNullableIntList()      => new NullableIntListSerializer(pi, id)
              , var t when t.IsUIntArray()            => new UIntArraySerializer(pi, id)
              , var t when t.IsNullableUIntArray()    => new NullableUIntArraySerializer(pi, id)
              , var t when t.IsUIntList()             => new UIntListSerializer(pi, id)
              , var t when t.IsNullableUIntList()     => new NullableUIntListSerializer(pi, id)
              , var t when t.IsLongArray()            => new LongArraySerializer(pi, id)
              , var t when t.IsNullableLongArray()    => new NullableLongArraySerializer(pi, id)
              , var t when t.IsLongList()             => new LongListSerializer(pi, id)
              , var t when t.IsNullableLongList()     => new NullableLongListSerializer(pi, id)
              , var t when t.IsULongArray()           => new ULongArraySerializer(pi, id)
              , var t when t.IsNullableULongArray()   => new NullableULongArraySerializer(pi, id)
              , var t when t.IsULongList()            => new ULongListSerializer(pi, id)
              , var t when t.IsNullableULongList()    => new NullableULongListSerializer(pi, id)
              , var t when t.IsDecimalArray()         => new DecimalArraySerializer(pi, id)
              , var t when t.IsNullableDecimalArray() => new NullableDecimalArraySerializer(pi, id)
              , var t when t.IsDecimalList()          => new DecimalListSerializer(pi, id)
              , var t when t.IsNullableDecimalList()  => new NullableDecimalListSerializer(pi, id)
              , var t when t.IsStringArray()          => new StringArraySerializer(pi, id)
              , var t when t.IsStringList()           => new StringListSerializer(pi, id)

              , var t when t.IsMutableStringArray()    => new MutableStringArraySerializer(pi, id)
              , var t when t.IsMutableStringList()     => new MutableStringListSerializer(pi, id)
              , var t when t.IsDateTimeArray()         => new DateTimeArraySerializer(pi, id)
              , var t when t.IsNullableDateTimeArray() => new NullableDateTimeArraySerializer(pi, id)
              , var t when t.IsDateTimeList()          => new DateTimeListSerializer(pi, id)
              , var t when t.IsNullableDateTimeList()  => new NullableDateTimeListSerializer(pi, id)
              , var t when t.IsStringToStringMap()     => new MapSerializer(pi, id)
              , _                                      => null
            };

        if (foundSerializer == null)
        {
            interrogatingTypes.Push(pi.PropertyType);
            if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                pi.PropertyType.GenericTypeArguments[0].IsClass)
                foundSerializer = (ITypeSerializer)Activator.CreateInstance
                    (typeof(ObjectListSerializer<>).MakeGenericType
                        (typeof(TMsg), pi.PropertyType.GenericTypeArguments[0]), pi, id)!;
            // ReSharper disable once PossibleNullReferenceException
            else if (pi.PropertyType.GetElementType()!.IsClass && pi.PropertyType.GetArrayRank() == 1)
                foundSerializer = (ITypeSerializer)Activator.CreateInstance
                    (typeof(ObjectArraySerializer<>).MakeGenericType
                        (typeof(TMsg), pi.PropertyType.GetElementType()!), pi, id)!;
            interrogatingTypes.Pop();
        }

        if (foundSerializer != null)
        {
            return foundSerializer;
        }

        throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
    }

    private void OptionalBasicTypes
    (PropertyInfo pi, ushort id, Dictionary<string, ITypeSerializer?> currObjSerializers
      , Dictionary<string, ISerializer?> rootAllSerializers)
    {
        if (interrogatingTypes.Contains(pi.PropertyType)) return;
        var piPath = PropertyInfoFullPath(pi);
        currObjSerializers[piPath] = ResolveBasicTypesSerializer(pi, id, currObjSerializers, rootAllSerializers);
        rootAllSerializers[piPath] = currObjSerializers[piPath]!;
    }

    private Stack<Type> interrogatingTypes = new Stack<Type>();

    private ITypeSerializer? ResolveBasicTypesSerializer
    (PropertyInfo pi, ushort id, Dictionary<string, ITypeSerializer?> currObjSerializers
      , Dictionary<string, ISerializer?> rootAllSerializers)
    {
        var piPath = PropertyInfoFullPath(pi);
        if (!currObjSerializers.ContainsKey(piPath) && rootAllSerializers.TryGetValue(piPath, out var serializer))
        {
            return serializer as ITypeSerializer;  // can be temporary null placeholder
        }

        ITypeSerializer? foundSerializer =
            pi.PropertyType switch
            {
                var t when t.IsBool()            => new BoolSerializer(pi, id)
              , var t when t.IsNullableBool()    => new NullableBoolSerializer(pi, id)
              , var t when t.IsByte()            => new ByteSerializer(pi, id)
              , var t when t.IsNullableByte()    => new NullableByteSerializer(pi, id)
              , var t when t.IsShort()           => new ShortSerializer(pi, id)
              , var t when t.IsNullableShort()   => new NullableShortSerializer(pi, id)
              , var t when t.IsUShort()          => new UShortSerializer(pi, id)
              , var t when t.IsNullableUShort()  => new NullableUShortSerializer(pi, id)
              , var t when t.IsInt()             => new IntSerializer(pi, id)
              , var t when t.IsNullableInt()     => new NullableIntSerializer(pi, id)
              , var t when t.IsUInt()            => new UIntSerializer(pi, id)
              , var t when t.IsNullableUInt()    => new NullableUIntSerializer(pi, id)
              , var t when t.IsLong()            => new LongSerializer(pi, id)
              , var t when t.IsNullableLong()    => new NullableLongSerializer(pi, id)
              , var t when t.IsULong()           => new ULongSerializer(pi, id)
              , var t when t.IsNullableULong()   => new NullableULongSerializer(pi, id)
              , var t when t.IsDecimal()         => new DecimalSerializer(pi, id)
              , var t when t.IsNullableDecimal() => new NullableDecimalSerializer(pi, id)
              , var t when t.IsString()          => new StringSerializer(pi, id)

              , var t when t.IsMutableString()     => new MutableStringSerializer(pi, id)
              , var t when t.IsDateTime()          => new DateTimeSerializer(pi, id)
              , var t when t.IsNullableDateTime()  => new NullableDateTimeSerializer(pi, id)
              , var t when t.IsStringToStringMap() => new MapSerializer(pi, id)
              , _                                  => null
            };

        if (foundSerializer == null)
        {
            interrogatingTypes.Push(pi.PropertyType);
            rootAllSerializers[piPath] = rootAllSerializers.GetValueOrDefault(piPath, null!); // temporary null serializer prevent re-entry
            if (id == ushort.MaxValue)
            {
                if (pi.PropertyType == typeof(TMsg))
                {
                    var customAttributes = (OrxMandatoryField?)pi.GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                    if (customAttributes != null)
                        foundSerializer = new SelfSerializer
                            (pi, ushort.MaxValue, customAttributes.Mapping, this, rootAllSerializers);
                }
                else if (pi.PropertyType.IsClass)
                {
                    var mapDerivedAttribs = (OrxMapToDerivedClasses?)pi.PropertyType.GetCustomAttributes(typeof(OrxMapToDerivedClasses), false).FirstOrDefault();
                    if ((mapDerivedAttribs != null && pi.PropertyType.IsAbstract) ||
                        pi.PropertyType.GetConstructor(Type.EmptyTypes) == null ||
                        (mapDerivedAttribs?.Mapping?.Count ?? 0) > 0)
                        foundSerializer = (ITypeSerializer)Activator.CreateInstance
                            (typeof(ObjectSerializer<>).MakeGenericType
                                 (typeof(TMsg), typeof(object)), pi, ushort.MaxValue, rootAllSerializers, mapDerivedAttribs?.Mapping)!;
                    else
                        foundSerializer = (ITypeSerializer)Activator.CreateInstance
                            (typeof(ObjectSerializer<>) .MakeGenericType
                                (typeof(TMsg), pi.PropertyType), pi, ushort.MaxValue, rootAllSerializers)!;
                }
            }
            else
            {
                if (pi.PropertyType == typeof(TMsg))
                {
                    var customAttributes = (OrxOptionalField?)pi.GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                    if (customAttributes != null) foundSerializer = new SelfSerializer(pi, id, customAttributes.Mapping, this, rootAllSerializers);
                }
                else if (pi.PropertyType.IsClass)
                {
                    var mapDerivedAttribs = (OrxMapToDerivedClasses?)pi.PropertyType.GetCustomAttributes(typeof(OrxMapToDerivedClasses), false).FirstOrDefault();
                    if ((mapDerivedAttribs != null && pi.PropertyType.IsAbstract) ||
                        pi.PropertyType.GetConstructor(Type.EmptyTypes) == null ||
                        (mapDerivedAttribs?.Mapping.Count ?? 0) > 0)
                        foundSerializer =
                            (ITypeSerializer)Activator.CreateInstance
                                (typeof(ObjectSerializer<>).MakeGenericType
                                    (typeof(TMsg), typeof(object)), pi, id, rootAllSerializers, mapDerivedAttribs?.Mapping)!;
                    else
                        foundSerializer =
                            (ITypeSerializer)Activator.CreateInstance
                                (typeof(ObjectSerializer<>).MakeGenericType
                                    (typeof(TMsg), pi.PropertyType), pi, id, rootAllSerializers)!;
                }
            }
            interrogatingTypes.Pop();
        }

        if (foundSerializer != null)
        {
            return foundSerializer;
        }
        throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
    }

    private void MandatoryArrayTypes
    (PropertyInfo pi, Dictionary<string, ITypeSerializer?> currObjSerializers
      , Dictionary<string, ISerializer?> rootAllSerializers)
    {
        if (interrogatingTypes.Contains(pi.PropertyType)) return;
        var piPath = PropertyInfoFullPath(pi);
        currObjSerializers[piPath] = ResolveArrayTypes(pi, ushort.MaxValue, currObjSerializers, rootAllSerializers);
        rootAllSerializers[piPath] = currObjSerializers[piPath]!;
    }


    private void MandatoryBasicTypes
    (PropertyInfo pi, Dictionary<string, ITypeSerializer?> currObjSerializers
      , Dictionary<string, ISerializer?> rootAllSerializers)
    {
        if (interrogatingTypes.Contains(pi.PropertyType)) return;
        var piPath = PropertyInfoFullPath(pi);
        currObjSerializers[piPath] = ResolveBasicTypesSerializer(pi, ushort.MaxValue, currObjSerializers, rootAllSerializers);
        rootAllSerializers[piPath] = currObjSerializers[piPath]!;
    }

    private abstract class Serializer<TP>(PropertyInfo property, ushort id) : ITypeSerializer
    {
        protected readonly Func<TMsg, TP?> Get = (Func<TMsg, TP>)Delegate.CreateDelegate(typeof(Func<TMsg, TP>), property.GetGetMethod(true)!);
        protected readonly ushort          Id  = id;

        protected bool IsOptional => Id != ushort.MaxValue;

        public Type SerializesType => typeof(TP);

        public abstract unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr);
    }

    private class ObjectSerializer<TO>(PropertyInfo property, ushort id, Dictionary<string, ISerializer?> allSerializers)
        : Serializer<object>(property, id)
        where TO : class, new()
    {
        private readonly Dictionary<Type, IOrxSerializer> serializerLookup = new();
        protected        OrxByteSerializer<TO>            ItemSerializer   = new(allSerializers);
        private          Dictionary<ushort, Type>?        mapping          = new();

        // ReSharper disable once UnusedMember.Local
        public ObjectSerializer
        (PropertyInfo property, ushort id, Dictionary<string, ISerializer?> allSerializers
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

        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
            if (mapping is { Count: > 0 })
            {
                var subClassKey = mapping.FirstOrDefault(kvp => kvp.Value == obj.GetType()).Key;
                if (subClassKey == 0) return false;
                StreamByteOps.ToBytes(ref ptr, subClassKey);
                var mappedSerializer = serializerLookup[obj.GetType()];
                if ((size = mappedSerializer.Serialize(obj, ptr, endPtr)) == 0) return false;
            }
            else if ((size = ItemSerializer.Serialize(obj, ptr, endPtr)) == 0)
            {
                return false;
            }

            StreamByteOps.ToBytes(ref sizePtr, (ushort)size);
            ptr += size;

            return true;
        }
    }

    private class SelfSerializer : ObjectSerializer<TMsg>
    {
        public SelfSerializer
        (PropertyInfo property, ushort id, Dictionary<ushort, Type> mapping,
            OrxByteSerializer<TMsg> serializer, Dictionary<string, ISerializer?> allSerializers)
            : base(property, id, allSerializers)
        {
            ItemSerializer = serializer;
            ImportMapping(mapping);
        }
    }

    #region Collections

    private sealed class BoolArraySerializer(PropertyInfo property, ushort id) : Serializer<bool[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableBoolArraySerializer(PropertyInfo property, ushort id) : Serializer<bool?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    *ptr = array![i] == true ? (byte)0xFF : (byte)00;
                    ptr++;
                }

                return true;
            }

            return false;
        }
    }

    private sealed class BoolListSerializer(PropertyInfo property, ushort id) : Serializer<List<bool>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableBoolListSerializer(PropertyInfo property, ushort id) : Serializer<List<bool?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    *ptr = array![i] == true ? (byte)0xFF : (byte)00;
                    ptr++;
                }

                return true;
            }

            return false;
        }
    }

    private sealed class ByteArraySerializer(PropertyInfo property, ushort id) : Serializer<byte[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableByteArraySerializer(PropertyInfo property, ushort id) : Serializer<byte?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    *ptr = array![i] ?? 0;
                    ptr++;
                }

                return true;
            }

            return false;
        }
    }

    private sealed class ByteListSerializer(PropertyInfo property, ushort id) : Serializer<List<byte>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableByteListSerializer(PropertyInfo property, ushort id) : Serializer<List<byte?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    *ptr = array![i] ?? 0;
                    ptr++;
                }

                return true;
            }

            return false;
        }
    }

    private sealed class ShortArraySerializer(PropertyInfo property, ushort id) : Serializer<short[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableShortArraySerializer(PropertyInfo property, ushort id) : Serializer<short?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class ShortListSerializer(PropertyInfo property, ushort id) : Serializer<List<short>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableShortListSerializer(PropertyInfo property, ushort id) : Serializer<List<short?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class UShortArraySerializer(PropertyInfo property, ushort id) : Serializer<ushort[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableUShortArraySerializer(PropertyInfo property, ushort id) : Serializer<ushort?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class UShortListSerializer(PropertyInfo property, ushort id) : Serializer<List<ushort>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableUShortListSerializer(PropertyInfo property, ushort id) : Serializer<List<ushort?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class IntArraySerializer(PropertyInfo property, ushort id) : Serializer<int[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableIntArraySerializer(PropertyInfo property, ushort id) : Serializer<int?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class IntListSerializer(PropertyInfo property, ushort id) : Serializer<List<int>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableIntListSerializer(PropertyInfo property, ushort id) : Serializer<List<int?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    var intValue = array![i] ?? 0;
                    StreamByteOps.ToBytes(ref ptr, intValue);
                }
                return true;
            }

            return false;
        }
    }

    private sealed class UIntArraySerializer(PropertyInfo property, ushort id) : Serializer<uint[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableUIntArraySerializer(PropertyInfo property, ushort id) : Serializer<uint?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class UIntListSerializer(PropertyInfo property, ushort id) : Serializer<List<uint>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableUIntListSerializer(PropertyInfo property, ushort id) : Serializer<List<uint?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class LongArraySerializer(PropertyInfo property, ushort id) : Serializer<long[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableLongArraySerializer(PropertyInfo property, ushort id) : Serializer<long?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class ULongArraySerializer(PropertyInfo property, ushort id) : Serializer<ulong[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableULongArraySerializer(PropertyInfo property, ushort id) : Serializer<ulong?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Length ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class LongListSerializer(PropertyInfo property, ushort id) : Serializer<List<long>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableLongListSerializer(PropertyInfo property, ushort id) : Serializer<List<long?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class ULongListSerializer(PropertyInfo property, ushort id) : Serializer<List<ulong>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableULongListSerializer(PropertyInfo property, ushort id) : Serializer<List<ulong?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                for (var i = 0; i < (array?.Count ?? 0); i++) StreamByteOps.ToBytes(ref ptr, array![i] ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class DecimalArraySerializer(PropertyInfo property, ushort id) : Serializer<decimal[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    var elementDecimal    = array![i];
                    var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(elementDecimal)[3])[2];
                    var roundingNoDecimal = (long)((decimal)Math.Pow(10, decimalPlaces) * elementDecimal);
                    StreamByteOps.ToBytes(ref ptr, decimalPlaces);
                    StreamByteOps.ToBytes(ref ptr, roundingNoDecimal);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class NullableDecimalArraySerializer(PropertyInfo property, ushort id) : Serializer<decimal?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    var elementDecimal    = array![i] ?? 0m;
                    var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(elementDecimal)[3])[2];
                    var roundingNoDecimal = (long)((decimal)Math.Pow(10, decimalPlaces) * elementDecimal);
                    StreamByteOps.ToBytes(ref ptr, decimalPlaces);
                    StreamByteOps.ToBytes(ref ptr, roundingNoDecimal);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class DecimalListSerializer(PropertyInfo property, ushort id) : Serializer<List<decimal>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    var elementDecimal    = array![i];
                    var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(elementDecimal)[3])[2];
                    var roundingNoDecimal = (long)((decimal)Math.Pow(10, decimalPlaces) * elementDecimal);
                    StreamByteOps.ToBytes(ref ptr, decimalPlaces);
                    StreamByteOps.ToBytes(ref ptr, roundingNoDecimal);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class NullableDecimalListSerializer(PropertyInfo property, ushort id) : Serializer<List<decimal?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    var elementDecimal    = array![i] ?? 0m;
                    var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(elementDecimal)[3])[2];
                    var roundingNoDecimal = (long)((decimal)Math.Pow(10, decimalPlaces) * elementDecimal);
                    StreamByteOps.ToBytes(ref ptr, decimalPlaces);
                    StreamByteOps.ToBytes(ref ptr, roundingNoDecimal);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class DateTimeArraySerializer(PropertyInfo property, ushort id) : Serializer<DateTime[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    var elementDateTime           = array![i];
                    var timeAsNanosSinceUnixEpoch = (elementDateTime.Ticks - DateTimeConstants.UnixEpochTicks) * 100;
                    StreamByteOps.ToBytes(ref ptr, timeAsNanosSinceUnixEpoch);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class NullableDateTimeArraySerializer(PropertyInfo property, ushort id) : Serializer<DateTime?[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    var elementDateTime           = array![i] ?? DateTime.MinValue;
                    var timeAsNanosSinceUnixEpoch = (elementDateTime.Ticks - DateTimeConstants.UnixEpochTicks) * 100;
                    StreamByteOps.ToBytes(ref ptr, timeAsNanosSinceUnixEpoch);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class DateTimeListSerializer(PropertyInfo property, ushort id) : Serializer<List<DateTime>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    var elementDateTime           = array![i];
                    var timeAsNanosSinceUnixEpoch = (elementDateTime.Ticks - DateTimeConstants.UnixEpochTicks) * 100;
                    StreamByteOps.ToBytes(ref ptr, timeAsNanosSinceUnixEpoch);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class NullableDateTimeListSerializer(PropertyInfo property, ushort id) : Serializer<List<DateTime?>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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
                    var elementDateTime           = array![i] ?? DateTime.MinValue;
                    var timeAsNanosSinceUnixEpoch = (elementDateTime.Ticks - DateTimeConstants.UnixEpochTicks) * 100;
                    StreamByteOps.ToBytes(ref ptr, timeAsNanosSinceUnixEpoch);
                }

                return true;
            }

            return false;
        }
    }

    private sealed class StringArraySerializer(PropertyInfo property, ushort id) : Serializer<string[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var array       = Get(message);
            var sizeOfArray = ptr;

            if (Id != ushort.MaxValue)
            {
                if (ptr + 256 > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray =  ptr;
                ptr         += 2;
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                var arrayLength = array?.Length ?? 0;
                StreamByteOps.ToBytes(ref ptr, (ushort)arrayLength);
                for (var i = 0; i < arrayLength; i++)
                    if (ptr + arrayLength * 256 <= endPtr)
                        StreamByteOps.ToBytesWithSizeHeader(ref ptr, array![i], 256);
                if (Id != ushort.MaxValue) StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray - OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class StringListSerializer(PropertyInfo property, ushort id) : Serializer<List<string>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var array       = Get(message);
            var sizeOfArray = ptr;

            if (Id != ushort.MaxValue)
            {
                if (ptr + 256 > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray =  ptr;
                ptr         += 2;
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                var arrayCount = array?.Count ?? 0;
                StreamByteOps.ToBytes(ref ptr, (ushort)arrayCount);
                for (var i = 0; i < arrayCount; i++)
                    if (ptr + arrayCount * 256 <= endPtr)
                        StreamByteOps.ToBytesWithSizeHeader(ref ptr, array![i], 256);
                if (Id != ushort.MaxValue) StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray - OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class MutableStringArraySerializer(PropertyInfo property, ushort id) : Serializer<MutableString[]>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var array       = Get(message);
            var sizeOfArray = ptr;

            if (Id != ushort.MaxValue)
            {
                if (ptr + 1024 > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray =  ptr;
                ptr         += 2;
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                var arrayLength = array?.Length ?? 0;
                StreamByteOps.ToBytes(ref ptr, (ushort)arrayLength);
                for (var i = 0; i < arrayLength; i++)
                    if (ptr + arrayLength * 1024 <= endPtr)
                        StreamByteOps.ToBytesWithSizeHeader(ref ptr, array![i], 1024);
                if (Id != ushort.MaxValue) StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray - OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class MutableStringListSerializer(PropertyInfo property, ushort id) : Serializer<List<MutableString>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var array       = Get(message);
            var sizeOfArray = ptr;

            if (Id != ushort.MaxValue)
            {
                if (ptr + 1024 > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray =  ptr;
                ptr         += 2;
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                var arrayCount = array?.Count ?? 0;
                StreamByteOps.ToBytes(ref ptr, (ushort)arrayCount);
                for (var i = 0; i < arrayCount; i++)
                    if (ptr + arrayCount * 1024 <= endPtr)
                        StreamByteOps.ToBytesWithSizeHeader(ref ptr, array![i], 1024);
                if (Id != ushort.MaxValue) StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray - OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class MapSerializer(PropertyInfo property, ushort id) : Serializer<Dictionary<string, string>>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var dictionary = Get(message);
            var dic        = dictionary;

            byte* sizePtr = null;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizePtr =  ptr;
                ptr     += OrxConstants.UInt16Sz;
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

    private sealed class ObjectArraySerializer<TObj>(PropertyInfo property, ushort id) : Serializer<TObj[]>(property, id)
        where TObj : class, new()
    {
        private readonly OrxByteSerializer<TObj> itemSerializer = new();

        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var array   = Get(message);
            var wasNull = array == null;
            array ??= [];

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
                if ((size = itemSerializer.Serialize(array[i], ptr + OrxConstants.UInt16Sz, endPtr)) == 0) return false;
                StreamByteOps.ToBytes(ref ptr, (ushort)size);
                ptr += size;
            }

            StreamByteOps.ToBytes(ref objectsListSize, (ushort)(ptr - objectListStart));

            return true;
        }
    }

    private sealed class ObjectListSerializer<TObj>(PropertyInfo property, ushort id) : Serializer<List<TObj>>(property, id)
        where TObj : class, new()
    {
        private readonly OrxByteSerializer<TObj> itemSerializer = new();

        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var list    = Get(message);
            var wasNull = list == null;
            list ??= new List<TObj>();
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
                if ((size = itemSerializer.Serialize(list[i], ptr + OrxConstants.UInt16Sz, endPtr)) == 0) return false;
                StreamByteOps.ToBytes(ref ptr, (ushort)size);
                ptr += size;
            }

            StreamByteOps.ToBytes(ref objectsListSize, (ushort)(ptr - objectListStart));

            return true;
        }
    }

    #endregion

    #region Basic types

    private sealed class BoolSerializer(PropertyInfo property, ushort id) : Serializer<bool>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableBoolSerializer(PropertyInfo property, ushort id) : Serializer<bool?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var boolValue = Get(message);
            if (boolValue == null && IsOptional) return true;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt8Sz);
            }

            if (ptr + OrxConstants.UInt8Sz <= endPtr)
            {
                *ptr++ = boolValue == true ? (byte)1 : (byte)0;
                return true;
            }

            return false;
        }
    }

    private sealed class ByteSerializer(PropertyInfo property, ushort id) : Serializer<byte>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableByteSerializer(PropertyInfo property, ushort id) : Serializer<byte?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var byteValue = Get(message);
            if (byteValue == null && IsOptional) return true;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt8Sz);
            }

            if (ptr + OrxConstants.UInt8Sz <= endPtr)
            {
                *ptr++ = byteValue ?? 0;
                return true;
            }

            return false;
        }
    }

    private sealed class UShortSerializer(PropertyInfo property, ushort id) : Serializer<ushort>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableUShortSerializer(PropertyInfo property, ushort id) : Serializer<ushort?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var shortValue = Get(message);
            if (shortValue == null && IsOptional) return true;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt16Sz);
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, shortValue ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class ShortSerializer(PropertyInfo property, ushort id) : Serializer<short>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableShortSerializer(PropertyInfo property, ushort id) : Serializer<short?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var ushortValue = Get(message);
            if (ushortValue == null && IsOptional) return true;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt16Sz);
            }

            if (ptr + OrxConstants.UInt16Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, ushortValue ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class UIntSerializer(PropertyInfo property, ushort id) : Serializer<uint>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableUIntSerializer(PropertyInfo property, ushort id) : Serializer<uint?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var uintValue = Get(message);
            if (uintValue == null && IsOptional) return true;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt32Sz);
            }
            if (ptr + OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, uintValue ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class IntSerializer(PropertyInfo property, ushort id) : Serializer<int>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableIntSerializer(PropertyInfo property, ushort id) : Serializer<int?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var intValue = Get(message);
            if (intValue == null && IsOptional) return true;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt32Sz);
            }

            if (ptr + OrxConstants.UInt32Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, intValue ?? 0m);
                return true;
            }

            return false;
        }
    }

    private sealed class StringSerializer(PropertyInfo property, ushort id) : Serializer<string>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var str = Get(message);
            if (str == null) return true;
            var sizeOfArray = ptr;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray =  ptr;
                ptr         += 2;
            }

            if (ptr + str.Length * OrxConstants.UInt8Sz + 1 <= endPtr)
            {
                StreamByteOps.ToBytesWithSizeHeader(ref ptr, str, ushort.MaxValue);
                if (Id != ushort.MaxValue) StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray + OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class MutableStringSerializer(PropertyInfo property, ushort id) : Serializer<MutableString>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var str = Get(message);
            if (str == null) return true;
            var sizeOfArray = ptr;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                sizeOfArray =  ptr;
                ptr         += 2;
            }

            if (ptr + str.Length * OrxConstants.UInt8Sz + 1 <= endPtr)
            {
                StreamByteOps.ToBytesWithSizeHeader(ref ptr, str, ushort.MaxValue);
                if (Id != ushort.MaxValue) StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray + OrxConstants.UInt16Sz));
                return true;
            }

            return false;
        }
    }

    private sealed class DecimalSerializer(PropertyInfo property, ushort id) : Serializer<decimal>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt8Sz + OrxConstants.UInt32Sz));
            }

            if (ptr + OrxConstants.UInt8Sz + OrxConstants.UInt32Sz <= endPtr)
            {
                var value  = Get(message);
                var factor = OrxScaling.GetScalingFactor(value);
                *ptr++ = factor;
                StreamByteOps.ToBytes(ref ptr, OrxScaling.Scale(value, factor));
                return true;
            }

            return false;
        }
    }

    private sealed class NullableDecimalSerializer(PropertyInfo property, ushort id) : Serializer<decimal?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var decimalValue = Get(message);
            if (decimalValue == null && IsOptional) return true;
            if (Id != ushort.MaxValue)
            {
                if (ptr + 2 * OrxConstants.UInt16Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt8Sz + OrxConstants.UInt32Sz));
            }

            if (ptr + OrxConstants.UInt8Sz + OrxConstants.UInt32Sz <= endPtr)
            {
                var value  = decimalValue ?? 0m;
                var factor = OrxScaling.GetScalingFactor(value);
                *ptr++ = factor;
                StreamByteOps.ToBytes(ref ptr, OrxScaling.Scale(value, factor));
                return true;
            }

            return false;
        }
    }

    private sealed class LongSerializer(PropertyInfo property, ushort id) : Serializer<long>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableLongSerializer(PropertyInfo property, ushort id) : Serializer<long?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var longValue = Get(message);
            if (longValue == null && IsOptional) return true;
            if (Id != ushort.MaxValue)
            {
                if (ptr + OrxConstants.UInt16Sz + 2 * OrxConstants.UInt64Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt64Sz);
            }

            if (ptr + OrxConstants.UInt64Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, longValue ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class ULongSerializer(PropertyInfo property, ushort id) : Serializer<ulong>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableULongSerializer(PropertyInfo property, ushort id) : Serializer<ulong?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var ulongValue = Get(message);
            if (ulongValue == null && IsOptional) return true;
            if (Id != ushort.MaxValue)
            {
                if (ptr + OrxConstants.UInt16Sz + 2 * OrxConstants.UInt64Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt64Sz);
            }

            if (ptr + OrxConstants.UInt64Sz <= endPtr)
            {
                StreamByteOps.ToBytes(ref ptr, ulongValue ?? 0);
                return true;
            }

            return false;
        }
    }

    private sealed class DateTimeSerializer(PropertyInfo property, ushort id) : Serializer<DateTime>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
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

    private sealed class NullableDateTimeSerializer(PropertyInfo property, ushort id) : Serializer<DateTime?>(property, id)
    {
        public override unsafe bool Serialize(TMsg message, ref byte* ptr, byte* endPtr)
        {
            var dateTime = Get(message);
            if (dateTime.IsNullOrUnixEpochOrDefault() && IsOptional) return true;
            if (IsOptional)
            {
                if (ptr + OrxConstants.UInt16Sz + 2 * OrxConstants.UInt64Sz > endPtr) return false;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt64Sz);
            }

            if (ptr + OrxConstants.UInt64Sz <= endPtr)
            {
                var timeAsNanosSinceUnixEpoch = dateTime != null ? (dateTime.Value.Ticks - DateTimeConstants.UnixEpochTicks) * 100 : 0;
                StreamByteOps.ToBytes(ref ptr, timeAsNanosSinceUnixEpoch);
                return true;
            }

            return false;
        }
    }

    #endregion

    #endregion
}
