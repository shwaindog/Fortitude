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
  , PublishUpdateFlags         = 45 // 0x2D
  , StageSequenceIds           = 46 // 0x2E
  , ContinuousPriceAdjustments = 47 // 0x2F
    // price period summary fields
  , PriceCandleStick        = 50 // 0x32
  , CandleConflationSummary = 51 // 0x33
  , CandleRecentShift       = 52 // 0x34
  , CandleRecent            = 53 // 0x35
  , CandleDaily             = 54 // 0x36
  , CandlePreviousDay       = 55 // 0x37
  , CandleHistoricalShift   = 56 // 0x38
  , CandleHistorical        = 57 // 0x39

  , MovingAverageCurrentSlidingWindow   = 60 // 0x3C
  , MovingAverageDiscreetShortTermShift = 61 // 0x3D
  , MovingAverageDiscreetShortTerm      = 62 // 0x3E
  , MovingAverageRecentDailyShift       = 63 // 0x3F
  , MovingAverageRecentDaily            = 64 // 0x40
  , MovingAverageHistoricalShift        = 65 // 0x41
  , MovingAverageHistorical             = 66 // 0x42

  , VolatilityCurrentSlidingWindow   = 69 // 0x45
  , VolatilityDiscreetShortTermShift = 70 // 0x46
  , VolatilityDiscreetShortTerm      = 71 // 0x47
  , VolatilityRecentDailyShift       = 72 // 0x48
  , VolatilityRecentDaily            = 73 // 0x49
  , VolatilityHistoricalShift        = 74 // 0x4A
  , VolatilityHistorical             = 75 // 0x4B

  , QuoteOptionalInfoDictionaryUpsertCommand = 78 // 0x4E

  , MarketEventCurrent        = 80 // 0x50
  , MarketEventsRecentShift   = 81 // 0x51
  , MarketEventsRecent        = 82 // 0x52
  , MarketEventsUpcomingShift = 83 // 0x53
  , MarketEventsUpcoming      = 84 // 0x54

  , PivotShortTermShift  = 86 // 0x56
  , PivotShortTerm       = 87 // 0x57
  , PivotMediumTermShift = 88 // 0x58
  , PivotMediumTerm      = 89 // 0x59
  , PivotLongTermShift   = 90 // 0x5A
  , PivotLongTerm        = 91 // 0x5B

  , PriceBoundaryLineClosest                = 94  // 0x5E
  , PriceBoundaryLinesActiveShortTermShift  = 95  // 0x5F
  , PriceBoundaryLinesActiveShortTerm       = 96  // 0x60
  , PriceBoundaryLinesActiveMediumTermShift = 97  // 0x61
  , PriceBoundaryLinesActiveMediumTerm      = 98  // 0x62
  , PriceBoundaryLinesActiveLongTermShift   = 99  // 0x63
  , PriceBoundaryLinesActiveLongTerm        = 100 // 0x64
  , PriceBoundaryLinesRecentlyBreachedShift = 101 // 0x65
  , PriceBoundaryLinesRecentlyBreached      = 102 // 0x66

  , StrategyDecisionLastMade          = 105 // 0x69
  , StrategyDecisionRecentShift       = 106 // 0x6A
  , StrategyDecisionRecent            = 107 // 0x6B
  , StrategyDecisionActiveOrdersShift = 108 // 0x6C
  , StrategyDecisionActiveOrders      = 109 // 0x6D

    // Level 2 Quote Fields
  , OpenInterestTotal     = 113 // 0x71
  , OpenInterestSided     = 114 // 0x72
  , DailyAdapterTickCount = 115 // 0x73
  , DailyTradedTotal      = 116 // 0x74

    // Level 2 Quote Layer Fields
  , LayerNameDictionaryUpsertCommand = 118 // 0x76  StringFieldUpdate
  , ShiftBookByLayers                = 119 // 0x77

  , Price                       = 120 // 0x78 
  , Volume                      = 121 // 0x79 
  , LayerBooleanFlags           = 122 // 0x7A 
  , SourceId                    = 123 // 0x7B 
  , LayerSourceQuoteRef         = 124 // 0x7C 
  , LayerValueDate              = 125 // 0x7D 
  , ContractType                = 126 // 0x7E 
  , ExpiryDate                  = 127 // 0x7F 
  , NearValueDate               = 128 // 0x80 
  , FarValueDate                = 129 // 0x81 
  , NearForwardPoints           = 130 // 0x82
  , FarForwardPoints            = 131 // 0x83
  , UnderlyingTickerId          = 132 // 0x84
  , BrokeragePriceDelta         = 133 // 0x85
  , SlippagePriceDelta          = 134 // 0x86
  , EffectivePriceDelta         = 135 // 0x87
  , RemainingVolume             = 136 // 0x88
  , PendingExecutionVolume      = 137 // 0x89
  , InternalVolume              = 138 // 0x8A
  , OrdersCount                 = 139 // 0x8B
  , ShiftLayerOrders            = 140 // 0x8C
  , LayerOrders                 = 141 // 0x8D
  , LayerValidFromDate          = 142 // 0x9B
  , LayerValidFromSub2MinTime   = 143 // 0x9C
  , LayerValidToDate            = 144 // 0x9D
  , LayerValidToSub2MinTime     = 145 // 0x9E
  , LayerLastUpdatedDate        = 146 // 0x9F
  , LayerLastUpdatedSub2MinTime = 147 // 0xA0

    , AllLayersRangeEnd = 179 // 0xBE

  , ExecutionStatsCurrent     = 180 // 0xB4
  , ExecutionStatsRecentShift = 181 // 0xB5
  , ExecutionStatsRecent      = 182 // 0xB6
  , ExecutionStatsDaily       = 183 // 0xB7
  , ExecutionStatsLifetime    = 184 // 0xB8

    // Level 3 Quote Layer Fields
  , LastTradedDictionaryUpsertCommand = 200 // 0xC8
  , LastTradedTickTrades              = 201 // 0xC9
  , LastTradedRecentlyShift           = 202 // 0xCA
  , LastTradedRecently                = 203 // 0xCB

  , AdapterInternalOrdersTick        = 211 // 0xD3
  , AdapterInternalOrdersOpenShift   = 212 // 0xD4
  , AdapterInternalOrdersOpen        = 213 // 0xD5
  , AdapterInternalOrdersRecentShift = 214 // 0xD6
  , AdapterInternalOrdersRecent      = 215 // 0xD7

  , BatchId                 = 220 // 0xDC
  , QuoteSourceQuoteRef     = 221 // 0xDD
  , QuoteValueDate          = 222 // 0xDE
  , QuoteNearDate           = 223 // 0xDF
  , QuoteNearForwardType    = 224 // 0xE0
  , QuoteFarDate            = 225 // 0xE1
  , QuoteFarForwardType     = 226 // 0xE2
  , QuoteUnderlyingTickerId = 227 // 0xE3
  , QuoteContractType       = 228 // 0xE4
  , QuoteExpiryDate         = 229 // 0xE5

  , LimitBreachCurrent     = 234 // 0xEA
  , LimitBreachRecentShift = 235 // 0xEB
  , LimitBreachRecent      = 236 // 0xEC

  , SourceRemainingBuyMarginVolume  = 240 // 0xF0
  , SourceRemainingSellMarginVolume = 241 // 0xF1
  , SourceOpenConsumedBuyMargin     = 242 // 0xF2
  , SourceOpenConsumedSellMargin    = 243 // 0xF3
  , NetPositionExposure             = 244 // 0xF4
  , MaxAllowedExposure              = 245 // 0xF5
  , MaxAllowedRemainingExposure     = 246 // 0xF6
}

[TestClassNotRequired]
public static class PQQuoteFieldsExtensions
{
    public const byte   SingleByteFieldIdMaxBookDepth          = PQDepthKeyExtensions.SingleByteDepthMask + 1; // 63
    public const ushort TwoByteFieldIdMaxBookDepth             = (ushort)PQDepthKey.DepthMask + 1;             // 16,383
    public const byte   SingleByteFieldIdMaxPossibleLastTrades = 10;
}
