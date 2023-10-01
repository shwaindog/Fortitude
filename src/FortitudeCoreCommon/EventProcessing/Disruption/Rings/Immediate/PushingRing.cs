using System;
using System.Collections.Generic;
using System.Threading;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Consuming;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Immediate
{
    /// <summary>
    ///     (http://code.google.com/p/disruptor/)
    /// </summary>
    public class PushingRing<T> where T : class
    {
        private readonly List<Thread> threads = new List<Thread>();

        public PushingRing(string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType,
            WaitStrategyType waitStrategyType)
        {
            Name = name;
            ringSize = MemoryUtils.CeilingNextPowerOfTwo(size);
            ringMask = ringSize - 1;
            cells = new T[ringSize];

            claimStrategy = claimStrategyType.GetInstance(name, ringSize);
            waitStrategy = waitStrategyType.GetInstance();

            scheduler = new RingScheduler<T>(this, waitStrategy);

            for (var i = 0; i < cells.Length; i++)
            {
                cells[i] = dataFactory();
            }
        }

        public T this[long sequence] => cells[(int) sequence & ringMask];

        public long Claim()
        {
            var sequence = claimStrategy.Claim();
            claimStrategy.WaitFor(sequence, endCursors);
            return sequence;
        }

        public void Publish(long sequence)
        {
            claimStrategy.Serialize(cursor, sequence);
            cursor.Value = sequence;
            waitStrategy.NotifyAll();
        }

        #region Consumers registration

        public void Register(params IRingConsumer<T>[] consumers)
        {
            lock (scheduler)
            {
                if (running)
                {
                    throw new InvalidOperationException("New consumers cannot be registered while processors are active");
                }
                scheduler.Register(consumers);
            }
        }

        #endregion

        public void Start()
        {
            lock (scheduler)
            {
                if (!running)
                {
                    waitStrategy.ClearInterrupt();
                    endCursors = scheduler.EndCursors;
                    var count = 1;
                    foreach (RingPusher<T> processor in scheduler.Processors)
                    {
                        var thread = new Thread(processor.Start)
                        {
                            IsBackground = true,
                            Priority = ThreadPriority.AboveNormal,
                            Name = Name + "Walker#" + count++
                        };
                        thread.Start();
                        threads.Add(thread);
                    }
                    foreach (RingPusher<T> processor in scheduler.Processors)
                    {
                        while (!processor.Running)
                        {
                            Thread.Sleep(1);
                        }
                    }
                    running = true;
                }
            }
        }

        public void Stop()
        {
            lock (scheduler)
            {
                if (running)
                {
                    foreach (var processor in scheduler.Processors)
                    {
                        processor.Stop();
                    }
                    waitStrategy.InterruptAll();
                    foreach (var thread in threads)
                    {
                        thread.Join();
                    }
                    threads.Clear();
                    running = false;
                }
            }
        }

        #region Fields

        private readonly Sequence cursor = new Sequence(Sequence.InitialValue);

        public Sequence Cursor => cursor;

        public readonly string Name;

        private readonly int ringSize;
        private readonly int ringMask;
        private readonly T[] cells;

        private readonly IClaimStrategy claimStrategy;
        private readonly IWaitStrategy waitStrategy;

        private readonly RingScheduler<T> scheduler;

        private Sequence[] endCursors = new Sequence[0];
        private bool running;

        #endregion
    }
}