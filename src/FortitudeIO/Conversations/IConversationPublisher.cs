using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;

namespace FortitudeIO.Transports
{
    public interface IConversationPublisher
    {
        void RegisterSerializer(uint messageId, IBinarySerializer serializer);
        void Enqueue(IVersionedMessage message);
        void Send(IVersionedMessage message);
        bool SendEnqueued();
    }
}