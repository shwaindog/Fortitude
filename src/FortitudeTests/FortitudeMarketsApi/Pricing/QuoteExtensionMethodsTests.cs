// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.TimeSeries;
using static FortitudeIO.TimeSeries.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsApi.Pricing;

[TestClass]
public class QuoteExtensionMethodsTests
{
    private const string  OriginalQuoteExchangeName = "TestDiffExchange";
    private const string  OriginalQuoteTickerName   = "TestDiffTicker";
    private const decimal OriginalBidTopPrice       = 1.1123m;
    private const decimal OriginalAskTopPrice       = 1.1125m;
    private const decimal OriginalBidTopVolume      = 100000;
    private const decimal OriginalAskTopVolume      = 30000;

    private readonly DateTime originalQuoteAdapterTime = new(2015, 08, 09, 22, 07, 24);
    private readonly DateTime originalQuoteAskDateTime = new(2015, 10, 16, 16, 44, 12);
    private readonly DateTime originalQuoteBidDateTime = new(2015, 10, 16, 16, 44, 11);

    private readonly DateTime originalQuoteClientReceiveTime = DateTime.Parse("2015-08-09 10:06:23.123456");
    private readonly DateTime originalQuoteExchangeTime      = new(2015, 11, 17, 22, 07, 23);

    private readonly SourceTickerQuoteInfo originalSourceTickerQuoteInfo =
        new(1, OriginalQuoteExchangeName, 1, OriginalQuoteTickerName, Level3, Unknown
          , 20, 0.0001m, 1m, 1000000m, 1m);

    private ILevel3Quote originalQuote = null!;

    [TestInitialize]
    public void TestSetup()
    {
        originalQuote =
            new Level3PriceQuote
                (originalSourceTickerQuoteInfo, originalQuoteExchangeTime, false, 1.1124m, originalQuoteClientReceiveTime
               , originalQuoteAdapterTime, originalQuoteAdapterTime, originalQuoteBidDateTime
               , true, originalQuoteAskDateTime, true, true, new PricePeriodSummary()
               , new OrderBook
                     (BookSide.BidBook,
                      new[]
                      {
                          new PriceVolumeLayer(OriginalBidTopPrice, OriginalBidTopVolume), new PriceVolumeLayer(1.1122m, 20000)
                      }), true
               , new OrderBook
                     (BookSide.AskBook,
                      new[]
                      {
                          new PriceVolumeLayer(OriginalAskTopPrice, OriginalAskTopVolume), new PriceVolumeLayer(1.1126m, 40000)
                      }), true
               , null, 0, 0, new DateTime(2017, 12, 29, 21, 0, 0));
    }

    [TestMethod]
    public void OneOrBothQuotesNull_Diff_ReturnsOneIsNullString()
    {
        var q2 = (Level3PriceQuote)null!;
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
        var newSourceTickerQuoteInfo = new SourceTickerQuoteInfo
            (1, newExchangerName, 1, OriginalQuoteTickerName, Level3, Unknown
           , 20, 0.0001m, 1m, 1000000m, 1m);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2, (Level3PriceQuote pq) => pq.SourceTickerQuoteInfo, newSourceTickerQuoteInfo);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.Write(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.SourceTickerQuoteInfo))
                   && differences.Contains(OriginalQuoteExchangeName)
                   && differences.Contains(newExchangerName));
        var updatedNewQuoteInfo = (SourceTickerQuoteInfo)q2.SourceTickerQuoteInfo!;
        NonPublicInvocator.SetAutoPropertyInstanceField
            (updatedNewQuoteInfo, (SourceTickerQuoteInfo stqi) => stqi.Source, OriginalQuoteExchangeName);
        differences = originalQuote.DiffQuotes(q2);
        Console.Out.Write(differences);
        Assert.AreEqual("", differences);
    }

    [TestMethod]
    public void IdenticalExceptTickerQuotes_Diff_ReturnsTickerDifferent()
    {
        var q2            = originalQuote.Clone();
        var newTickerName = "DifferentTicker";
        var newSourceTickerQuoteInfo = new SourceTickerQuoteInfo
            (1, OriginalQuoteExchangeName, 1, newTickerName, Level3, Unknown
           , 20, 0.0001m, 1m, 1000000m, 1m);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (q2, (Level3PriceQuote pq) => pq.SourceTickerQuoteInfo, newSourceTickerQuoteInfo);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.Write(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.SourceTickerQuoteInfo))
                   && differences.Contains(OriginalQuoteTickerName)
                   && differences.Contains(newTickerName));
        var updatedNewQuoteInfo = (SourceTickerQuoteInfo)q2.SourceTickerQuoteInfo!;
        NonPublicInvocator.SetAutoPropertyInstanceField
            (updatedNewQuoteInfo, (SourceTickerQuoteInfo stqi) => stqi.Ticker, OriginalQuoteTickerName);
        differences = originalQuote.DiffQuotes(q2);
        Console.Out.Write(differences);
        Assert.AreEqual("", differences);
    }

    [TestMethod]
    public void IdenticalExceptExchangeTimeQuotes_Diff_ReturnsExchangeTimeDifferent()
    {
        var q2 = originalQuote.Clone();

        var newExchangeDateTime = originalQuoteExchangeTime.AddMilliseconds(100);
        NonPublicInvocator.SetAutoPropertyInstanceField(q2, (Level3PriceQuote q) => q.SourceTime, newExchangeDateTime);
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
            (q2, (Level3PriceQuote pq) => pq.AdapterSentTime, newAdapterDateTime);
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
            (q2, (Level3PriceQuote pq) => pq.ClientReceivedTime, newClientReceivedDateTime);
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
        NonPublicInvocator.SetAutoPropertyInstanceField(q2.BidBook[0]!,
                                                        (PriceVolumeLayer pvl) => pvl.Price, newBidTopPrice);
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
            (differences.Contains(nameof(q2.BidBook))
          && differences.Contains("[ 0]") && differences.Contains("PriceVolumeLayer"));
    }

    [TestMethod]
    public void IdenticalExceptAskTopQuotes_Diff_ReturnsAskTopDifferent()
    {
        var q2 = originalQuote.Clone();

        var newAskTopPrice = OriginalAskTopPrice - 0.0004m;
        NonPublicInvocator.SetAutoPropertyInstanceField(q2.AskBook[0]!,
                                                        (PriceVolumeLayer pvl) => pvl.Price, newAskTopPrice);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);

        var expectedOriginal = OriginalAskTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        var expectedNew      = newAskTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 2);
        Assert.IsTrue(differences.Contains(nameof(q2.AskBook))
                   && differences.Contains("[ 0]") && differences.Contains("PriceVolumeLayer"));
    }

    [TestMethod]
    public void IdenticalExceptBidBookQuotes_Diff_ReturnsBidBookDifferent()
    {
        var q2 = originalQuote.Clone();

        var newBidTopDateTime = new OrderBook(BookSide.BidBook, new List<IPriceVolumeLayer>());
        NonPublicInvocator.SetAutoPropertyInstanceField(q2, (Level3PriceQuote pq) => pq.BidBook, newBidTopDateTime);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        var expectedOriginal = OriginalBidTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        var expectedNew      = "=0";
        Assert.IsTrue(differences.Contains(nameof(q2.BidPriceTop)) &&
                      differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.BidBook))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);
        NonPublicInvocator.SetAutoPropertyInstanceField(q2, (Level3PriceQuote pq) => pq.BidBook
                                                      , new OrderBook(BookSide.BidBook, 2));

        differences = q2.DiffQuotes(originalQuote);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.BidPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.BidBook))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);
        NonPublicInvocator.SetAutoPropertyInstanceField(q2, (Level3PriceQuote pq) => pq.BidBook
                                                      , new OrderBook(BookSide.BidBook, 2));

        differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.BidPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.BidBook))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("l2=null")) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);

        differences = q2.DiffQuotes(originalQuote);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.BidPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.BidBook))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("l1=null")) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);

        var newBidTopVolume = OriginalBidTopVolume + 10000;
        q2 = originalQuote.Clone();
        NonPublicInvocator.SetAutoPropertyInstanceField(q2.BidBook[0]!,
                                                        (PriceVolumeLayer pvl) => pvl.Volume, newBidTopVolume);
        differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        expectedOriginal = OriginalBidTopVolume.ToString(PricingConstants.UniversalVolumeFormating);
        expectedNew      = newBidTopVolume.ToString(PricingConstants.UniversalVolumeFormating);
        Assert.IsTrue(differences.Contains(nameof(q2.BidBook))
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
        var newAskTopDateTime = new OrderBook(BookSide.AskBook, new List<IPriceVolumeLayer>());
        NonPublicInvocator.SetAutoPropertyInstanceField(q2, (Level3PriceQuote pq) => pq.AskBook, newAskTopDateTime);
        var differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        var expectedOriginal = OriginalAskTopPrice.ToString(PricingConstants.UniversalPriceFormating);
        var expectedNew      = "=0";
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.AskBook))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);
        NonPublicInvocator.SetAutoPropertyInstanceField(q2, (Level3PriceQuote pq) => pq.AskBook
                                                      , new OrderBook(BookSide.AskBook, 2));

        differences = q2.DiffQuotes(originalQuote);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.AskBook))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);
        NonPublicInvocator.SetAutoPropertyInstanceField(q2, (Level3PriceQuote pq) => pq.AskBook
                                                      , new OrderBook(BookSide.AskBook, 2));

        differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.AskBook))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("l2=null")) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);

        differences = q2.DiffQuotes(originalQuote);
        Console.Out.WriteLine(differences);
        Assert.IsTrue(differences.Contains(nameof(q2.AskPriceTop))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1);
        Assert.IsTrue(differences.Contains(nameof(q2.AskBook))
                   && differences.Contains("[ 0]")
                   && differences.Contains("[ 1]")
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("l1=null")) ==
                      2
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith("PriceVolumeLayer")) == 2);

        var newAskTopVolume = OriginalAskTopVolume + 10000;
        q2 = originalQuote.Clone();
        NonPublicInvocator.SetAutoPropertyInstanceField(q2.AskBook[0]!,
                                                        (PriceVolumeLayer pvl) => pvl.Volume, newAskTopVolume);
        differences = originalQuote.DiffQuotes(q2);
        Console.Out.WriteLine(differences);
        expectedOriginal = OriginalAskTopVolume.ToString(PricingConstants.UniversalVolumeFormating);
        expectedNew      = newAskTopVolume.ToString(PricingConstants.UniversalVolumeFormating);
        Assert.IsTrue(differences.Contains(nameof(q2.AskBook))
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedOriginal)) == 1
                   && differences.Select((c, i) => differences.Substring(i))
                                 .Count(sub => sub.StartsWith(expectedNew)) == 1
                   && differences.Contains("PriceVolumeLayer"));
    }
}
