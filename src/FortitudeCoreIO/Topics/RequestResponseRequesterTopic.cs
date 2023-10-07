#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports;

#endregion

namespace FortitudeIO.Topics;

public interface IRequestResponseRequesterTopic : ITopic, IRequestResponseRequesterConversation { }

public class RequestResponseRequesterTopic : Topic, IRequestResponseRequesterTopic
{
    private IRequestResponseRequesterConversation sessionConnection;

    public RequestResponseRequesterTopic(string description, IRequestResponseRequesterConversation sessionConnection) :
        base(description, ConversationType.RequestResponseRequester) =>
        this.sessionConnection = sessionConnection;

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Stop()
    {
        throw new NotImplementedException();
    }

    public IConversationListener? ConversationListener { get; set; }
    public IConversationPublisher? ConversationPublisher { get; set; }
}
