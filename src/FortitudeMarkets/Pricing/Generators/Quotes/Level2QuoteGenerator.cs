// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes;

public abstract class Level2QuoteGeneratorBase<TQuote> : Level1QuoteGeneratorBase<TQuote> where TQuote : IMutableLevel2Quote
{
    protected readonly IBookGenerator BookGenerator;

    protected Level2QuoteGeneratorBase(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) =>
        BookGenerator = CreateBookGenerator(generateQuoteValues.GenerateQuoteInfo.BookGenerationInfo);

    protected virtual IBookGenerator CreateBookGenerator
        (BookGenerationInfo bookGenerationInfo) =>
        new BookGenerator(GenerateQuoteValues.BookGenerator);

    public void PopulateQuote(IMutableLevel2Quote populateQuote, MidPriceTimePair midPriceTimePair)
    {
        base.PopulateQuote(populateQuote, midPriceTimePair);
        BookGenerator.PopulateBidAskBooks(populateQuote, midPriceTimePair);
        populateQuote.IsAskPriceTopChanged = (PreviousReturnedQuote?.AskPriceTop ?? 0) != populateQuote.AskPriceTop;
        populateQuote.IsAskPriceTopChanged = (PreviousReturnedQuote?.BidPriceTop ?? 0) != populateQuote.BidPriceTop;
    }
}

public class Level2QuoteGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : Level2QuoteGeneratorBase<Level2PriceQuote>(generateQuoteValues)
{
    public override Level2PriceQuote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new Level2PriceQuote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        return toPopulate;
    }
}
