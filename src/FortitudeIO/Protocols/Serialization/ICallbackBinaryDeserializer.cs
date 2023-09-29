using System;
using FortitudeIO.Sockets;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Sockets;

namespace FortitudeIO.Protocols.Serialization
{
    public interface ICallbackBinaryDeserializer<out Tm> : IBinaryDeserializer where Tm : class
    {
        [Obsolete]
        event Action<Tm, object, ISession> Deserialized;
        event Action<Tm, object, ISocketConversation> Deserialized2;
        bool IsRegistered(Action<Tm, object, ISessionConnection> deserializedHandler);
    }
}