// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.Generators;

public abstract class Level1QuoteGeneratorBase<TQuote> : Level0QuoteGeneratorBase<TQuote> where TQuote : Level1PriceQuote
{
    protected Level1QuoteGeneratorBase(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public void PopulateQuote(Level1PriceQuote populateQuote, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        base.PopulateQuote(populateQuote, previousCurrentMidPriceTime);
        populateQuote.Executable          = PseudoRandom.Next(0, 1000) < 995;
        populateQuote.SourceAskTime       = populateQuote.SourceTime;
        populateQuote.SourceBidTime       = populateQuote.SourceTime;
        populateQuote.AdapterReceivedTime = AdapterReceivedDateTime;
        populateQuote.AdapterSentTime     = AdapterSentDateTime;
        if (populateQuote.QuoteLevel == QuoteLevel.Level1)
        {
            var bookGenerationInfo = GenerateQuoteInfo.BookGenerationInfo;

            var topBidAskSpread = Math.Max(bookGenerationInfo.TightestSpreadPips,
                                           bookGenerationInfo.AverageSpreadPips +
                                           (decimal)NormalDist.Sample() * bookGenerationInfo.SpreadStandardDeviation);
            var mid           = previousCurrentMidPriceTime.CurrentMid.Mid;
            var roundedTopBid = decimal.Round(mid - topBidAskSpread / 2);
            var roundedTopAsk = decimal.Round(mid + topBidAskSpread / 2);

            while (roundedTopAsk - roundedTopBid > bookGenerationInfo.TightestSpreadPips)
                roundedTopBid -= bookGenerationInfo.SmallestPriceLayerPips;

            populateQuote.AskPriceTop          = roundedTopAsk;
            populateQuote.BidPriceTop          = roundedTopBid;
            populateQuote.IsAskPriceTopUpdated = (PreviousReturnedQuote?.AskPriceTop ?? 0) != populateQuote.AskPriceTop;
            populateQuote.IsBidPriceTopUpdated = (PreviousReturnedQuote?.BidPriceTop ?? 0) != populateQuote.BidPriceTop;
        }
    }
}

public class Level1QuoteGenerator : Level1QuoteGeneratorBase<Level1PriceQuote>
{
    public Level1QuoteGenerator(GenerateQuoteInfo generateQuoteInfo) : base(generateQuoteInfo) { }

    public override Level1PriceQuote BuildQuote(PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var toPopulate = new Level1PriceQuote(GenerateQuoteInfo.SourceTickerQuoteInfo);
        PopulateQuote(toPopulate, previousCurrentMidPriceTime);
        return toPopulate;
    }
}
