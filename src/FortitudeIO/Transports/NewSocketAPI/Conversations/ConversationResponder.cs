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

    public event Action<IConversationRequester>? NewClient
    {
        add => acceptorControls.NewClient += value;
        remove => acceptorControls.NewClient -= value;
    }

    public event Action<IConversationRequester>? ClientRemoved
    {
        add => acceptorControls.ClientRemoved += value;
        remove => acceptorControls.ClientRemoved -= value;
    }

    public IReadOnlyDictionary<int, IConversationRequester> Clients => acceptorControls.Clients;

    public void RemoveClient(IConversationRequester clientSocketSessionContext) =>
        acceptorControls.RemoveClient(clientSocketSessionContext);

    public void Broadcast(IVersionedMessage message) => acceptorControls.Broadcast(message);

    public void RegisterSerializer(uint messageId, IMessageSerializer serializer)
    {
        SocketSessionContext.SerdesFactory.StreamEncoderFactory!.RegisterMessageSerializer(messageId, serializer);
        foreach (var client in acceptorControls.Clients.Values)
            client.ConversationPublisher!.RegisterSerializer(messageId, serializer);
    }
}
