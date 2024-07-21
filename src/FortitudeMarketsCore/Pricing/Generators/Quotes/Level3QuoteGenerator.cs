// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Generators.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.Generators.Quotes;

public abstract class Level3QuoteGeneratorBase<TQuote> : Level2QuoteGeneratorBase<TQuote> where TQuote : IMutableLevel3Quote
{
    protected readonly ILastTradedGenerator LastTradedGenerator;

    protected Level3QuoteGeneratorBase(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) =>
        LastTradedGenerator = CreateLastTradedGenerator(generateQuoteInfo.LastTradeInfo);

    public void PopulateQuote(IMutableLevel3Quote populateQuote, PreviousCurrentMidPriceTime previousCurrentMid)
    {
        base.PopulateQuote(populateQuote, previousCurrentMid);
        LastTradedGenerator.PopulateLevel3LastTraded(populateQuote, previousCurrentMid);
        populateQuote.ValueDate            = BookGenerator.BidLayerGenerator.GenerateValueDate(previousCurrentMid);
        populateQuote.SourceQuoteReference = BookGenerator.BidLayerGenerator.GenerateQuoteRef(previousCurrentMid);
        populateQuote.BatchId              = BookGenerator.BidLayerGenerator.GenerateQuoteRef(previousCurrentMid) + 1000;
    }

    protected virtual ILastTradedGenerator CreateLastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo) =>
        new LastTradedGenerator(generateLastTradeInfo);
}

public class Level3QuoteGenerator : Level3QuoteGeneratorBase<Level3PriceQuote>
{
    public Level3QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override Level3PriceQuote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new Level3PriceQuote(GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
