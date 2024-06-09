// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.Generators.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.Generators;
using FortitudeMarketsCore.Pricing.Quotes.Generators.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.Generators;

public class PQLevel2QuoteGenerator : Level2QuoteGeneratorBase<PQLevel2Quote>
{
    public PQLevel2QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override PQLevel2Quote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new PQLevel2Quote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        toPopulate.IsAskPriceTopUpdatedChanged = (PreviousReturnedQuote?.IsAskPriceTopUpdated ?? false) != toPopulate.IsAskPriceTopUpdated;
        toPopulate.IsBidPriceTopUpdatedChanged = (PreviousReturnedQuote?.IsBidPriceTopUpdated ?? false) != toPopulate.IsBidPriceTopUpdated;

        toPopulate.PQSequenceId = (uint)sequenceNumber;
        return toPopulate;
    }

    protected override IBookGenerator CreateBookGenerator(BookGenerationInfo bookGenerationInfo) => new PQBookGenerator(bookGenerationInfo);
}
