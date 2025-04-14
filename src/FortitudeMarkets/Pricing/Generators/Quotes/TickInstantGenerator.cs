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
    protected TickInstantGeneratorBase(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) { }

    public void PopulateQuote(IMutableTickInstant populateQuote, MidPriceTimePair midPriceTimePair)
    {
        var sourceTime = GenerateQuoteValues.SourceTime;
        populateQuote.SourceTime         = sourceTime;
        populateQuote.SingleTickValue    = midPriceTimePair.CurrentMid.Mid;
        AdapterReceivedDateTime          = GenerateQuoteValues.AdapterReceivedTime;
        AdapterSentDateTime              = GenerateQuoteValues.AdapterSentTime;
        populateQuote.ClientReceivedTime = GenerateQuoteValues.ClientReceivedTime;
        populateQuote.IsReplay           = GenerateQuoteValues.IsReplay;
    }
}

public class TickInstantGenerator : TickInstantGeneratorBase<TickInstant>
{
    public TickInstantGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) { }

    public override TickInstant BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new TickInstant(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        return toPopulate;
    }
}
