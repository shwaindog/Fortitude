#region

using System;
using FortitudeIO.Sockets;
using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Protocols.Serialization
{
    public interface ICallbackBinaryDeserializer<out Tm> : IBinaryDeserializer where Tm : class
    {
        // [Obsolete] TODO restore when switched over
        event Action<Tm, object, ISession> Deserialized;
        event Action<Tm, object, ISocketConversation> Deserialized2;
        bool IsRegistered(Action<Tm, object, ISessionConnection> deserializedHandler);
    }
}
