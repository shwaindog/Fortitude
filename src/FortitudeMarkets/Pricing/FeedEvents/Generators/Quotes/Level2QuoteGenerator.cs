// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes;

public abstract class Level2QuoteGeneratorBase<TQuote> : Level1QuoteGeneratorBase<TQuote> where TQuote : IMutableLevel2Quote
{
    protected readonly IBookGenerator BookGenerator;

    protected Level2QuoteGeneratorBase(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) =>
        BookGenerator = CreateBookGenerator(generateQuoteValues.GenerateQuoteInfo.BookGenerationInfo);

    protected virtual IBookGenerator CreateBookGenerator
        (BookGenerationInfo bookGenerationInfo) =>
        new BookGenerator(GenerateQuoteValues.BookGenerator, GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);

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

public abstract class PublishableLevel2QuoteGeneratorBase<TQuote> : PublishableLevel1QuoteGeneratorBase<TQuote> where TQuote : IMutablePublishableLevel2Quote
{
    protected readonly IBookGenerator BookGenerator;

    protected PublishableLevel2QuoteGeneratorBase(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) =>
        BookGenerator = CreateBookGenerator(generateQuoteValues.GenerateQuoteInfo.BookGenerationInfo);

    protected virtual IBookGenerator CreateBookGenerator
        (BookGenerationInfo bookGenerationInfo) =>
        new BookGenerator(GenerateQuoteValues.BookGenerator, GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);

    public void PopulateQuote(IMutablePublishableLevel2Quote populateQuote, MidPriceTimePair midPriceTimePair)
    {
        base.PopulateQuote(populateQuote, midPriceTimePair);
        BookGenerator.PopulateBidAskBooks(populateQuote, midPriceTimePair);
        populateQuote.IsAskPriceTopChanged = (PreviousReturnedQuote?.AskPriceTop ?? 0) != populateQuote.AskPriceTop;
        populateQuote.IsAskPriceTopChanged = (PreviousReturnedQuote?.BidPriceTop ?? 0) != populateQuote.BidPriceTop;
    }
}

public class PublishableLevel2QuoteGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : PublishableLevel2QuoteGeneratorBase<PublishableLevel2PriceQuote>(generateQuoteValues)
{
    public override PublishableLevel2PriceQuote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PublishableLevel2PriceQuote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        return toPopulate;
    }
}
