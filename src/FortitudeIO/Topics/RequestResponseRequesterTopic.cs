#region

using FortitudeIO.Conversations;

#endregion

namespace FortitudeIO.Topics;

public interface IRequestResponseRequesterTopic : ITopic, IConversationRequester { }

public class RequestResponseRequesterTopic : Topic, IRequestResponseRequesterTopic
{
    private IConversationRequester sessionConnection;

    public RequestResponseRequesterTopic(string description, IConversationRequester sessionConnection) :
        base(description, ConversationType.Requester) =>
        this.sessionConnection = sessionConnection;

    public IConversationListener? ConversationListener { get; set; }
    public IConversationPublisher? ConversationPublisher { get; set; }

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Stop()
    {
        throw new NotImplementedException();
    }
}
