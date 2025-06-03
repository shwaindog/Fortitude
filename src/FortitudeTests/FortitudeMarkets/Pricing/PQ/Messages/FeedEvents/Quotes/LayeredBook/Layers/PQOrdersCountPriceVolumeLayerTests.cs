// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

[TestClass]
public class PQOrdersCountPriceVolumeLayerTests
{
    private const decimal Price          = 1.3456m;
    private const decimal Volume         = 100_000m;
    private const uint    OrdersCount    = 732;
    private const decimal InternalVolume = 50_000m;

    private IPQOrdersCountPriceVolumeLayer emptyPvl     = null!;
    private IPQOrdersCountPriceVolumeLayer populatedPvl = null!;

    private static DateTime testDateTime = new (2017, 10, 08, 18, 33, 24);


    [TestInitialize]
    public void SetUp()
    {
        emptyPvl     = new PQOrdersCountPriceVolumeLayer();
        populatedPvl = new PQOrdersCountPriceVolumeLayer(Price, Volume, OrdersCount, InternalVolume);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new PQOrdersCountPriceVolumeLayer(Price, Volume, OrdersCount, InternalVolume);
        Assert.AreEqual(Price, newPvl.Price);
        Assert.AreEqual(Volume, newPvl.Volume);
        Assert.AreEqual(OrdersCount, newPvl.OrdersCount);
        Assert.AreEqual(InternalVolume, newPvl.InternalVolume);

        var newEmptyPvl = new OrdersCountPriceVolumeLayer();
        Assert.AreEqual(0m, newEmptyPvl.Price);
        Assert.AreEqual(0m, newEmptyPvl.Volume);
        Assert.AreEqual(0u, newEmptyPvl.OrdersCount);
        Assert.AreEqual(0m, newEmptyPvl.InternalVolume);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new PQOrdersCountPriceVolumeLayer(populatedPvl);
        Assert.AreEqual(Price, newPopulatedPvl.Price);
        Assert.AreEqual(Volume, newPopulatedPvl.Volume);
        Assert.AreEqual(OrdersCount, newPopulatedPvl.OrdersCount);
        Assert.AreEqual(InternalVolume, newPopulatedPvl.InternalVolume);
        Assert.IsTrue(newPopulatedPvl.IsPriceUpdated);
        Assert.IsTrue(newPopulatedPvl.IsVolumeUpdated);
        Assert.IsTrue(newPopulatedPvl.IsOrdersCountUpdated);
        Assert.IsTrue(newPopulatedPvl.IsInternalVolumeUpdated);
        Assert.IsFalse(newPopulatedPvl.IsEmpty);
        Assert.IsTrue(newPopulatedPvl.HasUpdates);

        var nonPqvTraderPvl   = new OrdersCountPriceVolumeLayer(populatedPvl);
        var fromNonPqInstance = new PQOrdersCountPriceVolumeLayer(nonPqvTraderPvl);
        Assert.AreEqual(Price, fromNonPqInstance.Price);
        Assert.AreEqual(Volume, fromNonPqInstance.Volume);
        Assert.AreEqual(OrdersCount, fromNonPqInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromNonPqInstance.InternalVolume);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsOrdersCountUpdated);
        Assert.IsTrue(fromNonPqInstance.IsInternalVolumeUpdated);
        Assert.IsFalse(newPopulatedPvl.IsEmpty);
        Assert.IsTrue(newPopulatedPvl.HasUpdates);

        var newEmptyPvl = new PQOrdersCountPriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(0u, newEmptyPvl.OrdersCount);
        Assert.AreEqual(0m, newEmptyPvl.InternalVolume);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsOrdersCountUpdated);
        Assert.IsFalse(newEmptyPvl.IsInternalVolumeUpdated);
        Assert.IsTrue(newEmptyPvl.IsEmpty);
        Assert.IsFalse(newEmptyPvl.HasUpdates);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedPvl = new PQOrdersCountPriceVolumeLayer(Price, Volume, OrdersCount, InternalVolume)
        {
            IsVolumeUpdated = false, IsOrdersCountUpdated = false, IsInternalVolumeUpdated = false
        };
        var fromPQInstance = new PQOrdersCountPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsFalse(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);

        newPopulatedPvl.IsPriceUpdated  = false;
        newPopulatedPvl.IsVolumeUpdated = true;
        fromPQInstance                  = new PQOrdersCountPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsFalse(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);

        newPopulatedPvl.IsVolumeUpdated      = false;
        newPopulatedPvl.IsOrdersCountUpdated = true;
        fromPQInstance                       = new PQOrdersCountPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrdersCountUpdated);

        newPopulatedPvl.IsOrdersCountUpdated    = false;
        newPopulatedPvl.IsInternalVolumeUpdated = true;
        fromPQInstance                          = new PQOrdersCountPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsTrue(fromPQInstance.IsInternalVolumeUpdated);
    }

    [TestMethod]
    public void PopulatedPvl_LayerOrdersCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedPvl.HasUpdates = false;

        AssertOrdersCountFieldUpdatesReturnAsExpected(populatedPvl);
    }

    public static void AssertOrdersCountFieldUpdatesReturnAsExpected
    (
        IPQOrdersCountPriceVolumeLayer? ordersCountLayer,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (ordersCountLayer == null) return;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBookSide == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(ordersCountLayer.IsOrdersCountUpdated);
        Assert.IsFalse(ordersCountLayer.HasUpdates);
        ordersCountLayer.OrdersCount = 12;
        Assert.IsTrue(ordersCountLayer.HasUpdates);
        ordersCountLayer.UpdateComplete();
        ordersCountLayer.OrdersCount          = 0;
        ordersCountLayer.IsOrdersCountUpdated = false;
        ordersCountLayer.HasUpdates           = false;

        Assert.AreEqual(0, ordersCountLayer.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedOrdersCount= 128u;
        ordersCountLayer.OrdersCount = expectedOrdersCount;
        Assert.IsTrue(ordersCountLayer.HasUpdates);
        Assert.AreEqual(expectedOrdersCount, ordersCountLayer.OrdersCount);
        Assert.IsTrue(ordersCountLayer.IsOrdersCountUpdated);
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
        var layerUpdates = ordersCountLayer
                           .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        var expectedLayer
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrdersCount, expectedOrdersCount);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedLayer, layerUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        ordersCountLayer.IsOrdersCountUpdated = false;
        Assert.IsFalse(ordersCountLayer.HasUpdates);
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
        Assert.IsTrue(ordersCountLayer.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
            (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == PQFeedFields.QuoteLayerOrdersCount && update.DepthId == depthWithSide
                select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersCountPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            Assert.AreEqual(expectedOrdersCount, foundLayer.OrdersCount);
            Assert.IsTrue(foundLayer.IsOrdersCountUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerOrdersCount && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersCountPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            Assert.AreEqual(expectedOrdersCount, foundLayer.OrdersCount);
            Assert.IsTrue(foundLayer.IsOrdersCountUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerOrdersCount && update.DepthId == depthNoSide
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer = (IPQOrdersCountPriceVolumeLayer)newEmpty[bookDepth]!;
            Assert.AreEqual(expectedOrdersCount, foundLayer.OrdersCount);
            Assert.IsTrue(foundLayer.IsOrdersCountUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        layerUpdates = 
            (from update in ordersCountLayer.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == PQFeedFields.QuoteLayerOrdersCount
                select update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        Assert.AreEqual(expectedLayer, layerUpdates[0]);

        var newLayer = new PQOrdersCountPriceVolumeLayer();
        newLayer.UpdateField(layerUpdates[0]);
        Assert.AreEqual(expectedOrdersCount, newLayer.OrdersCount);
        Assert.IsTrue(newLayer.IsOrdersCountUpdated);
        Assert.IsTrue(newLayer.HasUpdates);

        ordersCountLayer.OrdersCount = 0u;
        ordersCountLayer.HasUpdates     = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedPvl_LayerInternalVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedPvl.HasUpdates = false;

        AssertInternalVolumeFieldUpdatesReturnAsExpected(populatedPvl);
    }

    public static void AssertInternalVolumeFieldUpdatesReturnAsExpected
    (
        IPQOrdersCountPriceVolumeLayer? ordersCountLayer,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (ordersCountLayer == null) return;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(ordersCountLayer.IsInternalVolumeUpdated);
        Assert.IsFalse(ordersCountLayer.HasUpdates);
        ordersCountLayer.InternalVolume = 777m;
        Assert.IsTrue(ordersCountLayer.HasUpdates);
        ordersCountLayer.UpdateComplete();
        ordersCountLayer.InternalVolume          = 0m;
        ordersCountLayer.IsInternalVolumeUpdated = false;
        ordersCountLayer.HasUpdates              = false;

        Assert.AreEqual(0, ordersCountLayer.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedInternalVolume = 89_325m;
        ordersCountLayer.InternalVolume = expectedInternalVolume;
        Assert.IsTrue(ordersCountLayer.HasUpdates);
        Assert.AreEqual(expectedInternalVolume, ordersCountLayer.InternalVolume);
        Assert.IsTrue(ordersCountLayer.IsInternalVolumeUpdated);
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
        var layerUpdates = ordersCountLayer
                           .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        var expectedLayer
            = new PQFieldUpdate(PQFeedFields.QuoteLayerInternalVolume, expectedInternalVolume, precisionSettings.VolumeScalingPrecision);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedLayer, layerUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);


        ordersCountLayer.IsInternalVolumeUpdated = false;
        Assert.IsFalse(ordersCountLayer.HasUpdates);
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
        Assert.IsTrue(ordersCountLayer.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
            (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == PQFeedFields.QuoteLayerInternalVolume && update.DepthId == depthWithSide
                select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersCountPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            Assert.AreEqual(expectedInternalVolume, foundLayer.InternalVolume);
            Assert.IsTrue(foundLayer.IsInternalVolumeUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerInternalVolume && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersCountPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            Assert.AreEqual(expectedInternalVolume, foundLayer.InternalVolume);
            Assert.IsTrue(foundLayer.IsInternalVolumeUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerInternalVolume && update.DepthId == depthNoSide
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer = (IPQOrdersCountPriceVolumeLayer)newEmpty[bookDepth]!;
            Assert.AreEqual(expectedInternalVolume, foundLayer.InternalVolume);
            Assert.IsTrue(foundLayer.IsInternalVolumeUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        layerUpdates = 
            (from update in ordersCountLayer.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == PQFeedFields.QuoteLayerInternalVolume
                select update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        Assert.AreEqual(expectedLayer, layerUpdates[0]);

        var newLayer = new PQOrdersCountPriceVolumeLayer();
        newLayer.UpdateField(layerUpdates[0]);
        Assert.AreEqual(expectedInternalVolume, newLayer.InternalVolume);
        Assert.IsTrue(newLayer.IsInternalVolumeUpdated);
        Assert.IsTrue(newLayer.HasUpdates);

        ordersCountLayer.InternalVolume = 0m;
        ordersCountLayer.HasUpdates     = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void EmptyPvl_SetOrdersCountOnly_UpdatesCount()
    {
        Assert.AreEqual(0u, emptyPvl.OrdersCount);
        emptyPvl.OrdersCount = byte.MaxValue;
        Assert.AreEqual(byte.MaxValue, emptyPvl.OrdersCount);
        Assert.IsTrue(emptyPvl.IsOrdersCountUpdated);
        Assert.IsFalse(emptyPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.HasUpdates);
    }

    [TestMethod]
    public void EmptyPvl_SetInternalVolumeOnly_UpdatesCount()
    {
        Assert.AreEqual(0m, emptyPvl.InternalVolume);
        emptyPvl.InternalVolume = InternalVolume;
        Assert.AreEqual(InternalVolume, emptyPvl.InternalVolume);
        Assert.IsTrue(emptyPvl.IsInternalVolumeUpdated);
        Assert.IsFalse(emptyPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.HasUpdates);
    }

    [TestMethod]
    public void EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPvl_IsEmptyTrue_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedPvl.Price);
        Assert.AreNotEqual(0m, populatedPvl.Volume);
        Assert.AreNotEqual(0u, populatedPvl.OrdersCount);
        Assert.AreNotEqual(0m, populatedPvl.InternalVolume);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsOrdersCountUpdated);
        Assert.IsTrue(populatedPvl.IsInternalVolumeUpdated);
        Assert.IsTrue(populatedPvl.HasUpdates);
        populatedPvl.IsEmpty = true;
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(0u, populatedPvl.OrdersCount);
        Assert.AreEqual(0m, populatedPvl.InternalVolume);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsOrdersCountUpdated);
        Assert.IsTrue(populatedPvl.IsInternalVolumeUpdated);
        Assert.IsTrue(populatedPvl.HasUpdates);
        populatedPvl.StateReset();
        Assert.IsFalse(populatedPvl.IsPriceUpdated);
        Assert.IsFalse(populatedPvl.IsVolumeUpdated);
        Assert.IsFalse(populatedPvl.IsOrdersCountUpdated);
        Assert.IsFalse(populatedPvl.IsInternalVolumeUpdated);
        Assert.IsFalse(populatedPvl.HasUpdates);
    }

    [TestMethod]
    public void PopulatedPvlWithOrderCountUpdated_GetDeltaUpdate_ReturnsOrderCountFieldAsUpdated()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);

        populatedPvl.OrdersCount = 1;
        pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithInternalVolumeUpdated_GetDeltaUpdate_ReturnsInternalVolumeFieldAsUpdated()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);

        var updatedInternalVolume = InternalVolume + 10_000;
        populatedPvl.InternalVolume = updatedInternalVolume;
        pqFieldUpdates =
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

        populatedPvl.OrdersCount = 1;
        var updatedInternalVolume = InternalVolume + 10_000;
        populatedPvl.InternalVolume = updatedInternalVolume;
        populatedPvl.HasUpdates     = false;
        pqFieldUpdates =
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

        populatedPvl.OrdersCount = 1;
        var updatedInternalVolume = InternalVolume + 10_000;
        populatedPvl.InternalVolume = updatedInternalVolume;
        populatedPvl.HasUpdates     = false;
        pqFieldUpdates =
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
        var newEmpty = new PQOrdersCountPriceVolumeLayer();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new OrdersCountPriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQOrdersCountPriceVolumeLayer();
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(0u, emptyPriceVolumeLayer.OrdersCount);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.InternalVolume);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsOrdersCountUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsInternalVolumeUpdated);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IMutableOrdersCountPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);

        var cloned2 = ((IOrdersCountPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(cloned2, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned2);

        var cloned3 = (PQOrdersCountPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned3, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned3);

        var cloned4 = populatedPvl.Clone();
        Assert.AreNotSame(cloned4, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned4);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQOrdersCountPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
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
        Assert.AreEqual(populatedPvl, ((IMutableOrdersCountPriceVolumeLayer)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<IOrdersCountPriceVolumeLayer>)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, populatedPvl.Clone());
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
        var toString = populatedPvl.ToString()!;

        Assert.IsTrue(toString.Contains(populatedPvl.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Price)}: {populatedPvl.Price:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Volume)}: {populatedPvl.Volume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.OrdersCount)}: {populatedPvl.OrdersCount}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.InternalVolume)}: {populatedPvl.InternalVolume:N2}"));
    }

    public static void AssertContainsAllPvlFields
    (IList<PQFieldUpdate> checkFieldUpdates, IPQOrdersCountPriceVolumeLayer pvl, int bookIndex = 0,
        PQFieldFlags priceScale = (PQFieldFlags)1, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        PQPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl, bookIndex, priceScale, volumeScale);
        var depthId = (PQDepthKey)bookIndex;

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrdersCount, depthId, pvl.OrdersCount),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrdersCount, depthId),
                        $"For {pvl.GetType().Name} at {bookIndex} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerInternalVolume, depthId, pvl.InternalVolume, volumeScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerInternalVolume, depthId, volumeScale),
                        $"For {pvl.GetType().Name} at {bookIndex} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQOrdersCountPriceVolumeLayer? original,
        IPQOrdersCountPriceVolumeLayer? changingPriceVolumeLayer,
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

        OrdersCountPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide, changingOrderBookSide,
             originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQOrdersCountPriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new OrdersCountPriceVolumeLayer(original), exactComparison));

        changingPriceVolumeLayer.IsOrdersCountUpdated = !changingPriceVolumeLayer.IsOrdersCountUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsOrdersCountUpdated = original.IsOrdersCountUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.IsInternalVolumeUpdated = !changingPriceVolumeLayer.IsInternalVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsInternalVolumeUpdated = original.IsInternalVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
