using FortitudeCommon.EventProcessing.Disruption.Consuming;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Immediate;

internal class RingScheduler<T>(PushingRing<T> ringBuffer, IWaitStrategyInt waitStrategy)
    where T : class
{
    private readonly Dictionary<IRingConsumer<T>, RingPusher<T>> processorsByConsumers = new ();

    public IEnumerable<RingPusher<T>> Processors
    {
        get
        {
            var consumerRingPushers = new List<RingPusher<T>>(processorsByConsumers.Count);
            foreach (var proc in processorsByConsumers.Values)
            {
                if (!consumerRingPushers.Contains(proc))
                {
                    consumerRingPushers.Add(proc);
                }
            }
            return consumerRingPushers.ToArray();
        }
    }

    public Sequence[] EndCursors
    {
        get
        {
            var cursors = new List<Sequence>(processorsByConsumers.Count);
            foreach (var proc in processorsByConsumers.Values)
            {
                if (!cursors.Contains(proc.Sequence))
                {
                    cursors.Add(proc.Sequence);
                }
            }
            return cursors.ToArray();
        }
    }

    public void Register(params IRingConsumer<T>[] consumers)
    {
        foreach (var consumer in consumers)
        {
            if (processorsByConsumers.ContainsKey(consumer))
            {
                throw new InvalidOperationException("A RingConsumer can only be registered once");
            }
        }
        var processor = new RingPusher<T>(ringBuffer,
                                          new IndependentRingBarrier(waitStrategy, ringBuffer.Cursor),
                                          consumers);
        foreach (var consumer in consumers)
        {
            processorsByConsumers[consumer] = processor;
        }
    }
}

internal class RingSchedulerLong<T>(PushingRingLong<T> ringBuffer, IWaitStrategyLong waitStrategy)
    where T : class
{
    private readonly Dictionary<IRingConsumer<T>, RingPusherLong<T>> processorsByConsumers = new ();

    public IEnumerable<RingPusherLong<T>> Processors
    {
        get
        {
            var consumerRingPushers = new List<RingPusherLong<T>>(processorsByConsumers.Count);
            foreach (var proc in processorsByConsumers.Values)
            {
                if (!consumerRingPushers.Contains(proc))
                {
                    consumerRingPushers.Add(proc);
                }
            }
            return consumerRingPushers.ToArray();
        }
    }

    public SequenceLong[] EndCursors
    {
        get
        {
            var cursors = new List<SequenceLong>(processorsByConsumers.Count);
            foreach (var proc in processorsByConsumers.Values)
            {
                if (!cursors.Contains(proc.Sequence))
                {
                    cursors.Add(proc.Sequence);
                }
            }
            return cursors.ToArray();
        }
    }

    public void Register(params IRingConsumer<T>[] consumers)
    {
        foreach (var consumer in consumers)
        {
            if (processorsByConsumers.ContainsKey(consumer))
            {
                throw new InvalidOperationException("A RingConsumer can only be registered once");
            }
        }
        var processor = new RingPusherLong<T>(ringBuffer,
                                          new IndependentRingBarrierLong(waitStrategy, ringBuffer.Cursor),
                                          consumers);
        foreach (var consumer in consumers)
        {
            processorsByConsumers[consumer] = processor;
        }
    }
}