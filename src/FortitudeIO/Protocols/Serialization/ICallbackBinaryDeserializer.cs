﻿#region

using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Protocols.Serialization;

public interface ICallbackBinaryDeserializer<out TM> : IBinaryDeserializer where TM : class
{
    //[Obsolete]  TODO restore when switched over
    event Action<TM, object?, ISession?>? Deserialized;

    event Action<TM, object?, ISocketConversation?>? Deserialized2;
    bool IsRegistered(Action<TM, object, ISessionConnection> deserializedHandler);
}
