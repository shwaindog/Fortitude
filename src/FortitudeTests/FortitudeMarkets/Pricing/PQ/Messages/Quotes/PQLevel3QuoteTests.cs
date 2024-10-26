﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
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
               , 20, 0.000001m, 0.0001m, 3000m, 50000000m, 30000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.ValueDate |
                             LayerFlags.TraderCount | LayerFlags.SourceQuoteReference
               , lastTradedFlags: LastTradedFlags.None);
        simpleRecentlyTradedSrcTkrInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 3000m, 50000000m, 30000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.ValueDate
                           | LayerFlags.TraderCount | LayerFlags.SourceQuoteReference
               , lastTradedFlags: LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice);
        paidGivenVolumeRecentlyTradedSrcTkrInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 3000m, 50000000m, 30000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.ValueDate
                           | LayerFlags.TraderCount | LayerFlags.SourceQuoteReference
               , lastTradedFlags: LastTradedFlags.LastTradedTime | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                                  LastTradedFlags.LastTradedVolume);
        traderPaidGivenVolumeRecentlyTradedSrcTkrInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.000001m, 0.0001m, 3000m, 50000000m, 30000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.ValueDate |
                             LayerFlags.TraderCount | LayerFlags.SourceQuoteReference
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
        var expectedLayerField = new PQFieldUpdate(PQFieldKeys.BatchId, expectedBatchId);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[2]);

        noRecentlyTradedEmptyQuote.IsBatchIdUpdated = false;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeDateUpdated    = false;
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        noRecentlyTradedEmptyQuote.IsBatchIdUpdated = true;
        level3QuoteUpdates =
            (from update in noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQFieldKeys.BatchId
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
        Assert.AreEqual(0u, noRecentlyTradedEmptyQuote.SourceQuoteReference);
        Assert.AreEqual(2, noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedSourceQuoteReference = 4_111_222_333;
        noRecentlyTradedEmptyQuote.SourceQuoteReference = expectedSourceQuoteReference;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.IsSourceQuoteReferenceUpdated);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(expectedSourceQuoteReference, noRecentlyTradedEmptyQuote.SourceQuoteReference);
        var level3QuoteUpdates = noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, level3QuoteUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFieldKeys.SourceQuoteReference, expectedSourceQuoteReference);
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[2]);

        noRecentlyTradedEmptyQuote.IsSourceQuoteReferenceUpdated = false;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeDateUpdated    = false;
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        noRecentlyTradedEmptyQuote.IsSourceQuoteReferenceUpdated = true;
        level3QuoteUpdates =
            (from update in noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQFieldKeys.SourceQuoteReference
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
        Assert.AreEqual(DateTimeConstants.UnixEpoch, noRecentlyTradedEmptyQuote.ValueDate);
        Assert.AreEqual(2, noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        var expectedValueDate = new DateTime(2017, 12, 31, 0, 0, 0);
        noRecentlyTradedEmptyQuote.ValueDate = expectedValueDate;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.IsValueDateUpdated);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.AreEqual(expectedValueDate, noRecentlyTradedEmptyQuote.ValueDate);
        var level3QuoteUpdates = noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(3, level3QuoteUpdates.Count);
        var expectedLayerField = new PQFieldUpdate(PQFieldKeys.ValueDate, expectedValueDate.GetHoursFromUnixEpoch());
        Assert.AreEqual(expectedLayerField, level3QuoteUpdates[2]);

        noRecentlyTradedEmptyQuote.IsValueDateUpdated = false;
        Assert.IsTrue(noRecentlyTradedEmptyQuote.HasUpdates);
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeDateUpdated    = false;
        noRecentlyTradedEmptyQuote.IsAdapterSentTimeSubHourUpdated = false;
        Assert.IsFalse(noRecentlyTradedEmptyQuote.HasUpdates);
        Assert.IsTrue(noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        noRecentlyTradedEmptyQuote.IsValueDateUpdated = true;
        level3QuoteUpdates =
            (from update in noRecentlyTradedEmptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.Id == PQFieldKeys.ValueDate
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
                    = new PQFieldUpdate(PQFieldKeys.LastTradePriceOffset, PQScaling.Scale(expectedPrice, priceScale), priceScale);
                var expectedQuoteLastTradeField =
                    new PQFieldUpdate
                        ((byte)(PQFieldKeys.LastTradePriceOffset + i), expectedLastTradeField.Value, expectedLastTradeField.Flag);
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsTradePriceUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsTradePriceUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQFieldKeys.LastTradePriceOffset + i
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

                Assert.IsFalse(lastTrade.IsTradeTimeDateUpdated);
                Assert.IsFalse(lastTrade.IsTradeTimeSubHourUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(DateTimeConstants.UnixEpoch, lastTrade.TradeTime);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedTradeTime = new DateTime(2017, 12, 31, 13, 12, 20).AddTicks(9_999_999);
                lastTrade.TradeTime = expectedTradeTime;
                Assert.IsTrue(lastTrade.IsTradeTimeDateUpdated);
                Assert.IsTrue(lastTrade.IsTradeTimeSubHourUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedTradeTime, lastTrade.TradeTime);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(4, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(2, lastTradeUpdates.Count);
                var hoursFromEpoch = expectedTradeTime.GetHoursFromUnixEpoch();
                var flag           = expectedTradeTime.GetSubHourComponent().BreakLongToByteAndUint(out var subHourBase);

                var expectedLastTradeHoursField =
                    new PQFieldUpdate(PQFieldKeys.LastTradeTimeHourOffset, hoursFromEpoch);
                var expectedLastTradeSubHoursField =
                    new PQFieldUpdate(PQFieldKeys.LastTradeTimeSubHourOffset, subHourBase, flag);
                Assert.AreEqual(expectedLastTradeHoursField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedLastTradeSubHoursField, lastTradeUpdates[1]);
                var expectedQuoteLastTradeTimeHoursField =
                    new PQFieldUpdate((byte)(PQFieldKeys.LastTradeTimeHourOffset + i), hoursFromEpoch);
                var expectedQuoteLastTradeTimeSubHoursField =
                    new PQFieldUpdate((byte)(PQFieldKeys.LastTradeTimeSubHourOffset + i), subHourBase, flag);
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

                lastTrade.IsTradeTimeSubHourUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsTradeTimeDateUpdated    = true;
                lastTrade.IsTradeTimeSubHourUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQFieldKeys.LastTradeTimeHourOffset + i ||
                              update.Id == PQFieldKeys.LastTradeTimeSubHourOffset + i
                        select update).ToList();
                Assert.AreEqual(2, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeTimeHoursField, quoteUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeTimeSubHoursField, quoteUpdates[1]);
                lastTrade.TradeTime                 = DateTimeConstants.UnixEpoch;
                lastTrade.IsTradeTimeDateUpdated    = false;
                lastTrade.IsTradeTimeSubHourUpdated = false;

                var newEmpty = new PQLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                newEmpty.UpdateField(quoteUpdates[1]);
                var foundLayer = newEmpty.RecentlyTraded![i]!;
                Assert.AreEqual(expectedTradeTime, foundLayer.TradeTime);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundLayer.IsTradeTimeDateUpdated);
                Assert.IsTrue(foundLayer.IsTradeTimeSubHourUpdated);
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

                Assert.IsFalse(lastTrade.IsWasGivenUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(false, lastTrade.WasGiven);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                const bool expectedWasGiven = true;
                lastTrade.WasGiven = expectedWasGiven;
                var pqSrcTrkQtInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                var volumeScale    = pqSrcTrkQtInfo.VolumeScalingPrecision;
                Assert.IsTrue(lastTrade.IsWasGivenUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedWasGiven, lastTrade.WasGiven);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField = new PQFieldUpdate
                    (PQFieldKeys.LastTradeVolumeOffset, 0, (byte)(PQFieldFlags.IsGivenFlag | volumeScale));
                var expectedQuoteLastTradeField = new PQFieldUpdate
                    ((byte)(PQFieldKeys.LastTradeVolumeOffset + i), 0, (byte)(PQFieldFlags.IsGivenFlag | volumeScale));
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsWasGivenUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsWasGivenUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQFieldKeys.LastTradeVolumeOffset + i
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

                Assert.IsFalse(lastTrade.IsWasPaidUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(false, lastTrade.WasPaid);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                const bool expectedWasPaid = true;
                lastTrade.WasPaid = expectedWasPaid;
                var pqSrcTrkQtInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                var volumeScale    = pqSrcTrkQtInfo.VolumeScalingPrecision;
                Assert.IsTrue(lastTrade.IsWasPaidUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedWasPaid, lastTrade.WasPaid);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField =
                    new PQFieldUpdate(PQFieldKeys.LastTradeVolumeOffset, 0, (byte)(PQFieldFlags.IsPaidFlag | volumeScale));
                var expectedQuoteLastTradeField =
                    new PQFieldUpdate((byte)(PQFieldKeys.LastTradeVolumeOffset + i),
                                      0, (byte)(PQFieldFlags.IsPaidFlag | volumeScale));
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsWasPaidUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsWasPaidUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQFieldKeys.LastTradeVolumeOffset + i
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
    public void AllLevel3QuoteTypes_LastTradeVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            if (!(emptyQuote.RecentlyTraded?[0] is IPQLastPaidGivenTrade)) continue;
            for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = (IPQLastPaidGivenTrade)emptyQuote.RecentlyTraded[i]!;

                Assert.IsFalse(lastTrade.IsTradeVolumeUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(0m, lastTrade.TradeVolume);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedVolume = 42_130_000m;
                var pqSrcTrkQtInfo = (PQSourceTickerInfo)emptyQuote.SourceTickerInfo!;
                var volumeScale    = pqSrcTrkQtInfo.VolumeScalingPrecision;
                lastTrade.TradeVolume = expectedVolume;
                Assert.IsTrue(lastTrade.IsTradeVolumeUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedVolume, lastTrade.TradeVolume);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update, pqSrcTrkQtInfo).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField =
                    new PQFieldUpdate(PQFieldKeys.LastTradeVolumeOffset, PQScaling.Scale(expectedVolume, volumeScale), volumeScale);
                var expectedQuoteLastTradeField =
                    new PQFieldUpdate((byte)(PQFieldKeys.LastTradeVolumeOffset + i), expectedLastTradeField.Value, expectedLastTradeField.Flag);
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);

                lastTrade.IsTradeVolumeUpdated = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsTradeVolumeUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQFieldKeys.LastTradeVolumeOffset + i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.TradeVolume          = 0m;
                lastTrade.IsTradeVolumeUpdated = false;

                var newEmpty = new PQLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                var foundLayer = (IPQLastPaidGivenTrade)newEmpty.RecentlyTraded![i]!;
                Assert.AreEqual(expectedVolume, foundLayer.TradeVolume);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundLayer.IsTradeVolumeUpdated);
            }
        }
    }

    [TestMethod]
    public void AllLevel3QuoteTypes_LastTradeTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        foreach (var emptyQuote in allEmptyQuotes)
        {
            if (!(emptyQuote.RecentlyTraded?[0] is IPQLastTraderPaidGivenTrade)) continue;
            for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
            {
                testDateTime = testDateTime.AddHours(1).AddSeconds(1);

                var lastTrade = (IPQLastTraderPaidGivenTrade)emptyQuote.RecentlyTraded[i]!;

                Assert.IsFalse(lastTrade.IsTraderNameUpdated);
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.AreEqual(null, lastTrade.TraderName);
                Assert.AreEqual(0, lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());
                Assert.AreEqual(2, emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

                var expectedTraderName = "LastTrade_TraderName" + i;
                lastTrade.TraderName = expectedTraderName;
                Assert.IsTrue(lastTrade.IsTraderNameUpdated);
                Assert.IsTrue(emptyQuote.HasUpdates);
                Assert.AreEqual(expectedTraderName, lastTrade.TraderName);
                var quoteUpdates = emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(3, quoteUpdates.Count);
                var lastTradeUpdates = lastTrade.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
                Assert.AreEqual(1, lastTradeUpdates.Count);
                var expectedLastTradeField =
                    new PQFieldUpdate(PQFieldKeys.LastTraderIdOffset, lastTrade.TraderId);
                var expectedQuoteLastTradeField =
                    new PQFieldUpdate((byte)(PQFieldKeys.LastTraderIdOffset + i), lastTrade.TraderId);
                Assert.AreEqual(expectedLastTradeField, lastTradeUpdates[0]);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[2]);
                var stringUpdates = emptyQuote.GetStringUpdates(testDateTime, StorageFlags.Update).ToList();
                var expectedStringUpdates = new PQFieldStringUpdate
                {
                    Field = new PQFieldUpdate(PQFieldKeys.LastTraderDictionaryUpsertCommand, 0u, PQFieldFlags.IsUpsert)
                  , StringUpdate = new PQStringUpdate
                    {
                        Command = CrudCommand.Upsert, DictionaryId = lastTrade.NameIdLookup[lastTrade.TraderName]
                      , Value   = lastTrade.TraderName
                    }
                };
                Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

                lastTrade.IsTraderNameUpdated = false;
                Assert.IsTrue(lastTrade.HasUpdates);
                lastTrade.NameIdLookup.HasUpdates = false;
                Assert.IsFalse(lastTrade.HasUpdates);
                Assert.IsTrue(emptyQuote.HasUpdates);
                emptyQuote.IsAdapterSentTimeDateUpdated    = false;
                emptyQuote.IsAdapterSentTimeSubHourUpdated = false;
                Assert.IsFalse(emptyQuote.HasUpdates);
                Assert.IsTrue(emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

                lastTrade.IsTraderNameUpdated = true;
                quoteUpdates =
                    (from update in emptyQuote.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                        where update.Id == PQFieldKeys.LastTraderIdOffset + i
                        select update).ToList();
                Assert.AreEqual(1, quoteUpdates.Count);
                Assert.AreEqual(expectedQuoteLastTradeField, quoteUpdates[0]);
                lastTrade.TraderName          = null;
                lastTrade.IsTraderNameUpdated = false;

                var newEmpty = new PQLevel3Quote(emptyQuote.SourceTickerInfo!);
                newEmpty.UpdateField(quoteUpdates[0]);
                newEmpty.UpdateFieldString(stringUpdates[0]);
                var foundLayer = (IPQLastTraderPaidGivenTrade)newEmpty.RecentlyTraded![i]!;
                Assert.AreEqual(expectedTraderName, foundLayer.TraderName);
                Assert.IsTrue(newEmpty.HasUpdates);
                Assert.IsTrue(foundLayer.IsTraderNameUpdated);
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
            AssertContainsAllLevel3Fields
                (precisionSettings, pqFieldUpdates, populatedL3Quote,
                 PQBooleanValuesExtensions
                     .AllExceptExecutableUpdated
                     .Unset
                         ((populatedL3Quote.IsBidPriceTopUpdated ? PQBooleanValues.None : PQBooleanValues.IsBidPriceTopUpdatedSetFlag)
                        | (populatedL3Quote.IsAskPriceTopUpdated ? PQBooleanValues.None : PQBooleanValues.IsAskPriceTopUpdatedSetFlag)));
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
            AssertContainsAllLevel3Fields
                (precisionSettings, pqFieldUpdates, populatedL3Quote,
                 PQBooleanValuesExtensions
                     .AllFields
                     .Unset
                         ((populatedL3Quote.IsBidPriceTopUpdated ? PQBooleanValues.None : PQBooleanValues.IsBidPriceTopUpdatedSetFlag)
                        | (populatedL3Quote.IsAskPriceTopUpdated ? PQBooleanValues.None : PQBooleanValues.IsAskPriceTopUpdatedSetFlag)));
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
                    NameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand)
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
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.ValueDate);
            Assert.AreEqual(pqLevel3Quote.PQSequenceId, emptyQuote.PQSequenceId);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceTime);
            Assert.IsTrue(pqLevel3Quote.SourceTickerInfo!.AreEquivalent(emptyQuote.SourceTickerInfo));
            Assert.AreEqual(false, emptyQuote.IsReplay);
            Assert.AreEqual(0m, emptyQuote.SingleTickValue);
            Assert.AreEqual(FeedSyncStatus.OutOfSync, emptyQuote.FeedSyncStatus);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceBidTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SourceAskTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterReceivedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.AdapterSentTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.ClientReceivedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.ProcessedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.DispatchedTime);
            Assert.AreEqual(DateTimeConstants.UnixEpoch, emptyQuote.SocketReceivingTime);
            Assert.AreEqual(0m, emptyQuote.BidPriceTop);
            Assert.AreEqual(0m, emptyQuote.AskPriceTop);
            Assert.IsTrue(emptyQuote.Executable);
            Assert.IsFalse(emptyQuote.IsBatchIdUpdated);
            Assert.IsFalse(emptyQuote.IsSourceQuoteReferenceUpdated);
            Assert.IsFalse(emptyQuote.IsValueDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsReplayUpdated);
            Assert.IsFalse(emptyQuote.IsSingleValueUpdated);
            Assert.IsFalse(emptyQuote.IsFeedSyncStatusUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceBidTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsSourceAskTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterReceivedTimeSubHourUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeDateUpdated);
            Assert.IsFalse(emptyQuote.IsAdapterSentTimeSubHourUpdated);
            Assert.AreEqual(pqLevel3Quote.IsBidPriceTopUpdated, emptyQuote.IsBidPriceTopUpdated);
            Assert.AreEqual(pqLevel3Quote.IsAskPriceTopUpdated, emptyQuote.IsAskPriceTopUpdated);
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
        PQLevel3Quote l3Q, PQBooleanValues expectedBooleanFlags = PQBooleanValuesExtensions.AllExceptExecutableUpdated)
    {
        PQLevel2QuoteTests.AssertContainsAllLevel2Fields(precisionSettings, checkFieldUpdates, l3Q, expectedBooleanFlags);

        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.BatchId, l3Q.BatchId),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.BatchId));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceQuoteReference, l3Q.SourceQuoteReference),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceQuoteReference));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.ValueDate, l3Q.ValueDate.GetHoursFromUnixEpoch()),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.ValueDate));
        if (l3Q.RecentlyTraded == null) return;
        var priceScale  = precisionSettings.PriceScalingPrecision;
        var volumeScale = precisionSettings.VolumeScalingPrecision;
        for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
        {
            var lastTrade = l3Q.RecentlyTraded[i]!;

            Assert.AreEqual
                (new PQFieldUpdate
                     ((byte)(PQFieldKeys.LastTradePriceOffset + i), PQScaling.Scale(lastTrade.TradePrice, priceScale), priceScale)
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, (byte)(PQFieldKeys.LastTradePriceOffset + i), priceScale)
               , $"For lastTradeType {lastTrade.GetType().Name} level {i}");
            Assert.AreEqual
                (new PQFieldUpdate((byte)(PQFieldKeys.LastTradeTimeHourOffset + i), lastTrade.TradeTime.GetHoursFromUnixEpoch())
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, (byte)(PQFieldKeys.LastTradeTimeHourOffset + i))
               , $"For bidlayer {lastTrade.GetType().Name} level {i}");
            var flag = lastTrade.TradeTime.GetSubHourComponent().BreakLongToByteAndUint(out var subHourBase);
            Assert.AreEqual
                (new PQFieldUpdate((byte)(PQFieldKeys.LastTradeTimeSubHourOffset + i), subHourBase, flag)
               , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, (byte)(PQFieldKeys.LastTradeTimeSubHourOffset + i), flag)
               , $"For asklayer {lastTrade.GetType().Name} level {i}");

            if (lastTrade is IPQLastPaidGivenTrade pqPaidGivenTrade)
            {
                byte noVal        = 0;
                var  expectedFlag = (byte)(volumeScale | (pqPaidGivenTrade.WasGiven ? PQFieldFlags.IsGivenFlag : noVal));
                expectedFlag |= pqPaidGivenTrade.WasPaid ? PQFieldFlags.IsPaidFlag : noVal;


                Assert.AreEqual
                    (new PQFieldUpdate
                         ((byte)(PQFieldKeys.LastTradeVolumeOffset + i), PQScaling.Scale(pqPaidGivenTrade.TradeVolume, volumeScale), expectedFlag)
                   , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, (byte)(PQFieldKeys.LastTradeVolumeOffset + i)
                                                               , expectedFlag)
                   , $"For asklayer {lastTrade.GetType().Name} level {i}");
            }

            if (lastTrade is IPQLastTraderPaidGivenTrade pqTraderPaidGivenTrade)
                Assert.AreEqual
                    (new PQFieldUpdate((byte)(PQFieldKeys.LastTraderIdOffset + i), pqTraderPaidGivenTrade.TraderId)
                   , PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, (byte)(PQFieldKeys.LastTraderIdOffset + i))
                   , $"For asklayer {lastTrade.GetType().Name} level {i}");
        }
    }

    public static void AssertAreDefaultValues(IPQLastTrade pvl)
    {
        Assert.AreEqual(0m, pvl.TradePrice);
        Assert.AreEqual(DateTimeConstants.UnixEpoch, pvl.TradeTime);
        Assert.IsFalse(pvl.IsTradePriceUpdated);
        Assert.IsFalse(pvl.IsTradeTimeDateUpdated);
        Assert.IsFalse(pvl.IsTradeTimeSubHourUpdated);
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
