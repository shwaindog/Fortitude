﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Summaries;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Summaries;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes;

[TestClass]
public class Level1PriceQuoteTests
{
    private Level1PriceQuote emptyQuote                = null!;
    private Level1PriceQuote fullyPopulatedLevel1Quote = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private ISourceTickerInfo sourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        emptyQuote                = new Level1PriceQuote(sourceTickerInfo);
        fullyPopulatedLevel1Quote = new Level1PriceQuote(sourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedLevel1Quote, 1);
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreSame(sourceTickerInfo, emptyQuote.SourceTickerInfo);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.ClientReceivedTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterSentTime);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceBidTime);
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceAskTime);
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        Assert.AreEqual(false, emptyQuote.Executable);
        Assert.IsNull(emptyQuote.SummaryPeriod);
    }

    [TestMethod]
    public void IntializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 23, 56, 59);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSingleValue        = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime    = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime      = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop        = 2.34567m;
        var expectedSourceAskTime      = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop        = 3.45678m;
        var expectedPeriodSummary      = new PricePeriodSummary();

        var fromConstructor =
            new Level1PriceQuote
                (sourceTickerInfo, expectedSourceTime, true, FeedSyncStatus.Good, expectedSingleValue, expectedClientReceivedTime
               , expectedAdapterReceiveTime, expectedAdapterSentTime, expectedSourceBidTime
               , expectedSourceTime, expectedSourceTime.AddSeconds(2), expectedBidPriceTop, true, expectedSourceAskTime, expectedAskPriceTop
               , true, true, expectedPeriodSummary);

        Assert.AreSame(sourceTickerInfo, fromConstructor.SourceTickerInfo);
        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(true, fromConstructor.IsReplay);
        Assert.AreEqual(expectedSingleValue, fromConstructor.SingleTickValue);
        Assert.AreEqual(expectedClientReceivedTime, fromConstructor.ClientReceivedTime);
        Assert.AreEqual(expectedAdapterReceiveTime, fromConstructor.AdapterReceivedTime);
        Assert.AreEqual(expectedAdapterSentTime, fromConstructor.AdapterSentTime);
        Assert.AreEqual(expectedSourceBidTime, fromConstructor.SourceBidTime);
        Assert.AreEqual(expectedBidPriceTop, fromConstructor.BidPriceTop);
        Assert.AreEqual(true, fromConstructor.IsBidPriceTopUpdated);
        Assert.AreEqual(expectedSourceAskTime, fromConstructor.SourceAskTime);
        Assert.AreEqual(expectedAskPriceTop, fromConstructor.AskPriceTop);
        Assert.AreEqual(true, fromConstructor.IsAskPriceTopUpdated);
        Assert.AreEqual(true, fromConstructor.Executable);
        Assert.AreEqual(expectedPeriodSummary, fromConstructor.SummaryPeriod);
    }

    [TestMethod]
    public void NonPeriodSummary_New_ConvertsToPeriodSummary()
    {
        var pqPeriodSummary = new PQPricePeriodSummary();

        var nonSourceTickerInfoL1Quote =
            new Level1PriceQuote
                (sourceTickerInfo, DateTime.Now, true, FeedSyncStatus.Good, 1m, DateTime.Now, DateTime.Now
               , DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now.AddSeconds(2), 1m, true, DateTime.Now
               , 1m, true, true, pqPeriodSummary);

        Assert.IsInstanceOfType(nonSourceTickerInfoL1Quote.SummaryPeriod, typeof(PricePeriodSummary));
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValues()
    {
        var copyQuote = new Level1PriceQuote(fullyPopulatedLevel1Quote);

        Assert.AreEqual(fullyPopulatedLevel1Quote, copyQuote);
    }

    [TestMethod]
    public void NonSourceTickerInfo_New_CopiesExceptPeriodSummaryIsConverted()
    {
        var originalPeriodSummary = fullyPopulatedLevel1Quote.SummaryPeriod!;
        var pqPeriodSummary       = new PQPricePeriodSummary(originalPeriodSummary);
        fullyPopulatedLevel1Quote.SummaryPeriod = pqPeriodSummary;
        var copyQuote = new Level1PriceQuote(fullyPopulatedLevel1Quote);
        Assert.AreNotEqual(fullyPopulatedLevel1Quote, copyQuote);
        fullyPopulatedLevel1Quote.SummaryPeriod = originalPeriodSummary;
        Assert.AreEqual(fullyPopulatedLevel1Quote, copyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_SourceTimeIsGreaterOfBidAskOrOriginalSourceTime()
    {
        var originalSourceTime = new DateTime(2017, 11, 08, 22, 30, 51);
        emptyQuote.SourceTime = originalSourceTime;
        Assert.AreEqual(originalSourceTime, emptyQuote.SourceTime);

        var higherAskTime = originalSourceTime.AddMilliseconds(123);
        emptyQuote.SourceAskTime = higherAskTime;
        Assert.AreEqual(higherAskTime, emptyQuote.SourceTime);

        var higherBidTime = higherAskTime.AddMilliseconds(123);
        emptyQuote.SourceBidTime = higherBidTime;
        Assert.AreEqual(higherBidTime, emptyQuote.SourceTime);

        var highestSourceTime = higherBidTime.AddMilliseconds(123);
        emptyQuote.SourceTime = highestSourceTime;
        Assert.AreEqual(highestSourceTime, emptyQuote.SourceTime);
    }

    [TestMethod]
    public void PopulatedQuote_Mutate_UpdatesFields()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 23, 56, 59);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSingleValue        = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime    = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime      = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop        = 2.34567m;
        var expectedSourceAskTime      = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop        = 3.45678m;
        var expectedPeriodSummary      = new PricePeriodSummary();

        emptyQuote.SourceTime           = expectedSourceTime;
        emptyQuote.IsReplay             = true;
        emptyQuote.SingleTickValue      = expectedSingleValue;
        emptyQuote.ClientReceivedTime   = expectedClientReceivedTime;
        emptyQuote.AdapterReceivedTime  = expectedAdapterReceiveTime;
        emptyQuote.AdapterSentTime      = expectedAdapterSentTime;
        emptyQuote.SourceBidTime        = expectedSourceBidTime;
        emptyQuote.BidPriceTop          = expectedBidPriceTop;
        emptyQuote.IsBidPriceTopUpdated = true;
        emptyQuote.SourceAskTime        = expectedSourceAskTime;
        emptyQuote.AskPriceTop          = expectedAskPriceTop;
        emptyQuote.IsAskPriceTopUpdated = true;
        emptyQuote.Executable           = true;
        emptyQuote.SummaryPeriod        = expectedPeriodSummary;

        Assert.AreSame(sourceTickerInfo, emptyQuote.SourceTickerInfo);
        Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
        Assert.AreEqual(true, emptyQuote.IsReplay);
        Assert.AreEqual(expectedSingleValue, emptyQuote.SingleTickValue);
        Assert.AreEqual(expectedClientReceivedTime, emptyQuote.ClientReceivedTime);
        Assert.AreEqual(expectedAdapterReceiveTime, emptyQuote.AdapterReceivedTime);
        Assert.AreEqual(expectedAdapterSentTime, emptyQuote.AdapterSentTime);
        Assert.AreEqual(expectedSourceBidTime, emptyQuote.SourceBidTime);
        Assert.AreEqual(expectedBidPriceTop, emptyQuote.BidPriceTop);
        Assert.AreEqual(true, emptyQuote.IsBidPriceTopUpdated);
        Assert.AreEqual(expectedSourceAskTime, emptyQuote.SourceAskTime);
        Assert.AreEqual(expectedAskPriceTop, emptyQuote.AskPriceTop);
        Assert.AreEqual(true, emptyQuote.IsAskPriceTopUpdated);
        Assert.AreEqual(true, emptyQuote.Executable);
        Assert.AreEqual(expectedPeriodSummary, emptyQuote.SummaryPeriod);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new Level1PriceQuote(sourceTickerInfo);
        emptyQuote.CopyFrom(fullyPopulatedLevel1Quote);

        Assert.AreEqual(fullyPopulatedLevel1Quote, emptyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent()
    {
        var emptyLowerLevelQuote = new TickInstant(sourceTickerInfo);
        emptyLowerLevelQuote.CopyFrom(fullyPopulatedLevel1Quote);

        Assert.AreNotEqual(fullyPopulatedLevel1Quote, emptyLowerLevelQuote);
        Assert.IsTrue(emptyLowerLevelQuote.AreEquivalent(fullyPopulatedLevel1Quote));
    }

    [TestMethod]
    public void PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var pqLevel1Quote = new PQLevel1Quote(fullyPopulatedLevel1Quote);
        emptyQuote.CopyFrom(fullyPopulatedLevel1Quote);
        Assert.IsTrue(emptyQuote.AreEquivalent(pqLevel1Quote));
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        IMutableTickInstant clone = fullyPopulatedLevel1Quote.Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = (IMutableTickInstant)((ICloneable<ITickInstant>)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = (IMutableTickInstant)((ITickInstant)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = ((IMutableTickInstant)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = (IMutableTickInstant)((ICloneable<ILevel1Quote>)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = (IMutableTickInstant)((ILevel1Quote)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
        clone = ((IMutableLevel1Quote)fullyPopulatedLevel1Quote).Clone();
        Assert.AreNotSame(clone, fullyPopulatedLevel1Quote);
        Assert.AreEqual(fullyPopulatedLevel1Quote, clone);
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, fullyPopulatedLevel1Quote, (IMutableLevel1Quote)fullyPopulatedLevel1Quote.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        Assert.AreNotEqual(0, fullyPopulatedLevel1Quote.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        var q        = fullyPopulatedLevel1Quote;
        var toString = q.ToString();

        Assert.IsTrue(toString.Contains(q.GetType().Name));

        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTickerInfo)}: {q.SourceTickerInfo}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTime)}: {q.SourceTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsReplay)}: {q.IsReplay}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SingleTickValue)}: {q.SingleTickValue:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.ClientReceivedTime)}: {q.ClientReceivedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.AdapterReceivedTime)}: {q.AdapterReceivedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.AdapterSentTime)}: {q.AdapterSentTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceBidTime)}: {q.SourceBidTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.BidPriceTop)}: {q.BidPriceTop:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsBidPriceTopUpdated)}: {q.IsBidPriceTopUpdated}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceAskTime)}: {q.SourceAskTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.AskPriceTop)}: {q.AskPriceTop:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsAskPriceTopUpdated)}: {q.IsAskPriceTopUpdated}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.Executable)}: {q.Executable}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SummaryPeriod)}: {q.SummaryPeriod}"));
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
        (bool exactComparison, IMutableLevel1Quote commonCompareQuote, IMutableLevel1Quote changingQuote)
    {
        TickInstantTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareQuote, changingQuote);

        var diffPeriodSummary = commonCompareQuote.SummaryPeriod!.Clone();
        diffPeriodSummary.HighestAskPrice = 3.45678m;
        changingQuote.SummaryPeriod       = diffPeriodSummary;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SummaryPeriod = commonCompareQuote.SummaryPeriod.Clone();
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.AdapterReceivedTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.AdapterReceivedTime = commonCompareQuote.AdapterReceivedTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.AdapterSentTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.AdapterSentTime = commonCompareQuote.AdapterSentTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SourceBidTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceBidTime = commonCompareQuote.SourceBidTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.BidPriceTop = 3.4567m;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.BidPriceTop = commonCompareQuote.BidPriceTop;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.IsBidPriceTopUpdated = !commonCompareQuote.IsBidPriceTopUpdated;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.IsBidPriceTopUpdated = commonCompareQuote.IsBidPriceTopUpdated;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SourceAskTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceAskTime = commonCompareQuote.SourceAskTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.AskPriceTop = 3.4567m;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.AskPriceTop = commonCompareQuote.AskPriceTop;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.IsAskPriceTopUpdated = !commonCompareQuote.IsAskPriceTopUpdated;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.IsAskPriceTopUpdated = commonCompareQuote.IsAskPriceTopUpdated;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.Executable = !commonCompareQuote.Executable;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.Executable = commonCompareQuote.Executable;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));
    }
}

public static class Level1PriceQuoteTestExtensions
{
    public static Level1PriceQuote CreateLevel1Quote
        (this ISourceTickerInfo tickerId, DateTime sourceTime, decimal mid, decimal openCloseSpread = 0.02m)
    {
        var halfSpread = openCloseSpread / 2;

        var bid1 = mid - halfSpread;
        var ask1 = mid + halfSpread;
        return new Level1PriceQuote
            (tickerId, sourceTime, false, FeedSyncStatus.Good, 0m, sourceTime, sourceTime, sourceTime
           , sourceTime, sourceTime, sourceTime.AddSeconds(10), bid1, false, sourceTime
           , ask1, false, true);
    }
}
