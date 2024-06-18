// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Generators.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.Generators.Quotes.LayeredBook;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarketsCore.Pricing.Generators.Quotes;

public struct TimeStampGenerationInfo
{
    public TimeStampGenerationInfo() { }

    public long     IntervalStdDeviationTicks = 3000;
    public TimeSpan MinQuoteTimeSpan          = TimeSpan.FromTicks(100);

    public TimeSpan AverageAdapterDelayTimeSpan      = TimeSpan.FromTicks(400_000);
    public long     MinAdapterDelayTicks             = 10_000;
    public long     AdapterStandardDeviationTicks    = 200_000;
    public TimeSpan AverageAdapterProcessingTimeSpan = TimeSpan.FromTicks(10_000);

    public TimeSpan ClientAdapterDelayTimeSpan          = TimeSpan.FromTicks(200_000);
    public long     MinClientAdapterDelayTicks          = 120_000;
    public long     ClientAdapterStandardDeviationTicks = 100_000;
}

public struct GenerateQuoteInfo
{
    public GenerateQuoteInfo
    (ISourceTickerQuoteInfo sourceTickerQuoteInfo,
        IMidPriceGenerator? midPriceGenerator = null,
        BookGenerationInfo? bookGenerationInfo = null,
        TimeStampGenerationInfo? timeStampGenerationInfo = null,
        GenerateLastTradeInfo? lastTradeInfo = null)
    {
        SourceTickerQuoteInfo   = sourceTickerQuoteInfo;
        MidPriceGenerator       = midPriceGenerator ?? new SyntheticRepeatableMidPriceGenerator(1.0m, DateTime.Now);
        BookGenerationInfo      = bookGenerationInfo ?? new BookGenerationInfo();
        TimeStampGenerationInfo = timeStampGenerationInfo ?? new TimeStampGenerationInfo();
        LastTradeInfo           = lastTradeInfo ?? new GenerateLastTradeInfo();
    }

    public DateTime SingleQuoteStartTime = DateTime.Now;

    public TimeSpan SingleQuoteNextQuoteIncrement = TimeSpan.FromMilliseconds(1);

    public int SingleQuoteStartSequenceNumber = 0;

    public ISourceTickerQuoteInfo SourceTickerQuoteInfo;

    public IMidPriceGenerator MidPriceGenerator;

    public BookGenerationInfo BookGenerationInfo;

    public TimeStampGenerationInfo TimeStampGenerationInfo;

    public GenerateLastTradeInfo LastTradeInfo;
}

public interface IQuoteGenerator<out TQuote> where TQuote : IMutableLevel0Quote
{
    TQuote Next { get; }

    IEnumerable<TQuote> Quotes(DateTime startDateTime, TimeSpan averageInterval, int numToGenerate, int sequenceNumber = 0);
}

public abstract class QuoteGenerator<TQuote> : IQuoteGenerator<TQuote> where TQuote : IMutableLevel0Quote
{
    protected readonly GenerateQuoteInfo GenerateQuoteInfo;
    private readonly   TimeSpan          singleQuoteInterval;

    private int nextSingleQuoteSequenceNumber;

    private DateTime nextSingleQuoteStartTime;

    protected Normal  NormalDist = null!;
    protected TQuote? PreviousReturnedQuote;
    protected Random  PseudoRandom = null!;

    protected QuoteGenerator(GenerateQuoteInfo generateQuoteInfo)
    {
        GenerateQuoteInfo             = generateQuoteInfo;
        nextSingleQuoteStartTime      = generateQuoteInfo.SingleQuoteStartTime;
        singleQuoteInterval           = generateQuoteInfo.SingleQuoteNextQuoteIncrement;
        nextSingleQuoteSequenceNumber = generateQuoteInfo.SingleQuoteStartSequenceNumber;
    }

    public TQuote Next
    {
        get
        {
            var prevCurrMid = GenerateQuoteInfo.MidPriceGenerator
                                               .PreviousCurrentPrices(nextSingleQuoteStartTime, singleQuoteInterval, 1
                                                                    , nextSingleQuoteSequenceNumber++)
                                               .First();
            PseudoRandom             =  new Random(prevCurrMid.PreviousMid.Mid.GetHashCode() ^ prevCurrMid.CurrentMid.Mid.GetHashCode());
            NormalDist               =  new Normal(0, 1, PseudoRandom);
            nextSingleQuoteStartTime += singleQuoteInterval;
            PreviousReturnedQuote    =  BuildQuote(prevCurrMid, nextSingleQuoteSequenceNumber);
            return PreviousReturnedQuote;
        }
    }

    public IEnumerable<TQuote> Quotes(DateTime startingFromTime, TimeSpan averageInterval, int numToGenerate, int sequenceNumber = 0)
    {
        var currentSeqNum = sequenceNumber;
        foreach (var prevCurrMids in GenerateQuoteInfo.MidPriceGenerator
                                                      .PreviousCurrentPrices(startingFromTime, averageInterval, numToGenerate, sequenceNumber))
        {
            PseudoRandom          = new Random(prevCurrMids.PreviousMid.Mid.GetHashCode() ^ prevCurrMids.CurrentMid.Mid.GetHashCode());
            NormalDist            = new Normal(0, 1, PseudoRandom);
            PreviousReturnedQuote = BuildQuote(prevCurrMids, currentSeqNum++);
            yield return PreviousReturnedQuote;
        }
    }

    public abstract TQuote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber);
}
