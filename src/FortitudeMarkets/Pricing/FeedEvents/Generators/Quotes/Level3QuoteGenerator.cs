// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes;

public abstract class Level3QuoteGeneratorBase<TQuote> : Level2QuoteGeneratorBase<TQuote> where TQuote : IMutableLevel3Quote
{
    protected readonly ILastTradedGenerator LastTradedGenerator;

    protected Level3QuoteGeneratorBase(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) =>
        LastTradedGenerator = CreateLastTradedGenerator(generateQuoteValues.GenerateQuoteInfo.LastTradeInfo);

    public void PopulateQuote(IMutableLevel3Quote populateQuote, MidPriceTimePair midPriceTimePair)
    {
        base.PopulateQuote(populateQuote, midPriceTimePair);
        LastTradedGenerator.PopulateLevel3LastTraded(populateQuote, midPriceTimePair);
        populateQuote.ValueDate            = BookGenerator.QuoteBookGenerator.BookValueDate;
        populateQuote.SourceQuoteReference = BookGenerator.QuoteBookGenerator.BookSourceQuoteRef;
        populateQuote.BatchId              = BookGenerator.QuoteBookGenerator.BookBatchId;
    }

    protected virtual ILastTradedGenerator CreateLastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo) =>
        new LastTradedGenerator(generateLastTradeInfo);
}

public class Level3QuoteGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : Level3QuoteGeneratorBase<Level3PriceQuote>(generateQuoteValues)
{
    public override Level3PriceQuote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new Level3PriceQuote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        return toPopulate;
    }
}

public abstract class PublishableLevel3QuoteGeneratorBase<TQuote> : PublishableLevel2QuoteGeneratorBase<TQuote> where TQuote : IMutablePublishableLevel3Quote
{
    protected readonly ILastTradedGenerator LastTradedGenerator;

    protected PublishableLevel3QuoteGeneratorBase(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) =>
        LastTradedGenerator = CreateLastTradedGenerator(generateQuoteValues.GenerateQuoteInfo.LastTradeInfo);

    public void PopulateQuote(IMutablePublishableLevel3Quote populateQuote, MidPriceTimePair midPriceTimePair)
    {
        base.PopulateQuote(populateQuote, midPriceTimePair);
        LastTradedGenerator.PopulateLevel3LastTraded(populateQuote, midPriceTimePair);
        populateQuote.ValueDate            = BookGenerator.QuoteBookGenerator.BookValueDate;
        populateQuote.SourceQuoteReference = BookGenerator.QuoteBookGenerator.BookSourceQuoteRef;
        populateQuote.BatchId              = BookGenerator.QuoteBookGenerator.BookBatchId;
    }

    protected virtual ILastTradedGenerator CreateLastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo) =>
        new LastTradedGenerator(generateLastTradeInfo);
}

public class PublishableLevel3QuoteGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    : PublishableLevel3QuoteGeneratorBase<PublishableLevel3PriceQuote>(generateQuoteValues)
{
    public override PublishableLevel3PriceQuote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PublishableLevel3PriceQuote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        return toPopulate;
    }
}
