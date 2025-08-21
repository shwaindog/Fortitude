using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;

public class FLogAsyncSwitchableProxyQueue(int queueNumber, IFLogAsyncQueue backingQueue)
    : FLogAsyncQueue(queueNumber, backingQueue.QueueType, backingQueue.QueueCapacity), IReleaseBlockingDisposable, IFLogAsyncSwitchableQueueClient
{
    private readonly ManualResetEvent blockPublishing = new (true);

    public bool IsBlocking { get; private set; }

    public IFLogAsyncQueue ActualQueue { get; private set; } = backingQueue;

    public override int  QueueBackLogSize => ActualQueue.QueueBackLogSize;

    public IReleaseBlockingDisposable StartSwitchQueue(IFLogAsyncQueue switchToQueue)
    {
        if (ReferenceEquals(ActualQueue, switchToQueue))
        {
            blockPublishing.Reset();
            IsBlocking  = true;
            ActualQueue = switchToQueue;
        }
        return this;
    }

    public override void Execute(Action job)
    {
        blockPublishing.WaitOne();
        ActualQueue.Execute(job);
    }

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush)
    {
        blockPublishing.WaitOne();
        ActualQueue.FlushBufferToAppender(toFlush);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IForkingFLogEntrySink> logEntrySinks, ITargetingFLogEntrySource publishSource)
    {
        blockPublishing.WaitOne();
        ActualQueue.SendLogEntryEventTo(logEntryEvent, logEntrySinks, publishSource);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogEntrySink logEntrySink, ITargetingFLogEntrySource publishSource)
    {
        blockPublishing.WaitOne();
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

    public void Dispose()
    {
        if (IsBlocking)
        {
            blockPublishing.Set();
            IsBlocking = false;
        }
    }
}