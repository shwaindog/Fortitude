#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Topics;

public interface IResponderTopic : ITopic, IConversationResponder { }

public class ResponderTopic : Topic, IResponderTopic
{
    private IConversationResponder sessionConnection;

    public ResponderTopic(string description, IConversationResponder sessionConnection) :
        base(description, ConversationType.Requester) =>
        this.sessionConnection = sessionConnection;

    public IReadOnlyDictionary<int, ISocketConversation>? Clients { get; set; }
    public IConversationListener? ConversationListener { get; set; }

    public override bool IsStarted { get; } = false;

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Stop()
    {
        throw new NotImplementedException();
    }

    public void RegisterSerializer(uint messageId, IMessageSerializer serializer) { }

    public void RemoveClient(ISocketConversation clientSocketSessionContext)
    {
        throw new NotImplementedException();
    }

    public void Broadcast(IVersionedMessage message)
    {
        throw new NotImplementedException();
    }

#pragma warning disable 67
    public event Action<ISocketSessionContext>? OnNewClient;
    public event Action<ISocketSessionContext>? OnClientRemoved;
#pragma warning restore 67
}
