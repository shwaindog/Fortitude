#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

[TestClass]
public class SourceQuoteRefTraderValueDatePriceVolumeLayerTests
{
    private SourceQuoteRefTraderValueDatePriceVolumeLayer emptyPvl = null!;
    private int populatedNumberOfTraders;
    private SourceQuoteRefTraderValueDatePriceVolumeLayer populatedPvl = null!;
    private decimal populatedQuotePrice;
    private uint populatedQuoteRef;
    private decimal populatedQuoteVolume;
    private string populatedSourceName = null!;
    private DateTime populatedValueDate;

    [TestInitialize]
    public void SetUp()
    {
        populatedNumberOfTraders = 2;
        populatedSourceName = "TestSourceName";
        emptyPvl = new SourceQuoteRefTraderValueDatePriceVolumeLayer();
        populatedValueDate = new DateTime(2017, 12, 26, 21, 00, 00); // only to the nearest hour.
        populatedQuoteRef = 4_2949_672u;
        populatedQuotePrice = 4.2949_672m;
        populatedQuoteVolume = 42_949_672m;
        populatedPvl = new SourceQuoteRefTraderValueDatePriceVolumeLayer(populatedQuotePrice,
            populatedQuoteVolume, populatedValueDate, populatedSourceName, true, populatedQuoteRef);
        TraderPriceVolumeLayerTests.AddTraderLayers(populatedPvl, populatedNumberOfTraders);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        Assert.AreEqual(populatedQuotePrice, populatedPvl.Price);
        Assert.AreEqual(populatedQuoteVolume, populatedPvl.Volume);
        Assert.AreEqual(populatedValueDate, populatedPvl.ValueDate);
        Assert.AreEqual(populatedSourceName, populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.AreEqual(populatedQuoteRef, populatedPvl.SourceQuoteReference);
        TraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(populatedPvl, new[] { true, true });

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPvl.ValueDate);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.IsFalse(emptyPvl.Executable);
        Assert.AreEqual(0u, emptyPvl.SourceQuoteReference);
        TraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(emptyPvl, new[] { false, false });

        var newEmptyPvl = new SourceQuoteRefTraderValueDatePriceVolumeLayer();
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, newEmptyPvl.ValueDate);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
        TraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(newEmptyPvl, new[] { false, false });
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new SourceQuoteRefTraderValueDatePriceVolumeLayer(populatedQuotePrice,
            populatedQuoteVolume, populatedValueDate, populatedSourceName, true, populatedQuoteRef);
        TraderPriceVolumeLayerTests.AddTraderLayers(populatedPvl, populatedNumberOfTraders);
        var fromPQInstance = new SourceQuoteRefTraderValueDatePriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
        TraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(fromPQInstance);

        var pqTraderPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(populatedQuotePrice,
            populatedQuoteVolume, null, null, populatedValueDate, populatedSourceName, true, populatedQuoteRef);
        TraderPriceVolumeLayerTests.AddTraderLayers(pqTraderPvl, populatedNumberOfTraders);
        var fromNonPqInstance = new SourceQuoteRefTraderValueDatePriceVolumeLayer(pqTraderPvl);
        Assert.AreEqual(populatedQuotePrice, fromNonPqInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromNonPqInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromNonPqInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromNonPqInstance.SourceName);
        Assert.IsTrue(fromNonPqInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromNonPqInstance.SourceQuoteReference);
        TraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(fromNonPqInstance, new[] { true, true });

        var newEmptyPvl = new SourceQuoteRefTraderValueDatePriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, newEmptyPvl.ValueDate);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
        TraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(newEmptyPvl, new[] { false, false });
    }

    [TestMethod]
    public void PopulatedSrcQuoteRefPvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new SourceQuoteRefPriceVolumeLayer(populatedPvl);
        var emptyClone = new SourceQuoteRefTraderValueDatePriceVolumeLayer(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcQtRefTrdVlDtPvl = new PQSourceQuoteRefPriceVolumeLayer(populatedPvl);
        emptyClone = new SourceQuoteRefTraderValueDatePriceVolumeLayer(pqSrcQtRefTrdVlDtPvl);
        pqSrcQtRefTrdVlDtPvl.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedSrcNamePvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new SourcePriceVolumeLayer(populatedPvl);
        var emptyClone = new SourceQuoteRefTraderValueDatePriceVolumeLayer(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcPvl = new PQSourcePriceVolumeLayer(populatedPvl);
        emptyClone = new SourceQuoteRefTraderValueDatePriceVolumeLayer(pqSrcPvl);
        pqSrcPvl.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedValueDatePvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new ValueDatePriceVolumeLayer(populatedPvl);
        var emptyClone = new SourceQuoteRefTraderValueDatePriceVolumeLayer(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqValueDatePvl = new ValueDatePriceVolumeLayer(populatedPvl);
        emptyClone = new SourceQuoteRefTraderValueDatePriceVolumeLayer(pqValueDatePvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedTraderPvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new TraderPriceVolumeLayer(populatedPvl);
        var emptyClone = new SourceQuoteRefTraderValueDatePriceVolumeLayer(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqTrdPvl = new TraderPriceVolumeLayer(populatedPvl);
        emptyClone = new SourceQuoteRefTraderValueDatePriceVolumeLayer(pqTrdPvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void EmptyLayer_Mutate_UpdatesFields()
    {
        const decimal expectedPrice = 3.45678m;
        const decimal expectedVolume = 5.67890m;
        const string expectedSourceName = "NewSourceName";
        const uint expectedSrcQuoteRef = 612656;
        var expectedValueDate = new DateTime(2018, 2, 10, 18, 0, 0);

        emptyPvl.Price = expectedPrice;
        emptyPvl.Volume = expectedVolume;
        emptyPvl.SourceName = expectedSourceName;
        emptyPvl.Executable = true;
        emptyPvl.SourceQuoteReference = expectedSrcQuoteRef;
        emptyPvl.ValueDate = expectedValueDate;

        Assert.AreEqual(expectedPrice, emptyPvl.Price);
        Assert.AreEqual(expectedVolume, emptyPvl.Volume);
        Assert.AreEqual(expectedSourceName, emptyPvl.SourceName);
        Assert.IsTrue(emptyPvl.Executable);
        Assert.AreEqual(expectedSrcQuoteRef, emptyPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, emptyPvl.ValueDate);
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
        Assert.AreNotEqual(DateTimeConstants.UnixEpoch, populatedPvl.ValueDate);
        Assert.AreNotEqual(null, populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.AreEqual(populatedQuoteRef, populatedPvl.SourceQuoteReference);
        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            Assert.AreNotEqual(null, populatedPvl[i]?.TraderName);
            Assert.AreNotEqual(0m, populatedPvl[i]?.TraderVolume);
            Assert.IsFalse(populatedPvl[i]!.IsEmpty);
        }

        populatedPvl.StateReset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, populatedPvl.ValueDate);
        Assert.AreEqual(null, populatedPvl.SourceName);
        Assert.IsFalse(populatedPvl.Executable);
        Assert.AreEqual(0u, populatedPvl.SourceQuoteReference);
        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            Assert.AreEqual(null, populatedPvl[i]?.TraderName);
            Assert.AreEqual(0m, populatedPvl[i]?.TraderVolume);
            Assert.IsTrue(populatedPvl[i]!.IsEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromNonPQSrcQtRefTrdrVlDtToEmptyQuote_PvlsEqualEachOther()
    {
        var pqPriceVolume = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(pqPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);

        var fromConstructor = new SourceQuoteRefTraderValueDatePriceVolumeLayer(pqPriceVolume);
        Assert.AreEqual(fromConstructor, populatedPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_NewAndCopyFromSrcQtRefToEmptyQuote_PvlsEqualEachOther()
    {
        var pqNonExactQuote = new PQSourceQuoteRefPriceVolumeLayer(populatedPvl);
        var emptyClone = (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(pqNonExactQuote);
        pqNonExactQuote.AreEquivalent(emptyClone);

        var pqSrcQtRefTrdVlDtPvl = new SourceQuoteRefPriceVolumeLayer(populatedPvl);
        emptyClone = (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(pqSrcQtRefTrdVlDtPvl);
        pqNonExactQuote.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromSrcPvlToEmptyQuote_PvlsEqualEachOther()
    {
        var pqNonExactPriceVolume = new PQSourcePriceVolumeLayer(populatedPvl);
        var emptyClone = (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(pqNonExactPriceVolume);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);

        var srcPvl = new SourcePriceVolumeLayer(populatedPvl);
        emptyClone = (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(srcPvl);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromValueDatePvlToEmptyQuote_PvlsEqualEachOther()
    {
        var pqNonExactPriceVolume = new PQValueDatePriceVolumeLayer(populatedPvl);
        var emptyClone = (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(pqNonExactPriceVolume);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);

        var valueDatePvl = new ValueDatePriceVolumeLayer(populatedPvl);
        emptyClone = (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(valueDatePvl);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromTraderPvlToEmptyQuote_PvlsEqualEachOther()
    {
        var pqNonExactPriceVolume = new PQTraderPriceVolumeLayer(populatedPvl);
        var emptyClone = (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(pqNonExactPriceVolume);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);

        var trdPvl = new TraderPriceVolumeLayer(populatedPvl);
        emptyClone = (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(trdPvl);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((ISourceQuoteRefTraderValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);
        var clonedQuote2 = ((IValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote2, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote2);
        var clonedQuote3 = ((ISourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote3, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote3);
        var clonedQuote4 = ((ICloneable<ISourcePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote4, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote4);
        var clonedQuote5 = ((IMutableSourcePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote5, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote5);
        var clonedQuote6 = ((ISourcePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote6, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote6);
        var clonedQuote8 = ((ICloneable<ISourceQuoteRefPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote8, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote8);
        var clonedQuote10 = ((ICloneable<ISourceQuoteRefTraderValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote10, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote10);
        var clonedQuote12 = ((ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote12, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote12);
        var clonedQuote13 = ((IMutableSourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote13, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote13);
        var clonedQuote14 = ((ICloneable<IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer>)
            populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote14, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote14);
        var clonedQuote15 = ((IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote15, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote15);
        var clonedQuote16 = ((ICloneable<IMutableValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote16, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote16);
        var clonedQuote17 = ((IMutableValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote17, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote17);
        var clonedQuote18 = ((ICloneable<IValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote18, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote18);
        var cloned20 = (SourceQuoteRefTraderValueDatePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned20, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned20);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)
            ((ICloneable)populatedPvl).Clone();
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.SourceQuoteReference)}: " +
                                        $"{populatedPvl.SourceQuoteReference:N0}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.ValueDate)}: {populatedPvl.ValueDate}"));
        Assert.IsTrue(toString.Contains("TraderDetails:["));
        for (var i = 0; i < populatedPvl.Count; i++) Assert.IsTrue(toString.Contains(populatedPvl[i]!.ToString()!));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer? original,
        IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        TraderPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            original, changingPriceVolumeLayer, originalOrderBook,
            changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.SourceName = "TestChangedSourceName";
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

        changingPriceVolumeLayer.SourceQuoteReference = 98765432;
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.SourceQuoteReference = original.SourceQuoteReference;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.ValueDate = new DateTime(2017, 11, 30, 21, 52, 02);
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.IsFalse(
                originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.ValueDate = original.ValueDate;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
