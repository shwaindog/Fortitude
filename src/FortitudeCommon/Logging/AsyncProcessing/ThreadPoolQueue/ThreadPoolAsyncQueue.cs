using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.AsyncProcessing.ThreadPoolQueue;

public class ThreadPoolAsyncQueue : FLogAsyncQueue
{
    private readonly AsyncPayloadQueue threadPoolRequestQueue;
    private readonly WaitCallback? pollQueueFromThreadPool;
    
    private PaddedLong runningReceiverToken = new (0);

    public ThreadPoolAsyncQueue(int queueNumber, int queueCapacity) : base(queueNumber, AsyncProcessingType.AsyncUsesThreadPool, queueCapacity)
    {
        threadPoolRequestQueue  = new AsyncPayloadQueue($"FLog_ThreadPool_Q_{queueNumber}", queueCapacity);
        pollQueueFromThreadPool = ThreadPoolExecution;
    }

    public override int  QueueBackLogSize => threadPoolRequestQueue.Count;

    private void ThreadPoolExecution(object? stateMySelf)
    {
        try
        {
            if (stateMySelf is ThreadPoolAsyncQueue tpaQ)
            {
                while (tpaQ.threadPoolRequestQueue.TryPoll() is { } asyncPayload)
                {
                    asyncPayload.RunAsyncRequest();
                }
            }
        }
        finally
        {
            Thread.VolatileWrite(ref runningReceiverToken.Value, 0);
        }
    }

    private void TryLaunchThreadPoolReceiver()
    {
        if (Interlocked.CompareExchange(ref runningReceiverToken.Value, 1, 0) == 0 )
        {
            ThreadPool.QueueUserWorkItem(pollQueueFromThreadPool!, this);
        }
    }
    
    public override void Execute(Action job)
    {
        var slot = threadPoolRequestQueue.Claim();

        var flogAsyncPayload = threadPoolRequestQueue[slot];
        flogAsyncPayload.SetAsClosureJob(job);
        threadPoolRequestQueue.Publish(slot);
        TryLaunchThreadPoolReceiver();
    }

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender)
    {
        var slot = threadPoolRequestQueue.Claim();

        var flogAsyncPayload = threadPoolRequestQueue[slot];
        flogAsyncPayload.SetAsFlushAppenderBuffer(toFlush, fromAppender);
        threadPoolRequestQueue.Publish(slot);
        TryLaunchThreadPoolReceiver();
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogAsyncTargetReceiveQueueAppender> appenders)
    {
        var slot = threadPoolRequestQueue.Claim();

        var flogAsyncPayload = threadPoolRequestQueue[slot];
        flogAsyncPayload.SetAsSendLogEntryEvent(logEntryEvent, appenders);
        threadPoolRequestQueue.Publish(slot);
        TryLaunchThreadPoolReceiver();
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogAsyncTargetReceiveQueueAppender appender)
    {
        var slot = threadPoolRequestQueue.Claim();

        var flogAsyncPayload = threadPoolRequestQueue[slot];
        flogAsyncPayload.SetAsSendLogEntryEvent(logEntryEvent, appender);
        threadPoolRequestQueue.Publish(slot);
        TryLaunchThreadPoolReceiver();
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
