#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationRequester : IConversation
{
    IStreamListener? StreamListener { get; }
    IStreamPublisher? StreamPublisher { get; }
    void Send(IVersionedMessage versionedMessage);
}
