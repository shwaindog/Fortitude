#region

using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

public interface ISourceTickerQuoteInfo : IUniqueSourceTickerIdentifier
{
    decimal RoundingPrecision { get; }
    decimal MinSubmitSize { get; }
    decimal MaxSubmitSize { get; }
    decimal IncrementSize { get; }
    ushort MinimumQuoteLife { get; }
    LayerFlags LayerFlags { get; }
    byte MaximumPublishedLayers { get; }
    LastTradedFlags LastTradedFlags { get; }
    string FormatPrice { get; }
    new ISourceTickerQuoteInfo Clone();
}
