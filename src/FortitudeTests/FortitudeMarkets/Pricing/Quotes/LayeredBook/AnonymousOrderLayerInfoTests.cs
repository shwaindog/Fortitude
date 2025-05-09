// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerOrders;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

[TestClass]
public class AnonymousOrderLayerInfoTests
{
    private const    int             OrderNumber          = 80085;
    private const    LayerOrderFlags OrderFlags           = LayerOrderFlags.ExplicitlyDefinedFromSource | LayerOrderFlags.IsInternallyCreatedOrder;
    private const    decimal         OrderVolume          = 100_000.50m;
    private const    decimal         OrderRemainingVolume = 50_000.25m;
    private readonly DateTime        CreatedTime          = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private readonly DateTime        UpdatedTime          = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private AnonymousOrderLayerInfo emptyAoli     = null!;
    private AnonymousOrderLayerInfo populatedAoli = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyAoli     = new AnonymousOrderLayerInfo();
        populatedAoli = new AnonymousOrderLayerInfo(OrderNumber, OrderFlags, CreatedTime, OrderVolume, UpdatedTime, OrderRemainingVolume);
    }

    [TestMethod]
    public void NewAoli_SetsValues_PropertiesInitializedAsExpected()
    {
        var newAoli = new AnonymousOrderLayerInfo(OrderNumber, OrderFlags, CreatedTime, OrderVolume);
        Assert.AreEqual(OrderNumber, newAoli.OrderId);
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
    public void NewAoli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var fromInstance = new AnonymousOrderLayerInfo(populatedAoli);
        Assert.AreEqual(populatedAoli.OrderId, fromInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderFlags, fromInstance.OrderFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdatedTime, fromInstance.UpdatedTime);
        Assert.AreEqual(populatedAoli.OrderVolume, fromInstance.OrderVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromInstance.OrderRemainingVolume);

        var fromEmptyAoli = new AnonymousOrderLayerInfo(emptyAoli);
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);

        var pqInstance     = new PQAnonymousOrderLayerInfo(populatedAoli);
        var fromPQInstance = new PQAnonymousOrderLayerInfo(pqInstance);
        Assert.AreEqual(populatedAoli.OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(populatedAoli.OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);

        var newPqEmptyAoli  = new PQAnonymousOrderLayerInfo(emptyAoli);
        var fromPqEmptyAoli = new PQAnonymousOrderLayerInfo(newPqEmptyAoli);
        Assert.AreEqual(0, fromPqEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromPqEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromPqEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromPqEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromPqEmptyAoli.OrderRemainingVolume);
    }

    [TestMethod]
    public void EmptyAndPopulatedAoli_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.IsEmpty);
    }

    [TestMethod]
    public void PopulatedAoli_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedAoli.IsEmpty);
        Assert.AreNotEqual(0, populatedAoli.OrderId);
        Assert.AreNotEqual(LayerOrderFlags.None, populatedAoli.OrderFlags);
        Assert.AreNotEqual(DateTime.MinValue, populatedAoli.CreatedTime);
        Assert.AreNotEqual(DateTime.MinValue, populatedAoli.UpdatedTime);
        Assert.AreNotEqual(0m, populatedAoli.OrderVolume);
        Assert.AreNotEqual(0m, populatedAoli.OrderRemainingVolume);
        Assert.IsFalse(populatedAoli.IsEmpty);
        populatedAoli.StateReset();
        Assert.AreEqual(0, populatedAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, populatedAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.UpdatedTime);
        Assert.AreEqual(0m, populatedAoli.OrderVolume);
        Assert.AreEqual(0m, populatedAoli.OrderRemainingVolume);
    }

    [TestMethod]
    public void FullyPopulatedAoli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        emptyAoli.CopyFrom(populatedAoli);
        Assert.AreEqual(populatedAoli, emptyAoli);
        emptyAoli = new AnonymousOrderLayerInfo();
        var nonPqInstance = new AnonymousOrderLayerInfo(populatedAoli);
        Assert.AreEqual(populatedAoli, nonPqInstance);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)populatedAoli).Clone();
        Assert.AreNotSame(clone, populatedAoli);
        Assert.AreEqual(populatedAoli, clone);
        clone = ((IMutableAnonymousOrderLayerInfo)populatedAoli).Clone();
        Assert.AreNotSame(clone, populatedAoli);
        Assert.AreEqual(populatedAoli, clone);
        clone = ((IAnonymousOrderLayerInfo)populatedAoli).Clone();
        Assert.AreNotSame(clone, populatedAoli);
        Assert.AreEqual(populatedAoli, clone);
    }

    [TestMethod]
    public void FullyPopulatedAoliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableAnonymousOrderLayerInfo)populatedAoli).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedAoli, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedAoli_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedAoli.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedAoli.ToString();

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
        IMutableAnonymousOrderLayerInfo? original,
        IMutableAnonymousOrderLayerInfo? changingTraderLayerInfo,
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

        changingTraderLayerInfo.OrderId = 73423;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.OrderId = original.OrderId;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.OrderFlags = LayerOrderFlags.ImpliedCreatedByAdapter | LayerOrderFlags.IsSyntheticTrackingOrder;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.OrderFlags = original.OrderFlags;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.CreatedTime = DateTime.Now;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.CreatedTime = original.CreatedTime;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.UpdatedTime = DateTime.Now;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.UpdatedTime = original.UpdatedTime;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.OrderVolume = 8_765_432;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.OrderVolume = original.OrderVolume;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.OrderRemainingVolume = 3_219_870;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.OrderRemainingVolume = original.OrderRemainingVolume;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
