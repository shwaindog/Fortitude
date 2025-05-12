// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Candles.Construction;

public abstract class RequestResponseAttendant
    (IHistoricalCandleResolverRule constructingRule, HistoricalCandleResponseRequest responseRequest)
    : CandleAttendantBase(constructingRule), ICandleRequestResponseAttendant
{
    protected HistoricalCandleResponseRequest ResponseRequest = responseRequest;

    public abstract ValueTask<List<Candle>> BuildFromParts(BoundedTimeRange requestRange);
}

public class SubCandleRequestResponseAttendant
(
    IHistoricalCandleResolverRule constructingRule
  , HistoricalCandleResponseRequest responseRequest
  , TimeBoundaryPeriod subCandlesPeriod)
    : RequestResponseAttendant(constructingRule, responseRequest), ICandleRequestResponseAttendant
{
    public override async ValueTask<List<Candle>> BuildFromParts(BoundedTimeRange requestRange)
    {
        var subPeriodRequest = new HistoricalCandleResponseRequest(requestRange);
        var subPeriodsInRange
            = await ConstructingRule.RequestAsync<HistoricalCandleResponseRequest, List<Candle>>
                (((SourceTickerIdentifier)State.PricingInstrumentId).HistoricalCandleResponseRequest(subCandlesPeriod), subPeriodRequest);

        if (subPeriodsInRange.Count == 0) return new List<Candle>();
        var remainingPeriods = new List<Candle>();
        var firstSubPeriod   = subPeriodsInRange[0];
        var keepFrom         = (TimeContext.UtcNow - State.CacheTimeSpan).Min(State.ExistingRepoRange.ToTime - State.CacheTimeSpan);
        var currentPeriod    = new Candle(State.CandlePeriod, State.CandlePeriod.ContainingPeriodBoundaryStart(firstSubPeriod.PeriodStartTime));
        foreach (var subPeriodHistory in subPeriodsInRange)
        {
            if (!subPeriodHistory.IsWhollyBoundedBy(currentPeriod))
            {
                if (currentPeriod.PeriodEndTime > keepFrom) State.Cache.AddReplace(currentPeriod);
                remainingPeriods.Add(currentPeriod);
                currentPeriod = new Candle(State.CandlePeriod
                                         , State.CandlePeriod.ContainingPeriodBoundaryStart(subPeriodHistory.PeriodStartTime));
            }
            currentPeriod.MergeBoundaries(subPeriodHistory);
        }
        if (currentPeriod.PeriodEndTime > keepFrom) State.Cache.AddReplace(currentPeriod);
        remainingPeriods.Add(currentPeriod);
        return remainingPeriods;
    }
}

public class QuoteToCandleRequestResponseAttendant<TQuote>
    (IHistoricalCandleResolverRule constructingRule, HistoricalCandleResponseRequest responseRequest)
    : RequestResponseAttendant(constructingRule, responseRequest), ICandleRequestResponseAttendant
    where TQuote : class, ITimeSeriesEntry, IPublishableLevel1Quote, new()
{
    public override async ValueTask<List<Candle>> BuildFromParts(BoundedTimeRange requestRange)
    {
        // request ticks build ConflatedTicksCandle
        var limitedRecycler = ConstructingRule.Context.PooledRecycler.Borrow<LimitedBlockingRecycler>();
        limitedRecycler.MaxTypeBorrowLimit = 200;
        var remainingCandles      = new List<Candle>();
        var rebuildCompleteSource = new TaskCompletionSource<bool>();
        var rebuildCompleteTask   = rebuildCompleteSource.Task;

        var keepFrom = (TimeContext.UtcNow - State.CacheTimeSpan).Min(State.ExistingRepoRange.ToTime - State.CacheTimeSpan);
        var summaryPopulateChannel = ConstructingRule.CreateChannelFactory(ce =>
        {
            if (ce.IsLastEvent)
            {
                rebuildCompleteSource.SetResult(true);
            }
            else
            {
                if (ce.Event.PeriodEndTime > keepFrom) State.Cache.AddReplace(ce.Event);
                remainingCandles.Add(ce.Event);
            }
            return !ce.IsLastEvent;
        }, limitedRecycler);
        var rebuildRequest = summaryPopulateChannel.ToChannelPublishRequest();
        var req            = new HistoricalCandleStreamRequest(requestRange, new ResponsePublishParams(rebuildRequest));

        var historicalQuoteAttendant = new QuoteToCandleStreamRequestAttendant<TQuote>(ConstructingRule, req)
        {
            ReadCacheFromTime = requestRange.ToTime
        };

        var expectResult = await historicalQuoteAttendant.BuildFromParts();

        if (!expectResult)
        {
            rebuildCompleteSource.TrySetResult(true);
            ConstructingRule.Context.PooledRecycler.Recycle(limitedRecycler);
            return new List<Candle>();
        }
        await rebuildCompleteTask;
        return remainingCandles;
    }
}
