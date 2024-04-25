#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageDeserializer : IStoreState<IMessageDeserializer>, ICloneable<IMessageDeserializer>
{
    Type MessageType { get; }
    IMessageDeserializationRepository? RegisteredRepository { get; set; }
    object? Deserialize(IBufferContext socketBufferReadContext);
}

public interface IMessageDeserializer<out TM> : IMessageDeserializer, IDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    new TM? Deserialize(IBufferContext bufferContext);
}

public abstract class BinaryMessageDeserializer<TM> : IMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
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
    where TM : class, IVersionedMessage, new();

public delegate void MessageDeserializedHandler<in TM>(TM deserializedMessage, IBufferContext bufferContext)
    where TM : class, IVersionedMessage, new();

public interface INotifyingMessageDeserializer : IMessageDeserializer
{
    IEnumerable<IDeserializedNotifier> AllDeserializedNotifiers { get; }
    IDeserializedNotifier? this[string name] { get; set; }
    bool RemoveOnZeroNotifiers { get; set; }
    void OnNotifierSubscribeCountChanged(IDeserializedNotifier changeDeserializedNotifier, int count);
}

public interface INotifyingMessageDeserializer<TM> : IMessageDeserializer<TM>, INotifyingMessageDeserializer
    , IStoreState<INotifyingMessageDeserializer<TM>>
    where TM : class, IVersionedMessage, new()
{
    event ConversationMessageReceivedHandler<TM>? ConversationMessageDeserialized;
    event MessageDeserializedHandler<TM>? MessageDeserialized;
    bool IsRegistered(MessageDeserializedHandler<TM> deserializedHandler);
    bool IsRegistered(ConversationMessageReceivedHandler<TM> deserializedHandler);
    IDeserializedNotifier<TM, TR>? AddDeserializedNotifier<TR>(IDeserializedNotifier<TM, TR> deserializedNotifier);
}

public abstract class MessageDeserializer<TM> : BinaryMessageDeserializer<TM>, INotifyingMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    private readonly IMap<string, IDeserializedNotifier> registeredNotifiers = new ConcurrentMap<string, IDeserializedNotifier>();

    protected MessageDeserializer() { }

    protected MessageDeserializer(MessageDeserializer<TM> toClone)
    {
        foreach (var cloneRegisteredNotifier in toClone.registeredNotifiers)
            registeredNotifiers.Add(cloneRegisteredNotifier.Key, cloneRegisteredNotifier.Value.Clone());
    }

    object? IMessageDeserializer.Deserialize(IBufferContext bufferContext) => Deserialize(bufferContext);
    public bool RemoveOnZeroNotifiers { get; set; }

    public IEnumerable<IDeserializedNotifier> AllDeserializedNotifiers => registeredNotifiers.Values;

    public bool IsRegistered(ConversationMessageReceivedHandler<TM> deserializedHandler)
    {
        return ConversationMessageDeserialized != null && ConversationMessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);
    }

    public bool IsRegistered(MessageDeserializedHandler<TM> deserializedHandler)
    {
        return MessageDeserialized != null && MessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);
    }

    public event ConversationMessageReceivedHandler<TM>? ConversationMessageDeserialized;

    public event MessageDeserializedHandler<TM>? MessageDeserialized;

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

    protected void OnNotify(TM data, IBufferContext bufferContext)
    {
        MessageDeserialized?.Invoke(data, bufferContext);
        if (bufferContext is ISocketBufferReadContext socketBufferReadContext)
            ConversationMessageDeserialized?.Invoke(data, socketBufferReadContext.MessageHeader, socketBufferReadContext.Conversation!);
    }
}
