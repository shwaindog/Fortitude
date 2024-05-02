#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication.BusRules.BusMessages;

public class PublishQuoteEvent : RecyclableObject
{
    public ILevel0Quote PublishQuote { get; set; } = null!;

    public PQMessageFlags? MessageFlags { get; set; }

    public uint? OverrideSequenceNumber { get; set; }

    public override void StateReset()
    {
        PublishQuote = null!;
        MessageFlags = null!;
        base.StateReset();
    }

    public override string ToString() =>
        $"{nameof(PublishQuoteEvent)}({nameof(PublishQuote)}: {PublishQuote}, {nameof(MessageFlags)}: {MessageFlags})";
}
