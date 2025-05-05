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
  , QuoteFlags               = 14 // 0x0E
  , LayerFlags               = 15 // 0x0F
  , LastTradedFlags          = 16 // 0x10
  , ExecutionUpdateFlags     = 17 // 0x11
  , SupplementaryInfoFlags   = 18 // 0x12

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

  , SourceSequenceId       = 50 // 0x32
  , AdapterSeqId           = 51 // 0x33
  , ClientSeqId            = 52 // 0x34
  , SourceState            = 53 // 0x35
  , SourceNextState        = 54 // 0x36
  , SourceNextStateTimeMin = 55 // 0x37

    // period summary fields
  , ConflatedPublishPriceSummary     = 60 // 0x3C
  , PriceSummaryCurrentPeriod        = 61 // 0x3D
  , PriceSummaryCurrentDay           = 62 // 0x3E
  , PriceSummaryPreviousTradingDay   = 63 // 0x3F
  , PriceSummaryShiftPreviousPeriods = 64 // 0x40
  , PriceSummaryPreviousPeriods      = 65 // 0x41

  
  , PriceSummaryPeriod          = 70 // 0x46
  , SummaryPeriod          = 71 // 0x47
  , PeriodStartDateTime    = 72 // 0x48
  , PeriodStartSub2MinTime = 73 // 0x49
  , PeriodStartPrice       = 74 // 0x4A
  , PeriodEndDateTime      = 75 // 0x4B
  , PeriodEndSub2MinTime   = 76 // 0x4C 
  , PeriodEndPrice         = 77 // 0x4D 
  , PeriodHighestPrice     = 78 // 0x4E 
  , PeriodLowestPrice      = 79 // 0x4F 
  , PeriodTickCount        = 80 // 0x50 
  , PeriodVolume           = 81 // 0x51
  , PeriodSummaryFlags     = 82 // 0x52
  , PeriodAveragePrice     = 83 // 0x53

  , SignalProcessingSeqId = 90 // 0x5A
  , StrategySeqId         = 91 // 0x5B
  , AlgoSeqId             = 92 // 0x5C
  , StoragePersistSeqId   = 93 // 0x5D

    // Level 2 Quote Layer Fields
  , LayerNameDictionaryUpsertCommand = 100 // 0x64  StringFieldUpdate
  , ShiftBookByLayers                = 101 // 0x65

  , SourceTotalOpenInterestVolume = 102 // 0x66
  , SourceTotalOpenInterestVwap   = 103 // 0x67
  , SourceSideOpenInterestVolume  = 104 // 0x68
  , SourceSideOpenInterestVwap    = 105 // 0x69
  , AdapterSideOpenInterestVolume = 106 // 0x6A
  , AdapterSideOpenInterestVwap   = 107 // 0x6B
  , DailyAdapterTickCount         = 108 // 0x6C
  , DailySourceTradedVolume       = 109 // 0x6D
  , DailyAdapterTradedVolume      = 110 // 0x6E
  , DailySourceTradedVwap         = 111 // 0x6F
  , DailyAdapterTradedVwap        = 112 // 0x70

    // Level 2 Quote Layer Fields
  , Price                        = 120 // 0x78 
  , Volume                       = 121 // 0x79 
  , LayerBooleanFlags            = 122 // 0x7A 
  , SourceId                     = 123 // 0x7B 
  , LayerSourceQuoteRef          = 124 // 0x7C 
  , LayerValueDate               = 125 // 0x7D 
  , ContractType                 = 126 // 0x7E 
  , ExpiryDate                   = 127 // 0x7F 
  , NearValueDate                = 128 // 0x80 
  , FarValueDate                 = 129 // 0x81 
  , NearForwardPoints            = 130 // 0x82
  , FarForwardPoints             = 131 // 0x83
  , UnderlyingTickerId           = 132 // 0x84
  , BrokeragePriceDelta          = 133 // 0x85
  , SlippagePriceDelta           = 134 // 0x86
  , EffectivePriceDelta          = 135 // 0x87
  , RemainingVolume              = 136 // 0x88
  , PendingExecutionVolume       = 137 // 0x89
  , InternalVolume               = 138 // 0x8A
  , OrdersCount                  = 139 // 0x8B
  , ShiftLayerOrders             = 140 // 0x8C
  , LayerOrders                  = 141 // 0x8D
  , LayerValidFromDate           = 156 // 0x9B
  , LayerValidFromSub2MinTime    = 157 // 0x9C
  , LayerValidToDate             = 158 // 0x9D
  , LayerValidToSub2MinTime      = 159 // 0x9E
  , LayerLastUpdatedDate         = 160 // 0x9F
  , LayerLastUpdatedSub2MinTime  = 161 // 0xA0

  , AllLayersRangeEnd      = 190 // 0xBE
  , ExecutionStatsCurrent  = 191 // 0xBF
  , ExecutionStatsPrevious = 192 // 0xC0
  , ExecutionStatsDaily    = 193 // 0xC0
  , ExecutionStatsSource   = 194 // 0xC0

    // Level 3 Quote Layer Fields
  , LastTradedDictionaryUpsertCommand = 200 // 0xC8
  , TickLastTradedTrades              = 201 // 0xC9
  , ShiftRecentLastTraded             = 202 
  , RecentLastTradedTrades         

  , BatchId             = 230 // 0xE6
  , QuoteSourceQuoteRef = 231 // 0xE7
  , QuoteValueDate      = 232 // 0xE8
  , QuoteNearDate       = 233 // 0xE9
  , QuoteFarDate        = 234 // 0xEA
  , QuoteContractType   = 235 // 0xEB
  , QuoteExpiryDate     = 236 // 0xEC

  , ExecutionRemainingBuyMarginVolume  = 240
  , ExecutionRemainingSellMarginVolume = 241
  , ExecutionOpenConsumedBuyMargin     = 242
  , ExecutionOpenConsumedSellMargin    = 243
}

[TestClassNotRequired]
public static class PQQuoteFieldsExtensions
{
    public const byte   SingleByteFieldIdMaxBookDepth          = PQDepthKeyExtensions.SingleByteDepthMask + 1; // 63
    public const ushort TwoByteFieldIdMaxBookDepth             = (ushort)PQDepthKey.DepthMask + 1;             // 16,383
    public const byte   SingleByteFieldIdMaxPossibleLastTrades = 10;

}
