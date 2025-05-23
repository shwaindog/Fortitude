﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Logging;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public class OrxByteDeserializer<Tm> : IOrxDeserializer where Tm : class, new()
{
    private static int lastInstanceNum;

    private IDeserializer[] mandatory;

    private Dictionary<ushort, IDeserializer> optional = new();

    private byte thisVersion;

    public OrxByteDeserializer(IOrxDeserializerLookup orxDeserializerLookup, byte version = 0)
    {
        OrxDeserializerLookup = orxDeserializerLookup;
        var currentType = typeof(Tm);
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

    public OrxByteDeserializer(OrxByteDeserializer<Tm> toClone)
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
        if (source is not OrxByteDeserializer<Tm> orxByteDeserializer) return this;
        mandatory = [..orxByteDeserializer.mandatory];
        optional  = new Dictionary<ushort, IDeserializer>(orxByteDeserializer.optional);
        return this;
    }

    public Type MessageType => typeof(Tm);

    public unsafe object Deserialize(byte* ptr, uint length, byte messageVersion)
    {
        if (messageVersion >= thisVersion) return DeserializeCurrentType(ptr, length);

        var deserializerForVersion = OrxDeserializerLookup!.GetDeserializerForVersion(typeof(Tm), messageVersion)!;
        var messagePart = deserializerForVersion.Deserialize(ptr, length, messageVersion) is
            IOlderVersionMessagePart<Tm> olderObjectVersion
            ? olderObjectVersion.ToLatestVersion()
            : DeserializeCurrentType(ptr, length);

        return messagePart;
    }

    object? IMessageDeserializer.Deserialize(IBufferContext bufferContext) => Deserialize(bufferContext);

    object ICloneable.Clone() => Clone();

    public IMessageDeserializer Clone() => new OrxByteDeserializer<Tm>(this);

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

    public unsafe Tm Deserialize(ISerdeContext readContext)
    {
        var sockBuffContext = readContext as SocketBufferReadContext;
        sockBuffContext?.DispatchLatencyLogger?.Add(SocketDataLatencyLogger.EnterDeserializer);
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            if (ReadMessageHeader)
            {
                using var buffer       = messageBufferContext.EncodedBuffer!;
                var       fixBufferPtr = buffer.ReadBuffer + buffer.BufferRelativeReadCursor;
                messageBufferContext.MessageHeader = ReadHeader(ref fixBufferPtr);
            }
            var messageVersion = messageBufferContext.MessageHeader.Version;
            var messageSize    = messageBufferContext.MessageHeader.MessageSize;
            return (Tm)Deserialize(messageBufferContext.EncodedBuffer!, messageSize - MessageHeader.SerializationSize, messageVersion);
        }

        throw new ArgumentException("Expected readContext to be of type IBufferContext");
    }

    private void SetCorrectMessageVersion(byte version, Type currentType)
    {
        if (currentType.IsSubclassOf(typeof(IVersionedMessage)) && version == 0)
        {
            var instanceOfType =
                (IVersionedMessage)OrxDeserializerLookup!.Recycler.Borrow<Tm>();
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
                        (typeof(Tm), (IOrxDeserializer)Activator.CreateInstance
                             (typeof(OrxByteDeserializer<>).MakeGenericType(oldVersion)
                            , OrxDeserializerLookup, previousVersionDetails.ToVersion)!
                       , previousVersionDetails.FromVersion, previousVersionDetails.ToVersion);
            }
        }
    }

    private unsafe Tm DeserializeCurrentType(byte* ptr, uint length)
    {
        var messagePart = OrxDeserializerLookup!.Recycler.Borrow<Tm>();
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
        if (pi.PropertyType == typeof(bool[]))
            optional.Add(id, new BoolArrayDeserializer(pi));
        else if (pi.PropertyType == typeof(List<bool>))
            optional.Add(id, new BoolListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(byte[]))
            optional.Add(id, new ByteArrayDeserializer(pi));
        else if (pi.PropertyType == typeof(List<byte>))
            optional.Add(id, new ByteListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(short[]))
            optional.Add(id, new ShortArrayDeserializer(pi));
        else if (pi.PropertyType == typeof(List<short>))
            optional.Add(id, new ShortListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(ushort[]))
            optional.Add(id, new UShortArrayDeserializer(pi));
        else if (pi.PropertyType == typeof(List<ushort>))
            optional.Add(id, new UShortListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(int[]))
            optional.Add(id, new IntArrayDeserializer(pi));
        else if (pi.PropertyType == typeof(List<int>))
            optional.Add(id, new IntListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(uint[]))
            optional.Add(id, new UIntArrayDeserializer(pi));
        else if (pi.PropertyType == typeof(List<uint>))
            optional.Add(id, new UIntListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(long[]))
            optional.Add(id, new LongArrayDeserializer(pi));
        else if (pi.PropertyType == typeof(List<long>))
            optional.Add(id, new LongListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(decimal[]))
            optional.Add(id, new DecimalArrayDeserializer(pi));
        else if (pi.PropertyType == typeof(List<decimal>))
            optional.Add(id, new DecimalListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(string[]))
            optional.Add(id, new StringArrayDeserializer(pi));
        else if (pi.PropertyType == typeof(List<string>))
            optional.Add(id, new StringListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(MutableString[]))
            optional.Add(id, new MutableStringArrayDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (pi.PropertyType == typeof(List<MutableString>))
            optional.Add(id, new MutableStringListDeserializer(pi, OrxDeserializerLookup.Recycler));
        else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                 pi.PropertyType.GenericTypeArguments[0].IsClass)
            optional.Add(id, (IDeserializer)Activator.CreateInstance(typeof(OptionalObjectListDeserializer<>)
                                                                         .MakeGenericType(typeof(Tm), pi.PropertyType.GenericTypeArguments[0]), pi,
                                                                     OrxDeserializerLookup, messageVersion)!);
        else if ((pi.PropertyType.GetElementType()?.IsClass ?? false)
              && pi.PropertyType.GetArrayRank() == 1)
            optional.Add(id, (IDeserializer)Activator.CreateInstance(typeof(OptionalObjectArrayDeserializer<>)
                                                                         .MakeGenericType(typeof(Tm), pi.PropertyType.GetElementType()!), pi,
                                                                     OrxDeserializerLookup, messageVersion)!);
        else
            throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
    }

    private void OptionalBasicTypes(byte messageVersion, PropertyInfo pi, ushort id)
    {
        if (pi.PropertyType == typeof(bool))
        {
            optional.Add(id, new BoolDeserializer(pi));
        }
        else if (pi.PropertyType == typeof(byte) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(byte)))
        {
            optional.Add(id, new UInt8Deserializer(pi));
        }
        else if (pi.PropertyType == typeof(short) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(short)))
        {
            optional.Add(id, new Int16Deserializer(pi));
        }
        else if (pi.PropertyType == typeof(ushort) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(ushort)))
        {
            optional.Add(id, new UInt16Deserializer(pi));
        }
        else if (pi.PropertyType == typeof(int) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(int)))
        {
            optional.Add(id, new Int32Deserializer(pi));
        }
        else if (pi.PropertyType == typeof(uint) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(uint)))
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
            optional.Add(id, new MutableStringDeserializer(pi, OrxDeserializerLookup.Recycler));
        }
        else if (pi.PropertyType == typeof(Dictionary<string, string>))
        {
            optional.Add(id, new MapDeserializer(pi));
        }
        else if (pi.PropertyType.IsClass && (OrxMandatoryField.FindAll(pi.PropertyType).Any()
                                          || OrxOptionalField.FindAll(pi.PropertyType).Any()))
        {
            var customAttributes = (OrxOptionalField?)pi
                                                      .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();

            if ((customAttributes != null && pi.PropertyType.IsAbstract) ||
                pi.PropertyType.GetConstructor(Type.EmptyTypes) == null ||
                (customAttributes?.Mapping?.Count ?? 0) > 0)
                optional.Add
                    (id, (IDeserializer)Activator.CreateInstance
                        (typeof(OptionalObjectDeserializer<>).MakeGenericType(typeof(Tm), typeof(object)), pi
                       , customAttributes?.Mapping, OrxDeserializerLookup, messageVersion)!);
            else
                optional.Add
                    (id, (IDeserializer)Activator.CreateInstance
                        (typeof(OptionalObjectDeserializer<>).MakeGenericType(typeof(Tm), pi.PropertyType), pi
                       , customAttributes?.Mapping, OrxDeserializerLookup, messageVersion)!);
        }
        else
        {
            throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
        }
    }

    private void MandatoryArrayTypes(byte messageVersion, PropertyInfo pi, int i)
    {
        if (pi.PropertyType == typeof(bool[]))
            mandatory[i] = new BoolArrayDeserializer(pi);
        else if (pi.PropertyType == typeof(List<bool>))
            mandatory[i] = new BoolListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(byte[]))
            mandatory[i] = new ByteArrayDeserializer(pi);
        else if (pi.PropertyType == typeof(List<byte>))
            mandatory[i] = new ByteListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(short[]))
            mandatory[i] = new ShortArrayDeserializer(pi);
        else if (pi.PropertyType == typeof(List<short>))
            mandatory[i] = new ShortListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(ushort[]))
            mandatory[i] = new UShortArrayDeserializer(pi);
        else if (pi.PropertyType == typeof(List<ushort>))
            mandatory[i] = new UShortListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(int[]))
            mandatory[i] = new IntArrayDeserializer(pi);
        else if (pi.PropertyType == typeof(List<int>))
            mandatory[i] = new IntListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(uint[]))
            mandatory[i] = new UIntArrayDeserializer(pi);
        else if (pi.PropertyType == typeof(List<uint>))
            mandatory[i] = new UIntListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(long[]))
            mandatory[i] = new LongArrayDeserializer(pi);
        else if (pi.PropertyType == typeof(List<long>))
            mandatory[i] = new LongListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(decimal[]))
            mandatory[i] = new DecimalArrayDeserializer(pi);
        else if (pi.PropertyType == typeof(List<decimal>))
            mandatory[i] = new DecimalListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(string[]))
            mandatory[i] = new StringArrayDeserializer(pi);
        else if (pi.PropertyType == typeof(List<string>))
            mandatory[i] = new StringListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(MutableString[]))
            mandatory[i] = new MutableStringArrayDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (pi.PropertyType == typeof(List<MutableString>))
            mandatory[i] = new MutableStringListDeserializer(pi, OrxDeserializerLookup.Recycler);
        else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType) &&
                 pi.PropertyType.GenericTypeArguments[0].IsClass)
            mandatory[i] = (IDeserializer)Activator.CreateInstance(typeof(MandatoryObjectListDeserializer<>)
                                                                       .MakeGenericType(typeof(Tm), pi.PropertyType.GenericTypeArguments[0]), pi,
                                                                   OrxDeserializerLookup, messageVersion)!;
        else if ((pi.PropertyType.GetElementType()?.IsClass ?? false) && pi.PropertyType.GetArrayRank() == 1)
            mandatory[i] = (IDeserializer)Activator.CreateInstance(typeof(MandatoryObjectArrayDeserializer<>)
                                                                       .MakeGenericType(typeof(Tm), pi.PropertyType.GetElementType()!), pi,
                                                                   OrxDeserializerLookup, messageVersion)!;
        else
            throw new Exception("Unsupported type: " + pi.PropertyType.FullName);
    }

    private void MandatoryBasicTypes(byte messageVersion, PropertyInfo pi, int i)
    {
        if (pi.PropertyType == typeof(bool))
        {
            mandatory[i] = new BoolDeserializer(pi);
        }
        else if (pi.PropertyType == typeof(byte) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(byte)))
        {
            mandatory[i] = new UInt8Deserializer(pi);
        }
        else if (pi.PropertyType == typeof(short) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(short)))
        {
            mandatory[i] = new Int16Deserializer(pi);
        }
        else if (pi.PropertyType == typeof(ushort) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(ushort)))
        {
            mandatory[i] = new UInt16Deserializer(pi);
        }
        else if (pi.PropertyType == typeof(int) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(int)))
        {
            mandatory[i] = new Int32Deserializer(pi);
        }
        else if (pi.PropertyType == typeof(uint) ||
                 (pi.PropertyType.IsEnum && Enum.GetUnderlyingType(pi.PropertyType) == typeof(uint)))
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
            mandatory[i] = new MutableStringDeserializer(pi, OrxDeserializerLookup.Recycler);
        }
        else if (pi.PropertyType.IsClass && (OrxMandatoryField.FindAll(pi.PropertyType).Any()
                                          || OrxOptionalField.FindAll(pi.PropertyType).Any()))
        {
            var customAttributes = (OrxMandatoryField?)pi
                                                       .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();

            if ((customAttributes != null && pi.PropertyType.IsAbstract) ||
                pi.PropertyType.GetConstructor(Type.EmptyTypes) == null ||
                (customAttributes?.Mapping?.Count ?? 0) > 0)
                mandatory[i] = (IDeserializer)Activator.CreateInstance
                    (typeof(MandatoryObjectDeserializer<,>).MakeGenericType(typeof(Tm), typeof(object), pi.PropertyType), pi
                   , customAttributes?.Mapping, OrxDeserializerLookup, messageVersion)!;
            else
                mandatory[i] = (IDeserializer)Activator.CreateInstance
                    (typeof(MandatoryObjectDeserializer<,>).MakeGenericType(typeof(Tm), pi.PropertyType, pi.PropertyType), pi
                   , OrxDeserializerLookup, messageVersion)!;
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
        private static IFLogger logger
            = FLoggerFactory.Instance.GetLogger(
                                                "FortitudeIO.Protocols.ORX.Serialization.Deserialization.OrxByteDeserializer.Deserializer");

        protected readonly Action<Tm, Tp?> Set;

        protected Deserializer(PropertyInfo property)
        {
            try
            {
                Set = (Action<Tm, Tp?>)Delegate.CreateDelegate(typeof(Action<Tm, Tp?>), property.GetSetMethod(true)!);
            }
            catch (ArgumentException ae)
            {
                logger.Error(
                             $"Error when trying to bind Tm:{typeof(Tm).FullName} and Tp:{typeof(Tp).FullName} in Deserializer"
                           , ae);
                throw;
            }
        }

        public abstract unsafe void Deserialize(Tm message, ref byte* buffer);
    }

    private sealed class MandatoryObjectDeserializer<TD, TP> : Deserializer<TP> where TD : class, new() where TP : class
    {
        private readonly Dictionary<ushort, IOrxDeserializer>? deserializerLookup;

        private readonly IOrxDeserializer itemSerializer;

        private readonly byte messageVersion;

        private readonly IOrxDeserializerLookup orxDeserializerLookup;

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
        public MandatoryObjectDeserializer
        (PropertyInfo property,
            IOrxDeserializerLookup orxDeserializerLookup, byte version) : base(property)
        {
            itemSerializer = orxDeserializerLookup.GetOrCreateDeserializerForVersion(typeof(TD), version);
            messageVersion = version;

            this.orxDeserializerLookup = orxDeserializerLookup;
        }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
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
                typedProp = (TD)mappedDeserializer.Deserialize(ptr, size, messageVersion);
            }
            else
            {
                typedProp = (TD)itemSerializer.Deserialize(ptr, size, messageVersion);
            }

            if (typedProp is IRecyclableObject recyclableObject) recyclableObject.Recycler = orxDeserializerLookup.Recycler;

            Set(message, (TP)(object)typedProp);
            ptr += size;
        }
    }

    private sealed class OptionalObjectDeserializer<T> : Deserializer<T> where T : class, new()
    {
        private readonly Dictionary<ushort, IOrxDeserializer>? deserializerLookup;

        private readonly IOrxDeserializer itemSerializer;
        private readonly byte             messageVersion;

        private readonly IOrxDeserializerLookup orxDeserializerLookup;

        // ReSharper disable once UnusedMember.Local
        public OptionalObjectDeserializer
        (PropertyInfo property, Dictionary<ushort, Type>? mapping,
            IOrxDeserializerLookup orxDeserializerLookup, byte version) : base(property)
        {
            itemSerializer = orxDeserializerLookup.GetOrCreateDeserializerForVersion(typeof(T), version);
            messageVersion = version;

            this.orxDeserializerLookup = orxDeserializerLookup;
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
            uint size = StreamByteOps.ToUShort(ref ptr);
            if (size == 0)
            {
                // TODO recycle previous instance
                Set(message, null);
                return;
            }

            T typedProp;
            if (deserializerLookup != null && deserializerLookup.Count > 0)
            {
                var typeId = StreamByteOps.ToUShort(ref ptr);

                var mappedDeserializer = deserializerLookup[typeId];
                typedProp = (T)mappedDeserializer.Deserialize(ptr, size, messageVersion);
            }
            else
            {
                typedProp = (T)itemSerializer.Deserialize(ptr, size, messageVersion);
            }

            if (typedProp is IRecyclableObject recyclableObject) recyclableObject.Recycler = orxDeserializerLookup.Recycler;

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
            var array = new bool[StreamByteOps.ToUShort(ref ptr)];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = *ptr > 0;
                ptr++;
            }

            Set(message, array);
        }
    }

    private sealed class BoolListDeserializer : Deserializer<List<bool>>
    {
        private readonly IRecycler recycler;

        public BoolListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
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

    private sealed class ByteArrayDeserializer : Deserializer<byte[]>
    {
        public ByteArrayDeserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
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

    private sealed class ByteListDeserializer : Deserializer<List<byte>>
    {
        private readonly IRecycler recycler;

        public ByteListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
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

    private sealed class ShortArrayDeserializer : Deserializer<short[]>
    {
        public ShortArrayDeserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            var array = new short[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToShort(ref ptr);
            Set(message, array);
        }
    }

    private sealed class ShortListDeserializer : Deserializer<List<short>>
    {
        private readonly IRecycler recycler;

        public ShortListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            int size      = StreamByteOps.ToUShort(ref ptr);
            var shortList = recycler.Borrow<List<short>>();
            shortList.Clear();
            for (var i = 0; i < size; i++) shortList.Add(StreamByteOps.ToShort(ref ptr));
            Set(message, shortList);
        }
    }

    private sealed class UShortArrayDeserializer : Deserializer<ushort[]>
    {
        public UShortArrayDeserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            var array = new ushort[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToUShort(ref ptr);
            Set(message, array);
        }
    }

    private sealed class UShortListDeserializer : Deserializer<List<ushort>>
    {
        private readonly IRecycler recycler;

        public UShortListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            int size       = StreamByteOps.ToUShort(ref ptr);
            var ushortList = recycler.Borrow<List<ushort>>();
            ushortList.Clear();
            for (var i = 0; i < size; i++) ushortList.Add(StreamByteOps.ToUShort(ref ptr));
            Set(message, ushortList);
        }
    }

    private sealed class IntArrayDeserializer : Deserializer<int[]>
    {
        public IntArrayDeserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            var array = new int[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToInt(ref ptr);
            Set(message, array);
        }
    }

    private sealed class IntListDeserializer : Deserializer<List<int>>
    {
        private readonly IRecycler recycler;

        public IntListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            int size    = StreamByteOps.ToUShort(ref ptr);
            var intList = recycler.Borrow<List<int>>();
            intList.Clear();
            for (var i = 0; i < size; i++) intList.Add(StreamByteOps.ToInt(ref ptr));
            Set(message, intList);
        }
    }

    private sealed class UIntArrayDeserializer : Deserializer<uint[]>
    {
        public UIntArrayDeserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            var array = new uint[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToUInt(ref ptr);
            Set(message, array);
        }
    }

    private sealed class UIntListDeserializer : Deserializer<List<uint>>
    {
        private readonly IRecycler recycler;

        public UIntListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var uintList = recycler.Borrow<List<uint>>();
            uintList.Clear();
            for (var i = 0; i < size; i++) uintList.Add(StreamByteOps.ToUInt(ref ptr));
            Set(message, uintList);
        }
    }

    private sealed class LongArrayDeserializer : Deserializer<long[]>
    {
        public LongArrayDeserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            var array = new long[StreamByteOps.ToUInt(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToLong(ref ptr);
            Set(message, array);
        }
    }

    private sealed class LongListDeserializer : Deserializer<List<long>>
    {
        private readonly IRecycler recycler;

        public LongListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var longList = recycler.Borrow<List<long>>();
            longList.Clear();
            for (var i = 0; i < size; i++) longList.Add(StreamByteOps.ToLong(ref ptr));
            Set(message, longList);
        }
    }

    private sealed class DecimalArrayDeserializer : Deserializer<decimal[]>
    {
        public DecimalArrayDeserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            var array = new decimal[StreamByteOps.ToUInt(ref ptr)];
            for (var i = 0; i < array.Length; i++)
            {
                var numberOfDecimalPlaces = *ptr++;
                var deDecimalizedLong     = StreamByteOps.ToLong(ref ptr);
                array[i] = (decimal)Math.Pow(10, -numberOfDecimalPlaces) * deDecimalizedLong;
            }

            Set(message, array);
        }
    }

    private sealed class DecimalListDeserializer : Deserializer<List<decimal>>
    {
        private readonly IRecycler recycler;

        public DecimalListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var decimalList = recycler.Borrow<List<decimal>>();
            decimalList.Clear();
            for (var i = 0; i < size; i++)
            {
                var numberOfDecimalPlaces = *ptr++;
                var deDecimalizedLong     = StreamByteOps.ToLong(ref ptr);
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
            var array = new string[StreamByteOps.ToUShort(ref ptr)];

            for (var i = 0; i < array.Length; i++) array[i] = StreamByteOps.ToStringWithSizeHeader(ref ptr)!;
            Set(message, array);
        }
    }

    private sealed class StringListDeserializer : Deserializer<List<string>>
    {
        private readonly IRecycler recycler;

        public StringListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            int size = StreamByteOps.ToUShort(ref ptr);

            var stringList = recycler.Borrow<List<string>>();
            for (var i = 0; i < size; i++) stringList.Add(StreamByteOps.ToStringWithSizeHeader(ref ptr)!);
            Set(message, stringList);
        }
    }

    private sealed class MutableStringArrayDeserializer : Deserializer<MutableString[]>
    {
        private readonly IRecycler recycler;

        public MutableStringArrayDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
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

    private sealed class MutableStringListDeserializer : Deserializer<List<MutableString>>
    {
        private readonly IRecycler recycler;

        public MutableStringListDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
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

    private sealed class MapDeserializer : Deserializer<Dictionary<string, string>>
    {
        public MapDeserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
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

    private class OptionalObjectArrayDeserializer<To> : Deserializer<To[]> where To : class, new()
    {
        private readonly Func<ushort, To[]> get;

        private readonly OrxByteDeserializer<To> itemDeserializer;

        private readonly byte version;

        // ReSharper disable once MemberCanBeProtected.Local
        public OptionalObjectArrayDeserializer
        (PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup
          , byte version)
            : base(property)
        {
            this.version = version;

            get = size => (To[])Activator.CreateInstance(typeof(To[]), (int)size)!;

            itemDeserializer = new OrxByteDeserializer<To>(orxDeserializerLookup, version);
        }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            var arraySize = StreamByteOps.ToUShort(ref ptr);
            if (arraySize != ushort.MaxValue)
            {
                var array = get(arraySize);
                for (var i = 0; i < array.Length; i++)
                {
                    uint size = StreamByteOps.ToUShort(ref ptr);

                    array[i] =  (To)itemDeserializer.Deserialize(ptr, size, version);
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

    private class OptionalObjectListDeserializer<To> : Deserializer<List<To>> where To : class, new()
    {
        private readonly OrxByteDeserializer<To> itemDeserializer;

        private readonly IRecycler recycler;
        private readonly byte      version;

        // ReSharper disable once MemberCanBeProtected.Local
        public OptionalObjectListDeserializer
        (PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup
          , byte version)
            : base(property)
        {
            this.version = version;

            recycler = orxDeserializerLookup.Recycler;

            itemDeserializer = new OrxByteDeserializer<To>(orxDeserializerLookup, version);
        }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            var arraySize = StreamByteOps.ToUShort(ref ptr);
            if (arraySize != ushort.MaxValue)
            {
                var objectList = recycler.Borrow<List<To>>();
                objectList.Clear();
                for (var i = 0; i < arraySize; i++)
                {
                    uint size = StreamByteOps.ToUShort(ref ptr);
                    objectList.Add((To)itemDeserializer.Deserialize(ptr, size, version));
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

    private class MandatoryObjectArrayDeserializer<To> : OptionalObjectArrayDeserializer<To>
        where To : class, new()
    {
        public MandatoryObjectArrayDeserializer
        (PropertyInfo property,
            IOrxDeserializerLookup orxDeserializerLookup, byte version)
            : base(property, orxDeserializerLookup, version) { }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            int ignoredArrayId   = StreamByteOps.ToUShort(ref ptr);
            int ignoredEntrySize = StreamByteOps.ToUShort(ref ptr);
            base.Deserialize(message, ref ptr);
        }
    }

    private class MandatoryObjectListDeserializer<To> : OptionalObjectListDeserializer<To>
        where To : class, new()
    {
        public MandatoryObjectListDeserializer
            (PropertyInfo property, IOrxDeserializerLookup orxDeserializerLookup, byte version)
            : base(property, orxDeserializerLookup, version) { }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            int ignoredArrayId   = StreamByteOps.ToUShort(ref ptr);
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
            Set(message, *ptr++ != 0);
        }
    }

    private sealed class UInt8Deserializer : Deserializer<byte>
    {
        public UInt8Deserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            Set(message, *ptr++);
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
        private readonly IRecycler recycler;

        public MutableStringDeserializer(PropertyInfo property, IRecycler recycler)
            : base(property) =>
            this.recycler = recycler;

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            Set(message
              , StreamByteOps.ToMutableStringWithSizeHeader(ref ptr, recycler.Borrow<MutableString>()));
        }
    }

    private sealed class DecimalDeserializer : Deserializer<decimal>
    {
        public DecimalDeserializer(PropertyInfo property)
            : base(property) { }

        public override unsafe void Deserialize(Tm message, ref byte* ptr)
        {
            var factor = *ptr++;
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
