#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

public class ConversationRequester : SocketConversation, IInitiateControls, IConversationRequester
{
    public ConversationRequester(ISocketSessionContext socketSessionContext,
        IInitiateControls initiateControls) : base(socketSessionContext, initiateControls) =>
        socketSessionContext.OwningConversation = this;


    public void StartAsync() => ((IInitiateControls)InitiateControls).StartAsync();
}
