using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Logging.Config.AsyncBuffering;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding.Queues;

public class SynchroniseQueue(string appenderName, int capacity, FullQueueHandling queueFullHandling) : ILogEntryQueue
{
    private readonly BlockingStaticRing<FLogEntry> queue = new(appenderName, capacity, ClaimStrategyType.MultiProducers);

    private object dropSyncLock = new ();

    public int Capacity => queue.RingSize;

    public int Count    => (int)queue.Count;

    public bool TryEnqueue(IFLogEntry logEntry)
    {
        return queue.TryAdd((FLogEntry)logEntry);
    }

    public int Enqueue(IFLogEntry logEntry)
    {
        queue.Add((FLogEntry)logEntry);
        return Count;
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

    public bool QueuedItemsAny(Func<IFLogEntry, bool> predicate)
    {
        var countTo   = Count;
        if (countTo <= 0) return false;
        var peekIndex = queue.NextPeekIndex();
        var end = peekIndex + countTo;

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
