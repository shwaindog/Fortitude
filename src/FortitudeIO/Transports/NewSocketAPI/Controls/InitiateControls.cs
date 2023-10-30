#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Controls;

public interface IInitiateControls : IStreamControls
{
    void ConnectAsync();
}

public class InitiateControls : IInitiateControls
{
    private readonly object connSync = new();
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(InitiateControls));
    private readonly IOSParallelController parallelController;
    private readonly uint reconnectIntervalMs;
    private readonly ISocketSessionContext socketSessionContext;
    private IIntraOSThreadSignal? triggerConnectNowSignal;

    public InitiateControls(ISocketSessionContext socketSessionContext,
        uint reconnectIntervalMs = 1_000)
    {
        parallelController = socketSessionContext.SocketFactories.ParallelController!;
        this.socketSessionContext = socketSessionContext;
        this.reconnectIntervalMs = reconnectIntervalMs;
    }

    public virtual void StartMessaging()
    {
        socketSessionContext.SocketDispatcher.Start();
    }

    public virtual void StopMessaging()
    {
        socketSessionContext.SocketDispatcher.Stop();
    }

    public virtual void ConnectAsync()
    {
        ScheduleConnect(0);
    }

    public void Connect()
    {
        socketSessionContext.OnSocketStateChanged(SocketSessionState.Connecting);
        lock (connSync)
        {
            triggerConnectNowSignal = null;
            if ((socketSessionContext.SocketConnection != null && socketSessionContext.SocketConnection.IsConnected) ||
                socketSessionContext.SocketSessionState != SocketSessionState.Connecting) return;
            var connConfig = socketSessionContext.SocketConnectionConfig;
            var maxAttempts = Math.Max(connConfig.PortEndRange - connConfig.PortStartRange, 1);
            var socketFactory = socketSessionContext.SocketFactories.SocketFactory;
            for (ushort attemptNum = 0; attemptNum < maxAttempts; attemptNum++)
            {
                var port = socketFactory!.GetPort(connConfig, attemptNum);
                try
                {
                    var subscriberSocket = socketFactory.Create(
                        socketSessionContext.SocketConversationProtocol,
                        socketSessionContext.SocketConnectionConfig, attemptNum);

                    var ipEndPoint = subscriberSocket.RemoteOrLocalIPEndPoint()!;

                    // will create socketSessionContext.SocketReceive and register listener with dispatcher
                    socketSessionContext.OnConnected(new SocketConnection(connConfig.InstanceName
                        , socketSessionContext.ConversationType,
                        subscriberSocket, ipEndPoint.Address, (ushort)ipEndPoint.Port));
                }
                catch (Exception ex)
                {
                    logger.Info("Connection to {0} {1}:{2} rejected: {3}",
                        socketSessionContext.ConversationDescription, connConfig.Hostname, port, ex);
                }

                if (socketSessionContext.SocketConnection?.IsConnected ?? false)
                {
                    logger.Info("Connection to id:{0} {1}:{2} accepted",
                        socketSessionContext.ConversationDescription, socketSessionContext.Id,
                        connConfig.Hostname, port);
                    StartMessaging();
                    break;
                }
            }

            if (socketSessionContext.SocketConnection?.IsConnected ?? false) return;
            Disconnect(true);
            ScheduleConnect(reconnectIntervalMs);
        }
    }

    public void Disconnect()
    {
        Disconnect(false);
    }

    private void ScheduleConnect(uint spanMs)
    {
        lock (connSync)
        {
            if ((socketSessionContext.SocketConnection != null && socketSessionContext.SocketConnection.IsConnected) ||
                socketSessionContext.SocketSessionState == SocketSessionState.Connecting ||
                triggerConnectNowSignal != null) return;
            socketSessionContext.OnSocketStateChanged(SocketSessionState.Connecting);
            if (spanMs > 0)
                triggerConnectNowSignal = parallelController.ScheduleWithEarlyTrigger(
                    (state, timeout) => Connect(), spanMs);
            else parallelController.CallFromThreadPool(state => Connect());
        }
    }

    protected void Disconnect(bool inError)
    {
        lock (connSync)
        {
            if (socketSessionContext.SocketConnection?.IsConnected ?? false)
            {
                if (!inError || socketSessionContext.SocketSessionState == SocketSessionState.Connecting)
                    socketSessionContext.OnDisconnecting();
                triggerConnectNowSignal?.Set();
                StopMessaging();
                logger.Info("Connection to {0} {1} id {2}:{3} closed",
                    socketSessionContext.ConversationDescription, socketSessionContext.Id,
                    socketSessionContext.SocketConnectionConfig.Hostname,
                    socketSessionContext.SocketConnection.ConnectedPort);
                socketSessionContext.SocketConnection.OSSocket?.Close();
                socketSessionContext.OnDisconnected();
            }
        }
    }
}
