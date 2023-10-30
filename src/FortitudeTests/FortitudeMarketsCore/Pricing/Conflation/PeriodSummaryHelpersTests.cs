using System;
using FortitudeMarketsApi.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.Conflation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Conflation
{
    [TestClass]
    public class PeriodSummaryHelpersTests
    {
        [TestMethod]
        public void UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly()
        {
            var t = new DateTime(2018, 2, 7, 22, 35, 10);
            var calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddSeconds(1));
            Assert.AreEqual(TimeFrame.OneSecond, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(1));
            Assert.AreEqual(TimeFrame.OneMinute, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(5));
            Assert.AreEqual(TimeFrame.FiveMinutes, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(10));
            Assert.AreEqual(TimeFrame.TenMinutes, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(15));
            Assert.AreEqual(TimeFrame.FifteenMinutes, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(30));
            Assert.AreEqual(TimeFrame.ThirtyMinutes, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddHours(1));
            Assert.AreEqual(TimeFrame.OneHour, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddHours(4));
            Assert.AreEqual(TimeFrame.FourHours, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddDays(1));
            Assert.AreEqual(TimeFrame.OneDay, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddDays(7));
            Assert.AreEqual(TimeFrame.OneWeek, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddDays(28));
            Assert.AreEqual(TimeFrame.OneMonth, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddDays(30));
            Assert.AreEqual(TimeFrame.OneMonth, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddSeconds(2));
            Assert.AreEqual(TimeFrame.Conflation, calculatePeriodSummary.CalcTimeFrame());
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMilliseconds(10));
            Assert.AreEqual(TimeFrame.Conflation, calculatePeriodSummary.CalcTimeFrame());
        }
    }
}