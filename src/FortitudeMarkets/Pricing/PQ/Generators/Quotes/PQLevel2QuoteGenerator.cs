﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Quotes;

public class PQLevel2QuoteGenerator
    (CurrentQuoteInstantValueGenerator generateQuoteValues) : Level2QuoteGeneratorBase<PQLevel2Quote>(generateQuoteValues)
{
    public override PQLevel2Quote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PQLevel2Quote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        toPopulate.IsAskPriceTopChangedUpdated = (PreviousReturnedQuote?.IsAskPriceTopChanged ?? false) != toPopulate.IsAskPriceTopChanged;
        toPopulate.IsBidPriceTopChangedUpdated = (PreviousReturnedQuote?.IsBidPriceTopChanged ?? false) != toPopulate.IsBidPriceTopChanged;

        return toPopulate;
    }

    protected override IBookGenerator CreateBookGenerator
        (BookGenerationInfo bookGenerationInfo) =>
        new PQBookGenerator(GenerateQuoteValues.BookGenerator, GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
}


public class PQPublishableLevel2QuoteGenerator
    (CurrentQuoteInstantValueGenerator generateQuoteValues) : PublishableLevel2QuoteGeneratorBase<PQPublishableLevel2Quote>(generateQuoteValues)
{
    public override PQPublishableLevel2Quote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PQPublishableLevel2Quote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        toPopulate.IsAskPriceTopChangedUpdated = (PreviousReturnedQuote?.IsAskPriceTopChanged ?? false) != toPopulate.IsAskPriceTopChanged;
        toPopulate.IsBidPriceTopChangedUpdated = (PreviousReturnedQuote?.IsBidPriceTopChanged ?? false) != toPopulate.IsBidPriceTopChanged;

        toPopulate.PQSequenceId = (uint)sequenceNumber;
        return toPopulate;
    }

    protected override IBookGenerator CreateBookGenerator
        (BookGenerationInfo bookGenerationInfo) =>
        new PQBookGenerator(GenerateQuoteValues.BookGenerator, GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
}
