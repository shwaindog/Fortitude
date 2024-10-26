// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests
{
    private IPQNameIdLookupGenerator                        emptyNameIdLookup    = null!;
    private PQSourceQuoteRefTraderValueDatePriceVolumeLayer emptyPvl             = null!;
    private IPQNameIdLookupGenerator                        nonEmptyNameIdLookup = null!;

    private int populatedNumberOfTraders;

    private PQSourceQuoteRefTraderValueDatePriceVolumeLayer populatedPvl = null!;

    private decimal  populatedQuotePrice;
    private uint     populatedQuoteRef;
    private decimal  populatedQuoteVolume;
    private string   populatedSourceName = null!;
    private DateTime populatedValueDate;
    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        populatedNumberOfTraders = 2;
        populatedSourceName      = "TestSourceName";
        emptyNameIdLookup        = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        nonEmptyNameIdLookup     = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        emptyPvl                 = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookup.Clone(), 0m, 0m);
        testDateTime             = new DateTime(2017, 12, 17, 18, 54, 52);
        populatedValueDate       = new DateTime(2017, 12, 26, 21, 00, 00); // only to the nearest hour.
        populatedQuoteRef        = 4_2949_672u;
        populatedQuotePrice      = 4.2949_672m;
        populatedQuoteVolume     = 42_949_672m;
        populatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nonEmptyNameIdLookup.Clone(), populatedQuotePrice,
                                                                           populatedQuoteVolume, populatedValueDate, populatedSourceName, true
                                                                         , populatedQuoteRef);
        PQTraderPriceVolumeLayerTests.AddTraderLayers(populatedPvl, populatedNumberOfTraders);
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
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        Assert.IsTrue(populatedPvl.IsSourceQuoteReferenceUpdated);
        PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(populatedPvl, new[] { true, true });

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPvl.ValueDate);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.IsFalse(emptyPvl.Executable);
        Assert.AreEqual(0u, emptyPvl.SourceQuoteReference);
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
        Assert.IsFalse(emptyPvl.IsValueDateUpdated);
        Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(emptyPvl.IsExecutableUpdated);
        Assert.IsFalse(emptyPvl.IsSourceQuoteReferenceUpdated);
        PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(emptyPvl, new[] { false, false });

        var newEmptyPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookup);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, newEmptyPvl.ValueDate);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsValueDateUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(newEmptyPvl.IsExecutableUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceQuoteReferenceUpdated);
        PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(newEmptyPvl, new[] { false, false });
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer
            (nonEmptyNameIdLookup, populatedQuotePrice, populatedQuoteVolume, populatedValueDate,
             populatedSourceName, true, populatedQuoteRef);
        PQTraderPriceVolumeLayerTests.AddTraderLayers(populatedPvl, populatedNumberOfTraders);
        var fromPQInstance = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
        PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(fromPQInstance);

        var nonPqTraderPvl = new SourceQuoteRefTraderValueDatePriceVolumeLayer
            (populatedQuotePrice, populatedQuoteVolume, populatedValueDate, populatedSourceName, true, populatedQuoteRef);
        PQTraderPriceVolumeLayerTests.AddTraderLayers(nonPqTraderPvl, populatedNumberOfTraders);
        var fromNonPqInstance = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nonPqTraderPvl, emptyNameIdLookup.Clone());
        Assert.AreEqual(populatedQuotePrice, fromNonPqInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromNonPqInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromNonPqInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromNonPqInstance.SourceName);
        Assert.IsTrue(fromNonPqInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromNonPqInstance.SourceQuoteReference);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsValueDateUpdated);
        Assert.IsTrue(fromNonPqInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromNonPqInstance.IsExecutableUpdated);
        Assert.IsTrue(fromNonPqInstance.IsSourceQuoteReferenceUpdated);
        PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(fromNonPqInstance, new[] { true, true });

        var newEmptyPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookup.Clone());
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, newEmptyPvl.ValueDate);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsValueDateUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(newEmptyPvl.IsExecutableUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceQuoteReferenceUpdated);
        PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(newEmptyPvl, new[] { false, false });
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer
            (nonEmptyNameIdLookup, populatedQuotePrice, populatedQuoteVolume, populatedValueDate,
             populatedSourceName, true, populatedQuoteRef)
            {
                IsPriceUpdated = false
            };
        PQTraderPriceVolumeLayerTests.AddTraderLayers(newPopulatedPvl, populatedNumberOfTraders);
        var fromPQInstance = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
        PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(fromPQInstance, new[] { true, true });

        newPopulatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer
            (nonEmptyNameIdLookup, populatedQuotePrice, populatedQuoteVolume, populatedValueDate
           , populatedSourceName, true, populatedQuoteRef)
            {
                IsVolumeUpdated = false
            };
        PQTraderPriceVolumeLayerTests.AddTraderLayers(newPopulatedPvl, populatedNumberOfTraders);
        Assert.AreEqual(populatedQuotePrice, newPopulatedPvl.Price);
        Assert.AreEqual(populatedQuoteVolume, newPopulatedPvl.Volume);
        Assert.AreEqual(populatedValueDate, newPopulatedPvl.ValueDate);
        Assert.AreEqual(populatedSourceName, newPopulatedPvl.SourceName);
        Assert.IsTrue(newPopulatedPvl.Executable);
        Assert.AreEqual(populatedQuoteRef, newPopulatedPvl.SourceQuoteReference);
        Assert.IsTrue(newPopulatedPvl.IsPriceUpdated);
        Assert.IsFalse(newPopulatedPvl.IsVolumeUpdated);
        Assert.IsTrue(newPopulatedPvl.IsValueDateUpdated);
        Assert.IsTrue(newPopulatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(newPopulatedPvl.IsExecutableUpdated);
        Assert.IsTrue(newPopulatedPvl.IsSourceQuoteReferenceUpdated);
        PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(newPopulatedPvl, new[] { true, true });

        newPopulatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer
            (nonEmptyNameIdLookup, populatedQuotePrice, populatedQuoteVolume, populatedValueDate,
             populatedSourceName, true, populatedQuoteRef)
            {
                IsSourceNameUpdated = false
            };
        fromPQInstance = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsFalse(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
        PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(fromPQInstance);

        newPopulatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer
            (nonEmptyNameIdLookup, populatedQuotePrice, populatedQuoteVolume, populatedValueDate,
             populatedSourceName, true, populatedQuoteRef)
            {
                IsExecutableUpdated = false
            };
        fromPQInstance = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsFalse(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer
            (nonEmptyNameIdLookup, populatedQuotePrice, populatedQuoteVolume, populatedValueDate,
             populatedSourceName, true, populatedQuoteRef)
            {
                IsSourceQuoteReferenceUpdated = false
            };
        fromPQInstance = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsFalse(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer
            (nonEmptyNameIdLookup, populatedQuotePrice, populatedQuoteVolume, populatedValueDate,
             populatedSourceName, true, populatedQuoteRef)
            {
                IsValueDateUpdated = false
            };
        fromPQInstance = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            newPopulatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer
                (nonEmptyNameIdLookup, populatedQuotePrice, populatedQuoteVolume, populatedValueDate,
                 populatedSourceName, true, populatedQuoteRef);
            PQTraderPriceVolumeLayerTests.AddTraderLayers(newPopulatedPvl, populatedNumberOfTraders);
            newPopulatedPvl[i]!.IsTraderNameUpdated = false;
            fromPQInstance = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
            Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
            Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
            Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
            Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
            Assert.IsTrue(fromPQInstance.Executable);
            Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
            Assert.IsTrue(fromPQInstance.IsPriceUpdated);
            Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
            Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
            Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
            Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
            Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
            var expectedUpdated = Enumerable.Repeat(true, populatedNumberOfTraders).ToArray();
            expectedUpdated[i] = false;
            PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected
                (fromPQInstance, new[] { true, true }, null, null, expectedUpdated);
        }

        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            newPopulatedPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer
                (nonEmptyNameIdLookup, populatedQuotePrice, populatedQuoteVolume, populatedValueDate,
                 populatedSourceName, true, populatedQuoteRef);
            PQTraderPriceVolumeLayerTests.AddTraderLayers(newPopulatedPvl, populatedNumberOfTraders);
            newPopulatedPvl[i]!.IsTraderVolumeUpdated = false;
            fromPQInstance = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
            Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
            Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
            Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
            Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
            Assert.IsTrue(fromPQInstance.Executable);
            Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
            Assert.IsTrue(fromPQInstance.IsPriceUpdated);
            Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
            Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
            Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
            Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
            Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
            var expectedUpdated = Enumerable.Repeat(true, populatedNumberOfTraders).ToArray();
            expectedUpdated[i] = false;
            PQTraderPriceVolumeLayerTests.AssertTraderLayersAreExpected(fromPQInstance, new[] { true, true }, null,
                                                                        null, null, expectedUpdated);
        }
    }

    [TestMethod]
    public void PopulatedSrcQuoteRefPvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new SourceQuoteRefPriceVolumeLayer(populatedPvl);
        var emptyClone          = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nonExactPriceVolume, emptyNameIdLookup.Clone());
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcQtRefTrdVlDtPvl = new PQSourceQuoteRefPriceVolumeLayer(populatedPvl, populatedPvl.NameIdLookup);
        emptyClone = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(pqSrcQtRefTrdVlDtPvl, emptyNameIdLookup);
        pqSrcQtRefTrdVlDtPvl.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedSrcNamePvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new SourcePriceVolumeLayer(populatedPvl);
        var emptyClone          = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nonExactPriceVolume, emptyNameIdLookup.Clone());
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcPvl = new PQSourcePriceVolumeLayer(populatedPvl, populatedPvl.NameIdLookup);
        emptyClone = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(pqSrcPvl, emptyNameIdLookup);
        pqSrcPvl.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedValueDatePvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new ValueDatePriceVolumeLayer(populatedPvl);
        var emptyClone          = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nonExactPriceVolume, emptyNameIdLookup.Clone());
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqValueDatePvl = new ValueDatePriceVolumeLayer(populatedPvl);
        emptyClone = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(pqValueDatePvl, emptyNameIdLookup);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedTraderPvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new TraderPriceVolumeLayer(populatedPvl);
        var emptyClone          = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nonExactPriceVolume, emptyNameIdLookup.Clone());
        nonExactPriceVolume.AreEquivalent(emptyClone);


        var pqTrdPvl = new TraderPriceVolumeLayer(populatedPvl);
        emptyClone = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(pqTrdPvl, emptyNameIdLookup);
        nonExactPriceVolume.AreEquivalent(emptyClone);
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

        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.FirstLayerDateOffset,
                                                    expectedDateTime.GetHoursFromUnixEpoch(), PQFieldFlags.IsExtendedFieldId);
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
        expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.FirstLayerDateOffset,
                                                nextExpectedValueDate.GetHoursFromUnixEpoch(), PQFieldFlags.IsExtendedFieldId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.FirstLayerDateOffset
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookup);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedValueDate, newEmpty.ValueDate);
        Assert.IsTrue(newEmpty.IsValueDateUpdated);
    }

    [TestMethod]
    public void EmptyPvl_LayerSourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.AreEqual(0u, emptyPvl.SourceQuoteReference);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        emptyPvl.SourceQuoteReference = populatedQuoteRef;
        Assert.IsTrue(emptyPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(populatedQuoteRef, emptyPvl.SourceQuoteReference);
        var sourceLayerUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerSourceQuoteRefOffset,
                                                   populatedQuoteRef);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        emptyPvl.IsSourceQuoteReferenceUpdated = false;
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyPvl.IsSourceQuoteReferenceUpdated = true;
        sourceLayerUpdates =
            (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQFieldKeys.LayerSourceQuoteRefOffset
                select update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        var newEmpty = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookup);
        newEmpty.UpdateField(sourceLayerUpdates[0]);
        Assert.AreEqual(populatedQuoteRef, newEmpty.SourceQuoteReference);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsSourceQuoteReferenceUpdated);
    }

    [TestMethod]
    public void EmptyPvl_SourceNameChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        emptyPvl.SourceName = populatedSourceName;
        Assert.IsTrue(emptyPvl.IsSourceNameUpdated);
        Assert.AreEqual(emptyNameIdLookup[populatedSourceName], emptyPvl.SourceId);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(populatedSourceName, emptyPvl.SourceName);
        var sourceUpdates = emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset,
                                                    emptyPvl.SourceId);
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
        var stringUpdates = emptyPvl.GetStringUpdates(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(2, stringUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset,
                                                emptyPvl.SourceId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
        var expectedStringUpdates = new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(
                                      PQFieldKeys.LayerNameDictionaryUpsertCommand, 0u, PQFieldFlags.IsUpsert)
          , StringUpdate = new PQStringUpdate
            {
                Command = CrudCommand.Upsert, DictionaryId = emptyPvl.NameIdLookup[emptyPvl.SourceName]
              , Value   = emptyPvl.SourceName
            }
        };
        Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

        sourceUpdates = (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.LayerSourceIdOffset
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmptyNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        var newEmpty             = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(newEmptyNameIdLookup, 0m, 0m);
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
        var expectedLayerField = new PQFieldUpdate(PQFieldKeys.LayerBooleanFlagsOffset, PQFieldFlags.LayerExecutableFlag);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        emptyPvl.IsExecutableUpdated = false;
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyPvl.IsExecutableUpdated = true;
        sourceLayerUpdates =
            (from update in emptyPvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQFieldKeys.LayerBooleanFlagsOffset
                select update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        var newEmpty = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookup);
        newEmpty.UpdateField(sourceLayerUpdates[0]);
        Assert.IsTrue(newEmpty.Executable);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsExecutableUpdated);
    }

    [TestMethod]
    public void PopulatedPvl_HasUpdatesSetFalse_LookupAndLayerHaveNoUpdates()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.IsTrue(populatedPvl.NameIdLookup.HasUpdates);
        for (var i = 0; i < populatedNumberOfTraders; i++) Assert.IsTrue(populatedPvl[i]!.HasUpdates);
        Assert.IsTrue(emptyPvl.IsEmpty);

        populatedPvl.HasUpdates = false;

        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.IsFalse(populatedPvl.NameIdLookup.HasUpdates);
        for (var i = 0; i < populatedNumberOfTraders; i++) Assert.IsFalse(populatedPvl[i]!.HasUpdates);
        Assert.IsTrue(emptyPvl.IsEmpty);
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
        Assert.AreNotEqual(DateTimeConstants.UnixEpoch, populatedPvl.ValueDate);
        Assert.AreNotEqual(null, populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.AreEqual(populatedQuoteRef, populatedPvl.SourceQuoteReference);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        Assert.IsTrue(populatedPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            var sourceTraderLayer = populatedPvl[i]!;
            Assert.AreNotEqual(null, sourceTraderLayer.TraderName);
            Assert.AreNotEqual(0m, sourceTraderLayer.TraderVolume);
            Assert.IsTrue(sourceTraderLayer.IsTraderNameUpdated);
            Assert.IsTrue(sourceTraderLayer.IsTraderVolumeUpdated);
            Assert.IsFalse(sourceTraderLayer.IsEmpty);
        }

        populatedPvl.IsEmpty = true;
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, populatedPvl.ValueDate);
        Assert.AreEqual(null, populatedPvl.SourceName);
        Assert.IsFalse(populatedPvl.Executable);
        Assert.AreEqual(0u, populatedPvl.SourceQuoteReference);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        Assert.IsTrue(populatedPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
        for (var i = 0; i < populatedNumberOfTraders; i++)
        {
            var sourceTraderLayer = populatedPvl[i]!;
            Assert.AreEqual(null, sourceTraderLayer.TraderName);
            Assert.AreEqual(0m, sourceTraderLayer.TraderVolume);
            Assert.IsTrue(sourceTraderLayer.IsTraderNameUpdated);
            Assert.IsTrue(sourceTraderLayer.IsTraderVolumeUpdated);
            Assert.IsTrue(sourceTraderLayer.IsEmpty);
        }
    }

    [TestMethod]
    public void PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);

        populatedPvl.SetTradersCountOnly(3);
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

        populatedPvl.SetTradersCountOnly(3);
        populatedPvl.HasUpdates = false;
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
        var newEmpty = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookup.Clone(), 0m, 0m);
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromNonPQSrcQtRefTrdrVlDtToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new SourceQuoteRefTraderValueDatePriceVolumeLayer(populatedPvl);
        emptyPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);

        var emptyPqNameIdLookupGen = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        var fromConstructor        = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(nonPQPriceVolume, emptyPqNameIdLookupGen);
        Assert.AreEqual(fromConstructor, populatedPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_NewAndCopyFromSrcQtRefToEmptyQuote_PvlsEqualEachOther()
    {
        var nonExactPriceVolume = new SourceQuoteRefPriceVolumeLayer(populatedPvl);
        var emptyClone          = emptyPvl.Clone();
        emptyClone.CopyFrom(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcQtRefTrdVlDtPvl = new PQSourceQuoteRefPriceVolumeLayer(populatedPvl, populatedPvl.NameIdLookup);
        emptyClone = emptyPvl.Clone();
        emptyClone.CopyFrom(pqSrcQtRefTrdVlDtPvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromSrcPvlToEmptyQuote_PvlsEqualEachOther()
    {
        var nonExactPriceVolume = new SourcePriceVolumeLayer(populatedPvl);
        var emptyClone          = emptyPvl.Clone();
        emptyClone.CopyFrom(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcPvl = new PQSourcePriceVolumeLayer(populatedPvl, populatedPvl.NameIdLookup);
        emptyClone = emptyPvl.Clone();
        emptyClone.CopyFrom(pqSrcPvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromValueDatePvlToEmptyQuote_PvlsEqualEachOther()
    {
        var nonExactPriceVolume = new ValueDatePriceVolumeLayer(populatedPvl);
        var emptyClone          = emptyPvl.Clone();
        emptyClone.CopyFrom(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqValueDatePvl = new PQValueDatePriceVolumeLayer(populatedPvl);
        emptyClone = emptyPvl.Clone();
        emptyClone.CopyFrom(pqValueDatePvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromTraderPvlToEmptyQuote_PvlsEqualEachOther()
    {
        var nonExactPriceVolume = new TraderPriceVolumeLayer(populatedPvl);
        var emptyClone          = emptyPvl.Clone();
        emptyClone.CopyFrom(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqTrdPvl = new PQTraderPriceVolumeLayer(populatedPvl, populatedPvl.NameIdLookup);
        emptyClone = emptyPvl.Clone();
        emptyClone.CopyFrom(pqTrdPvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookup);
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyPriceVolumeLayer.ValueDate);
        Assert.AreEqual(null, emptyPriceVolumeLayer.SourceName);
        Assert.IsFalse(emptyPriceVolumeLayer.Executable);
        Assert.AreEqual(0u, emptyPriceVolumeLayer.SourceQuoteReference);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsValueDateUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsSourceNameUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsExecutableUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsSourceQuoteReferenceUpdated);
        Assert.AreEqual(0, emptyPriceVolumeLayer.Count);
    }

    [TestMethod]
    public void EmptyPvlMissingLookup_Construction_SetsTraderNameIdLookupWhenNullOrSameAsInfo()
    {
        var moqSrcTkrQuoteInfo = new Mock<IPQSourceTickerInfo>();

        var newEmpty = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(new PQNameIdLookupGenerator(0));

        Assert.IsNotNull(newEmpty.NameIdLookup);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        Assert.AreNotSame(emptyNameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void EmptyPvl_Construction_SharesTraderNameIdLookupBetweenLayers()
    {
        var newEmpty = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(emptyNameIdLookup, 0m);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        Assert.AreSame(emptyNameIdLookup, newEmpty.NameIdLookup);
        Assert.AreNotSame(populatedPvl.NameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IPQSourceQuoteRefTraderValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);
        var clonedQuote2 = ((IPQValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote2, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote2);
        var clonedQuote3 = ((IPQSourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote3, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote3);
        var clonedQuote4 = ((ICloneable<ISourcePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote4, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote4);
        var clonedQuote5 = ((IMutableSourcePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote5, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote5);
        var clonedQuote6 = ((IPQSourcePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote6, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote6);
        var clonedQuote7 = ((ISourcePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote7, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote7);
        var clonedQuote8 = ((ICloneable<ISourceQuoteRefPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote8, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote8);
        var clonedQuote9 = ((ISourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote9, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote9);
        var clonedQuote10 = ((ICloneable<ISourceQuoteRefTraderValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote10, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote10);
        var clonedQuote11 = ((ISourceQuoteRefTraderValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote11, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote11);
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
        var clonedQuote19 = ((IValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote19, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote19);
        var cloned20 = (PQSourceQuoteRefTraderValueDatePriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned20, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned20);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQSourceQuoteRefTraderValueDatePriceVolumeLayer)
            ((ICloneable)populatedPvl).Clone();
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.ValueDate)}: {populatedPvl.ValueDate}"));
        Assert.IsTrue(toString.Contains("TraderDetails: ["));
        for (var i = 0; i < populatedPvl.Count; i++) Assert.IsTrue(toString.Contains(populatedPvl[i]!.ToString()!));
    }

    public static void AssertContainsAllPvlFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        PQSourceQuoteRefTraderValueDatePriceVolumeLayer pvl)
    {
        PQTraderPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl);

        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.FirstLayerDateOffset, pvl.ValueDate.GetHoursFromUnixEpoch(),
                                          PQFieldFlags.IsExtendedFieldId), PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                         PQFieldKeys.FirstLayerDateOffset, PQFieldFlags.IsExtendedFieldId), $"For {pvl.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset, pvl.SourceId),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQFieldKeys.LayerSourceIdOffset), $"For {pvl.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerBooleanFlagsOffset,
                                          pvl.Executable ? PQFieldFlags.LayerExecutableFlag : 0),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQFieldKeys.LayerBooleanFlagsOffset), $"For {pvl.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerSourceQuoteRefOffset, pvl.SourceQuoteReference),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQFieldKeys.LayerSourceQuoteRefOffset), $"For {pvl.GetType().Name} ");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQSourceQuoteRefTraderValueDatePriceVolumeLayer? original,
        PQSourceQuoteRefTraderValueDatePriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        if (original.GetType() == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer) &&
            changingPriceVolumeLayer.GetType() == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer))
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));

        PQTraderPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBook,
             changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent
                                (new SourceQuoteRefTraderValueDatePriceVolumeLayer(original), exactComparison));

        SourceQuoteRefTraderValueDatePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
         exactComparison, original, changingPriceVolumeLayer, originalOrderBook,
         changingOrderBook, originalQuote, changingQuote);

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

        changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated =
            !changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated;
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
