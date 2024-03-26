#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Topics;

public interface IRequesterTopic : ITopic, IConversationRequester { }

public class RequesterTopic : Topic, IRequesterTopic
{
    private IConversationRequester sessionConnection;

    public RequesterTopic(string description, IConversationRequester sessionConnection) :
        base(description, ConversationType.Requester) =>
        this.sessionConnection = sessionConnection;

    public IStreamListener? StreamListener { get; set; }
    public IStreamPublisher? StreamPublisher { get; set; }

    public override bool IsStarted { get; } = false;

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Stop()
    {
        throw new NotImplementedException();
    }

    public void Send(IVersionedMessage versionedMessage)
    {
        throw new NotImplementedException();
    }
}
