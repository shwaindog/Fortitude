// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

public enum PQQuoteFields : byte
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
  , QuoteAdditionalFlags     = 14 // 0x0E
  , LayerFlags               = 15 // 0x0F
  , LastTradedFlags          = 16 // 0x10
  , ExecutionUpdateFlags     = 17 // 0x11

    // Source TickInstant Values
  , QuoteNameDictionaryUpsertCommand = 19

  , SingleTickValue       = 20 // 0x14
  , SourceSentDateTime    = 21 // 0x15
  , SourceSentSub2MinTime = 22 // 0x16
  , QuoteBooleanFlags     = 23 // 0x17
    // Saved when stored 
  , PQSyncStatus               = 24 // 0x14
  , SocketReceivingDateTime    = 25 // 0x15
  , SocketReceivingSub2MinTime = 26 // 0x16
  , ProcessedDateTime          = 27 // 0x17
  , ProcessedSub2MinTime       = 28 // 0x18
  , DispatchedDateTime         = 29 // 0x1D
  , DispatchedSub2MinTime      = 30 // 0x1E
  , ClientReceivedDateTime     = 31 // 0x1F
  , ClientReceivedSub2MinTime  = 32 // 0x20

    // Level 1 Quote Fields
  , AdapterSentDateTime        = 33 // 0x21
  , AdapterSentSub2MinTime     = 34 // 0x22
  , AdapterReceivedDateTime    = 35 // 0x23
  , AdapterReceivedSub2MinTime = 36 // 0x24
  , SourceBidDateTime          = 37 // 0x25
  , SourceBidSub2MinTime       = 38 // 0x26
  , SourceAskDateTime          = 39 // 0x27
  , SourceAskSub2MinTime       = 40 // 0x28
  , QuoteValidFromDate         = 41 // 0x29
  , QuoteValidFromSub2MinTime  = 42 // 0x2A
  , QuoteValidToDate           = 43 // 0x2B
  , QuoteValidToSub2MinTime    = 44 // 0x2C
    // Best Bid Ask Price Sent a Depth 0 from Level 2 Quote Fields below
  , PublishUpdateFlags     = 45 // 0x2D
  , FeedTypeFlags          = 46 // 0x2E
  , StageSequenceIds       = 47 // 0x2F
  , PreviousTradingDayDate = 48 // 0x30
  , NextTradingDayDate     = 49 // 0x31

  , ContinuousPriceAdjustmentOriginalAtPublish    = 56  // 0x38
  , ContinuousPriceAdjustmentTargetReplayOverride = 57  // 0x39
  , ContinuousPriceAdjustmentToAppliedToPrices    = 58  // 0x3A
  , ContinuousPriceAdjustmentPreviousAtPublish    = 59  // 0x3B
  , ContinuousPriceAdjustmentHistorical           = 60  // 0x3C

    // price period summary fields
  , PriceCandleStick        = 61 // 0x3D
  , CandleConflationSummary = 62 // 0x3E
  , CandleRecent            = 63 // 0x3F
  , CandleShortTerm         = 64 // 0x40
  , CandleMediumTerm        = 65 // 0x41
  , CandleCurrentDay        = 66 // 0x42
  , CandlePreviousDay       = 67 // 0x43
  , CandleHistoricalDaily   = 68 // 0x44
  , CandleLongTerm          = 69 // 0x45

  , MovingAverageCurrentPeriodSlidingWindow    = 70 //0x46
  , MovingAverageCurrentTickCountSlidingWindow = 71 //0x47
  , MovingAverageDiscreetShortTerm             = 72 //0x48
  , MovingAverageDiscreetMediumTerm            = 73 //0x49
  , MovingAverageRecentDaily                   = 74 //0x4A
  , MovingAverageLongTerm                      = 75 //0x4B
  , MovingAverageHistorical                    = 76 //0x4C

  , VolatilityCurrentSlidingWindow           = 77 // 0x4D
  , VolatilityDiscreetTickCountSlidingWindow = 78 // 0x4E
  , VolatilityDiscreetShortTerm              = 79 // 0x4F
  , VolatilityDiscreetMediumTerm             = 80 // 0x50
  , VolatilityRecentDaily                    = 81 // 0x51
  , VolatilityHistoricalDaily                = 82 // 0x52
  , VolatilityHistoricalLongTerm             = 83 // 0x53

  , MarketEventNameIdUpsertCommand = 84 // 0x54
  , MarketEventCurrent             = 85 // 0x55
  , MarketEventsRecent             = 86 // 0x56
  , MarketEventsUpcoming           = 87 // 0x57

  , QuoteChartingDictionaryUpsertCommand = 88 // 0x58

  , PivotShortTerm  = 89 // 0x59
  , PivotMediumTerm = 90 // 0x5A
  , PivotLongTerm   = 91 // 0x5B

  , PriceBoundaryLineClosest           = 92 // 0x5C
  , PriceBoundaryLinesActiveShortTerm  = 93 // 0x5D
  , PriceBoundaryLinesActiveMediumTerm = 94 // 0x5E
  , PriceBoundaryLinesActiveLongTerm   = 95 // 0x5F
  , PriceBoundaryLinesRecentlyBreached = 96 // 0x60

  , SignalsActive             = 98  // 0x62
  , SignalAggregate           = 97  // 0x63
  , SignalAggregateComponents = 99  // 0x64
  , SignalActivationsRecent   = 100 // 0x65

  , StrategyDecisionsOnTick      = 102 // 0x66
  , StrategyDecisionsRecent      = 103 // 0x67
  , StrategyDecisionActiveOrders = 104 // 0x68

  , ExecutionStatsCurrent  = 107 // 0x6B
  , ExecutionStatsRecent   = 108 // 0x6C
  , ExecutionStatsDaily    = 109 // 0x6D
  , ExecutionStatsLifetime = 110 // 0x6E

  , LimitMarginLimits         = 113 // 0x71
  , LimitPnlLimits            = 114 // 0x72
  , LimitSourceLimits         = 115 // 0x73
  , LimitTickerLimits         = 116 // 0x74
  , LimitOpenPositionLimits   = 117 // 0x75
  , LimitAdapterLimits        = 118 // 0x76
  , LimitPortfolioLimits      = 119 // 0x77
  , LimitStrategyLimits       = 120 // 0x78
  , LimitTraderLimits         = 121 // 0x79
  , LimitSimulatorLimits      = 122 // 0x7A
  , LimitMarginCurrency       = 123 // 0x7B
  , LimitMarginConversionRate = 124 // 0x7C

  , NetOpenDirection      = 128 // 0x80
  , NetOpenMarketPosition = 129 // 0x81
  , NetOpenPnl            = 130 // 0x82
  , NetRealisedDailyPnl   = 131 // 0x83

  , NetOpenSimulatorDirection    = 134 // 0x86
  , NetOpenSimulatorPosition     = 135 // 0x87
  , NetOpenSimulatorPnl          = 136 // 0x88
  , NetRealisedSimulatorDailyPnl = 137 // 0x89

  , SourcePositions        = 140  // 0x8C
  , AdapterTickerPositions = 141  // 0x8D
  , PortfolioPositions     = 142  // 0x8E
  , StrategyPositions      = 143  // 0x8F
  , TraderPositions        = 144  // 0x90
  , SimulatorPositions     = 145  // 0x91

  , LimitBreachesCurrent   = 147 // 0x93
  , LimitBreachRecent      = 148 // 0x94
  , LimitApproachingAlerts = 149 // 0x95

  , PortfolioLimitBreachCurrent    = 150 // 0x96
  , PortfolioLimitBreachRecent     = 151 // 0x97
  , PortfolioLimitApproachingAlert = 152 // 0x98

    // Level 2 Quote Fields
  , OpenInterestTotal    = 153 // 0x99
  , OpenInterestSided    = 154 // 0x9A
  , DailyTotalTickCount  = 155 // 0x9B
  , DailySidedTickCount  = 156 // 0x9C
  , DailyTradedAggregate = 157 // 0x9D

    // Level 2 Quote Layer Fields
  , LayerNameDictionaryUpsertCommand = 158 // 0x9E  StringFieldUpdate
  , LayerElementShift                = 159 // 0x9F

  , Price                       = 160 // 0xA0 
  , Volume                      = 161 // 0xA1
  , LayerBooleanFlags           = 162 // 0xA2
  , SourceId                    = 163 // 0xA3
  , LayerSourceQuoteRef         = 164 // 0xA4
  , LayerValueDate              = 165 // 0xA5
  , ContractType                = 166 // 0xA6
  , ContractExpiryDate          = 167 // 0xA7
  , ContractNameId              = 168 // 0xA8
  , NearValueDate               = 169 // 0xA9
  , FarValueDate                = 170 // 0xAA
  , NearForwardPoints           = 171 // 0xAB
  , FarForwardPoints            = 172 // 0xAC
  , UnderlyingTickerId          = 173 // 0xAD
  , BrokeragePriceDelta         = 174 // 0xAE
  , SlippagePriceDelta          = 175 // 0xAF
  , EffectivePriceDelta         = 176 // 0xB0
  , RemainingVolume             = 177 // 0xB1
  , PendingExecutionVolume      = 178 // 0xB2
  , InternalVolume              = 179 // 0xB3
  , OrdersCount                 = 180 // 0xB4
  , ShiftLayerOrders            = 181 // 0xB5
  , LayerOrders                 = 182 // 0xB6
  , LayerValidFromDate          = 183 // 0xB7
  , LayerValidFromSub2MinTime   = 184 // 0xB8
  , LayerValidToDate            = 185 // 0xB9
  , LayerValidToSub2MinTime     = 186 // 0xBA
  , LayerLastUpdatedDate        = 187 // 0xBB
  , LayerLastUpdatedSub2MinTime = 188 // 0xBC

  , AllLayersRangeEnd = 209 // 0xD1

    // Level 3 Quote Layer Fields
  , LastTradedDictionaryUpsertCommand = 210 // 0xD2
  , LastTradedTickTrades              = 211 // 0xD3
  , LastTradedRecentlyShift           = 212 // 0xD4
  , LastTradedRecently                = 213 // 0xD5
  , LastTradedInternalDailyAggregate  = 214 // 0xD6
  , LastTradedMarketDailyAggregate    = 215 // 0xD7

  , AdapterInternalOrdersTick   = 216 // 0xD8
  , AdapterInternalOrdersOpen   = 217 // 0xD9
  , AdapterInternalOrdersRecent = 218 // 0xDA

  , BatchId                 = 225 // 0xE1
  , QuoteSourceQuoteRef     = 226 // 0xE2
  , QuoteValueDate          = 227 // 0xE3
  , QuoteNearDate           = 228 // 0xE4
  , QuoteNearForwardType    = 229 // 0xE5
  , QuoteFarDate            = 230 // 0xE6
  , QuoteFarForwardType     = 231 // 0xE7
  , QuoteUnderlyingTickerId = 232 // 0xE8
  , QuoteContractType       = 233 // 0xE9
  , QuoteExpiryDate         = 234 // 0xEA

  , ParentContextRemapped = 255 // 0xFF
}

[TestClassNotRequired]
public static class PQQuoteFieldsExtensions
{
    public const byte   SingleByteFieldIdMaxBookDepth          = PQDepthKeyExtensions.SingleByteDepthMask + 1; // 63
    public const ushort TwoByteFieldIdMaxBookDepth             = (ushort)PQDepthKey.DepthMask + 1;             // 16,383
    public const byte   SingleByteFieldIdMaxPossibleLastTrades = SingleByteFieldIdMaxBookDepth;
}
