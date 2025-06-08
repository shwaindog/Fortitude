// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

[Flags]
public enum FeedInstantBehaviorFlags : uint
{
    None                                        = 0x00_00_00_00
  , NoAccountPositionUpdates                    = 0x00_00_00_01
  , NoAdapterExecutionDetailUpdates             = 0x00_00_00_02
  , NoCandlesUpdates                            = 0x00_00_00_04
  , NoPivotUpdates                              = 0x00_00_00_08
  , NoMovingAverageUpdates                      = 0x00_00_00_10
  , NoVolatilityUpdates                         = 0x00_00_00_20
  , NoBoundaryLineUpdates                       = 0x00_00_00_40
  , NoIndicatorsUpdates                         = 0x00_00_00_80
  , NoInternalOrderUpdates                      = 0x00_00_00_F8
  , NoInternalOrderLastTradeUpdates             = 0x00_00_01_00
  , NoOnTickLastTradeUpdates                    = 0x00_00_02_00
  , NoRecentlyTradedHistoryUpdates              = 0x00_00_04_00
  , NoMarketNewsUpdates                         = 0x00_00_08_00
  , NoMarketCalendarUpdates                     = 0x00_00_10_00
  , NoMarketTradingStateUpdates                 = 0x00_00_20_00
  , NoSignalsUpdates                            = 0x00_00_40_00
  , NoStrategiesUpdates                         = 0x00_00_80_00
  , NoLimitUpdates                              = 0x00_01_00_00
  , NoLimitBreachesUpdates                      = 0x00_02_00_00
  , NoContinuousPriceAdjustmentUpdates          = 0x00_04_00_00
  , NoPositionConversionInfo                    = 0x00_08_00_00
  , NoPnLInfo                                   = 0x00_10_00_00
  , JustPublishableQuoteFeed                    = 0x00_1F_FF_FF
  , JustQuoteFeed                               = 0x00_3F_FF_FF
  , AlwaysCopyCacheItems                        = 0x00_40_00_00
  , MarketConnectivityStatusCreatesEvents       = 0x00_80_00_00
  , AutoToggleOffInstantConnectivityStatusFlags = 0x01_00_00_00
  , AutoEventsFromAdapterSentTime               = 0x02_00_00_00
  , AutoEventsFromClientReceivedTime            = 0x04_00_00_00
}

public static class FeedInstantBehaviorFlagsExtensions
{

}