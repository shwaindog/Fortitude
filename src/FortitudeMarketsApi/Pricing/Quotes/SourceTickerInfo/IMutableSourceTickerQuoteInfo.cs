#region

using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

public interface IMutableSourceTickerQuoteInfo : ISourceTickerQuoteInfo, IMutableUniqueSourceTickerIdentifier
{
    new decimal RoundingPrecision { get; set; }
    new decimal MinSubmitSize { get; set; }
    new decimal MaxSubmitSize { get; set; }
    new decimal IncrementSize { get; set; }
    new ushort MinimumQuoteLife { get; set; }
    new LayerFlags LayerFlags { get; set; }
    new byte MaximumPublishedLayers { get; set; }
    new LastTradedFlags LastTradedFlags { get; set; }
    new IMutableSourceTickerQuoteInfo Clone();
}
