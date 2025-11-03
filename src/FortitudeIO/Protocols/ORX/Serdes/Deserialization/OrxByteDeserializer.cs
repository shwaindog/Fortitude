// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Logging;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public class OrxByteDeserializer<TMsg> : IOrxDeserializer where TMsg : class, new()
{
    private static int lastInstanceNum;

    private IDeserializer[] mandatory;

    private Dictionary<ushort, IDeserializer> optional = new();

    private byte thisVersion;

    public OrxByteDeserializer(IOrxDeserializerLookup orxDeserializerLookup, byte version = 0)
    {
        OrxDeserializerLookup = orxDeserializerLookup;
        var currentType = typeof(TMsg);
        SetCorrectMessageVersion(version, currentType);
        OrxDeserializerLookup.SetDeserializerForVersion(currentType, this, thisVersion, thisVersion);
        LoadPreviousVersionHandlers(orxDeserializerLookup, currentType);
        var props = OrxMandatoryField.FindAll(currentType);
        mandatory = new IDeserializer[props.Count];
        for (var i = 0; i < props.Count; i++)
        {
            var pi = props.Values[i];
            if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                MandatoryBasicTypes(thisVersion, pi, i);
            else
                MandatoryArrayTypes(thisVersion, pi, i);
        }

        foreach (var kv in OrxOptionalField.FindAll(currentType))
        {
            var id = kv.Key;
            var pi = kv.Value;
            if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                OptionalBasicTypes(thisVersion, pi, id);
            else
                OptionalArrayTypes(thisVersion, pi, id);
        }
    }

    public OrxByteDeserializer(OrxByteDeserializer<TMsg> toClone)
    {
        mandatory   = [..toClone.mandatory];
        optional    = new Dictionary<ushort, IDeserializer>(toClone.optional);
        thisVersion = toClone.thisVersion;
        MessageId   = toClone.MessageId;

        OrxDeserializerLookup = toClone.OrxDeserializerLookup;
    }

    public uint MessageId { get; private set; }

    public IOrxDeserializerLookup OrxDeserializerLookup { get; set; }

    public int InstanceNumber { get; } = Interlocked.Increment(ref lastInstanceNum);


    public bool ReadMessageHeader { get; set; }

    public uint? RegisteredForMessageId { get; set; }

    public IMessageDeserializationRepository? RegisteredRepository { get; set; }

    public ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) => throw new NotImplementedException();

    public IMessageDeserializer CopyFrom(IMessageDeserializer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not OrxByteDeserializer<TMsg> orxByteDeserializer) return this;
        mandatory = [..orxByteDeserializer.mandatory];
        optional  = new Dictionary<ushort, IDeserializer>(orxByteDeserializer.optional);
        return this;
    }

    public Type MessageType => typeof(TMsg);

    public unsafe object Deserialize(byte* ptr, uint length, byte messageVersion)
    {
        if (messageVersion >= thisVersion) return DeserializeCurrentType(ptr, length);

        var deserializerForVersion = OrxDeserializerLookup.GetDeserializerForVersion(typeof(TMsg), messageVersion)!;
        var messagePart = deserializerForVersion.Deserialize(ptr, length, messageVersion) is
            IOlderVersionMessagePart<TMsg> olderObjectVersion
            ? olderObjectVersion.ToLatestVersion()
            : DeserializeCurrentType(ptr, length);

        return messagePart;
    }

    object IMessageDeserializer.Deserialize(IBufferContext bufferContext) => Deserialize(bufferContext);

    object ICloneable.Clone() => Clone();

    public IMessageDeserializer Clone() => new OrxByteDeserializer<TMsg>(this);

    protected unsafe MessageHeader ReadHeader(ref byte* ptr)
    {
        var version      = *ptr++;
        var messageFlags = *ptr++;
        var messageId    = StreamByteOps.ToUInt(ref ptr);
        var messageSize  = StreamByteOps.ToUInt(ref ptr);

        return new MessageHeader(version, messageFlags, messageId, messageSize);
    }

    public unsafe object Deserialize(IBuffer buffer, uint length, byte messageVersion)
    {
        using var fixBufferPtr = buffer;
        var       ptr          = fixBufferPtr.ReadBuffer + fixBufferPtr.BufferRelativeReadCursor;
        return Deserialize(ptr, length, messageVersion);
    }

    public unsafe TMsg Deserialize(ISerdeContext readContext)
    {
        var sockBuffContext = readContext as SocketBufferReadContext;
        sockBuffContext?.DispatchLatencyLogger?.Add(SocketDataLatencyLogger.EnterDeserializer);
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            if (ReadMessageHeader)
            {
                using var buffer       = messageBufferContext.EncodedBuffer;
                var       fixBufferPtr = buffer.ReadBuffer + buffer.BufferRelativeReadCursor;
                messageBufferContext.MessageHeader = ReadHeader(ref fixBufferPtr);
            }
            var messageVersion = messageBufferContext.MessageHeader.Version;
            var messageSize    = messageBufferContext.MessageHeader.MessageSize;
            return (TMsg)Deserialize(messageBufferContext.EncodedBuffer, messageSize - MessageHeader.SerializationSize, messageVersion);
        }

        throw new ArgumentException("Expected readContext to be of type IBufferContext");
    }

    private void SetCorrectMessageVersion(byte version, Type currentType)
    {
        if (currentType.IsSubclassOf(typeof(IVersionedMessage)) && version == 0)
        {
            var instanceOfType =
                (IVersionedMessage)OrxDeserializerLookup.Recycler.Borrow<TMsg>();
            version   = instanceOfType.Version;
            MessageId = instanceOfType.MessageId;
            instanceOfType.DecrementRefCount();
        }

        thisVersion = version;
    }

    private void LoadPreviousVersionHandlers(IOrxDeserializerLookup deserializerLookup, Type currentType)
    {
        var hasPreviousVersion = (OrxMandatoryField?)currentType
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
                var previousVersionDetails = (OrxIsPreviousVersion?)oldVersion
                                                                    .GetCustomAttributes(typeof(OrxIsPreviousVersion)).FirstOrDefault();
                if (previousVersionDetails != null)
                    deserializerLookup.SetDeserializerForVersion
                        (typeof(TMsg), (IOrxDeserializer)Activator.CreateInstance
                             (typeof(OrxByteDeserializer<>).MakeGenericType(oldVersion)
                            , OrxDeserializerLookup, previousVersionDetails.ToVersion)!
                       , previousVersionDetails.FromVersion, previousVersionDetails.ToVersion);
            }
        }
    }

    private unsafe TMsg DeserializeCurrentType(byte* ptr, uint length)
    {
        var messagePart = OrxDeserializerLookup.Recycler.Borrow<TMsg>();
        var end         = ptr + length;

        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < mandatory.Length; i++)
        {
            if (ptr >= end) break;

            mandatory[i].Deserialize(messagePart, ref ptr);
        }

        if (optional.Count > 0)
            while (ptr < end)
            {
                var id   = StreamByteOps.ToUShort(ref ptr);
                var size = StreamByteOps.ToUShort(ref ptr);
                if (optional.TryGetValue(id, out var deserializer))
                    deserializer.Deserialize(messagePart, ref ptr);
                else
                    ptr += size;
            }

        return messagePart;
    }

    private void OptionalArrayTypes(byte messageVersion, PropertyInfo pi, ushort id)
    {
        if (interrogatingTypes.Contains(pi.PropertyType)) return;
        var deserializer = ResolveArrayTypes(messageVersion, pi, id);
        optional.Add(id, deserializer);
    }

    private IDeserializer ResolveArrayTypes(byte messageVersion, PropertyInfo pi, ushort id)
    {
        IDeserializer? foundDeserializer =
            pi.PropertyType switch
            {
                var t when t.IsBoolArray()            => new BoolArrayDeserializer(pi)
              , var t when t.IsNullableBoolArray()    => new NullableBoolArrayDeserializer(pi)
              , var t when t.IsBoolList()             => new BoolListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableBoolList()     => new NullableBoolListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsByteArray()            => new ByteArrayDeserializer(pi)
              , var t when t.IsNullableByteArray()    => new NullableByteArrayDeserializer(pi)
              , var t when t.IsByteList()             => new ByteListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableByteList()     => new NullableByteListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsShortArray()           => new ShortArrayDeserializer(pi)
              , var t when t.IsNullableShortArray()   => new NullableShortArrayDeserializer(pi)
              , var t when t.IsShortList()            => new ShortListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableShortList()    => new NullableShortListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsUShortArray()          => new UShortArrayDeserializer(pi)
              , var t when t.IsNullableUShortArray()  => new NullableUShortArrayDeserializer(pi)
              , var t when t.IsUShortList()           => new UShortListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableUShortList()   => new NullableUShortListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsIntArray()             => new IntArrayDeserializer(pi)
              , var t when t.IsNullableIntArray()     => new NullableIntArrayDeserializer(pi)
              , var t when t.IsIntList()              => new IntListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableIntList()      => new NullableIntListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsUIntArray()            => new UIntArrayDeserializer(pi)
              , var t when t.IsNullableUIntArray()    => new NullableUIntArrayDeserializer(pi)
              , var t when t.IsUIntList()             => new UIntListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableUIntList()     => new NullableUIntListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsLongArray()            => new LongArrayDeserializer(pi)
              , var t when t.IsNullableLongArray()    => new NullableLongArrayDeserializer(pi)
              , var t when t.IsLongList()             => new LongListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableLongList()     => new NullableLongListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsULongArray()           => new ULongArrayDeserializer(pi)
              , var t when t.IsNullableULongArray()   => new NullableULongArrayDeserializer(pi)
              , var t when t.IsULongList()            => new ULongListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableULongList()    => new NullableULongListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsDecimalArray()         => new DecimalArrayDeserializer(pi)
              , var t when t.IsNullableDecimalArray() => new NullableDecimalArrayDeserializer(pi)
              , var t when t.IsDecimalList()          => new DecimalListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableDecimalList()  => new NullableDecimalListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsStringArray()          => new StringArrayDeserializer(pi)
              , var t when t.IsStringList()           => new StringListDeserializer(pi, OrxDeserializerLookup.Recycler)

              , var t when t.IsMutableStringArray()    => new MutableStringArrayDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsMutableStringList()     => new MutableStringListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsDateTimeArray()         => new DateTimeArrayDeserializer(pi)
              , var t when t.IsNullableDateTimeArray() => new NullableDateTimeArrayDeserializer(pi)
              , var t when t.IsDateTimeList()          => new DateTimeListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsNullableDateTimeList()  => new NullableDateTimeListDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsStringToStringMap()     => new MapDeserializer(pi)

              , _ => null
            };

        if (foundDeserializer == null)
        {
            if (id == ushort.MaxValue)
            {
                if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                    pi.PropertyType.GenericTypeArguments[0].IsClass)
                    foundDeserializer = (IDeserializer)Activator.CreateInstance
                        (typeof(MandatoryObjectListDeserializer<>).MakeGenericType
                             (typeof(TMsg), pi.PropertyType.GenericTypeArguments[0]), pi, OrxDeserializerLookup, messageVersion)!;
                else if ((pi.PropertyType.GetElementType()?.IsClass ?? false) && pi.PropertyType.GetArrayRank() == 1)
                    foundDeserializer = (IDeserializer)Activator.CreateInstance
                        (typeof(MandatoryObjectArrayDeserializer<>).MakeGenericType
                             (typeof(TMsg), pi.PropertyType.GetElementType()!), pi, OrxDeserializerLookup, messageVersion)!;
            }
            else
            {
                if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                    pi.PropertyType.GenericTypeArguments[0].IsClass)
                    foundDeserializer = (IDeserializer)Activator.CreateInstance
                        (typeof(OptionalObjectListDeserializer<>).MakeGenericType
                             (typeof(TMsg), pi.PropertyType.GenericTypeArguments[0]) , pi, OrxDeserializerLookup, messageVersion)!;
                else if ((pi.PropertyType.GetElementType()?.IsClass ?? false)
                      && pi.PropertyType.GetArrayRank() == 1)
                    foundDeserializer = (IDeserializer)Activator.CreateInstance
                        (typeof(OptionalObjectArrayDeserializer<>).MakeGenericType
                             (typeof(TMsg), pi.PropertyType.GetElementType()!) , pi, OrxDeserializerLookup, messageVersion)!;
            }
        }
        if (foundDeserializer != null)
        {
            return foundDeserializer;
        }

        throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
    }

    private void OptionalBasicTypes(byte messageVersion, PropertyInfo pi, ushort id)
    {
        if (interrogatingTypes.Contains(pi.PropertyType)) return;
        var deserializer = ResolveBasicTypes(messageVersion, pi, id);
        optional.Add(id, deserializer);
    }

    private Stack<Type> interrogatingTypes = new Stack<Type>();

    private IDeserializer ResolveBasicTypes(byte messageVersion, PropertyInfo pi, ushort id)
    {
        IDeserializer? foundDeserializer =
            pi.PropertyType switch
            {
                var t when t.IsBool()            => new BoolDeserializer(pi)
              , var t when t.IsNullableBool()    => new NullableBoolDeserializer(pi)
              , var t when t.IsByte()            => new ByteDeserializer(pi)
              , var t when t.IsNullableByte()    => new NullableByteDeserializer(pi)
              , var t when t.IsShort()           => new ShortDeserializer(pi)
              , var t when t.IsNullableShort()   => new NullableShortDeserializer(pi)
              , var t when t.IsUShort()          => new UShortDeserializer(pi)
              , var t when t.IsNullableUShort()  => new NullableUShortDeserializer(pi)
              , var t when t.IsInt()             => new IntDeserializer(pi)
              , var t when t.IsNullableInt()     => new NullableIntDeserializer(pi)
              , var t when t.IsUInt()            => new UIntDeserializer(pi)
              , var t when t.IsNullableUInt()    => new NullableUIntDeserializer(pi)
              , var t when t.IsLong()            => new LongDeserializer(pi)
              , var t when t.IsNullableLong()    => new NullableLongDeserializer(pi)
              , var t when t.IsULong()           => new ULongDeserializer(pi)
              , var t when t.IsNullableULong()   => new NullableULongDeserializer(pi)
              , var t when t.IsDecimal()         => new DecimalDeserializer(pi)
              , var t when t.IsNullableDecimal() => new NullableDecimalDeserializer(pi)
              , var t when t.IsString()          => new StringDeserializer(pi)

              , var t when t.IsMutableString()     => new MutableStringDeserializer(pi, OrxDeserializerLookup.Recycler)
              , var t when t.IsDateTime()          => new DateTimeDeserializer(pi)
              , var t when t.IsNullableDateTime()  => new NullableDateTimeDeserializer(pi)
              , var t when t.IsStringToStringMap() => new MapDeserializer(pi)
              , _                                  => null
            };

        if (foundDeserializer == null)
        {
            interrogatingTypes.Push(pi.PropertyType);
            if (id == ushort.MaxValue)
            {
                if (pi.PropertyType.IsClass && (OrxMandatoryField.FindAll(pi.PropertyType).Any()
                                             || OrxOptionalField.FindAll(pi.PropertyType).Any()))
                {
                    var mapDerivedAttribs = (OrxMapToDerivedClasses?)pi.PropertyType.GetCustomAttributes(typeof(OrxMapToDerivedClasses), false).FirstOrDefault();

                    if ((mapDerivedAttribs != null && pi.PropertyType.IsAbstract) ||
                        pi.PropertyType.GetConstructor(Type.EmptyTypes) == null ||
                        (mapDerivedAttribs?.Mapping.Count ?? 0) > 0)
                        foundDeserializer = (IDeserializer)Activator.CreateInstance
                            (typeof(MandatoryObjectDeserializer<,>).MakeGenericType(typeof(TMsg), typeof(object), pi.PropertyType), pi
                           , mapDerivedAttribs?.Mapping, OrxDeserializerLookup, messageVersion)!;
                    else
                        foundDeserializer = (IDeserializer)Activator.CreateInstance
                            (typeof(MandatoryObjectDeserializer<,>).MakeGenericType(typeof(TMsg), pi.PropertyType, pi.PropertyType), pi
                           , OrxDeserializerLookup, messageVersion)!;
                }
            }
            else
            {
                if (pi.PropertyType.IsClass && (OrxMandatoryField.FindAll(pi.PropertyType).Any()
                                             || OrxOptionalField.FindAll(pi.PropertyType).Any()))
                {
                    var mapDerivedAttribs = (OrxMapToDerivedClasses?)pi.PropertyType.GetCustomAttributes(typeof(OrxMapToDerivedClasses), false).FirstOrDefault();

                    if ((mapDerivedAttribs != null && pi.PropertyType.IsAbstract) ||
                        pi.PropertyType.GetConstructor(Type.EmptyTypes) == null ||
                        (mapDerivedAttribs?.Mapping.Count ?? 0) > 0)
                        foundDeserializer = (IDeserializer)Activator.CreateInstance
                            (typeof(OptionalObjectDeserializer<,>).MakeGenericType(typeof(TMsg), typeof(object), pi.PropertyType), pi
                           , mapDerivedAttribs?.Mapping, OrxDeserializerLookup, messageVersion)!;
                    else
                        foundDeserializer = (IDeserializer)Activator.CreateInstance
                            (typeof(OptionalObjectDeserializer<,>).MakeGenericType(typeof(TMsg), pi.PropertyType, pi.PropertyType), pi
                           , OrxDeserializerLookup, messageVersion)!;
                }
            }
            interrogatingTypes.Pop();
        }
        if (foundDeserializer != null)
        {
            return foundDeserializer;
        }

        throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
    }

    private void MandatoryArrayTypes(byte messageVersion, PropertyInfo pi, int i)
    {
        if (interrogatingTypes.Contains(pi.PropertyType)) return;
        var deserializer = ResolveArrayTypes(messageVersion, pi, ushort.MaxValue);
        mandatory[i] =   deserializer;
    }

    private void MandatoryBasicTypes(byte messageVersion, PropertyInfo pi, int i)
    {
        if (interrogatingTypes.Contains(pi.PropertyType)) return;
        IDeserializer deserializer = ResolveBasicTypes(messageVersion, pi, ushort.MaxValue);
        
        mandatory[i] =   deserializer;
    }

    private interface IDeserializer
    {
        unsafe void Deserialize(TMsg message, ref byte* bufferPtr);
    }

    private abstract class Deserializer<TProp> : IDeserializer
    {
        private static IFLogger logger
            = FLoggerFactory.Instance.GetLogger(
                                                "FortitudeIO.Protocols.ORX.Serialization.Deserialization.OrxByteDeserializer.Deserializer");

        protected readonly Action<TMsg, TProp?> Set;

        protected Deserializer(PropertyInfo property)
        {
            try
            {
                Set = (Action<TMsg, TProp?>)Delegate.CreateDelegate(typeof(Action<TMsg, TProp?>), property.GetSetMethod(true)!);
            }
            catch (ArgumentException ae)
            {
                logger.Error($"Error when trying to bind Tm:{typeof(TMsg).FullName} and Tp:{typeof(TProp).FullName} in Deserializer", ae);
                throw;
            }
        }

        public abstract unsafe void Deserialize(TMsg message, ref byte* buffer);
    }

    private sealed class MandatoryObjectDeserializer<TD, TP>(PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup, byte version)
        : Deserializer<TP>(property)
        where TD : class, new()
        where TP : class
    {
        private readonly Dictionary<ushort, IOrxDeserializer>? deserializerLookup;

        private readonly IOrxDeserializer itemSerializer = orxDeserializerLookup.GetOrCreateDeserializerForVersion(typeof(TD), version);

        // ReSharper disable once UnusedMember.Local
        public MandatoryObjectDeserializer
        (PropertyInfo property, Dictionary<ushort, Type>? mapping,
            IOrxDeserializerLookup orxDeserializerLookup, byte version) : this(property, orxDeserializerLookup, version)
        {
            deserializerLookup = new Dictionary<ushort, IOrxDeserializer>();
            if (mapping != null)
                foreach (var keyValuePair in mapping)
                {
                    var mappedSerializer = orxDeserializerLookup.GetOrCreateDeserializerForVersion(
                                                                                                   keyValuePair.Value, version);
                    deserializerLookup.Add(keyValuePair.Key, mappedSerializer);
                }
        }

        // ReSharper disable once MemberCanBePrivate.Local

        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            // ReSharper disable once UnusedVariable
            int  ignoredId = StreamByteOps.ToUShort(ref ptr);
            uint size      = StreamByteOps.ToUShort(ref ptr);
            if (size == 0)
            {
                Set(message, null);
                return;
            }

            TD typedProp;
            if (deserializerLookup is { Count: > 0 })
            {
                var typeId = StreamByteOps.ToUShort(ref ptr);

                var mappedDeserializer = deserializerLookup[typeId];
                typedProp = (TD)mappedDeserializer.Deserialize(ptr, size, version);
            }
            else
            {
                typedProp = (TD)itemSerializer.Deserialize(ptr, size, version);
            }

            if (typedProp is IRecyclableObject recyclableObject) recyclableObject.Recycler = orxDeserializerLookup.Recycler;

            Set(message, (TP)(object)typedProp);
            ptr += size;
        }
    }

    private sealed class OptionalObjectDeserializer<TD, TP>(PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup, byte version) : Deserializer<TP>(property) 
        where TD : class, new()
        where TP : class
    {
        private readonly Dictionary<ushort, IOrxDeserializer>? deserializerLookup;

        private readonly IOrxDeserializer itemSerializer = orxDeserializerLookup.GetOrCreateDeserializerForVersion(typeof(TD), version);


        // ReSharper disable once UnusedMember.Local
        public OptionalObjectDeserializer
        (PropertyInfo property, Dictionary<ushort, Type>? mapping,
            IOrxDeserializerLookup orxDeserializerLookup, byte version) : this(property, orxDeserializerLookup, version)
        {
            if (mapping != null)
            {
                deserializerLookup = new Dictionary<ushort, IOrxDeserializer>();
                foreach (var keyValuePair in mapping)
                {
                    var mappedSerializer = 
                        orxDeserializerLookup.GetOrCreateDeserializerForVersion(keyValuePair.Value, version);
                    deserializerLookup.Add(keyValuePair.Key, mappedSerializer);
                }
            }
        }

        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            ptr -= OrxConstants.UInt16Sz;
            uint size = StreamByteOps.ToUShort(ref ptr);
            if (size == 0)
            {
                // TODO recycle previous instance
                Set(message, null);
                return;
            }

            TD typedProp;
            if (deserializerLookup is { Count: > 0 })
            {
                var typeId = StreamByteOps.ToUShort(ref ptr);

                var mappedDeserializer = deserializerLookup[typeId];
                typedProp = (TD)mappedDeserializer.Deserialize(ptr, size, version);
            }
            else
            {
                typedProp = (TD)itemSerializer.Deserialize(ptr, size, version);
            }

            if (typedProp is IRecyclableObject recyclableObject) recyclableObject.Recycler = orxDeserializerLookup.Recycler;

            Set(message, (TP)(object)typedProp);
            ptr += size;
        }
    }

    private sealed class BoolArrayDeserializer(PropertyInfo property) : Deserializer<bool[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new bool[StreamByteOps.ToUShort(ref ptr)];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = *ptr > 0;
                ptr++;
            }

            Set(message, array);
        }
    }

    private sealed class NullableBoolArrayDeserializer(PropertyInfo property) : Deserializer<bool?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new bool?[StreamByteOps.ToUShort(ref ptr)];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = *ptr > 0;
                ptr++;
            }

            Set(message, array);
        }
    }

    private sealed class BoolListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<bool>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var boolList = recycler.Borrow<List<bool>>();
            boolList.Clear();
            for (var i = 0; i < size; i++)
            {
                boolList.Add(*ptr > 0);
                ptr++;
            }

            Set(message, boolList);
        }
    }

    private sealed class NullableBoolListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<bool?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var boolList = recycler.Borrow<List<bool?>>();
            boolList.Clear();
            for (var i = 0; i < size; i++)
            {
                boolList.Add(*ptr > 0);
                ptr++;
            }

            Set(message, boolList);
        }
    }

    private sealed class ByteArrayDeserializer(PropertyInfo property) : Deserializer<byte[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new byte[StreamByteOps.ToUShort(ref ptr)];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = *ptr;
                ptr++;
            }

            Set(message, array);
        }
    }

    private sealed class NullableByteArrayDeserializer(PropertyInfo property) : Deserializer<byte?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new byte?[StreamByteOps.ToUShort(ref ptr)];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = *ptr;
                ptr++;
            }

            Set(message, array);
        }
    }

    private sealed class ByteListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<byte>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var byteList = recycler.Borrow<List<byte>>();
            byteList.Clear();
            for (var i = 0; i < size; i++)
            {
                byteList.Add(*ptr);
                ptr++;
            }

            Set(message, byteList);
        }
    }

    private sealed class NullableByteListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<byte?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var byteList = recycler.Borrow<List<byte?>>();
            byteList.Clear();
            for (var i = 0; i < size; i++)
            {
                byteList.Add(*ptr);
                ptr++;
            }

            Set(message, byteList);
        }
    }

    private sealed class ShortArrayDeserializer(PropertyInfo property) : Deserializer<short[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new short[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToShort(ref ptr);
            Set(message, array);
        }
    }

    private sealed class NullableShortArrayDeserializer(PropertyInfo property) : Deserializer<short?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new short?[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToShort(ref ptr);
            Set(message, array);
        }
    }

    private sealed class ShortListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<short>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size      = StreamByteOps.ToUShort(ref ptr);
            var shortList = recycler.Borrow<List<short>>();
            shortList.Clear();
            for (var i = 0; i < size; i++) shortList.Add(StreamByteOps.ToShort(ref ptr));
            Set(message, shortList);
        }
    }

    private sealed class NullableShortListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<short?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size      = StreamByteOps.ToUShort(ref ptr);
            var shortList = recycler.Borrow<List<short?>>();
            shortList.Clear();
            for (var i = 0; i < size; i++) shortList.Add(StreamByteOps.ToShort(ref ptr));
            Set(message, shortList);
        }
    }

    private sealed class UShortArrayDeserializer(PropertyInfo property) : Deserializer<ushort[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new ushort[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToUShort(ref ptr);
            Set(message, array);
        }
    }

    private sealed class NullableUShortArrayDeserializer(PropertyInfo property) : Deserializer<ushort?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new ushort?[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToUShort(ref ptr);
            Set(message, array);
        }
    }

    private sealed class UShortListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<ushort>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size       = StreamByteOps.ToUShort(ref ptr);
            var ushortList = recycler.Borrow<List<ushort>>();
            ushortList.Clear();
            for (var i = 0; i < size; i++) ushortList.Add(StreamByteOps.ToUShort(ref ptr));
            Set(message, ushortList);
        }
    }

    private sealed class NullableUShortListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<ushort?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size       = StreamByteOps.ToUShort(ref ptr);
            var ushortList = recycler.Borrow<List<ushort?>>();
            ushortList.Clear();
            for (var i = 0; i < size; i++) ushortList.Add(StreamByteOps.ToUShort(ref ptr));
            Set(message, ushortList);
        }
    }

    private sealed class IntArrayDeserializer(PropertyInfo property) : Deserializer<int[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new int[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToInt(ref ptr);
            Set(message, array);
        }
    }

    private sealed class NullableIntArrayDeserializer(PropertyInfo property) : Deserializer<int?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new int?[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToInt(ref ptr);
            Set(message, array);
        }
    }

    private sealed class IntListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<int>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size    = StreamByteOps.ToUShort(ref ptr);
            var intList = recycler.Borrow<List<int>>();
            intList.Clear();
            for (var i = 0; i < size; i++) intList.Add(StreamByteOps.ToInt(ref ptr));
            Set(message, intList);
        }
    }

    private sealed class NullableIntListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<int?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size    = StreamByteOps.ToUShort(ref ptr);
            var intList = recycler.Borrow<List<int?>>();
            intList.Clear();
            for (var i = 0; i < size; i++) intList.Add(StreamByteOps.ToInt(ref ptr));
            Set(message, intList);
        }
    }

    private sealed class UIntArrayDeserializer(PropertyInfo property) : Deserializer<uint[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new uint[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToUInt(ref ptr);
            Set(message, array);
        }
    }

    private sealed class NullableUIntArrayDeserializer(PropertyInfo property) : Deserializer<uint?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new uint?[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToUInt(ref ptr);
            Set(message, array);
        }
    }

    private sealed class UIntListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<uint>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var uintList = recycler.Borrow<List<uint>>();
            uintList.Clear();
            for (var i = 0; i < size; i++) uintList.Add(StreamByteOps.ToUInt(ref ptr));
            Set(message, uintList);
        }
    }

    private sealed class NullableUIntListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<uint?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var uintList = recycler.Borrow<List<uint?>>();
            uintList.Clear();
            for (var i = 0; i < size; i++) uintList.Add(StreamByteOps.ToUInt(ref ptr));
            Set(message, uintList);
        }
    }

    private sealed class LongArrayDeserializer(PropertyInfo property) : Deserializer<long[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new long[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToLong(ref ptr);
            Set(message, array);
        }
    }

    private sealed class NullableLongArrayDeserializer(PropertyInfo property) : Deserializer<long?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new long?[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToLong(ref ptr);
            Set(message, array);
        }
    }

    private sealed class LongListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<long>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var longList = recycler.Borrow<List<long>>();
            longList.Clear();
            for (var i = 0; i < size; i++) longList.Add(StreamByteOps.ToLong(ref ptr));
            Set(message, longList);
        }
    }

    private sealed class NullableLongListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<long?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var longList = recycler.Borrow<List<long?>>();
            longList.Clear();
            for (var i = 0; i < size; i++) longList.Add(StreamByteOps.ToLong(ref ptr));
            Set(message, longList);
        }
    }

    private sealed class ULongArrayDeserializer(PropertyInfo property) : Deserializer<ulong[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new ulong[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToULong(ref ptr);
            Set(message, array);
        }
    }

    private sealed class NullableULongArrayDeserializer(PropertyInfo property) : Deserializer<ulong?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new ulong?[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToULong(ref ptr);
            Set(message, array);
        }
    }

    private sealed class ULongListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<ulong>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var longList = recycler.Borrow<List<ulong>>();
            longList.Clear();
            for (var i = 0; i < size; i++) longList.Add(StreamByteOps.ToULong(ref ptr));
            Set(message, longList);
        }
    }

    private sealed class NullableULongListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<ulong?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var longList = recycler.Borrow<List<ulong?>>();
            longList.Clear();
            for (var i = 0; i < size; i++) longList.Add(StreamByteOps.ToULong(ref ptr));
            Set(message, longList);
        }
    }

    private sealed class DecimalArrayDeserializer(PropertyInfo property) : Deserializer<decimal[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new decimal[StreamByteOps.ToUInt(ref ptr)];
            for (var i = 0; i < array.Length; i++)
            {
                var numberOfDecimalPlaces = *ptr++;
                var convertLongToDecimal  = StreamByteOps.ToLong(ref ptr);
                array[i] = (decimal)Math.Pow(10, -numberOfDecimalPlaces) * convertLongToDecimal;
            }

            Set(message, array);
        }
    }

    private sealed class NullableDecimalArrayDeserializer(PropertyInfo property) : Deserializer<decimal?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new decimal?[StreamByteOps.ToUInt(ref ptr)];
            for (var i = 0; i < array.Length; i++)
            {
                var numberOfDecimalPlaces = *ptr++;
                var convertLongToDecimal  = StreamByteOps.ToLong(ref ptr);
                array[i] = (decimal)Math.Pow(10, -numberOfDecimalPlaces) * convertLongToDecimal;
            }

            Set(message, array);
        }
    }

    private sealed class DecimalListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<decimal>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var decimalList = recycler.Borrow<List<decimal>>();
            decimalList.Clear();
            for (var i = 0; i < size; i++)
            {
                var numberOfDecimalPlaces = *ptr++;
                var convertLongToDecimal  = StreamByteOps.ToLong(ref ptr);
                decimalList.Add((decimal)Math.Pow(10, -numberOfDecimalPlaces) * convertLongToDecimal);
            }

            Set(message, decimalList);
        }
    }

    private sealed class NullableDecimalListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<decimal?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var decimalList = recycler.Borrow<List<decimal?>>();
            decimalList.Clear();
            for (var i = 0; i < size; i++)
            {
                var numberOfDecimalPlaces = *ptr++;
                var convertLongToDecimal  = StreamByteOps.ToLong(ref ptr);
                decimalList.Add((decimal)Math.Pow(10, -numberOfDecimalPlaces) * convertLongToDecimal);
            }

            Set(message, decimalList);
        }
    }

    private sealed class DateTimeArrayDeserializer(PropertyInfo property) : Deserializer<DateTime[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new DateTime[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++)
            {
                var longValue = StreamByteOps.ToLong(ref ptr);
                longValue /= 100;
                array[i]  =  DateTime.FromBinary(longValue);
            }
            Set(message, array);
        }
    }

    private sealed class NullableDateTimeArrayDeserializer(PropertyInfo property) : Deserializer<DateTime?[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new DateTime?[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++)
            {
                var longValue = StreamByteOps.ToLong(ref ptr);
                longValue /= 100;
                array[i]  =  DateTime.FromBinary(longValue);
            }
            Set(message, array);
        }
    }

    private sealed class DateTimeListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<DateTime>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var longList = recycler.Borrow<List<DateTime>>();
            longList.Clear();
            for (var i = 0; i < size; i++)
            {
                var longValue = StreamByteOps.ToLong(ref ptr);
                longValue /= 100;
                longList.Add(DateTime.FromBinary(longValue));
            }
            Set(message, longList);
        }
    }

    private sealed class NullableDateTimeListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<DateTime?>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var longList = recycler.Borrow<List<DateTime?>>();
            longList.Clear();
            for (var i = 0; i < size; i++)
            {
                var longValue = StreamByteOps.ToLong(ref ptr);
                longValue /= 100;
                longList.Add(DateTime.FromBinary(longValue));
            }
            Set(message, longList);
        }
    }

    private sealed class StringArrayDeserializer(PropertyInfo property) : Deserializer<string[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new string[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToStringWithSizeHeader(ref ptr)!;
            Set(message, array);
        }
    }

    private sealed class StringListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<string>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var stringList = recycler.Borrow<List<string>>();
            for (var i = 0; i < size; i++) stringList.Add(StreamByteOps.ToStringWithSizeHeader(ref ptr)!);
            Set(message, stringList);
        }
    }

    private sealed class MutableStringArrayDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<MutableString[]>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var array = new MutableString[StreamByteOps.ToUShort(ref ptr)];
            for (var i = 0; i < array.Length; i++)
            {
                var mutableString = recycler.Borrow<MutableString>();
                array[i] = StreamByteOps.ToMutableStringWithSizeHeader(ref ptr, mutableString);
            }

            Set(message, array);
        }
    }

    private sealed class MutableStringListDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<List<MutableString>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var numberOfElements  = StreamByteOps.ToUShort(ref ptr);
            var mutableStringList = recycler.Borrow<List<MutableString>>();
            mutableStringList.Clear();
            for (var i = 0; i < numberOfElements; i++)
            {
                var mutableString = recycler.Borrow<MutableString>();
                mutableStringList.Add(StreamByteOps.ToMutableStringWithSizeHeader(ref ptr, mutableString));
            }

            Set(message, mutableStringList);
        }
    }

    private sealed class MapDeserializer(PropertyInfo property) : Deserializer<Dictionary<string, string>>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var count = StreamByteOps.ToUInt(ref ptr);
            var dic   = new Dictionary<string, string>();
            for (var i = 0; i < count; i++)
            {
                var key   = StreamByteOps.ToStringWithSizeHeader(ref ptr)!;
                var value = StreamByteOps.ToStringWithSizeHeader(ref ptr)!;
                dic[key] = value;
            }

            Set(message, dic);
        }
    }

    private class OptionalObjectArrayDeserializer<TObj> : Deserializer<TObj[]> where TObj : class, new()
    {
        private readonly Func<ushort, TObj[]> get;

        private readonly OrxByteDeserializer<TObj> itemDeserializer;

        private readonly byte version;

        // ReSharper disable once MemberCanBeProtected.Local
        public OptionalObjectArrayDeserializer
        (PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup
          , byte version)
            : base(property)
        {
            this.version = version;

            get = size => (TObj[])Activator.CreateInstance(typeof(TObj[]), (int)size)!;

            itemDeserializer = new OrxByteDeserializer<TObj>(orxDeserializerLookup, version);
        }

        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var arraySize = StreamByteOps.ToUShort(ref ptr);
            if (arraySize != ushort.MaxValue)
            {
                var array = get(arraySize);
                for (var i = 0; i < array.Length; i++)
                {
                    uint size = StreamByteOps.ToUShort(ref ptr);

                    array[i] =  (TObj)itemDeserializer.Deserialize(ptr, size, version);
                    ptr      += size;
                }

                Set(message, array);
            }
            else
            {
                Set(message, null);
            }
        }
    }

    private class OptionalObjectListDeserializer<TObj>
    (
        PropertyInfo property
      , IOrxDeserializerLookup orxDeserializerLookup
      , byte version)
        : Deserializer<List<TObj>>(property)
        where TObj : class, new()
    {
        private readonly OrxByteDeserializer<TObj> itemDeserializer = new(orxDeserializerLookup, version);

        private readonly IRecycler recycler = orxDeserializerLookup.Recycler;

        // ReSharper disable once MemberCanBeProtected.Local

        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var arraySize = StreamByteOps.ToUShort(ref ptr);
            if (arraySize != ushort.MaxValue)
            {
                var objectList = recycler.Borrow<List<TObj>>();
                objectList.Clear();
                for (var i = 0; i < arraySize; i++)
                {
                    uint size = StreamByteOps.ToUShort(ref ptr);
                    objectList.Add((TObj)itemDeserializer.Deserialize(ptr, size, version));
                    ptr += size;
                }

                Set(message, objectList);
            }
            else
            {
                Set(message, null);
            }
        }
    }

    private class MandatoryObjectArrayDeserializer<TObj>(PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup, byte version)
        : OptionalObjectArrayDeserializer<TObj>(property, orxDeserializerLookup, version)
        where TObj : class, new()
    {
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int ignoredArrayId   = StreamByteOps.ToUShort(ref ptr);
            int ignoredEntrySize = StreamByteOps.ToUShort(ref ptr);
            base.Deserialize(message, ref ptr);
        }
    }

    private class MandatoryObjectListDeserializer<TObj>(PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup, byte version)
        : OptionalObjectListDeserializer<TObj>(property, orxDeserializerLookup, version)
        where TObj : class, new()
    {
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            int ignoredArrayId   = StreamByteOps.ToUShort(ref ptr);
            int ignoredEntrySize = StreamByteOps.ToUShort(ref ptr);
            base.Deserialize(message, ref ptr);
        }
    }

    private sealed class BoolDeserializer(PropertyInfo property) : Deserializer<bool>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, *ptr++ != 0);
        }
    }

    private sealed class NullableBoolDeserializer(PropertyInfo property) : Deserializer<bool?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, *ptr++ != 0);
        }
    }

    private sealed class ByteDeserializer(PropertyInfo property) : Deserializer<byte>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, *ptr++);
        }
    }

    private sealed class NullableByteDeserializer(PropertyInfo property) : Deserializer<byte?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, *ptr++);
        }
    }

    private sealed class ShortDeserializer(PropertyInfo property) : Deserializer<short>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToShort(ref ptr));
        }
    }

    private sealed class NullableShortDeserializer(PropertyInfo property) : Deserializer<short?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToShort(ref ptr));
        }
    }

    private sealed class UShortDeserializer(PropertyInfo property) : Deserializer<ushort>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToUShort(ref ptr));
        }
    }

    private sealed class NullableUShortDeserializer(PropertyInfo property) : Deserializer<ushort?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToUShort(ref ptr));
        }
    }

    private sealed class IntDeserializer(PropertyInfo property) : Deserializer<int>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToInt(ref ptr));
        }
    }

    private sealed class NullableIntDeserializer(PropertyInfo property) : Deserializer<int?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToInt(ref ptr));
        }
    }

    private sealed class UIntDeserializer(PropertyInfo property) : Deserializer<uint>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToUInt(ref ptr));
        }
    }

    private sealed class NullableUIntDeserializer(PropertyInfo property) : Deserializer<uint?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToUInt(ref ptr));
        }
    }

    private sealed class LongDeserializer(PropertyInfo property) : Deserializer<long>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToLong(ref ptr));
        }
    }

    private sealed class NullableLongDeserializer(PropertyInfo property) : Deserializer<long?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToLong(ref ptr));
        }
    }

    private sealed class ULongDeserializer(PropertyInfo property) : Deserializer<ulong>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToULong(ref ptr));
        }
    }

    private sealed class NullableULongDeserializer(PropertyInfo property) : Deserializer<ulong?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToULong(ref ptr));
        }
    }

    private sealed class DecimalDeserializer(PropertyInfo property) : Deserializer<decimal>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var factor = *ptr++;
            Set(message, OrxScaling.Unscale(StreamByteOps.ToUInt(ref ptr), factor));
        }
    }

    private sealed class NullableDecimalDeserializer(PropertyInfo property) : Deserializer<decimal?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var factor = *ptr++;
            Set(message, OrxScaling.Unscale(StreamByteOps.ToUInt(ref ptr), factor));
        }
    }

    private sealed class StringDeserializer(PropertyInfo property) : Deserializer<string>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message, StreamByteOps.ToStringWithSizeHeader(ref ptr));
        }
    }

    private sealed class MutableStringDeserializer(PropertyInfo property, IRecycler recycler) : Deserializer<MutableString>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            Set(message
              , StreamByteOps.ToMutableStringWithSizeHeader(ref ptr, recycler.Borrow<MutableString>()));
        }
    }

    private sealed class DateTimeDeserializer(PropertyInfo property) : Deserializer<DateTime>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var nanosSinceUnixEpoch = StreamByteOps.ToLong(ref ptr);
            var dateTime = new DateTime(DateTimeConstants.UnixEpochTicks + nanosSinceUnixEpoch / 100,
                                        DateTimeKind.Utc);
            Set(message, dateTime);
        }
    }

    private sealed class NullableDateTimeDeserializer(PropertyInfo property) : Deserializer<DateTime?>(property)
    {
        public override unsafe void Deserialize(TMsg message, ref byte* ptr)
        {
            var nanosSinceUnixEpoch = StreamByteOps.ToLong(ref ptr);
            var dateTime = new DateTime(DateTimeConstants.UnixEpochTicks + nanosSinceUnixEpoch / 100,
                                        DateTimeKind.Utc);
            Set(message, dateTime);
        }
    }
}
