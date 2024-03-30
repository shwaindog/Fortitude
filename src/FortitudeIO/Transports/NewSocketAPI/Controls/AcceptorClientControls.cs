#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Controls;

public class AcceptorClientControls : IInitiateControls
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(AcceptorClientControls));

    private ISocketSessionContext socketSessionContext;

    public AcceptorClientControls(ISocketSessionContext sSocketSessionContext) => socketSessionContext = sSocketSessionContext;

    public void Start()
    {
        logger.Warn("Can NOT start client acceptor session {0} will not attempt start.", socketSessionContext);
    }

    public void Stop()
    {
        Disconnect();
    }

    public void StartAsync()
    {
        Start();
    }

    public void Connect()
    {
        logger.Warn("Can NOT connect to client acceptor session {0} will not attempt connect.", socketSessionContext);
    }

    public void Disconnect()
    {
        if (socketSessionContext.SocketSessionState == SocketSessionState.Connected)
            socketSessionContext.OnDisconnecting();
        if (socketSessionContext.SocketConnection?.IsConnected ?? false)
        {
            logger.Info("Connection client acceptor session to {0} {1} id {2}:{3} closed",
                socketSessionContext.Name, socketSessionContext.Id,
                socketSessionContext.NetworkTopicConnectionConfig,
                socketSessionContext.SocketConnection.ConnectedPort);
            socketSessionContext.SocketConnection.OSSocket?.Close();
        }

        socketSessionContext.OnDisconnected();
    }

    public void StartMessaging() { }

    public void StopMessaging() { }

    public void OnSessionFailure(string reason)
    {
        logger.Warn("Problem communicating with client session {0} will not attempt reconnect. Reason {1}", socketSessionContext, reason);
        Disconnect();
    }
}
