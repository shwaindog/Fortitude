#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Controls;

public class InitiateControls : SocketStreamControls
{
    private readonly object connSync = new();
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(InitiateControls));
    private readonly IOSParallelController parallelController;
    private IIntraOSThreadSignal? triggerConnectNowSignal;

    public InitiateControls(ISocketSessionContext socketSessionContext) : base(socketSessionContext) =>
        parallelController = socketSessionContext.SocketFactoryResolver.ParallelController!;

    public override bool Connect()
    {
        if (ConnectAttemptSucceeded()) return true;

        OnSessionFailure(
            $"'Connect' failed to successfully open connection to Topic.Name: '{SocketSessionContext.NetworkTopicConnectionConfig.TopicName}'");
        return false;
    }

    public override void OnSessionFailure(string reason)
    {
        Disconnect(true);
        var scheduleConnectWaitMs = ReconnectConfig.NextReconnectIntervalMs;
        logger.Info("Will attempt reconnecting to {0} {1} id {2} reason {3} in {4}ms",
            SocketSessionContext.Name, SocketSessionContext.Id,
            SocketSessionContext.NetworkTopicConnectionConfig, reason, scheduleConnectWaitMs);
        SocketSessionContext.OnReconnecting();
        ScheduleConnect(scheduleConnectWaitMs);
    }

    public override void Disconnect()
    {
        Disconnect(false);
    }


    protected override bool ConnectAttemptSucceeded()
    {
        lock (connSync)
        {
            triggerConnectNowSignal = null;
            if (SocketSessionContext.SocketConnection is { IsConnected: true } ||
                SocketSessionContext.SocketSessionState == SocketSessionState.Connecting ||
                SocketSessionContext.SocketSessionState == SocketSessionState.Disconnecting) return true;
            SocketSessionContext.OnSocketStateChanged(SocketSessionState.Connecting);
            var connConfig = SocketSessionContext.NetworkTopicConnectionConfig;
            var socketFactory = SocketSessionContext.SocketFactoryResolver.SocketFactory!;
            foreach (var socketConConfig in SocketSessionContext.NetworkTopicConnectionConfig)
            {
                try
                {
                    var subscriberSocket = socketFactory.Create(connConfig, socketConConfig);

                    var ipEndPoint = subscriberSocket.RemoteOrLocalIPEndPoint()!;

                    // will create socketSessionContext.SocketReceive and register listener with dispatcher
                    SocketSessionContext.OnConnected(new SocketConnection(connConfig.TopicName
                        , SocketSessionContext.ConversationType,
                        subscriberSocket, ipEndPoint.Address, (ushort)ipEndPoint.Port));
                }
                catch (Exception ex)
                {
                    logger.Info("Connection to {0} {1}:{2} rejected: {3}",
                        SocketSessionContext.Name, socketConConfig.Hostname, socketConConfig.Port, ex);
                }

                if (SocketSessionContext.SocketConnection?.IsConnected ?? false)
                {
                    ReconnectConfig.NextReconnectIntervalMs = ReconnectConfig.StartReconnectIntervalMs;
                    logger.Info("Connection to id:{0} {1}:{2}:{3} accepted",
                        SocketSessionContext.Name, SocketSessionContext.Id,
                        socketConConfig.Hostname, socketConConfig.Port);
                    StartMessaging();
                    return true;
                }
            }

            return false;
        }
    }


    private void ScheduleConnect(uint spanMs)
    {
        lock (connSync)
        {
            if (SocketSessionContext.SocketConnection is { IsConnected: true } ||
                SocketSessionContext.SocketSessionState == SocketSessionState.Connecting ||
                triggerConnectNowSignal != null) return;
            if (spanMs > 0)
                triggerConnectNowSignal = parallelController.ScheduleWithEarlyTrigger(
                    (state, timeout) => Connect(), spanMs);
            else parallelController.CallFromThreadPool(state => Connect());
        }
    }

    protected void Disconnect(bool inError)
    {
        if (SocketSessionContext.SocketReceiver != null)
            SocketSessionContext.SocketDispatcher.Listener?.UnregisterForListen(SocketSessionContext.SocketReceiver);
        lock (connSync)
        {
            if (!inError || SocketSessionContext.SocketSessionState == SocketSessionState.Connected ||
                SocketSessionContext.SocketSessionState == SocketSessionState.Connecting)
                SocketSessionContext.OnDisconnecting();
            if (SocketSessionContext.SocketConnection?.IsConnected ?? false)
            {
                triggerConnectNowSignal?.Set();
                StopMessaging();
                logger.Info("Connection to {0} {1} id {2}:{3} closed.",
                    SocketSessionContext.Name, SocketSessionContext.Id,
                    string.Join(", ", SocketSessionContext.NetworkTopicConnectionConfig.AvailableConnections),
                    SocketSessionContext.SocketConnection.ConnectedPort);
                SocketSessionContext.SocketConnection.OSSocket?.Close();
            }

            SocketSessionContext.OnDisconnected();
        }
    }
}
