#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationRequester : IConversation
{
    IListener? ConversationListener { get; }
    IPublisher? ConversationPublisher { get; }
    void Send(IVersionedMessage versionedMessage);
}
