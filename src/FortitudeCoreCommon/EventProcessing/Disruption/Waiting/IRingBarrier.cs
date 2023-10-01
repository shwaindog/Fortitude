namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal interface IRingBarrier
    {
        long WaitFor(long sequence);
    }
}