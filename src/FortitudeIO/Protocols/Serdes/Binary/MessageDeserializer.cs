#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageDeserializer : IStoreState<IMessageDeserializer>, ICloneable<IMessageDeserializer>
{
    int InstanceNumber { get; }
    uint? RegisteredForMessageId { get; set; }
    Type MessageType { get; }
    IMessageDeserializationRepository? RegisteredRepository { get; set; }
    object? Deserialize(IBufferContext socketBufferReadContext);
}

public interface IMessageDeserializer<out TM> : IMessageDeserializer, IDeserializer<TM>
    where TM : class, IVersionedMessage
{
    new TM? Deserialize(IBufferContext bufferContext);
}

public abstract class BinaryMessageDeserializer<TM> : IMessageDeserializer<TM>
    where TM : class, IVersionedMessage
{
    private static int lastInstanceNumber;

    public int InstanceNumber { get; } = Interlocked.Increment(ref lastInstanceNumber);

    public uint? RegisteredForMessageId { get; set; }
    public virtual IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) => this;
    public MarshalType MarshalType => MarshalType.Binary;
    public virtual IMessageDeserializer CopyFrom(IMessageDeserializer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => this;

    object ICloneable.Clone() => Clone();

    public abstract IMessageDeserializer Clone();
    public abstract TM? Deserialize(ISerdeContext readContext);
    TM? IMessageDeserializer<TM>.Deserialize(IBufferContext bufferContext) => Deserialize(bufferContext);
    public Type MessageType => typeof(TM);
    public IMessageDeserializationRepository? RegisteredRepository { get; set; }
    object? IMessageDeserializer.Deserialize(IBufferContext socketBufferReadContext) => Deserialize(socketBufferReadContext);
}

public delegate void ConversationMessageReceivedHandler<in TM>(TM deserializedMessage, MessageHeader header, IConversation conversation)
    where TM : class, IVersionedMessage;

public delegate void MessageDeserializedHandler<in TM>(TM deserializedMessage, int deserializedCount, IMessageDeserializer deserializer)
    where TM : class, IVersionedMessage;

public interface INotifyingMessageDeserializer : IMessageDeserializer
{
    IEnumerable<IDeserializedNotifier> AllDeserializedNotifiers { get; }
    IDeserializedNotifier? this[string name] { get; set; }
    bool RemoveOnZeroNotifiers { get; set; }
    event ConversationMessageReceivedHandler<IVersionedMessage>? ConversationMessageDeserialized;
    event MessageDeserializedHandler<IVersionedMessage>? MessageDeserialized;

    bool IsRegistered(MessageDeserializedHandler<IVersionedMessage> deserializedHandler);
    bool IsRegistered(ConversationMessageReceivedHandler<IVersionedMessage> deserializedHandler);
    void OnNotifierSubscribeCountChanged(IDeserializedNotifier changeDeserializedNotifier, int count);
}

public interface INotifyingMessageDeserializer<TM> : IMessageDeserializer<TM>, INotifyingMessageDeserializer
    , IStoreState<INotifyingMessageDeserializer<TM>>
    where TM : class, IVersionedMessage
{
    new event ConversationMessageReceivedHandler<TM>? ConversationMessageDeserialized;
    new event MessageDeserializedHandler<TM>? MessageDeserialized;
    bool IsRegistered(MessageDeserializedHandler<TM> deserializedHandler);
    bool IsRegistered(ConversationMessageReceivedHandler<TM> deserializedHandler);
    IDeserializedNotifier<TM, TR>? AddDeserializedNotifier<TR>(IDeserializedNotifier<TM, TR> deserializedNotifier);
}

public abstract class MessageDeserializer<TM> : BinaryMessageDeserializer<TM>, INotifyingMessageDeserializer<TM>
    where TM : class, IVersionedMessage
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MessageDeserializer<TM>));
    private readonly IMap<string, IDeserializedNotifier> registeredNotifiers = new ConcurrentMap<string, IDeserializedNotifier>();
    private int messageDeserializedCounter = 1;

    private ConversationMessageReceivedHandler<IVersionedMessage>? untypedRegisteredConversationsCallbacks;
    private MessageDeserializedHandler<IVersionedMessage>? untypedRegisteredMessageCallbacks;

    protected MessageDeserializer() { }

    protected MessageDeserializer(MessageDeserializer<TM> toClone)
    {
        foreach (var cloneRegisteredNotifier in toClone.registeredNotifiers)
            registeredNotifiers.Add(cloneRegisteredNotifier.Key, cloneRegisteredNotifier.Value.Clone());
    }

    object? IMessageDeserializer.Deserialize(IBufferContext bufferContext) => Deserialize(bufferContext);
    public bool RemoveOnZeroNotifiers { get; set; }

    public IEnumerable<IDeserializedNotifier> AllDeserializedNotifiers => registeredNotifiers.Values;

    public bool IsRegistered(MessageDeserializedHandler<IVersionedMessage> deserializedHandler) =>
        ConversationMessageDeserialized != null && ConversationMessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);

    public bool IsRegistered(ConversationMessageReceivedHandler<IVersionedMessage> deserializedHandler) =>
        ConversationMessageDeserialized != null && ConversationMessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);

    public bool IsRegistered(ConversationMessageReceivedHandler<TM> deserializedHandler) =>
        ConversationMessageDeserialized != null && ConversationMessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);

    public bool IsRegistered(MessageDeserializedHandler<TM> deserializedHandler) =>
        MessageDeserialized != null && MessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);

    public event ConversationMessageReceivedHandler<TM>? ConversationMessageDeserialized;

    public event MessageDeserializedHandler<TM>? MessageDeserialized;

    event ConversationMessageReceivedHandler<IVersionedMessage>? INotifyingMessageDeserializer.ConversationMessageDeserialized
    {
        add
        {
            if (untypedRegisteredConversationsCallbacks == null) ConversationMessageDeserialized += TypedConversationToUntypedInvoker;
            untypedRegisteredConversationsCallbacks += value;
        }
        remove
        {
            untypedRegisteredConversationsCallbacks -= value;
            if (untypedRegisteredConversationsCallbacks == null) ConversationMessageDeserialized -= TypedConversationToUntypedInvoker;
        }
    }

    event MessageDeserializedHandler<IVersionedMessage>? INotifyingMessageDeserializer.MessageDeserialized
    {
        add
        {
            if (untypedRegisteredMessageCallbacks == null) MessageDeserialized += TypedMessageToUntypedInvoker;
            untypedRegisteredMessageCallbacks += value;
        }
        remove
        {
            untypedRegisteredMessageCallbacks -= value;
            if (untypedRegisteredMessageCallbacks == null) MessageDeserialized -= TypedMessageToUntypedInvoker;
        }
    }

    public IDeserializedNotifier? this[string name]
    {
        get => registeredNotifiers.TryGetValue(name, out var deserializedNotifier) ? deserializedNotifier : null;
        set
        {
            if (registeredNotifiers.TryGetValue(name, out var preexistingNotifier))
            {
                if (value == preexistingNotifier) return;
                preexistingNotifier?.Unsubscribe?.Invoke();
            }

            registeredNotifiers.AddOrUpdate(name, value!);
            value?.RegisterMessageDeserializer(this);
        }
    }

    public IDeserializedNotifier<TM, TR> AddDeserializedNotifier<TR>(IDeserializedNotifier<TM, TR> deserializedNotifier)
    {
        registeredNotifiers.Add(deserializedNotifier.Name, deserializedNotifier);
        deserializedNotifier.RegisterMessageDeserializer(this);
        return deserializedNotifier;
    }

    public void OnNotifierSubscribeCountChanged(IDeserializedNotifier changeDeserializedNotifier, int count)
    {
        if (count > 0 || !changeDeserializedNotifier.RemoveOnZeroSubscribers) return;
        changeDeserializedNotifier.Unsubscribe?.Invoke();
        registeredNotifiers.Remove(changeDeserializedNotifier.Name);
        if (!RemoveOnZeroNotifiers || RegisteredRepository == null || AllDeserializedNotifiers.Any()) return;
        foreach (var msgId in RegisteredRepository.GetRegisteredMessageIds(this)) RegisteredRepository.UnregisterDeserializer(msgId);
    }

    public override IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((INotifyingMessageDeserializer<TM>)source, copyMergeFlags);

    public override IMessageDeserializer CopyFrom(IMessageDeserializer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((INotifyingMessageDeserializer<TM>)source, copyMergeFlags);

    public INotifyingMessageDeserializer<TM> CopyFrom(INotifyingMessageDeserializer<TM> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if ((copyMergeFlags & CopyMergeFlags.RemoveUnmatched) > 0) registeredNotifiers.Clear();

        foreach (var sourceAllDeserializedNotifier in source.AllDeserializedNotifiers)
            if ((copyMergeFlags & CopyMergeFlags.FullReplace) == 0)
            {
                IDeserializedNotifier? preExisting;
                if ((copyMergeFlags & CopyMergeFlags.JustDifferences) > 0)
                {
                    preExisting = this[sourceAllDeserializedNotifier.Name];
                    if (preExisting == null)
                        this[sourceAllDeserializedNotifier.Name] = sourceAllDeserializedNotifier.Clone();
                    else
                        preExisting.CopyFrom(sourceAllDeserializedNotifier);
                }

                if ((copyMergeFlags & CopyMergeFlags.AppendMissing) > 0)
                {
                    preExisting = this[sourceAllDeserializedNotifier.Name];
                    if (preExisting == null)
                        this[sourceAllDeserializedNotifier.Name] = sourceAllDeserializedNotifier.Clone();
                }
            }
            else
            {
                registeredNotifiers.AddOrUpdate(sourceAllDeserializedNotifier.Name, sourceAllDeserializedNotifier.Clone());
            }

        return this;
    }

    private void TypedConversationToUntypedInvoker(TM deserializedMessage, MessageHeader header, IConversation conversation)
    {
        untypedRegisteredConversationsCallbacks?.Invoke(deserializedMessage, header, conversation);
    }

    private void TypedMessageToUntypedInvoker(TM deserializedMessage, int deserializedCount, IMessageDeserializer deserializer)
    {
        untypedRegisteredMessageCallbacks?.Invoke(deserializedMessage, deserializedCount, deserializer);
    }

    protected void OnNotify(TM message)
    {
        messageDeserializedCounter++;
        MessageDeserialized?.Invoke(message, messageDeserializedCounter, this);
    }

    protected void OnNotify(TM message, IBufferContext bufferContext)
    {
        messageDeserializedCounter++;
        MessageDeserialized?.Invoke(message, messageDeserializedCounter, this);
        if (bufferContext is ISocketBufferReadContext socketBufferReadContext)
            ConversationMessageDeserialized?.Invoke(message, socketBufferReadContext.MessageHeader, socketBufferReadContext.Conversation!);
    }
}
