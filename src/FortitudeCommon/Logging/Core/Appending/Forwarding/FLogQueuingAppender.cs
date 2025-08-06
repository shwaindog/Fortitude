// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Appending.Forwarding.Queues;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;
using FortitudeCommon.Logging.Core.Pooling;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding;

public interface IFLogQueuingAppender : IFLogForwardingAppender
{
    FullQueueHandling InboundQueueFullHandling { get; }

    int QueueReadBatchSize { get; }

    int QueueSize { get; }
}

public interface IMutableFLogQueuingAppender : IFLogQueuingAppender
{
    new FullQueueHandling InboundQueueFullHandling { get; set; }

    new int QueueReadBatchSize { get; set; }

    new int QueueSize { get; set; }
}

public abstract class FLogQueuingAppender : FLogForwardingAppender, IMutableFLogQueuingAppender
{
    private static readonly IFLogger Logger = FLog.FLoggerForType;

    private static readonly TimeSpan OneMillisecond = TimeSpan.FromMilliseconds(1);

    protected ILogEntryQueue AppenderLogEntryQueue;

    protected FLogEntryPool AppenderLogEntryPool;

    protected Action<ILogEntryQueue> SwapFilteredBack;

    private static readonly object FullQueueSyncLock = new();

    protected DateTime LastDrainTime = DateTime.MinValue;

    private PaddedLong drainToken = new(0);

    private readonly ManualResetEvent drainComplete = new(true);

    private IntervalMatchAll      intervalMatchAllEntries   = null!;
    private IntervalMatchAllDebug intervalMatchDebugOrLower = null!;
    private IntervalMatchAllInfo  intervalMatchInfoOrLower  = null!;

    protected FLogQueuingAppender(IQueueingAppenderConfig queuingAppenderConfig, IFLogContext context)
        : base(queuingAppenderConfig, context)
    {
        InboundQueueFullHandling = queuingAppenderConfig.InboundQueue.QueueFullHandling;
        QueueReadBatchSize       = queuingAppenderConfig.InboundQueue.QueueReadBatchSize;
        QueueSize                = queuingAppenderConfig.InboundQueue.QueueSize;

        AppenderLogEntryPool =
            context.LogEntryPoolRegistry.ResolveFLogEntryPool
                (queuingAppenderConfig.InboundQueue.LogEntryPool ?? context.LogEntryPoolRegistry.LogEntryPoolInitConfig.AppendersGlobalLogEntryPool);

        var entryPoolBatchSize = AppenderLogEntryPool.NewBatchLogEntryListSize;
        var queueBatchSize     = queuingAppenderConfig.InboundQueue.QueueReadBatchSize;
        if (entryPoolBatchSize < queueBatchSize)
        {
            AppenderLogEntryPool.NewBatchLogEntryListSize = entryPoolBatchSize;
        }

        DropInterval = queuingAppenderConfig.InboundQueue.QueueDropInterval;

        AppenderLogEntryQueue = CreateAppenderQueue(this);

        SwapFilteredBack = filtered => AppenderLogEntryQueue = filtered;
    }

    protected abstract ILogEntryQueue CreateAppenderQueue(IMutableFLogQueuingAppender forAppender);

    public FullQueueHandling InboundQueueFullHandling { get; set; }

    public TimeSpan EnqueueAttemptTimeout { get; set; }

    public TimeSpan EnqueueAttemptInterval { get; set; }

    public int DropInterval
    {
        get => dropInterval;
        set
        {
            if (dropInterval == value) return;
            dropInterval              = value;
            intervalMatchAllEntries   = new IntervalMatchAll(dropInterval);
            intervalMatchDebugOrLower = new IntervalMatchAllDebug(dropInterval);
            intervalMatchInfoOrLower  = new IntervalMatchAllInfo(dropInterval);
        }
    }

    protected record struct DrainQueueResult(bool ShouldStartDrain, int DrainAmount, bool ShouldEmptyBuffer)
    {
        public static DrainQueueResult NotRequired = new(false, 0, false);
        public static DrainQueueResult OneBatchReadSize(int batchReadSize) => new(true, batchReadSize, false);
        public static DrainQueueResult DrainToEmpty(int queueSize)         => new(true, queueSize, true);
    };

    protected virtual DrainQueueResult AmountToDrain(IFLogEntry flogEntry)
    {
        return QueueSize > QueueReadBatchSize
            ? DrainQueueResult.OneBatchReadSize(QueueReadBatchSize)
            : DrainQueueResult.NotRequired;
    }

    public override void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent)
    {
        if (logEntryEvent.LogEntryEventType == LogEntryEventType.SingleEntry)
        {
            AddLogEntryToBuffer(logEntryEvent.LogEntry!);
        }
        else if (logEntryEvent.LogEntryEventType == LogEntryEventType.BatchEntries)
        {
            var batch = logEntryEvent.LogEntriesBatch!;
            for (int i = 0; i < batch.Count; i++)
            {
                var logEntry = batch[i];
                AddLogEntryToBuffer(logEntry);
            }
        }
    }

    protected virtual void AddLogEntryToBuffer(IFLogEntry flogEntry)
    {
        flogEntry.IncrementRefCount();

        if (WaitForDrainedSignal)
        {
            drainComplete.WaitOne();
        }
        if (!AppenderLogEntryQueue.TryEnqueue(flogEntry))
        {
            HandleFirstEnqueueFailed(flogEntry);
        }
        var drainAmount = AmountToDrain(flogEntry);
        if (drainAmount.ShouldStartDrain)
        {
            TryDrainAmount(drainAmount);
        }
    }

    private void HandleFirstEnqueueFailed(IFLogEntry flogEntry)
    {
        var queue = AppenderLogEntryQueue;
        Thread.Yield();
        int attemptCount = 0;
        while (true)
        {
            if (!queue.TryEnqueue(flogEntry))
            {
                if (TryDrainAmount(DrainQueueResult.OneBatchReadSize(QueueReadBatchSize)))
                {
                    if (!queue.TryEnqueue(flogEntry))
                    {
                        SyncProtectedHandleQueueFull(flogEntry);
                    }
                    break;
                }
                if (HasTimedOutOrCompleted(flogEntry, attemptCount))
                {
                    break;
                }
                attemptCount++;
            }
            else
            {
                break;
            }
        }
    }

    private bool HasTimedOutOrCompleted(IFLogEntry flogEntry, int attemptCount)
    {
        if (InboundQueueFullHandling == FullQueueHandling.Block)
        {
            drainComplete.WaitOne();
            if (AppenderLogEntryQueue.TryEnqueue(flogEntry))
            {
                return true;
            }
        }
        var sleepInterval = (int)EnqueueAttemptInterval.Max(TimeSpan.Zero).TotalMilliseconds;
        var maxAttempts   = EnqueueAttemptTimeout.TotalMilliseconds / EnqueueAttemptInterval.Max(OneMillisecond).TotalMilliseconds;
        if (attemptCount > maxAttempts)
        {
            if (HasTimeoutHandling(flogEntry)) return true;
        }
        Thread.Sleep(sleepInterval);
        return false;
    }

    private bool HasTimeoutHandling(IFLogEntry flogEntry)
    {
        if (InboundQueueFullHandling == FullQueueHandling.TimeoutDropNewestForwardToFailedAppender)
        {
            TimeoutDropNewestForwardToFailHandling(flogEntry);
        }
        if (InboundQueueFullHandling == FullQueueHandling.TimeoutDropNewest)
        {
            TimeoutDropNewestHandling(flogEntry);
            return true;
        }
        return false;
    }

    protected bool WaitForDrainedSignal => Thread.VolatileRead(ref drainToken.Value) == 1;

    protected bool TryDrainAmount(DrainQueueResult drainAmount, int remainingAttempts = 1)
    {
        if (Interlocked.CompareExchange(ref drainToken.Value, 1, 0) == 0)
        {
            try
            {
                drainComplete.Reset();
                DrainToDownstreamAppenders(drainAmount.DrainAmount);
            }
            finally
            {
                drainComplete.Set();
                Thread.VolatileWrite(ref drainToken.Value, 0);
            }
            return true;
        }
        return false;
    }

    protected void DrainToDownstreamAppenders(int minDrainAmount)
    {
        LastDrainTime = TimeContext.UtcNow;
        var batchFlogEntries = AppenderLogEntryPool.SourceBatchLogEntryContainer(QueueReadBatchSize);
        while (minDrainAmount > 0)
        {
            var forwardToAppenders =
                AppenderLogEntryQueue.PollBatch(QueueReadBatchSize, batchFlogEntries);

            var entriesCount = forwardToAppenders.Count;
            minDrainAmount -= entriesCount;
            if (entriesCount > 0)
            {
                if (entriesCount == 1)
                {
                    AsyncOnForward(new LogEntryPublishEvent(forwardToAppenders[0]));
                }
                else
                {
                    AsyncOnForward(new LogEntryPublishEvent(forwardToAppenders));
                }
            }
        }
        batchFlogEntries.DecrementRefCount();
    }

    protected void AsyncOnForward(LogEntryPublishEvent logEntryEvent)
    {
        var forwardToList = ForwardToAppenders;
        for (var i = 0; i < forwardToList.Count; i++)
        {
            var appendersByQueueNum = forwardToList[i];
            ForwardingAsyncClient.ForwardLogEntryEventToAppenders(appendersByQueueNum.Key, logEntryEvent, appendersByQueueNum.Value);
        }
    }

    public int QueueReadBatchSize { get; set; }

    public int QueueSize { get; set; }

    private readonly StringBuilder sb = new();

    private int dropInterval;

    protected virtual void SyncProtectedHandleQueueFull(IFLogEntry toAppend)
    {
        lock (FullQueueSyncLock)
        {
            HandleQueueFull(toAppend);
        }
    }

    protected virtual void HandleQueueFull(IFLogEntry toAppend)
    {
        if (AppenderLogEntryQueue.TryEnqueue(toAppend)) return;
        switch (InboundQueueFullHandling)
        {
            case FullQueueHandling.DropAll:                            DropAllHandling(toAppend); break;
            case FullQueueHandling.DropAllAddDroppedLog:               DropAllAddDropLogHandling(toAppend); break;
            case FullQueueHandling.DropAllDebugLevelInQueue:           DropAllDebugLevelHandling(toAppend); break;
            case FullQueueHandling.DropAllAddDroppedLogToFailAppender: DropAllAddDropLogToFailAppenderHandling(toAppend); break;
            case FullQueueHandling.DropAllDebugInfoLevelInQueue:       DropAllDebugInfoLevelHandling(toAppend); break;
            case FullQueueHandling.DropNewest:                         DropNewestHandling(toAppend); break;
            case FullQueueHandling.DropNewestForwardToFailAppender:    DropNewestForwardToFailAppender(toAppend); break;
            case FullQueueHandling.DropOldest:                         DropOldestHandling(toAppend); break;
            case FullQueueHandling.DropOldestForwardToFailAppender:    DropOldestForwardToFailAppenderHandling(toAppend); break;
            case FullQueueHandling.DropOldestTwoAddDroppedLog:         DropOldestTwoHandling(toAppend); break;
            case FullQueueHandling.DropNewestTwoAddDroppedLog:         DropNewestTwoHandling(toAppend); break;
            case FullQueueHandling.DropEveryQueueInterval:             DropAllIntervalHandling(toAppend); break;
            case FullQueueHandling.DropDebugQueueInterval:             DropAllDebugOrLowerIntervalHandling(toAppend); break;
            case FullQueueHandling.DropDebugAndInfoQueueInterval:      DropAllInfoOrLowerIntervalHandling(toAppend); break;

            case FullQueueHandling.TimeoutDropNewestForwardToFailedAppender:
                AttemptEnqueueUntilTimeoutDropNewestForwardToFailHandling(toAppend);
                break;
            case FullQueueHandling.TimeoutDropNewest: AttemptEnqueueUntilTimeoutDropNewestHandling(toAppend); break;
            case FullQueueHandling.Default:
            case FullQueueHandling.Block:
                AppenderLogEntryQueue.Enqueue(toAppend);
                break;
        }
    }

    protected virtual int DropAllHandling(IFLogEntry flogEntry)
    {
        var amountDropped = AppenderLogEntryQueue.DropAll();
        AppenderLogEntryQueue.Enqueue(flogEntry);
        return amountDropped;
    }

    protected virtual int DropAllAddDropLogHandling(IFLogEntry flogEntry)
    {
        var queue         = AppenderLogEntryQueue;
        var amountDropped = AppenderLogEntryQueue.DropAll();
        var dropMessage   = AppenderLogEntryPool.SourceLogEntry();
        dropMessage.Initialize(new LoggerEntryContext(Logger, new NoOpFLogEntrySink(), FLogCallLocation.NoneAppenderAlertMessage, FLogLevel.Warn));
        sb.Clear().Append("The following entry caused the queue to drop '").Append(amountDropped).Append("' entries for '").Append(AppenderName)
          .Append("':");
        dropMessage.Message.Insert(0, sb);
        queue.Enqueue(dropMessage);
        queue.Enqueue(flogEntry);
        return amountDropped;
    }

    protected virtual int DropAllAddDropLogToFailAppenderHandling(IFLogEntry flogEntry)
    {
        var queue         = AppenderLogEntryQueue;
        var amountDropped = AppenderLogEntryQueue.DropAll();
        var dropMessage   = AppenderLogEntryPool.SourceLogEntry();
        dropMessage.Initialize(new LoggerEntryContext(Logger, new NoOpFLogEntrySink(), FLogCallLocation.NoneAppenderAlertMessage, FLogLevel.Warn));
        sb.Clear().Append("Queue full event caused queue to drop '").Append(amountDropped).Append("' entries for '").Append(AppenderName)
          .Append("':");
        dropMessage.Message.Insert(0, sb);
        AppenderRegistry.FailAppender.PublishLogEntryEvent(new LogEntryPublishEvent(dropMessage.Freeze), ReceiveEndpoint);
        queue.Enqueue(flogEntry);
        return amountDropped;
    }

    protected virtual int DropAllDebugLevelHandling(IFLogEntry toAppend)
    {
        return AppenderLogEntryQueue.RunRemoveFilter(toAppend, MatchDebugOrLower);
    }

    protected virtual int DropNewestHandling(IFLogEntry toAppend)
    {
        toAppend.DecrementRefCount();
        return 1;
    }

    protected virtual int DropAllDebugInfoLevelHandling(IFLogEntry toAppend)
    {
        return AppenderLogEntryQueue.RunRemoveFilter(toAppend, MatchInfoOrLower);
    }

    protected virtual int DropNewestForwardToFailAppender(IFLogEntry toAppend)
    {
        var dropNewestMessage = AppenderLogEntryPool.SourceLogEntry();
        dropNewestMessage.CopyFrom(toAppend);
        toAppend.DecrementRefCount();
        sb.Clear().Append("Dropped at '").Append(AppenderName).Append("':");
        dropNewestMessage.Message.Insert(0, sb);
        AppenderRegistry.FailAppender.PublishLogEntryEvent
            (new LogEntryPublishEvent(dropNewestMessage.Freeze), ReceiveEndpoint);
        return 1;
    }

    protected virtual int DropOldestHandling(IFLogEntry toAppend)
    {
        var dropped = AppenderLogEntryQueue.ForceEnqueue(toAppend);
        dropped?.DecrementRefCount();
        return dropped != null ? 1 : 0;
    }

    protected virtual int DropOldestForwardToFailAppenderHandling(IFLogEntry toAppend)
    {
        var droppedEntry = AppenderLogEntryQueue.ForceEnqueue(toAppend);
        if (droppedEntry != null)
        {
            var droppedMessage = AppenderLogEntryPool.SourceLogEntry();
            droppedMessage.CopyFrom(droppedEntry);
            droppedMessage.DecrementRefCount();
            sb.Clear().Append("Dropped at '").Append(AppenderName).Append("':");
            droppedMessage.Message.Insert(0, sb);
            AppenderRegistry.FailAppender.PublishLogEntryEvent
                (new LogEntryPublishEvent(droppedMessage.Freeze), ReceiveEndpoint);
        }
        return droppedEntry != null ? 1 : 0;
    }

    protected virtual int DropOldestTwoHandling(IFLogEntry toAppend)
    {
        var queue = AppenderLogEntryQueue;

        var countDropped  = 0;
        var droppedOldest = queue.ForceEnqueue(toAppend);
        if (droppedOldest != null)
        {
            countDropped++;
            var droppedMessage = AppenderLogEntryPool.SourceLogEntry();
            droppedMessage.CopyFrom(droppedOldest);
            droppedMessage.DecrementRefCount();
            sb.Clear().Append("Dropped at '").Append(AppenderName).Append("':");
            droppedMessage.Message.Insert(0, sb);
            var dropped = queue.ForceEnqueue(droppedMessage.Freeze);
            dropped?.DecrementRefCount();
            if (dropped != null) countDropped++;
        }
        return countDropped;
    }

    protected virtual int DropNewestTwoHandling(IFLogEntry toAppend)
    {
        var queue = AppenderLogEntryQueue;

        var countDropped = 0;
        var dropMessage  = AppenderLogEntryPool.SourceLogEntry();
        dropMessage.Initialize(new LoggerEntryContext(Logger, new NoOpFLogEntrySink(), FLogCallLocation.NoneAppenderAlertMessage, FLogLevel.Warn));
        sb.Clear().Append("The following entry caused two entries before it to be removed to include this message'").Append(AppenderName)
          .Append("':");
        dropMessage.Message.Insert(0, sb);
        var (newest, secondNewest) = queue.ReplaceNewestQueued(toAppend, dropMessage.Freeze);
        if (newest != null) countDropped++;
        newest?.DecrementRefCount();
        if (secondNewest != null) countDropped++;
        secondNewest?.DecrementRefCount();
        return countDropped;
    }

    protected virtual int DropAllIntervalHandling(IFLogEntry toAppend)
    {
        return AppenderLogEntryQueue.RunRemoveFilter(toAppend, intervalMatchAllEntries.MatchOn);
    }

    protected virtual int DropAllDebugOrLowerIntervalHandling(IFLogEntry toAppend)
    {
        return AppenderLogEntryQueue.RunRemoveFilter(toAppend, intervalMatchDebugOrLower.MatchOn);
    }

    protected virtual int DropAllInfoOrLowerIntervalHandling(IFLogEntry toAppend)
    {
        return AppenderLogEntryQueue.RunRemoveFilter(toAppend, intervalMatchInfoOrLower.MatchOn);
    }

    protected virtual int AttemptEnqueueUntilTimeoutDropNewestForwardToFailHandling(IFLogEntry flogEntry)
    {
        var queue        = AppenderLogEntryQueue;
        int attemptCount = 0;
        while (true)
        {
            if (!queue.TryEnqueue(flogEntry))
            {
                var sleepInterval = (int)EnqueueAttemptInterval.Max(TimeSpan.Zero).TotalMilliseconds;
                var maxAttempts   = EnqueueAttemptTimeout.TotalMilliseconds / EnqueueAttemptInterval.Max(OneMillisecond).TotalMilliseconds;
                if (attemptCount > maxAttempts)
                {
                    return TimeoutDropNewestForwardToFailHandling(flogEntry);
                }
                Thread.Sleep(sleepInterval);
                attemptCount++;
            }
            else
            {
                break;
            }
        }
        return 0;
    }

    protected virtual int TimeoutDropNewestForwardToFailHandling(IFLogEntry flogEntry)
    {
        var dropNewestMessage = AppenderLogEntryPool.SourceLogEntry();
        dropNewestMessage.CopyFrom(flogEntry);
        flogEntry.DecrementRefCount();
        sb.Clear().Append("Failed to handle queue full and dropped at '").Append(AppenderName).Append("':");
        dropNewestMessage.Message.Insert(0, sb);
        AppenderRegistry.FailAppender.PublishLogEntryEvent
            (new LogEntryPublishEvent(dropNewestMessage.Freeze), ReceiveEndpoint);
        return 1;
    }

    protected virtual int AttemptEnqueueUntilTimeoutDropNewestHandling(IFLogEntry flogEntry)
    {
        var queue        = AppenderLogEntryQueue;
        int attemptCount = 0;
        while (true)
        {
            if (!queue.TryEnqueue(flogEntry))
            {
                var sleepInterval = (int)EnqueueAttemptInterval.Max(TimeSpan.Zero).TotalMilliseconds;
                var maxAttempts   = EnqueueAttemptTimeout.TotalMilliseconds / EnqueueAttemptInterval.Max(OneMillisecond).TotalMilliseconds;
                if (attemptCount > maxAttempts)
                {
                    return TimeoutDropNewestHandling(flogEntry);
                }
                Thread.Sleep(sleepInterval);
                attemptCount++;
            }
            else
            {
                break;
            }
        }
        return 0;
    }

    protected virtual int TimeoutDropNewestHandling(IFLogEntry flogEntry)
    {
        flogEntry.DecrementRefCount();
        return 1;
    }

    private class NoOpFLogEntrySink() : FLogEntryPipelineEndpoint("NoOpFLogEntrySink", null!)
    {
        public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Sink;

        public override FLogEntryProcessChainState LogEntryProcessState
        {
            get => FLogEntryProcessChainState.Terminating;
            protected set => _ = value;
        }

        public override string Name { get; } = "NoOpFLogEntrySink";
    }

    protected static Predicate<IFLogEntry> MatchDebugOrLower = static logEntry => logEntry.LogLevel <= FLogLevel.Debug;
    protected static Predicate<IFLogEntry> MatchInfoOrLower  = static logEntry => logEntry.LogLevel <= FLogLevel.Info;


    protected abstract class MatchLogEntryIntervalPredicate
    {
        protected readonly int Interval;

        protected int Count;

        public readonly Predicate<IFLogEntry> MatchOn;

        protected MatchLogEntryIntervalPredicate(int interval)
        {
            Interval = interval;

            MatchOn = Match;
        }

        public abstract bool Match(IFLogEntry subject);
    }

    protected class IntervalMatchAll(int interval) : MatchLogEntryIntervalPredicate(interval)
    {
        public override bool Match(IFLogEntry subject)
        {
            return Count++ % Interval == 0;
        }
    }

    protected class IntervalMatchAllDebug(int interval) : MatchLogEntryIntervalPredicate(interval)
    {
        public override bool Match(IFLogEntry subject)
        {
            if (subject.LogLevel <= FLogLevel.Debug)
            {
                return Count++ % Interval == 0;
            }
            return false;
        }
    }

    protected class IntervalMatchAllInfo(int interval) : MatchLogEntryIntervalPredicate(interval)
    {
        public override bool Match(IFLogEntry subject)
        {
            if (subject.LogLevel <= FLogLevel.Info)
            {
                return Count++ % Interval == 0;
            }
            return false;
        }
    }
}
