using FortitudeCommon.Chronometry;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Appending.Forwarding.Queues;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding;

public interface IFLoggerBufferingAppender : IFLoggerQueuingAppender
{
    int MaxBufferTimeMs    { get; }

    FLogLevel FlushFromLogLevel    { get; }
}

public interface IMutableFLoggerBufferingAppender : IFLoggerBufferingAppender
{
    new int MaxBufferTimeMs    { get; set; }

    new FLogLevel FlushFromLogLevel    { get; set; }
}

public class FLogBufferingAppender : FLogQueuingAppender, IMutableFLoggerBufferingAppender
{
    private Func<IFLogEntry, bool> queuedHasFlushLogLevel;

    public FLogBufferingAppender(IBufferingAppenderConfig bufferingAppenderConfig, IFLogContext context)
        : base(bufferingAppenderConfig, context)
    {
        queuedHasFlushLogLevel = IsAtOrGreaterThanLogLevel;

        AppenderQueue = CreateAppenderQueue(this);
    }

    protected override ILogEntryQueue CreateAppenderQueue(IMutableFLoggerQueuingAppender forAppender)
    {
        return null!;
    }

    protected SynchroniseQueue AsSynchroniseQueue => (SynchroniseQueue)AppenderQueue;

    public FLogLevel FlushFromLogLevel { get; set; }

    public int MaxBufferTimeMs { get; set; }

    protected bool IsAtOrGreaterThanLogLevel(IFLogEntry logEntry) => logEntry.LogLevel >= FlushFromLogLevel;

    public override void ForwardLogEntryTo(IFLogEntry logEntry)
    {
        logEntry.IncrementRefCount();
        
        var syncQueue = AsSynchroniseQueue;
        if (!syncQueue.TryEnqueue(logEntry))
        {
            while (syncQueue.Count > QueueReadBatchSize)
            {
                var batchList = AppenderLogEntryPool.SourceBatchLogEntryContainer(QueueReadBatchSize);
                syncQueue.PollBatch(QueueReadBatchSize, batchList);
                if (batchList.Any())
                {
                    Append(batchList);
                }
            }
            //if()
            syncQueue.Enqueue(logEntry);
        }
    }

    protected bool ShouldReadQueue
    {
        get
        {
            if (AppenderQueue.Count > QueueReadBatchSize) return true;
            var syncQueue        = AsSynchroniseQueue;
            var oldestQueuedTime = syncQueue.OldestQueued?.LogDateTime;
            if (oldestQueuedTime != null)
            {
                var now        = TimeContext.UtcNow;
                var durationMs = (now - oldestQueuedTime.Value).TotalMilliseconds;
                if(durationMs > MaxBufferTimeMs) return true;
            }
            return syncQueue.QueuedItemsAny(queuedHasFlushLogLevel);
        }
    }

    public override IBufferingAppenderConfig GetAppenderConfig() => (IBufferingAppenderConfig)AppenderConfig;
}
