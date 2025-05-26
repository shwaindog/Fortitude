// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Generators.Quotes.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes;

public struct TimeStampGenerationInfo
{
    public TimeStampGenerationInfo() { }

    public long     IntervalStdDeviationTicks = 3000;
    public TimeSpan MinQuoteTimeSpan          = TimeSpan.FromTicks(100);

    public TimeSpan AverageToAdapterDelayTimeSpan    = TimeSpan.FromTicks(400_000);
    public long     MinAdapterDelayTicks             = 10_000;
    public long     AdapterStandardDeviationTicks    = 200_000;
    public TimeSpan AverageAdapterProcessingTimeSpan = TimeSpan.FromTicks(10_000);

    public TimeSpan ClientAdapterDelayTimeSpan          = TimeSpan.FromTicks(200_000);
    public long     MinClientAdapterDelayTicks          = 120_000;
    public long     ClientAdapterStandardDeviationTicks = 100_000;
}

public struct GenerateQuoteInfo
{
    public BookGenerationInfo BookGenerationInfo;

    public int CycleQuotesAmount = 0;

    public GenerateLastTradeInfo LastTradeInfo;

    public IMidPriceGenerator MidPriceGenerator;


    public TimeSpan SingleQuoteNextQuoteIncrement = TimeSpan.FromMilliseconds(1);

    public int SingleQuoteStartSequenceNumber = 0;

    public DateTime SingleQuoteStartTime = DateTime.Now;

    public ISourceTickerInfo SourceTickerInfo;

    public TimeStampGenerationInfo TimeStampGenerationInfo;

    public GenerateQuoteInfo
    (ISourceTickerInfo sourceTickerInfo,
        IMidPriceGenerator? midPriceGenerator = null,
        BookGenerationInfo? bookGenerationInfo = null,
        TimeStampGenerationInfo? timeStampGenerationInfo = null,
        GenerateLastTradeInfo? lastTradeInfo = null)
    {
        SourceTickerInfo        = sourceTickerInfo;
        MidPriceGenerator       = midPriceGenerator ?? new SyntheticRepeatableMidPriceGenerator(1.0m, DateTime.Now);
        BookGenerationInfo      = bookGenerationInfo ?? new BookGenerationInfo();
        TimeStampGenerationInfo = timeStampGenerationInfo ?? new TimeStampGenerationInfo();
        LastTradeInfo           = lastTradeInfo ?? new GenerateLastTradeInfo();
    }
}