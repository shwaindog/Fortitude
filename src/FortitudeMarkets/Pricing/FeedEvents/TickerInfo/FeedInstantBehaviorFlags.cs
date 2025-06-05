// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

[Flags]
public enum FeedInstantBehaviorFlags : uint
{
    None                                  = 0x00_00_00_00
  , DefaultFullFeedUpdates                = 0x00_00_00_01
  , NoAccountPositionUpdates              = 0x00_00_00_02
  , NoAdapterExecutionDetailUpdates       = 0x00_00_00_04
  , NoCandlesUpdates                      = 0x00_00_00_08
  , NoPivotUpdates                        = 0x00_00_00_10
  , NoMovingAverageUpdates                = 0x00_00_00_20
  , NoVolatilityUpdates                   = 0x00_00_00_40
  , NoBoundaryLineUpdates                 = 0x00_00_00_80
  , NoIndicatorsUpdates                   = 0x00_00_01_00
  , NoInternalOrdersUpdates               = 0x00_00_02_00
  , NoOnTickLastTradeUpdates              = 0x00_00_04_00
  , NoRecentlyTradedHistoryUpdates        = 0x00_00_08_00
  , NoTradingEventUpdates                 = 0x00_00_10_00
  , NoMarketEventUpdates                  = 0x00_00_20_00
  , NoSignalsUpdates                      = 0x00_00_40_00
  , NoStrategiesUpdates                   = 0x00_00_80_00
  , NoLimitUpdates                        = 0x00_01_00_00
  , NoLimitBreachesUpdates                = 0x00_02_00_00
  , NoContinuousPriceAdjustmentUpdates    = 0x00_04_00_00
  , NoPositionConversionInfo              = 0x00_08_00_00
  , NoPnLInfo                             = 0x00_10_00_00
  , JustPublishableQuoteFeed              = 0x00_1F_FF_FF
  , JustQuoteFeed                         = 0x00_3F_FF_FF
  , AlwaysCopyCacheItems                  = 0x00_40_00_00
  , MarketConnectivityStatusCreatesEvents = 0x00_80_00_00
  , AutoEventsFromAdapterSentTime         = 0x01_00_00_00
  , AutoEventsFromClientReceivedTime      = 0x02_00_00_00
}
