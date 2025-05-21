// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

[TestClass]
public class PQExternalCounterPartyOrderTests
{
    private const int     OrderId              = 80085;
    private const decimal OrderVolume          = 100_000.50m;
    private const decimal OrderRemainingVolume = 50_000.25m;
    private const string  CounterPartyName     = "TestCounterPartyName";
    private const string  TraderName           = "TestTraderName";
    

    private const int ExpectedTrackingId       = 7;
    private const int ExpectedTraderId       = 2;
    private const int ExpectedCounterPartyId = 1;

    private const OrderGenesisFlags ExpectedGenesisFlags = OrderGenesisFlags.FromAdapter | OrderGenesisFlags.IsInternal;
    private const OrderType         ExpectedOrderType    = OrderType.PassiveLimit;

    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.ConfirmedActiveOnMarket;

    private static readonly DateTime CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private IPQExternalCounterPartyOrder emptyCpoli = null!;

    private static IPQNameIdLookupGenerator emptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

    private IPQNameIdLookupGenerator      nameIdLookup   = null!;
    private IPQExternalCounterPartyOrder populatedCpoli = null!;


    private static DateTime testDateTime = new (2017, 10, 08, 18, 33, 24);

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        nameIdLookup      = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        emptyCpoli        = new PQExternalCounterPartyOrder(emptyNameIdLookup.Clone());
        populatedCpoli = new PQExternalCounterPartyOrder
            (new PQAnonymousOrder(nameIdLookup, OrderId, CreatedTime, OrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState, UpdatedTime, OrderRemainingVolume, ExpectedTrackingId)
             {
                 ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo(nameIdLookup, ExpectedCounterPartyId, CounterPartyName, ExpectedTraderId, TraderName)
             });
    }

    [TestMethod]
    public void NewTli_SetsValues_PropertiesInitializedAsExpected()
    {
        var newCpoli = new PQExternalCounterPartyOrder
            (new PQAnonymousOrder(nameIdLookup, OrderId, CreatedTime, OrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState, UpdatedTime, OrderRemainingVolume, ExpectedTrackingId)
            {
                ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo(nameIdLookup, ExpectedCounterPartyId, CounterPartyName, ExpectedTraderId, TraderName)
            });

        Assert.AreEqual(OrderId, newCpoli.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, newCpoli.GenesisFlags);
        Assert.AreEqual(CreatedTime, newCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, newCpoli.UpdateTime);
        Assert.AreEqual(OrderVolume, newCpoli.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, newCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, newCpoli.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, newCpoli.ExternalTraderName);
        Assert.IsNotNull(newCpoli.NameIdLookup);
        Assert.IsTrue(newCpoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(newCpoli.IsOrderIdUpdated);
        Assert.IsTrue(newCpoli.IsGenesisFlagsUpdated);
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

        var fromEmptyAoli = new PQExternalCounterPartyOrder(emptyNameIdLookup);
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, fromEmptyAoli.GenesisFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);
        Assert.IsNotNull(fromEmptyAoli.NameIdLookup);
        Assert.IsNull(fromEmptyAoli.ExternalCounterPartyName);
        Assert.IsNull(fromEmptyAoli.ExternalTraderName);
        Assert.IsFalse(fromEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromEmptyAoli.IsGenesisFlagsUpdated);
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

        var nonPqInstance = new ExternalCounterPartyOrder(populatedCpoli);
        var fromNonPQInstance
            = new PQExternalCounterPartyOrder(nonPqInstance, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(populatedCpoli.OrderId, fromNonPQInstance.OrderId);
        Assert.AreEqual(populatedCpoli.GenesisFlags, fromNonPQInstance.GenesisFlags);
        Assert.AreEqual(populatedCpoli.CreatedTime, fromNonPQInstance.CreatedTime);
        Assert.AreEqual(populatedCpoli.UpdateTime, fromNonPQInstance.UpdateTime);
        Assert.AreEqual(((IAnonymousOrder)populatedCpoli).OrderDisplayVolume, fromNonPQInstance.OrderDisplayVolume);
        Assert.AreEqual(populatedCpoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.AreEqual(populatedCpoli.ExternalCounterPartyName, fromNonPQInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromNonPQInstance.ExternalTraderName);
        Assert.IsNotNull(fromNonPQInstance.NameIdLookup);
        Assert.IsTrue(fromNonPQInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromNonPQInstance.IsGenesisFlagsUpdated);
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

        var newNonPqEmptyAoli = new ExternalCounterPartyOrder(emptyCpoli);
        var fromNonPqEmptyAoli
            = new PQExternalCounterPartyOrder(newNonPqEmptyAoli);
        Assert.AreEqual(0, fromNonPqEmptyAoli.OrderId);
        Assert.AreEqual(IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, fromNonPqEmptyAoli.GenesisFlags);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderRemainingVolume);
        Assert.IsNull(fromNonPqEmptyAoli.ExternalCounterPartyName);
        Assert.IsNull(fromNonPqEmptyAoli.ExternalTraderName);
        Assert.IsNotNull(fromNonPqEmptyAoli.NameIdLookup);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsGenesisFlagsUpdated);
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
        var newCpoli = new PQExternalCounterPartyOrder( populatedCpoli, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));

        Assert.AreEqual(OrderId, newCpoli.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, newCpoli.GenesisFlags);
        Assert.AreEqual(CreatedTime, newCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, newCpoli.UpdateTime);
        Assert.AreEqual(OrderVolume, newCpoli.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, newCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, newCpoli.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, newCpoli.ExternalTraderName);
        Assert.IsNotNull(newCpoli.NameIdLookup);
        Assert.IsTrue(newCpoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(newCpoli.IsOrderIdUpdated);
        Assert.IsTrue(newCpoli.IsGenesisFlagsUpdated);
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

        var fromEmptyAoli = new PQExternalCounterPartyOrder(emptyCpoli, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, fromEmptyAoli.GenesisFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);
        Assert.IsNotNull(fromEmptyAoli.NameIdLookup);
        Assert.IsNull(fromEmptyAoli.ExternalCounterPartyName);
        Assert.IsNull(fromEmptyAoli.ExternalTraderName);
        Assert.IsFalse(fromEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromEmptyAoli.IsGenesisFlagsUpdated);
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

        var nonPqInstance = new ExternalCounterPartyOrder(populatedCpoli);
        var fromNonPQInstance
            = new PQExternalCounterPartyOrder(nonPqInstance, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(populatedCpoli.OrderId, fromNonPQInstance.OrderId);
        Assert.AreEqual(populatedCpoli.GenesisFlags, fromNonPQInstance.GenesisFlags);
        Assert.AreEqual(populatedCpoli.CreatedTime, fromNonPQInstance.CreatedTime);
        Assert.AreEqual(populatedCpoli.UpdateTime, fromNonPQInstance.UpdateTime);
        Assert.AreEqual(((IAnonymousOrder)populatedCpoli).OrderDisplayVolume, fromNonPQInstance.OrderDisplayVolume);
        Assert.AreEqual(populatedCpoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.AreEqual(populatedCpoli.ExternalCounterPartyName, fromNonPQInstance.ExternalCounterPartyName);
        Assert.AreEqual(populatedCpoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.IsNotNull(fromNonPQInstance.NameIdLookup);
        Assert.IsTrue(fromNonPQInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromNonPQInstance.IsGenesisFlagsUpdated);
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

        var newNonPqEmptyCpoli = new ExternalCounterPartyOrder(emptyCpoli);
        var fromNonPqEmptyCpoli
            = new PQExternalCounterPartyOrder( newNonPqEmptyCpoli, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(0, fromNonPqEmptyCpoli.OrderId);
        Assert.AreEqual(IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, fromNonPqEmptyCpoli.GenesisFlags);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyCpoli.UpdateTime);
        Assert.AreEqual(0m, fromNonPqEmptyCpoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromNonPqEmptyCpoli.OrderRemainingVolume);
        Assert.IsNull(fromNonPqEmptyCpoli.ExternalCounterPartyName);
        Assert.IsNull(fromNonPqEmptyCpoli.ExternalTraderName);
        Assert.IsNotNull(fromNonPqEmptyCpoli.NameIdLookup);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsOrderIdUpdated);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsGenesisFlagsUpdated);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsUpdateTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsUpdateTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsExternalCounterPartyNameUpdated);
        Assert.IsFalse(fromNonPqEmptyCpoli.IsExternalTraderNameUpdated);
        Assert.IsTrue(fromNonPqEmptyCpoli.IsEmpty);
        Assert.IsFalse(fromNonPqEmptyCpoli.HasUpdates);
    }

    [TestMethod]
    public void NewTli_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedAoli = new PQExternalCounterPartyOrder
            (new PQAnonymousOrder(emptyNameIdLookup, OrderId, CreatedTime, OrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState, UpdatedTime, OrderRemainingVolume)
             {
                 ExternalCounterPartyOrderInfo = new PQAdditionalExternalCounterPartyInfo(emptyNameIdLookup, ExpectedCounterPartyId, CounterPartyName, ExpectedTraderId, TraderName)
             })
        {
            IsOrderIdUpdated         = false, IsGenesisFlagsUpdated = false, IsCreatedTimeDateUpdated = false, IsCreatedTimeSub2MinUpdated = false
          , IsUpdateTimeDateUpdated = false, IsUpdateTimeSub2MinUpdated = false, IsOrderVolumeUpdated = false, IsOrderRemainingVolumeUpdated = false
          , IsExternalTraderNameUpdated      = false
        };
        var fromPQInstance
            = new PQExternalCounterPartyOrder( newPopulatedAoli, new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, fromPQInstance.GenesisFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPQInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromPQInstance.ExternalTraderName);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsGenesisFlagsUpdated);
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
        fromPQInstance = new PQExternalCounterPartyOrder(  newPopulatedAoli , new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates));
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, fromPQInstance.GenesisFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPQInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromPQInstance.ExternalTraderName);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsGenesisFlagsUpdated);
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
        IPQAdditionalExternalCounterPartyOrderInfo? cpOrderInfo,
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
        if (bsNotNull)
        {
            var deltaUpdateFields = orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(0, deltaUpdateFields.Count());
        }
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
            var foundCpOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo)foundLayer[orderIndex]!;
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
            var foundAnonOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo)foundLayer[orderIndex]!;
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
            var foundAnonOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo)foundLayer[orderIndex]!;
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
            var foundAnonOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo)newLayer[orderIndex]!;
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

        var newAnonOrderInfo = new PQExternalCounterPartyOrder();
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
        IPQAdditionalExternalCounterPartyOrderInfo? cpOrderInfo,
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
        Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyOrder)cpOrderInfo).ExternalTraderName);
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
            var foundCpOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo)foundLayer[orderIndex]!;
            Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyOrder)foundCpOrderInfo).ExternalTraderName);
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
            var foundAnonOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo)foundLayer[orderIndex]!;
            Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyOrder)foundAnonOrderInfo).ExternalTraderName);
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
            var foundAnonOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo)foundLayer[orderIndex]!;
            Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyOrder)foundAnonOrderInfo).ExternalTraderName);
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
            var foundAnonOrderInfo = (IPQAdditionalExternalCounterPartyOrderInfo)newLayer[orderIndex]!;
            Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyOrder)foundAnonOrderInfo).ExternalTraderName);
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

        var newAnonOrderInfo = new PQExternalCounterPartyOrder();
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
        var newEmptyPqCpoli = new PQExternalCounterPartyOrder();
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
        var newEmptyPqCpoli = new PQExternalCounterPartyOrder();
        Assert.IsTrue(newEmptyPqCpoli.IsEmpty);
        Assert.IsFalse(newEmptyPqCpoli.HasUpdates);

        Assert.AreEqual(emptyCpoli, newEmptyPqCpoli);

        var expectedTraderName = "SetTraderNameToThis";
        emptyCpoli.ExternalTraderName = expectedTraderName;

        Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyOrder)emptyCpoli).ExternalTraderName);
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
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, populatedCpoli.GenesisFlags);
        Assert.AreEqual(CreatedTime, populatedCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, populatedCpoli.UpdateTime);
        Assert.AreEqual(OrderVolume, ((IAnonymousOrder)populatedCpoli).OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, populatedCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, populatedCpoli.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, ((IExternalCounterPartyOrder)populatedCpoli).ExternalTraderName);
        Assert.IsTrue(populatedCpoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedCpoli.IsGenesisFlagsUpdated);
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
        Assert.AreEqual(IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, populatedCpoli.GenesisFlags);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.UpdateTime);
        Assert.AreEqual(0m, ((IAnonymousOrder)populatedCpoli).OrderDisplayVolume);
        Assert.AreEqual(0m, populatedCpoli.OrderRemainingVolume);
        Assert.IsNull(populatedCpoli.ExternalCounterPartyName);
        Assert.IsNull(((IExternalCounterPartyOrder)populatedCpoli).ExternalTraderName);
        Assert.IsTrue(populatedCpoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedCpoli.IsGenesisFlagsUpdated);
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
        Assert.IsFalse(populatedCpoli.IsGenesisFlagsUpdated);
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
        var nonPQTraderLayerInfo = new ExternalCounterPartyOrder(populatedCpoli);
        emptyCpoli.CopyFrom(nonPQTraderLayerInfo);
        Assert.AreEqual(populatedCpoli, emptyCpoli);
    }

    [TestMethod]
    public void FullyPopulatedTli_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((PQExternalCounterPartyOrder)populatedCpoli).Clone();
        Assert.AreNotSame(clonedQuote, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clonedQuote);

        var cloned2 = (PQExternalCounterPartyOrder)((ICloneable)populatedCpoli).Clone();
        Assert.AreNotSame(cloned2, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned2);

        var cloned3 = (PQExternalCounterPartyOrder)((IMutableAnonymousOrder)populatedCpoli).Clone();
        Assert.AreNotSame(cloned3, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned3);

        var cloned4 = (PQExternalCounterPartyOrder)((IExternalCounterPartyOrder)populatedCpoli).Clone();
        Assert.AreNotSame(cloned4, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned4);

        var cloned5 = (PQExternalCounterPartyOrder)((IMutableExternalCounterPartyOrder)populatedCpoli).Clone();
        Assert.AreNotSame(cloned5, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned5);

        var cloned6 = (PQExternalCounterPartyOrder)((PQExternalCounterPartyOrder)populatedCpoli).Clone();
        Assert.AreNotSame(cloned6, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned6);

        var cloned7 = populatedCpoli.Clone();
        Assert.AreNotSame(cloned7, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned7);
    }

    [TestMethod]
    public void FullyPopulatedPqCpoliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQExternalCounterPartyOrder)((ICloneable)populatedCpoli).Clone();
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
        Assert.AreEqual(populatedCpoli, ((IPQExternalCounterPartyOrder)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((ICloneable<IPQAnonymousOrder>)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IMutableAnonymousOrder)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IExternalCounterPartyOrder)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IMutableExternalCounterPartyOrder)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((PQExternalCounterPartyOrder)populatedCpoli).Clone());
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.GenesisFlags)}: {populatedCpoli.GenesisFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.CreatedTime)}: {populatedCpoli.CreatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.UpdateTime)}: {populatedCpoli.UpdateTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(IAnonymousOrder.OrderDisplayVolume)}: {((IAnonymousOrder)populatedCpoli).OrderDisplayVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderRemainingVolume)}: {populatedCpoli.OrderRemainingVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(IExternalCounterPartyOrder.ExternalTraderName)}: {((IExternalCounterPartyOrder)populatedCpoli).ExternalTraderName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.ExternalCounterPartyName)}: {populatedCpoli.ExternalCounterPartyName}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQExternalCounterPartyOrder? original,
        IPQExternalCounterPartyOrder? changingTraderLayerInfo,
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

        if (original.GetType() == typeof(PQExternalCounterPartyOrder))
            Assert.AreEqual
                (!exactComparison,
                 original.AreEquivalent(new ExternalCounterPartyOrder(changingTraderLayerInfo), exactComparison));

        PQAnonymousOrderTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingTraderLayerInfo, originalTraderPriceVolumeLayer
           , changingTraderPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingTraderLayerInfo.IsExternalCounterPartyNameUpdated = !changingTraderLayerInfo.IsExternalCounterPartyNameUpdated;
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
        changingTraderLayerInfo.IsExternalCounterPartyNameUpdated = original.IsExternalCounterPartyNameUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsExternalTraderNameUpdated = !changingTraderLayerInfo.IsExternalTraderNameUpdated;
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
        changingTraderLayerInfo.IsExternalTraderNameUpdated = original.IsExternalTraderNameUpdated;
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
