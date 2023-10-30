#region

using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Conversations;

public interface IPublisherConversation : IConversation
{
    IConversationPublisher? ConversationPublisher { get; }
}
