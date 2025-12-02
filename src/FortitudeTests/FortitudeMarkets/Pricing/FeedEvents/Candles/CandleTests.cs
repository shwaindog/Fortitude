// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Candles;

[TestClass]
public class CandleTests
{
    private Candle emptyCandle = null!;

    private decimal  expectedEndAskPrice;
    private decimal  expectedEndBidPrice;
    private DateTime expectedEndTime;
    private decimal  expectedHighestAskPrice;
    private decimal  expectedHighestBidPrice;
    private decimal  expectedLowestAskPrice;
    private decimal  expectedLowestBidPrice;
    private decimal  expectedStartAskPrice;
    private decimal  expectedStartBidPrice;
    private DateTime expectedStartTime;
    private uint     expectedTickCount;

    private TimeBoundaryPeriod expectedTimeBoundaryPeriod;
    private long               expectedVolume;

    private Candle fullyPopulatedCandle = null!;

    [TestInitialize]
    public void SetUp()
    {
        expectedTimeBoundaryPeriod = TimeBoundaryPeriod.OneMinute;
        expectedStartTime          = new DateTime(2018, 2, 6, 20, 51, 0);
        expectedEndTime            = new DateTime(2018, 2, 6, 20, 52, 0);
        expectedStartBidPrice      = 1.234567m;
        expectedStartAskPrice      = 1.234577m;
        expectedHighestBidPrice    = 1.235567m;
        expectedHighestAskPrice    = 1.235577m;
        expectedLowestBidPrice     = 1.233567m;
        expectedLowestAskPrice     = 1.233577m;
        expectedEndBidPrice        = 1.234467m;
        expectedEndAskPrice        = 1.234477m;
        expectedTickCount          = 532;
        expectedVolume             = 428700;

        emptyCandle = new Candle();
        fullyPopulatedCandle =
            new Candle
                (expectedTimeBoundaryPeriod, expectedStartTime, expectedEndTime, expectedStartBidPrice, expectedStartAskPrice
               , expectedHighestBidPrice, expectedHighestAskPrice, expectedLowestBidPrice, expectedLowestAskPrice
               , expectedEndBidPrice, expectedEndAskPrice, expectedTickCount, expectedVolume);
    }

    [TestMethod]
    public void EmptyCandle_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(TimeBoundaryPeriod.Tick, emptyCandle.TimeBoundaryPeriod);
        Assert.AreEqual(DateTime.MinValue, emptyCandle.PeriodStartTime);
        Assert.AreEqual(DateTime.MinValue, emptyCandle.PeriodEndTime);

        Assert.AreEqual(0m, emptyCandle.StartBidPrice);
        Assert.AreEqual(0m, emptyCandle.StartAskPrice);
        Assert.AreEqual(0m, emptyCandle.HighestBidPrice);
        Assert.AreEqual(0m, emptyCandle.HighestAskPrice);
        Assert.AreEqual(0m, emptyCandle.LowestBidPrice);
        Assert.AreEqual(0m, emptyCandle.LowestAskPrice);
        Assert.AreEqual(0m, emptyCandle.EndBidPrice);
        Assert.AreEqual(0m, emptyCandle.EndAskPrice);
        Assert.AreEqual(0u, emptyCandle.TickCount);
        Assert.AreEqual(0L, emptyCandle.PeriodVolume);
    }

    [TestMethod]
    public void PopulatedCandle_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(expectedTimeBoundaryPeriod, fullyPopulatedCandle.TimeBoundaryPeriod);
        Assert.AreEqual(expectedStartTime, fullyPopulatedCandle.PeriodStartTime);
        Assert.AreEqual(expectedEndTime, fullyPopulatedCandle.PeriodEndTime);
        Assert.AreEqual(expectedStartBidPrice, fullyPopulatedCandle.StartBidPrice);
        Assert.AreEqual(expectedStartAskPrice, fullyPopulatedCandle.StartAskPrice);
        Assert.AreEqual(expectedHighestBidPrice, fullyPopulatedCandle.HighestBidPrice);
        Assert.AreEqual(expectedHighestAskPrice, fullyPopulatedCandle.HighestAskPrice);
        Assert.AreEqual(expectedLowestBidPrice, fullyPopulatedCandle.LowestBidPrice);
        Assert.AreEqual(expectedLowestAskPrice, fullyPopulatedCandle.LowestAskPrice);
        Assert.AreEqual(expectedEndBidPrice, fullyPopulatedCandle.EndBidPrice);
        Assert.AreEqual(expectedEndAskPrice, fullyPopulatedCandle.EndAskPrice);
        Assert.AreEqual(expectedTickCount, fullyPopulatedCandle.TickCount);
        Assert.AreEqual(expectedVolume, fullyPopulatedCandle.PeriodVolume);
    }

    [TestMethod]
    public void UnknownTimeFrame_New_CalculatesCorrectTimeFrameForCandle()
    {
        fullyPopulatedCandle =
            new Candle
                (expectedTimeBoundaryPeriod, expectedStartTime, expectedEndTime, expectedStartBidPrice, expectedStartAskPrice
               , expectedHighestBidPrice, expectedHighestAskPrice, expectedLowestBidPrice, expectedLowestAskPrice
               , expectedEndBidPrice, expectedEndAskPrice, expectedTickCount, expectedVolume);

        Assert.AreEqual(expectedTimeBoundaryPeriod, fullyPopulatedCandle.TimeBoundaryPeriod);
    }

    [TestMethod]
    public void PopulatedCandle_New_CopiesValues()
    {
        var copyCandle = new Candle(fullyPopulatedCandle);

        Assert.AreEqual(fullyPopulatedCandle, copyCandle);
    }

    [TestMethod]
    public void UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly()
    {
        var t = expectedStartTime;

        var calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.OneSecond, t, t.AddSeconds(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneSecond, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.OneMinute, t, t.AddMinutes(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneMinute, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.FiveMinutes, t, t.AddMinutes(5));
        Assert.AreEqual(TimeBoundaryPeriod.FiveMinutes, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.TenMinutes, t, t.AddMinutes(10));
        Assert.AreEqual(TimeBoundaryPeriod.TenMinutes, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.FifteenMinutes, t, t.AddMinutes(15));
        Assert.AreEqual(TimeBoundaryPeriod.FifteenMinutes, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.ThirtyMinutes, t, t.AddMinutes(30));
        Assert.AreEqual(TimeBoundaryPeriod.ThirtyMinutes, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.OneHour, t, t.AddHours(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneHour, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.FourHours, t, t.AddHours(4));
        Assert.AreEqual(TimeBoundaryPeriod.FourHours, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.OneDay, t, t.AddDays(1));
        Assert.AreEqual(TimeBoundaryPeriod.OneDay, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.OneWeek, t, t.AddDays(7));
        Assert.AreEqual(TimeBoundaryPeriod.OneWeek, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.OneMonth, t, t.AddDays(28));
        Assert.AreEqual(TimeBoundaryPeriod.OneMonth, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.OneMonth, t, t.AddDays(30));
        Assert.AreEqual(TimeBoundaryPeriod.OneMonth, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.Tick, t, t.AddSeconds(2));
        Assert.AreEqual(TimeBoundaryPeriod.Tick, calculateCandleTimeBoundary.TimeBoundaryPeriod);
        calculateCandleTimeBoundary = new Candle(TimeBoundaryPeriod.Tick, t, t.AddMilliseconds(10));
        Assert.AreEqual(TimeBoundaryPeriod.Tick, calculateCandleTimeBoundary.TimeBoundaryPeriod);
    }

    [TestMethod]
    public void PopulatedQuote_Mutate_UpdatesFields()
    {
        emptyCandle.TimeBoundaryPeriod = expectedTimeBoundaryPeriod;
        emptyCandle.PeriodStartTime    = expectedStartTime;
        emptyCandle.PeriodEndTime      = expectedEndTime;
        emptyCandle.StartBidPrice      = expectedStartBidPrice;
        emptyCandle.StartAskPrice      = expectedStartAskPrice;
        emptyCandle.HighestBidPrice    = expectedHighestBidPrice;
        emptyCandle.HighestAskPrice    = expectedHighestAskPrice;
        emptyCandle.LowestBidPrice     = expectedLowestBidPrice;
        emptyCandle.LowestAskPrice     = expectedLowestAskPrice;
        emptyCandle.EndBidPrice        = expectedEndBidPrice;
        emptyCandle.EndAskPrice        = expectedEndAskPrice;
        emptyCandle.TickCount          = expectedTickCount;
        emptyCandle.PeriodVolume       = expectedVolume;

        Assert.AreEqual(expectedTimeBoundaryPeriod, emptyCandle.TimeBoundaryPeriod);
        Assert.AreEqual(expectedStartTime, emptyCandle.PeriodStartTime);
        Assert.AreEqual(expectedEndTime, emptyCandle.PeriodEndTime);
        Assert.AreEqual(expectedStartBidPrice, emptyCandle.StartBidPrice);
        Assert.AreEqual(expectedStartAskPrice, emptyCandle.StartAskPrice);
        Assert.AreEqual(expectedHighestBidPrice, emptyCandle.HighestBidPrice);
        Assert.AreEqual(expectedHighestAskPrice, emptyCandle.HighestAskPrice);
        Assert.AreEqual(expectedLowestBidPrice, emptyCandle.LowestBidPrice);
        Assert.AreEqual(expectedLowestAskPrice, emptyCandle.LowestAskPrice);
        Assert.AreEqual(expectedEndBidPrice, emptyCandle.EndBidPrice);
        Assert.AreEqual(expectedEndAskPrice, emptyCandle.EndAskPrice);
        Assert.AreEqual(expectedTickCount, emptyCandle.TickCount);
        Assert.AreEqual(expectedVolume, emptyCandle.PeriodVolume);
    }

    [TestMethod]
    public void FullyPopulatedCandle_CopyFromToEmptyCandle_CandlesEqualEachOther()
    {
        emptyCandle.CopyFrom(fullyPopulatedCandle);

        Assert.AreEqual(fullyPopulatedCandle, emptyCandle);
    }

    [TestMethod]
    public void PQPopulatedCandle_CopyFromToEmptyCandle_CandlesEquivalentToEachOther()
    {
        var pqCandle = new PQCandle(fullyPopulatedCandle);
        emptyCandle.CopyFrom(fullyPopulatedCandle);
        Assert.IsTrue(emptyCandle.AreEquivalent(pqCandle));
    }

    [TestMethod]
    public void PopulatedCandle_Clone_CreatesCopyOfEverythingExceptSrcTkrInfo()
    {
        var clone = fullyPopulatedCandle.Clone();

        Assert.AreEqual(fullyPopulatedCandle, clone);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)fullyPopulatedCandle).Clone();

        Assert.AreNotSame(clone, fullyPopulatedCandle);
        Assert.AreEqual(fullyPopulatedCandle, clone);
        clone = fullyPopulatedCandle.Clone();
        Assert.AreNotSame(clone, fullyPopulatedCandle);
        Assert.AreEqual(fullyPopulatedCandle, clone);
        clone = ((ICloneable<ICandle>)fullyPopulatedCandle).Clone();
        Assert.AreNotSame(clone, fullyPopulatedCandle);
        Assert.AreEqual(fullyPopulatedCandle, clone);
        clone = ((IMutableCandle)fullyPopulatedCandle).Clone();
        Assert.AreNotSame(clone, fullyPopulatedCandle);
        Assert.AreEqual(fullyPopulatedCandle, clone);
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        AssertOneDifferenceAtATime
            (false, fullyPopulatedCandle, fullyPopulatedCandle.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        Assert.AreNotEqual(0, fullyPopulatedCandle.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        var q = fullyPopulatedCandle;

        var toString = q.ToString();

        Assert.IsTrue(toString.Contains(q.GetType().Name));

        Assert.IsTrue(toString.Contains($"{nameof(q.TimeBoundaryPeriod)}: {nameof(TimeBoundaryPeriod)}.{q.TimeBoundaryPeriod}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.PeriodStartTime)}: {q.PeriodStartTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.PeriodEndTime)}: {q.PeriodEndTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.StartBidPrice)}: {q.StartBidPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.StartAskPrice)}: {q.StartAskPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.HighestBidPrice)}: {q.HighestBidPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.HighestAskPrice)}: {q.HighestAskPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.LowestBidPrice)}: {q.LowestBidPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.LowestAskPrice)}: {q.LowestAskPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.EndBidPrice)}: {q.EndBidPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.EndAskPrice)}: {q.EndAskPrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.TickCount)}: {q.TickCount}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.PeriodVolume)}: {q.PeriodVolume:N2}"));
    }

    internal static void AssertOneDifferenceAtATime
    (bool exactComparison, IMutableCandle commonCandle,
        IMutableCandle changingCandle)
    {
        changingCandle.TimeBoundaryPeriod = TimeBoundaryPeriod.FourHours;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.TimeBoundaryPeriod = commonCandle.TimeBoundaryPeriod;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.PeriodStartTime = DateTime.Now;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.PeriodStartTime = commonCandle.PeriodStartTime;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.PeriodEndTime = DateTime.Now;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.PeriodEndTime = commonCandle.PeriodEndTime;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.StartBidPrice = 6.78901m;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.StartBidPrice = commonCandle.StartBidPrice;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.StartAskPrice = 6.78901m;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.StartAskPrice = commonCandle.StartAskPrice;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.HighestBidPrice = 6.78901m;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.HighestBidPrice = commonCandle.HighestBidPrice;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.HighestAskPrice = 6.78901m;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.HighestAskPrice = commonCandle.HighestAskPrice;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.LowestBidPrice = 6.78901m;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.LowestBidPrice = commonCandle.LowestBidPrice;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.LowestAskPrice = 6.78901m;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.LowestAskPrice = commonCandle.LowestAskPrice;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.EndBidPrice = 6.78901m;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.EndBidPrice = commonCandle.EndBidPrice;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.EndAskPrice = 6.78901m;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.EndAskPrice = commonCandle.EndAskPrice;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.TickCount = 23456u;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.TickCount = commonCandle.TickCount;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));

        changingCandle.PeriodVolume = 98765L;
        Assert.IsFalse(commonCandle.AreEquivalent(changingCandle));
        changingCandle.PeriodVolume = commonCandle.PeriodVolume;
        Assert.IsTrue(commonCandle.AreEquivalent(changingCandle));
    }


    public static Candle CreateCandle
        (TimeBoundaryPeriod period, DateTime startTime, decimal mid, decimal openCloseSpread = 0.02m, decimal highLowSpread = 0.4m)
    {
        var halfSpread        = openCloseSpread / 2;
        var halfHighLowSpread = highLowSpread / 2;

        var bid1 = mid - halfSpread;
        var ask1 = mid + halfSpread;
        var bid2 = mid + halfHighLowSpread - halfSpread;
        var ask2 = mid + halfHighLowSpread + halfSpread;
        var bid3 = mid - halfHighLowSpread - halfSpread;
        var ask3 = mid - halfHighLowSpread + halfSpread;

        return new Candle
            (period, startTime, period.PeriodEnd(startTime), bid1, ask1, bid2, ask2, bid3
           , ask3, bid1, ask1, 10, 0, bid1, ask1);
    }
}
