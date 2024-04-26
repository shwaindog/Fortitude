#region

using System.Collections;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IDeserializedNotifier : IStoreState<IDeserializedNotifier>, ICloneable<IDeserializedNotifier>
{
    bool RemoveOnZeroSubscribers { get; set; }
    string Name { get; }
    int SubscriberCount { get; }
    Action? Unsubscribe { get; set; }
    Type NotifyingType { get; }
    IReceiverListenContext? this[string name] { get; set; }
    void AddRequestExpected(int requestId, IAsyncResponseSource responseValueSource);

    void RegisterMessageDeserializer(INotifyingMessageDeserializer notifyingMessageDeserializer);
}

public interface IDeserializedNotifier<TM, TR> : IDeserializedNotifier, IEnumerable<IReceiverListenContext<TR>>
    where TM : class, IVersionedMessage, new()
{
    bool Add(IReceiverListenContext<TR> receiverListenContext);
    void AddRequestExpected(int requestId, IReusableAsyncResponseSource<TR> responseValueTaskSource);
    void RegisterMessageDeserializer(INotifyingMessageDeserializer<TM> notifyingMessageDeserializer);
}

[Flags]
public enum DeserializeTypeFlags
{
    None = 0
    , DeserializeConversation = 1
    , DeserializeBuffer = 2
}

public abstract class DeserializedNotifierBase<TM> : IDeserializedNotifier where TM : class, IVersionedMessage, new()
{
    protected readonly bool DeserializedIsResponseMessage;
    protected readonly DeserializeTypeFlags DeserializeType;

    protected readonly IMap<int, IAsyncResponseSource>
        ExpectedRequestResponses = new ConcurrentMap<int, IAsyncResponseSource>();

    private int subscriberCount = 1;


    protected DeserializedNotifierBase(string registrationLocation)
    {
        Name = registrationLocation;
        DeserializedIsResponseMessage = typeof(TM).GetInterfaces().Any(t => t == typeof(IResponseMessage));
    }

    protected DeserializedNotifierBase(string registrationLocation, DeserializeTypeFlags deserializeType) : this(registrationLocation)
    {
        DeserializeType = deserializeType;
        Name = registrationLocation;
    }

    protected DeserializedNotifierBase(DeserializedNotifierBase<TM> toClone)
    {
        DeserializedIsResponseMessage = toClone.DeserializedIsResponseMessage;
        DeserializeType = toClone.DeserializeType;
        ExpectedRequestResponses = toClone.ExpectedRequestResponses.Clone();
        subscriberCount = toClone.subscriberCount;
        Name = toClone.Name;
    }

    public abstract ConversationMessageReceivedHandler<TM>? AttachToDeserializerConversationHandler { get; }

    public abstract MessageDeserializedHandler<TM>? AttachToDeserializerMessageHandler { get; }

    public abstract IReceiverListenContext? this[string name] { get; set; }

    public Type NotifyingType => typeof(TM);

    public string Name { get; }

    public int SubscriberCount
    {
        get => subscriberCount;
        set
        {
            if (subscriberCount == value) return;
            subscriberCount = value;
            RegistrationCountChanged?.Invoke(this, value);
        }
    }

    public Action? Unsubscribe { get; set; }
    public bool RemoveOnZeroSubscribers { get; set; } = true;

    public void AddRequestExpected(int requestId, IAsyncResponseSource responseValueSource)
    {
        if (DeserializedIsResponseMessage && responseValueSource.ResponseType == typeof(TM))
        {
            ExpectedRequestResponses.Add(requestId, responseValueSource);
            SubscriberCount++;
        }
        else
        {
            throw new ArgumentException(
                "Attempting to add a responseValueSource that is either the wrong type expected or the message can not source a request Id ");
        }
    }

    public void RegisterMessageDeserializer(INotifyingMessageDeserializer notifyingMessageDeserializer)
    {
        if (notifyingMessageDeserializer is INotifyingMessageDeserializer<TM> typedNotifyingMessageDeserializer)
            RegisterMessageDeserializer(typedNotifyingMessageDeserializer);
        else
            throw new ArgumentException(
                $"Expected to received a INotifyingMessageDeserializer<{typeof(TM).Name}> but got {notifyingMessageDeserializer.GetType()}");
    }

    object ICloneable.Clone() => Clone();

    public abstract IDeserializedNotifier Clone();

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) => CopyFrom((IDeserializedNotifier)source, copyMergeFlags);

    public abstract IDeserializedNotifier CopyFrom(IDeserializedNotifier source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    public event Action<IDeserializedNotifier, int>? RegistrationCountChanged;

    public void RegisterMessageDeserializer(INotifyingMessageDeserializer<TM> notifyingMessageDeserializer)
    {
        RegistrationCountChanged += notifyingMessageDeserializer.OnNotifierSubscribeCountChanged;
        Unsubscribe += () => { RegistrationCountChanged -= notifyingMessageDeserializer.OnNotifierSubscribeCountChanged; };
        if (AttachToDeserializerConversationHandler != null)
        {
            notifyingMessageDeserializer.ConversationMessageDeserialized += AttachToDeserializerConversationHandler;
            Unsubscribe += () =>
            {
                RegistrationCountChanged -= notifyingMessageDeserializer.OnNotifierSubscribeCountChanged;
                notifyingMessageDeserializer.ConversationMessageDeserialized -= AttachToDeserializerConversationHandler;
            };
        }

        if (AttachToDeserializerMessageHandler != null)
        {
            notifyingMessageDeserializer.MessageDeserialized += AttachToDeserializerMessageHandler;
            Unsubscribe += () => { notifyingMessageDeserializer.ConversationMessageDeserialized -= AttachToDeserializerConversationHandler; };
        }
    }
}

public class PassThroughDeserializedNotifier<TM> : DeserializedNotifierBase<TM>, IDeserializedNotifier<TM, TM>
    , IStoreState<PassThroughDeserializedNotifier<TM>>
    where TM : class, IVersionedMessage, new()
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PassThroughDeserializedNotifier<TM>));
    private readonly IMap<string, IReceiverListenContext<TM>> registeredReceiverContexts = new ConcurrentMap<string, IReceiverListenContext<TM>>();
    private ConversationMessageReceivedHandler<TM>? receiverConversationMessageReceivedHandler;
    private MessageDeserializedHandler<TM>? receiverMessageDeserializedHandler;

    public PassThroughDeserializedNotifier(string registrationLocation, ConversationMessageReceivedHandler<TM> conversationMessageReceived)
        : base(registrationLocation) =>
        receiverConversationMessageReceivedHandler = conversationMessageReceived;

    public PassThroughDeserializedNotifier(string registrationLocation, MessageDeserializedHandler<TM> messageDeserializedHandler)
        : base(registrationLocation) =>
        receiverMessageDeserializedHandler = messageDeserializedHandler;

    public PassThroughDeserializedNotifier(string registrationLocation, DeserializeTypeFlags deserializeType
        , params IReceiverListenContext<TM>[] registerAllReceiverListenContexts) :
        base(registrationLocation, deserializeType)
    {
        foreach (var receiverListenContext in registerAllReceiverListenContexts)
            registeredReceiverContexts.Add(receiverListenContext.Name, receiverListenContext);
    }

    public PassThroughDeserializedNotifier(PassThroughDeserializedNotifier<TM> toClone) : base(toClone)
    {
        receiverConversationMessageReceivedHandler = toClone.receiverConversationMessageReceivedHandler;
        receiverMessageDeserializedHandler = toClone.receiverMessageDeserializedHandler;
        foreach (var kvpRReceiverContexts in toClone.registeredReceiverContexts)
            registeredReceiverContexts.Add(kvpRReceiverContexts.Key, kvpRReceiverContexts.Value.Clone());
    }

    public override ConversationMessageReceivedHandler<TM>? AttachToDeserializerConversationHandler =>
        receiverConversationMessageReceivedHandler != null
        || (DeserializeType & DeserializeTypeFlags.DeserializeConversation) > 0 ?
            ConversationMessageDeserialized :
            null;

    public override MessageDeserializedHandler<TM>? AttachToDeserializerMessageHandler =>
        receiverConversationMessageReceivedHandler != null
        || (DeserializeType & DeserializeTypeFlags.DeserializeBuffer) > 0 ?
            MessageDeserialized :
            null;

    public override IReceiverListenContext? this[string name]
    {
        get => registeredReceiverContexts[name];
        set
        {
            var oldSubscriberCount = registeredReceiverContexts.Count;
            if (value != null)
                registeredReceiverContexts.AddOrUpdate(name, (IReceiverListenContext<TM>)value);
            else
                registeredReceiverContexts.Remove(name);
            SubscriberCount += registeredReceiverContexts.Count - oldSubscriberCount;
        }
    }

    public bool Add(IReceiverListenContext<TM> receiverListenContext)
    {
        var oldReceiverListenCount = registeredReceiverContexts.Count;
        var wasAdded = registeredReceiverContexts.Add(receiverListenContext.Name, receiverListenContext);
        SubscriberCount += registeredReceiverContexts.Count - oldReceiverListenCount;
        return wasAdded;
    }

    public void AddRequestExpected(int requestId, IReusableAsyncResponseSource<TM> responseValueTaskSource)
    {
        if (DeserializedIsResponseMessage)
            ExpectedRequestResponses.Add(requestId, responseValueTaskSource);
        else
            throw new ArgumentException("Attempting to add a responseValueTaskSource to a message that can not source a request Id ");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IReceiverListenContext<TM>> GetEnumerator() => registeredReceiverContexts.Values.GetEnumerator();

    public override IDeserializedNotifier Clone() => new PassThroughDeserializedNotifier<TM>(this);

    public override IDeserializedNotifier CopyFrom(IDeserializedNotifier source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((PassThroughDeserializedNotifier<TM>)source, copyMergeFlags);

    public PassThroughDeserializedNotifier<TM> CopyFrom(PassThroughDeserializedNotifier<TM> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if ((copyMergeFlags & CopyMergeFlags.RemoveUnmatched) > 0) registeredReceiverContexts.Clear();

        receiverConversationMessageReceivedHandler = source.receiverConversationMessageReceivedHandler;
        receiverMessageDeserializedHandler = source.receiverMessageDeserializedHandler;
        foreach (var kvpReceiverContexts in source.registeredReceiverContexts)
            if ((copyMergeFlags & CopyMergeFlags.FullReplace) == 0)
            {
                IReceiverListenContext<TM>? preExisting = null;
                if ((copyMergeFlags & CopyMergeFlags.JustDifferences) > 0)
                {
                    if (!registeredReceiverContexts.TryGetValue(kvpReceiverContexts.Key, out preExisting))
                        registeredReceiverContexts.Add(kvpReceiverContexts.Key, kvpReceiverContexts.Value.Clone());
                    else
                        preExisting!.CopyFrom(kvpReceiverContexts.Value);
                }

                if ((copyMergeFlags & CopyMergeFlags.AppendMissing) > 0)
                    if (!registeredReceiverContexts.TryGetValue(kvpReceiverContexts.Key, out preExisting))
                        registeredReceiverContexts.Add(kvpReceiverContexts.Key, kvpReceiverContexts.Value.Clone());
            }
            else
            {
                registeredReceiverContexts.AddOrUpdate(kvpReceiverContexts.Key, kvpReceiverContexts.Value.Clone());
            }

        return this;
    }

    private void ConversationMessageDeserialized(TM message, MessageHeader messageHeader, IConversation conversation)
    {
        if (DeserializedIsResponseMessage && message is IResponseMessage responseMessage)
            if (ExpectedRequestResponses.TryGetValue(responseMessage.RequestId, out var taskSource))
                try
                {
                    if (taskSource is IReusableAsyncResponseSource<TM> typedAsyncResponseSource)
                        typedAsyncResponseSource!.SetResult(message);
                    ExpectedRequestResponses.Remove(responseMessage.RequestId);

                    SubscriberCount--;
                    // TODO add taskSource decrement timer
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Warn("Caught exception attempt to set response callback to for message {0}. Got {1}", responseMessage, ex);
                }

        if (registeredReceiverContexts.Any())
            foreach (var receiverListenContext in registeredReceiverContexts.Values)
            {
                var messageConversation = new ConversationMessageNotification<TM>(message, messageHeader, conversation);
                receiverListenContext.SendToReceiver(messageConversation);
            }
        else
            receiverConversationMessageReceivedHandler?.Invoke(message, messageHeader, conversation);
    }

    private void MessageDeserialized(TM message, IBufferContext bufferContext)
    {
        if (registeredReceiverContexts.Any())
            foreach (var receiverListenContext in registeredReceiverContexts.Values)
                receiverListenContext.SendToReceiver(message);
        else
            receiverMessageDeserializedHandler?.Invoke(message, bufferContext);
    }
}

public delegate void ConvertedConversationMessageReceivedHandler<in TM>(TM deserializedMessage, MessageHeader header, IConversation conversation);

public delegate void ConvertedMessageDeserializedHandler<in TM>(TM deserializedMessage, IBufferContext bufferContext);

public class ConvertingDeserializedNotifier<TM, TR> : DeserializedNotifierBase<TM>, IDeserializedNotifier<TM, TR>
    , IStoreState<ConvertingDeserializedNotifier<TM, TR>>
    where TM : class, IVersionedMessage, new()
{
    private static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(ConvertingDeserializedNotifier<TM, TR>));

    private readonly bool convertedIsResponseMessage;
    private readonly IMap<string, IReceiverListenContext<TR>> registeredReceiverContexts = new ConcurrentMap<string, IReceiverListenContext<TR>>();

    private ConvertedConversationMessageReceivedHandler<TR>? receiverConversationMessageReceivedHandler;
    private ConvertedMessageDeserializedHandler<TR>? receiverMessageDeserializedHandler;

    public ConvertingDeserializedNotifier(string registrationLocation, IConverter<TM, TR> converter
        , ConvertedConversationMessageReceivedHandler<TR> conversationMessageReceived) : base(registrationLocation)
    {
        convertedIsResponseMessage = typeof(TR).GetInterfaces().Any(t => t == typeof(IResponseMessage));
        Converter = converter;
        receiverConversationMessageReceivedHandler = conversationMessageReceived;
    }

    public ConvertingDeserializedNotifier(string registrationLocation, IConverter<TM, TR> converter
        , ConvertedMessageDeserializedHandler<TR> messageDeserializedHandler) : base(registrationLocation)
    {
        convertedIsResponseMessage = typeof(TR).GetInterfaces().Any(t => t == typeof(IResponseMessage));
        Converter = converter;
        receiverMessageDeserializedHandler = messageDeserializedHandler;
    }

    public ConvertingDeserializedNotifier(string registrationLocation, IConverter<TM, TR> converter, DeserializeTypeFlags deserializeType
        , params IReceiverListenContext<TR>[] registerAllReceiverListenContexts) : base(registrationLocation, deserializeType)
    {
        convertedIsResponseMessage = typeof(TR).GetInterfaces().Any(t => t == typeof(IResponseMessage));
        Converter = converter;
        foreach (var receiverListenContext in registerAllReceiverListenContexts)
            registeredReceiverContexts.Add(receiverListenContext.Name, receiverListenContext);
    }

    public ConvertingDeserializedNotifier(ConvertingDeserializedNotifier<TM, TR> toClone) : base(toClone)
    {
        convertedIsResponseMessage = toClone.convertedIsResponseMessage;
        receiverConversationMessageReceivedHandler = toClone.receiverConversationMessageReceivedHandler;
        receiverMessageDeserializedHandler = toClone.receiverMessageDeserializedHandler;
        foreach (var kvpRReceiverContexts in toClone.registeredReceiverContexts)
            registeredReceiverContexts.Add(kvpRReceiverContexts.Key, kvpRReceiverContexts.Value.Clone());
        registeredReceiverContexts = toClone.registeredReceiverContexts.Clone();
        Converter = toClone.Converter;
    }

    private IConverter<TM, TR> Converter { get; }

    public override ConversationMessageReceivedHandler<TM>? AttachToDeserializerConversationHandler =>
        receiverConversationMessageReceivedHandler != null
        || (DeserializeType & DeserializeTypeFlags.DeserializeConversation) > 0 ?
            ConversationMessageDeserialized :
            null;

    public override MessageDeserializedHandler<TM>? AttachToDeserializerMessageHandler =>
        receiverConversationMessageReceivedHandler != null
        || (DeserializeType & DeserializeTypeFlags.DeserializeBuffer) > 0 ?
            MessageDeserialized :
            null;

    public override IReceiverListenContext? this[string name]
    {
        get => registeredReceiverContexts[name];
        set
        {
            var oldSubscriberCount = registeredReceiverContexts.Count;
            if (value != null)
                registeredReceiverContexts.AddOrUpdate(name, (IReceiverListenContext<TR>)value);
            else
                registeredReceiverContexts.Remove(name);
            SubscriberCount += registeredReceiverContexts.Count - oldSubscriberCount;
        }
    }

    public bool Add(IReceiverListenContext<TR> receiverListenContext)
    {
        var oldReceiverListenCount = registeredReceiverContexts.Count;
        var wasAdded = registeredReceiverContexts.Add(receiverListenContext.Name, receiverListenContext);
        SubscriberCount += registeredReceiverContexts.Count - oldReceiverListenCount;
        return wasAdded;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IReceiverListenContext<TR>> GetEnumerator() => registeredReceiverContexts.Values.GetEnumerator();

    public void AddRequestExpected(int requestId, IReusableAsyncResponseSource<TR> responseValueTaskSource)
    {
        if (DeserializedIsResponseMessage || convertedIsResponseMessage)
        {
            ExpectedRequestResponses.Add(requestId, responseValueTaskSource);
            SubscriberCount++;
        }
        else
        {
            throw new ArgumentException("Attempting to add a responseValueTaskSource to a message that can not source a request Id ");
        }
    }

    public override IDeserializedNotifier Clone() => new ConvertingDeserializedNotifier<TM, TR>(this);

    public override IDeserializedNotifier CopyFrom(IDeserializedNotifier source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((ConvertingDeserializedNotifier<TM, TR>)source, copyMergeFlags);

    public ConvertingDeserializedNotifier<TM, TR> CopyFrom(ConvertingDeserializedNotifier<TM, TR> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if ((copyMergeFlags & CopyMergeFlags.RemoveUnmatched) > 0) registeredReceiverContexts.Clear();

        receiverConversationMessageReceivedHandler = source.receiverConversationMessageReceivedHandler;
        receiverMessageDeserializedHandler = source.receiverMessageDeserializedHandler;
        foreach (var kvpReceiverContexts in source.registeredReceiverContexts)
            if ((copyMergeFlags & CopyMergeFlags.FullReplace) == 0)
            {
                IReceiverListenContext<TR>? preExisting = null;
                if ((copyMergeFlags & CopyMergeFlags.JustDifferences) > 0)
                {
                    if (!registeredReceiverContexts.TryGetValue(kvpReceiverContexts.Key, out preExisting))
                        registeredReceiverContexts.Add(kvpReceiverContexts.Key, kvpReceiverContexts.Value.Clone());
                    else
                        preExisting!.CopyFrom(kvpReceiverContexts.Value);
                }

                if ((copyMergeFlags & CopyMergeFlags.AppendMissing) > 0)
                    if (!registeredReceiverContexts.TryGetValue(kvpReceiverContexts.Key, out preExisting))
                        registeredReceiverContexts.Add(kvpReceiverContexts.Key, kvpReceiverContexts.Value.Clone());
            }
            else
            {
                registeredReceiverContexts.AddOrUpdate(kvpReceiverContexts.Key, kvpReceiverContexts.Value.Clone());
            }

        return this;
    }

    private void ConversationMessageDeserialized(TM message, MessageHeader messageHeader, IConversation conversation)
    {
        var convertedMessage = Converter.Convert(message);
        if (DeserializedIsResponseMessage && message is IResponseMessage deserializedResponseMessage)
            if (ExpectedRequestResponses.TryGetValue(deserializedResponseMessage.RequestId, out var taskSource))
                if (taskSource is IReusableAsyncResponseSource<TR> typedAsyncResponseSource)
                    try
                    {
                        typedAsyncResponseSource!.SetResult(convertedMessage);
                        ExpectedRequestResponses.Remove(deserializedResponseMessage.RequestId);
                        SubscriberCount--;
                        // TODO add taskSource decrement timer
                        return;
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Caught exception attempt to set response callback to for message {0}. Got {1}", deserializedResponseMessage, ex);
                    }

        if (convertedIsResponseMessage && convertedMessage is IResponseMessage convertedResponseMessage)
            if (ExpectedRequestResponses.TryGetValue(convertedResponseMessage.RequestId, out var taskSource))
                if (taskSource is IReusableAsyncResponseSource<TR> typedAsyncResponseSource)
                {
                    typedAsyncResponseSource!.SetResult(convertedMessage);
                    ExpectedRequestResponses.Remove(convertedResponseMessage.RequestId);
                    // TODO add taskSource decrement timer
                    return;
                }

        if (registeredReceiverContexts.Any())
        {
            var messageConversation = new ConversationMessageNotification<TR>(convertedMessage, messageHeader, conversation);
            foreach (var receiverListenContext in registeredReceiverContexts.Values) receiverListenContext.SendToReceiver(messageConversation);
        }
        else
        {
            receiverConversationMessageReceivedHandler?.Invoke(convertedMessage, messageHeader, conversation);
        }
    }

    private void MessageDeserialized(TM message, IBufferContext bufferContext)
    {
        var convertedMessage = Converter.Convert(message);
        if (registeredReceiverContexts.Any())
            foreach (var receiverListenContext in registeredReceiverContexts.Values)
                receiverListenContext.SendToReceiver(convertedMessage);
        else
            receiverMessageDeserializedHandler?.Invoke(convertedMessage, bufferContext);
    }
}
