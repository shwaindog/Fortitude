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
using FortitudeMarkets.Indicators.Pricing.Candles.Construction;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Converters;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Candles;

public struct LivePublishCandleParams
{
    public LivePublishCandleParams(PricingInstrumentId pricingInstrument)
    {
        SourceTickerIdentifier = pricingInstrument;

        Period = pricingInstrument.CoveringPeriod.Period;

        LivePublishInterval   = new IndicatorPublishInterval(OneSecond);
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishCandleParams(PricingInstrumentId pricingInstrument, IndicatorPublishInterval livePublishInterval)
    {
        SourceTickerIdentifier = pricingInstrument;

        Period = pricingInstrument.CoveringPeriod.Period;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishCandleParams
        (SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period, IndicatorPublishInterval livePublishInterval)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;

        Period = period;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishCandleParams
        (PricingInstrumentId pricingInstrument, IndicatorPublishInterval livePublishInterval, ResponsePublishParams livePublishParams)
    {
        SourceTickerIdentifier = pricingInstrument;

        Period = pricingInstrument.CoveringPeriod.Period;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = livePublishParams;
        CompletePublishParams = new ResponsePublishParams();
    }

    public LivePublishCandleParams
    (SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period, IndicatorPublishInterval livePublishInterval
      , ResponsePublishParams livePublishParams)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;

        Period = period;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = livePublishParams;
        CompletePublishParams = new ResponsePublishParams();
    }

    public SourceTickerIdentifier    SourceTickerIdentifier { get; set; }
    public TimeBoundaryPeriod        Period                 { get; set; }
    public IndicatorPublishInterval? LivePublishInterval    { get; set; }
    public ResponsePublishParams     LivePublishParams      { get; set; }

    public ResponsePublishParams CompletePublishParams { get; set; }
}

public class LiveCandlePublisherRule<TQuote> : PriceListenerIndicatorRule<TQuote> where TQuote : class, IPublishableLevel1Quote
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(LiveCandlePublisherRule<TQuote>));

    private static readonly TimeSpan LogNoDataPeriod = TimeSpan.FromHours(1);

    private readonly ResponsePublishParams     completeResponsePublishParams;
    private readonly IndicatorPublishInterval? livePricePublishInterval;
    private readonly ResponsePublishParams     liveResponsePublishParams;

    private readonly Dictionary<TimeBoundaryPeriod, ISubscription> liveSubCandleCompletePublisherSubscriptions = new();

    private readonly int logInterval;

    private readonly TimeBoundaryPeriod     periodToPublish;
    private readonly PricingInstrumentId    pricingInstrumentId;
    private readonly SourceTickerIdentifier sourceTickerIdentifier;

    private List<ValueTask>? asyncSubPeriodExecutions = new();

    private ITimerUpdate completeIntervalTimer = null!;

    private int continuousEmptyPeriod;

    private CandleState  currentCandleState = null!;
    private CandleState? lastCandleState;

    private ITimerUpdate? liveIntervalTimer;

    public LiveCandlePublisherRule(LivePublishCandleParams livePublishCandleParams)
        : base(livePublishCandleParams.SourceTickerIdentifier
             , nameof(LiveCandlePublisherRule<TQuote>)
             + $"_{livePublishCandleParams.SourceTickerIdentifier.Source}_{livePublishCandleParams.SourceTickerIdentifier.Ticker}" +
               $"_{livePublishCandleParams.Period.ShortName()}")
    {
        pricingInstrumentId = new PricingInstrumentId
            (livePublishCandleParams.SourceTickerIdentifier, new PeriodInstrumentTypePair(InstrumentType.Candle
           , new DiscreetTimePeriod(livePublishCandleParams.Period)));
        sourceTickerIdentifier    = livePublishCandleParams.SourceTickerIdentifier;
        periodToPublish           = livePublishCandleParams.Period;
        liveResponsePublishParams = livePublishCandleParams.LivePublishParams;
        if (liveResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
            liveResponsePublishParams.AlternativePublishAddress = pricingInstrumentId.LiveCandleAddress();
        else if (liveResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
            if (liveResponsePublishParams.ChannelRequest!.Channel is not IChannel<ICandle>)
                throw new Exception("Expected channel to be of type ICandle");
        completeResponsePublishParams = livePublishCandleParams.CompletePublishParams;

        if (completeResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
            completeResponsePublishParams.AlternativePublishAddress = pricingInstrumentId.CompleteCandleAddress();
        else if (completeResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
            if (completeResponsePublishParams.ChannelRequest!.Channel is not IChannel<ICandle>)
                throw new Exception("Expected channel to be of type ICandle");
        livePricePublishInterval = livePublishCandleParams.LivePublishInterval;
        var publishTimeSpan = periodToPublish.AveragePeriodTimeSpan().Min(LogNoDataPeriod / 2).Max(TimeSpan.FromMilliseconds(50));
        logInterval = (int)(LogNoDataPeriod.Ticks / publishTimeSpan.Ticks);
    }

    public void StartLiveTimeSeriesPeriodInterval()
    {
        liveIntervalTimer = Timer.RunEvery(livePricePublishInterval!.Value.PublishInterval.AveragePeriodTimeSpan(), PublishLiveCandle);
        PublishLiveCandle();
    }

    public void StartCompleteTimeSeriesPeriodInterval()
    {
        completeIntervalTimer = Timer.RunEvery
            (periodToPublish.AveragePeriodTimeSpan().Min(TimeSpan.FromDays(1)), PublishCompleteCandle);
        PublishCompleteCandle();
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();

        completeIntervalTimer
            = Timer.RunAt(periodToPublish.ContainingPeriodEnd(TimeContext.UtcNow) + (periodToPublish.AveragePeriodTimeSpan() / 2).Min(TimeSpan.FromSeconds(5))
                        , StartCompleteTimeSeriesPeriodInterval);

        if (livePricePublishInterval?.PublishInterval.IsUncommonTimeSpan() == true)
            liveIntervalTimer = Timer.RunEvery(livePricePublishInterval!.Value.PublishInterval.TimeSpan, PublishLiveCandle);
        if (livePricePublishInterval?.PublishInterval.IsTimeBoundaryPeriod() == true)
            liveIntervalTimer = Timer.RunAt(livePricePublishInterval!.Value.PublishInterval.Period.ContainingPeriodEnd(TimeContext.UtcNow)
                                          , StartLiveTimeSeriesPeriodInterval);
        var now = TimeContext.UtcNow;

        var currentLivePeriodStartTime = periodToPublish.ContainingPeriodBoundaryStart(now);
        currentCandleState = new CandleState(currentLivePeriodStartTime, periodToPublish);
        // subscribe to prices and start caching
        var startHistoricalPeriods =
            periodToPublish.WholeSecondConstructingDivisiblePeriods(now)
                           .Where(tsp => tsp.TimeBoundaryPeriod >= CandleConstants.PersistPeriodsFrom);

        foreach (var timeSeriesPeriod in startHistoricalPeriods.Select(tspr => tspr.TimeBoundaryPeriod).Distinct())
        {
            var subPeriodHistoricalLastTime = timeSeriesPeriod.ContainingPeriodBoundaryStart(now);
            var boundedTime                 = new BoundedTimeRange(currentLivePeriodStartTime, subPeriodHistoricalLastTime);
            var requestHistorical           = new HistoricalCandleResponseRequest(boundedTime);
            asyncSubPeriodExecutions?.Add(RequestHistoricalSubCandles(timeSeriesPeriod, requestHistorical));
        }

        EnsureLiveSubPeriodCandlePublishersRunning();
    }

    public override async ValueTask StopAsync()
    {
        liveIntervalTimer?.Cancel();
        completeIntervalTimer.Cancel();
        foreach (var subPeriodCompleteSubscription in liveSubCandleCompletePublisherSubscriptions.Values)
            await subPeriodCompleteSubscription.NullSafeUnsubscribe();
        if (asyncSubPeriodExecutions?.Any() == true)
            Logger.Warn("Incomplete sub period complete subscription or historical period request before LiveCandlePublisherRule " +
                        "shut down for tickerIdl {0} and period {1}", sourceTickerIdentifier, periodToPublish);
    }

    private void EnsureLiveSubPeriodCandlePublishersRunning()
    {
        if (periodToPublish > FiveMinutes)
        {
            foreach (var subPeriod in periodToPublish.WholeSecondConstructingDivisiblePeriods().Where(tsp => tsp >= FiveMinutes))
                asyncSubPeriodExecutions?.Add(LaunchAndSubscribeToLiveSubCandle(subPeriod));
        }
        else
        {
            var subPeriod = periodToPublish.GranularDivisiblePeriod();
            if (subPeriod >= CandleConstants.PersistPeriodsFrom)
                asyncSubPeriodExecutions?.Add(LaunchAndSubscribeToLiveSubCandle(subPeriod));
        }
    }

    private async ValueTask LaunchAndSubscribeToLiveSubCandle(TimeBoundaryPeriod subPeriod)
    {
        var tickerSubPeriodService = new TickerPeriodServiceRequest
            (RequestType.StartOrStatus, ServiceType.LiveCandle, sourceTickerIdentifier, new DiscreetTimePeriod(subPeriod)
           , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>(), PQQuoteConverterExtensions.IsPQQuoteType<TQuote>());

        var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);
        if (!response.IsRunning())
        {
            Logger.Info("Problem starting LiveCandlePublisherRule for ticker {0} sub period {1} got {2}",
                        sourceTickerIdentifier.SourceTickerShortName(), subPeriod.ShortName(), response.RunStatus);
            return;
        }
        var completeLiveSubSummarySubscription
            = await this.RegisterListenerAsync<Candle>(sourceTickerIdentifier.CompleteCandleAddress(subPeriod)
                                                     , ReceiveSubPeriodHandler);
        liveSubCandleCompletePublisherSubscriptions.Add(subPeriod, completeLiveSubSummarySubscription);
    }

    private void PublishLiveCandle()
    {
        var now = TimeContext.UtcNow;

        var currentPeriodStart = periodToPublish.ContainingPeriodBoundaryStart(now);
        if (currentCandleState is { IsEmpty: false } && currentCandleState.PeriodStartTime <= currentPeriodStart)
        {
            var candle = currentCandleState.BuildCandle(Context.PooledRecycler, now);
            PublishSummaryTo(candle, liveResponsePublishParams);
        }
    }

    private void PublishCompleteCandle()
    {
        var now = TimeContext.UtcNow;
        if (lastCandleState is { HasPublishedComplete: false, IsEmpty: false })
        {
            var candle = lastCandleState.BuildCandle(Context.PooledRecycler, now);
            if (candle.TickCount == 0) continuousEmptyPeriod++;
            PublishSummaryTo(candle, completeResponsePublishParams);
            lastCandleState.HasPublishedComplete = true;
        }
        else
        {
            var currentPeriodStart = periodToPublish.ContainingPeriodBoundaryStart(now);
            if (currentCandleState is { HasPublishedComplete: false, IsEmpty: false }
             && currentCandleState.PeriodStartTime <= currentPeriodStart)
            {
                var candle = currentCandleState.BuildCandle(Context.PooledRecycler, now);
                if (candle.TickCount == 0) continuousEmptyPeriod++;
                PublishSummaryTo(candle, completeResponsePublishParams);
                currentCandleState.HasPublishedComplete = true;

                lastCandleState ??= new CandleState(currentCandleState.PeriodStartTime, periodToPublish);
                var lastPeriodEnd                = lastCandleState.SubCandlesPeriods.Tail?.EndBidAsk;
                var newCurrentCandleState = ClearExisting(lastCandleState);
                lastCandleState    = currentCandleState;
                currentCandleState = newCurrentCandleState;
                currentCandleState.Configure(currentPeriodStart, periodToPublish);
                currentCandleState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
        }
        if (continuousEmptyPeriod > 0 && continuousEmptyPeriod % logInterval == 0)
            Logger.Warn("Have received {0} empty live price periods for {1} {2}", continuousEmptyPeriod
                      , sourceTickerIdentifier.SourceTickerShortName()
                      , periodToPublish);
    }

    private void PublishSummaryTo(Candle candle, ResponsePublishParams responsePublishParams)
    {
        if (responsePublishParams.ResponsePublishMethod is ResponsePublishMethod.AlternativeBroadcastAddress
                                                        or ResponsePublishMethod.ListenerDefaultBroadcastAddress)
        {
            this.Publish(responsePublishParams.AlternativePublishAddress!, candle, responsePublishParams.PublishDispatchOptions);
        }
        else
        {
            var publishChannel = (IChannel<ICandle>)responsePublishParams.ChannelRequest!.Channel;
            publishChannel.Publish(this, candle);
        }
        if (asyncSubPeriodExecutions != null && !asyncSubPeriodExecutions.Any()) asyncSubPeriodExecutions = null;
    }

    private async ValueTask RequestHistoricalSubCandles(TimeBoundaryPeriod liveConstructingSubPeriod, HistoricalCandleResponseRequest requestRange)
    {
        var tickerSubPeriodService = new TickerPeriodServiceRequest
            (RequestType.StartOrStatus, ServiceType.HistoricalCandlesResolver, sourceTickerIdentifier
           , new DiscreetTimePeriod(liveConstructingSubPeriod)
           , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>(), PQQuoteConverterExtensions.IsPQQuoteType<TQuote>());

        var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);
        if (!response.IsRunning())
        {
            Logger.Info("Problem starting HistoricalCandleResolverRule for ticker {0} sub period {1} got {2}",
                        sourceTickerIdentifier.SourceTickerShortName(), liveConstructingSubPeriod.ShortName(), response.RunStatus);
            return;
        }
        var subPeriods = await this.RequestAsync<HistoricalCandleResponseRequest, List<Candle>>
            (sourceTickerIdentifier.HistoricalCandleResponseRequest(liveConstructingSubPeriod), requestRange);
        foreach (var subPeriod in subPeriods)
            if (subPeriod.IsWhollyBoundedBy(currentCandleState))
                currentCandleState.SubCandlesPeriods.AddReplace(subPeriod);
            else if (lastCandleState != null && subPeriod.IsWhollyBoundedBy(lastCandleState))
                lastCandleState.SubCandlesPeriods.AddReplace(subPeriod);
    }

    private ValueTask ReceiveSubPeriodHandler(IBusMessage<Candle> subPeriodMsg)
    {
        var subSummary = subPeriodMsg.Payload.Body();
        return AddToCurrentOrPreviousState(subSummary);
    }

    protected override ValueTask ReceiveQuoteMessageHandler(IBusMessage<TQuote> priceQuoteMessage)
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

        var quotePeriodWrapper = Context.PooledRecycler.Borrow<QuoteWrappingCandle>();
        quotePeriodWrapper.Configure(quote);
        return AddToCurrentOrPreviousState(quotePeriodWrapper);
    }

    private ValueTask AddToCurrentOrPreviousState(ICandle subCandle)
    {
        var subCandleContainerStart = periodToPublish.ContainingPeriodBoundaryStart(subCandle.PeriodStartTime);
        if (currentCandleState.PeriodStartTime == subCandleContainerStart)
        {
            currentCandleState.SubCandlesPeriods.AddReplace(subCandle);
        }
        else if (subCandleContainerStart > currentCandleState.PeriodStartTime)
        {
            var noLastCandle = lastCandleState == null;

            if (!currentCandleState.IsEmpty) currentCandleState.NextPeriodBidAskStart = subCandle.EndBidAsk;

            if (noLastCandle)
            {
                lastCandleState = currentCandleState;
                if (lastCandleState.IsEmpty &&
                    lastCandleState.PeriodStartTime != periodToPublish.PreviousPeriodStart(subCandleContainerStart))
                    lastCandleState.PeriodStartTime = periodToPublish.PreviousPeriodStart(subCandleContainerStart);
                currentCandleState = new CandleState(subCandleContainerStart, periodToPublish)
                {
                    PreviousPeriodBidAskEnd = lastCandleState.SubCandlesPeriods.Tail?.EndBidAsk
                };
            }
            else
            {
                var lastPeriodEnd = lastCandleState!.SubCandlesPeriods.Tail?.EndBidAsk;

                if (!lastCandleState.IsEmpty && !lastCandleState.HasPublishedComplete) PublishCompleteCandle();

                var newCurrentCandleState = ClearExisting(lastCandleState);
                lastCandleState    = currentCandleState;
                currentCandleState = newCurrentCandleState;
                currentCandleState.Configure(subCandleContainerStart, periodToPublish);
                currentCandleState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentCandleState.SubCandlesPeriods.AddFirst(subCandle);
        }
        return ValueTask.CompletedTask;
    }

    private CandleState ClearExisting(CandleState toClear)
    {
        var currentSummary = toClear.SubCandlesPeriods.Head;
        while (currentSummary != null)
        {
            var removed = toClear.SubCandlesPeriods.Remove(currentSummary);
            removed.DecrementRefCount();

            currentSummary = currentSummary.Next;
        }
        toClear.NextPeriodBidAskStart   = null;
        toClear.PreviousPeriodBidAskEnd = null;
        toClear.HasPublishedComplete    = false;
        return toClear;
    }
}
