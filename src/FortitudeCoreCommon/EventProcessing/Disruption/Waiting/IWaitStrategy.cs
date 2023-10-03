using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal interface IWaitStrategy
    {
        long WaitFor(Sequence cursor, long sequence);
        void NotifyAll();
        void InterruptAll();
        void ClearInterrupt();
    }
}