// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

[TestClass]
public class PQOrdersPriceVolumeLayerTests
{
    private const decimal Price          = 1.3456m;
    private const decimal Volume         = 100_000m;
    private const uint    OrdersCount    = 3;
    private const int     NumOfOrders    = 3;
    private const decimal InternalVolume = 50_000m;

    private const int     OrderId              = 250;
    private const decimal OrderVolume          = 50.50m;
    private const decimal OrderRemainingVolume = 10.25m;
    private const string  CounterPartyBase     = "TestCounterPartyName";
    private const string  TraderNameBase       = "TestTraderName";


    private const int ExpectedTraderId       = 2;
    private const int ExpectedCounterPartyId = 1;

    private const OrderFlags      ExpectedTypeFlags  = OrderFlags.FromAdapter;
    private const OrderType       ExpectedOrderType  = OrderType.PassiveLimit;
    private const LayerOrderFlags ExpectedLayerFlags = LayerOrderFlags.ExplicitlyDefinedFromSource;

    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.SourceActiveOnMarket;


    private static readonly DateTime CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private IPQOrdersPriceVolumeLayer emptyAnonymousOrdersPvl    = null!;
    private IPQOrdersPriceVolumeLayer emptyCounterPartyOrdersPvl = null!;

    private IPQNameIdLookupGenerator  emptyNameIdLookup              = null!;
    private PQAnonymousOrderLayerInfo exampleAnonymousOrderLayer     = null!;
    private IPQNameIdLookupGenerator  nameIdLookup                   = null!;
    private IPQOrdersPriceVolumeLayer populatedAnonymousOrdersPvl    = null!;
    private IPQOrdersPriceVolumeLayer populatedCounterPartyOrdersPvl = null!;
    private DateTime                  testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        nameIdLookup      = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

        exampleAnonymousOrderLayer = new PQAnonymousOrderLayerInfo
            (OrderId, CreatedTime, OrderVolume, ExpectedLayerFlags, ExpectedOrderType, ExpectedTypeFlags, ExpectedLifecycleState, UpdatedTime
           , OrderRemainingVolume);

        emptyAnonymousOrdersPvl    = new PQOrdersPriceVolumeLayer(emptyNameIdLookup.Clone(), LayerType.OrdersAnonymousPriceVolume);
        emptyCounterPartyOrdersPvl = new PQOrdersPriceVolumeLayer(emptyNameIdLookup.Clone(), LayerType.OrdersFullPriceVolume);
        testDateTime               = new DateTime(2017, 12, 17, 18, 54, 52);
        populatedAnonymousOrdersPvl
            = new PQOrdersPriceVolumeLayer(nameIdLookup, LayerType.OrdersAnonymousPriceVolume, Price, Volume, OrdersCount, InternalVolume);
        AddAnonymousOrders(populatedAnonymousOrdersPvl, NumOfOrders);
        populatedCounterPartyOrdersPvl
            = new PQOrdersPriceVolumeLayer(nameIdLookup, LayerType.OrdersFullPriceVolume, Price, Volume, OrdersCount, InternalVolume);
        AddCounterPartyOrders(populatedCounterPartyOrdersPvl, NumOfOrders);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        bool[] expectPopulated = [true, true, true];
        var    newPvl = new PQOrdersPriceVolumeLayer(nameIdLookup, LayerType.OrdersAnonymousPriceVolume, Price, Volume, OrdersCount, InternalVolume);
        AddAnonymousOrders(newPvl, NumOfOrders);
        Assert.AreEqual(Price, newPvl.Price);
        Assert.AreEqual(Volume, newPvl.Volume);
        Assert.AreEqual(OrdersCount, newPvl.OrdersCount);
        Assert.AreEqual(InternalVolume, newPvl.InternalVolume);
        Assert.IsNotNull(newPvl.NameIdLookup);
        Assert.IsTrue(newPvl.IsPriceUpdated);
        Assert.IsTrue(newPvl.IsVolumeUpdated);
        Assert.IsTrue(newPvl.IsOrdersCountUpdated);
        Assert.IsTrue(newPvl.IsInternalVolumeUpdated);
        Assert.IsFalse(newPvl.IsEmpty);
        Assert.IsTrue(newPvl.HasUpdates);
        AssertOrdersAreAsExpected(newPvl, expectPopulated);

        newPvl = new PQOrdersPriceVolumeLayer(nameIdLookup, LayerType.OrdersFullPriceVolume, Price, Volume, OrdersCount, InternalVolume);
        AddCounterPartyOrders(newPvl, NumOfOrders);
        Assert.AreEqual(Price, newPvl.Price);
        Assert.AreEqual(Volume, newPvl.Volume);
        Assert.AreEqual(OrdersCount, newPvl.OrdersCount);
        Assert.AreEqual(InternalVolume, newPvl.InternalVolume);
        Assert.IsNotNull(newPvl.NameIdLookup);
        Assert.IsTrue(newPvl.IsPriceUpdated);
        Assert.IsTrue(newPvl.IsVolumeUpdated);
        Assert.IsTrue(newPvl.IsOrdersCountUpdated);
        Assert.IsTrue(newPvl.IsInternalVolumeUpdated);
        Assert.IsTrue(((IPQCounterPartyOrderLayerInfo)newPvl[0]!).IsExternalCounterPartyNameUpdated);
        Assert.IsTrue(((IPQCounterPartyOrderLayerInfo)newPvl[0]!).IsExternalTraderNameUpdated);
        Assert.IsFalse(newPvl.IsEmpty);
        Assert.IsTrue(newPvl.HasUpdates);
        AssertOrdersAreAsExpected(newPvl, expectPopulated);

        var newEmptyPvl = new PQOrdersPriceVolumeLayer(emptyNameIdLookup, LayerType.OrdersAnonymousPriceVolume);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(0u, newEmptyPvl.OrdersCount);
        Assert.AreEqual(0m, newEmptyPvl.InternalVolume);
        Assert.IsNotNull(newEmptyPvl.NameIdLookup);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsOrdersCountUpdated);
        Assert.IsFalse(newEmptyPvl.IsInternalVolumeUpdated);
        Assert.IsTrue(newEmptyPvl.IsEmpty);
        Assert.IsFalse(newEmptyPvl.HasUpdates);
        AssertOrdersAreAsExpected(newEmptyPvl, expectPopulated);

        newEmptyPvl = new PQOrdersPriceVolumeLayer(emptyNameIdLookup, LayerType.OrdersFullPriceVolume);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(0u, newEmptyPvl.OrdersCount);
        Assert.AreEqual(0m, newEmptyPvl.InternalVolume);
        Assert.IsNotNull(newEmptyPvl.NameIdLookup);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsOrdersCountUpdated);
        Assert.IsFalse(newEmptyPvl.IsInternalVolumeUpdated);
        Assert.IsTrue(newEmptyPvl.IsEmpty);
        Assert.IsFalse(newEmptyPvl.HasUpdates);
        AssertOrdersAreAsExpected(newEmptyPvl, expectPopulated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var expectPopulated = new[] { true, true, true };
        var newPopulatedPvl
            = new PQOrdersPriceVolumeLayer(populatedAnonymousOrdersPvl, LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
        AddAnonymousOrders(newPopulatedPvl, NumOfOrders);
        var fromPQInstance
            = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup.Clone());
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsTrue(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulated);

        var nonPqvTraderPvl = new OrdersPriceVolumeLayer(populatedAnonymousOrdersPvl, populatedAnonymousOrdersPvl.LayerType);
        AddAnonymousOrders(nonPqvTraderPvl, NumOfOrders);
        var fromNonPqInstance = new PQOrdersPriceVolumeLayer(nonPqvTraderPvl, LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup.Clone());
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsTrue(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);
        AssertOrdersAreAsExpected(fromNonPqInstance, expectPopulated);

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(populatedCounterPartyOrdersPvl, LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
        AddCounterPartyOrders(newPopulatedPvl, NumOfOrders);
        fromPQInstance = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersFullPriceVolume, newPopulatedPvl.NameIdLookup.Clone());
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsTrue(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulated);

        nonPqvTraderPvl = new OrdersPriceVolumeLayer(populatedCounterPartyOrdersPvl, populatedCounterPartyOrdersPvl.LayerType);
        AddCounterPartyOrders(nonPqvTraderPvl, NumOfOrders);
        fromNonPqInstance = new PQOrdersPriceVolumeLayer(nonPqvTraderPvl, LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsTrue(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);
        AssertOrdersAreAsExpected(fromNonPqInstance, expectPopulated);

        var newEmptyPvl = new PQOrdersPriceVolumeLayer(emptyAnonymousOrdersPvl, LayerType.OrdersAnonymousPriceVolume, emptyNameIdLookup);
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
        AssertOrdersAreAsExpected(newEmptyPvl);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var expectPopulatedOrders = new[] { true, true, true };
        var newPopulatedPvl = new PQOrdersPriceVolumeLayer(nameIdLookup.Clone(), LayerType.OrdersFullPriceVolume, Price, Volume, OrdersCount
                                                         , InternalVolume)
        {
            IsVolumeUpdated = false, IsOrdersCountUpdated = false, IsInternalVolumeUpdated = false
        };
        AddCounterPartyOrders(newPopulatedPvl, NumOfOrders);
        var originalCpLayerOrder = newPopulatedPvl;
        var fromPQInstance       = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsFalse(fromPQInstance.IsInternalVolumeUpdated);
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders);

        newPopulatedPvl.IsPriceUpdated  = false;
        newPopulatedPvl.IsVolumeUpdated = true;
        fromPQInstance                  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsFalse(fromPQInstance.IsInternalVolumeUpdated);

        newPopulatedPvl.IsVolumeUpdated = false;
        newPopulatedPvl.IsOrdersCountUpdated = true;
        fromPQInstance = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsFalse(fromPQInstance.IsInternalVolumeUpdated);

        newPopulatedPvl.IsOrdersCountUpdated = false;
        newPopulatedPvl.IsInternalVolumeUpdated = true;
        fromPQInstance = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsTrue(fromPQInstance.IsInternalVolumeUpdated);


        newPopulatedPvl.IsInternalVolumeUpdated = false;

        foreach (var orderLayer in newPopulatedPvl.Orders)
        {
            orderLayer.IsOrderIdUpdated              = false;
            orderLayer.IsOrderLayerFlagsUpdated      = false;
            orderLayer.IsCreatedTimeDateUpdated      = false;
            orderLayer.IsCreatedTimeSub2MinUpdated   = false;
            orderLayer.IsUpdateTimeDateUpdated       = false;
            orderLayer.IsUpdateTimeSub2MinUpdated    = false;
            orderLayer.IsOrderVolumeUpdated          = false;
            orderLayer.IsOrderRemainingVolumeUpdated = false;
            if (orderLayer is IPQCounterPartyOrderLayerInfo counterPartyOrderLayer)
            {
                counterPartyOrderLayer.IsExternalCounterPartyNameUpdated = false;
                counterPartyOrderLayer.IsExternalTraderNameUpdated       = false;
            }
        }
        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderIdUpdated = true;
        fromPQInstance = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        var expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderIdUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderLayerFlagsUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsOrderFlagsUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderLayerFlagsUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsCreatedTimeDateUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsCreatedDateUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsCreatedTimeDateUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsCreatedTimeSub2MinUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsCreatedTimeSub2MinUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsCreatedTimeSub2MinUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsUpdateTimeDateUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsUpdatedTimeDateUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsUpdateTimeDateUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsUpdateTimeSub2MinUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsUpdatedTimeSub2MinUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsUpdateTimeSub2MinUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderVolumeUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsOrderVolumeUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderVolumeUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderRemainingVolumeUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsOrderRemainingVolumeUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderRemainingVolumeUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersFullPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) ((IPQCounterPartyOrderLayerInfo?)orderLayer)!.IsExternalCounterPartyNameUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersFullPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsOrderCounterPartyNameIdUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) ((IPQCounterPartyOrderLayerInfo?)orderLayer)!.IsExternalCounterPartyNameUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersFullPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) ((IPQCounterPartyOrderLayerInfo?)orderLayer)!.IsExternalTraderNameUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersFullPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsOrderTraderNameIdUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
    }

    [TestMethod]
    public void EmptyPvl_IndexerGetSets_AddNewLayersIfIndexedViaVariousInterfaces()
    {
        var newEmpty = emptyAnonymousOrdersPvl.Clone();
        Assert.AreEqual(0u, newEmpty.OrdersCount);
        ((IMutableOrdersPriceVolumeLayer)newEmpty)[3] =
            new PQAnonymousOrderLayerInfo
            {
                OrderId = OrderId + 3, OrderDisplayVolume = OrderVolume
            };
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        Assert.AreEqual(OrderId + 3, ((IOrdersPriceVolumeLayer)newEmpty)[3]!.OrderId);
        Assert.AreEqual(OrderVolume, ((IOrdersPriceVolumeLayer)newEmpty)[3]!.OrderDisplayVolume);

        Assert.IsNotNull(((IOrdersPriceVolumeLayer)newEmpty)[255]);
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        ((IMutableOrdersPriceVolumeLayer)newEmpty)[255]!.OrderId = OrderId + 255;
        Assert.AreEqual(256u, newEmpty.OrdersCount);

        newEmpty[255] = new PQAnonymousOrderLayerInfo
        {
            OrderId = OrderId + 255, OrderDisplayVolume = OrderVolume
        };
        Assert.AreEqual(256u, newEmpty.OrdersCount);
        Assert.AreEqual(OrderId + 255, ((IOrdersPriceVolumeLayer)newEmpty)[255]!.OrderId);
        Assert.AreEqual(OrderVolume, ((IOrdersPriceVolumeLayer)newEmpty)[255]!.OrderDisplayVolume);
    }

    [TestMethod]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    public void EmptyPvl_IndexerGetSetOutOfIndex_ThrowsArgumentOutOfRangeException()
    {
        var caughtException = false;
        try
        {
            var neverSet = ((IOrdersPriceVolumeLayer)emptyAnonymousOrdersPvl)[ushort.MaxValue + 1];
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);

        caughtException = false;
        try
        {
            var neverSet = ((IMutableOrdersPriceVolumeLayer)emptyAnonymousOrdersPvl)[ushort.MaxValue + 1];
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);

        caughtException = false;
        try
        {
            ((IMutableOrdersPriceVolumeLayer)emptyAnonymousOrdersPvl)[ushort.MaxValue + 1] = null;
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);

        caughtException = false;
        try
        {
            var neverSet = emptyCounterPartyOrdersPvl[ushort.MaxValue + 1];
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);

        caughtException = false;
        try
        {
            emptyCounterPartyOrdersPvl[ushort.MaxValue + 1] = null;
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
    }

    [TestMethod]
    public void PopulatedPvl_HasUpdatesSetFalse_LookupAndTraderLayersHaveNoUpdates()
    {
        Assert.IsTrue(populatedCounterPartyOrdersPvl.HasUpdates);
        for (var i = 0; i < NumOfOrders; i++) Assert.IsTrue(populatedCounterPartyOrdersPvl[i]!.HasUpdates);
        Assert.IsTrue(populatedCounterPartyOrdersPvl.NameIdLookup.HasUpdates);

        populatedCounterPartyOrdersPvl.HasUpdates = false;

        Assert.IsFalse(populatedCounterPartyOrdersPvl.HasUpdates);
        for (var i = 0; i < NumOfOrders; i++) Assert.IsFalse(populatedCounterPartyOrdersPvl[i]!.HasUpdates);
        Assert.IsFalse(populatedCounterPartyOrdersPvl.NameIdLookup.HasUpdates);
    }


    [TestMethod]
    public void EmptyPvl_LayerOrderIdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var orderLayerInfo = emptyCounterPartyOrdersPvl[i]!;

            Assert.IsFalse(orderLayerInfo.IsOrderIdUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(0, orderLayerInfo.OrderId);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedOrderId = OrderId + 1000;
            orderLayerInfo.OrderId = expectedOrderId;
            Assert.IsTrue(orderLayerInfo.IsOrderIdUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedOrderId, orderLayerInfo.OrderId);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex         = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderId, orderIndex, expectedOrderId);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsOrderIdUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            expectedLayerField = new PQFieldUpdate(expectedLayerField.Id, expectedLayerField.TradingSubId, orderIndex, expectedLayerField.Payload);

            orderLayerInfo.IsOrderIdUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderId && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.OrderId          = 0;
            orderLayerInfo.IsOrderIdUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedOrderId, foundTraderInfo!.OrderId);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsOrderIdUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerOrderFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var orderLayerInfo = emptyCounterPartyOrdersPvl[i]!;

            Assert.IsFalse(orderLayerInfo.IsOrderLayerFlagsUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(LayerOrderFlags.None, orderLayerInfo.OrderLayerFlags);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedOrderFlags = LayerOrderFlags.ExplicitlyDefinedFromSource | LayerOrderFlags.CancelRequested |
                                     LayerOrderFlags.NotExternalVolume;
            orderLayerInfo.OrderLayerFlags = expectedOrderFlags;
            Assert.IsTrue(orderLayerInfo.IsOrderLayerFlagsUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedOrderFlags, orderLayerInfo.OrderLayerFlags);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex = (ushort)i;
            var expectedLayerField
                = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderLayerFlags, orderIndex, (uint)expectedOrderFlags);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsOrderLayerFlagsUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());


            orderLayerInfo.IsOrderLayerFlagsUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderLayerFlags && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.OrderLayerFlags          = LayerOrderFlags.None;
            orderLayerInfo.IsOrderLayerFlagsUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedOrderFlags, foundTraderInfo!.OrderLayerFlags);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsOrderLayerFlagsUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerCreatedTimeDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var orderLayerInfo = emptyCounterPartyOrdersPvl[i]!;
            // increment NumUpdatesSinceEmpty
            orderLayerInfo.CreatedTime = testDateTime;
            orderLayerInfo.UpdateComplete();
            orderLayerInfo.CreatedTime = DateTime.MinValue;
            orderLayerInfo.UpdateComplete();

            Assert.IsFalse(orderLayerInfo.IsCreatedTimeDateUpdated);
            Assert.IsFalse(orderLayerInfo.IsCreatedTimeSub2MinUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(DateTime.MinValue, orderLayerInfo.CreatedTime);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedCreatedTime = DateTime.UnixEpoch.AddHours(1);
            orderLayerInfo.CreatedTime = expectedCreatedTime;
            Assert.IsTrue(orderLayerInfo.IsCreatedTimeDateUpdated);
            Assert.IsFalse(orderLayerInfo.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedCreatedTime, orderLayerInfo.CreatedTime);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var value              = expectedCreatedTime.Get2MinIntervalsFromUnixEpoch();
            var orderIndex         = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderCreatedDate, orderIndex, value);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsCreatedTimeDateUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            orderLayerInfo.IsCreatedTimeDateUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderCreatedDate && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.CreatedTime                 = DateTime.MinValue;
            orderLayerInfo.IsCreatedTimeDateUpdated    = false;
            orderLayerInfo.IsCreatedTimeSub2MinUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedCreatedTime, foundTraderInfo!.CreatedTime);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsCreatedTimeDateUpdated);
            Assert.IsFalse(foundTraderInfo.IsCreatedTimeSub2MinUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerCreatedTimeSub2MinChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var orderLayerInfo = emptyCounterPartyOrdersPvl[i]!;
            // increment NumUpdatesSinceEmpty
            orderLayerInfo.CreatedTime = testDateTime;
            orderLayerInfo.UpdateComplete();
            orderLayerInfo.CreatedTime = DateTime.MinValue;
            orderLayerInfo.UpdateComplete();

            Assert.IsFalse(orderLayerInfo.IsCreatedTimeDateUpdated);
            Assert.IsFalse(orderLayerInfo.IsCreatedTimeSub2MinUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(DateTime.MinValue, orderLayerInfo.CreatedTime);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedCreatedTime = DateTime.MinValue.AddMinutes(1);
            orderLayerInfo.CreatedTime = expectedCreatedTime;
            Assert.IsFalse(orderLayerInfo.IsCreatedTimeDateUpdated);
            Assert.IsTrue(orderLayerInfo.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedCreatedTime, orderLayerInfo.CreatedTime);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var extended   = expectedCreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            var orderIndex = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderCreatedSub2MinTime, orderIndex, value
                                                     , extended);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsCreatedTimeSub2MinUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            orderLayerInfo.IsCreatedTimeSub2MinUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderCreatedSub2MinTime && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.CreatedTime                 = DateTime.MinValue;
            orderLayerInfo.IsCreatedTimeDateUpdated    = false;
            orderLayerInfo.IsCreatedTimeSub2MinUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedCreatedTime, foundTraderInfo!.CreatedTime);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsCreatedTimeSub2MinUpdated);
            Assert.IsFalse(foundTraderInfo.IsCreatedTimeDateUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerUpdatedTimeDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var orderLayerInfo = emptyCounterPartyOrdersPvl[i]!;
            // increment NumUpdatesSinceEmpty
            orderLayerInfo.UpdateTime = testDateTime;
            orderLayerInfo.UpdateComplete();
            orderLayerInfo.UpdateTime = DateTime.MinValue;
            orderLayerInfo.UpdateComplete();

            Assert.IsFalse(orderLayerInfo.IsUpdateTimeDateUpdated);
            Assert.IsFalse(orderLayerInfo.IsUpdateTimeSub2MinUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(DateTime.MinValue, orderLayerInfo.UpdateTime);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedUpdatedTime = DateTime.UnixEpoch.AddHours(1);
            orderLayerInfo.UpdateTime = expectedUpdatedTime;
            Assert.IsTrue(orderLayerInfo.IsUpdateTimeDateUpdated);
            Assert.IsFalse(orderLayerInfo.IsUpdateTimeSub2MinUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedUpdatedTime, orderLayerInfo.UpdateTime);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var value              = expectedUpdatedTime.Get2MinIntervalsFromUnixEpoch();
            var orderIndex         = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderUpdatedDate, orderIndex, value);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsUpdateTimeDateUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            orderLayerInfo.IsUpdateTimeDateUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderUpdatedDate && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.UpdateTime                 = DateTime.MinValue;
            orderLayerInfo.IsUpdateTimeDateUpdated    = false;
            orderLayerInfo.IsUpdateTimeSub2MinUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedUpdatedTime, foundTraderInfo!.UpdateTime);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsUpdateTimeDateUpdated);
            Assert.IsFalse(foundTraderInfo.IsUpdateTimeSub2MinUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerUpdatedTimeSub2MinChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var orderLayerInfo = emptyCounterPartyOrdersPvl[i]!;
            // increment NumUpdatesSinceEmpty
            orderLayerInfo.UpdateTime = testDateTime;
            orderLayerInfo.UpdateComplete();
            orderLayerInfo.UpdateTime = DateTime.MinValue;
            orderLayerInfo.UpdateComplete();

            Assert.IsFalse(orderLayerInfo.IsUpdateTimeDateUpdated);
            Assert.IsFalse(orderLayerInfo.IsUpdateTimeSub2MinUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(DateTime.MinValue, orderLayerInfo.UpdateTime);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedUpdatedTime = DateTime.MinValue.AddMinutes(1);
            orderLayerInfo.UpdateTime = expectedUpdatedTime;
            Assert.IsFalse(orderLayerInfo.IsUpdateTimeDateUpdated);
            Assert.IsTrue(orderLayerInfo.IsUpdateTimeSub2MinUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedUpdatedTime, orderLayerInfo.UpdateTime);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var extended   = expectedUpdatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            var orderIndex = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime, orderIndex, value
                                                     , extended);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsUpdateTimeSub2MinUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            orderLayerInfo.IsUpdateTimeSub2MinUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.UpdateTime                 = DateTime.MinValue;
            orderLayerInfo.IsUpdateTimeDateUpdated    = false;
            orderLayerInfo.IsUpdateTimeSub2MinUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedUpdatedTime, foundTraderInfo!.UpdateTime);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsUpdateTimeSub2MinUpdated);
            Assert.IsFalse(foundTraderInfo.IsUpdateTimeDateUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerOrderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var traderLayerInfo = emptyCounterPartyOrdersPvl[i]!;

            Assert.IsFalse(traderLayerInfo.IsOrderVolumeUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(0m, ((IPublishedOrder)traderLayerInfo).OrderDisplayVolume);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedOrderVolume = OrderVolume + 1000;
            traderLayerInfo.OrderDisplayVolume = expectedOrderVolume;
            Assert.IsTrue(traderLayerInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedOrderVolume, ((IPublishedOrder)traderLayerInfo).OrderDisplayVolume);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderDisplayVolume, orderIndex
                                                     , expectedOrderVolume
                                                     , (PQFieldFlags)6);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            traderLayerInfo.IsOrderVolumeUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            traderLayerInfo.IsOrderVolumeUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderDisplayVolume && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            traderLayerInfo.OrderDisplayVolume   = 0;
            traderLayerInfo.IsOrderVolumeUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedOrderVolume, ((IPublishedOrder)foundTraderInfo!).OrderDisplayVolume);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsOrderVolumeUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerOrderRemainingVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var traderLayerInfo = emptyCounterPartyOrdersPvl[i]!;

            Assert.IsFalse(traderLayerInfo.IsOrderRemainingVolumeUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(0m, traderLayerInfo.OrderRemainingVolume);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedOrderRemainingVolume = OrderRemainingVolume + 1000;
            traderLayerInfo.OrderRemainingVolume = expectedOrderRemainingVolume;
            Assert.IsTrue(traderLayerInfo.IsOrderRemainingVolumeUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedOrderRemainingVolume, traderLayerInfo.OrderRemainingVolume);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderRemainingVolume, orderIndex
                                                     , expectedOrderRemainingVolume, (PQFieldFlags)6);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            traderLayerInfo.IsOrderRemainingVolumeUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            traderLayerInfo.IsOrderRemainingVolumeUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderRemainingVolume && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            traderLayerInfo.OrderRemainingVolume          = 0m;
            traderLayerInfo.IsOrderRemainingVolumeUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedOrderRemainingVolume, foundTraderInfo!.OrderRemainingVolume);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsOrderRemainingVolumeUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerCounterPartyNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var cpOrderLayerInfo  = (IPQCounterPartyOrderLayerInfo?)emptyCounterPartyOrdersPvl[i]!;
            var layerNameIdLookup = emptyCounterPartyOrdersPvl.NameIdLookup;

            emptyCounterPartyOrdersPvl.HasUpdates = false;

            Assert.IsFalse(cpOrderLayerInfo.IsExternalCounterPartyNameUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(null, cpOrderLayerInfo.ExternalCounterPartyName);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedCounterPartyName = CounterPartyBase + i;
            cpOrderLayerInfo.ExternalCounterPartyName = expectedCounterPartyName;
            Assert.IsTrue(cpOrderLayerInfo.IsExternalCounterPartyNameUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedCounterPartyName, cpOrderLayerInfo.ExternalCounterPartyName);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId, orderIndex
                                                     , (uint)layerNameIdLookup[cpOrderLayerInfo.ExternalCounterPartyName]);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            var stringUpdates = emptyCounterPartyOrdersPvl.GetStringUpdates(testDateTime, StorageFlags.Update)
                                                          .ToList();
            Assert.AreEqual(1, stringUpdates.Count);
            var expectedStringUpdates = new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQFeedFields.QuoteLayerStringUpdates, CrudCommand.Upsert.ToPQSubFieldId(), orderIndex, 0u)
              , StringUpdate = new PQStringUpdate
                {
                    Command = CrudCommand.Upsert, DictionaryId = emptyCounterPartyOrdersPvl.NameIdLookup[cpOrderLayerInfo.ExternalCounterPartyName]
                  , Value   = expectedCounterPartyName
                }
            };
            Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

            cpOrderLayerInfo.IsExternalCounterPartyNameUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            cpOrderLayerInfo.NameIdLookup.HasUpdates = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());
            cpOrderLayerInfo.IsExternalCounterPartyNameUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            cpOrderLayerInfo.NameIdLookup.Clear();
            cpOrderLayerInfo.ExternalCounterPartyName          = null;
            cpOrderLayerInfo.IsExternalCounterPartyNameUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            newEmpty.UpdateFieldString(stringUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedCounterPartyName, foundTraderInfo!.ExternalCounterPartyName);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsExternalCounterPartyNameUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var cpOrderLayerInfo  = (IPQCounterPartyOrderLayerInfo?)emptyCounterPartyOrdersPvl[i]!;
            var layerNameIdLookup = emptyCounterPartyOrdersPvl.NameIdLookup;

            Assert.IsFalse(cpOrderLayerInfo.IsExternalTraderNameUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(null, ((IExternalCounterPartyInfoOrder)cpOrderLayerInfo).ExternalTraderName);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedTraderName = TraderNameBase + i;
            cpOrderLayerInfo.ExternalTraderName = expectedTraderName;
            Assert.IsTrue(cpOrderLayerInfo.IsExternalTraderNameUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedTraderName, cpOrderLayerInfo.ExternalTraderName);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalTraderNameId, orderIndex
                                                     , (uint)layerNameIdLookup
                                                           [((IExternalCounterPartyInfoOrder)cpOrderLayerInfo).ExternalTraderName]);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            var stringUpdates = emptyCounterPartyOrdersPvl.GetStringUpdates(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, stringUpdates.Count);
            var expectedStringUpdates = new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQFeedFields.QuoteLayerStringUpdates, CrudCommand.Upsert.ToPQSubFieldId(), orderIndex, 0u)
              , StringUpdate = new PQStringUpdate
                {
                    Command      = CrudCommand.Upsert
                  , DictionaryId = emptyCounterPartyOrdersPvl.NameIdLookup[((IExternalCounterPartyInfoOrder)cpOrderLayerInfo).ExternalTraderName]
                  , Value        = expectedTraderName
                }
            };
            Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

            cpOrderLayerInfo.IsExternalTraderNameUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            cpOrderLayerInfo.NameIdLookup.HasUpdates = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());
            cpOrderLayerInfo.IsExternalTraderNameUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.OrdersSubId == PQOrdersSubFieldKeys.OrderExternalTraderNameId && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            cpOrderLayerInfo.NameIdLookup.Clear();
            cpOrderLayerInfo.ExternalTraderName          = null;
            cpOrderLayerInfo.IsExternalTraderNameUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            newEmpty.UpdateFieldString(stringUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedTraderName, foundTraderInfo!.ExternalTraderName);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsExternalTraderNameUpdated);
        }
    }


    [TestMethod]
    public void PopulatedPvl_RemoveAt_ClearsOrReducesCount()
    {
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.RemoveAt(1);
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);
        Assert.IsTrue(populatedAnonymousOrdersPvl[1]!.IsEmpty);
        populatedAnonymousOrdersPvl.RemoveAt(2);
        Assert.AreEqual(1u, populatedAnonymousOrdersPvl.OrdersCount);
        Assert.IsTrue(populatedAnonymousOrdersPvl[2]!.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPvl_AddAndReset_AddsToLastNonEmptySlot()
    {
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.Add(exampleAnonymousOrderLayer.Clone());
        Assert.AreEqual(OrdersCount + 1, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl[(int)OrdersCount]?.StateReset();
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.Add(exampleAnonymousOrderLayer.Clone());
        Assert.AreEqual(OrdersCount + 1, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.StateReset();
        Assert.AreEqual(0u, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.Add(exampleAnonymousOrderLayer.Clone());
        Assert.AreEqual(1u, populatedAnonymousOrdersPvl.OrdersCount);
    }


    [TestMethod]
    public void EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedAnonymousOrdersPvl.IsEmpty);
        Assert.IsFalse(populatedCounterPartyOrdersPvl.IsEmpty);
        Assert.IsTrue(emptyAnonymousOrdersPvl.IsEmpty);
        Assert.IsTrue(emptyCounterPartyOrdersPvl.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPvl_IsEmptyTrue_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedCounterPartyOrdersPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedCounterPartyOrdersPvl.Price);
        Assert.AreNotEqual(0m, populatedCounterPartyOrdersPvl.Volume);
        Assert.AreNotEqual(0u, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.AreNotEqual(0m, populatedCounterPartyOrdersPvl.InternalVolume);
        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsPriceUpdated);
        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsOrdersCountUpdated);
        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsInternalVolumeUpdated);
        for (var i = 0; i < NumOfOrders; i++)
        {
            var checkOrderLayer = populatedCounterPartyOrdersPvl[i]!;
            Assert.AreNotEqual(0, checkOrderLayer.OrderId);
            Assert.AreNotEqual(LayerOrderFlags.None, checkOrderLayer.OrderLayerFlags);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreNotEqual(0m, ((IPublishedOrder)checkOrderLayer).OrderDisplayVolume);
            Assert.AreNotEqual(0m, checkOrderLayer.OrderRemainingVolume);
            Assert.IsTrue(checkOrderLayer.IsOrderIdUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderLayerFlagsUpdated);
            Assert.IsTrue(checkOrderLayer.IsCreatedTimeDateUpdated);
            Assert.IsTrue(checkOrderLayer.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(checkOrderLayer.IsUpdateTimeDateUpdated);
            Assert.IsTrue(checkOrderLayer.IsUpdateTimeSub2MinUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderVolumeUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderRemainingVolumeUpdated);
            if (checkOrderLayer is IExternalCounterPartyOrderLayerInfo checkCounterPartyLayer)
            {
                Assert.IsNotNull(checkCounterPartyLayer.ExternalCounterPartyName);
                Assert.IsNotNull(checkCounterPartyLayer.ExternalTraderName);
                if (checkCounterPartyLayer is IPQCounterPartyOrderLayerInfo pqCounterPartyLayer)
                {
                    Assert.IsTrue(pqCounterPartyLayer.IsExternalCounterPartyNameUpdated);
                    Assert.IsTrue(pqCounterPartyLayer.IsExternalTraderNameUpdated);
                }
            }
        }
        Assert.IsFalse(populatedCounterPartyOrdersPvl.IsEmpty);

        populatedCounterPartyOrdersPvl.IsEmpty = true;

        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsEmpty);
        Assert.AreEqual(0m, populatedCounterPartyOrdersPvl.Price);
        Assert.AreEqual(0m, populatedCounterPartyOrdersPvl.Volume);
        Assert.AreEqual(0u, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.AreEqual(0m, populatedCounterPartyOrdersPvl.InternalVolume);
        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsPriceUpdated);
        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsOrdersCountUpdated);
        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsInternalVolumeUpdated);
        for (var i = 0; i < NumOfOrders; i++)
        {
            var checkOrderLayer = populatedCounterPartyOrdersPvl[i]!;
            Assert.AreEqual(0, checkOrderLayer.OrderId);
            Assert.AreEqual(LayerOrderFlags.None, checkOrderLayer.OrderLayerFlags);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreEqual(0m, ((IPublishedOrder)checkOrderLayer).OrderDisplayVolume);
            Assert.AreEqual(0m, checkOrderLayer.OrderRemainingVolume);
            Assert.IsTrue(checkOrderLayer.IsOrderIdUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderLayerFlagsUpdated);
            Assert.IsTrue(checkOrderLayer.IsCreatedTimeDateUpdated);
            Assert.IsTrue(checkOrderLayer.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(checkOrderLayer.IsUpdateTimeDateUpdated);
            Assert.IsTrue(checkOrderLayer.IsUpdateTimeSub2MinUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderVolumeUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderRemainingVolumeUpdated);
            if (checkOrderLayer is IExternalCounterPartyOrderLayerInfo checkCounterPartyLayer)
            {
                Assert.IsNull(checkCounterPartyLayer.ExternalCounterPartyName);
                Assert.IsNull(checkCounterPartyLayer.ExternalTraderName);
                if (checkCounterPartyLayer is IPQCounterPartyOrderLayerInfo pqCounterPartyLayer)
                {
                    Assert.IsTrue(pqCounterPartyLayer.IsExternalCounterPartyNameUpdated);
                    Assert.IsTrue(pqCounterPartyLayer.IsExternalTraderNameUpdated);
                }
            }
        }
    }

    [TestMethod]
    public void PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
    {
        var pqFieldUpdates =
            populatedCounterPartyOrdersPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedCounterPartyOrdersPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedCounterPartyOrdersPvl.HasUpdates = false;
        var pqFieldUpdates =
            populatedCounterPartyOrdersPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Snapshot).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedCounterPartyOrdersPvl);

        populatedAnonymousOrdersPvl.HasUpdates = false;
        pqFieldUpdates =
            populatedAnonymousOrdersPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Snapshot).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedAnonymousOrdersPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedCounterPartyOrdersPvl.HasUpdates = false;
        var pqFieldUpdates =
            populatedCounterPartyOrdersPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        var pqStringUpdates =
            populatedCounterPartyOrdersPvl.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        Assert.AreEqual(0, pqStringUpdates.Count);

        populatedAnonymousOrdersPvl.HasUpdates = false;
        pqFieldUpdates =
            populatedAnonymousOrdersPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        pqStringUpdates =
            populatedAnonymousOrdersPvl.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        Assert.AreEqual(0, pqStringUpdates.Count);
    }

    [TestMethod]
    public void PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates =
            populatedCounterPartyOrdersPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var pqStringUpdates =
            populatedCounterPartyOrdersPvl.GetStringUpdates
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQOrdersPriceVolumeLayer(emptyNameIdLookup.Clone(), LayerType.OrdersFullPriceVolume);
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new OrdersPriceVolumeLayer(populatedCounterPartyOrdersPvl, populatedCounterPartyOrdersPvl.LayerType);
        emptyCounterPartyOrdersPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, emptyCounterPartyOrdersPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQOrdersPriceVolumeLayer(emptyNameIdLookup, LayerType.OrdersAnonymousPriceVolume);
        populatedCounterPartyOrdersPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedCounterPartyOrdersPvl);
        Assert.AreEqual(0, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(0u, emptyPriceVolumeLayer.OrdersCount);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.InternalVolume);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsOrdersCountUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsInternalVolumeUpdated);
        Assert.IsTrue(emptyPriceVolumeLayer.IsEmpty);
        Assert.IsFalse(emptyPriceVolumeLayer.HasUpdates);
    }

    [TestMethod]
    public void LayerWithManyTraderDetails_CopyFromSmallerTraderPvl_ClearsDownExtraLayers()
    {
        var lotsOfTraders = new PQOrdersPriceVolumeLayer(nameIdLookup, LayerType.OrdersFullPriceVolume);
        AddCounterPartyOrders(lotsOfTraders, 59);
        Assert.AreEqual(59u, lotsOfTraders.OrdersCount);
        var smallerPvl = new PQOrdersPriceVolumeLayer(nameIdLookup, LayerType.OrdersFullPriceVolume);
        AddCounterPartyOrders(smallerPvl, 1);
        Assert.AreEqual(1u, smallerPvl.OrdersCount);

        lotsOfTraders.CopyFrom(smallerPvl);

        Assert.AreEqual(1u, lotsOfTraders.OrdersCount);
    }

    [TestMethod]
    public void LayerWithManyTraderDetails_CopyFromNonPQSmallerTraderPvl_ClearsDownExtraLayers()
    {
        var lotsOfTraders = new PQOrdersPriceVolumeLayer(nameIdLookup, LayerType.OrdersFullPriceVolume);
        AddCounterPartyOrders(lotsOfTraders, 59);
        Assert.AreEqual(59u, lotsOfTraders.OrdersCount);
        var smallerPvl = new OrdersPriceVolumeLayer(price: 2.3456_78m, volume: 2_949_672m);
        AddCounterPartyOrders(smallerPvl, 1);
        Assert.AreEqual(1u, smallerPvl.OrdersCount);

        lotsOfTraders.CopyFrom(smallerPvl);

        Assert.AreEqual(1u, lotsOfTraders.OrdersCount);
    }

    [TestMethod]
    public void SomeOrderIdAndVolumeUpdates_CopyFrom_OnlyChangesUpdated()
    {
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);

        var clonePopulated = populatedAnonymousOrdersPvl.Clone();
        for (var i = 0; i < clonePopulated.OrdersCount; i++)
        {
            clonePopulated[i]!.OrderId            = i;
            clonePopulated[i]!.OrderDisplayVolume = 50 * i;
        }

        clonePopulated[1]!.IsOrderIdUpdated = false;

        populatedAnonymousOrdersPvl.HasUpdates = false;
        populatedAnonymousOrdersPvl.CopyFrom(clonePopulated);

        Assert.AreEqual(0, populatedAnonymousOrdersPvl[0]!.OrderId);
        Assert.AreEqual(0, ((IPublishedOrder)populatedAnonymousOrdersPvl[0]!).OrderDisplayVolume);
        Assert.IsTrue(populatedAnonymousOrdersPvl[0]!.IsOrderIdUpdated);
        Assert.IsTrue(populatedAnonymousOrdersPvl[0]!.IsOrderVolumeUpdated);

        Assert.AreEqual(251, populatedAnonymousOrdersPvl[1]!.OrderId);
        Assert.AreEqual(50, ((IPublishedOrder)populatedAnonymousOrdersPvl[1]!).OrderDisplayVolume);
        Assert.IsFalse(populatedAnonymousOrdersPvl[1]!.IsOrderIdUpdated);
        Assert.IsTrue(populatedAnonymousOrdersPvl[1]!.IsOrderVolumeUpdated);

        Assert.AreEqual(2, populatedAnonymousOrdersPvl[2]!.OrderId);
        Assert.AreEqual(100, ((IPublishedOrder)populatedAnonymousOrdersPvl[2]!).OrderDisplayVolume);
        Assert.IsTrue(populatedAnonymousOrdersPvl[2]!.IsOrderIdUpdated);
        Assert.IsTrue(populatedAnonymousOrdersPvl[2]!.IsOrderVolumeUpdated);
    }

    [TestMethod]
    public void EmptyPvl_Construction_SetsTraderNameIdLookupWhenNullOrSameAsInfo()
    {
        Assert.AreEqual(populatedCounterPartyOrdersPvl.Orders.Count * 2, nameIdLookup.Count);

        var newEmpty = new PQOrdersPriceVolumeLayer(emptyNameIdLookup, LayerType.OrdersFullPriceVolume);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        Assert.AreSame(emptyNameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void EmptyPvlMissingLookup_Construction_SetsNameIdLookupWhenNullOrSameAsInfo()
    {
        Assert.AreEqual(NumOfOrders * 2, nameIdLookup.Count);

        var moqSrcTkrQuoteInfo = new Mock<IPQSourceTickerInfo>();
        moqSrcTkrQuoteInfo.SetupGet(stqi => stqi.NameIdLookup).Returns(emptyNameIdLookup);

        var newEmpty = new PQOrdersPriceVolumeLayer(new PQNameIdLookupGenerator(0), LayerType.OrdersFullPriceVolume);

        Assert.IsNotNull(newEmpty.NameIdLookup);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        newEmpty.NameIdLookup = emptyNameIdLookup;
        Assert.AreSame(emptyNameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void EmptyPvl_Construction_SharesTraderNameIdLookupBetweenLayers()
    {
        var newEmpty = new PQOrdersPriceVolumeLayer(emptyNameIdLookup, LayerType.OrdersFullPriceVolume);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        Assert.AreSame(emptyNameIdLookup, newEmpty.NameIdLookup);
        newEmpty.NameIdLookup = populatedAnonymousOrdersPvl.NameIdLookup;
        Assert.AreSame(populatedCounterPartyOrdersPvl.NameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IOrdersPriceVolumeLayer)populatedCounterPartyOrdersPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, clonedQuote);

        var cloned2 = ((IMutableOrdersPriceVolumeLayer)populatedCounterPartyOrdersPvl).Clone();
        Assert.AreNotSame(cloned2, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, cloned2);

        var cloned3 = (PQOrdersPriceVolumeLayer)((ICloneable)populatedCounterPartyOrdersPvl).Clone();
        Assert.AreNotSame(cloned3, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, cloned3);

        var cloned4 = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreNotSame(cloned4, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, cloned4);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQOrdersPriceVolumeLayer)((ICloneable)populatedCounterPartyOrdersPvl).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedCounterPartyOrdersPvl, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedCounterPartyOrdersPvl, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedPvlSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedCounterPartyOrdersPvl, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, ((ICloneable)populatedCounterPartyOrdersPvl).Clone());
        Assert.AreEqual(populatedCounterPartyOrdersPvl, ((IOrdersPriceVolumeLayer)populatedCounterPartyOrdersPvl).Clone());
        Assert.AreEqual(populatedCounterPartyOrdersPvl, ((IMutableOrdersPriceVolumeLayer)populatedCounterPartyOrdersPvl).Clone());
        Assert.AreEqual(populatedCounterPartyOrdersPvl, ((ICloneable<IOrdersPriceVolumeLayer>)populatedCounterPartyOrdersPvl).Clone());
        Assert.AreEqual(populatedCounterPartyOrdersPvl, ((PQOrdersCountPriceVolumeLayer)populatedCounterPartyOrdersPvl).Clone());
        Assert.AreEqual(populatedCounterPartyOrdersPvl, populatedCounterPartyOrdersPvl.Clone());
    }

    [TestMethod]
    public void FullyPopulatedPvl_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedCounterPartyOrdersPvl.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedCounterPartyOrdersPvl.ToString()!;

        Assert.IsTrue(toString.Contains(populatedCounterPartyOrdersPvl.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCounterPartyOrdersPvl.Price)}: {populatedCounterPartyOrdersPvl.Price:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCounterPartyOrdersPvl.Volume)}: {populatedCounterPartyOrdersPvl.Volume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCounterPartyOrdersPvl.OrdersCount)}: {populatedCounterPartyOrdersPvl.OrdersCount}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCounterPartyOrdersPvl.InternalVolume)}: {populatedCounterPartyOrdersPvl.InternalVolume:N2}"));
        Assert.IsTrue(toString.Contains("Orders: ["));
        for (var i = 0; i < populatedCounterPartyOrdersPvl.OrdersCount; i++)
            Assert.IsTrue(toString.Contains(populatedCounterPartyOrdersPvl[i]!.ToString()!));
    }

    [TestMethod]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        // ReSharper disable SuspiciousTypeConversion.Global
        Assert.AreEqual(OrdersCount, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.AreEqual(NumOfOrders, ((IEnumerable<IPQAnonymousOrderLayerInfo>)populatedCounterPartyOrdersPvl.Orders).Count());
        Assert.AreEqual(NumOfOrders, ((IEnumerable<IMutableAnonymousOrderLayerInfo>)populatedCounterPartyOrdersPvl.Orders).Count());
        Assert.AreEqual(NumOfOrders, ((IEnumerable<IAnonymousOrderLayerInfo>)populatedCounterPartyOrdersPvl.Orders).Count());

        populatedCounterPartyOrdersPvl.StateReset();

        Assert.AreEqual(0u, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.AreEqual(0, ((IEnumerable<IPQAnonymousOrderLayerInfo>)populatedCounterPartyOrdersPvl.Orders).Count(tli => !tli.IsEmpty));
        Assert.AreEqual(0, ((IEnumerable<IMutableAnonymousOrderLayerInfo>)populatedCounterPartyOrdersPvl.Orders).Count(tli => !tli.IsEmpty));
        Assert.AreEqual(0, ((IEnumerable<IAnonymousOrderLayerInfo>)populatedCounterPartyOrdersPvl.Orders).Count(tli => !tli.IsEmpty));
        // ReSharper restore SuspiciousTypeConversion.Global
    }

    public static void AssertContainsAllPvlFields
    (IList<PQFieldUpdate> checkFieldUpdates, IPQOrdersPriceVolumeLayer pvl, int bookIndex = 0,
        PQFieldFlags priceScale = (PQFieldFlags)1, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        PQOrdersCountPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl, bookIndex, priceScale, volumeScale);
        var depthId      = (PQDepthKey)bookIndex;
        var nameIdLookup = pvl.NameIdLookup;

        for (var i = 0; i < pvl.Orders.Count; i++)
        {
            var orderLayerInfo = pvl[i]!;

            var value      = (uint)orderLayerInfo.OrderId;
            var orderIndex = (ushort)i;
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderId, orderIndex, value),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId
                                                                      , PQOrdersSubFieldKeys.OrderId, orderIndex, value),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = (uint)orderLayerInfo.OrderLayerFlags;
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderLayerFlags, orderIndex, value),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId
                                                                      , PQOrdersSubFieldKeys.OrderLayerFlags, orderIndex, value),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = orderLayerInfo.CreatedTime.Get2MinIntervalsFromUnixEpoch();
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderCreatedDate, orderIndex, value),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId
                                                                      , PQOrdersSubFieldKeys.OrderCreatedDate
                                                                      , orderIndex, value),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var extended = orderLayerInfo.CreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderCreatedSub2MinTime, orderIndex, value, extended)
                           ,
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderCreatedSub2MinTime, orderIndex
                               , value, extended),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = orderLayerInfo.UpdateTime.Get2MinIntervalsFromUnixEpoch();
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderUpdatedDate, orderIndex, value),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId
                                                                      , PQOrdersSubFieldKeys.OrderUpdatedDate
                                                                      , orderIndex, value),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            extended = orderLayerInfo.UpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime, orderIndex, value, extended)
                           ,
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId
                                                                      , PQOrdersSubFieldKeys.OrderUpdatedSub2MinTime,
                                                                        orderIndex, value, extended),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = PQScaling.Scale(((IPublishedOrder)orderLayerInfo).OrderDisplayVolume, volumeScale);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderDisplayVolume, orderIndex, value, volumeScale)
                           ,
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderDisplayVolume, orderIndex, value
                               , volumeScale),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = PQScaling.Scale(orderLayerInfo.OrderRemainingVolume, volumeScale);
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderRemainingVolume, orderIndex, value, volumeScale)
                           ,
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderRemainingVolume, orderIndex
                               , value
                               , volumeScale),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (orderLayerInfo is IPQCounterPartyOrderLayerInfo counterPartyLayerInfo)
            {
                value = (uint)counterPartyLayerInfo.ExternalCounterPartyNameId;
                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId, orderIndex, value)
                               ,
                                PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                    (checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId
                                   , orderIndex, value),
                                $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");


                value = (uint)counterPartyLayerInfo.ExternalTraderNameId;
                Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderExternalTraderNameId, orderIndex, value)
                               ,
                                PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                    (checkFieldUpdates, PQFeedFields.QuoteLayerOrders, depthId, PQOrdersSubFieldKeys.OrderExternalTraderNameId
                                   , orderIndex, value),
                                $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }
        }
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQOrdersPriceVolumeLayer? original,
        IPQOrdersPriceVolumeLayer? changingPriceVolumeLayer,
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

        if (original.GetType() == typeof(PQOrdersPriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new OrdersPriceVolumeLayer(original, original.LayerType), exactComparison));

        Assert.AreEqual(original.OrdersCount, changingPriceVolumeLayer.OrdersCount);
        for (var i = 0; i < original.OrdersCount; i++)
        {
            var originalTraderInfo = original[i];
            var changingTraderInfo = changingPriceVolumeLayer[i];

            Assert.AreEqual(originalTraderInfo != null, changingTraderInfo != null);
            if (originalTraderInfo is PQAnonymousOrderLayerInfo pqOriginalTraderInfo)
                PQAnonymousOrderLayerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, pqOriginalTraderInfo, (PQAnonymousOrderLayerInfo)changingTraderInfo!, original
                   , changingPriceVolumeLayer, originalOrderBookSide, changingOrderBookSide, originalQuote, changingQuote);
        }
    }

    public static void AddAnonymousOrders
    (IMutableOrdersPriceVolumeLayer addOrdersLayers,
        int numOrdersToCreate)
    {
        var createdTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
        var updatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);
        for (var i = 0; i < numOrdersToCreate; i++)
        {
            var anonOrderLayer = addOrdersLayers[i]!;
            anonOrderLayer.OrderId              = OrderId + i;
            anonOrderLayer.OrderLayerFlags      = ExpectedLayerFlags;
            anonOrderLayer.CreatedTime          = createdTime.AddMinutes(5 * i);
            anonOrderLayer.UpdateTime           = updatedTime.AddMinutes(10 * i);
            anonOrderLayer.OrderDisplayVolume   = OrderVolume + i * 100;
            anonOrderLayer.OrderRemainingVolume = OrderVolume / 2;
        }
    }

    public static void AddCounterPartyOrders
    (IMutableOrdersPriceVolumeLayer addOrdersLayers,
        int numOrdersToCreate)
    {
        var createdTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
        var updatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);
        for (var i = 0; i < numOrdersToCreate; i++)
        {
            var anonOrderLayer = addOrdersLayers[i]!;
            anonOrderLayer.OrderId              = OrderId + i;
            anonOrderLayer.OrderLayerFlags      = ExpectedLayerFlags;
            anonOrderLayer.OrderType            = ExpectedOrderType;
            anonOrderLayer.TypeFlags            = ExpectedTypeFlags;
            anonOrderLayer.OrderLifeCycleState  = ExpectedLifecycleState;
            anonOrderLayer.CreatedTime          = createdTime.AddMinutes(5 * i);
            anonOrderLayer.UpdateTime           = updatedTime.AddMinutes(10 * i);
            anonOrderLayer.OrderDisplayVolume   = OrderVolume + i * 100;
            anonOrderLayer.OrderRemainingVolume = OrderVolume / 2;
            anonOrderLayer.TrackingId           = (uint)i + 10;
            if (anonOrderLayer is IMutableExternalCounterPartyOrderLayerInfo counterPartyLayer)
            {
                counterPartyLayer.ExternalCounterPartyId   = ExpectedCounterPartyId;
                counterPartyLayer.ExternalCounterPartyName = $"{CounterPartyBase}_{i}";
                counterPartyLayer.ExternalTraderId         = ExpectedTraderId;
                counterPartyLayer.ExternalTraderName       = $"{TraderNameBase}_{i}";
            }
        }
    }

    public static void AssertOrdersAreAsExpected
    (IOrdersPriceVolumeLayer checkOrdersLayers,
        bool[]? expectPopulated = null, ExpectedValues[]? expectedValues = null, ExpectedUpdated[]? expectedUpdated = null)
    {
        var createdTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
        var updatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);
        if (expectPopulated == null || expectPopulated.Length == 0) Assert.AreEqual(0u, checkOrdersLayers.OrdersCount);
        for (var i = 0; i < checkOrdersLayers.OrdersCount; i++)
            if (expectPopulated == null || expectPopulated[i])
            {
                var expectedOrderId              = expectedValues?[i].OrderId ?? OrderId + i;
                var expectedOrderFlags           = expectedValues?[i].OrderFlags ?? ExpectedLayerFlags;
                var expectedCreatedTime          = expectedValues?[i].CreatedTime ?? createdTime.AddMinutes(5 * i);
                var expectedUpdatedTime          = expectedValues?[i].UpdatedTime ?? updatedTime.AddMinutes(10 * i);
                var expectedOrderVolume          = expectedValues?[i].OrderVolume ?? OrderVolume + i * 100;
                var expectedOrderRemainingVolume = expectedValues?[i].OrderRemainingVolume ?? OrderVolume / 2;

                var expectedCounterPartyName = expectedValues?[i].CounterPartyName ?? $"{CounterPartyBase}_{i}";
                var expectedTraderName       = expectedValues?[i].TraderName ?? $"{TraderNameBase}_{i}";

                var checkOrderLayer = checkOrdersLayers[i]!;

                Assert.AreEqual(expectedOrderId, checkOrderLayer.OrderId);
                Assert.AreEqual(expectedOrderFlags, checkOrderLayer.OrderLayerFlags);
                Assert.AreEqual(expectedCreatedTime, checkOrderLayer.CreatedTime);
                Assert.AreEqual(expectedUpdatedTime, checkOrderLayer.UpdateTime);
                Assert.AreEqual(expectedOrderVolume, checkOrderLayer.OrderDisplayVolume);
                Assert.AreEqual(expectedOrderRemainingVolume, checkOrderLayer.OrderRemainingVolume);
                if (checkOrderLayer is IPQAnonymousOrderLayerInfo pqAnonymousOrder)
                {
                    Assert.AreEqual(expectedUpdated?[i].IsOrderIdUpdated ?? true, pqAnonymousOrder.IsOrderIdUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsOrderFlagsUpdated ?? true, pqAnonymousOrder.IsOrderLayerFlagsUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsCreatedDateUpdated ?? true, pqAnonymousOrder.IsCreatedTimeDateUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsCreatedTimeSub2MinUpdated ?? true, pqAnonymousOrder.IsCreatedTimeSub2MinUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsUpdatedTimeDateUpdated ?? true, pqAnonymousOrder.IsUpdateTimeDateUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsUpdatedTimeSub2MinUpdated ?? true, pqAnonymousOrder.IsUpdateTimeSub2MinUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsOrderVolumeUpdated ?? true, pqAnonymousOrder.IsOrderVolumeUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsOrderRemainingVolumeUpdated ?? true, pqAnonymousOrder.IsOrderRemainingVolumeUpdated);
                }
                if (checkOrderLayer is IExternalCounterPartyOrderLayerInfo checkCounterPartyLayer)
                {
                    Assert.AreEqual(expectedCounterPartyName, checkCounterPartyLayer.ExternalCounterPartyName);
                    Assert.AreEqual(expectedTraderName, checkCounterPartyLayer.ExternalTraderName);
                    if (checkCounterPartyLayer is IPQCounterPartyOrderLayerInfo pqCounterPartyLayer)
                    {
                        Assert.AreEqual(expectedUpdated?[i].IsOrderCounterPartyNameIdUpdated ?? true
                                      , pqCounterPartyLayer.IsExternalCounterPartyNameUpdated);
                        Assert.AreEqual(expectedUpdated?[i].IsOrderTraderNameIdUpdated ?? true, pqCounterPartyLayer.IsExternalTraderNameUpdated);
                    }
                }
            }
            else
            {
                var checkOrderLayer = checkOrdersLayers[i]!;
                Assert.AreEqual(0, checkOrderLayer.OrderId);
                Assert.AreEqual(LayerOrderFlags.None, checkOrderLayer.OrderLayerFlags);
                Assert.AreEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
                Assert.AreEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
                Assert.AreEqual(0m, checkOrderLayer.OrderDisplayVolume);
                Assert.AreEqual(0m, checkOrderLayer.OrderRemainingVolume);
                if (checkOrderLayer is IPQAnonymousOrderLayerInfo pqAnonymousOrder)
                {
                    Assert.IsFalse(pqAnonymousOrder.IsOrderIdUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsOrderLayerFlagsUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsCreatedTimeDateUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsCreatedTimeSub2MinUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsUpdateTimeDateUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsUpdateTimeSub2MinUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsOrderVolumeUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsOrderRemainingVolumeUpdated);
                }
                if (checkOrderLayer is IExternalCounterPartyOrderLayerInfo checkCounterPartyLayer)
                {
                    Assert.IsNull(checkCounterPartyLayer.ExternalCounterPartyName);
                    Assert.IsNull(checkCounterPartyLayer.ExternalTraderName);
                    if (checkCounterPartyLayer is IPQCounterPartyOrderLayerInfo pqCounterPartyLayer)
                    {
                        Assert.IsFalse(pqCounterPartyLayer.IsExternalCounterPartyNameUpdated);
                        Assert.IsFalse(pqCounterPartyLayer.IsExternalTraderNameUpdated);
                    }
                }
            }
    }

    public record ExpectedValues
    (
        // ReSharper disable MemberHidesStaticFromOuterClass
        int? OrderId = null
      , LayerOrderFlags? OrderFlags = null
      , DateTime? CreatedTime = null
      , DateTime? UpdatedTime = null
      , decimal? OrderVolume = null
      , decimal? OrderRemainingVolume = null
      , string? CounterPartyName = null
      , string? TraderName = null
        // ReSharper restore MemberHidesStaticFromOuterClass
    );

    public record ExpectedUpdated
    (
        bool IsOrderIdUpdated = false
      , bool IsOrderFlagsUpdated = false
      , bool IsCreatedDateUpdated = false
      , bool IsCreatedTimeSub2MinUpdated = false
      , bool IsUpdatedTimeDateUpdated = false
      , bool IsUpdatedTimeSub2MinUpdated = false
      , bool IsOrderVolumeUpdated = false
      , bool IsOrderRemainingVolumeUpdated = false
      , bool IsOrderCounterPartyNameIdUpdated = false
      , bool IsOrderTraderNameIdUpdated = false
    )
    {
        public static ExpectedUpdated AllUpdatedExcept
        (
            bool isOrderIdUpdated = true
          , bool isOrderFlagsUpdated = true
          , bool isCreatedDateUpdated = true
          , bool isCreatedTimeSub2MinUpdated = true
          , bool isUpdatedTimeDateUpdated = true
          , bool isUpdatedTimeSub2MinUpdated = true
          , bool isOrderVolumeUpdated = true
          , bool isOrderRemainingVolumeUpdated = true
          , bool isOrderCounterPartyNameIdUpdated = true
          , bool isOrderTraderNameIdUpdated = true) =>
            new(isOrderIdUpdated, isOrderFlagsUpdated, isCreatedDateUpdated, isCreatedTimeSub2MinUpdated,
                isUpdatedTimeDateUpdated, isUpdatedTimeSub2MinUpdated, isOrderVolumeUpdated, isOrderRemainingVolumeUpdated,
                isOrderCounterPartyNameIdUpdated, isOrderTraderNameIdUpdated);
    };
}
