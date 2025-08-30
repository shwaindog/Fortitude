// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Appending.Forwarding.Queues;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding;

public interface IFLogBufferingAppender : IFLogQueuingAppender
{
    int MaxBufferTimeMs { get; }

    FLogLevel FlushFromLogLevel { get; }
}

public interface IMutableFLogBufferingAppender : IFLogBufferingAppender
{
    new int MaxBufferTimeMs { get; set; }

    new FLogLevel FlushFromLogLevel { get; set; }
}

public class FLogBufferingAppender : FLogQueuingAppender, IMutableFLogBufferingAppender
{
    private readonly Predicate<IFLogEntry> queuedHasFlushLogLevel;

    public FLogBufferingAppender(IBufferingAppenderConfig bufferingAppenderConfig, IFLogContext context)
        : base(bufferingAppenderConfig, context) =>
        queuedHasFlushLogLevel = IsAtOrGreaterThanLogLevel;

    protected FlogEntryBufferQueue AsFlogEntryBufferQueue => (FlogEntryBufferQueue)AppenderLogEntryQueue;


    protected bool ShouldReadQueue
    {
        get
        {
            if (AppenderLogEntryQueue.Count > QueueReadBatchSize) return true;
            var syncQueue        = AsFlogEntryBufferQueue;
            var oldestQueuedTime = syncQueue.OldestQueued?.LogDateTime;
            if (oldestQueuedTime != null)
            {
                var now        = TimeContext.UtcNow;
                var durationMs = (now - oldestQueuedTime.Value).TotalMilliseconds;
                if (durationMs > MaxBufferTimeMs) return true;
            }
            return syncQueue.QueuedItemsAny(queuedHasFlushLogLevel);
        }
    }

    public FLogLevel FlushFromLogLevel { get; set; }

    public int MaxBufferTimeMs { get; set; }

    protected override ILogEntryQueue CreateAppenderQueue(IMutableFLogQueuingAppender forAppender) => null!;

    protected override DrainQueueResult AmountToDrain(IFLogEntry flogEntry)
    {
        if (IsAtOrGreaterThanLogLevel(flogEntry)) return DrainQueueResult.DrainToEmpty(AppenderLogEntryQueue.Count);
        if (LastDrainTime.AddMilliseconds(MaxBufferTimeMs) < TimeContext.UtcNow) return DrainQueueResult.DrainToEmpty(AppenderLogEntryQueue.Count);
        return base.AmountToDrain(flogEntry);
    }

    protected bool IsAtOrGreaterThanLogLevel(IFLogEntry logEntry) => logEntry.LogLevel >= FlushFromLogLevel;

    public override void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent)
    {
        if (logEntryEvent.LogEntryEventType == LogEntryEventType.SingleEntry)
            AddLogEntryToBuffer(logEntryEvent.LogEntry!);
        else
            for (var i = 0; i < logEntryEvent.LogEntriesBatch!.Count; i++)
            {
                var flogEntry = logEntryEvent.LogEntriesBatch![i];
                AddLogEntryToBuffer(flogEntry);
            }
    }

    public override IBufferingAppenderConfig GetAppenderConfig() => (IBufferingAppenderConfig)AppenderConfig;
}
