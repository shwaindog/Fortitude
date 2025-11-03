#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.EventProcessing.Disruption.Consuming;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Immediate;

/// <summary>
///     (http://code.google.com/p/disruptor/)
/// </summary>
public class PushingRing<T> where T : class
{
    private readonly List<Thread> threads = new();

    public PushingRing(string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType,
        WaitStrategyType waitStrategyType)
    {
        Name = name;
        var ringSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        ringMask = ringSize - 1;
        cells = new T[ringSize];

        Cursor = new(Sequence.InitialValue, ringMask);

        claimStrategy = claimStrategyType.GetInstance(name, ringSize);
        waitStrategy = waitStrategyType.GetInstance(ringMask);

        scheduler = new RingScheduler<T>(this, waitStrategy);

        for (var i = 0; i < cells.Length; i++) cells[i] = dataFactory();
    }

    public T this[int sequence] => cells[sequence & ringMask];

    public int Length => cells.Length;

    public int Claim()
    {
        var sequence = claimStrategy.Claim();
        claimStrategy.WaitFor(sequence, endCursors);
        return sequence;
    }

    public void Publish(int sequence)
    {
        claimStrategy.Serialize(Cursor, sequence);
        Cursor.Value = sequence;
        waitStrategy.NotifyAll();
    }

    #region Consumers registration

    public void Register(params IRingConsumer<T>[] consumers)
    {
        lock (scheduler)
        {
            if (running) throw new InvalidOperationException("New consumers cannot be registered while processors are active");
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
                        IsBackground = true, Priority = ThreadPriority.AboveNormal, Name = Name + "Walker#" + count++
                    };
                    thread.Start();
                    threads.Add(thread);
                }

                foreach (RingPusher<T> processor in scheduler.Processors)
                    while (!processor.Running)
                        Thread.Sleep(1);
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
                foreach (var processor in scheduler.Processors) processor.Stop();
                waitStrategy.InterruptAll();
                foreach (var thread in threads) thread.Join();
                threads.Clear();
                running = false;
            }
        }
    }

    #region Fields

    public Sequence Cursor { get; }

    public readonly string Name;

    private readonly int ringMask;
    private readonly T[] cells;

    private readonly IClaimStrategy claimStrategy;
    private readonly IWaitStrategyInt waitStrategy;

    private readonly RingScheduler<T> scheduler;

    private Sequence[] endCursors = [];
    private bool running;

    #endregion
}

public class PushingRingLong<T> where T : class
{
    private readonly List<Thread> threads = new();

    public PushingRingLong(string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType,
        WaitStrategyType waitStrategyType)
    {
        Name = name;
        var ringSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        ringMask = ringSize - 1;
        cells = new T[ringSize];

        claimStrategy = claimStrategyType.GetInstanceLong(name, ringSize);
        waitStrategy = waitStrategyType.GetInstanceLong();

        scheduler = new RingSchedulerLong<T>(this, waitStrategy);

        for (var i = 0; i < cells.Length; i++) cells[i] = dataFactory();
    }

    public T this[long sequence] => cells[(int)sequence & ringMask];

    public int Length => cells.Length;

    public long Claim()
    {
        var sequence = claimStrategy.Claim();
        claimStrategy.WaitFor(sequence, endCursors);
        return sequence;
    }

    public void Publish(long sequence)
    {
        claimStrategy.Serialize(Cursor, sequence);
        Cursor.Value = sequence;
        waitStrategy.NotifyAll();
    }

    #region Consumers registration

    public void Register(params IRingConsumer<T>[] consumers)
    {
        lock (scheduler)
        {
            if (running) throw new InvalidOperationException("New consumers cannot be registered while processors are active");
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
                foreach (RingPusherLong<T> processor in scheduler.Processors)
                {
                    var thread = new Thread(processor.Start)
                    {
                        IsBackground = true, Priority = ThreadPriority.AboveNormal, Name = Name + "Walker#" + count++
                    };
                    thread.Start();
                    threads.Add(thread);
                }

                foreach (RingPusherLong<T> processor in scheduler.Processors)
                    while (!processor.Running)
                        Thread.Sleep(1);
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
                foreach (var processor in scheduler.Processors) processor.Stop();
                waitStrategy.InterruptAll();
                foreach (var thread in threads) thread.Join();
                threads.Clear();
                running = false;
            }
        }
    }

    #region Fields

    public SequenceLong Cursor { get; } = new(SequenceLong.InitialValue);

    public readonly string Name;

    private readonly int ringMask;
    private readonly T[] cells;

    private readonly IClaimStrategyLong claimStrategy;
    private readonly IWaitStrategyLong waitStrategy;

    private readonly RingSchedulerLong<T> scheduler;

    private SequenceLong[] endCursors = [];
    private bool running;

    #endregion
}
