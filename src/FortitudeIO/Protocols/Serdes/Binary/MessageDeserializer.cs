#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageDeserializer
{
    object? Deserialize(IBufferContext socketBufferReadContext);
}

public interface IMessageDeserializer<out TM> : IMessageDeserializer, IDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    new TM? Deserialize(IBufferContext bufferContext);
}

public delegate void ConversationMessageReceivedHandler<in TM>(TM deserializedMessage, MessageHeader header, IConversation conversation)
    where TM : class, IVersionedMessage, new();

public delegate void MessageDeserializedHandler<in TM>(TM deserializedMessage, IBufferContext bufferContext)
    where TM : class, IVersionedMessage, new();

public interface INotifyingMessageDeserializer<TM> : IMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    IEnumerable<IDeserializedNotifier> AllDeserializedNotifiers { get; }
    event ConversationMessageReceivedHandler<TM>? ConversationMessageDeserialized;
    event MessageDeserializedHandler<TM>? MessageDeserialized;

    bool IsRegistered(Action<TM, IBufferContext> deserializedHandler);
    bool IsRegistered(ConversationMessageReceivedHandler<TM> deserializedHandler);
    IDeserializedNotifier? FindDeserializedNotifier(string id);
    IDeserializedNotifier<TM, TR>? AddDeserializedNotifier<TR>(IDeserializedNotifier<TM, TR> deserializedNotifier);
    int DecrementDeserializedNotifierUsage(string id);
    int IncrementDeserializedNotifierUsage(string id);
}

public abstract class MessageDeserializer<TM> : INotifyingMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    private readonly IMap<string, IDeserializedNotifier> registeredNotifiers = new ConcurrentMap<string, IDeserializedNotifier>();


    object? IMessageDeserializer.Deserialize(IBufferContext bufferContext) => Deserialize(bufferContext);

    public MarshalType MarshalType => MarshalType.Binary;
    public abstract TM? Deserialize(ISerdeContext readContext);

    public IEnumerable<IDeserializedNotifier> AllDeserializedNotifiers => registeredNotifiers.Values;

    TM? IMessageDeserializer<TM>.Deserialize(IBufferContext bufferContext) => Deserialize(bufferContext);

    public bool IsRegistered(ConversationMessageReceivedHandler<TM> deserializedHandler)
    {
        return ConversationMessageDeserialized != null && ConversationMessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);
    }

    public bool IsRegistered(Action<TM, IBufferContext> deserializedHandler)
    {
        return MessageDeserialized != null && MessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);
    }

    public event ConversationMessageReceivedHandler<TM>? ConversationMessageDeserialized;

    public event MessageDeserializedHandler<TM>? MessageDeserialized;

    public IDeserializedNotifier? FindDeserializedNotifier(string id) =>
        registeredNotifiers.TryGetValue(id, out var deserializedNotifier) ? deserializedNotifier : null;

    public IDeserializedNotifier<TM, TR>? AddDeserializedNotifier<TR>(IDeserializedNotifier<TM, TR> deserializedNotifier)
    {
        registeredNotifiers.Add(deserializedNotifier.Id, deserializedNotifier);
        if (deserializedNotifier.AttachToDeserializerConversationHandler != null)
        {
            ConversationMessageDeserialized += deserializedNotifier.AttachToDeserializerConversationHandler;
            deserializedNotifier.Unsubscribe += () =>
            {
                ConversationMessageDeserialized -= deserializedNotifier.AttachToDeserializerConversationHandler;
            };
        }

        if (deserializedNotifier.AttachToDeserializerMessageHandler != null)
        {
            MessageDeserialized += deserializedNotifier.AttachToDeserializerMessageHandler;
            deserializedNotifier.Unsubscribe += () =>
            {
                ConversationMessageDeserialized -= deserializedNotifier.AttachToDeserializerConversationHandler;
            };
        }

        return deserializedNotifier;
    }

    public int DecrementDeserializedNotifierUsage(string id)
    {
        if (registeredNotifiers.TryGetValue(id, out var deserializedNotifier))
        {
            if (--deserializedNotifier!.SubscriberCount <= 0)
            {
                deserializedNotifier.Unsubscribe?.Invoke();
                registeredNotifiers.Remove(deserializedNotifier.Id);
            }

            return deserializedNotifier.SubscriberCount;
        }

        return -1;
    }

    public int IncrementDeserializedNotifierUsage(string id)
    {
        if (registeredNotifiers.TryGetValue(id, out var deserializedNotifier)) return ++deserializedNotifier!.SubscriberCount;
        return -1;
    }

    protected void OnNotify(TM data, IBufferContext bufferContext)
    {
        MessageDeserialized?.Invoke(data, bufferContext);
        if (bufferContext is ISocketBufferReadContext socketBufferReadContext)
            ConversationMessageDeserialized?.Invoke(data, socketBufferReadContext.MessageHeader, socketBufferReadContext.Conversation!);
    }
}
