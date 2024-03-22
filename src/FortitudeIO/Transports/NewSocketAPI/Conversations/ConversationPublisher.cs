#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class ConversationPublisher : SocketConversation, IInitiateControls, IConversationPublisher
{
    public ConversationPublisher(ISocketSessionContext socketSessionContext,
        IInitiateControls initiateControls) : base(socketSessionContext, initiateControls) =>
        socketSessionContext.OwningConversation = this;

    public void Send(IVersionedMessage versionedMessage)
    {
        SocketSessionContext.SocketSender!.Send(versionedMessage);
    }

    public void StartAsync() => ((IInitiateControls)InitiateControls).StartAsync();
}
