#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageSerdesRepositoryFactory
{
    IMessageSerializationRepository MessageSerializationRepository { get; }

    IMessageStreamDecoderFactory MessageStreamDecoderFactory { get; }

    IMessageDeserializationRepository MessageDeserializationRepository { get; }
}

public interface IMessageSerdesRepository
{
    IEnumerable<uint> RegisteredMessageIds { get; }
    bool IsRegistered(uint msgId);
}

public interface IMessageSerializationRepository : IMessageSerdesRepository
{
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

    protected abstract IMessageSerializer? SourceMessageSerializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
}
