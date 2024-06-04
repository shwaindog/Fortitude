// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQPriceVolumeLayerTests
{
    private PQPriceVolumeLayer emptyPvl     = null!;
    private PQPriceVolumeLayer populatedPvl = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        emptyPvl     = new PQPriceVolumeLayer();
        testDateTime = new DateTime(2017, 12, 17, 16, 11, 52);
        populatedPvl = new PQPriceVolumeLayer(4.2949_672m, 42_949_672m);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new PQPriceVolumeLayer(20, 40_000_000);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        Assert.IsTrue(newPvl.IsPriceUpdated);
        Assert.IsTrue(newPvl.IsVolumeUpdated);

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new PQPriceVolumeLayer(20, 40_000_000);
        var fromPQInstance  = new PQPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);

        var nonPQPvl          = new PriceVolumeLayer(1.23456m, 5_123_456m);
        var fromNonPqInstance = new PQPriceVolumeLayer(nonPQPvl);
        Assert.AreEqual(1.23456m, fromNonPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromNonPqInstance.Volume);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);

        var newEmptyPvl = new PQPriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedPvl = new PQPriceVolumeLayer(20, 40_000_000) { IsPriceUpdated = false };
        var fromPQInstance  = new PQPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);

        newPopulatedPvl = new PQPriceVolumeLayer(20, 40_000_000) { IsVolumeUpdated = false };
        fromPQInstance  = new PQPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
    }

    [TestMethod]
    public void EmptyPvl_PriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.AreEqual(0m, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        const decimal expectedPrice = 2.345678m;
        emptyPvl.Price = expectedPrice;
        Assert.IsTrue(emptyPvl.IsPriceUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(expectedPrice, emptyPvl.Price);
        var sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerPriceOffset,
                                                    expectedPrice, 1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyPvl.IsPriceUpdated = false;
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        const decimal nextExpectedPrice = 2.345677m;
        emptyPvl.Price = nextExpectedPrice;
        Assert.IsTrue(emptyPvl.IsPriceUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(nextExpectedPrice, emptyPvl.Price);
        sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, nextExpectedPrice, 1);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.LayerPriceOffset
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPriceVolumeLayer();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedPrice, newEmpty.Price);
        Assert.IsTrue(newEmpty.IsPriceUpdated);
    }

    [TestMethod]
    public void EmptyPvl_VolumeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.AreEqual(0m, emptyPvl.Volume);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        const decimal expectedVolume = 42_949_672; // max level 6 scale can handle. (2^32 / 100)
        emptyPvl.Volume = expectedVolume;
        Assert.IsTrue(emptyPvl.IsVolumeUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(expectedVolume, emptyPvl.Volume);
        var sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerVolumeOffset, expectedVolume, 6);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyPvl.IsVolumeUpdated = false;
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        const decimal nextExpectedVolume = 0.01m;
        emptyPvl.Volume = nextExpectedVolume;
        Assert.IsTrue(emptyPvl.IsVolumeUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(nextExpectedVolume, emptyPvl.Volume);
        sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerVolumeOffset, nextExpectedVolume, 6);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.LayerVolumeOffset
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQPriceVolumeLayer();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedVolume, newEmpty.Volume);
        Assert.IsTrue(newEmpty.IsVolumeUpdated);
    }

    [TestMethod]
    public void PopulatedPvl_HasUpdates_ClearedAndSetAffectsAllTrackedFields()
    {
        Assert.IsTrue(populatedPvl.HasUpdates);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        populatedPvl.HasUpdates = false;
        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.IsFalse(populatedPvl.IsPriceUpdated);
        Assert.IsFalse(populatedPvl.IsVolumeUpdated);
        populatedPvl.HasUpdates = true;
        Assert.IsTrue(populatedPvl.HasUpdates);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
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
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        populatedPvl.StateReset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
    }

    [TestMethod]
    public void PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Snapshot).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewPvl_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQPriceVolumeLayer();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new PriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQPriceVolumeLayer();
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
    }

    [TestMethod]
    public void NonPQPopulatedPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther()
    {
        var nonPQPvl = new PriceVolumeLayer(populatedPvl);
        var newEmpty = new PQPriceVolumeLayer();
        newEmpty.CopyFrom(nonPQPvl);
        Assert.IsTrue(populatedPvl.AreEquivalent(newEmpty));
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedPvl = ((ICloneable<IPriceVolumeLayer>)populatedPvl).Clone();

        Assert.AreNotSame(clonedPvl, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedPvl);

        var cloned2 = (PQPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned2, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned2);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedPvl, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedPvl, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedPvlSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedPvl, populatedPvl);
        Assert.AreEqual(populatedPvl, ((ICloneable)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<IPriceVolumeLayer>)populatedPvl).Clone());
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
    }

    public static void AssertContainsAllPvlFields(IList<PQFieldUpdate> checkFieldUpdates,
        IPQPriceVolumeLayer pvl)
    {
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, pvl.Price, 1),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQFieldKeys.LayerPriceOffset, 1), $"For {pvl.GetType().Name} ");
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerVolumeOffset, pvl.Volume, 6),
                        PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQFieldKeys.LayerVolumeOffset, 6), $"For {pvl.GetType().Name} ");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IPQPriceVolumeLayer? original, IPQPriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        if (original.GetType() == changingPriceVolumeLayer.GetType())
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        else
            Assert.AreEqual(!exactComparison, original.AreEquivalent(
                                                                     changingPriceVolumeLayer, exactComparison));

        if (original.GetType() == typeof(PQPriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new PriceVolumeLayer(original), exactComparison));

        PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
                                                                                  original, changingPriceVolumeLayer, originalOrderBook
                                                                                , changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.IsPriceUpdated = !changingPriceVolumeLayer.IsPriceUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsPriceUpdated = original.IsPriceUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.IsVolumeUpdated = !changingPriceVolumeLayer.IsVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsVolumeUpdated = original.IsVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
