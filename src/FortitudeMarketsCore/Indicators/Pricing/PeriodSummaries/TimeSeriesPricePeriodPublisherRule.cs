// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;

public struct TimeSeriesPricePeriodParams
{
    public ISourceTickerIdentifier TickerId              { get; set; }
    public TimeSeriesPeriod        PublishPeriod         { get; set; }
    public PricePublishInterval    PublishInterval       { get; set; }
    public IndicatorPublishParams  LivePublishParams     { get; set; }
    public IndicatorPublishParams  CompletePublishParams { get; set; }
}

public class PeriodSummaryState : ITimeSeriesPeriodRange
{
    public BidAskPair? NextPeriodBidAskStart;

    public BidAskPair? PreviousPeriodBidAskEnd;

    public IDoublyLinkedList<IPricePeriodSummary> SummaryPricePeriods = new DoublyLinkedList<IPricePeriodSummary>();

    public PeriodSummaryState(DateTime periodStartTime, TimeSeriesPeriod timeSeriesPeriod)
    {
        PeriodStartTime  = periodStartTime;
        TimeSeriesPeriod = timeSeriesPeriod;
    }

    public bool             HasPublishedComplete { get; set; }
    public DateTime         PeriodStartTime      { get; set; }
    public TimeSeriesPeriod TimeSeriesPeriod     { get; set; }

    public BoundedTimeRange ToBoundedTimeRange
        (DateTime? maxDateTime = null) =>
        new(PeriodStartTime, TimeSeriesPeriod.PeriodEnd(PeriodStartTime).Min(maxDateTime));

    public void Configure(DateTime periodStartTime, TimeSeriesPeriod timeSeriesPeriod)
    {
        PeriodStartTime  = periodStartTime;
        TimeSeriesPeriod = timeSeriesPeriod;
    }

    public PricePeriodSummaryFlags SummaryFlagsAsAt(IRecycler recycler, DateTime? currentTime = null)
    {
        var now = currentTime ?? DateTime.UtcNow;

        var nowPeriodStart = TimeSeriesPeriod.ContainingPeriodBoundaryStart(now);
        var currentFlags   = nowPeriodStart == PeriodStartTime ? PricePeriodSummaryFlags.PeriodLatest : PricePeriodSummaryFlags.None;
        currentFlags |= PreviousPeriodBidAskEnd != null ? PricePeriodSummaryFlags.CreatedFromPreviousEnd : PricePeriodSummaryFlags.None;

        var boundedPeriodToNow = new BoundedTimeRange(PeriodStartTime, now);

        var    currentPeriodSummary = SummaryPricePeriods.Tail;
        ushort missingTickPeriods   = 0;

        foreach (var subRange in this.Reverse16SubTimeRanges(recycler))
        {
            if (subRange.IntersectsWith(boundedPeriodToNow))
            {
                currentPeriodSummary ??= SummaryPricePeriods.Tail;
                var previousPeriodSummary = currentPeriodSummary?.Next;

                var subRangeCurrentIntersection = subRange.Intersection(boundedPeriodToNow)!.Value;

                while (currentPeriodSummary != null && currentPeriodSummary.PeriodStartTime < subRangeCurrentIntersection.ToTime &&
                       (previousPeriodSummary == null || previousPeriodSummary.PeriodStartTime > subRangeCurrentIntersection.ToTime))
                    if (currentPeriodSummary.PeriodStartTime >= subRangeCurrentIntersection.ToTime)
                    {
                        previousPeriodSummary = currentPeriodSummary;
                        currentPeriodSummary  = currentPeriodSummary.Previous;
                    }
                    else if (previousPeriodSummary != null && previousPeriodSummary.PeriodStartTime < subRangeCurrentIntersection.ToTime)
                    {
                        currentPeriodSummary  = previousPeriodSummary;
                        previousPeriodSummary = previousPeriodSummary.Next;
                    }
                var percentageComplete = 0.0;
                while (currentPeriodSummary != null && currentPeriodSummary.PeriodEndTime > subRangeCurrentIntersection.FromTime)
                {
                    percentageComplete   += currentPeriodSummary.ContributingCompletePercentage(subRangeCurrentIntersection, recycler);
                    currentPeriodSummary =  currentPeriodSummary.Previous;
                }
                if (percentageComplete < 0.5) missingTickPeriods |= 1;
            }
            missingTickPeriods <<= 1;
        }
        currentFlags |= (PricePeriodSummaryFlags)((uint)missingTickPeriods << 16);

        if (missingTickPeriods == 0 && nowPeriodStart != PeriodStartTime && NextPeriodBidAskStart != null)
            currentFlags |= PricePeriodSummaryFlags.IsBestPossible;

        return currentFlags;
    }
}

public class TimeSeriesPricePeriodPublisherRule<TQuote> : PriceListenerIndicatorRule<TQuote> where TQuote : class, ILevel1Quote
{
    private readonly Dictionary<TimeSeriesPeriod, ISubscription> historicalPeriodsSubscriptions = new();
    private readonly TimeSeriesPeriod                            periodToPublish;
    private readonly ISourceTickerIdentifier                     tickerId;

    private string                 completeAddress;
    private IndicatorPublishParams completePublishParams;
    private PeriodSummaryState     currentPeriodSummaryState;
    private PeriodSummaryState?    lastPeriodSummaryState;
    private string                 liveAddress;

    private IndicatorPublishParams livePublishParams;

    private TimeSeriesPricePeriodParams pricingPeriodParams;

    private List<TimeSeriesPeriodRange> startHistoricalPeriods;

    public TimeSeriesPricePeriodPublisherRule(TimeSeriesPricePeriodParams pricingPeriodParams)
        : base(pricingPeriodParams.TickerId
             , nameof(TimeSeriesPricePeriodPublisherRule<TQuote>)
             + $"_{pricingPeriodParams.TickerId.Source}_{pricingPeriodParams.TickerId.Source}_{pricingPeriodParams.PublishPeriod.ShortName()}")
    {
        this.pricingPeriodParams = pricingPeriodParams;
        tickerId                 = pricingPeriodParams.TickerId;
        periodToPublish          = pricingPeriodParams.PublishPeriod;
        livePublishParams        = pricingPeriodParams.LivePublishParams;
        if (livePublishParams.PublishSelection == PublishSelection.DefaultIndicatorBroadcastAddress)
            livePublishParams.AlternativePublishAddress = tickerId.LivePeriodSummaryAddress(periodToPublish);
        else if (livePublishParams.PublishSelection == PublishSelection.ReceiverChannel)
            if (livePublishParams.ChannelRequest!.Channel is not IChannel<IPricePeriodSummary>)
                throw new Exception("Expected channel to be of type IPricePeriodSummary");
        completePublishParams = pricingPeriodParams.CompletePublishParams;

        if (completePublishParams.PublishSelection == PublishSelection.DefaultIndicatorBroadcastAddress)
            completePublishParams.AlternativePublishAddress = tickerId.CompletePeriodSummaryAddress(periodToPublish);
        else if (completePublishParams.PublishSelection == PublishSelection.ReceiverChannel)
            if (completePublishParams.ChannelRequest!.Channel is not IChannel<IPricePeriodSummary>)
                throw new Exception("Expected channel to be of type IPricePeriodSummary");
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        var now                        = DateTime.UtcNow;
        var currentLivePeriodStartTime = periodToPublish.ContainingPeriodBoundaryStart(now);
        currentPeriodSummaryState = new PeriodSummaryState(currentLivePeriodStartTime, periodToPublish);
        // subscribe to prices and start caching
        startHistoricalPeriods = periodToPublish.ConstructingDivisiblePeriods(now);

        foreach (var timeSeriesPeriod in startHistoricalPeriods.Select(tspr => tspr.TimeSeriesPeriod).Distinct())
        {
            var historicalPeriodSubscription = await this.RegisterListenerAsync<IPricePeriodSummary>
                (tickerId.HistoricalPeriodSummaryAddress(timeSeriesPeriod), CurrentPeriodHistorical);
            historicalPeriodsSubscriptions.Add(timeSeriesPeriod, historicalPeriodSubscription);
        }


        // get summaries that can summarize as much of the current period range as possible
    }

    private void PublishSummaryTo(PeriodSummaryState periodState, IndicatorPublishParams publishParams, DateTime? atTime = null)
    {
        var priceSummary = BuildPeriodSummary(periodState, atTime);
        if (publishParams.PublishSelection is PublishSelection.AlternativeAddress or PublishSelection.DefaultIndicatorBroadcastAddress)
        {
            this.Publish(publishParams.AlternativePublishAddress!, priceSummary, publishParams.PublishDispatchOptions);
        }
        else
        {
            var publishChannel = (IChannel<IPricePeriodSummary>)publishParams.ChannelRequest!.Channel;
            publishChannel.Publish(this, priceSummary);
        }
    }

    private IPricePeriodSummary BuildPeriodSummary(PeriodSummaryState periodState, DateTime? atTime = null)
    {
        var toPopulate = Context.PooledRecycler.Borrow<PricePeriodSummary>();
        toPopulate.TimeSeriesPeriod = periodState.TimeSeriesPeriod;
        toPopulate.PeriodStartTime  = periodState.PeriodStartTime;
        toPopulate.PeriodEndTime    = periodState.PeriodEnd();

        var runningTimeWeightedBidAverage = 0m;
        var runningTimeWeightedAskAverage = 0m;
        if (periodState is { PreviousPeriodBidAskEnd: not null, SummaryPricePeriods.Head: QuoteWrappingPricePeriodSummary quoteSummary })
        {
            var periodToQuoteMs = (decimal)(quoteSummary.PeriodStartTime - periodState.PeriodStartTime).TotalMilliseconds;
            runningTimeWeightedBidAverage = periodToQuoteMs * periodState.PreviousPeriodBidAskEnd!.Value.BidPrice;
            runningTimeWeightedAskAverage = periodToQuoteMs * periodState.PreviousPeriodBidAskEnd!.Value.AskPrice;
        }
        var currentPeriodSummary = periodState.SummaryPricePeriods.Head;
        while (currentPeriodSummary != null)
        {
            var currentPeriodBoundedMs = (decimal)currentPeriodSummary.ToBoundedTimeRange(toPopulate.PeriodEndTime).TimeSpan().TotalMilliseconds;

            runningTimeWeightedBidAverage += currentPeriodBoundedMs * currentPeriodSummary.AverageBidAsk.BidPrice;
            runningTimeWeightedAskAverage += currentPeriodBoundedMs * currentPeriodSummary.AverageBidAsk.AskPrice;

            toPopulate.Merge(currentPeriodSummary);
            currentPeriodSummary = currentPeriodSummary.Next;
        }
        toPopulate.AverageBidPrice = runningTimeWeightedBidAverage;
        toPopulate.AverageAskPrice = runningTimeWeightedAskAverage;
        var now = atTime ?? DateTime.UtcNow;

        toPopulate.PeriodSummaryFlags = periodState.SummaryFlagsAsAt(Context.PooledRecycler, now);

        return toPopulate;
    }

    private async ValueTask CurrentPeriodHistorical(IBusMessage<IPricePeriodSummary> historicalPeriodMsg)
    {
        var subPeriod       = historicalPeriodMsg.Payload.Body();
        var timeSeriesRange = subPeriod.AsTimeSeriesPeriodRange();
        if (startHistoricalPeriods.Contains(timeSeriesRange))
        {
            startHistoricalPeriods.Remove(timeSeriesRange);
            var containingStartTime = subPeriod.TimeSeriesPeriod.ContainingPeriodBoundaryStart(subPeriod.PeriodStartTime);
            if (currentPeriodSummaryState.PeriodStartTime == containingStartTime)
            {
                currentPeriodSummaryState.SummaryPricePeriods.AddReplace(subPeriod, Context.PooledRecycler);
            }
            else if (lastPeriodSummaryState != null &&
                     lastPeriodSummaryState.TimeSeriesPeriod.ContainingPeriodBoundaryStart(lastPeriodSummaryState.PeriodStartTime) ==
                     containingStartTime)
            {
                lastPeriodSummaryState.SummaryPricePeriods.AddReplace(subPeriod, Context.PooledRecycler);
            }
            else
            {
                foreach (var historicalPeriodsSubscription in historicalPeriodsSubscriptions)
                    await historicalPeriodsSubscription.Value.UnsubscribeAsync();
                historicalPeriodsSubscriptions.Clear();
                startHistoricalPeriods.Clear();
            }
        }
    }

    protected override async ValueTask ReceiveQuoteHandler(IBusMessage<TQuote> priceQuoteMessage)
    {
        var quote = priceQuoteMessage.Payload.Body();

        var quotePeriodWrapper = Context.PooledRecycler.Borrow<QuoteWrappingPricePeriodSummary>();
        quotePeriodWrapper.Configure(quote);
        var quoteStartPeriod = periodToPublish.ContainingPeriodBoundaryStart(quote.SourceTime);
        if (currentPeriodSummaryState.PeriodStartTime == quoteStartPeriod)
        {
            currentPeriodSummaryState.SummaryPricePeriods.AddReplace(quotePeriodWrapper, Context.PooledRecycler);
        }
        else if (quoteStartPeriod > currentPeriodSummaryState.PeriodStartTime)
        {
            var noLastSummaryPeriod = lastPeriodSummaryState == null;
            currentPeriodSummaryState.NextPeriodBidAskStart = quote.BidAskTop;

            if (noLastSummaryPeriod)
            {
                lastPeriodSummaryState = currentPeriodSummaryState;
                currentPeriodSummaryState = new PeriodSummaryState(quoteStartPeriod, periodToPublish)
                {
                    PreviousPeriodBidAskEnd = lastPeriodSummaryState.SummaryPricePeriods.Tail?.EndBidAsk
                };
            }
            else
            {
                var lastPeriodEnd                = lastPeriodSummaryState!.SummaryPricePeriods.Tail?.EndBidAsk;
                var newCurrentPeriodSummaryState = ClearExisting(lastPeriodSummaryState);
                lastPeriodSummaryState    = currentPeriodSummaryState;
                currentPeriodSummaryState = newCurrentPeriodSummaryState;
                currentPeriodSummaryState.Configure(quoteStartPeriod, periodToPublish);
                currentPeriodSummaryState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentPeriodSummaryState.SummaryPricePeriods.AddFirst(quotePeriodWrapper);
            if (!noLastSummaryPeriod)
            {
                foreach (var historicalPeriodsSubscription in historicalPeriodsSubscriptions)
                    await historicalPeriodsSubscription.Value.UnsubscribeAsync();
                historicalPeriodsSubscriptions.Clear();
                startHistoricalPeriods.Clear();
            }
        }
    }

    private PeriodSummaryState ClearExisting(PeriodSummaryState toClear)
    {
        var currentSummary = toClear.SummaryPricePeriods.Head;
        while (currentSummary != null)
        {
            var removed = toClear.SummaryPricePeriods.Remove(currentSummary);
            removed.DecrementRefCount();

            currentSummary = currentSummary.Next;
        }
        toClear.NextPeriodBidAskStart   = null;
        toClear.PreviousPeriodBidAskEnd = null;
        toClear.HasPublishedComplete    = false;
        return toClear;
    }
}
