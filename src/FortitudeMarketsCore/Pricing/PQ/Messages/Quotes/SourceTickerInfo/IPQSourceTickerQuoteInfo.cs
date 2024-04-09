#region

using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

public interface IPQSourceTickerQuoteInfo : IPQUniqueSourceTickerIdentifier, IMutableSourceTickerQuoteInfo,
    IPQQuotePublicationPrecisionSettings
{
    bool IsRoundingPrecisionUpdated { get; set; }
    bool IsMinSubmitSizeUpdated { get; set; }
    bool IsMaxSubmitSizeUpdated { get; set; }
    bool IsIncrementSizeUpdated { get; set; }
    bool IsMinimumQuoteLifeUpdated { get; set; }
    bool IsLayerFlagsUpdated { get; set; }
    bool IsMaximumPublishedLayersUpdated { get; set; }
    bool IsLastTradedFlagsUpdated { get; set; }
    IPQNameIdLookupGenerator? SourceNameIdLookup { get; set; }
    IPQNameIdLookupGenerator? TraderNameIdLookup { get; set; }
    IPQNameIdLookupGenerator? LastTraderNameLookup { get; set; }
}
