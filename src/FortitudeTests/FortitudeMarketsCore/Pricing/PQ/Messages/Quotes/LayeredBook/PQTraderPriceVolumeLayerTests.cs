#region

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQTraderPriceVolumeLayerTests
{
    private const string TraderNameBase = "TestTraderName";
    private IPQNameIdLookupGenerator emptyNameIdLookup = null!;
    private IPQTraderPriceVolumeLayer emptyPvl = null!;
    private IPQNameIdLookupGenerator nameIdLookup = null!;
    private int populatedNumberOfTraders;
    private IPQTraderPriceVolumeLayer populatedPvl = null!;
    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        populatedNumberOfTraders = 3;
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        nameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        emptyPvl = new PQTraderPriceVolumeLayer(emptyNameIdLookup.Clone(), 0m);
        testDateTime = new DateTime(2017, 12, 17, 18, 54, 52);
        populatedPvl = new PQTraderPriceVolumeLayer(nameIdLookup, 4.2949_672m, 42_949_672m);
        AddTraderLayers(populatedPvl, populatedNumberOfTraders);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new PQTraderPriceVolumeLayer(nameIdLookup, 20, 40_000_000);
        AddTraderLayers(newPvl, populatedNumberOfTraders);
        Assert.AreEqual(20m, newPvl.Price);
        Assert.AreEqual(40_000_000m, newPvl.Volume);
        Assert.IsNotNull(newPvl.NameIdLookup);
        Assert.IsTrue(newPvl.IsPriceUpdated);
        Assert.IsTrue(newPvl.IsVolumeUpdated);
        Assert.IsTrue(newPvl.HasUpdates);
        AssertTraderLayersAreExpected(newPvl, new[] { true, true, true });

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.IsNotNull(emptyPvl.NameIdLookup);
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        AssertTraderLayersAreExpected(emptyPvl, new[] { false, false, false });

        var newEmptyPvl = new PQTraderPriceVolumeLayer(emptyNameIdLookup);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.IsNotNull(newEmptyPvl.NameIdLookup);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.HasUpdates);
        AssertTraderLayersAreExpected(newEmptyPvl, new[] { false, false, false });
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new PQTraderPriceVolumeLayer(nameIdLookup, 20, 40_000_000);
        AddTraderLayers(populatedPvl, populatedNumberOfTraders);
        var fromPQInstance = new PQTraderPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        AssertTraderLayersAreExpected(fromPQInstance);

        var nonPqvTraderPvl = new TraderPriceVolumeLayer(1.23456m, 5_123_456m);
        AddTraderLayers(nonPqvTraderPvl, populatedNumberOfTraders);
        var fromNonPqInstance = new PQTraderPriceVolumeLayer(nonPqvTraderPvl, emptyNameIdLookup.Clone());
        Assert.AreEqual(1.23456m, fromNonPqInstance.Price);
        Assert.AreEqual(5_123_456m, fromNonPqInstance.Volume);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        AssertTraderLayersAreExpected(fromNonPqInstance, new[] { true, true, true });

        var newEmptyPvl = new PQTraderPriceVolumeLayer(emptyPvl, emptyNameIdLookup);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        AssertTraderLayersAreExpected(newEmptyPvl);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedPvl = new PQTraderPriceVolumeLayer(nameIdLookup, 20, 40_000_000)
        {
            IsPriceUpdated = false
        };
        AddTraderLayers(newPopulatedPvl, populatedNumberOfTraders);
        var fromPQInstance = new PQTraderPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        AssertTraderLayersAreExpected(fromPQInstance, new[] { true, true, true });

        newPopulatedPvl = new PQTraderPriceVolumeLayer(nameIdLookup, 20, 40_000_000)
        {
            IsVolumeUpdated = false
        };
        AddTraderLayers(newPopulatedPvl, populatedNumberOfTraders);
        fromPQInstance = new PQTraderPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        AssertTraderLayersAreExpected(fromPQInstance, new[] { true, true, true });

        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            newPopulatedPvl = new PQTraderPriceVolumeLayer(nameIdLookup, 20, 40_000_000);
            AddTraderLayers(newPopulatedPvl, populatedNumberOfTraders);
            newPopulatedPvl[i]!.IsTraderNameUpdated = false;
            fromPQInstance = new PQTraderPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
            Assert.AreEqual(20m, fromPQInstance.Price);
            Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
            Assert.IsTrue(fromPQInstance.IsPriceUpdated);
            Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
            var expectedUpdated = Enumerable.Repeat(true, populatedNumberOfTraders).ToArray();
            expectedUpdated[i] = false;
            AssertTraderLayersAreExpected(fromPQInstance, new[] { true, true, true }, null, null, expectedUpdated);
        }

        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            newPopulatedPvl = new PQTraderPriceVolumeLayer(nameIdLookup, 20, 40_000_000);
            AddTraderLayers(newPopulatedPvl, populatedNumberOfTraders);
            newPopulatedPvl[i]!.IsTraderVolumeUpdated = false;
            fromPQInstance = new PQTraderPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
            Assert.AreEqual(20m, fromPQInstance.Price);
            Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
            Assert.IsTrue(fromPQInstance.IsPriceUpdated);
            Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
            var expectedUpdated = Enumerable.Repeat(true, populatedNumberOfTraders).ToArray();
            expectedUpdated[i] = false;
            AssertTraderLayersAreExpected(fromPQInstance, new[] { true, true, true }, null, null, null
                , expectedUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_IndexerGetSets_AddNewLayersIfIndexedViaVariousInterfaces()
    {
        var newEmpty = emptyPvl.Clone();
        Assert.AreEqual(0, newEmpty.Count);
        ((IMutableTraderPriceVolumeLayer)newEmpty)[3] =
            new PQTraderLayerInfo(newEmpty.NameIdLookup)
            {
                TraderName = TraderNameBase + 3, TraderVolume = 4_000_000
            };
        Assert.AreEqual(4, newEmpty.Count);
        Assert.AreEqual(TraderNameBase + 3, ((ITraderPriceVolumeLayer)newEmpty)[3]!.TraderName);
        Assert.AreEqual(4_000_000, ((ITraderPriceVolumeLayer)newEmpty)[3]!.TraderVolume);

        Assert.IsNotNull(((ITraderPriceVolumeLayer)newEmpty)[255]);
        Assert.AreEqual(4, newEmpty.Count);
        ((IMutableTraderPriceVolumeLayer)newEmpty)[255]!.TraderName = "NonEmptyTraderName";
        Assert.AreEqual(256, newEmpty.Count);

        newEmpty[255] = new PQTraderLayerInfo(newEmpty.NameIdLookup)
        {
            TraderName = TraderNameBase + 255, TraderVolume = 1_500_000
        };
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
    public void PopulatedPvl_HasUpdatesSetFalse_LookupAndTraderLayersHaveNoUpdates()
    {
        Assert.IsTrue(populatedPvl.HasUpdates);
        for (var i = 0; i < populatedNumberOfTraders; i++) Assert.IsTrue(populatedPvl[i]!.HasUpdates);
        Assert.IsTrue(populatedPvl.NameIdLookup.HasUpdates);

        populatedPvl.HasUpdates = false;

        Assert.IsFalse(populatedPvl.HasUpdates);
        for (var i = 0; i < populatedNumberOfTraders; i++) Assert.IsFalse(populatedPvl[i]!.HasUpdates);
        Assert.IsFalse(populatedPvl.NameIdLookup.HasUpdates);
    }

    [TestMethod]
    public void PopulatedPvl_SetTradersCountOnly_ExpectToBeCopied()
    {
        Assert.IsFalse(populatedPvl.IsTraderCountOnly);

        populatedPvl.SetTradersCountOnly(byte.MaxValue);
        Assert.AreEqual(byte.MaxValue, populatedPvl.Count);

        var newEmpty = new PQTraderPriceVolumeLayer(populatedPvl, emptyNameIdLookup.Clone());
        Assert.AreEqual(byte.MaxValue, newEmpty.Count);
        Assert.IsTrue(newEmpty.IsTraderCountOnly);
    }

    [TestMethod]
    public void EmptyPvl_LayerTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var traderLayerInfo = emptyPvl[i];

            Assert.IsFalse(traderLayerInfo!.IsTraderNameUpdated);
            Assert.IsFalse(emptyPvl.HasUpdates);
            Assert.AreEqual(null, traderLayerInfo.TraderName);
            Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

            var expectedTraderName = TraderNameBase + i;
            traderLayerInfo.TraderName = expectedTraderName;
            Assert.IsTrue(traderLayerInfo.IsTraderNameUpdated);
            Assert.IsTrue(emptyPvl.HasUpdates);
            Assert.AreEqual(expectedTraderName, traderLayerInfo.TraderName);
            var layerUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var indexShifted = i << 24;
            var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset,
                indexShifted | emptyPvl.NameIdLookup[traderLayerInfo.TraderName]);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            var stringUpdates = emptyPvl.GetStringUpdates(testDateTime, PQMessageFlags.Update)
                .ToList();
            Assert.AreEqual(1, stringUpdates.Count);
            var expectedStringUpdates = new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(
                    PQFieldKeys.LayerNameDictionaryUpsertCommand, 0u, PQFieldFlags.IsUpsert)
                , StringUpdate = new PQStringUpdate
                {
                    Command = CrudCommand.Upsert, DictionaryId = emptyPvl.NameIdLookup[traderLayerInfo.TraderName]
                    , Value = expectedTraderName
                }
            };
            Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

            traderLayerInfo.IsTraderNameUpdated = false;
            Assert.IsTrue(emptyPvl.HasUpdates);
            traderLayerInfo.NameIdLookup!.HasUpdates = false;
            Assert.IsFalse(emptyPvl.HasUpdates);
            Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

            traderLayerInfo.IsTraderNameUpdated = true;
            layerUpdates =
                (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                    where update.Id == PQFieldKeys.LayerTraderIdOffset
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            traderLayerInfo.TraderName = null;
            traderLayerInfo.IsTraderNameUpdated = false;

            var newEmpty = new PQTraderPriceVolumeLayer(emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            newEmpty.UpdateFieldString(stringUpdates[0]);
            var foundTraderInfo = newEmpty[i];
            Assert.AreEqual(expectedTraderName, foundTraderInfo!.TraderName);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsTraderNameUpdated);
        }
    }

    [TestMethod]
    public void EmptyPvl_LayerTraderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        for (var i = 0; i < 256; i++)
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var traderLayerInfo = emptyPvl[i];

            Assert.IsFalse(traderLayerInfo!.IsTraderVolumeUpdated);
            Assert.IsFalse(emptyPvl.HasUpdates);
            Assert.AreEqual(0m, traderLayerInfo.TraderVolume);
            Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

            var expectedTraderVolume = 100_000m + (i + 1);
            traderLayerInfo.TraderVolume = expectedTraderVolume;
            Assert.IsTrue(traderLayerInfo.IsTraderVolumeUpdated);
            Assert.IsTrue(emptyPvl.HasUpdates);
            Assert.AreEqual(expectedTraderVolume, traderLayerInfo.TraderVolume);
            var layerUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var indexShifted = i << 24;
            var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerTraderVolumeOffset,
                indexShifted | (int)expectedTraderVolume, 8);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);

            traderLayerInfo.IsTraderVolumeUpdated = false;
            Assert.IsFalse(emptyPvl.HasUpdates);
            Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

            traderLayerInfo.IsTraderVolumeUpdated = true;
            layerUpdates =
                (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                    where update.Id == PQFieldKeys.LayerTraderVolumeOffset
                    select update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            traderLayerInfo.TraderVolume = 0m;
            traderLayerInfo.IsTraderVolumeUpdated = false;

            var newEmpty = new PQTraderPriceVolumeLayer(emptyNameIdLookup.Clone());
            newEmpty.UpdateField(layerUpdates[0]);
            var foundTraderInfo = newEmpty[i];
            Assert.AreEqual(expectedTraderVolume, foundTraderInfo!.TraderVolume);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundTraderInfo.HasUpdates);
            Assert.IsTrue(foundTraderInfo.IsTraderVolumeUpdated);
        }
    }

    [TestMethod]
    public void PopulatedPvl_LayerTraderCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        populatedPvl.HasUpdates = false;
        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(populatedPvl.IsTraderCountOnly);
        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.AreEqual(0, populatedPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        populatedPvl.SetTradersCountOnly(byte.MaxValue);
        Assert.IsTrue(populatedPvl.HasUpdates);
        Assert.AreEqual(byte.MaxValue, populatedPvl.Count);
        var layerUpdates = populatedPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset,
            0x00800000 | byte.MaxValue);
        Assert.AreEqual(expectedLayerField, layerUpdates[0]);

        populatedPvl.HasUpdates = false;
        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.IsTrue(populatedPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        populatedPvl.HasUpdates = true;
        layerUpdates =
            (from update in populatedPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                where update.Id == PQFieldKeys.LayerTraderIdOffset
                select update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        Assert.AreEqual(expectedLayerField, layerUpdates[0]);

        var newEmpty = new PQTraderPriceVolumeLayer(emptyNameIdLookup.Clone());
        newEmpty.UpdateField(layerUpdates[0]);
        Assert.AreEqual(byte.MaxValue, newEmpty.Count);
        Assert.IsTrue(newEmpty.HasUpdates);
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
        var copyPopulated = populatedPvl.Clone();

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
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            Assert.AreNotEqual(null, populatedPvl[i]!.TraderName);
            Assert.AreNotEqual(0m, populatedPvl[i]!.TraderVolume);
            Assert.IsTrue(populatedPvl[i]!.IsTraderNameUpdated);
            Assert.IsTrue(populatedPvl[i]!.IsTraderVolumeUpdated);
            Assert.IsFalse(populatedPvl[i]!.IsEmpty);
        }

        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);
        Assert.IsFalse(populatedPvl.IsEmpty);
        populatedPvl.StateReset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            Assert.AreEqual(null, populatedPvl[i]!.TraderName);
            Assert.AreEqual(0m, populatedPvl[i]!.TraderVolume);
            Assert.IsTrue(populatedPvl[i]!.IsTraderNameUpdated);
            Assert.IsTrue(populatedPvl[i]!.IsTraderVolumeUpdated);
            Assert.IsTrue(populatedPvl[i]!.IsEmpty);
        }

        Assert.AreEqual(0, populatedPvl.Count);
        Assert.IsTrue(populatedPvl.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
    {
        var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);

        populatedPvl.SetTradersCountOnly(3);
        pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);

        populatedPvl.SetTradersCountOnly(3);
        populatedPvl.HasUpdates = false;
        pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        var pqStringUpdates = populatedPvl.GetStringUpdates(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        Assert.AreEqual(0, pqStringUpdates.Count);

        populatedPvl.SetTradersCountOnly(3);
        populatedPvl.HasUpdates = false;
        pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        pqStringUpdates = populatedPvl.GetStringUpdates(
            new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        Assert.AreEqual(0, pqStringUpdates.Count);
    }

    [TestMethod]
    public void PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates = populatedPvl.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 13, 33, 3), PQMessageFlags.Update | PQMessageFlags.Replay).ToList();
        var pqStringUpdates = populatedPvl.GetStringUpdates(
            new DateTime(2017, 11, 04, 13, 33, 3), PQMessageFlags.Update | PQMessageFlags.Replay).ToList();
        var newEmpty = new PQTraderPriceVolumeLayer(emptyNameIdLookup.Clone(), 0m);
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new TraderPriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQTraderPriceVolumeLayer(emptyNameIdLookup, 0m, 0m);
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.AreEqual(0, emptyPriceVolumeLayer.Count);
    }

    [TestMethod]
    public void LayerWithManyTraderDetails_CopyFromSmallerTraderPvl_ClearsDownExtraLayers()
    {
        var lotsOfTraders = new PQTraderPriceVolumeLayer(nameIdLookup, 4.2949_672m, 42_949_672m);
        AddTraderLayers(lotsOfTraders, 59);
        Assert.AreEqual(59, lotsOfTraders.Count);
        var smallerPvl = new PQTraderPriceVolumeLayer(nameIdLookup, 2.3456_78m, 2_949_672m);
        AddTraderLayers(smallerPvl, 1);
        Assert.AreEqual(1, smallerPvl.Count);

        lotsOfTraders.CopyFrom(smallerPvl);

        Assert.AreEqual(1, lotsOfTraders.Count);
    }

    [TestMethod]
    public void LayerWithManyTraderDetails_CopyFromNonPQSmallerTraderPvl_ClearsDownExtraLayers()
    {
        var lotsOfTraders = new PQTraderPriceVolumeLayer(nameIdLookup, 4.2949_672m, 42_949_672m);
        AddTraderLayers(lotsOfTraders, 59);
        Assert.AreEqual(59, lotsOfTraders.Count);
        var smallerPvl = new TraderPriceVolumeLayer(2.3456_78m, 2_949_672m);
        AddTraderLayers(smallerPvl, 1);
        Assert.AreEqual(1, smallerPvl.Count);

        lotsOfTraders.CopyFrom(smallerPvl);

        Assert.AreEqual(1, lotsOfTraders.Count);
    }

    [TestMethod]
    public void SomeTraderNameVolumeUpdates_CopyFrom_OnlyChangesUpdated()
    {
        const string newTraderName = "NewTraderName";
        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);

        var clonePopulated = populatedPvl.Clone();
        for (var i = 0; i < clonePopulated.Count; i++)
        {
            clonePopulated[i]!.TraderName = newTraderName + i;
            clonePopulated[i]!.TraderVolume = 50 * i;
        }

        clonePopulated[1]!.IsTraderNameUpdated = false;

        populatedPvl.HasUpdates = false;
        populatedPvl.CopyFrom(clonePopulated);

        Assert.AreEqual(newTraderName + 0, populatedPvl[0]!.TraderName);
        Assert.AreEqual(0, populatedPvl[0]!.TraderVolume);
        Assert.IsTrue(populatedPvl[0]!.IsTraderNameUpdated);
        Assert.IsTrue(populatedPvl[0]!.IsTraderVolumeUpdated);

        Assert.AreEqual(TraderNameBase + 1, populatedPvl[1]!.TraderName);
        Assert.AreEqual(50, populatedPvl[1]!.TraderVolume);
        Assert.IsFalse(populatedPvl[1]!.IsTraderNameUpdated);
        Assert.IsTrue(populatedPvl[1]!.IsTraderVolumeUpdated);

        Assert.AreEqual(newTraderName + 2, populatedPvl[2]!.TraderName);
        Assert.AreEqual(100, populatedPvl[2]!.TraderVolume);
        Assert.IsTrue(populatedPvl[2]!.IsTraderNameUpdated);
        Assert.IsTrue(populatedPvl[2]!.IsTraderVolumeUpdated);
    }

    [TestMethod]
    public void SomeTraderVolumeUpdates_CopyFrom_OnlyChangesUpdated()
    {
        const string newTraderName = "NewTraderName";
        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);

        var clonePopulated = populatedPvl.Clone();
        for (var i = 0; i < clonePopulated.Count; i++)
        {
            clonePopulated[i]!.TraderName = newTraderName + i;
            clonePopulated[i]!.TraderVolume = 50 * i;
        }

        clonePopulated[2]!.IsTraderVolumeUpdated = false;

        populatedPvl.HasUpdates = false;
        populatedPvl.CopyFrom(clonePopulated);

        Assert.AreEqual(newTraderName + 0, populatedPvl[0]!.TraderName);
        Assert.AreEqual(0, populatedPvl[0]!.TraderVolume);
        Assert.IsTrue(populatedPvl[0]!.IsTraderNameUpdated);
        Assert.IsTrue(populatedPvl[0]!.IsTraderVolumeUpdated);

        Assert.AreEqual(newTraderName + 1, populatedPvl[1]!.TraderName);
        Assert.AreEqual(50, populatedPvl[1]!.TraderVolume);
        Assert.IsTrue(populatedPvl[1]!.IsTraderNameUpdated);
        Assert.IsTrue(populatedPvl[1]!.IsTraderVolumeUpdated);

        Assert.AreEqual(newTraderName + 2, populatedPvl[2]!.TraderName);
        Assert.AreEqual(3_000_000, populatedPvl[2]!.TraderVolume);
        Assert.IsTrue(populatedPvl[2]!.IsTraderNameUpdated);
        Assert.IsFalse(populatedPvl[2]!.IsTraderVolumeUpdated);
    }

    [TestMethod]
    public void EmptyPvl_Construction_SetsTraderNameIdLookupWhenNullOrSameAsInfo()
    {
        Assert.AreEqual(populatedNumberOfTraders, nameIdLookup.Count);

        var moqSrcTkrQuoteInfo = new Mock<IPQSourceTickerQuoteInfo>();
        moqSrcTkrQuoteInfo.SetupGet(stqi => stqi.NameIdLookup!).Returns(emptyNameIdLookup);

        var newEmpty = new PQTraderPriceVolumeLayer(emptyNameIdLookup);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        Assert.AreSame(emptyNameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void EmptyPvlMissingLookup_Construction_SetsTraderNameIdLookupWhenNullOrSameAsInfo()
    {
        Assert.AreEqual(populatedNumberOfTraders, nameIdLookup.Count);

        var moqSrcTkrQuoteInfo = new Mock<IPQSourceTickerQuoteInfo>();
        moqSrcTkrQuoteInfo.SetupGet(stqi => stqi.NameIdLookup!).Returns(emptyNameIdLookup);

        var newEmpty = new PQTraderPriceVolumeLayer(new PQNameIdLookupGenerator(0));

        Assert.IsNotNull(newEmpty.NameIdLookup);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        newEmpty.NameIdLookup = emptyNameIdLookup;
        Assert.AreSame(emptyNameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void EmptyPvl_Construction_SharesTraderNameIdLookupBetweenLayers()
    {
        var newEmpty = new PQTraderPriceVolumeLayer(emptyNameIdLookup);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        Assert.AreSame(emptyNameIdLookup, newEmpty.NameIdLookup);
        newEmpty.NameIdLookup = populatedPvl.NameIdLookup;
        Assert.AreSame(populatedPvl.NameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IMutableTraderPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);

        var cloned2 = (PQTraderPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned2, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned2);

        var cloned3 = populatedPvl.Clone();
        Assert.AreNotSame(cloned3, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned3);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQTraderPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
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
        Assert.AreEqual(populatedPvl, ((IMutableTraderPriceVolumeLayer)populatedPvl).Clone());
        Assert.AreEqual(populatedPvl, ((ICloneable<ITraderPriceVolumeLayer>)populatedPvl).Clone());
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
        Assert.IsTrue(toString.Contains("TraderDetails: ["));
        for (var i = 0; i < populatedPvl.Count; i++) Assert.IsTrue(toString.Contains(populatedPvl[i]!.ToString()!));
    }

    [TestMethod]
    public void FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries()
    {
        Assert.AreEqual(populatedNumberOfTraders, populatedPvl.Count);
        Assert.AreEqual(populatedNumberOfTraders, ((IEnumerable<IPQTraderLayerInfo>)populatedPvl).Count());
        Assert.AreEqual(populatedNumberOfTraders, ((IEnumerable<IMutableTraderLayerInfo>)populatedPvl).Count());
        Assert.AreEqual(populatedNumberOfTraders, ((IEnumerable<ITraderLayerInfo>)populatedPvl).Count());

        populatedPvl.StateReset();

        Assert.AreEqual(0, populatedPvl.Count);
        Assert.AreEqual(0, ((IEnumerable<IPQTraderLayerInfo>)populatedPvl).Count());
        Assert.AreEqual(0, ((IEnumerable<IMutableTraderLayerInfo>)populatedPvl).Count());
        Assert.AreEqual(0, ((IEnumerable<ITraderLayerInfo>)populatedPvl).Count());
    }

    public static void AssertContainsAllPvlFields(IList<PQFieldUpdate> checkFieldUpdates,
        IPQTraderPriceVolumeLayer pvl)
    {
        PQPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl);

        for (var i = 0; i < pvl.Count; i++)
        {
            var traderLayerInfo = pvl[i];
            var indexShifted = (uint)i << 24;
            if (traderLayerInfo!.TraderName == PQTraderPriceVolumeLayer.TraderCountTraderName)
            {
                Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset,
                        0x0080_0000 | pvl.Count),
                    PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                        PQFieldKeys.LayerTraderIdOffset, 0x0080_0000, 0x0080_0000),
                    $"For {pvl.GetType().Name} ");
                return;
            }

            Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerTraderIdOffset,
                    indexShifted | (uint)traderLayerInfo.TraderNameId),
                PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    PQFieldKeys.LayerTraderIdOffset, indexShifted, 0xFF00_0000),
                $"For {pvl.GetType().Name} ");

            var value = PQScaling.AutoScale(traderLayerInfo.TraderVolume, 6, out var flag);
            Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerTraderVolumeOffset,
                    indexShifted | value, flag),
                PQLevel2QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                    PQFieldKeys.LayerTraderVolumeOffset, indexShifted, 0xFF00_0000, flag),
                $"For {pvl.GetType().Name} ");
        }
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        IPQTraderPriceVolumeLayer? original,
        IPQTraderPriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        PQPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
            original, changingPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQTraderPriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                changingPriceVolumeLayer.AreEquivalent(new TraderPriceVolumeLayer(original), exactComparison));

        Assert.AreEqual(original.Count, changingPriceVolumeLayer.Count);
        for (var i = 0; i < original.Count; i++)
        {
            var originalTraderInfo = original[i];
            var changingTraderInfo = changingPriceVolumeLayer[i];

            Assert.AreEqual(originalTraderInfo != null, changingTraderInfo != null);
            if (originalTraderInfo is PQTraderLayerInfo pqOriginalTraderInfo)
                PQTraderLayerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
                    pqOriginalTraderInfo, (PQTraderLayerInfo)changingTraderInfo!, original, changingPriceVolumeLayer,
                    originalOrderBook, changingOrderBook, originalQuote, changingQuote);
        }
    }

    public static void AddTraderLayers(IMutableTraderPriceVolumeLayer addTraderLayers, int numberOfTraderLayersToCrete)
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
                Assert.AreEqual(traderName, checkTraderLayers[i]!.TraderName);
                Assert.AreEqual(expectedVolume, checkTraderLayers[i]!.TraderVolume);

                if (checkTraderLayers is IPQTraderPriceVolumeLayer pqCheckTraderLayers)
                {
                    var expectedTraderUpdate = expectedTraderNameUpdated?[i] ?? true;
                    var expectedVolumeUpdate = expectedVolumeUpdated?[i] ?? true;
                    Assert.AreEqual(expectedTraderUpdate, pqCheckTraderLayers[i]!.IsTraderNameUpdated);
                    Assert.AreEqual(expectedVolumeUpdate, pqCheckTraderLayers[i]!.IsTraderVolumeUpdated);
                }
            }
            else
            {
                Assert.AreEqual(null, checkTraderLayers[i]!.TraderName);
                Assert.AreEqual(0m, checkTraderLayers[i]!.TraderVolume);

                if (checkTraderLayers is IPQTraderPriceVolumeLayer pqCheckTraderLayers)
                {
                    var expectedTraderUpdate = expectedTraderNameUpdated?[i] ?? false;
                    var expectedVolumeUpdate = expectedVolumeUpdated?[i] ?? false;
                    Assert.AreEqual(expectedTraderUpdate, pqCheckTraderLayers[i]!.IsTraderNameUpdated);
                    Assert.AreEqual(expectedVolumeUpdate, pqCheckTraderLayers[i]!.IsTraderVolumeUpdated);
                }
            }
    }
}
