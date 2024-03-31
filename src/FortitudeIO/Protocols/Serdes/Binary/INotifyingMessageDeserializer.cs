#region

using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface INotifyingMessageDeserializer<out TM> : IMessageDeserializer<TM>
    where TM : class, IVersionedMessage, new()
{
    event Action<TM, object?, IConversation?>? ConversationMessageDeserialized;
    event Action<TM, IBufferContext>? MessageDeserialized;

    bool IsRegistered(Action<TM, IBufferContext> deserializedHandler);
    bool IsRegistered(Action<TM, object, IConversation> deserializedHandler);
}
