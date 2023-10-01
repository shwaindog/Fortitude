using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal interface IClaimStrategy
    {
        long Claim();
        void WaitFor(long sequence, Sequence[] cursors);
        void Serialize(Sequence cursor, long sequence);
    }
}