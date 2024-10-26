// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Indicators.Pricing.PeriodSummaries;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Summaries;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;
using static FortitudeTests.FortitudeMarkets.Pricing.Summaries.PricePeriodSummaryTests;

#endregion

namespace FortitudeTests.FortitudeMarkets.Indicators.Pricing.PeriodSummaries;

[TestClass]
public class PeriodSummaryStateTests
{
    private readonly ISourceTickerInfo tickerInfo = new SourceTickerInfo
        (1, "SourceName", 1, "TickerName", Level1Quote, Unknown
       , 1, 0.001m, 10m, 100m, 10m);

    private List<ILevel1Quote> alternatingLevel1Quotes = null!;

    private List<IPricePeriodSummary> alternatingPeriodSummaries = null!;

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

        alternatingPeriodSummaries = new List<IPricePeriodSummary>
        {
            CreatePricePeriodSummary(tsp, time, mid1, spread, highLowSpread)
          , CreatePricePeriodSummary(tsp, time = tsp.PeriodEnd(time), mid2, highLowSpread)
          , CreatePricePeriodSummary(tsp, time = tsp.PeriodEnd(time), mid1, spread, highLowSpread)
          , CreatePricePeriodSummary(tsp, time = tsp.PeriodEnd(time), mid2, spread, highLowSpread)
          , CreatePricePeriodSummary(tsp, time = tsp.PeriodEnd(time), mid1, spread, highLowSpread)
          , CreatePricePeriodSummary(tsp, time = tsp.PeriodEnd(time), mid2, spread, highLowSpread)
          , CreatePricePeriodSummary(tsp, tsp.PeriodEnd(time), mid1, spread, highLowSpread)
        };

        tsp  = OneMinute;
        time = tsp.PreviousPeriodStart(seedDateTime);

        alternatingLevel1Quotes = new List<ILevel1Quote>();
        for (var i = 0; i < alternatingPeriodSummaries.Count * 5 + 15; i++)
            alternatingLevel1Quotes.Add(tickerInfo.CreateLevel1Quote(time = tsp.PeriodEnd(time), i % 2 == 0 ? mid1 : mid2, spread));
    }

    [TestMethod]
    [Timeout(20_000)]
    public void OneSummaryAndTwoTicks_BuildPeriodSummary_ReturnsExpectedResults()
    {
        var periodSummaryState = new PeriodSummaryState(seedDateTime, TenMinutes);

        var firstSummary = alternatingPeriodSummaries.First();
        var firstQuote   = alternatingLevel1Quotes.Skip(5).First().ToSummary();
        var secondQuote  = alternatingLevel1Quotes.Skip(6).First().ToSummary();

        periodSummaryState.SubSummaryPeriods.AddFirst(firstSummary);
        periodSummaryState.SubSummaryPeriods.AddLast(firstQuote);
        periodSummaryState.SubSummaryPeriods.AddLast(secondQuote);

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

        var builtSummary = periodSummaryState.BuildPeriodSummary(recycler, alternatingLevel1Quotes.Skip(7).First().SourceTime);

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
    public void ThreeSummaries_BuildPeriodSummary_ReturnsExpectedResults()
    {
        var periodSummaryState = new PeriodSummaryState(seedDateTime, FifteenMinutes);

        var firstSummary  = alternatingPeriodSummaries.First();
        var secondSummary = alternatingPeriodSummaries.Skip(1).First();
        var thirdSummary  = alternatingPeriodSummaries.Skip(2).First();

        periodSummaryState.SubSummaryPeriods.AddFirst(firstSummary);
        periodSummaryState.SubSummaryPeriods.AddLast(secondSummary);
        periodSummaryState.SubSummaryPeriods.AddLast(thirdSummary);

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

        var builtSummary = periodSummaryState.BuildPeriodSummary(recycler, thirdSummary.PeriodEndTime);

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
    public void SingleTick_BuildPeriodSummary_ReturnsExpectedResults()
    {
        var periodSummaryState = new PeriodSummaryState(seedDateTime, FifteenMinutes);

        var firstQuote = alternatingLevel1Quotes.First().ToSummary();

        periodSummaryState.SubSummaryPeriods.AddFirst(firstQuote);

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

        var builtSummary = periodSummaryState.BuildPeriodSummary(recycler, alternatingLevel1Quotes.Skip(7).First().SourceTime);

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
    public void TwoOneMinAndOneEightMinuteTick_BuildPeriodSummary_TimeWeightedAverageIsOnTheLongestLiveTick()
    {
        var periodSummaryState = new PeriodSummaryState(seedDateTime, TenMinutes);

        var firstQuote    = alternatingLevel1Quotes.First().ToSummary();
        var secondQuote   = alternatingLevel1Quotes.Skip(1).First().ToSummary();
        var lastTickQuote = alternatingLevel1Quotes.Skip(9).First().ToSummary();

        periodSummaryState.SubSummaryPeriods.AddFirst(firstQuote);
        periodSummaryState.SubSummaryPeriods.AddLast(secondQuote);
        periodSummaryState.SubSummaryPeriods.AddLast(lastTickQuote);

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

        var builtSummary = periodSummaryState.BuildPeriodSummary(recycler, alternatingLevel1Quotes.Skip(10).First().SourceTime);

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
    public void OneLastHalfSummary_BuildPeriodSummary_SummaryFlagsIncompleteStartHalfSet()
    {
        var periodSummaryState = new PeriodSummaryState(seedDateTime, TenMinutes);

        var firstSummary = alternatingPeriodSummaries.Skip(1).First();

        periodSummaryState.SubSummaryPeriods.AddFirst(firstSummary);

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

        var builtSummary = periodSummaryState.BuildPeriodSummary(recycler, firstSummary.PeriodEndTime);

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
        Assert.AreEqual(PricePeriodSummaryFlags.IncompleteStartMask, builtSummary.PeriodSummaryFlags);
    }

    [TestMethod]
    [Timeout(20_000)]
    public void OneLastHalfSummaryWithPreviousEndBidAsk_BuildPeriodSummary_NoMissingPeriodFlags()
    {
        var periodSummaryState = new PeriodSummaryState(seedDateTime, TenMinutes);

        var previousPeriodBidAsk = alternatingPeriodSummaries.First().EndBidAsk;
        var firstSummary         = alternatingPeriodSummaries.Skip(1).First();

        periodSummaryState.PreviousPeriodBidAskEnd = previousPeriodBidAsk;
        periodSummaryState.SubSummaryPeriods.AddFirst(firstSummary);

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

        var builtSummary = periodSummaryState.BuildPeriodSummary(recycler, firstSummary.PeriodEndTime);

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
        Assert.AreEqual(PricePeriodSummaryFlags.CreatedFromPreviousEnd, builtSummary.PeriodSummaryFlags);
    }

    [TestMethod]
    [Timeout(20_000)]
    public void OneFirstHalfSummaryWithPreviousEndBidAsk_BuildPeriodSummaryHalfWayThroughPeriod_NoMissingPeriodFlagsPeriodLatest()
    {
        var periodSummaryState = new PeriodSummaryState(seedDateTime, TenMinutes);

        var firstSummary = alternatingPeriodSummaries.First();

        periodSummaryState.SubSummaryPeriods.AddFirst(firstSummary);

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

        var builtSummary = periodSummaryState.BuildPeriodSummary(recycler, firstSummary.PeriodEndTime);

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
        Assert.AreEqual(PricePeriodSummaryFlags.PeriodLatest, builtSummary.PeriodSummaryFlags);
    }
}
