#region

using FortitudeCommon.Types;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

[TestClassNotRequired]
internal class PQQuoteTransmissionHeader
{
    public uint SequenceId;

    public PQMessageFlags MessageFlags { get; set; }
}
