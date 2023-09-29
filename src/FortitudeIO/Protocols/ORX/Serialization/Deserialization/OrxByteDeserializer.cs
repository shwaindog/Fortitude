using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.Logging;

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization
{
    public class OrxByteDeserializer<Tm> : IOrxDeserializer where Tm : class
    {
        private readonly IDeserializer[] mandatory;
        private readonly Dictionary<ushort, IDeserializer> optional = new Dictionary<ushort, IDeserializer>();
        private IOrxDeserializerLookup orxDeserializerLookup;
        private byte thisVersion;

        public IOrxDeserializerLookup OrxDeserializerLookup
        {
            get => orxDeserializerLookup;
            set => orxDeserializerLookup = value;
        }

        public OrxByteDeserializer(IOrxDeserializerLookup orxDeserializerLookup, 
            byte messageVersion = 0)
        {
            OrxDeserializerLookup = orxDeserializerLookup;
            var currentType = typeof(Tm);
            SetCorrectMessageVersion(messageVersion, currentType);
            OrxDeserializerLookup.SetDeserializerForVersion(currentType, this, thisVersion, thisVersion);
            LoadPreviousVersionHandlers(orxDeserializerLookup, currentType);
            SortedList<ushort, PropertyInfo> props = OrxMandatoryField.FindAll(currentType);
            mandatory = new IDeserializer[props.Count];
            for (int i = 0; i < props.Count; i++)
            {
                PropertyInfo pi = props.Values[i];
                if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                {
                    ManditoryBasicTypes(thisVersion, pi, i);
                }
                else
                {
                    ManditoryArrayTypes(thisVersion, pi, i);
                }
            }

            foreach (var kv in OrxOptionalField.FindAll(currentType))
            {
                ushort id = kv.Key;
                PropertyInfo pi = kv.Value;
                if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                {
                    OptionalBasicTypes(thisVersion, pi, id);
                }
                else
                {
                    OptionalArrayTypes(thisVersion, pi, id);
                }
            }
        }

        private void SetCorrectMessageVersion(byte messageVersion, Type currentType)
        {
            if (currentType.IsSubclassOf(typeof(IVersionedMessage)) &&
                messageVersion == 0)
            {
                var instanceOfType =
                    (IVersionedMessage) OrxDeserializerLookup.OrxRecyclingFactory.Borrow<Tm>();
                messageVersion = instanceOfType.Version;
            }
            thisVersion = messageVersion;
        }

        private void LoadPreviousVersionHandlers(IOrxDeserializerLookup deserializerLookup, Type currentType)
        {
            var hasPreviousVersion = (OrxMandatoryField) currentType
                .GetCustomAttributes(typeof(OrxHasPreviousVersions)).FirstOrDefault();
            if (hasPreviousVersion != null)
            {
                var oldVersionsOfType =
                    from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    where type.IsSubclassOf(typeof(IOlderVersionMessagePart<>).MakeGenericType(type))
                    from previousVersionAttrib in type.GetCustomAttributes(typeof(OrxIsPreviousVersion))
                        .Cast<OrxIsPreviousVersion>()
                    where previousVersionAttrib.TargetType == currentType
                    select type;
                foreach (var oldVersion in oldVersionsOfType)
                {
                    var previousVersionDetails = (OrxIsPreviousVersion) oldVersion
                        .GetCustomAttributes(typeof(OrxIsPreviousVersion)).FirstOrDefault();
                    deserializerLookup.SetDeserializerForVersion(typeof(Tm), Activator.CreateInstance(
                            typeof(OrxByteDeserializer<>).MakeGenericType(oldVersion),
                            OrxDeserializerLookup, previousVersionDetails.ToVersion) as IOrxDeserializer,
                        previousVersionDetails.FromVersion, previousVersionDetails.ToVersion);
                }
            }
        }

        public unsafe object Deserialize(DispatchContext dispatchContext)
        {
            dispatchContext.DispatchLatencyLogger.Add(SocketDataLatencyLogger.EnterDeserializer);
            fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
            {
                return Deserialize(fptr + dispatchContext.EncodedBuffer.ReadCursor, 
                    dispatchContext.MessageSize, dispatchContext.MessageVersion);
            }
        }

        public unsafe object Deserialize(byte* ptr, int length, byte messageVersion)
        {
            if (messageVersion >= thisVersion)
            {
                return DeserializeCurrentType(ptr, length);
            }
            else
            {
                var deserializerForVersion = OrxDeserializerLookup.GetDeserializerForVersion(typeof(Tm), messageVersion);
                var messagePart = deserializerForVersion.Deserialize(ptr, length, messageVersion) is 
                        IOlderVersionMessagePart<Tm> olderObjectVersion 
                    ? olderObjectVersion.ToLatestVersion() 
                    : DeserializeCurrentType(ptr, length);

                return messagePart;
            }
        }

        private unsafe Tm DeserializeCurrentType(byte* ptr, int length)
        {
            Tm messagePart = orxDeserializerLookup.OrxRecyclingFactory.Borrow<Tm>();
            byte* end = ptr + length;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < mandatory.Length; i++)
            {
                if (ptr >= end)
                {
                    break;
                }

                mandatory[i].Deserialize(messagePart, ref ptr);
            }

            if (optional.Count > 0)
            {
                while (ptr < end)
                {
                    ushort id = StreamByteOps.ToUShort(ref ptr);
                    ushort size = StreamByteOps.ToUShort(ref ptr);
                    if (optional.TryGetValue(id, out var deserializer))
                    {
                        deserializer.Deserialize(messagePart, ref ptr);
                    }
                    else
                    {
                        ptr += size;
                    }
                }
            }

            return messagePart;
        }

        private void OptionalArrayTypes(byte messageVersion, PropertyInfo pi, ushort id)
        {
            if (pi.PropertyType == typeof(bool[]))
            {
                optional.Add(id, new BoolArrayDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(List<bool>))
            {
                optional.Add(id, new BoolListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(byte[]))
            {
                optional.Add(id, new ByteArrayDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(List<byte>))
            {
                optional.Add(id, new ByteListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(short[]))
            {
                optional.Add(id, new ShortArrayDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(List<short>))
            {
                optional.Add(id, new ShortListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(ushort[]))
            {
                optional.Add(id, new UShortArrayDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(List<ushort>))
            {
                optional.Add(id, new UShortListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(int[]))
            {
                optional.Add(id, new IntArrayDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(List<int>))
            {
                optional.Add(id, new IntListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(uint[]))
            {
                optional.Add(id, new UIntArrayDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(List<uint>))
            {
                optional.Add(id, new UIntListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(long[]))
            {
                optional.Add(id, new LongArrayDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(List<long>))
            {
                optional.Add(id, new LongListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(decimal[]))
            {
                optional.Add(id, new DecimalArrayDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(List<decimal>))
            {
                optional.Add(id, new DecimalListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(string[]))
            {
                optional.Add(id, new StringArrayDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(List<string>))
            {
                optional.Add(id, new StringListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(MutableString[]))
            {
                optional.Add(id, new MutableStringArrayDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(List<MutableString>))
            {
                optional.Add(id, new MutableStringListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                     pi.PropertyType.GenericTypeArguments[0].IsClass)
            {
                optional.Add(id, Activator.CreateInstance(typeof(OptionalObjectListDeserializer<>)
                        .MakeGenericType(typeof(Tm), pi.PropertyType.GenericTypeArguments[0]), pi,
                    OrxDeserializerLookup, messageVersion) as IDeserializer);
            }
            else if ((pi.PropertyType.GetElementType()?.IsClass ?? false)
                     && pi.PropertyType.GetArrayRank() == 1)
            {
                optional.Add(id, Activator.CreateInstance(typeof(OptionalObjectArrayDeserializer<>)
                        .MakeGenericType(typeof(Tm), pi.PropertyType.GetElementType()), pi,
                    OrxDeserializerLookup, messageVersion) as IDeserializer);
            }
            else
            {
                throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
            }
        }

        private void OptionalBasicTypes(byte messageVersion, PropertyInfo pi, ushort id)
        {
            if (pi.PropertyType == typeof(bool))
            {
                optional.Add(id, new BoolDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(byte) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(byte))
            {
                optional.Add(id, new UInt8Deserializer(pi));
            }
            else if (pi.PropertyType == typeof(short) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(short))
            {
                optional.Add(id, new Int16Deserializer(pi));
            }
            else if (pi.PropertyType == typeof(ushort) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(ushort))
            {
                optional.Add(id, new UInt16Deserializer(pi));
            }
            else if (pi.PropertyType == typeof(int) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(int))
            {
                optional.Add(id, new Int32Deserializer(pi));
            }
            else if (pi.PropertyType == typeof(uint) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(uint))
            {
                optional.Add(id, new UInt32Deserializer(pi));
            }
            else if (pi.PropertyType == typeof(decimal))
            {
                optional.Add(id, new DecimalDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(long))
            {
                optional.Add(id, new LongDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(DateTime))
            {
                optional.Add(id, new DateTimeDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(string))
            {
                optional.Add(id, new StringDeserializer(pi));
            }
            else if (pi.PropertyType == typeof(MutableString))
            {
                optional.Add(id, new MutableStringDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory));
            }
            else if (pi.PropertyType == typeof(Dictionary<string, string>))
            {
                optional.Add(id, new MapDeserializer(pi));
            }
            else if (pi.PropertyType.IsClass && (OrxMandatoryField.FindAll(pi.PropertyType).Any()
                                                 || OrxOptionalField.FindAll(pi.PropertyType).Any()))
            {
                var customAttributes = (OrxOptionalField)pi
                    .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                optional.Add(id, Activator.CreateInstance(typeof(OptionalObjectDeserializer<>)
                        .MakeGenericType(typeof(Tm), pi.PropertyType), pi, customAttributes?.Mapping,
                    OrxDeserializerLookup, messageVersion) as IDeserializer);
            }
            else
            {
                throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
            }
        }

        private void ManditoryArrayTypes(byte messageVersion, PropertyInfo pi, int i)
        {
            if (pi.PropertyType == typeof(bool[]))
            {
                mandatory[i] = new BoolArrayDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(List<bool>))
            {
                mandatory[i] = new BoolListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType == typeof(byte[]))
            {
                mandatory[i] = new ByteArrayDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(List<byte>))
            {
                mandatory[i] = new ByteListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType == typeof(short[]))
            {
                mandatory[i] = new ShortArrayDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(List<short>))
            {
                mandatory[i] = new ShortListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType == typeof(ushort[]))
            {
                mandatory[i] = new UShortArrayDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(List<ushort>))
            {
                mandatory[i] = new UShortListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType == typeof(int[]))
            {
                mandatory[i] = new IntArrayDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(List<int>))
            {
                mandatory[i] = new IntListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if(pi.PropertyType == typeof(uint[]))
            {
                mandatory[i] = new UIntArrayDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(List<uint>))
            {
                mandatory[i] = new UIntListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType == typeof(long[]))
            {
                mandatory[i] = new LongArrayDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(List<long>))
            {
                mandatory[i] = new LongListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType == typeof(decimal[]))
            {
                mandatory[i] = new DecimalArrayDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(List<decimal>))
            {
                mandatory[i] = new DecimalListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType == typeof(string[]))
            {
                mandatory[i] = new StringArrayDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(List<string>))
            {
                mandatory[i] = new StringListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType == typeof(MutableString[]))
            {
                mandatory[i] = new MutableStringArrayDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType == typeof(List<MutableString>))
            {
                mandatory[i] = new MutableStringListDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) && 
                     pi.PropertyType.GenericTypeArguments[0].IsClass)
            {
                mandatory[i] = Activator.CreateInstance(typeof(MandatoryObjectListDeserializer<>)
                        .MakeGenericType(typeof(Tm), pi.PropertyType.GenericTypeArguments[0]), pi,
                    OrxDeserializerLookup, messageVersion) as IDeserializer;
            }
            else if ((pi.PropertyType.GetElementType()?.IsClass ?? false) && pi.PropertyType.GetArrayRank() == 1)
            {
                mandatory[i] = Activator.CreateInstance(typeof(MandatoryObjectArrayDeserializer<>)
                        .MakeGenericType(typeof(Tm), pi.PropertyType.GetElementType()), pi,
                    OrxDeserializerLookup, messageVersion) as IDeserializer;
            }
            else
            {
                throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
            }
        }

        private void ManditoryBasicTypes(byte messageVersion, PropertyInfo pi, int i)
        {
            if (pi.PropertyType == typeof(bool))
            {
                mandatory[i] = new BoolDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(byte) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(byte))
            {
                mandatory[i] = new UInt8Deserializer(pi);
            }
            else if (pi.PropertyType == typeof(short) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(short))
            {
                mandatory[i] = new Int16Deserializer(pi);
            }
            else if (pi.PropertyType == typeof(ushort) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(ushort))
            {
                mandatory[i] = new UInt16Deserializer(pi);
            }
            else if (pi.PropertyType == typeof(int) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(int))
            {
                mandatory[i] = new Int32Deserializer(pi);
            }
            else if (pi.PropertyType == typeof(uint) ||
                     pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(uint))
            {
                mandatory[i] = new UInt32Deserializer(pi);
            }
            else if (pi.PropertyType == typeof(decimal))
            {
                mandatory[i] = new DecimalDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(long))
            {
                mandatory[i] = new LongDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(DateTime))
            {
                mandatory[i] = new DateTimeDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(string))
            {
                mandatory[i] = new StringDeserializer(pi);
            }
            else if (pi.PropertyType == typeof(MutableString))
            {
                mandatory[i] = new MutableStringDeserializer(pi, orxDeserializerLookup.OrxRecyclingFactory);
            }
            else if (pi.PropertyType.IsClass && (OrxMandatoryField.FindAll(pi.PropertyType).Any()
                                                 || OrxOptionalField.FindAll(pi.PropertyType).Any()))
            {
                var customAttributes = (OrxMandatoryField)pi
                    .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                mandatory[i] = Activator.CreateInstance(typeof(ManditoryObjectDeserializer<>)
                        .MakeGenericType(typeof(Tm), pi.PropertyType), pi, customAttributes?.Mapping,
                    OrxDeserializerLookup, messageVersion) as IDeserializer;
            }
            else
            {
                throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
            }
        }

        private interface IDeserializer
        {
            unsafe void Deserialize(Tm message, ref byte* bufferPtr);
        }

        private abstract class Deserializer<Tp> : IDeserializer
        {
            protected Deserializer(PropertyInfo property)
            {
                Set = (Action<Tm, Tp>)Delegate.CreateDelegate(typeof(Action<Tm, Tp>), property.GetSetMethod(true));
            }

            public abstract unsafe void Deserialize(Tm message, ref byte* buffer);

            protected readonly Action<Tm, Tp> Set;
        }

        private sealed class ManditoryObjectDeserializer<T> : Deserializer<T> where T : class
        {
            private readonly byte messageVersion;
            private readonly IOrxDeserializer itemSerializer;
            private readonly Dictionary<ushort, IOrxDeserializer> deserializerLookup;
            
            // ReSharper disable once UnusedMember.Local
            public ManditoryObjectDeserializer(PropertyInfo property, Dictionary<ushort, Type> mapping,
                IOrxDeserializerLookup orxDeserializerLookup, byte version) : base(property)
            {
                itemSerializer = orxDeserializerLookup.GetOrCreateDeserializerForVersion(typeof(T), version);
                messageVersion = version;
                if (mapping != null)
                {
                    deserializerLookup = new Dictionary<ushort, IOrxDeserializer>();
                    foreach (var keyValuePair in mapping)
                    {
                        var mappedSerializer = orxDeserializerLookup.GetOrCreateDeserializerForVersion(
                            keyValuePair.Value, version);
                        deserializerLookup.Add(keyValuePair.Key, mappedSerializer);
                    }
                }
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                // ReSharper disable once UnusedVariable
                int ignoredId = StreamByteOps.ToUShort(ref ptr);
                int size = StreamByteOps.ToUShort(ref ptr);
                if (size == 0)
                {
                    Set(message, null);
                    return;
                }

                T typedProp;
                if (deserializerLookup != null && deserializerLookup.Count > 0)
                {
                    ushort typeId = StreamByteOps.ToUShort(ref ptr);
                    var mappedDeserializer = deserializerLookup[typeId];
                    typedProp = mappedDeserializer.Deserialize(ptr, size, messageVersion) as T;
                }
                else
                {
                    typedProp = itemSerializer.Deserialize(ptr, size, messageVersion) as T;
                }
                Set(message, typedProp);
                ptr += size;
            }
        }

        private sealed class OptionalObjectDeserializer<T> : Deserializer<T> where T : class
        {
            private readonly byte messageVersion;
            private readonly IOrxDeserializer itemSerializer;
            private readonly Dictionary<ushort, IOrxDeserializer> deserializerLookup;
            
            // ReSharper disable once UnusedMember.Local
            public OptionalObjectDeserializer(PropertyInfo property, Dictionary<ushort, Type> mapping,
                IOrxDeserializerLookup orxDeserializerLookup, byte version) : base(property)
            {
                itemSerializer = orxDeserializerLookup.GetOrCreateDeserializerForVersion(typeof(T), version);
                messageVersion = version;
                if (mapping != null)
                {
                    deserializerLookup = new Dictionary<ushort, IOrxDeserializer>();
                    foreach (var keyValuePair in mapping)
                    {
                        var mappedSerializer = orxDeserializerLookup.GetOrCreateDeserializerForVersion(
                            keyValuePair.Value, version);
                        deserializerLookup.Add(keyValuePair.Key, mappedSerializer);
                    }
                }
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                ptr -= OrxConstants.UInt16Sz;
                int size = StreamByteOps.ToUShort(ref ptr);
                if (size == 0)
                {
                    Set(message, null);
                    return;
                }

                T typedProp;
                if (deserializerLookup != null && deserializerLookup.Count > 0)
                {
                    ushort typeId = StreamByteOps.ToUShort(ref ptr);
                    var mappedDeserializer = deserializerLookup[typeId];
                    typedProp = mappedDeserializer.Deserialize(ptr, size, messageVersion) as T;
                }
                else
                {
                    typedProp = itemSerializer.Deserialize(ptr, size, messageVersion) as T;
                }
                Set(message, typedProp);
                ptr += size;
            }
        }

        private sealed class BoolArrayDeserializer : Deserializer<bool[]>
        {
            public BoolArrayDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                bool[] array = new bool[StreamByteOps.ToUShort(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = *ptr > 0;
                    ptr++;
                }
                Set(message, array);
            }
        }

        private sealed class BoolListDeserializer : Deserializer<List<bool>>
        {
            private readonly IRecycler orxRecyclingFactory;

            public BoolListDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int size = StreamByteOps.ToUShort(ref ptr);
                List<bool> boolList = orxRecyclingFactory.Borrow<List<bool>>();
                boolList.Clear();
                for (int i = 0; i < size; i++)
                {
                    boolList.Add(*ptr > 0);
                    ptr++;
                }
                Set(message, boolList);
            }
        }

        private sealed class ByteArrayDeserializer : Deserializer<byte[]>
        {
            public ByteArrayDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                byte[] array = new byte[StreamByteOps.ToUShort(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = *ptr;
                    ptr++;
                }
                Set(message, array);
            }
        }

        private sealed class ByteListDeserializer : Deserializer<List<byte>>
        {
            private readonly IRecycler orxRecyclingFactory;

            public ByteListDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int size = StreamByteOps.ToUShort(ref ptr);
                List<byte> byteList = orxRecyclingFactory.Borrow<List<byte>>();
                byteList.Clear();
                for (int i = 0; i < size; i++)
                {
                    byteList.Add(*ptr);
                    ptr++;
                }
                Set(message, byteList);
            }
        }

        private sealed class ShortArrayDeserializer : Deserializer<short[]>
        {
            public ShortArrayDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                short[] array = new short[StreamByteOps.ToUShort(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = StreamByteOps.ToShort(ref ptr);
                }
                Set(message, array);
            }
        }

        private sealed class ShortListDeserializer : Deserializer<List<short>>
        {
            private readonly IRecycler orxRecyclingFactory;

            public ShortListDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int size = StreamByteOps.ToUShort(ref ptr);
                List<short> shortList = orxRecyclingFactory.Borrow<List<short>>();
                shortList.Clear();
                for (int i = 0; i < size; i++)
                {
                    shortList.Add(StreamByteOps.ToShort(ref ptr));
                }
                Set(message, shortList);
            }
        }

        private sealed class UShortArrayDeserializer : Deserializer<ushort[]>
        {
            public UShortArrayDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                ushort[] array = new ushort[StreamByteOps.ToUShort(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = StreamByteOps.ToUShort(ref ptr);
                }
                Set(message, array);
            }
        }

        private sealed class UShortListDeserializer : Deserializer<List<ushort>>
        {
            private readonly IRecycler orxRecyclingFactory;

            public UShortListDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int size = StreamByteOps.ToUShort(ref ptr);
                List<ushort> ushortList = orxRecyclingFactory.Borrow<List<ushort>>();
                ushortList.Clear();
                for (int i = 0; i < size; i++)
                {
                    ushortList.Add(StreamByteOps.ToUShort(ref ptr));
                }
                Set(message, ushortList);
            }
        }

        private sealed class IntArrayDeserializer : Deserializer<int[]>
        {
            public IntArrayDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int[] array = new int[StreamByteOps.ToUShort(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = StreamByteOps.ToInt(ref ptr);
                }
                Set(message, array);
            }
        }

        private sealed class IntListDeserializer : Deserializer<List<int>>
        {
            private readonly IRecycler orxRecyclingFactory;

            public IntListDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int size = StreamByteOps.ToUShort(ref ptr);
                List<int> intList = orxRecyclingFactory.Borrow<List<int>>();
                intList.Clear();
                for (int i = 0; i < size; i++)
                {
                    intList.Add(StreamByteOps.ToInt(ref ptr));
                }
                Set(message, intList);
            }
        }

        private sealed class UIntArrayDeserializer : Deserializer<uint[]>
        {
            public UIntArrayDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                uint[] array = new uint[StreamByteOps.ToUInt(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = StreamByteOps.ToUInt(ref ptr);
                }
                Set(message, array);
            }
        }

        private sealed class UIntListDeserializer : Deserializer<List<uint>>
        {
            private readonly IRecycler orxRecyclingFactory;

            public UIntListDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int size = StreamByteOps.ToUShort(ref ptr);
                List<uint> uintList = orxRecyclingFactory.Borrow<List<uint>>();
                uintList.Clear();
                for (int i = 0; i < size; i++)
                {
                    uintList.Add(StreamByteOps.ToUInt(ref ptr));
                }
                Set(message, uintList);
            }
        }

        private sealed class LongArrayDeserializer : Deserializer<long[]>
        {
            public LongArrayDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                long[] array = new long[StreamByteOps.ToUInt(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = StreamByteOps.ToLong(ref ptr);
                }
                Set(message, array);
            }
        }

        private sealed class LongListDeserializer : Deserializer<List<long>>
        {
            private readonly IRecycler orxRecyclingFactory;

            public LongListDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int size = StreamByteOps.ToUShort(ref ptr);
                List<long> longList = orxRecyclingFactory.Borrow<List<long>>();
                longList.Clear();
                for (int i = 0; i < size; i++)
                {
                    longList.Add(StreamByteOps.ToLong(ref ptr));
                }
                Set(message, longList);
            }
        }

        private sealed class DecimalArrayDeserializer : Deserializer<decimal[]>
        {
            public DecimalArrayDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                decimal[] array = new decimal[StreamByteOps.ToUInt(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    var numberOfDecimalPlaces = *ptr++;
                    var deDecimalizedLong = StreamByteOps.ToLong(ref ptr);
                    array[i] = (decimal)Math.Pow(10, -numberOfDecimalPlaces) * deDecimalizedLong;
                }
                Set(message, array);
            }
        }

        private sealed class DecimalListDeserializer : Deserializer<List<decimal>>
        {
            private readonly IRecycler orxRecyclingFactory;

            public DecimalListDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int size = StreamByteOps.ToUShort(ref ptr);
                List<decimal> decimalList = orxRecyclingFactory.Borrow<List<decimal>>();
                decimalList.Clear();
                for (int i = 0; i < size; i++)
                {
                    var numberOfDecimalPlaces = *ptr++;
                    var deDecimalizedLong = StreamByteOps.ToLong(ref ptr);
                    decimalList.Add((decimal)Math.Pow(10, -numberOfDecimalPlaces) * deDecimalizedLong);
                }
                Set(message, decimalList);
            }
        }

        private sealed class StringArrayDeserializer : Deserializer<string[]>
        {
            public StringArrayDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                string[] array = new string[StreamByteOps.ToUShort(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = StreamByteOps.ToStringWithSizeHeader(ref ptr);
                }
                Set(message, array);
            }
        }

        private sealed class StringListDeserializer : Deserializer<List<string>>
        {
            private readonly IRecycler orxRecyclingFactory;

            public StringListDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int size = StreamByteOps.ToUShort(ref ptr);
                List<string> stringList = orxRecyclingFactory.Borrow<List<string>>();
                for (int i = 0; i < size; i++)
                {
                    stringList.Add(StreamByteOps.ToStringWithSizeHeader(ref ptr));
                }
                Set(message, stringList);
            }
        }

        private sealed class MutableStringArrayDeserializer : Deserializer<MutableString[]>
        {
            private readonly IRecycler recyclingFactory;

            public MutableStringArrayDeserializer(PropertyInfo property, IRecycler recyclingFactory)
                : base(property)
            {
                this.recyclingFactory = recyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                MutableString[] array = new MutableString[StreamByteOps.ToUShort(ref ptr)];
                for (int i = 0; i < array.Length; i++)
                {
                    var mutableString = recyclingFactory.Borrow<MutableString>();
                    array[i] = StreamByteOps.ToMutableStringWithSizeHeader(ref ptr, mutableString);
                }
                Set(message, array);
            }
        }

        private sealed class MutableStringListDeserializer : Deserializer<List<MutableString>>
        {
            private readonly IRecycler recyclingFactory;

            public MutableStringListDeserializer(PropertyInfo property, IRecycler recyclingFactory)
                : base(property)
            {
                this.recyclingFactory = recyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                var numberOfElements = StreamByteOps.ToUShort(ref ptr);
                List<MutableString> mutableStringList = recyclingFactory.Borrow<List<MutableString>>();
                mutableStringList.Clear();
                for (int i = 0; i < numberOfElements; i++)
                {
                    var mutableString = recyclingFactory.Borrow<MutableString>();
                    mutableStringList.Add(StreamByteOps.ToMutableStringWithSizeHeader(ref ptr, mutableString));
                }
                Set(message, mutableStringList);
            }
        }

        private sealed class MapDeserializer : Deserializer<Dictionary<string, string>>
        {
            public MapDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                uint count = StreamByteOps.ToUInt(ref ptr);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                for (int i = 0; i < count; i++)
                {
                    string key = StreamByteOps.ToStringWithSizeHeader(ref ptr);
                    string value = StreamByteOps.ToStringWithSizeHeader(ref ptr);
                    dic[key] = value;
                }
                Set(message, dic);
            }
        }

        private class OptionalObjectArrayDeserializer<To> : Deserializer<To[]> where To : class, new()
        {
            private readonly byte version;

            // ReSharper disable once MemberCanBeProtected.Local
            public OptionalObjectArrayDeserializer(PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup, byte version)
                : base(property)
            {
                this.version = version;
                get = size => (To[])Activator.CreateInstance(typeof(To[]), (int)size );
                itemDeserializer = new OrxByteDeserializer<To>(orxDeserializerLookup, version);
            }

            private readonly Func<ushort, To[]> get;
            private readonly OrxByteDeserializer<To> itemDeserializer;

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                ushort arraySize = StreamByteOps.ToUShort(ref ptr);
                var array = get(arraySize);
                for (int i = 0; i < array.Length; i++)
                {
                    int size = StreamByteOps.ToUShort(ref ptr);
                    array[i] = itemDeserializer.Deserialize(ptr, size, version) as To;
                    ptr += size;
                }
                Set(message, array);
            }
        }

        private class OptionalObjectListDeserializer<To> : Deserializer<List<To>> where To : class, new()
        {
            private readonly byte version;

            // ReSharper disable once MemberCanBeProtected.Local
            public OptionalObjectListDeserializer(PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup, byte version)
                : base(property)
            {
                this.version = version;
                itemDeserializer = new OrxByteDeserializer<To>(orxDeserializerLookup, version);
                orxRecyclingFactory = orxDeserializerLookup.OrxRecyclingFactory;
            }
            
            private readonly OrxByteDeserializer<To> itemDeserializer;
            private readonly IRecycler orxRecyclingFactory;

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                ushort arraySize = StreamByteOps.ToUShort(ref ptr);
                var objectList = orxRecyclingFactory.Borrow<List<To>>();
                objectList.Clear();
                for (int i = 0; i < arraySize; i++)
                {
                    int size = StreamByteOps.ToUShort(ref ptr);
                    objectList.Add(itemDeserializer.Deserialize(ptr, size, version) as To);
                    ptr += size;
                }
                Set(message, objectList);
            }
        }

        private class MandatoryObjectArrayDeserializer<To> : OptionalObjectArrayDeserializer<To> 
            where To : class, new()
        {
            public MandatoryObjectArrayDeserializer(PropertyInfo property, 
                IOrxDeserializerLookup orxDeserializerLookup, byte version) 
                : base(property, orxDeserializerLookup, version)
            {
            }

            [SuppressMessage("ReSharper", "UnusedVariable")]
            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int ignoredArrayId = StreamByteOps.ToUShort(ref ptr);
                int ignoredEntrySize = StreamByteOps.ToUShort(ref ptr);
                base.Deserialize(message, ref ptr);
            }
        }

        private class MandatoryObjectListDeserializer<To> : OptionalObjectListDeserializer<To> 
            where To : class, new()
        {
            public MandatoryObjectListDeserializer(PropertyInfo property, 
                IOrxDeserializerLookup orxDeserializerLookup, byte version) 
                : base(property, orxDeserializerLookup, version)
            {
            }

            [SuppressMessage("ReSharper", "UnusedVariable")]
            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                int ignoredArrayId = StreamByteOps.ToUShort(ref ptr);
                int ignoredEntrySize = StreamByteOps.ToUShort(ref ptr);
                base.Deserialize(message, ref ptr);
            }
        }

        private sealed class BoolDeserializer : Deserializer<bool>
        {
            public BoolDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                Set(message, *(ptr++) != 0);
            }
        }

        private sealed class UInt8Deserializer : Deserializer<byte>
        {
            public UInt8Deserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                Set(message, *(ptr++));
            }
        }

        private sealed class UInt16Deserializer : Deserializer<ushort>
        {
            public UInt16Deserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                Set(message, StreamByteOps.ToUShort(ref ptr));
            }
        }

        private sealed class Int16Deserializer : Deserializer<short>
        {
            public Int16Deserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                Set(message, StreamByteOps.ToShort(ref ptr));
            }
        }

        private sealed class UInt32Deserializer : Deserializer<uint>
        {
            public UInt32Deserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                Set(message, StreamByteOps.ToUInt(ref ptr));
            }
        }

        private sealed class Int32Deserializer : Deserializer<int>
        {
            public Int32Deserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                Set(message, StreamByteOps.ToInt(ref ptr));
            }
        }

        private sealed class StringDeserializer : Deserializer<string>
        {
            public StringDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                Set(message, StreamByteOps.ToStringWithSizeHeader(ref ptr));
            }
        }

        private sealed class MutableStringDeserializer : Deserializer<MutableString>
        {
            private readonly IRecycler orxRecyclingFactory;

            public MutableStringDeserializer(PropertyInfo property, IRecycler orxRecyclingFactory)
                : base(property)
            {
                this.orxRecyclingFactory = orxRecyclingFactory;
            }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                Set(message, StreamByteOps.ToMutableStringWithSizeHeader(ref ptr, orxRecyclingFactory.Borrow<MutableString>()));
            }
        }

        private sealed class DecimalDeserializer : Deserializer<decimal>
        {
            public DecimalDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                byte factor = *(ptr++);
                Set(message, OrxScaling.Unscale(StreamByteOps.ToUInt(ref ptr), factor));
            }
        }

        private sealed class LongDeserializer : Deserializer<long>
        {
            public LongDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                Set(message, StreamByteOps.ToLong(ref ptr));
            }
        }

        private sealed class DateTimeDeserializer : Deserializer<DateTime>
        {
            public DateTimeDeserializer(PropertyInfo property)
                : base(property) { }

            public override unsafe void Deserialize(Tm message, ref byte* ptr)
            {
                var nanosSinceUnixEpoch = StreamByteOps.ToLong(ref ptr);
                var dateTime = new DateTime(DateTimeConstants.UnixEpochTicks + nanosSinceUnixEpoch / 100,
                    DateTimeKind.Utc);
                Set(message, dateTime);
            }
        }
    }
}
