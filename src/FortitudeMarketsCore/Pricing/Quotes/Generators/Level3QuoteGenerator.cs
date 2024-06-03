// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.Generators.LastTraded;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.Generators;

public abstract class Level3QuoteGeneratorBase<TQuote> : Level2QuoteGeneratorBase<TQuote> where TQuote : IMutableLevel3Quote
{
    protected readonly LastTradedGenerator LastTradedGenerator;

    protected Level3QuoteGeneratorBase(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) =>
        LastTradedGenerator = new LastTradedGenerator(generateQuoteInfo.LastTradeInfo, NormalDist, PseudoRandom);

    public void PopulateQuote(IMutableLevel3Quote populateQuote, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        base.PopulateQuote(populateQuote, previousCurrentMidPriceTime);
    }
}

public class Level3QuoteGenerator : Level3QuoteGeneratorBase<Level3PriceQuote>
{
    public Level3QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override Level3PriceQuote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var toPopulate = new Level3PriceQuote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        LastTradedGenerator.PopulateLevel3LastTraded(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
