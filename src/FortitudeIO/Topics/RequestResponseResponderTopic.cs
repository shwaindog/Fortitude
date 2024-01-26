#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Topics;

public interface IRequestResponseResponderTopic : ITopic, IRequestResponseResponderConversation { }

public class RequestResponseResponderTopic : Topic, IRequestResponseResponderTopic
{
    private IRequestResponseResponderConversation sessionConnection;

    public RequestResponseResponderTopic(string description, IRequestResponseResponderConversation sessionConnection) :
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

    public void RegisterSerializer(uint messageId, IMessageSerializer serializer) { }

    public IReadOnlyDictionary<int, ISocketConversation>? Clients { get; set; }
    public IConversationListener? ConversationListener { get; set; }

    public void RemoveClient(ISocketConversation clientSocketSessionContext)
    {
        throw new NotImplementedException();
    }

    public void Broadcast(IVersionedMessage message)
    {
        throw new NotImplementedException();
    }

#pragma warning disable 67
    public event Action<ISocketConversation>? OnNewClient;
    public event Action<ISocketConversation>? OnClientRemoved;
#pragma warning restore 67
}
