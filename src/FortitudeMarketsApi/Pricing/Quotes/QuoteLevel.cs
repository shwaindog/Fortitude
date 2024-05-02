#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public enum QuoteLevel : byte
{
    Level0 // used for publishing metrics, indicators or single values not exchange quotes
    , Level1
    , Level2
    , Level3
}

public static class QuoteLevelExtensions
{
    public static QuoteLevel MostCompactQuoteLevel(this ISourceTickerQuoteInfo sourceTickerQuoteInfo)
    {
        return sourceTickerQuoteInfo switch
        {
            _ when sourceTickerQuoteInfo.LayerFlags.MostCompactLayerType() == LayerType.PriceVolume &&
                   sourceTickerQuoteInfo is { MaximumPublishedLayers: <= 1, LastTradedFlags: LastTradedFlags.None } => QuoteLevel.Level1
            , _ when sourceTickerQuoteInfo is { LastTradedFlags: LastTradedFlags.None, MaximumPublishedLayers: > 1 } => QuoteLevel.Level2
            , _ => QuoteLevel.Level3
        };
    }
}
