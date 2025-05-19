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
  , MarketClassification     = 3
  , TickerDetailLevelType    = 4
  , PriceRoundingPrecision   = 5
  , Pip                      = 6
  , TickerDetailBooleanFlags = 7
  , MaximumPublishedLayers   = 8
  , MinSubmitSize            = 9
  , MaxSubmitSize            = 10 // 0x0A
  , IncrementSize            = 11 // 0x0B
  , DefaultMaxValidMs        = 12 // 0x0C 
  , MinimumQuoteLifeMs       = 13 // 0x0D 
  , FeedAdditionalFlags      = 14 // 0x0E
  , QuoteLayerFlags          = 15 // 0x0F
  , LastTradedFlags          = 16 // 0x10
  , ExecutionUpdateFlags     = 17 // 0x11

  , PQSyncStatus                    = 19 // 0x13
  , FeedMarketConnectivityStatus    = 20 // 0x14
  , SourceFeedUpdateSentDateTime    = 21 // 0x15
  , SourceFeedUpdateSentSub2MinTime = 22 // 0x16
  , SourceSequenceId                = 23 // 0x17
  , AdapterSequenceId               = 24 // 0x18
  , ClientSequenceId                = 25 // 0x19
  , FeedSequenceId                  = 26 // 0x1A
  , FeedPublishUpdateFlags          = 27 // 0x1B
  , AdapterSentDateTime             = 28 // 0x1C
  , AdapterSentSub2MinTime          = 29 // 0x1D
  , AdapterReceivedDateTime         = 30 // 0x1E
  , AdapterReceivedSub2MinTime      = 31 // 0x1F
  , ClientReceivedDateTime          = 32 // 0x20
  , ClientReceivedSub2MinTime       = 33 // 0x21
    // Saved when stored 
  , ClientSocketReceivingDateTime    = 34 // 0x22
  , ClientSocketReceivingSub2MinTime = 35 // 0x23
  , ClientProcessedDateTime          = 36 // 0x24
  , ClientProcessedSub2MinTime       = 37 // 0x25
  , ClientDispatchedDateTime         = 38 // 0x26
  , ClientDispatchedSub2MinTime      = 39 // 0x27
  , DownStreamDateTime               = 40 // 0x28
  , DownStreamSub2MinTime            = 41 // 0x29

    // Source TickInstant Values
  , SingleTickValue            = 42 // 0x2A
  , SourceQuoteSentDateTime    = 43 // 0x2B
  , SourceQuoteSentSub2MinTime = 44 // 0x2C
  , QuoteBooleanFlags          = 45 // 0x2D
    // Level 1 Quote Fields
  , SourceQuoteBidDateTime    = 46 // 0x2E
  , SourceQuoteBidSub2MinTime = 47 // 0x2F
  , SourceQuoteAskDateTime    = 48 // 0x30
  , SourceQuoteAskSub2MinTime = 49 // 0x31
  , QuoteValidFromDate        = 50 // 0x32
  , QuoteValidFromSub2MinTime = 51 // 0x33
  , QuoteValidToDate          = 52 // 0x34
  , QuoteValidToSub2MinTime   = 53 // 0x35
    // Best Bid Ask Price Sent a Depth 0 from Level 2 Quote Field "QuoteLayerPrice" below

  , FeedStringUpdates          = 54 // 0x37
  , TrackingDownStream         = 55 // 0x38
  , TickerRegionDetails        = 56 // 0x39 
  , TickerPnLConversionDetails = 57 // 0x39 
  , TickerMarginDetails        = 58 // 0x3A

  , ContinuousPriceAdjustmentOriginalAtPublish    = 63 // 0x3F
  , ContinuousPriceAdjustmentTargetReplayOverride = 64 // 0x40 
  , ContinuousPriceAdjustmentToAppliedToPrices    = 65 // 0x41
  , ContinuousPriceAdjustmentPreviousAtPublish    = 66 // 0x42
  , ContinuousPriceAdjustmentHistorical           = 67 // 0x43

    // price period summary fields
  , PriceCandleStick        = 68 // 0x44
  , CandleConflationSummary = 69 // 0x45
  , CandleRecent            = 70 // 0x46
  , CandleShortTerm         = 71 // 0x47
  , CandleMediumTerm        = 72 // 0x48
  , CandleCurrentDay        = 73 // 0x49
  , CandlePreviousDay       = 74 // 0x4A
  , CandleHistoricalDaily   = 75 // 0x4B
  , CandleLongTerm          = 76 // 0x4C

  , IndicatorsStringUpdates = 77 // 0x4D

  , IndicatorMovingAverageCurrentPeriodSlidingWindow    = 78 // 0x4E
  , IndicatorMovingAverageCurrentTickCountSlidingWindow = 79 // 0x62
  , IndicatorMovingAverageDiscreetShortTerm             = 80 //0x50
  , IndicatorMovingAverageDiscreetMediumTerm            = 81 //0x51
  , IndicatorMovingAverageRecentDaily                   = 82 //0x52
  , IndicatorMovingAverageLongTerm                      = 83 //0x53
  , IndicatorMovingAverageHistorical                    = 84 //0x54

  , IndicatorVolatilityCurrentSlidingWindow           = 85 //0x55
  , IndicatorVolatilityDiscreetTickCountSlidingWindow = 86 //0x56
  , IndicatorVolatilityDiscreetShortTerm              = 87 // 0x57
  , IndicatorVolatilityDiscreetMediumTerm             = 88 // 0x58
  , IndicatorVolatilityRecentDaily                    = 89 // 0x59
  , IndicatorVolatilityHistoricalDaily                = 90 // 0x5A
  , IndicatorVolatilityHistoricalLongTerm             = 91 // 0x5B

  , IndicatorPivotShortTerm  = 92 // 0x5C
  , IndicatorPivotMediumTerm = 93 // 0x5D
  , IndicatorPivotLongTerm   = 94 // 0x5E

  , IndicatorPriceBoundaryLineClosest           = 95 // 0x5F
  , IndicatorPriceBoundaryLinesActiveShortTerm  = 96 // 0x60
  , IndicatorPriceBoundaryLinesActiveMediumTerm = 97 // 0x61
  , IndicatorPriceBoundaryLinesActiveLongTerm   = 98 // 0x62
  , IndicatorPriceBoundaryLinesRecentlyBreached = 99 // 0x63

  , MarketEventStringUpdates = 100 // 0x64

  , MarketEventCurrent   = 101 // 0x5F
  , MarketEventsRecent   = 102 // 0x60
  , MarketEventsUpcoming = 103 // 0x61

  , SignalStringUpdates = 106 // 0x5E

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
  , LastTradedStringUpdates                   = 210 // 0xD2
  , LastTradedTickTrades                      = 211 // 0xD3
  , LastTradedRecentlyByPeriod                = 213 // 0xD5
  , LastTradedRecentlyByTradeCount            = 214 // 0xD6
  , LastTradedInternalDailyAggregate          = 215 // 0xD7
  , LastTradedSimulatorTickTrades             = 217 // 0xD9 
  , LastTradedSimulatorRecentlyByPeriod       = 218 // 0xDA
  , LastTradedSimulatorRecentlyByTradeCount   = 219 // 0xDB
  , LastTradedSimulatorInternalDailyAggregate = 220 // 0xDC
  , LastTradedMarketDailyAggregate            = 221 // 0xDD

  , AdapterInternalOrdersStringUpdates           = 222 // 0xDE
  , AdapterInternalOrdersTick                    = 223 // 0xDF
  , AdapterInternalOrdersRecentByPeriod          = 224 // 0xE0
  , AdapterInternalOrdersRecentByCount           = 225 // 0xE1
  , AdapterInternalOrdersOpen                    = 226 // 0xE2
  , AdapterInternalOrdersSimulatorTick           = 227 // 0xE3
  , AdapterInternalOrdersSimulatorRecentByPeriod = 228 // 0xE4
  , AdapterInternalOrdersSimulatorRecentByCount  = 229 // 0xE5
  , AdapterInternalOrdersSimulatorOpen           = 230 // 0xE6

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
public static class PQQuoteFieldsExtensions
{
    public const byte   SingleByteFieldIdMaxBookDepth          = PQDepthKeyExtensions.SingleByteDepthMask + 1; // 63
    public const ushort TwoByteFieldIdMaxBookDepth             = (ushort)PQDepthKey.DepthMask + 1;             // 16,383
    public const byte   SingleByteFieldIdMaxPossibleLastTrades = SingleByteFieldIdMaxBookDepth;
}
