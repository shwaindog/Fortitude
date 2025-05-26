// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Generators.Quotes.LastTraded;

public struct GenerateLastTradeInfo
{
    public GenerateLastTradeInfo() { }

    public bool GenerateLastTrades = true;

    public double AverageLastTradeProbability = 0.10;

    public double LastTradedWasBidProbability = 0.5;

    public int MaxNumberOfLastTrade = 10;

    public int AverageNumberOfLastTrades = 3;

    public int TradeCountStdDeviation = 3;

    public int MaxDifferentTraderNames = 50;
}

public interface ILastTradedGenerator
{
    void PopulateLevel3LastTraded(IMutablePublishableLevel3Quote level3Quote, MidPriceTimePair midPriceTimePair);
}

public class LastTradedGenerator : ILastTradedGenerator
{
    protected readonly GenerateLastTradeInfo GenerateLastTradeInfo;

    private int    askCount;
    private int    bidCount;
    private Normal normalDist   = null!;
    private Random pseudoRandom = null!;

    public LastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo) => GenerateLastTradeInfo = generateLastTradeInfo;

    public void PopulateLevel3LastTraded(IMutablePublishableLevel3Quote level3Quote, MidPriceTimePair midPriceTimePair)
    {
        if (!GenerateLastTradeInfo.GenerateLastTrades) return;
        var prev = midPriceTimePair.PreviousMid;
        var curr = midPriceTimePair.CurrentMid;
        bidCount = 0;
        askCount = 0;

        pseudoRandom = new Random(prev.Mid.GetHashCode() ^ curr.Mid.GetHashCode());
        normalDist   = new Normal(0, 1, pseudoRandom);

        var generateThisQuote = pseudoRandom.NextDouble() < GenerateLastTradeInfo.AverageLastTradeProbability;
        if (!generateThisQuote) return;
        var numberOfLastTrades =
            Math.Min
                (GenerateLastTradeInfo.MaxNumberOfLastTrade,
                 Math.Max(0, (int)(GenerateLastTradeInfo.AverageNumberOfLastTrades
                                 + normalDist.Sample() * GenerateLastTradeInfo.TradeCountStdDeviation)));
        if (numberOfLastTrades == 0) return;

        var lastTradeType = level3Quote.OnTickLastTraded!.LastTradesOfType;

        switch (lastTradeType)
        {
            case LastTradeType.None:                   break;
            case LastTradeType.Price:                  PopulateLastTrades(level3Quote, numberOfLastTrades, midPriceTimePair); break;
            case LastTradeType.PricePaidOrGivenVolume: PopulateLastPaidGivenTrades(level3Quote, numberOfLastTrades, midPriceTimePair); break;
            case LastTradeType.PriceLastTraderPaidOrGivenVolume:
            case LastTradeType.PriceLastTraderName:
            default:
                PopulateLastTraderPaidGivenTrades(level3Quote, numberOfLastTrades, midPriceTimePair);
                break;
        }
    }

    protected virtual void PopulateLastTrades
        (IMutablePublishableLevel3Quote level3Quote, int numberOfTrades, MidPriceTimePair midPriceTimePair)
    {
        var recentlyTraded = level3Quote.OnTickLastTraded!;
        InitializeRecentlyTraded(recentlyTraded);

        for (var i = 0; i < numberOfTrades && i < GenerateLastTradeInfo.MaxNumberOfLastTrade; i++)
        {
            var currentLastTradeCount = recentlyTraded.Count;
            if (currentLastTradeCount >= GenerateLastTradeInfo.MaxNumberOfLastTrade || currentLastTradeCount >= i) break;
            var index        = recentlyTraded.AppendEntryAtEnd();
            var lastTradInfo = recentlyTraded[index]!;
            var isBidTrade   = pseudoRandom.NextDouble() < GenerateLastTradeInfo.LastTradedWasBidProbability;
            PopulateLastTradePriceAndTime(lastTradInfo, isBidTrade, level3Quote, midPriceTimePair);
        }
    }

    protected virtual void InitializeRecentlyTraded(IOnTickLastTraded onTickLastTraded) { }

    protected virtual void PopulateLastTradePriceAndTime
        (IMutableLastTrade lastTrade, bool isBidTrade, IMutableLevel3Quote level3Quote, MidPriceTimePair midPriceTimePair)
    {
        if (isBidTrade)
        {
            var bidBookEntries = level3Quote.BidBook.Count;
            var priceAtIndex   = Math.Min(++bidCount, bidBookEntries);
            if (priceAtIndex <= 0) return;
            var bidBookPrice = level3Quote.BidBook[priceAtIndex - 1]!;
            lastTrade.TradePrice = bidBookPrice.Price;
            lastTrade.TradeTime  = midPriceTimePair.CurrentMid.Time - TimeSpan.FromTicks(bidCount * 777);
        }
        else
        {
            var askBookEntries = level3Quote.AskBook.Count;
            var priceAtIndex   = Math.Min(++askCount, askBookEntries);
            if (priceAtIndex <= 0) return;
            var askBookPriceEntry = level3Quote.AskBook[priceAtIndex - 1]!;
            lastTrade.TradePrice = askBookPriceEntry.Price;
            lastTrade.TradeTime  = midPriceTimePair.CurrentMid.Time - TimeSpan.FromTicks(askCount * 689);
        }
    }

    protected virtual void PopulateLastPaidGivenTrades
        (IMutablePublishableLevel3Quote level3Quote, int numberOfTrades, MidPriceTimePair midPriceTimePair)
    {
        var recentlyTraded = level3Quote.OnTickLastTraded!;

        for (var i = 0; i < numberOfTrades && i < GenerateLastTradeInfo.MaxNumberOfLastTrade; i++)
        {
            var currentLastTradeCount = recentlyTraded.Count;
            if (currentLastTradeCount >= GenerateLastTradeInfo.MaxNumberOfLastTrade || currentLastTradeCount >= i) break;
            var index        = recentlyTraded.AppendEntryAtEnd();
            var lastTradInfo = (IMutableLastPaidGivenTrade)recentlyTraded[index]!;
            var isBidTrade   = pseudoRandom.NextDouble() < GenerateLastTradeInfo.LastTradedWasBidProbability;
            PopulateLastPaidGivenVolume(lastTradInfo, isBidTrade, level3Quote, midPriceTimePair);
        }
    }


    protected virtual void PopulateLastPaidGivenVolume
    (IMutableLastPaidGivenTrade lastTradePaidGiven, bool isBidTrade, IMutableLevel3Quote level3Quote
      , MidPriceTimePair midPriceTimePair)
    {
        if (isBidTrade)
        {
            lastTradePaidGiven.WasPaid  = bidCount == 0;
            lastTradePaidGiven.WasGiven = bidCount > 0;
            var bidBookEntries = level3Quote.BidBook.Count;
            var priceAtIndex   = Math.Min(++bidCount, bidBookEntries);
            if (priceAtIndex <= 0) return;
            var bidBookPrice = level3Quote.BidBook[priceAtIndex - 1]!;
            lastTradePaidGiven.TradeVolume = bidBookPrice.Volume;
            PopulateLastTradePriceAndTime(lastTradePaidGiven, true, level3Quote, midPriceTimePair);
        }
        else
        {
            lastTradePaidGiven.WasGiven = askCount == 0;
            lastTradePaidGiven.WasPaid  = askCount > 0;
            var askBookEntries = level3Quote.AskBook.Count;
            var priceAtIndex   = Math.Min(++askCount, askBookEntries);
            if (priceAtIndex <= 0) return;
            var askBookPriceEntry = level3Quote.AskBook[priceAtIndex - 1]!;
            lastTradePaidGiven.TradeVolume = askBookPriceEntry.Volume;
            PopulateLastTradePriceAndTime(lastTradePaidGiven, false, level3Quote, midPriceTimePair);
        }
    }

    protected virtual void PopulateLastTraderPaidGivenTrades
        (IMutablePublishableLevel3Quote level3Quote, int numberOfTrades, MidPriceTimePair midPriceTimePair)
    {
        var recentlyTraded = level3Quote.OnTickLastTraded!;

        for (var i = 0; i < numberOfTrades && i < GenerateLastTradeInfo.MaxNumberOfLastTrade; i++)
        {
            var currentLastTradeCount = recentlyTraded.Count;
            if (currentLastTradeCount >= GenerateLastTradeInfo.MaxNumberOfLastTrade || currentLastTradeCount >= i) break;
            var index        = recentlyTraded.AppendEntryAtEnd();
            var lastTradInfo = (IMutableLastExternalCounterPartyTrade)recentlyTraded[index]!;
            var isBidTrade   = pseudoRandom.NextDouble() < GenerateLastTradeInfo.LastTradedWasBidProbability;

            var traderNum = pseudoRandom.Next(1, GenerateLastTradeInfo.MaxDifferentTraderNames + 1);

            SetTraderName(lastTradInfo, $"TraderName_{traderNum}");
            PopulateLastPaidGivenVolume(lastTradInfo, isBidTrade, level3Quote, midPriceTimePair);
        }
    }

    protected virtual void SetTraderName(IMutableLastExternalCounterPartyTrade lastExternalCounterPartyTrade, string traderName)
    {
        lastExternalCounterPartyTrade.ExternalTraderName = traderName;
    }
}
