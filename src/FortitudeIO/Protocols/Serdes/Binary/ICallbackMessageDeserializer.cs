#region

using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface ICallbackMessageDeserializer<out TM> : IMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    event Action<TM, object?, IConversation?>? Deserialized2;

    bool IsRegistered(Action<TM, object, IConversation> deserializedHandler);
}
