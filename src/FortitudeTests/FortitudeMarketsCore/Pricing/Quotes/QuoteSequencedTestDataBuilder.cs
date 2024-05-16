#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries;
using FortitudeMarketsCore.Pricing.TimeSeries;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

public class QuoteSequencedTestDataBuilder
{
    public void InitializeQuotes(IList<IPQLevel0Quote> initializeQuotes, uint batchId)
    {
        foreach (var quote in initializeQuotes)
        {
            SetupLevel0Quote(quote, batchId);
            SetupLevel1Quote(quote as IMutableLevel1Quote, batchId);
            SetupLevel2Quote(quote as IMutableLevel2Quote, batchId);
            SetupLevel3Quote(quote as IMutableLevel3Quote, batchId);
        }
    }

    public void InitializeQuote(IMutableLevel0Quote initializeQuotes, uint batchId)
    {
        SetupLevel0Quote(initializeQuotes, batchId);
        SetupLevel1Quote(initializeQuotes as IMutableLevel1Quote, batchId);
        SetupLevel2Quote(initializeQuotes as IMutableLevel2Quote, batchId);
        SetupLevel3Quote(initializeQuotes as IMutableLevel3Quote, batchId);
    }

    public void InitalizePeriodSummary(IMutableQuotePeriodSummary quotePeriodSummary, uint batchId)
    {
        quotePeriodSummary.SummaryStartTime = new DateTime(2017, 11, 18, 20, 09, 11);
        quotePeriodSummary.StartBidPrice = 0.79324m;
        quotePeriodSummary.StartAskPrice = 0.79334m;
        quotePeriodSummary.HighestBidPrice = 0.79354m;
        quotePeriodSummary.HighestAskPrice = 0.79364m;
        quotePeriodSummary.LowestBidPrice = 0.79304m;
        quotePeriodSummary.LowestAskPrice = 0.79314m;
        quotePeriodSummary.EndBidPrice = 0.79334m;
        quotePeriodSummary.EndAskPrice = 0.79344m;
        quotePeriodSummary.TickCount = 10;
        quotePeriodSummary.PeriodVolume = 400_000_000_000_000;
        quotePeriodSummary.SummaryEndTime = new DateTime(2017, 11, 18, 20, 09, 12);
    }

    private void SetupLevel3Quote(IMutableLevel3Quote? pqLevel3Quote, uint batchId)
    {
        if (pqLevel3Quote == null) return;
        pqLevel3Quote.BatchId = batchId;
        pqLevel3Quote.SourceQuoteReference = batchId;
        pqLevel3Quote.ValueDate = new DateTime(2018, 1, 1, 14, 0, 0).AddHours(batchId);

        var toggleGivenBool = false;
        var togglePaidBool = true;
        if (pqLevel3Quote.RecentlyTraded == null) return;
        for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
        {
            var tradePriceDelta = 0.00001m * i;
            var tradeVolumeDelta = batchId * 10000 + 10000m * i;

            pqLevel3Quote.RecentlyTraded[i]!.TradePrice = 0.76591m + batchId * 0.00001m + tradePriceDelta;
            pqLevel3Quote.RecentlyTraded[i]!.TradeTime = new DateTime(2017, 07, 02, 13, 40, 11);

            if (pqLevel3Quote.RecentlyTraded[i] is IPQLastPaidGivenTrade lastPaidGivenTrade)
            {
                lastPaidGivenTrade.TradeVolume = 2000000 + tradeVolumeDelta;
                lastPaidGivenTrade.WasGiven = toggleGivenBool = !toggleGivenBool;
                lastPaidGivenTrade.WasPaid = togglePaidBool = !togglePaidBool;
            }

            if (pqLevel3Quote.RecentlyTraded[i] is IPQLastTraderPaidGivenTrade lastTraderTrade)
                lastTraderTrade.TraderName = "NewTraderName " + i;
        }
    }

    private void SetupLevel2Quote(IMutableLevel2Quote? level2Quote, uint batchId)
    {
        if (level2Quote == null) return;
        var numLayers = level2Quote.BidBook.Capacity;
        Assert.IsTrue(numLayers >= 20);
        for (var i = 0; i < numLayers; i++)
        {
            var deltaPrice = 0.00001m * i;
            var deltaVolume = batchId * 10000 + 10000m * i;
            var mutableBid = level2Quote.BidBook[i]!;
            var mutableAsk = level2Quote.AskBook[i]!;
            mutableBid.Price = 0.791905m + batchId * 0.00001m - deltaPrice;
            mutableBid.Volume = 30000 + deltaVolume;
            mutableAsk.Price = 0.791906m + batchId * 0.00001m + deltaPrice;
            mutableAsk.Volume = 30000 + deltaVolume;

            if (mutableBid is IMutableSourcePriceVolumeLayer mutableBidPriceVal)
                SetupSourceNameOnLayer(mutableBidPriceVal, true, batchId);
            if (mutableAsk is IMutableSourcePriceVolumeLayer mutableAskPriceVal)
                SetupSourceNameOnLayer(mutableAskPriceVal, false, batchId);
            if (mutableBid is IMutableSourceQuoteRefPriceVolumeLayer mutableBidSrcQuotRef)
                SetupSourceQuoteRefOnLayer(mutableBidSrcQuotRef, true, batchId);
            if (mutableAsk is IMutableSourceQuoteRefPriceVolumeLayer mutableAskSrcQuotRef)
                SetupSourceQuoteRefOnLayer(mutableAskSrcQuotRef, false, batchId);
            if (mutableBid is IMutableValueDatePriceVolumeLayer mutableBidValueDate)
                SetupValueDateOnLayer(mutableBidValueDate, true, batchId);
            if (mutableAsk is IMutableValueDatePriceVolumeLayer mutableAskValueDate)
                SetupValueDateOnLayer(mutableAskValueDate, false, batchId);
            if (mutableBid is IMutableTraderPriceVolumeLayer mutableBidTrader)
                SetupTraderDetailsOnLayer(mutableBidTrader, true, batchId);
            if (mutableAsk is IMutableTraderPriceVolumeLayer mutableAskTrader)
                SetupTraderDetailsOnLayer(mutableAskTrader, false, batchId);
        }
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
            layer[i]!.TraderName = (isBidBook ? "B" : "A") + "TN" + (batchId * 5 + i);
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
        level1Quote.IsReplay = true;
        level1Quote.Executable = true;
        var sequenceIdTimeSpan = TimeSpan.FromSeconds(batchId * 10);
        level1Quote.SourceAskTime = new DateTime(2017, 07, 16, 15, 49, 10).Add(sequenceIdTimeSpan);
        level1Quote.SourceBidTime = new DateTime(2017, 07, 16, 15, 49, 20).Add(sequenceIdTimeSpan);
        level1Quote.AdapterReceivedTime = new DateTime(2017, 07, 16, 15, 49, 30).Add(sequenceIdTimeSpan);
        level1Quote.AdapterSentTime = new DateTime(2017, 07, 16, 15, 49, 40).Add(sequenceIdTimeSpan);
        level1Quote.BidPriceTop = 0.79324m + batchId * 0.00001m;
        level1Quote.AskPriceTop = 0.79326m + batchId * 0.00001m;
        switch (level1Quote.SummaryPeriod)
        {
            case null when level1Quote is PQLevel1Quote:
                level1Quote.SummaryPeriod = new PQQuotePeriodSummary();
                break;
            case null:
                level1Quote.SummaryPeriod = new QuotePeriodSummary();
                break;
        }

        InitalizePeriodSummary(level1Quote.SummaryPeriod, batchId);
    }

    private void SetupLevel0Quote(IMutableLevel0Quote level0Quote, uint batchId)
    {
        level0Quote.SinglePrice = 0.78568m + batchId * 0.00001m;
        level0Quote.SourceTime = new DateTime(2017, 07, 16, 15, 46, 00).Add(TimeSpan.FromSeconds(batchId * 10));
        level0Quote.IsReplay = true;
        level0Quote.ClientReceivedTime = level0Quote.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 10));
        if (level0Quote is IPQLevel0Quote pqLevel0Quote)
        {
            pqLevel0Quote.PQSyncStatus = PQSyncStatus.Good;
            pqLevel0Quote.PQSequenceId = batchId;
            pqLevel0Quote.DispatchedTime = pqLevel0Quote.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 11));
            pqLevel0Quote.SocketReceivingTime =
                pqLevel0Quote.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 12));
            pqLevel0Quote.ProcessedTime = pqLevel0Quote.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 13));
            pqLevel0Quote.LastPublicationTime =
                pqLevel0Quote.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 14));
        }
    }
}
