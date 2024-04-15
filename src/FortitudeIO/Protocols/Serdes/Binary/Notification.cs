#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public struct ConversationMessageNotification<TR>
{
    public ConversationMessageNotification(TR message, MessageHeader header, IConversation conversation)
    {
        Message = message;
        Header = header;
        Conversation = conversation;
    }

    private TR Message { get; }

    private MessageHeader Header { get; }

    private IConversation Conversation { get; }
}

public struct MessageNotification<TR>
{
    public MessageNotification(TR message) => Message = message;

    private TR Message { get; }
}

public interface IReceiverListenContext<TR>
{
    void SendToReceiver(ConversationMessageNotification<TR> conversationMessageNotification);
    void SendToReceiver(MessageNotification<TR> conversationMessageNotification);
}

public interface IDeserializedNotifier
{
    string Id { get; }
    int SubscriberCount { get; set; }
    Action? Unsubscribe { get; set; }
}

public interface IDeserializedNotifier<TM, TR> : IDeserializedNotifier where TM : class, IVersionedMessage, new()
{
    ConversationMessageReceivedHandler<TM>? AttachToDeserializerConversationHandler { get; }
    MessageDeserializedHandler<TM>? AttachToDeserializerMessageHandler { get; }
    IReceiverListenContext<TR>? ReceiverListenContext { get; set; }
    void AddRequestExpected(int requestId, ReusableValueTaskSource<TR> responseValueTaskSource);
}

public class PassThroughDeserializedNotifier<TM> : IDeserializedNotifier<TM, TM> where TM : class, IVersionedMessage, new()
{
    private readonly IMap<int, ReusableValueTaskSource<TM>> expectedRequestResponses = new ConcurrentMap<int, ReusableValueTaskSource<TM>>();

    private ConversationMessageReceivedHandler<TM>? receiverConversationMessageReceivedHandler;
    private MessageDeserializedHandler<TM>? receiverMessageDeserializedHandler;

    public PassThroughDeserializedNotifier(string id, ConversationMessageReceivedHandler<TM> conversationMessageReceived)
    {
        Id = id;
        receiverConversationMessageReceivedHandler = conversationMessageReceived;
    }

    public PassThroughDeserializedNotifier(string id, MessageDeserializedHandler<TM> messageDeserializedHandler)
    {
        Id = id;
        receiverMessageDeserializedHandler = messageDeserializedHandler;
    }

    public string Id { get; }
    public int SubscriberCount { get; set; } = 1;

    public Action? Unsubscribe { get; set; }

    public ConversationMessageReceivedHandler<TM>? AttachToDeserializerConversationHandler =>
        receiverConversationMessageReceivedHandler != null ? ConversationMessageDeserialized : null;

    public MessageDeserializedHandler<TM>? AttachToDeserializerMessageHandler =>
        receiverConversationMessageReceivedHandler != null ? MessageDeserialized : null;

    public IReceiverListenContext<TM>? ReceiverListenContext { get; set; }

    public void AddRequestExpected(int requestId, ReusableValueTaskSource<TM> responseValueTaskSource)
    {
        expectedRequestResponses.Add(requestId, responseValueTaskSource);
    }

    private void ConversationMessageDeserialized(TM message, MessageHeader messageHeader, IConversation conversation)
    {
        if (message is IRequestMessage requestMessage)
            if (expectedRequestResponses.TryGetValue(requestMessage.RequestId, out var taskSource))
            {
                taskSource!.SetResult(message);
                expectedRequestResponses.Remove(requestMessage.RequestId);
                return;
            }

        if (ReceiverListenContext != null)
        {
            var messageConversation = new ConversationMessageNotification<TM>(message, messageHeader, conversation);
            ReceiverListenContext.SendToReceiver(messageConversation);
        }
        else
        {
            receiverConversationMessageReceivedHandler?.Invoke(message, messageHeader, conversation);
        }
    }

    private void MessageDeserialized(TM message, IBufferContext bufferContext)
    {
        if (ReceiverListenContext != null)
        {
            var messageConversation = new MessageNotification<TM>(message);
            ReceiverListenContext.SendToReceiver(messageConversation);
        }
        else
        {
            receiverMessageDeserializedHandler?.Invoke(message, bufferContext);
        }
    }
}

public delegate void ConvertedConversationMessageReceivedHandler<in TM>(TM deserializedMessage, MessageHeader header, IConversation conversation);

public delegate void ConvertedMessageDeserializedHandler<in TM>(TM deserializedMessage, IBufferContext bufferContext);

public class ConvertingDeserializedNotifier<TM, TR> : IDeserializedNotifier<TM, TR> where TM : class, IVersionedMessage, new()
{
    private readonly IMap<int, ReusableValueTaskSource<TR>> expectedRequestResponses = new ConcurrentMap<int, ReusableValueTaskSource<TR>>();


    private readonly ConvertedConversationMessageReceivedHandler<TR>? receiverConversationMessageReceivedHandler;
    private readonly ConvertedMessageDeserializedHandler<TR>? receiverMessageDeserializedHandler;


    public ConvertingDeserializedNotifier(string id, IConverter<TM, TR> converter
        , ConvertedConversationMessageReceivedHandler<TR> conversationMessageReceived)
    {
        Id = id;
        Converter = converter;
        receiverConversationMessageReceivedHandler = conversationMessageReceived;
    }

    public ConvertingDeserializedNotifier(string id, IConverter<TM, TR> converter, ConvertedMessageDeserializedHandler<TR> messageDeserializedHandler)
    {
        Id = id;
        Converter = converter;
        receiverMessageDeserializedHandler = messageDeserializedHandler;
    }

    private IConverter<TM, TR> Converter { get; }

    public Action? Unsubscribe { get; set; }
    public string Id { get; }
    public int SubscriberCount { get; set; } = 1;

    public ConversationMessageReceivedHandler<TM>? AttachToDeserializerConversationHandler =>
        receiverConversationMessageReceivedHandler != null ? ConversationMessageDeserialized : null;

    public MessageDeserializedHandler<TM>? AttachToDeserializerMessageHandler =>
        receiverConversationMessageReceivedHandler != null ? MessageDeserialized : null;

    public IReceiverListenContext<TR>? ReceiverListenContext { get; set; }

    public void AddRequestExpected(int requestId, ReusableValueTaskSource<TR> responseValueTaskSource)
    {
        expectedRequestResponses.Add(requestId, responseValueTaskSource);
    }

    private void ConversationMessageDeserialized(TM message, MessageHeader messageHeader, IConversation conversation)
    {
        var convertedMessage = Converter.Convert(message);
        if (message is IRequestMessage requestMessage)
            if (expectedRequestResponses.TryGetValue(requestMessage.RequestId, out var taskSource))
            {
                taskSource!.SetResult(convertedMessage);
                expectedRequestResponses.Remove(requestMessage.RequestId);
                return;
            }

        if (ReceiverListenContext != null)
        {
            var messageConversation = new ConversationMessageNotification<TR>(convertedMessage, messageHeader, conversation);
            ReceiverListenContext.SendToReceiver(messageConversation);
        }
        else
        {
            receiverConversationMessageReceivedHandler?.Invoke(convertedMessage, messageHeader, conversation);
        }
    }

    private void MessageDeserialized(TM message, IBufferContext bufferContext)
    {
        var convertedMessage = Converter.Convert(message);
        if (ReceiverListenContext != null)
        {
            var messageConversation = new MessageNotification<TR>(convertedMessage);
            ReceiverListenContext.SendToReceiver(messageConversation);
        }
        else
        {
            receiverMessageDeserializedHandler?.Invoke(convertedMessage, bufferContext);
        }
    }
}
