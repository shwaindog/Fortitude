// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

[TestClass]
public class OrdersPriceVolumeLayerTests
{
    private const decimal Price            = 1.3456m;
    private const decimal Volume           = 100_000m;
    private const uint    OrdersCount      = 3;
    private const int     NumOfOrders      = 3;
    private const decimal InternalVolume   = 50_000m;
    private const int     OrderId          = 250;
    private const decimal OrderVolume      = 50.50m;
    private const string  CounterPartyBase = "TestCounterPartyName";
    private const string  TraderNameBase   = "TestTraderName";

    private const decimal OrderRemainingVolume = 10.25m;

    private const int ExpectedTraderId       = 2;
    private const int ExpectedCounterPartyId = 1;

    private const OrderGenesisFlags ExpectedGenesisFlags = OrderGenesisFlags.FromAdapter;
    private const OrderType         ExpectedOrderType    = OrderType.PassiveLimit;

    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.ConfirmedActiveOnMarket;

    private static readonly DateTime CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private OrdersPriceVolumeLayer emptyAnonymousOrdersPvl    = null!;
    private OrdersPriceVolumeLayer emptyCounterPartyOrdersPvl = null!;

    private AnonymousOrder           exampleAnonymousOrderLayer     = null!;
    private IPQNameIdLookupGenerator nameIdLookupGenerator          = null!;
    private OrdersPriceVolumeLayer   populatedAnonymousOrdersPvl    = null!;
    private OrdersPriceVolumeLayer   populatedCounterPartyOrdersPvl = null!;

    private ExternalCounterPartyOrder exampleCounterPartyOrderLayer = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

        exampleAnonymousOrderLayer =
            new AnonymousOrder
                (OrderId, CreatedTime, OrderVolume, ExpectedOrderType
               , ExpectedGenesisFlags, ExpectedLifecycleState, UpdatedTime, OrderRemainingVolume);
        exampleCounterPartyOrderLayer =
            new ExternalCounterPartyOrder
                (new AnonymousOrder(OrderId, CreatedTime, OrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState, UpdatedTime
                                  , OrderRemainingVolume)
                {
                    ExternalCounterPartyOrderInfo
                        = new AdditionalExternalCounterPartyInfo(ExpectedCounterPartyId, CounterPartyBase, ExpectedTraderId, TraderNameBase)
                });

        emptyAnonymousOrdersPvl     = new OrdersPriceVolumeLayer();
        emptyCounterPartyOrdersPvl  = new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume);
        populatedAnonymousOrdersPvl = new OrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, Price, Volume, OrdersCount, InternalVolume);
        AddAnonymousOrders(populatedAnonymousOrdersPvl, NumOfOrders);
        populatedCounterPartyOrdersPvl = new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, Price, Volume, OrdersCount, InternalVolume);
        AddCounterPartyOrders(populatedCounterPartyOrdersPvl, NumOfOrders);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        bool[] expectPopulated    = [true, true, true];
        bool[] expectNotPopulated = [false, false, false];

        var newPvl = new OrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, Price, Volume, OrdersCount, InternalVolume);
        AddAnonymousOrders(newPvl, NumOfOrders);
        Assert.AreEqual(Price, newPvl.Price);
        Assert.AreEqual(Volume, newPvl.Volume);
        Assert.AreEqual(OrdersCount, newPvl.OrdersCount);
        Assert.AreEqual(InternalVolume, newPvl.InternalVolume);
        AssertOrderLayersAreExpected(newPvl, expectPopulated);

        Assert.AreEqual(0, emptyAnonymousOrdersPvl.Price);
        Assert.AreEqual(0, emptyAnonymousOrdersPvl.Volume);
        Assert.AreEqual(0u, emptyAnonymousOrdersPvl.OrdersCount);
        Assert.AreEqual(0m, emptyAnonymousOrdersPvl.InternalVolume);
        AssertOrderLayersAreExpected(emptyAnonymousOrdersPvl, expectNotPopulated);

        Assert.AreEqual(0, emptyCounterPartyOrdersPvl.Price);
        Assert.AreEqual(0, emptyCounterPartyOrdersPvl.Volume);
        Assert.AreEqual(0u, emptyCounterPartyOrdersPvl.OrdersCount);
        Assert.AreEqual(0m, emptyCounterPartyOrdersPvl.InternalVolume);
        AssertOrderLayersAreExpected(emptyCounterPartyOrdersPvl, expectNotPopulated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var expectPopulated           = new[] { true, true, true };
        var newPopulatedAnonOrdersPvl = new OrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume, Price, Volume, OrdersCount, InternalVolume);
        AddAnonymousOrders(newPopulatedAnonOrdersPvl, NumOfOrders);
        var fromInstance = new OrdersPriceVolumeLayer(newPopulatedAnonOrdersPvl, newPopulatedAnonOrdersPvl.LayerType);
        Assert.AreEqual(Price, fromInstance.Price);
        Assert.AreEqual(Volume, fromInstance.Volume);
        Assert.AreEqual(OrdersCount, fromInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromInstance.InternalVolume);
        AssertOrderLayersAreExpected(fromInstance, expectPopulated);

        var pqAnonOrdersPvl = new PQOrdersPriceVolumeLayer(nameIdLookupGenerator.Clone(), LayerType.OrdersAnonymousPriceVolume, Price, Volume
                                                         , OrdersCount, InternalVolume);
        AddAnonymousOrders(pqAnonOrdersPvl, NumOfOrders);
        var fromPqInstance = new OrdersPriceVolumeLayer(pqAnonOrdersPvl, newPopulatedAnonOrdersPvl.LayerType);
        Assert.AreEqual(Price, fromPqInstance.Price);
        Assert.AreEqual(Volume, fromPqInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPqInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPqInstance.InternalVolume);
        AssertOrderLayersAreExpected(fromPqInstance, expectPopulated);

        var newEmptyPvl = new PQOrdersPriceVolumeLayer(emptyAnonymousOrdersPvl, LayerType.OrdersAnonymousPriceVolume, nameIdLookupGenerator);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(0u, newEmptyPvl.OrdersCount);
        Assert.AreEqual(0m, newEmptyPvl.InternalVolume);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsOrdersCountUpdated);
        Assert.IsFalse(newEmptyPvl.IsInternalVolumeUpdated);
        AssertOrderLayersAreExpected(newEmptyPvl);

        var newPopulatedCounterPartyOrdersPvl
            = new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume, Price, Volume, OrdersCount, InternalVolume);
        AddCounterPartyOrders(newPopulatedCounterPartyOrdersPvl, NumOfOrders);
        fromInstance = new OrdersPriceVolumeLayer(newPopulatedCounterPartyOrdersPvl, newPopulatedCounterPartyOrdersPvl.LayerType);
        Assert.AreEqual(Price, fromInstance.Price);
        Assert.AreEqual(Volume, fromInstance.Volume);
        Assert.AreEqual(OrdersCount, fromInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromInstance.InternalVolume);
        AssertOrderLayersAreExpected(fromInstance, expectPopulated);

        var pqCounterPartyOrdersPvl = new PQOrdersPriceVolumeLayer(nameIdLookupGenerator.Clone(), LayerType.OrdersFullPriceVolume,
                                                                   Price, Volume, OrdersCount, InternalVolume);
        AddCounterPartyOrders(pqCounterPartyOrdersPvl, NumOfOrders);
        fromPqInstance = new OrdersPriceVolumeLayer(pqCounterPartyOrdersPvl, pqCounterPartyOrdersPvl.LayerType);
        Assert.AreEqual(Price, fromPqInstance.Price);
        Assert.AreEqual(Volume, fromPqInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPqInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPqInstance.InternalVolume);
        AssertOrderLayersAreExpected(fromPqInstance, expectPopulated);

        newEmptyPvl = new PQOrdersPriceVolumeLayer(emptyAnonymousOrdersPvl, LayerType.OrdersFullPriceVolume, nameIdLookupGenerator);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(0u, newEmptyPvl.OrdersCount);
        Assert.AreEqual(0m, newEmptyPvl.InternalVolume);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsOrdersCountUpdated);
        Assert.IsFalse(newEmptyPvl.IsInternalVolumeUpdated);
        AssertOrderLayersAreExpected(newEmptyPvl);
    }

    [TestMethod]
    public void EmptyPvl_IndexerGetSets_AddNewLayersIfIndexedViaVariousInterfaces()
    {
        var newEmpty = (IMutableOrdersPriceVolumeLayer)emptyAnonymousOrdersPvl.Clone();
        Assert.AreEqual(0u, newEmpty.OrdersCount);
        newEmpty[3] = exampleAnonymousOrderLayer.Clone();
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        var orderLayer = newEmpty[3]!;
        Assert.AreEqual(OrderId, orderLayer.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, orderLayer.GenesisFlags);
        Assert.AreEqual(CreatedTime, orderLayer.CreatedTime);
        Assert.AreEqual(UpdatedTime, orderLayer.UpdateTime);
        Assert.AreEqual(OrderVolume, ((IAnonymousOrder)orderLayer).OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, orderLayer.OrderRemainingVolume);

        Assert.IsNotNull(((IOrdersPriceVolumeLayer)newEmpty)[255]);
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        newEmpty[255]!.OrderId = OrderId;
        Assert.AreEqual(256u, newEmpty.OrdersCount);

        newEmpty[255] = exampleAnonymousOrderLayer.Clone();
        orderLayer    = newEmpty[255]!;
        Assert.AreEqual(OrderId, orderLayer.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, orderLayer.GenesisFlags);
        Assert.AreEqual(CreatedTime, orderLayer.CreatedTime);
        Assert.AreEqual(UpdatedTime, orderLayer.UpdateTime);
        Assert.AreEqual(OrderVolume, ((IAnonymousOrder)orderLayer).OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, orderLayer.OrderRemainingVolume);

        newEmpty = emptyCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(0u, newEmpty.OrdersCount);
        newEmpty[3] = exampleCounterPartyOrderLayer.Clone();
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        var counterPartyOrderLayer = (IExternalCounterPartyOrder)newEmpty[3]!;
        Assert.AreEqual(OrderId, counterPartyOrderLayer.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags
                      , counterPartyOrderLayer.GenesisFlags);
        Assert.AreEqual(CreatedTime, counterPartyOrderLayer.CreatedTime);
        Assert.AreEqual(UpdatedTime, counterPartyOrderLayer.UpdateTime);
        Assert.AreEqual(OrderVolume, counterPartyOrderLayer.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, counterPartyOrderLayer.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyBase, counterPartyOrderLayer.ExternalCounterPartyName);
        Assert.AreEqual(TraderNameBase, counterPartyOrderLayer.ExternalTraderName);

        Assert.IsNotNull(((IOrdersPriceVolumeLayer)newEmpty)[255]);
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        newEmpty[255]!.OrderId = OrderId;
        Assert.AreEqual(256u, newEmpty.OrdersCount);

        newEmpty[255]          = exampleCounterPartyOrderLayer.Clone();
        counterPartyOrderLayer = (IExternalCounterPartyOrder)newEmpty[255]!;
        Assert.AreEqual(OrderId, orderLayer.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, orderLayer.GenesisFlags);
        Assert.AreEqual(CreatedTime, orderLayer.CreatedTime);
        Assert.AreEqual(UpdatedTime, orderLayer.UpdateTime);
        Assert.AreEqual(OrderVolume, ((IAnonymousOrder)orderLayer).OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, orderLayer.OrderRemainingVolume);
        Assert.AreEqual(OrderRemainingVolume, counterPartyOrderLayer.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyBase, counterPartyOrderLayer.ExternalCounterPartyName);
        Assert.AreEqual(TraderNameBase, counterPartyOrderLayer.ExternalTraderName);
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
            var neverSet = emptyAnonymousOrdersPvl[ushort.MaxValue + 1];
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);

        caughtException = false;
        try
        {
            emptyAnonymousOrdersPvl[ushort.MaxValue + 1] = null;
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
    }

    [TestMethod]
    public void PopulatedPvl_AddAndReset_AddsToLastNonEmptySlot()
    {
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.Add(exampleAnonymousOrderLayer.Clone());
        Assert.AreEqual(OrdersCount + 1, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl[NumOfOrders]?.StateReset();
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.Add(exampleAnonymousOrderLayer.Clone());
        Assert.AreEqual(OrdersCount + 1, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.StateReset();
        Assert.AreEqual(0u, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.Add(exampleAnonymousOrderLayer.Clone());
        Assert.AreEqual(1u, populatedAnonymousOrdersPvl.OrdersCount);

        Assert.AreEqual(OrdersCount, populatedCounterPartyOrdersPvl.OrdersCount);
        populatedCounterPartyOrdersPvl.Add(exampleCounterPartyOrderLayer.Clone());
        Assert.AreEqual(OrdersCount + 1, populatedCounterPartyOrdersPvl.OrdersCount);
        populatedCounterPartyOrdersPvl[NumOfOrders]?.StateReset();
        Assert.AreEqual(OrdersCount, populatedCounterPartyOrdersPvl.OrdersCount);
        populatedCounterPartyOrdersPvl.Add(exampleCounterPartyOrderLayer.Clone());
        Assert.AreEqual(OrdersCount + 1, populatedCounterPartyOrdersPvl.OrdersCount);
        populatedCounterPartyOrdersPvl.StateReset();
        Assert.AreEqual(0u, populatedCounterPartyOrdersPvl.OrdersCount);
        populatedCounterPartyOrdersPvl.Add(exampleCounterPartyOrderLayer.Clone());
        Assert.AreEqual(1u, populatedCounterPartyOrdersPvl.OrdersCount);
    }

    [TestMethod]
    public void PopulatedPvl_RemoveAt_ClearsOrReducesCount()
    {
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);
        populatedAnonymousOrdersPvl.RemoveAt(1);
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);
        Assert.IsTrue(populatedAnonymousOrdersPvl[1]!.IsEmpty);
        populatedAnonymousOrdersPvl.RemoveAt(2);
        Assert.IsTrue(populatedAnonymousOrdersPvl[2]!.IsEmpty);
        Assert.AreEqual(1u, populatedAnonymousOrdersPvl.OrdersCount);
        Assert.IsTrue(populatedAnonymousOrdersPvl[2]!.IsEmpty);

        Assert.AreEqual(OrdersCount, populatedCounterPartyOrdersPvl.OrdersCount);
        populatedCounterPartyOrdersPvl.RemoveAt(1);
        Assert.AreEqual(OrdersCount, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.IsTrue(populatedCounterPartyOrdersPvl[1]!.IsEmpty);
        populatedCounterPartyOrdersPvl.RemoveAt(2);
        Assert.AreEqual(1u, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.IsTrue(populatedCounterPartyOrdersPvl[2]!.IsEmpty);
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
    public void PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedAnonymousOrdersPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedAnonymousOrdersPvl.Price);
        Assert.AreNotEqual(0m, populatedAnonymousOrdersPvl.Volume);
        Assert.AreNotEqual(0u, populatedAnonymousOrdersPvl.OrdersCount);
        Assert.AreNotEqual(0m, populatedAnonymousOrdersPvl.InternalVolume);
        for (var i = 0; i < OrdersCount; i++)
        {
            var checkOrderLayer = populatedAnonymousOrdersPvl[i]!;
            Assert.AreNotEqual(0, checkOrderLayer.OrderId);
            Assert.AreNotEqual(OrderGenesisFlags.None, checkOrderLayer.GenesisFlags);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreNotEqual(0m, ((IAnonymousOrder)checkOrderLayer).OrderDisplayVolume);
            Assert.AreNotEqual(0m, checkOrderLayer.OrderRemainingVolume);
            Assert.IsFalse(checkOrderLayer.IsEmpty);
        }

        populatedAnonymousOrdersPvl.StateReset();
        Assert.IsTrue(populatedAnonymousOrdersPvl.IsEmpty);
        Assert.AreEqual(0m, populatedAnonymousOrdersPvl.Price);
        Assert.AreEqual(0m, populatedAnonymousOrdersPvl.Volume);
        Assert.AreEqual(0u, populatedAnonymousOrdersPvl.OrdersCount);
        Assert.AreEqual(0m, populatedAnonymousOrdersPvl.InternalVolume);
        for (var i = 0; i < OrdersCount; i++)
        {
            var checkOrderLayer = populatedAnonymousOrdersPvl[i]!;
            Assert.AreEqual(0, checkOrderLayer.OrderId);
            Assert.AreEqual(OrderGenesisFlags.None, checkOrderLayer.GenesisFlags);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreEqual(0m, ((IAnonymousOrder)checkOrderLayer).OrderDisplayVolume);
            Assert.AreEqual(0m, checkOrderLayer.OrderRemainingVolume);
            Assert.IsTrue(checkOrderLayer.IsEmpty);
        }

        Assert.IsFalse(populatedCounterPartyOrdersPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedCounterPartyOrdersPvl.Price);
        Assert.AreNotEqual(0m, populatedCounterPartyOrdersPvl.Volume);
        Assert.AreNotEqual(0u, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.AreNotEqual(0m, populatedCounterPartyOrdersPvl.InternalVolume);
        for (var i = 0; i < OrdersCount; i++)
        {
            var checkOrderLayer = populatedCounterPartyOrdersPvl[i]!;
            Assert.AreNotEqual(0, checkOrderLayer.OrderId);
            Assert.AreNotEqual(OrderGenesisFlags.None, checkOrderLayer.GenesisFlags);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreNotEqual(0m, ((IAnonymousOrder)checkOrderLayer).OrderDisplayVolume);
            Assert.AreNotEqual(0m, checkOrderLayer.OrderRemainingVolume);
            Assert.IsFalse(checkOrderLayer.IsEmpty);
            if (checkOrderLayer is IMutableExternalCounterPartyOrder checkCounterPartyLayer)
            {
                Assert.IsNotNull(checkCounterPartyLayer.ExternalCounterPartyName);
                Assert.IsNotNull(((IExternalCounterPartyOrder)checkCounterPartyLayer).ExternalTraderName);
            }
        }

        populatedCounterPartyOrdersPvl.StateReset();
        Assert.IsTrue(populatedCounterPartyOrdersPvl.IsEmpty);
        Assert.AreEqual(0m, populatedCounterPartyOrdersPvl.Price);
        Assert.AreEqual(0m, populatedCounterPartyOrdersPvl.Volume);
        Assert.AreEqual(0u, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.AreEqual(0m, populatedCounterPartyOrdersPvl.InternalVolume);
        for (var i = 0; i < OrdersCount; i++)
        {
            var checkOrderLayer = populatedCounterPartyOrdersPvl[i]!;
            Assert.AreEqual(0, checkOrderLayer.OrderId);
            Assert.AreEqual(IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, checkOrderLayer.GenesisFlags);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreEqual(0m, ((IAnonymousOrder)checkOrderLayer).OrderDisplayVolume);
            Assert.AreEqual(0m, checkOrderLayer.OrderRemainingVolume);
            Assert.IsTrue(checkOrderLayer.IsEmpty);
            if (checkOrderLayer is IMutableExternalCounterPartyOrder checkCounterPartyLayer)
            {
                Assert.IsNull(checkCounterPartyLayer.ExternalCounterPartyName);
                Assert.IsNull(((IExternalCounterPartyOrder)checkCounterPartyLayer).ExternalTraderName);
            }
        }
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther()
    {
        emptyAnonymousOrdersPvl.CopyFrom(populatedAnonymousOrdersPvl);
        Assert.AreEqual(populatedAnonymousOrdersPvl, emptyAnonymousOrdersPvl);

        emptyCounterPartyOrdersPvl.CopyFrom(populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, emptyCounterPartyOrdersPvl);
    }

    [TestMethod]
    public void PQPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther()
    {
        var pqPvl    = new PQOrdersPriceVolumeLayer(populatedAnonymousOrdersPvl, LayerType.OrdersFullPriceVolume, nameIdLookupGenerator);
        var newEmpty = new OrdersPriceVolumeLayer(LayerType.OrdersAnonymousPriceVolume);
        newEmpty.CopyFrom(pqPvl);
        Assert.AreEqual(populatedAnonymousOrdersPvl, newEmpty);

        pqPvl    = new PQOrdersPriceVolumeLayer(populatedCounterPartyOrdersPvl, LayerType.OrdersFullPriceVolume, nameIdLookupGenerator);
        newEmpty = new OrdersPriceVolumeLayer(LayerType.OrdersFullPriceVolume);
        newEmpty.CopyFrom(pqPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, newEmpty);
    }

    [TestMethod]
    public void LayerWithOrders_CopyFromSmallerOrderPvl_ClearsDownExtraLayers()
    {
        var lotsOfTraders = new PQOrdersPriceVolumeLayer(nameIdLookupGenerator.Clone(), LayerType.OrdersAnonymousPriceVolume);
        AddAnonymousOrders(lotsOfTraders, 59);
        Assert.AreEqual(59u, lotsOfTraders.OrdersCount);
        var smallerPvl = new PQOrdersPriceVolumeLayer(nameIdLookupGenerator, LayerType.OrdersAnonymousPriceVolume);
        AddAnonymousOrders(smallerPvl, 1);
        Assert.AreEqual(1u, smallerPvl.OrdersCount);
        lotsOfTraders.CopyFrom(smallerPvl);
        Assert.AreEqual(1u, lotsOfTraders.OrdersCount);

        lotsOfTraders = new PQOrdersPriceVolumeLayer(nameIdLookupGenerator.Clone(), LayerType.OrdersFullPriceVolume);
        AddAnonymousOrders(lotsOfTraders, 59);
        Assert.AreEqual(59u, lotsOfTraders.OrdersCount);
        smallerPvl = new PQOrdersPriceVolumeLayer(nameIdLookupGenerator, LayerType.OrdersFullPriceVolume);
        AddAnonymousOrders(smallerPvl, 1);
        Assert.AreEqual(1u, smallerPvl.OrdersCount);
        lotsOfTraders.CopyFrom(smallerPvl);
        Assert.AreEqual(1u, lotsOfTraders.OrdersCount);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((IPriceVolumeLayer)populatedAnonymousOrdersPvl).Clone();
        Assert.AreNotSame(clone, populatedAnonymousOrdersPvl);
        Assert.AreEqual(populatedAnonymousOrdersPvl, clone);
        clone = populatedAnonymousOrdersPvl.Clone();
        Assert.AreNotSame(clone, populatedAnonymousOrdersPvl);
        Assert.AreEqual(populatedAnonymousOrdersPvl, clone);
        clone = ((ICloneable<IOrdersPriceVolumeLayer>)populatedAnonymousOrdersPvl).Clone();
        Assert.AreNotSame(clone, populatedAnonymousOrdersPvl);
        Assert.AreEqual(populatedAnonymousOrdersPvl, clone);
        clone = ((IOrdersPriceVolumeLayer)populatedAnonymousOrdersPvl).Clone();
        Assert.AreNotSame(clone, populatedAnonymousOrdersPvl);
        Assert.AreEqual(populatedAnonymousOrdersPvl, clone);
        clone = ((IMutableOrdersPriceVolumeLayer)populatedAnonymousOrdersPvl).Clone();
        Assert.AreNotSame(clone, populatedAnonymousOrdersPvl);
        Assert.AreEqual(populatedAnonymousOrdersPvl, clone);

        clone = ((IPriceVolumeLayer)populatedCounterPartyOrdersPvl).Clone();
        Assert.AreNotSame(clone, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, clone);
        clone = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreNotSame(clone, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, clone);
        clone = ((ICloneable<IOrdersPriceVolumeLayer>)populatedCounterPartyOrdersPvl).Clone();
        Assert.AreNotSame(clone, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, clone);
        clone = ((IOrdersPriceVolumeLayer)populatedCounterPartyOrdersPvl).Clone();
        Assert.AreNotSame(clone, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, clone);
        clone = ((IMutableOrdersPriceVolumeLayer)populatedCounterPartyOrdersPvl).Clone();
        Assert.AreNotSame(clone, populatedCounterPartyOrdersPvl);
        Assert.AreEqual(populatedCounterPartyOrdersPvl, clone);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableOrdersPriceVolumeLayer)populatedCounterPartyOrdersPvl).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedCounterPartyOrdersPvl, fullyPopulatedClone);
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
        var toString = populatedCounterPartyOrdersPvl.ToString();

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
        // ReSharper disable RedundantCast
        Assert.AreEqual(OrdersCount, populatedAnonymousOrdersPvl.OrdersCount);
        Assert.AreEqual(NumOfOrders, ((IEnumerable<IAnonymousOrder>)populatedAnonymousOrdersPvl.Orders).Count());
        Assert.AreEqual(NumOfOrders, ((IEnumerable<IMutableAnonymousOrder>)populatedAnonymousOrdersPvl.Orders).Count());
        Assert.AreEqual(0, ((IEnumerable)populatedAnonymousOrdersPvl.Orders).OfType<IExternalCounterPartyOrder>().Count());

        populatedAnonymousOrdersPvl.StateReset();

        Assert.AreEqual(0u, populatedAnonymousOrdersPvl.OrdersCount);
        Assert.AreEqual(0, ((IEnumerable<IAnonymousOrder>)populatedAnonymousOrdersPvl.Orders).Count(tli => !tli.IsEmpty));
        Assert.AreEqual(0, ((IEnumerable<IMutableAnonymousOrder>)populatedAnonymousOrdersPvl.Orders).Count(tli => !tli.IsEmpty));
        Assert.AreEqual(0, ((IEnumerable)populatedAnonymousOrdersPvl.Orders).OfType<IExternalCounterPartyOrder>().Count(tli => !tli.IsEmpty));


        Assert.AreEqual(OrdersCount, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.AreEqual(NumOfOrders, ((IEnumerable<IAnonymousOrder>)populatedCounterPartyOrdersPvl.Orders).Count());
        Assert.AreEqual(NumOfOrders, ((IEnumerable<IMutableAnonymousOrder>)populatedCounterPartyOrdersPvl.Orders).Count());
        Assert.AreEqual(NumOfOrders, ((IEnumerable)populatedCounterPartyOrdersPvl.Orders).OfType<IExternalCounterPartyOrder>().Count());

        populatedCounterPartyOrdersPvl.StateReset();

        Assert.AreEqual(0u, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.AreEqual(0, ((IEnumerable<IAnonymousOrder>)populatedCounterPartyOrdersPvl.Orders).Count(tli => !tli.IsEmpty));
        Assert.AreEqual(0, ((IEnumerable<IMutableAnonymousOrder>)populatedCounterPartyOrdersPvl.Orders).Count(tli => !tli.IsEmpty));
        Assert.AreEqual(0, ((IEnumerable)populatedCounterPartyOrdersPvl.Orders).OfType<IExternalCounterPartyOrder>().Count(tli => !tli.IsEmpty));
        // ReSharper restore RedundantCast
    }

    public static void AddAnonymousOrders
        (IMutableOrdersPriceVolumeLayer addOrdersLayers, int numOrdersToCreate)
    {
        var createdTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
        var updatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);
        for (var i = 0; i < numOrdersToCreate; i++)
        {
            var anonOrderLayer = addOrdersLayers[i]!;
            anonOrderLayer.OrderId              = OrderId + i;
            anonOrderLayer.GenesisFlags         = ExpectedGenesisFlags;
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
            anonOrderLayer.GenesisFlags         = ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags;
            anonOrderLayer.CreatedTime          = createdTime.AddMinutes(5 * i);
            anonOrderLayer.UpdateTime           = updatedTime.AddMinutes(10 * i);
            anonOrderLayer.OrderDisplayVolume   = OrderVolume + i * 100;
            anonOrderLayer.OrderRemainingVolume = OrderVolume / 2;
            if (anonOrderLayer is IMutableExternalCounterPartyOrder counterPartyLayer)
            {
                counterPartyLayer.ExternalCounterPartyName = $"{CounterPartyBase}_{i}";
                counterPartyLayer.ExternalTraderName       = $"{TraderNameBase}_{i}";
            }
        }
    }

    public static void AssertOrderLayersAreExpected
    (IOrdersPriceVolumeLayer checkOrdersLayers,
        bool[]? expectPopulated = null, string[]? expectedTraderNames = null, decimal[]? expectedVolumes = null,
        bool[]? expectedTraderNameUpdated = null, bool[]? expectedVolumeUpdated = null)
    {
        var createdTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
        var updatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);
        if (expectPopulated == null || expectPopulated.Length == 0) Assert.AreEqual(0u, checkOrdersLayers.OrdersCount);
        for (var i = 0; i < checkOrdersLayers.OrdersCount; i++)
            if (expectPopulated == null || expectPopulated[i])
            {
                var checkOrderLayer = checkOrdersLayers[i]!;

                var expectedOrderId = OrderId + i;
                var expectedGenesisFlags = checkOrderLayer is IExternalCounterPartyOrder
                    ? ExpectedGenesisFlags | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags
                    : ExpectedGenesisFlags;
                var expectedCreatedTime = createdTime.AddMinutes(5 * i);
                var expectedUpdatedTime = updatedTime.AddMinutes(10 * i);
                var expectedOrderVolume = OrderVolume + i * 100;

                var expectedOrderRemainingVolume = OrderVolume / 2;

                var expectedCounterPartyName = $"{CounterPartyBase}_{i}";
                var expectedTraderName       = expectedTraderNames?[i] ?? $"{TraderNameBase}_{i}";

                Assert.AreEqual(expectedOrderId, checkOrderLayer.OrderId);
                Assert.AreEqual(expectedGenesisFlags, checkOrderLayer.GenesisFlags);
                Assert.AreEqual(expectedCreatedTime, checkOrderLayer.CreatedTime);
                Assert.AreEqual(expectedUpdatedTime, checkOrderLayer.UpdateTime);
                Assert.AreEqual(expectedOrderVolume, checkOrderLayer.OrderDisplayVolume);
                Assert.AreEqual(expectedOrderRemainingVolume, checkOrderLayer.OrderRemainingVolume);
                if (checkOrderLayer is IMutableExternalCounterPartyOrder checkCounterPartyLayer)
                {
                    Assert.AreEqual(expectedCounterPartyName, checkCounterPartyLayer.ExternalCounterPartyName);
                    Assert.AreEqual(expectedTraderName, ((IExternalCounterPartyOrder)checkCounterPartyLayer).ExternalTraderName);
                }
            }
            else
            {
                var checkOrderLayer = checkOrdersLayers[i]!;
                Assert.AreEqual(0, checkOrderLayer.OrderId);
                Assert.AreEqual(OrderGenesisFlags.None, checkOrderLayer.GenesisFlags);
                Assert.AreEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
                Assert.AreEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
                Assert.AreEqual(0m, checkOrderLayer.OrderDisplayVolume);
                Assert.AreEqual(0m, checkOrderLayer.OrderRemainingVolume);
                if (checkOrderLayer is IMutableExternalCounterPartyOrder checkCounterPartyLayer)
                {
                    Assert.IsNull(checkCounterPartyLayer.ExternalCounterPartyName);
                    Assert.IsNull(((IExternalCounterPartyOrder)checkCounterPartyLayer).ExternalTraderName);
                }
            }
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableOrdersPriceVolumeLayer? original,
        IMutableOrdersPriceVolumeLayer? changingPriceVolumeLayer,
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

        OrdersCountPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide, changingOrderBookSide,
             originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        Assert.AreEqual(original.OrdersCount, changingPriceVolumeLayer.OrdersCount);
        for (var i = 0; i < original.OrdersCount; i++)
        {
            var originalOrderLayerInfo = original[i];
            var changingOrderLayerInfo = changingPriceVolumeLayer[i];

            Assert.AreEqual(originalOrderLayerInfo != null, changingOrderLayerInfo != null);
            if (originalOrderLayerInfo is IExternalCounterPartyOrder || changingOrderLayerInfo is IExternalCounterPartyOrder)
            {
                ExternalCounterPartyOrderTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, originalOrderLayerInfo as IMutableExternalCounterPartyOrder,
                     changingOrderLayerInfo as IMutableExternalCounterPartyOrder, original, changingPriceVolumeLayer
                   , originalOrderBookSide, changingOrderBookSide, originalQuote, changingQuote);
            }
            else
            {
                AnonymousOrderTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                    (exactComparison, originalOrderLayerInfo, changingOrderLayerInfo, original, changingPriceVolumeLayer
                   , originalOrderBookSide, changingOrderBookSide, originalQuote, changingQuote);
            }
        }
    }
}
