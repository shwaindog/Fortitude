// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsApi.Indicators;

public static class IndicatorConstants
{
    // Indicator Ids
    // Prices as indicator
    public const ushort MidPriceQuotesId    = 1;
    public const ushort BidAskPriceQuotesId = 2;
    public const ushort PriceLevel2QuotesId = 3;
    public const ushort PriceLevel3QuotesId = 4;

    // price period summaries as indicator
    public const ushort PricePeriodSummariesId = 5;

    // Moving Average
    public const ushort MovingAverageTimeWeightedMidId    = 6;
    public const ushort MovingAverageTimeWeightedBidAskId = 7;

    // Indicator Names
    public const string PriceMidQuotes    = nameof(PriceMidQuotes);
    public const string PriceBidAskQuotes = nameof(PriceBidAskQuotes);
    public const string PriceLevel2Quotes = nameof(PriceLevel2Quotes);
    public const string PriceLevel3Quotes = nameof(PriceLevel3Quotes);

    public const string PricePeriodSummaries = nameof(PricePeriodSummaries);

    public const string MovingAverageTimeWeightedMid    = nameof(MovingAverageTimeWeightedMid);
    public const string MovingAverageTimeWeightedBidAsk = nameof(MovingAverageTimeWeightedBidAsk);

    // Indicator Descriptions
    public const string PriceMidQuotesDescription = "Raw source single value ticks.  Can be mid price or any other single value the source will send";
    public const string PriceBidAskQuotesDescription
        = "Raw source top of book bid and ask ticks.  Can be a price, forward or any other value that has a different value depending on direction";
    public const string PriceLevel2QuotesDescription
        = "Raw source multi-layer book bid and ask layers.  All layers can contain price and volume and optional additional values";
    public const string PriceLevel3QuotesDescription
        = "Raw source multi-layer book bid and ask layers with last trade information.  Last trade can contain paid and given, trader name and side";

    public const string PricePeriodSummariesDescription
        = "Summaries are candles that contain start, highest, lowest and end bid ask prices as well as the average bid ask for the period";

    public const string MovingAverageTimeWeightedMidDescription    = "Contains a single mid point time weighted moving average.";
    public const string MovingAverageTimeWeightedBidAskDescription = "Contains a bid and ask point time weighted moving average.";


    public static IndicatorType GetIndicatorType(ushort indicatorId) =>
        indicatorId switch
        {
            >= MidPriceQuotesId and <= PriceLevel3QuotesId => IndicatorType.MarketPrice
          , PricePeriodSummariesId => IndicatorType.MarketPriceSummary
          , >= MovingAverageTimeWeightedMidId and <= MovingAverageTimeWeightedBidAskId => IndicatorType.Averaging
          , _ => throw new ArgumentException("Indicator Id has no known Indicator Type")
        };

    public static IIndicator CreateIndicatorFor
        (this SourceTickerIdValue sourceTickerId, ushort indicatorId, DiscreetTimePeriod? specificCoveringPeriod = null)
    {
        var coveringPeriod = specificCoveringPeriod ?? new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        switch (indicatorId)
        {
            case MidPriceQuotesId:
                return new Indicator(MidPriceQuotesId, PriceMidQuotes, sourceTickerId.Source, IndicatorType.MarketPrice
                                   , coveringPeriod, PriceMidQuotesDescription);
            case BidAskPriceQuotesId:
                return new Indicator(BidAskPriceQuotesId, PriceBidAskQuotes, sourceTickerId.Source, IndicatorType.MarketPrice
                                   , coveringPeriod, PriceBidAskQuotesDescription);
            case PriceLevel2QuotesId:
                return new Indicator(PriceLevel2QuotesId, PriceLevel2Quotes, sourceTickerId.Source, IndicatorType.MarketPrice
                                   , coveringPeriod, PriceLevel2QuotesDescription);
            case PriceLevel3QuotesId:
                return new Indicator(PriceLevel3QuotesId, PriceLevel3Quotes, sourceTickerId.Source, IndicatorType.MarketPrice
                                   , coveringPeriod, PriceLevel3QuotesDescription);
            case PricePeriodSummariesId:
                return new Indicator(PricePeriodSummariesId, PricePeriodSummaries, sourceTickerId.Source, IndicatorType.MarketPrice
                                   , coveringPeriod, PricePeriodSummariesDescription);
            case MovingAverageTimeWeightedMidId:
                return new Indicator(MovingAverageTimeWeightedMidId, MovingAverageTimeWeightedMid, sourceTickerId.Source, IndicatorType.MarketPrice
                                   , coveringPeriod, MovingAverageTimeWeightedMidDescription);
            case MovingAverageTimeWeightedBidAskId:
                return new Indicator(MovingAverageTimeWeightedBidAskId, MovingAverageTimeWeightedBidAsk, sourceTickerId.Source
                                   , IndicatorType.MarketPrice
                                   , coveringPeriod, MovingAverageTimeWeightedBidAskDescription);
        }
        throw new ArgumentException($"Could not find indicator with indicatorId {indicatorId}");
    }
}
