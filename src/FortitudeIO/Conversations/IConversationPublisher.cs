#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationPublisher : IConversation
{
    IStreamPublisher? StreamPublisher { get; }

    void Send(IVersionedMessage versionedMessage);
}
