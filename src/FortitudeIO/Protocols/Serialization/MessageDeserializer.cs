﻿#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets.Logging;

#endregion

namespace FortitudeIO.Protocols.Serialization;

public abstract class MessageDeserializer<TM> : ICallbackMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    object? IMessageDeserializer.Deserialize(DispatchContext dispatchContext) => Deserialize(dispatchContext);

    public MarshalType MarshalType => MarshalType.Binary;
    public abstract TM? Deserialize(ISerdeContext readContext);

    TM? IMessageDeserializer<TM>.Deserialize(DispatchContext dispatchContext) => Deserialize(dispatchContext);

    //[Obsolete]  TODO restore when switched over
    public event Action<TM, object?, ISession?>? Deserialized;

    public event Action<TM, object?, ISocketConversation?>? Deserialized2;

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

    protected void Dispatch(TM data, object? state, ISocketConversation? sender,
        IPerfLogger? detectionToPublishLatencyTraceLogger)
    {
        detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.BeforePublish);
        Deserialized2?.Invoke(data, state, sender);
    }
}