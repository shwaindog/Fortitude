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
using FortitudeIO.Storage.TimeSeries;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.BusRules;
using static FortitudeMarkets.Pricing.PQ.Converters.PQQuoteConverterExtensions;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Candles.Construction;

public abstract class StreamRequestAttendant : CandleAttendantBase, ICandleStreamRequestAttendant
{
    protected CandleState? CurrentCandleState;

    protected CandleState? PreviousCandleState;
    protected IChannel?           PublishChannel;

    protected HistoricalCandleStreamRequest StreamRequest;

    protected StreamRequestAttendant
        (IHistoricalCandleResolverRule constructingRule, HistoricalCandleStreamRequest streamRequest) : base(constructingRule) =>
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
        var retrieveUncachedRequest = new FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.BusRules.HistoricalCandleStreamRequest
            (State.PricingInstrumentId, channelRequest, requestRange);

        ReadCacheFromTime = requestRange.ToTime;

        var expectResults = await ConstructingRule.RequestAsync<FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.BusRules.HistoricalCandleStreamRequest, bool>
            (HistoricalQuoteTimeSeriesRepositoryConstants.CandleRepoStreamRequest, retrieveUncachedRequest);

        if (!expectResults)
        {
            await tickChannel.PublishComplete(ConstructingRule);
            ConstructingRule.Context.PooledRecycler.Recycle(limitedRecycler);
        }
        return expectResults;
    }

    protected async ValueTask<bool> PublishHistoricalSummary(Candle candle, bool shouldCache = true)
    {
        var publishParams = StreamRequest.PublishParams;
        return await PublishHistoricalSummary(candle, publishParams, shouldCache);
    }

    protected async Task<bool> PublishHistoricalSummary
        (Candle candle, ResponsePublishParams publishParams, bool shouldCache = true)
    {
        var keepFrom = (TimeContext.UtcNow - State.CacheTimeSpan).Min(State.ExistingRepoRange.ToTime - State.CacheTimeSpan);
        if (shouldCache && candle.PeriodEndTime > keepFrom) State.Cache.AddReplace(candle);
        if (publishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
        {
            await ConstructingRule.PublishAsync(State.PricingInstrumentId.CandlePublish(), candle);
        }
        else if (publishParams.ResponsePublishMethod == ResponsePublishMethod.AlternativeBroadcastAddress)
        {
            ConstructingRule.Publish(publishParams.AlternativePublishAddress!, candle, publishParams.PublishDispatchOptions);
        }
        else
        {
            var publishChannel = (IChannel<Candle>)publishParams.ChannelRequest!.Channel;
            var getMore        = await publishChannel.Publish(ConstructingRule, candle);
            return getMore;
        }
        return true;
    }

    protected async ValueTask<bool> PublishCachedRangeAndCloseRequest(UnboundedTimeRange? timeRange = null)
    {
        var cacheRange = timeRange ?? StreamRequest.RequestTimeRange ?? new UnboundedTimeRange();

        var lastPublishedCurrent = ReadCacheFromTime;
        var maxPreviousTimeRange = State.CandlePeriod.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
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
            var publishChannel = (IChannel<Candle>)publishParams.ChannelRequest!.Channel;
            await publishChannel.PublishComplete(ConstructingRule);
        }
        PublishChannel?.DecrementRefCount();
        HasCompleted = true;
        return false;
    }

    protected async Task<bool> HandleLastChannelEvent()
    {
        if (PreviousCandleState is { HasPublishedComplete: false })
            await PublishHistoricalSummary
                (PreviousCandleState.BuildCandle
                    (ConstructingRule.Context.PooledRecycler, PreviousCandleState.PeriodEnd()));
        if (CurrentCandleState is { HasPublishedComplete: false }
         && CurrentCandleState.PeriodStartTime < State.CandlePeriod.ContainingPeriodBoundaryStart(TimeContext.UtcNow))
            await PublishHistoricalSummary
                (CurrentCandleState.BuildCandle
                    (ConstructingRule.Context.PooledRecycler, CurrentCandleState.PeriodEnd()));
        await PublishCachedRangeAndCloseRequest();
        return false;
    }

    protected async Task<bool> AddSubSummaryToState(ICandle subCandle)
    {
        var subCandleBoundaryStart = State.CandlePeriod.ContainingPeriodBoundaryStart(subCandle.PeriodStartTime);
        var currentGenerateCandleState = CurrentCandleState ??=
            new CandleState(subCandleBoundaryStart, State.CandlePeriod);

        var sendMore = true;
        if (subCandle.IsWhollyBoundedBy(currentGenerateCandleState))
        {
            currentGenerateCandleState.SubCandlesPeriods.AddLast(subCandle);
        }
        else if (subCandleBoundaryStart > currentGenerateCandleState.PeriodStartTime)
        {
            var previousGenerateCandleSubCandleState = PreviousCandleState;
            var noLastCandlePeriod                     = previousGenerateCandleSubCandleState == null;
            currentGenerateCandleState.NextPeriodBidAskStart = subCandle.EndBidAsk;

            if (noLastCandlePeriod)
            {
                previousGenerateCandleSubCandleState = PreviousCandleState = currentGenerateCandleState;
                currentGenerateCandleState = CurrentCandleState
                    = new CandleState(subCandleBoundaryStart, State.CandlePeriod)
                    {
                        PreviousPeriodBidAskEnd = previousGenerateCandleSubCandleState.SubCandlesPeriods.Tail?.EndBidAsk
                    };
            }
            else
            {
                var lastPeriodEnd = previousGenerateCandleSubCandleState!.SubCandlesPeriods.Tail?.EndBidAsk;
                if (!previousGenerateCandleSubCandleState.HasPublishedComplete)
                {
                    sendMore = await PublishHistoricalSummary
                        (previousGenerateCandleSubCandleState.BuildCandle(ConstructingRule.Context.PooledRecycler
                                                                                  , previousGenerateCandleSubCandleState.PeriodEnd()));
                    previousGenerateCandleSubCandleState.HasPublishedComplete = true;
                }

                var newCurrentCandleState = previousGenerateCandleSubCandleState.Clear();
                PreviousCandleState                    = currentGenerateCandleState;
                currentGenerateCandleState = CurrentCandleState = newCurrentCandleState;
                currentGenerateCandleState.Configure(subCandleBoundaryStart, State.CandlePeriod);
                currentGenerateCandleState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentGenerateCandleState.SubCandlesPeriods.AddFirst(subCandle);
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
            var publishChannel = (IChannel<Candle>)StreamRequest.PublishParams.ChannelRequest!.Channel;
            await publishChannel.PublishComplete(ConstructingRule);
        }
    }

    private async ValueTask<bool> ReceiveHistoricalPeriod(ChannelEvent<Candle> channelEvent)
    {
        if (channelEvent.IsLastEvent)
        {
            await PublishCachedRangeAndCloseRequest();
            return false;
        }
        var subCandlesPeriod = channelEvent.Event;
        var sendMore         = await PublishHistoricalSummary(subCandlesPeriod);
        return sendMore;
    }
}

public class SubCandleStreamRequestAttendant : StreamRequestAttendant, ICandleStreamRequestAttendant
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SubCandleStreamRequestAttendant));

    private readonly TimeBoundaryPeriod subCandlesPeriod;

    public SubCandleStreamRequestAttendant
    (IHistoricalCandleResolverRule constructingRule, HistoricalCandleStreamRequest streamRequest
      , TimeBoundaryPeriod subCandlesPeriod)
        : base(constructingRule, streamRequest) =>
        this.subCandlesPeriod = subCandlesPeriod;

    public override async ValueTask<bool> BuildFromParts()
    {
        await ConstructingRule.EnsureSubPeriodResolverRunning();
        var requestHistoricalRange = StreamRequest.RequestTimeRange;
        var endHistoricalPeriods   = State.CandlePeriod.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        ReadCacheFromTime = endHistoricalPeriods.Min(requestHistoricalRange?.ToTime);

        var historicalSummariesChannel = ConstructingRule.CreateChannelFactory(ReceiveHistoricalSubPeriod, new LimitedBlockingRecycler(200));
        var channelRequest             = historicalSummariesChannel.ToChannelPublishRequest(-1, 50);
        var request                    = new HistoricalCandleStreamRequest(requestHistoricalRange, new ResponsePublishParams(channelRequest));

        var expectResults
            = await
                ConstructingRule
                    .RequestAsync<HistoricalCandleStreamRequest, bool>
                        (((SourceTickerIdentifier)State.PricingInstrumentId).HistoricalCandleStreamRequest(subCandlesPeriod), request);
        if (!expectResults) await historicalSummariesChannel.PublishComplete(ConstructingRule);
        return expectResults;
    }


    private async ValueTask<bool> ReceiveHistoricalSubPeriod(ChannelEvent<Candle> channelEvent)
    {
        if (channelEvent.IsLastEvent) return await HandleLastChannelEvent();
        return await AddSubSummaryToState(channelEvent.Event);
    }
}

public class QuoteToCandleStreamRequestAttendant<TQuote> : StreamRequestAttendant, ICandleStreamRequestAttendant
    where TQuote : class, ITimeSeriesEntry, IPublishableLevel1Quote, new()
{
    public QuoteToCandleStreamRequestAttendant
        (IHistoricalCandleResolverRule constructingRule, HistoricalCandleStreamRequest streamRequest)
        : base(constructingRule, streamRequest) { }

    public override async ValueTask<bool> BuildFromParts()
    {
        if (Equals(State.QuotesRepoRange, default)) return false;
        var endHistoricalPeriods = State.CandlePeriod.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var requestHistoricalRange = new BoundedTimeRange
            (StreamRequest.RequestTimeRange?.FromTime ?? State.QuotesRepoRange.FromTime
           , endHistoricalPeriods.Min(StreamRequest.RequestTimeRange?.ToTime));

        var historicalQuotesChannel = ConstructingRule.CreateChannelFactory<TQuote>
            (ReceiveHistoricalQuote, new LimitedBlockingRecycler(200, ConstructingRule.Context.PooledRecycler));
        var channelRequest = historicalQuotesChannel.ToChannelPublishRequest(-1, 50);
        var request        = channelRequest.ToHistoricalQuotesRequest(State.PricingInstrumentId, requestHistoricalRange);

        var retrieving = await ConstructingRule.RequestAsync<HistoricalQuotesRequest<TQuote>, bool>(request.RequestAddress, request);
        if (!retrieving) await historicalQuotesChannel.PublishComplete(ConstructingRule);
        return retrieving;
    }

    private async ValueTask<bool> ReceiveHistoricalQuote(ChannelEvent<TQuote> channelEvent)
    {
        if (channelEvent.IsLastEvent) return await HandleLastChannelEvent();

        var quote = channelEvent.Event;

        var quoteWrapperCandle = ConstructingRule.Context.PooledRecycler.Borrow<QuoteWrappingCandle>();
        quoteWrapperCandle.Configure(quote);

        return await AddSubSummaryToState(quoteWrapperCandle);
    }
}
