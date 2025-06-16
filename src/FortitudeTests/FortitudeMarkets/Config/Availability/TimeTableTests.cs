// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;
using FortitudeMarkets.Config;
using FortitudeMarkets.Config.Availability;
using Microsoft.Extensions.Configuration;
using static FortitudeMarkets.Config.Availability.TradingPeriodTypeFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Config.Availability;

[TestClass]
public class TimeTableConfigTests
{
    private readonly DateTimeOffset weekBeforeUsDstInChiOffset =
        new(TimeZoneInfo.ConvertTime(MidWeekBeforeUsDst, UsChi),
            UsChi.GetUtcOffset(MidWeekBeforeUsDst)); // Monday before US daylightSavings

    private readonly DateTimeOffset weekAfterUsDstInChiOffset =
        new(TimeZoneInfo.ConvertTime(MidWeekAfterUsDst, UsChi),
            UsChi.GetUtcOffset(MidWeekAfterUsDst)); // Monday after US daylightSavings

    private readonly DateTimeOffset weekAfterAuDstInChiOffset =
        new(TimeZoneInfo.ConvertTime(MidWeekAfterAuDst, UsChi),
            UsChi.GetUtcOffset(MidWeekAfterAuDst)); // Tuesday after Au daylightSavings

    private const TradingPeriodTypeFlags ExpectedDayOpenFlags = 
        IsOpen | IsPricing | IsNonPreferredTradingPeriod | IsHighActivityPeriod;

    private const TradingPeriodTypeFlags ExpectedDayOpenTradingStartFlags = 
        ExpectedDayOpenFlags | IsTrading | IsAcceptingOrders;

    private const TradingPeriodTypeFlags InPreferredTradingZone = 
        IsOpen | IsPricing | IsHighActivityPeriod | IsTrading | IsAcceptingOrders | IsPreferredTradingPeriod;

    private static readonly TimeZoneInfo AuMel = TimeZoneInfo.FindSystemTimeZoneById("Australia/Melbourne");
    private static readonly TimeZoneInfo UsChi = TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");

    private static readonly DateTime MidWeekBeforeUsDst = new(2025, 3, 4);
    private static readonly DateTime MidWeekAfterUsDst  = new(2025, 3, 11);
    private static readonly DateTime MidWeekAfterAuDst  = new(2025, 4, 8);

    [TestMethod]
    public void OpenSampleFile_SimpleConfigWithAvailability_WeeklyScheduleBeforeUsDstExpectedTradingTransitions()
    {
        var config = new ConfigurationBuilder()
                     .AddJsonFile("FortitudeMarkets/Config/SampleMarketConfig.json")
                     .Build();
        var marketsConfig = new MarketsConfig(config, "MarketsConfig");

        Assert.IsNotNull(marketsConfig);
        var marketConnConfig = marketsConfig.Markets["SimpleConfigWithAvailability"];
        Assert.IsNotNull(marketConnConfig);
        Assert.AreEqual(3, marketConnConfig.SourceId);
        Assert.AreEqual("SimpleConfigWithAvailability", marketConnConfig.SourceName);
        Assert.AreEqual(MarketConnectionType.Pricing | MarketConnectionType.Trading, marketConnConfig.MarketConnectionType);
        var sourceTickerConfig = marketConnConfig.SourceTickerConfig;
        Assert.IsNotNull(sourceTickerConfig);

        var cornSchedule = sourceTickerConfig.TickerWeeklyTradingSchedule(weekBeforeUsDstInChiOffset, "CORN");
        Assert.IsNotNull(cornSchedule);
        AssertWeekBeforeUsDstHasExpectedValues(weekBeforeUsDstInChiOffset, cornSchedule);
    }

    [TestMethod]
    public void OpenSampleFile_SimpleConfigWithAvailability_WeeklyScheduleAfterUsDstExpectedTradingTransitions()
    {
        var config = new ConfigurationBuilder()
                     .AddJsonFile("FortitudeMarkets/Config/SampleMarketConfig.json")
                     .Build();
        var marketsConfig = new MarketsConfig(config, "MarketsConfig");

        Assert.IsNotNull(marketsConfig);
        var marketConnConfig = marketsConfig.Markets["SimpleConfigWithAvailability"];
        Assert.IsNotNull(marketConnConfig);
        Assert.AreEqual(3, marketConnConfig.SourceId);
        Assert.AreEqual("SimpleConfigWithAvailability", marketConnConfig.SourceName);
        Assert.AreEqual(MarketConnectionType.Pricing | MarketConnectionType.Trading, marketConnConfig.MarketConnectionType);
        var sourceTickerConfig = marketConnConfig.SourceTickerConfig;
        Assert.IsNotNull(sourceTickerConfig);

        var cornSchedule = sourceTickerConfig.TickerWeeklyTradingSchedule(weekAfterUsDstInChiOffset, "CORN");
        Assert.IsNotNull(cornSchedule);
        AssertWeekBeforeUsDstHasExpectedValues(weekAfterUsDstInChiOffset, cornSchedule);
    }

    [TestMethod]
    public void OpenSampleFile_SimpleConfigWithAvailability_WeeklyScheduleAfterAuDstExpectedTradingTransitions()
    {
        var config = new ConfigurationBuilder()
                     .AddJsonFile("FortitudeMarkets/Config/SampleMarketConfig.json")
                     .Build();
        var marketsConfig = new MarketsConfig(config, "MarketsConfig");

        Assert.IsNotNull(marketsConfig);
        var marketConnConfig = marketsConfig.Markets["SimpleConfigWithAvailability"];
        Assert.IsNotNull(marketConnConfig);
        Assert.AreEqual(3, marketConnConfig.SourceId);
        Assert.AreEqual("SimpleConfigWithAvailability", marketConnConfig.SourceName);
        Assert.AreEqual(MarketConnectionType.Pricing | MarketConnectionType.Trading, marketConnConfig.MarketConnectionType);
        var sourceTickerConfig = marketConnConfig.SourceTickerConfig;
        Assert.IsNotNull(sourceTickerConfig);

        var cornSchedule = sourceTickerConfig.TickerWeeklyTradingSchedule(weekAfterAuDstInChiOffset, "CORN");
        Assert.IsNotNull(cornSchedule);
        AssertWeekBeforeUsDstHasExpectedValues(weekAfterAuDstInChiOffset, cornSchedule);
    }

    public void AssertWeekBeforeUsDstHasExpectedValues(DateTimeOffset timeInWeek, WeeklyTradingSchedule weeklyTradingSchedule)
    {
        var startOfWeekUsChi = timeInWeek.TruncToWeekBoundary();
        Console.Out.WriteLine($"weeklyTradingSchedule: {weeklyTradingSchedule}");

        var incrementNonMainLiquidityDays = 5;
        var incrementMainLiquidityDays = UsChi.IsDaylightSavingTime(timeInWeek) ? 8 : 7;

        var transAtTime = weeklyTradingSchedule[0];
        Assert.AreEqual(startOfWeekUsChi, transAtTime.AtTime);
        Assert.AreEqual(IsMarketClosed | IsWeekend, transAtTime.MarketState);
        var readAtIndex = 1;
        AssertDayWithoutMainLiquidityScheduleExpected(weeklyTradingSchedule, startOfWeekUsChi, readAtIndex);
        readAtIndex += incrementNonMainLiquidityDays;
        AssertDayWithMainLiquidityScheduleExpected(weeklyTradingSchedule, startOfWeekUsChi.AddDays(1), readAtIndex);
        readAtIndex += incrementMainLiquidityDays;
        AssertDayWithMainLiquidityScheduleExpected(weeklyTradingSchedule, startOfWeekUsChi.AddDays(2), readAtIndex);
        readAtIndex += incrementMainLiquidityDays;
        AssertDayWithMainLiquidityScheduleExpected(weeklyTradingSchedule, startOfWeekUsChi.AddDays(3), readAtIndex);
        readAtIndex += incrementMainLiquidityDays + 1;
        AssertDayWithoutMainLiquidityScheduleExpected(weeklyTradingSchedule, startOfWeekUsChi.AddDays(4), readAtIndex);
        readAtIndex += incrementNonMainLiquidityDays;
        var fridayClose = startOfWeekUsChi.AddDays(5) + TimeSpan.FromHours(17);
        transAtTime = weeklyTradingSchedule[readAtIndex];
        Assert.AreEqual(fridayClose, transAtTime.AtTime);
        Assert.AreEqual(IsMarketClosed | IsWeekend, transAtTime.MarketState);
    }

    private static void AssertDayWithoutMainLiquidityScheduleExpected(WeeklyTradingSchedule weeklyTradingSchedule, DateTimeOffset startOfPreviousDayWeekUsChi, int startIndex)
    {
        var i           = startIndex;
        var transAtTime = weeklyTradingSchedule[i++];
        var mondayUsChi = startOfPreviousDayWeekUsChi.AddDays(1);
        var expectedTime = mondayUsChi + TimeSpan.FromHours(9);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(ExpectedDayOpenFlags, transAtTime.MarketState);

        transAtTime  = weeklyTradingSchedule[i++];
        expectedTime = expectedTime.AddMinutes(2);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(ExpectedDayOpenTradingStartFlags, transAtTime.MarketState);
        
        var tuesday      = mondayUsChi.AddDays(1);
        var tuesdayAuMel = new DateTimeOffset(tuesday.DateTime, AuMel.GetUtcOffset(tuesday));
        transAtTime  = weeklyTradingSchedule[i++];
        expectedTime = tuesdayAuMel.AddHours(3);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(InPreferredTradingZone, transAtTime.MarketState);

        transAtTime  = weeklyTradingSchedule[i++];
        expectedTime = tuesdayAuMel.AddHours(5);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(ExpectedDayOpenTradingStartFlags, transAtTime.MarketState);

        transAtTime  = weeklyTradingSchedule[i];
        expectedTime = mondayUsChi + TimeSpan.FromHours(16) + TimeSpan.FromMinutes(58);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(ExpectedDayOpenTradingStartFlags | IsClosingSoon, transAtTime.MarketState);
    }

    private static void AssertDayWithMainLiquidityScheduleExpected(WeeklyTradingSchedule weeklyTradingSchedule, DateTimeOffset startOfPreviousDayWeekUsChi, int startIndex)
    {
        var closePreviousDay = startOfPreviousDayWeekUsChi.AddHours(17);
        var i                = startIndex;
        var transAtTime      = weeklyTradingSchedule[i++];
        Assert.AreEqual(closePreviousDay, transAtTime.AtTime);
        Assert.AreEqual(IsMarketClosed | IsOutOfHours, transAtTime.MarketState);

        var dayToCheckUsChi = startOfPreviousDayWeekUsChi.AddDays(1);
        transAtTime  = weeklyTradingSchedule[i++];
        var expectedTime = dayToCheckUsChi + TimeSpan.FromHours(9);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(ExpectedDayOpenFlags | IsGreyMarketTradingPeriod, transAtTime.MarketState);

        transAtTime  = weeklyTradingSchedule[i++];
        expectedTime = expectedTime.AddMinutes(2);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(ExpectedDayOpenTradingStartFlags | IsGreyMarketTradingPeriod, transAtTime.MarketState);
        if (UsChi.IsDaylightSavingTime(dayToCheckUsChi))
        {
            transAtTime  = weeklyTradingSchedule[i++];
            expectedTime = dayToCheckUsChi.AddHours(10);
            Assert.AreEqual(expectedTime, transAtTime.AtTime);
            Assert.AreEqual(ExpectedDayOpenTradingStartFlags | IsMainTradingPeriod, transAtTime.MarketState);
        }

        var nextDay      = dayToCheckUsChi.AddDays(1);
        var nextDayAuMel = new DateTimeOffset(nextDay.DateTime, AuMel.GetUtcOffset(nextDay));
        transAtTime  = weeklyTradingSchedule[i++];
        expectedTime = nextDayAuMel.AddHours(3);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(InPreferredTradingZone | IsMainTradingPeriod, transAtTime.MarketState);

        transAtTime  = weeklyTradingSchedule[i++];
        expectedTime = nextDayAuMel.AddHours(5);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(ExpectedDayOpenTradingStartFlags | IsMainTradingPeriod, transAtTime.MarketState);

        transAtTime  = weeklyTradingSchedule[i++];
        expectedTime = dayToCheckUsChi + TimeSpan.FromHours(16);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(ExpectedDayOpenTradingStartFlags | IsGreyMarketTradingPeriod, transAtTime.MarketState);

        transAtTime  = weeklyTradingSchedule[i++];
        expectedTime = dayToCheckUsChi + TimeSpan.FromHours(16) + TimeSpan.FromMinutes(58);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(ExpectedDayOpenTradingStartFlags | IsClosingSoon| IsGreyMarketTradingPeriod, transAtTime.MarketState);

        transAtTime  = weeklyTradingSchedule[i];
        expectedTime = dayToCheckUsChi + TimeSpan.FromHours(17);
        Assert.AreEqual(expectedTime, transAtTime.AtTime);
        Assert.AreEqual(IsMarketClosed | IsOutOfHours, transAtTime.MarketState);
    }
}
