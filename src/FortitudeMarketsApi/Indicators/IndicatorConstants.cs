// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Indicators;

public static class IndicatorConstants
{
    public const ushort MidPriceQuotesId       = 1;
    public const ushort BidAskPriceQuotesId    = 2;
    public const ushort PriceLevel2QuotesId    = 3;
    public const ushort PriceLevel3QuotesId    = 4;
    public const ushort PricePeriodSummariesId = 5;
    public const ushort MidMovingAverageId     = 6;
    public const ushort BidAskMovingAverageId  = 7;

    public const string MidPriceQuotes       = nameof(MidPriceQuotes);
    public const string BidAskPriceQuotes    = nameof(BidAskPriceQuotes);
    public const string PriceLevel2Quotes    = nameof(PriceLevel2Quotes);
    public const string PriceLevel3Quotes    = nameof(PriceLevel3Quotes);
    public const string PricePeriodSummaries = nameof(PricePeriodSummaries);
    public const string MidMovingAverage     = nameof(MidMovingAverage);
    public const string BidAskMovingAverage  = nameof(BidAskMovingAverage);
}
