namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal interface IRingBarrier
    {
        int WaitFor(int sequence);
    }

    internal interface IRingBarrierLong
    {
        long WaitFor(long sequence);
    }
}