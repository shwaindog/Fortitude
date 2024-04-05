#region

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
    new TM? Deserialize(IBufferContext socketBufferReadContext);
}

public interface INotifyingMessageDeserializer<out TM> : IMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    event Action<TM, object?, IConversation?>? ConversationMessageDeserialized;
    event Action<TM, IBufferContext>? MessageDeserialized;

    bool IsRegistered(Action<TM, IBufferContext> deserializedHandler);
    bool IsRegistered(Action<TM, object, IConversation> deserializedHandler);
}

public abstract class MessageDeserializer<TM> : INotifyingMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    object? IMessageDeserializer.Deserialize(IBufferContext socketBufferReadContext) => Deserialize(socketBufferReadContext);

    public MarshalType MarshalType => MarshalType.Binary;
    public abstract TM? Deserialize(ISerdeContext readContext);

    TM? IMessageDeserializer<TM>.Deserialize(IBufferContext socketBufferReadContext) => Deserialize(socketBufferReadContext);

    public bool IsRegistered(Action<TM, object, IConversation> deserializedHandler)
    {
        return ConversationMessageDeserialized != null && ConversationMessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);
    }

    public bool IsRegistered(Action<TM, IBufferContext> deserializedHandler)
    {
        return MessageDeserialized != null && MessageDeserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);
    }

    public event Action<TM, object?, IConversation?>? ConversationMessageDeserialized;

    public event Action<TM, IBufferContext>? MessageDeserialized;

    protected void OnNotify(TM data, IBufferContext bufferContext)
    {
        MessageDeserialized?.Invoke(data, bufferContext);
        if (bufferContext is ISocketBufferReadContext socketBufferReadContext)
            ConversationMessageDeserialized?.Invoke(data, socketBufferReadContext.MessageHeader, socketBufferReadContext.Conversation);
    }
}
