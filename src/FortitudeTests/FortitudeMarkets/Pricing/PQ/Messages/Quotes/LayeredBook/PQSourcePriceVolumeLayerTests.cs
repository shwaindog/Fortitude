// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQSourcePriceVolumeLayerTests
{
    private static IPQNameIdLookupGenerator emptyNameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);

    private PQSourcePriceVolumeLayer emptyPvl     = null!;
    private IPQNameIdLookupGenerator nameIdLookup = null!;
    private PQSourcePriceVolumeLayer populatedPvl = null!;

    private string wellKnownSourceName = null!;


    private static DateTime testDateTime = new(2017, 10, 08, 18, 33, 24);

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
    public void PopulatedPvl_SourceNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        emptyPvl.HasUpdates = false;

        AssertSourceNameFieldUpdatesReturnAsExpected(emptyPvl);
    }


    public static void AssertSourceNameFieldUpdatesReturnAsExpected
    (
        IPQSourcePriceVolumeLayer? sourcePvl,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQLevel2Quote? l2Quote = null
    )
    {
        if (sourcePvl == null) return;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(sourcePvl.IsSourceNameUpdated);
        Assert.IsFalse(sourcePvl.HasUpdates);
        sourcePvl.SourceName = "yellow";
        Assert.IsTrue(sourcePvl.HasUpdates);
        sourcePvl.UpdateComplete();
        sourcePvl.SourceName          = null;
        sourcePvl.IsSourceNameUpdated = false;
        sourcePvl.HasUpdates          = false;

        Assert.AreEqual(0, sourcePvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSourceName = "NewChangedSourceName" + bookDepth;
        sourcePvl.SourceName = expectedSourceName;
        Assert.IsTrue(sourcePvl.HasUpdates);
        Assert.AreEqual(expectedSourceName, sourcePvl.SourceName);
        Assert.IsTrue(sourcePvl.IsSourceNameUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCounterPartyL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        var orderInfoUpdates = sourcePvl
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        var dictId = sourcePvl.NameIdLookup[expectedSourceName];
        var expectedLayer
            = new PQFieldUpdate(PQQuoteFields.SourceId, dictId);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedLayer, orderInfoUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        sourcePvl.IsSourceNameUpdated = false;
        Assert.IsFalse(sourcePvl.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(sourcePvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.SourceId }
                       && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in l2Quote.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQSourcePriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            Assert.AreEqual(expectedSourceName, foundLayer.SourceName);
            Assert.IsTrue(foundLayer.IsSourceNameUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.SourceId }
                       && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in orderBook.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQSourcePriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            Assert.AreEqual(expectedSourceName, foundLayer.SourceName);
            Assert.IsTrue(foundLayer.IsSourceNameUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.SourceId }
                       && update.DepthId == depthNoSide
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in orderBookSide.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQSourcePriceVolumeLayer)newEmpty[bookDepth]!;
            Assert.AreEqual(expectedSourceName, foundLayer.SourceName);
            Assert.IsTrue(foundLayer.IsSourceNameUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        orderInfoUpdates =
            (from update in sourcePvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQQuoteFields.SourceId }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedLayer, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQSourcePriceVolumeLayer(emptyNameIdLookup.Clone());
        foreach (var stringUpdate in sourcePvl.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
        {
            newAnonOrderInfo.UpdateFieldString(stringUpdate);
        }
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedSourceName, newAnonOrderInfo.SourceName);
        Assert.IsTrue(newAnonOrderInfo.IsSourceNameUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        sourcePvl.SourceName = null;
        sourcePvl.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void PopulatedPvl_ExecutableChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        emptyPvl.HasUpdates = false;

        AssertExecutableFieldUpdatesReturnAsExpected(emptyPvl);
    }

    public static void AssertExecutableFieldUpdatesReturnAsExpected
    (
        IPQSourcePriceVolumeLayer? sourcePvl,
        int bookDepth = 0,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQLevel2Quote? l2Quote = null
    )
    {
        if (sourcePvl == null) return;
        var bsNotNull     = orderBookSide != null;
        var bkNotNull     = orderBook != null;
        var l2QNotNull    = l2Quote != null;
        var isBid         = orderBook == null || orderBookSide?.BookSide == BookSide.BidBook;
        var depthNoSide   = (PQDepthKey)bookDepth;
        var depthWithSide = (PQDepthKey)bookDepth | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(sourcePvl.IsExecutableUpdated);
        Assert.IsFalse(sourcePvl.HasUpdates);
        sourcePvl.Executable = !sourcePvl.Executable;
        Assert.IsTrue(sourcePvl.HasUpdates);
        sourcePvl.UpdateComplete();
        sourcePvl.Executable          = !sourcePvl.Executable;
        sourcePvl.IsExecutableUpdated = false;
        sourcePvl.HasUpdates          = false;

        Assert.AreEqual(0, sourcePvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedExecutable   = !sourcePvl.Executable;
        var expectedBooleanFlags = expectedExecutable ? LayerBooleanFlags.IsExecutableFlag : LayerBooleanFlags.None;
        sourcePvl.Executable = expectedExecutable;
        Assert.IsTrue(sourcePvl.HasUpdates);
        Assert.AreEqual(expectedExecutable, sourcePvl.Executable);
        Assert.IsTrue(sourcePvl.IsExecutableUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCounterPartyL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        var orderInfoUpdates = sourcePvl
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        var expectedLayer
            = new PQFieldUpdate(PQQuoteFields.LayerBooleanFlags, (uint)expectedBooleanFlags);
        var expectedBookSide  = expectedLayer.WithDepth(depthNoSide);
        var expectedOrderBook = expectedBookSide.WithDepth(depthWithSide);
        Assert.AreEqual(expectedLayer, orderInfoUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedOrderBook, l2QUpdates[2]);

        sourcePvl.IsExecutableUpdated = false;
        Assert.IsFalse(sourcePvl.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(sourcePvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerBooleanFlags }
                       && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBook, l2QUpdates[0]);

            var newEmpty = new PQLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in l2Quote.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(l2QUpdates[0]);
            var foundLayer =
                (IPQSourcePriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[bookDepth]!;
            Assert.AreEqual(expectedExecutable, foundLayer.Executable);
            Assert.IsTrue(foundLayer.IsExecutableUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerBooleanFlags }
                       && update.DepthId == depthWithSide
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in orderBook.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(bkUpdates[0]);
            var foundLayer =
                (IPQSourcePriceVolumeLayer)(isBid ? newEmpty.BidSide : newEmpty.AskSide)[bookDepth]!;
            Assert.AreEqual(expectedExecutable, foundLayer.Executable);
            Assert.IsTrue(foundLayer.IsExecutableUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                    where update is { Id: PQQuoteFields.LayerBooleanFlags }
                       && update.DepthId == depthNoSide
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            foreach (var stringUpdate in orderBookSide.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
            {
                newEmpty.UpdateFieldString(stringUpdate);
            }
            newEmpty.UpdateField(bsUpdates[0]);
            var foundLayer         = (IPQSourcePriceVolumeLayer)newEmpty[bookDepth]!;
            Assert.AreEqual(expectedExecutable, foundLayer.Executable);
            Assert.IsTrue(foundLayer.IsExecutableUpdated);
            Assert.IsTrue(foundLayer.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        orderInfoUpdates =
            (from update in sourcePvl.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot, precisionSettings)
                where update is { Id: PQQuoteFields.LayerBooleanFlags }
                select update).ToList();
        Assert.AreEqual(1, orderInfoUpdates.Count);
        Assert.AreEqual(expectedLayer, orderInfoUpdates[0]);

        var newAnonOrderInfo = new PQSourcePriceVolumeLayer(emptyNameIdLookup.Clone());
        foreach (var stringUpdate in sourcePvl.GetStringUpdates(testDateTime, StorageFlags.Snapshot))
        {
            newAnonOrderInfo.UpdateFieldString(stringUpdate);
        }
        newAnonOrderInfo.UpdateField(orderInfoUpdates[0]);
        Assert.AreEqual(expectedExecutable, newAnonOrderInfo.Executable);
        Assert.IsTrue(newAnonOrderInfo.IsExecutableUpdated);
        Assert.IsTrue(newAnonOrderInfo.HasUpdates);

        sourcePvl.Executable = true;
        sourcePvl.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
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
        IOrderBookSide? originalOrderBookSide = null,
        IOrderBookSide? changingOrderBookSide = null,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        PQPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide
           , changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQSourcePriceVolumeLayer))
            Assert.AreEqual(!exactComparison,
                            changingPriceVolumeLayer.AreEquivalent(new SourcePriceVolumeLayer(original), exactComparison));

        SourcePriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide
           , changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

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
    }
}
