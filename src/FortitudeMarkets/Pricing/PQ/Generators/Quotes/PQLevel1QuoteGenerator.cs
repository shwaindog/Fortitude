// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Quotes;

public class PQLevel1QuoteGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : Level1QuoteGeneratorBase<PQLevel1Quote>(generateQuoteValues)
{
    public override PQLevel1Quote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PQLevel1Quote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        toPopulate.IsAskPriceTopChangedUpdated = (PreviousReturnedQuote?.IsAskPriceTopChanged ?? false) != toPopulate.IsAskPriceTopChanged;
        toPopulate.IsBidPriceTopChangedUpdated = (PreviousReturnedQuote?.IsBidPriceTopChanged ?? false) != toPopulate.IsBidPriceTopChanged;

        return toPopulate;
    }
}


public class PQPublishableLevel1QuoteGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : PublishableLevel1QuoteGeneratorBase<PQPublishableLevel1Quote>(generateQuoteValues)
{
    public override PQPublishableLevel1Quote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PQPublishableLevel1Quote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        toPopulate.IsAskPriceTopChangedUpdated = (PreviousReturnedQuote?.IsAskPriceTopChanged ?? false) != toPopulate.IsAskPriceTopChanged;
        toPopulate.IsBidPriceTopChangedUpdated = (PreviousReturnedQuote?.IsBidPriceTopChanged ?? false) != toPopulate.IsBidPriceTopChanged;

        toPopulate.PQSequenceId = (uint)sequenceNumber;
        return toPopulate;
    }
}
