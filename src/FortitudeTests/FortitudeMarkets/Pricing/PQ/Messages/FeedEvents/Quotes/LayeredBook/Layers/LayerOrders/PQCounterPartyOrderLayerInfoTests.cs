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
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

[TestClass]
public class PQCounterPartyOrderLayerInfoTests
{

    private const int     OrderId              = 80085;
    private const decimal OrderVolume          = 100_000.50m;
    private const decimal OrderRemainingVolume = 50_000.25m;
    private const string  CounterPartyName     = "TestCounterPartyName";
    private const string  TraderName           = "TestTraderName";
    

    private const int ExpectedTrackingId       = 7;
    private const int ExpectedTraderId       = 2;
    private const int ExpectedCounterPartyId = 1;

    private const OrderFlags      ExpectedTypeFlags  = OrderFlags.FromAdapter;
    private const OrderType       ExpectedOrderType  = OrderType.PassiveLimit;
    private const LayerOrderFlags ExpectedLayerFlags = LayerOrderFlags.ExplicitlyDefinedFromSource | LayerOrderFlags.IsInternallyCreatedOrder;

    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.SourceActiveOnMarket;

    private static readonly DateTime CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private IPQCounterPartyOrderLayerInfo emptyCpoli = null!;

    private static IPQNameIdLookupGenerator emptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

    private IPQNameIdLookupGenerator      nameIdLookup   = null!;
    private IPQCounterPartyOrderLayerInfo populatedCpoli = null!;


    private static DateTime testDateTime = new (2017, 10, 08, 18, 33, 24);

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        nameIdLookup      = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        emptyCpoli        = new PQCounterPartyOrderLayerInfo(emptyNameIdLookup.Clone());
        populatedCpoli = new PQCounterPartyOrderLayerInfo
            (nameIdLookup, OrderId, CreatedTime, OrderVolume, ExpectedLayerFlags, ExpectedOrderType, ExpectedTypeFlags, ExpectedLifecycleState
           , CounterPartyName, TraderName, ExpectedCounterPartyId, ExpectedTraderId, UpdatedTime, OrderRemainingVolume, ExpectedTrackingId);
    }

    [TestMethod]
    public void NewTli_SetsValues_PropertiesInitializedAsExpected()
    {
        var newCpoli = new PQCounterPartyOrderLayerInfo
            (nameIdLookup, OrderId, CreatedTime, OrderVolume, ExpectedLayerFlags, ExpectedOrderType, ExpectedTypeFlags, ExpectedLifecycleState
          , CounterPartyName, TraderName, ExpectedCounterPartyId, ExpectedTraderId, UpdatedTime, OrderRemainingVolume);

        Assert.AreEqual(OrderId, newCpoli.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, newCpoli.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, newCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, newCpoli.UpdateTime);
        Assert.AreEqual(OrderVolume, newCpoli.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, newCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, newCpoli.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, newCpoli.ExternalTraderName);
        Assert.IsNotNull(newCpoli.NameIdLookup);
        Assert.IsTrue(newCpoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(newCpoli.IsOrderIdUpdated);
        Assert.IsTrue(newCpoli.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(newCpoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(newCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(newCpoli.IsUpdateTimeDateUpdated);
        Assert.IsTrue(newCpoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(newCpoli.IsOrderVolumeUpdated);
        Assert.IsTrue(newCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(newCpoli.IsExternalCounterPartyNameUpdated);
        Assert.IsTrue(newCpoli.IsExternalTraderNameUpdated);
        Assert.IsFalse(newCpoli.IsEmpty);
        Assert.IsTrue(newCpoli.HasUpdates);

        var fromEmptyAoli = new PQCounterPartyOrderLayerInfo(emptyNameIdLookup);
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromEmptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);
        Assert.IsNotNull(fromEmptyAoli.NameIdLookup);
        Assert.IsNull(fromEmptyAoli.ExternalCounterPartyName);
        Assert.IsNull(fromEmptyAoli.ExternalTraderName);
        Assert.IsFalse(fromEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsExternalCounterPartyNameUpdated);
        Assert.IsFalse(fromEmptyAoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(fromEmptyAoli.IsEmpty);
        Assert.IsFalse(fromEmptyAoli.HasUpdates);

        var nonPqInstance = new ExternalCounterPartyOrderLayerInfo(populatedCpoli);
        var fromNonPQInstance
            = new PQCounterPartyOrderLayerInfo(nonPqInstance, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(populatedCpoli.OrderId, fromNonPQInstance.OrderId);
        Assert.AreEqual(populatedCpoli.OrderLayerFlags, fromNonPQInstance.OrderLayerFlags);
        Assert.AreEqual(populatedCpoli.CreatedTime, fromNonPQInstance.CreatedTime);
        Assert.AreEqual(populatedCpoli.UpdateTime, fromNonPQInstance.UpdateTime);
        Assert.AreEqual(((IPublishedOrder)populatedCpoli).OrderDisplayVolume, fromNonPQInstance.OrderDisplayVolume);
        Assert.AreEqual(populatedCpoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.AreEqual(populatedCpoli.ExternalCounterPartyName, fromNonPQInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromNonPQInstance.ExternalTraderName);
        Assert.IsNotNull(fromNonPQInstance.NameIdLookup);
        Assert.IsTrue(fromNonPQInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsExternalCounterPartyNameUpdated);
        Assert.IsTrue(fromNonPQInstance.IsExternalTraderNameUpdated);
        Assert.IsFalse(fromNonPQInstance.IsEmpty);
        Assert.IsTrue(fromNonPQInstance.HasUpdates);

        var newNonPqEmptyAoli = new ExternalCounterPartyOrderLayerInfo(emptyCpoli);
        var fromNonPqEmptyAoli
            = new PQCounterPartyOrderLayerInfo(newNonPqEmptyAoli, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(0, fromNonPqEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromNonPqEmptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderRemainingVolume);
        Assert.IsNull(fromNonPqEmptyAoli.ExternalCounterPartyName);
        Assert.IsNull(fromNonPqEmptyAoli.ExternalTraderName);
        Assert.IsNotNull(fromNonPqEmptyAoli.NameIdLookup);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsExternalCounterPartyNameUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(fromNonPqEmptyAoli.IsEmpty);
        Assert.IsFalse(fromNonPqEmptyAoli.HasUpdates);
    }

    [TestMethod]
    public void NewTli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newCpoli = new PQCounterPartyOrderLayerInfo(populatedCpoli, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));

        Assert.AreEqual(OrderId, newCpoli.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, newCpoli.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, newCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, newCpoli.UpdateTime);
        Assert.AreEqual(OrderVolume, newCpoli.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, newCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, newCpoli.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, newCpoli.ExternalTraderName);
        Assert.IsNotNull(newCpoli.NameIdLookup);
        Assert.IsTrue(newCpoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(newCpoli.IsOrderIdUpdated);
        Assert.IsTrue(newCpoli.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(newCpoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(newCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(newCpoli.IsUpdateTimeDateUpdated);
        Assert.IsTrue(newCpoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(newCpoli.IsOrderVolumeUpdated);
        Assert.IsTrue(newCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(newCpoli.IsExternalCounterPartyNameUpdated);
        Assert.IsTrue(newCpoli.IsExternalTraderNameUpdated);
        Assert.IsFalse(newCpoli.IsEmpty);
        Assert.IsTrue(newCpoli.HasUpdates);

        var fromEmptyAoli = new PQCounterPartyOrderLayerInfo(emptyCpoli, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromEmptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);
        Assert.IsNotNull(fromEmptyAoli.NameIdLookup);
        Assert.IsNull(fromEmptyAoli.ExternalCounterPartyName);
        Assert.IsNull(fromEmptyAoli.ExternalTraderName);
        Assert.IsFalse(fromEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsExternalCounterPartyNameUpdated);
        Assert.IsFalse(fromEmptyAoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(fromEmptyAoli.IsEmpty);
        Assert.IsFalse(fromEmptyAoli.HasUpdates);

        var nonPqInstance = new ExternalCounterPartyOrderLayerInfo(populatedCpoli);
        var fromNonPQInstance
            = new PQCounterPartyOrderLayerInfo(nonPqInstance, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(populatedCpoli.OrderId, fromNonPQInstance.OrderId);
        Assert.AreEqual(populatedCpoli.OrderLayerFlags, fromNonPQInstance.OrderLayerFlags);
        Assert.AreEqual(populatedCpoli.CreatedTime, fromNonPQInstance.CreatedTime);
        Assert.AreEqual(populatedCpoli.UpdateTime, fromNonPQInstance.UpdateTime);
        Assert.AreEqual(((IPublishedOrder)populatedCpoli).OrderDisplayVolume, fromNonPQInstance.OrderDisplayVolume);
        Assert.AreEqual(populatedCpoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.AreEqual(populatedCpoli.ExternalCounterPartyName, fromNonPQInstance.ExternalCounterPartyName);
        Assert.AreEqual(populatedCpoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.IsNotNull(fromNonPQInstance.NameIdLookup);
        Assert.IsTrue(fromNonPQInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsExternalCounterPartyNameUpdated);
        Assert.IsTrue(fromNonPQInstance.IsExternalTraderNameUpdated);
        Assert.IsFalse(fromNonPQInstance.IsEmpty);
        Assert.IsTrue(fromNonPQInstance.HasUpdates);

        var newNonPqEmptyAoli = new ExternalCounterPartyOrderLayerInfo(emptyCpoli);
        var fromNonPqEmptyAoli
            = new PQCounterPartyOrderLayerInfo(newNonPqEmptyAoli, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(0, fromNonPqEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromNonPqEmptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderRemainingVolume);
        Assert.IsNull(fromNonPqEmptyAoli.ExternalCounterPartyName);
        Assert.IsNull(fromNonPqEmptyAoli.ExternalTraderName);
        Assert.IsNotNull(fromNonPqEmptyAoli.NameIdLookup);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsExternalCounterPartyNameUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(fromNonPqEmptyAoli.IsEmpty);
        Assert.IsFalse(fromNonPqEmptyAoli.HasUpdates);
    }

    [TestMethod]
    public void NewTli_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedAoli = new PQCounterPartyOrderLayerInfo
            (emptyNameIdLookup, OrderId, CreatedTime, OrderVolume, ExpectedLayerFlags, ExpectedOrderType, ExpectedTypeFlags
           , ExpectedLifecycleState, CounterPartyName, TraderName, ExpectedCounterPartyId, ExpectedTraderId, UpdatedTime, OrderRemainingVolume)
        {
            IsOrderIdUpdated         = false, IsOrderLayerFlagsUpdated = false, IsCreatedTimeDateUpdated = false, IsCreatedTimeSub2MinUpdated = false
          , IsUpdateTimeDateUpdated = false, IsUpdateTimeSub2MinUpdated = false, IsOrderVolumeUpdated = false, IsOrderRemainingVolumeUpdated = false
          , IsExternalTraderNameUpdated      = false
        };
        var fromPQInstance
            = new PQCounterPartyOrderLayerInfo(newPopulatedAoli, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPQInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromPQInstance.ExternalTraderName);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsExternalTraderNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExternalCounterPartyNameUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsExternalCounterPartyNameUpdated = false;
        newPopulatedAoli.IsExternalTraderNameUpdated       = true;
        fromPQInstance = new PQCounterPartyOrderLayerInfo(newPopulatedAoli
                                                        , new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPQInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromPQInstance.ExternalTraderName);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsExternalCounterPartyNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExternalTraderNameUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);
    }

    [TestMethod]
    public void PopulatedCpoli_OrdersCounterPartyNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedCpoli.HasUpdates = false;

        AssertOrdersCounterPartyNameFieldUpdatesReturnAsExpected(populatedCpoli);
    }

    public static void AssertOrdersCounterPartyNameFieldUpdatesReturnAsExpected
    (
        IPQCounterPartyOrderLayerInfo? cpOrderInfo,
        ushort orderIndex = 0,
        IPQOrdersPriceVolumeLayer? ordersLayer = null,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (cpOrderInfo == null) return;
        var olNotNull     = ordersLayer != null;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(cpOrderInfo.IsExternalCounterPartyNameUpdated);
        Assert.IsFalse(cpOrderInfo.HasUpdates);
        cpOrderInfo.ExternalCounterPartyName = "blah";
        Assert.IsTrue(cpOrderInfo.HasUpdates);
        cpOrderInfo.UpdateComplete();
        cpOrderInfo.ExternalCounterPartyName          = null;
        cpOrderInfo.IsExternalCounterPartyNameUpdated = false;
        cpOrderInfo.HasUpdates       = false;

        Assert.AreEqual(0, cpOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        
        var expectedCounterPartyName = "NewChangedCounterPartyName" + orderIndex;
        cpOrderInfo.ExternalCounterPartyName = expectedCounterPartyName;
        Assert.IsTrue(cpOrderInfo.HasUpdates);
        Assert.AreEqual(expectedCounterPartyName, cpOrderInfo.ExternalCounterPartyName);
        Assert.IsTrue(cpOrderInfo.IsExternalCounterPartyNameUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCounterPartyL3TraderNamePaidOrGivenSti;
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
        var orderInfoUpdates = cpOrderInfo
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        var dictId             = cpOrderInfo.NameIdLookup[expectedCounterPartyName];
        var expectedOrderInfo
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId, dictId);
        var expectedLayer     = expectedOrderInfo.WithAuxiliary(orderIndex);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);
        if (olNotNull) Assert.AreEqual(expectedLayer, olUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        cpOrderInfo.IsExternalCounterPartyNameUpdated = false;
        Assert.IsFalse(cpOrderInfo.HasUpdates);
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
        Assert.IsTrue(cpOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in l2Quote.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundCpOrderInfo = (IPQCounterPartyOrderLayerInfo)foundLayer[orderIndex]!;
            Assert.AreEqual(expectedCounterPartyName, foundCpOrderInfo.ExternalCounterPartyName);
            Assert.IsTrue(foundCpOrderInfo.IsExternalCounterPartyNameUpdated);
            Assert.IsTrue(foundCpOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in orderBook.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = (IPQCounterPartyOrderLayerInfo)foundLayer[orderIndex]!;
            Assert.AreEqual(expectedCounterPartyName, foundAnonOrderInfo.ExternalCounterPartyName);
            Assert.IsTrue(foundAnonOrderInfo.IsExternalCounterPartyNameUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId }
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in orderBookSide.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = (IPQCounterPartyOrderLayerInfo)foundLayer[orderIndex]!;
            Assert.AreEqual(expectedCounterPartyName, foundAnonOrderInfo.ExternalCounterPartyName);
            Assert.IsTrue(foundAnonOrderInfo.IsExternalCounterPartyNameUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId } && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, olUpdates.Count);
            Assert.AreEqual(expectedLayer, olUpdates[0]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            foreach (var stringUpdate in ordersLayer.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newLayer.UpdateFieldString(stringUpdate);
            }
            newLayer.UpdateField(olUpdates[0]);
            var foundAnonOrderInfo = (IPQCounterPartyOrderLayerInfo)newLayer[orderIndex]!;
            Assert.AreEqual(expectedCounterPartyName, foundAnonOrderInfo.ExternalCounterPartyName);
            Assert.IsTrue(foundAnonOrderInfo.IsExternalCounterPartyNameUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in cpOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQCounterPartyOrderLayerInfo();
        foreach (var stringUpdate in cpOrderInfo.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
        {
            newAnonOrderInfo.UpdateFieldString(stringUpdate);
        }
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedCounterPartyName, newAnonOrderInfo.ExternalCounterPartyName);
        Assert.IsTrue(newAnonOrderInfo.IsExternalCounterPartyNameUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        cpOrderInfo.ExternalCounterPartyName    = null;
        cpOrderInfo.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedCpoli_OrdersTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedCpoli.HasUpdates = false;

        AssertOrdersTraderNameFieldUpdatesReturnAsExpected(populatedCpoli);
    }

    public static void AssertOrdersTraderNameFieldUpdatesReturnAsExpected
    (
        IPQCounterPartyOrderLayerInfo? cpOrderInfo,
        ushort orderIndex = 0,
        IPQOrdersPriceVolumeLayer? ordersLayer = null,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (cpOrderInfo == null) return;
        var olNotNull     = ordersLayer != null;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(cpOrderInfo.IsExternalTraderNameUpdated);
        Assert.IsFalse(cpOrderInfo.HasUpdates);
        cpOrderInfo.ExternalTraderName = "blah";
        Assert.IsTrue(cpOrderInfo.HasUpdates);
        cpOrderInfo.UpdateComplete();
        cpOrderInfo.ExternalTraderName          = null;
        cpOrderInfo.IsExternalTraderNameUpdated = false;
        cpOrderInfo.HasUpdates       = false;

        Assert.AreEqual(0, cpOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        
        var expectedTraderName = "NewChangedTraderName" + orderIndex;
        cpOrderInfo.ExternalTraderName = expectedTraderName;
        Assert.IsTrue(cpOrderInfo.HasUpdates);
        Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyInfoOrder)cpOrderInfo).ExternalTraderName);
        Assert.IsTrue(cpOrderInfo.IsExternalTraderNameUpdated);
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
        var orderInfoUpdates = cpOrderInfo
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        var dictId             = cpOrderInfo.NameIdLookup[expectedTraderName];
        var expectedOrderInfo
            = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalTraderNameId, dictId);
        var expectedLayer     = expectedOrderInfo.WithAuxiliary(orderIndex);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);
        if (olNotNull) Assert.AreEqual(expectedLayer, olUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        cpOrderInfo.IsExternalTraderNameUpdated = false;
        Assert.IsFalse(cpOrderInfo.HasUpdates);
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
        Assert.IsTrue(cpOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalTraderNameId }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in l2Quote.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            var foundCpOrderInfo = (IPQCounterPartyOrderLayerInfo)foundLayer[orderIndex]!;
            Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyInfoOrder)foundCpOrderInfo).ExternalTraderName);
            Assert.IsTrue(foundCpOrderInfo.IsExternalTraderNameUpdated);
            Assert.IsTrue(foundCpOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalTraderNameId }
                       && update.DepthId == depthWithSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in orderBook.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            var foundAnonOrderInfo = (IPQCounterPartyOrderLayerInfo)foundLayer[orderIndex]!;
            Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyInfoOrder)foundAnonOrderInfo).ExternalTraderName);
            Assert.IsTrue(foundAnonOrderInfo.IsExternalTraderNameUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalTraderNameId }
                       && update.DepthId == depthNoSide && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in orderBookSide.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQOrdersPriceVolumeLayer)newEmpty[bookDepth]!;
            var foundAnonOrderInfo = (IPQCounterPartyOrderLayerInfo)foundLayer[orderIndex]!;
            Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyInfoOrder)foundAnonOrderInfo).ExternalTraderName);
            Assert.IsTrue(foundAnonOrderInfo.IsExternalTraderNameUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (olNotNull)
        {
            olUpdates =
                (from update in ordersLayer!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalTraderNameId } && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, olUpdates.Count);
            Assert.AreEqual(expectedLayer, olUpdates[0]);

            var newLayer = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            foreach (var stringUpdate in ordersLayer.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newLayer.UpdateFieldString(stringUpdate);
            }
            newLayer.UpdateField(olUpdates[0]);
            var foundAnonOrderInfo = (IPQCounterPartyOrderLayerInfo)newLayer[orderIndex]!;
            Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyInfoOrder)foundAnonOrderInfo).ExternalTraderName);
            Assert.IsTrue(foundAnonOrderInfo.IsExternalTraderNameUpdated);
            Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
            Assert.IsTrue(newLayer.HasUpdates);
        }
        orderInfoUpdates =
            (from update in cpOrderInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQFeedFields.QuoteLayerOrders, OrdersSubId: PQOrdersSubFieldKeys.OrderExternalTraderNameId }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedOrderInfo, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQCounterPartyOrderLayerInfo();
        foreach (var stringUpdate in cpOrderInfo.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
        {
            newAnonOrderInfo.UpdateFieldString(stringUpdate);
        }
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedTraderName, newAnonOrderInfo.ExternalTraderName);
        Assert.IsTrue(newAnonOrderInfo.IsExternalTraderNameUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        cpOrderInfo.ExternalTraderName    = null;
        cpOrderInfo.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }


    [TestMethod]
    public void EmptyCpoli_CounterPartyNameChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqCpoli = new PQCounterPartyOrderLayerInfo();
        Assert.IsTrue(newEmptyPqCpoli.IsEmpty);
        Assert.IsFalse(newEmptyPqCpoli.HasUpdates);

        Assert.AreEqual(emptyCpoli, newEmptyPqCpoli);

        var expectedCounterPartyName = "SetCounterPartyNameToThis";
        emptyCpoli.ExternalCounterPartyName = expectedCounterPartyName;

        Assert.AreEqual(expectedCounterPartyName, emptyCpoli.ExternalCounterPartyName);
        Assert.AreEqual(1, emptyCpoli.ExternalCounterPartyNameId);
        Assert.IsTrue(emptyCpoli.IsExternalCounterPartyNameUpdated);
        Assert.AreEqual(1, emptyCpoli.NameIdLookup.Count);
        Assert.IsFalse(emptyCpoli.IsEmpty);
        Assert.IsTrue(emptyCpoli.HasUpdates);

        Assert.AreNotEqual(emptyCpoli, newEmptyPqCpoli);

        newEmptyPqCpoli.ExternalCounterPartyName = expectedCounterPartyName;

        Assert.AreEqual(expectedCounterPartyName, newEmptyPqCpoli.ExternalCounterPartyName);
        Assert.AreEqual(1, newEmptyPqCpoli.ExternalCounterPartyNameId);
        Assert.IsTrue(newEmptyPqCpoli.IsExternalCounterPartyNameUpdated);
        Assert.AreEqual(1, newEmptyPqCpoli.NameIdLookup.Count);
        Assert.IsFalse(newEmptyPqCpoli.IsEmpty);
        Assert.IsTrue(newEmptyPqCpoli.HasUpdates);

        Assert.AreEqual(emptyCpoli, newEmptyPqCpoli);
    }

    [TestMethod]
    public void EmptyPqCpoli_TradeNameChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqCpoli = new PQCounterPartyOrderLayerInfo();
        Assert.IsTrue(newEmptyPqCpoli.IsEmpty);
        Assert.IsFalse(newEmptyPqCpoli.HasUpdates);

        Assert.AreEqual(emptyCpoli, newEmptyPqCpoli);

        var expectedTraderName = "SetTraderNameToThis";
        emptyCpoli.ExternalTraderName = expectedTraderName;

        Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyInfoOrder)emptyCpoli).ExternalTraderName);
        Assert.AreEqual(1, emptyCpoli.ExternalTraderNameId);
        Assert.IsTrue(emptyCpoli.IsExternalTraderNameUpdated);
        Assert.AreEqual(1, emptyCpoli.NameIdLookup.Count);
        Assert.IsFalse(emptyCpoli.IsEmpty);
        Assert.IsTrue(emptyCpoli.HasUpdates);

        Assert.AreNotEqual(emptyCpoli, newEmptyPqCpoli);

        newEmptyPqCpoli.ExternalTraderName = expectedTraderName;

        Assert.AreEqual(expectedTraderName, newEmptyPqCpoli.ExternalTraderName);
        Assert.AreEqual(1, newEmptyPqCpoli.ExternalTraderNameId);
        Assert.IsTrue(newEmptyPqCpoli.IsExternalTraderNameUpdated);
        Assert.AreEqual(1, newEmptyPqCpoli.NameIdLookup.Count);
        Assert.IsFalse(newEmptyPqCpoli.IsEmpty);
        Assert.IsTrue(newEmptyPqCpoli.HasUpdates);

        Assert.AreEqual(emptyCpoli, newEmptyPqCpoli);
    }


    [TestMethod]
    public void EmptyAndPopulatedTli_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedCpoli.IsEmpty);
        Assert.IsTrue(emptyCpoli.IsEmpty);
    }

    [TestMethod]
    public void PopulatedTli_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.AreEqual(OrderId, populatedCpoli.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, populatedCpoli.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, populatedCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, populatedCpoli.UpdateTime);
        Assert.AreEqual(OrderVolume, ((IPublishedOrder)populatedCpoli).OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, populatedCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, populatedCpoli.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, ((IExternalCounterPartyInfoOrder)populatedCpoli).ExternalTraderName);
        Assert.IsTrue(populatedCpoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(populatedCpoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedCpoli.IsUpdateTimeDateUpdated);
        Assert.IsTrue(populatedCpoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(populatedCpoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(populatedCpoli.IsExternalCounterPartyNameUpdated);
        Assert.IsFalse(populatedCpoli.IsEmpty);
        Assert.IsTrue(populatedCpoli.HasUpdates);
        populatedCpoli.IsEmpty = true;
        Assert.AreEqual(0, populatedCpoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, populatedCpoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.UpdateTime);
        Assert.AreEqual(0m, ((IPublishedOrder)populatedCpoli).OrderDisplayVolume);
        Assert.AreEqual(0m, populatedCpoli.OrderRemainingVolume);
        Assert.IsNull(populatedCpoli.ExternalCounterPartyName);
        Assert.IsNull(((IExternalCounterPartyInfoOrder)populatedCpoli).ExternalTraderName);
        Assert.IsTrue(populatedCpoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderLayerFlagsUpdated);
        Assert.IsTrue(populatedCpoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedCpoli.IsUpdateTimeDateUpdated);
        Assert.IsTrue(populatedCpoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(populatedCpoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(populatedCpoli.IsExternalCounterPartyNameUpdated);
        Assert.IsTrue(populatedCpoli.IsEmpty);
        Assert.IsTrue(populatedCpoli.HasUpdates);
        populatedCpoli.StateReset();
        Assert.IsFalse(populatedCpoli.IsOrderIdUpdated);
        Assert.IsFalse(populatedCpoli.IsOrderLayerFlagsUpdated);
        Assert.IsFalse(populatedCpoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(populatedCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(populatedCpoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(populatedCpoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(populatedCpoli.IsOrderVolumeUpdated);
        Assert.IsFalse(populatedCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(populatedCpoli.IsExternalTraderNameUpdated);
        Assert.IsFalse(populatedCpoli.IsExternalCounterPartyNameUpdated);
        Assert.IsTrue(populatedCpoli.IsEmpty);
        Assert.IsFalse(populatedCpoli.HasUpdates);
    }

    [TestMethod]
    public void FullyPopulatedTli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQTraderLayerInfo = new ExternalCounterPartyOrderLayerInfo(populatedCpoli);
        emptyCpoli.CopyFrom(nonPQTraderLayerInfo);
        Assert.AreEqual(populatedCpoli, emptyCpoli);
    }

    [TestMethod]
    public void FullyPopulatedTli_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IAnonymousOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(clonedQuote, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clonedQuote);

        var cloned2 = (PQAnonymousOrderLayerInfo)((ICloneable)populatedCpoli).Clone();
        Assert.AreNotSame(cloned2, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned2);

        var cloned3 = (PQAnonymousOrderLayerInfo)((IMutableAnonymousOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(cloned3, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned3);

        var cloned4 = (PQAnonymousOrderLayerInfo)((IExternalCounterPartyOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(cloned4, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned4);

        var cloned5 = (PQAnonymousOrderLayerInfo)((IMutableExternalCounterPartyOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(cloned5, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned5);

        var cloned6 = (PQAnonymousOrderLayerInfo)((PQCounterPartyOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(cloned6, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned6);

        var cloned7 = populatedCpoli.Clone();
        Assert.AreNotSame(cloned7, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned7);
    }

    [TestMethod]
    public void FullyPopulatedPqCpoliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQCounterPartyOrderLayerInfo)((ICloneable)populatedCpoli).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedCpoli, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedCpoli, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedTliSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedCpoli, populatedCpoli);
        Assert.AreEqual(populatedCpoli, ((ICloneable)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IAnonymousOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((ICloneable<IAnonymousOrderLayerInfo>)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IMutableAnonymousOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IExternalCounterPartyOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IMutableExternalCounterPartyOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((PQCounterPartyOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, populatedCpoli.Clone());
    }

    [TestMethod]
    public void FullyPopulatedTli_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedCpoli.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedTli_ToString_ReturnsNameAndValues()
    {
        var toString = populatedCpoli.ToString()!;

        Assert.IsTrue(toString.Contains(populatedCpoli.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderId)}: {populatedCpoli.OrderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderLayerFlags)}: {populatedCpoli.OrderLayerFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.CreatedTime)}: {populatedCpoli.CreatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.UpdateTime)}: {populatedCpoli.UpdateTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(IPublishedOrder.OrderDisplayVolume)}: {((IPublishedOrder)populatedCpoli).OrderDisplayVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderRemainingVolume)}: {populatedCpoli.OrderRemainingVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(IExternalCounterPartyInfoOrder.ExternalTraderName)}: {((IExternalCounterPartyInfoOrder)populatedCpoli).ExternalTraderName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.ExternalCounterPartyName)}: {populatedCpoli.ExternalCounterPartyName}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQCounterPartyOrderLayerInfo? original,
        IPQCounterPartyOrderLayerInfo? changingTraderLayerInfo,
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

        if (original.GetType() == typeof(PQCounterPartyOrderLayerInfo))
            Assert.AreEqual
                (!exactComparison,
                 original.AreEquivalent((IAnonymousOrderLayerInfo)new ExternalCounterPartyOrderLayerInfo(changingTraderLayerInfo), exactComparison));

        PQAnonymousOrderLayerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingTraderLayerInfo, originalTraderPriceVolumeLayer
           , changingTraderPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingTraderLayerInfo.IsExternalCounterPartyNameUpdated = !changingTraderLayerInfo.IsExternalCounterPartyNameUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsExternalCounterPartyNameUpdated = original.IsExternalCounterPartyNameUpdated;
        Assert.IsTrue(original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsExternalTraderNameUpdated = !changingTraderLayerInfo.IsExternalTraderNameUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsExternalTraderNameUpdated = original.IsExternalTraderNameUpdated;
        Assert.IsTrue(original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
