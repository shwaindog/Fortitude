#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Conversations;

public interface IStreamPublisher
{
    void Enqueue(IVersionedMessage message);
    void Send(IVersionedMessage message);
    bool SendQueued();
    void RegisterSerializer(uint messageId, IMessageSerializer serializer);
}
