// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsCore.Pricing.Quotes.Generators;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.Generators;

public class PQLevel3QuoteGenerator : Level3QuoteGeneratorBase<PQLevel3Quote>
{
    public PQLevel3QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override PQLevel3Quote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var toPopulate = new PQLevel3Quote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
