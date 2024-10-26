// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Quotes;

public class PQLevel1QuoteGenerator : Level1QuoteGeneratorBase<PQLevel1Quote>
{
    public PQLevel1QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override PQLevel1Quote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new PQLevel1Quote(GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        toPopulate.IsAskPriceTopUpdatedChanged = (PreviousReturnedQuote?.IsAskPriceTopUpdated ?? false) != toPopulate.IsAskPriceTopUpdated;
        toPopulate.IsBidPriceTopUpdatedChanged = (PreviousReturnedQuote?.IsBidPriceTopUpdated ?? false) != toPopulate.IsBidPriceTopUpdated;

        toPopulate.PQSequenceId = (uint)sequenceNumber;
        return toPopulate;
    }
}
