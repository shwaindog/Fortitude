// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsCore.Pricing.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Generators.Quotes;

public class PQLevel1QuoteGenerator : Level1QuoteGeneratorBase<PQLevel1Quote>
{
    public PQLevel1QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override PQLevel1Quote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new PQLevel1Quote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        toPopulate.IsAskPriceTopUpdatedChanged = (PreviousReturnedQuote?.IsAskPriceTopUpdated ?? false) != toPopulate.IsAskPriceTopUpdated;
        toPopulate.IsBidPriceTopUpdatedChanged = (PreviousReturnedQuote?.IsBidPriceTopUpdated ?? false) != toPopulate.IsBidPriceTopUpdated;

        toPopulate.PQSequenceId = (uint)sequenceNumber;
        return toPopulate;
    }
}
