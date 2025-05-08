// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQAnonymousOrderLayerInfoTests
{
    private const LayerOrderFlags OrderFlags = LayerOrderFlags.CreatedFromSource | LayerOrderFlags.IsInternallyCreatedOrder;

    private const int     OrderId              = 80085;
    private const decimal OrderVolume          = 100_000.50m;
    private const decimal OrderRemainingVolume = 50_000.25m;

    private readonly DateTime CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private readonly DateTime UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private IPQAnonymousOrderLayerInfo emptyAoli = null!;

    private static IPQNameIdLookupGenerator   emptyNameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);

    private        IPQNameIdLookupGenerator   nameIdLookup      = null!;
    private        IPQAnonymousOrderLayerInfo populatedAoli     = null!;
    
    private static DateTime testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        nameIdLookup      = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        emptyAoli         = new PQAnonymousOrderLayerInfo();
        populatedAoli     = new PQAnonymousOrderLayerInfo(OrderId, OrderFlags, CreatedTime, OrderVolume, UpdatedTime, OrderRemainingVolume);
    }

    [TestMethod]
    public void NewPqAoli_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newAoli = new PQAnonymousOrderLayerInfo(OrderId, OrderFlags, CreatedTime, OrderVolume);
        Assert.AreEqual(OrderId, newAoli.OrderId);
        Assert.AreEqual(OrderFlags, newAoli.OrderFlags);
        Assert.AreEqual(CreatedTime, newAoli.CreatedTime);
        Assert.AreEqual(CreatedTime, newAoli.UpdatedTime);
        Assert.AreEqual(OrderVolume, newAoli.OrderVolume);
        Assert.AreEqual(OrderVolume, newAoli.OrderRemainingVolume);

        Assert.AreEqual(0, emptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, emptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, emptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, emptyAoli.UpdatedTime);
        Assert.AreEqual(0m, emptyAoli.OrderVolume);
        Assert.AreEqual(0m, emptyAoli.OrderRemainingVolume);
    }

    [TestMethod]
    public void NewPqAoli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var fromInstance = new PQAnonymousOrderLayerInfo(populatedAoli);
        Assert.AreEqual(populatedAoli.OrderId, fromInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderFlags, fromInstance.OrderFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdatedTime, fromInstance.UpdatedTime);
        Assert.AreEqual(populatedAoli.OrderVolume, fromInstance.OrderVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromInstance.OrderRemainingVolume);
        Assert.IsTrue(fromInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromInstance.IsOrderFlagsUpdated);
        Assert.IsTrue(fromInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromInstance.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(fromInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(fromInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromInstance.IsEmpty);
        Assert.IsTrue(fromInstance.HasUpdates);

        var fromEmptyAoli = new PQAnonymousOrderLayerInfo(emptyAoli);
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);
        Assert.IsFalse(fromEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromEmptyAoli.IsEmpty);
        Assert.IsFalse(fromEmptyAoli.HasUpdates);

        var nonPqInstance     = new AnonymousOrderLayerInfo(populatedAoli);
        var fromNonPQInstance = new PQAnonymousOrderLayerInfo(nonPqInstance);
        Assert.AreEqual(populatedAoli.OrderId, fromNonPQInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderFlags, fromNonPQInstance.OrderFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromNonPQInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdatedTime, fromNonPQInstance.UpdatedTime);
        Assert.AreEqual(populatedAoli.OrderVolume, fromNonPQInstance.OrderVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.IsTrue(fromNonPQInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderFlagsUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromNonPQInstance.IsEmpty);
        Assert.IsTrue(fromNonPQInstance.HasUpdates);

        var newNonPqEmptyAoli  = new AnonymousOrderLayerInfo(emptyAoli);
        var fromNonPqEmptyAoli = new PQAnonymousOrderLayerInfo(newNonPqEmptyAoli);
        fromNonPqEmptyAoli.HasUpdates = false;
        Assert.AreEqual(0, fromNonPqEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromNonPqEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderRemainingVolume);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromNonPqEmptyAoli.IsEmpty);
        Assert.IsFalse(fromNonPqEmptyAoli.HasUpdates);
    }

    [TestMethod]
    public void NewPqAoli_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedAoli = new PQAnonymousOrderLayerInfo(OrderId, OrderFlags, CreatedTime, OrderVolume, UpdatedTime, OrderRemainingVolume)
        {
            IsOrderFlagsUpdated = false, IsCreatedTimeDateUpdated = false, IsCreatedTimeSub2MinUpdated = false, IsUpdatedTimeDateUpdated = false
          , IsUpdatedTimeSub2MinUpdated = false, IsOrderVolumeUpdated = false, IsOrderRemainingVolumeUpdated = false
        };
        var fromPQInstance = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsOrderIdUpdated    = false;
        newPopulatedAoli.IsOrderFlagsUpdated = true;
        fromPQInstance                       = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsOrderFlagsUpdated      = false;
        newPopulatedAoli.IsCreatedTimeDateUpdated = true;
        fromPQInstance                            = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsCreatedTimeDateUpdated    = false;
        newPopulatedAoli.IsCreatedTimeSub2MinUpdated = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsCreatedTimeSub2MinUpdated = false;
        newPopulatedAoli.IsUpdatedTimeDateUpdated    = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsUpdatedTimeDateUpdated    = false;
        newPopulatedAoli.IsUpdatedTimeSub2MinUpdated = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsUpdatedTimeSub2MinUpdated = false;
        newPopulatedAoli.IsOrderVolumeUpdated        = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsOrderVolumeUpdated          = false;
        newPopulatedAoli.IsOrderRemainingVolumeUpdated = true;
        fromPQInstance                                 = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
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
        IPQLevel2Quote? l2Quote = null
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
        anonOrderInfo.HasUpdates           = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedOrderId= 128;
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
            = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderId, expectedOrderId);
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
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderId } 
                   && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
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
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderId } 
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
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderId } 
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
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderId } && update.AuxiliaryPayload == orderIndex
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
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderId }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderId, newAnonOrderInfo.OrderId);
        Assert.IsTrue(newAnonOrderInfo.IsOrderIdUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.OrderId = 0;
        anonOrderInfo.HasUpdates     = false;
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
        IPQLevel2Quote? l2Quote = null
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
        anonOrderInfo.OrderFlags =  LayerOrderFlags.AmendRequested;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.OrderFlags       = LayerOrderFlags.None;
        anonOrderInfo.IsOrderIdUpdated = false;
        anonOrderInfo.HasUpdates       = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedOrderFlags= LayerOrderFlags.IsInternallyCreatedOrder | LayerOrderFlags.IsSyntheticTrackingOrder;
        anonOrderInfo.OrderFlags = expectedOrderFlags;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        Assert.AreEqual(expectedOrderFlags, anonOrderInfo.OrderFlags);
        Assert.IsTrue(anonOrderInfo.IsOrderFlagsUpdated);
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
            = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderFlags, (uint)expectedOrderFlags);
        var expectedLayer     = expectedOrderInfo.WithAuxiliary(orderIndex);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);
        if (olNotNull) Assert.AreEqual(expectedLayer, olUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        anonOrderInfo.IsOrderFlagsUpdated = false;
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
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderFlags } 
                   && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderFlags);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderFlagsUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderFlags } 
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderFlags);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderFlagsUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderFlags } 
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderFlags);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderFlagsUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderFlags } && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, olUpdates.Count);
            Assert.AreEqual(expectedLayer, olUpdates[0]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
            newLayer.UpdateField(olUpdates[0]);
            var foundAnonOrderInfo = newLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderFlags);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderFlagsUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderFlags }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderFlags, newAnonOrderInfo.OrderFlags);
        Assert.IsTrue(newAnonOrderInfo.IsOrderFlagsUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.OrderFlags = LayerOrderFlags.None;
        anonOrderInfo.HasUpdates = false;
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
        IPQLevel2Quote? l2Quote = null
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
        anonOrderInfo.CreatedTime =  DateTime.Now;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.CreatedTime      = DateTime.MinValue;
        anonOrderInfo.IsCreatedTimeDateUpdated = false;
        anonOrderInfo.IsCreatedTimeSub2MinUpdated = false;
        anonOrderInfo.HasUpdates       = false;

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
        var twoMinIntervalsSinceUnixEpoch       = expectedCreatedTime.Get2MinIntervalsFromUnixEpoch();
        var expectedOrderInfoDate    = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderCreatedDate, twoMinIntervalsSinceUnixEpoch);
        var extended                  = expectedCreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBottom);
        var expectedOrderInfoTime = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderCreatedSub2MinTime, subHourBottom, extended);
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

        anonOrderInfo.IsCreatedTimeDateUpdated = false;
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
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderCreatedDate or PQSubFieldKeys.OrderCreatedSub2MinTime } 
                   && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                select update).ToList();
            Assert.AreEqual(2, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBookDate, l2QUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, l2QUpdates[1]);

            var newEmpty = new PQLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
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
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderCreatedDate or PQSubFieldKeys.OrderCreatedSub2MinTime } 
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
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderCreatedDate or PQSubFieldKeys.OrderCreatedSub2MinTime } 
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
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderCreatedDate or PQSubFieldKeys.OrderCreatedSub2MinTime }
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
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderCreatedDate or PQSubFieldKeys.OrderCreatedSub2MinTime }
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
        IPQLevel2Quote? l2Quote = null
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

        Assert.IsFalse(anonOrderInfo.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(anonOrderInfo.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdatedTime =  DateTime.Now;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.UpdatedTime                 = DateTime.MinValue;
        anonOrderInfo.IsUpdatedTimeDateUpdated    = false;
        anonOrderInfo.IsUpdatedTimeSub2MinUpdated = false;
        anonOrderInfo.HasUpdates                  = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        
        var expectedUpdatedTime = new DateTime(2017, 12, 03, 19, 27, 53);
        anonOrderInfo.UpdatedTime = expectedUpdatedTime;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        Assert.AreEqual(expectedUpdatedTime, anonOrderInfo.UpdatedTime);
        Assert.IsTrue(anonOrderInfo.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(anonOrderInfo.IsUpdatedTimeSub2MinUpdated);
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
        var hoursSinceUnixEpoch       = expectedUpdatedTime.Get2MinIntervalsFromUnixEpoch();
        var expectedOrderInfoDate    = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderUpdatedDate, hoursSinceUnixEpoch);
        var extended                  = expectedUpdatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBottom);
        var expectedOrderInfoTime = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderUpdatedSub2MinTime, subHourBottom, extended);
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

        anonOrderInfo.IsUpdatedTimeDateUpdated = false;
        anonOrderInfo.IsUpdatedTimeSub2MinUpdated = false;
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
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderUpdatedDate or PQSubFieldKeys.OrderUpdatedSub2MinTime } 
                   && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                select update).ToList();
            Assert.AreEqual(2, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBookDate, l2QUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, l2QUpdates[1]);

            var newEmpty = new PQLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            newEmpty.UpdateField(l2QUpdates[1]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdatedTime);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderUpdatedDate or PQSubFieldKeys.OrderUpdatedSub2MinTime } 
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
            Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdatedTime);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderUpdatedDate or PQSubFieldKeys.OrderUpdatedSub2MinTime } 
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
            Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdatedTime);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderUpdatedDate or PQSubFieldKeys.OrderUpdatedSub2MinTime }
                       && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(2, olUpdates.Count);
            Assert.AreEqual(expectedLayerDate, olUpdates[0]);
            Assert.AreEqual(expectedLayerTime, olUpdates[1]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
            newLayer.UpdateField(olUpdates[0]);
            newLayer.UpdateField(olUpdates[1]);
            var foundAnonOrderInfo = newLayer[orderIndex]!;
            Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdatedTime);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeDateUpdated);
            Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeSub2MinUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderUpdatedDate or PQSubFieldKeys.OrderUpdatedSub2MinTime }
                select update).ToList();
        Assert.AreEqual(2, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfoDate, orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderInfoTime, orderInfoUpdates[1]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        newAnonOrderInfo.UpdateField(orderInfoUpdates[1]);
        Assert.AreEqual(expectedUpdatedTime, newAnonOrderInfo.UpdatedTime);
        Assert.IsTrue(newAnonOrderInfo.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(newAnonOrderInfo.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.UpdatedTime = DateTime.MinValue;
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
        IPQLevel2Quote? l2Quote = null
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
        anonOrderInfo.HasUpdates           = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedOrderRemainingVolume= 4682m;
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
            = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderRemainingVolume, expectedOrderRemainingVolume, precisionSettings.VolumeScalingPrecision);
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
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderRemainingVolume } 
                   && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
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
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderRemainingVolume } 
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
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderRemainingVolume } 
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
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderRemainingVolume } && update.AuxiliaryPayload == orderIndex
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
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderRemainingVolume }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderRemainingVolume, newAnonOrderInfo.OrderRemainingVolume);
        Assert.IsTrue(newAnonOrderInfo.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.OrderRemainingVolume = 0;
        anonOrderInfo.HasUpdates     = false;
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
        IPQLevel2Quote? l2Quote = null
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
        anonOrderInfo.OrderVolume = 12;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        anonOrderInfo.UpdateComplete();
        anonOrderInfo.OrderVolume          = 0;
        anonOrderInfo.IsOrderVolumeUpdated = false;
        anonOrderInfo.HasUpdates           = false;

        Assert.AreEqual(0, anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedOrderVolume= 4682m;
        anonOrderInfo.OrderVolume = expectedOrderVolume;
        Assert.IsTrue(anonOrderInfo.HasUpdates);
        Assert.AreEqual(expectedOrderVolume, anonOrderInfo.OrderVolume);
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
            = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderVolume, expectedOrderVolume, precisionSettings.VolumeScalingPrecision);
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
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderVolume } 
                   && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderVolume, foundAnonOrderInfo.OrderVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderVolume } 
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderVolume, foundAnonOrderInfo.OrderVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderVolume } 
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = foundLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderVolume, foundAnonOrderInfo.OrderVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderVolume } && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, olUpdates.Count);
            Assert.AreEqual(expectedLayer, olUpdates[0]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
            newLayer.UpdateField(olUpdates[0]);
            var foundAnonOrderInfo = newLayer[orderIndex]!;
            Assert.AreEqual(expectedOrderVolume, foundAnonOrderInfo.OrderVolume);
            Assert.IsTrue(foundAnonOrderInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in anonOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQQuoteFields.LayerOrders, SubId: PQSubFieldKeys.OrderVolume }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQAnonymousOrderLayerInfo();
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedOrderVolume, newAnonOrderInfo.OrderVolume);
        Assert.IsTrue(newAnonOrderInfo.IsOrderVolumeUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        anonOrderInfo.OrderVolume = 0;
        anonOrderInfo.HasUpdates     = false;
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

        var changedOrderFlags = LayerOrderFlags.CreatedFromSimulator | LayerOrderFlags.IsSyntheticTrackingOrder | LayerOrderFlags.NotExternalVolume;
        emptyAoli.OrderFlags = changedOrderFlags;

        Assert.AreEqual(changedOrderFlags, emptyAoli.OrderFlags);
        Assert.IsTrue(emptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderFlags = changedOrderFlags;

        Assert.AreEqual(changedOrderFlags, newEmptyPqAoli.OrderFlags);
        Assert.IsTrue(newEmptyPqAoli.IsOrderFlagsUpdated);
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

        var changedCreatedSubHour = DateTime.UnixEpoch.AddMinutes(1);
        emptyAoli.CreatedTime = changedCreatedSubHour;

        Assert.AreEqual(changedCreatedSubHour, emptyAoli.CreatedTime);
        Assert.IsTrue(emptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(emptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.CreatedTime = changedCreatedSubHour;

        Assert.AreEqual(changedCreatedSubHour, newEmptyPqAoli.CreatedTime);
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
            UpdatedTime = DateTime.UnixEpoch
        };
        newEmptyPqAoli.UpdateComplete();
        emptyAoli.UpdatedTime = DateTime.UnixEpoch;
        emptyAoli.UpdateComplete();

        Assert.IsFalse(emptyAoli.HasUpdates);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);
        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedUpdatedSubHour = DateTime.UnixEpoch.AddMinutes(1);
        emptyAoli.UpdatedTime = changedUpdatedSubHour;

        Assert.AreEqual(changedUpdatedSubHour, emptyAoli.UpdatedTime);
        Assert.IsTrue(emptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(emptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.UpdatedTime = changedUpdatedSubHour;

        Assert.AreEqual(changedUpdatedSubHour, newEmptyPqAoli.UpdatedTime);
        Assert.IsTrue(newEmptyPqAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
        newEmptyPqAoli.UpdatedTime = DateTime.UnixEpoch;
        newEmptyPqAoli.HasUpdates  = false;
        emptyAoli.UpdatedTime      = DateTime.UnixEpoch;
        emptyAoli.HasUpdates       = false;

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedUpdatedHour = DateTime.UnixEpoch.AddHours(2);
        emptyAoli.UpdatedTime = changedUpdatedHour;

        Assert.AreEqual(changedUpdatedHour, emptyAoli.UpdatedTime);
        Assert.IsFalse(emptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.UpdatedTime = changedUpdatedHour;

        Assert.AreEqual(changedUpdatedHour, newEmptyPqAoli.UpdatedTime);
        Assert.IsFalse(newEmptyPqAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
        newEmptyPqAoli.UpdatedTime = DateTime.UnixEpoch;
        newEmptyPqAoli.HasUpdates  = false;
        emptyAoli.UpdatedTime      = DateTime.UnixEpoch;
        emptyAoli.HasUpdates       = false;

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedUpdatedDateAndHour = DateTime.UnixEpoch.AddMinutes(3);
        emptyAoli.UpdatedTime = changedUpdatedDateAndHour;

        Assert.AreEqual(changedUpdatedDateAndHour, emptyAoli.UpdatedTime);
        Assert.IsTrue(emptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.UpdatedTime = changedUpdatedDateAndHour;

        Assert.AreEqual(changedUpdatedDateAndHour, newEmptyPqAoli.UpdatedTime);
        Assert.IsTrue(newEmptyPqAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderVolumeChanged_ExpectedPropertiesUpdated()
    {
        Assert.AreEqual(0m, emptyAoli.OrderVolume);
        Assert.IsFalse(emptyAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(emptyAoli.IsEmpty);
        Assert.IsFalse(emptyAoli.HasUpdates);
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.AreEqual(0m, newEmptyPqAoli.OrderVolume);
        Assert.IsFalse(newEmptyPqAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsEmpty);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedOrderVolume = 4_294_967_280;
        emptyAoli.OrderVolume = changedOrderVolume;

        Assert.AreEqual(changedOrderVolume, emptyAoli.OrderVolume);
        Assert.IsTrue(emptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderVolume = changedOrderVolume;

        Assert.AreEqual(changedOrderVolume, newEmptyPqAoli.OrderVolume);
        Assert.IsTrue(newEmptyPqAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderRemainingVolumeChanged_ExpectedPropertiesUpdated()
    {
        Assert.AreEqual(0m, emptyAoli.OrderVolume);
        Assert.IsFalse(emptyAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(emptyAoli.IsEmpty);
        Assert.IsFalse(emptyAoli.HasUpdates);
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.AreEqual(0m, newEmptyPqAoli.OrderVolume);
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
        Assert.AreEqual(OrderFlags, populatedAoli.OrderFlags);
        Assert.AreEqual(CreatedTime, populatedAoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, populatedAoli.UpdatedTime);
        Assert.AreEqual(OrderVolume, populatedAoli.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, populatedAoli.OrderRemainingVolume);
        Assert.IsTrue(populatedAoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedAoli.IsOrderFlagsUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(populatedAoli.IsEmpty);
        Assert.IsTrue(populatedAoli.HasUpdates);
        populatedAoli.IsEmpty = true;
        Assert.AreEqual(0, populatedAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, populatedAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.UpdatedTime);
        Assert.AreEqual(0m, populatedAoli.OrderVolume);
        Assert.AreEqual(0m, populatedAoli.OrderRemainingVolume);
        Assert.IsTrue(populatedAoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedAoli.IsOrderFlagsUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsEmpty);
        Assert.IsTrue(populatedAoli.HasUpdates);
        populatedAoli.StateReset();
        Assert.IsFalse(populatedAoli.IsOrderIdUpdated);
        Assert.IsFalse(populatedAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(populatedAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(populatedAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(populatedAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(populatedAoli.IsUpdatedTimeSub2MinUpdated);
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderFlags)}: {populatedAoli.OrderFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.CreatedTime)}: {populatedAoli.CreatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.UpdatedTime)}: {populatedAoli.UpdatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderVolume)}: {populatedAoli.OrderVolume:N2}"));
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
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
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

        changingTraderLayerInfo.IsOrderFlagsUpdated = !changingTraderLayerInfo.IsOrderFlagsUpdated;
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
        changingTraderLayerInfo.IsOrderFlagsUpdated = original.IsOrderFlagsUpdated;
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

        changingTraderLayerInfo.IsUpdatedTimeDateUpdated = !changingTraderLayerInfo.IsUpdatedTimeDateUpdated;
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
        changingTraderLayerInfo.IsUpdatedTimeDateUpdated = original.IsUpdatedTimeDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsUpdatedTimeSub2MinUpdated = !changingTraderLayerInfo.IsUpdatedTimeSub2MinUpdated;
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
        changingTraderLayerInfo.IsUpdatedTimeSub2MinUpdated = original.IsUpdatedTimeSub2MinUpdated;
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
