#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary.Deserialization;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageDeserializationRepository : IMessageSerdesRepository, IStoreState<IMessageDeserializationRepository>
{
    string Name { get; }
    IEnumerable<KeyValuePair<uint, IMessageDeserializer>> AllRegisteredDeserializers { get; }

    IMessageDeserializationRepository? CascadingFallbackDeserializationRepo { get; set; }

    INotifyingMessageDeserializer<TM>? RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM>? messageDeserializer = null
        , bool forceOverride = false)
        where TM : class, IVersionedMessage, new();

    IMessageDeserializer RegisterDeserializer(uint msgId, IMessageDeserializer messageDeserializer, bool forceOverride = false);
    bool UnregisterDeserializer(uint msgId);

    IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializersOfType(Type messageType);

    bool TryGetDeserializer(uint msgId, out IMessageDeserializer? messageDeserializer);
    IMessageDeserializer? GetDeserializer(uint msgId);
    IEnumerable<uint> GetRegisteredMessageIds(IMessageDeserializer messageDeserializer);
    INotifyingMessageDeserializer<TM>? GetDeserializer<TM>(uint msgId) where TM : class, IVersionedMessage, new();
    bool IsRegisteredWithType<TS, TM>(uint msgId) where TS : INotifyingMessageDeserializer<TM> where TM : class, IVersionedMessage, new();

    event Action<IMessageDeserializer>? MessageDeserializerRegistered;
    event Action<IMessageDeserializer>? MessageDeserializerUnregistered;
}

public class MessageDeserializationRepository : IMessageDeserializationRepository
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MessageDeserializationRepository));

    protected readonly IRecycler Recycler;
    protected readonly IMap<uint, IMessageDeserializer> RegisteredDeserializers = new ConcurrentMap<uint, IMessageDeserializer>();

    public MessageDeserializationRepository(string name, IRecycler recycler
        , IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null)
    {
        Recycler = recycler;
        Name = name;
        CascadingFallbackDeserializationRepo = cascadingFallbackDeserializationRepo;
    }

    public IMessageDeserializationRepository? CascadingFallbackDeserializationRepo { get; set; }

    public IEnumerable<uint> RegisteredMessageIds => RegisteredDeserializers.Keys;
    public bool IsRegistered(uint msgId) => RegisteredDeserializers.ContainsKey(msgId);

    public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializersOfType(Type messageType) =>
        AllRegisteredDeserializers.Where(kvp => kvp.Value.MessageType == messageType);

    public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> AllRegisteredDeserializers =>
        RegisteredDeserializers
            .Concat(CascadingFallbackDeserializationRepo?.AllRegisteredDeserializers
                        .Where(kvp => !RegisteredDeserializers.ContainsKey(kvp.Key)) ??
                    Enumerable.Empty<KeyValuePair<uint, IMessageDeserializer>>());

    public IEnumerable<uint> GetRegisteredMessageIds(IMessageDeserializer messageDeserializer) =>
        AllRegisteredDeserializers.Where(kvp => kvp.Value == messageDeserializer).Select(kvp => kvp.Key);

    public string Name { get; }

    public virtual INotifyingMessageDeserializer<TM>? RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM>? messageDeserializer = null
        , bool forceOverride = false)
        where TM : class, IVersionedMessage, new()
    {
        var instanceOfTypeToSerialize = Recycler.Borrow<TM>();
        var msgId = instanceOfTypeToSerialize.MessageId;
        instanceOfTypeToSerialize.DecrementRefCount();
        return RegisterDeserializer(msgId, messageDeserializer!, forceOverride) as INotifyingMessageDeserializer<TM>;
    }

    public IMessageDeserializer RegisterDeserializer(uint msgId, IMessageDeserializer messageDeserializer, bool forceOverride = false)
    {
        if ((messageDeserializer.RegisteredRepository != null && messageDeserializer.RegisteredRepository != this) ||
            (messageDeserializer.RegisteredForMessageId != null && messageDeserializer.RegisteredForMessageId != msgId))
            Logger.Warn(
                "Attempting to register a MessageDeserializer {0} with MessageId {1} that is registered already registered on another repository or with a different MessageId"
                ,
                messageDeserializer, msgId);
        messageDeserializer.RegisteredForMessageId = msgId;
        messageDeserializer.RegisteredRepository = this;
        if (!RegisteredDeserializers.TryGetValue(msgId, out var existingMessageDeserializer))
        {
            if (forceOverride || CascadingFallbackDeserializationRepo == null ||
                !CascadingFallbackDeserializationRepo.IsRegistered(msgId))
            {
                RegisteredDeserializers.Add(msgId, messageDeserializer);
                MessageDeserializerRegistered?.Invoke(messageDeserializer);
                return messageDeserializer;
            }

            return messageDeserializer;
        }

        if (forceOverride)
        {
            if (existingMessageDeserializer != null)
            {
                MessageDeserializerUnregistered?.Invoke(existingMessageDeserializer);
                existingMessageDeserializer.RegisteredForMessageId = null;
                existingMessageDeserializer.RegisteredRepository = null;
            }

            RegisteredDeserializers.AddOrUpdate(msgId, messageDeserializer);
            MessageDeserializerRegistered?.Invoke(messageDeserializer);
            return messageDeserializer;
        }

        if (RegisteredDeserializers.Add(msgId, messageDeserializer))
        {
            MessageDeserializerRegistered?.Invoke(messageDeserializer);
            return messageDeserializer;
        }

        return RegisteredDeserializers.GetValue(msgId)!;
    }


    public bool UnregisterDeserializer(uint msgId)
    {
        if (RegisteredDeserializers.TryGetValue(msgId, out var existing))
        {
            RegisteredDeserializers.Remove(msgId);
            MessageDeserializerUnregistered?.Invoke(existing!);
            existing!.RegisteredForMessageId = null;
            existing.RegisteredRepository = null;
            return true;
        }

        return false;
    }

    public bool TryGetDeserializer(uint msgId, out IMessageDeserializer? messageDeserializer) =>
        RegisteredDeserializers.TryGetValue(msgId, out messageDeserializer) ||
        (CascadingFallbackDeserializationRepo?.TryGetDeserializer(msgId, out messageDeserializer) ?? false);

    public IMessageDeserializer? GetDeserializer(uint msgId) =>
        RegisteredDeserializers.TryGetValue(msgId, out var msgDeserializer) ?
            msgDeserializer :
            CascadingFallbackDeserializationRepo?.GetDeserializer(msgId);

    public INotifyingMessageDeserializer<TM>? GetDeserializer<TM>(uint msgId) where TM : class, IVersionedMessage, new() =>
        GetDeserializer(msgId) as INotifyingMessageDeserializer<TM>;

    public IMessageDeserializationRepository CopyFrom(IMessageDeserializationRepository source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if ((copyMergeFlags & CopyMergeFlags.RemoveUnmatched) > 0) RegisteredDeserializers.Clear();
        foreach (var kvpMessageDeserializerEntry in source.AllRegisteredDeserializers)
            if ((copyMergeFlags & CopyMergeFlags.FullReplace) == 0)
            {
                if ((copyMergeFlags & CopyMergeFlags.JustDifferences) > 0)
                {
                    if (!TryGetDeserializer(kvpMessageDeserializerEntry.Key, out var preExisting))
                    {
                        var messageDeserializer = kvpMessageDeserializerEntry.Value.Clone();
                        messageDeserializer.RegisteredRepository = this;
                        RegisterDeserializer(kvpMessageDeserializerEntry.Key, messageDeserializer);
                    }
                    else
                    {
                        preExisting!.CopyFrom(kvpMessageDeserializerEntry.Value);
                    }
                }

                if ((copyMergeFlags & CopyMergeFlags.AppendMissing) > 0)
                    if (!TryGetDeserializer(kvpMessageDeserializerEntry.Key, out var preExisting))
                    {
                        var messageDeserializer = kvpMessageDeserializerEntry.Value.Clone();
                        messageDeserializer.RegisteredRepository = this;
                        RegisterDeserializer(kvpMessageDeserializerEntry.Key, messageDeserializer);
                        MessageDeserializerRegistered?.Invoke(messageDeserializer);
                    }
            }
            else
            {
                var messageDeserializer = kvpMessageDeserializerEntry.Value.Clone();
                messageDeserializer.RegisteredRepository = this;
                RegisterDeserializer(kvpMessageDeserializerEntry.Key, messageDeserializer);
            }

        return this;
    }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IMessageDeserializationRepository)source, copyMergeFlags);

    public IMessageSerdesRepository CopyFrom(IMessageSerdesRepository source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IMessageDeserializationRepository)source, copyMergeFlags);

    public bool IsRegisteredWithType<TS, TM>(uint msgId) where TS : INotifyingMessageDeserializer<TM> where TM : class, IVersionedMessage, new() =>
        RegisteredDeserializers.TryGetValue(msgId, out var msgSerializer) ?
            msgSerializer is TS :
            CascadingFallbackDeserializationRepo?.IsRegisteredWithType<TS, TM>(msgId) ?? false;

    public event Action<IMessageDeserializer>? MessageDeserializerRegistered;
    public event Action<IMessageDeserializer>? MessageDeserializerUnregistered;

    protected void OnMessageDeserializerRegistered(IMessageDeserializer messageDeserializer)
    {
        MessageDeserializerRegistered?.Invoke(messageDeserializer);
    }
}

public interface IMessageStreamDecoderFactory
{
    IMessageStreamDecoder Supply(string name);
}

public interface IMessageDeserializerFactoryRepository : IMessageDeserializationRepository, IMessageStreamDecoderFactory
{
    INotifyingMessageDeserializer<TM>? SourceNotifyingMessageDeserializerFromMessageId<TM>(uint msgId) where TM : class, IVersionedMessage, new();
    IMessageDeserializer<TM>? SourceTypedMessageDeserializerFromMessageId<TM>(uint msgId) where TM : class, IVersionedMessage, new();
    IMessageDeserializer? SourceDeserializerFromMessageId(uint msgId, Type messageType);
    uint? ResolveExpectedMessageIdForMessageType(Type messageType);
}

public abstract class MessageDeserializationFactoryRepository : MessageDeserializationRepository, IMessageDeserializerFactoryRepository
{
    protected MessageDeserializationFactoryRepository(string name, IRecycler recycler
        , IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) :
        base(name, recycler, cascadingFallbackDeserializationRepo)
    {
        RegisterDeserializer(RequesterNameMessage.RequesterNameMessageId, new RequesterNameMessageDeserializer(recycler));
        RegisterDeserializer(ExpectSessionCloseMessage.ExpectSessionCloseMessageId, new ExpectSessionCloseMessageDeserializer(recycler));
    }

    protected IMessageDeserializerFactoryRepository? CascadingFallbackDeserializationFactoryRepo =>
        CascadingFallbackDeserializationRepo as IMessageDeserializerFactoryRepository;

    public abstract IMessageStreamDecoder Supply(string name);

    public override INotifyingMessageDeserializer<TM>? RegisterDeserializer<TM>(INotifyingMessageDeserializer<TM>? messageDeserializer = null
        , bool forceOverride = false)
    {
        var instanceOfTypeToSerialize = Recycler.Borrow<TM>();
        var msgId = instanceOfTypeToSerialize.MessageId;
        instanceOfTypeToSerialize.DecrementRefCount();
        if (!RegisteredDeserializers.TryGetValue(msgId, out var existingMessageDeserializer))
        {
            if (forceOverride || CascadingFallbackDeserializationRepo == null ||
                !CascadingFallbackDeserializationRepo.IsRegisteredWithType<INotifyingMessageDeserializer<TM>, TM>(msgId))
            {
                var sourcedMsgDeserializer = messageDeserializer ?? SourceTypedMessageDeserializerFromMessageId<TM>(msgId);
                if (sourcedMsgDeserializer == null) return null;
                RegisterDeserializer(msgId, sourcedMsgDeserializer);
                return sourcedMsgDeserializer as INotifyingMessageDeserializer<TM>;
            }

            return CascadingFallbackDeserializationRepo.GetDeserializer<TM>(msgId);
        }

        if (existingMessageDeserializer as INotifyingMessageDeserializer<TM> == null)
            throw new Exception("Two different message types cannot be registered to the same Id");
        if (messageDeserializer == null) return (INotifyingMessageDeserializer<TM>)existingMessageDeserializer;
        RegisterDeserializer(msgId, messageDeserializer, forceOverride);
        return GetDeserializer<TM>(msgId)!;
    }

    public abstract INotifyingMessageDeserializer<TM>? SourceNotifyingMessageDeserializerFromMessageId<TM>(uint msgId)
        where TM : class, IVersionedMessage, new();

    public abstract IMessageDeserializer<TM>? SourceTypedMessageDeserializerFromMessageId<TM>(uint msgId) where TM : class, IVersionedMessage, new();

    public abstract IMessageDeserializer? SourceDeserializerFromMessageId(uint msgId, Type messageType);

    public virtual uint? ResolveExpectedMessageIdForMessageType(Type messageType) =>
        CascadingFallbackDeserializationFactoryRepo?.ResolveExpectedMessageIdForMessageType(messageType);
}

public interface IConversationDeserializationRepository : IMessageDeserializerFactoryRepository
{
    INotifyingMessageDeserializer<TM> RegisterDeserializer<TM>(bool forceOverride = false)
        where TM : class, IVersionedMessage, new();
}

public abstract class ConversationRepository : MessageDeserializationFactoryRepository, IConversationDeserializationRepository
{
    public ConversationRepository(string name, IRecycler recycler
        , IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) :
        base(name, recycler, cascadingFallbackDeserializationRepo) { }

    public INotifyingMessageDeserializer<TM> RegisterDeserializer<TM>(bool forceOverride = false) where TM : class, IVersionedMessage, new()
    {
        var instanceOfTypeToDeserialize = Recycler.Borrow<TM>();
        var msgId = instanceOfTypeToDeserialize.MessageId;
        instanceOfTypeToDeserialize.DecrementRefCount();
        return RegisterDeserializer<TM>(msgId, forceOverride);
    }

    public INotifyingMessageDeserializer<TM> RegisterDeserializer<TM>(uint msgId, bool forceOverride = false)
        where TM : class, IVersionedMessage, new()
    {
        if (!RegisteredDeserializers.TryGetValue(msgId, out var existingMessageDeserializer))
        {
            var sourceNewMessageDeserializer
                = existingMessageDeserializer as INotifyingMessageDeserializer<TM> ?? SourceTypedMessageDeserializerFromMessageId<TM>(msgId);
            if (sourceNewMessageDeserializer != null)
            {
                RegisterDeserializer(msgId, sourceNewMessageDeserializer, forceOverride);
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
        return resolvedDeserializer;
    }
}
