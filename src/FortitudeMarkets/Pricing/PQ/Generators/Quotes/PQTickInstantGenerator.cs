// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Quotes;

public class PQTickInstantGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : TickInstantGeneratorBase<PQTickInstant>(generateQuoteValues)
{
    public override PQTickInstant BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PQTickInstant(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        return toPopulate;
    }
}

public class PQPublishableTickInstantGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : PublishableTickInstantGeneratorBase<PQPublishableTickInstant>(generateQuoteValues)
{
    public override PQPublishableTickInstant BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PQPublishableTickInstant(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        toPopulate.PQSequenceId = (uint)sequenceNumber;
        return toPopulate;
    }
}
