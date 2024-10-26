﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Summaries;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.PeriodSummaries.Construction;

public abstract class RequestResponseAttendant
    (IHistoricalPricePeriodSummaryResolverRule constructingRule, HistoricalPeriodResponseRequest responseRequest)
    : SummaryAttendantBase(constructingRule), ISummaryRequestResponseAttendant
{
    protected HistoricalPeriodResponseRequest ResponseRequest = responseRequest;

    public abstract ValueTask<List<PricePeriodSummary>> BuildFromParts(BoundedTimeRange requestRange);
}

public class SubSummaryRequestResponseAttendant
    (IHistoricalPricePeriodSummaryResolverRule constructingRule, HistoricalPeriodResponseRequest responseRequest
      , TimeBoundaryPeriod subSummaryPeriod)
    : RequestResponseAttendant(constructingRule, responseRequest), ISummaryRequestResponseAttendant
{
    public override async ValueTask<List<PricePeriodSummary>> BuildFromParts(BoundedTimeRange requestRange)
    {
        var subPeriodRequest = new HistoricalPeriodResponseRequest(requestRange);
        var subPeriodsInRange
            = await ConstructingRule.RequestAsync<HistoricalPeriodResponseRequest, List<PricePeriodSummary>>
                (((SourceTickerIdentifier)State.PricingInstrumentId).HistoricalPeriodSummaryResponseRequest(subSummaryPeriod), subPeriodRequest);

        if (subPeriodsInRange.Count == 0) return new List<PricePeriodSummary>();
        var remainingPeriods = new List<PricePeriodSummary>();
        var firstSubPeriod   = subPeriodsInRange[0];
        var keepFrom         = (TimeContext.UtcNow - State.CacheTimeSpan).Min(State.ExistingRepoRange.ToTime - State.CacheTimeSpan);
        var currentPeriod    = new PricePeriodSummary(State.Period, State.Period.ContainingPeriodBoundaryStart(firstSubPeriod.PeriodStartTime));
        foreach (var subPeriodHistory in subPeriodsInRange)
        {
            if (!subPeriodHistory.IsWhollyBoundedBy(currentPeriod))
            {
                if (currentPeriod.PeriodEndTime > keepFrom) State.Cache.AddReplace(currentPeriod);
                remainingPeriods.Add(currentPeriod);
                currentPeriod = new PricePeriodSummary(State.Period
                                                     , State.Period.ContainingPeriodBoundaryStart(subPeriodHistory.PeriodStartTime));
            }
            currentPeriod.MergeBoundaries(subPeriodHistory);
        }
        if (currentPeriod.PeriodEndTime > keepFrom) State.Cache.AddReplace(currentPeriod);
        remainingPeriods.Add(currentPeriod);
        return remainingPeriods;
    }
}

public class QuoteToSummaryRequestResponseAttendant<TQuote>
    (IHistoricalPricePeriodSummaryResolverRule constructingRule, HistoricalPeriodResponseRequest responseRequest)
    : RequestResponseAttendant(constructingRule, responseRequest), ISummaryRequestResponseAttendant
    where TQuote : class, ITimeSeriesEntry, ILevel1Quote, new()
{
    public override async ValueTask<List<PricePeriodSummary>> BuildFromParts(BoundedTimeRange requestRange)
    {
        // request ticks build PricePeriodSummaries
        var limitedRecycler = ConstructingRule.Context.PooledRecycler.Borrow<LimitedBlockingRecycler>();
        limitedRecycler.MaxTypeBorrowLimit = 200;
        var remainingPeriods      = new List<PricePeriodSummary>();
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
                remainingPeriods.Add(ce.Event);
            }
            return !ce.IsLastEvent;
        }, limitedRecycler);
        var rebuildRequest = summaryPopulateChannel.ToChannelPublishRequest();
        var req            = new HistoricalPeriodStreamRequest(requestRange, new ResponsePublishParams(rebuildRequest));

        var historicalQuoteAttendant = new QuoteToSummaryStreamRequestAttendant<TQuote>(ConstructingRule, req)
        {
            ReadCacheFromTime = requestRange.ToTime
        };

        var expectResult = await historicalQuoteAttendant.BuildFromParts();

        if (!expectResult)
        {
            rebuildCompleteSource.TrySetResult(true);
            ConstructingRule.Context.PooledRecycler.Recycle(limitedRecycler);
            return new List<PricePeriodSummary>();
        }
        await rebuildCompleteTask;
        return remainingPeriods;
    }
}
