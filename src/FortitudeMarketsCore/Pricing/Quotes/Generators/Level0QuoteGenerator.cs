// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.Generators;

public abstract class Level0QuoteGeneratorBase<TQuote> : QuoteGenerator<TQuote> where TQuote : IMutableLevel0Quote
{
    protected DateTime AdapterReceivedDateTime;
    protected DateTime AdapterSentDateTime;
    protected Level0QuoteGeneratorBase(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public void PopulateQuote(IMutableLevel0Quote populateQuote, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var timeStampInfo  = GenerateQuoteInfo.TimeStampGenerationInfo;
        var lastSourceTime = PreviousReturnedQuote?.SourceTime ?? DateTime.MinValue;
        var sourceTime = previousCurrentMidPriceTime.CurrentMid.Time +
                         TimeSpan.FromTicks((long)(NormalDist.Sample() *
                                                   timeStampInfo.IntervalStdDeviationTicks));
        if (sourceTime - lastSourceTime < timeStampInfo.MinQuoteTimeSpan) sourceTime = lastSourceTime.Add(timeStampInfo.MinQuoteTimeSpan);

        populateQuote.SourceTime  = sourceTime;
        populateQuote.SinglePrice = previousCurrentMidPriceTime.CurrentMid.Mid;


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

public class Level0QuoteGenerator : Level0QuoteGeneratorBase<Level0PriceQuote>
{
    public Level0QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override Level0PriceQuote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new Level0PriceQuote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
