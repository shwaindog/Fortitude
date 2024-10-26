// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

[TestClassNotRequired]
public static class PQFieldKeys
{
    public const byte SingleByteFieldIdMaxBookDepth          = 20;
    public const byte SingleByteFieldIdMaxPossibleLastTrades = 10;

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

    public const byte LayerPriceOffset          = 60;               // 0x3C
    public const byte FirstLayersRangeStart     = LayerPriceOffset; // 0x3C
    public const byte LayerVolumeOffset         = 80;               // 0x50
    public const byte LayerTraderIdOffset       = 100;              // 0x64
    public const byte LayerTraderVolumeOffset   = 120;              // 0x78
    public const byte LayerSourceIdOffset       = 140;              // 0x8C
    public const byte LayerSourceQuoteRefOffset = 160;              // 0xA0
    public const byte LayerBooleanFlagsOffset   = 180;              // 0xB4
    public const byte FirstLayersRangeEnd       = 200;              // 0xC8

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

    public const ushort FirstLayerExtendedRangeStart     = 266;
    public const ushort FirstLayerEnumValueOffset        = FirstLayerExtendedRangeStart;
    public const ushort FirstLayerDateOffset             = 286;
    public const ushort SecondLayersRangeStart           = 310;
    public const ushort FirstLayerExtendedRangeEnd       = SecondLayersRangeStart;
    public const ushort FirstToSecondLayersOffset        = SecondLayersRangeStart - FirstLayersRangeStart;
    public const ushort SecondLayerPriceOffset           = SecondLayersRangeStart;
    public const ushort SecondLayerVolumeOffset          = 330;
    public const ushort SecondLayerTraderIdOffset        = 350;
    public const ushort SecondLayerTraderVolumeOffset    = 370;
    public const ushort SecondLayerSourceIdOffset        = 390;
    public const ushort SecondLayerSourceQuoteRefOffset  = 410;
    public const ushort SecondLayerBooleanFlagsOffset    = 430;
    public const ushort SecondLastTradedRangeStart       = 450;
    public const ushort SecondLastTradePriceOffset       = 450;
    public const ushort SecondLastTradeVolumeOffset      = 460;
    public const ushort SecondLastTradeTimeHourOffset    = 470;
    public const ushort SecondLastTradeTimeSubHourOffset = 480;
    public const ushort SecondLastTraderIdOffset         = 490;
    public const ushort SecondLayersRangeEnd             = 500;

    public const ushort SecondLayerExtendedRangeStart = 516;
    public const ushort SecondEnumValueOffset         = SecondLayerExtendedRangeStart;
    public const ushort SecondLayerDateOffset         = 536;
    public const ushort ThirdLayersRangeStart         = 560;
    public const ushort SecondLayerExtendedRangeEnd   = ThirdLayersRangeStart;
    public const ushort FirstToThirdLayersOffset      = ThirdLayersRangeStart - FirstLayersRangeStart;
    // repeat above if required
    public const ushort AllLayersRangeEnd = 562;


    public const ushort ReservedExtendedEnd = 999;
}
