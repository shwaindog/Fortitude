using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.AsyncBuffering;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding.Queues;

public interface ILogEntryQueue
{
    int Capacity { get; }

    int Count { get; }

    FullQueueHandling InboundQueueFullHandling { get; }

    bool TryEnqueue(IFLogEntry logEntry);

    int Enqueue(IFLogEntry logEntry);

    bool QueuedItemsAny(Func<IFLogEntry, bool> predicate);

    int DropAll();

    IFLogEntry Poll();

    IFLogEntry? TryPoll();

    IReusableList<IFLogEntry> PollBatch(int maxBatchSize, IReusableList<IFLogEntry> toPopulate);
}

public class BlockDrainSwapBackQueue : ReusableObject<ILogEntryQueue>, ILogEntryQueue
{
    private readonly ReusableList<IFLogEntry> drainedEntries  = new();
    private readonly ReusableList<IFLogEntry> filteredEntries = new();

    private readonly ManualResetEvent blockInboundOutboundEvent = new(false);

    private int filteredToIndex;
    private int filteredItemsCount;

    private ILogEntryQueue         originalQueue    = null!;
    private Func<IFLogEntry, bool> removeMatching   = null!;
    private Action<ILogEntryQueue> onCompleteAction = null!;

    public BlockDrainSwapBackQueue() { }

    public BlockDrainSwapBackQueue(BlockDrainSwapBackQueue toClone)
    {
        drainedEntries.AddRange(toClone.drainedEntries);
    }

    public BlockDrainSwapBackQueue Initialize
        (ILogEntryQueue toFilter, Func<IFLogEntry, bool> removePredicate, Action<ILogEntryQueue> swapBackAction)
    {
        originalQueue    = toFilter;
        removeMatching   = removePredicate;
        onCompleteAction = swapBackAction;

        drainedEntries.Clear();

        filteredToIndex         = 0;

        filteredItemsCount = 0;

        blockInboundOutboundEvent.Reset();

        return this;
    }

    public void RunFilter(IFLogEntry blockedLogEntry)
    {
        var fullQueueSize = originalQueue.Count;
        drainedEntries.Capacity = Math.Max(drainedEntries.Capacity, fullQueueSize);
        originalQueue!.PollBatch(fullQueueSize, drainedEntries);
        RunDrainToFiltered();
        Thread.Yield();
        RunDrainToFiltered();
        Thread.Yield();
        for (var i = 0; originalQueue.Count > 0 && i < 10; i++)
        {
            RunDrainToFiltered();
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
    }
    //
    // private FLogEntry BuildFilteredWarningLogEntry()
    // {
    //     var logEntry = Recycler?.Borrow<FLogEntry>() ?? new FLogEntry();
    //     logEntry.Initialize(new LoggerEntryContext())
    // }

    private void RunDrainToFiltered()
    {
        var drainedCount = drainedEntries.Count;
        for (var index = filteredToIndex; index < drainedCount; index++)
        {
            var drainedEntry = drainedEntries[index];
            if (removeMatching(drainedEntry))
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

    public IReusableList<IFLogEntry> PollBatch(int maxBatchSize, IReusableList<IFLogEntry> toPopulate)
    {
        blockInboundOutboundEvent.WaitOne();
        return originalQueue.PollBatch(maxBatchSize, toPopulate);
    }

    public bool QueuedItemsAny(Func<IFLogEntry, bool> predicate)
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
