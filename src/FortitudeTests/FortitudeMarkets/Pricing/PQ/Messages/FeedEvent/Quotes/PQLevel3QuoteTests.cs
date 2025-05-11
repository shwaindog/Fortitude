// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes;

[TestClass]
public class PQLevel3QuoteTests
{
    private IList<PQPublishableLevel3Quote> allEmptyQuotes          = null!;
    private IList<PQPublishableLevel3Quote> allFullyPopulatedQuotes = null!;

    private PQPublishableLevel3Quote noRecentlyTradedEmptyQuote          = null!;
    private PQPublishableLevel3Quote noRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo noRecentlyTradedSrcTkrInfo = null!;

    private PQPublishableLevel3Quote paidGivenVolumeRecentlyTradedEmptyQuote          = null!;
    private PQPublishableLevel3Quote paidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo             paidGivenVolumeRecentlyTradedSrcTkrInfo = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder           = null!;

    private PQPublishableLevel3Quote simpleRecentlyTradedEmptyQuote          = null!;
    private PQPublishableLevel3Quote simpleRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo simpleRecentlyTradedSrcTkrInfo = null!;

    private DateTime      testDateTime;
    private PQPublishableLevel3Quote traderPaidGivenVolumeRecentlyTradedEmptyQuote          = null!;
    private PQPublishableLevel3Quote traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo traderPaidGivenVolumeRecentlyTradedSrcTkrInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        noRecentlyTradedSrcTkrInfo                    = PQSourceTickerInfoTests.FullSupportL3NoRecentlyTradeSti;
        simpleRecentlyTradedSrcTkrInfo                = PQSourceTickerInfoTests.OrdersAnonL3JustTradeTradeSti;
        paidGivenVolumeRecentlyTradedSrcTkrInfo       = PQSourceTickerInfoTests.FullSupportL3PaidOrGivenTradeSti;
        traderPaidGivenVolumeRecentlyTradedSrcTkrInfo = PQSourceTickerInfoTests.FullSupportL3TraderNamePaidOrGivenSti;
        noRecentlyTradedEmptyQuote                    = new PQPublishableLevel3Quote(noRecentlyTradedSrcTkrInfo.Clone()) { HasUpdates = false };
        noRecentlyTradedFullyPopulatedQuote           = new PQPublishableLevel3Quote(noRecentlyTradedSrcTkrInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(noRecentlyTradedFullyPopulatedQuote, 9);
        simpleRecentlyTradedEmptyQuote = new PQPublishableLevel3Quote(simpleRecentlyTradedSrcTkrInfo.Clone())
        {
            HasUpdates = false
        };
        simpleRecentlyTradedFullyPopulatedQuote = new PQPublishableLevel3Quote(simpleRecentlyTradedSrcTkrInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(simpleRecentlyTradedFullyPopulatedQuote, 10);
        paidGivenVolumeRecentlyTradedEmptyQuote = new PQPublishableLevel3Quote(paidGivenVolumeRecentlyTradedSrcTkrInfo.Clone())
        {
            HasUpdates = false
        };
        paidGivenVolumeRecentlyTradedFullyPopulatedQuote =
            new PQPublishableLevel3Quote(paidGivenVolumeRecentlyTradedSrcTkrInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(paidGivenVolumeRecentlyTradedFullyPopulatedQuote, 10);
        traderPaidGivenVolumeRecentlyTradedEmptyQuote =
            new PQPublishableLevel3Quote(traderPaidGivenVolumeRecentlyTradedSrcTkrInfo.Clone()) { HasUpdates = false };
        traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote =
            new PQPublishableLevel3Quote(traderPaidGivenVolumeRecentlyTradedSrcTkrInfo.Clone());
        quoteSequencedTestDataBuilder
            .InitializeQuote(traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote, 10);


        allFullyPopulatedQuotes = new List<PQPublishableLevel3Quote>
        {
            noRecentlyTradedFullyPopulatedQuote, simpleRecentlyTradedFullyPopulatedQuote
          , paidGivenVolumeRecentlyTradedFullyPopulatedQuote, traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote
        };
        // {
        //     simpleRecentlyTradedFullyPopulatedQuote
        // };
        allEmptyQuotes = new List<PQPublishableLevel3Quote>
        {
            noRecentlyTradedEmptyQuote, simpleRecentlyTradedEmptyQuote, paidGivenVolumeRecentlyTradedEmptyQuote
          , traderPaidGivenVolumeRecentlyTradedEmptyQuote
        };

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void NoRecentlyTradedLevel3Quote_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        Assert.IsNull(noRecentlyTradedEmptyQuote.RecentlyTraded);
        Assert.IsNull(noRecentlyTradedFullyPopulatedQuote.RecentlyTraded);
    }

    [TestMethod]
    public void SimpleLevel3Quote_New_BuildsOnlySimpleLastTradeEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(PQLastTrade), simpleRecentlyTradedEmptyQuote, simpleRecentlyTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void PaidGivenVolumeLevel3Quote_New_BuildsOnlyPaidGivenTradeEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(PQLastPaidGivenTrade), paidGivenVolumeRecentlyTradedEmptyQuote, paidGivenVolumeRecentlyTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void TraderPaidGivenVolumeLevel3Quote_New_BuildsLastTraderPaidGivenEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(PQLastTraderPaidGivenTrade), traderPaidGivenVolumeRecentlyTradedEmptyQuote
           , traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void EmptyLevel3Quote_BatchIdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(noRecentlyTradedEmptyQuote.IsBatchIdUpdated);
        Assert.IsFalse(noRecentlyTradedEmptyQuote.HasUpdates);

        noRecentlyTradedEmptyQuote.Executable          = false;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        noRecentlyTradedEmptyQuote.UpdateComplete();
        noRecentlyTradedEmptyQuote.Executable          = true;
        noRecentlyTradedEmptyQuote.IsExecutableUpdated = false;
        noRecentlyTradedEmptyQuote.HasUpdates          = false;

        Assert.AreEqual(0u, noRecentlyTradedEmptyQuote.BatchId);
        Assert.AreEqual(2, noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedBatchId = 4_111_222_333;
        noRecentlyTradedEmptyQuote.BatchId = expectedBatchId;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.IsBatchIdUpdated);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(expectedBatchId, noRecentlyTradedEmptyQuote.BatchId);
        var level3QuoteUpdates =
            noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, level3QuoteUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQQuoteFields.BatchId, expectedBatchId);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[2]);

        noRecentlyTradedEmptyQuote.IsBatchIdUpdated = false;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeDateUpdated    = false;
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        noRecentlyTradedEmptyQuote.IsBatchIdUpdated = true;
        level3QuoteUpdates =
            (from update in noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQQuoteFields.BatchId
                select update).ToList();
        Assert.AreEqual(1, level3QuoteUpdates.Count);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[0]);

        var newEmpty = new PQPublishableLevel3Quote(noRecentlyTradedSrcTkrInfo);
        newEmpty.UpdateField(level3QuoteUpdates[0]);
        Assert.AreEqual(expectedBatchId, newEmpty.BatchId);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsBatchIdUpdated);
    }

    [TestMethod]
    public void EmptyLevel3Quote_SourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(noRecentlyTradedEmptyQuote.IsSourceQuoteReferenceUpdated);
        Assert.IsFalse(noRecentlyTradedEmptyQuote.HasUpdates);

        noRecentlyTradedEmptyQuote.Executable          = false;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        noRecentlyTradedEmptyQuote.UpdateComplete();
        noRecentlyTradedEmptyQuote.Executable          = true;
        noRecentlyTradedEmptyQuote.IsExecutableUpdated = false;
        noRecentlyTradedEmptyQuote.HasUpdates          = false;

        Assert.AreEqual(0u, noRecentlyTradedEmptyQuote.SourceQuoteReference);
        Assert.AreEqual(2, noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSourceQuoteReference = 4_111_222_333;
        noRecentlyTradedEmptyQuote.SourceQuoteReference = expectedSourceQuoteReference;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(expectedSourceQuoteReference, noRecentlyTradedEmptyQuote.SourceQuoteReference);
        var level3QuoteUpdates = noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, level3QuoteUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQQuoteFields.QuoteSourceQuoteRef, expectedSourceQuoteReference);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[2]);

        noRecentlyTradedEmptyQuote.IsSourceQuoteReferenceUpdated = false;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeDateUpdated    = false;
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        noRecentlyTradedEmptyQuote.IsSourceQuoteReferenceUpdated = true;
        level3QuoteUpdates =
            (from update in noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQQuoteFields.QuoteSourceQuoteRef
                select update).ToList();
        Assert.AreEqual(1, level3QuoteUpdates.Count);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[0]);

        var newEmpty = new PQPublishableLevel3Quote(noRecentlyTradedSrcTkrInfo);
        newEmpty.UpdateField(level3QuoteUpdates[0]);
        Assert.AreEqual(expectedSourceQuoteReference, newEmpty.SourceQuoteReference);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsSourceQuoteReferenceUpdated);
    }

    [TestMethod]
    public void EmptyLevel3Quote_ValueDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(noRecentlyTradedEmptyQuote.IsValueDateUpdated);
        Assert.IsFalse(noRecentlyTradedEmptyQuote.HasUpdates);

        noRecentlyTradedEmptyQuote.Executable          = false;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        noRecentlyTradedEmptyQuote.UpdateComplete();
        noRecentlyTradedEmptyQuote.Executable          = true;
        noRecentlyTradedEmptyQuote.IsExecutableUpdated = false;
        noRecentlyTradedEmptyQuote.HasUpdates          = false;

        Assert.AreEqual(default, noRecentlyTradedEmptyQuote.ValueDate);
        Assert.AreEqual(2, noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedValueDate = new DateTime(2017, 12, 31, 0, 0, 0);
        noRecentlyTradedEmptyQuote.ValueDate = expectedValueDate;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.IsValueDateUpdated);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(expectedValueDate, noRecentlyTradedEmptyQuote.ValueDate);
        var level3QuoteUpdates = noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, level3QuoteUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQQuoteFields.QuoteValueDate, expectedValueDate.Get2MinIntervalsFromUnixEpoch());
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[2]);

        noRecentlyTradedEmptyQuote.IsValueDateUpdated = false;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeDateUpdated    = false;
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        noRecentlyTradedEmptyQuote.IsValueDateUpdated = true;
        level3QuoteUpdates =
            (from update in noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQQuoteFields.QuoteValueDate
                select update).ToList();
        Assert.AreEqual(1, level3QuoteUpdates.Count);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[0]);

        var newEmpty = new PQPublishableLevel3Quote(noRecentlyTradedSrcTkrInfo);
        newEmpty.UpdateField(level3QuoteUpdates[0]);
        Assert.AreEqual(expectedValueDate, newEmpty.ValueDate);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsValueDateUpdated);
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LastTradePriceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            if (emptyQuote.RecentlyTraded == null) continue;
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = emptyQuote.RecentlyTraded[i]!;

                emptyQuote.Executable = false;
                lastTrade.TradePrice       = 22m;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                lastTrade.TradePrice           = 0m;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;

                Assert.IsFalse(lastTrade.IsTradePriceUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0m, lastTrade.TradePrice);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedPrice  = 50.1234m;
                var pqSrcTrkQtInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                var priceScale     = pqSrcTrkQtInfo.PriceScalingPrecision;
                lastTrade.TradePrice = expectedPrice;
                Assert.IsTrue(lastTrade.IsTradePriceUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedPrice, lastTrade.TradePrice);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField
                    = new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAtPrice, PQScaling.Scale(expectedPrice, priceScale), priceScale);
                var depthKey = (PQDepthKey)i;
                var expectedQuoteLastTradeField =
                    new PQFieldUpdate
                        (PQQuoteFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedAtPrice, expectedLastTradeField.Payload, expectedLastTradeField.Flag);
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsTradePriceUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsTradePriceUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.TradingSubId == PQTradingSubFieldKeys.LastTradedAtPrice && update.DepthId == (PQDepthKey)i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.TradePrice          = 0m;
                lastTrade.IsTradePriceUpdated = false;

                var newEmpty = new PQPublishableLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer = newEmpty.RecentlyTraded![i]!;
                Assert.AreEqual(expectedPrice, foundLayer.TradePrice);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundLayer.IsTradePriceUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LastTradeTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            if (emptyQuote.RecentlyTraded == null) continue;
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = emptyQuote.RecentlyTraded[i]!;

                emptyQuote.Executable = false;
                lastTrade.TradePrice  = 22m;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                lastTrade.TradePrice           = 0m;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;

                Assert.IsFalse(lastTrade.IsTradeTimeDateUpdated);
                Assert.IsFalse(lastTrade.IsTradeTimeSub2MinUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(DateTime.MinValue, lastTrade.TradeTime);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedTradeTime = new DateTime(2017, 12, 31, 13, 12, 20).AddTicks(9_999_999);
                lastTrade.TradeTime = expectedTradeTime;
                Assert.IsTrue(lastTrade.IsTradeTimeDateUpdated);
                Assert.IsTrue(lastTrade.IsTradeTimeSub2MinUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedTradeTime, lastTrade.TradeTime);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(4, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(2, lastTradeUpdates.Count);
                var hoursFromEpoch = expectedTradeTime.Get2MinIntervalsFromUnixEpoch();
                var flag           = expectedTradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBase);

                var expectedLastTradeHoursField =
                    new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeTimeDate, hoursFromEpoch);
                var expectedLastTradeSubHoursField =
                    new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, subHourBase, flag);
                Assert.AreEqual(expectedLastTradeHoursField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedLastTradeSubHoursField, lastTradeUpdates[1]);
                var depthKey = (PQDepthKey)i;
                var expectedQuoteLastTradeTimeHoursField =
                    new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedTradeTimeDate, hoursFromEpoch);
                var expectedQuoteLastTradeTimeSubHoursField =
                    new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, subHourBase, flag);
                Assert.AreEqual(expectedQuoteLastTradeTimeHoursField, quoteUpdates[2]);
                Assert.AreEqual(expectedQuoteLastTradeTimeSubHoursField, quoteUpdates[3]);

                lastTrade.IsTradeTimeDateUpdated = false;
                Assert.IsTrue(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                var lastTradeJustSubHourUpdate =
                    lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(1, lastTradeJustSubHourUpdate.Count);
                Assert.AreEqual(expectedLastTradeSubHoursField, lastTradeJustSubHourUpdate[0]);
                var quoteSubHourUpdates =
                    emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteSubHourUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeTimeSubHoursField, quoteSubHourUpdates[2]);

                lastTrade.IsTradeTimeSub2MinUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsTradeTimeDateUpdated    = true;
                lastTrade.IsTradeTimeSub2MinUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where (update.TradingSubId == PQTradingSubFieldKeys.LastTradedTradeTimeDate && update.DepthId == (PQDepthKey)i) ||
                              (update.TradingSubId == PQTradingSubFieldKeys.LastTradedTradeSub2MinTime && update.DepthId == (PQDepthKey)i)
                        select update).ToList();
                Assert.AreEqual(2, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeTimeHoursField, quoteUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeTimeSubHoursField, quoteUpdates[1]);
                lastTrade.TradeTime                 = DateTime.MinValue;
                lastTrade.IsTradeTimeDateUpdated    = false;
                lastTrade.IsTradeTimeSub2MinUpdated = false;

                var newEmpty = new PQPublishableLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                newEmpty.UpdateField(quoteUpdates[1]);
                var foundLayer = newEmpty.RecentlyTraded![i]!;
                Assert.AreEqual(expectedTradeTime, foundLayer.TradeTime);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundLayer.IsTradeTimeDateUpdated);
                Assert.IsTrue(foundLayer.IsTradeTimeSub2MinUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LastTradeWasGivenChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            if (!(emptyQuote.RecentlyTraded?[0] is IPQLastPaidGivenTrade)) continue;
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = (IPQLastPaidGivenTrade)emptyQuote.RecentlyTraded[i]!;

                emptyQuote.Executable = false;
                lastTrade.TradePrice  = 22m;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                lastTrade.TradePrice           = 0m;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;


                Assert.IsFalse(lastTrade.IsWasGivenUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(false, lastTrade.WasGiven);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                const bool expectedWasGiven = true;
                lastTrade.WasGiven = expectedWasGiven;
                var pqSrcTrkQtInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                Assert.IsTrue(lastTrade.IsWasGivenUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedWasGiven, lastTrade.WasGiven);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField = new PQFieldUpdate
                    (PQQuoteFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasGiven);
                var depthKey = (PQDepthKey)i;
                var expectedQuoteLastTradeField = new PQFieldUpdate
                    (PQQuoteFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasGiven);
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsWasGivenUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsWasGivenUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.TradingSubId == PQTradingSubFieldKeys.LastTradedBooleanFlags && update.DepthId == (PQDepthKey)i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.WasGiven          = false;
                lastTrade.IsWasGivenUpdated = false;

                var newEmpty = new PQPublishableLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer = (IPQLastPaidGivenTrade)newEmpty.RecentlyTraded![i]!;
                Assert.AreEqual(expectedWasGiven, foundLayer.WasGiven);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundLayer.IsWasGivenUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LastTradeWasPaidChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            if (!(emptyQuote.RecentlyTraded?[0] is IPQLastPaidGivenTrade)) continue;
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = (IPQLastPaidGivenTrade)emptyQuote.RecentlyTraded[i]!;

                emptyQuote.Executable = false;
                lastTrade.TradePrice  = 22m;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                lastTrade.TradePrice           = 0m;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;

                Assert.IsFalse(lastTrade.IsWasPaidUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(false, lastTrade.WasPaid);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                const bool expectedWasPaid = true;
                lastTrade.WasPaid = expectedWasPaid;
                var pqSrcTrkQtInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                Assert.IsTrue(lastTrade.IsWasPaidUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedWasPaid, lastTrade.WasPaid);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField =
                    new PQFieldUpdate(PQQuoteFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasPaid);
                var depthKey = (PQDepthKey)i;
                var expectedQuoteLastTradeField = new PQFieldUpdate
                    (PQQuoteFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasPaid);
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsWasPaidUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsWasPaidUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.TradingSubId == PQTradingSubFieldKeys.LastTradedBooleanFlags && update.DepthId == (PQDepthKey)i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.WasPaid          = false;
                lastTrade.IsWasPaidUpdated = false;

                var newEmpty = new PQPublishableLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer = (IPQLastPaidGivenTrade)newEmpty.RecentlyTraded![i]!;
                Assert.AreEqual(expectedWasPaid, foundLayer.WasPaid);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundLayer.IsWasPaidUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersCountPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQOrdersCountPriceVolumeLayer)bidBook[i]!;
                    PQOrdersCountPriceVolumeLayerTests.AssertOrdersCountFieldUpdatesReturnAsExpected(ordersCountLayer, i, bidBook, orderBook
                   , emptyQuote);
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQOrdersCountPriceVolumeLayer)askBook[i]!;
                    PQOrdersCountPriceVolumeLayerTests.AssertOrdersCountFieldUpdatesReturnAsExpected(ordersCountLayer, i, askBook, orderBook
                   , emptyQuote);
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderIdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderIdFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderIdFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, askBook
                       , orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderFlagsFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderFlagsFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, askBook
                       , orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderCreatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderCreatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderCreatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , askBook, orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderUpdatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderUpdatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderUpdatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , askBook, orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, askBook
                       , orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderRemainingVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersAnonymousPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderRemainingVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderLayerInfoTests.AssertOrdersOrderRemainingVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , askBook, orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderCounterPartyNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersFullPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQCounterPartyOrderLayerInfo)ordersLayer[j]!;
                        PQCounterPartyOrderLayerInfoTests.AssertOrdersCounterPartyNameFieldUpdatesReturnAsExpected
                            (cpOrderInfo, j, ordersLayer, i, bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQCounterPartyOrderLayerInfo)ordersLayer[j]!;
                        PQCounterPartyOrderLayerInfoTests.AssertOrdersCounterPartyNameFieldUpdatesReturnAsExpected
                            (cpOrderInfo, j, ordersLayer, i, askBook, orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var orderBook = emptyQuote.OrderBook;
            if (orderBook.LayerSupportedFlags.HasAllOf(LayerType.OrdersFullPriceVolume.SupportedLayerFlags()))
            {
                var bidBook = orderBook.BidSide;

                for (int i = 0; i < bidBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQCounterPartyOrderLayerInfo)ordersLayer[j]!;
                        PQCounterPartyOrderLayerInfoTests.AssertOrdersTraderNameFieldUpdatesReturnAsExpected(cpOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i]!;
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQCounterPartyOrderLayerInfo)ordersLayer[j]!;
                        PQCounterPartyOrderLayerInfoTests.AssertOrdersTraderNameFieldUpdatesReturnAsExpected(cpOrderInfo, j, ordersLayer, i, askBook
                       , orderBook, emptyQuote);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void AllFullyPopulatedQuotes_HasUpdatesSetFalse_RemovesUpdatesFromAllLastTrades()
    {
        foreach (var populatedQuote in allFullyPopulatedQuotes)
        {
            if (populatedQuote.RecentlyTraded == null) continue;
            foreach (var lastTrade in populatedQuote.RecentlyTraded) Assert.IsTrue(lastTrade.HasUpdates);

            populatedQuote.HasUpdates = false;

            foreach (var lastTrade in populatedQuote.RecentlyTraded) Assert.IsFalse(lastTrade.HasUpdates);
        }
    }

    [TestMethod]
    public void AllFullyPopulatedQuotes_Reset_SameAsEmptyQuotes()
    {
        Assert.AreEqual(allFullyPopulatedQuotes.Count, allEmptyQuotes.Count);
        for (var i = 0; i < allFullyPopulatedQuotes.Count; i++)
        {
            var popQuote   = allFullyPopulatedQuotes[i];
            var emptyQuote = allEmptyQuotes[i];

            Assert.IsFalse(popQuote.AreEquivalent(emptyQuote));

            popQuote.ResetFields();

            Assert.IsTrue(popQuote.AreEquivalent(emptyQuote));
        }
    }

    [TestMethod]
    public void PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel3Fields()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            var precisionSettings = (PQSourceTickerInfo)populatedL3Quote.SourceTickerInfo!;
            var pqFieldUpdates =
                populatedL3Quote.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Update, precisionSettings).ToList();
            AssertContainsAllLevel3Fields(precisionSettings, pqFieldUpdates, populatedL3Quote);
        }
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel3Fields()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            var precisionSettings = (PQSourceTickerInfo)populatedL3Quote.SourceTickerInfo!;
            populatedL3Quote.HasUpdates = false;
            var pqFieldUpdates =
                populatedL3Quote.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 12, 33, 1), StorageFlags.Snapshot, precisionSettings).ToList();
            AssertContainsAllLevel3Fields(precisionSettings, pqFieldUpdates, populatedL3Quote);
        }
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            populatedL3Quote.IsReplay   = true;
            populatedL3Quote.HasUpdates = false;
            var pqFieldUpdates =
                populatedL3Quote.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            var pqStringUpdates =
                populatedL3Quote.GetStringUpdates
                    (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
            Assert.AreEqual(0, pqFieldUpdates.Count);
            Assert.AreEqual(0, pqStringUpdates.Count);
        }
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            var pqFieldUpdates =
                populatedL3Quote.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var pqStringUpdates =
                populatedL3Quote.GetStringUpdates
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL3Quote.SourceTickerInfo!)
                {
                    NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand)
                };
            var newEmpty = new PQPublishableLevel3Quote(emptyQuoteSourceTickerInfo);
            foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
            foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
            // not copied from field updates as is used in by server to track publication times.
            newEmpty.PQSequenceId        = populatedL3Quote.PQSequenceId;
            newEmpty.LastPublicationTime = populatedL3Quote.LastPublicationTime;
            Console.WriteLine(populatedL3Quote.DiffQuotes(newEmpty));
            Assert.AreEqual(populatedL3Quote, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        foreach (var pqLevel3Quote in allFullyPopulatedQuotes)
        {
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(pqLevel3Quote.SourceTickerInfo!);
            var newEmpty = new PQPublishableLevel3Quote(emptyQuoteSourceTickerInfo);
            newEmpty.CopyFrom(pqLevel3Quote);
            Assert.AreEqual(pqLevel3Quote, newEmpty);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        foreach (var pqLevel3Quote in allFullyPopulatedQuotes)
        {
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(pqLevel3Quote.SourceTickerInfo!);
            var emptyQuote = new PQPublishableLevel3Quote(emptyQuoteSourceTickerInfo);
            pqLevel3Quote.HasUpdates = false;
            emptyQuote.CopyFrom(pqLevel3Quote);
            Assert.AreEqual(0u, emptyQuote.BatchId);
            Assert.AreEqual(0u, emptyQuote.SourceQuoteReference);
            Assert.AreEqual(default, emptyQuote.ValueDate);
            Assert.AreEqual(pqLevel3Quote.PQSequenceId, emptyQuote.PQSequenceId);
            Assert.AreEqual(default, emptyQuote.SourceTime);
            Assert.IsTrue(pqLevel3Quote.SourceTickerInfo!.AreEquivalent(emptyQuote.SourceTickerInfo));
            Assert.AreEqual(false, emptyQuote.IsReplay);
            Assert.AreEqual(0m, emptyQuote.SingleTickValue);
            Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
            Assert.AreEqual(default, emptyQuote.SourceBidTime);
            Assert.AreEqual(default, emptyQuote.SourceAskTime);
            Assert.AreEqual(default, emptyQuote.AdapterReceivedTime);
            Assert.AreEqual(default, emptyQuote.AdapterSentTime);
            Assert.AreEqual(default, emptyQuote.ClientReceivedTime);
            Assert.AreEqual(default, emptyQuote.ProcessedTime);
            Assert.AreEqual(default, emptyQuote.DispatchedTime);
            Assert.AreEqual(default, emptyQuote.SocketReceivingTime);
            Assert.AreEqual(0m, emptyQuote.BidPriceTop);
            Assert.AreEqual(0m, emptyQuote.AskPriceTop);
            Assert.IsTrue(emptyQuote.Executable);
            Assert.IsFalse(emptyQuote.IsBatchIdUpdated);
            Assert.IsFalse(emptyQuote.IsSourceQuoteReferenceUpdated);
            Assert.IsFalse(emptyQuote.IsValueDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsReplayUpdated);
            Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
            Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsBidPriceTopUpdated);
            Assert.IsFalse(emptyQuote.IsAskPriceTopUpdated);
            Assert.IsFalse(emptyQuote.IsExecutableUpdated);
            foreach (var pvl in emptyQuote.BidBook) PQLevel2QuoteTests.AssertAreDefaultValues(pvl);
            foreach (var pvl in emptyQuote.AskBook) PQLevel2QuoteTests.AssertAreDefaultValues(pvl);
            if (emptyQuote.RecentlyTraded == null) continue;
            foreach (var lastTraded in emptyQuote.RecentlyTraded) AssertAreDefaultValues(lastTraded);
        }
    }

    [TestMethod]
    public void NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            var nonPQLevel3Quote = new PublishableLevel3PriceQuote(populatedL3Quote);
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL3Quote.SourceTickerInfo!);
            var newEmpty = new PQPublishableLevel3Quote(emptyQuoteSourceTickerInfo);
            newEmpty.CopyFrom(nonPQLevel3Quote);
            Assert.IsTrue(populatedL3Quote.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            var clonedQuote = ((ICloneable<IPublishableTickInstant>)populatedL3Quote).Clone();
            Assert.AreNotSame(clonedQuote, populatedL3Quote);
            Assert.AreEqual
                (populatedL3Quote, clonedQuote
               , "clonedQuote differences are \n '" + clonedQuote.DiffQuotes(populatedL3Quote) + "'");

            var cloned2 = (PQPublishableLevel3Quote)((ICloneable)populatedL3Quote).Clone();
            Assert.AreNotSame(cloned2, populatedL3Quote);
            if (!cloned2.Equals(populatedL3Quote))
                Console.Out.WriteLine("clonedQuote differences are \n '" + cloned2.DiffQuotes(populatedL3Quote) + "'");
            Assert.AreEqual(populatedL3Quote, cloned2);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            var fullyPopulatedClone = (PQPublishableLevel3Quote)((ICloneable)populatedL3Quote).Clone();
            // by default SourceTickerInfo is shared
            fullyPopulatedClone.SourceTickerInfo
                = new PQSourceTickerInfo(populatedL3Quote.SourceTickerInfo!);
            AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedL3Quote, fullyPopulatedClone);
            AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedL3Quote, fullyPopulatedClone);
        }
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            Assert.AreEqual(populatedL3Quote, populatedL3Quote);
            Assert.AreEqual(populatedL3Quote, ((ICloneable)populatedL3Quote).Clone());
            Assert.AreEqual(populatedL3Quote, ((ICloneable<IPublishableTickInstant>)populatedL3Quote).Clone());
            Assert.AreEqual(populatedL3Quote, ((ICloneable<IPublishableLevel1Quote>)populatedL3Quote).Clone());
            Assert.AreEqual(populatedL3Quote, ((ICloneable<IPublishableLevel2Quote>)populatedL3Quote).Clone());
            Assert.AreEqual(populatedL3Quote, ((ICloneable<IPublishableLevel3Quote>)populatedL3Quote).Clone());
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_GetHashCode_ReturnNumberNoException()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            var hashCode = populatedL3Quote.GetHashCode();
            Assert.IsTrue(hashCode != 0);
        }
    }


    [TestMethod]
    public void NoRecentlyTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = noRecentlyTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SimpleRecentlyTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = simpleRecentlyTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void PaidGivenRecentlyTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = paidGivenVolumeRecentlyTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void TraderPaidGivenRecentlyFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
        (bool exactComparison, PQPublishableLevel3Quote original, PQPublishableLevel3Quote changingLevel3Quote)
    {
        PQLevel1QuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison, original, changingLevel3Quote);

        if (original.GetType() == typeof(PQPublishableLevel3Quote))
            Assert.AreEqual(!exactComparison,
                            changingLevel3Quote.AreEquivalent(new PublishableLevel3PriceQuote(original), exactComparison));

        changingLevel3Quote.BatchId = 4_294_967_295u;
        Assert.IsFalse(changingLevel3Quote.AreEquivalent(original, exactComparison));
        changingLevel3Quote.BatchId = original.BatchId;
        Assert.IsTrue(original.AreEquivalent(changingLevel3Quote, exactComparison));

        changingLevel3Quote.SourceQuoteReference = 4_294_967_295u;
        Assert.IsFalse(changingLevel3Quote.AreEquivalent(original, exactComparison));
        changingLevel3Quote.SourceQuoteReference = original.SourceQuoteReference;
        Assert.IsTrue(original.AreEquivalent(changingLevel3Quote, exactComparison));

        changingLevel3Quote.ValueDate = new DateTime(2018, 01, 01, 19, 0, 0);
        Assert.IsFalse(original.AreEquivalent(changingLevel3Quote, exactComparison));
        changingLevel3Quote.ValueDate = original.ValueDate;
        Assert.IsTrue(changingLevel3Quote.AreEquivalent(original, exactComparison));

        Assert.IsTrue((original.RecentlyTraded != null && changingLevel3Quote.RecentlyTraded != null) ||
                      (original.RecentlyTraded == null && changingLevel3Quote.RecentlyTraded == null));
        if (original.RecentlyTraded != null)
            PQRecentlyTradedTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (exactComparison, (PQRecentlyTraded)original.RecentlyTraded, (PQRecentlyTraded)changingLevel3Quote.RecentlyTraded!,
                 original, changingLevel3Quote);
    }

    public static void AssertContainsAllLevel3Fields
    (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates,
        PQPublishableLevel3Quote l3Q, PQBooleanValues expectedBooleanFlags = PQBooleanValuesExtensions.AllFields)
    {
        PQLevel2QuoteTests.AssertContainsAllLevel2Fields(precisionSettings, checkFieldUpdates, l3Q, expectedBooleanFlags);

        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.BatchId, l3Q.BatchId),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.BatchId));
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.QuoteSourceQuoteRef, l3Q.SourceQuoteReference),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.QuoteSourceQuoteRef));
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.QuoteValueDate, l3Q.ValueDate.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.QuoteValueDate));
        if (l3Q.RecentlyTraded == null) return;
        var pqSrcTkrInfo = l3Q.SourceTickerInfo!;

        PQRecentlyTradedTests.AssertContainsAllLevelRecentlyTradedFields
            (checkFieldUpdates, l3Q.RecentlyTraded, expectedBooleanFlags, 
             pqSrcTkrInfo.PriceScalingPrecision, pqSrcTkrInfo.VolumeScalingPrecision );
    }

    public static void AssertAreDefaultValues(IPQLastTrade pvl)
    {
        Assert.AreEqual(0m, pvl.TradePrice);
        Assert.AreEqual(DateTime.MinValue, pvl.TradeTime);
        Assert.IsFalse(pvl.IsTradePriceUpdated);
        Assert.IsFalse(pvl.IsTradeTimeDateUpdated);
        Assert.IsFalse(pvl.IsTradeTimeSub2MinUpdated);
        if (pvl is IPQLastPaidGivenTrade paidGivenTrade)
        {
            Assert.AreEqual(0m, paidGivenTrade.TradeVolume);
            Assert.IsFalse(paidGivenTrade.WasGiven);
            Assert.IsFalse(paidGivenTrade.WasPaid);
            Assert.IsFalse(paidGivenTrade.IsTradeVolumeUpdated);
            Assert.IsFalse(paidGivenTrade.IsWasGivenUpdated);
            Assert.IsFalse(paidGivenTrade.IsWasPaidUpdated);
        }

        if (pvl is IPQLastTraderPaidGivenTrade traderPaidGivenTrade)
        {
            Assert.IsNull(traderPaidGivenTrade.TraderName);
            Assert.IsFalse(traderPaidGivenTrade.IsTraderNameUpdated);
        }
    }

    private void AssertLastTradeTypeIsExpected(Type expectedType, params PQPublishableLevel3Quote[] quotesToCheck)
    {
        foreach (var level3Quote in quotesToCheck)
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
                Assert.AreEqual(expectedType, level3Quote.RecentlyTraded![i]!.GetType());
    }
}
