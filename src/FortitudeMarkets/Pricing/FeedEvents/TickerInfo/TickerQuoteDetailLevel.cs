// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

public enum TickerQuoteDetailLevel : byte
{
    SingleValue // used for publishing metrics, indicators or single values not exchange quotes
  , Level1Quote
  , Level2Quote
  , Level3Quote
}

public static class TickerDetailLevelExtensions
{
    public static TickerQuoteDetailLevel MostCompactQuoteLevel(this ISourceTickerInfo sourceTickerInfo)
    {
        return sourceTickerInfo switch
               {
                   _ when sourceTickerInfo.LayerFlags.MostCompactLayerType() == LayerType.PriceVolume &&
                          sourceTickerInfo is { MaximumPublishedLayers: <= 1, LastTradedFlags: LastTradedFlags.None } => TickerQuoteDetailLevel
                       .Level1Quote
                 , _ when sourceTickerInfo is { LastTradedFlags: LastTradedFlags.None, MaximumPublishedLayers: > 1 } => TickerQuoteDetailLevel
                       .Level2Quote
                 , _ => TickerQuoteDetailLevel.Level3Quote
               };
    }

    public static bool LessThan(this TickerQuoteDetailLevel lhs, TickerQuoteDetailLevel rhs)             => (byte)lhs < (byte)rhs;
    public static bool GreaterThanOrEqualTo(this TickerQuoteDetailLevel lhs, TickerQuoteDetailLevel rhs) => (byte)lhs >= (byte)rhs;
}
