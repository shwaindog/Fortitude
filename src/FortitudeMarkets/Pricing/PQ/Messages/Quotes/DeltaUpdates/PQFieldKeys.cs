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
  , LayerFlags               = 14 // 0x0E
  , LastTradedFlags          = 15 // 0x0F

    // Source Ticker Info
  , SingleTickValue       = 16 // 0x10
  , SourceSentDateTime    = 17 // 0x11
  , SourceSentSubHourTime = 18 // 0x12
  , QuoteBooleanFlags     = 19 // 0x13
    // Saved when stored 
  , PQSyncStatus               = 20 // 0x14
  , SocketReceivingDateTime    = 21 // 0x15
  , SocketReceivingSubHourTime = 22 // 0x16
  , ProcessedDateTime          = 23 // 0x17
  , ProcessedSubHourTime       = 24 // 0x18
  , DispatchedDateTime         = 25 // 0x19
  , DispatchedSubHourTime      = 26 // 0x1A
  , ClientReceivedDateTime     = 27 // 0x1B
  , ClientReceivedSubHourTime  = 28 // 0x1C

    // Level 1 Quote Fields
  , AdapterSentDateTime        = 29 // 0x1D
  , AdapterSentSubHourTime     = 30 // 0x1E
  , AdapterReceivedDateTime    = 31 // 0x1F
  , AdapterReceivedSubHourTime = 32 // 0x20
  , SourceBidDateTime          = 33 // 0x21
  , SourceBidSubHourTime       = 34 // 0x22
  , SourceAskDateTime          = 35 // 0x23
  , SourceAskSubHourTime       = 36 // 0x24
  , QuoteValidFromDate         = 37 // 0x25
  , QuoteValidFromSubHourTime  = 38 // 0x26
  , QuoteValidToDate           = 39 // 0x27
  , QuoteValidToSubHourTime    = 40 // 0x28

    // period summary fields
  , SummaryPeriod          = 41 // 0x29
  , PeriodStartDateTime    = 42 // 0x2A
  , PeriodStartSubHourTime = 43 // 0x2B
  , PeriodStartPrice       = 44 // 0x2C
  , PeriodEndDateTime      = 45 // 0x2D
  , PeriodEndSubHourTime   = 46 // 0x2E
  , PeriodEndPrice         = 47 // 0x2F
  , PeriodHighestPrice     = 48 // 0x30
  , PeriodLowestPrice      = 49 // 0x31
  , PeriodTickCount        = 50 // 0x32
  , PeriodVolume           = 51 // 0x33
  , PeriodSummaryFlags     = 52 // 0x34
  , PeriodAveragePrice     = 53 // 0x35

    // Level 2 Quote Layer Fields
  , LayerNameDictionaryUpsertCommand = 100 // 0x64  StringFieldUpdate
  , ShiftBookByLayers                = 101 // 0x65

    // Level 2 Quote Layer Fields
  , Price                       = 102 // 0x66 
  , Volume                      = 103 // 0x67
  , LayerBooleanFlags           = 104 // 0x68
  , SourceId                    = 105 // 0x69
  , LayerSourceQuoteRef         = 106 // 0x6A
  , LayerValueDate              = 107 // 0x6B
  , ContractType                = 108 // 0x6C
  , ExpiryDate                  = 109 // 0x6D
  , NearValueDate               = 110 // 0x6E
  , FarValueDate                = 111 // 0x6F
  , NearForwardPoints           = 112 // 0x70
  , FarForwardPoints            = 113 // 0x71
  , UnderlyingTickerId          = 114 // 0x72
  , BrokeragePriceDelta         = 115 // 0x73
  , SlippagePriceDelta          = 116 // 0x74
  , EffectivePriceDelta         = 117 // 0x75
  , RemainingVolume             = 118 // 0x76
  , PendingExecutionVolume      = 119 // 0x77
  , InternalVolume              = 120 // 0x78
  , OrdersCount                 = 121 // 0x79
  , ShiftOrders                 = 122 // 0x7A
  , OrderId                     = 123 // 0x7B
  , OrderFlags                  = 124 // 0x7C
  , OrderCreatedDate            = 125 // 0x7D
  , OrderCreatedTimeSub2Min     = 126 // 0x7E
  , OrderUpdatedDate            = 127 // 0x7F
  , OrderUpdatedTimeSubHour     = 128 // 0x80
  , OrderVolume                 = 129 // 0x81
  , OrderRemainingVolume        = 130 // 0x82
  , OrderCounterPartyNameId     = 131 // 0x83
  , OrderTraderNameId           = 132 // 0x84
  , LayerValidFromDate          = 133 // 0x85
  , LayerValidFromSubHourTime   = 134 // 0x86
  , LayerValidToSubHourTime     = 135 // 0x87
  , LayerValidToDate            = 136 // 0x88
  , LayerLastUpdatedDate        = 137 // 0x89
  , LayerLastUpdatedSubHourTime = 138 // 0x8A
    // Book totals
  , DailyTradedVolume        = 139 // 0x8B
  , DailyVolumeWeightedPrice = 140 // 0x8C
    // Book Side and book
  , CumulativeInterestVolume = 141 // 0x8D
  , ExchangeOpenInterest     = 142 // 0x8E
  , AdapterOpenInterest      = 143 // 0x8F
  , CreatedDate              = 144 // 0x90
  , CreatedTimeSubHour       = 145 // 0x91
  , PublishedDate            = 146 // 0x92
  , PublishedTimeSubHour     = 147 // 0x93
  , AllLayersRangeEnd        = 199 // 0xC7

    // Level 3 Quote Layer Fields
  , ShiftLastTraded                   = 200 // 0xC8
  , LastTradedDictionaryUpsertCommand = 201 // 0xC9
  , LastTradedOrderId                 = 201 // 0xCA
  , LastTradedAtPrice                 = 202 // 0xCB
  , LastTradedTradeTimeDate           = 203 // 0xCC
  , LastTradedTradeTimeSubHour        = 204 // 0xCD
  , LastTradedOrderVolume             = 205 // 0xCE
  , LastTradedBooleanFlags            = 206 // 0xCF
  , LastTradedCounterPartyId          = 207 // 0xD0
  , LastTradedTraderId                = 208 // 0xD1
  , BatchId                           = 209 // 0xD2
  , QuoteSourceQuoteRef               = 210 // 0xD3
  , QuoteValueDate                    = 211 // 0xD4
  , QuoteNearDate                     = 212 // 0xD5
  , QuoteFarDate                      = 213 // 0xD6
  , QuoteContractType                 = 214 // 0xD7
  , QuoteExpiryDate                   = 215 // 0xD8
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
