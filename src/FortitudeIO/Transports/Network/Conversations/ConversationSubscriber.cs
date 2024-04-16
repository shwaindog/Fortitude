#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Conversations;

public class ConversationSubscriber : SocketConversation, IStreamControls, IConversationSubscriber
{
    public ConversationSubscriber(ISocketSessionContext socketSessionContext, IStreamControls streamControls) : base(socketSessionContext
        , streamControls) { }

    public ValueTask<bool> StartAsync(TimeSpan timeoutTimeSpan
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null) =>
        SocketSessionContext.StreamControls!.StartAsync(timeoutTimeSpan, alternativeExecutionContext);

    public ValueTask<bool> StartAsync(int timeoutMs, IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null) =>
        SocketSessionContext.StreamControls!.StartAsync(timeoutMs, alternativeExecutionContext);
}
