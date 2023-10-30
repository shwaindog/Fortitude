using System;
using System.Collections;
using System.Collections.Generic;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Batching
{
    public class PollingRing<T> : IEnumerable<T> where T : class
    {
        public PollingRing(string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType,
            bool logErrors = true)
        {
            Name = name;
            var ringSize = MemoryUtils.CeilingNextPowerOfTwo(size);
            ringMask = ringSize - 1;
            cells = new T[ringSize];
            conCursors = new[] {conCursor};

            claimStrategy = claimStrategyType.GetInstance(name, ringSize, logErrors);

            for (var i = 0; i < cells.Length; i++)
            {
                cells[i] = dataFactory();
            }
        }

        public T this[long sequence] => cells[(int) sequence & ringMask];

        internal long CurrentSequence { get; private set; }
        internal long CurrentBatchSize { get; private set; }
        internal bool StartOfBatch { get; private set; }
        internal bool EndOfBatch { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            var maxPublishedSequence = pubCursor.Value;
            CurrentBatchSize = maxPublishedSequence - conCursor.Value;
            for (CurrentSequence = conCursor.Value + 1; CurrentSequence <= maxPublishedSequence; CurrentSequence++)
            {
                StartOfBatch = CurrentSequence == conCursor.Value + 1;
                EndOfBatch = CurrentSequence == maxPublishedSequence;
                yield return cells[(int) CurrentSequence & ringMask];
            }
            conCursor.Value = maxPublishedSequence;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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

        public readonly string Name;
        private readonly Sequence pubCursor = new Sequence(Sequence.InitialValue);
        private readonly Sequence conCursor = new Sequence(Sequence.InitialValue);
        private readonly Sequence[] conCursors;
        private readonly int ringMask;
        private readonly T[] cells;

        private readonly IClaimStrategy claimStrategy;

        #endregion
    }
}