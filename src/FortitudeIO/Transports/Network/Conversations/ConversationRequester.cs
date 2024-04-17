#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations;

public class ConversationRequester : SocketConversation, IStreamControls, IConversationRequester
{
    public ConversationRequester(ISocketSessionContext socketSessionContext,
        IStreamControls streamControls) : base(socketSessionContext, streamControls) { }

    public virtual void Send(IVersionedMessage versionedMessage)
    {
        versionedMessage.IncrementRefCount();
        SocketSessionContext.SocketSender!.Send(versionedMessage);
    }

    public virtual ValueTask<bool> StartAsync(TimeSpan timeoutTimeSpan
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null) =>
        SocketSessionContext.StreamControls!.StartAsync(timeoutTimeSpan, alternativeExecutionContext);

    public virtual ValueTask<bool>
        StartAsync(int timeoutMs, IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null) =>
        SocketSessionContext.StreamControls!.StartAsync(timeoutMs, alternativeExecutionContext);
}
