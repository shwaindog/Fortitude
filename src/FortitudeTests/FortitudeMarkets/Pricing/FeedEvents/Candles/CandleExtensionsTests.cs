// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing.FeedEvents.Candles;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Candles;

[TestClass]
public class CandleExtensionsTests
{
    [TestMethod]
    public void UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly()
    {
        var t = new DateTime(2018, 2, 7, 22, 35, 10);

        var calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddSeconds(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneSecond, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddMinutes(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneMinute, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddMinutes(5));
        Assert.AreEqual(TimeBoundaryPeriod.FiveMinutes, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddMinutes(10));
        Assert.AreEqual(TimeBoundaryPeriod.TenMinutes, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddMinutes(15));
        Assert.AreEqual(TimeBoundaryPeriod.FifteenMinutes, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddMinutes(30));
        Assert.AreEqual(TimeBoundaryPeriod.ThirtyMinutes, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddHours(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneHour, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddHours(4));
        Assert.AreEqual(TimeBoundaryPeriod.FourHours, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddDays(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneDay, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddDays(7));
        Assert.AreEqual(TimeBoundaryPeriod.OneWeek, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddDays(28));
        Assert.AreEqual(TimeBoundaryPeriod.OneMonth, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddDays(30));
        Assert.AreEqual(TimeBoundaryPeriod.OneMonth, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddSeconds(2));
        Assert.AreEqual(TimeBoundaryPeriod.Tick, calculateCandlePeriod.CalcTimeFrame());
        calculateCandlePeriod = new Candle(TimeBoundaryPeriod.Tick, t, t.AddMilliseconds(10));
        Assert.AreEqual(TimeBoundaryPeriod.Tick, calculateCandlePeriod.CalcTimeFrame());
    }
}
