// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.Generators.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.Generators;

public abstract class Level2QuoteGeneratorBase<TQuote> : Level1QuoteGeneratorBase<TQuote> where TQuote : IMutableLevel2Quote
{
    private readonly IBookGenerator bookGenerator;

    protected Level2QuoteGeneratorBase(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) =>
        bookGenerator = CreateBookGenerator(generateQuoteInfo.BookGenerationInfo);

    protected virtual IBookGenerator CreateBookGenerator(BookGenerationInfo bookGenerationInfo) => new BookGenerator(bookGenerationInfo);

    public void PopulateQuote(IMutableLevel2Quote populateQuote, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        base.PopulateQuote(populateQuote, previousCurrentMidPriceTime);
        bookGenerator.PopulateBidAskBooks(populateQuote, previousCurrentMidPriceTime);
        populateQuote.IsAskPriceTopUpdated = (PreviousReturnedQuote?.AskPriceTop ?? 0) != populateQuote.AskPriceTop;
        populateQuote.IsAskPriceTopUpdated = (PreviousReturnedQuote?.BidPriceTop ?? 0) != populateQuote.BidPriceTop;
    }
}

public class Level2QuoteGenerator : Level2QuoteGeneratorBase<Level2PriceQuote>
{
    public Level2QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override Level2PriceQuote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new Level2PriceQuote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
