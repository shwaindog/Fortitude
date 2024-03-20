#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class ConversationSubscriber : SocketConversation, IInitiateControls, IConversationSubscriber
{
    public ConversationSubscriber(ISocketSessionContext socketSessionContext,
        IInitiateControls initiateControls) : base(socketSessionContext, initiateControls) { }

    public void StartAsync() => ((IInitiateControls)InitiateControls).StartAsync();
}
