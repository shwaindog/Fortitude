using System;
using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;

namespace FortitudeIO.Transports.Sockets.Subscription
{
    public interface IBinaryStreamSubscriber
    {
        int RecvBufferSize { get; }
        int WholeMessagesPerReceive { get; }
        void RegisterDeserializer<Tm>(uint msgId, Action<Tm, object, ISession> msgHandler) where Tm : class;
        void UnregisterDeserializer<Tm>(uint msgId, Action<Tm, object, ISession> msgHandler) where Tm : class;
        int RegisteredDeserializersCount { get; }
        IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> deserializers);
    }
}