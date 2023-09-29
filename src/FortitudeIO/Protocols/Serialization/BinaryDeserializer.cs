using System;
using System.Linq;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Sockets;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Logging;

namespace FortitudeIO.Protocols.Serialization
{
    public abstract class BinaryDeserializer<TM> : ICallbackBinaryDeserializer<TM> where TM : class
    {
        public abstract object Deserialize(DispatchContext dispatchContext);
        
        [Obsolete]
        public event Action<TM, object, ISession> Deserialized;
        public event Action<TM, object, ISocketConversation> Deserialized2;

        protected void Dispatch(TM data, object state, ISession repositorySession,
            IPerfLogger detectionToPublishLatencyTraceLogger)
        {
            detectionToPublishLatencyTraceLogger.Add(SocketDataLatencyLogger.BeforePublish);
            Deserialized?.Invoke(data, state, repositorySession);
        }

        protected void Dispatch(TM data, object state, ISocketConversation sender,
            IPerfLogger detectionToPublishLatencyTraceLogger)
        {
            detectionToPublishLatencyTraceLogger.Add(SocketDataLatencyLogger.BeforePublish);
            Deserialized2?.Invoke(data, state, sender);
        }

        public bool IsRegistered(Action<TM, object, ISessionConnection> deserializedHandler)
        {
            return Deserialized != null && Deserialized.GetInvocationList()
                .Any(del => del.Target == deserializedHandler.Target && del.Method == deserializedHandler.Method);
        }
    }
}