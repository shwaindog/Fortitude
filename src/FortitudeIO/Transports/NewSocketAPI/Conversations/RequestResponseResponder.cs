#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class RequestResponseResponder : SocketConversation, IAcceptorControls, IRequestResponseResponderConversation
{
    private readonly IAcceptorControls acceptorControls;

    public RequestResponseResponder(SocketSessionContext socketSessionContext,
        TCPAcceptorControls tcpAcceptorControls)
        : base(socketSessionContext, tcpAcceptorControls) =>
        acceptorControls = tcpAcceptorControls;

    public event Action<ISocketConversation>? OnNewClient
    {
        add => acceptorControls.OnNewClient += value;
        remove => acceptorControls.OnNewClient -= value;
    }

    public event Action<ISocketConversation>? OnClientRemoved
    {
        add => acceptorControls.OnClientRemoved += value;
        remove => acceptorControls.OnClientRemoved -= value;
    }

    public IReadOnlyDictionary<int, ISocketConversation> Clients => acceptorControls.Clients;

    public void RemoveClient(ISocketConversation clientSocketSessionContext) =>
        acceptorControls.RemoveClient(clientSocketSessionContext);

    public void Broadcast(IVersionedMessage message) => acceptorControls.Broadcast(message);

    public void RegisterSerializer(uint messageId, IBinarySerializer serializer)
    {
        SocketSessionContext.SerdesFactory.StreamSerializers[messageId] = serializer;
        foreach (var client in acceptorControls.Clients.Values)
            client.ConversationPublisher!.RegisterSerializer(messageId, serializer);
    }
}
