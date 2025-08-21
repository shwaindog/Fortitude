using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
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
                SetCurrentThreadToQueueNumber(tpaQ.QueueNumber);
                while (tpaQ.threadPoolRequestQueue.TryPoll() is { } asyncPayload)
                {
                    asyncPayload.RunAsyncRequest();
                }
            }
        }
        finally
        {
            SetCurrentThreadToQueueNumber(0);
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

    public override void FlushBufferToAppender(IBufferedFormatWriter toFlush)
    {
        var slot = threadPoolRequestQueue.Claim();

        var flogAsyncPayload = threadPoolRequestQueue[slot];
        flogAsyncPayload.SetAsFlushAppenderBuffer(toFlush);
        threadPoolRequestQueue.Publish(slot);
        TryLaunchThreadPoolReceiver();
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IForkingFLogEntrySink> logEntrySinks, ITargetingFLogEntrySource publishSource)
    {
        var slot = threadPoolRequestQueue.Claim();

        var flogAsyncPayload = threadPoolRequestQueue[slot];
        flogAsyncPayload.SetAsSendLogEntryEvent(logEntryEvent, logEntrySinks, publishSource);
        threadPoolRequestQueue.Publish(slot);
        TryLaunchThreadPoolReceiver();
    }

    public override void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogEntrySink logEntrySink, ITargetingFLogEntrySource publishSource)
    {
        var slot = threadPoolRequestQueue.Claim();

        var flogAsyncPayload = threadPoolRequestQueue[slot];
        flogAsyncPayload.SetAsSendLogEntryEvent(logEntryEvent, logEntrySink, publishSource);
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
