#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class ConversationSubscriber : SocketConversation, IInitiateControls, ISubscriberConversation
{
    private readonly IInitiateControls initiateControls;

    public ConversationSubscriber(SocketSessionContext socketSessionContext,
        IInitiateControls initiateControls) : base(socketSessionContext, initiateControls) =>
        this.initiateControls = initiateControls;

    public void ConnectAsync() => initiateControls.ConnectAsync();
}
