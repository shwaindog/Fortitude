#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.NewSocketAPI.Logging;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageDeserializer
{
    object? Deserialize(ReadSocketBufferContext readSocketBufferContext);
}

public interface IMessageDeserializer<out TM> : IMessageDeserializer, IDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    new TM? Deserialize(ReadSocketBufferContext readSocketBufferContext);
}

public readonly struct BasicMessageHeader
{
    public BasicMessageHeader(byte version, uint messageId, uint messageSize
        , ISerdeContext? deserializationContext = null)
    {
        Version = version;
        MessageId = messageId;
        MessageSize = messageSize;
        DeserializationContext = deserializationContext;
    }

    public byte Version { get; }
    public uint MessageId { get; }
    public uint MessageSize { get; }
    public ISerdeContext? DeserializationContext { get; }
}

public abstract class MessageDeserializer<TM> : ICallbackMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    object? IMessageDeserializer.Deserialize(ReadSocketBufferContext readSocketBufferContext) => Deserialize(readSocketBufferContext);

    public MarshalType MarshalType => MarshalType.Binary;
    public abstract TM? Deserialize(ISerdeContext readContext);

    TM? IMessageDeserializer<TM>.Deserialize(ReadSocketBufferContext readSocketBufferContext) => Deserialize(readSocketBufferContext);

    public bool IsRegistered(Action<TM, object, IConversation> deserializedHandler)
    {
        return Deserialized2 != null && Deserialized2.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);
    }

    public event Action<TM, object?, IConversation?>? Deserialized2;


    public event Action<TM, IBufferContext>? MessageDeserialized;


    protected void Dispatch(TM data, IBufferContext bufferContext)
    {
        MessageDeserialized?.Invoke(data, bufferContext);
    }

    protected void Dispatch(TM data, object? state, IConversation? sender,
        IPerfLogger? detectionToPublishLatencyTraceLogger)
    {
        detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.BeforePublish);
        Deserialized2?.Invoke(data, state, sender);
    }
}
