// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

public class QuoteSequencedTestDataBuilder
{
    public const int GeneratedNumberOfLastTrades = 12;


    private decimal lastAskTop;
    private decimal lastBidTop;

    public void InitializeQuotes(IEnumerable<IMutableTickInstant> initializeQuotes, uint batchId)
    {
        lastAskTop = 0;
        lastBidTop = 0;
        foreach (var quote in initializeQuotes) InitializeQuoteSeries(quote, batchId);
    }

    public void InitializeQuote(IMutableTickInstant initializeQuote, uint batchId)
    {
        lastAskTop = 0;
        lastBidTop = 0;
        InitializeQuoteSeries(initializeQuote, batchId);
    }

    public void InitializeQuoteSeries(IMutableTickInstant initializeQuote, uint batchId)
    {
        initializeQuote.StateReset();
        SetupTickInstant(initializeQuote, batchId);
        SetupLevel1Quote(initializeQuote as IMutableLevel1Quote, batchId);
        SetupLevel2Quote(initializeQuote as IMutableLevel2Quote, batchId);
        SetupLevel3Quote(initializeQuote as IMutableLevel3Quote, batchId);
    }

    public void InitalizeCandle(IMutableCandle candle, uint batchId)
    {
        candle.TimeBoundaryPeriod = TimeBoundaryPeriod.OneSecond;
        candle.PeriodStartTime    = new DateTime(2017, 11, 18, 20, 09, 11);
        candle.StartBidPrice      = 0.793241m;
        candle.StartAskPrice      = 0.793341m;
        candle.HighestBidPrice    = 0.793541m;
        candle.HighestAskPrice    = 0.793641m;
        candle.LowestBidPrice     = 0.793041m;
        candle.LowestAskPrice     = 0.793141m;
        candle.EndBidPrice        = 0.793341m;
        candle.EndAskPrice        = 0.793441m;
        candle.AverageBidPrice    = 0.793241m;
        candle.AverageAskPrice    = 0.793341m;
        candle.TickCount          = 10;
        candle.PeriodVolume       = 100_000_000_000;
        candle.CandleFlags = CandleFlags.FromStorage;
        candle.PeriodEndTime      = new DateTime(2017, 11, 18, 20, 09, 12);
    }

    private void SetupLevel3Quote(IMutableLevel3Quote? pqLevel3Quote, uint batchId)
    {
        if (pqLevel3Quote == null) return;
        pqLevel3Quote.BatchId              = batchId;
        pqLevel3Quote.SourceQuoteReference = batchId;
        pqLevel3Quote.ValueDate            = new DateTime(2018, 1, 1, 14, 0, 0).AddHours(batchId);

        var toggleGivenBool = false;
        var togglePaidBool  = true;
        if (pqLevel3Quote.OnTickLastTraded == null || pqLevel3Quote is IPublishableLevel3Quote {SourceTickerInfo.LastTradedFlags: not LastTradedFlags.None}) return;
        for (var i = 0; i < GeneratedNumberOfLastTrades; i++)
        {
            var tradePriceDelta  = 0.00001m * i;
            var tradeVolumeDelta = batchId * 10000 + 10000m * i;

            var lastTradedInfo = pqLevel3Quote.OnTickLastTraded[i]!;
            // lastTradedInfo.StateReset();
            lastTradedInfo!.TradePrice = 0.76591m + batchId * 0.00001m + tradePriceDelta;
            lastTradedInfo!.TradeTime  = new DateTime(2017, 07, 02, 13, 40, 11);

            if (lastTradedInfo is IMutableLastPaidGivenTrade lastPaidGivenTrade)
            {
                lastPaidGivenTrade.TradeVolume = 2000000 + tradeVolumeDelta;
                lastPaidGivenTrade.WasGiven    = toggleGivenBool = !toggleGivenBool;
                lastPaidGivenTrade.WasPaid     = togglePaidBool  = !togglePaidBool;
            }

            if (lastTradedInfo is IMutableLastTraderPaidGivenTrade lastTraderTrade) lastTraderTrade.TraderName = "NewTraderName " + i;
        }
    }

    private void SetupLevel2Quote(IMutableLevel2Quote? level2Quote, uint batchId)
    {
        if (level2Quote == null) return;
        SetupOrderBook(level2Quote.OrderBook, batchId);
    }

    public void SetupOrderBook(IMutableOrderBook orderBook, uint batchId)
    {
        var numLayers             = orderBook.MaxPublishDepth;
        var totalOpenInterest = orderBook.OpenInterest!;
        totalOpenInterest.DataSource = batchId % 2 == 0 ? MarketDataSource.Adapter : MarketDataSource.Venue;
        totalOpenInterest.UpdateTime  = new DateTime(2017, 5, 7, 9, 40, 11).AddSeconds(batchId);
        totalOpenInterest.Volume     = batchId * 100_000;
        totalOpenInterest.Vwap       = 0.791905m;

        var bidOpenInterest = orderBook.BidSide.OpenInterestSide!;
        bidOpenInterest.DataSource = batchId % 2 == 0 ? MarketDataSource.Adapter : MarketDataSource.Venue;
        bidOpenInterest.UpdateTime = new DateTime(2017, 5, 7, 9, 20, 11).AddSeconds(batchId);
        bidOpenInterest.Volume     = batchId * 50_000;
        bidOpenInterest.Vwap       = 0.790905m;

        var askOpenInterest = orderBook.AskSide.OpenInterestSide!;
        askOpenInterest.DataSource = batchId % 2 == 0 ? MarketDataSource.Adapter : MarketDataSource.Venue;
        askOpenInterest.UpdateTime  = new DateTime(2017, 5, 7, 9, 30, 11).AddSeconds(batchId);
        askOpenInterest.Volume     = batchId * 50_000;
        askOpenInterest.Vwap       = 0.792905m;

        orderBook.DailyTickUpdateCount = batchId + 1;
        Assert.IsTrue(numLayers >= 1);
        for (var i = 0; i < numLayers; i++)
        {
            var deltaPrice  = 0.00001m * i;
            var deltaVolume = batchId * 100_000 + 100_000m * i;
            var mutableBid  = orderBook.BidSide[i]!;
            var mutableAsk  = orderBook.AskSide[i]!;
            mutableBid.Price  = 0.791905m + batchId * 0.00001m - deltaPrice;
            mutableBid.Volume = 100_000 + deltaVolume;
            mutableAsk.Price  = 0.791906m + batchId * 0.00001m + deltaPrice;
            mutableAsk.Volume = 100_0000 + deltaVolume;

            if (mutableBid is IMutableSourcePriceVolumeLayer mutableBidPriceVal)
                SetupSourceNameOnLayer(mutableBidPriceVal, orderBook.BidSide, batchId);
            if (mutableAsk is IMutableSourcePriceVolumeLayer mutableAskPriceVal)
                SetupSourceNameOnLayer(mutableAskPriceVal, orderBook.AskSide, batchId);
            if (mutableBid is IMutableSourceQuoteRefPriceVolumeLayer mutableBidSrcQuotRef)
                SetupSourceQuoteRefOnLayer(mutableBidSrcQuotRef, true, batchId);
            if (mutableAsk is IMutableSourceQuoteRefPriceVolumeLayer mutableAskSrcQuotRef)
                SetupSourceQuoteRefOnLayer(mutableAskSrcQuotRef, false, batchId);
            if (mutableBid is IMutableValueDatePriceVolumeLayer mutableBidValueDate) SetupValueDateOnLayer(mutableBidValueDate, true, batchId);
            if (mutableAsk is IMutableValueDatePriceVolumeLayer mutableAskValueDate) SetupValueDateOnLayer(mutableAskValueDate, false, batchId);
            if (mutableBid is IMutableOrdersCountPriceVolumeLayer mutableBidOrdersCount)
                SetupOrdersCountOnLayer(mutableBidOrdersCount, true, batchId);
            if (mutableAsk is IMutableOrdersCountPriceVolumeLayer mutableAskOrdersCount)
                SetupOrdersCountOnLayer(mutableAskOrdersCount, false, batchId);
            if (mutableBid is IMutableOrdersPriceVolumeLayer mutableBidAnonOrders) SetupAnonymousOrdersOnLayer(mutableBidAnonOrders, true, batchId);
            if (mutableAsk is IMutableOrdersPriceVolumeLayer mutableAskAnonOrders) SetupAnonymousOrdersOnLayer(mutableAskAnonOrders, false, batchId);
            if (mutableBid is IMutableOrdersPriceVolumeLayer mutableBidCounterPartyOrders
             && mutableBidCounterPartyOrders.LayerType.SupportsOrdersFullPriceVolume())
                SetupCounterPartyOrdersOnLayer(mutableBidCounterPartyOrders, orderBook.BidSide, batchId);
            if (mutableAsk is IMutableOrdersPriceVolumeLayer mutableAskCounterPartyOrders
             && mutableAskCounterPartyOrders.LayerType.SupportsOrdersFullPriceVolume())
                SetupCounterPartyOrdersOnLayer(mutableAskCounterPartyOrders, orderBook.AskSide, batchId);
        }
    }

    private void SetupValueDateOnLayer(IMutableValueDatePriceVolumeLayer layer, bool isBidBook, uint batchId)
    {
        // can only transmit to hour fidelity.  It's only supposed to be Date not DateTime.
        layer.ValueDate = new DateTime(2017, 11, 26, 18, 0, 0).AddDays(batchId);
    }

    private void SetupOrdersCountOnLayer(IMutableOrdersCountPriceVolumeLayer? layer, bool isBidBook, uint batchId)
    {
        if (layer == null) return;
        layer.OrdersCount    = 2;
        layer.InternalVolume = (batchId + 1) * 25600;
    }

    private void SetupAnonymousOrdersOnLayer(IMutableOrdersPriceVolumeLayer? layer, bool isBidBook, uint batchId)
    {
        if (layer == null) return;
        for (var i = 0; i < 2; i++)
        {
            var sequenceIdTimeSpan = TimeSpan.FromSeconds(batchId * 10);
            var updateTimeSpan     = TimeSpan.FromMinutes(batchId * 10);
            var anonOrderLayer     = layer[i]!;
            anonOrderLayer.StateReset();
            anonOrderLayer.OrderId              = (int)(batchId * 10 + i * 1000 + batchId);
            anonOrderLayer.OrderFlags           = ((uint)(batchId * 10 + i * 1000 + batchId)).ToLayerOrderFlags();
            anonOrderLayer.CreatedTime          = new DateTime(2017, 07, 16, 15, 49, 20).Add(sequenceIdTimeSpan);
            anonOrderLayer.UpdatedTime          = new DateTime(2017, 07, 16, 15, 49, 20).Add(updateTimeSpan);
            anonOrderLayer.OrderVolume          = batchId * 25600 + (i + 1) * 100;
            anonOrderLayer.OrderRemainingVolume = batchId * 25600 + (i + 1) * 100;
        }
    }

    private void SetupCounterPartyOrdersOnLayer(IMutableOrdersPriceVolumeLayer? layer, IMutableOrderBookSide orderBookSide, uint batchId)
    {
        if (layer == null) return;
        if (layer is IPQOrdersPriceVolumeLayer pqOrdersPriceVolumeLayer && orderBookSide is IPQOrderBookSide pqOrderBook)
            pqOrdersPriceVolumeLayer.NameIdLookup = pqOrderBook.NameIdLookup;
        for (var i = 0; i < 2; i++)
        {
            ((IMutableCounterPartyOrderLayerInfo)layer[i]!).ExternalCounterPartyName
                = (orderBookSide.BookSide == BookSide.BidBook ? "B" : "A") + "CPN" + (batchId * 20 + i);
            ((IMutableCounterPartyOrderLayerInfo)layer[i]!).ExternalTraderName
                = (orderBookSide.BookSide == BookSide.BidBook ? "B" : "A") + "TN" + (batchId * 5 + i);
        }
    }

    private void SetupSourceQuoteRefOnLayer(IMutableSourceQuoteRefPriceVolumeLayer? layer, bool isBidBook, uint batchId)
    {
        if (layer == null) return;
        layer.SourceQuoteReference = (isBidBook ? 1000u : 5000u) + batchId;
    }

    private void SetupSourceNameOnLayer(IMutableSourcePriceVolumeLayer? layer, IMutableOrderBookSide orderBookSide, uint batchId)
    {
        if (layer == null) return;
        if (layer is IPQSourcePriceVolumeLayer pqSourcePriceVolumeLayer && orderBookSide is IPQOrderBookSide pqOrderBook)
            pqSourcePriceVolumeLayer.NameIdLookup = pqOrderBook.NameIdLookup;
        layer.SourceName = "TestSourceName" + (orderBookSide.BookSide == BookSide.BidBook ? "_Bid_" : "_Ask_") + batchId;
        layer.Executable = true;
    }

    private void SetupLevel1Quote(IMutableLevel1Quote? level1Quote, uint batchId)
    {
        if (level1Quote == null) return;
        level1Quote.Executable = true;
        var sequenceIdTimeSpan = TimeSpan.FromSeconds(batchId * 10);

        level1Quote.SourceAskTime       = new DateTime(2017, 07, 16, 15, 49, 11).Add(sequenceIdTimeSpan);
        level1Quote.SourceBidTime       = new DateTime(2017, 07, 16, 15, 49, 21).Add(sequenceIdTimeSpan);
        level1Quote.ValidFrom           = new DateTime(2017, 07, 16, 15, 49, 11).Add(sequenceIdTimeSpan);
        level1Quote.ValidTo             = new DateTime(2017, 07, 16, 15, 49, 41).Add(sequenceIdTimeSpan);
        level1Quote.BidPriceTop         = 0.791905m + batchId * 0.00001m;
        level1Quote.AskPriceTop         = 0.791906m + batchId * 0.00001m;

        level1Quote.IsBidPriceTopChanged = lastBidTop != level1Quote.BidPriceTop;
        level1Quote.IsAskPriceTopChanged = lastAskTop != level1Quote.AskPriceTop;

        lastAskTop = level1Quote.AskPriceTop;
        lastBidTop = level1Quote.BidPriceTop;

        
        if (level1Quote is IMutablePublishableLevel1Quote pubL1Quote)
        {
            pubL1Quote.AdapterReceivedTime = new DateTime(2017, 07, 16, 15, 49, 31).Add(sequenceIdTimeSpan);
            pubL1Quote.AdapterSentTime     = new DateTime(2017, 07, 16, 15, 49, 41).Add(sequenceIdTimeSpan);
            switch (pubL1Quote.ConflatedTicksCandle)
            {
                case null when level1Quote is PQPublishableLevel1Quote: pubL1Quote.ConflatedTicksCandle = new PQCandle(); break;
                case null:                                              pubL1Quote.ConflatedTicksCandle = new Candle(); break;
            }

            InitalizeCandle(pubL1Quote.ConflatedTicksCandle, batchId);
        }
    }

    private void SetupTickInstant(IMutableTickInstant tickInstant, uint batchId)
    {
        tickInstant.SingleTickValue    = 0.785681m + batchId * 0.00001m;
        tickInstant.SourceTime         = new DateTime(2017, 07, 16, 15, 46, 00).Add(TimeSpan.FromSeconds(batchId * 10));
        tickInstant.IsReplay           = true;
        if (tickInstant is IMutablePublishableTickInstant pubTickInstant)
        {
            pubTickInstant.ClientReceivedTime = tickInstant.SourceTime.Add(TimeSpan.FromMilliseconds(batchId * 10));
        }
        if (tickInstant is IPQPublishableTickInstant pqTickInstant)
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
