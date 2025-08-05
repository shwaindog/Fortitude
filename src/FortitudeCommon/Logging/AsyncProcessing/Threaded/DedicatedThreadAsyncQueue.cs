using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.AsyncProcessing.Threaded;

public class DedicatedThreadAsyncQueue : FLogAsyncQueue
{
    private readonly EnumerableBatchPollingRing<FLogAsyncPayload> ring;

    private readonly FLogEventPoller ringPoller;

    public DedicatedThreadAsyncQueue
    (int queueNumber, AsyncProcessingType queueType, int queueCapacity
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

    public override int QueueBackLogSize => ring.Queued;

    public override void Execute(Action job)
    {
        var slot = ring.Claim();

        var flogAsyncPayload = ring[slot];
        flogAsyncPayload.SetAsClosureJob(job);
        ring.Publish(slot);
    }

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogBufferingFormatAppender fromAppender)
    {
        var slot = ring.Claim();

        var flogAsyncPayload = ring[slot];
        flogAsyncPayload.SetAsFlushAppenderBuffer(toFlush, fromAppender);
        ring.Publish(slot);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogEntrySink> logEntrySinks, ITargetingFLogEntrySource publishSource)
    {
        var slot = ring.Claim();

        var flogAsyncPayload = ring[slot];
        flogAsyncPayload.SetAsSendLogEntryEvent(logEntryEvent, logEntrySinks, publishSource);
        ring.Publish(slot);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogEntrySink logEntrySink, ITargetingFLogEntrySource publishSource)
    {
        var slot = ring.Claim();
        
        var flogAsyncPayload = ring[slot];
        flogAsyncPayload.SetAsSendLogEntryEvent(logEntryEvent, logEntrySink, publishSource);
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
