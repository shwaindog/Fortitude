#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets.Logging;

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

public struct BasicMessageHeader
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
    object? IMessageDeserializer.Deserialize(ReadSocketBufferContext readSocketBufferContext) =>
        Deserialize(readSocketBufferContext);

    public MarshalType MarshalType => MarshalType.Binary;
    public abstract TM? Deserialize(ISerdeContext readContext);

    TM? IMessageDeserializer<TM>.Deserialize(ReadSocketBufferContext readSocketBufferContext) =>
        Deserialize(readSocketBufferContext);

    //[Obsolete]  TODO restore when switched over
    public event Action<TM, object?, ISession?>? Deserialized;

    public event Action<TM, object?, ISocketConversation?>? Deserialized2;

    public event Action<TM, BasicMessageHeader>? MessageDeserialized;

    public bool IsRegistered(Action<TM, object, ISessionConnection> deserializedHandler)
    {
        return Deserialized != null && Deserialized.GetInvocationList()
            .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);
    }

    protected void Dispatch(TM data, object? state, ISession? repositorySession,
        IPerfLogger? detectionToPublishLatencyTraceLogger)
    {
        detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.BeforePublish);
        Deserialized?.Invoke(data, state, repositorySession);
    }

    protected void Dispatch(TM data, BasicMessageHeader messageHeader)
    {
        MessageDeserialized?.Invoke(data, messageHeader);
    }

    protected void Dispatch(TM data, object? state, ISocketConversation? sender,
        IPerfLogger? detectionToPublishLatencyTraceLogger)
    {
        detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.BeforePublish);
        Deserialized2?.Invoke(data, state, sender);
    }
}
