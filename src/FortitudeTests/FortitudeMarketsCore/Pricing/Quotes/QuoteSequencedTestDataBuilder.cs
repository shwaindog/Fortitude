// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Summaries;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

public class QuoteSequencedTestDataBuilder
{
    private decimal lastAskTop = 0;
    private decimal lastBidTop = 0;

    public void InitializeQuotes(IEnumerable<IMutableTickInstant> initializeQuotes, uint batchId)
    {
        foreach (var quote in initializeQuotes) InitializeQuote(quote, batchId);
    }

    public void InitializeQuote(IMutableTickInstant initializeQuote, uint batchId)
    {
        SetupTickInstant(initializeQuote, batchId);
        SetupLevel1Quote(initializeQuote as IMutableLevel1Quote, batchId);
        SetupLevel2Quote(initializeQuote as IMutableLevel2Quote, batchId);
        SetupLevel3Quote(initializeQuote as IMutableLevel3Quote, batchId);
        if (initializeQuote is IMutableLevel1Quote l1Quote)
        {
            lastBidTop = l1Quote.BidPriceTop;
            lastAskTop = l1Quote.AskPriceTop;
        }
    }

    public void InitalizePeriodSummary(IMutablePricePeriodSummary pricePeriodSummary, uint batchId)
    {
        pricePeriodSummary.TimeBoundaryPeriod = TimeBoundaryPeriod.OneSecond;
        pricePeriodSummary.PeriodStartTime    = new DateTime(2017, 11, 18, 20, 09, 11);
        pricePeriodSummary.StartBidPrice      = 0.79324m;
        pricePeriodSummary.StartAskPrice      = 0.79334m;
        pricePeriodSummary.HighestBidPrice    = 0.79354m;
        pricePeriodSummary.HighestAskPrice    = 0.79364m;
        pricePeriodSummary.LowestBidPrice     = 0.79304m;
        pricePeriodSummary.LowestAskPrice     = 0.79314m;
        pricePeriodSummary.EndBidPrice        = 0.79334m;
        pricePeriodSummary.EndAskPrice        = 0.79344m;
        pricePeriodSummary.TickCount          = 10;
        pricePeriodSummary.PeriodVolume       = 100_000_000_000;
        pricePeriodSummary.PeriodSummaryFlags = PricePeriodSummaryFlags.FromStorage;
        pricePeriodSummary.PeriodEndTime      = new DateTime(2017, 11, 18, 20, 09, 12);
    }

    private void SetupLevel3Quote(IMutableLevel3Quote? pqLevel3Quote, uint batchId)
    {
        if (pqLevel3Quote == null) return;
        pqLevel3Quote.BatchId              = batchId;
        pqLevel3Quote.SourceQuoteReference = batchId;
        pqLevel3Quote.ValueDate            = new DateTime(2018, 1, 1, 14, 0, 0).AddHours(batchId);

        var toggleGivenBool = false;
        var togglePaidBool  = true;
        if (pqLevel3Quote.RecentlyTraded == null) return;
        for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
        {
            var tradePriceDelta  = 0.00001m * i;
            var tradeVolumeDelta = batchId * 10000 + 10000m * i;

            pqLevel3Quote.RecentlyTraded[i]!.TradePrice = 0.76591m + batchId * 0.00001m + tradePriceDelta;
            pqLevel3Quote.RecentlyTraded[i]!.TradeTime  = new DateTime(2017, 07, 02, 13, 40, 11);

            if (pqLevel3Quote.RecentlyTraded[i] is IPQLastPaidGivenTrade lastPaidGivenTrade)
            {
                lastPaidGivenTrade.TradeVolume = 2000000 + tradeVolumeDelta;
                lastPaidGivenTrade.WasGiven    = toggleGivenBool = !toggleGivenBool;
                lastPaidGivenTrade.WasPaid     = togglePaidBool  = !togglePaidBool;
            }

            if (pqLevel3Quote.RecentlyTraded[i] is IPQLastTraderPaidGivenTrade lastTraderTrade) lastTraderTrade.TraderName = "NewTraderName " + i;
        }
    }

    private void SetupLevel2Quote(IMutableLevel2Quote? level2Quote, uint batchId)
    {
        if (level2Quote == null) return;
        var numLayers = level2Quote.BidBook.Capacity;
        Assert.IsTrue(numLayers >= 20);
        for (var i = 0; i < numLayers; i++)
        {
            var deltaPrice  = 0.00001m * i;
            var deltaVolume = batchId * 100_000 + 100_000m * i;
            var mutableBid  = level2Quote.BidBook[i]!;
            var mutableAsk  = level2Quote.AskBook[i]!;
            mutableBid.Price  = 0.791905m + batchId * 0.00001m - deltaPrice;
            mutableBid.Volume = 100_000 + deltaVolume;
            mutableAsk.Price  = 0.791906m + batchId * 0.00001m + deltaPrice;
            mutableAsk.Volume = 100_0000 + deltaVolume;

            if (mutableBid is IMutableSourcePriceVolumeLayer mutableBidPriceVal) SetupSourceNameOnLayer(mutableBidPriceVal, true, batchId);
            if (mutableAsk is IMutableSourcePriceVolumeLayer mutableAskPriceVal) SetupSourceNameOnLayer(mutableAskPriceVal, false, batchId);
            if (mutableBid is IMutableSourceQuoteRefPriceVolumeLayer mutableBidSrcQuotRef)
                SetupSourceQuoteRefOnLayer(mutableBidSrcQuotRef, true, batchId);
            if (mutableAsk is IMutableSourceQuoteRefPriceVolumeLayer mutableAskSrcQuotRef)
                SetupSourceQuoteRefOnLayer(mutableAskSrcQuotRef, false, batchId);
            if (mutableBid is IMutableValueDatePriceVolumeLayer mutableBidValueDate) SetupValueDateOnLayer(mutableBidValueDate, true, batchId);
            if (mutableAsk is IMutableValueDatePriceVolumeLayer mutableAskValueDate) SetupValueDateOnLayer(mutableAskValueDate, false, batchId);
            if (mutableBid is IMutableTraderPriceVolumeLayer mutableBidTrader) SetupTraderDetailsOnLayer(mutableBidTrader, true, batchId);
            if (mutableAsk is IMutableTraderPriceVolumeLayer mutableAskTrader) SetupTraderDetailsOnLayer(mutableAskTrader, false, batchId);
        }
        level2Quote.IsBidPriceTopUpdated = lastBidTop != level2Quote.BidPriceTop;
        level2Quote.IsAskPriceTopUpdated = lastAskTop != level2Quote.AskPriceTop;
    }

    private void SetupValueDateOnLayer(IMutableValueDatePriceVolumeLayer layer, bool isBidBook, uint batchId)
    {
        // can only transmit to hour fidelity.  It's only supposed to be Date not DateTime.
        layer.ValueDate = new DateTime(2017, 11, 26, 18, 0, 0).AddDays(batchId);
    }

    private void SetupTraderDetailsOnLayer(IMutableTraderPriceVolumeLayer? layer, bool isBidBook, uint batchId)
    {
        if (layer == null) return;
        for (var i = 0; i < 2; i++)
        {
            layer[i]!.TraderName   = (isBidBook ? "B" : "A") + "TN" + (batchId * 5 + i);
            layer[i]!.TraderVolume = batchId * 25600 + i * 100;
        }
    }

    private void SetupSourceQuoteRefOnLayer(IMutableSourceQuoteRefPriceVolumeLayer? layer, bool isBidBook, uint batchId)
    {
        if (layer == null) return;
        layer.SourceQuoteReference = (isBidBook ? 1000u : 5000u) + batchId;
    }

    private void SetupSourceNameOnLayer(IMutableSourcePriceVolumeLayer? layer, bool isBidBook, uint batchId)
    {
        if (layer == null) return;
        layer.SourceName = "TestSourceName" + (isBidBook ? "_Bid_" : "_Ask_") + batchId;
        layer.Executable = true;
    }

    private void SetupLevel1Quote(IMutableLevel1Quote? level1Quote, uint batchId)
    {
        if (level1Quote == null) return;
        level1Quote.IsReplay   = true;
        level1Quote.Executable = true;
        var sequenceIdTimeSpan = TimeSpan.FromSeconds(batchId * 10);

        level1Quote.SourceAskTime       = new DateTime(2017, 07, 16, 15, 49, 10).Add(sequenceIdTimeSpan);
        level1Quote.SourceBidTime       = new DateTime(2017, 07, 16, 15, 49, 20).Add(sequenceIdTimeSpan);
        level1Quote.AdapterReceivedTime = new DateTime(2017, 07, 16, 15, 49, 30).Add(sequenceIdTimeSpan);
        level1Quote.AdapterSentTime     = new DateTime(2017, 07, 16, 15, 49, 40).Add(sequenceIdTimeSpan);
        level1Quote.BidPriceTop         = 0.79324m + batchId * 0.00001m;
        level1Quote.AskPriceTop         = 0.79326m + batchId * 0.00001m;

        level1Quote.IsBidPriceTopUpdated = lastBidTop != level1Quote.BidPriceTop;
        level1Quote.IsAskPriceTopUpdated = lastAskTop != level1Quote.AskPriceTop;

        switch (level1Quote.SummaryPeriod)
        {
            case null when level1Quote is PQLevel1Quote:
                level1Quote.SummaryPeriod = new PQPricePeriodSummary();
                break;
            case null:
                level1Quote.SummaryPeriod = new PricePeriodSummary();
                break;
        }

        InitalizePeriodSummary(level1Quote.SummaryPeriod, batchId);
    }

    private void SetupTickInstant(IMutableTickInstant tickInstant, uint batchId)
    {
        tickInstant.SingleTickValue    = 0.78568m + batchId * 0.00001m;
        tickInstant.SourceTime         = new DateTime(2017, 07, 16, 15, 46, 00).Add(TimeSpan.FromSeconds(batchId * 10));
        tickInstant.IsReplay           = true;
        tickInstant.ClientReceivedTime = tickInstant.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 10));
        if (tickInstant is IPQTickInstant pqTickInstant)
        {
            pqTickInstant.FeedSyncStatus = FeedSyncStatus.Good;
            pqTickInstant.PQSequenceId   = batchId;
            pqTickInstant.DispatchedTime = pqTickInstant.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 11));
            pqTickInstant.SocketReceivingTime =
                pqTickInstant.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 12));
            pqTickInstant.ProcessedTime = pqTickInstant.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 13));
            pqTickInstant.LastPublicationTime =
                pqTickInstant.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 14));
        }
    }
}
