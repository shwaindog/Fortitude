using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeIO.Protocols.ORX.Serialization
{
    public class OrxByteSerializer<Tm> : IOrxSerializer where Tm : class
    {
        private readonly ISerializer[] serializers;

        public OrxByteSerializer()
        {
            List<ISerializer> all = new List<ISerializer>();
            
            foreach (var kv in OrxMandatoryField.FindAll(typeof(Tm)))
            {
                PropertyInfo pi = kv.Value;
                if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                {
                    ManditoryBasicTypes(pi, all);
                }
                else
                {
                    ManditoryArrayTypes(pi, all);
                }
            }
            
            foreach (var kv in OrxOptionalField.FindAll(typeof(Tm)))
            {
                ushort id = kv.Key;
                PropertyInfo pi = kv.Value;
                if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                {
                    OptionalBasicTypes(pi, all, id);
                }
                else
                {
                    OptionalArrayTypes(pi, all, id);
                }
            }

            serializers = all.ToArray();
        }

        public unsafe int Serialize(object message, byte[] buffer, int msgOffset, int headerOffset)
        {
            fixed (byte* fptr = buffer)
            {
                return Serialize(message, fptr + msgOffset + headerOffset, fptr + msgOffset, fptr + buffer.Length);
            }
        }

        public unsafe int Serialize(object message, byte* ptr, byte* msgStart, byte* endPtr)
        {
            Tm msg = message as Tm;
            byte* dataStart = ptr;
            for (int i = 0; i < serializers.Length; i++)
            {
                if (!serializers[i].Serialize(msg, ref ptr, msgStart, endPtr))
                {
                    return 0;
                }
            }
            return (int)(ptr - dataStart);
        }

        #region Inner members

        private interface ISerializer
        {
            unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr);
        }

        private void OptionalArrayTypes(PropertyInfo pi, List<ISerializer> all, ushort id)
        {
            if (pi.PropertyType == typeof(bool[]))
            {
                all.Add(new BoolArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<bool>))
            {
                all.Add(new BoolListSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(byte[]))
            {
                all.Add(new ByteArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<byte>))
            {
                all.Add(new ByteListSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(short[]))
            {
                all.Add(new ShortArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<short>))
            {
                all.Add(new ShortListSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(ushort[]))
            {
                all.Add(new UShortArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<ushort>))
            {
                all.Add(new UShortListSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(int[]))
            {
                all.Add(new IntArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<int>))
            {
                all.Add(new IntListSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(uint[]))
            {
                all.Add(new UIntArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<uint>))
            {
                all.Add(new UIntListSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(long[]))
            {
                all.Add(new LongArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<long>))
            {
                all.Add(new LongListSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(decimal[]))
            {
                all.Add(new DecimalArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<decimal>))
            {
                all.Add(new DecimalListSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(string[]))
            {
                all.Add(new StringArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<string>))
            {
                all.Add(new StringListSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(MutableString[]))
            {
                all.Add(new MutableStringArraySerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(List<MutableString>))
            {
                all.Add(new MutableStringListSerializer(pi, id));
            }
            else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                     pi.PropertyType.GenericTypeArguments[0].IsClass)
            {
                all.Add(Activator.CreateInstance(typeof(ObjectListSerializer<>).MakeGenericType(typeof(Tm),
                    pi.PropertyType.GenericTypeArguments[0]), pi, id) as ISerializer);
            }
            // ReSharper disable once PossibleNullReferenceException
            else if (pi.PropertyType.GetElementType().IsClass && pi.PropertyType.GetArrayRank() == 1)
            {
                all.Add(Activator.CreateInstance(typeof(ObjectArraySerializer<>)
                    .MakeGenericType(typeof(Tm), pi.PropertyType.GetElementType()), pi, id) as ISerializer);
            }
            else
            {
                throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
            }
        }

        private void OptionalBasicTypes(PropertyInfo pi, List<ISerializer> all, ushort id)
        {
            if (pi.PropertyType == typeof(bool))
            {
                all.Add(new BoolSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(byte) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(byte))
            {
                all.Add(new ByteSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(short) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(short))
            {
                all.Add(new ShortSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(ushort) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(ushort))
            {
                all.Add(new UShortSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(int) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(int))
            {
                all.Add(new IntSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(uint) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(uint))
            {
                all.Add(new UIntSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(decimal))
            {
                all.Add(new DecimalSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(long))
            {
                all.Add(new LongSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(DateTime))
            {
                all.Add(new DateTimeSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(string))
            {
                all.Add(new StringSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(MutableString))
            {
                all.Add(new MutableStringSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(Dictionary<string, string>))
            {
                all.Add(new MapSerializer(pi, id));
            }
            else if (pi.PropertyType == typeof(Tm))
            {
                var customAttributes = (OrxOptionalField) pi
                    .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                all.Add(new SelfSerializer(pi, id, customAttributes?.Mapping, this));
            }
            else if (pi.PropertyType.IsClass)
            {
                var customAttributes = (OrxOptionalField) pi
                    .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                all.Add(Activator.CreateInstance(typeof(ObjectSerializer<>)
                        .MakeGenericType(typeof(Tm), pi.PropertyType), pi, id, customAttributes?.Mapping)
                    as ISerializer);
            }
            else
            {
                throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
            }
        }

        private void ManditoryArrayTypes(PropertyInfo pi, List<ISerializer> all)
        {

            if (pi.PropertyType == typeof(bool[]))
            {
                all.Add(new BoolArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<bool>))
            {
                all.Add(new BoolListSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(byte[]))
            {
                all.Add(new ByteArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<byte>))
            {
                all.Add(new ByteListSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(short[]))
            {
                all.Add(new ShortArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<short>))
            {
                all.Add(new ShortListSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(ushort[]))
            {
                all.Add(new UShortArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<ushort>))
            {
                all.Add(new UShortListSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(int[]))
            {
                all.Add(new IntArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<int>))
            {
                all.Add(new IntListSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(uint[]))
            {
                all.Add(new UIntArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<uint>))
            {
                all.Add(new UIntListSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(long[]))
            {
                all.Add(new LongArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<long>))
            {
                all.Add(new LongListSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(decimal[]))
            {
                all.Add(new DecimalArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<decimal>))
            {
                all.Add(new DecimalListSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(string[]))
            {
                all.Add(new StringArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<string>))
            {
                all.Add(new StringListSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(MutableString[]))
            {
                all.Add(new MutableStringArraySerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(List<MutableString>))
            {
                all.Add(new MutableStringListSerializer(pi, ushort.MaxValue));
            }
            // ReSharper disable once PossibleNullReferenceException
            else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                     pi.PropertyType.GenericTypeArguments[0].IsClass)
            {
                all.Add(Activator.CreateInstance(typeof(ObjectListSerializer<>).MakeGenericType(typeof(Tm),
                    pi.PropertyType.GenericTypeArguments[0]), pi, ushort.MaxValue) as ISerializer);
            }
            // ReSharper disable once PossibleNullReferenceException
            else if (pi.PropertyType.GetElementType().IsClass && pi.PropertyType.GetArrayRank() == 1)
            {
                all.Add(Activator.CreateInstance(typeof(ObjectArraySerializer<>).MakeGenericType(typeof(Tm),
                    pi.PropertyType.GetElementType()), pi, ushort.MaxValue) as ISerializer);
            }
            else
            {
                throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
            }
        }

        private void ManditoryBasicTypes(PropertyInfo pi, List<ISerializer> all)
        {
            if (pi.PropertyType == typeof(bool))
            {
                all.Add(new BoolSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(byte) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(byte))
            {
                all.Add(new ByteSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(short) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(short))
            {
                all.Add(new ShortSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(ushort) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(ushort))
            {
                all.Add(new UShortSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(int) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(int))
            {
                all.Add(new IntSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(uint) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(uint))
            {
                all.Add(new UIntSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(decimal))
            {
                all.Add(new DecimalSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(long))
            {
                all.Add(new LongSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(DateTime))
            {
                all.Add(new DateTimeSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(string))
            {
                all.Add(new StringSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(MutableString))
            {
                all.Add(new MutableStringSerializer(pi, ushort.MaxValue));
            }
            else if (pi.PropertyType == typeof(Tm))
            {
                var customAttributes = (OrxMandatoryField) pi
                    .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                all.Add(new SelfSerializer(pi, ushort.MaxValue, customAttributes?.Mapping, this));
            }
            else if (pi.PropertyType.IsClass)
            {
                var customAttributes = (OrxMandatoryField) pi
                    .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                all.Add(Activator.CreateInstance(typeof(ObjectSerializer<>)
                        .MakeGenericType(typeof(Tm), pi.PropertyType), pi, ushort.MaxValue, customAttributes?.Mapping)
                    as ISerializer);
            }
            else
            {
                throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
            }
        }

        private abstract class Serializer<Tp> : ISerializer
        {
            protected readonly ushort Id;

            protected Serializer(PropertyInfo property, ushort id)
            {
                Id = id;
                Get = (Func<Tm, Tp>)Delegate.CreateDelegate(typeof(Func<Tm, Tp>), property.GetGetMethod(true));
            }

            protected bool IsOptional => Id != ushort.MaxValue;

            public abstract unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr);

            protected readonly Func<Tm, Tp> Get;
        }

        private class ObjectSerializer<To> : Serializer<To> where To : class
        {
            private Dictionary<ushort, Type> mapping;
            protected OrxByteSerializer<To> ItemSerializer;
            private Dictionary<Type, IOrxSerializer> serializerLookup;

            protected ObjectSerializer(PropertyInfo property, ushort id) : base(property, id)
            {
            }

            // ReSharper disable once UnusedMember.Local
            public ObjectSerializer(PropertyInfo property, ushort id, Dictionary<ushort, Type> mapping) 
                : this(property, id)
            {
                ItemSerializer = new OrxByteSerializer<To>();
                ImportMapping(mapping);
            }

            protected void ImportMapping(Dictionary<ushort, Type> typeLookupMapping)
            {
                mapping = typeLookupMapping;
                if (typeLookupMapping != null)
                {
                    serializerLookup = new Dictionary<Type, IOrxSerializer>();
                    foreach (var keyValuePair in typeLookupMapping)
                    {
                        var mappedSerializer = (IOrxSerializer) Activator.CreateInstance(typeof(OrxByteSerializer<>)
                            .MakeGenericType(keyValuePair.Value));
                        serializerLookup.Add(keyValuePair.Value, mappedSerializer);
                    }
                }
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                To obj = Get(message);
                if (obj == null && IsOptional) return true; 
                if (ptr + 2*OrxConstants.UInt16Sz > endPtr)
                {
                    return false;
                }
                StreamByteOps.ToBytes(ref ptr, Id);
                byte* sizePtr = ptr;
                ptr += 2;
                if (obj == null)
                {
                    StreamByteOps.ToBytes(ref sizePtr, (ushort)0);
                    return true;
                }

                int size;
                if (mapping != null && mapping.Count > 0)
                {
                    var mapEntry = mapping.FirstOrDefault(kvp => kvp.Value == obj.GetType()).Key;
                    if (mapEntry == 0)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, mapEntry);
                    var mappedSerializer = serializerLookup[obj.GetType()];
                    if ((size = mappedSerializer.Serialize(obj, ptr, msgStart, endPtr)) == 0)
                    {
                        return false;
                    }
                }
                else if ((size = ItemSerializer.Serialize(obj, ptr, msgStart, endPtr)) == 0)
                {
                    return false;
                }
                StreamByteOps.ToBytes(ref sizePtr, (ushort) size);
                ptr += size;

                return true;
            }
        }

        private class SelfSerializer : ObjectSerializer<Tm>
        {
            public SelfSerializer(PropertyInfo property, ushort id, Dictionary<ushort, Type> mapping,
                OrxByteSerializer<Tm> serializer)
                : base(property, id)
            {
                ItemSerializer = serializer;
                ImportMapping(mapping);
            }
        }

        #region Collections
        
        private sealed class BoolArraySerializer : Serializer<bool[]>
        {
            public BoolArraySerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                bool[] array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        *ptr = array[i] ? (byte)0xFF : (byte)00;
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
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<bool> array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Count * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        *ptr = array[i] ? (byte)0xFF : (byte)00;
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
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                byte[] array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        *ptr = array[i];
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
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<byte> array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Count * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        *ptr = array[i];
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
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                short[] array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class ShortListSerializer : Serializer<List<short>>
        {
            public ShortListSerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<short> array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Count * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class UShortArraySerializer : Serializer<ushort[]>
        {
            public UShortArraySerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                ushort[] array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class UShortListSerializer : Serializer<List<ushort>>
        {
            public UShortListSerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<ushort> array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Count * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class IntArraySerializer : Serializer<int[]>
        {
            public IntArraySerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                int[] array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class IntListSerializer : Serializer<List<int>>
        {
            public IntListSerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<int> array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Count * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class UIntArraySerializer : Serializer<uint[]>
        {
            public UIntArraySerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                uint[] array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class UIntListSerializer : Serializer<List<uint>>
        {
            public UIntListSerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<uint> array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Count * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class LongArraySerializer : Serializer<long[]>
        {
            public LongArraySerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                long[] array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class LongListSerializer : Serializer<List<long>>
        {
            public LongListSerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<long> array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Count * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        StreamByteOps.ToBytes(ref ptr, array[i]);
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class DecimalArraySerializer : Serializer<decimal[]>
        {
            public DecimalArraySerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                decimal[] array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Length * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        var elementDecimal = array[i];
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
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<decimal> array = Get(message);
                if (Id != ushort.MaxValue)
                {
                    if (array == null) return true;
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt16Sz + array.Count));
                }
                if (ptr + OrxConstants.UInt16Sz + array.Count * OrxConstants.UInt32Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        var elementDecimal = array[i];
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
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                string[] array = Get(message);
                byte* sizeOfArray = ptr;

                if (Id != ushort.MaxValue)
                {
                    if (ptr + 256 > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    sizeOfArray = ptr;
                    ptr += 2;
                }
                if (ptr + OrxConstants.UInt16Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (ptr + array.Length * 256 <= endPtr)
                        {
                            StreamByteOps.ToBytesWithSizeHeader(ref ptr, array[i], 256);
                        }
                    }
                    if (Id != ushort.MaxValue)
                    {
                        StreamByteOps.ToBytes(ref sizeOfArray, (ushort) (ptr - sizeOfArray - OrxConstants.UInt16Sz));
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class StringListSerializer : Serializer<List<string>>
        {
            public StringListSerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<string> array = Get(message);
                byte* sizeOfArray = ptr;

                if (Id != ushort.MaxValue)
                {
                    if (ptr + 256 > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    sizeOfArray = ptr;
                    ptr += 2;
                }
                if (ptr + OrxConstants.UInt16Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        if (ptr + array.Count * 256 <= endPtr)
                        {
                            StreamByteOps.ToBytesWithSizeHeader(ref ptr, array[i], 256);
                        }
                    }
                    if (Id != ushort.MaxValue)
                    {
                        StreamByteOps.ToBytes(ref sizeOfArray, (ushort) (ptr - sizeOfArray - OrxConstants.UInt16Sz));
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class MutableStringArraySerializer : Serializer<MutableString[]>
        {
            public MutableStringArraySerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                MutableString[] array = Get(message);
                byte* sizeOfArray = ptr;

                if (Id != ushort.MaxValue)
                {
                    if (ptr + 1024 > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    sizeOfArray = ptr;
                    ptr += 2;
                }
                if (ptr + OrxConstants.UInt16Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (ptr + array.Length * 1024 <= endPtr)
                        {
                            StreamByteOps.ToBytesWithSizeHeader(ref ptr, array[i], 1024);
                        }
                    }
                    if (Id != ushort.MaxValue)
                    {
                        StreamByteOps.ToBytes(ref sizeOfArray, (ushort) (ptr - sizeOfArray - OrxConstants.UInt16Sz));
                    }
                    return true;
                }
                return false;
            }
        }

        private sealed class MutableStringListSerializer : Serializer<List<MutableString>>
        {
            public MutableStringListSerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<MutableString> array = Get(message);
                byte* sizeOfArray = ptr;

                if (Id != ushort.MaxValue)
                {
                    if (ptr + 1024 > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    sizeOfArray = ptr;
                    ptr += 2;
                }
                if (ptr + OrxConstants.UInt16Sz <= endPtr)
                {
                    StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        if (ptr + array.Count * 1024 <= endPtr)
                        {
                            StreamByteOps.ToBytesWithSizeHeader(ref ptr, array[i], 1024);
                        }
                    }
                    if (Id != ushort.MaxValue)
                    {
                        StreamByteOps.ToBytes(ref sizeOfArray, (ushort) (ptr - sizeOfArray - OrxConstants.UInt16Sz));
                    }
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
                Dictionary<string, string> dic = Get(message);

                byte* sizePtr = null;
                if (Id != ushort.MaxValue)
                {
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    sizePtr = ptr;
                    ptr += OrxConstants.UInt16Sz;
                }

                if (ptr + OrxConstants.UInt32Sz > endPtr)
                {
                    return false;
                }
                StreamByteOps.ToBytes(ref ptr, (uint)dic.Count);

                int size = OrxConstants.UInt32Sz;
                foreach (var kv in Get(message))
                {
                    int recLen = (kv.Key.Length + kv.Value.Length + 2) * OrxConstants.UInt8Sz;
                    if (ptr + recLen > endPtr)
                    {
                        return false;
                    }
                    size += recLen;

                    StreamByteOps.ToBytesWithSizeHeader(ref ptr, kv.Key, ushort.MaxValue);
                    StreamByteOps.ToBytesWithSizeHeader(ref ptr, kv.Value, ushort.MaxValue);
                }

                if (sizePtr != null)
                {
                    StreamByteOps.ToBytes(ref sizePtr, (ushort)size);
                }

                return true;
            }
        }

        private sealed class ObjectArraySerializer<To> : Serializer<To[]> where To : class
        {
            private readonly OrxByteSerializer<To> itemSerializer = new OrxByteSerializer<To>();

            public ObjectArraySerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                To[] array = Get(message);

                if (ptr + (2 + array.Length) * OrxConstants.UInt16Sz > endPtr)
                {
                    return false;
                }
                StreamByteOps.ToBytes(ref ptr, Id);
                byte* objectsListSize = ptr;
                ptr += OrxConstants.UInt16Sz;
                StreamByteOps.ToBytes(ref ptr, (ushort)array.Length);
                byte* objectListStart = ptr;
                for (int i = 0; i < array.Length; i++)
                {
                    int size;
                    if ((size = itemSerializer.Serialize(array[i], ptr + OrxConstants.UInt16Sz, msgStart, endPtr)) == 0)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, (ushort)size);
                    ptr += size;
                }
                StreamByteOps.ToBytes(ref objectsListSize, (ushort)(ptr - objectListStart));

                return true;
            }
        }

        private sealed class ObjectListSerializer<To> : Serializer<List<To>> where To : class
        {
            private readonly OrxByteSerializer<To> itemSerializer = new OrxByteSerializer<To>();

            public ObjectListSerializer(PropertyInfo property, ushort id)
                : base(property, id)
            {
            }

            public override unsafe bool Serialize(Tm message, ref byte* ptr, byte* msgStart, byte* endPtr)
            {
                List<To> array = Get(message);

                if (ptr + (2 + array.Count) * OrxConstants.UInt16Sz > endPtr)
                {
                    return false;
                }
                StreamByteOps.ToBytes(ref ptr, Id);
                byte* objectsListSize = ptr;
                ptr += OrxConstants.UInt16Sz;
                StreamByteOps.ToBytes(ref ptr, (ushort)array.Count);
                byte* objectListStart = ptr;
                for (int i = 0; i < array.Count; i++)
                {
                    int size;
                    if ((size = itemSerializer.Serialize(array[i], ptr + OrxConstants.UInt16Sz, msgStart, endPtr)) == 0)
                    {
                        return false;
                    }
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
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt8Sz);
                }
                if (ptr + OrxConstants.UInt8Sz <= endPtr)
                {
                    *(ptr++) = Get(message) ? (byte)1 : (byte)0;
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
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, OrxConstants.UInt8Sz);
                }
                if (ptr + OrxConstants.UInt8Sz <= endPtr)
                {
                    *(ptr++) = Get(message);
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
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
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
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
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
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
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
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
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
                string str = Get(message);
                if (str == null) return true;
                byte* sizeOfArray = ptr;
                if (Id != ushort.MaxValue)
                {
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    sizeOfArray = ptr;
                    ptr += 2;
                }
                if (ptr + str.Length * OrxConstants.UInt8Sz + 1 <= endPtr)
                {
                    StreamByteOps.ToBytesWithSizeHeader(ref ptr, str, ushort.MaxValue);
                    if (Id != ushort.MaxValue)
                    {
                        StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray + OrxConstants.UInt16Sz));
                    }
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
                MutableString str = Get(message);
                if (str == null) return true;
                byte* sizeOfArray = ptr;
                if (Id != ushort.MaxValue)
                {
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    sizeOfArray = ptr;
                    ptr += 2;
                }
                if (ptr + str.Length * OrxConstants.UInt8Sz + 1 <= endPtr)
                {
                    StreamByteOps.ToBytesWithSizeHeader(ref ptr, str, ushort.MaxValue);
                    if (Id != ushort.MaxValue)
                    {
                        StreamByteOps.ToBytes(ref sizeOfArray, (ushort)(ptr - sizeOfArray + OrxConstants.UInt16Sz));
                    }
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
                    if (ptr + 2 * OrxConstants.UInt16Sz > endPtr)
                    {
                        return false;
                    }
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (ushort)(OrxConstants.UInt8Sz + OrxConstants.UInt32Sz));
                }
                if (ptr + OrxConstants.UInt8Sz + OrxConstants.UInt32Sz <= endPtr)
                {
                    decimal value = Get(message);
                    byte factor = OrxScaling.GetScalingFactor(value);
                    *(ptr++) = factor;
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
                    if (ptr + OrxConstants.UInt16Sz + 2 * OrxConstants.UInt64Sz > endPtr)
                    {
                        return false;
                    }
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
                if (dateTime == DateTimeConstants.UnixEpoch && IsOptional)
                {
                    return true;
                }
                if (IsOptional)
                {
                    if (ptr + OrxConstants.UInt16Sz + 2 * OrxConstants.UInt64Sz > endPtr)
                    {
                        return false;
                    }
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
}
