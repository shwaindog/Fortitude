#region

using System.Collections;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IEnumerableBatchPollingRing : IPollingRing
{
    int CurrentBatchSize { get; }
    bool StartOfBatch     { get; }
    bool EndOfBatch       { get; }
}

public interface IEnumerableBatchPollingRing<out T> : IEnumerableBatchPollingRing, IPollingRing<T>, IEnumerable<T> where T : class
{
    int CurrentSequence { get; }
}

public interface IEnumerableBatchPollingRingLong<out T> : IEnumerableBatchPollingRing, IPollingRingLong<T>, IEnumerable<T> where T : class
{
    long CurrentSequence { get; }
}

public class EnumerableBatchPollingRing<T> : IEnumerableBatchPollingRing<T> where T : class
{
    public EnumerableBatchPollingRing(string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType,
        bool logErrors = true)
    {
        Name = name;
        var ringSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        ringMask   = ringSize - 1;
        cells      = new T[ringSize];

        conCursor = new(Sequence.InitialValue, ringMask);
        pubCursor = new(Sequence.InitialValue, ringMask);

        conCursors = [conCursor];

        claimStrategy = claimStrategyType.GetInstance(name, ringSize, logErrors);

        for (var i = 0; i < cells.Length; i++) cells[i] = dataFactory();
    }

    public EnumerableBatchPollingRing(string name, T[] existing, ClaimStrategyType claimStrategyType,
        bool logErrors = true)
    {
        Name = name;
        bool sizeIsPowerOfTwo = MemoryUtils.IsPowerOfTwo(existing.Length);
        if (sizeIsPowerOfTwo)
        {
            throw new ArgumentException("Error array must be a power of Two in size");
        }
        var ringSize = existing.Length;

        ringMask   = ringSize - 1;

        conCursor  = new(Sequence.InitialValue, ringMask);
        pubCursor  = new(Sequence.InitialValue, ringMask);

        cells      = new T[ringSize];
        conCursors = [conCursor];

        claimStrategy = claimStrategyType.GetInstance(name, ringSize, logErrors);
    }

    public string Name { get; }

    public T this[int sequence] => cells[sequence & ringMask];

    public int CurrentSequence
    {
        get => currentSequence;
        private set => currentSequence = value & ringMask;
    }

    public int Queued
    {
        get
        {
            var snapConCursor = conCursor.Value;
            var snapPubCursor = pubCursor.Value;
            return snapPubCursor > snapConCursor ? (snapPubCursor & ringMask) - (snapConCursor & ringMask) : (snapPubCursor & ringMask) + (snapConCursor % cells.Length);
        }
    }

    public int CurrentBatchSize { get; private set; }
    public bool StartOfBatch { get; private set; }
    public bool EndOfBatch { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        var maxPublishedSequence = pubCursor.Value;
        CurrentBatchSize = (maxPublishedSequence - conCursor.Value) & ringMask;
        if (CurrentBatchSize != 0)
        {
            for (CurrentSequence = (conCursor.Value + 1) & ringMask; CurrentSequence != ((maxPublishedSequence + 1) & ringMask); CurrentSequence++)
            {
                StartOfBatch = CurrentSequence == conCursor.Value + 1;
                EndOfBatch   = CurrentSequence == maxPublishedSequence;
                yield return cells[CurrentSequence & ringMask];
            }

            conCursor.Value = maxPublishedSequence;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Claim()
    {
        var sequence = claimStrategy.Claim();
        claimStrategy.WaitFor(sequence, conCursors);
        return sequence;
    }

    public void Publish(int sequence)
    {
        claimStrategy.Serialize(pubCursor, sequence);
        pubCursor.Value = sequence;
    }

    #region Fields

    private readonly Sequence pubCursor;
    private readonly Sequence conCursor;
    private readonly Sequence[] conCursors;
    private readonly int ringMask;
    private readonly T[] cells;

    private readonly IClaimStrategy claimStrategy;

    private          int            currentSequence;

    #endregion
}


public class EnumerableBatchPollingRingLong<T> : IEnumerableBatchPollingRingLong<T> where T : class
{
    public EnumerableBatchPollingRingLong(string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType,
        bool logErrors = true)
    {
        Name = name;
        var ringSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        ringMask   = ringSize - 1;
        cells      = new T[ringSize];
        conCursors = [conCursor];

        claimStrategy = claimStrategyType.GetInstanceLong(name, ringSize, logErrors);

        for (var i = 0; i < cells.Length; i++) cells[i] = dataFactory();
    }

    public EnumerableBatchPollingRingLong(string name, T[] existing, ClaimStrategyType claimStrategyType,
        bool logErrors = true)
    {
        Name = name;
        bool sizeIsPowerOfTwo = MemoryUtils.IsPowerOfTwo(existing.Length);
        if (sizeIsPowerOfTwo)
        {
            throw new ArgumentException("Error array must be a power of Two in size");
        }
        var ringSize = existing.Length;
        ringMask   = ringSize - 1;
        cells      = new T[ringSize];
        conCursors = [conCursor];

        claimStrategy = claimStrategyType.GetInstanceLong(name, ringSize, logErrors);
    }

    public string Name { get; }

    public T this[long sequence] => cells[(int)sequence & ringMask];

    public long Queued
    {
        get
        {
            var snapConCursor = conCursor.Value;
            var snapPubCursor = pubCursor.Value;
            return snapPubCursor > snapConCursor 
                ? (snapPubCursor & ringMask) - (snapConCursor & ringMask) 
                : (snapPubCursor & ringMask) + (snapConCursor % cells.Length);
        }
    }

    public long CurrentSequence { get; private set; }
    public int CurrentBatchSize { get; private set; }
    public bool StartOfBatch { get; private set; }
    public bool EndOfBatch { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        var maxPublishedSequence = pubCursor.Value;
        CurrentBatchSize = (int)(maxPublishedSequence - conCursor.Value);
        for (CurrentSequence = conCursor.Value + 1; CurrentSequence <= maxPublishedSequence; CurrentSequence++)
        {
            StartOfBatch = CurrentSequence == conCursor.Value + 1;
            EndOfBatch = CurrentSequence == maxPublishedSequence;
            yield return cells[(int)CurrentSequence & ringMask];
        }

        conCursor.Value = maxPublishedSequence;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public long Claim()
    {
        var sequence = claimStrategy.Claim();
        claimStrategy.WaitFor(sequence, conCursors);
        return sequence;
    }

    public void Publish(long sequence)
    {
        claimStrategy.Serialize(pubCursor, sequence);
        pubCursor.Value = sequence;
    }

    #region Fields

    private readonly SequenceLong pubCursor = new(SequenceLong.InitialValue);
    private readonly SequenceLong conCursor = new(SequenceLong.InitialValue);
    private readonly SequenceLong[] conCursors;
    private readonly int ringMask;
    private readonly T[] cells;

    private readonly IClaimStrategyLong claimStrategy;

    #endregion
}
