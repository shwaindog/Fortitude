using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.PQ.Conflation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Conflation
{
    [TestClass]
    public class PeriodSummaryTests
    {
        private PeriodSummary emptyPeriodSummary;
        private PeriodSummary fullyPopulatedPeriodSummary;

        private TimeFrame expectedTimeFrame;
        private DateTime expectedStartTime;
        private DateTime expectedEndTime;
        private decimal expectedStartBidPrice;
        private decimal expectedStartAskPrice;
        private decimal expectedHighestBidPrice;
        private decimal expectedHighestAskPrice;
        private decimal expectedLowestBidPrice;
        private decimal expectedLowestAskPrice;
        private decimal expectedEndBidPrice;
        private decimal expectedEndAskPrice;
        private uint expectedTickCount;
        private long expectedVolume;

        [TestInitialize]
        public void SetUp()
        {
            expectedTimeFrame = TimeFrame.OneMinute;
            expectedStartTime = new DateTime(2018, 2, 6, 20, 51, 0);
            expectedEndTime = new DateTime(2018, 2, 6, 20, 52, 0);
            expectedStartBidPrice = 1.234567m;
            expectedStartAskPrice = 1.234577m;
            expectedHighestBidPrice = 1.235567m;
            expectedHighestAskPrice = 1.235577m;
            expectedLowestBidPrice = 1.233567m;
            expectedLowestAskPrice = 1.233577m;
            expectedEndBidPrice = 1.234467m;
            expectedEndAskPrice = 1.234477m;
            expectedTickCount = 532;
            expectedVolume = 428700;
            
            emptyPeriodSummary = new PeriodSummary();
            fullyPopulatedPeriodSummary = new PeriodSummary(expectedTimeFrame, expectedStartTime,
                expectedEndTime, expectedStartBidPrice, expectedStartAskPrice, expectedHighestBidPrice, 
                expectedHighestAskPrice, expectedLowestBidPrice, expectedLowestAskPrice, expectedEndBidPrice, 
                expectedEndAskPrice, expectedTickCount, expectedVolume);
        }
        
        [TestMethod]
        public void EmptyPeriodSummary_New_InitializesFieldsAsExpected()
        {
            Assert.AreEqual(TimeFrame.Unknown, emptyPeriodSummary.TimeFrame);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPeriodSummary.StartTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPeriodSummary.EndTime);
            Assert.AreEqual(0m, emptyPeriodSummary.StartBidPrice);
            Assert.AreEqual(0m, emptyPeriodSummary.StartAskPrice);
            Assert.AreEqual(0m, emptyPeriodSummary.HighestBidPrice);
            Assert.AreEqual(0m, emptyPeriodSummary.HighestAskPrice);
            Assert.AreEqual(0m, emptyPeriodSummary.LowestBidPrice);
            Assert.AreEqual(0m, emptyPeriodSummary.LowestAskPrice);
            Assert.AreEqual(0m, emptyPeriodSummary.EndBidPrice);
            Assert.AreEqual(0m, emptyPeriodSummary.EndAskPrice);
            Assert.AreEqual(0u, emptyPeriodSummary.TickCount);
            Assert.AreEqual(0L, emptyPeriodSummary.PeriodVolume);
        }
        
        [TestMethod]
        public void PopulatedPeriodSummary_New_InitializesFieldsAsExpected()
        {
            Assert.AreEqual(expectedTimeFrame, fullyPopulatedPeriodSummary.TimeFrame);
            Assert.AreEqual(expectedStartTime, fullyPopulatedPeriodSummary.StartTime);
            Assert.AreEqual(expectedEndTime, fullyPopulatedPeriodSummary.EndTime);
            Assert.AreEqual(expectedStartBidPrice, fullyPopulatedPeriodSummary.StartBidPrice);
            Assert.AreEqual(expectedStartAskPrice, fullyPopulatedPeriodSummary.StartAskPrice);
            Assert.AreEqual(expectedHighestBidPrice, fullyPopulatedPeriodSummary.HighestBidPrice);
            Assert.AreEqual(expectedHighestAskPrice, fullyPopulatedPeriodSummary.HighestAskPrice);
            Assert.AreEqual(expectedLowestBidPrice, fullyPopulatedPeriodSummary.LowestBidPrice);
            Assert.AreEqual(expectedLowestAskPrice, fullyPopulatedPeriodSummary.LowestAskPrice);
            Assert.AreEqual(expectedEndBidPrice, fullyPopulatedPeriodSummary.EndBidPrice);
            Assert.AreEqual(expectedEndAskPrice, fullyPopulatedPeriodSummary.EndAskPrice);
            Assert.AreEqual(expectedTickCount, fullyPopulatedPeriodSummary.TickCount);
            Assert.AreEqual(expectedVolume, fullyPopulatedPeriodSummary.PeriodVolume);
        }

        [TestMethod]
        public void UnknownTimeFrame_New_CalculatesCorrectTimeFrameForPeriodSummary()
        {
            fullyPopulatedPeriodSummary = new PeriodSummary(TimeFrame.Unknown, expectedStartTime,
                expectedEndTime, expectedStartBidPrice, expectedStartAskPrice, expectedHighestBidPrice,
                expectedHighestAskPrice, expectedLowestBidPrice, expectedLowestAskPrice, expectedEndBidPrice,
                expectedEndAskPrice, expectedTickCount, expectedVolume);

            Assert.AreEqual(expectedTimeFrame, fullyPopulatedPeriodSummary.TimeFrame);
        }

        [TestMethod]
        public void PopulatedPeriodSummary_New_CopiesValues()
        {
            var copyPeriodSummary = new PeriodSummary(fullyPopulatedPeriodSummary);

            Assert.AreEqual(fullyPopulatedPeriodSummary, copyPeriodSummary);
        }

        [TestMethod]
        public void UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly()
        {
            var t = expectedStartTime;
            var calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddSeconds(1));
            Assert.AreEqual(TimeFrame.OneSecond, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(1));
            Assert.AreEqual(TimeFrame.OneMinute, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(5));
            Assert.AreEqual(TimeFrame.FiveMinutes, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(10));
            Assert.AreEqual(TimeFrame.TenMinutes, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(15));
            Assert.AreEqual(TimeFrame.FifteenMinutes, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMinutes(30));
            Assert.AreEqual(TimeFrame.ThirtyMinutes, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddHours(1));
            Assert.AreEqual(TimeFrame.OneHour, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddHours(4));
            Assert.AreEqual(TimeFrame.FourHours, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddDays(1));
            Assert.AreEqual(TimeFrame.OneDay, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddDays(7));
            Assert.AreEqual(TimeFrame.OneWeek, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddDays(28));
            Assert.AreEqual(TimeFrame.OneMonth, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddDays(30));
            Assert.AreEqual(TimeFrame.OneMonth, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddSeconds(2));
            Assert.AreEqual(TimeFrame.Conflation, calculatePeriodSummary.TimeFrame);
            calculatePeriodSummary = new PeriodSummary(TimeFrame.Unknown, t, t.AddMilliseconds(10));
            Assert.AreEqual(TimeFrame.Conflation, calculatePeriodSummary.TimeFrame);
        }

        [TestMethod]
        public void PopulatedQuote_Mutate_UpdatesFields()
        {
            emptyPeriodSummary.TimeFrame = expectedTimeFrame;
            emptyPeriodSummary.StartTime = expectedStartTime;
            emptyPeriodSummary.EndTime = expectedEndTime;
            emptyPeriodSummary.StartBidPrice = expectedStartBidPrice;
            emptyPeriodSummary.StartAskPrice = expectedStartAskPrice;
            emptyPeriodSummary.HighestBidPrice = expectedHighestBidPrice;
            emptyPeriodSummary.HighestAskPrice = expectedHighestAskPrice;
            emptyPeriodSummary.LowestBidPrice = expectedLowestBidPrice;
            emptyPeriodSummary.LowestAskPrice = expectedLowestAskPrice;
            emptyPeriodSummary.EndBidPrice = expectedEndBidPrice;
            emptyPeriodSummary.EndAskPrice = expectedEndAskPrice;
            emptyPeriodSummary.TickCount = expectedTickCount;
            emptyPeriodSummary.PeriodVolume = expectedVolume;

            Assert.AreEqual(expectedTimeFrame, emptyPeriodSummary.TimeFrame);
            Assert.AreEqual(expectedStartTime, emptyPeriodSummary.StartTime);
            Assert.AreEqual(expectedEndTime, emptyPeriodSummary.EndTime);
            Assert.AreEqual(expectedStartBidPrice, emptyPeriodSummary.StartBidPrice);
            Assert.AreEqual(expectedStartAskPrice, emptyPeriodSummary.StartAskPrice);
            Assert.AreEqual(expectedHighestBidPrice, emptyPeriodSummary.HighestBidPrice);
            Assert.AreEqual(expectedHighestAskPrice, emptyPeriodSummary.HighestAskPrice);
            Assert.AreEqual(expectedLowestBidPrice, emptyPeriodSummary.LowestBidPrice);
            Assert.AreEqual(expectedLowestAskPrice, emptyPeriodSummary.LowestAskPrice);
            Assert.AreEqual(expectedEndBidPrice, emptyPeriodSummary.EndBidPrice);
            Assert.AreEqual(expectedEndAskPrice, emptyPeriodSummary.EndAskPrice);
            Assert.AreEqual(expectedTickCount, emptyPeriodSummary.TickCount);
            Assert.AreEqual(expectedVolume, emptyPeriodSummary.PeriodVolume);
        }

        [TestMethod]
        public void FullyPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_PeriodSummariesEqualEachOther()
        {
            emptyPeriodSummary.CopyFrom(fullyPopulatedPeriodSummary);

            Assert.AreEqual(fullyPopulatedPeriodSummary, emptyPeriodSummary);
        }

        [TestMethod]
        public void PQPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_PeriodSummariesEquivalentToEachOther()
        {
            var pqPeriodSummary = new PQPeriodSummary(fullyPopulatedPeriodSummary);
            emptyPeriodSummary.CopyFrom(fullyPopulatedPeriodSummary);
            Assert.IsTrue(emptyPeriodSummary.AreEquivalent(pqPeriodSummary));
        }

        [TestMethod]
        public void PopulatedPeriodSummary_Clone_CreatesCopyOfEverythingExceptSrcTkrQtInfo()
        {
            var clone = fullyPopulatedPeriodSummary.Clone();
            Assert.AreEqual(fullyPopulatedPeriodSummary, clone);
        }

        [TestMethod]
        public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
        {
            var clone = ((ICloneable)fullyPopulatedPeriodSummary).Clone();
            Assert.AreNotSame(clone, fullyPopulatedPeriodSummary);
            Assert.AreEqual(fullyPopulatedPeriodSummary, clone);
            clone = fullyPopulatedPeriodSummary.Clone();
            Assert.AreNotSame(clone, fullyPopulatedPeriodSummary);
            Assert.AreEqual(fullyPopulatedPeriodSummary, clone);
            clone = ((ICloneable<IPeriodSummary>)fullyPopulatedPeriodSummary).Clone();
            Assert.AreNotSame(clone, fullyPopulatedPeriodSummary);
            Assert.AreEqual(fullyPopulatedPeriodSummary, clone);
            clone = ((IMutablePeriodSummary)fullyPopulatedPeriodSummary).Clone();
            Assert.AreNotSame(clone, fullyPopulatedPeriodSummary);
            Assert.AreEqual(fullyPopulatedPeriodSummary, clone);
        }

        [TestMethod]
        public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
        {
            AssertOneDifferenceAtATime(false, fullyPopulatedPeriodSummary,
                fullyPopulatedPeriodSummary.Clone());
        }

        [TestMethod]
        public void PopulatedQuote_GetHashCode_NotEqualToZero()
        {
            Assert.AreNotEqual(0, fullyPopulatedPeriodSummary.GetHashCode());
        }

        [TestMethod]
        public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
        {
            var q = fullyPopulatedPeriodSummary;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));

            Assert.IsTrue(toString.Contains($"{nameof(q.TimeFrame)}: {q.TimeFrame}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.StartTime)}: {q.StartTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.EndTime)}: {q.EndTime:O}"));
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

        internal static void AssertOneDifferenceAtATime(bool exactComparison, IMutablePeriodSummary commonPeriodSummary,
            IMutablePeriodSummary changingPeriodSummary)
        {
            changingPeriodSummary.TimeFrame = TimeFrame.FourHours;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.TimeFrame = commonPeriodSummary.TimeFrame;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.StartTime = DateTime.Now;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.StartTime = commonPeriodSummary.StartTime;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.EndTime = DateTime.Now;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.EndTime = commonPeriodSummary.EndTime;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.StartBidPrice = 6.78901m;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.StartBidPrice = commonPeriodSummary.StartBidPrice;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.StartAskPrice = 6.78901m;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.StartAskPrice = commonPeriodSummary.StartAskPrice;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.HighestBidPrice = 6.78901m;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.HighestBidPrice = commonPeriodSummary.HighestBidPrice;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.HighestAskPrice = 6.78901m;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.HighestAskPrice = commonPeriodSummary.HighestAskPrice;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.LowestBidPrice = 6.78901m;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.LowestBidPrice = commonPeriodSummary.LowestBidPrice;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.LowestAskPrice = 6.78901m;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.LowestAskPrice = commonPeriodSummary.LowestAskPrice;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.EndBidPrice = 6.78901m;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.EndBidPrice = commonPeriodSummary.EndBidPrice;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.EndAskPrice = 6.78901m;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.EndAskPrice = commonPeriodSummary.EndAskPrice;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.TickCount = 23456u;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.TickCount = commonPeriodSummary.TickCount;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));

            changingPeriodSummary.PeriodVolume = 98765L;
            Assert.IsFalse(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
            changingPeriodSummary.PeriodVolume = commonPeriodSummary.PeriodVolume;
            Assert.IsTrue(commonPeriodSummary.AreEquivalent(changingPeriodSummary));
        }
    }
}