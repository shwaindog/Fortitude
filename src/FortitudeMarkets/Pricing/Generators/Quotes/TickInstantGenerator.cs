// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes;

public abstract class TickInstantGeneratorBase<TDetailLevel> : TickGenerator<TDetailLevel> where TDetailLevel : IMutableTickInstant
{
    protected DateTime AdapterReceivedDateTime;
    protected DateTime AdapterSentDateTime;
    protected TickInstantGeneratorBase(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public void PopulateQuote(IMutableTickInstant populateQuote, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var timeStampInfo  = GenerateQuoteInfo.TimeStampGenerationInfo;
        var lastSourceTime = PreviousReturnedQuote?.SourceTime ?? DateTime.MinValue;
        var sourceTime = previousCurrentMidPriceTime.CurrentMid.Time +
                         TimeSpan.FromTicks((long)(NormalDist.Sample() *
                                                   timeStampInfo.IntervalStdDeviationTicks));
        if (sourceTime - lastSourceTime < timeStampInfo.MinQuoteTimeSpan) sourceTime = lastSourceTime.Add(timeStampInfo.MinQuoteTimeSpan);

        populateQuote.SourceTime      = sourceTime;
        populateQuote.SingleTickValue = previousCurrentMidPriceTime.CurrentMid.Mid;


        AdapterReceivedDateTime = sourceTime + timeStampInfo.AverageAdapterDelayTimeSpan +
                                  TimeSpan.FromTicks((long)(NormalDist.Sample() *
                                                            timeStampInfo.AdapterStandardDeviationTicks));


        AdapterSentDateTime = AdapterReceivedDateTime + timeStampInfo.AverageAdapterProcessingTimeSpan +
                              TimeSpan.FromTicks(Math.Max(timeStampInfo.MinAdapterDelayTicks, (long)(NormalDist.Sample() *
                                                              timeStampInfo.AdapterStandardDeviationTicks)));

        populateQuote.ClientReceivedTime = AdapterSentDateTime + timeStampInfo.ClientAdapterDelayTimeSpan +
                                           TimeSpan.FromTicks(Math.Max(timeStampInfo.MinClientAdapterDelayTicks, (long)(NormalDist.Sample() *
                                                                           timeStampInfo.ClientAdapterStandardDeviationTicks)));
        populateQuote.IsReplay = PseudoRandom.Next(0, 1000) < 995;
    }
}

public class TickInstantGenerator : TickInstantGeneratorBase<TickInstant>
{
    public TickInstantGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override TickInstant BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new TickInstant(GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
