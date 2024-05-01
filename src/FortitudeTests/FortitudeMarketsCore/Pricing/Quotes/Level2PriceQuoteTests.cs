#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

[TestClass]
public class Level2PriceQuoteTests
{
    private IList<Level2PriceQuote> allEmptyQuotes = null!;

    private IList<Level2PriceQuote> allFullyPopulatedQuotes = null!;
    private Level2PriceQuote everyLayerEmptyLevel2Quote = null!;
    private Level2PriceQuote everyLayerFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo everyLayerSourceTickerQuoteInfo = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private Level2PriceQuote simpleEmptyLevel2Quote = null!;
    private Level2PriceQuote simpleFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo simpleSourceTickerQuoteInfo = null!;
    private Level2PriceQuote sourceNameEmptyLevel2Quote = null!;
    private Level2PriceQuote sourceNameFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo sourceNameSourceTickerQuoteInfo = null!;
    private Level2PriceQuote sourceQuoteRefEmptyLevel2Quote = null!;
    private Level2PriceQuote sourceQuoteRefFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo sourceQuoteRefSourceTickerQuoteInfo = null!;
    private DateTime testDateTime;
    private Level2PriceQuote traderDetailsEmptyLevel2Quote = null!;
    private Level2PriceQuote traderDetailsFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo traderDetailsSourceTickerQuoteInfo = null!;
    private Level2PriceQuote valueDateEmptyLevel2Quote = null!;
    private Level2PriceQuote valueDateFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo valueDateSourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        simpleSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", QuoteLevel.Level3, 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                                  LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        sourceNameSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", QuoteLevel.Level3, 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceName, LastTradedFlags.PaidOrGiven |
                                                                          LastTradedFlags.TraderName |
                                                                          LastTradedFlags.LastTradedVolume |
                                                                          LastTradedFlags.LastTradedTime);
        sourceQuoteRefSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", QuoteLevel.Level3, 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceQuoteReference, LastTradedFlags.PaidOrGiven |
                                                                                    LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                                                                    LastTradedFlags.LastTradedTime);
        traderDetailsSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", QuoteLevel.Level3, 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize |
            LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                    LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        valueDateSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", QuoteLevel.Level3, 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.ValueDate, LastTradedFlags.PaidOrGiven |
                                                                         LastTradedFlags.TraderName |
                                                                         LastTradedFlags.LastTradedVolume |
                                                                         LastTradedFlags.LastTradedTime);
        everyLayerSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", QuoteLevel.Level3, 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume.AllFlags(), LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                          LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        simpleEmptyLevel2Quote = new Level2PriceQuote(simpleSourceTickerQuoteInfo);
        simpleFullyPopulatedLevel2Quote = new Level2PriceQuote(simpleSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleFullyPopulatedLevel2Quote, 1);
        sourceNameEmptyLevel2Quote = new Level2PriceQuote(sourceNameSourceTickerQuoteInfo);
        sourceNameFullyPopulatedLevel2Quote = new Level2PriceQuote(sourceNameSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceNameFullyPopulatedLevel2Quote, 2);
        sourceQuoteRefEmptyLevel2Quote = new Level2PriceQuote(sourceQuoteRefSourceTickerQuoteInfo);
        sourceQuoteRefFullyPopulatedLevel2Quote = new Level2PriceQuote(sourceQuoteRefSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceQuoteRefFullyPopulatedLevel2Quote, 3);
        traderDetailsEmptyLevel2Quote = new Level2PriceQuote(traderDetailsSourceTickerQuoteInfo);
        traderDetailsFullyPopulatedLevel2Quote = new Level2PriceQuote(traderDetailsSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(traderDetailsFullyPopulatedLevel2Quote, 4);
        valueDateEmptyLevel2Quote = new Level2PriceQuote(valueDateSourceTickerQuoteInfo);
        valueDateFullyPopulatedLevel2Quote = new Level2PriceQuote(valueDateSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateFullyPopulatedLevel2Quote, 5);
        everyLayerEmptyLevel2Quote = new Level2PriceQuote(everyLayerSourceTickerQuoteInfo);
        everyLayerFullyPopulatedLevel2Quote = new Level2PriceQuote(everyLayerSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(everyLayerFullyPopulatedLevel2Quote, 5);

        allFullyPopulatedQuotes = new List<Level2PriceQuote>
        {
            simpleFullyPopulatedLevel2Quote, sourceNameFullyPopulatedLevel2Quote
            , sourceQuoteRefFullyPopulatedLevel2Quote, traderDetailsFullyPopulatedLevel2Quote
            , valueDateFullyPopulatedLevel2Quote, everyLayerFullyPopulatedLevel2Quote
        };
        allEmptyQuotes = new List<Level2PriceQuote>
        {
            simpleEmptyLevel2Quote, sourceNameEmptyLevel2Quote, sourceQuoteRefEmptyLevel2Quote
            , traderDetailsEmptyLevel2Quote, valueDateEmptyLevel2Quote, everyLayerEmptyLevel2Quote
        };

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreSame(simpleSourceTickerQuoteInfo, simpleEmptyLevel2Quote.SourceTickerQuoteInfo);
        Assert.AreSame(sourceNameSourceTickerQuoteInfo, sourceNameEmptyLevel2Quote.SourceTickerQuoteInfo);
        Assert.AreSame(sourceQuoteRefSourceTickerQuoteInfo, sourceQuoteRefEmptyLevel2Quote.SourceTickerQuoteInfo);
        Assert.AreSame(valueDateSourceTickerQuoteInfo, valueDateEmptyLevel2Quote.SourceTickerQuoteInfo);
        Assert.AreSame(traderDetailsSourceTickerQuoteInfo, traderDetailsEmptyLevel2Quote.SourceTickerQuoteInfo);
        Assert.AreSame(everyLayerSourceTickerQuoteInfo, everyLayerEmptyLevel2Quote.SourceTickerQuoteInfo);
        foreach (var emptyL2Quote in allEmptyQuotes)
        {
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.SourceTime);
            Assert.AreEqual(false, emptyL2Quote.IsReplay);
            Assert.AreEqual(0m, emptyL2Quote.SinglePrice);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.ClientReceivedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.AdapterReceivedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.AdapterSentTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.SourceBidTime);
            Assert.AreEqual(0m, emptyL2Quote.BidPriceTop);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyL2Quote.SourceAskTime);
            Assert.AreEqual(0m, emptyL2Quote.AskPriceTop);
            Assert.AreEqual(false, emptyL2Quote.Executable);
            Assert.IsNull(emptyL2Quote.PeriodSummary);
            Assert.AreEqual(new OrderBook(BookSide.BidBook, emptyL2Quote.SourceTickerQuoteInfo!), emptyL2Quote.BidBook);
            Assert.AreEqual(new OrderBook(BookSide.AskBook, emptyL2Quote.SourceTickerQuoteInfo!), emptyL2Quote.AskBook);
            Assert.IsFalse(emptyL2Quote.IsBidBookChanged);
            Assert.IsFalse(emptyL2Quote.IsAskBookChanged);
        }
    }

    [TestMethod]
    public void IntializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedSourceTime = new DateTime(2018, 02, 04, 23, 56, 59);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSinglePrice = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop = 2.34567m;
        var expectedSourceAskTime = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop = 3.45678m;
        var expectedPeriodSummary = new PeriodSummary();
        var expectedBidBook = new OrderBook(BookSide.BidBook, simpleSourceTickerQuoteInfo)
        {
            [0] = new PriceVolumeLayer(expectedBidPriceTop, 1_000_000)
        };
        var expectedAskBook = new OrderBook(BookSide.AskBook, simpleSourceTickerQuoteInfo)
        {
            [0] = new PriceVolumeLayer(expectedAskPriceTop, 1_000_000)
        };

        var fromConstructor = new Level2PriceQuote(simpleSourceTickerQuoteInfo, expectedSourceTime, true,
            expectedSinglePrice, expectedClientReceivedTime, expectedAdapterReceiveTime, expectedAdapterSentTime,
            expectedSourceBidTime, true, expectedSourceAskTime,
            true, true, expectedPeriodSummary, expectedBidBook, true, expectedAskBook, true);

        Assert.AreSame(simpleSourceTickerQuoteInfo, fromConstructor.SourceTickerQuoteInfo);
        Assert.AreEqual(expectedSourceTime, fromConstructor.SourceTime);
        Assert.AreEqual(true, fromConstructor.IsReplay);
        Assert.AreEqual(expectedSinglePrice, fromConstructor.SinglePrice);
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
        Assert.AreEqual(expectedPeriodSummary, fromConstructor.PeriodSummary);
        Assert.AreEqual(expectedBidBook, fromConstructor.BidBook);
        Assert.AreEqual(expectedAskBook, fromConstructor.AskBook);
        Assert.IsTrue(fromConstructor.IsBidBookChanged);
        Assert.IsTrue(fromConstructor.IsAskBookChanged);
    }

    [TestMethod]
    public void NonOrderBooks_New_ConvertsToOrderBook()
    {
        var expectedSourceTime = new DateTime(2018, 02, 04, 18, 56, 9);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSinglePrice = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop = 2.34567m;
        var expectedSourceAskTime = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop = 3.45678m;
        var expectedPeriodSummary = new PeriodSummary();
        var convertedBidBook = new PQOrderBook(BookSide.BidBook, new PQSourceTickerQuoteInfo(simpleSourceTickerQuoteInfo))
        {
            [0] = new PQPriceVolumeLayer(expectedBidPriceTop, 1_000_000)
        };
        var convertedAskBook = new PQOrderBook(BookSide.AskBook, new PQSourceTickerQuoteInfo(simpleSourceTickerQuoteInfo))
        {
            [0] = new PQPriceVolumeLayer(expectedAskPriceTop, 1_000_000)
        };
        var expectedBidBook = new OrderBook(convertedBidBook);
        var expectedAskBook = new OrderBook(convertedAskBook);

        var fromConstructor = new Level2PriceQuote(simpleSourceTickerQuoteInfo, expectedSourceTime, true,
            expectedSinglePrice, expectedClientReceivedTime, expectedAdapterReceiveTime, expectedAdapterSentTime,
            expectedSourceBidTime, true, expectedSourceAskTime,
            true, true, expectedPeriodSummary, convertedBidBook, true, convertedAskBook, true);

        Assert.AreEqual(expectedBidBook, fromConstructor.BidBook);
        Assert.AreEqual(expectedAskBook, fromConstructor.AskBook);
    }

    [TestMethod]
    public void SimpleLevel2Quote_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(PriceVolumeLayer), simpleEmptyLevel2Quote,
            simpleFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void SourceNameLevel2Quote_New_BuildsSourcePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(SourcePriceVolumeLayer), sourceNameEmptyLevel2Quote,
            sourceNameFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void SourceQuoteRefLevel2Quote_New_BuildsSourceQuoteRefPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(SourceQuoteRefPriceVolumeLayer), sourceQuoteRefEmptyLevel2Quote,
            sourceQuoteRefFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void TraderLevel2Quote_New_BuildsTraderPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(TraderPriceVolumeLayer), traderDetailsEmptyLevel2Quote,
            traderDetailsFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void ValueDateLevel2Quote_New_BuildsValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(ValueDatePriceVolumeLayer), valueDateEmptyLevel2Quote,
            valueDateFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void EveryLayerLevel2Quote_New_BuildsSourceQuoteRefTraderValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer),
            everyLayerEmptyLevel2Quote, everyLayerFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void PopulatedQuote_New_CopiesValues()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var copyQuote = new Level2PriceQuote(populatedQuote);
            Assert.AreEqual(populatedQuote, copyQuote);
        }
    }

    [TestMethod]
    public void NonOrderBookPopulatedQuote_New_CopiesValuesConvertsOrderBook()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var originalBidBook = populatedQuote.BidBook;
            var originalAskBook = populatedQuote.AskBook;
            var pqBidBook = new PQOrderBook(originalBidBook);
            var pqAskBook = new PQOrderBook(originalAskBook);
            populatedQuote.BidBook = pqBidBook;
            populatedQuote.AskBook = pqAskBook;

            var copyQuote = new Level2PriceQuote(populatedQuote);
            Assert.AreNotEqual(populatedQuote, copyQuote);
            Assert.IsTrue(populatedQuote.AreEquivalent(copyQuote));
            Assert.IsTrue(copyQuote.AreEquivalent(populatedQuote));

            populatedQuote.BidBook = originalBidBook;
            populatedQuote.AskBook = originalAskBook;
        }
    }

    [TestMethod]
    public void PopulatedQuote_Mutate_UpdatesFields()
    {
        var expectedSourceTime = new DateTime(2018, 02, 04, 23, 56, 59);
        var expectedClientReceivedTime = new DateTime(2018, 02, 04, 19, 56, 9);
        var expectedSinglePrice = 1.23456m;
        var expectedAdapterReceiveTime = new DateTime(2018, 02, 04, 20, 56, 9);
        var expectedAdapterSentTime = new DateTime(2018, 02, 04, 21, 56, 9);
        var expectedSourceBidTime = new DateTime(2018, 02, 04, 22, 56, 9);
        var expectedBidPriceTop = 2.34567m;
        var expectedSourceAskTime = new DateTime(2018, 02, 04, 23, 56, 9);
        var expectedAskPriceTop = 3.45678m;
        var expectedPeriodSummary = new PeriodSummary();

        foreach (var emptyQuote in allEmptyQuotes)
        {
            var expectedBidOrderBook = emptyQuote.BidBook.Clone();
            expectedBidOrderBook[0]!.Price = expectedBidPriceTop;
            var expectedAskOrderBook = emptyQuote.AskBook.Clone();
            expectedAskOrderBook[0]!.Price = expectedAskPriceTop;

            emptyQuote.SourceTime = expectedSourceTime;
            emptyQuote.IsReplay = true;
            emptyQuote.SinglePrice = expectedSinglePrice;
            emptyQuote.ClientReceivedTime = expectedClientReceivedTime;
            emptyQuote.AdapterReceivedTime = expectedAdapterReceiveTime;
            emptyQuote.AdapterSentTime = expectedAdapterSentTime;
            emptyQuote.SourceBidTime = expectedSourceBidTime;
            emptyQuote.BidPriceTop = expectedBidPriceTop;
            emptyQuote.IsBidPriceTopUpdated = true;
            emptyQuote.SourceAskTime = expectedSourceAskTime;
            emptyQuote.AskPriceTop = expectedAskPriceTop;
            emptyQuote.IsAskPriceTopUpdated = true;
            emptyQuote.Executable = true;
            emptyQuote.PeriodSummary = expectedPeriodSummary;
            emptyQuote.BidBook = expectedBidOrderBook;
            emptyQuote.IsBidBookChanged = true;
            emptyQuote.AskBook = expectedAskOrderBook;
            emptyQuote.IsAskBookChanged = true;

            Assert.AreEqual(expectedSourceTime, emptyQuote.SourceTime);
            Assert.AreEqual(true, emptyQuote.IsReplay);
            Assert.AreEqual(expectedSinglePrice, emptyQuote.SinglePrice);
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
            Assert.AreEqual(expectedPeriodSummary, emptyQuote.PeriodSummary);
            Assert.AreSame(expectedBidOrderBook, emptyQuote.BidBook);
            Assert.AreEqual(true, emptyQuote.IsBidBookChanged);
            Assert.AreSame(expectedAskOrderBook, emptyQuote.AskBook);
            Assert.AreEqual(true, emptyQuote.IsAskBookChanged);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var emptyQuote = new Level2PriceQuote(fullyPopulatedQuote.SourceTickerQuoteInfo!);
            emptyQuote.CopyFrom(fullyPopulatedQuote);

            Assert.AreEqual(fullyPopulatedQuote, emptyQuote);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var emptyLowerLevelQuote = new Level1PriceQuote(fullyPopulatedQuote.SourceTickerQuoteInfo!);
            emptyLowerLevelQuote.CopyFrom(fullyPopulatedQuote);

            Assert.AreNotEqual(fullyPopulatedQuote, emptyLowerLevelQuote);
            Assert.IsTrue(emptyLowerLevelQuote.AreEquivalent(fullyPopulatedQuote));
        }
    }

    [TestMethod]
    public void PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedQuotes)
        {
            var pqLevel2Quote = new PQLevel2Quote(fullyPopulatedQuote);
            var newEmpty = new Level2PriceQuote(fullyPopulatedQuote.SourceTickerQuoteInfo!);
            newEmpty.CopyFrom(pqLevel2Quote);
            Assert.IsTrue(newEmpty.AreEquivalent(pqLevel2Quote));
        }
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var clone = populatedQuote.Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableLevel0Quote)((ICloneable<ILevel0Quote>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableLevel0Quote)((ILevel0Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutableLevel0Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableLevel0Quote)((ICloneable<ILevel1Quote>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableLevel0Quote)((ILevel1Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutableLevel1Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableLevel0Quote)((ICloneable<ILevel2Quote>)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = (IMutableLevel0Quote)((ILevel2Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
            clone = ((IMutableLevel2Quote)populatedQuote).Clone();
            Assert.AreNotSame(clone, populatedQuote);
            Assert.AreEqual(populatedQuote, clone);
        }
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedQuote,
                (IMutableLevel2Quote)populatedQuote.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes) Assert.AreNotEqual(0, populatedQuote.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            var q = populatedQuote;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));

            Assert.IsTrue(toString.Contains($"{nameof(q.SourceTickerQuoteInfo)}: {q.SourceTickerQuoteInfo}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SourceTime)}: {q.SourceTime:O}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsReplay)}: {q.IsReplay}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.SinglePrice)}: {q.SinglePrice:N5}"));
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
            Assert.IsTrue(toString.Contains($"{nameof(q.PeriodSummary)}: {q.PeriodSummary}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.BidBook)}: {q.BidBook}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsBidBookChanged)}: {q.IsBidBookChanged}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AskBook)}: {q.AskBook}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsAskBookChanged)}: {q.IsAskBookChanged}"));
        }
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IMutableLevel2Quote commonCompareQuote, IMutableLevel2Quote changingQuote)
    {
        Level1PriceQuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            commonCompareQuote, changingQuote);

        OrderBookTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            (OrderBook)commonCompareQuote.BidBook, (OrderBook)changingQuote.BidBook,
            commonCompareQuote, changingQuote);

        OrderBookTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            (OrderBook)commonCompareQuote.AskBook, (OrderBook)changingQuote.AskBook,
            commonCompareQuote, changingQuote);
    }

    private void AssertLayerTypeIsExpected(Type expectedType, params Level2PriceQuote[] quotesToCheck)
    {
        foreach (var level2Quote in quotesToCheck)
            for (var i = 0; i < level2Quote.SourceTickerQuoteInfo!.MaximumPublishedLayers; i++)
            {
                Assert.AreEqual(expectedType, level2Quote.BidBook[i]?.GetType(),
                    $"BidBook[{i}] expectedType: {expectedType.Name} " +
                    $"actualType: {level2Quote.BidBook[i]?.GetType()?.Name ?? "null"}");
                Assert.AreEqual(expectedType, level2Quote.AskBook[i]?.GetType(),
                    $"BidBook[{i}] expectedType: {expectedType.Name} " +
                    $"actualType: {level2Quote.BidBook[i]?.GetType()?.Name ?? "null"}");
            }
    }
}
