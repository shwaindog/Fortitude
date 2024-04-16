#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations;

public class ConversationPublisher : SocketConversation, IStreamControls, IConversationPublisher
{
    public ConversationPublisher(ISocketSessionContext socketSessionContext,
        IStreamControls streamControls) : base(socketSessionContext, streamControls) { }

    public void Send(IVersionedMessage versionedMessage)
    {
        SocketSessionContext.SocketSender!.Send(versionedMessage);
    }

    public ValueTask<bool> StartAsync(TimeSpan timeoutTimeSpan
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null) =>
        SocketSessionContext.StreamControls!.StartAsync(timeoutTimeSpan, alternativeExecutionContext);

    public ValueTask<bool> StartAsync(int timeoutMs, IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null) =>
        SocketSessionContext.StreamControls!.StartAsync(timeoutMs, alternativeExecutionContext);
}
