// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

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
public class PQOrdersCountPriceVolumeLayerTests
{
    private const decimal Price          = 1.3456m;
    private const decimal Volume         = 100_000m;
    private const uint    OrdersCount    = 732;
    private const decimal InternalVolume = 50_000m;

    private IPQOrdersCountPriceVolumeLayer emptyPvl     = null!;
    private IPQOrdersCountPriceVolumeLayer populatedPvl = null!;

    private DateTime testDateTime;


    [TestInitialize]
    public void SetUp()
    {
        emptyPvl     = new PQOrdersCountPriceVolumeLayer();
        populatedPvl = new PQOrdersCountPriceVolumeLayer(Price, Volume, OrdersCount, InternalVolume);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new PQOrdersCountPriceVolumeLayer(Price, Volume, OrdersCount, InternalVolume);
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
        var newPopulatedPvl = new PQOrdersCountPriceVolumeLayer(populatedPvl);
        Assert.AreEqual(Price, newPopulatedPvl.Price);
        Assert.AreEqual(Volume, newPopulatedPvl.Volume);
        Assert.AreEqual(OrdersCount, newPopulatedPvl.OrdersCount);
        Assert.AreEqual(InternalVolume, newPopulatedPvl.InternalVolume);
        Assert.IsTrue(newPopulatedPvl.IsPriceUpdated);
        Assert.IsTrue(newPopulatedPvl.IsVolumeUpdated);
        Assert.IsTrue(newPopulatedPvl.IsOrdersCountUpdated);
        Assert.IsTrue(newPopulatedPvl.IsInternalVolumeUpdated);
        Assert.IsFalse(newPopulatedPvl.IsEmpty);
        Assert.IsTrue(newPopulatedPvl.HasUpdates);

        var nonPqvTraderPvl   = new OrdersCountPriceVolumeLayer(populatedPvl);
        var fromNonPqInstance = new PQOrdersCountPriceVolumeLayer(nonPqvTraderPvl);
        Assert.AreEqual(Price, fromNonPqInstance.Price);
        Assert.AreEqual(Volume, fromNonPqInstance.Volume);
        Assert.AreEqual(OrdersCount, fromNonPqInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromNonPqInstance.InternalVolume);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsOrdersCountUpdated);
        Assert.IsTrue(fromNonPqInstance.IsInternalVolumeUpdated);
        Assert.IsFalse(newPopulatedPvl.IsEmpty);
        Assert.IsTrue(newPopulatedPvl.HasUpdates);

        var newEmptyPvl = new PQOrdersCountPriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(0u, newEmptyPvl.OrdersCount);
        Assert.AreEqual(0m, newEmptyPvl.InternalVolume);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsOrdersCountUpdated);
        Assert.IsFalse(newEmptyPvl.IsInternalVolumeUpdated);
        Assert.IsTrue(newEmptyPvl.IsEmpty);
        Assert.IsFalse(newEmptyPvl.HasUpdates);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedPvl = new PQOrdersCountPriceVolumeLayer(Price, Volume, OrdersCount, InternalVolume)
        {
            IsVolumeUpdated = false, IsOrdersCountUpdated = false, IsInternalVolumeUpdated = false
        };
        var fromPQInstance = new PQOrdersCountPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsFalse(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);

        newPopulatedPvl.IsPriceUpdated  = false;
        newPopulatedPvl.IsVolumeUpdated = true;
        fromPQInstance                  = new PQOrdersCountPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsFalse(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);

        newPopulatedPvl.IsVolumeUpdated      = false;
        newPopulatedPvl.IsOrdersCountUpdated = true;
        fromPQInstance                       = new PQOrdersCountPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsInternalVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrdersCountUpdated);

        newPopulatedPvl.IsOrdersCountUpdated    = false;
        newPopulatedPvl.IsInternalVolumeUpdated = true;
        fromPQInstance                          = new PQOrdersCountPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(Price, fromPQInstance.Price);
        Assert.AreEqual(Volume, fromPQInstance.Volume);
        Assert.AreEqual(OrdersCount, fromPQInstance.OrdersCount);
        Assert.AreEqual(InternalVolume, fromPQInstance.InternalVolume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrdersCountUpdated);
        Assert.IsTrue(fromPQInstance.IsInternalVolumeUpdated);
    }

    [TestMethod]
    public void PopulatedPvl_LayerOrdersCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedPvl.HasUpdates = false;

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(populatedPvl.IsOrdersCountUpdated);
        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.AreEqual(0, populatedPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        populatedPvl.OrdersCount = byte.MaxValue;
        Assert.IsTrue(populatedPvl.HasUpdates);
        Assert.AreEqual(byte.MaxValue, populatedPvl.OrdersCount);
        var layerUpdates = populatedPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQQuoteFields.OrdersCount, byte.MaxValue);
        Assert.AreEqual(expectedLayerField, layerUpdates[0]);

        populatedPvl.HasUpdates = false;
        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.IsTrue(populatedPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        populatedPvl.IsOrdersCountUpdated = true;
        layerUpdates =
            (from update in populatedPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQQuoteFields.OrdersCount
                select update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        Assert.AreEqual(expectedLayerField, layerUpdates[0]);

        var newEmpty = new PQOrdersCountPriceVolumeLayer();
        newEmpty.UpdateField(layerUpdates[0]);
        Assert.AreEqual(byte.MaxValue, newEmpty.OrdersCount);
        Assert.IsTrue(newEmpty.HasUpdates);
    }

    [TestMethod]
    public void PopulatedPvl_LayerInternalVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedPvl.HasUpdates = false;

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(populatedPvl.IsInternalVolumeUpdated);
        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.AreEqual(0, populatedPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var newInternalVolume = InternalVolume + 10_000;
        populatedPvl.InternalVolume = newInternalVolume;
        Assert.IsTrue(populatedPvl.HasUpdates);
        Assert.AreEqual(newInternalVolume, populatedPvl.InternalVolume);
        var layerUpdates = populatedPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQQuoteFields.InternalVolume, newInternalVolume, (PQFieldFlags)6);
        Assert.AreEqual(expectedLayerField, layerUpdates[0]);

        populatedPvl.HasUpdates = false;
        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.IsTrue(populatedPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        populatedPvl.IsInternalVolumeUpdated = true;
        layerUpdates =
            (from update in populatedPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQQuoteFields.InternalVolume
                select update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        Assert.AreEqual(expectedLayerField, layerUpdates[0]);

        var newEmpty = new PQOrdersCountPriceVolumeLayer();
        newEmpty.UpdateField(layerUpdates[0]);
        Assert.AreEqual(newInternalVolume, newEmpty.InternalVolume);
        Assert.IsTrue(newEmpty.HasUpdates);
    }


    [TestMethod]
    public void EmptyPvl_SetOrdersCountOnly_UpdatesCount()
    {
        Assert.AreEqual(0u, emptyPvl.OrdersCount);
        emptyPvl.OrdersCount = byte.MaxValue;
        Assert.AreEqual(byte.MaxValue, emptyPvl.OrdersCount);
        Assert.IsTrue(emptyPvl.IsOrdersCountUpdated);
        Assert.IsFalse(emptyPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.HasUpdates);
    }

    [TestMethod]
    public void EmptyPvl_SetInternalVolumeOnly_UpdatesCount()
    {
        Assert.AreEqual(0m, emptyPvl.InternalVolume);
        emptyPvl.InternalVolume = InternalVolume;
        Assert.AreEqual(InternalVolume, emptyPvl.InternalVolume);
        Assert.IsTrue(emptyPvl.IsInternalVolumeUpdated);
        Assert.IsFalse(emptyPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.HasUpdates);
    }

    [TestMethod]
    public void EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPvl_IsEmptyTrue_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedPvl.Price);
        Assert.AreNotEqual(0m, populatedPvl.Volume);
        Assert.AreNotEqual(0u, populatedPvl.OrdersCount);
        Assert.AreNotEqual(0m, populatedPvl.InternalVolume);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsOrdersCountUpdated);
        Assert.IsTrue(populatedPvl.IsInternalVolumeUpdated);
        Assert.IsTrue(populatedPvl.HasUpdates);
        populatedPvl.IsEmpty = true;
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(0u, populatedPvl.OrdersCount);
        Assert.AreEqual(0m, populatedPvl.InternalVolume);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsOrdersCountUpdated);
        Assert.IsTrue(populatedPvl.IsInternalVolumeUpdated);
        Assert.IsTrue(populatedPvl.HasUpdates);
        populatedPvl.StateReset();
        Assert.IsFalse(populatedPvl.IsPriceUpdated);
        Assert.IsFalse(populatedPvl.IsVolumeUpdated);
        Assert.IsFalse(populatedPvl.IsOrdersCountUpdated);
        Assert.IsFalse(populatedPvl.IsInternalVolumeUpdated);
        Assert.IsFalse(populatedPvl.HasUpdates);
    }

    [TestMethod]
    public void PopulatedPvlWithOrderCountUpdated_GetDeltaUpdate_ReturnsOrderCountFieldAsUpdated()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);

        populatedPvl.OrdersCount = 1;
        pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithInternalVolumeUpdated_GetDeltaUpdate_ReturnsInternalVolumeFieldAsUpdated()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);

        var updatedInternalVolume = InternalVolume + 10_000;
        populatedPvl.InternalVolume = updatedInternalVolume;
        pqFieldUpdates =
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

        populatedPvl.OrdersCount = 1;
        var updatedInternalVolume = InternalVolume + 10_000;
        populatedPvl.InternalVolume = updatedInternalVolume;
        populatedPvl.HasUpdates     = false;
        pqFieldUpdates =
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

        populatedPvl.OrdersCount = 1;
        var updatedInternalVolume = InternalVolume + 10_000;
        populatedPvl.InternalVolume = updatedInternalVolume;
        populatedPvl.HasUpdates     = false;
        pqFieldUpdates =
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
        var newEmpty = new PQOrdersCountPriceVolumeLayer();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new OrdersCountPriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQOrdersCountPriceVolumeLayer();
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(0u, emptyPriceVolumeLayer.OrdersCount);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.InternalVolume);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsOrdersCountUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsInternalVolumeUpdated);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IMutableOrdersCountPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);

        var cloned2 = ((IOrdersCountPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(cloned2, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned2);

        var cloned3 = (PQOrdersCountPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned3, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned3);

        var cloned4 = populatedPvl.Clone();
        Assert.AreNotSame(cloned4, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned4);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQOrdersCountPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
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
        Assert.AreEqual(populatedPvl, ((IMutableOrdersCountPriceVolumeLayer)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<IOrdersCountPriceVolumeLayer>)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, populatedPvl.Clone());
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
        var toString = populatedPvl.ToString()!;

        Assert.IsTrue(toString.Contains(populatedPvl.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Price)}: {populatedPvl.Price:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Volume)}: {populatedPvl.Volume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.OrdersCount)}: {populatedPvl.OrdersCount}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.InternalVolume)}: {populatedPvl.InternalVolume:N2}"));
    }

    public static void AssertContainsAllPvlFields
    (IList<PQFieldUpdate> checkFieldUpdates, IPQOrdersCountPriceVolumeLayer pvl, int bookIndex = 0,
        PQFieldFlags priceScale = (PQFieldFlags)1, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        PQPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl, bookIndex, priceScale, volumeScale);
        var depthId = (PQDepthKey)bookIndex;

        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OrdersCount, depthId, pvl.OrdersCount),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OrdersCount, depthId),
                        $"For {pvl.GetType().Name} at {bookIndex} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.InternalVolume, depthId, pvl.InternalVolume, volumeScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.InternalVolume, depthId, volumeScale),
                        $"For {pvl.GetType().Name} at {bookIndex} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQOrdersCountPriceVolumeLayer? original,
        IPQOrdersCountPriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBookSide? originalOrderBook = null,
        IOrderBookSide? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        PQPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBook
           , changingOrderBook, originalQuote, changingQuote);

        OrdersCountPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQOrdersCountPriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new OrdersCountPriceVolumeLayer(original), exactComparison));

        changingPriceVolumeLayer.IsOrdersCountUpdated = !changingPriceVolumeLayer.IsOrdersCountUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsOrdersCountUpdated = original.IsOrdersCountUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.IsInternalVolumeUpdated = !changingPriceVolumeLayer.IsInternalVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsInternalVolumeUpdated = original.IsInternalVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
