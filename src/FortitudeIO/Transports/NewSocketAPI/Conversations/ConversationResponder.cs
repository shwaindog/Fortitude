#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class ConversationResponder : SocketConversation, IAcceptorControls, IConversationResponder
{
    private readonly IAcceptorControls acceptorControls;

    public ConversationResponder(ISocketSessionContext socketSessionContext,
        IAcceptorControls tcpAcceptorControls)
        : base(socketSessionContext, tcpAcceptorControls) =>
        acceptorControls = tcpAcceptorControls;

    public event Action<ISocketSessionContext>? OnNewClient
    {
        add => acceptorControls.OnNewClient += value;
        remove => acceptorControls.OnNewClient -= value;
    }

    public event Action<ISocketSessionContext>? OnClientRemoved
    {
        add => acceptorControls.OnClientRemoved += value;
        remove => acceptorControls.OnClientRemoved -= value;
    }

    public IReadOnlyDictionary<int, ISocketConversation> Clients => acceptorControls.Clients;

    public void RemoveClient(ISocketConversation clientSocketSessionContext) =>
        acceptorControls.RemoveClient(clientSocketSessionContext);

    public void Broadcast(IVersionedMessage message) => acceptorControls.Broadcast(message);

    public void RegisterSerializer(uint messageId, IMessageSerializer serializer)
    {
        SocketSessionContext.SerdesFactory.StreamEncoderFactory!.RegisterMessageSerializer(messageId, serializer);
        foreach (var client in acceptorControls.Clients.Values)
            client.ConversationPublisher!.RegisterSerializer(messageId, serializer);
    }
}
