using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal class IndependentRingBarrier(IWaitStrategyInt waitStrategy, Sequence cursor) : IRingBarrier
    {
        public int WaitFor(int sequence)
        {
            return waitStrategy.WaitFor(cursor, sequence);
        }
    }

    internal class IndependentRingBarrierLong(IWaitStrategyLong waitStrategy, SequenceLong cursor) : IRingBarrierLong
    {
        public long WaitFor(long sequence)
        {
            return waitStrategy.WaitFor(cursor, sequence);
        }
    }
}