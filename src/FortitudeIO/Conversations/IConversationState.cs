#region

using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationState
{
    ConversationType ConversationType { get; }
    ConversationState ConversationState { get; }
    string ConversationDescription { get; }
    event Action<string, int>? Error;
    event Action? Started;
    event Action? Stopped;
}
