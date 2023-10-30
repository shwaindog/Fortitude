#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook;
using FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook;

[TestClass]
public class PQSourceQuoteRefPriceVolumeLayerTests
{
    private IPQNameIdLookupGenerator emptyNameIdLookup = null!;
    private PQSourceQuoteRefPriceVolumeLayer emptyPvl = null!;
    private IPQNameIdLookupGenerator nameIdLookup = null!;
    private PQSourceQuoteRefPriceVolumeLayer populatedPvl = null!;
    private DateTime testDateTime;
    private uint wellKnownQuoteRef;
    private string wellKnownSourceName = null!;

    [TestInitialize]
    public void SetUp()
    {
        wellKnownSourceName = "TestSourceName";
        wellKnownQuoteRef = 4_294_967_295u;
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
            PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        nameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
            PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        emptyPvl = new PQSourceQuoteRefPriceVolumeLayer(0m, 0m, emptyNameIdLookup.Clone());
        testDateTime = new DateTime(2017, 12, 17, 18, 54, 52);
        populatedPvl = new PQSourceQuoteRefPriceVolumeLayer(4.2949_672m, 42_949_672m, nameIdLookup,
            wellKnownSourceName, true, wellKnownQuoteRef);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new PQSourceQuoteRefPriceVolumeLayer(20, 40_000_000, nameIdLookup,
            wellKnownSourceName, true, wellKnownQuoteRef);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        Assert.AreEqual(wellKnownSourceName, newPvl.SourceName);
        Assert.IsTrue(newPvl.Executable);
        Assert.AreEqual(wellKnownQuoteRef, newPvl.SourceQuoteReference);
        Assert.IsTrue(newPvl.IsPriceUpdated);
        Assert.IsTrue(newPvl.IsVolumeUpdated);
        Assert.IsTrue(newPvl.IsSourceNameUpdated);
        Assert.IsTrue(newPvl.IsExecutableUpdated);
        Assert.IsTrue(newPvl.IsSourceQuoteReferenceUpdated);

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.IsFalse(emptyPvl.Executable);
        Assert.AreEqual(0u, emptyPvl.SourceQuoteReference);
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
        Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(emptyPvl.IsExecutableUpdated);
        Assert.IsFalse(emptyPvl.IsSourceQuoteReferenceUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(20, 40_000_000, nameIdLookup,
            wellKnownSourceName, true, wellKnownQuoteRef);
        var fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(wellKnownQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        var nonPQValueDatePvl = new SourceQuoteRefPriceVolumeLayer(1.23456m, 5_123_456m,
            wellKnownSourceName, true, wellKnownQuoteRef);
        var fromNonPqInstance = new PQSourceQuoteRefPriceVolumeLayer(nonPQValueDatePvl);
        Assert.AreEqual(1.23456m, fromNonPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromNonPqInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromNonPqInstance.SourceName);
        Assert.IsTrue(fromNonPqInstance.Executable);
        Assert.AreEqual(wellKnownQuoteRef, fromNonPqInstance.SourceQuoteReference);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromNonPqInstance.IsExecutableUpdated);
        Assert.IsTrue(fromNonPqInstance.IsSourceQuoteReferenceUpdated);

        var fromNonSourcePriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer(new PriceVolumeLayer(20, 40_000_000));
        Assert.AreEqual(20, fromNonSourcePriceVolumeLayer.Price);
        Assert.AreEqual(40_000_000, fromNonSourcePriceVolumeLayer.Volume);
        Assert.AreEqual(null, fromNonSourcePriceVolumeLayer.SourceName);
        Assert.IsFalse(fromNonSourcePriceVolumeLayer.Executable);
        Assert.AreEqual(0u, fromNonSourcePriceVolumeLayer.SourceQuoteReference);
        Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsPriceUpdated);
        Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(fromNonSourcePriceVolumeLayer.IsSourceNameUpdated);
        Assert.IsFalse(fromNonSourcePriceVolumeLayer.IsExecutableUpdated);
        Assert.IsFalse(fromNonSourcePriceVolumeLayer.IsSourceQuoteReferenceUpdated);

        fromNonSourcePriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer(
            new SourcePriceVolumeLayer(20, 40_000_000, wellKnownSourceName, true));
        Assert.AreEqual(20, fromNonSourcePriceVolumeLayer.Price);
        Assert.AreEqual(40_000_000, fromNonSourcePriceVolumeLayer.Volume);
        Assert.AreEqual(wellKnownSourceName, fromNonSourcePriceVolumeLayer.SourceName);
        Assert.IsTrue(fromNonSourcePriceVolumeLayer.Executable);
        Assert.AreEqual(0u, fromNonSourcePriceVolumeLayer.SourceQuoteReference);
        Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsPriceUpdated);
        Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsVolumeUpdated);
        Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsSourceNameUpdated);
        Assert.IsTrue(fromNonSourcePriceVolumeLayer.IsExecutableUpdated);
        Assert.IsFalse(fromNonSourcePriceVolumeLayer.IsSourceQuoteReferenceUpdated);

        var newEmptyPvl = new PQSourceQuoteRefPriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(newEmptyPvl.IsExecutableUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceQuoteReferenceUpdated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(20, 40_000_000, nameIdLookup,
            wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsPriceUpdated = false
        };
        var fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(wellKnownQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(20, 40_000_000, nameIdLookup,
            wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsVolumeUpdated = false
        };
        fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(wellKnownQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(20, 40_000_000, nameIdLookup,
            wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsSourceNameUpdated = false
        };
        fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(wellKnownQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(20, 40_000_000, nameIdLookup,
            wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsExecutableUpdated = false
        };
        fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsFalse(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(20, 40_000_000, nameIdLookup,
            wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsSourceQuoteReferenceUpdated = false
        };
        fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsFalse(fromPQInstance.IsSourceQuoteReferenceUpdated);
    }

    [TestMethod]
    public void EmptyPvl_LayerSourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.AreEqual(0u, emptyPvl.SourceQuoteReference);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).Count());

        emptyPvl.SourceQuoteReference = wellKnownQuoteRef;
        Assert.IsTrue(emptyPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(wellKnownQuoteRef, emptyPvl.SourceQuoteReference);
        var sourceLayerUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerSourceQuoteRefOffset,
            wellKnownQuoteRef);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        emptyPvl.IsSourceQuoteReferenceUpdated = false;
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        emptyPvl.IsSourceQuoteReferenceUpdated = true;
        sourceLayerUpdates =
            (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates)
                where update.Id == PQFieldKeys.LayerSourceQuoteRefOffset
                select update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        var newEmpty = new PQSourceQuoteRefPriceVolumeLayer();
        newEmpty.UpdateField(sourceLayerUpdates[0]);
        Assert.AreEqual(wellKnownQuoteRef, newEmpty.SourceQuoteReference);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsSourceQuoteReferenceUpdated);
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
        Assert.AreNotEqual(null, populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.AreEqual(wellKnownQuoteRef, populatedPvl.SourceQuoteReference);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        Assert.IsTrue(populatedPvl.IsSourceQuoteReferenceUpdated);
        populatedPvl.Reset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(null, populatedPvl.SourceName);
        Assert.IsFalse(populatedPvl.Executable);
        Assert.AreEqual(0u, populatedPvl.SourceQuoteReference);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        Assert.IsTrue(populatedPvl.IsSourceQuoteReferenceUpdated);
    }

    [TestMethod]
    public void PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
    {
        var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 12, 17, 12, 33, 1), UpdateStyle.Updates).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 12, 17, 12, 33, 1), UpdateStyle.FullSnapshot).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
        var pqStringUpdates = populatedPvl.GetStringUpdates(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        Assert.AreEqual(0, pqStringUpdates.Count);
    }

    [TestMethod]
    public void PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 13, 33, 3), UpdateStyle.Updates | UpdateStyle.Replay).ToList();
        var pqStringUpdates = populatedPvl.GetStringUpdates(
            new DateTime(2017, 11, 04, 13, 33, 3), UpdateStyle.Updates | UpdateStyle.Replay).ToList();
        var newEmpty = new PQSourceQuoteRefPriceVolumeLayer(0m, 0m, emptyNameIdLookup.Clone());
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new SourceQuoteRefPriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer(0m, 0m, emptyNameIdLookup);
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(null, emptyPriceVolumeLayer.SourceName);
        Assert.IsFalse(emptyPriceVolumeLayer.Executable);
        Assert.AreEqual(0u, emptyPriceVolumeLayer.SourceQuoteReference);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsSourceNameUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsExecutableUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsSourceQuoteReferenceUpdated);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ICloneable<ISourceQuoteRefPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);

        var cloned2 = (PQSourceQuoteRefPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned2, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned2);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQSourceQuoteRefPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedPvl,
            fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedPvl,
            fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedPvlSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedPvl, populatedPvl);
        Assert.AreEqual(populatedPvl, ((ICloneable)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((IMutableSourcePriceVolumeLayer)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<ISourcePriceVolumeLayer>)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((IPQSourcePriceVolumeLayer)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((IMutableSourceQuoteRefPriceVolumeLayer)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((IPQSourceQuoteRefPriceVolumeLayer)populatedPvl).Clone());
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.SourceQuoteReference)}: " +
                                        $"{populatedPvl.SourceQuoteReference:N0}"));
    }

    public static void AssertContainsAllPvlFields(IList<PQFieldUpdate> checkFieldUpdates,
        PQSourceQuoteRefPriceVolumeLayer pvl)
    {
        PQSourcePriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl);

        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerSourceQuoteRefOffset, pvl.SourceQuoteReference),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                PQFieldKeys.LayerSourceQuoteRefOffset), $"For {pvl.GetType().Name} ");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IPQSourceQuoteRefPriceVolumeLayer? original, IPQSourceQuoteRefPriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        PQSourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            original, changingPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQSourceQuoteRefPriceVolumeLayer) &&
            changingPriceVolumeLayer.GetType() == typeof(PQSourceQuoteRefPriceVolumeLayer))
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        else
            Assert.AreEqual(!exactComparison, original.AreEquivalent(
                changingPriceVolumeLayer, exactComparison));

        if (original.GetType() == typeof(PQSourceQuoteRefPriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                changingPriceVolumeLayer.AreEquivalent(new SourceQuoteRefPriceVolumeLayer(original), exactComparison));

        SourceQuoteRefPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            original, changingPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated
            = !changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated = original.IsSourceQuoteReferenceUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
