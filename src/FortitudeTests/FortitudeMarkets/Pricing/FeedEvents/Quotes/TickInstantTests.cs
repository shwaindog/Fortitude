// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

[TestClass]
public class TickInstantTests
{
    private TickInstant emptyQuote                = null!;
    private TickInstant fullyPopulatedTickInstant = null!;
    private TickInstant newlyPopulatedTickInstant = null!;

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
        emptyQuote                = new TickInstant();
        fullyPopulatedTickInstant = new TickInstant();
        quoteSequencedTestDataBuilder.InitializeQuote(fullyPopulatedTickInstant, 1);
        newlyPopulatedTickInstant = new TickInstant();
        quoteSequencedTestDataBuilder.InitializeQuote(newlyPopulatedTickInstant, 2);
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(DateTime.MinValue, emptyQuote.SourceTime);
        Assert.AreEqual(false, emptyQuote.IsReplay);
        Assert.AreEqual(0m, emptyQuote.SingleTickValue);
    }

    [TestMethod]
    public void IntializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);

        var expectedSinglePrice = 1.23456m;

        var fromConstructor = new TickInstant( expectedSinglePrice, true, expectedSourceTime);

        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(true, fromConstructor.IsReplay);
        Assert.AreEqual(expectedSinglePrice, fromConstructor.SingleTickValue);
    }


    [TestMethod]
    public void PopulatedQuote_New_CopiesValuesExceptQuoteInfo()
    {
        var copyQuote = new TickInstant(fullyPopulatedTickInstant);

        Assert.AreEqual(fullyPopulatedTickInstant, copyQuote);
    }


    [TestMethod]
    public void EmptyQuote_Mutate_UpdatesFields()
    {
        var expectedSourceTime         = new DateTime(2018, 02, 04, 18, 56, 9);

        var expectedSingleValue = 1.23456m;

        emptyQuote.IsReplay           = true;
        emptyQuote.SourceTime         = expectedSourceTime;
        emptyQuote.SingleTickValue    = expectedSingleValue;

        Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
        Assert.AreEqual(true, emptyQuote.IsReplay);
        Assert.AreEqual(expectedSingleValue, emptyQuote.SingleTickValue);
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptyQuote = new TickInstant();
        emptyQuote.CopyFrom(fullyPopulatedTickInstant);

        Assert.AreEqual(fullyPopulatedTickInstant, emptyQuote);
    }

    [TestMethod]
    public void PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        var pqTickInstant = new PQTickInstant(fullyPopulatedTickInstant);
        emptyQuote.CopyFrom(fullyPopulatedTickInstant);
        Assert.IsTrue(emptyQuote.AreEquivalent(pqTickInstant));
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        IMutableTickInstant clone = fullyPopulatedTickInstant.Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
        clone = (IMutableTickInstant)((ICloneable<ITickInstant>)fullyPopulatedTickInstant).Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
        clone = (IMutableTickInstant)((ITickInstant)fullyPopulatedTickInstant).Clone();
        Assert.AreNotSame(clone, fullyPopulatedTickInstant);
        Assert.AreEqual(fullyPopulatedTickInstant, clone);
        clone = ((IMutableTickInstant)fullyPopulatedTickInstant).Clone();
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

        Assert.IsTrue(toString.Contains($"{nameof(q.SourceTime)}: {q.SourceTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.IsReplay)}: {q.IsReplay}"));
        Assert.IsTrue(toString.Contains($"{nameof(q.SingleTickValue)}: {q.SingleTickValue:N5}"));
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
        IMutableTickInstant commonCompareQuote, IMutableTickInstant changingQuote)
    {
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
    }
}
