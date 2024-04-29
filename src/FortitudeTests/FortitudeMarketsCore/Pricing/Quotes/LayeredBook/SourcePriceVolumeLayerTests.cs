#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

[TestClass]
public class SourcePriceVolumeLayerTests
{
    private SourcePriceVolumeLayer emptyPvl = null!;
    private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;
    private SourcePriceVolumeLayer populatedPvl = null!;

    private string wellKnownSourceName = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        wellKnownSourceName = "TestSourceName";

        emptyPvl = new SourcePriceVolumeLayer();
        populatedPvl = new SourcePriceVolumeLayer(4.2949_672m, 42_949_672m, wellKnownSourceName, true);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new SourcePriceVolumeLayer(20, 40_000_000, wellKnownSourceName, true);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        Assert.AreEqual(wellKnownSourceName, newPvl.SourceName);
        Assert.IsTrue(newPvl.Executable);

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.IsFalse(emptyPvl.Executable);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new SourcePriceVolumeLayer(20, 40_000_000, wellKnownSourceName, true);
        var fromSrcInstance = new SourcePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromSrcInstance.Price);
        Assert.AreEqual(40_000_000m, fromSrcInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromSrcInstance.SourceName);
        Assert.IsTrue(fromSrcInstance.Executable);

        var pqValueDatePvl = new PQSourcePriceVolumeLayer(nameIdLookupGenerator, 1.23456m, 5_123_456m, wellKnownSourceName, true);
        var fromPqInstance = new SourcePriceVolumeLayer(pqValueDatePvl);
        Assert.AreEqual(1.23456m, fromPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromPqInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPqInstance.SourceName);
        Assert.IsTrue(fromPqInstance.Executable);

        var fromNonSourcePriceVolumeLayer = new SourcePriceVolumeLayer(new PriceVolumeLayer(20, 40_000_000));
        Assert.AreEqual(20, fromNonSourcePriceVolumeLayer.Price);
        Assert.AreEqual(40_000_000, fromNonSourcePriceVolumeLayer.Volume);
        Assert.AreEqual(null, fromNonSourcePriceVolumeLayer.SourceName);
        Assert.IsFalse(fromNonSourcePriceVolumeLayer.Executable);

        var newEmptyPvl = new SourcePriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
    }

    [TestMethod]
    public void EmptyLayer_Mutate_UpdatesFields()
    {
        const decimal expectedPrice = 3.45678m;
        const decimal expectedVolume = 5.67890m;
        const string expectedSourceName = "NewSourceName";

        emptyPvl.Price = expectedPrice;
        emptyPvl.Volume = expectedVolume;
        emptyPvl.SourceName = expectedSourceName;
        emptyPvl.Executable = true;

        Assert.AreEqual(expectedPrice, emptyPvl.Price);
        Assert.AreEqual(expectedVolume, emptyPvl.Volume);
        Assert.AreEqual(expectedSourceName, emptyPvl.SourceName);
        Assert.IsTrue(emptyPvl.Executable);
    }

    [TestMethod]
    public void PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedPvl.Price);
        Assert.AreNotEqual(0m, populatedPvl.Volume);
        Assert.IsNotNull(populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        populatedPvl.StateReset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.IsNull(populatedPvl.SourceName);
        Assert.IsFalse(populatedPvl.Executable);
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
        var pqPvl = new PQSourcePriceVolumeLayer(populatedPvl, nameIdLookupGenerator);
        var newEmpty = new SourcePriceVolumeLayer();
        newEmpty.CopyFrom(pqPvl);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = populatedPvl.Clone();
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
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableSourcePriceVolumeLayer)populatedPvl).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedPvl,
            fullyPopulatedClone);
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

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IMutableSourcePriceVolumeLayer? original, IMutableSourcePriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            original, changingPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.SourceName = "ChangingSourceName";
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.SourceName = original.SourceName;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.Executable = !changingPriceVolumeLayer.Executable;
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.Executable = original.Executable;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
