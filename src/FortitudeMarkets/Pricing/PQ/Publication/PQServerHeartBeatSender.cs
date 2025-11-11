// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Reflection;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeMarkets.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication;

public class PQServerHeartBeatSender : IPQServerHeartBeatSender
{
    private readonly ITimeContext timeContext;

    private const int HbFrequencyMs = 1000;
    private const int HbToleranceMs = 750;

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static IFLogger Logger
        = FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly IOSParallelController parallelController;

    private volatile bool hasStarted;

    private IOSThread? heartBeatOSThread;

    public PQServerHeartBeatSender(IOSParallelController? parallelController = null, ITimeContext? timeContext = null)
    {
        this.timeContext        = timeContext ?? TimeContext.Provider;
        this.parallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
    }

    public IPQUpdateServer? UpdateServer { get; set; }

    public IDoublyLinkedList<IPQMessage>? ServerLinkedQuotes { get; set; }

    public ISyncLock? ServerLinkedLock { get; set; }

    public bool HasStarted => hasStarted && UpdateServer != null && ServerLinkedLock != null && ServerLinkedQuotes != null;

    public void StartSendingHeartBeats()
    {
        hasStarted        = true;
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
        var lastRun = timeContext.UtcNow;
        var hbs     = new List<IPQMessage>();
        var hbm     = new PQHeartBeatQuotesMessage(hbs);
        while (HasStarted)
            try
            {
                var elapsedMs = (int)(timeContext.UtcNow - lastRun).TotalMilliseconds;
                if (elapsedMs < HbFrequencyMs) parallelController.Sleep(HbFrequencyMs - elapsedMs);
                lastRun = timeContext.UtcNow;
                if (hasStarted)
                {
                    hbs.Clear();
                    while (!ServerLinkedQuotes!.IsEmpty)
                    {
                        IPQMessage? tickInstant;
                        ServerLinkedLock!.Acquire();
                        try
                        {
                            if ((tickInstant = ServerLinkedQuotes.Head) == null
                             || (timeContext.UtcNow - tickInstant.LastPublicationTime).TotalMilliseconds <
                                HbToleranceMs)
                                break;
                            ServerLinkedQuotes.Remove(tickInstant);
                            ServerLinkedQuotes.AddLast(tickInstant);
                            tickInstant.LastPublicationTime = timeContext.UtcNow;
                        }
                        finally { ServerLinkedLock.Release(); }

                        hbs.Add(tickInstant);
                    }

                    if (hbs.Count > 0 && UpdateServer is { IsStarted: true }) UpdateServer.Send(hbm);
                }
            }
            catch (Exception ex) { Logger.Error("Unexpected error in heart beat sender: {0}", ex); }
    }
}
