using System;
using System.Collections.Generic;
using System.Reflection;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public class PQServerHeartBeatSender : IPQServerHeartBeatSender
    {
        private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const int HbFrequencyMs = 1000;
        private const int HbToleranceMs = 750;
        private volatile bool hasStarted;
        private IPQUpdateServer updateServer;

        private IDoublyLinkedList<IPQLevel0Quote> serverLinkedQuotes;
        private ISyncLock serverLinkedLock;
        private readonly IOSParallelController parallelController;
        private IOSThread heartBeatOSThread;

        public PQServerHeartBeatSender()
        {
            parallelController = OSParallelControllerFactory.Instance.GetOSParallelController;
        }

        public IPQUpdateServer UpdateServer
        {
            get => updateServer;
            set => updateServer = value;
        }

        public IDoublyLinkedList<IPQLevel0Quote> ServerLinkedQuotes
        {
            get => serverLinkedQuotes;
            set => serverLinkedQuotes = value;
        }

        public ISyncLock ServerLinkedLock
        {
            get => serverLinkedLock;
            set => serverLinkedLock = value;
        }

        internal void CheckPublishHeartbeats()
        {
            DateTime lastRun = TimeContext.UtcNow;
            var hbs = new List<IPQLevel0Quote>();
            var hbm = new PQHeartBeatQuotesMessage(hbs);
            while (HasStarted)
            {
                try
                {
                    int elapsedMs = (int)(TimeContext.UtcNow - lastRun).TotalMilliseconds;
                    if (elapsedMs < HbFrequencyMs)
                    {
                        parallelController.Sleep(HbFrequencyMs - elapsedMs);
                    }
                    lastRun = TimeContext.UtcNow;
                    if (hasStarted)
                    {
                        hbs.Clear();
                        while (!serverLinkedQuotes.IsEmpty)
                        {
                            IPQLevel0Quote level0Quote;
                            serverLinkedLock.Acquire();
                            try
                            {
                                if ((level0Quote = serverLinkedQuotes.Head) == null
                                    || (TimeContext.UtcNow - level0Quote.LastPublicationTime).TotalMilliseconds < HbToleranceMs)
                                {
                                    break;
                                }
                                serverLinkedQuotes.Remove(level0Quote);
                                serverLinkedQuotes.AddLast(level0Quote);
                                level0Quote.LastPublicationTime = TimeContext.UtcNow;
                            }
                            finally
                            {
                                serverLinkedLock.Release();
                            }
                            hbs.Add(level0Quote);
                        }
                        if (hbs.Count > 0 && updateServer != null && updateServer.IsConnected)
                        {
                            updateServer.Send(hbm);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Unexpected error in heart beat sender: {0}", ex);
                }
            }
        }

        public bool HasStarted => hasStarted && updateServer != null 
            && serverLinkedLock != null && serverLinkedQuotes != null;

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
            heartBeatOSThread.Join();
        }
    }
}
