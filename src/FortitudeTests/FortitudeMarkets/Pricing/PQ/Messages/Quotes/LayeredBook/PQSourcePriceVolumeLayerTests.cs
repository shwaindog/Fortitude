// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQSourcePriceVolumeLayerTests
{
    private IPQNameIdLookupGenerator emptyNameIdLookup = null!;
    private PQSourcePriceVolumeLayer emptyPvl          = null!;
    private IPQNameIdLookupGenerator nameIdLookup      = null!;
    private PQSourcePriceVolumeLayer populatedPvl      = null!;

    private DateTime testDateTime;
    private string   wellKnownSourceName = null!;

    [TestInitialize]
    public void SetUp()
    {
        wellKnownSourceName = "TestSourceName";

        emptyNameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        nameIdLookup      = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        emptyPvl          = new PQSourcePriceVolumeLayer(nameIdLookup.Clone(), 0m, 0m);
        testDateTime      = new DateTime(2017, 12, 17, 18, 54, 52);
        populatedPvl      = new PQSourcePriceVolumeLayer(nameIdLookup, 4.2949_672m, 42_949_672m, wellKnownSourceName, true);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new PQSourcePriceVolumeLayer(nameIdLookup.Clone(), 20, 40_000_000, wellKnownSourceName, true);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        Assert.AreEqual(wellKnownSourceName, newPvl.SourceName);
        Assert.IsTrue(newPvl.Executable);
        Assert.IsTrue(newPvl.IsPriceUpdated);
        Assert.IsTrue(newPvl.IsVolumeUpdated);
        Assert.IsTrue(newPvl.IsSourceNameUpdated);
        Assert.IsTrue(newPvl.IsExecutableUpdated);

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.IsFalse(emptyPvl.Executable);
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
        Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(emptyPvl.IsExecutableUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new PQSourcePriceVolumeLayer(nameIdLookup.Clone(), 20, 40_000_000, wellKnownSourceName, true);
        var fromPQInstance  = new PQSourcePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);

        var nonPQValueDatePvl = new SourcePriceVolumeLayer(1.23456m, 5_123_456m, wellKnownSourceName, true);
        var fromNonPqInstance = new PQSourcePriceVolumeLayer(nonPQValueDatePvl, emptyNameIdLookup.Clone());
        Assert.AreEqual(1.23456m, fromNonPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromNonPqInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromNonPqInstance.SourceName);
        Assert.IsTrue(fromNonPqInstance.Executable);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromNonPqInstance.IsExecutableUpdated);

        var fromNonSourcePriceVolumeLayer = new PQSourcePriceVolumeLayer(new PriceVolumeLayer(20, 40_000_000), emptyNameIdLookup.Clone());
        Assert.AreEqual(20, fromNonSourcePriceVolumeLayer.Price);
        Assert.AreEqual(40_000_000, fromNonSourcePriceVolumeLayer.Volume);
        Assert.AreEqual(null, fromNonSourcePriceVolumeLayer.SourceName);
        Assert.IsFalse(fromNonSourcePriceVolumeLayer.Executable);
        Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsPriceUpdated);
        Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(fromNonSourcePriceVolumeLayer.IsSourceNameUpdated);
        Assert.IsFalse(fromNonSourcePriceVolumeLayer.IsExecutableUpdated);

        var newEmptyPvl = new PQSourcePriceVolumeLayer(emptyPvl, emptyNameIdLookup.Clone());
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(newEmptyPvl.IsExecutableUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedPvl = new PQSourcePriceVolumeLayer(nameIdLookup.Clone(), 20, 40_000_000, wellKnownSourceName, true)
        {
            IsPriceUpdated = false
        };
        var fromPQInstance = new PQSourcePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);

        newPopulatedPvl = new PQSourcePriceVolumeLayer(nameIdLookup.Clone(), 20, 40_000_000, wellKnownSourceName, true)
        {
            IsVolumeUpdated = false
        };
        fromPQInstance = new PQSourcePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);

        newPopulatedPvl = new PQSourcePriceVolumeLayer(nameIdLookup.Clone(), 20, 40_000_000, wellKnownSourceName, true)
        {
            IsSourceNameUpdated = false
        };
        fromPQInstance = new PQSourcePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);

        newPopulatedPvl = new PQSourcePriceVolumeLayer(nameIdLookup.Clone(), 20, 40_000_000, wellKnownSourceName, true)
        {
            IsExecutableUpdated = false
        };
        fromPQInstance = new PQSourcePriceVolumeLayer(newPopulatedPvl, emptyNameIdLookup.Clone());
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsFalse(fromPQInstance.IsExecutableUpdated);
    }

    [TestMethod]
    public void PopulatedPvl_HasUpdatesSetFalse_LookupAndLayerHaveNoUpdates()
    {
        Assert.IsTrue(populatedPvl.HasUpdates);
        Assert.IsTrue(populatedPvl.NameIdLookup.HasUpdates);

        populatedPvl.HasUpdates = false;

        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.IsFalse(populatedPvl.NameIdLookup.HasUpdates);
    }

    [TestMethod]
    public void EmptyPvl_SourceNameChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        emptyPvl.SourceName = wellKnownSourceName;
        Assert.IsTrue(emptyPvl.IsSourceNameUpdated);
        Assert.AreEqual(emptyNameIdLookup[wellKnownSourceName], emptyPvl.SourceId);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(wellKnownSourceName, emptyPvl.SourceName);
        var sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.SourceId, emptyPvl.SourceId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyPvl.IsSourceNameUpdated = false;
        Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        emptyPvl.NameIdLookup.HasUpdates = false;
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var nextExpectedSourceName = "AnotherSourceName";
        emptyPvl.SourceName = nextExpectedSourceName;
        Assert.IsTrue(emptyPvl.IsSourceNameUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(nextExpectedSourceName, emptyPvl.SourceName);
        sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var stringUpdates = emptyPvl.GetStringUpdates(testDateTime, StorageFlags.Update)
                                    .ToList();
        Assert.AreEqual(1, stringUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.SourceId, emptyPvl.SourceId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
        var expectedStringUpdates = new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(PQQuoteFields.LayerNameDictionaryUpsertCommand, 0u, CrudCommand.Upsert.ToUShort())
          , StringUpdate = new PQStringUpdate
            {
                Command = CrudCommand.Upsert, DictionaryId = emptyPvl.NameIdLookup[emptyPvl.SourceName]
              , Value   = emptyPvl.SourceName
            }
        };
        Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

        sourceUpdates = (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQQuoteFields.SourceId
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmptyNameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        var newEmpty             = new PQSourcePriceVolumeLayer(emptyNameIdLookup);
        newEmpty.UpdateField(sourceUpdates[0]);
        newEmpty.UpdateFieldString(stringUpdates[0]);
        Assert.AreEqual(nextExpectedSourceName, newEmpty.SourceName);
        Assert.IsTrue(newEmpty.IsSourceNameUpdated);
    }

    [TestMethod]
    public void EmptyPvl_LayerExecutableChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsExecutableUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsFalse(emptyPvl.Executable);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        emptyPvl.Executable = true;
        Assert.IsTrue(emptyPvl.IsExecutableUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.Executable);
        var sourceLayerUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQQuoteFields.LayerBooleanFlags, LayerBooleanFlags.IsExecutableFlag.ToUInt());
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        emptyPvl.IsExecutableUpdated = false;
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyPvl.IsExecutableUpdated = true;
        sourceLayerUpdates =
            (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQQuoteFields.LayerBooleanFlags
                select update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        var newEmpty = new PQSourcePriceVolumeLayer(emptyNameIdLookup);
        newEmpty.UpdateField(sourceLayerUpdates[0]);
        Assert.IsTrue(newEmpty.Executable);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsExecutableUpdated);
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
        Assert.AreNotEqual(null, populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        populatedPvl.IsEmpty = true;
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(null, populatedPvl.SourceName);
        Assert.IsFalse(populatedPvl.Executable);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
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
        var pqStringUpdates =
            populatedPvl.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        Assert.AreEqual(0, pqStringUpdates.Count);
    }

    [TestMethod]
    public void PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var pqStringUpdates =
            populatedPvl.GetStringUpdates
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQSourcePriceVolumeLayer(emptyNameIdLookup);
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new SourcePriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQSourcePriceVolumeLayer(emptyNameIdLookup);
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(null, emptyPriceVolumeLayer.SourceName);
        Assert.IsFalse(emptyPriceVolumeLayer.Executable);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsSourceNameUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsExecutableUpdated);
    }

    [TestMethod]
    public void EmptyPvl_OnConstruction_SharesTraderNameIdLookupBetweenLayers()
    {
        var newEmpty = new PQSourcePriceVolumeLayer(emptyNameIdLookup);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        Assert.AreSame(emptyNameIdLookup, newEmpty.NameIdLookup);
        newEmpty.NameIdLookup = populatedPvl.NameIdLookup;
        Assert.AreSame(populatedPvl.NameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ICloneable<ISourcePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);

        var cloned2 = (PQSourcePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned2, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned2);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQSourcePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
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
        Assert.AreEqual(populatedPvl, ((IMutableSourcePriceVolumeLayer)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<ISourcePriceVolumeLayer>)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((IPQSourcePriceVolumeLayer)populatedPvl).Clone());
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
    }

    public static void AssertContainsAllPvlFields
    (IList<PQFieldUpdate> checkFieldUpdates, PQSourcePriceVolumeLayer pvl, int bookIndex = 0,
        PQFieldFlags priceScale = (PQFieldFlags)1, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        PQPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl, bookIndex, priceScale, volumeScale);

        var depthId = (PQDepthKey)bookIndex;
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.SourceId, depthId, pvl.SourceId),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.SourceId, depthId),
                        $"For {pvl.GetType().Name} at {bookIndex} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.LayerBooleanFlags, depthId,
                                          pvl.Executable ? LayerBooleanFlags.IsExecutableFlag.ToUInt() : 0u),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LayerBooleanFlags, depthId),
                        $"For {pvl.GetType().Name} at {bookIndex} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQSourcePriceVolumeLayer? original, IPQSourcePriceVolumeLayer? changingPriceVolumeLayer,
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

        if (original.GetType() == typeof(PQSourcePriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new SourcePriceVolumeLayer(original), exactComparison));

        SourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBook
           , changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.IsSourceNameUpdated = !changingPriceVolumeLayer.IsSourceNameUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsSourceNameUpdated = original.IsSourceNameUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.IsExecutableUpdated = !changingPriceVolumeLayer.IsExecutableUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsExecutableUpdated = original.IsExecutableUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
