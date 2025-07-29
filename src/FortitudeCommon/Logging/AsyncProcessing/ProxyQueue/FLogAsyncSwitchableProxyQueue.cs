using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;

public class FLogAsyncSwitchableProxyQueue(int queueNumber, IFLogAsyncQueue backingQueue)
    : FLogAsyncQueue(queueNumber, backingQueue.QueueType, backingQueue.QueueCapacity), IReleaseBlockingDisposable, IFLogAsyncSwitchableQueueClient
{
    private ManualResetEvent blockPublishing = new (true);

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

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender)
    {
        blockPublishing.WaitOne();
        ActualQueue.FlushBufferToAppender(toFlush, fromAppender);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogAsyncTargetReceiveQueueAppender> appenders)
    {
        blockPublishing.WaitOne();
        ActualQueue.SendLogEntryEventTo(logEntryEvent, appenders);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogAsyncTargetReceiveQueueAppender appender)
    {
        blockPublishing.WaitOne();
        ActualQueue.SendLogEntryEventTo(logEntryEvent, appender);
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