// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

[TestClass]
public class PQValueDatePriceVolumeLayerTests
{
    private const QuoteInstantBehaviorFlags QuoteBehavior = QuoteInstantBehaviorFlags.None;

    private PQValueDatePriceVolumeLayer emptyPvl     = null!;
    private PQValueDatePriceVolumeLayer populatedPvl = null!;

    private static DateTime testDateTime = new(2017, 10, 08, 18, 33, 24);

    [TestInitialize]
    public void SetUp()
    {
        emptyPvl     = new PQValueDatePriceVolumeLayer(0m, 0m, DateTime.MinValue);
        testDateTime = new DateTime(2017, 12, 17, 18, 54, 52);
        populatedPvl = new PQValueDatePriceVolumeLayer
            (4.2949_672m, 42_949_672m, new DateTime(2017, 12, 17, 18, 00, 00));
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var expectedDate = new DateTime(2017, 12, 17, 19, 00, 11);
        var newPvl       = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        Assert.AreEqual(expectedDate, newPvl.ValueDate);
        Assert.IsTrue(newPvl.IsPriceUpdated);
        Assert.IsTrue(newPvl.IsVolumeUpdated);
        Assert.IsTrue(newPvl.IsValueDateUpdated);

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, emptyPvl.ValueDate);
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
        Assert.IsFalse(emptyPvl.IsValueDateUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var expectedDate    = new DateTime(2017, 12, 17, 19, 00, 11);
        var newPopulatedPvl = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate);
        var fromPQInstance  = new PQValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(expectedDate, fromPQInstance.ValueDate);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);

        var nonPQValueDatePvl = new ValueDatePriceVolumeLayer(1.23456m, 5_123_456m, expectedDate);
        var fromNonPqInstance = new PQValueDatePriceVolumeLayer(nonPQValueDatePvl);
        Assert.AreEqual(1.23456m, fromNonPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromNonPqInstance.Volume);
        Assert.AreEqual(expectedDate, fromNonPqInstance.ValueDate);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);

        var fromNonValueDate = new PQValueDatePriceVolumeLayer(new PriceVolumeLayer(20, 40_000_000));
        Assert.AreEqual(20, fromNonValueDate.Price);
        Assert.AreEqual(40_000_000, fromNonValueDate.Volume);
        Assert.AreEqual(DateTime.MinValue, fromNonValueDate.ValueDate);
        Assert.IsTrue(fromNonValueDate.IsPriceUpdated);
        Assert.IsTrue(fromNonValueDate.IsVolumeUpdated);
        Assert.IsFalse(fromNonValueDate.IsValueDateUpdated);

        var newEmptyPvl = new PQValueDatePriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, newEmptyPvl.ValueDate);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsValueDateUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var expectedDate = new DateTime(2017, 12, 17, 19, 00, 11);
        var newPopulatedPvl = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate)
        {
            IsPriceUpdated = false
        };
        var fromPQInstance = new PQValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(expectedDate, fromPQInstance.ValueDate);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);

        newPopulatedPvl = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate)
        {
            IsVolumeUpdated = false
        };
        fromPQInstance = new PQValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(expectedDate, fromPQInstance.ValueDate);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);

        newPopulatedPvl = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate)
        {
            IsValueDateUpdated = false
        };
        fromPQInstance = new PQValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(expectedDate, fromPQInstance.ValueDate);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsValueDateUpdated);
    }


    [TestMethod]
    public void EmptyPvl_LayerValueDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        emptyPvl.HasUpdates = false;

        AssertValueDateFieldUpdatesReturnAsExpected(emptyPvl);
    }

    public static void AssertValueDateFieldUpdatesReturnAsExpected
    (
        IPQValueDatePriceVolumeLayer? vlDateLayer,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (vlDateLayer == null) return;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBookSide == null || orderBookSide.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(vlDateLayer.IsValueDateUpdated);
        Assert.IsFalse(vlDateLayer.HasUpdates);
        vlDateLayer.ValueDate = DateTime.Now;
        Assert.IsTrue(vlDateLayer.HasUpdates);
        vlDateLayer.UpdateComplete();
        vlDateLayer.ValueDate          = DateTime.MinValue;
        vlDateLayer.IsValueDateUpdated = false;
        vlDateLayer.HasUpdates         = false;

        Assert.AreEqual(0, vlDateLayer.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedValueDate = new DateTime(2017, 12, 03, 19, 2, 0);
        vlDateLayer.ValueDate = expectedValueDate;
        Assert.IsTrue(vlDateLayer.HasUpdates);
        Assert.AreEqual(expectedValueDate, vlDateLayer.ValueDate);
        Assert.IsTrue(vlDateLayer.IsValueDateUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        var layerUpdates = vlDateLayer
                           .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        var twoMinIntervalsSinceUnixEpoch = expectedValueDate.Get2MinIntervalsFromUnixEpoch();
        var expectedLayer
            = new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, twoMinIntervalsSinceUnixEpoch);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedLayer, layerUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        vlDateLayer.IsValueDateUpdated = false;
        Assert.IsFalse(vlDateLayer.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(vlDateLayer.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerValueDate && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQValueDatePriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth];
            Assert.AreEqual(expectedValueDate, foundLayer.ValueDate);
            Assert.IsTrue(foundLayer.IsValueDateUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerValueDate && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQValueDatePriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth];
            Assert.AreEqual(expectedValueDate, foundLayer.ValueDate);
            Assert.IsTrue(foundLayer.IsValueDateUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerValueDate && update.DepthId == depthNoSide
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer = (IPQValueDatePriceVolumeLayer)newEmpty[bookDepth];
            Assert.AreEqual(expectedValueDate, foundLayer.ValueDate);
            Assert.IsTrue(foundLayer.IsValueDateUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        layerUpdates =
            (from update in vlDateLayer.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == PQFeedFields.QuoteLayerValueDate
                select update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        Assert.AreEqual(expectedLayer, layerUpdates[0]);

        var newLayer = new PQValueDatePriceVolumeLayer();
        newLayer.UpdateField(layerUpdates[0]);
        Assert.AreEqual(expectedValueDate, newLayer.ValueDate);
        Assert.IsTrue(newLayer.IsValueDateUpdated);
        Assert.IsTrue(newLayer.HasUpdates);

        vlDateLayer.ValueDate  = DateTime.MinValue;
        vlDateLayer.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }


    [TestMethod]
    public void EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPvl_SetIsEmptyTrue_ReturnsReturnsLayerToEmptyExceptUpdatedFlags()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedPvl.Price);
        Assert.AreNotEqual(0m, populatedPvl.Volume);
        Assert.AreNotEqual(DateTime.MinValue, populatedPvl.ValueDate);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
        populatedPvl.IsEmpty = true;
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, populatedPvl.ValueDate);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
    }

    [TestMethod]
    public void PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQValueDatePriceVolumeLayer(0m, 0m, DateTime.MinValue);
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new ValueDatePriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(nonPQPriceVolume, QuoteBehavior);
        Assert.AreEqual(populatedPvl, emptyPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQValueDatePriceVolumeLayer(0m, 0m, DateTime.MinValue);
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl, QuoteBehavior);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(DateTime.MinValue, emptyPriceVolumeLayer.ValueDate);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsValueDateUpdated);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ICloneable<IValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);

        var cloned2 = (PQValueDatePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned2, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned2);
    }


    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQValueDatePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedPvl, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedPvl, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedPvlSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedPvl, populatedPvl);
        Assert.AreEqual(populatedPvl, ((ICloneable)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((IMutableValueDatePriceVolumeLayer)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<IValueDatePriceVolumeLayer>)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<IMutableValueDatePriceVolumeLayer>)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((IPQValueDatePriceVolumeLayer)populatedPvl).Clone());
    }

    [TestMethod]
    public void FullyPopulatedPvl_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedPvl.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedPvl.ToString();

        Assert.IsTrue(toString.Contains(populatedPvl.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Price)}: {populatedPvl.Price:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Volume)}: {populatedPvl.Volume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.ValueDate)}: {populatedPvl.ValueDate}"));
    }

    public static void AssertContainsAllPvlFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        PQValueDatePriceVolumeLayer pvl)
    {
        PQPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl);

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, pvl.ValueDate.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQFeedFields.QuoteLayerValueDate), $"For {pvl.GetType().Name} ");
    }


    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQValueDatePriceVolumeLayer? original, PQValueDatePriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBookSide? originalOrderBookSide = null,
        IOrderBookSide? changingOrderBookSide = null,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        IPublishableLevel2Quote? originalQuote = null,
        IPublishableLevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        PQPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide
           , changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQValueDatePriceVolumeLayer) &&
            changingPriceVolumeLayer.GetType() == typeof(PQValueDatePriceVolumeLayer))
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        else
            Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));

        if (original.GetType() == typeof(PQValueDatePriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new ValueDatePriceVolumeLayer(original), exactComparison));

        ValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide
           , changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.IsValueDateUpdated = !changingPriceVolumeLayer.IsValueDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsValueDateUpdated = original.IsValueDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
