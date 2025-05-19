// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

[TestClass]
public class PQFullSupportPriceVolumeLayerTests
{
    private const int     PopulatedNumberOfOrders = 2;
    private const uint    OrdersCount             = 2;
    private const decimal PopulatedQuotePrice     = 4.2949_672m;
    private const uint    PopulatedQuoteRef       = 4_2949_672u;
    private const decimal PopulatedQuoteVolume    = 42_949_672m;
    private const string  PopulatedSourceName     = "TestSourceName";
    private const decimal PopulatedInternalVolume = 1_949_672m;

    private static readonly DateTime PopulatedValueDate = new(2017, 12, 26, 21, 00, 00); // only to the nearest hour.
    private static readonly DateTime TestDateTime       = new(2017, 12, 17, 18, 54, 52);


    private IPQNameIdLookupGenerator emptyNameIdLookup = null!;

    private PQFullSupportPriceVolumeLayer emptyPvl             = null!;
    private IPQNameIdLookupGenerator      nonEmptyNameIdLookup = null!;
    private PQFullSupportPriceVolumeLayer populatedPvl         = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup    = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        nonEmptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

        nonEmptyNameIdLookup.GetOrAddId("TestSourceName");

        emptyPvl = new PQFullSupportPriceVolumeLayer(emptyNameIdLookup.Clone(), 0m, 0m);
        populatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup.Clone(), PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate,
             PopulatedSourceName, true, PopulatedQuoteRef, OrdersCount, PopulatedInternalVolume);

        PQOrdersPriceVolumeLayerTests.AddCounterPartyOrders(populatedPvl, PopulatedNumberOfOrders);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        Assert.AreEqual(PopulatedQuotePrice, populatedPvl.Price);
        Assert.AreEqual(PopulatedQuoteVolume, populatedPvl.Volume);
        Assert.AreEqual(PopulatedValueDate, populatedPvl.ValueDate);
        Assert.AreEqual(PopulatedSourceName, populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.AreEqual(PopulatedQuoteRef, populatedPvl.SourceQuoteReference);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        Assert.IsTrue(populatedPvl.IsSourceQuoteReferenceUpdated);
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(populatedPvl, new[] { true, true });

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, emptyPvl.ValueDate);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.IsFalse(emptyPvl.Executable);
        Assert.AreEqual(0u, emptyPvl.SourceQuoteReference);
        Assert.IsFalse(emptyPvl.IsPriceUpdated);
        Assert.IsFalse(emptyPvl.IsVolumeUpdated);
        Assert.IsFalse(emptyPvl.IsValueDateUpdated);
        Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(emptyPvl.IsExecutableUpdated);
        Assert.IsFalse(emptyPvl.IsSourceQuoteReferenceUpdated);
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(emptyPvl, new[] { false, false });

        var newEmptyPvl = new PQFullSupportPriceVolumeLayer(emptyNameIdLookup);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, newEmptyPvl.ValueDate);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsValueDateUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(newEmptyPvl.IsExecutableUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceQuoteReferenceUpdated);
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(newEmptyPvl, new[] { false, false });
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup, PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate,
             PopulatedSourceName, true, PopulatedQuoteRef);
        PQOrdersPriceVolumeLayerTests.AddCounterPartyOrders(populatedPvl, PopulatedNumberOfOrders);
        var fromPQInstance = new PQFullSupportPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(PopulatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(PopulatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(PopulatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(PopulatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(PopulatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(fromPQInstance);

        var nonPqTraderPvl = new FullSupportPriceVolumeLayer
            (PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate, PopulatedSourceName, true, PopulatedQuoteRef);
        PQOrdersPriceVolumeLayerTests.AddCounterPartyOrders(nonPqTraderPvl, PopulatedNumberOfOrders);
        var fromNonPqInstance = new PQFullSupportPriceVolumeLayer(nonPqTraderPvl, emptyNameIdLookup.Clone());
        Assert.AreEqual(PopulatedQuotePrice, fromNonPqInstance.Price);
        Assert.AreEqual(PopulatedQuoteVolume, fromNonPqInstance.Volume);
        Assert.AreEqual(PopulatedValueDate, fromNonPqInstance.ValueDate);
        Assert.AreEqual(PopulatedSourceName, fromNonPqInstance.SourceName);
        Assert.IsTrue(fromNonPqInstance.Executable);
        Assert.AreEqual(PopulatedQuoteRef, fromNonPqInstance.SourceQuoteReference);
        Assert.IsTrue(fromNonPqInstance.IsPriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsValueDateUpdated);
        Assert.IsTrue(fromNonPqInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromNonPqInstance.IsExecutableUpdated);
        Assert.IsTrue(fromNonPqInstance.IsSourceQuoteReferenceUpdated);
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(fromNonPqInstance, new[] { true, true });

        var newEmptyPvl = new PQFullSupportPriceVolumeLayer(emptyNameIdLookup.Clone());
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, newEmptyPvl.ValueDate);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
        Assert.IsFalse(newEmptyPvl.IsPriceUpdated);
        Assert.IsFalse(newEmptyPvl.IsVolumeUpdated);
        Assert.IsFalse(newEmptyPvl.IsValueDateUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(newEmptyPvl.IsExecutableUpdated);
        Assert.IsFalse(newEmptyPvl.IsSourceQuoteReferenceUpdated);
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(newEmptyPvl, new[] { false, false });
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup, PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate,
             PopulatedSourceName, true, PopulatedQuoteRef)
            {
                IsPriceUpdated = false
            };
        PQOrdersPriceVolumeLayerTests.AddCounterPartyOrders(newPopulatedPvl, PopulatedNumberOfOrders);
        var fromPQInstance = new PQFullSupportPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(PopulatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(PopulatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(PopulatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(PopulatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(PopulatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsFalse(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(fromPQInstance, new[] { true, true });

        newPopulatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup, PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate
           , PopulatedSourceName, true, PopulatedQuoteRef)
            {
                IsVolumeUpdated = false
            };
        PQOrdersPriceVolumeLayerTests.AddCounterPartyOrders(newPopulatedPvl, PopulatedNumberOfOrders);
        Assert.AreEqual(PopulatedQuotePrice, newPopulatedPvl.Price);
        Assert.AreEqual(PopulatedQuoteVolume, newPopulatedPvl.Volume);
        Assert.AreEqual(PopulatedValueDate, newPopulatedPvl.ValueDate);
        Assert.AreEqual(PopulatedSourceName, newPopulatedPvl.SourceName);
        Assert.IsTrue(newPopulatedPvl.Executable);
        Assert.AreEqual(PopulatedQuoteRef, newPopulatedPvl.SourceQuoteReference);
        Assert.IsTrue(newPopulatedPvl.IsPriceUpdated);
        Assert.IsFalse(newPopulatedPvl.IsVolumeUpdated);
        Assert.IsTrue(newPopulatedPvl.IsValueDateUpdated);
        Assert.IsTrue(newPopulatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(newPopulatedPvl.IsExecutableUpdated);
        Assert.IsTrue(newPopulatedPvl.IsSourceQuoteReferenceUpdated);
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(newPopulatedPvl, new[] { true, true });

        newPopulatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup, PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate,
             PopulatedSourceName, true, PopulatedQuoteRef)
            {
                IsSourceNameUpdated = false
            };
        fromPQInstance = new PQFullSupportPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(PopulatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(PopulatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(PopulatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(PopulatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(PopulatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsFalse(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(fromPQInstance);

        newPopulatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup, PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate,
             PopulatedSourceName, true, PopulatedQuoteRef)
            {
                IsExecutableUpdated = false
            };
        fromPQInstance = new PQFullSupportPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(PopulatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(PopulatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(PopulatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(PopulatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(PopulatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsFalse(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup, PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate,
             PopulatedSourceName, true, PopulatedQuoteRef)
            {
                IsSourceQuoteReferenceUpdated = false
            };
        fromPQInstance = new PQFullSupportPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(PopulatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(PopulatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(PopulatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(PopulatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(PopulatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsFalse(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup, PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate,
             PopulatedSourceName, true, PopulatedQuoteRef)
            {
                IsValueDateUpdated = false
            };
        fromPQInstance = new PQFullSupportPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(PopulatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(PopulatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(PopulatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(PopulatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(PopulatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup, PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate,
             PopulatedSourceName, true, PopulatedQuoteRef);
        PQOrdersPriceVolumeLayerTests.AddCounterPartyOrders(newPopulatedPvl, PopulatedNumberOfOrders);
        for (var i = 0; i < PopulatedNumberOfOrders; i++)
            ((IPQCounterPartyOrderLayerInfo)newPopulatedPvl[i]!).IsExternalCounterPartyNameUpdated = false;
        fromPQInstance = new PQFullSupportPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(PopulatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(PopulatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(PopulatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(PopulatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(PopulatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
        var expectedUpdated =
            Enumerable.Repeat(PQOrdersPriceVolumeLayerTests.ExpectedUpdated.AllUpdatedExcept(isOrderCounterPartyNameIdUpdated: false)
                            , PopulatedNumberOfOrders)
                      .ToArray();
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected(fromPQInstance, new[] { true, true }, null,
                                                                expectedUpdated);

        newPopulatedPvl = new PQFullSupportPriceVolumeLayer
            (nonEmptyNameIdLookup, PopulatedQuotePrice, PopulatedQuoteVolume, PopulatedValueDate,
             PopulatedSourceName, true, PopulatedQuoteRef, OrdersCount, PopulatedInternalVolume);
        PQOrdersPriceVolumeLayerTests.AddCounterPartyOrders(newPopulatedPvl, PopulatedNumberOfOrders);

        for (var i = 0; i < PopulatedNumberOfOrders; i++) ((IPQCounterPartyOrderLayerInfo)newPopulatedPvl[i]!).IsExternalTraderNameUpdated = false;
        fromPQInstance = new PQFullSupportPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(PopulatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(PopulatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(PopulatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(PopulatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(PopulatedQuoteRef, fromPQInstance.SourceQuoteReference);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsValueDateUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsTrue(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);
        expectedUpdated =
            Enumerable.Repeat(PQOrdersPriceVolumeLayerTests.ExpectedUpdated.AllUpdatedExcept(isOrderTraderNameIdUpdated: false)
                            , PopulatedNumberOfOrders)
                      .ToArray();
        PQOrdersPriceVolumeLayerTests.AssertOrdersAreAsExpected
            (fromPQInstance, new[] { true, true }, null, expectedUpdated);
    }

    [TestMethod]
    public void PopulatedSrcQuoteRefPvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new SourceQuoteRefPriceVolumeLayer(populatedPvl);
        var emptyClone          = new PQFullSupportPriceVolumeLayer(nonExactPriceVolume, emptyNameIdLookup.Clone());
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcQtRefTrdVlDtPvl = new PQSourceQuoteRefPriceVolumeLayer(populatedPvl, populatedPvl.NameIdLookup);
        emptyClone = new PQFullSupportPriceVolumeLayer(pqSrcQtRefTrdVlDtPvl, emptyNameIdLookup);
        pqSrcQtRefTrdVlDtPvl.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedSrcNamePvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new SourcePriceVolumeLayer(populatedPvl);
        var emptyClone          = new PQFullSupportPriceVolumeLayer(nonExactPriceVolume, emptyNameIdLookup.Clone());
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcPvl = new PQSourcePriceVolumeLayer(populatedPvl, populatedPvl.NameIdLookup);
        emptyClone = new PQFullSupportPriceVolumeLayer(pqSrcPvl, emptyNameIdLookup);
        pqSrcPvl.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedValueDatePvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new ValueDatePriceVolumeLayer(populatedPvl);
        var emptyClone          = new PQFullSupportPriceVolumeLayer(nonExactPriceVolume, emptyNameIdLookup.Clone());
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqValueDatePvl = new ValueDatePriceVolumeLayer(populatedPvl);
        emptyClone = new PQFullSupportPriceVolumeLayer(pqValueDatePvl, emptyNameIdLookup);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedTraderPvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new OrdersPriceVolumeLayer(populatedPvl, populatedPvl.LayerType);
        var emptyClone          = new PQFullSupportPriceVolumeLayer(nonExactPriceVolume, emptyNameIdLookup.Clone());
        nonExactPriceVolume.AreEquivalent(emptyClone);


        var pqTrdPvl = new OrdersPriceVolumeLayer(populatedPvl, populatedPvl.LayerType);
        emptyClone = new PQFullSupportPriceVolumeLayer(pqTrdPvl, emptyNameIdLookup);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void EmptyPvl_ValueDateChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsValueDateUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.AreEqual(DateTime.MinValue, emptyPvl.ValueDate);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).Count());

        var expectedDateTime = new DateTime(2017, 12, 17, 19, 00, 00);
        emptyPvl.ValueDate = expectedDateTime;
        Assert.IsTrue(emptyPvl.IsValueDateUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(expectedDateTime, emptyPvl.ValueDate);
        var sourceUpdates = emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, expectedDateTime.Get2MinIntervalsFromUnixEpoch());
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyPvl.IsValueDateUpdated = false;
        Assert.IsFalse(emptyPvl.IsValueDateUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).IsNullOrEmpty());

        var nextExpectedValueDate = new DateTime(2017, 12, 17, 20, 00, 00);
        emptyPvl.ValueDate = nextExpectedValueDate;
        Assert.IsTrue(emptyPvl.IsValueDateUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(nextExpectedValueDate, emptyPvl.ValueDate);
        sourceUpdates = emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, nextExpectedValueDate.Get2MinIntervalsFromUnixEpoch());
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Snapshot)
            where update.Id == PQFeedFields.QuoteLayerValueDate
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQFullSupportPriceVolumeLayer(emptyNameIdLookup);
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
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).Count());

        emptyPvl.SourceQuoteReference = PopulatedQuoteRef;
        Assert.IsTrue(emptyPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(PopulatedQuoteRef, emptyPvl.SourceQuoteReference);
        var sourceLayerUpdates = emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerSourceQuoteRef, PopulatedQuoteRef);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        emptyPvl.IsSourceQuoteReferenceUpdated = false;
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyPvl.IsSourceQuoteReferenceUpdated = true;
        sourceLayerUpdates =
            (from update in emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update)
                where update.Id == PQFeedFields.QuoteLayerSourceQuoteRef
                select update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        var newEmpty = new PQFullSupportPriceVolumeLayer(emptyNameIdLookup);
        newEmpty.UpdateField(sourceLayerUpdates[0]);
        Assert.AreEqual(PopulatedQuoteRef, newEmpty.SourceQuoteReference);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsSourceQuoteReferenceUpdated);
    }

    [TestMethod]
    public void EmptyPvl_SourceNameChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).Count());

        emptyPvl.SourceName = PopulatedSourceName;
        Assert.IsTrue(emptyPvl.IsSourceNameUpdated);
        Assert.AreEqual(emptyNameIdLookup[PopulatedSourceName], emptyPvl.SourceId);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(PopulatedSourceName, emptyPvl.SourceName);
        var sourceUpdates = emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.QuoteLayerSourceId, emptyPvl.SourceId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyPvl.IsSourceNameUpdated = false;
        Assert.IsFalse(emptyPvl.IsSourceNameUpdated);
        Assert.IsFalse(emptyPvl.HasUpdates);
        emptyPvl.NameIdLookup.HasUpdates = false;
        Assert.IsFalse(emptyPvl.NameIdLookup.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).IsNullOrEmpty());

        var nextExpectedSourceName = "AnotherSourceName";
        emptyPvl.SourceName = nextExpectedSourceName;
        Assert.IsTrue(emptyPvl.IsSourceNameUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.AreEqual(nextExpectedSourceName, emptyPvl.SourceName);
        sourceUpdates = emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var stringUpdates = emptyPvl.GetStringUpdates(TestDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, stringUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.QuoteLayerSourceId, emptyPvl.SourceId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
        var expectedStringUpdates = new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(PQFeedFields.QuoteLayerStringUpdates, CrudCommand.Upsert.ToPQSubFieldId(), 0u)
          , StringUpdate = new PQStringUpdate
            {
                Command = CrudCommand.Upsert, DictionaryId = emptyPvl.NameIdLookup[emptyPvl.SourceName]
              , Value   = emptyPvl.SourceName
            }
        };
        Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

        sourceUpdates = (from update in emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Snapshot)
            where update.Id == PQFeedFields.QuoteLayerSourceId
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        var newEmpty             = new PQFullSupportPriceVolumeLayer(newEmptyNameIdLookup, 0m, 0m);
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
        Assert.AreEqual(0, emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).Count());

        emptyPvl.Executable = true;
        Assert.IsTrue(emptyPvl.IsExecutableUpdated);
        Assert.IsTrue(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.Executable);
        var sourceLayerUpdates = emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteLayerBooleanFlags, (uint)LayerBooleanFlags.IsExecutableFlag);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        emptyPvl.IsExecutableUpdated = false;
        Assert.IsFalse(emptyPvl.HasUpdates);
        Assert.IsTrue(emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyPvl.IsExecutableUpdated = true;
        sourceLayerUpdates =
            (from update in emptyPvl.GetDeltaUpdateFields(TestDateTime, StorageFlags.Update)
                where update.Id == PQFeedFields.QuoteLayerBooleanFlags
                select update).ToList();
        Assert.AreEqual(1, sourceLayerUpdates.Count);
        Assert.AreEqual(expectedLayerField, sourceLayerUpdates[0]);

        var newEmpty = new PQFullSupportPriceVolumeLayer(emptyNameIdLookup);
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
        for (var i = 0; i < PopulatedNumberOfOrders; i++) Assert.IsTrue(populatedPvl[i]!.HasUpdates);
        Assert.IsTrue(emptyPvl.IsEmpty);

        populatedPvl.HasUpdates = false;

        Assert.IsFalse(populatedPvl.HasUpdates);
        Assert.IsFalse(populatedPvl.NameIdLookup.HasUpdates);
        for (var i = 0; i < PopulatedNumberOfOrders; i++) Assert.IsFalse(populatedPvl[i]!.HasUpdates);
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
        Assert.AreNotEqual(DateTime.MinValue, populatedPvl.ValueDate);
        Assert.AreNotEqual(null, populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.AreEqual(PopulatedQuoteRef, populatedPvl.SourceQuoteReference);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        Assert.IsTrue(populatedPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
        for (var i = 0; i < PopulatedNumberOfOrders; i++)
        {
            var anonOrderLayer = populatedPvl[i]!;
            Assert.AreNotEqual(0m, ((IPublishedOrder)anonOrderLayer).OrderDisplayVolume);
            Assert.IsTrue(anonOrderLayer.IsOrderVolumeUpdated);
            Assert.IsFalse(anonOrderLayer.IsEmpty);
            if (anonOrderLayer is IPQCounterPartyOrderLayerInfo counterPartyOrderLayer)
            {
                Assert.AreNotEqual(null, ((IExternalCounterPartyInfoOrder)counterPartyOrderLayer).ExternalTraderName);
                Assert.IsTrue(counterPartyOrderLayer.IsExternalTraderNameUpdated);
            }
        }

        populatedPvl.IsEmpty = true;
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, populatedPvl.ValueDate);
        Assert.AreEqual(null, populatedPvl.SourceName);
        Assert.IsFalse(populatedPvl.Executable);
        Assert.AreEqual(0u, populatedPvl.SourceQuoteReference);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        Assert.IsTrue(populatedPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(populatedPvl.IsValueDateUpdated);
        for (var i = 0; i < PopulatedNumberOfOrders; i++)
        {
            var anonOrderLayer = populatedPvl[i]!;
            Assert.AreEqual(0m, ((IPublishedOrder)anonOrderLayer).OrderDisplayVolume);
            Assert.IsTrue(anonOrderLayer.IsOrderVolumeUpdated);
            Assert.IsTrue(anonOrderLayer.IsEmpty);
            if (anonOrderLayer is IPQCounterPartyOrderLayerInfo counterPartyOrderLayer)
            {
                Assert.AreEqual(null, ((IExternalCounterPartyInfoOrder)counterPartyOrderLayer).ExternalTraderName);
                Assert.IsTrue(counterPartyOrderLayer.IsExternalTraderNameUpdated);
            }
        }
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
        var newEmpty = new PQFullSupportPriceVolumeLayer(emptyNameIdLookup.Clone(), 0m, 0m);
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
        Assert.AreEqual(populatedPvl, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromNonPQSrcQtRefTrdrVlDtToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQPriceVolume = new FullSupportPriceVolumeLayer(populatedPvl);
        emptyPvl.NameIdLookup.CopyFrom(populatedPvl.NameIdLookup);
        emptyPvl.CopyFrom(nonPQPriceVolume);
        Assert.AreEqual(populatedPvl, emptyPvl);

        var emptyPqNameIdLookupGen = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        var fromConstructor        = new PQFullSupportPriceVolumeLayer(nonPQPriceVolume, emptyPqNameIdLookupGen);
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
    public void FullyPopulatedPvl_CopyFromAnonOrdersPvlToEmptyQuote_PvlsEqualEachOther()
    {
        var nonExactPriceVolume = new OrdersPriceVolumeLayer(populatedPvl, populatedPvl.LayerType);
        var emptyClone          = emptyPvl.Clone();
        emptyClone.CopyFrom(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqTrdPvl = new PQOrdersPriceVolumeLayer(populatedPvl, LayerType.OrdersAnonymousPriceVolume, populatedPvl.NameIdLookup.Clone());
        emptyClone = new PQFullSupportPriceVolumeLayer(populatedPvl.NameIdLookup.Clone());
        emptyClone = emptyClone.Clone();
        emptyClone.CopyFrom(pqTrdPvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromCounterPartyOrdersPvlToEmptyQuote_PvlsEqualEachOther()
    {
        var nonExactPriceVolume = new OrdersPriceVolumeLayer(populatedPvl, populatedPvl.LayerType);
        var emptyClone          = emptyPvl.Clone();
        emptyClone.CopyFrom(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqTrdPvl = new PQOrdersPriceVolumeLayer(populatedPvl, LayerType.OrdersFullPriceVolume, populatedPvl.NameIdLookup);
        emptyClone = new PQFullSupportPriceVolumeLayer(populatedPvl.NameIdLookup.Clone());
        emptyClone = emptyClone.Clone();
        emptyClone.CopyFrom(pqTrdPvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQFullSupportPriceVolumeLayer(emptyNameIdLookup);
        populatedPvl.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedPvl);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Price);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(DateTime.MinValue, emptyPriceVolumeLayer.ValueDate);
        Assert.AreEqual(null, emptyPriceVolumeLayer.SourceName);
        Assert.IsFalse(emptyPriceVolumeLayer.Executable);
        Assert.AreEqual(0u, emptyPriceVolumeLayer.SourceQuoteReference);
        Assert.IsFalse(emptyPriceVolumeLayer.IsPriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsValueDateUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsSourceNameUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsExecutableUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsSourceQuoteReferenceUpdated);
        Assert.AreEqual(0u, emptyPriceVolumeLayer.OrdersCount);
    }

    [TestMethod]
    public void EmptyPvlMissingLookup_Construction_SetsTraderNameIdLookupWhenNullOrSameAsInfo()
    {
        var moqSrcTkrQuoteInfo = new Mock<IPQSourceTickerInfo>();

        var newEmpty = new PQFullSupportPriceVolumeLayer(new PQNameIdLookupGenerator(0));

        Assert.IsNotNull(newEmpty.NameIdLookup);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        Assert.AreNotSame(emptyNameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void EmptyPvl_Construction_SharesTraderNameIdLookupBetweenLayers()
    {
        var newEmpty = new PQFullSupportPriceVolumeLayer(emptyNameIdLookup, 0m);
        Assert.AreEqual(0, newEmpty.NameIdLookup.Count);
        Assert.AreSame(emptyNameIdLookup, newEmpty.NameIdLookup);
        Assert.AreNotSame(populatedPvl.NameIdLookup, newEmpty.NameIdLookup);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IPQFullSupportPriceVolumeLayer)populatedPvl).Clone();
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
        var clonedQuote10 = ((ICloneable<IFullSupportPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote10, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote10);
        var clonedQuote11 = ((IFullSupportPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote11, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote11);
        var clonedQuote12 = ((ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote12, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote12);
        var clonedQuote13 = ((IMutableSourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote13, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote13);
        var clonedQuote14 = ((ICloneable<IMutableFullSupportPriceVolumeLayer>)
            populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote14, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote14);
        var clonedQuote15 = ((IMutableFullSupportPriceVolumeLayer)populatedPvl).Clone();
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
        var cloned20 = (PQFullSupportPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned20, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned20);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQFullSupportPriceVolumeLayer)
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
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.OrdersCount)}: {populatedPvl.OrdersCount}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.InternalVolume)}: {populatedPvl.InternalVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.SourceName)}: {populatedPvl.SourceName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Executable)}: {populatedPvl.Executable}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.SourceQuoteReference)}: " +
                                        $"{populatedPvl.SourceQuoteReference:N0}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.ValueDate)}: {populatedPvl.ValueDate}"));
        Assert.IsTrue(toString.Contains("Orders: ["));
        for (var i = 0; i < populatedPvl.OrdersCount; i++) Assert.IsTrue(toString.Contains(populatedPvl[i]!.ToString()!));
    }

    public static void AssertContainsAllPvlFields
    (IList<PQFieldUpdate> checkFieldUpdates, PQFullSupportPriceVolumeLayer pvl,
        int bookIndex = 0, PQFieldFlags priceScale = (PQFieldFlags)1, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        PQOrdersPriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl, bookIndex, priceScale, volumeScale);

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, pvl.ValueDate.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.QuoteLayerValueDate), $"For {pvl.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerSourceId, pvl.SourceId),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.QuoteLayerSourceId), $"For {pvl.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerBooleanFlags,
                                          (uint)(pvl.Executable ? LayerBooleanFlags.IsExecutableFlag : 0)),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.QuoteLayerBooleanFlags), $"For {pvl.GetType().Name} ");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerSourceQuoteRef, pvl.SourceQuoteReference),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.QuoteLayerSourceQuoteRef), $"For {pvl.GetType().Name} ");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQFullSupportPriceVolumeLayer? original,
        PQFullSupportPriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBookSide? originalOrderBookSide = null,
        IOrderBookSide? changingOrderBookSide = null,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        IPublishableLevel2Quote? originalQuote = null,
        IPublishableLevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        if (original.GetType() == typeof(PQFullSupportPriceVolumeLayer) &&
            changingPriceVolumeLayer.GetType() == typeof(PQFullSupportPriceVolumeLayer))
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));

        PQOrdersPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide,
             changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQFullSupportPriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent
                                (new FullSupportPriceVolumeLayer(original), exactComparison));

        FullSupportPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(
                                                                                             exactComparison, original, changingPriceVolumeLayer
                                                                                           , originalOrderBookSide,
                                                                                             changingOrderBookSide, originalOrderBook
                                                                                           , changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.IsSourceNameUpdated = !changingPriceVolumeLayer.IsSourceNameUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsSourceNameUpdated = original.IsSourceNameUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.IsExecutableUpdated = !changingPriceVolumeLayer.IsExecutableUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsExecutableUpdated = original.IsExecutableUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated =
            !changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated = original.IsSourceQuoteReferenceUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.IsValueDateUpdated = !changingPriceVolumeLayer.IsValueDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.IsValueDateUpdated = original.IsValueDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
