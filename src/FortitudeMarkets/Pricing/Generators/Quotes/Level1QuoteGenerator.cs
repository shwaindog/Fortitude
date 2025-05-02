// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes;

public abstract class Level1QuoteGeneratorBase<TQuote> : TickInstantGeneratorBase<TQuote> where TQuote : IMutableLevel1Quote
{
    protected Level1QuoteGeneratorBase(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) { }

    public void PopulateQuote(IMutableLevel1Quote populateQuote, MidPriceTimePair midPriceTimePair)
    {
        base.PopulateQuote(populateQuote, midPriceTimePair);
        populateQuote.Executable          = GenerateQuoteValues.IsExecutable;
        populateQuote.SourceAskTime       = GenerateQuoteValues.SourceAskTime;
        populateQuote.SourceBidTime       = GenerateQuoteValues.SourceBidTime;
        populateQuote.AdapterReceivedTime = GenerateQuoteValues.AdapterReceivedTime;
        populateQuote.AdapterSentTime     = GenerateQuoteValues.AdapterSentTime;
        populateQuote.ValidFrom           = GenerateQuoteValues.ValidFrom;
        populateQuote.ValidTo             = GenerateQuoteValues.ValidTo;
        if (populateQuote.TickerDetailLevel == TickerDetailLevel.Level1Quote)
        {
            var (roundedTopBid, roundedTopAsk) = GenerateQuoteValues.BookGenerator.TopBidAskPrice;
            populateQuote.AskPriceTop          = roundedTopAsk;
            populateQuote.BidPriceTop          = roundedTopBid;
            populateQuote.IsAskPriceTopChanged = (PreviousReturnedQuote?.AskPriceTop ?? 0) != populateQuote.AskPriceTop;
            populateQuote.IsBidPriceTopChanged = (PreviousReturnedQuote?.BidPriceTop ?? 0) != populateQuote.BidPriceTop;
        }
    }
}

public class Level1QuoteGenerator : Level1QuoteGeneratorBase<Level1PriceQuote>
{
    public Level1QuoteGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) { }

    public override Level1PriceQuote BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new Level1PriceQuote(GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo);
        PopulateQuote(toPopulate, midPriceTimePair);
        return toPopulate;
    }
}
