#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationPublisher
{
    void RegisterSerializer(uint messageId, IMessageSerializer serializer);
    void Enqueue(IVersionedMessage message);
    void Send(IVersionedMessage message);
    bool SendEnqueued();
}
