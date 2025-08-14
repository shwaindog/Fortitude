using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding.Queues;

public class BlockDrainSwapBackQueue : ReusableObject<ILogEntryQueue>, ILogEntryQueue
{
    private readonly LogEntriesBatch drainedEntries  = new();
    private readonly LogEntriesBatch filteredEntries = new();

    private readonly ManualResetEvent blockInboundOutboundEvent = new(false);

    private int filteredToIndex;
    private int filteredItemsCount;

    private ILogEntryQueue         originalQueue    = null!;
    private Action<ILogEntryQueue> onCompleteAction = null!;

    public BlockDrainSwapBackQueue() { }

    public BlockDrainSwapBackQueue(BlockDrainSwapBackQueue toClone)
    {
        drainedEntries.AddRange(toClone.drainedEntries);
    }

    public BlockDrainSwapBackQueue Initialize (Action<ILogEntryQueue> swapBackAction)
    {
        onCompleteAction = swapBackAction;

        drainedEntries.Clear();

        filteredToIndex         = 0;

        filteredItemsCount = 0;

        blockInboundOutboundEvent.Reset();

        return this;
    }

    public int RunRemoveFilter(IFLogEntry blockedLogEntry, Predicate<IFLogEntry> removePredicate)
    {
        var fullQueueSize = originalQueue.Count;
        drainedEntries.Capacity = Math.Max(drainedEntries.Capacity, fullQueueSize);
        originalQueue!.PollBatch(fullQueueSize, drainedEntries);
        RunDrainToFiltered(removePredicate);
        Thread.Yield();
        RunDrainToFiltered(removePredicate);
        Thread.Yield();
        for (var i = 0; originalQueue.Count > 0 && i < 10; i++)
        {
            RunDrainToFiltered(removePredicate);
            Thread.Sleep(i * 10);
        }
        // consider drained;
        for (int i = 0; i < filteredEntries.Count; i++)
        {
            if (!originalQueue.TryEnqueue(filteredEntries[i]))
            {
                filteredItemsCount++;  // dropping an unexpected
            }
        }
        if (!originalQueue.TryEnqueue(blockedLogEntry))
        {
            filteredItemsCount++;  // dropping blocking item
        }
        onCompleteAction(originalQueue);
        blockInboundOutboundEvent.Set();
        DecrementRefCount();
        return filteredItemsCount;
    }
    //
    // private FLogEntry BuildFilteredWarningLogEntry()
    // {
    //     var logEntry = Recycler?.Borrow<FLogEntry>() ?? new FLogEntry();
    //     logEntry.Initialize(new LoggerEntryContext())
    // }

    private void RunDrainToFiltered(Predicate<IFLogEntry> removePredicate)
    {
        var drainedCount = drainedEntries.Count;
        for (var index = filteredToIndex; index < drainedCount; index++)
        {
            var drainedEntry = drainedEntries[index];
            if (removePredicate(drainedEntry))
            {
                drainedEntry.DecrementRefCount();
                filteredItemsCount++;
            }
            else
            {
                filteredEntries.Add(drainedEntry);
            }
        }
        filteredToIndex = drainedCount;
    }

    public IFLogEntry? ReplaceNewestQueued(IFLogEntry flogEntry)
    {
        return originalQueue.ReplaceNewestQueued(flogEntry);
    }

    public (IFLogEntry?, IFLogEntry?) ReplaceNewestQueued(IFLogEntry lastFlogEntry, IFLogEntry secondLastFlogEntry)
    {
        return originalQueue.ReplaceNewestQueued(lastFlogEntry, secondLastFlogEntry);
    }

    public int Capacity
    {
        get
        {
            blockInboundOutboundEvent.WaitOne();
            return originalQueue.Capacity;
        }
    }
    public int Count
    {
        get
        {
            blockInboundOutboundEvent.WaitOne();
            return originalQueue.Count;
        }
    }

    public int DropAll()
    {
        blockInboundOutboundEvent.WaitOne();
        return originalQueue.DropAll();
    }

    public int DropItemsMatching(Func<IFLogEntry, bool> predicate)
    {
        blockInboundOutboundEvent.WaitOne();
        return filteredItemsCount;
    }

    public int Enqueue(IFLogEntry logEntry)
    {
        blockInboundOutboundEvent.WaitOne();
        return originalQueue.Enqueue(logEntry);
    }

    public IFLogEntry? ForceEnqueue(IFLogEntry logEntry)
    {
        return originalQueue.ForceEnqueue(logEntry);
    }

    public FullQueueHandling InboundQueueFullHandling
    {
        get
        {
            blockInboundOutboundEvent.WaitOne();
            return FullQueueHandling.TryAgain;
        }
    }

    public IFLogEntry Poll()
    {
        blockInboundOutboundEvent.WaitOne();
        return originalQueue.Poll();
    }

    public ILogEntriesBatch PollBatch(int maxBatchSize, ILogEntriesBatch toPopulate)
    {
        blockInboundOutboundEvent.WaitOne();
        return originalQueue.PollBatch(maxBatchSize, toPopulate);
    }

    public bool QueuedItemsAny(Predicate<IFLogEntry> predicate)
    {
        blockInboundOutboundEvent.WaitOne();
        return originalQueue.QueuedItemsAny(predicate);
    }

    public bool TryEnqueue(IFLogEntry logEntry)
    {
        blockInboundOutboundEvent.WaitOne();
        return originalQueue.TryEnqueue(logEntry);
    }

    public IFLogEntry? TryPoll()
    {
        blockInboundOutboundEvent.WaitOne();
        return originalQueue.TryPoll();
    }

    public override ILogEntryQueue Clone() =>
        Recycler?.Borrow<BlockDrainSwapBackQueue>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new BlockDrainSwapBackQueue(this);

    public override ILogEntryQueue CopyFrom(ILogEntryQueue source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is BlockDrainSwapBackQueue blockDrainReplace)
        {
            drainedEntries.AddRange(blockDrainReplace.drainedEntries);
        }

        return this;
    }
}
