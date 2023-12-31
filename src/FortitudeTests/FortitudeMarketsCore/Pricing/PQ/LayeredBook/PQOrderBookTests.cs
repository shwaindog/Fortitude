﻿#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook;

[TestClass]
public class PQOrderBookTests
{
    private const int MaxNumberOfLayers = 19; // test being less than max.
    private const int MaxNumberOfTraders = 3; // not too many traders.
    private PQOrderBook allFieldsFullyPopulatedOrderBook = null!;
    private IList<IPQSourceQuoteRefTraderValueDatePriceVolumeLayer> allFieldsLayers = null!;
    private List<IReadOnlyList<IPQPriceVolumeLayer?>> allPopulatedLayers = null!;

    private List<PQOrderBook> allPopulatedOrderBooks = null!;
    private PQSourceTickerQuoteInfo publicationPrecisionSettings = null!;

    private PQOrderBook simpleFullyPopulatedOrderBook = null!;
    private IList<IPQPriceVolumeLayer> simpleLayers = null!;
    private PQOrderBook sourceFullyPopulatedOrderBook = null!;
    private IList<IPQSourcePriceVolumeLayer> sourceLayers = null!;
    private PQNameIdLookupGenerator sourceNameIdLookupGenerator = null!;
    private PQOrderBook sourceQtRefFullyPopulatedOrderBook = null!;
    private IList<IPQSourceQuoteRefPriceVolumeLayer> sourceQtRefLayers = null!;
    private PQNameIdLookupGenerator srcQtRefTrdrVlDtSourceNameLookup = null!;
    private PQNameIdLookupGenerator srcQtRefTrdrVlDtTraderNameLookup = null!;
    private PQOrderBook traderFullyPopulatedOrderBook = null!;
    private IList<IPQTraderPriceVolumeLayer> traderLayers = null!;
    private PQNameIdLookupGenerator traderNameIdLookupGenerator = null!;
    private PQOrderBook valueDateFullyPopulatedOrderBook = null!;
    private IList<IPQValueDatePriceVolumeLayer> valueDateLayers = null!;

    [TestInitialize]
    public void SetUp()
    {
        traderNameIdLookupGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
            PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
        sourceNameIdLookupGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
            PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        srcQtRefTrdrVlDtSourceNameLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
            PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        srcQtRefTrdrVlDtTraderNameLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
            PQFieldFlags.TraderNameIdLookupSubDictionaryKey);

        simpleLayers = new List<IPQPriceVolumeLayer>(MaxNumberOfLayers);
        sourceLayers = new List<IPQSourcePriceVolumeLayer>(MaxNumberOfLayers);
        sourceQtRefLayers = new List<IPQSourceQuoteRefPriceVolumeLayer>(MaxNumberOfLayers);
        valueDateLayers = new List<IPQValueDatePriceVolumeLayer>(MaxNumberOfLayers);
        traderLayers = new List<IPQTraderPriceVolumeLayer>(MaxNumberOfLayers);
        allFieldsLayers = new List<IPQSourceQuoteRefTraderValueDatePriceVolumeLayer>(MaxNumberOfLayers);

        allPopulatedLayers = new List<IReadOnlyList<IPQPriceVolumeLayer?>>
        {
            (IReadOnlyList<IPQPriceVolumeLayer>)simpleLayers, (IReadOnlyList<IPQPriceVolumeLayer>)sourceLayers
            , (IReadOnlyList<IPQPriceVolumeLayer>)sourceQtRefLayers, (IReadOnlyList<IPQPriceVolumeLayer>)valueDateLayers
            , (IReadOnlyList<IPQPriceVolumeLayer>)traderLayers, (IReadOnlyList<IPQPriceVolumeLayer>)allFieldsLayers
        };

        for (var i = 0; i < MaxNumberOfLayers; i++)
        {
            simpleLayers.Add(new PQPriceVolumeLayer(1.234567m, 40_000_000m));
            var sourcePvl =
                new PQSourcePriceVolumeLayer(1.234567m, 40_000_000m, sourceNameIdLookupGenerator,
                    "TestSourceName", true);
            sourceLayers.Add(sourcePvl);
            var srcQtRefPvl = new PQSourceQuoteRefPriceVolumeLayer(1.234567m, 40_000_000m,
                sourceNameIdLookupGenerator, "TestSourceName", true, 12345678u);
            sourceQtRefLayers.Add(srcQtRefPvl);
            valueDateLayers.Add(new PQValueDatePriceVolumeLayer(1.234567m, 40_000_000m,
                new DateTime(2017, 12, 09, 14, 0, 0)));
            var allFieldsPvL = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(
                1.234567m, 40_000_000m, srcQtRefTrdrVlDtSourceNameLookup, srcQtRefTrdrVlDtTraderNameLookup,
                new DateTime(2017, 12, 09, 14, 0, 0), "TestSourceName", true, 12345678u);
            allFieldsLayers.Add(allFieldsPvL);
            var traderPvL = new PQTraderPriceVolumeLayer(1.234567m, 40_000_000m, traderNameIdLookupGenerator);
            traderLayers.Add(traderPvL);
            for (var j = 0; j < MaxNumberOfTraders; j++)
            {
                allFieldsPvL.Add("Trdr" + j, 40_000_000m);
                traderPvL.Add("Trdr" + j, 40_000_000m);
            }
        }

        simpleFullyPopulatedOrderBook = new PQOrderBook(simpleLayers);
        sourceFullyPopulatedOrderBook = new PQOrderBook(sourceLayers);
        sourceQtRefFullyPopulatedOrderBook = new PQOrderBook(sourceQtRefLayers);
        traderFullyPopulatedOrderBook = new PQOrderBook(traderLayers);
        valueDateFullyPopulatedOrderBook = new PQOrderBook(valueDateLayers);

        allFieldsFullyPopulatedOrderBook = new PQOrderBook(allFieldsLayers);

        allPopulatedOrderBooks = new List<PQOrderBook>
        {
            simpleFullyPopulatedOrderBook, sourceFullyPopulatedOrderBook, sourceQtRefFullyPopulatedOrderBook
            , valueDateFullyPopulatedOrderBook, traderFullyPopulatedOrderBook, allFieldsFullyPopulatedOrderBook
        };
        publicationPrecisionSettings = new PQSourceTickerQuoteInfo(new SourceTickerClientAndPublicationConfig(
            uint.MaxValue, "TestSource", "TestTicker", MaxNumberOfLayers, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
                                                  LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime
            , null,
            250u));
    }

    [TestMethod]
    public void FromSourceTickerQuoteInfo_New_InitializesOrderBookWithExpectedLayerTypes()
    {
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var orderBook = new PQOrderBook(publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                  LayerFlags.SourceName | LayerFlags.Executable;
        orderBook = new PQOrderBook(publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQSourcePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                  LayerFlags.SourceName | LayerFlags.Executable |
                                                  LayerFlags.SourceQuoteReference;
        orderBook = new PQOrderBook(publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQSourceQuoteRefPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                  LayerFlags.ValueDate;
        orderBook = new PQOrderBook(publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQValueDatePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                  LayerFlags.TraderName | LayerFlags.TraderCount |
                                                  LayerFlags.TraderSize;
        orderBook = new PQOrderBook(publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQTraderPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price.AllFlags();
        orderBook = new PQOrderBook(publicationPrecisionSettings);
        AssertBookHasLayersOfType(orderBook, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
    }

    [TestMethod]
    public void NonPQLayers_New_ConvertsToPQEquivalent()
    {
        IList<IPriceVolumeLayer> nonPQList = new List<IPriceVolumeLayer>
        {
            new PriceVolumeLayer()
        };
        var orderBook = new PQOrderBook(nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQPriceVolumeLayer));

        nonPQList.Clear();
        nonPQList.Add(new SourcePriceVolumeLayer());
        orderBook = new PQOrderBook(nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQSourcePriceVolumeLayer));
        nonPQList.Clear();
        nonPQList.Add(new SourceQuoteRefPriceVolumeLayer());
        orderBook = new PQOrderBook(nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQSourceQuoteRefPriceVolumeLayer));
        nonPQList.Clear();
        nonPQList.Add(new ValueDatePriceVolumeLayer());
        orderBook = new PQOrderBook(nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQValueDatePriceVolumeLayer));
        nonPQList.Clear();
        nonPQList.Add(new TraderPriceVolumeLayer());
        orderBook = new PQOrderBook(nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQTraderPriceVolumeLayer));
        nonPQList.Clear();
        nonPQList.Add(new SourceQuoteRefTraderValueDatePriceVolumeLayer());
        orderBook = new PQOrderBook(nonPQList);
        Assert.IsInstanceOfType(orderBook[0], typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));

        orderBook = new PQOrderBook();
        Assert.AreEqual(0, orderBook.Count);
    }

    [TestMethod]
    public void PQOrderBook_InitializedFromOrderBook_ConvertsLayers()
    {
        var nonPqOrderBook = new OrderBook(publicationPrecisionSettings);
        var pqorderBook = new PQOrderBook(nonPqOrderBook);
        AssertBookHasLayersOfType(pqorderBook, typeof(PQPriceVolumeLayer));

        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                  LayerFlags.SourceName | LayerFlags.Executable;
        nonPqOrderBook = new OrderBook(publicationPrecisionSettings);
        pqorderBook = new PQOrderBook(nonPqOrderBook);
        AssertBookHasLayersOfType(pqorderBook, typeof(PQSourcePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                  LayerFlags.SourceName | LayerFlags.Executable |
                                                  LayerFlags.SourceQuoteReference;
        nonPqOrderBook = new OrderBook(publicationPrecisionSettings);
        pqorderBook = new PQOrderBook(nonPqOrderBook);
        AssertBookHasLayersOfType(pqorderBook, typeof(PQSourceQuoteRefPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                  LayerFlags.ValueDate;
        nonPqOrderBook = new OrderBook(publicationPrecisionSettings);
        pqorderBook = new PQOrderBook(nonPqOrderBook);
        AssertBookHasLayersOfType(pqorderBook, typeof(PQValueDatePriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                  LayerFlags.TraderName | LayerFlags.TraderCount |
                                                  LayerFlags.TraderSize;
        nonPqOrderBook = new OrderBook(publicationPrecisionSettings);
        pqorderBook = new PQOrderBook(nonPqOrderBook);
        AssertBookHasLayersOfType(pqorderBook, typeof(PQTraderPriceVolumeLayer));
        publicationPrecisionSettings.LayerFlags = LayerFlags.Price.AllFlags();
        nonPqOrderBook = new OrderBook(publicationPrecisionSettings);
        pqorderBook = new PQOrderBook(nonPqOrderBook);
        AssertBookHasLayersOfType(pqorderBook, typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer));
    }

    [TestMethod]
    public void NewOrderBook_InitializedWithLayers_ContainsSameInstanceLayersAsInitialized()
    {
        for (var i = 0; i < allPopulatedOrderBooks.Count; i++)
        {
            var populatedOrderBook = allPopulatedOrderBooks[i];
            var populatedLayers = allPopulatedLayers[i];
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.AllLayers.Count);
            for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreSame(populatedLayers[j], populatedOrderBook[j]);
        }
    }

    [TestMethod]
    public void NewOrderBook_InitializedFromOrderBook_ClonesAllLayers()
    {
        for (var i = 0; i < allPopulatedOrderBooks.Count; i++)
        {
            var populatedOrderBook = allPopulatedOrderBooks[i];
            var clonedOrderBook = new PQOrderBook(populatedOrderBook);
            Assert.AreEqual(MaxNumberOfLayers, clonedOrderBook.AllLayers.Count);
            for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreNotSame(populatedOrderBook[j], clonedOrderBook[j]);
        }

        publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
        var emptyOrderBook = new PQOrderBook(publicationPrecisionSettings);
        Assert.AreEqual(0, emptyOrderBook.Count);
        var clonedEmptyOrderBook = new PQOrderBook(emptyOrderBook);
        Assert.AreEqual(0, clonedEmptyOrderBook.Count);
        for (var j = 0; j < MaxNumberOfTraders; j++) Assert.AreNotSame(emptyOrderBook[j], clonedEmptyOrderBook[j]);
    }

    [TestMethod]
    public void PopulatedOrderBook_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
            for (var i = 0; i < MaxNumberOfLayers; i++)
            {
                var layer = ((IOrderBook)populatedOrderBook)[i];
                var clonedLayer = (IPQPriceVolumeLayer)layer!.Clone();
                populatedOrderBook[i] = clonedLayer;
                Assert.AreNotSame(layer, ((IMutableOrderBook)populatedOrderBook)[i]);
                Assert.AreSame(clonedLayer, populatedOrderBook[i]);
                if (i == populatedOrderBook.AllLayers.Count - 1)
                {
                    ((IMutableOrderBook)populatedOrderBook)[i] = null;
                    Assert.AreEqual(MaxNumberOfLayers - 1, populatedOrderBook.AllLayers.Count);
                }
            }
    }

    [TestMethod]
    public void PopulatedOrderBook_SetAllLayers_ReplacesLayersWithNewSet()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
            for (var i = 0; i < allPopulatedLayers.Count; i++)
            {
                var originalLayers = populatedOrderBook.AllLayers;
                var index = (i + 2) % allPopulatedLayers.Count;
                var newReplacementLayerList = allPopulatedLayers[index].ToList()!;
                populatedOrderBook.AllLayers = newReplacementLayerList;
                Assert.AreNotSame(originalLayers, populatedOrderBook.AllLayers);
                Assert.AreSame(newReplacementLayerList, populatedOrderBook.AllLayers);
            }
    }

    [TestMethod]
    public void PopulatedOrderBook_Capacity_ShowMaxPossibleNumberOfLayersNotNull()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            Assert.AreEqual(populatedOrderBook.AllLayers.Count, populatedOrderBook.Capacity);
            Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Capacity);
            populatedOrderBook[MaxNumberOfLayers - 1] = null;
            Assert.AreEqual(MaxNumberOfLayers - 1, populatedOrderBook.Capacity);
            Assert.AreEqual(populatedOrderBook.AllLayers.Count, populatedOrderBook.Capacity);
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
    public void PopulatedOrderBookClearHasUpdates_HasUpdates_ChangeItemAtATimeReportsUpdates()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            Assert.IsTrue(populatedOrderBook.HasUpdates);
            populatedOrderBook.HasUpdates = false;
            Assert.IsFalse(populatedOrderBook.HasUpdates);
            foreach (var pvl in populatedOrderBook)
            {
                pvl.Price = 3.456789m;
                Assert.IsTrue(populatedOrderBook.HasUpdates);
                pvl.IsPriceUpdated = false;
                Assert.IsFalse(populatedOrderBook.HasUpdates);
                pvl.Volume = 2_345_678m;
                Assert.IsTrue(populatedOrderBook.HasUpdates);
                pvl.IsVolumeUpdated = false;
                Assert.IsFalse(populatedOrderBook.HasUpdates);
                if (pvl is IPQSourcePriceVolumeLayer srcPvl)
                {
                    srcPvl.SourceName = "newSourceName";
                    Assert.IsTrue(populatedOrderBook.HasUpdates);
                    srcPvl.IsSourceNameUpdated = false;
                    srcPvl.SourceNameIdLookup.HasUpdates = false;
                    Assert.IsFalse(populatedOrderBook.HasUpdates);
                }

                if (pvl is IPQSourceQuoteRefPriceVolumeLayer srcQtRefPvl)
                {
                    srcQtRefPvl.SourceQuoteReference = 98_765_421u;
                    Assert.IsTrue(populatedOrderBook.HasUpdates);
                    srcQtRefPvl.IsSourceQuoteReferenceUpdated = false;
                    Assert.IsFalse(populatedOrderBook.HasUpdates);
                }

                if (pvl is IPQValueDatePriceVolumeLayer valueDatePvl)
                {
                    valueDatePvl.ValueDate = new DateTime(2017, 12, 10, 0, 0, 0);
                    Assert.IsTrue(populatedOrderBook.HasUpdates);
                    valueDatePvl.IsValueDateUpdated = false;
                    Assert.IsFalse(populatedOrderBook.HasUpdates);
                }

                if (pvl is IPQTraderPriceVolumeLayer traderPvl)
                    for (var i = 0; i < MaxNumberOfTraders; i++)
                    {
                        var traderLayerInfo = traderPvl[i]!;
                        traderLayerInfo.TraderName = "NewTraderName" + i;
                        Assert.IsTrue(populatedOrderBook.HasUpdates);
                        traderLayerInfo.IsTraderNameUpdated = false;
                        traderPvl.TraderNameIdLookup.HasUpdates = false;
                        Assert.IsFalse(populatedOrderBook.HasUpdates);

                        traderLayerInfo.TraderVolume = 3_000m;
                        Assert.IsTrue(populatedOrderBook.HasUpdates);
                        traderLayerInfo.IsTraderVolumeUpdated = false;
                        Assert.IsFalse(populatedOrderBook.HasUpdates);
                    }
            }
        }
    }

    [TestMethod]
    public void StaticDefault_LayerConverter_IsPQLayerConverter()
    {
        Assert.AreSame(typeof(PQOrderBookLayerFactorySelector), PQOrderBook.LayerSelector.GetType());
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
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllOrderBookFields()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var pqFieldUpdates = populatedOrderBook.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 12, 33, 1), UpdateStyle.Updates).ToList();
            AssertContainsAllOrderBookFields(pqFieldUpdates, populatedOrderBook);
        }
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllOrderBookFields()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            populatedOrderBook.HasUpdates = false;
            var pqFieldUpdates = populatedOrderBook.GetDeltaUpdateFields(
                    new DateTime(2017, 11, 04, 12, 33, 1), UpdateStyle.FullSnapshot, publicationPrecisionSettings)
                .ToList();
            AssertContainsAllOrderBookFields(pqFieldUpdates, populatedOrderBook);
        }
    }

    [TestMethod]
    public void PopulatedOrderBookWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            populatedOrderBook.HasUpdates = false;
            var pqFieldUpdates = populatedOrderBook.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            var pqStringUpdates = populatedOrderBook.GetStringUpdates(
                new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
            Assert.AreEqual(0, pqFieldUpdates.Count);
            Assert.AreEqual(0, pqStringUpdates.Count);
        }
    }

    [TestMethod]
    public void PopulatedOrderBook_GetDeltaUpdatesUpdateFieldNewOrderBook_CopiesAllFieldsToNewOrderBook()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var pqFieldUpdates = populatedOrderBook.GetDeltaUpdateFields(
                new DateTime(2017, 11, 04, 13, 33, 3), UpdateStyle.Updates | UpdateStyle.Replay).ToList();
            var pqStringUpdates = populatedOrderBook.GetStringUpdates(
                new DateTime(2017, 11, 04, 13, 33, 3), UpdateStyle.Updates | UpdateStyle.Replay).ToList();
            var newEmpty = CreateNewEmpty(populatedOrderBook);
            Assert.AreNotEqual(populatedOrderBook, newEmpty);
            foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
            foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
            Assert.AreEqual(populatedOrderBook, newEmpty);
        }
    }

    [TestMethod]
    public void FullyOrderBook_CopyFromToEmptyQuote_OrderBooksEqualEachOther()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var newEmpty = CreateNewEmpty(populatedOrderBook);
            newEmpty.CopyFrom(populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, newEmpty);
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
        var notEmpty = new PQOrderBook(simpleFullyPopulatedOrderBook);
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
        clonePopulated[5] = null;
        Assert.AreEqual(MaxNumberOfLayers - 2, clonePopulated.Count);
        var notEmpty = new PQOrderBook(simpleFullyPopulatedOrderBook);
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], new PQPriceVolumeLayer() { IsPriceUpdated = true, IsVolumeUpdated = true });
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
        var notEmpty = new PQOrderBook(simpleFullyPopulatedOrderBook) { [5] = null };
        Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
        notEmpty.CopyFrom(clonePopulated);
        Assert.AreEqual(notEmpty[5], clonePopulated[5]);
        Assert.AreEqual(MaxNumberOfLayers - 2, notEmpty.Count);
    }

    [TestMethod]
    public void FullyOrderBook_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var newEmpty = CreateNewEmpty(populatedOrderBook);
            populatedOrderBook.HasUpdates = false;
            newEmpty.CopyFrom(populatedOrderBook);
            foreach (var pvl in newEmpty)
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
        }
    }

    [TestMethod]
    public void NonPQOrderBook_CopyFromToEmptyOrderBook_OrderBooksEquivalentToEachOther()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var nonPQOrderBook = new OrderBook(populatedOrderBook);
            var newEmpty = CreateNewEmpty(populatedOrderBook);
            newEmpty.CopyFrom(nonPQOrderBook);
            Assert.AreEqual(populatedOrderBook, newEmpty);
        }
    }

    [TestMethod]
    public void ForEachOrderBookType_EmptyOrderBookCopyFromAnotherOrderBookType_UpgradesToEverythingOrderBookItems()
    {
        foreach (var originalTypeOrderBook in allPopulatedOrderBooks)
        foreach (var otherOrderBook in allPopulatedOrderBooks
                     .Where(ob => !ReferenceEquals(ob, originalTypeOrderBook)))
        {
            var emptyOriginalTypeOrderBook = CreateNewEmpty(originalTypeOrderBook);
            AssertAllLayersAreOfTypeAndEquivalentTo(emptyOriginalTypeOrderBook, originalTypeOrderBook,
                originalTypeOrderBook[0]!.GetType(), false);
            emptyOriginalTypeOrderBook.CopyFrom(otherOrderBook);
            AssertAllLayersAreOfTypeAndEquivalentTo(emptyOriginalTypeOrderBook, otherOrderBook,
                GetExpectedType(originalTypeOrderBook[0]!.GetType(),
                    otherOrderBook[0]!.GetType()));
        }
    }

    [TestMethod]
    public void ForEachOrderBookType_PopulatedCopyFromAnotherOrderBookType_UpgradesToEverythingOrderBookItems()
    {
        foreach (var originalTypeOrderBook in allPopulatedOrderBooks)
        foreach (var otherOrderBook in allPopulatedOrderBooks
                     .Where(ob => !ReferenceEquals(ob, originalTypeOrderBook)))
        {
            var clonedPopulatedOrderBook = (PQOrderBook)originalTypeOrderBook.Clone();
            AssertAllLayersAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, originalTypeOrderBook,
                originalTypeOrderBook[0]!.GetType(), false);
            clonedPopulatedOrderBook.CopyFrom(otherOrderBook);
            AssertAllLayersAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, otherOrderBook,
                GetExpectedType(originalTypeOrderBook[0]!.GetType(),
                    otherOrderBook[0]!.GetType()));
            AssertAllLayersAreOfTypeAndEquivalentTo(clonedPopulatedOrderBook, originalTypeOrderBook,
                GetExpectedType(originalTypeOrderBook[0]!.GetType(),
                    otherOrderBook[0]!.GetType()));
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var clonedOrderBook = ((ICloneable<IOrderBook>)populatedOrderBook).Clone();
            Assert.AreNotSame(clonedOrderBook, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, clonedOrderBook);

            var cloned2 = (IPQOrderBook)((ICloneable)populatedOrderBook).Clone();
            Assert.AreNotSame(cloned2, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, cloned2);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            var fullyPopulatedClone = (PQOrderBook)((ICloneable)populatedOrderBook).Clone();
            AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedOrderBook,
                fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedOrderBook,
                fullyPopulatedClone);
        }
    }

    [TestMethod]
    public void FullyPopulatedOrderBookSameObj_Equals_ReturnsTrue()
    {
        foreach (var populatedOrderBook in allPopulatedOrderBooks)
        {
            Assert.AreEqual(populatedOrderBook, populatedOrderBook);
            Assert.AreEqual(populatedOrderBook, ((ICloneable)populatedOrderBook).Clone());
            Assert.AreEqual(populatedOrderBook, ((ICloneable<IOrderBook>)populatedOrderBook).Clone());
            Assert.AreEqual(populatedOrderBook, ((ICloneable<IMutableOrderBook>)populatedOrderBook).Clone());
            Assert.AreEqual(populatedOrderBook, ((ICloneable<IPQOrderBook>)populatedOrderBook).Clone());
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
            var q = populatedQuote;
            var toString = q.ToString();

            Assert.IsTrue(toString.Contains(q.GetType().Name));
            Assert.IsTrue(toString.Contains($"{nameof(q.Capacity)}: {q.Capacity}"));
            Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
            Assert.IsTrue(toString.Contains($"AllLayers:[" +
                                            $"{string.Join(", ", (IEnumerable<IPQPriceVolumeLayer>)q)}]"));
        }
    }

    [TestMethod]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        var rt = allFieldsFullyPopulatedOrderBook;
        Assert.AreEqual(MaxNumberOfLayers, rt.Count);
        Assert.AreEqual(MaxNumberOfLayers, ((IEnumerable<IPQPriceVolumeLayer>)rt).Count());
        Assert.AreEqual(MaxNumberOfLayers, ((IEnumerable<IPriceVolumeLayer>)rt).Count());
        Assert.AreEqual(MaxNumberOfLayers, rt.OfType<IPriceVolumeLayer>().Count());

        rt.StateReset();

        Assert.AreEqual(0, rt.Count);
        Assert.AreEqual(0, ((IEnumerable<IPQPriceVolumeLayer>)rt).Count());
        Assert.AreEqual(0, ((IEnumerable<IPriceVolumeLayer>)rt).Count());
        Assert.AreEqual(0, rt.OfType<IPriceVolumeLayer>().Count());
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        PQOrderBook original, PQOrderBook changingOrderBook, PQLevel2Quote? originalQuote = null,
        PQLevel2Quote? changingQuote = null)
    {
        if (original.GetType() == typeof(PQOrderBook))
            Assert.AreEqual(!exactComparison,
                changingOrderBook.AreEquivalent(new OrderBook(original), exactComparison));

        Assert.AreEqual(original.AllLayers.Count, changingOrderBook.AllLayers.Count);
        var originalLayers = original.AllLayers;
        var changingLayers = changingOrderBook.AllLayers;

        for (var i = 0; i < originalLayers.Count; i++)
        {
            PQPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                exactComparison, (PQPriceVolumeLayer)originalLayers[i]!,
                (PQPriceVolumeLayer)changingLayers[i]!, original,
                changingOrderBook, originalQuote!, changingQuote!);
            if (originalLayers[i] is PQSourcePriceVolumeLayer)
                PQSourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, (PQSourcePriceVolumeLayer)originalLayers[i]!,
                    (PQSourcePriceVolumeLayer)changingLayers[i]!, original,
                    changingOrderBook, originalQuote!, changingQuote!);
            if (originalLayers[i] is PQSourceQuoteRefPriceVolumeLayer)
                PQSourceQuoteRefPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, (PQSourceQuoteRefPriceVolumeLayer)originalLayers[i]!,
                    (PQSourceQuoteRefPriceVolumeLayer)changingLayers[i]!, original,
                    changingOrderBook, originalQuote!, changingQuote!);
            if (originalLayers[i] is PQValueDatePriceVolumeLayer)
                PQValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, (PQValueDatePriceVolumeLayer)originalLayers[i]!,
                    (PQValueDatePriceVolumeLayer)changingLayers[i]!, original,
                    changingOrderBook, originalQuote!, changingQuote);
            if (originalLayers[i] is PQTraderPriceVolumeLayer)
                PQTraderPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, (PQTraderPriceVolumeLayer)originalLayers[i]!,
                    (PQTraderPriceVolumeLayer)changingLayers[i]!, original,
                    changingOrderBook, originalQuote, changingQuote);
            if (originalLayers[i] is PQSourceQuoteRefTraderValueDatePriceVolumeLayer)
                PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests
                    .AssertAreEquivalentMeetsExpectedExactComparisonType(
                        exactComparison, (PQSourceQuoteRefTraderValueDatePriceVolumeLayer)originalLayers[i]!,
                        (PQSourceQuoteRefTraderValueDatePriceVolumeLayer)changingLayers[i]!, original,
                        changingOrderBook, originalQuote, changingQuote);
        }
    }

    public static void AssertContainsAllOrderBookFields(IList<PQFieldUpdate> checkFieldUpdates,
        PQOrderBook orderBook)
    {
        for (var i = 0; i < MaxNumberOfLayers; i++)
        {
            var pvl = orderBook[i];

            Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerPriceOffset + i), pvl!.Price, 1),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    (byte)(PQFieldKeys.LayerPriceOffset + i), 1), $"For layer {pvl.GetType().Name} level {i}");
            Assert.AreEqual(new PQFieldUpdate((ushort)(PQFieldKeys.LayerVolumeOffset + i), pvl.Volume, 6),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    (ushort)(PQFieldKeys.LayerVolumeOffset + i), 6),
                $"For bidlayer {pvl.GetType().Name} level {i}");

            if (pvl is IPQSourcePriceVolumeLayer srcPvl)
            {
                var srcId = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    (byte)(PQFieldKeys.LayerSourceIdOffset + i));

                var nameIdLookup = srcPvl.SourceNameIdLookup;
                Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerSourceIdOffset + i),
                    nameIdLookup[srcPvl.SourceName!]), srcId);
            }

            if (pvl is IPQSourceQuoteRefPriceVolumeLayer srcQtRefPvl)
            {
                var srcQtRef = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    (byte)(PQFieldKeys.LayerSourceQuoteRefOffset + i));

                Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerSourceQuoteRefOffset + i),
                    srcQtRefPvl.SourceQuoteReference), srcQtRef);
            }

            if (pvl is IPQValueDatePriceVolumeLayer valueDatePvl)
            {
                var valueDate = PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    (ushort)(PQFieldKeys.LayerDateOffset + i));

                var dateAsHoursFromEpoch = valueDatePvl.ValueDate.GetHoursFromUnixEpoch();
                Assert.AreEqual(new PQFieldUpdate((ushort)(PQFieldKeys.LayerDateOffset + i),
                    dateAsHoursFromEpoch, PQFieldFlags.IsExtendedFieldId), valueDate);
            }

            if (pvl is IPQTraderPriceVolumeLayer bidTrdPvl)
            {
                var nameIdLookup = bidTrdPvl.TraderNameIdLookup;
                AssertTraderLayerInfoIsExpected(checkFieldUpdates, bidTrdPvl, i, nameIdLookup);
            }
        }
    }

    private Type GetExpectedType(Type originalType, Type copyType)
    {
        if (copyType == typeof(PQPriceVolumeLayer)) return originalType;
        if (originalType == typeof(PQSourceQuoteRefPriceVolumeLayer) && copyType == typeof(PQSourcePriceVolumeLayer))
            return typeof(PQSourceQuoteRefPriceVolumeLayer);
        return typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer);
    }

    private void AssertAllLayersAreOfTypeAndEquivalentTo(PQOrderBook upgradedOrderBook,
        PQOrderBook equivalentTo, Type expectedType, bool compareForEquivalence = true,
        bool exactlyEquals = false)
    {
        for (var i = 0; i < upgradedOrderBook.Capacity; i++)
        {
            var upgradedLayer = upgradedOrderBook[i];
            var copyFromLayer = equivalentTo[i];

            Assert.IsInstanceOfType(upgradedLayer, expectedType);
            if (compareForEquivalence) Assert.IsTrue(copyFromLayer!.AreEquivalent(upgradedLayer, exactlyEquals));
        }
    }

    private static PQOrderBook CreateNewEmpty(PQOrderBook populatedOrderBook)
    {
        var cloneGensis = populatedOrderBook[0]!.Clone();
        cloneGensis.StateReset();
        if (cloneGensis is IPQSourcePriceVolumeLayer sourcePvl)
            sourcePvl.SourceNameIdLookup = new PQNameIdLookupGenerator(
                PQFieldKeys.LayerNameDictionaryUpsertCommand,
                PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        if (cloneGensis is IPQTraderPriceVolumeLayer traderPvl)
            traderPvl.TraderNameIdLookup = new PQNameIdLookupGenerator(
                PQFieldKeys.LayerNameDictionaryUpsertCommand,
                PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
        var clonedEmptyLayers = new List<IPQPriceVolumeLayer>(MaxNumberOfLayers);
        for (var i = 0; i < MaxNumberOfLayers; i++) clonedEmptyLayers.Add(cloneGensis.Clone());
        var newEmpty = new PQOrderBook(clonedEmptyLayers);
        return newEmpty;
    }

    private void AssertBookHasLayersOfType(PQOrderBook orderBook, Type expectedType)
    {
        for (var i = 0; i < MaxNumberOfLayers; i++) Assert.IsInstanceOfType(orderBook[i], expectedType);
    }

    private static void AssertTraderLayerInfoIsExpected(IList<PQFieldUpdate> checkFieldUpdates,
        IPQTraderPriceVolumeLayer traderPvl, int bookIndex, IPQNameIdLookupGenerator nameIdLookup)
    {
        for (var j = 0; j < traderPvl.Count; j++)
        {
            var trdLayerInfo = traderPvl[j];
            var fieldPosIndex = (uint)j << 24;

            var traderId = PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                (byte)(PQFieldKeys.LayerTraderIdOffset + bookIndex), fieldPosIndex, 0xFF80_0000);

            var traderVolume = PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                (byte)(PQFieldKeys.LayerTraderVolumeOffset + bookIndex), fieldPosIndex, 0xFF80_0000, 10);

            Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerTraderIdOffset + bookIndex),
                fieldPosIndex | (uint)nameIdLookup[trdLayerInfo!.TraderName!]), traderId);

            var value = PQScaling.AutoScale(trdLayerInfo.TraderVolume, 6, out var flag);
            Assert.AreEqual(new PQFieldUpdate((byte)(PQFieldKeys.LayerTraderVolumeOffset + bookIndex),
                value | fieldPosIndex, flag), traderVolume);
        }
    }
}
