#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Controls;

public interface IStreamControls : IConversationInitiator
{
    ValueTask<bool> StartAsync(TimeSpan timeoutTimeSpan, IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null);
    ValueTask<bool> StartAsync(int timeoutMs, IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null);
    bool Connect();
    void Disconnect(CloseReason closeReason, string? reason = null);
    void StartMessaging();
    void StopMessaging();
}

public abstract class SocketStreamControls : IStreamControls
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SocketStreamControls));
    protected readonly ISocketReconnectConfig ReconnectConfig;
    protected bool HaveStartedMessaging;
    protected ISocketSessionContext SocketSessionContext;

    protected SocketStreamControls(ISocketSessionContext socketSessionContext)
    {
        SocketSessionContext = socketSessionContext;
        ReconnectConfig = socketSessionContext.NetworkTopicConnectionConfig.ReconnectConfig;
    }

    public abstract bool Connect();

    public abstract void OnSessionFailure(string reason);

    public virtual ValueTask<bool> StartAsync(TimeSpan timeoutTimeSpan
        , IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null)
    {
        if (alternativeExecutionContext == null)
            return ImmediateConnectAsync(timeoutTimeSpan);
        else
            try
            {
                return alternativeExecutionContext.Execute(ImmediateConnectAsync, timeoutTimeSpan);
            }
            finally
            {
                alternativeExecutionContext.DecrementRefCount();
            }
    }

    public virtual ValueTask<bool>
        StartAsync(int timeoutMs, IAlternativeExecutionContextResult<bool, TimeSpan>? alternativeExecutionContext = null) =>
        StartAsync(TimeSpan.FromMilliseconds(timeoutMs), alternativeExecutionContext);

    public void Start()
    {
        if (SocketSessionContext.SocketConnection is not { IsConnected: true }) Connect();
        StartMessaging();
    }

    public void Stop(CloseReason closeReason = CloseReason.Completed, string? reason = null)
    {
        if (SocketSessionContext.SocketConnection is { IsConnected: true }) Disconnect(closeReason, reason);
        StopMessaging();
    }

    public virtual void StartMessaging()
    {
        if (!HaveStartedMessaging && SocketSessionContext.SocketConnection is { IsConnected: true })
        {
            Logger.Info("Starting messaging for {0}", SocketSessionContext.Name);
            HaveStartedMessaging = true;
            if (SocketSessionContext.SocketReceiver != null)
                SocketSessionContext.SocketDispatcher.Listener?.RegisterForListen(SocketSessionContext.SocketReceiver);
            SocketSessionContext.SocketDispatcher.Start();
            SocketSessionContext.OnStarted();
        }
    }

    public virtual void StopMessaging()
    {
        if (!HaveStartedMessaging) return;
        HaveStartedMessaging = false;
        SocketSessionContext.SocketDispatcher.Stop();
        SocketSessionContext.OnStopped();
    }

    public abstract void Disconnect(CloseReason closeReason, string? reason = null);

    protected abstract bool ConnectAttemptSucceeded();


    private async ValueTask<bool> ImmediateConnectAsync(TimeSpan timeoutTimeSpan)
    {
        var startTime = TimeContext.UtcNow;
        while (startTime + timeoutTimeSpan > TimeContext.UtcNow)
        {
            if (ConnectAttemptSucceeded()) return true;
            var scheduleConnectWaitMs = ReconnectConfig.NextReconnectIntervalMs;
            Logger.Warn("Failed to connect to {0} {1} id {2} will attempt reconnect in {3}ms",
                SocketSessionContext.Name, SocketSessionContext.Id,
                SocketSessionContext.NetworkTopicConnectionConfig, scheduleConnectWaitMs);
            await Task.Delay((int)scheduleConnectWaitMs);
        }

        throw new ConnectionTimeoutException(
            $"Timed out attempting to connect to Topic {SocketSessionContext.NetworkTopicConnectionConfig.TopicName}." +
            $"  Timeout {timeoutTimeSpan.TotalMilliseconds}ms", SocketSessionContext);
    }
}
