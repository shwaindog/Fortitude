// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

[TestClass]
public class PQAnonymousOrderLayerInfoTests
{
    private const int     OrderId              = 80085;
    private const decimal OrderVolume          = 100_000.50m;
    private const decimal OrderRemainingVolume = 50_000.25m;


    private const int ExpectedTrackingId       = 2;

    private const OrderFlags ExpectedTypeFlags = OrderFlags.FromAdapter;
    private const OrderType  ExpectedOrderType = OrderType.PassiveLimit;

    private const LayerOrderFlags ExpectedLayerFlags = LayerOrderFlags.ExplicitlyDefinedFromSource | LayerOrderFlags.IsInternallyCreatedOrder;

    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.SourceActiveOnMarket;

    private static readonly DateTime CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private IPQAnonymousOrderLayerInfo emptyAoli = null!;

    private static IPQNameIdLookupGenerator emptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

    private IPQNameIdLookupGenerator   nameIdLookup  = null!;
    private IPQAnonymousOrderLayerInfo populatedAoli = null!;

    private static DateTime testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        nameIdLookup      = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        emptyAoli         = new PQAnonymousOrderLayerInfo();
        populatedAoli = new PQAnonymousOrderLayerInfo
            (OrderId, CreatedTime, OrderVolume, ExpectedLayerFlags, ExpectedOrderType, ExpectedTypeFlags
           , ExpectedLifecycleState, UpdatedTime, OrderRemainingVolume, ExpectedTrackingId);
    }

    [TestMethod]
    public void NewPqAoli_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newAoli = new PQAnonymousOrderLayerInfo
            (OrderId, CreatedTime, OrderVolume, ExpectedLayerFlags, ExpectedOrderType, ExpectedTypeFlags, ExpectedLifecycleState,
            UpdatedTime, OrderRemainingVolume, ExpectedTrackingId);
        Assert.AreEqual(OrderId, newAoli.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, newAoli.OrderLayerFlags);
        Assert.AreEqual(ExpectedOrderType, newAoli.OrderType);
        Assert.AreEqual(ExpectedTypeFlags, newAoli.TypeFlags);
        Assert.AreEqual(ExpectedLifecycleState, newAoli.OrderLifeCycleState);
        Assert.AreEqual(CreatedTime, newAoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, newAoli.UpdateTime);
        Assert.AreEqual(OrderVolume, newAoli.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, newAoli.OrderRemainingVolume);

        Assert.AreEqual(0, emptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, emptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, emptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, emptyAoli.UpdateTime);
        Assert.AreEqual(0m, ((IPublishedOrder)emptyAoli).OrderDisplayVolume);
        Assert.AreEqual(0m, emptyAoli.OrderRemainingVolume);
    }

    [TestMethod]
    public void NewPqAoli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var fromInstance = new PQAnonymousOrderLayerInfo(populatedAoli);
        Assert.AreEqual(populatedAoli.OrderId, fromInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderLayerFlags, fromInstance.OrderLayerFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdateTime, fromInstance.UpdateTime);
        Assert.AreEqual(((IPublishedOrder)populatedAoli).OrderDisplayVolume, fromInstance.OrderDisplayVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromInstance.OrderRemainingVolume);
        Assert.IsTrue(fromInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromInstance.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(fromInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromInstance.IsUpdateTimeDateUpdated);
        Assert.IsTrue(fromInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(fromInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromInstance.IsEmpty);
        Assert.IsTrue(fromInstance.HasUpdates);

        var fromEmptyAoli = new PQAnonymousOrderLayerInfo(emptyAoli);
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromEmptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);
        Assert.IsFalse(fromEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromEmptyAoli.IsEmpty);
        Assert.IsFalse(fromEmptyAoli.HasUpdates);

        var nonPqInstance     = new AnonymousOrderLayerInfo(populatedAoli);
        var fromNonPQInstance = new PQAnonymousOrderLayerInfo(nonPqInstance);
        Assert.AreEqual(populatedAoli.OrderId, fromNonPQInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderLayerFlags, fromNonPQInstance.OrderLayerFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromNonPQInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdateTime, fromNonPQInstance.UpdateTime);
        Assert.AreEqual(((IPublishedOrder)populatedAoli).OrderDisplayVolume, fromNonPQInstance.OrderDisplayVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.IsTrue(fromNonPQInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromNonPQInstance.IsEmpty);
        Assert.IsTrue(fromNonPQInstance.HasUpdates);

        var newNonPqEmptyAoli  = new AnonymousOrderLayerInfo(emptyAoli);
        var fromNonPqEmptyAoli = new PQAnonymousOrderLayerInfo(newNonPqEmptyAoli);
        fromNonPqEmptyAoli.HasUpdates = false;
        Assert.AreEqual(0, fromNonPqEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromNonPqEmptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderRemainingVolume);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromNonPqEmptyAoli.IsEmpty);
        Assert.IsFalse(fromNonPqEmptyAoli.HasUpdates);
    }

    [TestMethod]
    public void NewPqAoli_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedAoli =
            new PQAnonymousOrderLayerInfo
                (OrderId, CreatedTime, OrderVolume, ExpectedLayerFlags, ExpectedOrderType, ExpectedTypeFlags
               , ExpectedLifecycleState, UpdatedTime, OrderRemainingVolume)
                {
                    IsOrderLayerFlagsUpdated   = false, IsCreatedTimeDateUpdated = false, IsCreatedTimeSub2MinUpdated = false
                  , IsUpdateTimeDateUpdated    = false
                  , IsUpdateTimeSub2MinUpdated = false, IsOrderVolumeUpdated = false, IsOrderRemainingVolumeUpdated = false
                };
        var fromPQInstance = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsOrderIdUpdated         = false;
        newPopulatedAoli.IsOrderLayerFlagsUpdated = true;
        fromPQInstance                            = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsOrderLayerFlagsUpdated = false;
        newPopulatedAoli.IsCreatedTimeDateUpdated = true;
        fromPQInstance                            = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsCreatedTimeDateUpdated    = false;
        newPopulatedAoli.IsCreatedTimeSub2MinUpdated = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsCreatedTimeSub2MinUpdated = false;
        newPopulatedAoli.IsUpdateTimeDateUpdated     = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsUpdateTimeDateUpdated    = false;
        newPopulatedAoli.IsUpdateTimeSub2MinUpdated = true;
        fromPQInstance                              = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsUpdateTimeSub2MinUpdated = false;
        newPopulatedAoli.IsOrderVolumeUpdated       = true;
        fromPQInstance                              = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsOrderVolumeUpdated          = false;
        newPopulatedAoli.IsOrderRemainingVolumeUpdated = true;
        fromPQInstance                                 = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);
    }

    [TestMethod]
    public void PopulatedAoli_OrderIdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedAoli.HasUpdates = false;

        AssertOrdersOrderIdFieldUpdatesReturnAsExpected(populatedAoli);
    }

    public static void AssertOrdersOrderIdFieldUpdatesReturnAsExpected
    (
        IPQAnonymousOrderLayerInfo? anonOrderInfo,
        ushort orderIndex = 0,
        IPQOrdersPriceVolumeLayer? ordersLayer = null,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (anonOrderInfo == null) return;
        var olNotNull     = ordersLayer != null;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(anonOrderInfo.IsOrderIdUpdated);
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        anonOrderInfo.OrderId = 12;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.OrderId          = 0;
        anonOrderInfo.IsOrderIdUpdated = false;
        anonOrderInfo.HasUpdates       = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedOrderId = 128;
        anonOrderInfo.OrderId = expectedOrderId;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        Assert.AreEqual(expectedOrderId, anonOrderInfo.OrderId);
        Assert.IsTrue(anonOrderInfo.IsOrderIdUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var olUpdates = olNotNull
            ? ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        if (olNotNull) Assert.AreEqual(1, olUpdates.Count);
        var orderInfoUpdates = anonOrderInfo
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        var expectedOrderInfo
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderId, expectedOrderId);
        var expectedLayer     = expectedOrderInfo.WithAuxiliary(orderIndex);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);
        if (olNotNull) Assert.AreEqual(expectedLayer, olUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        anonOrderInfo.IsOrderIdUpdated = false;
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        if (olNotNull) Assert.IsFalse(ordersLayer!.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderId }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderId, foundAnonOrderInfo.OrderId);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderIdUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderId }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderId, foundAnonOrderInfo.OrderId);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderIdUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderId }
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderId, foundAnonOrderInfo.OrderId);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderIdUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderId } &&
                          update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, olUpdates.Count);
            Assert.AreEqual(expectedLayer, olUpdates[0]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
            newLayer.UpdateField(olUpdates[0]);
            var foundAnonOrderInfo = newLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderId, foundAnonOrderInfo.OrderId);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderIdUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderId }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderId, newAnonOrderInfo.OrderId);
        Assert.IsTrue(newAnonOrderInfo.IsOrderIdUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.OrderId    = 0;
        anonOrderInfo.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedAoli_OrderFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedAoli.HasUpdates = false;

        AssertOrdersOrderFlagsFieldUpdatesReturnAsExpected(populatedAoli);
    }

    public static void AssertOrdersOrderFlagsFieldUpdatesReturnAsExpected
    (
        IPQAnonymousOrderLayerInfo? anonOrderInfo,
        ushort orderIndex = 0,
        IPQOrdersPriceVolumeLayer? ordersLayer = null,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (anonOrderInfo == null) return;
        var olNotNull     = ordersLayer != null;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(anonOrderInfo.IsOrderIdUpdated);
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        anonOrderInfo.OrderLayerFlags = LayerOrderFlags.AmendRequested;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.OrderLayerFlags  = LayerOrderFlags.None;
        anonOrderInfo.IsOrderIdUpdated = false;
        anonOrderInfo.HasUpdates       = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedOrderFlags = LayerOrderFlags.IsInternallyCreatedOrder | LayerOrderFlags.IsSyntheticTrackingOrder;
        anonOrderInfo.OrderLayerFlags = expectedOrderFlags;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        Assert.AreEqual(expectedOrderFlags, anonOrderInfo.OrderLayerFlags);
        Assert.IsTrue(anonOrderInfo.IsOrderLayerFlagsUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var olUpdates = olNotNull
            ? ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        if (olNotNull) Assert.AreEqual(1, olUpdates.Count);
        var orderInfoUpdates = anonOrderInfo
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        var expectedOrderInfo
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderLayerFlags, (uint)expectedOrderFlags);
        var expectedLayer     = expectedOrderInfo.WithAuxiliary(orderIndex);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);
        if (olNotNull) Assert.AreEqual(expectedLayer, olUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        anonOrderInfo.IsOrderLayerFlagsUpdated = false;
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        if (olNotNull) Assert.IsFalse(ordersLayer!.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderLayerFlags }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderLayerFlags);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderLayerFlagsUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderLayerFlags }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderLayerFlags);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderLayerFlagsUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderLayerFlags }
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderLayerFlags);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderLayerFlagsUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderLayerFlags } &&
                          update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, olUpdates.Count);
            Assert.AreEqual(expectedLayer, olUpdates[0]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
            newLayer.UpdateField(olUpdates[0]);
            var foundAnonOrderInfo = newLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderLayerFlags);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderLayerFlagsUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderLayerFlags }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderFlags, newAnonOrderInfo.OrderLayerFlags);
        Assert.IsTrue(newAnonOrderInfo.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.OrderLayerFlags = LayerOrderFlags.None;
        anonOrderInfo.HasUpdates      = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedAoli_OrderCreatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedAoli.HasUpdates = false;

        AssertOrdersOrderCreatedTimeFieldUpdatesReturnAsExpected(populatedAoli);
    }

    public static void AssertOrdersOrderCreatedTimeFieldUpdatesReturnAsExpected
    (
        IPQAnonymousOrderLayerInfo? anonOrderInfo,
        ushort orderIndex = 0,
        IPQOrdersPriceVolumeLayer? ordersLayer = null,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (anonOrderInfo == null) return;
        var olNotNull     = ordersLayer != null;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(anonOrderInfo.IsOrderIdUpdated);
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        anonOrderInfo.CreatedTime = DateTime.Now;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.CreatedTime                 = DateTime.MinValue;
        anonOrderInfo.IsCreatedTimeDateUpdated    = false;
        anonOrderInfo.IsCreatedTimeSub2MinUpdated = false;
        anonOrderInfo.HasUpdates                  = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedCreatedTime = new DateTime(2017, 12, 03, 19, 27, 53);
        anonOrderInfo.CreatedTime = expectedCreatedTime;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        Assert.AreEqual(expectedCreatedTime, anonOrderInfo.CreatedTime);
        Assert.IsTrue(anonOrderInfo.IsCreatedTimeDateUpdated);
        Assert.IsTrue(anonOrderInfo.IsCreatedTimeSub2MinUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var olUpdates = olNotNull
            ? ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(4, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(2, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(2, bsUpdates.Count);
        if (olNotNull) Assert.AreEqual(2, olUpdates.Count);
        var orderInfoUpdates = anonOrderInfo
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(2, orderInfoUpdates.Count);
        var twoMinIntervalsSinceUnixEpoch = expectedCreatedTime.Get2MinIntervalsFromUnixEpoch();
        var expectedOrderInfoDate
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderCreatedDate, twoMinIntervalsSinceUnixEpoch);
        var extended = expectedCreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBottom);
        var expectedOrderInfoTime
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderCreatedSub2MinTime, subHourBottom, extended);
        var expectedLayerDate     = expectedOrderInfoDate.WithAuxiliary(orderIndex);
        var expectedLayerTime     = expectedOrderInfoTime.WithAuxiliary(orderIndex);
        var expectedBookSideDate  = expectedLayerDate.WithDepth(depthNoSide);
        var expectedBookSideTime  = expectedLayerTime.WithDepth(depthNoSide);
        var expectedOrderBookDate = expectedBookSideDate.WithDepth(depthWithSide);
        var expectedOrderBookTime = expectedBookSideTime.WithDepth(depthWithSide);
        Assert.AreEqual(expectedOrderInfoDate, orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderInfoTime, orderInfoUpdates[1]);
        if (olNotNull)
        {
            Assert.AreEqual(expectedLayerDate, olUpdates[0]);
            Assert.AreEqual(expectedLayerTime, olUpdates[1]);
        }
        if (bsNotNull)
        {
            Assert.AreEqual(expectedBookSideDate, bsUpdates[0]);
            Assert.AreEqual(expectedBookSideTime, bsUpdates[1]);
        }
        if (bkNotNull)
        {
            Assert.AreEqual(expectedOrderBookDate, bkUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, bkUpdates[1]);
        }
        if (l2QNotNull)
        {
            Assert.AreEqual(expectedOrderBookDate, l2QUpdates[2]);
            Assert.AreEqual(expectedOrderBookTime, l2QUpdates[3]);
        }

        anonOrderInfo.IsCreatedTimeDateUpdated    = false;
        anonOrderInfo.IsCreatedTimeSub2MinUpdated = false;
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        if (olNotNull) Assert.IsFalse(ordersLayer!.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is
                          {
                              Id: PQFeedFields.QuoteLayerOrders
                            , OrdersSubId: PQOrdersSubFieldKeys.OrderCreatedDate or PQOrdersSubFieldKeys.OrderCreatedSub2MinTime
                          }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(2, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBookDate, l2QUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, l2QUpdates[1]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            newEmpty.UpdateField(l2QUpdates[1]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedCreatedTime, foundAnonOrderInfo.CreatedTime);
            Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is
                          {
                              Id: PQFeedFields.QuoteLayerOrders
                            , OrdersSubId: PQOrdersSubFieldKeys.OrderCreatedDate or PQOrdersSubFieldKeys.OrderCreatedSub2MinTime
                          }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(2, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBookDate, bkUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, bkUpdates[1]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            newEmpty.UpdateField(bkUpdates[1]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedCreatedTime, foundAnonOrderInfo.CreatedTime);
            Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is
                          {
                              Id: PQFeedFields.QuoteLayerOrders
                            , OrdersSubId: PQOrdersSubFieldKeys.OrderCreatedDate or PQOrdersSubFieldKeys.OrderCreatedSub2MinTime
                          }
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(2, bsUpdates.Count);
            Assert.AreEqual(expectedBookSideDate, bsUpdates[0]);
            Assert.AreEqual(expectedBookSideTime, bsUpdates[1]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            newEmpty.UpdateField(bsUpdates[1]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedCreatedTime, foundAnonOrderInfo.CreatedTime);
            Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is
                          {
                              Id: PQFeedFields.QuoteLayerOrders
                            , OrdersSubId: PQOrdersSubFieldKeys.OrderCreatedDate or PQOrdersSubFieldKeys.OrderCreatedSub2MinTime
                          }
                       && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(2, olUpdates.Count);
            Assert.AreEqual(expectedLayerDate, olUpdates[0]);
            Assert.AreEqual(expectedLayerTime, olUpdates[1]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
            newLayer.UpdateField(olUpdates[0]);
            newLayer.UpdateField(olUpdates[1]);
            var foundAnonOrderInfo = newLayer[orderIndex]!;
            Assert.AreEqual(expectedCreatedTime, foundAnonOrderInfo.CreatedTime);
            Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is
                {
                    Id: PQFeedFields.QuoteLayerOrders
                  , OrdersSubId: PQOrdersSubFieldKeys.OrderCreatedDate or PQOrdersSubFieldKeys.OrderCreatedSub2MinTime
                }
                select update).ToList();
        Assert.AreEqual(2, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfoDate, orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderInfoTime, orderInfoUpdates[1]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        newAnonOrderInfo.UpdateField(orderInfoUpdates[1]);
        Assert.AreEqual(expectedCreatedTime, newAnonOrderInfo.CreatedTime);
        Assert.IsTrue(newAnonOrderInfo.IsCreatedTimeDateUpdated);
        Assert.IsTrue(newAnonOrderInfo.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.CreatedTime = DateTime.MinValue;
        anonOrderInfo.HasUpdates  = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedAoli_OrderUpdatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedAoli.HasUpdates = false;

        AssertOrdersOrderUpdatedTimeFieldUpdatesReturnAsExpected(populatedAoli);
    }

    public static void AssertOrdersOrderUpdatedTimeFieldUpdatesReturnAsExpected
    (
        IPQAnonymousOrderLayerInfo? anonOrderInfo,
        ushort orderIndex = 0,
        IPQOrdersPriceVolumeLayer? ordersLayer = null,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (anonOrderInfo == null) return;
        var olNotNull     = ordersLayer != null;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(anonOrderInfo.IsUpdateTimeDateUpdated);
        Assert.IsFalse(anonOrderInfo.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateTime = DateTime.Now;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.UpdateTime                = DateTime.MinValue;
        anonOrderInfo.IsUpdateTimeDateUpdated    = false;
        anonOrderInfo.IsUpdateTimeSub2MinUpdated = false;
        anonOrderInfo.HasUpdates                 = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedUpdatedTime = new DateTime(2017, 12, 03, 19, 27, 53);
        anonOrderInfo.UpdateTime = expectedUpdatedTime;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        Assert.AreEqual(expectedUpdatedTime, anonOrderInfo.UpdateTime);
        Assert.IsTrue(anonOrderInfo.IsUpdateTimeDateUpdated);
        Assert.IsTrue(anonOrderInfo.IsUpdateTimeSub2MinUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var olUpdates = olNotNull
            ? ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(4, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(2, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(2, bsUpdates.Count);
        if (olNotNull) Assert.AreEqual(2, olUpdates.Count);
        var orderInfoUpdates = anonOrderInfo
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(2, orderInfoUpdates.Count);
        var hoursSinceUnixEpoch   = expectedUpdatedTime.Get2MinIntervalsFromUnixEpoch();
        var expectedOrderInfoDate = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderUpdatedDate, hoursSinceUnixEpoch);
        var extended              = expectedUpdatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBottom);
        var expectedOrderInfoTime
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime, subHourBottom, extended);
        var expectedLayerDate     = expectedOrderInfoDate.WithAuxiliary(orderIndex);
        var expectedLayerTime     = expectedOrderInfoTime.WithAuxiliary(orderIndex);
        var expectedBookSideDate  = expectedLayerDate.WithDepth(depthNoSide);
        var expectedBookSideTime  = expectedLayerTime.WithDepth(depthNoSide);
        var expectedOrderBookDate = expectedBookSideDate.WithDepth(depthWithSide);
        var expectedOrderBookTime = expectedBookSideTime.WithDepth(depthWithSide);
        Assert.AreEqual(expectedOrderInfoDate, orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderInfoTime, orderInfoUpdates[1]);
        if (olNotNull)
        {
            Assert.AreEqual(expectedLayerDate, olUpdates[0]);
            Assert.AreEqual(expectedLayerTime, olUpdates[1]);
        }
        if (bsNotNull)
        {
            Assert.AreEqual(expectedBookSideDate, bsUpdates[0]);
            Assert.AreEqual(expectedBookSideTime, bsUpdates[1]);
        }
        if (bkNotNull)
        {
            Assert.AreEqual(expectedOrderBookDate, bkUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, bkUpdates[1]);
        }
        if (l2QNotNull)
        {
            Assert.AreEqual(expectedOrderBookDate, l2QUpdates[2]);
            Assert.AreEqual(expectedOrderBookTime, l2QUpdates[3]);
        }

        anonOrderInfo.IsUpdateTimeDateUpdated    = false;
        anonOrderInfo.IsUpdateTimeSub2MinUpdated = false;
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        if (olNotNull) Assert.IsFalse(ordersLayer!.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is
                          {
                              Id: PQFeedFields.QuoteLayerOrders
                            , OrdersSubId: PQOrdersSubFieldKeys.OrderUpdatedDate or PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime
                          }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(2, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBookDate, l2QUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, l2QUpdates[1]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            newEmpty.UpdateField(l2QUpdates[1]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdateTime);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdateTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdateTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is
                          {
                              Id: PQFeedFields.QuoteLayerOrders
                            , OrdersSubId: PQOrdersSubFieldKeys.OrderUpdatedDate or PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime
                          }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(2, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBookDate, bkUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, bkUpdates[1]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            newEmpty.UpdateField(bkUpdates[1]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdateTime);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdateTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdateTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is
                          {
                              Id: PQFeedFields.QuoteLayerOrders
                            , OrdersSubId: PQOrdersSubFieldKeys.OrderUpdatedDate or PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime
                          }
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(2, bsUpdates.Count);
            Assert.AreEqual(expectedBookSideDate, bsUpdates[0]);
            Assert.AreEqual(expectedBookSideTime, bsUpdates[1]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            newEmpty.UpdateField(bsUpdates[1]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdateTime);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdateTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdateTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is
                          {
                              Id: PQFeedFields.QuoteLayerOrders
                            , OrdersSubId: PQOrdersSubFieldKeys.OrderUpdatedDate or PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime
                          }
                       && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(2, olUpdates.Count);
            Assert.AreEqual(expectedLayerDate, olUpdates[0]);
            Assert.AreEqual(expectedLayerTime, olUpdates[1]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
            newLayer.UpdateField(olUpdates[0]);
            newLayer.UpdateField(olUpdates[1]);
            var foundAnonOrderInfo = newLayer[orderIndex]!;
            Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdateTime);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdateTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdateTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is
                {
                    Id: PQFeedFields.QuoteLayerOrders
                  , OrdersSubId: PQOrdersSubFieldKeys.OrderUpdatedDate or PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime
                }
                select update).ToList();
        Assert.AreEqual(2, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfoDate, orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderInfoTime, orderInfoUpdates[1]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        newAnonOrderInfo.UpdateField(orderInfoUpdates[1]);
        Assert.AreEqual(expectedUpdatedTime, newAnonOrderInfo.UpdateTime);
        Assert.IsTrue(newAnonOrderInfo.IsUpdateTimeDateUpdated);
        Assert.IsTrue(newAnonOrderInfo.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.UpdateTime = DateTime.MinValue;
        anonOrderInfo.HasUpdates  = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedAoli_OrderRemainingVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedAoli.HasUpdates = false;

        AssertOrdersOrderRemainingVolumeFieldUpdatesReturnAsExpected(populatedAoli);
    }

    public static void AssertOrdersOrderRemainingVolumeFieldUpdatesReturnAsExpected
    (
        IPQAnonymousOrderLayerInfo? anonOrderInfo,
        ushort orderIndex = 0,
        IPQOrdersPriceVolumeLayer? ordersLayer = null,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (anonOrderInfo == null) return;
        var olNotNull     = ordersLayer != null;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(anonOrderInfo.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        anonOrderInfo.OrderRemainingVolume = 12;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.OrderRemainingVolume          = 0;
        anonOrderInfo.IsOrderRemainingVolumeUpdated = false;
        anonOrderInfo.HasUpdates                    = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedOrderRemainingVolume = 4682m;
        anonOrderInfo.OrderRemainingVolume = expectedOrderRemainingVolume;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        Assert.AreEqual(expectedOrderRemainingVolume, anonOrderInfo.OrderRemainingVolume);
        Assert.IsTrue(anonOrderInfo.IsOrderRemainingVolumeUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var olUpdates = olNotNull
            ? ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        if (olNotNull) Assert.AreEqual(1, olUpdates.Count);
        var orderInfoUpdates = anonOrderInfo
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        var expectedOrderInfo
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderRemainingVolume, expectedOrderRemainingVolume
                              , precisionSettings.VolumeScalingPrecision);
        var expectedLayer     = expectedOrderInfo.WithAuxiliary(orderIndex);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);
        if (olNotNull) Assert.AreEqual(expectedLayer, olUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        anonOrderInfo.IsOrderRemainingVolumeUpdated = false;
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        if (olNotNull) Assert.IsFalse(ordersLayer!.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderRemainingVolume }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderRemainingVolume, foundAnonOrderInfo.OrderRemainingVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderRemainingVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderRemainingVolume }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderRemainingVolume, foundAnonOrderInfo.OrderRemainingVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderRemainingVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderRemainingVolume }
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderRemainingVolume, foundAnonOrderInfo.OrderRemainingVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderRemainingVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderRemainingVolume } &&
                          update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, olUpdates.Count);
            Assert.AreEqual(expectedLayer, olUpdates[0]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
            newLayer.UpdateField(olUpdates[0]);
            var foundAnonOrderInfo = newLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderRemainingVolume, foundAnonOrderInfo.OrderRemainingVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderRemainingVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderRemainingVolume }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderRemainingVolume, newAnonOrderInfo.OrderRemainingVolume);
        Assert.IsTrue(newAnonOrderInfo.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.OrderRemainingVolume = 0;
        anonOrderInfo.HasUpdates           = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedAoli_OrderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedAoli.HasUpdates = false;

        AssertOrdersOrderVolumeFieldUpdatesReturnAsExpected(populatedAoli);
    }

    public static void AssertOrdersOrderVolumeFieldUpdatesReturnAsExpected
    (
        IPQAnonymousOrderLayerInfo? anonOrderInfo,
        ushort orderIndex = 0,
        IPQOrdersPriceVolumeLayer? ordersLayer = null,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (anonOrderInfo == null) return;
        var olNotNull     = ordersLayer != null;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(anonOrderInfo.IsOrderVolumeUpdated);
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        anonOrderInfo.OrderDisplayVolume = 12;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.OrderDisplayVolume = 0;
        anonOrderInfo.IsOrderVolumeUpdated                  = false;
        anonOrderInfo.HasUpdates                            = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedOrderVolume = 4682m;
        anonOrderInfo.OrderDisplayVolume = expectedOrderVolume;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        Assert.AreEqual(expectedOrderVolume, ((IPublishedOrder)anonOrderInfo).OrderDisplayVolume);
        Assert.IsTrue(anonOrderInfo.IsOrderVolumeUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var olUpdates = olNotNull
            ? ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        if (olNotNull) Assert.AreEqual(1, olUpdates.Count);
        var orderInfoUpdates = anonOrderInfo
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        var expectedOrderInfo
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderDisplayVolume, expectedOrderVolume
                              , precisionSettings.VolumeScalingPrecision);
        var expectedLayer     = expectedOrderInfo.WithAuxiliary(orderIndex);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);
        if (olNotNull) Assert.AreEqual(expectedLayer, olUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        anonOrderInfo.IsOrderVolumeUpdated = false;
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        if (olNotNull) Assert.IsFalse(ordersLayer!.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderDisplayVolume }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderVolume, ((IPublishedOrder)foundAnonOrderInfo).OrderDisplayVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderDisplayVolume }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderVolume, ((IPublishedOrder)foundAnonOrderInfo).OrderDisplayVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderDisplayVolume }
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderVolume, ((IPublishedOrder)foundAnonOrderInfo).OrderDisplayVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderDisplayVolume } &&
                          update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, olUpdates.Count);
            Assert.AreEqual(expectedLayer, olUpdates[0]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
            newLayer.UpdateField(olUpdates[0]);
            var foundAnonOrderInfo = newLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderVolume, ((IPublishedOrder)foundAnonOrderInfo).OrderDisplayVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderDisplayVolume }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderVolume, newAnonOrderInfo.OrderDisplayVolume);
        Assert.IsTrue(newAnonOrderInfo.IsOrderVolumeUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.OrderDisplayVolume = 0;
        anonOrderInfo.HasUpdates                            = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }


    [TestMethod]
    public void EmptyPqAoli_OrderIdChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.IsTrue(newEmptyPqAoli.IsEmpty);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedOrderId = 101101;
        emptyAoli.OrderId = changedOrderId;

        Assert.AreEqual(changedOrderId, emptyAoli.OrderId);
        Assert.IsTrue(emptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderId = changedOrderId;

        Assert.AreEqual(changedOrderId, newEmptyPqAoli.OrderId);
        Assert.IsTrue(newEmptyPqAoli.IsOrderIdUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderFlagsChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.IsTrue(newEmptyPqAoli.IsEmpty);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedOrderFlags = LayerOrderFlags.CreatedBySimulator | LayerOrderFlags.IsSyntheticTrackingOrder | LayerOrderFlags.NotExternalVolume;
        emptyAoli.OrderLayerFlags = changedOrderFlags;

        Assert.AreEqual(changedOrderFlags, emptyAoli.OrderLayerFlags);
        Assert.IsTrue(emptyAoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderLayerFlags = changedOrderFlags;

        Assert.AreEqual(changedOrderFlags, newEmptyPqAoli.OrderLayerFlags);
        Assert.IsTrue(newEmptyPqAoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderCreatedTimeChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo
        {
            CreatedTime = DateTime.UnixEpoch
        };
        newEmptyPqAoli.UpdateComplete();
        emptyAoli.CreatedTime = DateTime.UnixEpoch;
        emptyAoli.UpdateComplete();

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
        Assert.IsFalse(emptyAoli.HasUpdates);

        var changedCreatedSub2Min = DateTime.UnixEpoch.AddMinutes(1);
        emptyAoli.CreatedTime = changedCreatedSub2Min;

        Assert.AreEqual(changedCreatedSub2Min, emptyAoli.CreatedTime);
        Assert.IsTrue(emptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(emptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.CreatedTime = changedCreatedSub2Min;

        Assert.AreEqual(changedCreatedSub2Min, newEmptyPqAoli.CreatedTime);
        Assert.IsTrue(newEmptyPqAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        newEmptyPqAoli.CreatedTime = DateTime.UnixEpoch;
        newEmptyPqAoli.HasUpdates  = false;
        emptyAoli.CreatedTime      = DateTime.UnixEpoch;
        emptyAoli.HasUpdates       = false;
        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedCreatedDate = DateTime.UnixEpoch.AddHours(1);
        emptyAoli.CreatedTime = changedCreatedDate;

        Assert.AreEqual(changedCreatedDate, emptyAoli.CreatedTime);
        Assert.IsFalse(emptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.CreatedTime = changedCreatedDate;

        Assert.AreEqual(changedCreatedDate, newEmptyPqAoli.CreatedTime);
        Assert.IsFalse(newEmptyPqAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        emptyAoli             = new PQAnonymousOrderLayerInfo();
        emptyAoli.CreatedTime = DateTime.UnixEpoch;
        emptyAoli.HasUpdates  = false;

        newEmptyPqAoli = new PQAnonymousOrderLayerInfo
        {
            CreatedTime = DateTime.UnixEpoch, HasUpdates = false
        };
        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedCreatedDateAndHour = DateTime.UnixEpoch.AddMinutes(90);
        emptyAoli.CreatedTime = changedCreatedDateAndHour;

        Assert.AreEqual(changedCreatedDateAndHour, emptyAoli.CreatedTime);
        Assert.IsTrue(emptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.CreatedTime = changedCreatedDateAndHour;

        Assert.AreEqual(changedCreatedDateAndHour, newEmptyPqAoli.CreatedTime);
        Assert.IsTrue(newEmptyPqAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderUpdatedTimeChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo
        {
            UpdateTime = DateTime.UnixEpoch
        };
        newEmptyPqAoli.UpdateComplete();
        emptyAoli.UpdateTime = DateTime.UnixEpoch;
        emptyAoli.UpdateComplete();

        Assert.IsFalse(emptyAoli.HasUpdates);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);
        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedUpdatedSub2Min = DateTime.UnixEpoch.AddMinutes(1);
        emptyAoli.UpdateTime = changedUpdatedSub2Min;

        Assert.AreEqual(changedUpdatedSub2Min, emptyAoli.UpdateTime);
        Assert.IsTrue(emptyAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(emptyAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.UpdateTime = changedUpdatedSub2Min;

        Assert.AreEqual(changedUpdatedSub2Min, newEmptyPqAoli.UpdateTime);
        Assert.IsTrue(newEmptyPqAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
        newEmptyPqAoli.UpdateTime = DateTime.UnixEpoch;
        newEmptyPqAoli.HasUpdates = false;
        emptyAoli.UpdateTime     = DateTime.UnixEpoch;
        emptyAoli.HasUpdates      = false;

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedUpdatedHour = DateTime.UnixEpoch.AddHours(2);
        emptyAoli.UpdateTime = changedUpdatedHour;

        Assert.AreEqual(changedUpdatedHour, emptyAoli.UpdateTime);
        Assert.IsFalse(emptyAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.UpdateTime = changedUpdatedHour;

        Assert.AreEqual(changedUpdatedHour, newEmptyPqAoli.UpdateTime);
        Assert.IsFalse(newEmptyPqAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
        newEmptyPqAoli.UpdateTime = DateTime.UnixEpoch;
        newEmptyPqAoli.HasUpdates = false;
        emptyAoli.UpdateTime     = DateTime.UnixEpoch;
        emptyAoli.HasUpdates      = false;

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedUpdatedDateAndHour = DateTime.UnixEpoch.AddMinutes(3);
        emptyAoli.UpdateTime = changedUpdatedDateAndHour;

        Assert.AreEqual(changedUpdatedDateAndHour, emptyAoli.UpdateTime);
        Assert.IsTrue(emptyAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.UpdateTime = changedUpdatedDateAndHour;

        Assert.AreEqual(changedUpdatedDateAndHour, newEmptyPqAoli.UpdateTime);
        Assert.IsTrue(newEmptyPqAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderVolumeChanged_ExpectedPropertiesUpdated()
    {
        Assert.AreEqual(0m, ((IPublishedOrder)emptyAoli).OrderDisplayVolume);
        Assert.IsFalse(emptyAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(emptyAoli.IsEmpty);
        Assert.IsFalse(emptyAoli.HasUpdates);
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.AreEqual(0m, newEmptyPqAoli.OrderDisplayVolume);
        Assert.IsFalse(newEmptyPqAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsEmpty);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedOrderVolume = 4_294_967_280;
        emptyAoli.OrderDisplayVolume = changedOrderVolume;

        Assert.AreEqual(changedOrderVolume, ((IPublishedOrder)emptyAoli).OrderDisplayVolume);
        Assert.IsTrue(emptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderDisplayVolume = changedOrderVolume;

        Assert.AreEqual(changedOrderVolume, newEmptyPqAoli.OrderDisplayVolume);
        Assert.IsTrue(newEmptyPqAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderRemainingVolumeChanged_ExpectedPropertiesUpdated()
    {
        Assert.AreEqual(0m, ((IPublishedOrder)emptyAoli).OrderDisplayVolume);
        Assert.IsFalse(emptyAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(emptyAoli.IsEmpty);
        Assert.IsFalse(emptyAoli.HasUpdates);
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.AreEqual(0m, newEmptyPqAoli.OrderDisplayVolume);
        Assert.IsFalse(newEmptyPqAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsEmpty);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedOrderRemainingVolume = 2_294_967_280;
        emptyAoli.OrderRemainingVolume = changedOrderRemainingVolume;

        Assert.AreEqual(changedOrderRemainingVolume, emptyAoli.OrderRemainingVolume);
        Assert.IsTrue(emptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderRemainingVolume = changedOrderRemainingVolume;

        Assert.AreEqual(changedOrderRemainingVolume, newEmptyPqAoli.OrderRemainingVolume);
        Assert.IsTrue(newEmptyPqAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyAndPopulatedPqAoli_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPqAoli_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.AreEqual(OrderId, populatedAoli.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, populatedAoli.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, populatedAoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, populatedAoli.UpdateTime);
        Assert.AreEqual(OrderVolume, ((IPublishedOrder)populatedAoli).OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, populatedAoli.OrderRemainingVolume);
        Assert.IsTrue(populatedAoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedAoli.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsUpdateTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(populatedAoli.IsEmpty);
        Assert.IsTrue(populatedAoli.HasUpdates);
        populatedAoli.IsEmpty = true;
        Assert.AreEqual(0, populatedAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, populatedAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.UpdateTime);
        Assert.AreEqual(0m, ((IPublishedOrder)populatedAoli).OrderDisplayVolume);
        Assert.AreEqual(0m, populatedAoli.OrderRemainingVolume);
        Assert.IsTrue(populatedAoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedAoli.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsUpdateTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsEmpty);
        Assert.IsTrue(populatedAoli.HasUpdates);
        populatedAoli.StateReset();
        Assert.IsFalse(populatedAoli.IsOrderIdUpdated);
        Assert.IsFalse(populatedAoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(populatedAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(populatedAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(populatedAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(populatedAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(populatedAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(populatedAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsEmpty);
        Assert.IsFalse(populatedAoli.HasUpdates);
    }

    [TestMethod]
    public void FullyPopulatedAoli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQTraderLayerInfo = new AnonymousOrderLayerInfo(populatedAoli);
        emptyAoli.CopyFrom(nonPQTraderLayerInfo);
        Assert.AreEqual(populatedAoli, emptyAoli);
    }

    [TestMethod]
    public void FullyPopulatedPqAoli_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IMutableAnonymousOrderLayerInfo)populatedAoli).Clone();
        Assert.AreNotSame(clonedQuote, populatedAoli);
        Assert.AreEqual(populatedAoli, clonedQuote);

        var cloned2 = (PQAnonymousOrderLayerInfo)((ICloneable)populatedAoli).Clone();
        Assert.AreNotSame(cloned2, populatedAoli);
        Assert.AreEqual(populatedAoli, cloned2);

        var cloned3 = (PQAnonymousOrderLayerInfo)populatedAoli.Clone();
        Assert.AreNotSame(cloned3, populatedAoli);
        Assert.AreEqual(populatedAoli, cloned3);

        var cloned4 = ((PQAnonymousOrderLayerInfo)populatedAoli).Clone();
        Assert.AreNotSame(cloned4, populatedAoli);
        Assert.AreEqual(populatedAoli, cloned4);
    }

    [TestMethod]
    public void FullyPopulatedPqAoliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQAnonymousOrderLayerInfo)((ICloneable)populatedAoli).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedAoli, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedAoli, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedPqAoliSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedAoli, populatedAoli);
        Assert.AreEqual(populatedAoli, ((ICloneable)populatedAoli).Clone());
        Assert.AreEqual(populatedAoli, ((IMutableAnonymousOrderLayerInfo)populatedAoli).Clone());
        Assert.AreEqual(populatedAoli, ((ICloneable<IAnonymousOrderLayerInfo>)populatedAoli).Clone());
        Assert.AreEqual(populatedAoli, ((PQAnonymousOrderLayerInfo)populatedAoli).Clone());
        Assert.AreEqual(populatedAoli, populatedAoli.Clone());
    }

    [TestMethod]
    public void FullyPopulatedTli_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedAoli.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedTli_ToString_ReturnsNameAndValues()
    {
        var toString = populatedAoli.ToString()!;

        Assert.IsTrue(toString.Contains(populatedAoli.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderId)}: {populatedAoli.OrderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderLayerFlags)}: {populatedAoli.OrderLayerFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.CreatedTime)}: {populatedAoli.CreatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.UpdateTime)}: {populatedAoli.UpdateTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(IPublishedOrder.OrderDisplayVolume)}: {((IPublishedOrder)populatedAoli).OrderDisplayVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderRemainingVolume)}: {populatedAoli.OrderRemainingVolume:N2}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQAnonymousOrderLayerInfo? original,
        IPQAnonymousOrderLayerInfo? changingTraderLayerInfo,
        IOrdersPriceVolumeLayer? originalTraderPriceVolumeLayer = null,
        IOrdersPriceVolumeLayer? changingTraderPriceVolumeLayer = null,
        IOrderBookSide? originalOrderBook = null,
        IOrderBookSide? changingOrderBook = null,
        IPublishableLevel2Quote? originalQuote = null,
        IPublishableLevel2Quote? changingQuote = null)
    {
        if (original == null && changingTraderLayerInfo == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingTraderLayerInfo);

        if (original.GetType() == typeof(PQAnonymousOrderLayerInfo))
            Assert.AreEqual(!exactComparison, original.AreEquivalent(
                                                                     new AnonymousOrderLayerInfo(changingTraderLayerInfo), exactComparison));

        AnonymousOrderLayerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingTraderLayerInfo, originalTraderPriceVolumeLayer
           , changingTraderPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingTraderLayerInfo.IsOrderIdUpdated = !changingTraderLayerInfo.IsOrderIdUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsOrderIdUpdated = original.IsOrderIdUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsOrderLayerFlagsUpdated = !changingTraderLayerInfo.IsOrderLayerFlagsUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsOrderLayerFlagsUpdated = original.IsOrderLayerFlagsUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsCreatedTimeDateUpdated = !changingTraderLayerInfo.IsCreatedTimeDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsCreatedTimeDateUpdated = original.IsCreatedTimeDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsCreatedTimeSub2MinUpdated = !changingTraderLayerInfo.IsCreatedTimeSub2MinUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsCreatedTimeSub2MinUpdated = original.IsCreatedTimeSub2MinUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsUpdateTimeDateUpdated = !changingTraderLayerInfo.IsUpdateTimeDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsUpdateTimeDateUpdated = original.IsUpdateTimeDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsUpdateTimeSub2MinUpdated = !changingTraderLayerInfo.IsUpdateTimeSub2MinUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsUpdateTimeSub2MinUpdated = original.IsUpdateTimeSub2MinUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsOrderVolumeUpdated = !changingTraderLayerInfo.IsOrderVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsOrderVolumeUpdated = original.IsOrderVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsOrderRemainingVolumeUpdated = !changingTraderLayerInfo.IsOrderRemainingVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsOrderRemainingVolumeUpdated = original.IsOrderRemainingVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
