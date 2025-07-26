using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;

internal class FlogSynchroniseExecutionQueue(int queueNumber) 
: FLogAsyncQueue(queueNumber, AsyncProcessingType.Synchronise, 0)
{

public override void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender)
{
    fromAppender.FlushBufferToAppender(toFlush);
}

public override int  QueueBackLogSize => 0;

public override void SendLogEntriesTo(IReusableList<IFLogEntry> batchLogEntries, IFLogAppender appender)
{
    appender.Append(batchLogEntries);
}

public override void SendLogEntryTo(IFLogEntry logEntry, IFLogAppender appender)
{
    appender.Append(logEntry);
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