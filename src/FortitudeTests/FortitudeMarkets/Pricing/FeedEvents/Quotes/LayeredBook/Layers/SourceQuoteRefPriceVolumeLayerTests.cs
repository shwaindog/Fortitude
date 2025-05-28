// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

[TestClass]
public class SourceQuoteRefPriceVolumeLayerTests
{
    private SourceQuoteRefPriceVolumeLayer emptyPvl              = null!;
    private IPQNameIdLookupGenerator       nameIdLookupGenerator = null!;
    private SourceQuoteRefPriceVolumeLayer populatedPvl          = null!;

    private uint wellKnownQuoteRefNum;

    private string wellKnownSourceName = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        wellKnownSourceName  = "TestSourceName";
        wellKnownQuoteRefNum = 54416884u;

        emptyPvl = new SourceQuoteRefPriceVolumeLayer();
        populatedPvl = new SourceQuoteRefPriceVolumeLayer
            (4.2949_672m, 42_949_672m, wellKnownSourceName, true, wellKnownQuoteRefNum);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new SourceQuoteRefPriceVolumeLayer
            (20, 40_000_000, wellKnownSourceName, true, wellKnownQuoteRefNum);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        Assert.AreEqual(wellKnownSourceName, newPvl.SourceName);
        Assert.IsTrue(newPvl.Executable);
        Assert.AreEqual(wellKnownQuoteRefNum, newPvl.SourceQuoteReference);

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.IsFalse(emptyPvl.Executable);
        Assert.AreEqual(0u, emptyPvl.SourceQuoteReference);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new SourceQuoteRefPriceVolumeLayer
            (20, 40_000_000, wellKnownSourceName, true, wellKnownQuoteRefNum);
        var fromSrcQtRefInstance = new SourceQuoteRefPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromSrcQtRefInstance.Price);
        Assert.AreEqual(40_000_000m, fromSrcQtRefInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromSrcQtRefInstance.SourceName);
        Assert.IsTrue(fromSrcQtRefInstance.Executable);
        Assert.AreEqual(wellKnownQuoteRefNum, fromSrcQtRefInstance.SourceQuoteReference);

        var pqValueDatePvl = new PQSourceQuoteRefPriceVolumeLayer
            (nameIdLookupGenerator.Clone(), 1.23456m, 5_123_456m, wellKnownSourceName, true, wellKnownQuoteRefNum);
        var fromNonPqInstance = new SourceQuoteRefPriceVolumeLayer(pqValueDatePvl);
        Assert.AreEqual(1.23456m, fromNonPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromNonPqInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromNonPqInstance.SourceName);
        Assert.IsTrue(fromNonPqInstance.Executable);
        Assert.AreEqual(wellKnownQuoteRefNum, fromNonPqInstance.SourceQuoteReference);

        var fromNonSourceQtRefPriceVolumeLayer = new SourceQuoteRefPriceVolumeLayer(new PriceVolumeLayer(20, 40_000_000));
        Assert.AreEqual(20, fromNonSourceQtRefPriceVolumeLayer.Price);
        Assert.AreEqual(40_000_000, fromNonSourceQtRefPriceVolumeLayer.Volume);
        Assert.AreEqual(null, fromNonSourceQtRefPriceVolumeLayer.SourceName);
        Assert.IsFalse(fromNonSourceQtRefPriceVolumeLayer.Executable);
        Assert.AreEqual(0u, fromNonSourceQtRefPriceVolumeLayer.SourceQuoteReference);

        var newEmptyPvl = new SourceQuoteRefPriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
    }

    [TestMethod]
    public void EmptyLayer_Mutate_UpdatesFields()
    {
        const decimal expectedPrice       = 3.45678m;
        const decimal expectedVolume      = 5.67890m;
        const string  expectedSourceName  = "NewSourceName";
        const uint    expectedSrcQuoteRef = 612656;

        emptyPvl.Price      = expectedPrice;
        emptyPvl.Volume     = expectedVolume;
        emptyPvl.SourceName = expectedSourceName;
        emptyPvl.Executable = true;

        emptyPvl.SourceQuoteReference = expectedSrcQuoteRef;

        Assert.AreEqual(expectedPrice, emptyPvl.Price);
        Assert.AreEqual(expectedVolume, emptyPvl.Volume);
        Assert.AreEqual(expectedSourceName, emptyPvl.SourceName);
        Assert.IsTrue(emptyPvl.Executable);
        Assert.AreEqual(expectedSrcQuoteRef, emptyPvl.SourceQuoteReference);
    }

    [TestMethod]
    public void PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedPvl.Price);
        Assert.AreNotEqual(0m, populatedPvl.Volume);
        Assert.IsNotNull(populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.AreNotEqual(0u, populatedPvl.SourceQuoteReference);
        populatedPvl.StateReset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.IsNull(populatedPvl.SourceName);
        Assert.IsFalse(populatedPvl.Executable);
        Assert.AreEqual(0u, populatedPvl.SourceQuoteReference);
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
        var pqPvl    = new PQSourceQuoteRefPriceVolumeLayer(populatedPvl, nameIdLookupGenerator);
        var newEmpty = new SourceQuoteRefPriceVolumeLayer();
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
        clone = ((ICloneable<ISourcePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((IMutableSourcePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((ISourcePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((IMutableSourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((ICloneable<ISourceQuoteRefPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((ISourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableSourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.SourceName)}: {populatedPvl.SourceName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Executable)}: {populatedPvl.Executable}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.SourceQuoteReference)}: {populatedPvl.SourceQuoteReference:N0}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableSourceQuoteRefPriceVolumeLayer? original,
        IMutableSourceQuoteRefPriceVolumeLayer? changingPriceVolumeLayer,
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

        SourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide, changingOrderBookSide, 
             originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.SourceQuoteReference = 7654321;
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsFalse
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.SourceQuoteReference = original.SourceQuoteReference;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
