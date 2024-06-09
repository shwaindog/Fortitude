// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

[TestClassNotRequired]
public static class PQFieldKeys
{
    public const byte SingleByteFieldIdMaxBookDepth          = 20;
    public const byte SingleByteFieldIdMaxPossibleLastTrades = 10;

    public const byte PQSequenceId = 1;

    // Source Ticker Info
    public const byte PublishQuoteLevelType  = 2;
    public const byte SourceTickerId         = 3;
    public const byte SourceTickerNames      = 4; //StringFieldUpdate
    public const byte RoundingPrecision      = 5;
    public const byte MinSubmitSize          = 6;
    public const byte MaxSubmitSize          = 7;
    public const byte IncrementSize          = 8;
    public const byte MinimumQuoteLife       = 9;
    public const byte LayerFlags             = 10; // 0x0A
    public const byte MaximumPublishedLayers = 11; // 0x0B
    public const byte LastTradedFlags        = 12; // 0x0C

    // Level 0 Fields
    public const byte SinglePrice                = 13; // 0x0D
    public const byte SourceSentDateTime         = 14; // 0x0E 
    public const byte SourceSentSubHourTime      = 15; // 0x0F
    public const byte QuoteBooleanFlags          = 16; // 0x10
    public const byte PQSyncStatus               = 17; // 0x11
    public const byte SocketReceivingDateTime    = 18; // 0x12
    public const byte SocketReceivingSubHourTime = 19; // 0x13
    public const byte ProcessedDateTime          = 20; // 0x14
    public const byte ProcessedSubHourTime       = 21; // 0x15
    public const byte DispatchedDateTime         = 22; // 0x16
    public const byte DispatchedSubHourTime      = 23; // 0x17
    public const byte ClientReceivedDateTime     = 24; // 0x18
    public const byte ClientReceivedSubHourTime  = 25; // 0x19

    // Level 1 Fields
    public const byte AdapterSentDateTime          = 26; // 0x1A
    public const byte AdapterSentSubHourTime       = 27; // 0x1B
    public const byte AdapterReceivedDateTime      = 28; // 0x1C
    public const byte AdapterReceivedSubHourTime   = 29; // 0x1D
    public const byte SourceBidDateTime            = 30; // 0x1E
    public const byte SourceBidSubHourTime         = 31; // 0x1F
    public const byte SourceAskDateTime            = 32; // 0x20
    public const byte SourceAskSubHourTime         = 33; // 0x21
    public const byte ReplayAdapterSentDateTime    = 34; // 0x22
    public const byte ReplayAdapterSentSubHourTime = 35; // 0x23

    public const byte BidAskTopOfBookPrice = 50; // 0x32 same as book LayerPriceOffset on purpose :-)

    // period summary
    public const byte PeriodStartDateTime    = 36; // 0x24
    public const byte PeriodStartSubHourTime = 37; // 0x25
    public const byte PeriodStartPrice       = 38; // 0x26
    public const byte PeriodEndDateTime      = 39; // 0x27
    public const byte PeriodEndSubHourTime   = 40; // 0x28
    public const byte PeriodEndPrice         = 41; // 0x29
    public const byte PeriodHighestPrice     = 42; // 0x2A
    public const byte PeriodLowestPrice      = 43; // 0x2B
    public const byte PeriodTickCount        = 44; // 0x2C
    public const byte PeriodVolumeLowerBytes = 45; // 0x2D
    public const byte PeriodVolumeUpperBytes = 46; // 0x2E

    // Level 2 Fields - first 20 layers
    public const byte LayerNameDictionaryUpsertCommand = 47; // 0x2F StringFieldUpdate
    public const byte ShiftBookByLayers                = 48; // 0x30
    public const byte SourceQuoteReference             = 49; // 0x31

    public const byte LayerPriceOffset          = 50;               // 0x32
    public const byte FirstLayersRangeStart     = LayerPriceOffset; // 0x32
    public const byte LayerVolumeOffset         = 70;               // 0x46
    public const byte LayerTraderIdOffset       = 90;               // 0x5A
    public const byte LayerTraderVolumeOffset   = 110;              // 0x6E
    public const byte LayerSourceIdOffset       = 130;              // 0x82
    public const byte LayerSourceQuoteRefOffset = 150;              // 0x96
    public const byte LayerBooleanFlagsOffset   = 170;              // 0xAA
    public const byte FirstLayersRangeEnd       = 190;              // 0xBD

    // Level 3 Fields
    // recently traded info first 10 layers
    public const byte LastTradedRangeStart              = 190; // 0xBE
    public const byte LastTradePriceOffset              = 190; // 0xBE
    public const byte LastTradeVolumeOffset             = 200; // 0xC8
    public const byte LastTradeTimeSubHourOffset        = 210; // 0xD2
    public const byte LastTradeTimeHourOffset           = 220; // 0xDC
    public const byte LastTraderIdOffset                = 230; // 0xEC
    public const byte LastTraderDictionaryUpsertCommand = 240; // 0xF0
    public const byte LastTradedRangeEnd                = 240; // 0xF0

    public const byte BatchId   = 241; // 0xF1
    public const byte ValueDate = 242; // 0xF2

    public const byte ReservedStart = 243; // 0xF3
    public const byte ReservedEnd   = 255; // 0xFF

    public const ushort ReservedExtendedStart = 256;

    public const ushort FirstLayerExtendedRangeStart     = 266;
    public const ushort FirstLayerEnumValueOffset        = FirstLayerExtendedRangeStart;
    public const ushort FirstLayerDateOffset             = 286;
    public const ushort SecondLayersRangeStart           = 306;
    public const ushort FirstLayerExtendedRangeEnd       = SecondLayersRangeStart;
    public const ushort FirstToSecondLayersOffset        = 256;
    public const ushort SecondLayerPriceOffset           = SecondLayersRangeStart;
    public const ushort SecondLayerVolumeOffset          = 326;
    public const ushort SecondLayerTraderIdOffset        = 346;
    public const ushort SecondLayerTraderVolumeOffset    = 366;
    public const ushort SecondLayerSourceIdOffset        = 386;
    public const ushort SecondLayerSourceQuoteRefOffset  = 406;
    public const ushort SecondLayerBooleanFlagsOffset    = 426;
    public const ushort SecondLastTradedRangeStart       = 446;
    public const ushort SecondLastTradePriceOffset       = 446;
    public const ushort SecondLastTradeVolumeOffset      = 456;
    public const ushort SecondLastTradeTimeHourOffset    = 466;
    public const ushort SecondLastTradeTimeSubHourOffset = 476;
    public const ushort SecondLastTraderIdOffset         = 486;
    public const ushort SecondLayersRangeEnd             = 495;

    public const ushort SecondLayerExtendedRangeStart = 522;
    public const ushort SecondEnumValueOffset         = SecondLayerExtendedRangeStart;
    public const ushort SecondLayerDateOffset         = 542;
    public const ushort ThirdLayersRangeStart         = 562;
    public const ushort SecondLayerExtendedRangeEnd   = ThirdLayersRangeStart;
    public const ushort FirstToThirdLayersOffset      = 512;
    // repeat above if required
    public const ushort AllLayersRangeEnd = 562;


    public const ushort ReservedExtendedEnd = 999;
}
