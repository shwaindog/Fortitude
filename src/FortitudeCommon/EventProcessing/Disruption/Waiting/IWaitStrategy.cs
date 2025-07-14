using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal interface IWaitStrategy
    {
        void NotifyAll();
        void InterruptAll();
        void ClearInterrupt();
    }

    internal interface IWaitStrategyInt : IWaitStrategy
    {
        int WaitFor(Sequence cursor, int sequence);
    }

    internal interface IWaitStrategyLong : IWaitStrategy
    {
        long WaitFor(SequenceLong cursor, long sequence);
    }
}