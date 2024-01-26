#region

using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Protocols.Serialization;

public interface ICallbackMessageDeserializer<out TM> : IMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    //[Obsolete]  TODO restore when switched over
    event Action<TM, object?, ISession?>? Deserialized;

    event Action<TM, object?, ISocketConversation?>? Deserialized2;
    bool IsRegistered(Action<TM, object, ISessionConnection> deserializedHandler);
}
