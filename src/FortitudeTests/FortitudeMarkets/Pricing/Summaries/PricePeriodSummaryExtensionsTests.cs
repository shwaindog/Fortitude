// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Summaries;

[TestClass]
public class PricePeriodSummaryExtensionsTests
{
    [TestMethod]
    public void UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly()
    {
        var t = new DateTime(2018, 2, 7, 22, 35, 10);

        var calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddSeconds(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneSecond, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddMinutes(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneMinute, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddMinutes(5));
        Assert.AreEqual(TimeBoundaryPeriod.FiveMinutes, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddMinutes(10));
        Assert.AreEqual(TimeBoundaryPeriod.TenMinutes, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddMinutes(15));
        Assert.AreEqual(TimeBoundaryPeriod.FifteenMinutes, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddMinutes(30));
        Assert.AreEqual(TimeBoundaryPeriod.ThirtyMinutes, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddHours(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneHour, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddHours(4));
        Assert.AreEqual(TimeBoundaryPeriod.FourHours, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddDays(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneDay, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddDays(7));
        Assert.AreEqual(TimeBoundaryPeriod.OneWeek, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddDays(28));
        Assert.AreEqual(TimeBoundaryPeriod.OneMonth, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddDays(30));
        Assert.AreEqual(TimeBoundaryPeriod.OneMonth, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddSeconds(2));
        Assert.AreEqual(TimeBoundaryPeriod.None, calculatePeriodSummary.CalcTimeFrame());
        calculatePeriodSummary = new PricePeriodSummary(TimeBoundaryPeriod.None, t, t.AddMilliseconds(10));
        Assert.AreEqual(TimeBoundaryPeriod.None, calculatePeriodSummary.CalcTimeFrame());
    }
}
