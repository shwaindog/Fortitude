// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.Generators.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.Generators.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Quotes;

public class PQLevel3QuoteGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : Level3QuoteGeneratorBase<PQLevel3Quote>(generateQuoteValues)
{
    public override PQLevel3Quote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PQLevel3Quote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        toPopulate.IsAskPriceTopChangedUpdated = (PreviousReturnedQuote?.IsAskPriceTopChanged ?? false) != toPopulate.IsAskPriceTopChanged;
        toPopulate.IsBidPriceTopChangedUpdated = (PreviousReturnedQuote?.IsBidPriceTopChanged ?? false) != toPopulate.IsBidPriceTopChanged;

        return toPopulate;
    }

    protected override IBookGenerator CreateBookGenerator(BookGenerationInfo bookGenerationInfo) =>
        new PQBookGenerator(GenerateQuoteValues.BookGenerator, GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);

    protected override ILastTradedGenerator CreateLastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo) =>
        new PQLastTradedGenerator(generateLastTradeInfo);
}

public class PQPublishableLevel3QuoteGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : PublishableLevel3QuoteGeneratorBase<PQPublishableLevel3Quote>(generateQuoteValues)
{
    public override PQPublishableLevel3Quote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PQPublishableLevel3Quote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        toPopulate.IsAskPriceTopChangedUpdated = (PreviousReturnedQuote?.IsAskPriceTopChanged ?? false) != toPopulate.IsAskPriceTopChanged;
        toPopulate.IsBidPriceTopChangedUpdated = (PreviousReturnedQuote?.IsBidPriceTopChanged ?? false) != toPopulate.IsBidPriceTopChanged;

        toPopulate.PQSequenceId = (uint)sequenceNumber;
        return toPopulate;
    }

    protected override IBookGenerator CreateBookGenerator(BookGenerationInfo bookGenerationInfo) =>
        new PQBookGenerator(GenerateQuoteValues.BookGenerator, GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);

    protected override ILastTradedGenerator CreateLastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo) =>
        new PQLastTradedGenerator(generateLastTradeInfo);
}
