using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;

public class FLogAsyncProxyQueue(int queueNumber, IFLogAsyncQueue backingQueue) 
    : FLogAsyncQueue(queueNumber, backingQueue.QueueType, backingQueue.QueueCapacity)
{
    public IFLogAsyncQueue ActualQueue { get; } = backingQueue;

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender)
    {
        ActualQueue.FlushBufferToAppender(toFlush, fromAppender);
    }

    public override int  QueueBackLogSize => ActualQueue.QueueBackLogSize;

    public override void SendLogEntriesTo(IReusableList<IFLogEntry> batchLogEntries, IFLogAppender appender)
    {
        ActualQueue.SendLogEntriesTo(batchLogEntries, appender);
    }

    public override void SendLogEntryTo(IFLogEntry logEntry, IFLogAppender appender)
    {
        ActualQueue.SendLogEntryTo(logEntry, appender);
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