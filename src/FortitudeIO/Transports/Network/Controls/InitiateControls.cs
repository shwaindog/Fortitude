#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Controls;

public class InitiateControls : SocketStreamControls
{
    private readonly object connSync = new();
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(InitiateControls));
    private readonly IOSParallelController parallelController;
    private bool shouldAttemptReconnect = true;
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
        Disconnect(CloseReason.StartConnectionFailed, reason);
        if (!shouldAttemptReconnect || SocketSessionContext.SocketReceiver?.ExpectSessionCloseMessage is
                { CloseReason: CloseReason.Completed }) return;
        var scheduleConnectWaitMs = ReconnectConfig.NextReconnectIntervalMs;
        logger.Info("Will attempt reconnecting to {0} {1} id {2} reason {3} in {4}ms",
            SocketSessionContext.Name, SocketSessionContext.Id,
            SocketSessionContext.NetworkTopicConnectionConfig, reason, scheduleConnectWaitMs);
        SocketSessionContext.OnReconnecting();
        ScheduleConnect(scheduleConnectWaitMs);
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
                    logger.Info("Connection attempt to {0} to host {1}:{2} was rejected: {3}",
                        SocketSessionContext.Name, socketConConfig.Hostname, socketConConfig.Port, ex);
                }

                if (SocketSessionContext.SocketConnection?.IsConnected ?? false)
                {
                    ReconnectConfig.NextReconnectIntervalMs = ReconnectConfig.StartReconnectIntervalMs;
                    logger.Info("Connection {0} was accepted by host {1}:{2}",
                        SocketSessionContext.Name, socketConConfig.Hostname, socketConConfig.Port);
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

    public override void Disconnect(CloseReason closeReason, string? reason = null)
    {
        if (closeReason is not CloseReason.StartConnectionFailed) shouldAttemptReconnect = false;
        if (SocketSessionContext.SocketReceiver != null)
            SocketSessionContext.SocketDispatcher.Listener?.UnregisterForListen(SocketSessionContext.SocketReceiver);
        SocketSessionContext.OnSocketStateChanged(SocketSessionState.Disconnecting);
        lock (connSync)
        {
            if (reason != null || SocketSessionContext.SocketSessionState == SocketSessionState.Connected ||
                SocketSessionContext.SocketSessionState == SocketSessionState.Connecting)
                SocketSessionContext.OnDisconnecting();
            if (SocketSessionContext.SocketConnection?.IsConnected ?? false)
            {
                SocketSessionContext.OnDisconnected(closeReason, reason);
                logger.Info("Connection to {0} closed. {1}", SocketSessionContext.Name, reason);
            }
        }

        if (closeReason == CloseReason.StartConnectionFailed) triggerConnectNowSignal?.Set();
    }
}
