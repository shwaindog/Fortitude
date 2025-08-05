using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;

internal class FlogSynchroniseExecutionQueue(int queueNumber)
    : FLogAsyncQueue(queueNumber, AsyncProcessingType.Synchronise, 0)
{
    public override int QueueBackLogSize => 0;

    public override void Execute(Action job)
    {
        job();
    }

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogBufferingFormatAppender fromAppender)
    {
        fromAppender.FlushBufferToAppender(toFlush);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogEntrySink> logEntrySinks, ITargetingFLogEntrySource publishSource)
    {
        for (var i = 0; i < logEntrySinks.Count; i++)
        {
            var logEntrySink = logEntrySinks[i];
            logEntrySink.InBoundListener(logEntryEvent, publishSource);
        }
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogEntrySink logEntrySink, ITargetingFLogEntrySource publishSource)
    {
        logEntrySink.InBoundListener(logEntryEvent, publishSource);
    }

    public override void StartQueueReceiver()
    {
        // never start or stop
    }

    public override void StopQueueReceiver()
    {
        // never start or stop
    }
}
