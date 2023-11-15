#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Batching;

public interface IPollingRing<T> : IEnumerable<T> where T : class
{
    string Name { get; }
    T this[long sequence] { get; }
    long CurrentSequence { get; }
    long CurrentBatchSize { get; }
    bool StartOfBatch { get; }
    bool EndOfBatch { get; }
    long Claim();
    void Publish(long sequence);
}

public class PollingRing<T> : IPollingRing<T> where T : class
{
    public PollingRing(string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType,
        bool logErrors = true)
    {
        Name = name;
        var ringSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        ringMask = ringSize - 1;
        cells = new T[ringSize];
        conCursors = new[] { conCursor };

        claimStrategy = claimStrategyType.GetInstance(name, ringSize, logErrors);

        for (var i = 0; i < cells.Length; i++) cells[i] = dataFactory();
    }

    public string Name { get; }

    public T this[long sequence] => cells[(int)sequence & ringMask];

    public long CurrentSequence { get; private set; }
    public long CurrentBatchSize { get; private set; }
    public bool StartOfBatch { get; private set; }
    public bool EndOfBatch { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        var maxPublishedSequence = pubCursor.Value;
        CurrentBatchSize = maxPublishedSequence - conCursor.Value;
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

    private readonly Sequence pubCursor = new(Sequence.InitialValue);
    private readonly Sequence conCursor = new(Sequence.InitialValue);
    private readonly Sequence[] conCursors;
    private readonly int ringMask;
    private readonly T[] cells;

    private readonly IClaimStrategy claimStrategy;

    #endregion
}
