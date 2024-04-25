#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationInitiator
{
    void OnSessionFailure(string reason);
    void Start();
    void Stop(CloseReason closeReason = CloseReason.Completed, string? reason = null);
}
