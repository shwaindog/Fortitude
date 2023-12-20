#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationPublisher
{
    void RegisterSerializer(uint messageId, IBinarySerializer serializer);
    void Enqueue(IVersionedMessage message);
    void Send(IVersionedMessage message);
    bool SendEnqueued();
}
