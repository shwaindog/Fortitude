#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations;

public class ConversationSubscriber : SocketConversation, IInitiateControls, IConversationSubscriber
{
    public ConversationSubscriber(ISocketSessionContext socketSessionContext,
        IInitiateControls initiateControls) : base(socketSessionContext, initiateControls) { }

    public void StartAsync() => ((IInitiateControls?)SocketSessionContext.StreamControls)?.StartAsync();
}
