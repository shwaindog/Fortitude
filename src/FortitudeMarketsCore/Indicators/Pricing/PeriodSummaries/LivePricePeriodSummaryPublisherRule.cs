// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries.Construction;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.Summaries;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;

public struct LivePublishPricePeriodSummaryParams
{
    public LivePublishPricePeriodSummaryParams(ISourceTickerId tickerId, TimeSeriesPeriod publishPeriod)
    {
        PublishPeriod = publishPeriod;
        TickerId      = tickerId;

        LivePublishInterval   = new PricePublishInterval(OneSecond);
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishPricePeriodSummaryParams(ISourceTickerId tickerId, TimeSeriesPeriod publishPeriod, PricePublishInterval livePublishInterval)
    {
        PublishPeriod = publishPeriod;
        TickerId      = tickerId;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishPricePeriodSummaryParams
        (ISourceTickerId tickerId, TimeSeriesPeriod publishPeriod, PricePublishInterval livePublishInterval, ResponsePublishParams livePublishParams)
    {
        PublishPeriod = publishPeriod;
        TickerId      = tickerId;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = livePublishParams;
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishPricePeriodSummaryParams
    (ISourceTickerId tickerId, TimeSeriesPeriod publishPeriod, PricePublishInterval livePublishInterval
      , ResponsePublishParams livePublishParams, ResponsePublishParams completePublishParams)
    {
        PublishPeriod = publishPeriod;
        TickerId      = tickerId;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = livePublishParams;
        CompletePublishParams = completePublishParams;
    }

    public ISourceTickerId       TickerId            { get; set; }
    public TimeSeriesPeriod      PublishPeriod       { get; set; }
    public PricePublishInterval? LivePublishInterval { get; set; }
    public ResponsePublishParams LivePublishParams   { get; set; }

    public ResponsePublishParams CompletePublishParams { get; set; }
}

public class LivePricePeriodSummaryPublisherRule<TQuote> : PriceListenerIndicatorRule<TQuote> where TQuote : class, ILevel1Quote
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(LivePricePeriodSummaryPublisherRule<TQuote>));

    private readonly ResponsePublishParams completeResponsePublishParams;
    private readonly PricePublishInterval? livePricePublishInterval;
    private readonly ResponsePublishParams liveResponsePublishParams;

    private readonly Dictionary<TimeSeriesPeriod, ISubscription> liveSubPeriodCompletePublisherSubscriptions = new();

    private readonly TimeSeriesPeriod periodToPublish;
    private readonly ISourceTickerId  tickerId;

    private List<ValueTask>? asyncSubPeriodExecutions = new();

    private ITimerUpdate completeIntervalTimer = null!;

    private PeriodSummaryState  currentPeriodSummaryState = null!;
    private PeriodSummaryState? lastPeriodSummaryState;

    private ITimerUpdate? liveIntervalTimer;

    public LivePricePeriodSummaryPublisherRule(LivePublishPricePeriodSummaryParams livePublishPricePeriodSummaryParams)
        : base(livePublishPricePeriodSummaryParams.TickerId
             , nameof(LivePricePeriodSummaryPublisherRule<TQuote>)
             + $"_{livePublishPricePeriodSummaryParams.TickerId.Source}_{livePublishPricePeriodSummaryParams.TickerId.Source}" +
               $"_{livePublishPricePeriodSummaryParams.PublishPeriod.ShortName()}")
    {
        tickerId                  = livePublishPricePeriodSummaryParams.TickerId;
        periodToPublish           = livePublishPricePeriodSummaryParams.PublishPeriod;
        liveResponsePublishParams = livePublishPricePeriodSummaryParams.LivePublishParams;
        if (liveResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
            liveResponsePublishParams.AlternativePublishAddress = tickerId.LivePeriodSummaryAddress(periodToPublish);
        else if (liveResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
            if (liveResponsePublishParams.ChannelRequest!.Channel is not IChannel<IPricePeriodSummary>)
                throw new Exception("Expected channel to be of type IPricePeriodSummary");
        completeResponsePublishParams = livePublishPricePeriodSummaryParams.CompletePublishParams;

        if (completeResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
            completeResponsePublishParams.AlternativePublishAddress = tickerId.CompletePeriodSummaryAddress(periodToPublish);
        else if (completeResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
            if (completeResponsePublishParams.ChannelRequest!.Channel is not IChannel<IPricePeriodSummary>)
                throw new Exception("Expected channel to be of type IPricePeriodSummary");
        livePricePublishInterval = livePublishPricePeriodSummaryParams.LivePublishInterval;
    }

    public void StartLiveTimeSeriesPeriodInterval()
    {
        liveIntervalTimer = Timer.RunEvery(livePricePublishInterval!.Value.PublishPeriod!.Value.AveragePeriodTimeSpan(), PublishLivePeriod);
        PublishLivePeriod();
    }

    public void StartCompleteTimeSeriesPeriodInterval()
    {
        completeIntervalTimer = Timer.RunEvery
            (periodToPublish.AveragePeriodTimeSpan().Min(TimeSpan.FromDays(1)), PublishCompletePeriod);
        PublishCompletePeriod();
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();

        completeIntervalTimer
            = Timer.RunAt(periodToPublish.ContainingPeriodEnd(TimeContext.UtcNow) + (periodToPublish.AveragePeriodTimeSpan() / 2).Min(TimeSpan.FromSeconds(5))
                        , StartCompleteTimeSeriesPeriodInterval);

        if (livePricePublishInterval?.PriceIndicatorPublishType == PriceIndicatorPublishType.SetTimeSpan)
            liveIntervalTimer = Timer.RunEvery(livePricePublishInterval!.Value.PublishTimeSpan!.Value, PublishLivePeriod);
        if (livePricePublishInterval?.PriceIndicatorPublishType == PriceIndicatorPublishType.TimeSeriesPeriod)
            liveIntervalTimer = Timer.RunAt(livePricePublishInterval!.Value.PublishPeriod!.Value.ContainingPeriodEnd(TimeContext.UtcNow)
                                          , StartLiveTimeSeriesPeriodInterval);
        var now = TimeContext.UtcNow;

        var currentLivePeriodStartTime = periodToPublish.ContainingPeriodBoundaryStart(now);
        currentPeriodSummaryState = new PeriodSummaryState(currentLivePeriodStartTime, periodToPublish);
        // subscribe to prices and start caching
        var startHistoricalPeriods =
            periodToPublish.ConstructingDivisiblePeriods(now)
                           .Where(tsp => tsp.TimeSeriesPeriod >= PricePeriodSummaryConstants.PersistPeriodsFrom);

        foreach (var timeSeriesPeriod in startHistoricalPeriods.Select(tspr => tspr.TimeSeriesPeriod).Distinct())
        {
            var subPeriodHistoricalLastTime = timeSeriesPeriod.ContainingPeriodBoundaryStart(now);
            var boundedTime                 = new BoundedTimeRange(currentLivePeriodStartTime, subPeriodHistoricalLastTime);
            var requestHistorical           = new HistoricalPeriodResponseRequest(boundedTime);
            asyncSubPeriodExecutions?.Add(RequestHistoricalSubPeriods(timeSeriesPeriod, requestHistorical));
        }

        EnsureLiveSubPeriodSummaryPublishersRunning();
    }

    public override async ValueTask StopAsync()
    {
        liveIntervalTimer?.Cancel();
        completeIntervalTimer.Cancel();
        foreach (var subPeriodCompleteSubscription in liveSubPeriodCompletePublisherSubscriptions.Values)
            await subPeriodCompleteSubscription.NullSafeUnsubscribe();
        if (asyncSubPeriodExecutions?.Any() == true)
            Logger.Warn("Incomplete sub period complete subscription or historical period request before LivePricePeriodSummaryPublisherRule " +
                        "shut down for tickerIdl {0} and period {1}", tickerId, periodToPublish);
    }

    private void EnsureLiveSubPeriodSummaryPublishersRunning()
    {
        if (periodToPublish > FiveMinutes)
        {
            foreach (var subPeriod in periodToPublish.ConstructingDivisiblePeriods().Where(tsp => tsp >= FiveMinutes))
                asyncSubPeriodExecutions?.Add(LaunchAndSubscribeToLiveSubPeriod(subPeriod));
        }
        else
        {
            var subPeriod = periodToPublish.GranularDivisiblePeriod();
            if (subPeriod >= PricePeriodSummaryConstants.PersistPeriodsFrom)
                asyncSubPeriodExecutions?.Add(LaunchAndSubscribeToLiveSubPeriod(subPeriod));
        }
    }

    private async ValueTask LaunchAndSubscribeToLiveSubPeriod(TimeSeriesPeriod subPeriod)
    {
        var tickerSubPeriodService = new TickerPeriodServiceRequest
            (RequestType.StartOrStatus, ServiceType.LivePricePeriodSummary, tickerId, subPeriod
           , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>(), PQQuoteConverterExtensions.IsPQQuoteType<TQuote>());

        var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);
        if (!response.IsRunning())
        {
            Logger.Info("Problem starting LivePricePeriodSummaryPublisherRule for ticker {0} sub period {1} got {2}",
                        tickerId.ShortName(), subPeriod.ShortName(), response.RunStatus);
            return;
        }
        var completeLiveSubSummarySubscription
            = await this.RegisterListenerAsync<PricePeriodSummary>(tickerId.CompletePeriodSummaryAddress(subPeriod), ReceiveSubPeriodHandler);
        liveSubPeriodCompletePublisherSubscriptions.Add(subPeriod, completeLiveSubSummarySubscription);
    }

    private void PublishLivePeriod()
    {
        PublishSummaryTo(currentPeriodSummaryState, liveResponsePublishParams, TimeContext.UtcNow);
    }

    private void PublishCompletePeriod()
    {
        var now = TimeContext.UtcNow;
        if (lastPeriodSummaryState is { HasPublishedComplete: false, SubSummaryPeriods.Head: not null }
                                   or { HasPublishedComplete: false, PreviousPeriodBidAskEnd: not null })
        {
            PublishSummaryTo(lastPeriodSummaryState!, completeResponsePublishParams, now);
            lastPeriodSummaryState.HasPublishedComplete = true;
        }
        else
        {
            var currentPeriodStart = periodToPublish.ContainingPeriodBoundaryStart(now);
            if (currentPeriodSummaryState is { HasPublishedComplete: false, SubSummaryPeriods.Head: not null }
                                          or { HasPublishedComplete: false, PreviousPeriodBidAskEnd: not null }
             && currentPeriodSummaryState.PeriodStartTime < currentPeriodStart)
            {
                PublishSummaryTo(currentPeriodSummaryState!, completeResponsePublishParams, now);
                currentPeriodSummaryState.HasPublishedComplete = true;

                var lastPeriodEnd                = lastPeriodSummaryState!.SubSummaryPeriods.Tail?.EndBidAsk;
                var newCurrentPeriodSummaryState = ClearExisting(lastPeriodSummaryState);
                lastPeriodSummaryState    = currentPeriodSummaryState;
                currentPeriodSummaryState = newCurrentPeriodSummaryState;
                currentPeriodSummaryState.Configure(currentPeriodStart, periodToPublish);
                currentPeriodSummaryState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
        }
    }

    private void PublishSummaryTo(PeriodSummaryState periodState, ResponsePublishParams responsePublishParams, DateTime? atTime = null)
    {
        if (periodState.SubSummaryPeriods.Head == null && periodState.PreviousPeriodBidAskEnd == null) return;
        var priceSummary = periodState.BuildPeriodSummary(Context.PooledRecycler, atTime);
        if (responsePublishParams.ResponsePublishMethod is ResponsePublishMethod.AlternativeBroadcastAddress
                                                        or ResponsePublishMethod.ListenerDefaultBroadcastAddress)
        {
            this.Publish(responsePublishParams.AlternativePublishAddress!, priceSummary, responsePublishParams.PublishDispatchOptions);
        }
        else
        {
            var publishChannel = (IChannel<IPricePeriodSummary>)responsePublishParams.ChannelRequest!.Channel;
            publishChannel.Publish(this, priceSummary);
        }
        if (asyncSubPeriodExecutions != null && !asyncSubPeriodExecutions.Any()) asyncSubPeriodExecutions = null;
    }

    private async ValueTask RequestHistoricalSubPeriods(TimeSeriesPeriod liveConstructingSubPeriod, HistoricalPeriodResponseRequest requestRange)
    {
        var tickerSubPeriodService = new TickerPeriodServiceRequest
            (RequestType.StartOrStatus, ServiceType.HistoricalPricePeriodSummaryResolver, tickerId, liveConstructingSubPeriod
           , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>(), PQQuoteConverterExtensions.IsPQQuoteType<TQuote>());

        var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);
        if (!response.IsRunning())
        {
            Logger.Info("Problem starting HistoricalPeriodSummariesResolverRule for ticker {0} sub period {1} got {2}",
                        tickerId.ShortName(), liveConstructingSubPeriod.ShortName(), response.RunStatus);
            return;
        }
        var subPeriods = await this.RequestAsync<HistoricalPeriodResponseRequest, List<PricePeriodSummary>>
            (tickerId.HistoricalPeriodSummaryResponseRequest(liveConstructingSubPeriod), requestRange);
        foreach (var subPeriod in subPeriods)
            if (subPeriod.IsWhollyBoundedBy(currentPeriodSummaryState))
                currentPeriodSummaryState.SubSummaryPeriods.AddReplace(subPeriod, Context.PooledRecycler);
            else if (lastPeriodSummaryState != null && subPeriod.IsWhollyBoundedBy(lastPeriodSummaryState))
                lastPeriodSummaryState.SubSummaryPeriods.AddReplace(subPeriod, Context.PooledRecycler);
    }

    private ValueTask ReceiveSubPeriodHandler(IBusMessage<PricePeriodSummary> subPeriodMsg)
    {
        var subSummary = subPeriodMsg.Payload.Body();
        return AddToCurrentOrPreviousState(subSummary);
    }

    protected override ValueTask ReceiveQuoteHandler(IBusMessage<TQuote> priceQuoteMessage)
    {
        var quote = priceQuoteMessage.Payload.Body();
        if (asyncSubPeriodExecutions != null && asyncSubPeriodExecutions.Any())
            for (var i = 0; i < asyncSubPeriodExecutions.Count; i++)
            {
                var checkExecutionComplete = asyncSubPeriodExecutions[i];
                if (checkExecutionComplete.IsCompleted) // checking a value task is complete once completed frees ValueTaskCompletionSource
                {
                    asyncSubPeriodExecutions.RemoveAt(i);
                    i--;
                }
            }

        var quotePeriodWrapper = Context.PooledRecycler.Borrow<QuoteWrappingPricePeriodSummary>();
        quotePeriodWrapper.Configure(quote);
        return AddToCurrentOrPreviousState(quotePeriodWrapper);
    }

    private ValueTask AddToCurrentOrPreviousState(IPricePeriodSummary subPeriod)
    {
        var quoteStartPeriod = periodToPublish.ContainingPeriodBoundaryStart(subPeriod.PeriodStartTime);
        if (currentPeriodSummaryState.PeriodStartTime == quoteStartPeriod)
        {
            currentPeriodSummaryState.SubSummaryPeriods.AddReplace(subPeriod, Context.PooledRecycler);
        }
        else if (quoteStartPeriod > currentPeriodSummaryState.PeriodStartTime)
        {
            var noLastSummaryPeriod = lastPeriodSummaryState == null;
            currentPeriodSummaryState.NextPeriodBidAskStart = subPeriod.EndBidAsk;

            if (noLastSummaryPeriod)
            {
                lastPeriodSummaryState = currentPeriodSummaryState;
                currentPeriodSummaryState = new PeriodSummaryState(quoteStartPeriod, periodToPublish)
                {
                    PreviousPeriodBidAskEnd = lastPeriodSummaryState.SubSummaryPeriods.Tail?.EndBidAsk
                };
            }
            else
            {
                var lastPeriodEnd = lastPeriodSummaryState!.SubSummaryPeriods.Tail?.EndBidAsk;
                if (!lastPeriodSummaryState.HasPublishedComplete) PublishCompletePeriod();

                var newCurrentPeriodSummaryState = ClearExisting(lastPeriodSummaryState);
                lastPeriodSummaryState    = currentPeriodSummaryState;
                currentPeriodSummaryState = newCurrentPeriodSummaryState;
                currentPeriodSummaryState.Configure(quoteStartPeriod, periodToPublish);
                currentPeriodSummaryState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentPeriodSummaryState.SubSummaryPeriods.AddFirst(subPeriod);
        }
        return ValueTask.CompletedTask;
    }

    private PeriodSummaryState ClearExisting(PeriodSummaryState toClear)
    {
        var currentSummary = toClear.SubSummaryPeriods.Head;
        while (currentSummary != null)
        {
            var removed = toClear.SubSummaryPeriods.Remove(currentSummary);
            removed.DecrementRefCount();

            currentSummary = currentSummary.Next;
        }
        toClear.NextPeriodBidAskStart   = null;
        toClear.PreviousPeriodBidAskEnd = null;
        toClear.HasPublishedComplete    = false;
        return toClear;
    }
}
