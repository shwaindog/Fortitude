// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerInfo.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

[TestClass]
public class OrderBookSideTests
{
    private const int           MaxNumberOfLayers                    = 19; // test being less than max.
    private const int           MaxNumberOfTraders                   = 3;  // not too many traders.
    private       OrderBookSide allFieldsFullyPopulatedOrderBookSide = null!;

    private IList<IFullSupportPriceVolumeLayer> allFieldsLayers    = null!;
    private List<IReadOnlyList<IPriceVolumeLayer>>                allPopulatedLayers = null!;

    private List<OrderBookSide> allPopulatedOrderBooks = null!;

    private IPQNameIdLookupGenerator emptyNameIdLookupGenerator   = null!;
    private ISourceTickerInfo        publicationPrecisionSettings = null!;

    private OrderBookSide                          simpleFullyPopulatedOrderBookSide      = null!;
    private IList<IPriceVolumeLayer>               simpleLayers                           = null!;
    private OrderBookSide                          sourceFullyPopulatedOrderBookSide      = null!;
    private IList<ISourcePriceVolumeLayer>         sourceLayers                           = null!;
    private OrderBookSide                          sourceQtRefFullyPopulatedOrderBookSide = null!;
    private IList<ISourceQuoteRefPriceVolumeLayer> sourceQtRefLayers                      = null!;
    private OrderBookSide                          traderFullyPopulatedOrderBookSide      = null!;
    private IList<IOrdersPriceVolumeLayer>         traderLayers                           = null!;
    private OrderBookSide                          valueDateFullyPopulatedOrderBookSide   = null!;
    private IList<IValueDatePriceVolumeLayer>      valueDateLayers                        = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        simpleLayers      = new List<IPriceVolumeLayer>(MaxNumberOfLayers);
        sourceLayers      = new List<ISourcePriceVolumeLayer>(MaxNumberOfLayers);
        sourceQtRefLayers = new List<ISourceQuoteRefPriceVolumeLayer>(MaxNumberOfLayers);
        valueDateLayers   = new List<IValueDatePriceVolumeLayer>(MaxNumberOfLayers);
        traderLayers      = new List<IOrdersPriceVolumeLayer>(MaxNumberOfLayers);
        allFieldsLayers   = new List<IFullSupportPriceVolumeLayer>(MaxNumberOfLayers);

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
            var srcQtRefPvl = new SourceQuoteRefPriceVolumeLayer
                (1.234567m, 40_000_000m, "TestSourceName", true, 12345678u);
            sourceQtRefLayers.Add(srcQtRefPvl);
            valueDateLayers.Add
                (new ValueDatePriceVolumeLayer(1.234567m, 40_000_000m, new DateTime(2017, 12, 09, 14, 0, 0)));
            var allFieldsPvL = new FullSupportPriceVolumeLayer
                (1.234567m, 40_000_000m, new DateTime(2017, 12, 09, 14, 0, 0)
               , "TestSourceName", true, 12345678u);
            allFieldsLayers.Add(allFieldsPvL);
            var traderPvL = new OrdersPriceVolumeLayer(price: 1.234567m, volume: 40_000_000m);
            traderLayers.Add(traderPvL);
            for (var j = 0; j < MaxNumberOfTraders; j++)
            {
                allFieldsPvL.Add(new CounterPartyOrderLayerInfo
                                     (i, LayerOrderFlags.CreatedFromAdapter, new DateTime(2017, 12, 09, 14, 0, 0),
                                      40_000_000m, traderName: "Trdr" + j));
                traderPvL.Add(new CounterPartyOrderLayerInfo
                                  (i, LayerOrderFlags.CreatedFromAdapter, new DateTime(2017, 12, 09, 14, 0, 0),
                                   40_000_000m, traderName: "Trdr" + j));
            }
        }

        simpleFullyPopulatedOrderBookSide      = new OrderBookSide(BookSide.AskBook, (IReadOnlyList<IPriceVolumeLayer>)simpleLayers);
        sourceFullyPopulatedOrderBookSide      = new OrderBookSide(BookSide.BidBook, sourceLayers.ToList());
        sourceQtRefFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.BidBook, sourceQtRefLayers.Cast<IPriceVolumeLayer>().ToList());
        traderFullyPopulatedOrderBookSide      = new OrderBookSide(BookSide.AskBook, traderLayers.Cast<IPriceVolumeLayer>().ToList());
        valueDateFullyPopulatedOrderBookSide   = new OrderBookSide(BookSide.AskBook, valueDateLayers.Cast<IPriceVolumeLayer>().ToList());

        allFieldsFullyPopulatedOrderBookSide = new OrderBookSide(BookSide.BidBook, allFieldsLayers.Cast<IPriceVolumeLayer>().ToList());

        allPopulatedOrderBooks = new List<OrderBookSide>
        {
            simpleFullyPopulatedOrderBookSide, sourceFullyPopulatedOrderBookSide, sourceQtRefFullyPopulatedOrderBookSide
          , valueDateFullyPopulatedOrderBookSide, traderFullyPopulatedOrderBookSide, allFieldsFullyPopulatedOrderBookSide
        };
        publicationPrecisionSettings = new SourceTickerInfo
            (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);
    }

    [TestMethod]
    public void FromSourceTickerInfo_New_InitializesOrderBookWithExpectedLayerTypes()
    {
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var orderBook = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable;
        orderBook = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(SourcePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        orderBook = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate;
        orderBook = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(ValueDatePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrdersCount | LayerFlags.OrderSize;
        orderBook = new OrderBookSide(BookSide.BidBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(OrdersPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price.AllFlags();

        orderBook = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(FullSupportPriceVolumeLayer));
    }

    [TestMethod]
    public void PQLayers_New_ConvertsToExpectedEquivalent()
    {
        IList<IPriceVolumeLayer> pqList = new List<IPriceVolumeLayer>
        {
            new PQPriceVolumeLayer()
        };
        var orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PriceVolumeLayer));

        pqList.Clear();
        pqList.Add(new PQSourcePriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(SourcePriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQSourceQuoteRefPriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(SourceQuoteRefPriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQValueDatePriceVolumeLayer());
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(ValueDatePriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQOrdersPriceVolumeLayer(emptyNameIdLookupGenerator.Clone(), LayerType.OrdersFullPriceVolume));
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(OrdersPriceVolumeLayer));
        pqList.Clear();
        pqList.Add(new PQFullSupportPriceVolumeLayer(emptyNameIdLookupGenerator.Clone()));
        orderBook = new OrderBookSide(BookSide.BidBook, pqList);
        Assert.IsInstanceOfType(orderBook[0], typeof(FullSupportPriceVolumeLayer));

        orderBook = new OrderBookSide(BookSide.BidBook, Enumerable.Empty<IPriceVolumeLayer>());
        Assert.AreEqual(0, orderBook.Count);
    }

    [TestMethod]
    public void PQOrderBook_InitializedFromOrderBook_ConvertsLayers()
    {
        var pqSrcTkrQuoteInfo =
            new PQSourceTickerInfo(publicationPrecisionSettings)
            {
                LayerFlags = LayerFlags.Price | LayerFlags.Volume
            };
        var pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        var orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(PriceVolumeLayer));

        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable;
        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(SourcePriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefPriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.ValueDate;
        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(ValueDatePriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags =
            LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrdersCount | LayerFlags.OrderSize;
        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(OrdersPriceVolumeLayer));
        pqSrcTkrQuoteInfo.LayerFlags = LayerFlags.Price.AllFlags();

        pqOrderBook = new PQOrderBookSide(BookSide.AskBook, pqSrcTkrQuoteInfo);
        orderBook   = new OrderBookSide(pqOrderBook);
        AssertBookHasLayersOfType(orderBook, typeof(FullSupportPriceVolumeLayer));
    }

    [TestMethod]
    public void NewOrderBook_InitializedWithLayers_ContainsNewInstanceLayersWithSameValues()
    {
        for (var i = 0; i < allPopulatedOrderBooks.Count; i++)
        {
            var populatedOrderBook = allPopulatedOrderBooks[i];
            var populatedLayers    = allPopulatedLayers[i];
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Count);
            for (var j = 0; j < MaxNumberOfTraders; j++)
            {
                Assert.AreNotSame(populatedLayers[j], populatedOrderBook[j]);
                Assert.AreEqual(populatedLayers[j], populatedOrderBook[j]);
            }
        }
    }

    [TestMethod]
    public void NewOrderBook_InitializedFromOrderBook_ClonesAllLayers()
    {
        for (var i = 0; i < allPopulatedOrderBooks.Count; i++)
        {
            var populatedOrderBook = allPopulatedOrderBooks[i];
            var clonedOrderBook    = new OrderBookSide(populatedOrderBook);
            Assert.AreEqual(MaxNumberOfLayers, clonedOrderBook.Count);
            for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreNotSame(populatedOrderBook[j], clonedOrderBook[j]);
        }

        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var emptyOrderBook = new OrderBookSide(BookSide.AskBook, publicationPrecisionSettings);
        Assert.AreEqual(0, emptyOrderBook.Count);
        var clonedEmptyOrderBook = new OrderBookSide(emptyOrderBook);
        Assert.AreEqual(0, clonedEmptyOrderBook.Count);
        for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreNotSame(emptyOrderBook[j], clonedEmptyOrderBook[j]);
    }

    [TestMethod]
    public void PopulatedOrderBook_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
            for (var i = 0; i < MaxNumberOfLayers; i++)
            {
                var layer       = ((IOrderBookSide)populatedOrderBook)[i];
                var clonedLayer = (IMutablePriceVolumeLayer)layer!.Clone();
                populatedOrderBook[i] = clonedLayer;
                Assert.AreNotSame(layer, ((IMutableOrderBookSide)populatedOrderBook)[i]);
                Assert.AreSame(clonedLayer, populatedOrderBook[i]);
                if (i == populatedOrderBook.Count - 1)
                {
                    ((IMutableOrderBookSide)populatedOrderBook)[i] = null;
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
        simpleFullyPopulatedOrderBookSide.Capacity = PQQuoteFieldsExtensions.SingleByteFieldIdMaxBookDepth + 1;
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
        Assert.AreSame(typeof(OrderBookLayerFactorySelector), OrderBookSide.LayerSelector.GetType());
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
            var newEmpty = new OrderBookSide(populatedOrderBook);
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
            var newEmpty = new OrderBookSide(populatedOrderBook);
            newEmpty.StateReset();
            Assert.AreNotEqual(populatedOrderBook, newEmpty);
            newEmpty.CopyFrom(subType);
            Assert.IsTrue(subType.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromLessLayers_ReplicatesMissingValues()
    {
        var clonePopulated = simpleFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(MaxNumberOfLayers, clonePopulated.Count);
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        Assert.AreEqual(MaxNumberOfLayers - 3, clonePopulated.Count);
        var notEmpty = new OrderBookSide(simpleFullyPopulatedOrderBookSide);
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(MaxNumberOfLayers - 3, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGap()
    {
        var clonePopulated = simpleFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(MaxNumberOfLayers, clonePopulated.Count);
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[5]                        = null;
        Assert.AreEqual(MaxNumberOfLayers - 2, clonePopulated.Count);
        var notEmpty = new OrderBookSide(simpleFullyPopulatedOrderBookSide);
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], new PriceVolumeLayer()); // null copies to empty
        Assert.AreEqual(MaxNumberOfLayers - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap()
    {
        var clonePopulated = simpleFullyPopulatedOrderBookSide.Clone();
        Assert.AreEqual(MaxNumberOfLayers, clonePopulated.Count);
        clonePopulated[clonePopulated.Count - 1] = null;
        clonePopulated[clonePopulated.Count - 1] = null;
        Assert.AreEqual(MaxNumberOfLayers - 2, clonePopulated.Count);
        var notEmpty = new OrderBookSide(simpleFullyPopulatedOrderBookSide) { [5] = null };
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
            var cloned2 = ((ICloneable<IMutableOrderBookSide>)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned2, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned2);
            var cloned3 = ((IMutableOrderBookSide)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned3, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned3);
            var cloned4 = ((ICloneable<IOrderBookSide>)populatedOrderBook).Clone();
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
        var rt = allFieldsFullyPopulatedOrderBookSide;
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
                   copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        if (copySourceType == typeof(SourceQuoteRefPriceVolumeLayer))
            return copyDestinationType == typeof(SourceQuoteRefPriceVolumeLayer) ||
                   copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        if (copySourceType == typeof(ValueDatePriceVolumeLayer))
            return copyDestinationType == typeof(ValueDatePriceVolumeLayer) ||
                   copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        if (copySourceType == typeof(OrdersPriceVolumeLayer))
            return copyDestinationType == typeof(OrdersPriceVolumeLayer) ||
                   copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        if (copySourceType == typeof(FullSupportPriceVolumeLayer))
            return copyDestinationType == typeof(FullSupportPriceVolumeLayer);
        return false;
    }

    private void AssertBookHasLayersOfType(OrderBookSide orderBookSide, Type expectedType)
    {
        for (var i = 0; i < MaxNumberOfLayers; i++) Assert.IsInstanceOfType(orderBookSide[i], expectedType);
    }
    
    internal static OrderBookSide GenerateBookSide<T>
    (BookSide bookSide, int numberOfLayers, decimal startingPrice, decimal deltaPricePerLayer,
        decimal startingVolume, decimal deltaVolumePerLayer, Func<decimal, decimal, T> genNewLayerObj)
        where T : IPriceVolumeLayer
    {
        var generatedLayers = new List<T>();
        var currentPrice    = startingPrice;
        var currentVolume   = startingVolume;
        for (var i = 0; i < numberOfLayers; i++)
        {
            generatedLayers.Add(genNewLayerObj(currentPrice, currentVolume));
            if (bookSide == BookSide.AskBook)
            {
                currentPrice += deltaPricePerLayer;
            }
            else
            {
                currentPrice -= deltaPricePerLayer;
            }
            currentVolume += deltaVolumePerLayer;
        }

        return new OrderBookSide(bookSide, generatedLayers.Cast<IPriceVolumeLayer>().ToList());
    }

    internal static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableOrderBookSide commonOrderBookSide,
        IMutableOrderBookSide changingOrderBookSide,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        IMutableLevel2Quote? originalQuote = null,
        IMutableLevel2Quote? changingQuote = null)
    {
        if (changingOrderBook == null && changingOrderBook == null) return;
        Assert.AreEqual(commonOrderBookSide.Count, changingOrderBookSide.Count);

        for (var i = 0; i < commonOrderBookSide.Count; i++)
        {
            PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison, commonOrderBookSide[i],
                 changingOrderBookSide[i], commonOrderBookSide,
                 changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            SourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as IMutableSourcePriceVolumeLayer,
                 changingOrderBookSide[i] as IMutableSourcePriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide,  originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            SourceQuoteRefPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as
                     IMutableSourceQuoteRefPriceVolumeLayer,
                 changingOrderBookSide[i] as
                     IMutableSourceQuoteRefPriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide,  originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            ValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as IMutableValueDatePriceVolumeLayer,
                 changingOrderBookSide[i] as IMutableValueDatePriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide,  originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            OrdersCountPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as IMutableOrdersCountPriceVolumeLayer,
                 changingOrderBookSide[i] as IMutableOrdersCountPriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide,  originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            OrdersPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (
                 exactComparison
               , commonOrderBookSide[i] as IMutableOrdersPriceVolumeLayer,
                 changingOrderBookSide[i] as IMutableOrdersPriceVolumeLayer
               , commonOrderBookSide,
                 changingOrderBookSide,  originalOrderBook, changingOrderBook, originalQuote, changingQuote
                );
            FullSupportPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
             exactComparison, commonOrderBookSide[i] as IMutableFullSupportPriceVolumeLayer,
             changingOrderBookSide[i] as IMutableFullSupportPriceVolumeLayer, commonOrderBookSide,
             changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);
        }
    }
}
