// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQValueDatePriceVolumeLayerTests
{
    private PQValueDatePriceVolumeLayer emptyPvl     = null!;
    private PQValueDatePriceVolumeLayer populatedPvl = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        emptyPvl     = new PQValueDatePriceVolumeLayer(0m, 0m, DateTimeConstants.UnixEpoch);
        testDateTime = new DateTime(2017, 12, 17, 18, 54, 52);
        populatedPvl = new PQValueDatePriceVolumeLayer
            (4.2949_672m, 42_949_672m, new DateTime(2017, 12, 17, 18, 00, 00));
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var expectedDate = new DateTime(2017, 12, 17, 19, 00, 11);
        var newPvl       = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        Assert.AreEqual(expectedDate, newPvl.ValueDate);
        Assert.IsTrue(newPvl.IsPriceUpdated);
        Assert.IsTrue(newPvl.IsVolumeUpdated);
        Assert.IsTrue(newPvl.IsValueDateUpdated);

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPvl.ValueDate);
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
        Assert.IsFalse(emptyPvl.IsValueDateUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var expectedDate    = new DateTime(2017, 12, 17, 19, 00, 11);
        var newPopulatedPvl = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate);
        var fromPQInstance  = new PQValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(expectedDate, fromPQInstance.ValueDate);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);

        var nonPQValueDatePvl = new ValueDatePriceVolumeLayer(1.23456m, 5_123_456m, expectedDate);
        var fromNonPqInstance = new PQValueDatePriceVolumeLayer(nonPQValueDatePvl);
        Assert.AreEqual(1.23456m, fromNonPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromNonPqInstance.Volume);
        Assert.AreEqual(expectedDate, fromNonPqInstance.ValueDate);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);

        var fromNonValueDate = new PQValueDatePriceVolumeLayer(new PriceVolumeLayer(20, 40_000_000));
        Assert.AreEqual(20, fromNonValueDate.Price);
        Assert.AreEqual(40_000_000, fromNonValueDate.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, fromNonValueDate.ValueDate);
        Assert.IsTrue(fromNonValueDate.IsPriceUpdated);
        Assert.IsTrue(fromNonValueDate.IsVolumeUpdated);
        Assert.IsFalse(fromNonValueDate.IsValueDateUpdated);

        var newEmptyPvl = new PQValueDatePriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, newEmptyPvl.ValueDate);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsValueDateUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var expectedDate = new DateTime(2017, 12, 17, 19, 00, 11);
        var newPopulatedPvl = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate)
        {
            IsPriceUpdated = false
        };
        var fromPQInstance = new PQValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(expectedDate, fromPQInstance.ValueDate);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);

        newPopulatedPvl = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate)
        {
            IsVolumeUpdated = false
        };
        fromPQInstance = new PQValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(expectedDate, fromPQInstance.ValueDate);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);

        newPopulatedPvl = new PQValueDatePriceVolumeLayer(20, 40_000_000, expectedDate)
        {
            IsValueDateUpdated = false
        };
        fromPQInstance = new PQValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(expectedDate, fromPQInstance.ValueDate);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsValueDateUpdated);
    }

    [TestMethod]
    public void EmptyPvl_ValueDateChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsValueDateUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPvl.ValueDate);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedDateTime = new DateTime(2017, 12, 17, 19, 00, 00);
        emptyPvl.ValueDate = expectedDateTime;
        Assert.IsTrue(emptyPvl.IsValueDateUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(expectedDateTime, emptyPvl.ValueDate);
        var sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.LayerValueDate, expectedDateTime.GetHoursFromUnixEpoch());
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyPvl.IsValueDateUpdated = false;
        Assert.IsFalse(emptyPvl.IsValueDateUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var nextExpectedValueDate = new DateTime(2017, 12, 17, 20, 00, 00);
        emptyPvl.ValueDate = nextExpectedValueDate;
        Assert.IsTrue(emptyPvl.IsValueDateUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(nextExpectedValueDate, emptyPvl.ValueDate);
        sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.LayerValueDate, nextExpectedValueDate.GetHoursFromUnixEpoch());
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQQuoteFields.LayerValueDate
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQValueDatePriceVolumeLayer(0m, 0m, DateTimeConstants.UnixEpoch);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedValueDate, newEmpty.ValueDate);
        Assert.IsTrue(newEmpty.IsValueDateUpdated);
    }

    [TestMethod]
    public void EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPvl_SetIsEmptyTrue_ReturnsReturnsLayerToEmptyExceptUpdatedFlags()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedPvl.Price);
        Assert.AreNotEqual(0m, populatedPvl.Volume);
        Assert.AreNotEqual(DateTimeConstants.UnixEpoch, populatedPvl.ValueDate);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
        populatedPvl.IsEmpty = true;
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, populatedPvl.ValueDate);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
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
    public void PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQValueDatePriceVolumeLayer(0m, 0m, DateTimeConstants.UnixEpoch);
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new ValueDatePriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQValueDatePriceVolumeLayer(0m, 0m, DateTimeConstants.UnixEpoch);
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPriceVolumeLayer.ValueDate);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsValueDateUpdated);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ICloneable<IValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);

        var cloned2 = (PQValueDatePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned2, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned2);
    }


    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQValueDatePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
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
        Assert.AreEqual(populatedPvl, ((IMutableValueDatePriceVolumeLayer)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<IValueDatePriceVolumeLayer>)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<IMutableValueDatePriceVolumeLayer>)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((IPQValueDatePriceVolumeLayer)populatedPvl).Clone());
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

    public static void AssertContainsAllPvlFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        PQValueDatePriceVolumeLayer pvl)
    {
        PQPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl);

        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerValueDate, pvl.ValueDate.GetHoursFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQQuoteFields.LayerValueDate), $"For {pvl.GetType().Name} ");
    }


    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQValueDatePriceVolumeLayer? original, PQValueDatePriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        PQPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBook
           , changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQValueDatePriceVolumeLayer) &&
            changingPriceVolumeLayer.GetType() == typeof(PQValueDatePriceVolumeLayer))
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        else
            Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));

        if (original.GetType() == typeof(PQValueDatePriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new ValueDatePriceVolumeLayer(original), exactComparison));

        ValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBook
           , changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.IsValueDateUpdated = !changingPriceVolumeLayer.IsValueDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsValueDateUpdated = original.IsValueDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
