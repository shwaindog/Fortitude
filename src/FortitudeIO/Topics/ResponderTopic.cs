#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Topics;

public interface IResponderTopic : ITopic, IConversationResponder { }

public class ResponderTopic : Topic, IResponderTopic
{
    private IConversationResponder sessionConnection;

    public ResponderTopic(string description, IConversationResponder sessionConnection) :
        base(description, ConversationType.Requester) =>
        this.sessionConnection = sessionConnection;

    public IReadOnlyDictionary<int, IConversationRequester>? Clients { get; set; }
    public IStreamListener? StreamListener { get; set; }

    public override bool IsStarted { get; } = false;

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Stop()
    {
        throw new NotImplementedException();
    }

    public void RemoveClient(IConversationRequester clientSocketSessionContext)
    {
        throw new NotImplementedException();
    }

    public void Broadcast(IVersionedMessage message)
    {
        throw new NotImplementedException();
    }

    public void RegisterSerializer(uint messageId, IMessageSerializer serializer) { }

#pragma warning disable 67
    public event Action<IConversationRequester>? NewClient;
    public event Action<IConversationRequester>? ClientRemoved;
#pragma warning restore 67
}
