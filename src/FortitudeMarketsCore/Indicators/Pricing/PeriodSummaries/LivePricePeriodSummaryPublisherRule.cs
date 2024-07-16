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
    public LivePublishPricePeriodSummaryParams(PricingInstrumentId pricingInstrument)
    {
        SourceTickerIdentifier = pricingInstrument;

        Period = pricingInstrument.EntryPeriod;

        LivePublishInterval   = new IndicatorPublishInterval(OneSecond);
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishPricePeriodSummaryParams(PricingInstrumentId pricingInstrument, IndicatorPublishInterval livePublishInterval)
    {
        SourceTickerIdentifier = pricingInstrument;

        Period = pricingInstrument.EntryPeriod;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishPricePeriodSummaryParams
        (SourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod period, IndicatorPublishInterval livePublishInterval)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;

        Period = period;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishPricePeriodSummaryParams
        (PricingInstrumentId pricingInstrument, IndicatorPublishInterval livePublishInterval, ResponsePublishParams livePublishParams)
    {
        SourceTickerIdentifier = pricingInstrument;

        Period = pricingInstrument.EntryPeriod;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = livePublishParams;
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishPricePeriodSummaryParams
    (SourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod period, IndicatorPublishInterval livePublishInterval
      , ResponsePublishParams livePublishParams)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;

        Period = period;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = livePublishParams;
        CompletePublishParams = new ResponsePublishParams();
    }

    public SourceTickerIdentifier    SourceTickerIdentifier { get; set; }
    public TimeSeriesPeriod          Period                 { get; set; }
    public IndicatorPublishInterval? LivePublishInterval    { get; set; }
    public ResponsePublishParams     LivePublishParams      { get; set; }

    public ResponsePublishParams CompletePublishParams { get; set; }
}

public class LivePricePeriodSummaryPublisherRule<TQuote> : PriceListenerIndicatorRule<TQuote> where TQuote : class, ILevel1Quote
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(LivePricePeriodSummaryPublisherRule<TQuote>));

    private static readonly TimeSpan LogNoDataPeriod = TimeSpan.FromHours(1);

    private readonly ResponsePublishParams     completeResponsePublishParams;
    private readonly IndicatorPublishInterval? livePricePublishInterval;
    private readonly ResponsePublishParams     liveResponsePublishParams;

    private readonly Dictionary<TimeSeriesPeriod, ISubscription> liveSubPeriodCompletePublisherSubscriptions = new();

    private readonly int logInterval;

    private readonly TimeSeriesPeriod       periodToPublish;
    private readonly PricingInstrumentId    pricingInstrumentId;
    private readonly SourceTickerIdentifier sourceTickerIdentifier;

    private List<ValueTask>? asyncSubPeriodExecutions = new();

    private ITimerUpdate completeIntervalTimer = null!;

    private int continuousEmptyPeriod;

    private PeriodSummaryState  currentPeriodSummaryState = null!;
    private PeriodSummaryState? lastPeriodSummaryState;

    private ITimerUpdate? liveIntervalTimer;

    public LivePricePeriodSummaryPublisherRule(LivePublishPricePeriodSummaryParams livePublishPricePeriodSummaryParams)
        : base(livePublishPricePeriodSummaryParams.SourceTickerIdentifier
             , nameof(LivePricePeriodSummaryPublisherRule<TQuote>)
             + $"_{livePublishPricePeriodSummaryParams.SourceTickerIdentifier.Source}_{livePublishPricePeriodSummaryParams.SourceTickerIdentifier.Ticker}" +
               $"_{livePublishPricePeriodSummaryParams.Period.ShortName()}")
    {
        pricingInstrumentId = new PricingInstrumentId(livePublishPricePeriodSummaryParams.SourceTickerIdentifier
                                                    , new PeriodInstrumentTypePair(InstrumentType.PriceSummaryPeriod
                                                                                 , livePublishPricePeriodSummaryParams.Period));
        sourceTickerIdentifier    = livePublishPricePeriodSummaryParams.SourceTickerIdentifier;
        periodToPublish           = livePublishPricePeriodSummaryParams.Period;
        liveResponsePublishParams = livePublishPricePeriodSummaryParams.LivePublishParams;
        if (liveResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
            liveResponsePublishParams.AlternativePublishAddress = pricingInstrumentId.LivePeriodSummaryAddress();
        else if (liveResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
            if (liveResponsePublishParams.ChannelRequest!.Channel is not IChannel<IPricePeriodSummary>)
                throw new Exception("Expected channel to be of type IPricePeriodSummary");
        completeResponsePublishParams = livePublishPricePeriodSummaryParams.CompletePublishParams;

        if (completeResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
            completeResponsePublishParams.AlternativePublishAddress = pricingInstrumentId.CompletePeriodSummaryAddress();
        else if (completeResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
            if (completeResponsePublishParams.ChannelRequest!.Channel is not IChannel<IPricePeriodSummary>)
                throw new Exception("Expected channel to be of type IPricePeriodSummary");
        livePricePublishInterval = livePublishPricePeriodSummaryParams.LivePublishInterval;
        var publishTimeSpan = periodToPublish.AveragePeriodTimeSpan().Min(LogNoDataPeriod / 2).Max(TimeSpan.FromMilliseconds(50));
        logInterval = (int)(LogNoDataPeriod.Ticks / publishTimeSpan.Ticks);
    }

    public void StartLiveTimeSeriesPeriodInterval()
    {
        liveIntervalTimer = Timer.RunEvery(livePricePublishInterval!.Value.PublishInterval.AveragePeriodTimeSpan(), PublishLivePeriod);
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

        if (livePricePublishInterval?.PublishInterval.IsTimeSpan() == true)
            liveIntervalTimer = Timer.RunEvery(livePricePublishInterval!.Value.PublishInterval.TimeSpan, PublishLivePeriod);
        if (livePricePublishInterval?.PublishInterval.IsTimeSeriesPeriod() == true)
            liveIntervalTimer = Timer.RunAt(livePricePublishInterval!.Value.PublishInterval.TimeSeriesPeriod.ContainingPeriodEnd(TimeContext.UtcNow)
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
                        "shut down for tickerIdl {0} and period {1}", sourceTickerIdentifier, periodToPublish);
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
            (RequestType.StartOrStatus, ServiceType.LivePricePeriodSummary, sourceTickerIdentifier, subPeriod
           , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>(), PQQuoteConverterExtensions.IsPQQuoteType<TQuote>());

        var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);
        if (!response.IsRunning())
        {
            Logger.Info("Problem starting LivePricePeriodSummaryPublisherRule for ticker {0} sub period {1} got {2}",
                        sourceTickerIdentifier.SourceTickerShortName(), subPeriod.ShortName(), response.RunStatus);
            return;
        }
        var completeLiveSubSummarySubscription
            = await this.RegisterListenerAsync<PricePeriodSummary>(sourceTickerIdentifier.CompletePeriodSummaryAddress(subPeriod)
                                                                 , ReceiveSubPeriodHandler);
        liveSubPeriodCompletePublisherSubscriptions.Add(subPeriod, completeLiveSubSummarySubscription);
    }

    private void PublishLivePeriod()
    {
        var now = TimeContext.UtcNow;

        var currentPeriodStart = periodToPublish.ContainingPeriodBoundaryStart(now);
        if (currentPeriodSummaryState is { IsEmpty: false } && currentPeriodSummaryState.PeriodStartTime <= currentPeriodStart)
        {
            var priceSummary = currentPeriodSummaryState.BuildPeriodSummary(Context.PooledRecycler, now);
            PublishSummaryTo(priceSummary, liveResponsePublishParams);
        }
    }

    private void PublishCompletePeriod()
    {
        var now = TimeContext.UtcNow;
        if (lastPeriodSummaryState is { HasPublishedComplete: false, IsEmpty: false })
        {
            var priceSummary = lastPeriodSummaryState.BuildPeriodSummary(Context.PooledRecycler, now);
            if (priceSummary.TickCount == 0) continuousEmptyPeriod++;
            PublishSummaryTo(priceSummary, completeResponsePublishParams);
            lastPeriodSummaryState.HasPublishedComplete = true;
        }
        else
        {
            var currentPeriodStart = periodToPublish.ContainingPeriodBoundaryStart(now);
            if (currentPeriodSummaryState is { HasPublishedComplete: false, IsEmpty: false }
             && currentPeriodSummaryState.PeriodStartTime <= currentPeriodStart)
            {
                var priceSummary = currentPeriodSummaryState.BuildPeriodSummary(Context.PooledRecycler, now);
                if (priceSummary.TickCount == 0) continuousEmptyPeriod++;
                PublishSummaryTo(priceSummary, completeResponsePublishParams);
                currentPeriodSummaryState.HasPublishedComplete = true;

                var lastPeriodEnd                = lastPeriodSummaryState!.SubSummaryPeriods.Tail?.EndBidAsk;
                var newCurrentPeriodSummaryState = ClearExisting(lastPeriodSummaryState);
                lastPeriodSummaryState    = currentPeriodSummaryState;
                currentPeriodSummaryState = newCurrentPeriodSummaryState;
                currentPeriodSummaryState.Configure(currentPeriodStart, periodToPublish);
                currentPeriodSummaryState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
        }
        if (continuousEmptyPeriod > 0 && continuousEmptyPeriod % logInterval == 0)
            Logger.Warn("Have received {0} empty live price periods for {1} {2}", continuousEmptyPeriod
                      , sourceTickerIdentifier.SourceTickerShortName()
                      , periodToPublish);
    }

    private void PublishSummaryTo(PricePeriodSummary priceSummary, ResponsePublishParams responsePublishParams)
    {
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
            (RequestType.StartOrStatus, ServiceType.HistoricalPricePeriodSummaryResolver, sourceTickerIdentifier, liveConstructingSubPeriod
           , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>(), PQQuoteConverterExtensions.IsPQQuoteType<TQuote>());

        var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);
        if (!response.IsRunning())
        {
            Logger.Info("Problem starting HistoricalPeriodSummariesResolverRule for ticker {0} sub period {1} got {2}",
                        sourceTickerIdentifier.SourceTickerShortName(), liveConstructingSubPeriod.ShortName(), response.RunStatus);
            return;
        }
        var subPeriods = await this.RequestAsync<HistoricalPeriodResponseRequest, List<PricePeriodSummary>>
            (sourceTickerIdentifier.HistoricalPeriodSummaryResponseRequest(liveConstructingSubPeriod), requestRange);
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
        var subPeriodContainerStart = periodToPublish.ContainingPeriodBoundaryStart(subPeriod.PeriodStartTime);
        if (currentPeriodSummaryState.PeriodStartTime == subPeriodContainerStart)
        {
            currentPeriodSummaryState.SubSummaryPeriods.AddReplace(subPeriod, Context.PooledRecycler);
        }
        else if (subPeriodContainerStart > currentPeriodSummaryState.PeriodStartTime)
        {
            var noLastSummaryPeriod                                                                 = lastPeriodSummaryState == null;
            if (!currentPeriodSummaryState.IsEmpty) currentPeriodSummaryState.NextPeriodBidAskStart = subPeriod.EndBidAsk;

            if (noLastSummaryPeriod)
            {
                lastPeriodSummaryState = currentPeriodSummaryState;
                if (lastPeriodSummaryState.IsEmpty &&
                    lastPeriodSummaryState.PeriodStartTime != periodToPublish.PreviousPeriodStart(subPeriodContainerStart))
                    lastPeriodSummaryState.PeriodStartTime = periodToPublish.PreviousPeriodStart(subPeriodContainerStart);
                currentPeriodSummaryState = new PeriodSummaryState(subPeriodContainerStart, periodToPublish)
                {
                    PreviousPeriodBidAskEnd = lastPeriodSummaryState.SubSummaryPeriods.Tail?.EndBidAsk
                };
            }
            else
            {
                var lastPeriodEnd = lastPeriodSummaryState!.SubSummaryPeriods.Tail?.EndBidAsk;

                if (!lastPeriodSummaryState.IsEmpty && !lastPeriodSummaryState.HasPublishedComplete) PublishCompletePeriod();

                var newCurrentPeriodSummaryState = ClearExisting(lastPeriodSummaryState);
                lastPeriodSummaryState    = currentPeriodSummaryState;
                currentPeriodSummaryState = newCurrentPeriodSummaryState;
                currentPeriodSummaryState.Configure(subPeriodContainerStart, periodToPublish);
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
