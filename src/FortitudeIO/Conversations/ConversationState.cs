namespace FortitudeIO.Conversations;

public enum ConversationState
{
    New
    , Starting
    , Started
    , Stopping
    , Stopped
}

public interface IConversationState
{
    ConversationType ConversationType { get; }
    ConversationState ConversationState { get; }
    string Name { get; }
    event Action<string, int>? Error;
    event Action? Started;
    event Action? Stopped;
}

public interface IConversationSession
{
    int Id { get; }
}
