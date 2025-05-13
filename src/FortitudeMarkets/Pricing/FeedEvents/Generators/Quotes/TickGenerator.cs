// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Generators.Quotes;

public interface ITickGenerator<out TDetailLevel> where TDetailLevel : IMutableTickInstant
{
    TDetailLevel Next { get; }

    IEnumerable<TDetailLevel> Quotes(DateTime startDateTime, TimeSpan averageInterval, int numToGenerate, int sequenceNumber = 0);
}

public abstract class TickGenerator<TDetailLevel> : ITickGenerator<TDetailLevel> where TDetailLevel : IMutableTickInstant
{
    private readonly   TDetailLevel[]                    cycleQuotes = [];
    protected readonly CurrentQuoteInstantValueGenerator GenerateQuoteValues;
    private readonly   TimeSpan                          singleQuoteInterval;

    private int cyclePosition = 0;

    private int nextSingleQuoteSequenceNumber;

    private DateTime nextSingleQuoteStartTime;

    protected TDetailLevel? PreviousReturnedQuote;

    protected TickGenerator(CurrentQuoteInstantValueGenerator generateQuoteValues)
    {
        GenerateQuoteValues           = generateQuoteValues;
        nextSingleQuoteStartTime      = generateQuoteValues.GenerateQuoteInfo.SingleQuoteStartTime;
        singleQuoteInterval           = generateQuoteValues.GenerateQuoteInfo.SingleQuoteNextQuoteIncrement;
        nextSingleQuoteSequenceNumber = generateQuoteValues.GenerateQuoteInfo.SingleQuoteStartSequenceNumber;
        if (generateQuoteValues.GenerateQuoteInfo.CycleQuotesAmount > 0)
        {
            var preGeneratedQuotes = new TDetailLevel[generateQuoteValues.GenerateQuoteInfo.CycleQuotesAmount];
            for (var i = 0; i < generateQuoteValues.GenerateQuoteInfo.CycleQuotesAmount - 1; i++) preGeneratedQuotes[i] = Next;
            cycleQuotes = preGeneratedQuotes;
        }
    }

    public TDetailLevel Next
    {
        get
        {
            if (cycleQuotes.Length > 0)
            {
                var cycleIndex = cyclePosition % cycleQuotes.Length;
                PreviousReturnedQuote    =  cycleQuotes[cycleIndex];
                nextSingleQuoteStartTime += singleQuoteInterval;
                if (cyclePosition++ >= cycleQuotes.Length)
                {
                    PreviousReturnedQuote.IncrementTimeBy(cycleQuotes.Length * singleQuoteInterval);
                    if (PreviousReturnedQuote is IPQPublishableTickInstant pqTickInstant) pqTickInstant.HasUpdates = true;
                }
            }
            else
            {
                var prevCurrMid =
                    GenerateQuoteValues
                        .GenerateQuoteInfo
                        .MidPriceGenerator
                        .PreviousCurrentPrices(nextSingleQuoteStartTime, singleQuoteInterval, 1
                                             , nextSingleQuoteSequenceNumber++)
                        .First();
                nextSingleQuoteStartTime += singleQuoteInterval;

                GenerateQuoteValues.NextQuoteValuesInitialise(prevCurrMid);
                PreviousReturnedQuote = BuildQuote(prevCurrMid, nextSingleQuoteSequenceNumber);
            }
            return PreviousReturnedQuote;
        }
    }

    public IEnumerable<TDetailLevel> Quotes(DateTime startingFromTime, TimeSpan averageInterval, int numToGenerate, int sequenceNumber = 0)
    {
        var currentSeqNum = sequenceNumber;
        foreach (var prevCurrMids in GenerateQuoteValues.GenerateQuoteInfo.MidPriceGenerator
                                                        .PreviousCurrentPrices(startingFromTime, averageInterval, numToGenerate, sequenceNumber))
        {
            GenerateQuoteValues.NextQuoteValuesInitialise(prevCurrMids);
            PreviousReturnedQuote = BuildQuote(prevCurrMids, currentSeqNum++);
            yield return PreviousReturnedQuote;
        }
    }

    public abstract TDetailLevel BuildQuote(MidPriceTimePair midPriceTimePair, int sequenceNumber);
}
