namespace FortitudeIO.Conversations;

public interface IConversationInitiator
{
    void OnSessionFailure(string reason);
    void Start();
    void Stop();
}
