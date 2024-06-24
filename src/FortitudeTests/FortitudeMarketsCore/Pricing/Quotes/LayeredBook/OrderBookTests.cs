// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

[TestClass]
public class OrderBookTests
{
    private const int MaxNumberOfLayers  = 19; // test being less than max.
    private const int MaxNumberOfTraders = 3;  // not too many traders.

    private OrderBook                                             allFieldsFullyPopulatedOrderBook = null!;
    private IList<ISourceQuoteRefTraderValueDatePriceVolumeLayer> allFieldsLayers                  = null!;
    private List<IReadOnlyList<IPriceVolumeLayer>>                allPopulatedLayers               = null!;

    private List<OrderBook>          allPopulatedOrderBooks       = null!;
    private IPQNameIdLookupGenerator emptyNameIdLookupGenerator   = null!;
    private ISourceTickerQuoteInfo   publicationPrecisionSettings = null!;

    private OrderBook                              simpleFullyPopulatedOrderBook      = null!;
    private IList<IPriceVolumeLayer>               simpleLayers                       = null!;
    private OrderBook                              sourceFullyPopulatedOrderBook      = null!;
    private IList<ISourcePriceVolumeLayer>         sourceLayers                       = null!;
    private OrderBook                              sourceQtRefFullyPopulatedOrderBook = null!;
    private IList<ISourceQuoteRefPriceVolumeLayer> sourceQtRefLayers                  = null!;
    private OrderBook                              traderFullyPopulatedOrderBook      = null!;
    private IList<ITraderPriceVolumeLayer>         traderLayers                       = null!;
    private OrderBook                              valueDateFullyPopulatedOrderBook   = null!;
    private IList<IValueDatePriceVolumeLayer>      valueDateLayers                    = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        simpleLayers      = new List<IPriceVolumeLayer>(MaxNumberOfLayers);
        sourceLayers      = new List<ISourcePriceVolumeLayer>(MaxNumberOfLayers);
        sourceQtRefLayers = new List<ISourceQuoteRefPriceVolumeLayer>(MaxNumberOfLayers);
        valueDateLayers   = new List<IValueDatePriceVolumeLayer>(MaxNumberOfLayers);
        traderLayers      = new List<ITraderPriceVolumeLayer>(MaxNumberOfLayers);
        allFieldsLayers   = new List<ISourceQuoteRefTraderValueDatePriceVolumeLayer>(MaxNumberOfLayers);

        allPopulatedLayers = new List<IReadOnlyList<IPriceVolumeLayer>>
        {
            (IReadOnlyList<IPriceVolumeLayer>)simpleLayers, (IReadOnlyList<IPriceVolumeLayer>)sourceLayers
          , (IReadOnlyList<IPriceVolumeLayer>)sourceQtRefLayers, (IReadOnlyList<IPriceVolumeLayer>)valueDateLayers
          , (IReadOnlyList<IPriceVolumeLayer>)traderLayers, (IReadOnlyList<IPriceVolumeLayer>)allFieldsLayers
        };

        for (var i = 0; i < MaxNumberOfLayers; i++)
        {
            simpleLayers.Add(new PriceVolumeLayer(1.234567m, 40_000_000m));
            var sourcePvl =
                new SourcePriceVolumeLayer(1.234567m, 40_000_000m, "TestSourceName", true);
            sourceLayers.Add(sourcePvl);
            var srcQtRefPvl = new SourceQuoteRefPriceVolumeLayer(1.234567m, 40_000_000m,
                                                                 "TestSourceName", true, 12345678u);
            sourceQtRefLayers.Add(srcQtRefPvl);
            valueDateLayers.Add(new ValueDatePriceVolumeLayer(1.234567m, 40_000_000m,
                                                              new DateTime(2017, 12, 09, 14, 0, 0)));
            var allFieldsPvL = new SourceQuoteRefTraderValueDatePriceVolumeLayer(
                                                                                 1.234567m, 40_000_000m, new DateTime(2017, 12, 09, 14, 0, 0)
                                                                               , "TestSourceName", true, 12345678u);
            allFieldsLayers.Add(allFieldsPvL);
            var traderPvL = new TraderPriceVolumeLayer(1.234567m, 40_000_000m);
            traderLayers.Add(traderPvL);
            for (var j = 0; j < MaxNumberOfTraders; j++)
            {
                allFieldsPvL.Add("Trdr" + j, 40_000_000m);
                traderPvL.Add("Trdr" + j, 40_000_000m);
            }
        }

        simpleFullyPopulatedOrderBook      = new OrderBook(BookSide.AskBook, (IReadOnlyList<IPriceVolumeLayer>)simpleLayers);
        sourceFullyPopulatedOrderBook      = new OrderBook(BookSide.BidBook, sourceLayers.ToList());
        sourceQtRefFullyPopulatedOrderBook = new OrderBook(BookSide.BidBook, sourceQtRefLayers.Cast<IPriceVolumeLayer>().ToList());
        traderFullyPopulatedOrderBook      = new OrderBook(BookSide.AskBook, traderLayers.Cast<IPriceVolumeLayer>().ToList());
        valueDateFullyPopulatedOrderBook   = new OrderBook(BookSide.AskBook, valueDateLayers.Cast<IPriceVolumeLayer>().ToList());

        allFieldsFullyPopulatedOrderBook = new OrderBook(BookSide.BidBook, allFieldsLayers.Cast<IPriceVolumeLayer>().ToList());

        allPopulatedOrderBooks = new List<OrderBook>
        {
            simpleFullyPopulatedOrderBook, sourceFullyPopulatedOrderBook, sourceQtRefFullyPopulatedOrderBook
          , valueDateFullyPopulatedOrderBook, traderFullyPopulatedOrderBook, allFieldsFullyPopulatedOrderBook
        };
        publicationPrecisionSettings = new SourceTickerQuoteInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , LayerFlags.Volume | LayerFlags.Price
           , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
    }

    [TestMethod]
    public void FromSourceTickerQuoteInfo_New_InitializesOrderBookWithExpectedLayerTypes()
    {
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var orderBook = new OrderBook(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable;
        orderBook = new OrderBook(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(SourcePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        orderBook = new OrderBook(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate;
        orderBook = new OrderBook(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(ValueDatePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.TraderName | LayerFlags.TraderCount | LayerFlags.TraderSize;
        orderBook = new OrderBook(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(TraderPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price.AllFlags();

        orderBook = new OrderBook(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
    }

    [TestMethod]
    public void PQLayers_New_ConvertsToExpectedEquivalent()
    {
        IList<IPriceVolumeLayer> pqList = new List<IPriceVolumeLayer>
        {
            new PQPriceVolumeLayer()
        };
        var orderBook = new OrderBook(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PriceVolumeLayer));

        pqList.Clear();
        pqList.Add(new PQSourcePriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBook(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(SourcePriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQSourceQuoteRefPriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBook(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(SourceQuoteRefPriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQValueDatePriceVolumeLayer());
        orderBook = new OrderBook(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(ValueDatePriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQTraderPriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBook(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(TraderPriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBook(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));

        orderBook = new OrderBook(BookSide.BidBook, Enumerable.Empty<IPriceVolumeLayer>());
        Assert.AreEqual(0, orderBook.Count);
    }

    [TestMethod]
    public void PQOrderBook_InitializedFromOrderBook_ConvertsLayers()
    {
        var pqSrcTkrQuoteInfo =
            new PQSourceTickerQuoteInfo(publicationPrecisionSettings)
            {
                LayerFlags = LayerFlags.Price | LayerFlags.Volume
            };
        var pqOrderBook = new PQOrderBook(BookSide.AskBook, pqSrcTkrQuoteInfo);
        var orderBook   = new OrderBook(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(PriceVolumeLayer));

        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable;
        pqOrderBook = new PQOrderBook(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBook(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(SourcePriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqOrderBook = new PQOrderBook(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBook(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefPriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate;
        pqOrderBook = new PQOrderBook(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBook(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(ValueDatePriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.TraderName | LayerFlags.TraderCount | LayerFlags.TraderSize;
        pqOrderBook = new PQOrderBook(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBook(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(TraderPriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags = LayerFlags.Price.AllFlags();

        pqOrderBook = new PQOrderBook(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBook(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
    }

    [TestMethod]
    public void NewOrderBook_InitializedWithLayers_ContainsSameInstanceLayersAsInitialized()
    {
        for (var i = 0; i < allPopulatedOrderBooks.Count; i++)
        {
            var populatedOrderBook = allPopulatedOrderBooks[i];
            var populatedLayers    = allPopulatedLayers[i];
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Count);
            for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreSame(populatedLayers[j], populatedOrderBook[j]);
        }
    }

    [TestMethod]
    public void NewOrderBook_InitializedFromOrderBook_ClonesAllLayers()
    {
        for (var i = 0; i < allPopulatedOrderBooks.Count; i++)
        {
            var populatedOrderBook = allPopulatedOrderBooks[i];
            var clonedOrderBook    = new OrderBook(populatedOrderBook);
            Assert.AreEqual(MaxNumberOfLayers, clonedOrderBook.Count);
            for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreNotSame(populatedOrderBook[j], clonedOrderBook[j]);
        }

        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var emptyOrderBook = new OrderBook(BookSide.AskBook, publicationPrecisionSettings);
        Assert.AreEqual(0, emptyOrderBook.Count);
        var clonedEmptyOrderBook = new OrderBook(emptyOrderBook);
        Assert.AreEqual(0, clonedEmptyOrderBook.Count);
        for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreNotSame(emptyOrderBook[j], clonedEmptyOrderBook[j]);
    }

    [TestMethod]
    public void PopulatedOrderBook_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
            for (var i = 0; i < MaxNumberOfLayers; i++)
            {
                var layer       = ((IOrderBook)populatedOrderBook)[i];
                var clonedLayer = (IMutablePriceVolumeLayer)layer!.Clone();
                populatedOrderBook[i] = clonedLayer;
                Assert.AreNotSame(layer, ((IMutableOrderBook)populatedOrderBook)[i]);
                Assert.AreSame(clonedLayer, populatedOrderBook[i]);
                if (i == populatedOrderBook.Count - 1)
                {
                    ((IMutableOrderBook)populatedOrderBook)[i] = null;
                    Assert.AreEqual(MaxNumberOfLayers - 1, populatedOrderBook.Count);
                }
            }
    }

    [TestMethod]
    public void PopulatedOrderBook_Capacity_ShowMaxPossibleNumberOfLayersNotNull()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            Assert.AreEqual(populatedOrderBook.Count, populatedOrderBook.Capacity);
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Capacity);
            populatedOrderBook[MaxNumberOfLayers - 1] = null;
            Assert.AreEqual(MaxNumberOfLayers - 1, populatedOrderBook.Capacity);
            Assert.AreEqual(populatedOrderBook.Count, populatedOrderBook.Capacity);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PopulatedOrderBook_CapacityLargerThanMaxBookDepth_ThrowsException()
    {
        simpleFullyPopulatedOrderBook.Capacity = PQFieldKeys.SingleByteFieldIdMaxBookDepth + 1;
    }

    [TestMethod]
    public void PopulatedOrderBook_Count_UpdatesWhenPricesChanged()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            for (var i = MaxNumberOfLayers - 1; i >= 0; i--)
            {
                Assert.AreEqual(i, populatedOrderBook.Count - 1);
                populatedOrderBook[i]?.StateReset();
            }

            Assert.AreEqual(0, populatedOrderBook.Count);
        }
    }

    [TestMethod]
    public void StaticDefault_LayerConverter_IsOrderBookLayerFactorySelector()
    {
        Assert.AreSame(typeof(OrderBookLayerFactorySelector), OrderBook.LayerSelector.GetType());
    }

    [TestMethod]
    public void PopulatedOrderBook_Reset_ResetsAllLayers()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Count);
            foreach (var pvl in populatedOrderBook) Assert.IsFalse(pvl.IsEmpty);
            populatedOrderBook.StateReset();
            Assert.AreEqual(0, populatedOrderBook.Count);
            foreach (var pvl in populatedOrderBook) Assert.IsTrue(pvl.IsEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromToEmptyQuote_OrderBooksEqualEachOther()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var newEmpty = new OrderBook(populatedOrderBook);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedOrderBook, newEmpty);
            newEmpty.CopyFrom(populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromSubTypes_SubTypeSaysIsEquivalent()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        foreach (var subType in allPopulatedOrderBooks.Where(ob => !ReferenceEquals(ob, populatedOrderBook)))
        {
            if (!WholeyContainedBy(subType[0]!.GetType(), populatedOrderBook[0]!.GetType())) continue;
            var newEmpty = new OrderBook(populatedOrderBook);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedOrderBook, newEmpty);
            newEmpty.CopyFrom(subType);
            Assert.IsTrue(subType.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromLessLayers_ReplicatesMissingValues()
    {
        var clonePopulated = simpleFullyPopulatedOrderBook.Clone();
        Assert.AreEqual(MaxNumberOfLayers, clonePopulated.Count);
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        Assert.AreEqual(MaxNumberOfLayers - 3, clonePopulated.Count);
        var notEmpty = new OrderBook(simpleFullyPopulatedOrderBook);
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(MaxNumberOfLayers - 3, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGap()
    {
        var clonePopulated = simpleFullyPopulatedOrderBook.Clone();
        Assert.AreEqual(MaxNumberOfLayers, clonePopulated.Count);
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[5]                        = null;
        Assert.AreEqual(MaxNumberOfLayers - 2, clonePopulated.Count);
        var notEmpty = new OrderBook(simpleFullyPopulatedOrderBook);
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], new PriceVolumeLayer()); // null copies to empty
        Assert.AreEqual(MaxNumberOfLayers - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap()
    {
        var clonePopulated = simpleFullyPopulatedOrderBook.Clone();
        Assert.AreEqual(MaxNumberOfLayers, clonePopulated.Count);
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        Assert.AreEqual(MaxNumberOfLayers - 2, clonePopulated.Count);
        var notEmpty = new OrderBook(simpleFullyPopulatedOrderBook) { [5] = null };
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfLayers - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var clonedOrderBook = populatedOrderBook.Clone();
            Assert.AreNotSame(clonedOrderBook, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clonedOrderBook);
            var cloned2 = ((ICloneable<IMutableOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned2, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned2);
            var cloned3 = ((IMutableOrderBook)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned3, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned3);
            var cloned4 = ((ICloneable<IOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned4, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned4);
            var cloned5 = ((ICloneable)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned5, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned5);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var fullyPopulatedClone = populatedOrderBook.Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedOrderBook,
                                                                fullyPopulatedClone);
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_GetHashCode_ReturnNumberNoException()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var hashCode = populatedOrderBook.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_ToString_ReturnsNameAndValues()
    {
        foreach (var populatedQuote in allPopulatedOrderBooks)
        {
            var q        = populatedQuote;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));
            Assert.IsTrue(toString.Contains($"{nameof(q.Capacity)}: {q.Capacity}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
            Assert.IsTrue(toString.Contains($"bookLayers:[{string.Join(", ", q)}]"));
        }
    }

    [TestMethod]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = allFieldsFullyPopulatedOrderBook;
        Assert.AreEqual(MaxNumberOfLayers, rt.Count);
        Assert.AreEqual(MaxNumberOfLayers, ((IEnumerable)rt).Cast<IPriceVolumeLayer>().Count());
        Assert.AreEqual(MaxNumberOfLayers, ((IEnumerable<IPriceVolumeLayer>)rt).Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, ((IEnumerable)rt).Cast<IPriceVolumeLayer>().Count());
        Assert.AreEqual(0, ((IEnumerable<IPriceVolumeLayer>)rt).Count());
    }

    private bool WholeyContainedBy(Type copySourceType, Type copyDestinationType)
    {
        if (copySourceType == typeof(PriceVolumeLayer)) return true;
        if (copySourceType == typeof(SourcePriceVolumeLayer))
            return copyDestinationType == typeof(SourcePriceVolumeLayer) ||
                   copyDestinationType == typeof(SourceQuoteRefPriceVolumeLayer) ||
                   copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
        if (copySourceType == typeof(SourceQuoteRefPriceVolumeLayer))
            return copyDestinationType == typeof(SourceQuoteRefPriceVolumeLayer) ||
                   copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
        if (copySourceType == typeof(ValueDatePriceVolumeLayer))
            return copyDestinationType == typeof(ValueDatePriceVolumeLayer) ||
                   copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
        if (copySourceType == typeof(TraderPriceVolumeLayer))
            return copyDestinationType == typeof(TraderPriceVolumeLayer) ||
                   copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
        if (copySourceType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer))
            return copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
        return false;
    }

    private void AssertBookHasLayersOfType(OrderBook orderBook, Type expectedType)
    {
        for (var i = 0; i < MaxNumberOfLayers; i++) Assert.IsInstanceOfType(orderBook[i], expectedType);
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableOrderBook commonOrderBook, IMutableOrderBook changingOrderBook,
        IMutableLevel2Quote? originalQuote = null, IMutableLevel2Quote? changingQuote = null)
    {
        Assert.AreEqual(commonOrderBook.Count, changingOrderBook.Count);

        for (var i = 0; i < commonOrderBook.Count; i++)
        {
            PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison, commonOrderBook[i],
                 changingOrderBook[i], commonOrderBook,
                 changingOrderBook, originalQuote, changingQuote
                );
            SourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBook[i] as IMutableSourcePriceVolumeLayer,
                 changingOrderBook[i] as IMutableSourcePriceVolumeLayer
               , commonOrderBook,
                 changingOrderBook, originalQuote, changingQuote
                );
            SourceQuoteRefPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBook[i] as
                     IMutableSourceQuoteRefPriceVolumeLayer,
                 changingOrderBook[i] as
                     IMutableSourceQuoteRefPriceVolumeLayer
               , commonOrderBook,
                 changingOrderBook, originalQuote, changingQuote
                );
            ValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBook[i] as ValueDatePriceVolumeLayer,
                 changingOrderBook[i] as ValueDatePriceVolumeLayer
               , commonOrderBook,
                 changingOrderBook, originalQuote, changingQuote
                );
            TraderPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBook[i] as IMutableTraderPriceVolumeLayer,
                 changingOrderBook[i] as IMutableTraderPriceVolumeLayer
               , commonOrderBook,
                 changingOrderBook, originalQuote, changingQuote
                );
            SourceQuoteRefTraderValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
             exactComparison, commonOrderBook[i] as SourceQuoteRefTraderValueDatePriceVolumeLayer,
             changingOrderBook[i] as SourceQuoteRefTraderValueDatePriceVolumeLayer, commonOrderBook,
             changingOrderBook, originalQuote, changingQuote);
        }
    }
}
