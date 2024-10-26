// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

[TestClass]
public class TraderLayerInfoTests
{
    private const string TraderName = "TestTraderName";

    private TraderLayerInfo emptyTli = null!;

    private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;

    private TraderLayerInfo populatedTli = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);

        emptyTli     = new TraderLayerInfo();
        populatedTli = new TraderLayerInfo(TraderName, 42_111_222m);
    }

    [TestMethod]
    public void NewTli_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newExpectedTraderName = "DiffTraderName";
        var newTli                = new TraderLayerInfo(newExpectedTraderName, 2_333_440m);
        Assert.AreEqual(newExpectedTraderName, newTli.TraderName);
        Assert.AreEqual(2_333_440m, newTli.TraderVolume);

        Assert.IsNull(emptyTli.TraderName);
        Assert.AreEqual(0m, emptyTli.TraderVolume);
    }

    [TestMethod]
    public void NewTli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var fromPQInstance = new TraderLayerInfo(populatedTli);
        Assert.AreEqual(populatedTli.TraderName, fromPQInstance.TraderName);
        Assert.AreEqual(populatedTli.TraderVolume, fromPQInstance.TraderVolume);

        var newExpectedTraderName = "NonPQTraderName";
        var pqTli                 = new PQTraderLayerInfo(nameIdLookupGenerator.Clone(), newExpectedTraderName, 222_444);
        var fromNonPqInstance     = new TraderLayerInfo(pqTli);
        Assert.AreEqual(newExpectedTraderName, fromNonPqInstance.TraderName);
        Assert.AreEqual(222_444, fromNonPqInstance.TraderVolume);

        var newEmptyTli = new TraderLayerInfo(emptyTli);
        Assert.IsNull(newEmptyTli.TraderName);
        Assert.AreEqual(0, newEmptyTli.TraderVolume);
    }

    [TestMethod]
    public void EmptyAndPopulatedTli_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedTli.IsEmpty);
        Assert.IsTrue(emptyTli.IsEmpty);
    }

    [TestMethod]
    public void PopulatedTli_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedTli.IsEmpty);
        Assert.IsNotNull(populatedTli.TraderName);
        Assert.AreNotEqual(0m, populatedTli.TraderVolume);
        Assert.IsFalse(populatedTli.IsEmpty);
        populatedTli.StateReset();
        Assert.IsTrue(populatedTli.IsEmpty);
        Assert.IsNull(populatedTli.TraderName);
        Assert.AreEqual(0m, populatedTli.TraderVolume);
        Assert.IsTrue(populatedTli.IsEmpty);
    }

    [TestMethod]
    public void FullyPopulatedTli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQTraderLayerInfo = new PQTraderLayerInfo(populatedTli, nameIdLookupGenerator);
        emptyTli.CopyFrom(nonPQTraderLayerInfo);
        Assert.AreEqual(populatedTli, emptyTli);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)populatedTli).Clone();
        Assert.AreNotSame(clone, populatedTli);
        Assert.AreEqual(populatedTli, clone);
        clone = ((IMutableTraderLayerInfo)populatedTli).Clone();
        Assert.AreNotSame(clone, populatedTli);
        Assert.AreEqual(populatedTli, clone);
        clone = populatedTli.Clone();
        Assert.AreNotSame(clone, populatedTli);
        Assert.AreEqual(populatedTli, clone);
    }

    [TestMethod]
    public void FullyPopulatedTliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableTraderLayerInfo)populatedTli).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedTli, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedTli_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedTli.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedTli.ToString();

        Assert.IsTrue(toString.Contains(populatedTli.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedTli.TraderName)}: {populatedTli.TraderName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedTli.TraderVolume)}: {populatedTli.TraderVolume:N2}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableTraderLayerInfo? original,
        IMutableTraderLayerInfo? changingTraderLayerInfo,
        ITraderPriceVolumeLayer? originalTraderPriceVolumeLayer = null,
        ITraderPriceVolumeLayer? changingTraderPriceVolumeLayer = null,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingTraderLayerInfo == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingTraderLayerInfo);

        changingTraderLayerInfo.TraderName = "TestChangedTraderName";
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.TraderName = original.TraderName;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.TraderVolume = 8_765_432;
        Assert.IsFalse(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.TraderVolume = original.TraderVolume;
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
