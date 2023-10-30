using System.Threading;
using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.AsyncProcessing
{
    public class Sequencer : ISequencer
    {
        private PaddedAtomicLong sequence = new PaddedAtomicLong(-1);
        private PaddedVolatileLong cursor = new PaddedVolatileLong(-1);

        public long Claim()
        {
            return sequence.IncrementAndGet();
        }

        public void Serialize(long waitFor)
        {
            long expectedSequence = waitFor - 1;
            while (expectedSequence != cursor.Value)
            {
                Thread.Yield();
            }
        }

        public void Release(long completedAt)
        {
            cursor.Value = completedAt;
        }
    }
}
