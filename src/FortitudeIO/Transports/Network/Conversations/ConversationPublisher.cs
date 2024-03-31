#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations;

public class ConversationPublisher : SocketConversation, IInitiateControls, IConversationPublisher
{
    public ConversationPublisher(ISocketSessionContext socketSessionContext,
        IInitiateControls initiateControls) : base(socketSessionContext, initiateControls) { }

    public void Send(IVersionedMessage versionedMessage)
    {
        SocketSessionContext.SocketSender!.Send(versionedMessage);
    }

    public void StartAsync() => ((IInitiateControls?)SocketSessionContext.StreamControls)?.StartAsync();
}
