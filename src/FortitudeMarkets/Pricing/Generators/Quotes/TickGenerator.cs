// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes;

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

    public DateTime SingleQuoteStartTime = DateTime.Now;

    public TimeSpan SingleQuoteNextQuoteIncrement = TimeSpan.FromMilliseconds(1);

    public int SingleQuoteStartSequenceNumber = 0;

    public ISourceTickerInfo SourceTickerInfo;

    public IMidPriceGenerator MidPriceGenerator;

    public BookGenerationInfo BookGenerationInfo;

    public TimeStampGenerationInfo TimeStampGenerationInfo;

    public GenerateLastTradeInfo LastTradeInfo;
}

public interface ITickGenerator<out TDetailLevel> where TDetailLevel : IMutableTickInstant
{
    TDetailLevel Next { get; }

    IEnumerable<TDetailLevel> Quotes(DateTime startDateTime, TimeSpan averageInterval, int numToGenerate, int sequenceNumber = 0);
}

public abstract class TickGenerator<TDetailLevel> : ITickGenerator<TDetailLevel> where TDetailLevel : IMutableTickInstant
{
    protected readonly GenerateQuoteInfo GenerateQuoteInfo;
    private readonly   TimeSpan          singleQuoteInterval;

    private int nextSingleQuoteSequenceNumber;

    private DateTime nextSingleQuoteStartTime;

    protected Normal        NormalDist = null!;
    protected TDetailLevel? PreviousReturnedQuote;
    protected Random        PseudoRandom = null!;

    protected TickGenerator(GenerateQuoteInfo generateQuoteInfo)
    {
        GenerateQuoteInfo             = generateQuoteInfo;
        nextSingleQuoteStartTime      = generateQuoteInfo.SingleQuoteStartTime;
        singleQuoteInterval           = generateQuoteInfo.SingleQuoteNextQuoteIncrement;
        nextSingleQuoteSequenceNumber = generateQuoteInfo.SingleQuoteStartSequenceNumber;
    }

    public TDetailLevel Next
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

    public IEnumerable<TDetailLevel> Quotes(DateTime startingFromTime, TimeSpan averageInterval, int numToGenerate, int sequenceNumber = 0)
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

    public abstract TDetailLevel BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber);
}
