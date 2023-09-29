using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook
{
    [TestClass]
    public class OrderBookTests
    {
        private IMutableSourceTickerQuoteInfo publicationPrecisionSettings;
        private const int MaxNumberOfLayers = 19; // test being less than max.
        private const int MaxNumberOfTraders = 3; // not too many traders.
        private IList<IPriceVolumeLayer> simpleLayers;
        private IList<ISourcePriceVolumeLayer> sourceLayers;
        private IList<ISourceQuoteRefPriceVolumeLayer> sourceQtRefLayers;
        private IList<IValueDatePriceVolumeLayer> valueDateLayers;
        private IList<ITraderPriceVolumeLayer> traderLayers;
        private IList<ISourceQuoteRefTraderValueDatePriceVolumeLayer> allFieldsLayers;

        private OrderBook simpleFullyPopulatedOrderBook;
        private OrderBook sourceFullyPopulatedOrderBook;
        private OrderBook sourceQtRefFullyPopulatedOrderBook;
        private OrderBook valueDateFullyPopulatedOrderBook;
        private OrderBook traderFullyPopulatedOrderBook;
        private OrderBook allFieldsFullyPopulatedOrderBook;

        private List<OrderBook> allPopulatedOrderBooks;
        private List<IReadOnlyList<IPriceVolumeLayer>> allPopulatedLayers;

        [TestInitialize]
        public void SetUp()
        {
            simpleLayers = new List<IPriceVolumeLayer>(MaxNumberOfLayers);
            sourceLayers = new List<ISourcePriceVolumeLayer>(MaxNumberOfLayers);
            sourceQtRefLayers = new List<ISourceQuoteRefPriceVolumeLayer>(MaxNumberOfLayers);
            valueDateLayers = new List<IValueDatePriceVolumeLayer>(MaxNumberOfLayers);
            traderLayers = new List<ITraderPriceVolumeLayer>(MaxNumberOfLayers);
            allFieldsLayers = new List<ISourceQuoteRefTraderValueDatePriceVolumeLayer>(MaxNumberOfLayers);

            allPopulatedLayers = new List<IReadOnlyList<IPriceVolumeLayer>>
            {
                (IReadOnlyList<IPriceVolumeLayer>)simpleLayers, (IReadOnlyList<IPriceVolumeLayer>)sourceLayers,
                (IReadOnlyList<IPriceVolumeLayer>)sourceQtRefLayers, (IReadOnlyList<IPriceVolumeLayer>)valueDateLayers,
                (IReadOnlyList<IPriceVolumeLayer>)traderLayers, (IReadOnlyList<IPriceVolumeLayer>)allFieldsLayers
            };

            for (int i = 0; i < MaxNumberOfLayers; i++)
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
                    1.234567m, 40_000_000m, new DateTime(2017, 12, 09, 14, 0, 0), "TestSourceName", true, 12345678u);
                allFieldsLayers.Add(allFieldsPvL);
                var traderPvL = new TraderPriceVolumeLayer(1.234567m, 40_000_000m);
                traderLayers.Add(traderPvL);
                for (int j = 0; j < MaxNumberOfTraders; j++)
                {
                    allFieldsPvL.Add("Trdr" + j, 40_000_000m);
                    traderPvL.Add("Trdr" + j, 40_000_000m);
                }
            }
            simpleFullyPopulatedOrderBook = new OrderBook((IReadOnlyList<IPriceVolumeLayer>) simpleLayers);
            sourceFullyPopulatedOrderBook = new OrderBook(sourceLayers.ToList());
            sourceQtRefFullyPopulatedOrderBook = new OrderBook(sourceQtRefLayers.Cast<IPriceVolumeLayer>().ToList());
            traderFullyPopulatedOrderBook = new OrderBook(traderLayers.Cast<IPriceVolumeLayer>().ToList());
            valueDateFullyPopulatedOrderBook = new OrderBook(valueDateLayers.Cast<IPriceVolumeLayer>().ToList());

            allFieldsFullyPopulatedOrderBook = new OrderBook(allFieldsLayers.Cast<IPriceVolumeLayer>().ToList());

            allPopulatedOrderBooks = new List<OrderBook>
            {
                simpleFullyPopulatedOrderBook, sourceFullyPopulatedOrderBook, sourceQtRefFullyPopulatedOrderBook,
                valueDateFullyPopulatedOrderBook, traderFullyPopulatedOrderBook, allFieldsFullyPopulatedOrderBook
            };
            publicationPrecisionSettings = new SourceTickerQuoteInfo(uint.MaxValue, "TestSource", "TestTicker", 20, 
                0.00001m, 30000m, 50000000m, 1000m, 1, LayerFlags.Volume | LayerFlags.Price, LastTradedFlags.PaidOrGiven |
                LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        }

        [TestMethod]
        public void FromSourceTickerQuoteInfo_New_InitializesOrderBookWithExpectedLayerTypes()
        {
            publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
            var orderBook = new OrderBook(publicationPrecisionSettings);
            AssertBookHasLayersOfType(orderBook, typeof(PriceVolumeLayer));
            publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                      LayerFlags.SourceName | LayerFlags.Executable;
            orderBook = new OrderBook(publicationPrecisionSettings);
            AssertBookHasLayersOfType(orderBook, typeof(SourcePriceVolumeLayer));
            publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                              LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
            orderBook = new OrderBook(publicationPrecisionSettings);
            AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefPriceVolumeLayer));
            publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                      LayerFlags.ValueDate;
            orderBook = new OrderBook(publicationPrecisionSettings);
            AssertBookHasLayersOfType(orderBook, typeof(ValueDatePriceVolumeLayer));
            publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                             LayerFlags.TraderName | LayerFlags.TraderCount | LayerFlags.TraderSize;
            orderBook = new OrderBook(publicationPrecisionSettings);
            AssertBookHasLayersOfType(orderBook, typeof(TraderPriceVolumeLayer));
            publicationPrecisionSettings.LayerFlags = LayerFlags.Price.AllFlags();
            orderBook = new OrderBook(publicationPrecisionSettings);
            AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        }

        [TestMethod]
        public void PQLayers_New_ConvertsToExpectedEquivalent()
        {
            IList<IPriceVolumeLayer> pqList = new List<IPriceVolumeLayer>
            {
                new PQPriceVolumeLayer()
            };
            var orderBook = new OrderBook(pqList);
            Assert.IsInstanceOfType(orderBook[0], typeof(PriceVolumeLayer));

            pqList.Clear();
            pqList.Add(new PQSourcePriceVolumeLayer());
            orderBook = new OrderBook(pqList);
            Assert.IsInstanceOfType(orderBook[0], typeof(SourcePriceVolumeLayer));
            pqList.Clear();
            pqList.Add(new PQSourceQuoteRefPriceVolumeLayer());
            orderBook = new OrderBook(pqList);
            Assert.IsInstanceOfType(orderBook[0], typeof(SourceQuoteRefPriceVolumeLayer));
            pqList.Clear();
            pqList.Add(new PQValueDatePriceVolumeLayer());
            orderBook = new OrderBook(pqList);
            Assert.IsInstanceOfType(orderBook[0], typeof(ValueDatePriceVolumeLayer));
            pqList.Clear();
            pqList.Add(new PQTraderPriceVolumeLayer());
            orderBook = new OrderBook(pqList);
            Assert.IsInstanceOfType(orderBook[0], typeof(TraderPriceVolumeLayer));
            pqList.Clear();
            pqList.Add(new PQSourceQuoteRefTraderValueDatePriceVolumeLayer());
            orderBook = new OrderBook(pqList);
            Assert.IsInstanceOfType(orderBook[0], typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));

            orderBook = new OrderBook((IEnumerable<IPriceVolumeLayer>)null);
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
            var pqOrderBook = new PQOrderBook(pqSrcTkrQuoteInfo);
            var orderBook = new OrderBook(pqOrderBook);
            AssertBookHasLayersOfType(orderBook, typeof(PriceVolumeLayer));

            pqSrcTkrQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                      LayerFlags.SourceName | LayerFlags.Executable;
            pqOrderBook = new PQOrderBook(pqSrcTkrQuoteInfo);
            orderBook = new OrderBook(pqOrderBook);
            AssertBookHasLayersOfType(orderBook, typeof(SourcePriceVolumeLayer));
            pqSrcTkrQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                  LayerFlags.SourceName | LayerFlags.Executable | LayerFlags.SourceQuoteReference;
            pqOrderBook = new PQOrderBook(pqSrcTkrQuoteInfo);
            orderBook = new OrderBook(pqOrderBook);
            AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefPriceVolumeLayer));
            pqSrcTkrQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                                      LayerFlags.ValueDate;
            pqOrderBook = new PQOrderBook(pqSrcTkrQuoteInfo);
            orderBook = new OrderBook(pqOrderBook);
            AssertBookHasLayersOfType(orderBook, typeof(ValueDatePriceVolumeLayer));
            pqSrcTkrQuoteInfo.LayerFlags = LayerFlags.Price | LayerFlags.Volume |
                                 LayerFlags.TraderName | LayerFlags.TraderCount | LayerFlags.TraderSize;
            pqOrderBook = new PQOrderBook(pqSrcTkrQuoteInfo);
            orderBook = new OrderBook(pqOrderBook);
            AssertBookHasLayersOfType(orderBook, typeof(TraderPriceVolumeLayer));
            pqSrcTkrQuoteInfo.LayerFlags = LayerFlags.Price.AllFlags();
            pqOrderBook = new PQOrderBook(pqSrcTkrQuoteInfo);
            orderBook = new OrderBook(pqOrderBook);
            AssertBookHasLayersOfType(orderBook, typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer));
        }

        [TestMethod]
        public void NewOrderBook_InitializedWithLayers_ContainsSameInstanceLayersAsInitialized()
        {
            for (int i = 0; i < allPopulatedOrderBooks.Count; i++)
            {
                var populatedOrderBook = allPopulatedOrderBooks[i];
                var populatedLayers = allPopulatedLayers[i];
                Assert.AreEqual(MaxNumberOfLayers, populatedOrderBook.Count);
                for (int j = 0; j < MaxNumberOfTraders; j++)
                {
                    Assert.AreSame(populatedLayers[j], populatedOrderBook[j]);
                }
            }
        }

        [TestMethod]
        public void NewOrderBook_InitializedFromOrderBook_ClonesAllLayers()
        {
            for (int i = 0; i < allPopulatedOrderBooks.Count; i++)
            {
                var populatedOrderBook = allPopulatedOrderBooks[i];
                var clonedOrderBook = new OrderBook(populatedOrderBook);
                Assert.AreEqual(MaxNumberOfLayers, clonedOrderBook.Count);
                for (int j = 0; j < MaxNumberOfTraders; j++)
                {
                    Assert.AreNotSame(populatedOrderBook[j], clonedOrderBook[j]);
                }
            }
            publicationPrecisionSettings.LayerFlags = LayerFlags.Price | LayerFlags.Volume;
            var emptyOrderBook = new OrderBook(publicationPrecisionSettings);
            Assert.AreEqual(0, emptyOrderBook.Count);
            var clonedEmptyOrderBook = new OrderBook(emptyOrderBook);
            Assert.AreEqual(0, clonedEmptyOrderBook.Count);
            for (int j = 0; j < MaxNumberOfTraders; j++)
            {
                Assert.AreNotSame(emptyOrderBook[j], clonedEmptyOrderBook[j]);
            }
        }

        [TestMethod]
        public void PopulatedOrderBook_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull()
        {
            foreach (var populatedOrderBook in allPopulatedOrderBooks)
            {
                for (int i = 0; i < MaxNumberOfLayers; i++)
                {
                    var layer = ((IOrderBook)populatedOrderBook)[i];
                    var clonedLayer = (IMutablePriceVolumeLayer)layer.Clone();
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

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void PopulatedOrderBook_CapacityLargerThanMaxBookDepth_ThrowsException()
        {
            simpleFullyPopulatedOrderBook.Capacity = PQFieldKeys.SingleByteFieldIdMaxBookDepth + 1;
        }

        [TestMethod]
        public void PopulatedOrderBook_Count_UpdatesWhenPricesChanged()
        {
            foreach (var populatedOrderBook in allPopulatedOrderBooks)
            {
                for (int i = MaxNumberOfLayers - 1; i >= 0; i--)
                {
                    Assert.AreEqual(i, populatedOrderBook.Count - 1);
                    populatedOrderBook[i].Reset();
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
                foreach (var pvl in populatedOrderBook)
                {
                    Assert.IsFalse(pvl.IsEmpty);
                }
                populatedOrderBook.Reset();
                Assert.AreEqual(0, populatedOrderBook.Count);
                foreach (var pvl in populatedOrderBook)
                {
                    Assert.IsTrue(pvl.IsEmpty);
                }
            }
        }

        [TestMethod]
        public void FullyPopulatedOrderBook_CopyFromToEmptyQuote_OrderBooksEqualEachOther()
        {
            foreach (var populatedOrderBook in allPopulatedOrderBooks)
            {
                var newEmpty = new OrderBook(populatedOrderBook);
                newEmpty.Reset();
                Assert.AreNotEqual(populatedOrderBook, newEmpty);
                newEmpty.CopyFrom(populatedOrderBook);
                Assert.AreEqual(populatedOrderBook, newEmpty);
            }
        }

        [TestMethod]
        public void FullyPopulatedOrderBook_CopyFromSubTypes_SubTypeSaysIsEquivalent()
        {
            foreach (var populatedOrderBook in allPopulatedOrderBooks)
            {
                foreach (var subType in allPopulatedOrderBooks.Where(ob => !ReferenceEquals(ob, populatedOrderBook)))
                {
                    if (!WholeyContainedBy(subType[0].GetType(), populatedOrderBook[0].GetType()))
                    {
                        continue;
                    }
                    var newEmpty = new OrderBook(populatedOrderBook);
                    newEmpty.Reset();
                    Assert.AreNotEqual(populatedOrderBook, newEmpty);
                    newEmpty.CopyFrom(subType);
                    Assert.IsTrue(subType.AreEquivalent(newEmpty));
                }
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
            Assert.AreEqual(MaxNumberOfLayers -3, clonePopulated.Count);
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
            clonePopulated[5] = null;
            Assert.AreEqual(MaxNumberOfLayers - 2, clonePopulated.Count);
            var notEmpty = new OrderBook(simpleFullyPopulatedOrderBook);
            Assert.AreEqual(MaxNumberOfLayers, notEmpty.Count);
            notEmpty.CopyFrom(clonePopulated);
            Assert.AreEqual(notEmpty[5], clonePopulated[5]);
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
            var notEmpty = new OrderBook(simpleFullyPopulatedOrderBook) {[5] = null};
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
                var q = populatedQuote;
                var toString = q.ToString();

                Assert.IsTrue(toString.Contains(q.GetType().Name));
                Assert.IsTrue(toString.Contains($"{nameof(q.Capacity)}: {q.Capacity}"));
                Assert.IsTrue(toString.Contains($"{nameof(q.Count)}: {q.Count}"));
                Assert.IsTrue(toString.Contains($"BookLayers:[{String.Join(", ", q)}]"));
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

            rt.Reset();

            Assert.AreEqual(0, rt.Count);
            Assert.AreEqual(0, ((IEnumerable)rt).Cast<IPriceVolumeLayer>().Count());
            Assert.AreEqual(0, ((IEnumerable<IPriceVolumeLayer>)rt).Count());
        }

        private bool WholeyContainedBy(Type copySourceType, Type copyDestinationType)
        {
            if (copySourceType == typeof(PriceVolumeLayer))
            {
                return true;
            }
            if (copySourceType == typeof(SourcePriceVolumeLayer))
            {
                return copyDestinationType == typeof(SourcePriceVolumeLayer) ||
                       copyDestinationType == typeof(SourceQuoteRefPriceVolumeLayer) ||
                       copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            if (copySourceType == typeof(SourceQuoteRefPriceVolumeLayer))
            {
                return copyDestinationType == typeof(SourceQuoteRefPriceVolumeLayer) ||
                       copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            if (copySourceType == typeof(ValueDatePriceVolumeLayer))
            {
                return copyDestinationType == typeof(ValueDatePriceVolumeLayer) ||
                       copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            if (copySourceType == typeof(TraderPriceVolumeLayer))
            {
                return copyDestinationType == typeof(TraderPriceVolumeLayer) ||
                       copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            if (copySourceType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer))
            {
                return copyDestinationType == typeof(SourceQuoteRefTraderValueDatePriceVolumeLayer);
            }
            return false;
        }

        private void AssertBookHasLayersOfType(OrderBook orderBook, Type expectedType)
        {
            for (int i = 0; i < MaxNumberOfLayers; i++)
            {
                Assert.IsInstanceOfType(orderBook[i], expectedType);
            }
        }

        internal static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison, 
            IMutableOrderBook commonOrderBook, IMutableOrderBook changingOrderBook, 
            IMutableLevel2Quote originalQuote = null, IMutableLevel2Quote changingQuote = null)
        {
            Assert.AreEqual(commonOrderBook.Count, changingOrderBook.Count);

            for (int i = 0; i < commonOrderBook.Count; i++)
            {
                PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, commonOrderBook[i],
                    changingOrderBook[i], commonOrderBook,
                    changingOrderBook, originalQuote, changingQuote);
                SourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, commonOrderBook[i] as IMutableSourcePriceVolumeLayer,
                    changingOrderBook[i] as IMutableSourcePriceVolumeLayer, commonOrderBook,
                    changingOrderBook, originalQuote, changingQuote);
                SourceQuoteRefPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, commonOrderBook[i] as IMutableSourceQuoteRefPriceVolumeLayer, 
                    changingOrderBook[i] as IMutableSourceQuoteRefPriceVolumeLayer, commonOrderBook,
                    changingOrderBook, originalQuote, changingQuote);
                ValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, commonOrderBook[i] as ValueDatePriceVolumeLayer,
                    changingOrderBook[i] as ValueDatePriceVolumeLayer, commonOrderBook,
                    changingOrderBook, originalQuote, changingQuote);
                TraderPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, commonOrderBook[i] as IMutableTraderPriceVolumeLayer,
                    changingOrderBook[i] as IMutableTraderPriceVolumeLayer, commonOrderBook,
                    changingOrderBook, originalQuote, changingQuote);
                SourceQuoteRefTraderValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                    exactComparison, commonOrderBook[i] as SourceQuoteRefTraderValueDatePriceVolumeLayer,
                    changingOrderBook[i] as SourceQuoteRefTraderValueDatePriceVolumeLayer, commonOrderBook,
                    changingOrderBook, originalQuote, changingQuote);
            }
        }
    }
}