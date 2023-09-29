using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal class IndependentRingBarrier : IRingBarrier
    {
        private readonly Sequence cursor;
        private readonly IWaitStrategy waitStrategy;

        public IndependentRingBarrier(IWaitStrategy waitStrategy, Sequence cursor)
        {
            this.waitStrategy = waitStrategy;
            this.cursor = cursor;
        }

        public long WaitFor(long sequence)
        {
            return waitStrategy.WaitFor(cursor, sequence);
        }
    }
}