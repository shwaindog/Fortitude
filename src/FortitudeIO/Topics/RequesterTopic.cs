#region

using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Topics;

public interface IRequesterTopic : ITopic, IConversationRequester { }

public class RequesterTopic : Topic, IRequesterTopic
{
    private IConversationRequester sessionConnection;

    public RequesterTopic(string description, IConversationRequester sessionConnection) :
        base(description, ConversationType.Requester) =>
        this.sessionConnection = sessionConnection;

    public IConversationListener? ConversationListener { get; set; }
    public IConversationPublisher? ConversationPublisher { get; set; }

    public override bool IsStarted { get; } = false;

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Stop()
    {
        throw new NotImplementedException();
    }
}
