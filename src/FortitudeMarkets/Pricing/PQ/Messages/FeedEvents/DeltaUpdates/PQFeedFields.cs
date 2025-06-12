// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

public enum PQFeedFields : byte
{
    None = 0
    // Source Ticker Info
  , SourceTickerId                      = 1
  , SourceTickerDefinitionStringUpdates = 2 //StringFieldUpdate
  , SourceTickerDefinition              = 3

  , PQSyncStatus                    = 4 // Used when persisting to storage
  , FeedMarketConnectivityStatus    = 5
  , SourceFeedUpdateSentDateTime    = 6
  , SourceFeedUpdateSentSub2MinTime = 7
  , SourceSequenceId                = 8
  , AdapterSequenceId               = 9
  , ClientSequenceId                = 10 // 0x0A
  , FeedSequenceId                  = 11 // 0x0B
  , FeedPublishUpdateFlags          = 12 // 0x0C
  , AdapterSentDateTime             = 13 // 0x0D
  , AdapterSentSub2MinTime          = 14 // 0x0E
  , AdapterReceivedDateTime         = 15 // 0x0F
  , AdapterReceivedSub2MinTime      = 16 // 0x10
  , ClientReceivedDateTime          = 17 // 0x11
  , ClientReceivedSub2MinTime       = 18 // 0x12
    // Saved when stored 
  , ClientSocketReceivingDateTime    = 19 // 0x13
  , ClientSocketReceivingSub2MinTime = 20 // 0x14
  , ClientProcessedDateTime          = 21 // 0x15
  , ClientProcessedSub2MinTime       = 22 // 0x16
  , ClientDispatchedDateTime         = 23 // 0x17
  , ClientDispatchedSub2MinTime      = 24 // 0x18
  , DownStreamDateTime               = 25 // 0x19
  , DownStreamSub2MinTime            = 26 // 0x1A

    // Source TickInstant Values
  , SingleTickValue            = 27 // 0x1B
  , SourceQuoteSentDateTime    = 28 // 0x1C
  , SourceQuoteSentSub2MinTime = 29 // 0x1D
  , QuoteBooleanFlags          = 30 // 0x1E
    // Level 1 Quote Fields
  , InstantQuoteBehaviorFlags = 31 // 0x1F
  , InstantQuoteControlFlags  = 32 // 0x20
  , InstantFeedBehaviorFlags  = 33 // 0x21
  , InstantFeedControlFlags   = 34 // 0x22
  , SourceQuoteBidDateTime    = 35 // 0x23
  , SourceQuoteBidSub2MinTime = 36 // 0x24
  , SourceQuoteAskDateTime    = 37 // 0x25
  , SourceQuoteAskSub2MinTime = 38 // 0x26
  , QuoteValidFromDate        = 39 // 0x27
  , QuoteValidFromSub2MinTime = 40 // 0x28
  , QuoteValidToDate          = 41 // 0x29
  , QuoteValidToSub2MinTime   = 43 // 0x2A
    // Best Bid Ask Price Sent a Depth 0 from Level 2 Quote Field "QuoteLayerPrice" below

  , FeedStringUpdates          = 50 // 0x32
  , TrackingDownStream         = 51 // 0x33
  , TickerRegionDetails        = 52 // 0x34
  , TickerPnLConversionDetails = 53 // 0x35
  , TickerMarginDetails        = 54 // 0x36

  , ContinuousPriceAdjustmentOriginalAtPublish    = 55 // 0x41
  , ContinuousPriceAdjustmentTargetReplayOverride = 56 // 0x42
  , ContinuousPriceAdjustmentToAppliedToPrices    = 57 // 0x43
  , ContinuousPriceAdjustmentPreviousAtPublish    = 58 // 0x44
  , ContinuousPriceAdjustmentHistorical           = 59 // 0x45

    // price period summary fields
  , PriceCandleStick         = 60 // 0x3C
  , CandleConflationSummary  = 61 // 0x3D
  , CandleUnderConstruction  = 62 // 0x3E  // live charting
  , CandleLastComplete       = 63 // 0x3F  // live and historical charting
  , CandleHistoryShortTerm   = 64 // 0x40  // for strategies 
  , CandleHistoryMediumTerm  = 65 // 0x41  // |
  , CandleHistoryCurrentDay  = 66 // 0x42  // V
  , CandleHistoryPreviousDay = 67 // 0x43
  , CandleHistoricalDaily    = 68 // 0x44
  , CandleHistoryLongTerm    = 69 // 0x45

  , IndicatorsStringUpdates = 70 // 0x46

  , IndicatorMovingAverageCurrentPeriodSlidingWindow    = 71 // 0x47
  , IndicatorMovingAverageCurrentTickCountSlidingWindow = 72 // 0x48
  , IndicatorMovingAverageDiscreetShortTerm             = 73 // 0x49
  , IndicatorMovingAverageDiscreetMediumTerm            = 74 // 0x4A
  , IndicatorMovingAverageRecentDaily                   = 75 // 0x4B
  , IndicatorMovingAverageLongTerm                      = 76 // 0x4C
  , IndicatorMovingAverageHistorical                    = 77 // 0x4D

  , IndicatorVolatilityCurrentSlidingWindow           = 78 // 0x4E
  , IndicatorVolatilityDiscreetTickCountSlidingWindow = 79 // 0x4F
  , IndicatorVolatilityDiscreetShortTerm              = 80 // 0x50
  , IndicatorVolatilityDiscreetMediumTerm             = 81 // 0x51
  , IndicatorVolatilityRecentDaily                    = 82 // 0x52
  , IndicatorVolatilityHistoricalDaily                = 83 // 0x53
  , IndicatorVolatilityHistoricalLongTerm             = 84 // 0x54

  , IndicatorPivotShortTerm  = 85 // 0x55
  , IndicatorPivotMediumTerm = 86 // 0x56
  , IndicatorPivotLongTerm   = 87 // 0x57

  , IndicatorPriceBoundaryLineClosest           = 88 // 0x58
  , IndicatorPriceBoundaryLinesActiveShortTerm  = 89 // 0x59
  , IndicatorPriceBoundaryLinesActiveMediumTerm = 90 // 0x5A
  , IndicatorPriceBoundaryLinesActiveLongTerm   = 91 // 0x5B
  , IndicatorPriceBoundaryLinesRecentlyBreached = 92 // 0x5C 

  , SignalStringUpdates = 93 // 0x5D

  , SignalsActive             = 94 // 0x5E 
  , SignalAggregate           = 95 // 0x5F 
  , SignalAggregateComponents = 96 // 0x60 
  , SignalActivationsRecent   = 97 // 0x61

  , StrategyStringUpdates = 98 // 0x62 

  , StrategyDecisionsOnTick              = 99  // 0x63 
  , StrategyDecisionsRecent              = 100 // 0x64
  , StrategyDecisionsWithActivePositions = 101 // 0x65

  , MarketEventStringUpdates = 106 // 0x6A

  , MarketTradingStateEventOnTick = 107 // 0x6B
  , MarketTradingStateActive      = 108 // 0x6C
  , MarketTradingStateRecent      = 109 // 0x6D
  , MarketTradingStateUpcoming    = 110 // 0x6E
  , MarketCalendarCurrent         = 111 // 0x6F
  , MarketCalendarUpcoming        = 112 // 0x70
  , MarketNewsEventCurrent        = 113 // 0x71
  , MarketNewsEventsRecent        = 114 // 0x72
  , MarketNewsEventsUpcoming      = 115 // 0x73

  , ExecutionStatsCurrent  = 117 // 0x75
  , ExecutionStatsRecent   = 118 // 0x76
  , ExecutionStatsDaily    = 119 // 0x77
  , ExecutionStatsLifetime = 120 // 0x78

  , LimitStringUpdates = 121 // 0x79

  , LimitMarginLimits         = 123 // 0x7B
  , LimitPnlLimits            = 124 // 0x7C
  , LimitSourceLimits         = 125 // 0x7D
  , LimitTickerLimits         = 126 // 0x7E
  , LimitOpenPositionLimits   = 127 // 0x7F
  , LimitAdapterLimits        = 128 // 0x80
  , LimitPortfolioLimits      = 129 // 0x81
  , LimitStrategyLimits       = 130 // 0x82
  , LimitTraderLimits         = 131 // 0x83
  , LimitSimulatorLimits      = 132 // 0x84
  , LimitMarginCurrency       = 133 // 0x85
  , LimitMarginConversionRate = 134 // 0x86

  , AccountStringUpdates   = 135 // 0x87
  , SimulatorStringUpdates = 136 // 0x88

  , SimulatorBacktestOriginalPosition           = 140 // 0x8C  // Includes Aggregate, Daily and Open PnL
  , SimulatorBacktestOriginalComparisonPosition = 141 // 0x8D
  , SimulatorBacktestSimNoLimitsPosition        = 142 // 0x8E
  , SimulatorBacktestSimOnlyLimitsPosition      = 143 // 0x8F
  , SimulatorBacktestSimAllLimitsPosition       = 144 // 0x90
  , SimulatorBacktestSimEstimatedTotalPosition  = 145 // 0x91

  , PositionsSource                 = 148 // 0x94       // , NetOpenAccountPosition     = 137 // 0x89
  , PositionsAdapterTicker          = 149 // 0x95       // , NetOpenAccountPnl          = 138 // 0x8A
  , PositionsPortfolio              = 150 // 0x96       // , NetRealisedAccountDailyPnl = 139 // 0x8B
  , PositionsStrategy               = 151 // 0x97
  , PositionsTrader                 = 152 // 0x98
  , PositionsSimulatorNoLimits      = 153 // 0x99
  , PositionsSimulatorSimLimitsOnly = 154 // 0x9A
  , PositionsSimulatorAllLimited    = 155 // 0x9B

  , LimitBreachesCurrent   = 157 // 0x9D
  , LimitBreachRecent      = 158 // 0x9E
  , LimitApproachingAlerts = 159 // 0x9F

  , PortfolioLimitBreachCurrent    = 160 // 0xA0
  , PortfolioLimitBreachRecent     = 161 // 0xA1
  , PortfolioLimitApproachingAlert = 162 // 0xA2

    // Level 2 Quote Fields
  , QuoteOpenInterestTotal    = 163 // 0xA3
  , QuoteOpenInterestSided    = 164 // 0xA4
  , QuoteDailyTotalTickCount  = 165 // 0xA5
  , QuoteDailySidedTickCount  = 166 // 0xA6
  , QuoteDailyTradedAggregate = 167 // 0xA7

    // Level 2 Quote Layer Fields
  , QuoteLayerStringUpdates = 168 // 0xA8  StringFieldUpdate
  , QuoteLayers             = 169 // 0xA9

  , QuoteLayerPrice                  = 170 // 0xAA
  , QuoteLayerVolume                 = 171 // 0xAB
  , QuoteLayerBooleanFlags           = 172 // 0xAC
  , QuoteLayerSourceId               = 173 // 0xAD
  , QuoteLayerSourceQuoteRef         = 174 // 0xAE
  , QuoteLayerValueDate              = 175 // 0xAF
  , QuoteLayerContractType           = 176 // 0xB0
  , QuoteLayerContractExpiryDate     = 177 // 0xB1
  , QuoteLayerContractNameId         = 178 // 0xB2
  , QuoteLayerNearValueDate          = 179 // 0xB3
  , QuoteLayerFarValueDate           = 180 // 0xB4
  , QuoteLayerNearForwardPoints      = 181 // 0xB5
  , QuoteLayerFarForwardPoints       = 182 // 0xB6
  , QuoteLayerUnderlyingTickerId     = 183 // 0xB7
  , QuoteLayerBrokeragePriceDelta    = 184 // 0xB8
  , QuoteLayerSlippagePriceDelta     = 185 // 0xB9
  , QuoteLayerEffectivePriceDelta    = 186 // 0xBA
  , QuoteLayerRemainingVolume        = 187 // 0xBB
  , QuoteLayerPendingExecutionVolume = 188 // 0xBC
  , QuoteLayerInternalVolume         = 189 // 0xBD
  , QuoteLayerOrdersCount            = 190 // 0xBE
  , QuoteLayerShiftOrders            = 191 // 0xBF
  , QuoteLayerOrders                 = 192 // 0x90
  , QuoteLayerValidFromDate          = 193 // 0x91
  , QuoteLayerValidFromSub2MinTime   = 194 // 0x92
  , QuoteLayerValidToDate            = 195 // 0x93
  , QuoteLayerValidToSub2MinTime     = 196 // 0x94
  , QuoteLayerLastUpdatedDate        = 197 // 0x95
  , QuoteLayerLastUpdatedSub2MinTime = 198 // 0x96

  , QuoteLayersRangeEnd = 209 // 0xD1

    // Level 3 Quote Layer Fields
  , LastTradedStringUpdates                          = 210 // 0xD2
  , LastTradedTickTrades                             = 211 // 0xD3
  , LastTradedAllRecentlyLimitedHistory              = 212 // 0xD4  // expected no more than 100
  , LastTradedRecentInternalOrderTrades              = 213 // 0xD5
  , LastTradedInternalOpeningPositionTrades          = 215 // 0xD7 
  , LastTradedSimulatorTickTrades                    = 216 // 0xD8
  , LastTradedSimulatorAllRecentlyLimitedHistory     = 217 // 0xD9 
  , LastTradedSimulatorRecentInternalOrderTrades     = 218 // oxDA
  , LastTradedSimulatorInternalOpeningPositionTrades = 219 // 0xDB
  , LastTradedAlertTrades                            = 220 // 0xDC
  , LastTradedRecentMarketAggregates                 = 221 // 0xDD
  , LastTradedMarketDailyAggregates                  = 222 // 0xDE

  , AdapterInternalOrdersStringUpdates           = 226 // 0xE2
  , AdapterInternalOrdersTick                    = 227 // 0xE3
  , AdapterInternalOrdersRecentByPeriod          = 228 // 0xE4
  , AdapterInternalOrdersRecentByCount           = 229 // 0xE5
  , AdapterInternalOrdersOpen                    = 230 // 0xE6
  , AdapterInternalOrdersSimulatorTick           = 231 // 0xE7
  , AdapterInternalOrdersSimulatorRecentByPeriod = 232 // 0xE8
  , AdapterInternalOrdersSimulatorRecentByCount  = 233 // 0xE9
  , AdapterInternalOrdersSimulatorOpen           = 234 // 0xEA

  , QuoteBatchId            = 238 // 0xEE
  , QuoteSourceQuoteRef     = 239 // 0xEF
  , QuoteValueDate          = 240 // 0xF0
  , QuoteNearDate           = 241 // 0xF1
  , QuoteNearForwardType    = 242 // 0xF2
  , QuoteFarDate            = 243 // 0xF3
  , QuoteFarForwardType     = 244 // 0xF4
  , QuoteUnderlyingTickerId = 245 // 0xF5
  , QuoteContractType       = 246 // 0xF6
  , QuoteExpiryDate         = 247 // 0xF7

  , ParentContextRemapped = 255 // 0xFF
}

[TestClassNotRequired]
public static class PQFeedFieldsExtensions
{
    public const byte   SingleByteFieldIdMaxBookDepth          = PQDepthKeyExtensions.SingleByteDepthMask + 1; // 63
    public const ushort TwoByteFieldIdMaxBookDepth             = (ushort)PQDepthKey.DepthMask + 1;             // 16,383
    public const byte   SingleByteFieldIdMaxPossibleLastTrades = SingleByteFieldIdMaxBookDepth;
}
