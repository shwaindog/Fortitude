﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Generators.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.Generators.MidPrice;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes;

public abstract class TickInstantGeneratorBase<TDetailLevel> : TickGenerator<TDetailLevel> where TDetailLevel : IMutableTickInstant
{
    protected DateTime AdapterReceivedDateTime;
    protected DateTime AdapterSentDateTime;
    protected TickInstantGeneratorBase(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) { }

    public void PopulateQuote(IMutableTickInstant populateQuote, MidPriceTimePair midPriceTimePair)
    {
        var sourceTime = GenerateQuoteValues.SourceTime;
        populateQuote.SourceTime = sourceTime;
        populateQuote.SingleTickValue = decimal.Round(midPriceTimePair.CurrentMid.Mid,
                                                      GenerateQuoteValues.GenerateQuoteInfo.BookGenerationInfo.PriceRoundingDp);
    }
}

public class TickInstantGenerator : TickInstantGeneratorBase<TickInstant>
{
    public TickInstantGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) { }

    public override TickInstant BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new TickInstant();
        PopulateQuote(toPopulate, midPriceTimePair);
        return toPopulate;
    }
}

public abstract class PublishableTickInstantGeneratorBase<TDetailLevel> : TickGenerator<TDetailLevel> where TDetailLevel : IMutablePublishableTickInstant
{
    protected DateTime AdapterReceivedDateTime;
    protected DateTime AdapterSentDateTime;
    protected PublishableTickInstantGeneratorBase(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) { }

    public void PopulateQuote(IMutablePublishableTickInstant populateQuote, MidPriceTimePair midPriceTimePair)
    {
        var sourceTime = GenerateQuoteValues.SourceTime;
        populateQuote.SourceTickerInfo = GenerateQuoteValues.GenerateQuoteInfo.SourceTickerInfo;
        populateQuote.SourceTime       = sourceTime;
        populateQuote.SingleTickValue = decimal.Round(midPriceTimePair.CurrentMid.Mid,
                                                      GenerateQuoteValues.GenerateQuoteInfo.BookGenerationInfo.PriceRoundingDp);
        AdapterReceivedDateTime                    = GenerateQuoteValues.AdapterReceivedTime;
        AdapterSentDateTime                        = GenerateQuoteValues.AdapterSentTime;
        populateQuote.ClientReceivedTime           = GenerateQuoteValues.ClientReceivedTime;

        // this is required by the WeeklyLevel*QuoteTimeSeriesFileTests to stop AdapterSent time from being updated
        populateQuote.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.IsAdapterReplay;
    }
}

public class PublishableTickInstantGenerator : PublishableTickInstantGeneratorBase<PublishableTickInstant>
{
    public PublishableTickInstantGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues) : base(generateQuoteValues) { }

    public override PublishableTickInstant BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber)
    {
        var toPopulate = new PublishableTickInstant();
        PopulateQuote(toPopulate, midPriceTimePair);
        return toPopulate;
    }
}
