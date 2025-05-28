// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

[TestClass]
public class ValueDatePriceVolumeLayerTests
{
    private ValueDatePriceVolumeLayer emptyPvl     = null!;
    private ValueDatePriceVolumeLayer populatedPvl = null!;

    private DateTime wellKnownDate;

    [TestInitialize]
    public void SetUp()
    {
        wellKnownDate = new DateTime(2018, 2, 10, 17, 0, 0);

        emptyPvl     = new ValueDatePriceVolumeLayer();
        populatedPvl = new ValueDatePriceVolumeLayer(4.2949_672m, 42_949_672m, wellKnownDate);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new ValueDatePriceVolumeLayer(20, 40_000_000, wellKnownDate);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        Assert.AreEqual(wellKnownDate, newPvl.ValueDate);

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, emptyPvl.ValueDate);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl      = new ValueDatePriceVolumeLayer(20, 40_000_000, wellKnownDate);
        var fromSrcQtRefInstance = new ValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromSrcQtRefInstance.Price);
        Assert.AreEqual(40_000_000m, fromSrcQtRefInstance.Volume);
        Assert.AreEqual(wellKnownDate, fromSrcQtRefInstance.ValueDate);

        var nonPQValueDatePvl = new PQValueDatePriceVolumeLayer(1.23456m, 5_123_456m, wellKnownDate);
        var fromNonPqInstance = new ValueDatePriceVolumeLayer(nonPQValueDatePvl);
        Assert.AreEqual(1.23456m, fromNonPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromNonPqInstance.Volume);
        Assert.AreEqual(wellKnownDate, fromNonPqInstance.ValueDate);

        var fromNonSourceQtRefPriceVolumeLayer = new ValueDatePriceVolumeLayer
            (new PriceVolumeLayer(20, 40_000_000));
        Assert.AreEqual(20, fromNonSourceQtRefPriceVolumeLayer.Price);
        Assert.AreEqual(40_000_000, fromNonSourceQtRefPriceVolumeLayer.Volume);
        Assert.AreEqual(DateTime.MinValue, fromNonSourceQtRefPriceVolumeLayer.ValueDate);

        var newEmptyPvl = new ValueDatePriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, newEmptyPvl.ValueDate);
    }

    [TestMethod]
    public void EmptyLayer_Mutate_UpdatesFields()
    {
        const decimal expectedPrice  = 3.45678m;
        const decimal expectedVolume = 5.67890m;

        var expectedValueDate = new DateTime(2018, 2, 10, 18, 0, 0);

        emptyPvl.Price     = expectedPrice;
        emptyPvl.Volume    = expectedVolume;
        emptyPvl.ValueDate = expectedValueDate;

        Assert.AreEqual(expectedPrice, emptyPvl.Price);
        Assert.AreEqual(expectedVolume, emptyPvl.Volume);
        Assert.AreEqual(expectedValueDate, emptyPvl.ValueDate);
    }

    [TestMethod]
    public void PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedPvl.Price);
        Assert.AreNotEqual(0m, populatedPvl.Volume);
        Assert.AreNotEqual(DateTime.MinValue, populatedPvl.ValueDate);
        populatedPvl.StateReset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, populatedPvl.ValueDate);
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
        var pqPvl    = new PQValueDatePriceVolumeLayer(populatedPvl);
        var newEmpty = new ValueDatePriceVolumeLayer();
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
        clone = ((ICloneable<IValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((IValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((ICloneable<IMutableValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((IMutableValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableValueDatePriceVolumeLayer)populatedPvl).Clone();
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.ValueDate)}: {populatedPvl.ValueDate}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableValueDatePriceVolumeLayer? original, IMutableValueDatePriceVolumeLayer? changingPriceVolumeLayer,
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

        PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide, changingOrderBookSide, 
             originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.ValueDate = new DateTime(2017, 11, 30, 23, 44, 16);
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsFalse
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.ValueDate = original.ValueDate;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
