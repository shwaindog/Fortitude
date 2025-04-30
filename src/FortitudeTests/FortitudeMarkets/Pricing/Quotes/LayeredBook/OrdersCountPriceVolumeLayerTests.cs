// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

[TestClass]
public class OrdersCountPriceVolumeLayerTests
{
    private const decimal Price          = 1.3456m;
    private const decimal Volume         = 100_000m;
    private const uint    OrdersCount    = 732;
    private const decimal InternalVolume = 50_000m;

    private OrdersCountPriceVolumeLayer emptyPvl     = null!;
    private OrdersCountPriceVolumeLayer populatedPvl = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyPvl     = new OrdersCountPriceVolumeLayer();
        populatedPvl = new OrdersCountPriceVolumeLayer(Price, Volume, OrdersCount, InternalVolume);
    }

    [TestMethod]
    public void NewPvl_SetsValues_PropertiesInitializedAsExpected()
    {
        var newPvl = new OrdersCountPriceVolumeLayer(Price, Volume, OrdersCount, InternalVolume);
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
        var newPopulatedPvl = new OrdersCountPriceVolumeLayer(populatedPvl);
        Assert.AreEqual(Price, newPopulatedPvl.Price);
        Assert.AreEqual(Volume, newPopulatedPvl.Volume);
        Assert.AreEqual(OrdersCount, newPopulatedPvl.OrdersCount);
        Assert.AreEqual(InternalVolume, newPopulatedPvl.InternalVolume);

        var pqTraderPvl    = new PQOrdersCountPriceVolumeLayer(populatedPvl);
        var fromPqInstance = new OrdersCountPriceVolumeLayer(pqTraderPvl);
        Assert.AreEqual(Price, fromPqInstance.Price);
        Assert.AreEqual(Volume, fromPqInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPqInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPqInstance.InternalVolume);

        var newEmptyPvl = new PQOrdersCountPriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(0u, newEmptyPvl.OrdersCount);
        Assert.AreEqual(0m, newEmptyPvl.InternalVolume);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsOrdersCountUpdated);
        Assert.IsFalse(newEmptyPvl.IsInternalVolumeUpdated);
    }


    [TestMethod]
    public void EmptyPvl_SetOrdersCountOnly_UpdatesCount()
    {
        Assert.AreEqual(0u, emptyPvl.OrdersCount);
        Assert.IsTrue(emptyPvl.IsEmpty);
        emptyPvl.OrdersCount = OrdersCount;
        Assert.AreEqual(OrdersCount, emptyPvl.OrdersCount);
        Assert.IsFalse(emptyPvl.IsEmpty);
    }


    [TestMethod]
    public void EmptyPvl_SetInternalVolumeOnly_UpdatesInternalVolume()
    {
        Assert.AreEqual(0, emptyPvl.InternalVolume);
        Assert.IsTrue(emptyPvl.IsEmpty);
        emptyPvl.InternalVolume = InternalVolume;
        Assert.AreEqual(InternalVolume, emptyPvl.InternalVolume);
        Assert.IsFalse(emptyPvl.IsEmpty);
    }

    [TestMethod]
    public void EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedPvl.Price);
        Assert.AreNotEqual(0m, populatedPvl.Volume);
        Assert.AreNotEqual(0u, populatedPvl.OrdersCount);
        Assert.AreNotEqual(0m, populatedPvl.InternalVolume);
        Assert.IsFalse(populatedPvl.IsEmpty);
        populatedPvl.StateReset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(0u, populatedPvl.OrdersCount);
        Assert.AreEqual(0m, populatedPvl.InternalVolume);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther()
    {
        emptyPvl.CopyFrom(populatedPvl);
        Assert.AreEqual(populatedPvl, emptyPvl);
    }

    [TestMethod]
    public void PQPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther()
    {
        var pqPvl    = new PQOrdersCountPriceVolumeLayer(populatedPvl);
        var newEmpty = new OrdersCountPriceVolumeLayer();
        newEmpty.CopyFrom(pqPvl);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        IPriceVolumeLayer clone = populatedPvl.Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((IPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((ICloneable<IOrdersCountPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((IOrdersCountPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((IMutableOrdersCountPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableOrdersCountPriceVolumeLayer)populatedPvl).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedPvl, fullyPopulatedClone);
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.OrdersCount)}: {populatedPvl.OrdersCount}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.InternalVolume)}: {populatedPvl.InternalVolume:N2}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableOrdersCountPriceVolumeLayer? original,
        IMutableOrdersCountPriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original!);
        Assert.IsNotNull(changingPriceVolumeLayer!);

        if (original.GetType() == typeof(OrdersCountPriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new PQOrdersCountPriceVolumeLayer(original), exactComparison));

        PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        if (changingPriceVolumeLayer is IOrdersPriceVolumeLayer { OrdersCount: 0 })
        {
            changingPriceVolumeLayer.OrdersCount = 73423;
            Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
            if (originalOrderBook != null)
                Assert.IsFalse
                    (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
            changingPriceVolumeLayer.OrdersCount = original.OrdersCount;
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
            if (originalOrderBook != null)
                Assert.IsTrue
                    (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

            changingPriceVolumeLayer.InternalVolume = 777_666m;
            Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
            if (originalOrderBook != null)
                Assert.IsFalse
                    (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
            changingPriceVolumeLayer.InternalVolume = original.InternalVolume;
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
            if (changingPriceVolumeLayer != null)
                Assert.IsTrue
                    (original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
            if (originalOrderBook != null)
                Assert.IsTrue
                    (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
        }
    }
}
