using System.Text.Json;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

[TestClass]
public class OrderBookTests
{
    private IList<OrderBook> allEmptyOrderBooks          = null!;
    private IList<OrderBook> allFullyPopulatedOrderBooks = null!;

    private OrderBook fullSupportEmptyOrderBook          = null!;
    private OrderBook fullSupportFullyPopulatedOrderBook = null!;

    private ISourceTickerInfo fullSupportSourceTickerInfo = null!;

    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;

    private OrderBook simpleEmptyOrderBook          = null!;
    private OrderBook simpleFullyPopulatedOrderBook = null!;

    private ISourceTickerInfo simpleSourceTickerInfo = null!;

    private OrderBook sourceNameEmptyOrderBook          = null!;
    private OrderBook sourceNameFullyPopulatedOrderBook = null!;

    private ISourceTickerInfo sourceNameSourceTickerInfo = null!;

    private OrderBook sourceQuoteRefEmptyOrderBook          = null!;
    private OrderBook sourceQuoteRefFullyPopulatedOrderBook = null!;

    private OrderBook orderCountEmptyOrderBook          = null!;
    private OrderBook orderCountFullyPopulatedOrderBook = null!;

    private ISourceTickerInfo orderCountSourceTickerInfo = null!;

    private ISourceTickerInfo sourceRefSourceTickerInfo = null!;

    private OrderBook ordersEmptyOrderBook          = null!;
    private OrderBook ordersFullyPopulatedOrderBook = null!;

    private ISourceTickerInfo ordersCounterPartySourceTickerInfo = null!;

    private OrderBook valueDateEmptyOrderBook          = null!;
    private OrderBook valueDateFullyPopulatedOrderBook = null!;

    private ISourceTickerInfo valueDateSourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();


        simpleSourceTickerInfo             = SourceTickerInfoTests.SimpleL2PriceVolumeSti;
        sourceNameSourceTickerInfo         = SourceTickerInfoTests.SourceNameL2PriceVolumeSti;
        sourceRefSourceTickerInfo          = SourceTickerInfoTests.SourceQuoteRefL2PriceVolumeSti;
        orderCountSourceTickerInfo         = SourceTickerInfoTests.OrdersCountL3JustTradeTradeSti;
        ordersCounterPartySourceTickerInfo = SourceTickerInfoTests.OrdersCounterPartyL2PriceVolumeSti;
        valueDateSourceTickerInfo          = SourceTickerInfoTests.ValueDateL2PriceVolumeSti;
        fullSupportSourceTickerInfo        = SourceTickerInfoTests.FullSupportL2PriceVolumeSti;

        simpleEmptyOrderBook          = new OrderBook(simpleSourceTickerInfo);
        simpleFullyPopulatedOrderBook = new OrderBook(simpleSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(simpleFullyPopulatedOrderBook, 1);
        sourceNameEmptyOrderBook          = new OrderBook(sourceNameSourceTickerInfo);
        sourceNameFullyPopulatedOrderBook = new OrderBook(sourceNameSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(sourceNameFullyPopulatedOrderBook, 2);
        sourceQuoteRefEmptyOrderBook          = new OrderBook(sourceRefSourceTickerInfo);
        sourceQuoteRefFullyPopulatedOrderBook = new OrderBook(sourceRefSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(sourceQuoteRefFullyPopulatedOrderBook, 3);
        orderCountEmptyOrderBook          = new OrderBook(orderCountSourceTickerInfo);
        orderCountFullyPopulatedOrderBook = new OrderBook(orderCountSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(orderCountFullyPopulatedOrderBook, 4);
        ordersEmptyOrderBook          = new OrderBook(ordersCounterPartySourceTickerInfo);
        ordersFullyPopulatedOrderBook = new OrderBook(ordersCounterPartySourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(ordersFullyPopulatedOrderBook, 5);
        valueDateEmptyOrderBook          = new OrderBook(valueDateSourceTickerInfo);
        valueDateFullyPopulatedOrderBook = new OrderBook(valueDateSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(valueDateFullyPopulatedOrderBook, 6);
        fullSupportEmptyOrderBook          = new OrderBook(fullSupportSourceTickerInfo);
        fullSupportFullyPopulatedOrderBook = new OrderBook(fullSupportSourceTickerInfo);
        quoteSequencedTestDataBuilder.SetupOrderBook(fullSupportFullyPopulatedOrderBook, 7);

        allFullyPopulatedOrderBooks = new List<OrderBook>
        {
            simpleFullyPopulatedOrderBook, sourceNameFullyPopulatedOrderBook
          , sourceQuoteRefFullyPopulatedOrderBook, ordersFullyPopulatedOrderBook
          , orderCountFullyPopulatedOrderBook, valueDateFullyPopulatedOrderBook
          , fullSupportFullyPopulatedOrderBook
        };
        allEmptyOrderBooks = new List<OrderBook>
        {
            simpleEmptyOrderBook, sourceNameEmptyOrderBook, sourceQuoteRefEmptyOrderBook
          , orderCountEmptyOrderBook, ordersEmptyOrderBook, valueDateEmptyOrderBook
          , fullSupportEmptyOrderBook
        };
    }

    [TestMethod]
    public void EmptyQuote_New_InitializesFieldsAsExpected()
    {
        Assert.AreEqual(simpleSourceTickerInfo.MaximumPublishedLayers, simpleEmptyOrderBook.MaxPublishDepth);
        Assert.IsTrue(simpleEmptyOrderBook.LayerSupportedFlags.HasAllOf(simpleSourceTickerInfo.LayerFlags));
        Assert.AreEqual(simpleEmptyOrderBook.BidSide, new OrderBookSide(BookSide.BidBook, simpleSourceTickerInfo));
        Assert.AreEqual(simpleEmptyOrderBook.AskSide, new OrderBookSide(BookSide.AskBook, simpleSourceTickerInfo));

        Assert.AreEqual(sourceNameSourceTickerInfo.MaximumPublishedLayers, sourceNameEmptyOrderBook.MaxPublishDepth);
        Assert.IsTrue(sourceNameEmptyOrderBook.LayerSupportedFlags.HasAllOf(sourceNameSourceTickerInfo.LayerFlags));
        Assert.AreEqual(sourceNameEmptyOrderBook.BidSide, new OrderBookSide(BookSide.BidBook, sourceNameSourceTickerInfo));
        Assert.AreEqual(sourceNameEmptyOrderBook.AskSide, new OrderBookSide(BookSide.AskBook, sourceNameSourceTickerInfo));

        Assert.AreEqual(sourceRefSourceTickerInfo.MaximumPublishedLayers, sourceQuoteRefEmptyOrderBook.MaxPublishDepth);
        Assert.IsTrue(sourceQuoteRefEmptyOrderBook.LayerSupportedFlags.HasAllOf(sourceRefSourceTickerInfo.LayerFlags));
        Assert.AreEqual(sourceQuoteRefEmptyOrderBook.BidSide, new OrderBookSide(BookSide.BidBook, sourceRefSourceTickerInfo));
        Assert.AreEqual(sourceQuoteRefEmptyOrderBook.AskSide, new OrderBookSide(BookSide.AskBook, sourceRefSourceTickerInfo));

        Assert.AreEqual(valueDateSourceTickerInfo.MaximumPublishedLayers, valueDateEmptyOrderBook.MaxPublishDepth);
        Assert.IsTrue(valueDateEmptyOrderBook.LayerSupportedFlags.HasAllOf(valueDateSourceTickerInfo.LayerFlags));
        Assert.AreEqual(valueDateEmptyOrderBook.BidSide, new OrderBookSide(BookSide.BidBook, valueDateSourceTickerInfo));
        Assert.AreEqual(valueDateEmptyOrderBook.AskSide, new OrderBookSide(BookSide.AskBook, valueDateSourceTickerInfo));

        Assert.AreEqual(ordersCounterPartySourceTickerInfo.MaximumPublishedLayers, ordersEmptyOrderBook.MaxPublishDepth);
        Assert.IsTrue(ordersEmptyOrderBook.LayerSupportedFlags.HasAllOf(ordersCounterPartySourceTickerInfo.LayerFlags));
        Assert.AreEqual(ordersEmptyOrderBook.BidSide, new OrderBookSide(BookSide.BidBook, ordersCounterPartySourceTickerInfo));
        Assert.AreEqual(ordersEmptyOrderBook.AskSide, new OrderBookSide(BookSide.AskBook, ordersCounterPartySourceTickerInfo));

        Assert.AreEqual(fullSupportSourceTickerInfo.MaximumPublishedLayers, fullSupportEmptyOrderBook.MaxPublishDepth);
        Assert.IsTrue(fullSupportEmptyOrderBook.LayerSupportedFlags.HasAllOf(fullSupportSourceTickerInfo.LayerFlags));
        Assert.AreEqual(fullSupportEmptyOrderBook.BidSide, new OrderBookSide(BookSide.BidBook, fullSupportSourceTickerInfo));
        Assert.AreEqual(fullSupportEmptyOrderBook.AskSide, new OrderBookSide(BookSide.AskBook, fullSupportSourceTickerInfo));

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
            new OrderBookSide(BookSide.BidBook, simpleSourceTickerInfo)
            {
                [0] = new PriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var expectedAskBook =
            new OrderBookSide(BookSide.AskBook, simpleSourceTickerInfo)
            {
                [0] = new PriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };

        var fromConstructor = new OrderBook(expectedBidBook, expectedAskBook, expectedDailyTickCount, true);

        Assert.AreSame(expectedBidBook, fromConstructor.BidSide);
        Assert.AreSame(expectedAskBook, fromConstructor.AskSide);
        Assert.AreEqual(simpleSourceTickerInfo.MaximumPublishedLayers, fromConstructor.MaxPublishDepth);
        Assert.IsTrue(fromConstructor.LayerSupportedFlags.HasAllOf(simpleSourceTickerInfo.LayerFlags));
        Assert.AreEqual(expectedBidBook, fromConstructor.BidSide);
        Assert.AreEqual(expectedAskBook, fromConstructor.AskSide);
        Assert.AreEqual(expectedDailyTickCount, fromConstructor.DailyTickUpdateCount);
        Assert.IsFalse(fromConstructor.IsBidBookChanged);
        Assert.IsFalse(fromConstructor.IsAskBookChanged);
    }

    [TestMethod]
    public void NonOrderBooks_New_ConvertsToOrderBook()
    {
        var expectedSingleValue    = 1.23456m;
        var expectedBidPriceTop    = 2.34567m;
        var expectedAskPriceTop    = 3.45678m;
        var expectedDailyTickCount = 10u;
        var convertedBidBook =
            new PQOrderBookSide(BookSide.BidBook, new PQSourceTickerInfo(simpleSourceTickerInfo))
            {
                [0] = new PQPriceVolumeLayer(expectedBidPriceTop, 1_000_000)
            };
        var convertedAskBook =
            new PQOrderBookSide(BookSide.AskBook, new PQSourceTickerInfo(simpleSourceTickerInfo))
            {
                [0] = new PQPriceVolumeLayer(expectedAskPriceTop, 1_000_000)
            };

        var fromPQConstructor = new PQOrderBook(convertedBidBook, convertedAskBook, expectedDailyTickCount, true);
        var fromConstructor   = new OrderBook(fromPQConstructor);

        Assert.AreNotSame(convertedBidBook, fromConstructor.BidSide);
        Assert.AreNotSame(convertedAskBook, fromConstructor.AskSide);
        Assert.AreEqual(simpleSourceTickerInfo.MaximumPublishedLayers, fromConstructor.MaxPublishDepth);
        Assert.IsTrue(fromConstructor.LayerSupportedFlags.HasAllOf(simpleSourceTickerInfo.LayerFlags));
        Assert.AreNotEqual(convertedBidBook, fromConstructor.BidSide);
        Assert.AreNotEqual(convertedAskBook, fromConstructor.AskSide);
        Assert.IsTrue(convertedBidBook.AreEquivalent(fromConstructor.BidSide));
        Assert.IsTrue(convertedAskBook.AreEquivalent(fromConstructor.AskSide));
        Assert.AreEqual(expectedDailyTickCount, fromConstructor.DailyTickUpdateCount);
        Assert.IsFalse(fromConstructor.IsBidBookChanged);
        Assert.IsFalse(fromConstructor.IsAskBookChanged);
    }

    [TestMethod]
    public void SimpleLevelInOrderBook_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(PriceVolumeLayer), simpleEmptyOrderBook, simpleFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void SourceNameInOrderBook_New_BuildsSourcePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(SourcePriceVolumeLayer), sourceNameEmptyOrderBook, sourceNameFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void SourceQuoteInOrderBook_New_BuildsSourceQuoteRefPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(SourceQuoteRefPriceVolumeLayer), sourceQuoteRefEmptyOrderBook, sourceQuoteRefFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void OrderLevelInOrderBook_New_BuildsOrderPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(OrdersPriceVolumeLayer), ordersEmptyOrderBook, ordersFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void ValueDateInOrderBook_New_BuildsValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(ValueDatePriceVolumeLayer), valueDateEmptyOrderBook, valueDateFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void EveryLayerInOrderBook_New_BuildsSourceQuoteRefTraderValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected
            (typeof(FullSupportPriceVolumeLayer), fullSupportEmptyOrderBook, fullSupportFullyPopulatedOrderBook);
    }

    [TestMethod]
    public void PopulatedOrderBook_New_CopiesValues()
    {
        foreach (var populatedQuote in allFullyPopulatedOrderBooks)
        {
            var copyQuote = new OrderBook(populatedQuote);
            Assert.AreEqual(populatedQuote, copyQuote);
        }
    }

    [TestMethod]
    public void NonOrderBookPopulatedOrderBook_New_CopiesValuesConvertsOrderBook()
    {
        foreach (var populatedOrderBook in allFullyPopulatedOrderBooks)
        {
            var originalBidBook = populatedOrderBook.BidSide;
            var originalAskBook = populatedOrderBook.AskSide;

            var pqBidBook = new PQOrderBookSide(originalBidBook);
            var pqAskBook = new PQOrderBookSide(originalAskBook);

            populatedOrderBook.BidSide = pqBidBook;
            populatedOrderBook.AskSide = pqAskBook;

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
            var emptyQuote = new OrderBook();
            emptyQuote.CopyFrom(fullyPopulatedQuote);

            Assert.AreEqual(fullyPopulatedQuote, emptyQuote);
        }
    }


    [TestMethod]
    public void FullSupportPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        foreach (var emptyInitialised in allFullyPopulatedOrderBooks)
        {
            emptyInitialised.CopyFrom(fullSupportFullyPopulatedOrderBook);
            Assert.IsTrue(emptyInitialised.AreEquivalent(fullSupportFullyPopulatedOrderBook));
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
    public void SimpleFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
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
        var so = new JsonSerializerOptions()
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
        var so = new JsonSerializerOptions()
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


    internal static OrderBook GenerateBook<T>
    (int numberOfLayers, decimal startingPrice, decimal deltaPricePerLayer,
        decimal startingVolume, decimal deltaVolumePerLayer, Func<decimal, decimal, T> genNewLayerObj)
        where T : IPriceVolumeLayer
    {
        var askSide = OrderBookSideTests.GenerateBookSide(BookSide.AskBook, numberOfLayers, startingPrice, deltaPricePerLayer,
                                                          startingVolume, deltaVolumePerLayer, genNewLayerObj);
        var bidSide = OrderBookSideTests.GenerateBookSide(BookSide.BidBook, numberOfLayers, startingPrice - deltaPricePerLayer,
                                                          deltaPricePerLayer, startingVolume, deltaVolumePerLayer, genNewLayerObj);

        return new OrderBook(bidSide, askSide);
    }


    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableOrderBook? commonCompareOrderBook,
        IMutableOrderBook? changingOrderBook,
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
        changingOrderBook.DailyTickUpdateCount = commonCompareOrderBook.DailyTickUpdateCount;
        Assert.IsTrue(commonCompareOrderBook.AreEquivalent(changingOrderBook));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        MarketAggregateTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareOrderBook.OpenInterest, changingOrderBook.OpenInterest,
             null, null, commonCompareOrderBook, changingOrderBook, originalQuote, changingQuote);

        OrderBookSideTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareOrderBook.BidSide
           , changingOrderBook.BidSide, commonCompareOrderBook, changingOrderBook);

        OrderBookSideTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, commonCompareOrderBook.AskSide
           , changingOrderBook.AskSide, commonCompareOrderBook, changingOrderBook);
    }

    private void AssertLayerTypeIsExpected(Type expectedType, params OrderBook[] booksToCheck)
    {
        foreach (var orderBook in booksToCheck)
            for (var i = 0; i < orderBook.MaxPublishDepth; i++)
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
