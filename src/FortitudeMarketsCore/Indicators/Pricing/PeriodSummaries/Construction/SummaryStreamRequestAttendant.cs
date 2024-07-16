// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;
using FortitudeMarketsCore.Pricing.Summaries;
using static FortitudeMarketsCore.Pricing.PQ.Converters.PQQuoteConverterExtensions;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries.Construction;

public abstract class StreamRequestAttendant : SummaryAttendantBase, ISummaryStreamRequestAttendant
{
    protected PeriodSummaryState? CurrentPeriodState;

    protected PeriodSummaryState? PreviousPeriodState;
    protected IChannel?           PublishChannel;

    protected HistoricalPeriodStreamRequest StreamRequest;

    protected StreamRequestAttendant
        (IHistoricalPricePeriodSummaryResolverRule constructingRule, HistoricalPeriodStreamRequest streamRequest) : base(constructingRule) =>
        StreamRequest = streamRequest;

    public abstract ValueTask<bool> BuildFromParts();

    public DateTime ReadCacheFromTime { get; set; } = DateTime.MaxValue;

    public bool HasCompleted { get; protected set; }

    public ValueTask<bool> PublishFromCache(BoundedTimeRange requestRange)
    {
        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        ConstructingRule.Context.RegisteredOn.RunOn(ConstructingRule, () => PublishCacheToChannel(requestRange));
        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        return new ValueTask<bool>(true);
    }

    public async ValueTask<bool> RetrieveFromRepository(BoundedTimeRange requestRange)
    {
        var limitedRecycler = ConstructingRule.Context.PooledRecycler.Borrow<LimitedBlockingRecycler>();
        limitedRecycler.MaxTypeBorrowLimit = StreamRequest.PublishParams.MaxInflightEvents;
        var tickChannel    = ConstructingRule.CreateChannelFactory(ReceiveHistoricalPeriod, limitedRecycler);
        var channelRequest = tickChannel.ToChannelPublishRequest(-1, 50);
        var retrieveUncachedRequest = new HistoricalPricePeriodSummaryStreamRequest
            (State.PricingInstrumentId, channelRequest, requestRange);

        ReadCacheFromTime = requestRange.ToTime;

        var expectResults = await ConstructingRule.RequestAsync<HistoricalPricePeriodSummaryStreamRequest, bool>
            (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoStreamRequest, retrieveUncachedRequest);

        if (!expectResults)
        {
            await tickChannel.PublishComplete(ConstructingRule);
            ConstructingRule.Context.PooledRecycler.Recycle(limitedRecycler);
        }
        return expectResults;
    }

    protected async ValueTask<bool> PublishHistoricalSummary(PricePeriodSummary periodSummary, bool shouldCache = true)
    {
        var publishParams = StreamRequest.PublishParams;
        return await PublishHistoricalSummary(periodSummary, publishParams, shouldCache);
    }

    protected async Task<bool> PublishHistoricalSummary
        (PricePeriodSummary periodSummary, ResponsePublishParams publishParams, bool shouldCache = true)
    {
        var keepFrom = (TimeContext.UtcNow - State.CacheTimeSpan).Min(State.ExistingRepoRange.ToTime - State.CacheTimeSpan);
        if (shouldCache && periodSummary.PeriodEndTime > keepFrom) State.Cache.AddReplace(periodSummary);
        if (publishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
        {
            await ConstructingRule.PublishAsync(State.PricingInstrumentId.PeriodSummaryPublish(), periodSummary);
        }
        else if (publishParams.ResponsePublishMethod == ResponsePublishMethod.AlternativeBroadcastAddress)
        {
            ConstructingRule.Publish(publishParams.AlternativePublishAddress!, periodSummary, publishParams.PublishDispatchOptions);
        }
        else
        {
            var publishChannel = (IChannel<PricePeriodSummary>)publishParams.ChannelRequest!.Channel;
            var getMore        = await publishChannel.Publish(ConstructingRule, periodSummary);
            return getMore;
        }
        return true;
    }

    protected async ValueTask<bool> PublishCachedRangeAndCloseRequest(UnboundedTimeRange? timeRange = null)
    {
        var cacheRange = timeRange ?? StreamRequest.RequestTimeRange ?? new UnboundedTimeRange();

        var lastPublishedCurrent = ReadCacheFromTime;
        var maxPreviousTimeRange = State.Period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var bounded              = cacheRange.CapUpperTime(maxPreviousTimeRange);
        var cacheCurrent         = State.Cache.Head;
        var sendMore             = true;
        while (cacheCurrent != null && sendMore)
        {
            if (cacheCurrent.PeriodStartTime >= lastPublishedCurrent
             && bounded.CompletelyContains(cacheCurrent))
                sendMore = await PublishHistoricalSummary(cacheCurrent, false);
            cacheCurrent = cacheCurrent.Next;
        }
        var publishParams = StreamRequest.PublishParams;
        if (publishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
        {
            var publishChannel = (IChannel<PricePeriodSummary>)publishParams.ChannelRequest!.Channel;
            await publishChannel.PublishComplete(ConstructingRule);
        }
        PublishChannel?.DecrementRefCount();
        HasCompleted = true;
        return false;
    }

    protected async Task<bool> HandleLastChannelEvent()
    {
        if (PreviousPeriodState is { HasPublishedComplete: false })
            await PublishHistoricalSummary
                (PreviousPeriodState.BuildPeriodSummary
                    (ConstructingRule.Context.PooledRecycler, PreviousPeriodState.PeriodEnd()));
        if (CurrentPeriodState is { HasPublishedComplete: false }
         && CurrentPeriodState.PeriodStartTime < State.Period.ContainingPeriodBoundaryStart(TimeContext.UtcNow))
            await PublishHistoricalSummary
                (CurrentPeriodState.BuildPeriodSummary
                    (ConstructingRule.Context.PooledRecycler, CurrentPeriodState.PeriodEnd()));
        await PublishCachedRangeAndCloseRequest();
        return false;
    }

    protected async Task<bool> AddSubSummaryToState(IPricePeriodSummary subSummaryPeriod)
    {
        var subSummaryBoundaryStart = State.Period.ContainingPeriodBoundaryStart(subSummaryPeriod.PeriodStartTime);
        var currentGeneratePeriodSubSummariesState = CurrentPeriodState ??=
            new PeriodSummaryState(subSummaryBoundaryStart, State.Period);

        var sendMore = true;
        if (subSummaryPeriod.IsWhollyBoundedBy(currentGeneratePeriodSubSummariesState))
        {
            currentGeneratePeriodSubSummariesState.SubSummaryPeriods.AddLast(subSummaryPeriod);
        }
        else if (subSummaryBoundaryStart > currentGeneratePeriodSubSummariesState.PeriodStartTime)
        {
            var previousGeneratePeriodSubSummariesState = PreviousPeriodState;
            var noLastSummaryPeriod                     = previousGeneratePeriodSubSummariesState == null;
            currentGeneratePeriodSubSummariesState.NextPeriodBidAskStart = subSummaryPeriod.EndBidAsk;

            if (noLastSummaryPeriod)
            {
                previousGeneratePeriodSubSummariesState = PreviousPeriodState = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState = CurrentPeriodState
                    = new PeriodSummaryState(subSummaryBoundaryStart, State.Period)
                    {
                        PreviousPeriodBidAskEnd = previousGeneratePeriodSubSummariesState.SubSummaryPeriods.Tail?.EndBidAsk
                    };
            }
            else
            {
                var lastPeriodEnd = previousGeneratePeriodSubSummariesState!.SubSummaryPeriods.Tail?.EndBidAsk;
                if (!previousGeneratePeriodSubSummariesState.HasPublishedComplete)
                {
                    sendMore = await PublishHistoricalSummary
                        (previousGeneratePeriodSubSummariesState.BuildPeriodSummary(ConstructingRule.Context.PooledRecycler
                                                                                  , previousGeneratePeriodSubSummariesState.PeriodEnd()));
                    previousGeneratePeriodSubSummariesState.HasPublishedComplete = true;
                }

                var newCurrentPeriodSummaryState = previousGeneratePeriodSubSummariesState.Clear();
                PreviousPeriodState                    = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState = CurrentPeriodState = newCurrentPeriodSummaryState;
                currentGeneratePeriodSubSummariesState.Configure(subSummaryBoundaryStart, State.Period);
                currentGeneratePeriodSubSummariesState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentGeneratePeriodSubSummariesState.SubSummaryPeriods.AddFirst(subSummaryPeriod);
        }
        return sendMore;
    }

    public async ValueTask PublishCacheToChannel(BoundedTimeRange timeRange)
    {
        var sendMore       = true;
        var currentSummary = State.Cache.Head;
        while (currentSummary != null && sendMore)
        {
            if (currentSummary.PeriodEndTime >= timeRange.FromTime && currentSummary.PeriodEndTime <= timeRange.ToTime)
                sendMore = await PublishHistoricalSummary(currentSummary, StreamRequest.PublishParams);
            currentSummary = currentSummary.Next;
        }
        if (StreamRequest.PublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
        {
            var publishChannel = (IChannel<PricePeriodSummary>)StreamRequest.PublishParams.ChannelRequest!.Channel;
            await publishChannel.PublishComplete(ConstructingRule);
        }
    }

    private async ValueTask<bool> ReceiveHistoricalPeriod(ChannelEvent<PricePeriodSummary> channelEvent)
    {
        if (channelEvent.IsLastEvent)
        {
            await PublishCachedRangeAndCloseRequest();
            return false;
        }
        var subSummaryPeriod = channelEvent.Event;
        var sendMore         = await PublishHistoricalSummary(subSummaryPeriod);
        return sendMore;
    }
}

public class SubSummaryStreamRequestAttendant : StreamRequestAttendant, ISummaryStreamRequestAttendant
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SubSummaryStreamRequestAttendant));

    private readonly TimeSeriesPeriod subSummaryPeriod;

    public SubSummaryStreamRequestAttendant
        (IHistoricalPricePeriodSummaryResolverRule constructingRule, HistoricalPeriodStreamRequest streamRequest, TimeSeriesPeriod subSummaryPeriod)
        : base(constructingRule, streamRequest) =>
        this.subSummaryPeriod = subSummaryPeriod;

    public override async ValueTask<bool> BuildFromParts()
    {
        await ConstructingRule.EnsureSubPeriodResolverRunning();
        var requestHistoricalRange = StreamRequest.RequestTimeRange;
        var endHistoricalPeriods   = State.Period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        ReadCacheFromTime = endHistoricalPeriods.Min(requestHistoricalRange?.ToTime);

        var historicalSummariesChannel = ConstructingRule.CreateChannelFactory(ReceiveHistoricalSubPeriod, new LimitedBlockingRecycler(200));
        var channelRequest             = historicalSummariesChannel.ToChannelPublishRequest(-1, 50);
        var request                    = new HistoricalPeriodStreamRequest(requestHistoricalRange, new ResponsePublishParams(channelRequest));

        var expectResults
            = await
                ConstructingRule
                    .RequestAsync<HistoricalPeriodStreamRequest, bool>
                        (((SourceTickerIdentifier)State.PricingInstrumentId).HistoricalPeriodSummaryStreamRequest(subSummaryPeriod), request);
        if (!expectResults) await historicalSummariesChannel.PublishComplete(ConstructingRule);
        return expectResults;
    }


    private async ValueTask<bool> ReceiveHistoricalSubPeriod(ChannelEvent<PricePeriodSummary> channelEvent)
    {
        if (channelEvent.IsLastEvent) return await HandleLastChannelEvent();
        return await AddSubSummaryToState(channelEvent.Event);
    }
}

public class QuoteToSummaryStreamRequestAttendant<TQuote> : StreamRequestAttendant, ISummaryStreamRequestAttendant
    where TQuote : class, ITimeSeriesEntry<TQuote>, ILevel1Quote, new()
{
    public QuoteToSummaryStreamRequestAttendant
        (IHistoricalPricePeriodSummaryResolverRule constructingRule, HistoricalPeriodStreamRequest streamRequest)
        : base(constructingRule, streamRequest) { }

    public override async ValueTask<bool> BuildFromParts()
    {
        if (Equals(State.QuotesRepoRange, default)) return false;
        var endHistoricalPeriods = State.Period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var requestHistoricalRange = new BoundedTimeRange
            (StreamRequest.RequestTimeRange?.FromTime ?? State.QuotesRepoRange.FromTime
           , endHistoricalPeriods.Min(StreamRequest.RequestTimeRange?.ToTime));

        var historicalQuotesChannel = ConstructingRule.CreateChannelFactory<TQuote>(ReceiveHistoricalQuote, new LimitedBlockingRecycler(200));
        var channelRequest          = historicalQuotesChannel.ToChannelPublishRequest(-1, 50);
        var request                 = channelRequest.ToHistoricalQuotesRequest(State.PricingInstrumentId, requestHistoricalRange);

        var retrieving = await ConstructingRule.RequestAsync<HistoricalQuotesRequest<TQuote>, bool>(request.RequestAddress, request);
        if (!retrieving) await historicalQuotesChannel.PublishComplete(ConstructingRule);
        return retrieving;
    }

    private async ValueTask<bool> ReceiveHistoricalQuote(ChannelEvent<TQuote> channelEvent)
    {
        if (channelEvent.IsLastEvent) return await HandleLastChannelEvent();

        var quote = channelEvent.Event;

        var quotePeriodWrapper = ConstructingRule.Context.PooledRecycler.Borrow<QuoteWrappingPricePeriodSummary>();
        quotePeriodWrapper.Configure(quote);

        return await AddSubSummaryToState(quotePeriodWrapper);
    }
}
