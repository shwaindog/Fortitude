#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations;

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

    public void RemoveClient(IConversationRequester clientSocketSessionContext) => acceptorControls.RemoveClient(clientSocketSessionContext);

    public void Broadcast(IVersionedMessage message) => acceptorControls.Broadcast(message);
}
