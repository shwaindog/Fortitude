// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

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
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        emptyQuote                = new Level1PriceQuote();
        fullyPopulatedLevel1Quote = new Level1PriceQuote();
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedLevel1Quote, 1);
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(DateTime.MinValue, emptyQuote.SourceTime);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.AreEqual(DateTime.MinValue, emptyQuote.SourceBidTime);
        Assert.AreEqual(0m, emptyQuote.BidPriceTop);
        Assert.AreEqual(DateTime.MinValue, emptyQuote.SourceAskTime);
        Assert.AreEqual(0m, emptyQuote.AskPriceTop);
        Assert.AreEqual(false, emptyQuote.Executable);
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
        var expectedCandle             = new Candle();
        var quoteBehavior              = PublishableQuoteInstantBehaviorFlags.None;

        var fromConstructor =
            new PublishableLevel1PriceQuote
                (sourceTickerInfo, expectedSourceTime, expectedBidPriceTop, expectedAskPriceTop, quoteBehavior
               ,true, true, expectedSourceBidTime, expectedSourceAskTime, expectedSourceTime
               , expectedSourceTime.AddSeconds(2), true, FeedSyncStatus.Good, FeedConnectivityStatusFlags.IsAdapterReplay
               , expectedSingleValue, expectedCandle)
                {
                    ClientReceivedTime = expectedClientReceivedTime,
                    AdapterReceivedTime = expectedAdapterReceiveTime,
                    AdapterSentTime = expectedAdapterSentTime
                };

        Assert.AreSame(sourceTickerInfo, fromConstructor.SourceTickerInfo);
        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(FeedConnectivityStatusFlags.IsAdapterReplay, fromConstructor.FeedMarketConnectivityStatus);
        Assert.AreEqual(expectedSingleValue, fromConstructor.SingleTickValue);
        Assert.AreEqual(expectedClientReceivedTime, fromConstructor.ClientReceivedTime);
        Assert.AreEqual(expectedAdapterReceiveTime, fromConstructor.AdapterReceivedTime);
        Assert.AreEqual(expectedAdapterSentTime, fromConstructor.AdapterSentTime);
        Assert.AreEqual(expectedSourceBidTime, fromConstructor.SourceBidTime);
        Assert.AreEqual(expectedBidPriceTop, fromConstructor.BidPriceTop);
        Assert.AreEqual(true, fromConstructor.IsBidPriceTopChanged);
        Assert.AreEqual(expectedSourceAskTime, fromConstructor.SourceAskTime);
        Assert.AreEqual(expectedAskPriceTop, fromConstructor.AskPriceTop);
        Assert.AreEqual(true, fromConstructor.IsAskPriceTopChanged);
        Assert.AreEqual(true, fromConstructor.Executable);
        Assert.AreEqual(expectedCandle, fromConstructor.ConflatedTicksCandle);
    }

    [TestMethod]
    public void PQCandle_New_ConvertsToCandle()
    {
        var pqCandle      = new PQCandle();
        var quoteBehavior = PublishableQuoteInstantBehaviorFlags.None;

        var nonSourceTickerInfoL1Quote =
            new PublishableLevel1PriceQuote
                (sourceTickerInfo, DateTime.Now, 1m, 1m, quoteBehavior, true, true
               , DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now.AddSeconds(2), true
               , FeedSyncStatus.Good, FeedConnectivityStatusFlags.IsAdapterReplay, 1m, pqCandle);

        Assert.IsInstanceOfType(nonSourceTickerInfoL1Quote.ConflatedTicksCandle, typeof(Candle));
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValues()
    {
        var copyQuote = new Level1PriceQuote(fullyPopulatedLevel1Quote);

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
        var expectedSingleValue        = 1.23456m;
        var expectedSourceBidTime      = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop        = 2.34567m;
        var expectedSourceAskTime      = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop        = 3.45678m;

        emptyQuote.SourceTime           = expectedSourceTime;
        emptyQuote.SingleTickValue      = expectedSingleValue;
        emptyQuote.SourceBidTime        = expectedSourceBidTime;
        emptyQuote.BidPriceTop          = expectedBidPriceTop;
        emptyQuote.IsBidPriceTopChanged = true;
        emptyQuote.SourceAskTime        = expectedSourceAskTime;
        emptyQuote.AskPriceTop          = expectedAskPriceTop;
        emptyQuote.IsAskPriceTopChanged = true;
        emptyQuote.Executable           = true;

        Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
        Assert.AreEqual(expectedSingleValue, emptyQuote.SingleTickValue);
        Assert.AreEqual(expectedSourceBidTime, emptyQuote.SourceBidTime);
        Assert.AreEqual(expectedBidPriceTop, emptyQuote.BidPriceTop);
        Assert.AreEqual(true, emptyQuote.IsBidPriceTopChanged);
        Assert.AreEqual(expectedSourceAskTime, emptyQuote.SourceAskTime);
        Assert.AreEqual(expectedAskPriceTop, emptyQuote.AskPriceTop);
        Assert.AreEqual(true, emptyQuote.IsAskPriceTopChanged);
        Assert.AreEqual(true, emptyQuote.Executable);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new Level1PriceQuote();
        emptyQuote.CopyFrom(fullyPopulatedLevel1Quote);

        Assert.AreEqual(fullyPopulatedLevel1Quote, emptyQuote);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent()
    {
        var emptyLowerLevelQuote = new TickInstant();
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
            (false, fullyPopulatedLevel1Quote, fullyPopulatedLevel1Quote.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        Assert.AreNotEqual(0, fullyPopulatedLevel1Quote.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = fullyPopulatedLevel1Quote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        var q        = fullyPopulatedLevel1Quote;
        var toString = q.ToString();

        Assert.IsTrue(toString.Contains(q.GetType().Name));

        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTime)}: {q.SourceTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SingleTickValue)}: {q.SingleTickValue:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceBidTime)}: {q.SourceBidTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.BidPriceTop)}: {q.BidPriceTop:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsBidPriceTopChanged)}: {q.IsBidPriceTopChanged}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceAskTime)}: {q.SourceAskTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.AskPriceTop)}: {q.AskPriceTop:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsAskPriceTopChanged)}: {q.IsAskPriceTopChanged}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.Executable)}: {q.Executable}"));
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
        (bool exactComparison, IMutableLevel1Quote commonCompareQuote, IMutableLevel1Quote changingQuote)
    {
        TickInstantTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareQuote, changingQuote);


        changingQuote.SourceBidTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceBidTime = commonCompareQuote.SourceBidTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.BidPriceTop = 3.4567m;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.BidPriceTop = commonCompareQuote.BidPriceTop;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.IsBidPriceTopChanged = !commonCompareQuote.IsBidPriceTopChanged;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.IsBidPriceTopChanged = commonCompareQuote.IsBidPriceTopChanged;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SourceAskTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceAskTime = commonCompareQuote.SourceAskTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.AskPriceTop = 3.4567m;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.AskPriceTop = commonCompareQuote.AskPriceTop;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.IsAskPriceTopChanged = !commonCompareQuote.IsAskPriceTopChanged;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.IsAskPriceTopChanged = commonCompareQuote.IsAskPriceTopChanged;
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

        var quoteBehavior = QuoteInstantBehaviorFlags.None;

        var bid1 = mid - halfSpread;
        var ask1 = mid + halfSpread;
        return new Level1PriceQuote(sourceTime, bid1, ask1, quoteBehavior, false, false, sourceTime, sourceTime, sourceTime, sourceTime);
    }
}
