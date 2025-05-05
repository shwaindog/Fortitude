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
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes;

[TestClass]
public class PQLevel3QuoteTests
{
    private IList<PQLevel3Quote> allEmptyQuotes          = null!;
    private IList<PQLevel3Quote> allFullyPopulatedQuotes = null!;

    private PQLevel3Quote noRecentlyTradedEmptyQuote          = null!;
    private PQLevel3Quote noRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo noRecentlyTradedSrcTkrInfo = null!;

    private PQLevel3Quote paidGivenVolumeRecentlyTradedEmptyQuote          = null!;
    private PQLevel3Quote paidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo             paidGivenVolumeRecentlyTradedSrcTkrInfo = null!;
    private QuoteSequencedTestDataBuilder quoteSequencedTestDataBuilder           = null!;

    private PQLevel3Quote simpleRecentlyTradedEmptyQuote          = null!;
    private PQLevel3Quote simpleRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo simpleRecentlyTradedSrcTkrInfo = null!;

    private DateTime      testDateTime;
    private PQLevel3Quote traderPaidGivenVolumeRecentlyTradedEmptyQuote          = null!;
    private PQLevel3Quote traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote = null!;

    private ISourceTickerInfo traderPaidGivenVolumeRecentlyTradedSrcTkrInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        quoteSequencedTestDataBuilder = new QuoteSequencedTestDataBuilder();

        noRecentlyTradedSrcTkrInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
                , roundingPrecision:.000001m, layerFlags: LayerFlagsExtensions.FullSupportLayerFlags
               , lastTradedFlags: LastTradedFlags.None);
        simpleRecentlyTradedSrcTkrInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , roundingPrecision:.000001m, layerFlags: LayerFlagsExtensions.AdditionalAnonymousOrderFlags | LayerFlagsExtensions.PriceVolumeLayerFlags
               , lastTradedFlags: LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice);
        paidGivenVolumeRecentlyTradedSrcTkrInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , roundingPrecision:.000001m, layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize 
                                                       | LayerFlags.ValueDate | LayerFlags.OrdersCount | LayerFlags.SourceQuoteReference
               , lastTradedFlags: LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                                  LastTradedFlags.LastTradedVolume);
        traderPaidGivenVolumeRecentlyTradedSrcTkrInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , roundingPrecision:.000001m, layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize 
                                                       | LayerFlags.ValueDate | LayerFlags.OrdersCount | LayerFlags.SourceQuoteReference
               , lastTradedFlags: LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice | LastTradedFlags.TraderName);
        noRecentlyTradedEmptyQuote          = new PQLevel3Quote(new PQSourceTickerInfo(noRecentlyTradedSrcTkrInfo)) { HasUpdates = false };
        noRecentlyTradedFullyPopulatedQuote = new PQLevel3Quote(noRecentlyTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(noRecentlyTradedFullyPopulatedQuote, 9);
        simpleRecentlyTradedEmptyQuote = new PQLevel3Quote(new PQSourceTickerInfo(simpleRecentlyTradedSrcTkrInfo))
        {
            HasUpdates = false
        };
        simpleRecentlyTradedFullyPopulatedQuote = new PQLevel3Quote(simpleRecentlyTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(simpleRecentlyTradedFullyPopulatedQuote, 10);
        paidGivenVolumeRecentlyTradedEmptyQuote = new PQLevel3Quote(new PQSourceTickerInfo(paidGivenVolumeRecentlyTradedSrcTkrInfo))
        {
            HasUpdates = false
        };
        paidGivenVolumeRecentlyTradedFullyPopulatedQuote =
            new PQLevel3Quote(paidGivenVolumeRecentlyTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder.InitializeQuote(paidGivenVolumeRecentlyTradedFullyPopulatedQuote, 10);
        traderPaidGivenVolumeRecentlyTradedEmptyQuote =
            new PQLevel3Quote(new PQSourceTickerInfo(traderPaidGivenVolumeRecentlyTradedSrcTkrInfo)) { HasUpdates = false };
        traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote =
            new PQLevel3Quote(traderPaidGivenVolumeRecentlyTradedSrcTkrInfo);
        quoteSequencedTestDataBuilder
            .InitializeQuote(traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote, 10);


        allFullyPopulatedQuotes = new List<PQLevel3Quote>
        {
            noRecentlyTradedFullyPopulatedQuote, simpleRecentlyTradedFullyPopulatedQuote
          , paidGivenVolumeRecentlyTradedFullyPopulatedQuote, traderPaidGivenVolumeRecentlyTradedFullyPopulatedQuote
        };
        allEmptyQuotes = new List<PQLevel3Quote>
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

        var newEmpty = new PQLevel3Quote(noRecentlyTradedSrcTkrInfo);
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

        var newEmpty = new PQLevel3Quote(noRecentlyTradedSrcTkrInfo);
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

        var newEmpty = new PQLevel3Quote(noRecentlyTradedSrcTkrInfo);
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
            for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
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
                    = new PQFieldUpdate(PQQuoteFields.LastTradedAtPrice, PQScaling.Scale(expectedPrice, priceScale), priceScale);
                var expectedQuoteLastTradeField =
                    new PQFieldUpdate
                        (PQQuoteFields.LastTradedAtPrice, (PQDepthKey)i, expectedLastTradeField.Payload, expectedLastTradeField.Flag);
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
                        where update.Id == PQQuoteFields.LastTradedAtPrice && update.DepthId == (PQDepthKey)i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.TradePrice          = 0m;
                lastTrade.IsTradePriceUpdated = false;

                var newEmpty = new PQLevel3Quote(emptyQuote.SourceTickerInfo!);
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
            for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
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
                Assert.AreEqual(DateTimeConstants.UnixEpoch, lastTrade.TradeTime);
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
                    new PQFieldUpdate(PQQuoteFields.LastTradedTradeTimeDate, hoursFromEpoch);
                var expectedLastTradeSubHoursField =
                    new PQFieldUpdate(PQQuoteFields.LastTradedTradeSub2MinTime, subHourBase, flag);
                Assert.AreEqual(expectedLastTradeHoursField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedLastTradeSubHoursField, lastTradeUpdates[1]);
                var expectedQuoteLastTradeTimeHoursField =
                    new PQFieldUpdate(PQQuoteFields.LastTradedTradeTimeDate, (PQDepthKey)i, hoursFromEpoch);
                var expectedQuoteLastTradeTimeSubHoursField =
                    new PQFieldUpdate(PQQuoteFields.LastTradedTradeSub2MinTime, (PQDepthKey)i, subHourBase, flag);
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
                        where (update.Id == PQQuoteFields.LastTradedTradeTimeDate && update.DepthId == (PQDepthKey)i) ||
                              (update.Id == PQQuoteFields.LastTradedTradeSub2MinTime && update.DepthId == (PQDepthKey)i)
                        select update).ToList();
                Assert.AreEqual(2, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeTimeHoursField, quoteUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeTimeSubHoursField, quoteUpdates[1]);
                lastTrade.TradeTime                 = DateTimeConstants.UnixEpoch;
                lastTrade.IsTradeTimeDateUpdated    = false;
                lastTrade.IsTradeTimeSub2MinUpdated = false;

                var newEmpty = new PQLevel3Quote(emptyQuote.SourceTickerInfo!);
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
            for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
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
                    (PQQuoteFields.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasGiven);
                var expectedQuoteLastTradeField = new PQFieldUpdate
                    (PQQuoteFields.LastTradedBooleanFlags, (PQDepthKey)i, (uint)LastTradeBooleanFlags.WasGiven);
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
                        where update.Id == PQQuoteFields.LastTradedBooleanFlags && update.DepthId == (PQDepthKey)i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.WasGiven          = false;
                lastTrade.IsWasGivenUpdated = false;

                var newEmpty = new PQLevel3Quote(emptyQuote.SourceTickerInfo!);
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
            for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
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
                    new PQFieldUpdate(PQQuoteFields.LastTradedBooleanFlags, (uint)LastTradeBooleanFlags.WasPaid);
                var expectedQuoteLastTradeField =
                    new PQFieldUpdate(PQQuoteFields.LastTradedBooleanFlags, (PQDepthKey)i,
                                      (uint)LastTradeBooleanFlags.WasPaid);
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
                        where update.Id == PQQuoteFields.LastTradedBooleanFlags && update.DepthId == (PQDepthKey)i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.WasPaid          = false;
                lastTrade.IsWasPaidUpdated = false;

                var newEmpty = new PQLevel3Quote(emptyQuote.SourceTickerInfo!);
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
        foreach (var ordersCountPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                              .OfType<IPQOrdersCountPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, ordersCountPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                             .Select((pvl, i) => new { i, pvl })
                                             .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersCountPriceVolumeLayer))
                                             .Select(indexPvl => indexPvl.i).FirstOrDefault();

            testDateTime = testDateTime.AddHours(1).AddMinutes(1);

            Assert.IsFalse(ordersCountPriceVolumeLayer.IsOrdersCountUpdated);
            Assert.IsFalse(ordersCountPriceVolumeLayer.HasUpdates);

            Assert.IsFalse(ordersCountPriceVolumeLayer.IsOrdersCountUpdated);
            Assert.IsFalse(ordersCountPriceVolumeLayer.HasUpdates);
            emptyQuote.Executable = false;
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.UpdateComplete();
            emptyQuote.Executable          = true;
            emptyQuote.IsExecutableUpdated = false;
            emptyQuote.HasUpdates          = false;

            Assert.AreEqual(0, ordersCountPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
            Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

            ordersCountPriceVolumeLayer.OrdersCount = byte.MaxValue;
            Assert.IsTrue(ordersCountPriceVolumeLayer.HasUpdates);
            Assert.AreEqual(byte.MaxValue, ordersCountPriceVolumeLayer.OrdersCount);
            Assert.IsTrue(emptyQuote.HasUpdates);
            var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(3, quoteUpdates.Count);
            var layerUpdates = ordersCountPriceVolumeLayer
                               .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
            Assert.AreEqual(1, layerUpdates.Count);
            var expectedLayerField = new PQFieldUpdate(PQQuoteFields.OrdersCount, byte.MaxValue);
            var depthId            = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
            var expectedSideAdjustedLayerField =
                new PQFieldUpdate(PQQuoteFields.OrdersCount, depthId, byte.MaxValue);
            Assert.AreEqual(expectedLayerField, layerUpdates[0]);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

            ordersCountPriceVolumeLayer.HasUpdates = false;
            Assert.IsFalse(ordersCountPriceVolumeLayer.HasUpdates);
            Assert.IsTrue(emptyQuote.HasUpdates);
            emptyQuote.IsAdapterSentTimeDateUpdated    = false;
            emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(emptyQuote.HasUpdates);
            Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

            ordersCountPriceVolumeLayer.HasUpdates = true;
            quoteUpdates =
                (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                    where update.Id == PQQuoteFields.OrdersCount && update.DepthId == depthId
                    select update).ToList();
            Assert.AreEqual(1, quoteUpdates.Count);
            Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
            ordersCountPriceVolumeLayer.OrdersCount = 0;
            ordersCountPriceVolumeLayer.HasUpdates  = false;

            var diffNameIdLookupSrcTkrInfo =
                new PQSourceTickerInfo(emptyQuote.SourceTickerInfo!)
                {
                    NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand)
                };
            var newEmpty = new PQLevel2Quote(diffNameIdLookupSrcTkrInfo);
            newEmpty.UpdateField(quoteUpdates[0]);
            var foundLayer =
                (IPQOrdersCountPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
            Assert.AreEqual(byte.MaxValue, foundLayer.OrdersCount);
            Assert.IsTrue(newEmpty.HasUpdates);
            Assert.IsTrue(foundLayer.HasUpdates);
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderIdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var ordersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                         .OfType<IPQOrdersPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, ordersPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                             .Select((pvl, i) => new { i, pvl })
                                             .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersPriceVolumeLayer))
                                             .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = ordersPriceVolumeLayer[i]!;

                emptyQuote.Executable  = false;
                anonOrderLayer.OrderId = -1;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                anonOrderLayer.OrderId         = 0;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;

                Assert.IsFalse(anonOrderLayer.IsOrderIdUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0, anonOrderLayer.OrderId);
                Assert.AreEqual(0,
                                ordersPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedOrderId = 254682;
                anonOrderLayer.OrderId = expectedOrderId;
                Assert.IsTrue(anonOrderLayer.IsOrderIdUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedOrderId, anonOrderLayer.OrderId);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = ordersPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var orderIndex = (ushort)i;
                var expectedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderId, orderIndex, (uint)expectedOrderId);
                var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideAdjustedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderId, depthId, orderIndex, expectedLayerField.Payload, expectedLayerField.Flag);
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

                anonOrderLayer.IsOrderIdUpdated = false;
                Assert.IsFalse(ordersPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsOrderIdUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQQuoteFields.OrderId && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                anonOrderLayer.OrderId          = 0;
                anonOrderLayer.IsOrderIdUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer =
                    (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundAnonOrderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedOrderId, foundAnonOrderInfo.OrderId);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.IsOrderIdUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var ordersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                         .OfType<IPQOrdersPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, ordersPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                             .Select((pvl, i) => new { i, pvl })
                                             .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersPriceVolumeLayer))
                                             .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = ordersPriceVolumeLayer[i]!;

                emptyQuote.Executable  = false;
                anonOrderLayer.OrderId = -1;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                anonOrderLayer.OrderId         = 0;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;


                Assert.IsFalse(anonOrderLayer.IsOrderFlagsUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(LayerOrderFlags.None, anonOrderLayer.OrderFlags);
                Assert.AreEqual(0,
                                ordersPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedOrderFlags = LayerOrderFlags.IsInternallyCreatedOrder | LayerOrderFlags.IsSyntheticTrackingOrder;
                anonOrderLayer.OrderFlags = expectedOrderFlags;
                Assert.IsTrue(anonOrderLayer.IsOrderFlagsUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedOrderFlags, anonOrderLayer.OrderFlags);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = ordersPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var orderIndex = (ushort)i;
                var expectedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderFlags, orderIndex, (uint)expectedOrderFlags);
                var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideAdjustedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderFlags, depthId, 0, orderIndex, expectedLayerField.Payload, expectedLayerField.Flag);
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

                anonOrderLayer.IsOrderFlagsUpdated = false;
                Assert.IsFalse(ordersPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsOrderFlagsUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQQuoteFields.OrderFlags && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                anonOrderLayer.OrderFlags          = LayerOrderFlags.None;
                anonOrderLayer.IsOrderFlagsUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer =
                    (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundAnonOrderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedOrderFlags, foundAnonOrderInfo.OrderFlags);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.IsOrderFlagsUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderCreatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var ordersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                         .OfType<IPQOrdersPriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers
                                  .Any(pvl => ReferenceEquals(pvl, ordersPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook)
                .AllLayers
                .Select((pvl, i) => new { i, pvl })
                .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersPriceVolumeLayer))
                .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = ordersPriceVolumeLayer[i]!;

                emptyQuote.Executable  = false;
                anonOrderLayer.OrderId = -1;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                anonOrderLayer.OrderId         = 0;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;

                Assert.IsFalse(anonOrderLayer.IsCreatedTimeDateUpdated);
                Assert.IsFalse(anonOrderLayer.IsCreatedTimeSub2MinUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(DateTime.MinValue, anonOrderLayer.CreatedTime);
                Assert.AreEqual(0, anonOrderLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedCreatedTime = new DateTime(2017, 12, 03, 19, 27, 53);
                anonOrderLayer.CreatedTime = expectedCreatedTime;
                Assert.IsTrue(anonOrderLayer.IsCreatedTimeDateUpdated);
                Assert.IsTrue(anonOrderLayer.IsCreatedTimeSub2MinUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedCreatedTime, anonOrderLayer.CreatedTime);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(4, quoteUpdates.Count);
                var layerUpdates = ordersPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(2, layerUpdates.Count);
                var orderIndex                = (ushort)i;
                var hoursSinceUnixEpoch       = expectedCreatedTime.Get2MinIntervalsFromUnixEpoch();
                var expectedDateLayerField    = new PQFieldUpdate(PQQuoteFields.OrderCreatedDate, orderIndex, hoursSinceUnixEpoch);
                var extended                  = expectedCreatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var sub2MinBottom);
                var expectedSub2MinLayerField = new PQFieldUpdate(PQQuoteFields.OrderCreatedSub2MinTime, orderIndex, sub2MinBottom, extended);
                var depthId                   = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedDateField =
                    new PQFieldUpdate(PQQuoteFields.OrderCreatedDate, depthId, orderIndex, expectedDateLayerField.Payload);
                var expectedSub2MinField =
                    new PQFieldUpdate(PQQuoteFields.OrderCreatedSub2MinTime, depthId, orderIndex
                                    , expectedSub2MinLayerField.Payload , expectedSub2MinLayerField.Flag);
                Assert.AreEqual(expectedDateLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSub2MinLayerField, layerUpdates[1]);
                Assert.AreEqual(expectedDateField, quoteUpdates[2]);
                Assert.AreEqual(expectedSub2MinField, quoteUpdates[3]);

                anonOrderLayer.IsCreatedTimeDateUpdated    = false;
                anonOrderLayer.IsCreatedTimeSub2MinUpdated = false;
                Assert.IsFalse(ordersPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsCreatedTimeDateUpdated    = true;
                anonOrderLayer.IsCreatedTimeSub2MinUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where (update.Id == PQQuoteFields.OrderCreatedDate && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex)
                           || (update.Id == PQQuoteFields.OrderCreatedSub2MinTime && update.DepthId == depthId &&
                               update.AuxiliaryPayload == orderIndex)
                        select update).ToList();
                Assert.AreEqual(2, quoteUpdates.Count);
                Assert.AreEqual(expectedDateField, quoteUpdates[0]);
                Assert.AreEqual(expectedSub2MinField, quoteUpdates[1]);
                emptyQuote.AdapterSentTime = DateTime.UnixEpoch;
                anonOrderLayer.CreatedTime = DateTime.UnixEpoch;

                emptyQuote.HasUpdates = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                newEmpty.UpdateField(quoteUpdates[1]);
                var foundLayer = (IPQOrdersPriceVolumeLayer)
                    (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundAnonOrderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedCreatedTime, foundAnonOrderInfo.CreatedTime);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeDateUpdated);
                Assert.IsTrue(foundAnonOrderInfo.IsCreatedTimeSub2MinUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderUpdatedDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var ordersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                         .OfType<IPQOrdersPriceVolumeLayer>())
        {
            testDateTime = testDateTime.AddHours(1).AddMinutes(1);
            var isBid = emptyQuote.BidBook.AllLayers
                                  .Any(pvl => ReferenceEquals(pvl, ordersPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                             .Select((pvl, i) => new { i, pvl })
                                             .Where(indexPvl => ReferenceEquals(indexPvl.pvl, ordersPriceVolumeLayer))
                                             .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = ordersPriceVolumeLayer[i]!;

                emptyQuote.Executable  = false;
                anonOrderLayer.OrderId = -1;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                anonOrderLayer.OrderId         = 0;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;

                Assert.IsFalse(anonOrderLayer.IsUpdatedTimeDateUpdated);
                Assert.IsFalse(anonOrderLayer.IsUpdatedTimeSub2MinUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(DateTime.MinValue, anonOrderLayer.UpdatedTime);
                Assert.AreEqual(0, anonOrderLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedUpdatedTime = new DateTime(2017, 12, 03, 19, 41, 22); //only to the nearest hour
                anonOrderLayer.UpdatedTime = expectedUpdatedTime;
                Assert.IsTrue(anonOrderLayer.IsUpdatedTimeDateUpdated);
                Assert.IsTrue(anonOrderLayer.IsUpdatedTimeSub2MinUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedUpdatedTime, anonOrderLayer.UpdatedTime);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(4, quoteUpdates.Count);
                var layerUpdates = ordersPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(2, layerUpdates.Count);
                var orderIndex                = (ushort)i;
                var hoursSinceUnixEpoch       = expectedUpdatedTime.Get2MinIntervalsFromUnixEpoch();
                var expectedDateLayerField    = new PQFieldUpdate(PQQuoteFields.OrderUpdatedDate, orderIndex, hoursSinceUnixEpoch);
                var extended                  = expectedUpdatedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBottom);
                var expectedSub2MinLayerField = new PQFieldUpdate(PQQuoteFields.OrderUpdatedSub2MinTime, orderIndex, subHourBottom, extended);
                var depthId                   = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideDateLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderUpdatedDate, depthId,  orderIndex, expectedDateLayerField.Payload);
                var expectedSideSub2MinLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderUpdatedSub2MinTime, depthId, orderIndex, expectedSub2MinLayerField.Payload, expectedSub2MinLayerField.Flag);
                Assert.AreEqual(expectedDateLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSub2MinLayerField, layerUpdates[1]);
                Assert.AreEqual(expectedSideDateLayerField, quoteUpdates[2]);
                Assert.AreEqual(expectedSideSub2MinLayerField, quoteUpdates[3]);

                anonOrderLayer.IsUpdatedTimeDateUpdated    = false;
                anonOrderLayer.IsUpdatedTimeSub2MinUpdated = false;
                Assert.IsFalse(ordersPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsUpdatedTimeDateUpdated    = true;
                anonOrderLayer.IsUpdatedTimeSub2MinUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where (update.Id == PQQuoteFields.OrderUpdatedDate && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex)
                           || (update.Id == PQQuoteFields.OrderUpdatedSub2MinTime && update.DepthId == depthId &&
                               update.AuxiliaryPayload == orderIndex)
                        select update).ToList();
                Assert.AreEqual(2, quoteUpdates.Count);
                Assert.AreEqual(expectedSideDateLayerField, quoteUpdates[0]);
                Assert.AreEqual(expectedSideSub2MinLayerField, quoteUpdates[1]);
                emptyQuote.AdapterSentTime                 = DateTime.UnixEpoch;
                anonOrderLayer.UpdatedTime                 = DateTime.UnixEpoch;

                emptyQuote.HasUpdates = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                newEmpty.UpdateField(quoteUpdates[1]);
                var foundLayer = (IPQOrdersPriceVolumeLayer)
                    (isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundAnonOrderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedUpdatedTime, foundAnonOrderInfo.UpdatedTime);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeDateUpdated);
                Assert.IsTrue(foundAnonOrderInfo.IsUpdatedTimeSub2MinUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var orderPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                        .OfType<IPQOrdersPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, orderPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                             .Select((pvl, i) => new { i, pvl })
                                             .Where(indexPvl => ReferenceEquals(indexPvl.pvl, orderPriceVolumeLayer))
                                             .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = orderPriceVolumeLayer[i]!;

                emptyQuote.Executable  = false;
                anonOrderLayer.OrderId = -1;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                anonOrderLayer.OrderId         = 0;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;

                Assert.IsFalse(anonOrderLayer.IsOrderVolumeUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0m, anonOrderLayer.OrderVolume);
                Assert.AreEqual(0,
                                orderPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedOrderVolume = 254682m;
                anonOrderLayer.OrderVolume = expectedOrderVolume;
                Assert.IsTrue(anonOrderLayer.IsOrderVolumeUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedOrderVolume, anonOrderLayer.OrderVolume);
                var precisionSettings = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                var quoteUpdates      = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = orderPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var orderIndex             = (ushort)i;
                var volumeScalingPrecision = precisionSettings.VolumeScalingPrecision;
                var expectedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderVolume, orderIndex, expectedOrderVolume, volumeScalingPrecision);
                var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideAdjustedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderVolume, depthId
                                     , expectedLayerField.AuxiliaryPayload, expectedLayerField.Payload, expectedLayerField.Flag);
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

                anonOrderLayer.IsOrderVolumeUpdated = false;
                Assert.IsFalse(orderPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsOrderVolumeUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQQuoteFields.OrderVolume && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                anonOrderLayer.OrderVolume          = 0m;
                anonOrderLayer.IsOrderVolumeUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer =
                    (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundTraderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedOrderVolume, foundTraderInfo.OrderVolume);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundTraderInfo.HasUpdates);
                Assert.IsTrue(foundTraderInfo.IsOrderVolumeUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderRemainingVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        foreach (var orderPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                        .OfType<IPQOrdersPriceVolumeLayer>())
        {
            var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, orderPriceVolumeLayer));
            var indexFromTop =
                (isBid
                    ? emptyQuote.BidBook
                    : emptyQuote.AskBook).AllLayers
                                             .Select((pvl, i) => new { i, pvl })
                                             .Where(indexPvl => ReferenceEquals(indexPvl.pvl, orderPriceVolumeLayer))
                                             .Select(indexPvl => indexPvl.i).FirstOrDefault();

            for (var i = 0; i < 256; i++)
            {
                if (i == 5) i = 254;
                testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                var anonOrderLayer = orderPriceVolumeLayer[i]!;

                emptyQuote.Executable  = false;
                anonOrderLayer.OrderId = -1;
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.UpdateComplete();
                emptyQuote.Executable          = true;
                anonOrderLayer.OrderId         = 0;
                emptyQuote.IsExecutableUpdated = false;
                emptyQuote.HasUpdates          = false;

                Assert.IsFalse(anonOrderLayer.IsOrderRemainingVolumeUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0m, anonOrderLayer.OrderRemainingVolume);
                Assert.AreEqual(0,
                                orderPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedOrderRemainingVolume = 254682m;
                anonOrderLayer.OrderRemainingVolume = expectedOrderRemainingVolume;
                Assert.IsTrue(anonOrderLayer.IsOrderRemainingVolumeUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedOrderRemainingVolume, anonOrderLayer.OrderRemainingVolume);
                var precisionSettings = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                var quoteUpdates      = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var layerUpdates = orderPriceVolumeLayer
                                   .GetDeltaUpdateFields(testDateTime, StorageFlags.Update, precisionSettings).ToList();
                Assert.AreEqual(1, layerUpdates.Count);
                var orderIndex             = (ushort)i;
                var volumeScalingPrecision = precisionSettings.VolumeScalingPrecision;
                var expectedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderRemainingVolume, orderIndex, expectedOrderRemainingVolume, volumeScalingPrecision);
                var depthId = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                var expectedSideAdjustedLayerField =
                    new PQFieldUpdate(PQQuoteFields.OrderRemainingVolume, depthId, expectedLayerField.AuxiliaryPayload
                                    , expectedLayerField.Payload , expectedLayerField.Flag);
                Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[2]);

                anonOrderLayer.IsOrderRemainingVolumeUpdated = false;
                Assert.IsFalse(orderPriceVolumeLayer.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                anonOrderLayer.IsOrderRemainingVolumeUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQQuoteFields.OrderRemainingVolume && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                anonOrderLayer.OrderRemainingVolume          = 0m;
                anonOrderLayer.IsOrderRemainingVolumeUpdated = false;

                var newEmpty = new PQLevel2Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer =
                    (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                var foundTraderInfo = foundLayer[i]!;
                Assert.AreEqual(expectedOrderRemainingVolume, foundTraderInfo.OrderRemainingVolume);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundTraderInfo.HasUpdates);
                Assert.IsTrue(foundTraderInfo.IsOrderRemainingVolumeUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LayerOrderTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            var nameCounter = 0;
            foreach (var cpOrdersPriceVolumeLayer in emptyQuote.AskBook.AllLayers.Concat(emptyQuote.BidBook)
                                                               .OfType<IPQOrdersPriceVolumeLayer>()
                                                               .Where(opvl => opvl.LayerType.SupportsOrdersFullPriceVolume()))
            {
                var isBid = emptyQuote.BidBook.AllLayers.Any(pvl => ReferenceEquals(pvl, cpOrdersPriceVolumeLayer));
                var indexFromTop =
                    (isBid
                        ? emptyQuote.BidBook
                        : emptyQuote.AskBook).AllLayers
                                                 .Select((pvl, i) => new { i, pvl })
                                                 .Where(indexPvl => ReferenceEquals(indexPvl.pvl, cpOrdersPriceVolumeLayer))
                                                 .Select(indexPvl => indexPvl.i).FirstOrDefault();

                for (var i = 0; i < 256; i++)
                {
                    nameCounter++;
                    if (i == 5) i = 254;
                    testDateTime = testDateTime.AddHours(1).AddMinutes(1);
                    var cpOrderLayerInfo = (IPQCounterPartyOrderLayerInfo)cpOrdersPriceVolumeLayer[i]!;

                    emptyQuote.Executable    = false;
                    cpOrderLayerInfo.OrderId = -1;
                    Assert.IsTrue(emptyQuote.HasUpdates);
                    emptyQuote.UpdateComplete();
                    emptyQuote.Executable          = true;
                    cpOrderLayerInfo.OrderId       = 0;
                    emptyQuote.IsExecutableUpdated = false;
                    emptyQuote.HasUpdates          = false;


                    Assert.IsFalse(cpOrderLayerInfo.IsTraderNameUpdated);
                    Assert.IsFalse(emptyQuote.HasUpdates);
                    Assert.AreEqual(null, cpOrderLayerInfo.TraderName);
                    Assert.AreEqual(0, cpOrdersPriceVolumeLayer.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                    Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                    var expectedTraderName = "NewChangedTraderName" + nameCounter;
                    cpOrderLayerInfo.TraderName = expectedTraderName;
                    Assert.IsTrue(cpOrderLayerInfo.IsTraderNameUpdated);
                    Assert.IsTrue(emptyQuote.HasUpdates);
                    Assert.AreEqual(expectedTraderName, cpOrderLayerInfo.TraderName);
                    var allDeltaUpdateFields = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                    var quoteUpdates =
                        allDeltaUpdateFields
                            .Where(fu => fu.Id is PQQuoteFields.OrderTraderNameId).ToList();
                    Assert.AreEqual(1, quoteUpdates.Count);
                    var layerUpdates = cpOrdersPriceVolumeLayer
                                       .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                    Assert.AreEqual(1, layerUpdates.Count);
                    var orderIndex         = (ushort)i;
                    var dictId             = cpOrdersPriceVolumeLayer.NameIdLookup[cpOrderLayerInfo.TraderName];
                    var expectedLayerField = new PQFieldUpdate(PQQuoteFields.OrderTraderNameId, orderIndex, (uint)dictId);
                    var depthId            = (PQDepthKey)indexFromTop | (isBid ? PQDepthKey.None : PQDepthKey.AskSide);
                    var expectedSideAdjustedLayerField =
                        new PQFieldUpdate
                            (PQQuoteFields.OrderTraderNameId, depthId, orderIndex, expectedLayerField.Payload);
                    Assert.AreEqual(expectedLayerField, layerUpdates[0]);
                    Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                    var stringUpdates =
                        cpOrdersPriceVolumeLayer.GetStringUpdates(testDateTime, StorageFlags.Update).ToList();
                    var stringUpdateCommand = CrudCommand.Upsert.ToPQSubFieldId();
                    var selectedStringUpdate =
                        stringUpdates.FirstOrDefault
                            (su => su.Field.Id == PQQuoteFields.LayerNameDictionaryUpsertCommand
                                && su.Field.SubId == stringUpdateCommand && su.StringUpdate.DictionaryId == dictId);
                    Assert.IsFalse(Equals(selectedStringUpdate, new PQFieldStringUpdate()));
                    var expectedStringUpdates = new PQFieldStringUpdate
                    {
                        Field = new PQFieldUpdate(PQQuoteFields.LayerNameDictionaryUpsertCommand, stringUpdateCommand, orderIndex, 0u)
                      , StringUpdate = new PQStringUpdate
                        {
                            Command      = CrudCommand.Upsert
                          , DictionaryId = dictId
                          , Value        = expectedTraderName
                        }
                    };
                    Assert.AreEqual(expectedStringUpdates, selectedStringUpdate);

                    cpOrderLayerInfo.IsTraderNameUpdated      = false;
                    cpOrderLayerInfo.NameIdLookup!.HasUpdates = false;
                    Assert.IsFalse(cpOrdersPriceVolumeLayer.HasUpdates);
                    Assert.IsTrue(emptyQuote.HasUpdates);
                    emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                    emptyQuote.IsAdapterSentTimeSub2MinUpdated = false;
                    Assert.IsFalse(emptyQuote.HasUpdates);
                    Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                    cpOrderLayerInfo.IsTraderNameUpdated = true;
                    quoteUpdates =
                        (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                            where update.Id == PQQuoteFields.OrderTraderNameId && update.DepthId == depthId && update.AuxiliaryPayload == orderIndex
                            select update).ToList();
                    Assert.AreEqual(1, quoteUpdates.Count);
                    Assert.AreEqual(expectedSideAdjustedLayerField, quoteUpdates[0]);
                    cpOrderLayerInfo.TraderName          = null;
                    cpOrderLayerInfo.IsTraderNameUpdated = false;

                    var diffNameIdLookupSrcTkrInfo =
                        new PQSourceTickerInfo(emptyQuote.SourceTickerInfo!)
                        {
                            NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand)
                        };
                    var newEmpty = new PQLevel2Quote(diffNameIdLookupSrcTkrInfo);
                    newEmpty.UpdateField(quoteUpdates[0]);
                    var applySided = expectedStringUpdates.WithDepth(depthId);
                    newEmpty.UpdateFieldString(applySided);
                    var foundLayer =
                        (IPQOrdersPriceVolumeLayer)(isBid ? newEmpty.BidBook : newEmpty.AskBook)[indexFromTop]!;
                    var foundTraderInfo = (IPQCounterPartyOrderLayerInfo)foundLayer[i]!;
                    Assert.AreEqual(expectedTraderName, foundTraderInfo.TraderName);
                    Assert.IsTrue(newEmpty.HasUpdates);
                    Assert.IsTrue(foundTraderInfo.HasUpdates);
                    Assert.IsTrue(foundTraderInfo.IsTraderNameUpdated);
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
            var newEmpty = new PQLevel3Quote(emptyQuoteSourceTickerInfo);
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
            var newEmpty = new PQLevel3Quote(emptyQuoteSourceTickerInfo);
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
            var emptyQuote = new PQLevel3Quote(emptyQuoteSourceTickerInfo);
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
            var nonPQLevel3Quote = new Level3PriceQuote(populatedL3Quote);
            var emptyQuoteSourceTickerInfo
                = new PQSourceTickerInfo(populatedL3Quote.SourceTickerInfo!);
            var newEmpty = new PQLevel3Quote(emptyQuoteSourceTickerInfo);
            newEmpty.CopyFrom(nonPQLevel3Quote);
            Assert.IsTrue(populatedL3Quote.AreEquivalent(newEmpty));
        }
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        foreach (var populatedL3Quote in allFullyPopulatedQuotes)
        {
            var clonedQuote = ((ICloneable<ITickInstant>)populatedL3Quote).Clone();
            Assert.AreNotSame(clonedQuote, populatedL3Quote);
            Assert.AreEqual
                (populatedL3Quote, clonedQuote
               , "clonedQuote differences are \n '" + clonedQuote.DiffQuotes(populatedL3Quote) + "'");

            var cloned2 = (PQLevel3Quote)((ICloneable)populatedL3Quote).Clone();
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
            var fullyPopulatedClone = (PQLevel3Quote)((ICloneable)populatedL3Quote).Clone();
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
            Assert.AreEqual(populatedL3Quote, ((ICloneable<ITickInstant>)populatedL3Quote).Clone());
            Assert.AreEqual(populatedL3Quote, ((ICloneable<ILevel1Quote>)populatedL3Quote).Clone());
            Assert.AreEqual(populatedL3Quote, ((ICloneable<ILevel2Quote>)populatedL3Quote).Clone());
            Assert.AreEqual(populatedL3Quote, ((ICloneable<ILevel3Quote>)populatedL3Quote).Clone());
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
        (bool exactComparison, PQLevel3Quote original, PQLevel3Quote changingLevel3Quote)
    {
        PQLevel1QuoteTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison, original, changingLevel3Quote);

        if (original.GetType() == typeof(PQLevel3Quote))
            Assert.AreEqual(!exactComparison,
                            changingLevel3Quote.AreEquivalent(new Level3PriceQuote(original), exactComparison));

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
        PQLevel3Quote l3Q, PQBooleanValues expectedBooleanFlags = PQBooleanValuesExtensions.AllFields)
    {
        PQLevel2QuoteTests.AssertContainsAllLevel2Fields(precisionSettings, checkFieldUpdates, l3Q, expectedBooleanFlags);

        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.BatchId, l3Q.BatchId),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.BatchId));
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.QuoteSourceQuoteRef, l3Q.SourceQuoteReference),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.QuoteSourceQuoteRef));
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.QuoteValueDate, l3Q.ValueDate.Get2MinIntervalsFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.QuoteValueDate));
        if (l3Q.RecentlyTraded == null) return;
        var priceScale = precisionSettings.PriceScalingPrecision;
        for (var i = 0; i < l3Q.RecentlyTraded.Count; i++)
        {
            var lastTrade = l3Q.RecentlyTraded[i]!;

            Assert.AreEqual
                (new PQFieldUpdate
                     (PQQuoteFields.LastTradedAtPrice, (PQDepthKey)i, PQScaling.Scale(lastTrade.TradePrice, priceScale), priceScale)
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LastTradedAtPrice, (PQDepthKey)i, priceScale),
                 $"For asklayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            Assert.AreEqual
                (new PQFieldUpdate(PQQuoteFields.LastTradedTradeTimeDate, (PQDepthKey)i, lastTrade.TradeTime.Get2MinIntervalsFromUnixEpoch())
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LastTradedTradeTimeDate, (PQDepthKey)i),
                 $"For asklayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            var flag = lastTrade.TradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBase);
            Assert.AreEqual
                (new PQFieldUpdate(PQQuoteFields.LastTradedTradeSub2MinTime, (PQDepthKey)i, subHourBase, flag)
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LastTradedTradeSub2MinTime, (PQDepthKey)i),
                 $"For asklayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");

            if (lastTrade is IPQLastPaidGivenTrade pqPaidGivenTrade)
            {
                if (pqPaidGivenTrade.WasGiven)
                    Assert.AreEqual
                        (new PQFieldUpdate(PQQuoteFields.LastTradedBooleanFlags, (PQDepthKey)i, (uint)LastTradeBooleanFlags.WasGiven)
                       , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LastTradedBooleanFlags, (PQDepthKey)i),
                         $"For asklayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
                if (pqPaidGivenTrade.WasPaid)
                    Assert.AreEqual
                        (new PQFieldUpdate(PQQuoteFields.LastTradedBooleanFlags, (PQDepthKey)i, (uint)LastTradeBooleanFlags.WasPaid)
                       , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LastTradedBooleanFlags, (PQDepthKey)i),
                         $"For asklayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
            }

            if (lastTrade is IPQLastTraderPaidGivenTrade pqTraderPaidGivenTrade)
                Assert.AreEqual
                    (new PQFieldUpdate(PQQuoteFields.LastTradedTraderId, (PQDepthKey)i, (uint)pqTraderPaidGivenTrade.TraderId)
                   , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.LastTradedTraderId, (PQDepthKey)i),
                     $"For asklayer {lastTrade.GetType().Name} level {i} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        }
    }

    public static void AssertAreDefaultValues(IPQLastTrade pvl)
    {
        Assert.AreEqual(0m, pvl.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, pvl.TradeTime);
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

    private void AssertLastTradeTypeIsExpected(Type expectedType, params PQLevel3Quote[] quotesToCheck)
    {
        foreach (var level3Quote in quotesToCheck)
            for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
                Assert.AreEqual(expectedType, level3Quote.RecentlyTraded![i]!.GetType());
    }
}
