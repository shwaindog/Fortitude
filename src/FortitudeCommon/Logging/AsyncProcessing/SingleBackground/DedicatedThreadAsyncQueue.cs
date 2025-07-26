using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.AsyncProcessing.SingleBackground;

public class DedicatedThreadAsyncQueue : FLogAsyncQueue
{
    private readonly EnumerableBatchPollingRing<FLogAsyncPayload> ring;

    private readonly FLogEventPoller ringPoller;

    public DedicatedThreadAsyncQueue(int queueNumber, AsyncProcessingType queueType, int queueCapacity
      , uint drainToEmptyTimeoutMs = 500) : base(queueNumber, queueType, queueCapacity)
    {
        ring = new EnumerableBatchPollingRing<FLogAsyncPayload>
            (
             "AsyncFLogger_Q_" + queueNumber,
             queueCapacity,
             () => new FLogAsyncPayload(),
             ClaimStrategyType.MultiProducers,
             false);
        ringPoller = new FLogEventPoller(ring, drainToEmptyTimeoutMs);
    }

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender)
    {
        var slot             = ring.Claim();
        var flogAsyncPayload = ring[slot];
        flogAsyncPayload.SetAsFlushAppenderBuffer(toFlush, fromAppender);
        ring.Publish(slot);
    }

    public override int QueueBackLogSize => ring.Queued;

    public override void SendLogEntriesTo(IReusableList<IFLogEntry> batchLogEntries, IFLogAppender appender)
    {
        var slot = ring.Claim();
        var flogAsyncPayload = ring[slot];
        flogAsyncPayload.SetAsSendBatchLogEntries(batchLogEntries, appender);
        ring.Publish(slot);
    }

    public override void SendLogEntryTo(IFLogEntry logEntry, IFLogAppender appender)
    {
        var slot             = ring.Claim();
        var flogAsyncPayload = ring[slot];
        flogAsyncPayload.SetAsSendLogEntry(logEntry, appender);
        ring.Publish(slot);
    }

    public override void StartQueueReceiver()
    {
        ringPoller.Start(SetThreadQueueNumber);
    }

    private void SetThreadQueueNumber()
    {
        SetCurrentThreadToQueueNumber(QueueNumber);
    }

    public override void StopQueueReceiver()
    {
        ringPoller.Stop();
        ringPoller.WaitUntilDrained();
    }
}
