#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Logging;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public class SocketsPollerAndDecoding
{
    protected readonly IFLogger Logger;
    private readonly string name;
    private readonly IPerfLoggerPool receiveSocketDispatcherLatencyTraceLoggerPool;
    private readonly ISocketSelector selector;
    private readonly SocketBufferReadContext socketBufferReadContext = new();
    private readonly ISocketDataLatencyLogger? socketDataLatencyLogger;
    private readonly IIntraOSThreadSignal unpauseSignaller;

    public SocketsPollerAndDecoding(string name, ISocketSelector selector, IIntraOSThreadSignal unpauseSignaller)
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
        Logger = FLoggerFactory.Instance.GetLogger("FortitudeIO.Transports.Network.Dispatcher.SocketRingPollerListener." + name);
    }

    public int CountRegisteredReceivers => selector.CountRegisteredReceivers;

    public void AddForListen(ISocketReceiver receiver)
    {
        receiver.ListenActive = true;
        selector.Register(receiver);
        unpauseSignaller.Set();
    }

    public void RemoveFromListen(ISocketReceiver receiver)
    {
        if (selector.HasRegisteredReceiver(receiver) && selector.CountRegisteredReceivers == 1)
            unpauseSignaller.Reset();
        selector.Unregister(receiver);
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
            catch
            {
                // ignored
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
            try
            {
                socketBufferReadContext.DetectTimestamp = selector.WakeTs;
                connected = sockRecr.Poll(socketBufferReadContext);
            }
            catch (Exception ex)
            {
                Logger.Warn($"Caught exception attempting to read {sockRecr}. Got {ex}");
                detectionToPublishLatencyTraceLogger.Indent();
                detectionToPublishLatencyTraceLogger.Add("Read error: ", ex);
                detectionToPublishLatencyTraceLogger.Dedent();
                sockRecr.HandleReceiveError("Read error: ", ex);
                return;
            }

            if (connected) return;
            detectionToPublishLatencyTraceLogger.Indent();
            detectionToPublishLatencyTraceLogger.Add("Connection lost on SocketRingPoller " + name);
            detectionToPublishLatencyTraceLogger.Dedent();
            sockRecr.HandleReceiveError("Connection lost on SocketRingPoller " + name + $" with socketReceiver {sockRecr}"
                , new Exception("Connection Lost on SocketRingPoller " + name + $" with SocketReceiver {sockRecr}"));
        }
    }
}
