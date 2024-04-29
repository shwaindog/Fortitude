#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQTraderLayerInfoTests
{
    private const string TraderName = "TestTraderName";
    private IPQNameIdLookupGenerator emptyNameIdLookup = null!;
    private IPQTraderLayerInfo emptyTli = null!;
    private IPQNameIdLookupGenerator nameIdLookup = null!;
    private IPQTraderLayerInfo populatedTli = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        nameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        emptyTli = new PQTraderLayerInfo(emptyNameIdLookup.Clone());
        populatedTli = new PQTraderLayerInfo(nameIdLookup, TraderName, 42_111_222m);
    }

    [TestMethod]
    public void NewTli_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newExpectedTraderName = "DiffTraderName";
        var newTli = new PQTraderLayerInfo(nameIdLookup, newExpectedTraderName, 2_333_444m);
        Assert.AreEqual(newExpectedTraderName, newTli.TraderName);
        Assert.AreEqual(2_333_444m, newTli.TraderVolume);
        Assert.IsNotNull(newTli.NameIdLookup);
        Assert.IsTrue(newTli.IsTraderNameUpdated);
        Assert.IsTrue(newTli.IsTraderVolumeUpdated);
        Assert.IsTrue(newTli.HasUpdates);

        Assert.IsNull(emptyTli.TraderName);
        Assert.AreEqual(0m, emptyTli.TraderVolume);
        Assert.IsNotNull(emptyTli.NameIdLookup);
        Assert.IsFalse(emptyTli.IsTraderNameUpdated);
        Assert.IsFalse(emptyTli.IsTraderVolumeUpdated);
        Assert.IsFalse(emptyTli.HasUpdates);
    }

    [TestMethod]
    public void NewTli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var fromPQInstance = new PQTraderLayerInfo(populatedTli, populatedTli.NameIdLookup!);
        Assert.AreEqual(populatedTli.TraderName, fromPQInstance.TraderName);
        Assert.AreEqual(populatedTli.TraderVolume, fromPQInstance.TraderVolume);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderVolumeUpdated);

        var newExpectedTraderName = "NonPQTraderName";
        var nonPQTli = new TraderLayerInfo(newExpectedTraderName, 222_444);
        var fromNonPqInstance = new PQTraderLayerInfo(nonPQTli, emptyNameIdLookup.Clone());
        Assert.AreEqual(newExpectedTraderName, fromNonPqInstance.TraderName);
        Assert.AreEqual(222_444, fromNonPqInstance.TraderVolume);
        Assert.IsTrue(fromNonPqInstance.IsTraderNameUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTraderVolumeUpdated);

        var newEmptyTli = new PQTraderLayerInfo(emptyTli, emptyNameIdLookup);
        Assert.IsNull(newEmptyTli.TraderName);
        Assert.AreEqual(0, newEmptyTli.TraderVolume);
        Assert.IsFalse(newEmptyTli.IsTraderNameUpdated);
        Assert.IsFalse(newEmptyTli.IsTraderVolumeUpdated);
    }

    [TestMethod]
    public void NewTli_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedPvl = new PQTraderLayerInfo(nameIdLookup, TraderName, 123_234m)
        {
            IsTraderNameUpdated = false
        };
        var fromPQInstance = new PQTraderLayerInfo(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(newPopulatedPvl.TraderName, fromPQInstance.TraderName);
        Assert.AreEqual(newPopulatedPvl.TraderVolume, fromPQInstance.TraderVolume);
        Assert.IsFalse(fromPQInstance.IsTraderNameUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderVolumeUpdated);

        newPopulatedPvl = new PQTraderLayerInfo(nameIdLookup, TraderName, 123_234m)
        {
            IsTraderVolumeUpdated = false
        };
        fromPQInstance = new PQTraderLayerInfo(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(newPopulatedPvl.TraderName, fromPQInstance.TraderName);
        Assert.AreEqual(newPopulatedPvl.TraderVolume, fromPQInstance.TraderVolume);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);
        Assert.IsFalse(fromPQInstance.IsTraderVolumeUpdated);
    }

    [TestMethod]
    public void EmptyTli_TradeNameChanged_ExpectedPropertiesUpdated()
    {
        Assert.IsNull(emptyTli.TraderName);
        Assert.AreEqual(0, emptyTli.TraderNameId);
        Assert.IsFalse(emptyTli.IsTraderNameUpdated);
        Assert.AreEqual(0, emptyTli.NameIdLookup!.Count);
        Assert.IsTrue(emptyTli.IsEmpty);
        Assert.IsFalse(emptyTli.HasUpdates);
        var newEmptyTli = new PQTraderLayerInfo(emptyNameIdLookup.Clone());
        Assert.IsNull(newEmptyTli.TraderName);
        Assert.AreEqual(0, newEmptyTli.TraderNameId);
        Assert.IsFalse(newEmptyTli.IsTraderNameUpdated);
        Assert.AreEqual(0, newEmptyTli.NameIdLookup.Count);
        Assert.IsTrue(newEmptyTli.IsEmpty);
        Assert.IsFalse(newEmptyTli.HasUpdates);

        Assert.AreEqual(emptyTli, newEmptyTli);

        var expectedTraderName = "SetTraderNameToThis";
        emptyTli.TraderName = expectedTraderName;

        Assert.AreEqual(expectedTraderName, emptyTli.TraderName);
        Assert.AreEqual(1, emptyTli.TraderNameId);
        Assert.IsTrue(emptyTli.IsTraderNameUpdated);
        Assert.AreEqual(1, emptyTli.NameIdLookup.Count);
        Assert.IsFalse(emptyTli.IsEmpty);
        Assert.IsTrue(emptyTli.HasUpdates);

        Assert.AreNotEqual(emptyTli, newEmptyTli);

        newEmptyTli.TraderName = expectedTraderName;

        Assert.AreEqual(expectedTraderName, newEmptyTli.TraderName);
        Assert.AreEqual(1, newEmptyTli.TraderNameId);
        Assert.IsTrue(newEmptyTli.IsTraderNameUpdated);
        Assert.AreEqual(1, newEmptyTli.NameIdLookup.Count);
        Assert.IsFalse(newEmptyTli.IsEmpty);
        Assert.IsTrue(newEmptyTli.HasUpdates);

        Assert.AreEqual(emptyTli, newEmptyTli);
    }

    [TestMethod]
    public void EmptyTli_TradeVolumeChanged_ExpectedPropertiesUpdated()
    {
        Assert.AreEqual(0m, emptyTli.TraderVolume);
        Assert.IsFalse(emptyTli.IsTraderVolumeUpdated);
        Assert.IsTrue(emptyTli.IsEmpty);
        Assert.IsFalse(emptyTli.HasUpdates);
        var newEmptyTli = new PQTraderLayerInfo(emptyNameIdLookup.Clone());
        Assert.AreEqual(0m, newEmptyTli.TraderVolume);
        Assert.IsFalse(newEmptyTli.IsTraderVolumeUpdated);
        Assert.IsTrue(newEmptyTli.IsEmpty);
        Assert.IsFalse(newEmptyTli.HasUpdates);

        Assert.AreEqual(emptyTli, newEmptyTli);

        var expectedTraderVolume = 4_294_967_296;
        emptyTli.TraderVolume = expectedTraderVolume;

        Assert.AreEqual(expectedTraderVolume, emptyTli.TraderVolume);
        Assert.IsTrue(emptyTli.IsTraderVolumeUpdated);
        Assert.IsFalse(emptyTli.IsEmpty);
        Assert.IsTrue(emptyTli.HasUpdates);

        Assert.AreNotEqual(emptyTli, newEmptyTli);

        newEmptyTli.TraderVolume = expectedTraderVolume;

        Assert.AreEqual(expectedTraderVolume, newEmptyTli.TraderVolume);
        Assert.IsTrue(newEmptyTli.IsTraderVolumeUpdated);
        Assert.IsFalse(newEmptyTli.IsEmpty);
        Assert.IsTrue(newEmptyTli.HasUpdates);

        Assert.AreEqual(emptyTli, newEmptyTli);
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
        Assert.IsTrue(populatedTli.IsTraderNameUpdated);
        Assert.IsTrue(populatedTli.IsTraderVolumeUpdated);
        Assert.IsFalse(populatedTli.IsEmpty);
        populatedTli.StateReset();
        Assert.IsTrue(populatedTli.IsEmpty);
        Assert.IsNull(populatedTli.TraderName);
        Assert.AreEqual(0m, populatedTli.TraderVolume);
        Assert.IsTrue(populatedTli.IsTraderNameUpdated);
        Assert.IsTrue(populatedTli.IsTraderVolumeUpdated);
        Assert.IsTrue(populatedTli.IsEmpty);
    }

    [TestMethod]
    public void FullyPopulatedTli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQTraderLayerInfo = new TraderLayerInfo(populatedTli);
        emptyTli.CopyFrom(nonPQTraderLayerInfo);
        Assert.AreEqual(populatedTli, emptyTli);
    }

    [TestMethod]
    public void FullyPopulatedTli_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IMutableTraderLayerInfo)populatedTli).Clone();
        Assert.AreNotSame(clonedQuote, populatedTli);
        Assert.AreEqual(populatedTli, clonedQuote);

        var cloned2 = (PQTraderLayerInfo)((ICloneable)populatedTli).Clone();
        Assert.AreNotSame(cloned2, populatedTli);
        Assert.AreEqual(populatedTli, cloned2);

        var cloned3 = populatedTli.Clone();
        Assert.AreNotSame(cloned3, populatedTli);
        Assert.AreEqual(populatedTli, cloned3);
    }

    [TestMethod]
    public void FullyPopulatedTliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQTraderLayerInfo)((ICloneable)populatedTli).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedTli,
            fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedTli,
            fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedTliSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedTli, populatedTli);
        Assert.AreEqual(populatedTli, ((ICloneable)populatedTli).Clone());
        Assert.AreEqual(populatedTli, ((IMutableTraderLayerInfo)populatedTli).Clone());
        Assert.AreEqual(populatedTli, ((ICloneable<ITraderLayerInfo>)populatedTli).Clone());
        Assert.AreEqual(populatedTli, populatedTli.Clone());
    }

    [TestMethod]
    public void FullyPopulatedTli_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedTli.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedTli_ToString_ReturnsNameAndValues()
    {
        var toString = populatedTli.ToString()!;

        Assert.IsTrue(toString.Contains(populatedTli.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedTli.TraderName)}: {populatedTli.TraderName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedTli.TraderVolume)}: " +
                                        $"{populatedTli.TraderVolume:N2}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IPQTraderLayerInfo? original,
        IPQTraderLayerInfo? changingTraderLayerInfo,
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

        if (original.GetType() == typeof(PQTraderLayerInfo))
            Assert.AreEqual(!exactComparison, original.AreEquivalent(
                new TraderLayerInfo(changingTraderLayerInfo), exactComparison));

        TraderLayerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            original, changingTraderLayerInfo, originalTraderPriceVolumeLayer, changingTraderPriceVolumeLayer,
            originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingTraderLayerInfo.IsTraderNameUpdated = !changingTraderLayerInfo.IsTraderNameUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsTraderNameUpdated = original.IsTraderNameUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsTraderVolumeUpdated = !changingTraderLayerInfo.IsTraderVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsTraderVolumeUpdated = original.IsTraderVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
