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
  , SourceTickerId           = 1
  , SourceTickerNames        = 2 //StringFieldUpdate
  , SourceNameId             = 3
  , InstrumentNameId         = 4
  , MarketClassification     = 5
  , TickerDetailLevelType    = 6
  , PriceRoundingPrecision   = 7
  , Pip                      = 8
  , TickerDetailBooleanFlags = 9
  , MaximumPublishedLayers   = 10 // 0x0A
  , MinSubmitSize            = 11 // 0x0B
  , MaxSubmitSize            = 12 // 0x0C 
  , IncrementSize            = 13 // 0x0D 
  , DefaultMaxValidMs        = 14 // 0x0E
  , MinimumQuoteLifeMs       = 15 // 0x0Fk
  , QuoteLayerFlags          = 16 // 0x11
  , LastTradedFlags          = 17
  , ExecutionUpdateFlags     = 18
  , ConfigQuoteBehaviorFlags = 19 // 0x10
  , ConfigFeedBehaviorFlags  = 20 // 0x10

  , PQSyncStatus                    = 22 // 0x16
  , FeedMarketConnectivityStatus    = 23 // 0x17
  , SourceFeedUpdateSentDateTime    = 24 // 0x18
  , SourceFeedUpdateSentSub2MinTime = 25 // 0x19
  , SourceSequenceId                = 26 // 0x1A
  , AdapterSequenceId               = 27 // 0x1B
  , ClientSequenceId                = 28 // 0x1C
  , FeedSequenceId                  = 29 // 0x1D
  , FeedPublishUpdateFlags          = 30 // 0x1E
  , AdapterSentDateTime             = 31 // 0x1F
  , AdapterSentSub2MinTime          = 32 // 0x20
  , AdapterReceivedDateTime         = 33 // 0x21
  , AdapterReceivedSub2MinTime      = 34 // 0x22
  , ClientReceivedDateTime          = 35 // 0x23
  , ClientReceivedSub2MinTime       = 36 // 0x24
    // Saved when stored 
  , ClientSocketReceivingDateTime    = 37 // 0x25
  , ClientSocketReceivingSub2MinTime = 38 // 0x26
  , ClientProcessedDateTime          = 39 // 0x27
  , ClientProcessedSub2MinTime       = 40 // 0x28
  , ClientDispatchedDateTime         = 41 // 0x29
  , ClientDispatchedSub2MinTime      = 42 // 0x2A
  , DownStreamDateTime               = 43 // 0x2B
  , DownStreamSub2MinTime            = 44 // 0x2C

    // Source TickInstant Values
  , SingleTickValue            = 45 // 0x2D
  , SourceQuoteSentDateTime    = 46 // 0x2E
  , SourceQuoteSentSub2MinTime = 47 // 0x2F 
  , QuoteBooleanFlags          = 48 // 0x30 
    // Level 1 Quote Fields
  , InstantQuoteBehaviorFlags = 49 // 0x31
  , InstantFeedBehaviorFlags  = 50 // 0x32
  , SourceQuoteBidDateTime    = 51 // 0x33
  , SourceQuoteBidSub2MinTime = 52 // 0x34
  , SourceQuoteAskDateTime    = 53 // 0x35
  , SourceQuoteAskSub2MinTime = 54 // 0x37
  , QuoteValidFromDate        = 55 // 0x38
  , QuoteValidFromSub2MinTime = 56 // 0x39
  , QuoteValidToDate          = 57 // 0x39 
  , QuoteValidToSub2MinTime   = 58 // 0x3A 
    // Best Bid Ask Price Sent a Depth 0 from Level 2 Quote Field "QuoteLayerPrice" below

  , FeedStringUpdates          = 59 // 0x3B
  , TrackingDownStream         = 60 // 0x3C
  , TickerRegionDetails        = 61 // 0x3D
  , TickerPnLConversionDetails = 63 // 0x3F
  , TickerMarginDetails        = 64 // 0x40 

  , ContinuousPriceAdjustmentOriginalAtPublish    = 65 // 0x41
  , ContinuousPriceAdjustmentTargetReplayOverride = 66 // 0x42
  , ContinuousPriceAdjustmentToAppliedToPrices    = 67 // 0x43
  , ContinuousPriceAdjustmentPreviousAtPublish    = 68 // 0x44
  , ContinuousPriceAdjustmentHistorical           = 69 // 0x45

    // price period summary fields
  , PriceCandleStick        = 70 // 0x46
  , CandleConflationSummary = 71 // 0x47
  , CandleRecent            = 72 // 0x48
  , CandleShortTerm         = 73 // 0x49
  , CandleMediumTerm        = 74 // 0x4A
  , CandleCurrentDay        = 75 // 0x4B
  , CandlePreviousDay       = 76 // 0x4C
  , CandleHistoricalDaily   = 77 // 0x4D
  , CandleLongTerm          = 78 // 0x4E

  , IndicatorsStringUpdates = 79 // 0x62

  , IndicatorMovingAverageCurrentPeriodSlidingWindow    = 80 //0x50
  , IndicatorMovingAverageCurrentTickCountSlidingWindow = 81 //0x51
  , IndicatorMovingAverageDiscreetShortTerm             = 82 //0x52
  , IndicatorMovingAverageDiscreetMediumTerm            = 83 //0x53
  , IndicatorMovingAverageRecentDaily                   = 84 //0x54
  , IndicatorMovingAverageLongTerm                      = 85 //0x55
  , IndicatorMovingAverageHistorical                    = 86 //0x56

  , IndicatorVolatilityCurrentSlidingWindow           = 87 // 0x57
  , IndicatorVolatilityDiscreetTickCountSlidingWindow = 88 // 0x58
  , IndicatorVolatilityDiscreetShortTerm              = 89 // 0x59
  , IndicatorVolatilityDiscreetMediumTerm             = 90 // 0x5A
  , IndicatorVolatilityRecentDaily                    = 91 // 0x5B
  , IndicatorVolatilityHistoricalDaily                = 92 // 0x5C
  , IndicatorVolatilityHistoricalLongTerm             = 93 // 0x5D

  , IndicatorPivotShortTerm  = 94 // 0x5E
  , IndicatorPivotMediumTerm = 95 // 0x5F
  , IndicatorPivotLongTerm   = 96 // 0x60

  , IndicatorPriceBoundaryLineClosest           = 97  // 0x61
  , IndicatorPriceBoundaryLinesActiveShortTerm  = 98  // 0x62
  , IndicatorPriceBoundaryLinesActiveMediumTerm = 99  // 0x63
  , IndicatorPriceBoundaryLinesActiveLongTerm   = 100 // 0x64
  , IndicatorPriceBoundaryLinesRecentlyBreached = 101 // 0x65

  , MarketEventStringUpdates = 102 // 0x66

  , MarketEventCurrent   = 103 // 0x67
  , MarketEventsRecent   = 104 // 0x68
  , MarketEventsUpcoming = 105 // 0x69

  , SignalStringUpdates = 106 // 0x6A

  , SignalsActive             = 107 // 0x6B
  , SignalAggregate           = 108 // 0x6C
  , SignalAggregateComponents = 109 // 0x6D
  , SignalActivationsRecent   = 110 // 0x6E

  , StrategyStringUpdates = 111 // 0x6F

  , StrategyDecisionsOnTick              = 112 // 0x70
  , StrategyDecisionsRecent              = 113 // 0x71
  , StrategyDecisionsWithActivePositions = 114 // 0x72

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
