namespace FortitudeIO.Conversations;

public interface IConversation : IConversationState, IConversationInitiator
{
    bool IsStarted { get; }
}
