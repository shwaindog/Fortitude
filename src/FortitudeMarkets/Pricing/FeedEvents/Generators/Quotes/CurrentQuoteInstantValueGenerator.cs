// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes;

public struct SnapshotGeneratedValues
{
    public SnapshotGeneratedValues
        (MidPriceTimePair forMidPriceTimePair) =>
        ForMidPriceTimePair = forMidPriceTimePair;

    public MidPriceTimePair ForMidPriceTimePair;

    public DateTime? SourceTime;
    public DateTime? AdapterReceivedTime;
    public DateTime? AdapterSentTime;
    public DateTime? ClientReceivedTime;
    public DateTime? SourceAskTime;
    public DateTime? SourceBidTime;
    public DateTime? ValidFrom;
    public DateTime? ValidTo;
    public bool?     IsReplay;
    public bool?     IsExecutable;
}

public class CurrentQuoteInstantValueGenerator(GenerateQuoteInfo generateQuoteInfo)
{
    protected QuoteBookValuesGenerator? BookValuesGenerator;
    protected SnapshotGeneratedValues   CurrentQuoteValues = new(new MidPriceTimePair());
    private   Normal?                   normalDist;
    protected SnapshotGeneratedValues   PreviousQuoteValues = new(new MidPriceTimePair());

    private Random? pseudoRandom;

    public Normal NormalDist
    {
        get => normalDist ??= new Normal(0, 1, PseudoRandom);
        set => normalDist = value;
    }
    public Random PseudoRandom
    {
        get => pseudoRandom ??= new Random(DateTime.Now.Microsecond ^ DateTime.Now.Millisecond);
        set => pseudoRandom = value;
    }

    public GenerateQuoteInfo GenerateQuoteInfo => generateQuoteInfo;

    public MidPriceTimePair CurrentMidPriceTimePair { get; private set; }

    public virtual bool IsReplay
    {
        get
        {
            CurrentQuoteValues.IsReplay ??= PseudoRandom.Next(0, 1000) < 995;
            return CurrentQuoteValues.IsReplay!.Value;
        }
    }
    public virtual bool IsExecutable
    {
        get
        {
            CurrentQuoteValues.IsExecutable ??= PseudoRandom.Next(0, 1000) < 995;
            return CurrentQuoteValues.IsExecutable!.Value;
        }
    }

    public virtual DateTime SourceTime
    {
        get
        {
            if (CurrentQuoteValues.SourceTime != null) return CurrentQuoteValues.SourceTime.Value;
            var sourceTime = CurrentMidPriceTimePair.CurrentMid.Time +
                             NextTimeSpanOffset(generateQuoteInfo.TimeStampGenerationInfo.IntervalStdDeviationTicks);

            if (sourceTime - (PreviousQuoteValues.SourceTime ?? DateTime.MinValue) < generateQuoteInfo.TimeStampGenerationInfo.MinQuoteTimeSpan)
                sourceTime = PreviousQuoteValues.SourceTime!.Value.Add(generateQuoteInfo.TimeStampGenerationInfo.MinQuoteTimeSpan);
            CurrentQuoteValues.SourceTime = sourceTime;
            return sourceTime;
        }
    }

    public virtual DateTime AdapterReceivedTime
    {
        get
        {
            CurrentQuoteValues.AdapterReceivedTime ??=
                NextTime(SourceTime + generateQuoteInfo.TimeStampGenerationInfo.AverageToAdapterDelayTimeSpan
                       , generateQuoteInfo.TimeStampGenerationInfo.AdapterStandardDeviationTicks);
            return CurrentQuoteValues.AdapterReceivedTime!.Value;
        }
    }

    public virtual DateTime AdapterSentTime
    {
        get
        {
            CurrentQuoteValues.AdapterSentTime ??=
                NextTimeWithMinCap
                    (AdapterReceivedTime + generateQuoteInfo.TimeStampGenerationInfo.AverageAdapterProcessingTimeSpan,
                     generateQuoteInfo.TimeStampGenerationInfo.AdapterStandardDeviationTicks
                   , generateQuoteInfo.TimeStampGenerationInfo.MinAdapterDelayTicks);
            return CurrentQuoteValues.AdapterSentTime!.Value;
        }
    }

    public virtual DateTime ClientReceivedTime
    {
        get
        {
            CurrentQuoteValues.ClientReceivedTime ??=
                NextTimeWithMinCap
                    (AdapterSentTime + generateQuoteInfo.TimeStampGenerationInfo.ClientAdapterDelayTimeSpan,
                     generateQuoteInfo.TimeStampGenerationInfo.ClientAdapterStandardDeviationTicks
                   , generateQuoteInfo.TimeStampGenerationInfo.MinClientAdapterDelayTicks);
            return CurrentQuoteValues.ClientReceivedTime!.Value;
        }
    }

    public virtual DateTime SourceAskTime
    {
        get
        {
            CurrentQuoteValues.SourceAskTime ??= SourceTime;
            return CurrentQuoteValues.SourceAskTime!.Value;
        }
    }

    public virtual DateTime SourceBidTime
    {
        get
        {
            CurrentQuoteValues.SourceBidTime ??= SourceTime;
            return CurrentQuoteValues.SourceBidTime!.Value;
        }
    }

    public virtual DateTime ValidFrom
    {
        get
        {
            CurrentQuoteValues.ValidFrom ??= SourceTime;
            return CurrentQuoteValues.ValidFrom!.Value;
        }
    }

    public virtual DateTime ValidTo
    {
        get
        {
            CurrentQuoteValues.ValidTo ??= SourceTime.AddSeconds(30);
            return CurrentQuoteValues.ValidTo!.Value;
        }
    }

    public virtual QuoteBookValuesGenerator BookGenerator => BookValuesGenerator ??= new QuoteBookValuesGenerator(this);

    public virtual void NextQuoteValuesInitialise(MidPriceTimePair midPriceTimePair)
    {
        CurrentMidPriceTimePair = midPriceTimePair;
        PseudoRandom
            = new Random(midPriceTimePair.PreviousMid.Mid.GetHashCode() ^ midPriceTimePair.CurrentMid.Mid.GetHashCode());
        NormalDist          = new Normal(0, 1, PseudoRandom);
        PreviousQuoteValues = CurrentQuoteValues;
        CurrentQuoteValues  = new SnapshotGeneratedValues();
        BookGenerator.NextBookValuesInitialise();
    }

    protected virtual TimeSpan NextTimeSpanOffset(long variationStdDeviationTicks) =>
        TimeSpan.FromTicks((long)(NormalDist.Sample() * variationStdDeviationTicks));

    protected virtual DateTime NextTime(DateTime baseTime, long variationStdDeviationTicks) =>
        baseTime + NextTimeSpanOffset(variationStdDeviationTicks);

    protected virtual DateTime NextTimeWithMinCap(DateTime baseTime, long variationStdDeviationTicks, long minTickCap) =>
        baseTime + TimeSpan.FromTicks(Math.Max(minTickCap, NextTimeSpanOffset(variationStdDeviationTicks).Ticks));
}
