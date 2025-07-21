// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Appending.Forwarding.Queues;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.Pooling;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding;

public interface IFLoggerQueuingAppender : IFLoggerForwardingAppender
{
    FullQueueHandling InboundQueueFullHandling   { get; }

    int QueueReadBatchSize { get; }

    int QueueSize                 { get; }

}

public interface IMutableFLoggerQueuingAppender : IFLoggerQueuingAppender
{
    new FullQueueHandling InboundQueueFullHandling   { get; set; }

    new int QueueReadBatchSize { get; set; }

    new int QueueSize                 { get; set; }

}

public abstract class FLoggerQueuingAppender : FLoggerForwardingAppender, IMutableFLoggerQueuingAppender
{
    protected ILogEntryQueue AppenderQueue = null!;

    protected FLogEntryPool AppenderLogEntryPool;

    protected Action<ILogEntryQueue> SwapFilteredBack;

    protected FLoggerQueuingAppender(IQueueingAppenderConfig queuingAppenderConfig, IFloggerAppenderRegistry floggerAppenderRegistry)
        : base(queuingAppenderConfig, floggerAppenderRegistry)
    {
        InboundQueueFullHandling = queuingAppenderConfig.InboundQueue.QueueFullHandling;
        QueueReadBatchSize       = queuingAppenderConfig.InboundQueue.QueueReadBatchSize;
        QueueSize                = queuingAppenderConfig.InboundQueue.QueueSize;

        SwapFilteredBack = filtered => AppenderQueue = filtered;

        // AppenderLogEntryPool = appenderRegistry.AppenderFLogEntryPoolRegistry.ResolveFLogEntryPool(queuingAppenderConfig.LogEntryPool);
    }

    protected abstract ILogEntryQueue CreateAppenderQueue(IMutableFLoggerQueuingAppender forAppender);

    public FullQueueHandling InboundQueueFullHandling   { get; set; }

    public int QueueReadBatchSize { get; set; }

    public int QueueSize                 { get; set; }

    protected virtual void HandleQueueFull(IFLogEntry toAppend)
    {
        switch (InboundQueueFullHandling)
        {
            case FullQueueHandling.DropAllAndBounceConsumer: AppenderQueue.DropAll(); break;
            case FullQueueHandling.DropAllDebugLevelInQueue:     
                var blockDrainDebugQueue = Recycler?.Borrow<BlockDrainSwapBackQueue>() ?? new BlockDrainSwapBackQueue();
                blockDrainDebugQueue.Initialize(AppenderQueue, MatchDebugOrLower, SwapFilteredBack);
                blockDrainDebugQueue.RunFilter(toAppend);
                break;
            case FullQueueHandling.DropAllDebugInfoLevelInQueue: 
                var blockDrainInfoQueue = Recycler?.Borrow<BlockDrainSwapBackQueue>() ?? new BlockDrainSwapBackQueue();
                blockDrainInfoQueue.Initialize(AppenderQueue, MatchInfoOrLower, SwapFilteredBack);
                blockDrainInfoQueue.RunFilter(toAppend); 
                break;
            case FullQueueHandling.DropNewest:                   break;
            case FullQueueHandling.Default:
            case FullQueueHandling.Block: AppenderQueue.Enqueue(toAppend); break;
            
        }
    }

    protected static Func<IFLogEntry, bool> MatchDebugOrLower = static logEntry => logEntry.LogLevel <= FLogLevel.Debug;
    protected static Func<IFLogEntry, bool> MatchInfoOrLower = static logEntry => logEntry.LogLevel <= FLogLevel.Info;
}
