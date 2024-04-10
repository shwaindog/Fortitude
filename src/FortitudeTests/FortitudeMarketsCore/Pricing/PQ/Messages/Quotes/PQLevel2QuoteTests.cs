#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

[TestClass]
public class PQLevel2QuoteTests
{
    private IList<PQLevel2Quote> allEmptyQuotes = null!;

    private IList<PQLevel2Quote> allFullyPopulatedQuotes = null!;
    private PQLevel2Quote everyLayerEmptyLevel2Quote = null!;
    private PQLevel2Quote everyLayerFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo everyLayerSourceTickerQuoteInfo = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder = null!;
    private PQLevel2Quote simpleEmptyLevel2Quote = null!;
    private PQLevel2Quote simpleFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo simpleSourceTickerQuoteInfo = null!;
    private PQLevel2Quote sourceNameEmptyLevel2Quote = null!;
    private PQLevel2Quote sourceNameFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo sourceNameSourceTickerQuoteInfo = null!;
    private PQLevel2Quote sourceQuoteRefEmptyLevel2Quote = null!;
    private PQLevel2Quote sourceQuoteRefFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo sourceQuoteRefSourceTickerQuoteInfo = null!;
    private DateTime testDateTime;
    private PQLevel2Quote traderDetailsEmptyLevel2Quote = null!;
    private PQLevel2Quote traderDetailsFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo traderDetailsSourceTickerQuoteInfo = null!;
    private PQLevel2Quote valueDateEmptyLevel2Quote = null!;
    private PQLevel2Quote valueDateFullyPopulatedLevel2Quote = null!;
    private ISourceTickerQuoteInfo valueDateSourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        simpleSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                                  LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        sourceNameSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceName, LastTradedFlags.PaidOrGiven |
                                                                          LastTradedFlags.TraderName |
                                                                          LastTradedFlags.LastTradedVolume |
                                                                          LastTradedFlags.LastTradedTime);
        sourceQuoteRefSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue,
            "TestSource", ushort.MaxValue, "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.SourceQuoteReference, LastTradedFlags.PaidOrGiven |
                                                                                    LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                                                                    LastTradedFlags.LastTradedTime);
        traderDetailsSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue,
            "TestSource", ushort.MaxValue, "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize |
            LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                    LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        valueDateSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.ValueDate, LastTradedFlags.PaidOrGiven |
                                                                         LastTradedFlags.TraderName |
                                                                         LastTradedFlags.LastTradedVolume |
                                                                         LastTradedFlags.LastTradedTime);
        everyLayerSourceTickerQuoteInfo = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume.AllFlags(), LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                          LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        simpleEmptyLevel2Quote = new PQLevel2Quote(simpleSourceTickerQuoteInfo) { HasUpdates = false };
        simpleFullyPopulatedLevel2Quote = new PQLevel2Quote(simpleSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleFullyPopulatedLevel2Quote, 1);
        sourceNameEmptyLevel2Quote = new PQLevel2Quote(sourceNameSourceTickerQuoteInfo) { HasUpdates = false };
        sourceNameFullyPopulatedLevel2Quote = new PQLevel2Quote(sourceNameSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceNameFullyPopulatedLevel2Quote, 2);
        sourceQuoteRefEmptyLevel2Quote = new PQLevel2Quote(sourceQuoteRefSourceTickerQuoteInfo)
            { HasUpdates = false };
        sourceQuoteRefFullyPopulatedLevel2Quote = new PQLevel2Quote(sourceQuoteRefSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(sourceQuoteRefFullyPopulatedLevel2Quote, 3);
        traderDetailsEmptyLevel2Quote = new PQLevel2Quote(traderDetailsSourceTickerQuoteInfo)
            { HasUpdates = false };
        traderDetailsFullyPopulatedLevel2Quote = new PQLevel2Quote(traderDetailsSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(traderDetailsFullyPopulatedLevel2Quote, 4);
        valueDateEmptyLevel2Quote = new PQLevel2Quote(valueDateSourceTickerQuoteInfo) { HasUpdates = false };
        valueDateFullyPopulatedLevel2Quote = new PQLevel2Quote(valueDateSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(valueDateFullyPopulatedLevel2Quote, 5);
        everyLayerEmptyLevel2Quote = new PQLevel2Quote(everyLayerSourceTickerQuoteInfo) { HasUpdates = false };
        everyLayerFullyPopulatedLevel2Quote = new PQLevel2Quote(everyLayerSourceTickerQuoteInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(everyLayerFullyPopulatedLevel2Quote, 5);

        allFullyPopulatedQuotes = new List<PQLevel2Quote>
            // {
            //     simpleFullyPopulatedLevel2Quote, sourceNameFullyPopulatedLevel2Quote
            //     , sourceQuoteRefFullyPopulatedLevel2Quote, traderDetailsFullyPopulatedLevel2Quote
            //     , valueDateFullyPopulatedLevel2Quote, everyLayerFullyPopulatedLevel2Quote
            // };
            {
                traderDetailsFullyPopulatedLevel2Quote
            };
        allEmptyQuotes = new List<PQLevel2Quote>
        {
            simpleEmptyLevel2Quote, sourceNameEmptyLevel2Quote, sourceQuoteRefEmptyLevel2Quote
            , traderDetailsEmptyLevel2Quote, valueDateEmptyLevel2Quote, everyLayerEmptyLevel2Quote
        };

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void TooLargeMaxBookDepth_New_CapsBookDepthTo()
    {
        var tooLargeMaxDepth = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", 21, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                                  LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        var cappedLevel2Quote = new PQLevel2Quote(tooLargeMaxDepth);
        Assert.AreEqual(PQFieldKeys.SingleByteFieldIdMaxBookDepth,
            cappedLevel2Quote.BidBook.Capacity);
        Assert.AreEqual(PQFieldKeys.SingleByteFieldIdMaxBookDepth,
            cappedLevel2Quote.AskBook.Capacity);
    }

    [TestMethod]
    public void TooSmalMaxBookDepth_New_IncreaseBookDepthAtLeast1Level()
    {
        var tooLargeMaxDepth = new SourceTickerQuoteInfo(ushort.MaxValue, "TestSource", ushort.MaxValue,
            "TestTicker", 0, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                                  LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        var cappedLevel2Quote = new PQLevel2Quote(tooLargeMaxDepth);
        Assert.AreEqual(1, cappedLevel2Quote.BidBook.Capacity);
        Assert.AreEqual(1, cappedLevel2Quote.AskBook.Capacity);
    }

    [TestMethod]
    public void SimpleLevel2Quote_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(PQPriceVolumeLayer), simpleEmptyLevel2Quote,
            simpleFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void SourceNameLevel2Quote_New_BuildsSourcePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(PQSourcePriceVolumeLayer), sourceNameEmptyLevel2Quote,
            sourceNameFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void SourceQuoteRefLevel2Quote_New_BuildsSourceQuoteRefPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(PQSourceQuoteRefPriceVolumeLayer), sourceQuoteRefEmptyLevel2Quote,
            sourceQuoteRefFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void TraderLevel2Quote_New_BuildsTraderPriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(PQTraderPriceVolumeLayer), traderDetailsEmptyLevel2Quote,
            traderDetailsFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void ValueDateLevel2Quote_New_BuildsValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(PQValueDatePriceVolumeLayer), valueDateEmptyLevel2Quote,
            valueDateFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void EveryLayerLevel2Quote_New_BuildsSourceQuoteRefTraderValueDatePriceVolumeLayeredBook()
    {
        AssertLayerTypeIsExpected(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer),
            everyLayerEmptyLevel2Quote, everyLayerFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_IsBookChanged_SetsAndReadsBookChangedStatus()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            Assert.IsTrue(populatedL2Quote.IsBidBookChanged);
            Assert.IsTrue(populatedL2Quote.IsAskBookChanged);
            Assert.IsTrue(populatedL2Quote.BidBook.HasUpdates);
            Assert.IsTrue(populatedL2Quote.AskBook.HasUpdates);

            populatedL2Quote.IsBidBookChanged = false;
            populatedL2Quote.IsAskBookChanged = false;

            Assert.IsFalse(populatedL2Quote.IsBidBookChanged);
            Assert.IsFalse(populatedL2Quote.IsAskBookChanged);
            Assert.IsFalse(populatedL2Quote.BidBook.HasUpdates);
            Assert.IsFalse(populatedL2Quote.AskBook.HasUpdates);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_IsHasUpdates_SetsAndReadsBookChangedStatus()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            Assert.IsTrue(populatedL2Quote.HasUpdates);
            Assert.IsTrue(populatedL2Quote.BidBook.HasUpdates);
            Assert.IsTrue(populatedL2Quote.AskBook.HasUpdates);

            populatedL2Quote.HasUpdates = false;

            Assert.IsFalse(populatedL2Quote.HasUpdates);
            Assert.IsFalse(populatedL2Quote.BidBook.HasUpdates);
            Assert.IsFalse(populatedL2Quote.AskBook.HasUpdates);
        }
    }

    [TestMethod]
    public void SimpleLevel2Quote_OrderBookViaInterfaces_RetrievesSameInstance()
    {
        var orderBidBookViaClass = simpleFullyPopulatedLevel2Quote.BidBook;
        var orderAskBookViaClass = simpleFullyPopulatedLevel2Quote.AskBook;

        Assert.AreSame(orderBidBookViaClass, ((ILevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(orderBidBookViaClass, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(orderBidBookViaClass, ((IPQLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);

        Assert.AreSame(orderAskBookViaClass, ((ILevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(orderAskBookViaClass, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(orderAskBookViaClass, ((IPQLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
    }

    [TestMethod]
    public void SimpleLevel2Quote_MutableSetOrderBook_OnlyAllowsPQOrderBookToBeSet()
    {
        var orderBidBookViaClass = simpleFullyPopulatedLevel2Quote.BidBook;
        var orderAskBookViaClass = simpleFullyPopulatedLevel2Quote.AskBook;
        var caughtException = false;
        var nonPQOrderBook = new OrderBook(new List<IPriceVolumeLayer>());

        try
        {
            ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook = nonPQOrderBook;
        }
        catch (InvalidCastException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
        Assert.AreSame(orderBidBookViaClass, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(orderBidBookViaClass, simpleFullyPopulatedLevel2Quote.BidBook);

        caughtException = false;

        try
        {
            ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook = nonPQOrderBook;
        }
        catch (InvalidCastException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
        Assert.AreSame(orderAskBookViaClass, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(orderAskBookViaClass, simpleFullyPopulatedLevel2Quote.AskBook);

        var newPqOrderBook = new PQOrderBook(new List<IPriceVolumeLayer>());
        ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook = newPqOrderBook;
        ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook = newPqOrderBook;
        Assert.AreSame(newPqOrderBook, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).BidBook);
        Assert.AreSame(newPqOrderBook, simpleFullyPopulatedLevel2Quote.BidBook);
        Assert.AreSame(newPqOrderBook, ((IMutableLevel2Quote)simpleFullyPopulatedLevel2Quote).AskBook);
        Assert.AreSame(newPqOrderBook, simpleFullyPopulatedLevel2Quote.AskBook);
    }

    [TestMethod]
    public void SimpleLevel2Quote_AskPriceTop_SameAsBookLevel0()
    {
        var orderAskBookViaClass = simpleFullyPopulatedLevel2Quote.AskBook;

        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.AskPriceTop, orderAskBookViaClass[0]!.Price);

        orderAskBookViaClass[0]!.Price = 456321m;
        Assert.AreEqual(456321m, simpleFullyPopulatedLevel2Quote.AskPriceTop);
        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.AskPriceTop, orderAskBookViaClass[0]!.Price);

        simpleFullyPopulatedLevel2Quote.AskPriceTop = 12398745m;
        Assert.AreEqual(12398745m, orderAskBookViaClass[0]!.Price);
        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.AskPriceTop, orderAskBookViaClass[0]!.Price);
    }

    [TestMethod]
    public void SimpleLevel2Quote_BidPriceTop_SameAsBookLevel0()
    {
        var orderBidBookViaClass = simpleFullyPopulatedLevel2Quote.BidBook;

        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.BidPriceTop, orderBidBookViaClass[0]!.Price);

        orderBidBookViaClass[0]!.Price = 456321m;
        Assert.AreEqual(456321m, simpleFullyPopulatedLevel2Quote.BidPriceTop);
        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.BidPriceTop, orderBidBookViaClass[0]!.Price);

        simpleFullyPopulatedLevel2Quote.BidPriceTop = 12398745m;
        Assert.AreEqual(12398745m, orderBidBookViaClass[0]!.Price);
        Assert.AreEqual(simpleFullyPopulatedLevel2Quote.BidPriceTop, orderBidBookViaClass[0]!.Price);
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerPriceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var priceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook))
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, priceVolumeLayer));
            var indexFromTop = (isBid ? emptyQuote.BidBook : emptyQuote.AskBook).AllLayers
                .Select((pvl, i) => new { i, pvl }).Where(indexPvl => ReferenceEquals(indexPvl.pvl,
                    priceVolumeLayer)).Select(indexPvl => indexPvl.i).FirstOrDefault();

            Assert.IsFalse(priceVolumeLayer!.IsPriceUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.AreEqual(0m, priceVolumeLayer.Price);
            Assert.AreEqual(0, priceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

            var expectedPrice = 200.1234m;
            priceVolumeLayer.Price = expectedPrice;
            Assert.IsTrue(priceVolumeLayer.IsPriceUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.AreEqual(expectedPrice, priceVolumeLayer.Price);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = priceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerPriceOffset,
                expectedPrice, 1);
            var expectedSideAdjustedLayerField = new PQFieldUpdate(
                (byte)(PQFieldKeys.LayerPriceOffset + indexFromTop),
                expectedLayerField.Value, (byte)((isBid ? 0 : PQFieldFlags.IsAskSideFlag) | 1));
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            priceVolumeLayer.IsPriceUpdated = false;
            Assert.IsFalse(priceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            priceVolumeLayer.IsPriceUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                    where update.Id == PQFieldKeys.LayerPriceOffset + indexFromTop
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            priceVolumeLayer.Price = 0m;
            priceVolumeLayer.IsPriceUpdated = false;

            var newEmpty = new PQLevel2Quote(simpleSourceTickerQuoteInfo);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop];
            Assert.AreEqual(expectedPrice, foundLayer!.Price);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsPriceUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var priceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook))
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, priceVolumeLayer));
            var indexFromTop = (isBid ? emptyQuote.BidBook : emptyQuote.AskBook).AllLayers
                .Select((pvl, i) => new { i, pvl }).Where(indexPvl => ReferenceEquals(indexPvl.pvl,
                    priceVolumeLayer))
                .Select(indexPvl => indexPvl.i).FirstOrDefault();

            Assert.IsFalse(priceVolumeLayer!.IsVolumeUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.AreEqual(0m, priceVolumeLayer.Volume);
            Assert.AreEqual(0, priceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

            var expectedVolume = 40_000_000.12m;
            priceVolumeLayer.Volume = expectedVolume;
            Assert.IsTrue(priceVolumeLayer.IsVolumeUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.AreEqual(expectedVolume, priceVolumeLayer.Volume);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = priceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerVolumeOffset,
                expectedVolume, 6);
            var expectedSideAdjustedLayerField = new PQFieldUpdate(
                (byte)(PQFieldKeys.LayerVolumeOffset + indexFromTop),
                expectedLayerField.Value, (byte)((isBid ? 0 : PQFieldFlags.IsAskSideFlag) | 6));
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            priceVolumeLayer.IsVolumeUpdated = false;
            Assert.IsFalse(priceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            priceVolumeLayer.IsVolumeUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                    where update.Id == PQFieldKeys.LayerVolumeOffset + indexFromTop
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            priceVolumeLayer.Volume = 0m;
            priceVolumeLayer.IsVolumeUpdated = false;

            var newEmpty = new PQLevel2Quote(simpleSourceTickerQuoteInfo);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop];
            Assert.AreEqual(expectedVolume, foundLayer!.Volume);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsVolumeUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerSourceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var nameCounter = 0;
            foreach (var sourcePriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                         .OfType<IPQSourcePriceVolumeLayer>())
            {
                nameCounter++;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, sourcePriceVolumeLayer));
                var indexFromTop = (isBid ? emptyQuote.BidBook : emptyQuote.AskBook).AllLayers
                    .Select((pvl, i) => new { i, pvl }).Where(indexPvl =>
                        ReferenceEquals(indexPvl.pvl, sourcePriceVolumeLayer))
                    .Select(indexPvl => indexPvl.i).FirstOrDefault();

                Assert.IsFalse(sourcePriceVolumeLayer.IsSourceNameUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(null, sourcePriceVolumeLayer.SourceName);
                Assert.AreEqual(0, sourcePriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                    .Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

                var expectedSourceName = "NewChangedSourceName" + nameCounter;
                sourcePriceVolumeLayer.SourceName = expectedSourceName;
                Assert.IsTrue(sourcePriceVolumeLayer.IsSourceNameUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedSourceName, sourcePriceVolumeLayer.SourceName);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = sourcePriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                    .ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset,
                    sourcePriceVolumeLayer.SourceNameIdLookup[sourcePriceVolumeLayer.SourceName]);
                var expectedSideAdjustedLayerField = new PQFieldUpdate(
                    (byte)(PQFieldKeys.LayerSourceIdOffset + indexFromTop),
                    expectedLayerField.Value, (byte)(isBid ? 0 : PQFieldFlags.IsAskSideFlag));
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);
                var stringUpdates = sourcePriceVolumeLayer.GetStringUpdates(testDateTime, UpdateStyle.Updates)
                    .ToList();
                Assert.AreEqual(1, stringUpdates.Count);
                var expectedStringUpdates = new PQFieldStringUpdate
                {
                    Field = new PQFieldUpdate(
                        PQFieldKeys.LayerNameDictionaryUpsertCommand, 0u, 1 | PQFieldFlags.IsUpdate)
                    , StringUpdate = new PQStringUpdate
                    {
                        Command = CrudCommand.Update
                        , DictionaryId = sourcePriceVolumeLayer.SourceNameIdLookup[sourcePriceVolumeLayer.SourceName]
                        , Value = expectedSourceName
                    }
                };
                Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

                sourcePriceVolumeLayer.IsSourceNameUpdated = false;
                sourcePriceVolumeLayer.SourceNameIdLookup.HasUpdates = false;
                Assert.IsFalse(sourcePriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

                sourcePriceVolumeLayer.IsSourceNameUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                        where update.Id == PQFieldKeys.LayerSourceIdOffset + indexFromTop
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                sourcePriceVolumeLayer.SourceName = null;
                sourcePriceVolumeLayer.IsSourceNameUpdated = false;

                var diffNameIdLookupSrcTkrQtInfo =
                    new PQSourceTickerQuoteInfo(emptyQuote.SourceTickerQuoteInfo!)
                    {
                        SourceNameIdLookup = new PQNameIdLookupGenerator(
                            PQFieldKeys.LayerNameDictionaryUpsertCommand,
                            PQFieldFlags.SourceNameIdLookupSubDictionaryKey)
                        , TraderNameIdLookup = new PQNameIdLookupGenerator(
                            PQFieldKeys.LayerNameDictionaryUpsertCommand,
                            PQFieldFlags.TraderNameIdLookupSubDictionaryKey)
                    };
                var newEmpty = new PQLevel2Quote(diffNameIdLookupSrcTkrQtInfo);
                newEmpty.UpdateField(quoteUpdates[0]);
                newEmpty.UpdateFieldString(stringUpdates[0]);
                var foundLayer
                    = (IPQSourcePriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                Assert.AreEqual(expectedSourceName, foundLayer!.SourceName);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundLayer.IsSourceNameUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerExecutableChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var srcPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                     .OfType<IPQSourcePriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, srcPriceVolumeLayer));
            var indexFromTop = (isBid ? emptyQuote.BidBook : emptyQuote.AskBook).AllLayers
                .Select((pvl, i) => new { i, pvl }).Where(indexPvl =>
                    ReferenceEquals(indexPvl.pvl, srcPriceVolumeLayer))
                .Select(indexPvl => indexPvl.i).FirstOrDefault();

            Assert.IsFalse(srcPriceVolumeLayer.IsExecutableUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsFalse(srcPriceVolumeLayer.Executable);
            Assert.AreEqual(0, srcPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

            srcPriceVolumeLayer.Executable = true;
            Assert.IsTrue(srcPriceVolumeLayer.IsExecutableUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.IsTrue(srcPriceVolumeLayer.Executable);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = srcPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerBooleanFlagsOffset,
                PQFieldFlags.LayerExecutableFlag);
            var expectedSideAdjustedLayerField = new PQFieldUpdate(
                (byte)(PQFieldKeys.LayerBooleanFlagsOffset + indexFromTop),
                PQFieldFlags.LayerExecutableFlag, (byte)(isBid ? 0 : PQFieldFlags.IsAskSideFlag));
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            srcPriceVolumeLayer.IsExecutableUpdated = false;
            Assert.IsFalse(srcPriceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            srcPriceVolumeLayer.IsExecutableUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                    where update.Id == PQFieldKeys.LayerBooleanFlagsOffset + indexFromTop
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            srcPriceVolumeLayer.Executable = false;
            srcPriceVolumeLayer.IsExecutableUpdated = false;

            var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerQuoteInfo!);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (IPQSourcePriceVolumeLayer)
                (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
            Assert.IsTrue(foundLayer.Executable);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsExecutableUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerSourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var srcQtRefPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                     .OfType<IPQSourceQuoteRefPriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl,
                srcQtRefPriceVolumeLayer));
            var indexFromTop = (isBid ? emptyQuote.BidBook : emptyQuote.AskBook).AllLayers
                .Select((pvl, i) => new { i, pvl }).Where(indexPvl =>
                    ReferenceEquals(indexPvl.pvl, srcQtRefPriceVolumeLayer))
                .Select(indexPvl => indexPvl.i).FirstOrDefault();

            Assert.IsFalse(srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.AreEqual(0u, srcQtRefPriceVolumeLayer.SourceQuoteReference);
            Assert.AreEqual(0, srcQtRefPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

            var expectedSourceQuoteRef = 246813u;
            srcQtRefPriceVolumeLayer.SourceQuoteReference = expectedSourceQuoteRef;
            Assert.IsTrue(srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.AreEqual(expectedSourceQuoteRef, srcQtRefPriceVolumeLayer.SourceQuoteReference);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = srcQtRefPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerSourceQuoteRefOffset,
                expectedSourceQuoteRef);
            var expectedSideAdjustedLayerField = new PQFieldUpdate(
                (byte)(PQFieldKeys.LayerSourceQuoteRefOffset + indexFromTop),
                expectedLayerField.Value, (byte)(isBid ? 0 : PQFieldFlags.IsAskSideFlag));
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated = false;
            Assert.IsFalse(srcQtRefPriceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                    where update.Id == PQFieldKeys.LayerSourceQuoteRefOffset + indexFromTop
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            srcQtRefPriceVolumeLayer.SourceQuoteReference = 0u;
            srcQtRefPriceVolumeLayer.IsSourceQuoteReferenceUpdated = false;

            var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerQuoteInfo!);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (IPQSourceQuoteRefPriceVolumeLayer)
                (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
            Assert.AreEqual(expectedSourceQuoteRef, foundLayer.SourceQuoteReference);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsSourceQuoteReferenceUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerValueDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var srcQtRefPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                     .OfType<IPQValueDatePriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers
                .Any(pvl => ReferenceEquals(pvl, srcQtRefPriceVolumeLayer));
            var indexFromTop = (isBid ? emptyQuote.BidBook : emptyQuote.AskBook).AllLayers
                .Select((pvl, i) => new { i, pvl }).Where(indexPvl =>
                    ReferenceEquals(indexPvl.pvl, srcQtRefPriceVolumeLayer))
                .Select(indexPvl => indexPvl.i).FirstOrDefault();

            Assert.IsFalse(srcQtRefPriceVolumeLayer.IsValueDateUpdated);
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, srcQtRefPriceVolumeLayer.ValueDate);
            Assert.AreEqual(0, srcQtRefPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

            var expectedValueDate = new DateTime(2017, 12, 03, 19, 0, 0); //only to the nearest hour
            srcQtRefPriceVolumeLayer.ValueDate = expectedValueDate;
            Assert.IsTrue(srcQtRefPriceVolumeLayer.IsValueDateUpdated);
            Assert.IsTrue(emptyQuote.HasUpdates);
            Assert.AreEqual(expectedValueDate, srcQtRefPriceVolumeLayer.ValueDate);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = srcQtRefPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                .ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var hoursSinceUnixEpoch = expectedValueDate.GetHoursFromUnixEpoch();
            var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerDateOffset,
                hoursSinceUnixEpoch, PQFieldFlags.IsExtendedFieldId);
            var expectedSideAdjustedLayerField = new PQFieldUpdate(
                (ushort)(PQFieldKeys.LayerDateOffset + indexFromTop),
                expectedLayerField.Value, (byte)((isBid ? 0 : PQFieldFlags.IsAskSideFlag) |
                                                 PQFieldFlags.IsExtendedFieldId));
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            srcQtRefPriceVolumeLayer.IsValueDateUpdated = false;
            Assert.IsFalse(srcQtRefPriceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            srcQtRefPriceVolumeLayer.IsValueDateUpdated = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                    where update.Id == PQFieldKeys.LayerDateOffset + indexFromTop
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            srcQtRefPriceVolumeLayer.ValueDate = DateTimeConstants.UnixEpoch;
            srcQtRefPriceVolumeLayer.IsValueDateUpdated = false;

            var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerQuoteInfo!);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer = (IPQValueDatePriceVolumeLayer)
                (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
            Assert.AreEqual(expectedValueDate, foundLayer.ValueDate);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.IsValueDateUpdated);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerTraderCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var traderPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                     .OfType<IPQTraderPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, traderPriceVolumeLayer));
            var indexFromTop = (isBid ? emptyQuote.BidBook : emptyQuote.AskBook).AllLayers
                .Select((pvl, i) => new { i, pvl }).Where(indexPvl =>
                    ReferenceEquals(indexPvl.pvl, traderPriceVolumeLayer))
                .Select(indexPvl => indexPvl.i).FirstOrDefault();

            testDateTime = testDateTime.AddHours(1).AddMinutes(1);

            Assert.IsTrue(traderPriceVolumeLayer.IsTraderCountOnly);
            Assert.IsFalse(traderPriceVolumeLayer.HasUpdates);
            Assert.AreEqual(0,
                traderPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                    .Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

            traderPriceVolumeLayer.SetTradersCountOnly(byte.MaxValue);
            Assert.IsTrue(traderPriceVolumeLayer.HasUpdates);
            Assert.AreEqual(byte.MaxValue, traderPriceVolumeLayer.Count);
            Assert.IsTrue(emptyQuote.HasUpdates);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = traderPriceVolumeLayer
                .GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset,
                0x00800000 | byte.MaxValue);
            var expectedSideAdjustedLayerField = new PQFieldUpdate(
                (ushort)(PQFieldKeys.LayerTraderIdOffset + indexFromTop),
                0x00800000 | byte.MaxValue, (byte)(isBid ? 0 : PQFieldFlags.IsAskSideFlag));
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            traderPriceVolumeLayer.HasUpdates = false;
            Assert.IsFalse(traderPriceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated = false;
            emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(
                emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

            traderPriceVolumeLayer.HasUpdates = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                    where update.Id == PQFieldKeys.LayerTraderIdOffset + indexFromTop
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            traderPriceVolumeLayer.SetTradersCountOnly(0);
            traderPriceVolumeLayer.HasUpdates = false;

            var diffNameIdLookupSrcTkrQtInfo =
                new PQSourceTickerQuoteInfo(emptyQuote.SourceTickerQuoteInfo!)
                {
                    SourceNameIdLookup = new PQNameIdLookupGenerator(
                        PQFieldKeys.LayerNameDictionaryUpsertCommand,
                        PQFieldFlags.SourceNameIdLookupSubDictionaryKey)
                    , TraderNameIdLookup = new PQNameIdLookupGenerator(
                        PQFieldKeys.LayerNameDictionaryUpsertCommand,
                        PQFieldFlags.TraderNameIdLookupSubDictionaryKey)
                };
            var newEmpty = new PQLevel2Quote(diffNameIdLookupSrcTkrQtInfo);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer =
                (IPQTraderPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
            Assert.AreEqual(byte.MaxValue, foundLayer.Count);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var nameCounter = 0;
            foreach (var traderPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                         .OfType<IPQTraderPriceVolumeLayer>())
            {
                var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, traderPriceVolumeLayer));
                var indexFromTop = (isBid ? emptyQuote.BidBook : emptyQuote.AskBook).AllLayers
                    .Select((pvl, i) => new { i, pvl }).Where(indexPvl =>
                        ReferenceEquals(indexPvl.pvl, traderPriceVolumeLayer))
                    .Select(indexPvl => indexPvl.i).FirstOrDefault();

                for (var i = 0; i < 256; i++)
                {
                    nameCounter++;
                    if (i == 10) i = 254;
                    testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                    var traderLayerInfo = traderPriceVolumeLayer[i]!;

                    Assert.IsFalse(traderLayerInfo.IsTraderNameUpdated);
                    Assert.IsFalse(emptyQuote.HasUpdates);
                    Assert.AreEqual(null, traderLayerInfo.TraderName);
                    Assert.AreEqual(0,
                        traderPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                            .Count());
                    Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

                    var expectedTraderName = "NewChangedTraderName" + nameCounter;
                    traderLayerInfo.TraderName = expectedTraderName;
                    Assert.IsTrue(traderLayerInfo.IsTraderNameUpdated);
                    Assert.IsTrue(emptyQuote.HasUpdates);
                    Assert.AreEqual(expectedTraderName, traderLayerInfo.TraderName);
                    var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
                    Assert.AreEqual(3, quoteUpdates.Count);
                    var layerUpdates = traderPriceVolumeLayer
                        .GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
                    Assert.AreEqual(1, layerUpdates.Count);
                    var indexShifted = i << 24;
                    var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset,
                        indexShifted | traderPriceVolumeLayer.TraderNameIdLookup[traderLayerInfo.TraderName]);
                    var expectedSideAdjustedLayerField = new PQFieldUpdate(
                        (byte)(PQFieldKeys.LayerTraderIdOffset + indexFromTop),
                        expectedLayerField.Value, (byte)(isBid ? 0 : PQFieldFlags.IsAskSideFlag));
                    Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                    Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);
                    var stringUpdates = traderPriceVolumeLayer.GetStringUpdates(testDateTime, UpdateStyle.Updates)
                        .ToList();
                    Assert.AreEqual(1, stringUpdates.Count);
                    var expectedStringUpdates = new PQFieldStringUpdate
                    {
                        Field = new PQFieldUpdate(
                            PQFieldKeys.LayerNameDictionaryUpsertCommand, 0u, 2 | PQFieldFlags.IsUpdate)
                        , StringUpdate = new PQStringUpdate
                        {
                            Command = CrudCommand.Update
                            , DictionaryId = traderPriceVolumeLayer.TraderNameIdLookup[traderLayerInfo.TraderName]
                            , Value = expectedTraderName
                        }
                    };
                    Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

                    traderLayerInfo.IsTraderNameUpdated = false;
                    traderLayerInfo.TraderNameIdLookup.HasUpdates = false;
                    Assert.IsFalse(traderPriceVolumeLayer.HasUpdates);
                    Assert.IsTrue(emptyQuote.HasUpdates);
                    emptyQuote.IsAdapterSentTimeDateUpdated = false;
                    emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                    Assert.IsFalse(emptyQuote.HasUpdates);
                    Assert.IsTrue(
                        emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

                    traderLayerInfo.IsTraderNameUpdated = true;
                    quoteUpdates =
                        (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                            where update.Id == PQFieldKeys.LayerTraderIdOffset + indexFromTop
                            select update).ToList();
                    Assert.AreEqual(1, quoteUpdates.Count);
                    Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                    traderLayerInfo.TraderName = null;
                    traderLayerInfo.IsTraderNameUpdated = false;

                    var diffNameIdLookupSrcTkrQtInfo =
                        new PQSourceTickerQuoteInfo(emptyQuote.SourceTickerQuoteInfo!)
                        {
                            SourceNameIdLookup = new PQNameIdLookupGenerator(
                                PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                PQFieldFlags.SourceNameIdLookupSubDictionaryKey)
                            , TraderNameIdLookup = new PQNameIdLookupGenerator(
                                PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                PQFieldFlags.TraderNameIdLookupSubDictionaryKey)
                        };
                    var newEmpty = new PQLevel2Quote(diffNameIdLookupSrcTkrQtInfo);
                    newEmpty.UpdateField(quoteUpdates[0]);
                    newEmpty.UpdateFieldString(stringUpdates[0]);
                    var foundLayer =
                        (IPQTraderPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                    var foundTraderInfo = foundLayer[i]!;
                    Assert.AreEqual(expectedTraderName, foundTraderInfo.TraderName);
                    Assert.IsTrue(newEmpty.HasUpdates);
                    Assert.IsTrue(foundTraderInfo.HasUpdates);
                    Assert.IsTrue(foundTraderInfo.IsTraderNameUpdated);
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel2QuoteTypes_LayerTraderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var traderPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                     .OfType<IPQTraderPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, traderPriceVolumeLayer));
            var indexFromTop = (isBid ? emptyQuote.BidBook : emptyQuote.AskBook).AllLayers
                .Select((pvl, i) => new { i, pvl }).Where(indexPvl =>
                    ReferenceEquals(indexPvl.pvl, traderPriceVolumeLayer))
                .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 10) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var traderLayerInfo = traderPriceVolumeLayer[i]!;

                Assert.IsFalse(traderLayerInfo.IsTraderVolumeUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0m, traderLayerInfo.TraderVolume);
                Assert.AreEqual(0,
                    traderPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                        .Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

                var expectedTraderVolume = 254682m;
                traderLayerInfo.TraderVolume = expectedTraderVolume;
                Assert.IsTrue(traderLayerInfo.IsTraderVolumeUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedTraderVolume, traderLayerInfo.TraderVolume);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = traderPriceVolumeLayer
                    .GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var indexShifted = i << 24;
                var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerTraderVolumeOffset,
                    indexShifted | (int)expectedTraderVolume, 8);
                var expectedSideAdjustedLayerField = new PQFieldUpdate(
                    (byte)(PQFieldKeys.LayerTraderVolumeOffset + indexFromTop),
                    expectedLayerField.Value,
                    (byte)((isBid ? 0 : PQFieldFlags.IsAskSideFlag) | expectedLayerField.Flag));
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

                traderLayerInfo.IsTraderVolumeUpdated = false;
                Assert.IsFalse(traderPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(
                    emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

                traderLayerInfo.IsTraderVolumeUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                        where update.Id == PQFieldKeys.LayerTraderVolumeOffset + indexFromTop
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                traderLayerInfo.TraderVolume = 0m;
                traderLayerInfo.IsTraderVolumeUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerQuoteInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer =
                    (IPQTraderPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundTraderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedTraderVolume, foundTraderInfo.TraderVolume);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundTraderInfo.HasUpdates);
                Assert.IsTrue(foundTraderInfo.IsTraderVolumeUpdated);
            }
        }
    }

    [TestMethod]
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel2Fields()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var pqFieldUpdates = populatedL2Quote.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 12, 33, 1), UpdateStyle.Updates).ToList();
            AssertContainsAllLevel2Fields(pqFieldUpdates, populatedL2Quote);
        }
    }

    [TestMethod]
    public void TraderPopulatedWithUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel2Fields()
    {
        for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxBookDepth; i++)
        {
            ((IPQTraderPriceVolumeLayer)traderDetailsFullyPopulatedLevel2Quote.AskBook[i]!).SetTradersCountOnly(i);
            ((IPQTraderPriceVolumeLayer)traderDetailsFullyPopulatedLevel2Quote.BidBook[i]!)
                .SetTradersCountOnly(20 - i);
        }

        var pqFieldUpdates = traderDetailsFullyPopulatedLevel2Quote.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 12, 33, 1), UpdateStyle.Updates).ToList();
        AssertContainsAllLevel2Fields(pqFieldUpdates, traderDetailsFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel2Fields()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            populatedL2Quote.HasUpdates = false;
            var pqFieldUpdates = populatedL2Quote.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 12, 33, 1), UpdateStyle.FullSnapshot).ToList();
            AssertContainsAllLevel2Fields(pqFieldUpdates, populatedL2Quote);
        }
    }

    [TestMethod]
    public void TraderPopulatedWithUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel2Fields()
    {
        for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxBookDepth; i++)
        {
            ((IPQTraderPriceVolumeLayer)traderDetailsFullyPopulatedLevel2Quote.AskBook[i]!).SetTradersCountOnly(i);
            ((IPQTraderPriceVolumeLayer)traderDetailsFullyPopulatedLevel2Quote.BidBook[i]!)
                .SetTradersCountOnly(20 - i);
        }

        traderDetailsFullyPopulatedLevel2Quote.HasUpdates = false;
        var pqFieldUpdates = traderDetailsFullyPopulatedLevel2Quote.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 12, 33, 1), UpdateStyle.FullSnapshot).ToList();
        AssertContainsAllLevel2Fields(pqFieldUpdates, traderDetailsFullyPopulatedLevel2Quote);
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            populatedL2Quote.IsReplay = true;
            populatedL2Quote.HasUpdates = false;
            var pqFieldUpdates = populatedL2Quote.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            var pqStringUpdates = populatedL2Quote.GetStringUpdates(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            Assert.AreEqual(0, pqFieldUpdates.Count);
            Assert.AreEqual(0, pqStringUpdates.Count);
        }
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var pqFieldUpdates = populatedL2Quote.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 13, 33, 3), UpdateStyle.Updates | UpdateStyle.Replay).ToList();
            var pqStringUpdates = populatedL2Quote.GetStringUpdates(
                new DateTime(2017, 11, 04, 13, 33, 3), UpdateStyle.Updates | UpdateStyle.Replay).ToList();
            var emptyQuoteSourceTickerQuoteInfo
                = new PQSourceTickerQuoteInfo(populatedL2Quote.SourceTickerQuoteInfo!)
                {
                    SourceNameIdLookup = new PQNameIdLookupGenerator(
                        PQFieldKeys.LayerNameDictionaryUpsertCommand,
                        PQFieldFlags.SourceNameIdLookupSubDictionaryKey)
                    , TraderNameIdLookup = new PQNameIdLookupGenerator(
                        PQFieldKeys.LayerNameDictionaryUpsertCommand,
                        PQFieldFlags.TraderNameIdLookupSubDictionaryKey)
                };
            var newEmpty = new PQLevel2Quote(emptyQuoteSourceTickerQuoteInfo);
            foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
            foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
            // not copied from field updates as is used in by server to track publication times.
            newEmpty.LastPublicationTime = populatedL2Quote.LastPublicationTime;
            Assert.AreEqual(populatedL2Quote, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var emptyQuoteSourceTickerQuoteInfo
                = new PQSourceTickerQuoteInfo(populatedL2Quote.SourceTickerQuoteInfo!);
            var newEmpty = new PQLevel2Quote(emptyQuoteSourceTickerQuoteInfo);
            newEmpty.CopyFrom(populatedL2Quote);
            Assert.AreEqual(populatedL2Quote, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var emptyQuoteSourceTickerQuoteInfo
                = new PQSourceTickerQuoteInfo(populatedL2Quote.SourceTickerQuoteInfo!);
            var emptyQuote = new PQLevel2Quote(emptyQuoteSourceTickerQuoteInfo);
            populatedL2Quote.HasUpdates = false;
            emptyQuote.CopyFrom(populatedL2Quote);
            Assert.AreEqual(populatedL2Quote.PQSequenceId, emptyQuote.PQSequenceId);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
            Assert.IsTrue(populatedL2Quote.SourceTickerQuoteInfo!.AreEquivalent(emptyQuote.SourceTickerQuoteInfo));
            Assert.AreEqual(false, emptyQuote.IsReplay);
            Assert.AreEqual(0m, emptyQuote.SinglePrice);
            Assert.AreEqual(PQSyncStatus.OutOfSync, emptyQuote.PQSyncStatus);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceBidTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceAskTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterReceivedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterSentTime);
            Assert.AreEqual(0m, emptyQuote.BidPriceTop);
            Assert.AreEqual(0m, emptyQuote.AskPriceTop);
            Assert.IsTrue(emptyQuote.Executable);
            Assert.AreEqual(populatedL2Quote.ClientReceivedTime, emptyQuote.ClientReceivedTime);
            Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsReplayUpdated);
            Assert.IsFalse(emptyQuote.IsSinglePriceUpdated);
            Assert.IsFalse(emptyQuote.IsSyncStatusUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
            Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
            Assert.IsFalse(emptyQuote.IsExecutableUpdated);
            foreach (var pvl in emptyQuote.BidBook) AssertAreDefaultValues(pvl);
            foreach (var pvl in emptyQuote.AskBook) AssertAreDefaultValues(pvl);
        }
    }

    [TestMethod]
    public void NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var nonPQLevel2Quote = new Level2PriceQuote(populatedL2Quote);
            var emptyQuoteSourceTickerQuoteInfo
                = new PQSourceTickerQuoteInfo(populatedL2Quote.SourceTickerQuoteInfo!);
            var newEmpty = new PQLevel2Quote(emptyQuoteSourceTickerQuoteInfo);
            newEmpty.CopyFrom(nonPQLevel2Quote);
            Assert.IsTrue(populatedL2Quote.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var clonedQuote = ((ICloneable<ILevel0Quote>)populatedL2Quote).Clone();
            Assert.AreNotSame(clonedQuote, populatedL2Quote);
            if (!clonedQuote.Equals(populatedL2Quote))
                Console.Out.WriteLine("clonedQuote differences are \n"
                                      + clonedQuote.DiffQuotes(populatedL2Quote) + "'");
            Assert.AreEqual(populatedL2Quote, clonedQuote);

            var cloned2 = (PQLevel2Quote)((ICloneable)populatedL2Quote).Clone();
            Assert.AreNotSame(cloned2, populatedL2Quote);
            if (!cloned2.Equals(populatedL2Quote))
                Console.Out.WriteLine("clonedQuote differences are \n '"
                                      + cloned2.DiffQuotes(populatedL2Quote) + "'");
            Assert.AreEqual(populatedL2Quote, cloned2);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var fullyPopulatedClone = (PQLevel2Quote)((ICloneable)populatedL2Quote).Clone();
            // by default SourceTickerQuoteInfo is shared
            fullyPopulatedClone.SourceTickerQuoteInfo
                = new PQSourceTickerQuoteInfo(populatedL2Quote.SourceTickerQuoteInfo!);

            AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedL2Quote,
                fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedL2Quote,
                fullyPopulatedClone);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            Assert.AreEqual(populatedL2Quote, populatedL2Quote);
            Assert.AreEqual(populatedL2Quote, ((ICloneable)populatedL2Quote).Clone());
            Assert.AreEqual(populatedL2Quote, ((ICloneable<ILevel0Quote>)populatedL2Quote).Clone());
            Assert.AreEqual(populatedL2Quote, ((ICloneable<ILevel1Quote>)populatedL2Quote).Clone());
            Assert.AreEqual(populatedL2Quote, ((ICloneable<ILevel2Quote>)populatedL2Quote).Clone());
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_GetHashCode_ReturnNumberNoException()
    {
        foreach (var populatedL2Quote in allFullyPopulatedQuotes)
        {
            var hashCode = populatedL2Quote.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        PQLevel2Quote original, PQLevel2Quote changingLevel2Quote)
    {
        PQLevel1QuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison, original,
            changingLevel2Quote);

        if (original.GetType() == typeof(PQLevel2Quote))
            Assert.AreEqual(!exactComparison,
                changingLevel2Quote.AreEquivalent(new Level2PriceQuote(original), exactComparison));

        PQOrderBookTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            (PQOrderBook)original.BidBook, (PQOrderBook)changingLevel2Quote.BidBook,
            original, changingLevel2Quote);

        PQOrderBookTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            (PQOrderBook)original.AskBook, (PQOrderBook)changingLevel2Quote.AskBook,
            original, changingLevel2Quote);
    }

    public static void AssertContainsAllLevel2Fields(IList<PQFieldUpdate> checkFieldUpdates,
        PQLevel2Quote l2Q, uint expectedBooleanFlags = 3)
    {
        PQLevel1QuoteTests.AssertContainsAllLevel1Fields(checkFieldUpdates, l2Q, expectedBooleanFlags);

        for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxBookDepth; i++)
        {
            var bidL2Pvl = l2Q.BidBook[i]!;
            var askL2Pvl = l2Q.AskBook[i]!;

            Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerPriceOffset + i), bidL2Pvl.Price, 1),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    (byte)(PQFieldKeys.LayerPriceOffset + i), 1), $"For bidlayer {bidL2Pvl.GetType().Name} level {i}");
            Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerPriceOffset + i), askL2Pvl.Price,
                    1 | PQFieldFlags.IsAskSideFlag),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    (byte)(PQFieldKeys.LayerPriceOffset + i), 1 | PQFieldFlags.IsAskSideFlag),
                $"For asklayer {bidL2Pvl.GetType().Name} level {i}");
            Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerVolumeOffset + i), bidL2Pvl.Volume, 6),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    (byte)(PQFieldKeys.LayerVolumeOffset + i), 6),
                $"For bidlayer {bidL2Pvl.GetType().Name} level {i}");
            Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerVolumeOffset + i), askL2Pvl.Volume,
                    6 | PQFieldFlags.IsAskSideFlag),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    (byte)(PQFieldKeys.LayerVolumeOffset + i), 6 | PQFieldFlags.IsAskSideFlag),
                $"For asklayer {bidL2Pvl.GetType().Name} level {i}");

            if (bidL2Pvl is IPQSourcePriceVolumeLayer pqBidL2Pvl &&
                askL2Pvl is IPQSourcePriceVolumeLayer pqAskL2Pvl)
                AssertSourceContainsAllFields(checkFieldUpdates, i, pqBidL2Pvl, pqAskL2Pvl);

            if (bidL2Pvl is IPQSourceQuoteRefPriceVolumeLayer bidSrcQtRefPvl &&
                askL2Pvl is IPQSourceQuoteRefPriceVolumeLayer askSrcQtRefPvl)
                AssertSourceQuoteRefContainsAllFields(checkFieldUpdates, i, bidSrcQtRefPvl, askSrcQtRefPvl);

            if (bidL2Pvl is IPQValueDatePriceVolumeLayer bidValueDatePvl &&
                askL2Pvl is IPQValueDatePriceVolumeLayer askValueDatePvl)
                AssertValueDateLayerContainsAllFields(checkFieldUpdates, i, bidValueDatePvl, askValueDatePvl);

            if (bidL2Pvl is IPQTraderPriceVolumeLayer bidTrdPvl &&
                askL2Pvl is IPQTraderPriceVolumeLayer askTrdPvl)
            {
                var nameIdLookup = bidTrdPvl.TraderNameIdLookup;
                AssertTraderLayerInfoIsExpected(checkFieldUpdates, bidTrdPvl, false, i, nameIdLookup);
                AssertTraderLayerInfoIsExpected(checkFieldUpdates, askTrdPvl, true, i, nameIdLookup);
            }
        }
    }

    public static PQFieldUpdate ExtractFieldUpdateWithId(IList<PQFieldUpdate> allUpdates, ushort id,
        uint value, uint bitMask, byte flag = 0)
    {
        return allUpdates.First(fu => fu.Id == id
                                      && (fu.Value & bitMask) == (value & bitMask)
                                      && fu.Flag == flag);
    }

    private void AssertLayerTypeIsExpected(Type expectedType, params PQLevel2Quote[] quotesToCheck)
    {
        foreach (var level2Quote in quotesToCheck)
            for (var i = 0; i < level2Quote.SourceTickerQuoteInfo!.MaximumPublishedLayers; i++)
            {
                Assert.AreEqual(expectedType, level2Quote.BidBook[i]!.GetType());
                Assert.AreEqual(expectedType, level2Quote.AskBook[i]!.GetType());
                switch (level2Quote.BidBook[i])
                {
                    case PQSourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvl:
                        Assert.IsTrue(ReferenceEquals(((IPQSourcePriceVolumeLayer)level2Quote.BidBook[0]!)
                            .SourceNameIdLookup, srcQtRefTrdrVlDtPvl.SourceNameIdLookup));
                        Assert.IsTrue(ReferenceEquals(((IPQTraderPriceVolumeLayer)level2Quote.BidBook[0]!)
                            .TraderNameIdLookup, srcQtRefTrdrVlDtPvl.TraderNameIdLookup));
                        break;
                    case PQSourcePriceVolumeLayer sourcePriceVolumeLayer:
                        Assert.IsTrue(ReferenceEquals(((IPQSourcePriceVolumeLayer)level2Quote.BidBook[0]!)
                            .SourceNameIdLookup, sourcePriceVolumeLayer.SourceNameIdLookup));
                        break;
                    case PQTraderPriceVolumeLayer traderPriceVolumeLayer:
                        Assert.IsTrue(ReferenceEquals(((IPQTraderPriceVolumeLayer)level2Quote.BidBook[0]!)
                            .TraderNameIdLookup, traderPriceVolumeLayer.TraderNameIdLookup));
                        break;
                }
            }
    }

    public static void AssertAreDefaultValues(IPQPriceVolumeLayer pvl)
    {
        Assert.AreEqual(0m, pvl.Price);
        Assert.AreEqual(0m, pvl.Volume);
        Assert.IsFalse(pvl.IsPriceUpdated);
        Assert.IsFalse(pvl.IsVolumeUpdated);
        if (pvl is IPQSourcePriceVolumeLayer sourcePvl)
        {
            Assert.AreEqual(null, sourcePvl.SourceName);
            Assert.IsFalse(sourcePvl.IsSourceNameUpdated);
        }

        if (pvl is IPQSourceQuoteRefPriceVolumeLayer sourceQtRefPvl)
        {
            Assert.AreEqual(0m, sourceQtRefPvl.SourceQuoteReference);
            Assert.IsFalse(sourceQtRefPvl.IsSourceQuoteReferenceUpdated);
        }

        if (pvl is IPQValueDatePriceVolumeLayer valueDatePvl)
        {
            Assert.AreEqual(DateTimeConstants.UnixEpoch, valueDatePvl.ValueDate);
            Assert.IsFalse(valueDatePvl.IsValueDateUpdated);
        }

        if (pvl is IPQTraderPriceVolumeLayer traderPvl) Assert.AreEqual(0, traderPvl.Count);
    }

    private static void AssertSourceContainsAllFields(IList<PQFieldUpdate> checkFieldUpdates, int i,
        IPQSourcePriceVolumeLayer pqBidL2Pvl,
        IPQSourcePriceVolumeLayer pqAskL2Pvl)
    {
        var bidSrcId = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
            (byte)(PQFieldKeys.LayerSourceIdOffset + i));
        var askSrcId = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
            (byte)(PQFieldKeys.LayerSourceIdOffset + i), PQFieldFlags.IsAskSideFlag);

        var nameIdLookup = pqBidL2Pvl.SourceNameIdLookup;
        Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerSourceIdOffset + i),
            nameIdLookup[pqBidL2Pvl.SourceName!]), bidSrcId);
        Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerSourceIdOffset + i),
            nameIdLookup[pqAskL2Pvl.SourceName!], PQFieldFlags.IsAskSideFlag), askSrcId);

        var bidSrcExecutable = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
            (byte)(PQFieldKeys.LayerBooleanFlagsOffset + i));
        var askSrcExecutable = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
            (byte)(PQFieldKeys.LayerBooleanFlagsOffset + i), PQFieldFlags.IsAskSideFlag);

        Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerBooleanFlagsOffset + i),
            pqBidL2Pvl.Executable ? PQFieldFlags.LayerExecutableFlag : 0), bidSrcExecutable);
        Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerBooleanFlagsOffset + i),
            pqBidL2Pvl.Executable ? PQFieldFlags.LayerExecutableFlag : 0,
            PQFieldFlags.IsAskSideFlag), askSrcExecutable);
    }

    private static void AssertSourceQuoteRefContainsAllFields(IList<PQFieldUpdate> checkFieldUpdates, int i,
        IPQSourceQuoteRefPriceVolumeLayer bidSrcQtRefPvl, IPQSourceQuoteRefPriceVolumeLayer askSrcQtRefPvl)
    {
        var bidSrcQtRef = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
            (byte)(PQFieldKeys.LayerSourceQuoteRefOffset + i));
        var askSrcQtRef = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
            (byte)(PQFieldKeys.LayerSourceQuoteRefOffset + i), PQFieldFlags.IsAskSideFlag);

        Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerSourceQuoteRefOffset + i),
            bidSrcQtRefPvl.SourceQuoteReference), bidSrcQtRef);
        Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerSourceQuoteRefOffset + i),
            askSrcQtRefPvl.SourceQuoteReference, PQFieldFlags.IsAskSideFlag), askSrcQtRef);
    }

    private static void AssertValueDateLayerContainsAllFields(IList<PQFieldUpdate> checkFieldUpdates, int i,
        IPQValueDatePriceVolumeLayer bidValueDatePvl, IPQValueDatePriceVolumeLayer askValueDatePvl)
    {
        var bidValueDate = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
            (ushort)(PQFieldKeys.LayerDateOffset + i), PQFieldFlags.IsExtendedFieldId);
        var askValueDate = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
            (ushort)(PQFieldKeys.LayerDateOffset + i), PQFieldFlags.IsAskSideFlag | PQFieldFlags.IsExtendedFieldId);

        var dateAsHoursFromEpoch = bidValueDatePvl.ValueDate.GetHoursFromUnixEpoch();
        Assert.AreEqual(new PQFieldUpdate((ushort)(PQFieldKeys.LayerDateOffset + i),
            dateAsHoursFromEpoch, PQFieldFlags.IsExtendedFieldId), bidValueDate);
        dateAsHoursFromEpoch = askValueDatePvl.ValueDate.GetHoursFromUnixEpoch();
        Assert.AreEqual(new PQFieldUpdate((ushort)(PQFieldKeys.LayerDateOffset + i),
            dateAsHoursFromEpoch, PQFieldFlags.IsAskSideFlag | PQFieldFlags.IsExtendedFieldId), askValueDate);
    }

    private static void AssertTraderLayerInfoIsExpected(IList<PQFieldUpdate> checkFieldUpdates,
        IPQTraderPriceVolumeLayer traderPvl, bool isAskSide, int bookIndex, IPQNameIdLookupGenerator nameIdLookup)
    {
        for (var j = 0; j < traderPvl.Count; j++)
        {
            var trdLayerInfo = traderPvl[j]!;
            var fieldPosIndex = (uint)j << 24;

            if (trdLayerInfo.TraderName == PQTraderPriceVolumeLayer.TraderCountTraderName)
            {
                Assert.AreEqual(new PQFieldUpdate((ushort)(PQFieldKeys.LayerTraderIdOffset + bookIndex),
                        0x0080_0000 | traderPvl.Count, isAskSide ? PQFieldFlags.IsAskSideFlag : (byte)0),
                    ExtractFieldUpdateWithId(checkFieldUpdates,
                        (ushort)(PQFieldKeys.LayerTraderIdOffset + bookIndex), 0x0080_0000, 0x0080_0000,
                        isAskSide ? PQFieldFlags.IsAskSideFlag : (byte)0),
                    $"For {traderPvl.GetType().Name} ");
                return;
            }

            var traderId = ExtractFieldUpdateWithId(checkFieldUpdates,
                (byte)(PQFieldKeys.LayerTraderIdOffset + bookIndex), fieldPosIndex, 0xFF80_0000,
                (byte)(isAskSide ? PQFieldFlags.IsAskSideFlag : 0));

            var traderVolume = ExtractFieldUpdateWithId(checkFieldUpdates,
                (byte)(PQFieldKeys.LayerTraderVolumeOffset + bookIndex), fieldPosIndex, 0xFF80_0000,
                (byte)(08 | (isAskSide ? PQFieldFlags.IsAskSideFlag : 0)));

            Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerTraderIdOffset + bookIndex),
                fieldPosIndex | (uint)nameIdLookup[trdLayerInfo.TraderName!],
                isAskSide ? PQFieldFlags.IsAskSideFlag : (byte)0), traderId);

            var value = PQScaling.AutoScale(trdLayerInfo.TraderVolume, 6, out var flag);
            Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerTraderVolumeOffset + bookIndex),
                value | fieldPosIndex,
                (byte)(flag | (isAskSide ? PQFieldFlags.IsAskSideFlag : 0))), traderVolume);
        }
    }
}
