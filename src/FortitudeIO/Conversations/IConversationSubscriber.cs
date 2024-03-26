namespace FortitudeIO.Conversations;

public interface IConversationSubscriber : IConversation
{
    IStreamListener? StreamListener { get; }
}
