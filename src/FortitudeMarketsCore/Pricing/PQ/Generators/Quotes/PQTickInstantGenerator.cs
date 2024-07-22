// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsCore.Pricing.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Generators.Quotes;

public class PQTickInstantGenerator : TickInstantGeneratorBase<PQTickInstant>
{
    public PQTickInstantGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override PQTickInstant BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime, int sequenceNumber)
    {
        var toPopulate = new PQTickInstant(GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        toPopulate.PQSequenceId = (uint)sequenceNumber;
        return toPopulate;
    }
}
