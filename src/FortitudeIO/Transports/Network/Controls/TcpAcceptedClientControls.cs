#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Controls;

public class TcpAcceptedClientControls : SocketStreamControls, IInitiateControls
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TcpAcceptedClientControls));

    public TcpAcceptedClientControls(ISocketSessionContext socketSessionContext) : base(socketSessionContext) { }

    public void StartAsync()
    {
        Start();
    }

    public override void Connect()
    {
        logger.Warn("Can NOT connect to client accepted session {0} will not attempt connect.", SocketSessionContext);
    }

    public override void Disconnect()
    {
        if (SocketSessionContext.SocketReceiver != null)
            SocketSessionContext.SocketDispatcher.Listener.UnregisterForListen(SocketSessionContext.SocketReceiver);
        if (SocketSessionContext.SocketSessionState == SocketSessionState.Connected)
            SocketSessionContext.OnDisconnecting();
        if (SocketSessionContext.SocketConnection?.IsConnected ?? false)
        {
            logger.Info("Connection client accepted session to {0} {1} id {2}:{3} closed",
                SocketSessionContext.Name, SocketSessionContext.Id,
                SocketSessionContext.NetworkTopicConnectionConfig,
                SocketSessionContext.SocketConnection.ConnectedPort);
            SocketSessionContext.SocketConnection.OSSocket?.Close();
            StopMessaging();
        }

        SocketSessionContext.OnDisconnected();
    }

    public override void OnSessionFailure(string reason)
    {
        logger.Warn("Problem communicating with client session {0} will not attempt reconnect. Reason {1}", SocketSessionContext, reason);
        Disconnect();
    }
}
