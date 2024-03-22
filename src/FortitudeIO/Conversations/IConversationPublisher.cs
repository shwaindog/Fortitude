#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationPublisher : IConversation
{
    IPublisher? ConversationPublisher { get; }

    void Send(IVersionedMessage versionedMessage);
}
