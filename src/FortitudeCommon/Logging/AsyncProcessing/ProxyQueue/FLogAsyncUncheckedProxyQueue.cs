using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;

public class FLogAsyncUncheckedProxyQueue(int queueNumber, IFLogAsyncQueue backingQueue) 
    : FLogAsyncQueue(queueNumber, backingQueue.QueueType, backingQueue.QueueCapacity)
{
    public IFLogAsyncQueue ActualQueue { get; } = backingQueue;

    public override int  QueueBackLogSize => ActualQueue.QueueBackLogSize;

    public override void Execute(Action job)
    {
        ActualQueue.Execute(job);
    }

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogBufferingFormatAppender fromAppender)
    {
        ActualQueue.FlushBufferToAppender(toFlush, fromAppender);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogEntrySink> logEntrySinks, ITargetingFLogEntrySource publishSource)
    {
        ActualQueue.SendLogEntryEventTo(logEntryEvent, logEntrySinks, publishSource);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogEntrySink logEntrySink, ITargetingFLogEntrySource publishSource)
    {
        ActualQueue.SendLogEntryEventTo(logEntryEvent, logEntrySink, publishSource);
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