using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Config.AsyncBuffering;
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



public class FLoggerBufferingAppender : FLoggerQueuingAppender, IMutableFLoggerBufferingAppender
{
    private Func<IFLogEntry, bool> queuedHasFlushLogLevel;

    public FLoggerBufferingAppender(IBufferingAppenderConfig bufferingAppenderConfig, IAppenderRegistry appenderRegistry)
        : base(bufferingAppenderConfig, appenderRegistry)
    {
        InboundQueueFullHandling = bufferingAppenderConfig.InboundQueueFullHandling;
        MaxDownstreamBatchSize   = bufferingAppenderConfig.MaxDownstreamBatchSize;
        QueueSize                = bufferingAppenderConfig.QueueSize;

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
            while (syncQueue.Count > MaxDownstreamBatchSize)
            {
                var batchList = AppenderLogEntryPool.SourceBatchLogEntryContainer(MaxDownstreamBatchSize);
                syncQueue.PollBatch(MaxDownstreamBatchSize, batchList);
                if (batchList.Any())
                {
                    AppendBatch(batchList);
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
            if (AppenderQueue.Count > MaxDownstreamBatchSize) return true;
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
}
