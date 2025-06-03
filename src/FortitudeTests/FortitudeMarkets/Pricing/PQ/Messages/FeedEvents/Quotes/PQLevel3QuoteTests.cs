// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

[TestClass]
public class PQLevel3QuoteTests
{
    private IList<PQPublishableLevel3Quote> allEmptyQuotes          = null!;
    private IList<PQPublishableLevel3Quote> allFullyPopulatedQuotes = null!;

    private PQPublishableLevel3Quote noOnTickLastTradedEmptyQuote          = null!;
    private PQPublishableLevel3Quote noOnTickLastTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo noOnTickLastTradedSrcTkrInfo = null!;

    private PQPublishableLevel3Quote paidGivenVolumeOnTickLastTradedEmptyQuote          = null!;
    private PQPublishableLevel3Quote paidGivenVolumeOnTickLastTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo             paidGivenVolumeOnTickLastTradedSrcTkrInfo = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder             = null!;

    private PQPublishableLevel3Quote simpleOnTickLastTradedEmptyQuote          = null!;
    private PQPublishableLevel3Quote simpleOnTickLastTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo simpleOnTickLastTradedSrcTkrInfo = null!;

    private DateTime                 testDateTime;
    private PQPublishableLevel3Quote fullSupportOnTickLastTradedEmptyQuote          = null!;
    private PQPublishableLevel3Quote fullSupportOnTickLastTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo fullSupportOnTickLastTradedSrcTkrInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        noOnTickLastTradedSrcTkrInfo              = PQSourceTickerInfoTests.FullSupportL3NoOnTickLastTradedSti;
        simpleOnTickLastTradedSrcTkrInfo          = PQSourceTickerInfoTests.OrdersAnonL3JustTradeTradeSti;
        paidGivenVolumeOnTickLastTradedSrcTkrInfo = PQSourceTickerInfoTests.FullSupportL3PaidOrGivenTradeSti;
        fullSupportOnTickLastTradedSrcTkrInfo     = PQSourceTickerInfoTests.FullSupportL3TraderNamePaidOrGivenSti;
        noOnTickLastTradedEmptyQuote              = new PQPublishableLevel3Quote(noOnTickLastTradedSrcTkrInfo.Clone());
        noOnTickLastTradedEmptyQuote.UpdateStarted(1);
        noOnTickLastTradedEmptyQuote.HasUpdates = false;
        noOnTickLastTradedFullyPopulatedQuote   = new PQPublishableLevel3Quote(noOnTickLastTradedSrcTkrInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(noOnTickLastTradedFullyPopulatedQuote, 9);
        simpleOnTickLastTradedEmptyQuote = new PQPublishableLevel3Quote(simpleOnTickLastTradedSrcTkrInfo.Clone());
        simpleOnTickLastTradedEmptyQuote.UpdateStarted(1);
        simpleOnTickLastTradedEmptyQuote.HasUpdates   = false;
        simpleOnTickLastTradedFullyPopulatedQuote = new PQPublishableLevel3Quote(simpleOnTickLastTradedSrcTkrInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(simpleOnTickLastTradedFullyPopulatedQuote, 10);
        paidGivenVolumeOnTickLastTradedEmptyQuote = new PQPublishableLevel3Quote(paidGivenVolumeOnTickLastTradedSrcTkrInfo.Clone());
        paidGivenVolumeOnTickLastTradedEmptyQuote.UpdateStarted(1);
        paidGivenVolumeOnTickLastTradedEmptyQuote.HasUpdates   = false;
        paidGivenVolumeOnTickLastTradedFullyPopulatedQuote =
            new PQPublishableLevel3Quote(paidGivenVolumeOnTickLastTradedSrcTkrInfo.Clone());
        quoteSequencedTestDataBuilder.InitializeQuote(paidGivenVolumeOnTickLastTradedFullyPopulatedQuote, 10);
        fullSupportOnTickLastTradedEmptyQuote =
            new PQPublishableLevel3Quote(fullSupportOnTickLastTradedSrcTkrInfo.Clone());
        fullSupportOnTickLastTradedEmptyQuote.UpdateStarted(1);
        fullSupportOnTickLastTradedEmptyQuote.HasUpdates   = false;
        fullSupportOnTickLastTradedFullyPopulatedQuote =
            new PQPublishableLevel3Quote(fullSupportOnTickLastTradedSrcTkrInfo.Clone());
        quoteSequencedTestDataBuilder
            .InitializeQuote(fullSupportOnTickLastTradedFullyPopulatedQuote, 10);


        allFullyPopulatedQuotes = new List<PQPublishableLevel3Quote>
        {
            noOnTickLastTradedFullyPopulatedQuote, simpleOnTickLastTradedFullyPopulatedQuote
          , paidGivenVolumeOnTickLastTradedFullyPopulatedQuote, fullSupportOnTickLastTradedFullyPopulatedQuote
        };
        // {
        //     simpleOnTickLastTradedFullyPopulatedQuote
        // };
        allEmptyQuotes = new List<PQPublishableLevel3Quote>
        {
            noOnTickLastTradedEmptyQuote, simpleOnTickLastTradedEmptyQuote, paidGivenVolumeOnTickLastTradedEmptyQuote
          , fullSupportOnTickLastTradedEmptyQuote
        };

        testDateTime = new DateTime(2017, 10, 08, 18, 33, 24);
    }

    [TestMethod]
    public void NoOnTickLastTradedLevel3Quote_New_BuildsOnlyPriceVolumeLayeredBook()
    {
        Assert.IsNull(noOnTickLastTradedEmptyQuote.OnTickLastTraded);
        Assert.IsNull(noOnTickLastTradedFullyPopulatedQuote.OnTickLastTraded);
    }

    [TestMethod]
    public void SimpleLevel3Quote_New_BuildsOnlySimpleLastTradeEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(PQLastTrade), simpleOnTickLastTradedEmptyQuote, simpleOnTickLastTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void PaidGivenVolumeLevel3Quote_New_BuildsOnlyPaidGivenTradeEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(PQLastPaidGivenTrade), paidGivenVolumeOnTickLastTradedEmptyQuote, paidGivenVolumeOnTickLastTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void TraderPaidGivenVolumeLevel3Quote_New_BuildsLastTraderPaidGivenEntries()
    {
        AssertLastTradeTypeIsExpected
            (typeof(PQLastExternalCounterPartyTrade), fullSupportOnTickLastTradedEmptyQuote
           , fullSupportOnTickLastTradedFullyPopulatedQuote);
    }

    [TestMethod]
    public void EmptyLevel3Quote_BatchIdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(noOnTickLastTradedEmptyQuote.IsBatchIdUpdated);
        Assert.IsFalse(noOnTickLastTradedEmptyQuote.HasUpdates);

        noOnTickLastTradedEmptyQuote.Executable = false;
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.HasUpdates);
        noOnTickLastTradedEmptyQuote.UpdateComplete();
        noOnTickLastTradedEmptyQuote.Executable          = true;
        noOnTickLastTradedEmptyQuote.IsExecutableUpdated = false;
        noOnTickLastTradedEmptyQuote.HasUpdates          = false;

        Assert.AreEqual(0u, noOnTickLastTradedEmptyQuote.BatchId);
        Assert.AreEqual(2, noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedBatchId = 4_111_222_333;
        noOnTickLastTradedEmptyQuote.BatchId = expectedBatchId;
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.IsBatchIdUpdated);
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(expectedBatchId, noOnTickLastTradedEmptyQuote.BatchId);
        var level3QuoteUpdates =
            noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(3, level3QuoteUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteBatchId, expectedBatchId);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[2]);

        noOnTickLastTradedEmptyQuote.IsBatchIdUpdated = false;
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.HasUpdates);
        noOnTickLastTradedEmptyQuote.IsAdapterSentTimeDateUpdated    = false;
        noOnTickLastTradedEmptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(noOnTickLastTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(2, noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        noOnTickLastTradedEmptyQuote.IsBatchIdUpdated = true;
        level3QuoteUpdates =
            (from update in noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                where update.Id == PQFeedFields.QuoteBatchId
                select update).ToList();
        Assert.AreEqual(1, level3QuoteUpdates.Count);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[0]);

        var newEmpty = new PQPublishableLevel3Quote(noOnTickLastTradedSrcTkrInfo);
        newEmpty.UpdateField(level3QuoteUpdates[0]);
        Assert.AreEqual(expectedBatchId, newEmpty.BatchId);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsBatchIdUpdated);
    }

    [TestMethod]
    public void EmptyLevel3Quote_SourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(noOnTickLastTradedEmptyQuote.IsSourceQuoteReferenceUpdated);
        Assert.IsFalse(noOnTickLastTradedEmptyQuote.HasUpdates);

        noOnTickLastTradedEmptyQuote.Executable = false;
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.HasUpdates);
        noOnTickLastTradedEmptyQuote.UpdateComplete();
        noOnTickLastTradedEmptyQuote.Executable          = true;
        noOnTickLastTradedEmptyQuote.IsExecutableUpdated = false;
        noOnTickLastTradedEmptyQuote.HasUpdates          = false;

        Assert.AreEqual(0u, noOnTickLastTradedEmptyQuote.SourceQuoteReference);
        Assert.AreEqual(2, noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedSourceQuoteReference = 4_111_222_333;
        noOnTickLastTradedEmptyQuote.SourceQuoteReference = expectedSourceQuoteReference;
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(expectedSourceQuoteReference, noOnTickLastTradedEmptyQuote.SourceQuoteReference);
        var level3QuoteUpdates = noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(3, level3QuoteUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteSourceQuoteRef, expectedSourceQuoteReference);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[2]);

        noOnTickLastTradedEmptyQuote.IsSourceQuoteReferenceUpdated = false;
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.HasUpdates);
        noOnTickLastTradedEmptyQuote.IsAdapterSentTimeDateUpdated    = false;
        noOnTickLastTradedEmptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(noOnTickLastTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(2, noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        noOnTickLastTradedEmptyQuote.IsSourceQuoteReferenceUpdated = true;
        level3QuoteUpdates =
            (from update in noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                where update.Id == PQFeedFields.QuoteSourceQuoteRef
                select update).ToList();
        Assert.AreEqual(1, level3QuoteUpdates.Count);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[0]);

        var newEmpty = new PQPublishableLevel3Quote(noOnTickLastTradedSrcTkrInfo);
        newEmpty.UpdateField(level3QuoteUpdates[0]);
        Assert.AreEqual(expectedSourceQuoteReference, newEmpty.SourceQuoteReference);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsSourceQuoteReferenceUpdated);
    }

    [TestMethod]
    public void EmptyLevel3Quote_ValueDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(noOnTickLastTradedEmptyQuote.IsValueDateUpdated);
        Assert.IsFalse(noOnTickLastTradedEmptyQuote.HasUpdates);

        noOnTickLastTradedEmptyQuote.Executable = false;
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.HasUpdates);
        noOnTickLastTradedEmptyQuote.UpdateComplete();
        noOnTickLastTradedEmptyQuote.Executable          = true;
        noOnTickLastTradedEmptyQuote.IsExecutableUpdated = false;
        noOnTickLastTradedEmptyQuote.HasUpdates          = false;

        Assert.AreEqual(default, noOnTickLastTradedEmptyQuote.ValueDate);
        Assert.AreEqual(2, noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedValueDate = new DateTime(2017, 12, 31, 0, 0, 0);
        noOnTickLastTradedEmptyQuote.ValueDate = expectedValueDate;
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.IsValueDateUpdated);
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(expectedValueDate, noOnTickLastTradedEmptyQuote.ValueDate);
        var level3QuoteUpdates = noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(3, level3QuoteUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFeedFields.QuoteValueDate, expectedValueDate.Get2MinIntervalsFromUnixEpoch());
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[2]);

        noOnTickLastTradedEmptyQuote.IsValueDateUpdated = false;
        Assert.IsTrue(noOnTickLastTradedEmptyQuote.HasUpdates);
        noOnTickLastTradedEmptyQuote.IsAdapterSentTimeDateUpdated    = false;
        noOnTickLastTradedEmptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
        Assert.IsFalse(noOnTickLastTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(2, noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        noOnTickLastTradedEmptyQuote.IsValueDateUpdated = true;
        level3QuoteUpdates =
            (from update in noOnTickLastTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                where update.Id == PQFeedFields.QuoteValueDate
                select update).ToList();
        Assert.AreEqual(1, level3QuoteUpdates.Count);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[0]);

        var newEmpty = new PQPublishableLevel3Quote(noOnTickLastTradedSrcTkrInfo);
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
            if (emptyQuote.OnTickLastTraded == null) continue;
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = emptyQuote.OnTickLastTraded[i];

                emptyQuote.Executable = false;
                lastTrade.TradePrice  = 22m;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                lastTrade.TradePrice           = 0m;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;

                Assert.IsFalse(lastTrade.IsTradePriceUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0m, lastTrade.TradePrice);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

                var expectedPrice  = 50.1234m;
                var pqSrcTrkQtInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                var priceScale     = pqSrcTrkQtInfo.PriceScalingPrecision;
                lastTrade.TradePrice = expectedPrice;
                Assert.IsTrue(lastTrade.IsTradePriceUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedPrice, lastTrade.TradePrice);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField
                    = new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAtPrice
                                      , PQScaling.Scale(expectedPrice, priceScale), priceScale);
                var depthKey = (PQDepthKey)i;
                var expectedQuoteLastTradeField =
                    new PQFieldUpdate
                        (PQFeedFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedAtPrice, expectedLastTradeField.Payload
                       , expectedLastTradeField.Flag);
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsTradePriceUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

                lastTrade.IsTradePriceUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pqSrcTrkQtInfo)
                        where update.TradingSubId == PQTradingSubFieldKeys.LastTradedAtPrice && update.DepthId == (PQDepthKey)i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.TradePrice          = 0m;
                lastTrade.IsTradePriceUpdated = false;

                var newEmpty = new PQPublishableLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer = newEmpty.OnTickLastTraded![i];
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
            if (emptyQuote.OnTickLastTraded == null) continue;
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = emptyQuote.OnTickLastTraded[i];

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
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

                var expectedTradeTime = new DateTime(2017, 12, 31, 13, 12, 20).AddTicks(9_999_999);
                lastTrade.TradeTime = expectedTradeTime;
                Assert.IsTrue(lastTrade.IsTradeTimeDateUpdated);
                Assert.IsTrue(lastTrade.IsTradeTimeSub2MinUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedTradeTime, lastTrade.TradeTime);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
                Assert.AreEqual(4, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
                Assert.AreEqual(2, lastTradeUpdates.Count);
                var hoursFromEpoch = expectedTradeTime.Get2MinIntervalsFromUnixEpoch();
                var flag           = expectedTradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBase);

                var expectedLastTradeHoursField =
                    new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeTimeDate, hoursFromEpoch);
                var expectedLastTradeSub2MinField =
                    new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, subHourBase, flag);
                Assert.AreEqual(expectedLastTradeHoursField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedLastTradeSub2MinField, lastTradeUpdates[1]);
                var depthKey = (PQDepthKey)i;
                var expectedQuoteLastTradeTimeHoursField =
                    new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedTradeTimeDate, hoursFromEpoch);
                var expectedLastTradeTimeSub2MinField =
                    new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, subHourBase
                                    , flag);
                Assert.AreEqual(expectedQuoteLastTradeTimeHoursField, quoteUpdates[2]);
                Assert.AreEqual(expectedLastTradeTimeSub2MinField, quoteUpdates[3]);

                lastTrade.IsTradeTimeDateUpdated = false;
                Assert.IsTrue(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                var lastTradeJustSub2MinUpdate =
                    lastTrade.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
                Assert.AreEqual(1, lastTradeJustSub2MinUpdate.Count);
                Assert.AreEqual(expectedLastTradeSub2MinField, lastTradeJustSub2MinUpdate[0]);
                var quoteSub2MinUpdates =
                    emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
                Assert.AreEqual(3, quoteSub2MinUpdates.Count);
                Assert.AreEqual(expectedLastTradeTimeSub2MinField, quoteSub2MinUpdates[2]);

                lastTrade.IsTradeTimeSub2MinUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

                lastTrade.IsTradeTimeDateUpdated    = true;
                lastTrade.IsTradeTimeSub2MinUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                        where (update.TradingSubId == PQTradingSubFieldKeys.LastTradedTradeTimeDate && update.DepthId == (PQDepthKey)i) ||
                              (update.TradingSubId == PQTradingSubFieldKeys.LastTradedTradeSub2MinTime && update.DepthId == (PQDepthKey)i)
                        select update).ToList();
                Assert.AreEqual(2, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeTimeHoursField, quoteUpdates[0]);
                Assert.AreEqual(expectedLastTradeTimeSub2MinField, quoteUpdates[1]);
                lastTrade.TradeTime                 = DateTime.MinValue;
                lastTrade.IsTradeTimeDateUpdated    = false;
                lastTrade.IsTradeTimeSub2MinUpdated = false;

                var newEmpty = new PQPublishableLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                newEmpty.UpdateField(quoteUpdates[1]);
                var foundLayer = newEmpty.OnTickLastTraded![i];
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
            if (!(emptyQuote.OnTickLastTraded?[0] is IPQLastPaidGivenTrade)) continue;
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = (IPQLastPaidGivenTrade)emptyQuote.OnTickLastTraded[i];

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
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

                const bool expectedWasGiven = true;
                lastTrade.WasGiven = expectedWasGiven;
                var pqSrcTrkQtInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                Assert.IsTrue(lastTrade.IsWasGivenUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedWasGiven, lastTrade.WasGiven);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField = new PQFieldUpdate
                    (PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasGiven);
                var depthKey = (PQDepthKey)i;
                var expectedQuoteLastTradeField = new PQFieldUpdate
                    (PQFeedFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasGiven);
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsWasGivenUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

                lastTrade.IsWasGivenUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                        where update.TradingSubId == PQTradingSubFieldKeys.LastTradedBooleanFlags && update.DepthId == (PQDepthKey)i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.WasGiven          = false;
                lastTrade.IsWasGivenUpdated = false;

                var newEmpty = new PQPublishableLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer = (IPQLastPaidGivenTrade)newEmpty.OnTickLastTraded![i];
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
            if (!(emptyQuote.OnTickLastTraded?[0] is IPQLastPaidGivenTrade)) continue;
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = (IPQLastPaidGivenTrade)emptyQuote.OnTickLastTraded[i];

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
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

                const bool expectedWasPaid = true;
                lastTrade.WasPaid = expectedWasPaid;
                var pqSrcTrkQtInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                Assert.IsTrue(lastTrade.IsWasPaidUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedWasPaid, lastTrade.WasPaid);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField =
                    new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedBooleanFlags
                                    , (uint)LastTradeBooleanFlags.WasPaid);
                var depthKey = (PQDepthKey)i;
                var expectedQuoteLastTradeField = new PQFieldUpdate
                    (PQFeedFields.LastTradedTickTrades, depthKey, PQTradingSubFieldKeys.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasPaid);
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsWasPaidUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

                lastTrade.IsWasPaidUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                        where update.TradingSubId == PQTradingSubFieldKeys.LastTradedBooleanFlags && update.DepthId == (PQDepthKey)i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.WasPaid          = false;
                lastTrade.IsWasPaidUpdated = false;

                var newEmpty = new PQPublishableLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer = (IPQLastPaidGivenTrade)newEmpty.OnTickLastTraded![i];
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
                    var ordersCountLayer = (IPQOrdersCountPriceVolumeLayer)bidBook[i];
                    PQOrdersCountPriceVolumeLayerTests.AssertOrdersCountFieldUpdatesReturnAsExpected(ordersCountLayer, i, bidBook, orderBook
                   , emptyQuote);
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersCountLayer = (IPQOrdersCountPriceVolumeLayer)askBook[i];
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
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderIdFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, bidBook
                                                                                            , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderIdFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, askBook
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
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderGenesisFlagsFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderGenesisFlagsFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, askBook
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
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderCreatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderCreatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
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
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderUpdatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderUpdatedTimeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
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
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, bidBook
                                                                                                , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i, askBook
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
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderRemainingVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
                       , bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j     = 4;
                        var anonOrderInfo = ordersLayer[j];
                        PQAnonymousOrderTests.AssertOrdersOrderRemainingVolumeFieldUpdatesReturnAsExpected(anonOrderInfo, j, ordersLayer, i
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
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQExternalCounterPartyOrder)ordersLayer[j]!;
                        PQExternalCounterPartyOrderTests.AssertOrdersCounterPartyNameFieldUpdatesReturnAsExpected
                            (cpOrderInfo, j, ordersLayer, i, bidBook, orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQExternalCounterPartyOrder)ordersLayer[j]!;
                        PQExternalCounterPartyOrderTests.AssertOrdersCounterPartyNameFieldUpdatesReturnAsExpected
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
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)bidBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQExternalCounterPartyOrder)ordersLayer[j]!;
                        PQExternalCounterPartyOrderTests.AssertOrdersTraderNameFieldUpdatesReturnAsExpected(cpOrderInfo, j, ordersLayer, i, bidBook
                       , orderBook, emptyQuote);
                    }
                }
                var askBook = orderBook.AskSide;

                for (int i = 0; i < askBook.Capacity; i++)
                {
                    var ordersLayer = (IPQOrdersPriceVolumeLayer)askBook[i];
                    for (ushort j = 0; j < 7; j++)
                    {
                        if (j == 2) j   = 4;
                        var cpOrderInfo = (IPQExternalCounterPartyOrder)ordersLayer[j]!;
                        PQExternalCounterPartyOrderTests.AssertOrdersTraderNameFieldUpdatesReturnAsExpected(cpOrderInfo, j, ordersLayer, i, askBook
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
            if (populatedQuote.OnTickLastTraded == null) continue;
            foreach (var lastTrade in populatedQuote.OnTickLastTraded) Assert.IsTrue(lastTrade.HasUpdates);

            populatedQuote.HasUpdates = false;

            foreach (var lastTrade in populatedQuote.OnTickLastTraded) Assert.IsFalse(lastTrade.HasUpdates);
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

            popQuote.ResetWithTracking();

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
                    (new DateTime(2017, 11, 04, 12, 33, 1), PQMessageFlags.Update, precisionSettings).ToList();
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
                    (new DateTime(2017, 11, 04, 12, 33, 1), PQMessageFlags.Snapshot, precisionSettings).ToList();
            AssertContainsAllLevel3Fields(precisionSettings, pqFieldUpdates, populatedL3Quote);
        }
    }

    [TestMethod]
    public void PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            populatedL3Quote.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.IsAdapterReplay;
            populatedL3Quote.HasUpdates                   = false;
            var pqFieldUpdates =
                populatedL3Quote.GetDeltaUpdateFields
                    (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
            var pqStringUpdates =
                populatedL3Quote.GetStringUpdates
                    (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
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
                   , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
            var pqStringUpdates =
                populatedL3Quote.GetStringUpdates
                    (new DateTime(2017, 11, 04, 13, 33, 3)
                   , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL3Quote.SourceTickerInfo!)
                {
                    NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.SourceTickerNames)
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
            Assert.AreEqual(FeedConnectivityStatusFlags.None, emptyQuote.FeedMarketConnectivityStatus);
            Assert.AreEqual(0m, emptyQuote.SingleTickValue);
            Assert.AreEqual(FeedSyncStatus.Good, emptyQuote.FeedSyncStatus);
            Assert.AreEqual(default, emptyQuote.SourceBidTime);
            Assert.AreEqual(default, emptyQuote.SourceAskTime);
            Assert.AreEqual(default, emptyQuote.AdapterReceivedTime);
            Assert.AreEqual(default, emptyQuote.AdapterSentTime);
            Assert.AreEqual(default, emptyQuote.ClientReceivedTime);
            Assert.AreEqual(default, emptyQuote.InboundProcessedTime);
            Assert.AreEqual(default, emptyQuote.SubscriberDispatchedTime);
            Assert.AreEqual(default, emptyQuote.InboundSocketReceivingTime);
            Assert.AreEqual(0m, emptyQuote.BidPriceTop);
            Assert.AreEqual(0m, emptyQuote.AskPriceTop);
            Assert.IsTrue(emptyQuote.Executable);
            Assert.IsFalse(emptyQuote.IsBatchIdUpdated);
            Assert.IsFalse(emptyQuote.IsSourceQuoteReferenceUpdated);
            Assert.IsFalse(emptyQuote.IsValueDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceTimeSub2MinUpdated);
            Assert.IsFalse(emptyQuote.IsFeedConnectivityStatusUpdated);
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
            if (emptyQuote.OnTickLastTraded == null) continue;
            foreach (var lastTraded in emptyQuote.OnTickLastTraded) AssertAreDefaultValues(lastTraded);
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
    public void NoOnTickLastTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = noOnTickLastTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void SimpleOnTickLastTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = simpleOnTickLastTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void PaidGivenOnTickLastTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = paidGivenVolumeOnTickLastTradedFullyPopulatedQuote;
        var toJson = JsonSerializer.Serialize(q, so);
        Console.Out.WriteLine(toJson);
    }

    [TestMethod]
    public void FullSupportOnTickLastTradedFullyPopulatedQuote_JsonSerialize_ReturnsExpectedJsonString()
    {
        var so = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var q      = fullSupportOnTickLastTradedFullyPopulatedQuote;
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

        Assert.IsTrue((original.OnTickLastTraded != null && changingLevel3Quote.OnTickLastTraded != null) ||
                      (original.OnTickLastTraded == null && changingLevel3Quote.OnTickLastTraded == null));
        if (original.OnTickLastTraded != null)
            PQOnTickLastTradedTests.AssertAreEquivalentMeetsExpectedExactComparisonType
                (exactComparison, (PQOnTickLastTraded)original.OnTickLastTraded, (PQOnTickLastTraded)changingLevel3Quote.OnTickLastTraded!,
                 original, changingLevel3Quote);
    }

    public static void AssertContainsAllLevel3Fields
    (IPQPriceVolumePublicationPrecisionSettings precisionSettings, IList<PQFieldUpdate> checkFieldUpdates,
        PQPublishableLevel3Quote l3Q
      , PQQuoteBooleanValues expectedQuoteBooleanFlags = PQQuoteBooleanValuesExtensions.LivePricingFieldsSetNoReplayOrSnapshots)
    {
        PQLevel2QuoteTests.AssertContainsAllLevel2Fields(precisionSettings, checkFieldUpdates, l3Q, expectedQuoteBooleanFlags);

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteBatchId, l3Q.BatchId),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteBatchId));
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteSourceQuoteRef, l3Q.SourceQuoteReference),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteSourceQuoteRef));
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteValueDate, l3Q.ValueDate.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteValueDate));
        if (l3Q.OnTickLastTraded == null) return;
        var pqSrcTkrInfo = l3Q.SourceTickerInfo!;

        PQOnTickLastTradedTests.AssertContainsAllLevelOnTickLastTradedFields
            (checkFieldUpdates, l3Q.OnTickLastTraded,
             pqSrcTkrInfo.PriceScalingPrecision, pqSrcTkrInfo.VolumeScalingPrecision);
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

        if (pvl is IPQLastExternalCounterPartyTrade traderPaidGivenTrade)
        {
            Assert.IsNull(traderPaidGivenTrade.ExternalTraderName);
            Assert.IsFalse(traderPaidGivenTrade.IsExternalTraderNameUpdated);
        }
    }

    private void AssertLastTradeTypeIsExpected(Type expectedType, params PQPublishableLevel3Quote[] quotesToCheck)
    {
        foreach (var level3Quote in quotesToCheck)
            for (var i = 0; i < QuoteSequencedTestDataBuilder.GeneratedNumberOfLastTrades; i++)
                Assert.AreEqual(expectedType, level3Quote.OnTickLastTraded![i].GetType());
    }
}
