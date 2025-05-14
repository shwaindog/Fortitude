// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageDeserializer : ITransferState<IMessageDeserializer>, ICloneable<IMessageDeserializer>
{
    int   InstanceNumber         { get; }
    uint? RegisteredForMessageId { get; set; }

    bool ReadMessageHeader { get; set; }
    Type MessageType       { get; }

    IMessageDeserializationRepository? RegisteredRepository { get; set; }

    object? Deserialize(IBufferContext socketBufferReadContext);
}

public interface IMessageDeserializer<out TM> : IMessageDeserializer, IDeserializer<TM>
    where TM : IVersionedMessage
{
    new TM? Deserialize(IBufferContext bufferContext);
}

public abstract class BinaryMessageDeserializer<TM> : IMessageDeserializer<TM>
    where TM : IVersionedMessage
{
    private static int lastInstanceNumber;

    public virtual bool ReadMessageHeader { get; set; }

    public uint? RegisteredForMessageId { get; set; }
    public Type  MessageType            => typeof(TM);
    public int   InstanceNumber         { get; } = Interlocked.Increment(ref lastInstanceNumber);

    public MarshalType MarshalType => MarshalType.Binary;

    public IMessageDeserializationRepository? RegisteredRepository { get; set; }

    public virtual ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) => this;


    public virtual IMessageDeserializer CopyFrom(IMessageDeserializer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => this;

    object ICloneable.                   Clone() => Clone();
    public abstract IMessageDeserializer Clone();

    public abstract TM?          Deserialize(ISerdeContext readContext);
    TM? IMessageDeserializer<TM>.Deserialize(IBufferContext bufferContext)           => Deserialize(bufferContext);
    object? IMessageDeserializer.Deserialize(IBufferContext socketBufferReadContext) => Deserialize(socketBufferReadContext);
}

public delegate void ConversationMessageReceivedHandler<in TM>(TM deserializedMessage, MessageHeader header, IConversation conversation)
    where TM : IVersionedMessage;

public delegate void MessageDeserializedHandler<in TM>(TM deserializedMessage, int deserializedCount, IMessageDeserializer deserializer)
    where TM : IVersionedMessage;

public interface INotifyingMessageDeserializer : IMessageDeserializer
{
    IEnumerable<IDeserializedNotifier> AllDeserializedNotifiers { get; }
    IDeserializedNotifier? this[string name] { get; set; }
    bool RemoveOnZeroNotifiers { get; set; }

    event ConversationMessageReceivedHandler<IVersionedMessage>? ConversationMessageDeserialized;
    event MessageDeserializedHandler<IVersionedMessage>?         MessageDeserialized;

    bool IsRegistered(MessageDeserializedHandler<IVersionedMessage> deserializedHandler);
    bool IsRegistered(ConversationMessageReceivedHandler<IVersionedMessage> deserializedHandler);
    void OnNotifierSubscribeCountChanged(IDeserializedNotifier changeDeserializedNotifier, int count);
}

public interface INotifyingMessageDeserializer<TM> : IMessageDeserializer<TM>, INotifyingMessageDeserializer
  , ITransferState<INotifyingMessageDeserializer<TM>>
    where TM : IVersionedMessage
{
    new event ConversationMessageReceivedHandler<TM>? ConversationMessageDeserialized;
    new event MessageDeserializedHandler<TM>?         MessageDeserialized;

    bool IsRegistered(MessageDeserializedHandler<TM> deserializedHandler);
    bool IsRegistered(ConversationMessageReceivedHandler<TM> deserializedHandler);

    IDeserializedNotifier<TM, TR>? AddDeserializedNotifier<TR>(IDeserializedNotifier<TM, TR> deserializedNotifier);
}

public abstract class MessageDeserializer<TM> : BinaryMessageDeserializer<TM>, INotifyingMessageDeserializer<TM>
    where TM : IVersionedMessage
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MessageDeserializer<TM>));

    private readonly IMap<string, IDeserializedNotifier> registeredNotifiers = new ConcurrentMap<string, IDeserializedNotifier>();

    private int messageDeserializedCounter = 1;

    private ConversationMessageReceivedHandler<IVersionedMessage>? untypedRegisteredConversationsCallbacks;
    private MessageDeserializedHandler<IVersionedMessage>?         untypedRegisteredMessageCallbacks;

    protected MessageDeserializer() { }

    protected MessageDeserializer(MessageDeserializer<TM> toClone)
    {
        foreach (var cloneRegisteredNotifier in toClone.registeredNotifiers)
            registeredNotifiers.Add(cloneRegisteredNotifier.Key, cloneRegisteredNotifier.Value.Clone());
    }

    public bool RemoveOnZeroNotifiers { get; set; }

    public IEnumerable<IDeserializedNotifier> AllDeserializedNotifiers => registeredNotifiers.Values;

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

    object? IMessageDeserializer.Deserialize(IBufferContext bufferContext) => Deserialize(bufferContext);

    public bool IsRegistered(MessageDeserializedHandler<IVersionedMessage> deserializedHandler) =>
        ConversationMessageDeserialized != null
     && ConversationMessageDeserialized.GetInvocationList()
                                       .Any(del => del.Target == deserializedHandler.Target &&
                                                   del.Method == deserializedHandler.Method);

    public bool IsRegistered(ConversationMessageReceivedHandler<IVersionedMessage> deserializedHandler) =>
        ConversationMessageDeserialized != null
     && ConversationMessageDeserialized.GetInvocationList()
                                       .Any(del => del.Target == deserializedHandler.Target &&
                                                   del.Method == deserializedHandler.Method);

    public bool IsRegistered(ConversationMessageReceivedHandler<TM> deserializedHandler) =>
        ConversationMessageDeserialized != null
     && ConversationMessageDeserialized.GetInvocationList()
                                       .Any(del => del.Target == deserializedHandler.Target &&
                                                   del.Method == deserializedHandler.Method);

    public bool IsRegistered(MessageDeserializedHandler<TM> deserializedHandler) =>
        MessageDeserialized != null
     && MessageDeserialized.GetInvocationList()
                           .Any(del => del.Target == deserializedHandler.Target &&
                                       del.Method == deserializedHandler.Method);

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

    public override ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((INotifyingMessageDeserializer<TM>)source, copyMergeFlags);

    public override IMessageDeserializer CopyFrom(IMessageDeserializer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((INotifyingMessageDeserializer<TM>)source, copyMergeFlags);

    public INotifyingMessageDeserializer<TM> CopyFrom
    (INotifyingMessageDeserializer<TM> source
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
                    if (preExisting == null) this[sourceAllDeserializedNotifier.Name] = sourceAllDeserializedNotifier.Clone();
                }
            }
            else
            {
                registeredNotifiers.AddOrUpdate(sourceAllDeserializedNotifier.Name, sourceAllDeserializedNotifier.Clone());
            }

        return this;
    }

    protected unsafe MessageHeader ReadHeader(ref byte* ptr)
    {
        var version      = *ptr++;
        var messageFlags = *ptr++;
        var messageId    = StreamByteOps.ToUInt(ref ptr);
        var messageSize  = StreamByteOps.ToUInt(ref ptr);

        return new MessageHeader(version, messageFlags, messageId, messageSize);
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
