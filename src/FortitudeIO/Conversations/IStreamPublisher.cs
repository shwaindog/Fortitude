#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Conversations;

public interface IStreamPublisher
{
    void Enqueue(IVersionedMessage message);
    void Send(IVersionedMessage message);
    bool SendQueued();
}
