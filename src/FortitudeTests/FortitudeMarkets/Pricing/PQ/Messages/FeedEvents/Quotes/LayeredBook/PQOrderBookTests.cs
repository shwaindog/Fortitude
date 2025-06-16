using System.Text.Json;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;

[TestClass]
public class PQOrderBookTests
{
    private const QuoteInstantBehaviorFlags QuoteBehavior = QuoteInstantBehaviorFlags.None;

    private IList<PQOrderBook> allEmptyOrderBooks          = null!;
    private IList<PQOrderBook> allFullyPopulatedOrderBooks = null!;

    private PQOrderBook fullSupportEmptyOrderBook          = null!;
    private PQOrderBook fullSupportFullyPopulatedOrderBook = null!;

    private IPQSourceTickerInfo fullSupportSourceTickerInfo = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private PQOrderBook simpleEmptyOrderBook          = null!;
    private PQOrderBook simpleFullyPopulatedOrderBook = null!;

    private IPQSourceTickerInfo simpleSourceTickerInfo = null!;

    private PQOrderBook sourceNameEmptyOrderBook          = null!;
    private PQOrderBook sourceNameFullyPopulatedOrderBook = null!;

    private IPQSourceTickerInfo sourceNameSourceTickerInfo = null!;

    private PQOrderBook sourceQuoteRefEmptyOrderBook          = null!;
    private PQOrderBook sourceQuoteRefFullyPopulatedOrderBook = null!;

    private IPQSourceTickerInfo sourceRefSourceTickerInfo = null!;

    private PQOrderBook orderCountEmptyOrderBook          = null!;
    private PQOrderBook orderCountFullyPopulatedOrderBook = null!;

    private IPQSourceTickerInfo orderCountSourceTickerInfo = null!;

    private PQOrderBook ordersEmptyOrderBook          = null!;
    private PQOrderBook ordersFullyPopulatedOrderBook = null!;

    private IPQSourceTickerInfo ordersCounterPartySourceTickerInfo = null!;

    private PQOrderBook valueDateEmptyOrderBook          = null!;
    private PQOrderBook valueDateFullyPopulatedOrderBook = null!;

    private IPQSourceTickerInfo valueDateSourceTickerInfo = null!;

    private static DateTime testDateTime = new(2025, 5, 7, 18, 33, 24);

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        simpleSourceTickerInfo             = PQSourceTickerInfoTests.SimpleL2PriceVolumeSti;
        sourceNameSourceTickerInfo         = PQSourceTickerInfoTests.SourceNameL2PriceVolumeSti;
        sourceRefSourceTickerInfo          = PQSourceTickerInfoTests.SourceQuoteRefL2PriceVolumeSti;
        orderCountSourceTickerInfo         = PQSourceTickerInfoTests.OrdersCountL3JustTradeTradeSti;
        ordersCounterPartySourceTickerInfo = PQSourceTickerInfoTests.OrdersCounterPartyL2PriceVolumeSti;
        valueDateSourceTickerInfo          = PQSourceTickerInfoTests.ValueDateL2PriceVolumeSti;
        fullSupportSourceTickerInfo        = PQSourceTickerInfoTests.FullSupportL2PriceVolumeSti;

        simpleEmptyOrderBook          = new PQOrderBook(simpleSourceTickerInfo);
        simpleFullyPopulatedOrderBook = new PQOrderBook(simpleSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(simpleFullyPopulatedOrderBook, 1);
        sourceNameEmptyOrderBook          = new PQOrderBook(sourceNameSourceTickerInfo);
        sourceNameFullyPopulatedOrderBook = new PQOrderBook(sourceNameSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(sourceNameFullyPopulatedOrderBook, 2);
        sourceQuoteRefEmptyOrderBook          = new PQOrderBook(sourceRefSourceTickerInfo);
        sourceQuoteRefFullyPopulatedOrderBook = new PQOrderBook(sourceRefSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(sourceQuoteRefFullyPopulatedOrderBook, 3);
        orderCountEmptyOrderBook          = new PQOrderBook(orderCountSourceTickerInfo);
        orderCountFullyPopulatedOrderBook = new PQOrderBook(orderCountSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(orderCountFullyPopulatedOrderBook, 4);
        ordersEmptyOrderBook          = new PQOrderBook(ordersCounterPartySourceTickerInfo);
        ordersFullyPopulatedOrderBook = new PQOrderBook(ordersCounterPartySourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(ordersFullyPopulatedOrderBook, 5);
        valueDateEmptyOrderBook          = new PQOrderBook(valueDateSourceTickerInfo);
        valueDateFullyPopulatedOrderBook = new PQOrderBook(valueDateSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(valueDateFullyPopulatedOrderBook, 6);
        fullSupportEmptyOrderBook          = new PQOrderBook(fullSupportSourceTickerInfo);
        fullSupportFullyPopulatedOrderBook = new PQOrderBook(fullSupportSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(fullSupportFullyPopulatedOrderBook, 7);

        allFullyPopulatedOrderBooks = new List<PQOrderBook>
        {
            simpleFullyPopulatedOrderBook, sourceNameFullyPopulatedOrderBook
          , sourceQuoteRefFullyPopulatedOrderBook, ordersFullyPopulatedOrderBook
          , orderCountFullyPopulatedOrderBook, valueDateFullyPopulatedOrderBook
          , fullSupportFullyPopulatedOrderBook
        };
        allEmptyOrderBooks = new List<PQOrderBook>
        {
            simpleEmptyOrderBook, sourceNameEmptyOrderBook, sourceQuoteRefEmptyOrderBook
          , orderCountEmptyOrderBook, ordersEmptyOrderBook, valueDateEmptyOrderBook
          , fullSupportEmptyOrderBook
        };
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(simpleSourceTickerInfo.MaximumPublishedLayers, simpleEmptyOrderBook.MaxAllowedSize);
        Assert.IsTrue(simpleEmptyOrderBook.LayerSupportedFlags.HasAllOf(simpleSourceTickerInfo.LayerFlags));
        Assert.AreEqual(simpleEmptyOrderBook.BidSide
                      , new PQOrderBookSide(BookSide.BidBook, simpleSourceTickerInfo, simpleEmptyOrderBook.NameIdLookup));
        Assert.AreEqual(simpleEmptyOrderBook.AskSide
                      , new PQOrderBookSide(BookSide.AskBook, simpleSourceTickerInfo, simpleEmptyOrderBook.NameIdLookup));

        Assert.AreEqual(sourceNameSourceTickerInfo.MaximumPublishedLayers, sourceNameEmptyOrderBook.MaxAllowedSize);
        Assert.IsTrue(sourceNameEmptyOrderBook.LayerSupportedFlags.HasAllOf(sourceNameSourceTickerInfo.LayerFlags));
        Assert.AreEqual(sourceNameEmptyOrderBook.BidSide
                      , new PQOrderBookSide(BookSide.BidBook, sourceNameSourceTickerInfo, sourceNameEmptyOrderBook.NameIdLookup));
        Assert.AreEqual(sourceNameEmptyOrderBook.AskSide
                      , new PQOrderBookSide(BookSide.AskBook, sourceNameSourceTickerInfo, sourceNameEmptyOrderBook.NameIdLookup));

        Assert.AreEqual(sourceRefSourceTickerInfo.MaximumPublishedLayers, sourceQuoteRefEmptyOrderBook.MaxAllowedSize);
        Assert.IsTrue(sourceQuoteRefEmptyOrderBook.LayerSupportedFlags.HasAllOf(sourceRefSourceTickerInfo.LayerFlags));
        Assert.AreEqual(sourceQuoteRefEmptyOrderBook.BidSide
                      , new PQOrderBookSide(BookSide.BidBook, sourceRefSourceTickerInfo, sourceQuoteRefEmptyOrderBook.NameIdLookup));
        Assert.AreEqual(sourceQuoteRefEmptyOrderBook.AskSide
                      , new PQOrderBookSide(BookSide.AskBook, sourceRefSourceTickerInfo, sourceQuoteRefEmptyOrderBook.NameIdLookup));

        Assert.AreEqual(valueDateSourceTickerInfo.MaximumPublishedLayers, valueDateEmptyOrderBook.MaxAllowedSize);
        Assert.IsTrue(valueDateEmptyOrderBook.LayerSupportedFlags.HasAllOf(valueDateSourceTickerInfo.LayerFlags));
        Assert.AreEqual(valueDateEmptyOrderBook.BidSide
                      , new PQOrderBookSide(BookSide.BidBook, valueDateSourceTickerInfo, valueDateEmptyOrderBook.NameIdLookup));
        Assert.AreEqual(valueDateEmptyOrderBook.AskSide
                      , new PQOrderBookSide(BookSide.AskBook, valueDateSourceTickerInfo, valueDateEmptyOrderBook.NameIdLookup));

        Assert.AreEqual(orderCountSourceTickerInfo.MaximumPublishedLayers, orderCountEmptyOrderBook.MaxAllowedSize);
        Assert.IsTrue(orderCountEmptyOrderBook.LayerSupportedFlags.HasAllOf(orderCountSourceTickerInfo.LayerFlags));
        Assert.AreEqual(orderCountEmptyOrderBook.BidSide
                      , new PQOrderBookSide(BookSide.BidBook, orderCountSourceTickerInfo, orderCountEmptyOrderBook.NameIdLookup));
        Assert.AreEqual(orderCountEmptyOrderBook.AskSide
                      , new PQOrderBookSide(BookSide.AskBook, orderCountSourceTickerInfo, orderCountEmptyOrderBook.NameIdLookup));

        Assert.AreEqual(ordersCounterPartySourceTickerInfo.MaximumPublishedLayers, ordersEmptyOrderBook.MaxAllowedSize);
        Assert.IsTrue(ordersEmptyOrderBook.LayerSupportedFlags.HasAllOf(ordersCounterPartySourceTickerInfo.LayerFlags));
        Assert.AreEqual(ordersEmptyOrderBook.BidSide
                      , new PQOrderBookSide(BookSide.BidBook, ordersCounterPartySourceTickerInfo, ordersEmptyOrderBook.NameIdLookup));
        Assert.AreEqual(ordersEmptyOrderBook.AskSide
                      , new PQOrderBookSide(BookSide.AskBook, ordersCounterPartySourceTickerInfo, ordersEmptyOrderBook.NameIdLookup));

        Assert.AreEqual(fullSupportSourceTickerInfo.MaximumPublishedLayers, fullSupportEmptyOrderBook.MaxAllowedSize);
        Assert.IsTrue(fullSupportEmptyOrderBook.LayerSupportedFlags.HasAllOf(fullSupportSourceTickerInfo.LayerFlags));
        Assert.AreEqual(fullSupportEmptyOrderBook.BidSide
                      , new PQOrderBookSide(BookSide.BidBook, fullSupportSourceTickerInfo, fullSupportEmptyOrderBook.NameIdLookup));
        Assert.AreEqual(fullSupportEmptyOrderBook.AskSide
                      , new PQOrderBookSide(BookSide.AskBook, fullSupportSourceTickerInfo, fullSupportEmptyOrderBook.NameIdLookup));

        foreach (var emptyOrderBook in allEmptyOrderBooks)
        {
            Assert.IsFalse(emptyOrderBook.IsBidBookChanged);
            Assert.IsFalse(emptyOrderBook.IsAskBookChanged);
        }
    }

    [TestMethod]
    public void InitializedFromConstructor_New_InitializesFieldsAsExpected()
    {
        var expectedBidPriceTop    = 2.34567m;
        var expectedAskPriceTop    = 3.45678m;
        var expectedDailyTickCount = 10u;
        var expectedBidBook =
            new PQOrderBookSide(BookSide.BidBook, simpleSourceTickerInfo)
            {
                [0] = new PQPriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var expectedAskBook =
            new PQOrderBookSide(BookSide.AskBook, simpleSourceTickerInfo)
            {
                [0] = new PQPriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };

        var fromConstructor = new PQOrderBook(expectedBidBook, expectedAskBook, expectedDailyTickCount, true);

        Assert.AreSame(expectedBidBook, fromConstructor.BidSide);
        Assert.AreSame(expectedAskBook, fromConstructor.AskSide);
        Assert.AreEqual(simpleSourceTickerInfo.MaximumPublishedLayers, fromConstructor.MaxAllowedSize);
        Assert.IsTrue(fromConstructor.LayerSupportedFlags.HasAllOf(simpleSourceTickerInfo.LayerFlags));
        Assert.AreEqual(expectedBidBook, fromConstructor.BidSide);
        Assert.AreEqual(expectedAskBook, fromConstructor.AskSide);
        Assert.AreEqual(expectedDailyTickCount, fromConstructor.DailyTickUpdateCount);
        Assert.IsTrue(fromConstructor.IsBidBookChanged);
        Assert.IsTrue(fromConstructor.IsAskBookChanged);
    }

    [TestMethod]
    public void NonPQOrderBooks_New_ConvertsToOrderBook()
    {
        var expectedBidPriceTop    = 2.34567m;
        var expectedAskPriceTop    = 3.45678m;
        var expectedDailyTickCount = 10u;
        var convertedBidBook =
            new OrderBookSide(BookSide.BidBook, new SourceTickerInfo(simpleSourceTickerInfo))
            {
                [0] = new PriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var convertedAskBook =
            new OrderBookSide(BookSide.AskBook, new SourceTickerInfo(simpleSourceTickerInfo))
            {
                [0] = new PriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };

        var fromPQConstructor = new OrderBook(convertedBidBook, convertedAskBook, expectedDailyTickCount, true);
        var fromConstructor   = new PQOrderBook(fromPQConstructor);

        Assert.AreNotSame(convertedBidBook, fromConstructor.BidSide);
        Assert.AreNotSame(convertedAskBook, fromConstructor.AskSide);
        Assert.AreEqual(simpleSourceTickerInfo.MaximumPublishedLayers, fromConstructor.MaxAllowedSize);
        Assert.IsTrue(fromConstructor.LayerSupportedFlags.HasAllOf(simpleSourceTickerInfo.LayerFlags));
        Assert.AreNotEqual(convertedBidBook, fromConstructor.BidSide);
        Assert.AreNotEqual(convertedAskBook, fromConstructor.AskSide);
        Assert.IsTrue(convertedBidBook.AreEquivalent(fromConstructor.BidSide));
        Assert.IsTrue(convertedAskBook.AreEquivalent(fromConstructor.AskSide));
        Assert.AreEqual(expectedDailyTickCount, fromConstructor.DailyTickUpdateCount);
        Assert.IsTrue(fromConstructor.IsBidBookChanged);
        Assert.IsTrue(fromConstructor.IsAskBookChanged);
    }

    [TestMethod]
    public void SimpleLevelInOrderBook_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQPriceVolumeLayer), simpleEmptyOrderBook, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void SourceNameInOrderBook_New_BuildsSourcePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQSourcePriceVolumeLayer), sourceNameEmptyOrderBook, sourceNameFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void SourceQuoteInOrderBook_New_BuildsSourceQuoteRefPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQSourceQuoteRefPriceVolumeLayer), sourceQuoteRefEmptyOrderBook, sourceQuoteRefFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void OrderLevelInOrderBook_New_BuildsOrderPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQOrdersPriceVolumeLayer), ordersEmptyOrderBook, ordersFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void ValueDateInOrderBook_New_BuildsValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQValueDatePriceVolumeLayer), valueDateEmptyOrderBook, valueDateFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void EveryLayerInOrderBook_New_BuildsSourceQuoteRefTraderValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PQFullSupportPriceVolumeLayer), fullSupportEmptyOrderBook, fullSupportFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_New_CopiesValues()
    {
        foreach (var populatedQuote in allFullyPopulatedOrderBooks)
        {
            var copyQuote = new PQOrderBook(populatedQuote);
            Assert.AreEqual(populatedQuote, copyQuote);
        }
    }

    [TestMethod]
    public void NonPQOrderBookPopulatedOrderBook_New_CopiesValuesConvertsOrderBook()
    {
        foreach (var populatedOrderBook in allFullyPopulatedOrderBooks)
        {
            var originalBidBook = populatedOrderBook.BidSide;
            var originalAskBook = populatedOrderBook.AskSide;

            var copyQuote = new OrderBook(populatedOrderBook);
            Assert.AreNotEqual(populatedOrderBook, copyQuote);
            Assert.IsTrue(populatedOrderBook.AreEquivalent(copyQuote));
            Assert.IsTrue(copyQuote.AreEquivalent(populatedOrderBook));

            populatedOrderBook.BidSide = originalBidBook;
            populatedOrderBook.AskSide = originalAskBook;
        }
    }

    [TestMethod]
    public void PopulatedQuote_Mutate_UpdatesFields()
    {
        var expectedBidPriceTop = 2.34567m;
        var expectedAskPriceTop = 3.45678m;

        foreach (var emptyOrderBook in allEmptyOrderBooks)
        {
            var expectedBidOrderBook = emptyOrderBook.BidSide.Clone();
            expectedBidOrderBook[0]!.Price = expectedBidPriceTop;
            var expectedAskOrderBook = emptyOrderBook.AskSide.Clone();
            expectedAskOrderBook[0]!.Price = expectedAskPriceTop;

            emptyOrderBook.BidSide             = expectedBidOrderBook;
            emptyOrderBook.IsBidBookChanged    = true;
            emptyOrderBook.AskSide             = expectedAskOrderBook;
            emptyOrderBook.IsAskBookChanged    = true;
            emptyOrderBook.LayerSupportedFlags = LayerFlagsExtensions.FullSupportLayerFlags;
            emptyOrderBook.IsLadder            = true;

            Assert.AreEqual(true, emptyOrderBook.IsAskBookChanged);
            Assert.AreSame(expectedBidOrderBook, emptyOrderBook.BidSide);
            Assert.AreEqual(true, emptyOrderBook.IsBidBookChanged);
            Assert.AreSame(expectedAskOrderBook, emptyOrderBook.AskSide);
            Assert.AreEqual(expectedAskOrderBook.LayerSupportedFlags, LayerFlagsExtensions.FullSupportLayerFlags | LayerFlags.Ladder);
            Assert.AreEqual(expectedBidOrderBook.LayerSupportedFlags, LayerFlagsExtensions.FullSupportLayerFlags | LayerFlags.Ladder);
            Assert.AreEqual(emptyOrderBook.LayerSupportedFlags, LayerFlagsExtensions.FullSupportLayerFlags | LayerFlags.Ladder);
            Assert.AreEqual(LayerType.FullSupportPriceVolume, emptyOrderBook.LayersSupportedType);
            Assert.AreEqual(LayerType.FullSupportPriceVolume, emptyOrderBook.BidSide.LayerSupportedType);
            Assert.AreEqual(LayerType.FullSupportPriceVolume, emptyOrderBook.AskSide.LayerSupportedType);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        foreach (var fullyPopulatedQuote in allFullyPopulatedOrderBooks)
        {
            var emptyQuote = new PQOrderBook();
            emptyQuote.CopyFrom(fullyPopulatedQuote, QuoteBehavior);

            Assert.AreEqual(fullyPopulatedQuote, emptyQuote);
        }
    }


    [TestMethod]
    public void FullSupportPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        foreach (var populatedOrderBook in allFullyPopulatedOrderBooks)
        {
            if (populatedOrderBook is ISupportsPQNameIdLookupGenerator resetNameIdGen)
            {
                populatedOrderBook.NameIdLookup.Clear();
            }
            populatedOrderBook.CopyFrom(fullSupportFullyPopulatedOrderBook, QuoteBehavior);
            Assert.IsTrue(populatedOrderBook.AreEquivalent(fullSupportFullyPopulatedOrderBook));
        }
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        foreach (var populatedOrderBook in allFullyPopulatedOrderBooks)
        {
            IMutableOrderBook clone = populatedOrderBook.Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = (IMutableOrderBook)((ICloneable<IOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = (IMutableOrderBook)((IOrderBook)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((IMutableOrderBook)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((ICloneable<IMutableOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((ICloneable<IPQOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((IPQOrderBook)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((IPQOrderBook)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((ICloneable<IPQOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone.Recycler = new Recycler();
            clone          = populatedOrderBook.Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = (IMutableOrderBook)((ICloneable<IOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = (IMutableOrderBook)((IOrderBook)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((IMutableOrderBook)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((ICloneable<IMutableOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((ICloneable<IPQOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((IPQOrderBook)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((IPQOrderBook)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
            clone = ((ICloneable<IPQOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(clone, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clone);
        }
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsExpected()
    {
        foreach (var populatedOrderBook in allFullyPopulatedOrderBooks)
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedOrderBook, populatedOrderBook.Clone());
    }

    [TestMethod]
    public void PopulatedQuote_GetHashCode_NotEqualToZero()
    {
        foreach (var populatedQuote in allFullyPopulatedOrderBooks) Assert.AreNotEqual(0, populatedQuote.GetHashCode());
    }

    [TestMethod]
    public void PopulatedOrderBook_DailyTickUpdateCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        simpleEmptyOrderBook.HasUpdates = false;

        AssertDailyTickUpdateCountFieldUpdatesReturnAsExpected(simpleEmptyOrderBook);
    }

    public static void AssertDailyTickUpdateCountFieldUpdatesReturnAsExpected
    (
        IPQOrderBook? orderBook,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (orderBook == null) return;
        var l2QNotNull = l2Quote != null;

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(orderBook.IsDailyTickUpdateCountUpdated);
        Assert.IsFalse(orderBook.HasUpdates);
        orderBook.DailyTickUpdateCount = 12;
        Assert.IsTrue(orderBook.HasUpdates);
        orderBook.UpdateComplete();
        orderBook.DailyTickUpdateCount          = 0;
        orderBook.IsDailyTickUpdateCountUpdated = false;
        orderBook.HasUpdates                    = false;

        Assert.AreEqual(0, orderBook.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedDailyTickCount = 128u;
        orderBook.DailyTickUpdateCount = expectedDailyTickCount;
        Assert.IsTrue(orderBook.HasUpdates);
        Assert.AreEqual(expectedDailyTickCount, orderBook.DailyTickUpdateCount);
        Assert.IsTrue(orderBook.IsDailyTickUpdateCountUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        var layerUpdates = orderBook
                           .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        var expectedUpdate
            = new PQFieldUpdate(PQFeedFields.QuoteDailyTotalTickCount, expectedDailyTickCount);
        Assert.AreEqual(expectedUpdate, layerUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedUpdate, l2QUpdates[2]);

        orderBook.IsDailyTickUpdateCountUpdated = false;
        Assert.IsFalse(orderBook.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(orderBook.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrNone());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteDailyTotalTickCount
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedUpdate, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var newOrderBook = newEmpty.OrderBook;
            Assert.AreEqual(expectedDailyTickCount, newOrderBook.DailyTickUpdateCount);
            Assert.IsTrue(newOrderBook.IsDailyTickUpdateCountUpdated);
            Assert.IsTrue(newOrderBook.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        layerUpdates =
            (from update in orderBook.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == PQFeedFields.QuoteDailyTotalTickCount
                select update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        Assert.AreEqual(expectedUpdate, layerUpdates[0]);

        var newPQOrderBook = new PQOrderBook();
        newPQOrderBook.UpdateField(layerUpdates[0]);
        Assert.AreEqual(expectedDailyTickCount, newPQOrderBook.DailyTickUpdateCount);
        Assert.IsTrue(newPQOrderBook.IsDailyTickUpdateCountUpdated);
        Assert.IsTrue(newPQOrderBook.HasUpdates);

        orderBook.DailyTickUpdateCount = 0u;
        orderBook.HasUpdates  = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedOrderBook_TotalOpenInterestDataSourceField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertDataSourceFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.OpenInterest, PQFeedFields.QuoteOpenInterestTotal, null, 
             simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_TotalOpenInterestUpdatedTimeField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertUpdatedTimeFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.OpenInterest, PQFeedFields.QuoteOpenInterestTotal, null, 
             simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_TotalOpenInterestVolumeField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertVolumeFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.OpenInterest, PQFeedFields.QuoteOpenInterestTotal, null, 
             simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_TotalOpenInterestVwapField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertVwapFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.OpenInterest, PQFeedFields.QuoteOpenInterestTotal, null, 
             simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_BidSideOpenInterestDataSourceField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertDataSourceFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.BidSide.OpenInterestSide, PQFeedFields.QuoteOpenInterestSided, 
             simpleFullyPopulatedOrderBook.BidSide, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_BidOpenInterestUpdatedTimeField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertUpdatedTimeFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.BidSide.OpenInterestSide, PQFeedFields.QuoteOpenInterestSided, 
             simpleFullyPopulatedOrderBook.BidSide, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_BidSideOpenInterestVolumeField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertVolumeFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.BidSide.OpenInterestSide, PQFeedFields.QuoteOpenInterestSided, 
             simpleFullyPopulatedOrderBook.BidSide, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_BidSideOpenInterestVwapField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertVwapFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.BidSide.OpenInterestSide, PQFeedFields.QuoteOpenInterestSided, 
             simpleFullyPopulatedOrderBook.BidSide, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_AskSideOpenInterestDataSourceField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertDataSourceFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.AskSide.OpenInterestSide, PQFeedFields.QuoteOpenInterestSided, 
             simpleFullyPopulatedOrderBook.AskSide, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_AskOpenInterestUpdatedTimeField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertUpdatedTimeFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.AskSide.OpenInterestSide, PQFeedFields.QuoteOpenInterestSided, 
             simpleFullyPopulatedOrderBook.AskSide, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_AskSideOpenInterestVolumeField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertVolumeFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.AskSide.OpenInterestSide, PQFeedFields.QuoteOpenInterestSided, 
             simpleFullyPopulatedOrderBook.AskSide, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_AskSideOpenInterestVwapField_UpdatesAsExpected()
    {
        simpleFullyPopulatedOrderBook.HasUpdates = false;

        PQMarketAggregateTests.AssertVwapFieldUpdatesReturnAsExpected
            (simpleFullyPopulatedOrderBook.AskSide.OpenInterestSide, PQFeedFields.QuoteOpenInterestSided, 
             simpleFullyPopulatedOrderBook.AskSide, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void EmptyOrderBook_BidSideDailyTickUpdateCountField_UpdatesAsExpected()
    {
        simpleEmptyOrderBook.HasUpdates = false;

        PQOrderBookSideTests.AssertDailyTickUpdateCountFieldUpdatesReturnAsExpected
            (simpleEmptyOrderBook.BidSide, simpleEmptyOrderBook);
    }

    [TestMethod]
    public void EmptyOrderBook_AskSideDailyTickUpdateCountField_UpdatesAsExpected()
    {
        simpleEmptyOrderBook.HasUpdates = false;

        PQOrderBookSideTests.AssertDailyTickUpdateCountFieldUpdatesReturnAsExpected
            (simpleEmptyOrderBook.AskSide, simpleEmptyOrderBook);
    }

    [TestMethod]
    public void SimpleFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var q      = simpleFullyPopulatedOrderBook;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SourceNameFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var q      = sourceNameFullyPopulatedOrderBook;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SourceQuoteRefFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var q      = sourceQuoteRefFullyPopulatedOrderBook;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void OrderCountFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = orderCountFullyPopulatedOrderBook;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void OrdersFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = ordersFullyPopulatedOrderBook;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void ValueDateFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = valueDateFullyPopulatedOrderBook;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SourceQuoteRefTraderDetailsValueDateFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = fullSupportFullyPopulatedOrderBook;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedQuote in allFullyPopulatedOrderBooks)
        {
            var q        = populatedQuote;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));


            Assert.IsTrue(toString.Contains($"{nameof(q.LayersSupportedType)}: {q.LayersSupportedType}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.DailyTickUpdateCount)}: {q.DailyTickUpdateCount}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsAskBookChanged)}: {q.IsAskBookChanged}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsBidBookChanged)}: {q.IsBidBookChanged}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.OpenInterest)}: {q.OpenInterest}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.AskSide)}: {q.AskSide}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.BidSide)}: {q.BidSide}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.IsLadder)}: {q.IsLadder}"));
        }
    }


    internal static PQOrderBook GenerateBook<T>
    (int numberOfLayers, decimal startingPrice, decimal deltaPricePerLayer,
        decimal startingVolume, decimal deltaVolumePerLayer, Func<decimal, decimal, T> genNewLayerObj)
        where T : IPriceVolumeLayer
    {
        var askSide = OrderBookSideTests.GenerateBookSide(BookSide.AskBook, numberOfLayers, startingPrice, deltaPricePerLayer,
                                                          startingVolume, deltaVolumePerLayer, genNewLayerObj);
        var bidSide = OrderBookSideTests.GenerateBookSide(BookSide.BidBook, numberOfLayers, startingPrice - deltaPricePerLayer,
                                                          deltaPricePerLayer, startingVolume, deltaVolumePerLayer, genNewLayerObj);

        return new PQOrderBook(bidSide, askSide);
    }


    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQOrderBook? commonCompareOrderBook,
        IPQOrderBook? changingOrderBook,
        IMutablePublishableLevel2Quote? originalQuote = null,
        IMutablePublishableLevel2Quote? changingQuote = null)
    {
        if (commonCompareOrderBook == null && changingOrderBook == null) return;

        if (!commonCompareOrderBook!.LayerSupportedFlags.HasAllOf(LayerFlagsExtensions.FullSupportLayerFlags))
        {
            var originalFlags = changingOrderBook!.LayerSupportedFlags;
            var clone         = changingOrderBook.Clone();
            changingOrderBook.LayerSupportedFlags = LayerFlagsExtensions.FullSupportLayerFlags;
            Assert.IsFalse(commonCompareOrderBook.AreEquivalent(changingOrderBook));
            if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
            changingOrderBook.LayerSupportedFlags = originalFlags;
            Assert.IsFalse(commonCompareOrderBook.AreEquivalent(changingOrderBook));
            if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
            changingOrderBook = clone;

            Assert.IsTrue(commonCompareOrderBook.AreEquivalent(changingOrderBook));
            if (originalQuote != null)
            {
                changingQuote!.OrderBook = clone;
                Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
            }
        }
        changingOrderBook!.DailyTickUpdateCount = 300_000;
        Assert.IsFalse(commonCompareOrderBook.AreEquivalent(changingOrderBook));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOrderBook.DailyTickUpdateCount          = commonCompareOrderBook.DailyTickUpdateCount;
        changingOrderBook.IsDailyTickUpdateCountUpdated = commonCompareOrderBook.IsDailyTickUpdateCountUpdated;
        Assert.IsTrue(commonCompareOrderBook.AreEquivalent(changingOrderBook));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        PQMarketAggregateTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareOrderBook.OpenInterest, changingOrderBook.OpenInterest,
             null, null, commonCompareOrderBook, changingOrderBook, originalQuote, changingQuote);

        PQOrderBookSideTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareOrderBook.BidSide
           , changingOrderBook.BidSide, commonCompareOrderBook, changingOrderBook);

        PQOrderBookSideTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareOrderBook.AskSide
           , changingOrderBook.AskSide, commonCompareOrderBook, changingOrderBook);
    }

    private void AssertLayerTypeIsExpected(Type expectedType, params PQOrderBook[] booksToCheck)
    {
        foreach (var orderBook in booksToCheck)
            for (var i = 0; i < orderBook.MaxAllowedSize; i++)
            {
                Assert.AreEqual(expectedType, orderBook.BidSide[i]?.GetType(),
                                $"BidBook[{i}] expectedType: {expectedType.Name} " +
                                $"actualType: {orderBook.BidSide[i]?.GetType()?.Name ?? "null"}");
                Assert.AreEqual(expectedType, orderBook.AskSide[i]?.GetType(),
                                $"BidBook[{i}] expectedType: {expectedType.Name} " +
                                $"actualType: {orderBook.AskSide[i]?.GetType()?.Name ?? "null"}");
            }
    }
}
