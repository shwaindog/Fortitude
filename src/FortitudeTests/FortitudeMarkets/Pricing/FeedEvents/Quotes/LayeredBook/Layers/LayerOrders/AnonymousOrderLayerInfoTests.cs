// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

[TestClass]
public class AnonymousOrderLayerInfoTests
{
    private const    int             OrderNumber          = 80085;
    private const    decimal         OrderVolume          = 100_000.50m;
    private const    decimal         OrderRemainingVolume = 50_000.25m;

    private const OrderFlags      ExpectedTypeFlags  = OrderFlags.FromAdapter;
    private const OrderType       ExpectedOrderType  = OrderType.PassiveLimit;
    private const LayerOrderFlags ExpectedLayerFlags = LayerOrderFlags.ExplicitlyDefinedFromSource | LayerOrderFlags.IsInternallyCreatedOrder;

    private const OrderLifeCycleState ExpectedLifecycleState = OrderLifeCycleState.SourceActiveOnMarket;

    private static readonly DateTime        CreatedTime          = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime        UpdatedTime          = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private AnonymousOrderLayerInfo emptyAoli     = null!;
    private AnonymousOrderLayerInfo populatedAoli = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyAoli     = new AnonymousOrderLayerInfo();
        populatedAoli = new AnonymousOrderLayerInfo(OrderNumber, CreatedTime, OrderVolume, ExpectedLayerFlags, ExpectedOrderType, ExpectedTypeFlags
                                                  , ExpectedLifecycleState, UpdatedTime, OrderRemainingVolume);
    }

    [TestMethod]
    public void NewAoli_SetsValues_PropertiesInitializedAsExpected()
    {
        var newAoli = new AnonymousOrderLayerInfo(OrderNumber, CreatedTime, OrderVolume, ExpectedLayerFlags);
        Assert.AreEqual(OrderNumber, newAoli.OrderId);
        Assert.AreEqual(ExpectedLayerFlags, newAoli.OrderLayerFlags);
        Assert.AreEqual(CreatedTime, newAoli.CreatedTime);
        Assert.AreEqual(CreatedTime, newAoli.UpdateTime);
        Assert.AreEqual(OrderVolume, newAoli.OrderDisplayVolume);
        Assert.AreEqual(OrderVolume, newAoli.OrderRemainingVolume);

        Assert.AreEqual(0, emptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, emptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, emptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, emptyAoli.UpdateTime);
        Assert.AreEqual(0m, emptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, emptyAoli.OrderRemainingVolume);
    }

    [TestMethod]
    public void NewAoli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var fromInstance = new AnonymousOrderLayerInfo(populatedAoli);
        Assert.AreEqual(populatedAoli.OrderId, fromInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderLayerFlags, fromInstance.OrderLayerFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdateTime, fromInstance.UpdateTime);
        Assert.AreEqual(populatedAoli.OrderDisplayVolume, fromInstance.OrderDisplayVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromInstance.OrderRemainingVolume);

        var fromEmptyAoli = new AnonymousOrderLayerInfo(emptyAoli);
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromEmptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderDisplayVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);

        var pqInstance     = new PQAnonymousOrderLayerInfo(populatedAoli);
        var fromPQInstance = new PQAnonymousOrderLayerInfo(pqInstance);
        Assert.AreEqual(populatedAoli.OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderLayerFlags, fromPQInstance.OrderLayerFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(populatedAoli.OrderDisplayVolume, fromPQInstance.OrderDisplayVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);

        var newPqEmptyAoli  = new PQAnonymousOrderLayerInfo(emptyAoli);
        var fromPqEmptyAoli = new PQAnonymousOrderLayerInfo(newPqEmptyAoli);
        Assert.AreEqual(0, fromPqEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromPqEmptyAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, fromPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromPqEmptyAoli.UpdateTime);
        Assert.AreEqual(0m, fromPqEmptyAoli.OrderDisplayVolume);
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
        Assert.AreNotEqual(LayerOrderFlags.None, populatedAoli.OrderLayerFlags);
        Assert.AreNotEqual(DateTime.MinValue, populatedAoli.CreatedTime);
        Assert.AreNotEqual(DateTime.MinValue, populatedAoli.UpdateTime);
        Assert.AreNotEqual(0m, populatedAoli.OrderDisplayVolume);
        Assert.AreNotEqual(0m, populatedAoli.OrderRemainingVolume);
        Assert.IsFalse(populatedAoli.IsEmpty);
        populatedAoli.StateReset();
        Assert.AreEqual(0, populatedAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, populatedAoli.OrderLayerFlags);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.UpdateTime);
        Assert.AreEqual(0m, populatedAoli.OrderDisplayVolume);
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderLayerFlags)}: {populatedAoli.OrderLayerFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.CreatedTime)}: {populatedAoli.CreatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.UpdateTime)}: {populatedAoli.UpdateTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderDisplayVolume)}: {populatedAoli.OrderDisplayVolume:N2}"));
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
        IPublishableLevel2Quote? originalQuote = null,
        IPublishableLevel2Quote? changingQuote = null)
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

        changingTraderLayerInfo.OrderLayerFlags = LayerOrderFlags.TrackedOnAdapter | LayerOrderFlags.IsSyntheticTrackingOrder;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.OrderLayerFlags = original.OrderLayerFlags;
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

        changingTraderLayerInfo.UpdateTime = DateTime.Now;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.UpdateTime = original.UpdateTime;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.OrderDisplayVolume = 8_765_432;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.OrderDisplayVolume = original.OrderDisplayVolume;
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
