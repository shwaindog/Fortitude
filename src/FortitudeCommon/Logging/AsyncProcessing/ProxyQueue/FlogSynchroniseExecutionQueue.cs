using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending;
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

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender)
    {
        fromAppender.FlushBufferToAppender(toFlush);
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogAsyncTargetReceiveQueueAppender> appenders)
    {
        for (var i = 0; i < appenders.Count; i++)
        {
            var appender = appenders[i];
            appender.ProcessReceivedLogEntryEvent(logEntryEvent);
        }
    }


    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogAsyncTargetReceiveQueueAppender appender)
    {
        appender.ProcessReceivedLogEntryEvent(logEntryEvent);
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
