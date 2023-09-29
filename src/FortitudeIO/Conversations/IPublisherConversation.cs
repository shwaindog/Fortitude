using FortitudeIO.Conversations;

namespace FortitudeIO.Transports.NewSocketAPI.Conversations
{
    public interface IPublisherConversation : IConversation
    {
        IConversationPublisher ConversationPublisher { get; }
    }
}