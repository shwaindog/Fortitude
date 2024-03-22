#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface ICallbackMessageDeserializer<out TM> : IMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    //[Obsolete]  TODO restore when switched over
    event Action<TM, object?, ISession?>? Deserialized;

    event Action<TM, object?, IConversation?>? Deserialized2;

    event Action<TM, BasicMessageHeader> MessageDeserialized;
    bool IsRegistered(Action<TM, object, IConversation> deserializedHandler);
    bool IsRegistered(Action<TM, object, ISession> deserializedHandler);
}
