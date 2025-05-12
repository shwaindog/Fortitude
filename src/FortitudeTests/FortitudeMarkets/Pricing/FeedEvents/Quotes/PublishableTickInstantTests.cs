// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

[TestClass]
public class PublishableTickInstantTests
{
    private PublishableTickInstant emptyQuote                = null!;
    private PublishableTickInstant fullyPopulatedTickInstant = null!;
    private PublishableTickInstant newlyPopulatedTickInstant = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private ISourceTickerInfo sourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        sourceTickerInfo = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", SingleValue, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
        emptyQuote                = new PublishableTickInstant(sourceTickerInfo);
        fullyPopulatedTickInstant = new PublishableTickInstant(sourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedTickInstant, 1);
        newlyPopulatedTickInstant = new PublishableTickInstant(sourceTickerInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(newlyPopulatedTickInstant, 2);
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreSame(sourceTickerInfo, emptyQuote.SourceTickerInfo);
        Assert.AreEqual(DateTime.MinValue, emptyQuote.SourceTime);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
        Assert.AreEqual(DateTime.MinValue, emptyQuote.ClientReceivedTime);
    }

    [TestMethod]
    public void IntializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);

        var expectedSinglePrice = 1.23456m;

        var fromConstructor = new PublishableTickInstant
            (sourceTickerInfo, expectedSourceTime, true, FeedSyncStatus.Good, expectedSinglePrice, expectedClientReceivedTime);

        Assert.AreSame(sourceTickerInfo, fromConstructor.SourceTickerInfo);
        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(true, fromConstructor.IsReplay);
        Assert.AreEqual(expectedSinglePrice, fromConstructor.SingleTickValue);
        Assert.AreEqual(expectedClientReceivedTime, fromConstructor.ClientReceivedTime);
    }

    [TestMethod]
    public void NonSourceTickerInfo_New_ConvertsToSourceTickerInfo()
    {
        var pqSourceTickerInfo = new PQSourceTickerInfo(sourceTickerInfo);

        var nonSourceTickerInfoTickInstant = new PublishableTickInstant(pqSourceTickerInfo);

        Assert.IsInstanceOfType(nonSourceTickerInfoTickInstant.SourceTickerInfo, typeof(SourceTickerInfo));
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValuesExceptQuoteInfo()
    {
        var copyQuote = new PublishableTickInstant(fullyPopulatedTickInstant);

        Assert.AreEqual(fullyPopulatedTickInstant, copyQuote);
    }

    [TestMethod]
    public void NonSourceTickerInfo_New_CopiesExceptSourceTickerInfoIsConverted()
    {
        var pqSourceTickerInfo = new PQSourceTickerInfo(sourceTickerInfo);
        fullyPopulatedTickInstant.SourceTickerInfo = pqSourceTickerInfo;
        var copyQuote = new PublishableTickInstant(fullyPopulatedTickInstant);
        Assert.AreNotEqual(fullyPopulatedTickInstant, copyQuote);
        fullyPopulatedTickInstant.SourceTickerInfo = sourceTickerInfo;
        Assert.AreEqual(fullyPopulatedTickInstant, copyQuote);
    }

    [TestMethod]
    public void EmptyQuote_Mutate_UpdatesFields()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);

        var expectedSingleValue = 1.23456m;

        emptyQuote.IsReplay           = true;
        emptyQuote.SourceTime         = expectedSourceTime;
        emptyQuote.ClientReceivedTime = expectedClientReceivedTime;
        emptyQuote.SingleTickValue    = expectedSingleValue;

        Assert.AreSame(sourceTickerInfo, emptyQuote.SourceTickerInfo);
        Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
        Assert.AreEqual(true, emptyQuote.IsReplay);
        Assert.AreEqual(expectedSingleValue, emptyQuote.SingleTickValue);
        Assert.AreEqual(expectedClientReceivedTime, emptyQuote.ClientReceivedTime);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new PublishableTickInstant(sourceTickerInfo);
        emptyQuote.CopyFrom(fullyPopulatedTickInstant);

        Assert.AreEqual(fullyPopulatedTickInstant, emptyQuote);
    }

    [TestMethod]
    public void PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var pqTickInstant = new PQPublishableTickInstant(fullyPopulatedTickInstant);
        emptyQuote.CopyFrom(fullyPopulatedTickInstant);
        Assert.IsTrue(emptyQuote.AreEquivalent(pqTickInstant));
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        IMutablePublishableTickInstant clone = fullyPopulatedTickInstant.Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
        clone = (IMutablePublishableTickInstant)((ICloneable<IPublishableTickInstant>)fullyPopulatedTickInstant).Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
        clone = (IMutablePublishableTickInstant)((IPublishableTickInstant)fullyPopulatedTickInstant).Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
        clone = ((IMutablePublishableTickInstant)fullyPopulatedTickInstant).Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, fullyPopulatedTickInstant, fullyPopulatedTickInstant.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        Assert.AreNotEqual(0, fullyPopulatedTickInstant.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        var q        = fullyPopulatedTickInstant;
        var toString = q.ToString();

        Assert.IsTrue(toString.Contains(q.GetType().Name));

        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTickerInfo)}: {q.SourceTickerInfo}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTime)}: {q.SourceTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsReplay)}: {q.IsReplay}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SingleTickValue)}: {q.SingleTickValue:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.ClientReceivedTime)}: {q.ClientReceivedTime:O}"));
    }

    [TestMethod]
    public void FullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = fullyPopulatedTickInstant;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutablePublishableTickInstant commonCompareQuote, IMutablePublishableTickInstant changingQuote)
    {
        var diffSrcTkrInfo = commonCompareQuote.SourceTickerInfo!.Clone();
        diffSrcTkrInfo.SourceName      = "DifferSourceName";
        changingQuote.SourceTickerInfo = diffSrcTkrInfo;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceTickerInfo.SourceName = commonCompareQuote.SourceTickerInfo.SourceName;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SourceTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SourceTime = commonCompareQuote.SourceTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.IsReplay = !commonCompareQuote.IsReplay;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.IsReplay = commonCompareQuote.IsReplay;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.SingleTickValue = 3.4567m;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.SingleTickValue = commonCompareQuote.SingleTickValue;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));

        changingQuote.ClientReceivedTime = DateTime.Now;
        Assert.IsFalse(commonCompareQuote.AreEquivalent(changingQuote));
        changingQuote.ClientReceivedTime = commonCompareQuote.ClientReceivedTime;
        Assert.IsTrue(commonCompareQuote.AreEquivalent(changingQuote));
    }
}
