// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries.Construction;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;

public struct TimeSeriesPricePeriodParams
{
    public TimeSeriesPricePeriodParams(TimeSeriesPeriod publishPeriod, ISourceTickerIdentifier tickerId)
    {
        PublishPeriod = publishPeriod;
        TickerId      = tickerId;

        LivePublishInterval   = new PricePublishInterval(TimeSeriesPeriod.OneSecond);
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public TimeSeriesPricePeriodParams(TimeSeriesPeriod publishPeriod, ISourceTickerIdentifier tickerId, PricePublishInterval livePublishInterval)
    {
        PublishPeriod = publishPeriod;
        TickerId      = tickerId;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = new ResponsePublishParams();
        CompletePublishParams = new ResponsePublishParams();
    }

    public TimeSeriesPricePeriodParams
    (TimeSeriesPeriod publishPeriod, ISourceTickerIdentifier tickerId, PricePublishInterval livePublishInterval
      , ResponsePublishParams livePublishParams)
    {
        PublishPeriod = publishPeriod;
        TickerId      = tickerId;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = livePublishParams;
        CompletePublishParams = new ResponsePublishParams();
    }

    public TimeSeriesPricePeriodParams
    (TimeSeriesPeriod publishPeriod, ISourceTickerIdentifier tickerId, PricePublishInterval livePublishInterval
      , ResponsePublishParams livePublishParams, ResponsePublishParams completePublishParams)
    {
        PublishPeriod = publishPeriod;
        TickerId      = tickerId;

        LivePublishInterval   = livePublishInterval;
        LivePublishParams     = livePublishParams;
        CompletePublishParams = completePublishParams;
    }

    public ISourceTickerIdentifier TickerId            { get; set; }
    public TimeSeriesPeriod        PublishPeriod       { get; set; }
    public PricePublishInterval?   LivePublishInterval { get; set; }
    public ResponsePublishParams   LivePublishParams   { get; set; }

    public ResponsePublishParams CompletePublishParams { get; set; }
}

public class LivePricePeriodSummaryPublisherRule<TQuote> : PriceListenerIndicatorRule<TQuote> where TQuote : class, ILevel1Quote
{
    private readonly ResponsePublishParams   completeResponsePublishParams;
    private readonly TimeSeriesPeriod        periodToPublish;
    private readonly ISourceTickerIdentifier tickerId;

    private PeriodSummaryState  currentPeriodSummaryState = null!;
    private PeriodSummaryState? lastPeriodSummaryState;

    public LivePricePeriodSummaryPublisherRule(TimeSeriesPricePeriodParams pricingPeriodParams)
        : base(pricingPeriodParams.TickerId
             , nameof(LivePricePeriodSummaryPublisherRule<TQuote>)
             + $"_{pricingPeriodParams.TickerId.Source}_{pricingPeriodParams.TickerId.Source}_{pricingPeriodParams.PublishPeriod.ShortName()}")
    {
        tickerId        = pricingPeriodParams.TickerId;
        periodToPublish = pricingPeriodParams.PublishPeriod;
        var liveResponsePublishParams = pricingPeriodParams.LivePublishParams;
        if (liveResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
            liveResponsePublishParams.AlternativePublishAddress = tickerId.LivePeriodSummaryAddress(periodToPublish);
        else if (liveResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
            if (liveResponsePublishParams.ChannelRequest!.Channel is not IChannel<IPricePeriodSummary>)
                throw new Exception("Expected channel to be of type IPricePeriodSummary");
        completeResponsePublishParams = pricingPeriodParams.CompletePublishParams;

        if (completeResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
            completeResponsePublishParams.AlternativePublishAddress = tickerId.CompletePeriodSummaryAddress(periodToPublish);
        else if (completeResponsePublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
            if (completeResponsePublishParams.ChannelRequest!.Channel is not IChannel<IPricePeriodSummary>)
                throw new Exception("Expected channel to be of type IPricePeriodSummary");
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        var now                        = DateTime.UtcNow;
        var currentLivePeriodStartTime = periodToPublish.ContainingPeriodBoundaryStart(now);
        currentPeriodSummaryState = new PeriodSummaryState(currentLivePeriodStartTime, periodToPublish);
        // subscribe to prices and start caching
        var startHistoricalPeriods = periodToPublish.ConstructingDivisiblePeriods(now);

        foreach (var timeSeriesPeriod in startHistoricalPeriods.Select(tspr => tspr.TimeSeriesPeriod).Distinct())
        {
            var subPeriodHistoricalLastTime = timeSeriesPeriod.ContainingPeriodBoundaryStart(now);
            var boundedTime                 = new BoundedTimeRange(currentLivePeriodStartTime, subPeriodHistoricalLastTime);
            var requestHistorical           = new HistoricalPeriodResponseRequest(boundedTime);
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            RequestUpdateLiveSubPeriods(timeSeriesPeriod, requestHistorical);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        // get summaries that can summarize as much of the current period range as possible
    }

    private void PublishSummaryTo(PeriodSummaryState periodState, ResponsePublishParams responsePublishParams, DateTime? atTime = null)
    {
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
    }

    private async ValueTask RequestUpdateLiveSubPeriods(TimeSeriesPeriod liveConstructingSubPeriod, HistoricalPeriodResponseRequest requestRange)
    {
        var subPeriods = await this.RequestAsync<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>>
            (tickerId.HistoricalPeriodSummaryResponseRequest(liveConstructingSubPeriod), requestRange);
        foreach (var subPeriod in subPeriods)
            if (subPeriod.IsWhollyBoundedBy(currentPeriodSummaryState))
                currentPeriodSummaryState.SubSummaryPeriods.AddReplace(subPeriod, Context.PooledRecycler);
            else if (lastPeriodSummaryState != null && subPeriod.IsWhollyBoundedBy(lastPeriodSummaryState))
                lastPeriodSummaryState.SubSummaryPeriods.AddReplace(subPeriod, Context.PooledRecycler);
    }

    protected override ValueTask ReceiveQuoteHandler(IBusMessage<TQuote> priceQuoteMessage)
    {
        var quote = priceQuoteMessage.Payload.Body();

        var quotePeriodWrapper = Context.PooledRecycler.Borrow<QuoteWrappingPricePeriodSummary>();
        quotePeriodWrapper.Configure(quote);
        var quoteStartPeriod = periodToPublish.ContainingPeriodBoundaryStart(quote.SourceTime);
        if (currentPeriodSummaryState.PeriodStartTime == quoteStartPeriod)
        {
            currentPeriodSummaryState.SubSummaryPeriods.AddReplace(quotePeriodWrapper, Context.PooledRecycler);
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
                    PreviousPeriodBidAskEnd = lastPeriodSummaryState.SubSummaryPeriods.Tail?.EndBidAsk
                };
            }
            else
            {
                var lastPeriodEnd = lastPeriodSummaryState!.SubSummaryPeriods.Tail?.EndBidAsk;
                if (lastPeriodSummaryState.HasPublishedComplete)
                {
                    PublishSummaryTo(lastPeriodSummaryState, completeResponsePublishParams);
                    lastPeriodSummaryState.HasPublishedComplete = true;
                }

                var newCurrentPeriodSummaryState = ClearExisting(lastPeriodSummaryState);
                lastPeriodSummaryState    = currentPeriodSummaryState;
                currentPeriodSummaryState = newCurrentPeriodSummaryState;
                currentPeriodSummaryState.Configure(quoteStartPeriod, periodToPublish);
                currentPeriodSummaryState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentPeriodSummaryState.SubSummaryPeriods.AddFirst(quotePeriodWrapper);
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
