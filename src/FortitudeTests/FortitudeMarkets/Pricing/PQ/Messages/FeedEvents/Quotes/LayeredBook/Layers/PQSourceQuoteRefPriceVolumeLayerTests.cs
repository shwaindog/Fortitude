// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

[TestClass]
public class PQSourceQuoteRefPriceVolumeLayerTests
{
    private static IPQNameIdLookupGenerator emptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

    private PQSourceQuoteRefPriceVolumeLayer emptyPvl     = null!;
    private IPQNameIdLookupGenerator         nameIdLookup = null!;
    private PQSourceQuoteRefPriceVolumeLayer populatedPvl = null!;

    private static DateTime testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);

    private uint   wellKnownQuoteRef;
    private string wellKnownSourceName = null!;

    [TestInitialize]
    public void SetUp()
    {
        wellKnownSourceName = "TestSourceName";
        wellKnownQuoteRef   = 4_294_967_295u;
        emptyNameIdLookup   = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        nameIdLookup        = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        emptyPvl            = new PQSourceQuoteRefPriceVolumeLayer(emptyNameIdLookup.Clone());
        testDateTime        = new DateTime(2017, 12, 17, 18, 54, 52);
        populatedPvl = new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup, 4.2949_672m, 42_949_672m,
                                                            wellKnownSourceName, true, wellKnownQuoteRef);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newPvl = new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup, 20, 40_000_000,
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
        var newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup, 20, 40_000_000,
                                                                   wellKnownSourceName, true, wellKnownQuoteRef);
        var fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
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
        var fromNonPqInstance = new PQSourceQuoteRefPriceVolumeLayer(nonPQValueDatePvl, emptyNameIdLookup.Clone());
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

        var fromNonSourcePriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer(new PriceVolumeLayer(20, 40_000_000), emptyNameIdLookup.Clone());
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
                                                                             new SourcePriceVolumeLayer(20, 40_000_000, wellKnownSourceName, true)
                                                                           , emptyNameIdLookup.Clone());
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

        var newEmptyPvl = new PQSourceQuoteRefPriceVolumeLayer(emptyPvl, emptyNameIdLookup.Clone());
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
        var newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup, 20, 40_000_000,
                                                                   wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsPriceUpdated = false
        };
        var fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
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

        newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup, 20, 40_000_000,
                                                               wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsVolumeUpdated = false
        };
        fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
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

        newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup, 20, 40_000_000,
                                                               wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsSourceNameUpdated = false
        };
        fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
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

        newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup, 20, 40_000_000,
                                                               wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsExecutableUpdated = false
        };
        fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
        Assert.AreEqual(20m, fromPQInstance.Price);
        Assert.AreEqual(40_000_000m, fromPQInstance.Volume);
        Assert.AreEqual(wellKnownSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.IsTrue(fromPQInstance.IsPriceUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceNameUpdated);
        Assert.IsFalse(fromPQInstance.IsExecutableUpdated);
        Assert.IsTrue(fromPQInstance.IsSourceQuoteReferenceUpdated);

        newPopulatedPvl = new PQSourceQuoteRefPriceVolumeLayer(nameIdLookup, 20, 40_000_000,
                                                               wellKnownSourceName, true, wellKnownQuoteRef)
        {
            IsSourceQuoteReferenceUpdated = false
        };
        fromPQInstance = new PQSourceQuoteRefPriceVolumeLayer(newPopulatedPvl, newPopulatedPvl.NameIdLookup);
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
        emptyPvl.HasUpdates = false;

        AssertSourceQuoteRefFieldUpdatesReturnAsExpected(emptyPvl);
    }

    public static void AssertSourceQuoteRefFieldUpdatesReturnAsExpected
    (
        IPQSourceQuoteRefPriceVolumeLayer? srcQtRefPvl,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (srcQtRefPvl == null) return;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBookSide == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(srcQtRefPvl.IsSourceQuoteReferenceUpdated);
        Assert.IsFalse(srcQtRefPvl.HasUpdates);
        srcQtRefPvl.SourceQuoteReference = 2897;
        Assert.IsTrue(srcQtRefPvl.HasUpdates);
        srcQtRefPvl.UpdateComplete();
        srcQtRefPvl.SourceQuoteReference = 0;
        srcQtRefPvl.IsSourceQuoteReferenceUpdated = false;
        srcQtRefPvl.HasUpdates           = false;

        Assert.AreEqual(0, srcQtRefPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedSrcQtRef = 128u;
        srcQtRefPvl.SourceQuoteReference = expectedSrcQtRef;
        Assert.IsTrue(srcQtRefPvl.HasUpdates);
        Assert.AreEqual(expectedSrcQtRef, srcQtRefPvl.SourceQuoteReference);
        Assert.IsTrue(srcQtRefPvl.IsSourceQuoteReferenceUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        var layerUpdates = srcQtRefPvl
                           .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        var expectedLayer
            = new PQFieldUpdate(PQFeedFields.QuoteLayerSourceQuoteRef, expectedSrcQtRef);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedLayer, layerUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        srcQtRefPvl.IsSourceQuoteReferenceUpdated = false;
        Assert.IsFalse(srcQtRefPvl.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(srcQtRefPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerSourceQuoteRef && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQSourceQuoteRefPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            Assert.AreEqual(expectedSrcQtRef, foundLayer.SourceQuoteReference);
            Assert.IsTrue(foundLayer.IsSourceQuoteReferenceUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerSourceQuoteRef && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQSourceQuoteRefPriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            Assert.AreEqual(expectedSrcQtRef, foundLayer.SourceQuoteReference);
            Assert.IsTrue(foundLayer.IsSourceQuoteReferenceUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == PQFeedFields.QuoteLayerSourceQuoteRef && update.DepthId == depthNoSide
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer = (IPQSourceQuoteRefPriceVolumeLayer)newEmpty[bookDepth]!;
            Assert.AreEqual(expectedSrcQtRef, foundLayer.SourceQuoteReference);
            Assert.IsTrue(foundLayer.IsSourceQuoteReferenceUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        layerUpdates =
            (from update in srcQtRefPvl.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == PQFeedFields.QuoteLayerSourceQuoteRef
                select update).ToList();
        Assert.AreEqual(1, layerUpdates.Count);
        Assert.AreEqual(expectedLayer, layerUpdates[0]);

        var newLayer = new PQSourceQuoteRefPriceVolumeLayer(emptyNameIdLookup.Clone());
        newLayer.UpdateField(layerUpdates[0]);
        Assert.AreEqual(expectedSrcQtRef, newLayer.SourceQuoteReference);
        Assert.IsTrue(newLayer.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(newLayer.HasUpdates);

        srcQtRefPvl.SourceQuoteReference = 0u;
        srcQtRefPvl.HasUpdates  = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
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
        Assert.AreEqual(wellKnownQuoteRef, populatedPvl.SourceQuoteReference);
        Assert.IsTrue(populatedPvl.IsPriceUpdated);
        Assert.IsTrue(populatedPvl.IsVolumeUpdated);
        Assert.IsTrue(populatedPvl.IsSourceNameUpdated);
        Assert.IsTrue(populatedPvl.IsExecutableUpdated);
        Assert.IsTrue(populatedPvl.IsSourceQuoteReferenceUpdated);
        populatedPvl.IsEmpty = true;
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
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllPvlFields(pqFieldUpdates, populatedPvl);
    }

    [TestMethod]
    public void PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedPvl.HasUpdates = false;
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        var pqStringUpdates =
            populatedPvl.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        Assert.AreEqual(0, pqStringUpdates.Count);
    }

    [TestMethod]
    public void PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates =
            populatedPvl.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
        var pqStringUpdates =
            populatedPvl.GetStringUpdates
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQSourceQuoteRefPriceVolumeLayer(emptyNameIdLookup.Clone());
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
        var emptyPriceVolumeLayer = new PQSourceQuoteRefPriceVolumeLayer(emptyNameIdLookup);
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
    }

    public static void AssertContainsAllPvlFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        PQSourceQuoteRefPriceVolumeLayer pvl)
    {
        PQSourcePriceVolumeLayerTests.AssertContainsAllPvlFields(checkFieldUpdates, pvl);

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerSourceQuoteRef, pvl.SourceQuoteReference),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates,
                                                                    PQFeedFields.QuoteLayerSourceQuoteRef), $"For {pvl.GetType().Name} ");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQSourceQuoteRefPriceVolumeLayer? original, IPQSourceQuoteRefPriceVolumeLayer? changingPriceVolumeLayer,
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

        PQSourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide
           , changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQSourceQuoteRefPriceVolumeLayer) &&
            changingPriceVolumeLayer.GetType() == typeof(PQSourceQuoteRefPriceVolumeLayer))
            Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        else
            Assert.AreEqual(!exactComparison, original.AreEquivalent(
                                                                     changingPriceVolumeLayer, exactComparison));

        if (original.GetType() == typeof(PQSourceQuoteRefPriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new SourceQuoteRefPriceVolumeLayer(original), exactComparison));

        SourceQuoteRefPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide
           , changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated
            = !changingPriceVolumeLayer.IsSourceQuoteReferenceUpdated;
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
    }
}
