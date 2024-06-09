// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.Generators.LastTraded;

public struct GenerateLastTradeInfo
{
    public GenerateLastTradeInfo() { }

    public bool GenerateLastTrades = true;

    public double AverageLastTradeOneProbability = 0.10;

    public double LastTradedWasBidProbability = 0.5;

    public int MaxNumberOfLastTrade = 10;

    public int AverageNumberOfLastTrades = 3;

    public int TradeCountStdDeviation = 3;

    public int MaxDifferentTraderNames = 50;
}

public interface ILastTradedGenerator
{
    void PopulateLevel3LastTraded(IMutableLevel3Quote level3Quote, PreviousCurrentMidPriceTime previousCurrentMid);
}

public class LastTradedGenerator : ILastTradedGenerator
{
    protected readonly GenerateLastTradeInfo GenerateLastTradeInfo;

    protected readonly Normal NormalDist;
    protected readonly Random PseudoRandom;
    private            int    askCount;
    private            int    bidCount;

    public LastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo, Normal normalDist, Random pseudoRandom)
    {
        GenerateLastTradeInfo = generateLastTradeInfo;
        NormalDist            = normalDist;
        PseudoRandom          = pseudoRandom;
    }

    public void PopulateLevel3LastTraded(IMutableLevel3Quote level3Quote, PreviousCurrentMidPriceTime previousCurrentMid)
    {
        if (!GenerateLastTradeInfo.GenerateLastTrades) return;
        bidCount = 0;
        askCount = 0;
        var generateThisQuote = PseudoRandom.NextDouble() < GenerateLastTradeInfo.AverageLastTradeOneProbability;
        if (!generateThisQuote) return;
        var numberOfLastTrades = Math.Min(GenerateLastTradeInfo.MaxNumberOfLastTrade,
                                          Math.Max(0,
                                                   (int)(GenerateLastTradeInfo.AverageNumberOfLastTrades +
                                                         NormalDist.Sample() *
                                                         GenerateLastTradeInfo.TradeCountStdDeviation)));
        if (numberOfLastTrades == 0) return;

        var lastTradeType = level3Quote.RecentlyTraded!.LastTradesOfType;

        switch (lastTradeType)
        {
            case LastTradeType.None:
                break;
            case LastTradeType.Price:
                PopulateLastTrades(level3Quote, numberOfLastTrades, previousCurrentMid);
                break;
            case LastTradeType.PricePaidOrGivenVolume:
                PopulateLastPaidGivenTrades(level3Quote, numberOfLastTrades, previousCurrentMid);
                break;
            case LastTradeType.PriceLastTraderPaidOrGivenVolume:
            case LastTradeType.PriceLastTraderName:
            default:
                PopulateLastTraderPaidGivenTrades(level3Quote, numberOfLastTrades, previousCurrentMid);
                break;
        }
    }

    protected virtual void PopulateLastTrades(IMutableLevel3Quote level3Quote, int numberOfTrades
      , PreviousCurrentMidPriceTime previousCurrentMid)
    {
        var recentlyTraded = level3Quote.RecentlyTraded!;
        InitializeRecentlyTraded(recentlyTraded);

        for (var i = 0; i < numberOfTrades && i < GenerateLastTradeInfo.MaxNumberOfLastTrade; i++)
        {
            var currentLastTradeCount = recentlyTraded.Count;
            if (currentLastTradeCount >= GenerateLastTradeInfo.MaxNumberOfLastTrade || currentLastTradeCount >= i) break;
            var index        = recentlyTraded.AppendEntryAtEnd();
            var lastTradInfo = recentlyTraded[index]!;
            var isBidTrade   = PseudoRandom.NextDouble() < GenerateLastTradeInfo.LastTradedWasBidProbability;
            PopulateLastTradePriceAndTime(lastTradInfo, isBidTrade, level3Quote, previousCurrentMid);
        }
    }

    protected virtual void InitializeRecentlyTraded(IRecentlyTraded recentlyTraded) { }

    protected virtual void PopulateLastTradePriceAndTime(IMutableLastTrade lastTrade, bool isBidTrade,
        IMutableLevel3Quote level3Quote, PreviousCurrentMidPriceTime previousCurrentMid)
    {
        if (isBidTrade)
        {
            var bidBookEntries = level3Quote.BidBook.Count;
            var priceAtIndex   = Math.Min(++bidCount, bidBookEntries);
            if (priceAtIndex <= 0) return;
            var bidBookPrice = level3Quote.BidBook[priceAtIndex - 1]!;
            lastTrade.TradePrice = bidBookPrice.Price;
            lastTrade.TradeTime  = previousCurrentMid.CurrentMid.Time - TimeSpan.FromTicks(bidCount * 777);
        }
        else
        {
            var askBookEntries = level3Quote.AskBook.Count;
            var priceAtIndex   = Math.Min(++askCount, askBookEntries);
            if (priceAtIndex <= 0) return;
            var askBookPriceEntry = level3Quote.AskBook[priceAtIndex - 1]!;
            lastTrade.TradePrice = askBookPriceEntry.Price;
            lastTrade.TradeTime  = previousCurrentMid.CurrentMid.Time - TimeSpan.FromTicks(askCount * 689);
        }
    }

    protected virtual void PopulateLastPaidGivenTrades(IMutableLevel3Quote level3Quote, int numberOfTrades
      , PreviousCurrentMidPriceTime previousCurrentMid)
    {
        var recentlyTraded = level3Quote.RecentlyTraded!;

        for (var i = 0; i < numberOfTrades && i < GenerateLastTradeInfo.MaxNumberOfLastTrade; i++)
        {
            var currentLastTradeCount = recentlyTraded.Count;
            if (currentLastTradeCount >= GenerateLastTradeInfo.MaxNumberOfLastTrade || currentLastTradeCount >= i) break;
            var index        = recentlyTraded.AppendEntryAtEnd();
            var lastTradInfo = (IMutableLastPaidGivenTrade)recentlyTraded[index]!;
            var isBidTrade   = PseudoRandom.NextDouble() < GenerateLastTradeInfo.LastTradedWasBidProbability;
            PopulateLastPaidGivenVolume(lastTradInfo, isBidTrade, level3Quote, previousCurrentMid);
        }
    }


    protected virtual void PopulateLastPaidGivenVolume(IMutableLastPaidGivenTrade lastTradePaidGiven, bool isBidTrade,
        IMutableLevel3Quote level3Quote, PreviousCurrentMidPriceTime previousCurrentMid)
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
            PopulateLastTradePriceAndTime(lastTradePaidGiven, true, level3Quote, previousCurrentMid);
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
            PopulateLastTradePriceAndTime(lastTradePaidGiven, false, level3Quote, previousCurrentMid);
        }
    }

    protected virtual void PopulateLastTraderPaidGivenTrades(IMutableLevel3Quote level3Quote, int numberOfTrades
      , PreviousCurrentMidPriceTime previousCurrentMid)
    {
        var recentlyTraded = level3Quote.RecentlyTraded!;

        for (var i = 0; i < numberOfTrades && i < GenerateLastTradeInfo.MaxNumberOfLastTrade; i++)
        {
            var currentLastTradeCount = recentlyTraded.Count;
            if (currentLastTradeCount >= GenerateLastTradeInfo.MaxNumberOfLastTrade || currentLastTradeCount >= i) break;
            var index        = recentlyTraded.AppendEntryAtEnd();
            var lastTradInfo = (IMutableLastTraderPaidGivenTrade)recentlyTraded[index]!;
            var isBidTrade   = PseudoRandom.NextDouble() < GenerateLastTradeInfo.LastTradedWasBidProbability;

            var traderNum = PseudoRandom.Next(1, GenerateLastTradeInfo.MaxDifferentTraderNames + 1);

            SetTraderName(lastTradInfo, $"TraderName_{traderNum}");
            PopulateLastPaidGivenVolume(lastTradInfo, isBidTrade, level3Quote, previousCurrentMid);
        }
    }

    protected virtual void SetTraderName(IMutableLastTraderPaidGivenTrade lastTraderPaidGivenTrade, string traderName)
    {
        lastTraderPaidGivenTrade.TraderName = traderName;
    }
}
