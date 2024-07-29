// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Indicators.Pricing.Parameters;

public struct TickGapOptions
{
    public TickGapOptions()
    {
        DataGapTimeSpan         = TimeSpan.FromMinutes(10); // assume market gap if gap is greater than
        MarketDayCloseTimeSpan  = TimeSpan.FromHours(2);    // assume market close if gap is greater than
        MarketWeekCloseTimeSpan = TimeSpan.FromHours(36);
    }

    public TickGapOptions(TimeSpan dataGapsTimeSpan, TimeSpan marketDayCloseGapsTimeSpan, TimeSpan marketWeekCloseGapsTimeSpan)
    {
        DataGapTimeSpan         = dataGapsTimeSpan;
        MarketDayCloseTimeSpan  = marketDayCloseGapsTimeSpan;
        MarketWeekCloseTimeSpan = marketWeekCloseGapsTimeSpan;
    }

    public TimeSpan DataGapTimeSpan         { get; }
    public TimeSpan MarketDayCloseTimeSpan  { get; }
    public TimeSpan MarketWeekCloseTimeSpan { get; }
}
