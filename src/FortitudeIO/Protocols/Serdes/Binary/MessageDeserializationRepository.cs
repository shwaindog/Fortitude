#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageDeserializationRepository : IMessageSerdesRepository
{
    bool RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM> messageDeserializer) where TM : class, IVersionedMessage, new();
    bool RegisterDeserializer(uint msgId, IMessageDeserializer messageDeserializer);
    bool UnregisterDeserializer(uint msgId);

    bool TryGetDeserializer(uint msgId, out IMessageDeserializer? messageDeserializer);
    IMessageDeserializer? GetDeserializer(uint msgId);
    INotifyingMessageDeserializer<TM>? GetDeserializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
    bool IsRegisteredWithType<TS, TM>(uint msgId) where TS : INotifyingMessageDeserializer<TM> where TM : class, IVersionedMessage, new();
    IMessageStreamDecoder Supply();
}

public abstract class DeserializationRepositoryBase : IMessageDeserializationRepository
{
    protected readonly IMessageDeserializationRepository? CascadingFallbackDeserializationRepo;
    protected readonly IMap<uint, IMessageDeserializer> RegisteredDeserializers = new ConcurrentMap<uint, IMessageDeserializer>();

    protected DeserializationRepositoryBase(IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) =>
        CascadingFallbackDeserializationRepo = cascadingFallbackDeserializationRepo;

    public IEnumerable<uint> RegisteredMessageIds => RegisteredDeserializers.Keys;
    public bool IsRegistered(uint msgId) => RegisteredDeserializers.ContainsKey(msgId);

    public abstract bool RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM>? messageDeserializer = null)
        where TM : class, IVersionedMessage, new();

    public bool RegisterDeserializer(uint msgId, IMessageDeserializer messageDeserializer)
    {
        if (!RegisteredDeserializers.TryGetValue(msgId, out var existingMessageDeserializer))
        {
            if (CascadingFallbackDeserializationRepo == null ||
                !CascadingFallbackDeserializationRepo.IsRegistered(msgId))
            {
                RegisteredDeserializers.Add(msgId, messageDeserializer);
                return true;
            }

            return CascadingFallbackDeserializationRepo.IsRegistered(msgId);
        }

        RegisteredDeserializers.AddOrUpdate(msgId, messageDeserializer);
        return true;
    }

    public bool UnregisterDeserializer(uint msgId) => RegisteredDeserializers.Remove(msgId);

    public bool TryGetDeserializer(uint msgId, out IMessageDeserializer? messageDeserializer) =>
        RegisteredDeserializers.TryGetValue(msgId, out messageDeserializer) ||
        (CascadingFallbackDeserializationRepo?.TryGetDeserializer(msgId, out messageDeserializer) ?? false);

    public IMessageDeserializer? GetDeserializer(uint msgId) =>
        RegisteredDeserializers.TryGetValue(msgId, out var msgDeserializer) ?
            msgDeserializer :
            CascadingFallbackDeserializationRepo?.GetDeserializer(msgId);

    public INotifyingMessageDeserializer<TM>? GetDeserializer<TM>(uint msgId) where TM : class, IVersionedMessage, new() =>
        GetDeserializer(msgId) as INotifyingMessageDeserializer<TM>;

    public bool IsRegisteredWithType<TS, TM>(uint msgId) where TS : INotifyingMessageDeserializer<TM> where TM : class, IVersionedMessage, new() =>
        RegisteredDeserializers.TryGetValue(msgId, out var msgSerializer) ?
            msgSerializer is TS :
            CascadingFallbackDeserializationRepo?.IsRegisteredWithType<TS, TM>(msgId) ?? false;

    public abstract IMessageStreamDecoder Supply();
}

public abstract class FactoryDeserializationRepository : DeserializationRepositoryBase, IMessageDeserializationRepository
{
    protected readonly IRecycler Recycler;

    protected FactoryDeserializationRepository(IRecycler recycler
        , IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) : base(cascadingFallbackDeserializationRepo) =>
        Recycler = recycler;

    public override bool RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM>? messageDeserializer = null)
    {
        var instanceOfTypeToSerialize = Recycler.Borrow<TM>();
        var msgId = instanceOfTypeToSerialize.MessageId;
        instanceOfTypeToSerialize.DecrementRefCount();
        if (!RegisteredDeserializers.TryGetValue(msgId, out var existingMessageDeserializer))
        {
            if (CascadingFallbackDeserializationRepo == null ||
                !CascadingFallbackDeserializationRepo.IsRegisteredWithType<INotifyingMessageDeserializer<TM>, TM>(msgId))
            {
                var sourcedMsgDeserializer = messageDeserializer ?? SourceMessageDeserializer<TM>(msgId);
                if (sourcedMsgDeserializer == null) return false;
                RegisteredDeserializers.Add(msgId, sourcedMsgDeserializer);
                return true;
            }

            return CascadingFallbackDeserializationRepo.IsRegistered(msgId);
        }

        if (existingMessageDeserializer as INotifyingMessageDeserializer<TM> == null)
            throw new Exception("Two different message types cannot be registered to the same Id");
        if (messageDeserializer != null)
        {
            RegisteredDeserializers.AddOrUpdate(msgId, messageDeserializer);
            return true;
        }

        return false;
    }

    protected abstract IMessageDeserializer? SourceMessageDeserializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
}
