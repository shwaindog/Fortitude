// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

[TestClass]
public class QuoteExtensionMethodsTests
{
    private const string OriginalQuoteExchangeName = "TestDiffExchange";
    private const string OriginalQuoteTickerName   = "TestDiffTicker";

    private const decimal OriginalBidTopPrice  = 1.1123m;
    private const decimal OriginalAskTopPrice  = 1.1125m;
    private const decimal OriginalBidTopVolume = 100000;
    private const decimal OriginalAskTopVolume = 30000;

    private const FeedConnectivityStatusFlags OriginalFeedConnectivityStatusFlags =  FeedConnectivityStatusFlags.FromSourceSnapshot;

    private readonly DateTime originalQuoteAdapterTime = new(2015, 08, 09, 22, 07, 24);
    private readonly DateTime originalQuoteAskDateTime = new(2015, 10, 16, 16, 44, 12);
    private readonly DateTime originalQuoteBidDateTime = new(2015, 10, 16, 16, 44, 11);

    private readonly DateTime originalQuoteClientReceiveTime = DateTime.Parse("2015-08-09 10:06:23.123456");
    private readonly DateTime originalQuoteExchangeTime      = new(2015, 11, 17, 22, 07, 23);

    private readonly SourceTickerInfo originalSourceTickerInfo =
        new(1, OriginalQuoteExchangeName, 1, OriginalQuoteTickerName, Level3Quote, Unknown
          , 20, 0.0001m, 1m, 1000000m, 1m);

    private IPublishableLevel3Quote originalQuote = null!;

    [TestInitialize]
    public void TestSetup()
    {
        originalQuote =
            new PublishableLevel3PriceQuote
                (originalSourceTickerInfo, originalQuoteExchangeTime
               , new OrderBook
                     (new OrderBookSide
                          (BookSide.BidBook, [
                              new PriceVolumeLayer(OriginalBidTopPrice, OriginalBidTopVolume), new PriceVolumeLayer(1.1122m, 20000)
                          ])
                    , new OrderBookSide
                          (BookSide.AskBook, [
                              new PriceVolumeLayer(OriginalAskTopPrice, OriginalAskTopVolume), new PriceVolumeLayer(1.1126m, 40000)
                          ])
                     )
               , null, 0, 0, new DateTime(2017, 12, 29, 21, 0, 0)
               , true, true, originalQuoteBidDateTime, originalQuoteAskDateTime
               , originalQuoteExchangeTime, originalQuoteExchangeTime.AddSeconds(2), true
               , FeedSyncStatus.Good, OriginalFeedConnectivityStatusFlags, 1.1124m, new Candle())
                {
                    ClientReceivedTime = originalQuoteClientReceiveTime,
                    AdapterReceivedTime = originalQuoteAdapterTime,
                    AdapterSentTime = originalQuoteAdapterTime
                };
    }

    [TestMethod]
    public void OneOrBothQuotesNull_Diff_ReturnsOneIsNullString()
    {
        var q2 = (PublishableLevel3PriceQuote)null!;
        // ReSharper disable ExpressionIsAlwaysNull
        var differences = originalQuote.DiffQuotes(q2);
        Assert.AreEqual("q2 is null", differences);
        differences = q2.DiffQuotes(originalQuote);
        Assert.AreEqual("q1 is null", differences);
        differences = q2.DiffQuotes(null);
        // ReSharper restore ExpressionIsAlwaysNull
        Assert.AreEqual("", differences);
    }

    [TestMethod]
    public void TwoIdenticalQuotes_Diff_ReturnsEmptyString()
    {
        var q2          = originalQuote.Clone();
        var differences = originalQuote.DiffQuotes(q2);
        Assert.AreEqual("", differences);
    }

    [TestMethod]
    public void IdenticalExceptExchangeNameQuotes_Diff_ReturnsExchangeNameDifferent()
    {
        var q2 = originalQuote.Clone();

        var newExchangerName = "DifferentExchangeName";
        var newSourceTickerInfo = new SourceTickerInfo
            (1, newExchangerName, 1, OriginalQuoteTickerName, Level3Quote, Unknown
           , 20, 0.0001m, 1m, 1000000m, 1m);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2, (PublishableLevel3PriceQuote pq) => pq.SourceTickerInfo, newSourceTickerInfo);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.Write(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.SourceTickerInfo))
                   && differences.Contains(OriginalQuoteExchangeName)
                   && differences.Contains(newExchangerName));
        var updatedNewQuoteInfo = (SourceTickerInfo)q2.SourceTickerInfo!;
        NonPublicInvocator.SetAutoPropertyInstanceField
            (updatedNewQuoteInfo, (SourceTickerInfo stqi) => stqi.SourceName, OriginalQuoteExchangeName);
        differences = originalQuote.DiffQuotes(q2);
        Console.Out.Write(differences);
        Assert.AreEqual("", differences);
    }

    [TestMethod]
    public void IdenticalExceptTickerQuotes_Diff_ReturnsTickerDifferent()
    {
        var q2            = originalQuote.Clone();
        var newTickerName = "DifferentTicker";
        var newSourceTickerInfo = new SourceTickerInfo
            (1, OriginalQuoteExchangeName, 1, newTickerName, Level3Quote, Unknown
           , 20, 0.0001m, 1m, 1000000m, 1m);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2, (PublishableLevel3PriceQuote pq) => pq.SourceTickerInfo, newSourceTickerInfo);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.Write(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.SourceTickerInfo))
                   && differences.Contains(OriginalQuoteTickerName)
                   && differences.Contains(newTickerName));
        var updatedNewQuoteInfo = (SourceTickerInfo)q2.SourceTickerInfo!;
        NonPublicInvocator.SetAutoPropertyInstanceField
            (updatedNewQuoteInfo, (SourceTickerInfo stqi) => stqi.InstrumentName, OriginalQuoteTickerName);
        differences = originalQuote.DiffQuotes(q2);
        Console.Out.Write(differences);
        Assert.AreEqual("", differences);
    }

    [TestMethod]
    public void IdenticalExceptExchangeTimeQuotes_Diff_ReturnsExchangeTimeDifferent()
    {
        var q2 = originalQuote.Clone();

        var newExchangeDateTime = originalQuoteExchangeTime.AddMilliseconds(100);
        var nonPubQuote         = q2.AsNonPublishable;
        NonPublicInvocator.SetAutoPropertyInstanceField(nonPubQuote, (Level3PriceQuote q) => q.SourceTime, newExchangeDateTime);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        Assert.IsTrue
            (differences.Contains(nameof(q2.SourceTime))
          && differences.Contains
                 (originalQuoteExchangeTime.ToString(DateTimeConstants.FullDateTimeFormatToMicro))
          && differences.Contains
                 (newExchangeDateTime.ToString(DateTimeConstants.FullDateTimeFormatToMicro)));
    }

    [TestMethod]
    public void IdenticalExceptAdapterTimeQuotes_Diff_ReturnsAdapterTimeDifferent()
    {
        var q2 = originalQuote.Clone();

        var newAdapterDateTime = originalQuoteAdapterTime.AddMilliseconds(123);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2, (PublishableLevel3PriceQuote pq) => pq.AdapterSentTime, newAdapterDateTime);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        Assert.IsTrue
            (differences.Contains(nameof(q2.AdapterSentTime))
          && differences.Contains
                 (originalQuoteAdapterTime.ToString(DateTimeConstants.FullDateTimeFormatToMicro))
          && differences.Contains
                 (newAdapterDateTime.ToString(DateTimeConstants.FullDateTimeFormatToMicro)));
    }

    [TestMethod]
    public void IdenticalExceptReceivedTimeQuotes_Diff_ReturnsReceiveTimeDifferent()
    {
        var q2 = originalQuote.Clone();

        var newClientReceivedDateTime = originalQuoteClientReceiveTime.AddMilliseconds(123);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2, (PublishableLevel3PriceQuote pq) => pq.ClientReceivedTime, newClientReceivedDateTime);
        var differences = originalQuote.DiffQuotes(q2, true);
        Console.Out.WriteLine(differences);
        Assert.IsTrue
            (differences.Contains(nameof(q2.ClientReceivedTime))
          && differences.Contains
                 (originalQuoteClientReceiveTime.ToString(DateTimeConstants.FullDateTimeFormatToMicro))
          && differences.Contains
                 (newClientReceivedDateTime.ToString(DateTimeConstants.FullDateTimeFormatToMicro)));
        differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        Assert.AreEqual("", differences);
    }

    [TestMethod]
    public void IdenticalExceptBidTopQuotes_Diff_ReturnsBidTopDifferent()
    {
        var q2 = originalQuote.Clone();

        var newBidTopPrice = OriginalBidTopPrice + 0.0003m;
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2.BidBook[0]!, (PriceVolumeLayer pvl) => pvl.Price, newBidTopPrice);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        var expectedOriginal = OriginalBidTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        var expectedNew      = newBidTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        Assert.IsTrue
            (differences.Contains(nameof(q2.BidPriceTop))
          && differences.Select((c, i) => differences.Substring(i))
                        .Count(sub => sub.StartsWith(expectedOriginal)) == 2
          && differences.Select((c, i) => differences.Substring(i))
                        .Count(sub => sub.StartsWith(expectedNew)) == 2);
        Assert.IsTrue
            (differences.Contains(nameof(q2.OrderBook.BidSide))
          && differences.Contains("[ 0]") && differences.Contains("PriceVolumeLayer"));
    }

    [TestMethod]
    public void IdenticalExceptAskTopQuotes_Diff_ReturnsAskTopDifferent()
    {
        var q2 = originalQuote.Clone();

        var newAskTopPrice = OriginalAskTopPrice - 0.0004m;
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2.AskBook[0]!, (PriceVolumeLayer pvl) => pvl.Price, newAskTopPrice);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);

        var expectedOriginal = OriginalAskTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        var expectedNew      = newAskTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 2);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.AskSide))
                   && differences.Contains("[ 0]") && differences.Contains("PriceVolumeLayer"));
    }

    [TestMethod]
    public void IdenticalExceptBidBookQuotes_Diff_ReturnsBidBookDifferent()
    {
        var q2 = originalQuote.Clone();

        var newBidTopDateTime = new OrderBookSide(BookSide.BidBook, new List<IPriceVolumeLayer>());
        NonPublicInvocator.SetAutoPropertyInstanceField(q2.OrderBook, (OrderBook pq) => pq.BidSide, newBidTopDateTime);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        var expectedOriginal = OriginalBidTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        var expectedNew      = "=0";
        Assert.IsTrue(differences.Contains(nameof(q2.BidPriceTop)) &&
                      differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.BidSide))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2.OrderBook, (OrderBook pq) => pq.BidSide, new OrderBookSide(BookSide.BidBook, 2));

        differences = q2.DiffQuotes(originalQuote);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.BidPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.BidSide))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 4);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2.OrderBook, (OrderBook pq) => pq.BidSide, new OrderBookSide(BookSide.BidBook, 1));

        differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.BidPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.BidSide))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("l2=null")) == 1
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 3);

        differences = q2.DiffQuotes(originalQuote);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.BidPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.BidSide))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("l1=null")) == 1
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 3);

        var newBidTopVolume = OriginalBidTopVolume + 10000;
        q2 = originalQuote.Clone();
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2.BidBook[0]!, (PriceVolumeLayer pvl) => pvl.Volume, newBidTopVolume);
        differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        expectedOriginal = OriginalBidTopVolume.ToString(PricingConstants.UniversalVolumeFormating);
        expectedNew      = newBidTopVolume.ToString(PricingConstants.UniversalVolumeFormating);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.BidSide))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 1
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1
                   && differences.Contains("PriceVolumeLayer"));
    }

    [TestMethod]
    public void IdenticalExceptAskBookQuotes_Diff_ReturnsAskBookDifferent()
    {
        var q2                = originalQuote.Clone();
        var newAskTopDateTime = new OrderBookSide(BookSide.AskBook, new List<IPriceVolumeLayer>());
        NonPublicInvocator.SetAutoPropertyInstanceField(q2.OrderBook, (OrderBook pq) => pq.AskSide, newAskTopDateTime);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        var expectedOriginal = OriginalAskTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        var expectedNew      = "=0";
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.AskSide))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2.OrderBook, (OrderBook pq) => pq.AskSide, new OrderBookSide(BookSide.AskBook, 2));

        differences = q2.DiffQuotes(originalQuote);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.AskSide))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 4);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2.OrderBook, (OrderBook pq) => pq.AskSide, new OrderBookSide(BookSide.AskBook, 1));

        differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.AskSide))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("l2=null")) == 1
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 3);

        differences = q2.DiffQuotes(originalQuote);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.AskSide))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("l1=null")) == 1
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 3);

        var newAskTopVolume = OriginalAskTopVolume + 10000;
        q2 = originalQuote.Clone();
        NonPublicInvocator.SetAutoPropertyInstanceField(q2.AskBook[0]!,
                                                        (PriceVolumeLayer pvl) => pvl.Volume, newAskTopVolume);
        differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        expectedOriginal = OriginalAskTopVolume.ToString(PricingConstants.UniversalVolumeFormating);
        expectedNew      = newAskTopVolume.ToString(PricingConstants.UniversalVolumeFormating);
        Assert.IsTrue(differences.Contains(nameof(q2.OrderBook.AskSide))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 1
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1
                   && differences.Contains("PriceVolumeLayer"));
    }
}
