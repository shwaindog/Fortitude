namespace FortitudeIO.Conversations;

public interface IConversation : IConversationState, IConversationInitiator
{
    int Id { get; }
    bool IsStarted { get; }
    IConversationSession Session { get; }
}
