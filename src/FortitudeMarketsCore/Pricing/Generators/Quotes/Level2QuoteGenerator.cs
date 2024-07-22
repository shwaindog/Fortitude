// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.Generators.Quotes;

public abstract class Level2QuoteGeneratorBase<TQuote> : Level1QuoteGeneratorBase<TQuote> where TQuote : IMutableLevel2Quote
{
    protected readonly IBookGenerator BookGenerator;

    protected Level2QuoteGeneratorBase(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) =>
        BookGenerator = CreateBookGenerator(generateQuoteInfo.BookGenerationInfo);

    protected virtual IBookGenerator CreateBookGenerator(BookGenerationInfo bookGenerationInfo) => new BookGenerator(bookGenerationInfo);

    public void PopulateQuote(IMutableLevel2Quote populateQuote, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        base.PopulateQuote(populateQuote, previousCurrentMidPriceTime);
        BookGenerator.PopulateBidAskBooks(populateQuote, previousCurrentMidPriceTime);
        populateQuote.IsAskPriceTopUpdated = (PreviousReturnedQuote?.AskPriceTop ?? 0) != populateQuote.AskPriceTop;
        populateQuote.IsAskPriceTopUpdated = (PreviousReturnedQuote?.BidPriceTop ?? 0) != populateQuote.BidPriceTop;
    }
}

public class Level2QuoteGenerator : Level2QuoteGeneratorBase<Level2PriceQuote>
{
    public Level2QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override Level2PriceQuote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new Level2PriceQuote(GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
