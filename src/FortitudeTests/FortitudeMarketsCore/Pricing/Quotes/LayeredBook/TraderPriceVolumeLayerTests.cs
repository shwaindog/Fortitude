#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
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
public class TraderPriceVolumeLayerTests
{
    private const string TraderNameBase = "TestTraderName";
    private TraderPriceVolumeLayer emptyPvl = null!;
    private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;
    private int populatedNumberOfTraders;
    private TraderPriceVolumeLayer populatedPvl = null!;

    [TestInitialize]
    public void SetUp()
    {
        nameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        populatedNumberOfTraders = 3;
        emptyPvl = new TraderPriceVolumeLayer();
        populatedPvl = new TraderPriceVolumeLayer(4.2949_672m, 42_949_672m);
        AddTraderLayers(populatedPvl, populatedNumberOfTraders);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new TraderPriceVolumeLayer(20, 40_000_000);
        AddTraderLayers(newPvl, populatedNumberOfTraders);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        AssertTraderLayersAreExpected(newPvl, new[] { true, true, true });

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        AssertTraderLayersAreExpected(emptyPvl, new[] { false, false, false });

        var newEmptyPvl = new TraderPriceVolumeLayer();
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        AssertTraderLayersAreExpected(newEmptyPvl, new[] { false, false, false });
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new TraderPriceVolumeLayer(20, 40_000_000);
        AddTraderLayers(populatedPvl, populatedNumberOfTraders);
        var fromPQInstance = new TraderPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        AssertTraderLayersAreExpected(fromPQInstance);

        var pqTraderPvl = new PQTraderPriceVolumeLayer(nameIdLookupGenerator.Clone(), 1.23456m, 5_123_456m);
        AddTraderLayers(pqTraderPvl, populatedNumberOfTraders);
        var fromPqInstance = new TraderPriceVolumeLayer(pqTraderPvl);
        Assert.AreEqual(1.23456m, fromPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromPqInstance.Volume);
        AssertTraderLayersAreExpected(fromPqInstance, new[] { true, true, true });

        var newEmptyPvl = new PQTraderPriceVolumeLayer(emptyPvl, nameIdLookupGenerator);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        AssertTraderLayersAreExpected(newEmptyPvl);
    }

    [TestMethod]
    public void EmptyPvl_IndexerGetSets_AddNewLayersIfIndexedViaVariousInterfaces()
    {
        var newEmpty = (IMutableTraderPriceVolumeLayer)emptyPvl.Clone();
        Assert.AreEqual(0, newEmpty.Count);
        newEmpty[3] = new TraderLayerInfo(TraderNameBase + 3, 4_000_000);
        Assert.AreEqual(4, newEmpty.Count);
        Assert.AreEqual(TraderNameBase + 3, newEmpty[3]!.TraderName);
        Assert.AreEqual(4_000_000, ((ITraderPriceVolumeLayer)newEmpty)[3]!.TraderVolume);

        Assert.IsNotNull(((ITraderPriceVolumeLayer)newEmpty)[255]);
        Assert.AreEqual(4, newEmpty.Count);
        newEmpty[255]!.TraderName = "NonEmptyTraderName";
        Assert.AreEqual(256, newEmpty.Count);

        newEmpty[255] = new TraderLayerInfo(TraderNameBase + 255, 1_500_000);
        Assert.AreEqual(TraderNameBase + 255, newEmpty[255]!.TraderName);
        Assert.AreEqual(1_500_000, newEmpty[255]!.TraderVolume);
    }

    [TestMethod]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    public void EmptyPvl_IndexerGetSetOutOfIndex_ThrowsArgumentOutOfRangeException()
    {
        var caughtException = false;
        try
        {
            var neverSet = ((ITraderPriceVolumeLayer)emptyPvl)[256];
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);

        caughtException = false;
        try
        {
            var neverSet = ((IMutableTraderPriceVolumeLayer)emptyPvl)[256];
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);

        caughtException = false;
        try
        {
            ((IMutableTraderPriceVolumeLayer)emptyPvl)[256] = null;
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);

        caughtException = false;
        try
        {
            var neverSet = emptyPvl[256];
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);

        caughtException = false;
        try
        {
            emptyPvl[256] = null;
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
    }

    [TestMethod]
    public void PopulatedPvl_IsTraderCountOnly_TrueWhenSetTradersCountOnly()
    {
        Assert.IsFalse(populatedPvl.IsTraderCountOnly);

        populatedPvl.SetTradersCountOnly(byte.MaxValue);
        Assert.AreEqual(byte.MaxValue, populatedPvl.Count);

        var newEmpty = new TraderPriceVolumeLayer(populatedPvl);
        Assert.AreEqual(byte.MaxValue, newEmpty.Count);
        Assert.IsTrue(newEmpty.IsTraderCountOnly);
    }

    [TestMethod]
    public void PopulatedPvl_AddAndReset_AddsToLastNonEmptySlot()
    {
        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);
        populatedPvl.Add("NewTraderAtEnd", 12345m);
        Assert.AreEqual(populatedNumberOfTraders + 1, populatedPvl.Count);
        populatedPvl[populatedNumberOfTraders]?.StateReset();
        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);
        populatedPvl.Add("NewTraderAtEnd", 12345m);
        Assert.AreEqual(populatedNumberOfTraders + 1, populatedPvl.Count);
        populatedPvl.StateReset();
        Assert.AreEqual(0, populatedPvl.Count);
        populatedPvl.Add("NewTraderAtEnd", 12345m);
        Assert.AreEqual(1, populatedPvl.Count);
    }

    [TestMethod]
    public void PopulatedPvl_RemoveAt_ClearsOrReducesCount()
    {
        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);
        populatedPvl.RemoveAt(1);
        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);
        Assert.IsTrue(populatedPvl[1]!.IsEmpty);
        populatedPvl.RemoveAt(2);
        Assert.AreEqual(1, populatedPvl.Count);
        Assert.IsTrue(populatedPvl[2]!.IsEmpty);
    }

    [TestMethod]
    public void EmptyPvl_SetTradersCountOnly_UpdatesCount()
    {
        Assert.AreEqual(0, emptyPvl.Count);
        emptyPvl.SetTradersCountOnly(byte.MaxValue);
        Assert.AreEqual(byte.MaxValue, emptyPvl.Count);

        var caughtException = false;
        try
        {
            emptyPvl.SetTradersCountOnly(byte.MaxValue + 1);
        }
        catch (ArgumentOutOfRangeException)
        {
            caughtException = true;
        }

        Assert.IsTrue(caughtException);
        Assert.AreEqual(byte.MaxValue, emptyPvl.Count);

        emptyPvl.SetTradersCountOnly(0);
        Assert.AreEqual(0, emptyPvl.Count);
    }

    [TestMethod]
    public void PopulatedPvl_IsTraderCountOnly_DeterminesIfTraderCountOnlyWhenSet()
    {
        var copyPopulated = (IMutableTraderPriceVolumeLayer)populatedPvl.Clone();

        Assert.AreEqual(populatedNumberOfTraders, copyPopulated.Count);
        Assert.IsFalse(copyPopulated.IsTraderCountOnly);
        copyPopulated.StateReset();
        Assert.AreEqual(0, copyPopulated.Count);
        Assert.IsTrue(copyPopulated.IsTraderCountOnly);
        copyPopulated.SetTradersCountOnly(50);
        Assert.AreEqual(50, copyPopulated.Count);
        Assert.IsTrue(copyPopulated.IsTraderCountOnly);
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
        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            Assert.AreNotEqual(null, populatedPvl[i]!.TraderName);
            Assert.AreNotEqual(0m, populatedPvl[i]!.TraderVolume);
            Assert.IsFalse(populatedPvl[i]!.IsEmpty);
        }

        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);
        Assert.IsFalse(populatedPvl.IsEmpty);
        populatedPvl.StateReset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            Assert.AreEqual(null, populatedPvl[i]!.TraderName);
            Assert.AreEqual(0m, populatedPvl[i]!.TraderVolume);
            Assert.IsTrue(populatedPvl[i]!.IsEmpty);
        }

        Assert.AreEqual(0, populatedPvl.Count);
        Assert.IsTrue(populatedPvl.IsEmpty);
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
        var pqPvl = new PQTraderPriceVolumeLayer(populatedPvl, nameIdLookupGenerator);
        var newEmpty = new TraderPriceVolumeLayer();
        newEmpty.CopyFrom(pqPvl);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void LayerWithManyTraderDetails_CopyFromSmallerTraderPvl_ClearsDownExtraLayers()
    {
        var lotsOfTraders = new PQTraderPriceVolumeLayer(nameIdLookupGenerator.Clone(), 4.2949_672m, 42_949_672m);
        AddTraderLayers(lotsOfTraders, 59);
        Assert.AreEqual(59, lotsOfTraders.Count);
        var smallerPvl = new PQTraderPriceVolumeLayer(nameIdLookupGenerator, 2.3456_78m, 2_949_672m);
        AddTraderLayers(smallerPvl, 1);
        Assert.AreEqual(1, smallerPvl.Count);

        lotsOfTraders.CopyFrom(smallerPvl);

        Assert.AreEqual(1, lotsOfTraders.Count);
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
        clone = ((ICloneable<ITraderPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((ITraderPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
        clone = ((IMutableTraderPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clone, populatedPvl);
        Assert.AreEqual(populatedPvl, clone);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableTraderPriceVolumeLayer)populatedPvl).Clone();
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
        Assert.IsTrue(toString.Contains("TraderDetails:["));
        for (var i = 0; i < populatedPvl.Count; i++) Assert.IsTrue(toString.Contains(populatedPvl[i]!.ToString()!));
    }

    [TestMethod]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);
        Assert.AreEqual(populatedNumberOfTraders, ((IEnumerable<ITraderLayerInfo>)populatedPvl).Count());
        Assert.AreEqual(populatedNumberOfTraders, ((IEnumerable<IMutableTraderLayerInfo>)populatedPvl).Count());
        // ReSharper disable RedundantCast
        Assert.AreEqual(populatedNumberOfTraders, ((IEnumerable)populatedPvl).Cast<ITraderLayerInfo>().Count());

        populatedPvl.StateReset();

        Assert.AreEqual(0, populatedPvl.Count);
        Assert.AreEqual(0, ((IEnumerable<IMutableTraderLayerInfo>)populatedPvl).Count());
        Assert.AreEqual(0, ((IEnumerable<ITraderLayerInfo>)populatedPvl).Count());
        Assert.AreEqual(0, ((IEnumerable)populatedPvl).Cast<ITraderLayerInfo>().Count());
        // ReSharper restore RedundantCast
    }

    public static void AddTraderLayers(IMutableTraderPriceVolumeLayer addTraderLayers,
        int numberOfTraderLayersToCrete)
    {
        for (var i = 0; i < numberOfTraderLayersToCrete; i++)
        {
            addTraderLayers[i]!.TraderName = TraderNameBase + i;
            addTraderLayers[i]!.TraderVolume = (i + 1) * 1_000_000;
        }
    }

    public static void AssertTraderLayersAreExpected(ITraderPriceVolumeLayer checkTraderLayers,
        bool[]? expectPopulated = null, string[]? expectedTraderNames = null, decimal[]? expectedVolumes = null,
        bool[]? expectedTraderNameUpdated = null, bool[]? expectedVolumeUpdated = null)
    {
        if (expectPopulated == null || expectPopulated.Length == 0) Assert.AreEqual(0, checkTraderLayers.Count);
        for (var i = 0; i < checkTraderLayers.Count; i++)
            if (expectPopulated == null || expectPopulated[i])
            {
                var traderName = expectedTraderNames?[i] ?? TraderNameBase + i;
                var expectedVolume = expectedVolumes?[i] ?? 1_000_000 * (i + 1);
                Assert.AreEqual(traderName, checkTraderLayers[i]?.TraderName);
                Assert.AreEqual(expectedVolume, checkTraderLayers[i]?.TraderVolume);
            }
            else
            {
                Assert.AreEqual(null, checkTraderLayers[i]?.TraderName);
                Assert.AreEqual(0m, checkTraderLayers[i]?.TraderVolume);
            }
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IMutableTraderPriceVolumeLayer? original,
        IMutableTraderPriceVolumeLayer? changingPriceVolumeLayer,
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

        Assert.AreEqual(original.Count, changingPriceVolumeLayer.Count);
        for (var i = 0; i < original.Count; i++)
        {
            var originalTraderInfo = original[i];
            var changingTraderInfo = changingPriceVolumeLayer[i];

            Assert.AreEqual(originalTraderInfo != null, changingTraderInfo != null);
            TraderLayerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
                originalTraderInfo, changingTraderInfo, original, changingPriceVolumeLayer,
                originalOrderBook, changingOrderBook, originalQuote, changingQuote);
        }
    }
}
