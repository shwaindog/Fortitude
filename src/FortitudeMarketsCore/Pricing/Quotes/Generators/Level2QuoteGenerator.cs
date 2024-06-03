// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.Generators.Book;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.Generators;

public abstract class Level2QuoteGeneratorBase<TQuote> : Level1QuoteGeneratorBase<TQuote> where TQuote : Level2PriceQuote
{
    private readonly BookGenerator<OrderBook> bookGenerator;

    protected Level2QuoteGeneratorBase(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) =>
        bookGenerator = new BookGenerator<OrderBook>(generateQuoteInfo.BookGenerationInfo, CreateOrderBook);

    public OrderBook CreateOrderBook(BookSide bookSide, LayerType layerType) => new(bookSide, layerType);

    public void PopulateQuote(Level2PriceQuote populateQuote, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        base.PopulateQuote(populateQuote, previousCurrentMidPriceTime);
        var bidAskBooks = bookGenerator.GenerateBidAskBooks(previousCurrentMidPriceTime);
        populateQuote.BidBook = bidAskBooks.BidBook;
        populateQuote.AskBook = bidAskBooks.AskBook;
    }
}

public class Level2QuoteGenerator : Level2QuoteGeneratorBase<Level2PriceQuote>
{
    public Level2QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override Level2PriceQuote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var toPopulate = new Level2PriceQuote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
