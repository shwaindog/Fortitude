// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
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
    private const uint    OrdersCount      = 12;
    private const int     NumOfOrders      = 12;
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
    
    private readonly DateTime testDateTime = new(2025, 6, 1, 11, 54, 52);

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

        exampleAnonymousOrderLayer =
            new AnonymousOrder
                (OrderId + 50, CreatedTime, OrderVolume, ExpectedOrderType
               , ExpectedGenesisFlags, ExpectedLifecycleState, UpdatedTime, OrderRemainingVolume);
        exampleCounterPartyOrderLayer =
            new ExternalCounterPartyOrder
                (new AnonymousOrder(OrderId + 100, CreatedTime, OrderVolume, ExpectedOrderType, ExpectedGenesisFlags, ExpectedLifecycleState, UpdatedTime
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
        bool[] expectPopulated    = [true, true, true, true, true, true, true, true, true, true, true, true ];
        bool[] expectNotPopulated = [false, false, false, false, false, false, false, false, false, false, false, false ];

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
        var expectPopulated           = new[] { true, true, true, true, true, true, true, true, true, true, true, true };
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
        var orderLayer = newEmpty[3];
        Assert.AreEqual(OrderId + 50, orderLayer.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, orderLayer.GenesisFlags);
        Assert.AreEqual(CreatedTime, orderLayer.CreatedTime);
        Assert.AreEqual(UpdatedTime, orderLayer.UpdateTime);
        Assert.AreEqual(OrderVolume, orderLayer.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, orderLayer.OrderRemainingVolume);

        Assert.IsNotNull(((IOrdersPriceVolumeLayer)newEmpty)[255]);
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        newEmpty[255].OrderId = OrderId;
        Assert.AreEqual(256u, newEmpty.OrdersCount);

        newEmpty[255] = exampleAnonymousOrderLayer.Clone();
        orderLayer    = newEmpty[255];
        Assert.AreEqual(OrderId + 50, orderLayer.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, orderLayer.GenesisFlags);
        Assert.AreEqual(CreatedTime, orderLayer.CreatedTime);
        Assert.AreEqual(UpdatedTime, orderLayer.UpdateTime);
        Assert.AreEqual(OrderVolume, orderLayer.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, orderLayer.OrderRemainingVolume);

        newEmpty = emptyCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(0u, newEmpty.OrdersCount);
        newEmpty[3] = exampleCounterPartyOrderLayer.Clone();
        Assert.AreEqual(4u, newEmpty.OrdersCount);
        var counterPartyOrderLayer = (IExternalCounterPartyOrder)newEmpty[3];
        Assert.AreEqual(OrderId+100, counterPartyOrderLayer.OrderId);
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
        newEmpty[255].OrderId = OrderId;
        Assert.AreEqual(256u, newEmpty.OrdersCount);

        newEmpty[255]          = exampleCounterPartyOrderLayer.Clone();
        counterPartyOrderLayer = (IExternalCounterPartyOrder)newEmpty[255];
        Assert.AreEqual(OrderId + 50, orderLayer.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, orderLayer.GenesisFlags);
        Assert.AreEqual(CreatedTime, orderLayer.CreatedTime);
        Assert.AreEqual(UpdatedTime, orderLayer.UpdateTime);
        Assert.AreEqual(OrderVolume, orderLayer.OrderDisplayVolume);
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
            emptyAnonymousOrdersPvl[ushort.MaxValue + 1] = exampleAnonymousOrderLayer;
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
            emptyAnonymousOrdersPvl[ushort.MaxValue + 1] = exampleAnonymousOrderLayer;
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
        populatedAnonymousOrdersPvl[NumOfOrders].StateReset();
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
        populatedCounterPartyOrdersPvl[NumOfOrders].StateReset();
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
        Assert.AreEqual(OrdersCount - 1, populatedAnonymousOrdersPvl.OrdersCount);
        Assert.IsFalse(populatedAnonymousOrdersPvl[1].IsEmpty);
        populatedAnonymousOrdersPvl.RemoveAt(2);
        Assert.IsFalse(populatedAnonymousOrdersPvl[2].IsEmpty);
        Assert.AreEqual(10u, populatedAnonymousOrdersPvl.OrdersCount);

        Assert.AreEqual(OrdersCount, populatedCounterPartyOrdersPvl.OrdersCount);
        populatedCounterPartyOrdersPvl.RemoveAt(1);
        Assert.AreEqual(OrdersCount - 1, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.IsFalse(populatedCounterPartyOrdersPvl[1].IsEmpty);
        populatedCounterPartyOrdersPvl.RemoveAt(2);
        Assert.AreEqual(10u, populatedCounterPartyOrdersPvl.OrdersCount);
        Assert.IsFalse(populatedCounterPartyOrdersPvl[2].IsEmpty);
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
            var checkOrderLayer = populatedAnonymousOrdersPvl[i];
            Assert.AreNotEqual(0, checkOrderLayer.OrderId);
            Assert.AreNotEqual(OrderGenesisFlags.None, checkOrderLayer.GenesisFlags);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreNotEqual(0m, checkOrderLayer.OrderDisplayVolume);
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
            var checkOrderLayer = populatedAnonymousOrdersPvl[i];
            Assert.AreEqual(0, checkOrderLayer.OrderId);
            Assert.AreEqual(OrderGenesisFlags.None, checkOrderLayer.GenesisFlags);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreEqual(0m, checkOrderLayer.OrderDisplayVolume);
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
            var checkOrderLayer = populatedCounterPartyOrdersPvl[i];
            Assert.AreNotEqual(0, checkOrderLayer.OrderId);
            Assert.AreNotEqual(OrderGenesisFlags.None, checkOrderLayer.GenesisFlags);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreNotEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreNotEqual(0m, checkOrderLayer.OrderDisplayVolume);
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
            var checkOrderLayer = populatedCounterPartyOrdersPvl[i];
            Assert.AreEqual(0, checkOrderLayer.OrderId);
            Assert.AreEqual(IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, checkOrderLayer.GenesisFlags);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.CreatedTime);
            Assert.AreEqual(DateTime.MinValue, checkOrderLayer.UpdateTime);
            Assert.AreEqual(0m, checkOrderLayer.OrderDisplayVolume);
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
        var pqPvl    = new PQOrdersPriceVolumeLayer(populatedAnonymousOrdersPvl, LayerType.OrdersAnonymousPriceVolume, nameIdLookupGenerator);
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

        var opvl = populatedCounterPartyOrdersPvl;

        Assert.IsTrue(toString.Contains(opvl.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(opvl.Price)}: {opvl.Price:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(opvl.Volume)}: {opvl.Volume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(opvl.OrdersCount)}: {opvl.OrdersCount}"));
        Assert.IsTrue(toString.Contains($"{nameof(opvl.InternalVolume)}: {opvl.InternalVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(opvl.Orders)}: [\n{opvl.EachOrderByIndexOnNewLines()}]"));
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

    [TestMethod]
    public void PopulatedOrdersLayer_SmallerToLargerCalculateShifts_ShiftRightCommandsExpected()
    {
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        int[]        expectedIndices = [0, 2, 5, 9];
        IAnonymousOrder[] instances       = new IAnonymousOrder[10];

        int oldIndex    = 0;                   // original    0,1,2,3,4,5,6,7,8,9,10,11
        int actualIndex = 0;                   // deleted     1,3,4,6,7,8,10,11 
        var count       = toShift.Count;       // leaving     0,2,5,9
        for (var i = 0; oldIndex < count; i++) // shifts at   (2,3),(1,2)(0,1)
        {
            Console.Out.WriteLine($"Leaving index {oldIndex} with OrderId {toShift[actualIndex].OrderId}");
            instances[oldIndex] = toShift[actualIndex];
            oldIndex++;
            actualIndex++;
            for (var j = i + 1; j < 2 + 2 * i && oldIndex < count; j++)
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with OrderId {toShift[actualIndex].OrderId}");
                toShift.RemoveAt(actualIndex);
                oldIndex++;
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = populatedCounterPartyOrdersPvl.Clone();
        toShift.CalculateShift(testDateTime, shiftedNext);

        Assert.AreEqual(3, toShift.ShiftCommands.Count);
        AssertExpectedShiftCommands();

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < toShift.ShiftCommands.Count; i++)
            {
                var shift = toShift.ShiftCommands[i];
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(2, shift.PinnedFromIndex);
                        Assert.AreEqual(3, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(1, shift.PinnedFromIndex);
                        Assert.AreEqual(2, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 2:
                        Assert.AreEqual(0, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in toShift.ShiftCommands)
        {
            toShift.ApplyListShiftCommand(shiftElementShift);
        }
        foreach (var expectedIndex in expectedIndices)
        {
            Assert.AreEqual(populatedCounterPartyOrdersPvl[expectedIndex], toShift[expectedIndex]);
            Assert.AreSame(instances[expectedIndex], toShift[expectedIndex]);
        }
    }

    [TestMethod]
    public void PopulatedOrdersLayer_SmallerToLargerCalculateShiftsWithElementMovedToStart_ShiftRightCommandsExpected()
    {
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        int[]             expectedIndices = [0, 2, 4, 6, 7, 8, 10];
        IAnonymousOrder[] instances       = new IAnonymousOrder[11];


        int oldIndex    = 0;                   // original    0,1,2,3,4,5,6,7,8,9,10,11        
        int actualIndex = 0;                   // deleted     1,3,5,9,11         
        var count       = toShift.Count;       // leaving     7,0,2,4,6,8,10        
        for (var i = 0; oldIndex < count; i++) // shifts at   (6,1),(5,1),(4,1),(3,1)(2,1)(1,1)(0,1)
        {
            if (i % 2 == 1 && i != 7)
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with OrderId {toShift[actualIndex].OrderId}");
                toShift.RemoveAt(actualIndex);
                oldIndex++;
            }
            else if (i == 7)
            {
                Console.Out.WriteLine($"Moving index {oldIndex} with OrderId {toShift[actualIndex].OrderId} to Start");
                instances[oldIndex] = toShift[actualIndex];
                toShift.MoveToStart(actualIndex);
                oldIndex++;
                actualIndex++;
            }
            else
            {
                Console.Out.WriteLine($"Leaving index {oldIndex} with OrderId {toShift[actualIndex].OrderId}");
                instances[oldIndex] = toShift[actualIndex];
                oldIndex++;
                actualIndex++;
            }
        }

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = populatedCounterPartyOrdersPvl.Clone();
        toShift.CalculateShift(testDateTime, shiftedNext);

        Console.Out.WriteLine($"{toShift.ShiftCommands.JoinShiftCommandsOnNewLine()}");
        Assert.AreEqual(6, toShift.ShiftCommands.Count);
        AssertExpectedShiftCommands();

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < toShift.ShiftCommands.Count; i++)
            {
                var shift = toShift.ShiftCommands[i];
                Console.Out.WriteLine($"shift: {shift}");
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(5, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(4, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 2:
                        Assert.AreEqual(3, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 3:
                        Assert.AreEqual(2, shift.PinnedFromIndex);
                        Assert.AreEqual(1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsAwayFromPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 4:
                        Assert.AreEqual(1, shift.PinnedFromIndex);
                        Assert.AreEqual(0, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.MoveSingleElement | ListShiftCommandType.InsertElementsRange, shift.ShiftCommandType);
                        break;
                    case 5:
                        Assert.AreEqual(1, shift.PinnedFromIndex);
                        Assert.AreEqual(7, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.MoveSingleElement | ListShiftCommandType.InsertElementsRange, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in toShift.ShiftCommands)
        {
            toShift.ApplyListShiftCommand(shiftElementShift);
        }
        for (int i = 0; i < expectedIndices.Length; i++)
        {
            Assert.AreEqual(populatedCounterPartyOrdersPvl[expectedIndices[i]], toShift[expectedIndices[i]]);
            Assert.AreSame(instances[expectedIndices[i]], toShift[expectedIndices[i]]);
        }
    }

    [TestMethod]
    public void PopulatedOrdersLayer_LargerToSmallerCalculateShiftsWithNewEntryInMiddle_CalculateShiftLeftCommandsReturnsExpected()
    {
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var count       = toShift.Count;    // original    0,1,2,3,4,5,6,7,8,9,10,11
        int oldIndex    = count - 1;        // deleted     0,1,3,4,5,7,8,10                         
        int actualIndex = oldIndex;         // leaving     2,6,{new entry}, 9,11                     
        for (var i = 0; oldIndex >= 0; i++) // shifts at   (-1,-2),(0,-3), {new entry} ,(2,-1),(3,-1)
        {
            Console.Out.WriteLine($"Leaving index {oldIndex} with OrderId {toShift[actualIndex].OrderId}");
            oldIndex--;
            actualIndex--;
            for (var j = i + 1; j < 2 + 2 * i && oldIndex >= 0; j++)
            {
                Console.Out.WriteLine($"Deleting index {oldIndex} with OrderId {toShift[actualIndex].OrderId}");
                toShift.RemoveAt(actualIndex--);
                oldIndex--;
            }
        }

        var newEntry = exampleCounterPartyOrderLayer.Clone();

        toShift.InsertAt(2, newEntry);

        toShift.ShiftCommands = new List<ListShiftCommand>();

        var shiftedNext = populatedCounterPartyOrdersPvl.Clone();

        int[] expectedIndices = [0, 1, 3, 4];

        IAnonymousOrder[] instances = new IAnonymousOrder[5];

        instances[expectedIndices[0]] = shiftedNext[2];
        instances[expectedIndices[1]] = shiftedNext[6];
        instances[expectedIndices[2]] = shiftedNext[9];
        instances[expectedIndices[3]] = shiftedNext[11];
        shiftedNext.CalculateShift(testDateTime, toShift);

        Console.Out.WriteLine($"{shiftedNext.ShiftCommands.JoinShiftCommandsOnNewLine()}");

        AssertExpectedShiftCommands();
        Assert.AreEqual(4, shiftedNext.ShiftCommands.Count);

        void AssertExpectedShiftCommands()
        {
            for (int i = 0; i < shiftedNext.ShiftCommands.Count; i++)
            {
                var shift = shiftedNext.ShiftCommands[i];
                Console.Out.WriteLine($"shift: {shift}");
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(-1, shift.PinnedFromIndex);
                        Assert.AreEqual(-2, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 1:
                        Assert.AreEqual(0, shift.PinnedFromIndex);
                        Assert.AreEqual(-3, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 2:
                        Assert.AreEqual(2, shift.PinnedFromIndex);
                        Assert.AreEqual(-1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                    case 3:
                        Assert.AreEqual(3, shift.PinnedFromIndex);
                        Assert.AreEqual(-1, shift.Shift);
                        Assert.AreEqual(ListShiftCommandType.ShiftAllElementsTowardPinnedIndex, shift.ShiftCommandType);
                        break;
                }
            }
        }

        foreach (var shiftElementShift in shiftedNext.ShiftCommands)
        {
            shiftedNext.ApplyListShiftCommand(shiftElementShift);
        }

        foreach (var expectedIndex in expectedIndices)
        {
            Assert.AreEqual(toShift[expectedIndex], shiftedNext[expectedIndex]);
            Assert.AreSame(instances[expectedIndex], shiftedNext[expectedIndex]);
        }
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_ClearAfterMidElement_ListIsReducedByHalf()
    {
        var halfListSize = NumOfOrders / 2;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ClearRemainingElementsFromIndex = halfListSize;

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i]);
        }
        for (int i = halfListSize; i < populatedCounterPartyOrdersPvl.Count; i++)
        {
            Assert.IsTrue(toShift[i].IsEmpty);
        }
        Assert.AreEqual(populatedCounterPartyOrdersPvl.Count, toShift.Count + halfListSize);

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_InsertNewElementAtStart_RemainingElementsShiftRightByOne()
    {
        var newAnonOrder = exampleCounterPartyOrderLayer.Clone();

        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.InsertAtStart(newAnonOrder);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i + 1;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(0, toShift.IndexOf(newAnonOrder));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_DeleteMiddleElement_RemainingElementsShiftLeftByOne()
    {
        var midIndex = NumOfOrders / 2 + 1;

        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var middleElement = toShift[midIndex];

        toShift.DeleteAt(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i + 1;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(populatedCounterPartyOrdersPvl.Count, toShift.Count + 1);


        toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.Delete(middleElement);
        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i + 1;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(populatedCounterPartyOrdersPvl.Count, toShift.Count + 1);

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders - 1, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrdersLayer_InsertNewElementAtStart_RemainingElementsShiftRightExceptLastIsRemoved()
    {
        var newAnonOrder = exampleCounterPartyOrderLayer.Clone();

        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        Assert.AreEqual(NumOfOrders, toShift.Count);
        toShift.InsertAtStart(newAnonOrder);

        for (int i = 1; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i - 1;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(0, toShift.IndexOf(newAnonOrder));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_InsertNewElementAtEnd_NewElementAppearsAtTheEnd()
    {
        var newAnonOrder = exampleCounterPartyOrderLayer.Clone();

        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.AppendAtEnd(newAnonOrder);

        for (int i = 0; i < toShift.Count - 1; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders + 1, toShift.Count);
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(newAnonOrder));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders + 1, toShift.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachOrdersLayer_AttemptInsertNewElementAtEnd_ReturnsFalseAndNoElementIsAdded()
    {
        var newAnonOrder = exampleCounterPartyOrderLayer.Clone();

        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var result = toShift.AppendAtEnd(newAnonOrder);

        Assert.IsFalse(result);
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(populatedCounterPartyOrdersPvl[^1], toShift[^1]);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_MoveMiddleToStart_FormerMiddleElementIsAtStart()
    {
        var midIndex = NumOfOrders / 2 + 1;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var middleElement = toShift[midIndex];

        toShift.MoveToStart(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i == midIndex ? 0 : i < midIndex ? i + 1 : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(0, toShift.IndexOf(middleElement));

        toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveToStart(middleElement);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i == midIndex ? 0 : i < midIndex ? i + 1 : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(0, toShift.IndexOf(middleElement));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrdersLayer_MoveMiddleToStart_FormerMiddleElementIsAtStart()
    {
        var midIndex = NumOfOrders / 2 + 1;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var middleElement = toShift[midIndex];

        toShift.MoveToStart(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i == midIndex ? 0 : i < midIndex ? i + 1 : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(0, toShift.IndexOf(middleElement));

        toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveToStart(middleElement);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i == midIndex ? 0 : i < midIndex ? i + 1 : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(0, toShift.IndexOf(middleElement));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_MoveMiddleToEnd_FormerMiddleElementIsAtTheEnd()
    {
        var midIndex = NumOfOrders / 2 + 1;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var middleElement = toShift[midIndex];

        toShift.MoveToEnd(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i == toShift.Count - 1 ? midIndex : i + 1;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(middleElement));

        toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveToEnd(middleElement);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i == toShift.Count - 1 ? midIndex : i + 1;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(middleElement));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrdersLayer_MoveMiddleToEnd_FormerMiddleElementIsAtTheEnd()
    {
        var midIndex = NumOfOrders / 2 + 1;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var middleElement = toShift[midIndex];

        toShift.MoveToEnd(midIndex);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i == toShift.Count - 1 ? midIndex : i + 1;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(middleElement));

        toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveToEnd(middleElement);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i == toShift.Count - 1 ? midIndex : i + 1;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(toShift.Count - 1, toShift.IndexOf(middleElement));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_MoveMiddleRightByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = NumOfOrders / 2 + 1;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var middleElement = toShift[midIndex];
        var shiftAmount   = 2;

        toShift.MoveSingleElementBy(midIndex, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i < midIndex + shiftAmount ? i + 1 : i == midIndex + shiftAmount ? midIndex : i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveSingleElementBy(middleElement, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i < midIndex + shiftAmount ? i + 1 : i == midIndex + shiftAmount ? midIndex : i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrdersLayer_MoveMiddleRightByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = NumOfOrders / 2 + 1;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var middleElement = toShift[midIndex];
        var shiftAmount   = 2;

        toShift.MoveSingleElementBy(midIndex, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i < midIndex + shiftAmount ? i + 1 : i == midIndex + shiftAmount ? midIndex : i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveSingleElementBy(middleElement, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i;
            var prevIndex  = i < midIndex ? i : i < midIndex + shiftAmount ? i + 1 : i == midIndex + shiftAmount ? midIndex : i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_MoveMiddleLeftByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = NumOfOrders / 2 + 1;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var middleElement = toShift[midIndex];
        var shiftAmount   = -2;

        toShift.MoveSingleElementBy(midIndex, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i < midIndex + shiftAmount ? i : i < midIndex ? i + 1 : i == midIndex ? midIndex + shiftAmount : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveSingleElementBy(middleElement, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i < midIndex + shiftAmount ? i : i < midIndex ? i + 1 : i == midIndex ? midIndex + shiftAmount : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedRecentlyTraded_MoveMiddleLeftByTwoPlaces_FormerMiddleElementIsIndexPlus2()
    {
        var midIndex = NumOfOrders / 2 + 1;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        var middleElement = toShift[midIndex];
        var shiftAmount   = -2;

        toShift.MoveSingleElementBy(midIndex, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i < midIndex + shiftAmount ? i : i < midIndex ? i + 1 : i == midIndex ? midIndex + shiftAmount : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        middleElement = toShift[midIndex];

        toShift.MoveSingleElementBy(middleElement, shiftAmount);

        for (int i = 0; i < toShift.Count; i++)
        {
            var shiftIndex = i < midIndex + shiftAmount ? i : i < midIndex ? i + 1 : i == midIndex ? midIndex + shiftAmount : i;
            var prevIndex  = i;
            Assert.AreEqual(toShift[shiftIndex], populatedCounterPartyOrdersPvl[prevIndex]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(midIndex + shiftAmount, toShift.IndexOf(middleElement));

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_ShiftLeftFromMiddleByOne_DeletesEntryFirstEntryCreatesEmptyOneBelowPinIndex()
    {
        var pinAt = NumOfOrders / 2 + 1;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsFrom(-1, pinAt);

        for (int i = 0; i < pinAt - 1; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i + 1]);
        }
        Assert.IsTrue(toShift[pinAt - 1].IsEmpty);
        for (int i = pinAt; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i]);
        }

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrdersLayer_ShiftLeftFromMiddleByOne_DeletesEntryFirstEntryCreatesEmptyOneBelowPinIndex()
    {
        var pinAt = NumOfOrders / 2 + 1;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsFrom(-1, pinAt);

        for (int i = 0; i < pinAt - 1; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i + 1]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.IsTrue(toShift[pinAt - 1].IsEmpty);
        for (int i = pinAt; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i]);
        }

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_ShiftRightFromMiddleByOne_CreatesEmptyOneAbovePinIndexAndExtendsList()
    {
        var pinAt = NumOfOrders / 2 - 1;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsFrom(1, pinAt);

        for (int i = pinAt + 2; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i - 1]);
        }
        Assert.IsTrue(toShift[pinAt + 1].IsEmpty);
        for (int i = 0; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i]);
        }

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrdersLayer_ShiftRightFromMiddleByOne_CreatesEmptyOneAbovePinIndexAndDeletesLastEntry()
    {
        var pinAt = NumOfOrders / 2 - 1;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsFrom(1, pinAt);

        for (int i = pinAt + 2; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i - 1]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.IsTrue(toShift[pinAt + 1].IsEmpty);
        for (int i = 0; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i]);
        }

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayers_ShiftLeftTowardMiddleByOne_DeletesPreMiddleEntryCreatesEmptyAtEnd()
    {
        var pinAt = NumOfOrders / 2 + 1;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsUntil(-1, pinAt);

        for (int i = pinAt + 1; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i + 1]);
        }
        Assert.AreEqual(NumOfOrders - 1, toShift.Count);
        Assert.IsTrue(toShift[populatedCounterPartyOrdersPvl.Count - 1].IsEmpty);
        for (int i = 0; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i]);
        }

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders - 1, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedCacheMaxAllowedSizeReachedOrdersLayer_ShiftLeftTowardMiddleByOne_DeletesPreMiddleEntryCreatesEmptyAtEnd()
    {
        var pinAt = NumOfOrders / 2 + 1;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsUntil(-1, pinAt);

        for (int i = pinAt + 1; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i + 1]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count + 1);
        Assert.IsTrue(toShift[populatedCounterPartyOrdersPvl.Count - 1].IsEmpty);
        for (int i = 0; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i]);
        }

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders, shiftCopyFrom.Count + 1);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_ShiftRightTowardMiddleByOne_CreatesEmptyAtStartDeletesPreMiddleEntry()
    {
        var pinAt = NumOfOrders / 2 - 1;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsUntil(1, pinAt);

        for (int i = 1; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i - 1]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.IsTrue(toShift[0].IsEmpty);
        for (int i = pinAt; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i]);
        }

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrdersLayer_ShiftRightTowardMiddleByOne_CreatesEmptyAtStartDeletesPreMiddleEntry()
    {
        var pinAt = NumOfOrders / 2 - 1;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsUntil(1, pinAt);

        for (int i = 1; i < pinAt; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i - 1]);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.IsTrue(toShift[0].IsEmpty);
        for (int i = pinAt; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i]);
        }

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_ShiftLeftFromEndByHalfListSize_CreatesEmptyAtEndAndShortensListByHalf()
    {
        var halfListSize = NumOfOrders / 2;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsFrom(-halfListSize, short.MaxValue);

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i + halfListSize]);
        }
        Assert.AreEqual(halfListSize, toShift.Count);

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrdersLayer_ShiftLeftFromEndByHalfListSize_CreatesEmptyAtEndAndShortensListByHalf()
    {
        var halfListSize = NumOfOrders / 2;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsFrom(-halfListSize, short.MaxValue);

        for (int i = 0; i < halfListSize; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i + halfListSize]);
        }
        Assert.AreEqual(halfListSize, toShift.Count);
        Assert.AreEqual(populatedCounterPartyOrdersPvl.Count, toShift.Count + halfListSize);

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedNonMaxAllowedSizeOrdersLayer_ShiftRightFromStart_CreatesEmptyAtStartAndExtendsListByHalf()
    {
        var halfListSize = NumOfOrders / 2;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsFrom(halfListSize, short.MinValue);

        for (int i = halfListSize; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i - halfListSize]);
        }
        Assert.AreEqual(NumOfOrders + halfListSize, toShift.Count);
        for (int i = 0; i < halfListSize; i++)
        {
            Assert.IsTrue(toShift[i].IsEmpty);
        }
        Assert.AreEqual(populatedCounterPartyOrdersPvl.Count, toShift.Count - halfListSize);

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders + halfListSize, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }

    [TestMethod]
    public void PopulatedMaxAllowedSizeReachedOrdersLayer_ShiftRightFromStartByHalfList_CreatesEmptyItemsAtStartAndTruncatesLastHalfOfOrders()
    {
        var halfListSize = NumOfOrders / 2;
        populatedCounterPartyOrdersPvl.MaxAllowedSize = NumOfOrders;
        var toShift = populatedCounterPartyOrdersPvl.Clone();
        Assert.AreEqual(populatedCounterPartyOrdersPvl, toShift);

        toShift.ShiftElementsFrom(halfListSize, short.MinValue);

        for (int i = halfListSize; i < toShift.Count; i++)
        {
            Assert.AreEqual(toShift[i], populatedCounterPartyOrdersPvl[i - halfListSize]);
        }
        for (int i = 0; i < halfListSize; i++)
        {
            Assert.IsTrue(toShift[i].IsEmpty);
        }
        Assert.AreEqual(NumOfOrders, toShift.Count);
        Assert.AreEqual(populatedCounterPartyOrdersPvl.Count, toShift.Count);

        var shiftCopyFrom = populatedCounterPartyOrdersPvl.Clone();
        shiftCopyFrom.CopyFrom(toShift);
        Assert.AreEqual(NumOfOrders, shiftCopyFrom.Count);
        Assert.AreEqual(toShift, shiftCopyFrom);
    }


    public static void AddAnonymousOrders
        (IMutableOrdersPriceVolumeLayer addOrdersLayers, int numOrdersToCreate)
    {
        var createdTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
        var updatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);
        for (var i = 0; i < numOrdersToCreate; i++)
        {
            var anonOrderLayer = addOrdersLayers[i];
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
            var anonOrderLayer = addOrdersLayers[i];
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
                var checkOrderLayer = checkOrdersLayers[i];

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
                var checkOrderLayer = checkOrdersLayers[i];
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
