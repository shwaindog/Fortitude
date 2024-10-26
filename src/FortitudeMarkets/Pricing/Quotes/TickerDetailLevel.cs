// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public enum TickerDetailLevel : byte
{
    SingleValue // used for publishing metrics, indicators or single values not exchange quotes
  , Level1Quote
  , Level2Quote
  , Level3Quote
}

public static class TickerDetailLevelExtensions
{
    public static TickerDetailLevel MostCompactQuoteLevel(this ISourceTickerInfo sourceTickerInfo)
    {
        return sourceTickerInfo switch
               {
                   _ when sourceTickerInfo.LayerFlags.MostCompactLayerType() == LayerType.PriceVolume &&
                          sourceTickerInfo is { MaximumPublishedLayers: <= 1, LastTradedFlags: LastTradedFlags.None } => TickerDetailLevel
                       .Level1Quote
                 , _ when sourceTickerInfo is { LastTradedFlags: LastTradedFlags.None, MaximumPublishedLayers: > 1 } => TickerDetailLevel
                       .Level2Quote
                 , _ => TickerDetailLevel.Level3Quote
               };
    }

    public static bool LessThan(this TickerDetailLevel lhs, TickerDetailLevel rhs)             => (byte)lhs < (byte)rhs;
    public static bool GreaterThanOrEqualTo(this TickerDetailLevel lhs, TickerDetailLevel rhs) => (byte)lhs >= (byte)rhs;
}
