// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsCore.Pricing.Quotes.Generators;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.Generators;

public class PQLevel1QuoteGenerator : Level1QuoteGeneratorBase<PQLevel1Quote>
{
    public PQLevel1QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override PQLevel1Quote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var toPopulate = new PQLevel1Quote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
