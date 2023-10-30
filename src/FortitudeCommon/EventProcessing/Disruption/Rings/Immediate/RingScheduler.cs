using System;
using System.Collections.Generic;
using FortitudeCommon.EventProcessing.Disruption.Consuming;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Immediate
{
    internal class RingScheduler<T> where T : class
    {
        private readonly Dictionary<IRingConsumer<T>, RingPusher<T>> processorsByConsumers =
            new Dictionary<IRingConsumer<T>, RingPusher<T>>();

        private readonly PushingRing<T> ringBuffer;
        private readonly IWaitStrategy waitStrategy;

        public RingScheduler(PushingRing<T> ringBuffer, IWaitStrategy waitStrategy)
        {
            this.ringBuffer = ringBuffer;
            this.waitStrategy = waitStrategy;
        }

        public IEnumerable<RingPusher<T>> Processors
        {
            get
            {
                var procs = new List<RingPusher<T>>(processorsByConsumers.Count);
                foreach (var proc in processorsByConsumers.Values)
                {
                    if (!procs.Contains(proc))
                    {
                        procs.Add(proc);
                    }
                }
                return procs.ToArray();
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
}