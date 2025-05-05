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

  , SummaryPeriod          = 70 // 0x46
  , PeriodStartDateTime    = 71 // 0x47
  , PeriodStartSub2MinTime = 72 // 0x48
  , PeriodStartPrice       = 73 // 0x49
  , PeriodEndDateTime      = 74 // 0x4A
  , PeriodEndSub2MinTime   = 75 // 0x4B
  , PeriodEndPrice         = 76 // 0x4C 
  , PeriodHighestPrice     = 77 // 0x4D 
  , PeriodLowestPrice      = 78 // 0x4E 
  , PeriodTickCount        = 79 // 0x4F 
  , PeriodVolume           = 80 // 0x50 
  , PeriodSummaryFlags     = 81 // 0x51
  , PeriodAveragePrice     = 82 // 0x52

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
  , ShiftOrders                  = 140 // 0x8C
  , OrderId                      = 141 // 0x8D
  , OrderFlags                   = 142 // 0x8E
  , OrderCreatedDate             = 143 // 0x8F
  , OrderCreatedSub2MinTime      = 144 // 0x90
  , OrderUpdatedDate             = 145 // 0x91
  , OrderUpdatedSub2MinTime      = 146 // 0x92
  , OrderVolume                  = 147 // 0x93
  , OrderRemainingVolume         = 148 // 0x93
  , OrderCounterPartyNameId      = 149 // 0x94
  , OrderTraderNameId            = 150 // 0x95
  , OrderDeskNameId              = 151 // 0x96
  , OrderDivisionNameId          = 152 // 0x97
  , OrderStrategyNameId          = 153 // 0x98
  , OrderInternalPortfolioNameId = 154 // 0x99
  , OrderSubmitterTrackingId     = 155 // 0x9A
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
  , ShiftLastTraded                   = 201 // 0xC9

  , LastTradedTradeId          = 202 // 0xCA
  , LastTradedOrderId          = 203 // 0xCB
  , LastTradedBatchId          = 204 // 0xCC
  , LastTradedActionCommand    = 205 // 0xCD
  , LastTradedAtPrice          = 206 // 0xCE
  , LastTradedTradeTimeDate    = 207 // 0xCF
  , LastTradedTradeSub2MinTime = 208 // 0xD0
  , LastTradedOrderVolume      = 209 // 0xD1
  , LastTradedBooleanFlags     = 210 // 0xD2

  , LastTradedInternalFillNameId      = 211 // 0xD3
  , LastTradedInternalOrderIdNameId   = 212 // 0xD4
  , LastTradedInternalPlacerNameId    = 213 // 0xD5
  , LastTradedInternalDeskNameId      = 214 // 0xD6
  , LastTradedInternalDivisionNameId  = 215 // 0xD7
  , LastTradedInternalTrackingNameId  = 216 // 0xD8
  , LastTradedInternalStrategyNameId  = 217 // 0xD9
  , LastTradedInternalPortfolioNameId = 218 // 0xDA
  , LastTradedCounterPartyId          = 219 // 0xDB
  , LastTradedTraderId                = 220 // 0xDC

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
public static class PQFieldKeys
{
    public const byte   SingleByteFieldIdMaxBookDepth          = PQDepthKeyExtensions.SingleByteDepthMask + 1; // 63
    public const ushort TwoByteFieldIdMaxBookDepth             = (ushort)PQDepthKey.DepthMask + 1;             // 16,383
    public const byte   SingleByteFieldIdMaxPossibleLastTrades = 10;

    // Source Ticker Info
    public const byte SourceTickerId           = 1;
    public const byte SourceTickerNames        = 2; //StringFieldUpdate
    public const byte MarketClassification     = 3;
    public const byte TickerDetailLevelType    = 4;
    public const byte PriceRoundingPrecision   = 5;
    public const byte Pip                      = 6;
    public const byte TickerDetailBooleanFlags = 7;
    public const byte MaximumPublishedLayers   = 8;
    public const byte MinSubmitSize            = 9;
    public const byte MaxSubmitSize            = 10; // 0x0A
    public const byte IncrementSize            = 11; // 0x0B
    public const byte DefaultMaxValidMs        = 12; // 0x0C 
    public const byte MinimumQuoteLifeMs       = 13; // 0x0D 
    public const byte LayerFlags               = 14; // 0x0E
    public const byte LastTradedFlags          = 15; // 0x0F

    // Ticker Instant Value Fields
    public const byte SingleTickValue            = 16; // 0x10
    public const byte SourceSentDateTime         = 17; // 0x11
    public const byte SourceSentSubHourTime      = 18; // 0x12
    public const byte QuoteBooleanFlags          = 19; // 0x13
    public const byte PQSyncStatus               = 20; // 0x14
    public const byte SocketReceivingDateTime    = 21; // 0x15
    public const byte SocketReceivingSubHourTime = 22; // 0x16
    public const byte ProcessedDateTime          = 23; // 0x17
    public const byte ProcessedSubHourTime       = 24; // 0x18
    public const byte DispatchedDateTime         = 25; // 0x19
    public const byte DispatchedSubHourTime      = 26; // 0x1A
    public const byte ClientReceivedDateTime     = 27; // 0x1B
    public const byte ClientReceivedSubHourTime  = 28; // 0x1C

    // Level 1 Quote Fields
    public const byte AdapterSentDateTime        = 29; // 0x1D
    public const byte AdapterSentSubHourTime     = 30; // 0x1E
    public const byte AdapterReceivedDateTime    = 31; // 0x1F
    public const byte AdapterReceivedSubHourTime = 32; // 0x20
    public const byte SourceBidDateTime          = 33; // 0x21
    public const byte SourceBidSubHourTime       = 34; // 0x22
    public const byte SourceAskDateTime          = 35; // 0x23
    public const byte SourceAskSubHourTime       = 36; // 0x24
    public const byte ValidFromDateTime          = 37; // 0x25
    public const byte ValidFromSubHourTime       = 38; // 0x26
    public const byte ValidToDateTime            = 39; // 0x27
    public const byte ValidToSubHourTime         = 40; // 0x28

    public const byte BidAskTopOfBookPrice = 60; // 0x3C same as book LayerPriceOffset on purpose :-)

    // period summary fields
    public const byte SummaryPeriod          = 41; // 0x29
    public const byte PeriodStartDateTime    = 42; // 0x2A
    public const byte PeriodStartSubHourTime = 43; // 0x2B
    public const byte PeriodStartPrice       = 44; // 0x2C
    public const byte PeriodEndDateTime      = 45; // 0x2D
    public const byte PeriodEndSubHourTime   = 46; // 0x2E 
    public const byte PeriodEndPrice         = 47; // 0x2F
    public const byte PeriodHighestPrice     = 48; // 0x30
    public const byte PeriodLowestPrice      = 49; // 0x31
    public const byte PeriodTickCount        = 50; // 0x32
    public const byte PeriodVolume           = 51; // 0x33 
    public const byte PeriodSummaryFlags     = 52; // 0x34 
    public const byte PeriodAveragePrice     = 53; // 0x35

    // Level 2 Quote Fields - first 20 layers
    public const byte LayerNameDictionaryUpsertCommand = 54; // 0x36  StringFieldUpdate
    public const byte ShiftBookByLayers                = 55; // 0x37
    public const byte SourceQuoteReference             = 56; // 0x38

    public const byte FirstLayerSetRangeStart = 60;  // 0x3C
    public const byte FirstLayersRangeEnd     = 160; // 0xC8

    // Level 3 Quote Fields
    // recently traded info first 10 layers
    public const byte LastTradedRangeStart              = 200; // 0xC8
    public const byte LastTradePriceOffset              = 200; // 0xC8
    public const byte LastTradeVolumeOffset             = 210; // 0xD2
    public const byte LastTradeTimeSubHourOffset        = 220; // 0xDC
    public const byte LastTradeTimeHourOffset           = 230; // 0xE6
    public const byte LastTraderIdOffset                = 240; // 0xF0
    public const byte LastTraderDictionaryUpsertCommand = 250; // 0xFA
    public const byte LastTradedRangeEnd                = 250; // 0xFA

    public const byte BatchId   = 251; // 0xFB
    public const byte ValueDate = 252; // 0xFC

    public const byte ReservedStart = 253; // 0xFD
    public const byte ReservedEnd   = 255; // 0xFF

    public const ushort ReservedExtendedStart = 256;

    public const ushort FirstLayerSetExtendedRangeStart         = 260;
    public const ushort FirstLayerSetEnumValueOffset            = FirstLayerSetExtendedRangeStart;
    public const ushort FirstLayerSetDateOffset                 = 280;
    public const ushort FirstLayerSetOrderIdOffset              = 300;
    public const ushort FirstLayerSetOrderFlagsOffset           = 300;
    public const ushort FirstLayerSetOrderCreatedTimeOffset     = 320;
    public const ushort FirstLayerSetOrderUpdatedTimeOffset     = 340;
    public const ushort FirstLayerSetOrderVolumeOffset          = 386;
    public const ushort FirstLayerSetOrderRemainingVolumeOffset = 406;
    public const ushort SecondLayersRangeStart                  = 310;
    public const ushort FirstLayerExtendedRangeEnd              = SecondLayersRangeStart;
    public const ushort FirstToSecondLayersOffset               = SecondLayersRangeStart - FirstLayerSetRangeStart;
    public const ushort SecondLayerPriceOffset                  = SecondLayersRangeStart;
    public const ushort SecondLayerVolumeOffset                 = 330;
    public const ushort SecondLayerTraderIdOffset               = 350;
    public const ushort SecondLayerTraderVolumeOffset           = 370;
    public const ushort SecondLayerSourceIdOffset               = 390;
    public const ushort SecondLayerSourceQuoteRefOffset         = 410;
    public const ushort SecondLayerBooleanFlagsOffset           = 430;
    public const ushort SecondLastTradedRangeStart              = 450;
    public const ushort SecondLastTradePriceOffset              = 450;
    public const ushort SecondLastTradeVolumeOffset             = 460;
    public const ushort SecondLastTradeTimeHourOffset           = 470;
    public const ushort SecondLastTradeTimeSubHourOffset        = 480;
    public const ushort SecondLastTraderIdOffset                = 490;
    public const ushort SecondLayersRangeEnd                    = 500;

    public const ushort SecondLayerExtendedRangeStart = 516;
    public const ushort SecondEnumValueOffset         = SecondLayerExtendedRangeStart;
    public const ushort SecondLayerDateOffset         = 536;
    public const ushort ThirdLayersRangeStart         = 560;
    public const ushort SecondLayerExtendedRangeEnd   = ThirdLayersRangeStart;
    public const ushort FirstToThirdLayersOffset      = ThirdLayersRangeStart - FirstLayerSetRangeStart;
    // repeat above if required
    public const ushort AllLayersRangeEnd = 562;


    public const ushort ReservedExtendedEnd = 999;
}
