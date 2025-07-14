using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    internal interface IClaimStrategy
    {
        int Claim();
        void WaitFor(int sequenceValue, Sequence[] cursors);
        void Serialize(Sequence cursor, int sequenceValue);
    }

    internal interface IClaimStrategyLong
    {
        long Claim();
        void WaitFor(long sequenceValue, SequenceLong[] cursors);
        void Serialize(SequenceLong cursor, long sequenceValue);
    }
}