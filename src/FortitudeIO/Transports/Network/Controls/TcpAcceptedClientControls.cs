#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Controls;

public class TcpAcceptedClientControls : SocketStreamControls
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TcpAcceptedClientControls));

    public TcpAcceptedClientControls(ISocketSessionContext socketSessionContext) : base(socketSessionContext) { }

    public override bool Connect()
    {
        logger.Warn("Can NOT connect to client accepted session {0} will not attempt connect.", SocketSessionContext);
        return false;
    }

    public override void Disconnect(CloseReason closeReason, string? reason = null)
    {
        if (SocketSessionContext.SocketReceiver != null)
            SocketSessionContext.SocketDispatcher.Listener?.UnregisterForListen(SocketSessionContext.SocketReceiver);
        if (SocketSessionContext.SocketSessionState == SocketSessionState.Connected)
            SocketSessionContext.OnDisconnecting();
        if (SocketSessionContext.SocketConnection?.IsConnected ?? false)
        {
            logger.Info("Connection client session {0} closed. {1}", SocketSessionContext.Name, reason);

            SocketSessionContext.OnDisconnected(closeReason, reason);
            StopMessaging();
        }
    }

    public override void OnSessionFailure(string reason)
    {
        logger.Warn("Problem communicating with client session {0} will not attempt reconnect. Reason {1}", SocketSessionContext, reason);
        Disconnect(CloseReason.Error, reason);
    }

    protected override bool ConnectAttemptSucceeded() => false;

    public override ValueTask<bool> StartAsync(TimeSpan timeoutTimeSpan
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null)
    {
        logger.Warn("Can NOT connect to client accepted session {0} will not attempt connect.", SocketSessionContext);
        return new ValueTask<bool>(false);
    }

    public override ValueTask<bool> StartAsync(int timeoutMs, IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null)
    {
        logger.Warn("Can NOT connect to client accepted session {0} will not attempt connect.", SocketSessionContext);
        return new ValueTask<bool>(false);
    }
}
