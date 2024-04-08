#region

using System.Reflection;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public class PQServerHeartBeatSender : IPQServerHeartBeatSender
{
    private const int HbFrequencyMs = 1000;
    private const int HbToleranceMs = 750;

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static IFLogger Logger
        = FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly IOSParallelController parallelController;
    private volatile bool hasStarted;
    private IOSThread? heartBeatOSThread;

    public PQServerHeartBeatSender() => parallelController = OSParallelControllerFactory.Instance.GetOSParallelController;

    public IPQUpdateServer? UpdateServer { get; set; }

    public IDoublyLinkedList<IPQLevel0Quote>? ServerLinkedQuotes { get; set; }

    public ISyncLock? ServerLinkedLock { get; set; }

    public bool HasStarted =>
        hasStarted && UpdateServer != null
                   && ServerLinkedLock != null && ServerLinkedQuotes != null;

    public void StartSendingHeartBeats()
    {
        hasStarted = true;
        heartBeatOSThread = parallelController.CreateNewOSThread(CheckPublishHeartbeats);
        heartBeatOSThread.IsBackground = true;
        heartBeatOSThread.Start();
    }

    public void StopAndWaitUntilFinished()
    {
        hasStarted = false;
        heartBeatOSThread?.Join();
    }

    internal void CheckPublishHeartbeats()
    {
        var lastRun = TimeContext.UtcNow;
        var hbs = new List<IPQLevel0Quote>();
        var hbm = new PQHeartBeatQuotesMessage(hbs);
        while (HasStarted)
            try
            {
                var elapsedMs = (int)(TimeContext.UtcNow - lastRun).TotalMilliseconds;
                if (elapsedMs < HbFrequencyMs) parallelController.Sleep(HbFrequencyMs - elapsedMs);
                lastRun = TimeContext.UtcNow;
                if (hasStarted)
                {
                    hbs.Clear();
                    while (!ServerLinkedQuotes!.IsEmpty)
                    {
                        IPQLevel0Quote? level0Quote;
                        ServerLinkedLock!.Acquire();
                        try
                        {
                            if ((level0Quote = ServerLinkedQuotes.Head) == null
                                || (TimeContext.UtcNow - level0Quote.LastPublicationTime).TotalMilliseconds <
                                HbToleranceMs)
                                break;
                            ServerLinkedQuotes.Remove(level0Quote);
                            ServerLinkedQuotes.AddLast(level0Quote);
                            level0Quote.LastPublicationTime = TimeContext.UtcNow;
                        }
                        finally
                        {
                            ServerLinkedLock.Release();
                        }

                        hbs.Add(level0Quote);
                    }

                    if (hbs.Count > 0 && UpdateServer is { IsStarted: true }) UpdateServer.Send(hbm);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Unexpected error in heart beat sender: {0}", ex);
            }
    }
}
