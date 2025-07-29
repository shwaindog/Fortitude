using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding.Queues;

public class FlogEntryBufferQueue(string appenderName, int capacity, FullQueueHandling queueFullHandling) : ILogEntryQueue
{
    private readonly ReusableList<IFLogEntry> drainedEntries  = new();
    private readonly ReusableList<IFLogEntry> filteredEntries = new();

    private int filteredToIndex;
    private int filteredItemsCount;

    private readonly BlockingStaticRing<FLogEntry> queue = new(appenderName, capacity, ClaimStrategyType.MultiProducers);

    public int Capacity => queue.RingSize;

    public int Count => queue.Count;

    public bool TryEnqueue(IFLogEntry logEntry)
    {
        return queue.TryAdd((FLogEntry)logEntry);
    }

    public int Enqueue(IFLogEntry logEntry)
    {
        queue.Add((FLogEntry)logEntry);
        return Count;
    }

    public IFLogEntry? ForceEnqueue(IFLogEntry logEntry)
    {
        return queue.UnsafePopPush((FLogEntry)logEntry);
    }

    public (IFLogEntry?, IFLogEntry?) ForceEnqueue(IFLogEntry lastLogEntry, IFLogEntry secondLastLogEntry)
    {
        var firstEntry =  queue.UnsafePopPush((FLogEntry)secondLastLogEntry);
        var secondEntry =  queue.UnsafePopPush((FLogEntry)lastLogEntry);
        return (firstEntry, secondEntry);
    }

    public FLogEntry? OldestQueued
    {
        get
        {
            if (Count <= 0) return null;
            var peekIndex   = queue.NextPeekIndex();
            var itemAtIndex = queue.PeekAt(peekIndex);
            return itemAtIndex;
        }
    }

    public int RunRemoveFilter(IFLogEntry blockedLogEntry, Predicate<IFLogEntry> removePredicate)
    {
        var fullQueueSize = Count;
        drainedEntries.Capacity = Math.Max(Capacity, fullQueueSize);
        PollBatch(fullQueueSize, drainedEntries);
        RunDrainToFiltered(removePredicate);
        Thread.Yield();
        RunDrainToFiltered(removePredicate);
        Thread.Yield();
        for (var i = 0; Count > 0 && i < 10; i++)
        {
            RunDrainToFiltered(removePredicate);
            Thread.Sleep(i * 10);
        }
        // consider drained;
        for (int i = 0; i < filteredEntries.Count; i++)
        {
            if (!TryEnqueue(filteredEntries[i]))
            {
                filteredItemsCount++; // dropping an unexpected
            }
        }
        if (!TryEnqueue(blockedLogEntry))
        {
            filteredItemsCount++; // dropping blocking item
        }
        return filteredItemsCount;
    }

    public IFLogEntry? ReplaceNewestQueued(IFLogEntry flogEntry)
    {
        return queue.ReplaceLastAdded((FLogEntry)flogEntry);
    }

    public (IFLogEntry?, IFLogEntry?) ReplaceNewestQueued(IFLogEntry lastFlogEntry, IFLogEntry secondLastFlogEntry)
    {
         var secondLast = queue.ReplaceLastAdded((FLogEntry)secondLastFlogEntry, 1);
         var last = queue.ReplaceLastAdded((FLogEntry)secondLastFlogEntry);
         return (last, secondLast);
    }

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

    public bool QueuedItemsAny(Predicate<IFLogEntry> predicate)
    {
        var countTo = Count;
        if (countTo <= 0) return false;
        var peekIndex = queue.NextPeekIndex();
        var end       = peekIndex + countTo;

        var foundMatch = false;
        for (var i = peekIndex; i < end && !foundMatch; i++)
        {
            var itemAtIndex = queue.PeekAt(peekIndex);
            foundMatch = itemAtIndex != null && predicate(itemAtIndex);
        }
        return foundMatch;
    }

    public int DropAll() => queue.RemovalAll();

    public FullQueueHandling InboundQueueFullHandling => queueFullHandling;

    public IFLogEntry Poll()
    {
        return queue.Take();
    }

    public IReusableList<IFLogEntry> PollBatch(int maxBatchSize, IReusableList<IFLogEntry> toPopulate)
    {
        for (int i = 0; i < maxBatchSize; i++)
        {
            var maybeEntry = TryPoll();
            if (maybeEntry != null)
            {
                toPopulate.Add(maybeEntry);
            }
            else
            {
                break;
            }
        }
        return toPopulate;
    }

    public IFLogEntry? TryPoll()
    {
        return queue.TryTake(out var maybeItem) ? maybeItem : null;
    }
}
