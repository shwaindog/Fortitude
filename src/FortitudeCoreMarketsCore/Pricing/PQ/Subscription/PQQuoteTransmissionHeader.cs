using FortitudeCommon.Types;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    [TestClassNotRequired]
    internal class PQQuoteTransmissionHeader
    {
        public readonly PQFeedType Origin;
        public uint SequenceId;

        public PQQuoteTransmissionHeader(PQFeedType origin)
        {
            Origin = origin;
            SequenceId = 0;
        }
    }
}