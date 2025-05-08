// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQOrdersPriceVolumeLayerTests
{
    private const decimal Price          = 1.3456m;
    private const decimal Volume         = 100_000m;
    private const uint    OrdersCount    = 3;
    private const int     NumOfOrders    = 3;
    private const decimal InternalVolume = 50_000m;

    private const int             OrderId              = 250;
    private const LayerOrderFlags OrderFlags           = LayerOrderFlags.CreatedFromSource;
    private const decimal         OrderVolume          = 50.50m;
    private const decimal         OrderRemainingVolume = 10.25m;
    private const string          CounterPartyBase     = "TestCounterPartyName";
    private const string          TraderNameBase       = "TestTraderName";

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
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        nameIdLookup      = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);

        exampleAnonymousOrderLayer = new PQAnonymousOrderLayerInfo(OrderId, OrderFlags, CreatedTime, OrderVolume, UpdatedTime, OrderRemainingVolume);

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
        Assert.IsTrue(((IPQCounterPartyOrderLayerInfo)newPvl[0]!).IsCounterPartyNameUpdated);
        Assert.IsTrue(((IPQCounterPartyOrderLayerInfo)newPvl[0]!).IsTraderNameUpdated);
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
            orderLayer.IsOrderFlagsUpdated           = false;
            orderLayer.IsCreatedTimeDateUpdated      = false;
            orderLayer.IsCreatedTimeSub2MinUpdated   = false;
            orderLayer.IsUpdatedTimeDateUpdated      = false;
            orderLayer.IsUpdatedTimeSub2MinUpdated   = false;
            orderLayer.IsOrderVolumeUpdated          = false;
            orderLayer.IsOrderRemainingVolumeUpdated = false;
            if (orderLayer is IPQCounterPartyOrderLayerInfo counterPartyOrderLayer)
            {
                counterPartyOrderLayer.IsCounterPartyNameUpdated = false;
                counterPartyOrderLayer.IsTraderNameUpdated       = false;
            }
        }
        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderIdUpdated = true;
        fromPQInstance = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        var expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderIdUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderFlagsUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsOrderFlagsUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsOrderFlagsUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsCreatedTimeDateUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsCreatedDateUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsCreatedTimeDateUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsCreatedTimeSub2MinUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsCreatedTimeSubHourUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsCreatedTimeSub2MinUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsUpdatedTimeDateUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsUpdatedTimeDateUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsUpdatedTimeDateUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersAnonymousPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsUpdatedTimeSub2MinUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersAnonymousPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsUpdatedTimeSubHourUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) orderLayer!.IsUpdatedTimeSub2MinUpdated = false;

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
        foreach (var orderLayer in newPopulatedPvl.Orders) ((IPQCounterPartyOrderLayerInfo?)orderLayer)!.IsCounterPartyNameUpdated = true;
        fromPQInstance  = new PQOrdersPriceVolumeLayer(newPopulatedPvl, LayerType.OrdersFullPriceVolume, newPopulatedPvl.NameIdLookup);
        expectedUpdated = Enumerable.Range(1, NumOfOrders).Select(_ => new ExpectedUpdated(IsOrderCounterPartyNameIdUpdated: true)).ToArray();
        AssertOrdersAreAsExpected(fromPQInstance, expectPopulatedOrders, null, expectedUpdated);
        foreach (var orderLayer in newPopulatedPvl.Orders) ((IPQCounterPartyOrderLayerInfo?)orderLayer)!.IsCounterPartyNameUpdated = false;

        newPopulatedPvl = new PQOrdersPriceVolumeLayer(originalCpLayerOrder, LayerType.OrdersFullPriceVolume, nameIdLookup.Clone());
        foreach (var orderLayer in newPopulatedPvl.Orders) ((IPQCounterPartyOrderLayerInfo?)orderLayer)!.IsTraderNameUpdated = true;
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
                OrderId = OrderId + 3, OrderVolume = OrderVolume
            };
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        Assert.AreEqual(OrderId + 3, ((IOrdersPriceVolumeLayer)newEmpty)[3]!.OrderId);
        Assert.AreEqual(OrderVolume, ((IOrdersPriceVolumeLayer)newEmpty)[3]!.OrderVolume);

        Assert.IsNotNull(((IOrdersPriceVolumeLayer)newEmpty)[255]);
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        ((IMutableOrdersPriceVolumeLayer)newEmpty)[255]!.OrderId = OrderId + 255;
        Assert.AreEqual(256u, newEmpty.OrdersCount);

        newEmpty[255] = new PQAnonymousOrderLayerInfo
        {
            OrderId = OrderId + 255, OrderVolume = OrderVolume
        };
        Assert.AreEqual(256u, newEmpty.OrdersCount);
        Assert.AreEqual(OrderId + 255, ((IOrdersPriceVolumeLayer)newEmpty)[255]!.OrderId);
        Assert.AreEqual(OrderVolume, ((IOrdersPriceVolumeLayer)newEmpty)[255]!.OrderVolume);
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
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderId, orderIndex, expectedOrderId);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsOrderIdUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            expectedLayerField = new PQFieldUpdate(expectedLayerField.Id, expectedLayerField.SubId, orderIndex, expectedLayerField.Payload);

            orderLayerInfo.IsOrderIdUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderId && update.AuxiliaryPayload == orderIndex
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

            Assert.IsFalse(orderLayerInfo.IsOrderFlagsUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(LayerOrderFlags.None, orderLayerInfo.OrderFlags);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedOrderFlags = LayerOrderFlags.CreatedFromSource | LayerOrderFlags.CancelRequested | LayerOrderFlags.NotExternalVolume;
            orderLayerInfo.OrderFlags = expectedOrderFlags;
            Assert.IsTrue(orderLayerInfo.IsOrderFlagsUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedOrderFlags, orderLayerInfo.OrderFlags);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex         = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderFlags, orderIndex, (uint)expectedOrderFlags);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsOrderFlagsUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());


            orderLayerInfo.IsOrderFlagsUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderFlags && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.OrderFlags          = LayerOrderFlags.None;
            orderLayerInfo.IsOrderFlagsUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedOrderFlags, foundTraderInfo!.OrderFlags);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsOrderFlagsUpdated);
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
            orderLayerInfo.CreatedTime                 = testDateTime;
            orderLayerInfo.UpdateComplete();
            orderLayerInfo.CreatedTime                 = DateTime.MinValue;
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
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderCreatedDate, orderIndex, value);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsCreatedTimeDateUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            orderLayerInfo.IsCreatedTimeDateUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderCreatedDate && update.AuxiliaryPayload == orderIndex
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
    public void EmptyPvl_LayerCreatedTimeSubHourChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var orderLayerInfo = emptyCounterPartyOrdersPvl[i]!;
            // increment NumUpdatesSinceEmpty
            orderLayerInfo.CreatedTime                 = testDateTime;
            orderLayerInfo.UpdateComplete();
            orderLayerInfo.CreatedTime                 = DateTime.MinValue;
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
            var extended           = expectedCreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            var orderIndex         = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderCreatedSub2MinTime, orderIndex, value, extended);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsCreatedTimeSub2MinUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            orderLayerInfo.IsCreatedTimeSub2MinUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderCreatedSub2MinTime && update.AuxiliaryPayload == orderIndex
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
            orderLayerInfo.UpdatedTime                 = testDateTime;
            orderLayerInfo.UpdateComplete();
            orderLayerInfo.UpdatedTime                 = DateTime.MinValue;
            orderLayerInfo.UpdateComplete();

            Assert.IsFalse(orderLayerInfo.IsUpdatedTimeDateUpdated);
            Assert.IsFalse(orderLayerInfo.IsUpdatedTimeSub2MinUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(DateTime.MinValue, orderLayerInfo.UpdatedTime);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedUpdatedTime = DateTime.UnixEpoch.AddHours(1);
            orderLayerInfo.UpdatedTime = expectedUpdatedTime;
            Assert.IsTrue(orderLayerInfo.IsUpdatedTimeDateUpdated);
            Assert.IsFalse(orderLayerInfo.IsUpdatedTimeSub2MinUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedUpdatedTime, orderLayerInfo.UpdatedTime);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var value              = expectedUpdatedTime.Get2MinIntervalsFromUnixEpoch();
            var orderIndex         = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderUpdatedDate, orderIndex, value);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsUpdatedTimeDateUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            orderLayerInfo.IsUpdatedTimeDateUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderUpdatedDate && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.UpdatedTime                 = DateTime.MinValue;
            orderLayerInfo.IsUpdatedTimeDateUpdated    = false;
            orderLayerInfo.IsUpdatedTimeSub2MinUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedUpdatedTime, foundTraderInfo!.UpdatedTime);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsUpdatedTimeDateUpdated);
            Assert.IsFalse(foundTraderInfo.IsUpdatedTimeSub2MinUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerUpdatedTimeSubHourChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var orderLayerInfo = emptyCounterPartyOrdersPvl[i]!;
            // increment NumUpdatesSinceEmpty
            orderLayerInfo.UpdatedTime                 = testDateTime;
            orderLayerInfo.UpdateComplete();
            orderLayerInfo.UpdatedTime                 = DateTime.MinValue;
            orderLayerInfo.UpdateComplete();

            Assert.IsFalse(orderLayerInfo.IsUpdatedTimeDateUpdated);
            Assert.IsFalse(orderLayerInfo.IsUpdatedTimeSub2MinUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(DateTime.MinValue, orderLayerInfo.UpdatedTime);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedUpdatedTime = DateTime.MinValue.AddMinutes(1);
            orderLayerInfo.UpdatedTime = expectedUpdatedTime;
            Assert.IsFalse(orderLayerInfo.IsUpdatedTimeDateUpdated);
            Assert.IsTrue(orderLayerInfo.IsUpdatedTimeSub2MinUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedUpdatedTime, orderLayerInfo.UpdatedTime);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var extended           = expectedUpdatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            var orderIndex         = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderUpdatedSub2MinTime, orderIndex, value, extended);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.IsUpdatedTimeSub2MinUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            orderLayerInfo.IsUpdatedTimeSub2MinUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderUpdatedSub2MinTime && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            orderLayerInfo.UpdatedTime                 = DateTime.MinValue;
            orderLayerInfo.IsUpdatedTimeDateUpdated    = false;
            orderLayerInfo.IsUpdatedTimeSub2MinUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedUpdatedTime, foundTraderInfo!.UpdatedTime);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsUpdatedTimeSub2MinUpdated);
            Assert.IsFalse(foundTraderInfo.IsUpdatedTimeDateUpdated);
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
            Assert.AreEqual(0m, traderLayerInfo.OrderVolume);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedOrderVolume = OrderVolume + 1000;
            traderLayerInfo.OrderVolume = expectedOrderVolume;
            Assert.IsTrue(traderLayerInfo.IsOrderVolumeUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedOrderVolume, traderLayerInfo.OrderVolume);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex         = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderVolume, orderIndex, expectedOrderVolume, (PQFieldFlags)6);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            traderLayerInfo.IsOrderVolumeUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            traderLayerInfo.IsOrderVolumeUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderVolume && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            traderLayerInfo.OrderVolume          = 0;
            traderLayerInfo.IsOrderVolumeUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedOrderVolume, foundTraderInfo!.OrderVolume);
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
            var orderIndex         = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderRemainingVolume, orderIndex, expectedOrderRemainingVolume, (PQFieldFlags)6);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            traderLayerInfo.IsOrderRemainingVolumeUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            traderLayerInfo.IsOrderRemainingVolumeUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderRemainingVolume && update.AuxiliaryPayload == orderIndex
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

            Assert.IsFalse(cpOrderLayerInfo.IsCounterPartyNameUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(null, cpOrderLayerInfo.CounterPartyName);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedCounterPartyName = CounterPartyBase + i;
            cpOrderLayerInfo.CounterPartyName = expectedCounterPartyName;
            Assert.IsTrue(cpOrderLayerInfo.IsCounterPartyNameUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedCounterPartyName, cpOrderLayerInfo.CounterPartyName);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderCounterPartyNameId, orderIndex, (uint)layerNameIdLookup[cpOrderLayerInfo.CounterPartyName]);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            var stringUpdates = emptyCounterPartyOrdersPvl.GetStringUpdates(testDateTime, StorageFlags.Update)
                                                          .ToList();
            Assert.AreEqual(1, stringUpdates.Count);
            var expectedStringUpdates = new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQQuoteFields.LayerNameDictionaryUpsertCommand, CrudCommand.Upsert.ToPQSubFieldId(), orderIndex, 0u)
              , StringUpdate = new PQStringUpdate
                {
                    Command = CrudCommand.Upsert, DictionaryId = emptyCounterPartyOrdersPvl.NameIdLookup[cpOrderLayerInfo.CounterPartyName]
                  , Value   = expectedCounterPartyName
                }
            };
            Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

            cpOrderLayerInfo.IsCounterPartyNameUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            cpOrderLayerInfo.NameIdLookup.HasUpdates = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());
            cpOrderLayerInfo.IsCounterPartyNameUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderCounterPartyNameId && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            cpOrderLayerInfo.NameIdLookup.Clear();
            cpOrderLayerInfo.CounterPartyName          = null;
            cpOrderLayerInfo.IsCounterPartyNameUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            newEmpty.UpdateFieldString(stringUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedCounterPartyName, foundTraderInfo!.CounterPartyName);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsCounterPartyNameUpdated);
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

            Assert.IsFalse(cpOrderLayerInfo.IsTraderNameUpdated);
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(null, cpOrderLayerInfo.TraderName);
            Assert.AreEqual(0, emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            var expectedTraderName = TraderNameBase + i;
            cpOrderLayerInfo.TraderName = expectedTraderName;
            Assert.IsTrue(cpOrderLayerInfo.IsTraderNameUpdated);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.AreEqual(expectedTraderName, cpOrderLayerInfo.TraderName);
            var layerUpdates = emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var orderIndex = (ushort)i;
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderTraderNameId, orderIndex, (uint)layerNameIdLookup[cpOrderLayerInfo.TraderName]);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            var stringUpdates = emptyCounterPartyOrdersPvl.GetStringUpdates(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, stringUpdates.Count);
            var expectedStringUpdates = new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQQuoteFields.LayerNameDictionaryUpsertCommand, CrudCommand.Upsert.ToPQSubFieldId(), orderIndex,  0u)
              , StringUpdate = new PQStringUpdate
                {
                    Command = CrudCommand.Upsert, DictionaryId = emptyCounterPartyOrdersPvl.NameIdLookup[cpOrderLayerInfo.TraderName]
                  , Value   = expectedTraderName
                }
            };
            Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

            cpOrderLayerInfo.IsTraderNameUpdated = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            cpOrderLayerInfo.NameIdLookup.HasUpdates = false;
            Assert.IsFalse(emptyCounterPartyOrdersPvl.HasUpdates);
            Assert.IsTrue(emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());
            cpOrderLayerInfo.IsTraderNameUpdated = true;
            layerUpdates =
                (from update in emptyCounterPartyOrdersPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.SubId == PQSubFieldKeys.OrderTraderNameId && update.AuxiliaryPayload == orderIndex
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            cpOrderLayerInfo.NameIdLookup.Clear();
            cpOrderLayerInfo.TraderName          = null;
            cpOrderLayerInfo.IsTraderNameUpdated = false;

            var newEmpty = new PQOrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            newEmpty.UpdateFieldString(stringUpdates[0]);
            var foundTraderInfo = (IPQCounterPartyOrderLayerInfo?)newEmpty[i];
            Assert.AreEqual(expectedTraderName, foundTraderInfo!.TraderName);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsTraderNameUpdated);
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
            Assert.AreNotEqual(LayerOrderFlags.None, checkOrderLayer.OrderFlags);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.UpdatedTime);
            Assert.AreNotEqual(0m, checkOrderLayer.OrderVolume);
            Assert.AreNotEqual(0m, checkOrderLayer.OrderRemainingVolume);
            Assert.IsTrue(checkOrderLayer.IsOrderIdUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderFlagsUpdated);
            Assert.IsTrue(checkOrderLayer.IsCreatedTimeDateUpdated);
            Assert.IsTrue(checkOrderLayer.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(checkOrderLayer.IsUpdatedTimeDateUpdated);
            Assert.IsTrue(checkOrderLayer.IsUpdatedTimeSub2MinUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderVolumeUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderRemainingVolumeUpdated);
            if (checkOrderLayer is ICounterPartyOrderLayerInfo checkCounterPartyLayer)
            {
                Assert.IsNotNull(checkCounterPartyLayer.CounterPartyName);
                Assert.IsNotNull(checkCounterPartyLayer.TraderName);
                if (checkCounterPartyLayer is IPQCounterPartyOrderLayerInfo pqCounterPartyLayer)
                {
                    Assert.IsTrue(pqCounterPartyLayer.IsCounterPartyNameUpdated);
                    Assert.IsTrue(pqCounterPartyLayer.IsTraderNameUpdated);
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
            Assert.AreEqual(LayerOrderFlags.None, checkOrderLayer.OrderFlags);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.UpdatedTime);
            Assert.AreEqual(0m, checkOrderLayer.OrderVolume);
            Assert.AreEqual(0m, checkOrderLayer.OrderRemainingVolume);
            Assert.IsTrue(checkOrderLayer.IsOrderIdUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderFlagsUpdated);
            Assert.IsTrue(checkOrderLayer.IsCreatedTimeDateUpdated);
            Assert.IsTrue(checkOrderLayer.IsCreatedTimeSub2MinUpdated);
            Assert.IsTrue(checkOrderLayer.IsUpdatedTimeDateUpdated);
            Assert.IsTrue(checkOrderLayer.IsUpdatedTimeSub2MinUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderVolumeUpdated);
            Assert.IsTrue(checkOrderLayer.IsOrderRemainingVolumeUpdated);
            if (checkOrderLayer is ICounterPartyOrderLayerInfo checkCounterPartyLayer)
            {
                Assert.IsNull(checkCounterPartyLayer.CounterPartyName);
                Assert.IsNull(checkCounterPartyLayer.TraderName);
                if (checkCounterPartyLayer is IPQCounterPartyOrderLayerInfo pqCounterPartyLayer)
                {
                    Assert.IsTrue(pqCounterPartyLayer.IsCounterPartyNameUpdated);
                    Assert.IsTrue(pqCounterPartyLayer.IsTraderNameUpdated);
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
            clonePopulated[i]!.OrderId     = i;
            clonePopulated[i]!.OrderVolume = 50 * i;
        }

        clonePopulated[1]!.IsOrderIdUpdated = false;

        populatedAnonymousOrdersPvl.HasUpdates = false;
        populatedAnonymousOrdersPvl.CopyFrom(clonePopulated);

        Assert.AreEqual(0, populatedAnonymousOrdersPvl[0]!.OrderId);
        Assert.AreEqual(0, populatedAnonymousOrdersPvl[0]!.OrderVolume);
        Assert.IsTrue(populatedAnonymousOrdersPvl[0]!.IsOrderIdUpdated);
        Assert.IsTrue(populatedAnonymousOrdersPvl[0]!.IsOrderVolumeUpdated);

        Assert.AreEqual(251, populatedAnonymousOrdersPvl[1]!.OrderId);
        Assert.AreEqual(50, populatedAnonymousOrdersPvl[1]!.OrderVolume);
        Assert.IsFalse(populatedAnonymousOrdersPvl[1]!.IsOrderIdUpdated);
        Assert.IsTrue(populatedAnonymousOrdersPvl[1]!.IsOrderVolumeUpdated);

        Assert.AreEqual(2, populatedAnonymousOrdersPvl[2]!.OrderId);
        Assert.AreEqual(100, populatedAnonymousOrdersPvl[2]!.OrderVolume);
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
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderId, orderIndex, value),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderId, orderIndex, value),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = (uint)orderLayerInfo.OrderFlags;
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderFlags, orderIndex, value),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderFlags, orderIndex, value),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = orderLayerInfo.CreatedTime.Get2MinIntervalsFromUnixEpoch();
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderCreatedDate, orderIndex, value),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderCreatedDate
                                                                      , orderIndex, value),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            var extended = orderLayerInfo.CreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderCreatedSub2MinTime, orderIndex, value, extended),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderCreatedSub2MinTime, orderIndex, value , extended),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = orderLayerInfo.UpdatedTime.Get2MinIntervalsFromUnixEpoch();
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderUpdatedDate, orderIndex, value),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderUpdatedDate
                                                                      , orderIndex, value),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            extended = orderLayerInfo.UpdatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out value);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderUpdatedSub2MinTime, orderIndex, value, extended),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderUpdatedSub2MinTime,
                                                                        orderIndex, value, extended),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = PQScaling.Scale(orderLayerInfo.OrderVolume, volumeScale);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderVolume, orderIndex, value, volumeScale),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderVolume, orderIndex, value, volumeScale),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            value = PQScaling.Scale(orderLayerInfo.OrderRemainingVolume, volumeScale);
            Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderRemainingVolume, orderIndex, value, volumeScale),
                            PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                (checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderRemainingVolume, orderIndex, value, volumeScale),
                            $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (orderLayerInfo is IPQCounterPartyOrderLayerInfo counterPartyLayerInfo)
            {
                value = (uint)counterPartyLayerInfo.OrderCounterPartyNameId;
                Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderCounterPartyNameId, orderIndex, value),
                                PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                    (checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderCounterPartyNameId, orderIndex, value),
                                $"For {pvl.GetType().Name} at {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");


                value = (uint)counterPartyLayerInfo.OrderTraderNameId;
                Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderTraderNameId, orderIndex, value),
                                PQLevel2QuoteTests.ExtractFieldUpdateWithId
                                    (checkFieldUpdates, PQQuoteFields.LayerOrders, depthId, PQSubFieldKeys.OrderTraderNameId, orderIndex, value),
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
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
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
            anonOrderLayer.OrderFlags           = OrderFlags;
            anonOrderLayer.CreatedTime          = createdTime.AddMinutes(5 * i);
            anonOrderLayer.UpdatedTime          = updatedTime.AddMinutes(10 * i);
            anonOrderLayer.OrderVolume          = OrderVolume + i * 100;
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
            anonOrderLayer.OrderFlags           = OrderFlags;
            anonOrderLayer.CreatedTime          = createdTime.AddMinutes(5 * i);
            anonOrderLayer.UpdatedTime          = updatedTime.AddMinutes(10 * i);
            anonOrderLayer.OrderVolume          = OrderVolume + i * 100;
            anonOrderLayer.OrderRemainingVolume = OrderVolume / 2;
            if (anonOrderLayer is IMutableCounterPartyOrderLayerInfo counterPartyLayer)
            {
                counterPartyLayer.CounterPartyName = $"{CounterPartyBase}_{i}";
                counterPartyLayer.TraderName       = $"{TraderNameBase}_{i}";
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
                var expectedOrderFlags           = expectedValues?[i].OrderFlags ?? OrderFlags;
                var expectedCreatedTime          = expectedValues?[i].CreatedTime ?? createdTime.AddMinutes(5 * i);
                var expectedUpdatedTime          = expectedValues?[i].UpdatedTime ?? updatedTime.AddMinutes(10 * i);
                var expectedOrderVolume          = expectedValues?[i].OrderVolume ?? OrderVolume + i * 100;
                var expectedOrderRemainingVolume = expectedValues?[i].OrderRemainingVolume ?? OrderVolume / 2;

                var expectedCounterPartyName = expectedValues?[i].CounterPartyName ?? $"{CounterPartyBase}_{i}";
                var expectedTraderName       = expectedValues?[i].TraderName ?? $"{TraderNameBase}_{i}";

                var checkOrderLayer = checkOrdersLayers[i]!;

                Assert.AreEqual(expectedOrderId, checkOrderLayer.OrderId);
                Assert.AreEqual(expectedOrderFlags, checkOrderLayer.OrderFlags);
                Assert.AreEqual(expectedCreatedTime, checkOrderLayer.CreatedTime);
                Assert.AreEqual(expectedUpdatedTime, checkOrderLayer.UpdatedTime);
                Assert.AreEqual(expectedOrderVolume, checkOrderLayer.OrderVolume);
                Assert.AreEqual(expectedOrderRemainingVolume, checkOrderLayer.OrderRemainingVolume);
                if (checkOrderLayer is IPQAnonymousOrderLayerInfo pqAnonymousOrder)
                {
                    Assert.AreEqual(expectedUpdated?[i].IsOrderIdUpdated ?? true, pqAnonymousOrder.IsOrderIdUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsOrderFlagsUpdated ?? true, pqAnonymousOrder.IsOrderFlagsUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsCreatedDateUpdated ?? true, pqAnonymousOrder.IsCreatedTimeDateUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsCreatedTimeSubHourUpdated ?? true, pqAnonymousOrder.IsCreatedTimeSub2MinUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsUpdatedTimeDateUpdated ?? true, pqAnonymousOrder.IsUpdatedTimeDateUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsUpdatedTimeSubHourUpdated ?? true, pqAnonymousOrder.IsUpdatedTimeSub2MinUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsOrderVolumeUpdated ?? true, pqAnonymousOrder.IsOrderVolumeUpdated);
                    Assert.AreEqual(expectedUpdated?[i].IsOrderRemainingVolumeUpdated ?? true, pqAnonymousOrder.IsOrderRemainingVolumeUpdated);
                }
                if (checkOrderLayer is ICounterPartyOrderLayerInfo checkCounterPartyLayer)
                {
                    Assert.AreEqual(expectedCounterPartyName, checkCounterPartyLayer.CounterPartyName);
                    Assert.AreEqual(expectedTraderName, checkCounterPartyLayer.TraderName);
                    if (checkCounterPartyLayer is IPQCounterPartyOrderLayerInfo pqCounterPartyLayer)
                    {
                        Assert.AreEqual(expectedUpdated?[i].IsOrderCounterPartyNameIdUpdated ?? true, pqCounterPartyLayer.IsCounterPartyNameUpdated);
                        Assert.AreEqual(expectedUpdated?[i].IsOrderTraderNameIdUpdated ?? true, pqCounterPartyLayer.IsTraderNameUpdated);
                    }
                }
            }
            else
            {
                var checkOrderLayer = checkOrdersLayers[i]!;
                Assert.AreEqual(0, checkOrderLayer.OrderId);
                Assert.AreEqual(LayerOrderFlags.None, checkOrderLayer.OrderFlags);
                Assert.AreEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
                Assert.AreEqual(DateTime.MinValue, checkOrderLayer.UpdatedTime);
                Assert.AreEqual(0m, checkOrderLayer.OrderVolume);
                Assert.AreEqual(0m, checkOrderLayer.OrderRemainingVolume);
                if (checkOrderLayer is IPQAnonymousOrderLayerInfo pqAnonymousOrder)
                {
                    Assert.IsFalse(pqAnonymousOrder.IsOrderIdUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsOrderFlagsUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsCreatedTimeDateUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsCreatedTimeSub2MinUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsUpdatedTimeDateUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsUpdatedTimeSub2MinUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsOrderVolumeUpdated);
                    Assert.IsFalse(pqAnonymousOrder.IsOrderRemainingVolumeUpdated);
                }
                if (checkOrderLayer is ICounterPartyOrderLayerInfo checkCounterPartyLayer)
                {
                    Assert.IsNull(checkCounterPartyLayer.CounterPartyName);
                    Assert.IsNull(checkCounterPartyLayer.TraderName);
                    if (checkCounterPartyLayer is IPQCounterPartyOrderLayerInfo pqCounterPartyLayer)
                    {
                        Assert.IsFalse(pqCounterPartyLayer.IsCounterPartyNameUpdated);
                        Assert.IsFalse(pqCounterPartyLayer.IsTraderNameUpdated);
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
      , bool IsCreatedTimeSubHourUpdated = false
      , bool IsUpdatedTimeDateUpdated = false
      , bool IsUpdatedTimeSubHourUpdated = false
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
          , bool isCreatedTimeSubHourUpdated = true
          , bool isUpdatedTimeDateUpdated = true
          , bool isUpdatedTimeSubHourUpdated = true
          , bool isOrderVolumeUpdated = true
          , bool isOrderRemainingVolumeUpdated = true
          , bool isOrderCounterPartyNameIdUpdated = true
          , bool isOrderTraderNameIdUpdated = true) =>
            new(isOrderIdUpdated, isOrderFlagsUpdated, isCreatedDateUpdated, isCreatedTimeSubHourUpdated,
                isUpdatedTimeDateUpdated, isUpdatedTimeSubHourUpdated, isOrderVolumeUpdated, isOrderRemainingVolumeUpdated,
                isOrderCounterPartyNameIdUpdated, isOrderTraderNameIdUpdated);
    };
}
