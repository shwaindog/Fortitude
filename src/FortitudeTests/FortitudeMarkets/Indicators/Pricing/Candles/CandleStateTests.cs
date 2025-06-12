// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Configuration;
using FortitudeMarkets.Indicators.Pricing.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;
using static FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Candles.CandleTests;

#endregion

namespace FortitudeTests.FortitudeMarkets.Indicators.Pricing.Candles;

[TestClass]
public class CandleStateTests
{
    private readonly ISourceTickerInfo tickerInfo = new SourceTickerInfo
        (1, "SourceName", 1, "TickerName", Level1Quote, MarketClassification.Unknown
       , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 10m, 100m, 10m);

    private List<IPublishableLevel1Quote> alternatingLevel1Quotes = null!;

    private List<ICandle> alternatingPeriodSummaries = null!;

    private decimal   highLowSpread;
    private decimal   mid1;
    private decimal   mid2;
    private IRecycler recycler = null!;
    private DateTime  seedDateTime;
    private decimal   spread;

    [TestInitialize]
    public void Setup()
    {
        recycler     = new Recycler();
        seedDateTime = new DateTime(2024, 6, 19);

        var time = seedDateTime;
        var tsp  = FiveMinutes;

        mid1 = 0.5000m;
        mid2 = 1.0000m;

        spread        = 0.2000m;
        highLowSpread = 0.8000m;

        alternatingPeriodSummaries = new List<ICandle>
        {
            CreateCandle(tsp, time, mid1, spread, highLowSpread)
          , CreateCandle(tsp, time = tsp.PeriodEnd(time), mid2, highLowSpread)
          , CreateCandle(tsp, time = tsp.PeriodEnd(time), mid1, spread, highLowSpread)
          , CreateCandle(tsp, time = tsp.PeriodEnd(time), mid2, spread, highLowSpread)
          , CreateCandle(tsp, time = tsp.PeriodEnd(time), mid1, spread, highLowSpread)
          , CreateCandle(tsp, time = tsp.PeriodEnd(time), mid2, spread, highLowSpread)
          , CreateCandle(tsp, tsp.PeriodEnd(time), mid1, spread, highLowSpread)
        };

        tsp  = OneMinute;
        time = tsp.PreviousPeriodStart(seedDateTime);

        alternatingLevel1Quotes = new List<IPublishableLevel1Quote>();
        for (var i = 0; i < alternatingPeriodSummaries.Count * 5 + 15; i++)
            alternatingLevel1Quotes.Add(tickerInfo.CreatePublishableLevel1Quote(time = tsp.PeriodEnd(time), i % 2 == 0 ? mid1 : mid2, spread));
    }

    [TestMethod]
    [Timeout(20_000)]
    public void OneSummaryAndTwoTicks_BuildCandle_ReturnsExpectedResults()
    {
        var candleState = new CandleState(seedDateTime, TenMinutes);

        var firstSummary = alternatingPeriodSummaries.First();
        var firstQuote   = alternatingLevel1Quotes.Skip(5).First().ToSummary();
        var secondQuote  = alternatingLevel1Quotes.Skip(6).First().ToSummary();

        candleState.SubCandlesPeriods.AddFirst(firstSummary);
        candleState.SubCandlesPeriods.AddLast(firstQuote);
        candleState.SubCandlesPeriods.AddLast(secondQuote);

        var openBid  = firstSummary.StartBidAsk.BidPrice;
        var openAsk  = firstSummary.StartBidAsk.AskPrice;
        var highBid  = firstQuote.HighestBidAsk.BidPrice;
        var highAsk  = firstQuote.HighestBidAsk.AskPrice;
        var lowBid   = firstSummary.LowestBidAsk.BidPrice;
        var lowAsk   = firstSummary.LowestBidAsk.AskPrice;
        var closeBid = secondQuote.EndBidAsk.BidPrice;
        var closeAsk = secondQuote.EndBidAsk.AskPrice;

        var oneMinuteMs   = (decimal)TimeSpan.FromMinutes(1).TotalMilliseconds;
        var fiveMinuteMs  = (decimal)TimeSpan.FromMinutes(5).TotalMilliseconds;
        var sevenMinuteMs = (decimal)TimeSpan.FromMinutes(7).TotalMilliseconds;

        var averageBid =
            (firstSummary.AverageBidAsk.BidPrice * fiveMinuteMs + firstQuote.AverageBidAsk.BidPrice * oneMinuteMs
                                                                + secondQuote.AverageBidAsk.BidPrice * oneMinuteMs) / sevenMinuteMs;
        var averageAsk =
            (firstSummary.AverageBidAsk.AskPrice * fiveMinuteMs + firstQuote.AverageBidAsk.AskPrice * oneMinuteMs
                                                                + secondQuote.AverageBidAsk.AskPrice * oneMinuteMs) / sevenMinuteMs;

        var builtSummary = candleState.BuildCandle(recycler, alternatingLevel1Quotes.Skip(7).First().SourceTime);

        Assert.AreEqual(openBid, builtSummary.StartBidAsk.BidPrice);
        Assert.AreEqual(openAsk, builtSummary.StartBidAsk.AskPrice);
        Assert.AreEqual(highBid, builtSummary.HighestBidAsk.BidPrice);
        Assert.AreEqual(highAsk, builtSummary.HighestBidAsk.AskPrice);
        Assert.AreEqual(lowBid, builtSummary.LowestBidAsk.BidPrice);
        Assert.AreEqual(lowAsk, builtSummary.LowestBidAsk.AskPrice);
        Assert.AreEqual(closeBid, builtSummary.EndBidAsk.BidPrice);
        Assert.AreEqual(closeAsk, builtSummary.EndBidAsk.AskPrice);
        Assert.AreEqual(averageBid, builtSummary.AverageBidAsk.BidPrice);
        Assert.AreEqual(averageAsk, builtSummary.AverageBidAsk.AskPrice);
    }

    [TestMethod]
    [Timeout(20_000)]
    public void ThreeSummaries_BuildCandle_ReturnsExpectedResults()
    {
        var candleState = new CandleState(seedDateTime, FifteenMinutes);

        var firstSummary  = alternatingPeriodSummaries.First();
        var secondSummary = alternatingPeriodSummaries.Skip(1).First();
        var thirdSummary  = alternatingPeriodSummaries.Skip(2).First();

        candleState.SubCandlesPeriods.AddFirst(firstSummary);
        candleState.SubCandlesPeriods.AddLast(secondSummary);
        candleState.SubCandlesPeriods.AddLast(thirdSummary);

        var openBid  = firstSummary.StartBidAsk.BidPrice;
        var openAsk  = firstSummary.StartBidAsk.AskPrice;
        var highBid  = secondSummary.HighestBidAsk.BidPrice;
        var highAsk  = secondSummary.HighestBidAsk.AskPrice;
        var lowBid   = firstSummary.LowestBidAsk.BidPrice;
        var lowAsk   = firstSummary.LowestBidAsk.AskPrice;
        var closeBid = thirdSummary.EndBidAsk.BidPrice;
        var closeAsk = thirdSummary.EndBidAsk.AskPrice;

        var fiveMinuteMs    = (decimal)TimeSpan.FromMinutes(5).TotalMilliseconds;
        var fifteenMinuteMs = (decimal)TimeSpan.FromMinutes(15).TotalMilliseconds;

        var averageBid =
            (firstSummary.AverageBidAsk.BidPrice * fiveMinuteMs + secondSummary.AverageBidAsk.BidPrice * fiveMinuteMs
                                                                + thirdSummary.AverageBidAsk.BidPrice * fiveMinuteMs) / fifteenMinuteMs;
        var averageAsk =
            (firstSummary.AverageBidAsk.AskPrice * fiveMinuteMs + secondSummary.AverageBidAsk.AskPrice * fiveMinuteMs
                                                                + thirdSummary.AverageBidAsk.AskPrice * fiveMinuteMs) / fifteenMinuteMs;

        var builtSummary = candleState.BuildCandle(recycler, thirdSummary.PeriodEndTime);

        Assert.AreEqual(openBid, builtSummary.StartBidAsk.BidPrice);
        Assert.AreEqual(openAsk, builtSummary.StartBidAsk.AskPrice);
        Assert.AreEqual(highBid, builtSummary.HighestBidAsk.BidPrice);
        Assert.AreEqual(highAsk, builtSummary.HighestBidAsk.AskPrice);
        Assert.AreEqual(lowBid, builtSummary.LowestBidAsk.BidPrice);
        Assert.AreEqual(lowAsk, builtSummary.LowestBidAsk.AskPrice);
        Assert.AreEqual(closeBid, builtSummary.EndBidAsk.BidPrice);
        Assert.AreEqual(closeAsk, builtSummary.EndBidAsk.AskPrice);
        Assert.AreEqual(averageBid, builtSummary.AverageBidAsk.BidPrice);
        Assert.AreEqual(averageAsk, builtSummary.AverageBidAsk.AskPrice);
    }

    [TestMethod]
    [Timeout(20_000)]
    public void SingleTick_BuildCandle_ReturnsExpectedResults()
    {
        var candleState = new CandleState(seedDateTime, FifteenMinutes);

        var firstQuote = alternatingLevel1Quotes.First().ToSummary();

        candleState.SubCandlesPeriods.AddFirst(firstQuote);

        var openBid    = firstQuote.StartBidAsk.BidPrice;
        var openAsk    = firstQuote.StartBidAsk.AskPrice;
        var highBid    = firstQuote.HighestBidAsk.BidPrice;
        var highAsk    = firstQuote.HighestBidAsk.AskPrice;
        var lowBid     = firstQuote.LowestBidAsk.BidPrice;
        var lowAsk     = firstQuote.LowestBidAsk.AskPrice;
        var closeBid   = firstQuote.EndBidAsk.BidPrice;
        var closeAsk   = firstQuote.EndBidAsk.AskPrice;
        var averageBid = firstQuote.AverageBidAsk.BidPrice;
        var averageAsk = firstQuote.AverageBidAsk.AskPrice;

        var builtSummary = candleState.BuildCandle(recycler, alternatingLevel1Quotes.Skip(7).First().SourceTime);

        Assert.AreEqual(openBid, builtSummary.StartBidAsk.BidPrice);
        Assert.AreEqual(openAsk, builtSummary.StartBidAsk.AskPrice);
        Assert.AreEqual(highBid, builtSummary.HighestBidAsk.BidPrice);
        Assert.AreEqual(highAsk, builtSummary.HighestBidAsk.AskPrice);
        Assert.AreEqual(lowBid, builtSummary.LowestBidAsk.BidPrice);
        Assert.AreEqual(lowAsk, builtSummary.LowestBidAsk.AskPrice);
        Assert.AreEqual(closeBid, builtSummary.EndBidAsk.BidPrice);
        Assert.AreEqual(closeAsk, builtSummary.EndBidAsk.AskPrice);
        Assert.AreEqual(averageBid, builtSummary.AverageBidAsk.BidPrice);
        Assert.AreEqual(averageAsk, builtSummary.AverageBidAsk.AskPrice);
    }

    [TestMethod]
    [Timeout(20_000)]
    public void TwoOneMinAndOneEightMinuteTick_BuildCandle_TimeWeightedAverageIsOnTheLongestLiveTick()
    {
        var candleState = new CandleState(seedDateTime, TenMinutes);

        var firstQuote    = alternatingLevel1Quotes.First().ToSummary();
        var secondQuote   = alternatingLevel1Quotes.Skip(1).First().ToSummary();
        var lastTickQuote = alternatingLevel1Quotes.Skip(9).First().ToSummary();

        candleState.SubCandlesPeriods.AddFirst(firstQuote);
        candleState.SubCandlesPeriods.AddLast(secondQuote);
        candleState.SubCandlesPeriods.AddLast(lastTickQuote);

        var openBid  = firstQuote.StartBidAsk.BidPrice;
        var openAsk  = firstQuote.StartBidAsk.AskPrice;
        var highBid  = secondQuote.HighestBidAsk.BidPrice;
        var highAsk  = secondQuote.HighestBidAsk.AskPrice;
        var lowBid   = firstQuote.LowestBidAsk.BidPrice;
        var lowAsk   = firstQuote.LowestBidAsk.AskPrice;
        var closeBid = lastTickQuote.EndBidAsk.BidPrice;
        var closeAsk = lastTickQuote.EndBidAsk.AskPrice;

        var oneMinuteMs   = (decimal)TimeSpan.FromMinutes(1).TotalMilliseconds;
        var eightMinuteMs = (decimal)TimeSpan.FromMinutes(8).TotalMilliseconds;
        var tenMinuteMs   = (decimal)TimeSpan.FromMinutes(10).TotalMilliseconds;

        var averageBid =
            (firstQuote.AverageBidAsk.BidPrice * oneMinuteMs + lastTickQuote.AverageBidAsk.BidPrice * oneMinuteMs
                                                             + secondQuote.AverageBidAsk.BidPrice * eightMinuteMs) / tenMinuteMs;
        var averageAsk =
            (firstQuote.AverageBidAsk.AskPrice * oneMinuteMs + lastTickQuote.AverageBidAsk.AskPrice * oneMinuteMs
                                                             + secondQuote.AverageBidAsk.AskPrice * eightMinuteMs) / tenMinuteMs;

        var builtSummary = candleState.BuildCandle(recycler, alternatingLevel1Quotes.Skip(10).First().SourceTime);

        Assert.AreEqual(openBid, builtSummary.StartBidAsk.BidPrice);
        Assert.AreEqual(openAsk, builtSummary.StartBidAsk.AskPrice);
        Assert.AreEqual(highBid, builtSummary.HighestBidAsk.BidPrice);
        Assert.AreEqual(highAsk, builtSummary.HighestBidAsk.AskPrice);
        Assert.AreEqual(lowBid, builtSummary.LowestBidAsk.BidPrice);
        Assert.AreEqual(lowAsk, builtSummary.LowestBidAsk.AskPrice);
        Assert.AreEqual(closeBid, builtSummary.EndBidAsk.BidPrice);
        Assert.AreEqual(closeAsk, builtSummary.EndBidAsk.AskPrice);
        Assert.AreEqual(averageBid, builtSummary.AverageBidAsk.BidPrice);
        Assert.AreEqual(averageAsk, builtSummary.AverageBidAsk.AskPrice);
    }

    [TestMethod]
    [Timeout(20_000)]
    public void OneLastHalfSummary_BuildCandle_SummaryFlagsIncompleteStartHalfSet()
    {
        var candleState = new CandleState(seedDateTime, TenMinutes);

        var firstSummary = alternatingPeriodSummaries.Skip(1).First();

        candleState.SubCandlesPeriods.AddFirst(firstSummary);

        var openBid  = firstSummary.StartBidAsk.BidPrice;
        var openAsk  = firstSummary.StartBidAsk.AskPrice;
        var highBid  = firstSummary.HighestBidAsk.BidPrice;
        var highAsk  = firstSummary.HighestBidAsk.AskPrice;
        var lowBid   = firstSummary.LowestBidAsk.BidPrice;
        var lowAsk   = firstSummary.LowestBidAsk.AskPrice;
        var closeBid = firstSummary.EndBidAsk.BidPrice;
        var closeAsk = firstSummary.EndBidAsk.AskPrice;

        var averageBid = firstSummary.AverageBidAsk.BidPrice;
        var averageAsk = firstSummary.AverageBidAsk.AskPrice;

        var builtSummary = candleState.BuildCandle(recycler, firstSummary.PeriodEndTime);

        Assert.AreEqual(openBid, builtSummary.StartBidAsk.BidPrice);
        Assert.AreEqual(openAsk, builtSummary.StartBidAsk.AskPrice);
        Assert.AreEqual(highBid, builtSummary.HighestBidAsk.BidPrice);
        Assert.AreEqual(highAsk, builtSummary.HighestBidAsk.AskPrice);
        Assert.AreEqual(lowBid, builtSummary.LowestBidAsk.BidPrice);
        Assert.AreEqual(lowAsk, builtSummary.LowestBidAsk.AskPrice);
        Assert.AreEqual(closeBid, builtSummary.EndBidAsk.BidPrice);
        Assert.AreEqual(closeAsk, builtSummary.EndBidAsk.AskPrice);
        Assert.AreEqual(averageBid, builtSummary.AverageBidAsk.BidPrice);
        Assert.AreEqual(averageAsk, builtSummary.AverageBidAsk.AskPrice);
        Assert.AreEqual(CandleFlags.IncompleteStartMask, builtSummary.CandleFlags);
    }

    [TestMethod]
    [Timeout(20_000)]
    public void OneLastHalfSummaryWithPreviousEndBidAsk_BuildCandle_NoMissingPeriodFlags()
    {
        var candleState = new CandleState(seedDateTime, TenMinutes);

        var previousPeriodBidAsk = alternatingPeriodSummaries.First().EndBidAsk;
        var firstSummary         = alternatingPeriodSummaries.Skip(1).First();

        candleState.PreviousPeriodBidAskEnd = previousPeriodBidAsk;
        candleState.SubCandlesPeriods.AddFirst(firstSummary);

        var openBid  = previousPeriodBidAsk.BidPrice;
        var openAsk  = previousPeriodBidAsk.AskPrice;
        var highBid  = firstSummary.HighestBidAsk.BidPrice;
        var highAsk  = firstSummary.HighestBidAsk.AskPrice;
        var lowBid   = previousPeriodBidAsk.BidPrice;
        var lowAsk   = previousPeriodBidAsk.AskPrice;
        var closeBid = firstSummary.EndBidAsk.BidPrice;
        var closeAsk = firstSummary.EndBidAsk.AskPrice;

        var fiveMinuteMs = (decimal)TimeSpan.FromMinutes(5).TotalMilliseconds;
        var tenMinuteMs  = (decimal)TimeSpan.FromMinutes(10).TotalMilliseconds;

        var averageBid =
            (previousPeriodBidAsk.BidPrice * fiveMinuteMs + firstSummary.AverageBidAsk.BidPrice * fiveMinuteMs) / tenMinuteMs;
        var averageAsk =
            (previousPeriodBidAsk.AskPrice * fiveMinuteMs + firstSummary.AverageBidAsk.AskPrice * fiveMinuteMs) / tenMinuteMs;

        var builtSummary = candleState.BuildCandle(recycler, firstSummary.PeriodEndTime);

        Assert.AreEqual(openBid, builtSummary.StartBidAsk.BidPrice);
        Assert.AreEqual(openAsk, builtSummary.StartBidAsk.AskPrice);
        Assert.AreEqual(highBid, builtSummary.HighestBidAsk.BidPrice);
        Assert.AreEqual(highAsk, builtSummary.HighestBidAsk.AskPrice);
        Assert.AreEqual(lowBid, builtSummary.LowestBidAsk.BidPrice);
        Assert.AreEqual(lowAsk, builtSummary.LowestBidAsk.AskPrice);
        Assert.AreEqual(closeBid, builtSummary.EndBidAsk.BidPrice);
        Assert.AreEqual(closeAsk, builtSummary.EndBidAsk.AskPrice);
        Assert.AreEqual(averageBid, builtSummary.AverageBidAsk.BidPrice);
        Assert.AreEqual(averageAsk, builtSummary.AverageBidAsk.AskPrice);
        Assert.AreEqual(CandleFlags.CreatedFromPreviousEnd, builtSummary.CandleFlags);
    }

    [TestMethod]
    [Timeout(20_000)]
    public void OneFirstHalfSummaryWithPreviousEndBidAsk_BuildCandleHalfWayThroughPeriod_NoMissingPeriodFlagsPeriodLatest()
    {
        var candleState = new CandleState(seedDateTime, TenMinutes);

        var firstSummary = alternatingPeriodSummaries.First();

        candleState.SubCandlesPeriods.AddFirst(firstSummary);

        var openBid  = firstSummary.StartBidAsk.BidPrice;
        var openAsk  = firstSummary.StartBidAsk.AskPrice;
        var highBid  = firstSummary.HighestBidAsk.BidPrice;
        var highAsk  = firstSummary.HighestBidAsk.AskPrice;
        var lowBid   = firstSummary.LowestBidAsk.BidPrice;
        var lowAsk   = firstSummary.LowestBidAsk.AskPrice;
        var closeBid = firstSummary.EndBidAsk.BidPrice;
        var closeAsk = firstSummary.EndBidAsk.AskPrice;

        var averageBid = firstSummary.AverageBidAsk.BidPrice;
        var averageAsk = firstSummary.AverageBidAsk.AskPrice;

        var builtSummary = candleState.BuildCandle(recycler, firstSummary.PeriodEndTime);

        Assert.AreEqual(openBid, builtSummary.StartBidAsk.BidPrice);
        Assert.AreEqual(openAsk, builtSummary.StartBidAsk.AskPrice);
        Assert.AreEqual(highBid, builtSummary.HighestBidAsk.BidPrice);
        Assert.AreEqual(highAsk, builtSummary.HighestBidAsk.AskPrice);
        Assert.AreEqual(lowBid, builtSummary.LowestBidAsk.BidPrice);
        Assert.AreEqual(lowAsk, builtSummary.LowestBidAsk.AskPrice);
        Assert.AreEqual(closeBid, builtSummary.EndBidAsk.BidPrice);
        Assert.AreEqual(closeAsk, builtSummary.EndBidAsk.AskPrice);
        Assert.AreEqual(averageBid, builtSummary.AverageBidAsk.BidPrice);
        Assert.AreEqual(averageAsk, builtSummary.AverageBidAsk.AskPrice);
        Assert.AreEqual(CandleFlags.PeriodLatest, builtSummary.CandleFlags);
    }
}
