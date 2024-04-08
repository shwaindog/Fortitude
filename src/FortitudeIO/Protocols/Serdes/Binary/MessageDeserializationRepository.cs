#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageDeserializationRepository : IMessageSerdesRepository
{
    bool RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM>? messageDeserializer = null) where TM : class, IVersionedMessage, new();
    bool RegisterDeserializer(uint msgId, IMessageDeserializer messageDeserializer);
    bool UnregisterDeserializer(uint msgId);

    bool TryGetDeserializer(uint msgId, out IMessageDeserializer? messageDeserializer);
    IMessageDeserializer? GetDeserializer(uint msgId);
    INotifyingMessageDeserializer<TM>? GetDeserializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
    bool IsRegisteredWithType<TS, TM>(uint msgId) where TS : INotifyingMessageDeserializer<TM> where TM : class, IVersionedMessage, new();
}

public class MessageDeserializationRepository : IMessageDeserializationRepository
{
    protected readonly IMessageDeserializationRepository? CascadingFallbackDeserializationRepo;
    protected readonly IRecycler Recycler;
    protected readonly IMap<uint, IMessageDeserializer> RegisteredDeserializers = new ConcurrentMap<uint, IMessageDeserializer>();

    protected MessageDeserializationRepository(IRecycler recycler, IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null)
    {
        Recycler = recycler;
        CascadingFallbackDeserializationRepo = cascadingFallbackDeserializationRepo;
    }

    public IEnumerable<uint> RegisteredMessageIds => RegisteredDeserializers.Keys;
    public bool IsRegistered(uint msgId) => RegisteredDeserializers.ContainsKey(msgId);

    public virtual bool RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM>? messageDeserializer = null)
        where TM : class, IVersionedMessage, new()
    {
        var instanceOfTypeToSerialize = Recycler.Borrow<TM>();
        var msgId = instanceOfTypeToSerialize.MessageId;
        instanceOfTypeToSerialize.DecrementRefCount();
        return RegisterDeserializer(msgId, messageDeserializer!);
    }

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
}

public interface IMessageStreamDecoderFactory
{
    IMessageStreamDecoder Supply();
}

public abstract class FactoryDeserializationRepository : MessageDeserializationRepository, IMessageStreamDecoderFactory
{
    protected FactoryDeserializationRepository(IRecycler recycler
        , IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) : base(recycler, cascadingFallbackDeserializationRepo) { }

    public abstract IMessageStreamDecoder Supply();

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

public interface IConversationDeserializationRepository : IMessageDeserializationRepository
{
    bool RegisterDeserializer<TM>(Action<TM, object?, IConversation?>? msgHandler) where TM : class, IVersionedMessage, new();
}

public abstract class ConversationDeserializationRepository : FactoryDeserializationRepository, IConversationDeserializationRepository
{
    public ConversationDeserializationRepository(IRecycler recycler
        , IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) :
        base(recycler, cascadingFallbackDeserializationRepo) { }

    public bool RegisterDeserializer<T>(Action<T, object?, IConversation?>? msgHandler)
        where T : class, IVersionedMessage, new()
    {
        var instanceOfTypeToDeserialize = Recycler.Borrow<T>();
        var msgId = instanceOfTypeToDeserialize.MessageId;
        instanceOfTypeToDeserialize.DecrementRefCount();
        return RegisterDeserializer(msgId, msgHandler);
    }

    public bool RegisterDeserializer<TM>(uint msgId
        , Action<TM, object?, IConversation?>? msgHandler)
        where TM : class, IVersionedMessage, new()
    {
        if (msgHandler == null)
            throw new Exception("Message Handler cannot be null");
        if (!RegisteredDeserializers.TryGetValue(msgId, out var existingMessageDeserializer))
        {
            var sourceNewMessageDeserializer
                = existingMessageDeserializer as INotifyingMessageDeserializer<TM> ?? SourceMessageDeserializer<TM>(msgId);
            if (sourceNewMessageDeserializer != null)
            {
                RegisterDeserializer(msgId, sourceNewMessageDeserializer);
                existingMessageDeserializer = sourceNewMessageDeserializer;
            }
            else
            {
                throw new Exception($"Could not source MessageDeserializer for {nameof(TM)}");
            }
        }

        INotifyingMessageDeserializer<TM>? resolvedDeserializer = existingMessageDeserializer as INotifyingMessageDeserializer<TM>;
        if (resolvedDeserializer == null)
            throw new Exception("Two different message types cannot be registered to the same Id");
        if (resolvedDeserializer.IsRegistered(msgHandler)) throw new Exception("Message Handler already registered");

        resolvedDeserializer.ConversationMessageDeserialized += msgHandler;
        return true;
    }
}
