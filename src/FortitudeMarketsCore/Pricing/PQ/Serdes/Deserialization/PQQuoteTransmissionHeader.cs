#region

using FortitudeCommon.Types;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

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

    public PQMessageFlags MessageFlags { get; set; }
}
