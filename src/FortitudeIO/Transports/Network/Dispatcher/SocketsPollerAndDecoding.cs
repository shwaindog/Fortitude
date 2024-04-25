#region

using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Logging;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public class SocketsPollerAndDecoding
{
    private readonly ActionListTimer actionListTimer;
    protected readonly IFLogger Logger;
    private readonly string name;
    private readonly IPerfLoggerPool receiveSocketDispatcherLatencyTraceLoggerPool;
    private readonly ISocketSelector selector;
    private readonly SocketBufferReadContext socketBufferReadContext = new();
    private readonly ISocketDataLatencyLogger? socketDataLatencyLogger;
    private readonly List<ITimerCallbackPayload> timerActionsToExecute = new();
    private readonly IIntraOSThreadSignal unpauseSignaller;

    public SocketsPollerAndDecoding(string name, ISocketSelector selector, IIntraOSThreadSignal unpauseSignaller, ActionListTimer actionListTimer)
    {
        this.name = name;
        this.selector = selector;
        this.unpauseSignaller = unpauseSignaller;
        socketDataLatencyLogger = SocketDataLatencyLoggerFactory.Instance.GetSocketDataLatencyLogger(name);
        receiveSocketDispatcherLatencyTraceLoggerPool =
            PerfLoggingPoolFactory.Instance
                .GetLatencyTracingLoggerPool(name + ".Receive",
                    //  Heartbeats are normally set at 1 second so wait just over 1 second on select.
                    TimeSpan.FromMilliseconds(1100), typeof(ISocketDispatcher));
        this.actionListTimer = actionListTimer;
        Logger = FLoggerFactory.Instance.GetLogger("FortitudeIO.Transports.Network.Dispatcher.SocketRingPollerListener." + name);
    }

    public int CountRegisteredReceivers => selector.CountRegisteredReceivers;

    public void AddForListen(ISocketReceiver receiver)
    {
        receiver.ListenActive = true;
        receiver.ResponseTimer = actionListTimer;
        selector.Register(receiver);
        unpauseSignaller.Set();
    }

    public void RemoveFromListen(ISocketReceiver receiver)
    {
        if (selector.HasRegisteredReceiver(receiver) && selector.CountRegisteredReceivers == 1)
            unpauseSignaller.Reset();
        selector.Unregister(receiver);
        receiver.ResponseTimer = null;
        if (selector.CountRegisteredReceivers > 0) unpauseSignaller.Set();
    }


    public void PollSocketsAndDecodeData()
    {
        if (selector.CountRegisteredReceivers > 0)
        {
            IPerfLogger? detectionToPublishLatencyTraceLogger = null;
            var numSockets = 0;
            try
            {
                detectionToPublishLatencyTraceLogger = receiveSocketDispatcherLatencyTraceLoggerPool.StartNewTrace();
                socketBufferReadContext.DispatchLatencyLogger = detectionToPublishLatencyTraceLogger;
                numSockets = 0;
                var socketReceivers = selector.WatchSocketsForRecv(detectionToPublishLatencyTraceLogger);
                foreach (var sockRecr in socketReceivers)
                {
                    numSockets++;
                    ProcessSocketEvent(sockRecr, detectionToPublishLatencyTraceLogger);
                }
            }
            catch (Exception ex)
            {
                Logger.Warn("SocketsPollerAndDecoding Caught exception {0} with the following registered SocketReceivers [{1}]", ex,
                    string.Join(", ", selector.AllRegisteredReceivers));
            }
            finally
            {
                if (detectionToPublishLatencyTraceLogger != null)
                {
                    detectionToPublishLatencyTraceLogger.AddContextMeasurement(numSockets);
                    detectionToPublishLatencyTraceLogger.Add("End Processing Socket Data");
                    socketDataLatencyLogger?.ParseTraceLog(detectionToPublishLatencyTraceLogger);
                    receiveSocketDispatcherLatencyTraceLoggerPool.StopTrace(detectionToPublishLatencyTraceLogger);
                }
            }
        }

        actionListTimer.GetTimerActionsToExecute(timerActionsToExecute);
        foreach (var timerCallbackPayload in timerActionsToExecute)
        {
            if (timerCallbackPayload.IsAsyncInvoke())
                timerCallbackPayload.InvokeAsync();
            else
                timerCallbackPayload.Invoke();
            timerCallbackPayload?.DecrementRefCount();
        }
    }

    private void ProcessSocketEvent(ISocketReceiver sockRecr, IPerfLogger detectionToPublishLatencyTraceLogger)
    {
        if (sockRecr.IsAcceptor)
        {
            sockRecr.NewClientSocketRequest();
        }
        else
        {
            bool connected;
            string? exceptionMessage = null;
            try
            {
                socketBufferReadContext.DetectTimestamp = selector.WakeTs;
                socketBufferReadContext.SocketReceiver = sockRecr;
                connected = sockRecr.Poll(socketBufferReadContext);
            }
            catch (Exception ex)
            {
                connected = false;
                exceptionMessage = ex.Message;
            }

            if (connected && sockRecr.ExpectSessionCloseMessage == null) return;
            var closeReason = sockRecr.Name + " had " + (sockRecr.ExpectSessionCloseMessage?.ReasonText ?? exceptionMessage ?? "");
            if (sockRecr.ExpectSessionCloseMessage?.CloseReason is not CloseReason.Completed)
            {
                detectionToPublishLatencyTraceLogger.Indent();
                detectionToPublishLatencyTraceLogger.Add("Connection lost on SocketRingPoller.");
                detectionToPublishLatencyTraceLogger.Dedent();
                sockRecr.HandleReceiveError("Connection lost on SocketRingPoller " + name + $" with socketReceiver {sockRecr.Name}. {closeReason}"
                    , new Exception("Connection lost on SocketRingPoller " + name + $" with SocketReceiver {sockRecr.Name}"));
            }
            else
            {
                sockRecr.HandleRemoteDisconnecting(sockRecr.ExpectSessionCloseMessage!);
            }
        }
    }
}
