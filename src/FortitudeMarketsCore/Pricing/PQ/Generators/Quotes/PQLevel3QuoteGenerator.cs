// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsCore.Pricing.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Generators.Quotes;
using FortitudeMarketsCore.Pricing.Generators.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.Generators.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.Generators.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Generators.Quotes;

public class PQLevel3QuoteGenerator : Level3QuoteGeneratorBase<PQLevel3Quote>
{
    public PQLevel3QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override PQLevel3Quote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new PQLevel3Quote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        toPopulate.IsAskPriceTopUpdatedChanged = (PreviousReturnedQuote?.IsAskPriceTopUpdated ?? false) != toPopulate.IsAskPriceTopUpdated;
        toPopulate.IsBidPriceTopUpdatedChanged = (PreviousReturnedQuote?.IsBidPriceTopUpdated ?? false) != toPopulate.IsBidPriceTopUpdated;

        toPopulate.PQSequenceId = (uint)sequenceNumber;
        return toPopulate;
    }

    protected override IBookGenerator CreateBookGenerator(BookGenerationInfo bookGenerationInfo) => new PQBookGenerator(bookGenerationInfo);

    protected override ILastTradedGenerator CreateLastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo) =>
        new PQLastTradedGenerator(generateLastTradeInfo);
}
