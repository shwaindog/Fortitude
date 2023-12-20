#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

[TestClassNotRequired]
public static class PQFieldKeys
{
    public const byte SingleByteFieldIdMaxBookDepth = 20;
    public const byte SingleByteFieldIdMaxPossibleLastTrades = 10;

    // Level 0 Fields
    public const byte PQSequenceId = 1;
    public const byte SinglePrice = 2;
    public const byte SourceSentDateTime = 3;
    public const byte SourceSentSubHourTime = 4;
    public const byte QuoteBooleanFlags = 5;
    public const byte PQSyncStatus = 6;
    public const byte SocketReceivingDateTime = 7;
    public const byte SocketReceivingSubHourTime = 8;
    public const byte ProcessedDateTime = 9;
    public const byte ProcessedSubHourTime = 10;
    public const byte DispatchedDateTime = 11;
    public const byte DispatchedSubHourTime = 12;
    public const byte ClientReceivedDateTime = 13;

    public const byte ClientReceivedSubHourTime = 14;

    // Source Ticker Info
    public const byte SourceTickerId = 15;
    public const byte SourceTickerNames = 16; //StringFieldUpdate
    public const byte RoundingPrecision = 17;
    public const byte MinSubmitSize = 18;
    public const byte MaxSubmitSize = 19;
    public const byte IncrementSize = 20;
    public const byte MinimumQuoteLife = 21;
    public const byte LayerFlags = 22;
    public const byte MaximumPublishedLayers = 23;
    public const byte LastTradedFlags = 24;
    public const byte SourceTickerBooleanFields = 25;

    // Level 1 Fields
    public const byte AdapterSentDateTime = 26;
    public const byte AdapterSentSubHourTime = 27;
    public const byte AdapterReceivedDateTime = 28;
    public const byte AdapterReceivedSubHourTime = 29;
    public const byte SourceBidDateTime = 30;
    public const byte SourceBidSubHourTime = 31;
    public const byte SourceAskDateTime = 32;
    public const byte SourceAskSubHourTime = 33;
    public const byte ReplayAdapterSentDateTime = 34;
    public const byte ReplayAdapterSentSubHourTime = 35;

    public const byte BidAskTopOfBookPrice = 50; // same as book LayerPriceOffset on purpose :-)

    // period summary
    public const byte PeriodStartDateTime = 36;
    public const byte PeriodStartSubHourTime = 37;
    public const byte PeriodStartPrice = 38;
    public const byte PeriodEndDateTime = 39;
    public const byte PeriodEndSubHourTime = 40;
    public const byte PeriodEndPrice = 41;
    public const byte PeriodHighestPrice = 42;
    public const byte PeriodLowestPrice = 43;
    public const byte PeriodTickCount = 44;
    public const byte PeriodVolumeLowerBytes = 45;
    public const byte PeriodVolumeUpperBytes = 46;

    // Level 2 Fields - first 20 layers
    public const byte LayerNameDictionaryUpsertCommand = 47; //StringFieldUpdate
    public const byte ShiftBookByLayers = 48;
    public const byte SourceQuoteReference = 49;

    public const byte FirstLayersRangeStart = 50;
    public const byte LayerPriceOffset = 50;
    public const byte LayerVolumeOffset = 70;
    public const byte LayerTraderIdOffset = 90;
    public const byte LayerTraderVolumeOffset = 110;
    public const byte LayerSourceIdOffset = 130;
    public const byte LayerSourceQuoteRefOffset = 150;
    public const byte LayerBooleanFlagsOffset = 170;
    public const byte FirstLayersRangeEnd = 189;

    // Level 3 Fields
    // recently traded info first 10 layers
    public const byte LastTradedRangeStart = 190;
    public const byte LastTradePriceOffset = 190;
    public const byte LastTradeVolumeOffset = 200;
    public const byte LastTradeTimeSubHourOffset = 210;
    public const byte LastTradeTimeHourOffset = 220;
    public const byte LastTraderIdOffset = 230;
    public const byte LastTraderDictionaryUpsertCommand = 240;
    public const byte LastTradedRangeEnd = 240;

    public const byte BatchId = 241;
    public const byte ValueDate = 242;

    public const byte ReservedStart = 243;
    public const byte ReservedEnd = 255;

    public const ushort ReservedExtendedStart = 256;

    public const ushort SecondLayersRangeStart = 270;
    public const ushort LayerDateOffset = 270;
    public const ushort SecondLayersRangeEnd = 469;

    public const ushort ReservedExtendedEnd = 999;
}
