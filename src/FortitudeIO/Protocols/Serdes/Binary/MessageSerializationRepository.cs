#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageSerdesRepositoryFactory
{
    IMessageSerializationRepository MessageSerializationRepository { get; }

    IMessageStreamDecoderFactory MessageStreamDecoderFactory(string name);

    IMessageDeserializationRepository MessageDeserializationRepository(string name);
}

public interface IMessageSerdesRepository : IStoreState<IMessageSerdesRepository>
{
    IEnumerable<uint> RegisteredMessageIds { get; }
    bool IsRegistered(uint msgId);
}

public interface IMessageSerializationRepository : IMessageSerdesRepository, IStoreState<IMessageSerializationRepository>
{
    IEnumerable<KeyValuePair<uint, IMessageSerializer>> AllRegisteredSerializers { get; }
    bool RegisterSerializer<TM>(IMessageSerializer<TM>? messageSerializer = null) where TM : class, IVersionedMessage, new();
    bool RegisterSerializer(uint msgId, IMessageSerializer messageSerializer);
    bool UnregisterSerializer(uint msgId);
    bool TryGetSerializer(uint msgId, out IMessageSerializer? messageSerializer);
    IMessageSerializer? GetSerializer(uint msgId);
    IMessageSerializer<TM>? GetSerializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
    bool IsRegisteredWithType<TS, TM>(uint msgId) where TS : IMessageSerializer<TM> where TM : class, IVersionedMessage, new();
}

public abstract class FactorySerializationRepository(IRecycler recycler
        , IMessageSerializationRepository? cascadingFallbackMsgSerializationRepo = null)
    : IMessageSerializationRepository
{
    private readonly IMap<uint, IMessageSerializer> registeredSerializers = new ConcurrentMap<uint, IMessageSerializer>();

    public IEnumerable<uint> RegisteredMessageIds => registeredSerializers.Keys;

    public IEnumerable<KeyValuePair<uint, IMessageSerializer>> AllRegisteredSerializers => registeredSerializers;
    public bool IsRegistered(uint msgId) => registeredSerializers.ContainsKey(msgId);

    public bool UnregisterSerializer(uint msgId) => registeredSerializers.Remove(msgId);

    public bool RegisterSerializer<TM>(IMessageSerializer<TM>? messageSerializer = null) where TM : class, IVersionedMessage, new()
    {
        var instanceOfTypeToSerialize = recycler.Borrow<TM>();
        var msgId = instanceOfTypeToSerialize.MessageId;
        if (!registeredSerializers.TryGetValue(msgId, out var existingMessageSerializer))
        {
            if (cascadingFallbackMsgSerializationRepo == null ||
                !cascadingFallbackMsgSerializationRepo.IsRegisteredWithType<IMessageSerializer<TM>, TM>(msgId))
            {
                var sourcedMessageSerializer = messageSerializer ?? SourceMessageSerializer<TM>(msgId);
                if (sourcedMessageSerializer == null) return false;
                registeredSerializers.Add(msgId, sourcedMessageSerializer);
                return true;
            }

            return cascadingFallbackMsgSerializationRepo.IsRegistered(msgId);
        }

        if (existingMessageSerializer is not IMessageSerializer<TM>)
            throw new Exception("Two different message types cannot be registered to the same Id");
        if (messageSerializer != null)
        {
            registeredSerializers.AddOrUpdate(msgId, messageSerializer);
            return true;
        }

        return true;
    }

    public bool RegisterSerializer(uint msgId, IMessageSerializer messageSerializer)
    {
        if (!registeredSerializers.TryGetValue(msgId, out _))
        {
            if (cascadingFallbackMsgSerializationRepo == null ||
                !cascadingFallbackMsgSerializationRepo.IsRegistered(msgId))
            {
                registeredSerializers.Add(msgId, messageSerializer);
                return true;
            }

            return cascadingFallbackMsgSerializationRepo.IsRegistered(msgId);
        }

        registeredSerializers.AddOrUpdate(msgId, messageSerializer);
        return true;
    }

    public bool TryGetSerializer(uint msgId, out IMessageSerializer? messageSerializer) =>
        registeredSerializers.TryGetValue(msgId, out messageSerializer) ||
        (cascadingFallbackMsgSerializationRepo?.TryGetSerializer(msgId, out messageSerializer) ?? false);

    public IMessageSerializer? GetSerializer(uint msgId) =>
        registeredSerializers.TryGetValue(msgId, out var msgSerializer) ? msgSerializer : cascadingFallbackMsgSerializationRepo?.GetSerializer(msgId);

    public IMessageSerializer<TM>? GetSerializer<TM>(uint msgId) where TM : class, IVersionedMessage, new() =>
        GetSerializer(msgId) as IMessageSerializer<TM>;

    public bool IsRegisteredWithType<TS, TM>(uint msgId) where TS : IMessageSerializer<TM> where TM : class, IVersionedMessage, new() =>
        registeredSerializers.TryGetValue(msgId, out var msgSerializer) ?
            msgSerializer is TS :
            cascadingFallbackMsgSerializationRepo?.IsRegisteredWithType<TS, TM>(msgId) ?? false;

    public IMessageSerializationRepository CopyFrom(IMessageSerializationRepository source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if ((copyMergeFlags & CopyMergeFlags.RemoveUnmatched) > 0) registeredSerializers.Clear();
        foreach (var kvpMessageDeserializerEntry in source.AllRegisteredSerializers)
            if ((copyMergeFlags & CopyMergeFlags.AppendMissing) > 0)
            {
                if (!TryGetSerializer(kvpMessageDeserializerEntry.Key, out var preExisting))
                    registeredSerializers.Add(kvpMessageDeserializerEntry.Key, kvpMessageDeserializerEntry.Value);
            }
            else
            {
                registeredSerializers.AddOrUpdate(kvpMessageDeserializerEntry.Key, kvpMessageDeserializerEntry.Value);
            }

        return this;
    }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) => throw new NotImplementedException();

    public IMessageSerdesRepository CopyFrom(IMessageSerdesRepository source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        throw new NotImplementedException();

    protected abstract IMessageSerializer? SourceMessageSerializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
}
