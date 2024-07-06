// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;
using FortitudeMarketsCore.Pricing.Summaries;
using static FortitudeIO.TimeSeries.InstrumentType;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;

public struct HistoricalPeriodParams
{
    public HistoricalPeriodParams
        (ISourceTickerIdentifier tickerId, TimeSeriesPeriod period, TimeLength cacheLength)
    {
        CacheLength = cacheLength;
        Period      = period;
        TickerId    = tickerId;
    }

    public ISourceTickerIdentifier TickerId { get; set; }

    public TimeSeriesPeriod Period      { get; set; }
    public TimeLength       CacheLength { get; set; }
}

public struct HistoricalPeriodStreamRequest
{
    public HistoricalPeriodStreamRequest(UnboundedTimeRange? requestTimeRange, ResponsePublishParams publishParams)
    {
        PublishParams    = publishParams;
        RequestTimeRange = requestTimeRange;
    }

    public UnboundedTimeRange?   RequestTimeRange { get; set; }
    public ResponsePublishParams PublishParams    { get; set; }
}

public struct HistoricalPeriodResponseRequest
{
    public HistoricalPeriodResponseRequest(BoundedTimeRange requestTimeRange) => RequestTimeRange = requestTimeRange;

    public BoundedTimeRange RequestTimeRange { get; set; }
}

public class HistoricalPeriodRequestState
{
    public HistoricalPeriodRequestState(HistoricalPeriodStreamRequest periodStreamRequest, IChannel publishChannel)
    {
        HistoricalPeriodStreamRequest = periodStreamRequest;
        PublishChannel                = publishChannel;
    }

    public HistoricalPeriodStreamRequest HistoricalPeriodStreamRequest { get; set; }

    public PeriodSummaryState? CurrentPeriodState { get; set; }

    public PeriodSummaryState? PreviousPeriodState { get; set; }

    public IChannel? PublishChannel { get; set; }
}

public class HistoricalPeriodSummariesResolverRule<TQuote> : Rule where TQuote : class, ITimeSeriesEntry<TQuote>, ILevel1Quote, new()
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalPeriodSummariesResolverRule<TQuote>));

    private readonly TimeSeriesPeriod buildPeriod;

    private readonly IDoublyLinkedList<IPricePeriodSummary> cacheLatest = new DoublyLinkedList<IPricePeriodSummary>();

    private readonly Task<bool> generateHistoricalComplete;

    private readonly HistoricalPeriodParams historicalPeriodParams;

    private readonly Dictionary<int, HistoricalPeriodRequestState> historicalPeriodsPublishLookup = new();

    private readonly TimeSeriesPeriod period;
    private readonly TimeSeriesPeriod subPeriod;

    private readonly ISourceTickerIdentifier tickerId;

    private TimeSpan cacheTimeSpan;

    private BoundedTimeRange    existingSummariesHistoricalRange;
    private InstrumentFileInfo? existingSummariesInfo;
    private ISubscription?      historicalPriceResponseRequestSubscription;

    private ISubscription? historicalPriceStreamRequestSubscription;

    private TaskCompletionSource<bool>? historicalSummariesUpToDate;
    private DateTime                    nextStorageCheckTime;


    private InstrumentFileInfo? quotesFileInfo;
    private BoundedTimeRange    quotesHistoricalRange;
    private ISubscription?      recentlyCompletedSummariesSubscription;

    private InstrumentFileInfo? subPeriodFileInfo;
    private bool                subPeriodResolverStarted;
    private BoundedTimeRange    subPeriodsHistoricalRange;

    public HistoricalPeriodSummariesResolverRule(HistoricalPeriodParams historicalPeriod)
    {
        historicalPeriodParams = historicalPeriod;

        tickerId = historicalPeriod.TickerId;
        period   = historicalPeriod.Period;

        if (period is (None or > OneYear)) throw new Exception("Period to generate is greater than expected bounds");

        subPeriod   = period.GranularDivisiblePeriod();
        buildPeriod = subPeriod.RoundNonPersistPeriodsToTick();

        historicalSummariesUpToDate = new TaskCompletionSource<bool>();
        generateHistoricalComplete  = historicalSummariesUpToDate.Task;

        cacheTimeSpan = buildPeriod != Tick
            ? historicalPeriod.CacheLength.LargerPeriodOf(period, 32)
            : historicalPeriod.CacheLength.ToTimeSpan().Max(TimeSpan.FromHours(4));
    }

    public override async ValueTask StartAsync()
    {
        recentlyCompletedSummariesSubscription
            = await this.RegisterListenerAsync<IPricePeriodSummary>
                (tickerId.CompletePeriodSummaryAddress(period), CacheRecentlyCompletedSummariesHandler);
        historicalPriceStreamRequestSubscription = await this.RegisterRequestListenerAsync<HistoricalPeriodStreamRequest, bool>
            (tickerId.HistoricalPeriodSummaryStreamRequest(period), HistoricalPeriodStreamRequestHandler);
        historicalPriceResponseRequestSubscription
            = await this.RegisterRequestListenerAsync<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>>
                (tickerId.HistoricalPeriodSummaryResponseRequest(period), HistoricalPeriodResponseRequestHandler);
        if (period is >= PricePeriodSummaryConstants.PersistPeriodsFrom and <= PricePeriodSummaryConstants.PersistPeriodsTo)
        {
            var tickerSubPeriodService = new TickerPeriodServiceRequest
                (RequestType.StartOrStatus, ServiceType.PricePeriodSummaryFilePersister, tickerId, period, PQQuoteExtensions.GetQuoteLevel<TQuote>());

            var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
                (IndicatorServiceConstants.PricePeriodIndicatorsServiceStartRequest, tickerSubPeriodService);
            if (response.RunStatus == ServiceRunStatus.Disabled)
                Logger.Info("File Persistence for PricePeriodSummary {0} for {1} is disable in this configuration", tickerId.ShortName(), period);
        }
        var now                  = TimeContext.UtcNow;
        var maxPreviousTimeRange = period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var quotesRangeRequest   = new TimeSeriesRepositoryInstrumentFileInfoRequest(tickerId.Ticker, Price, Tick);
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, quotesRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new Exception($"Received More than one instrument for {tickerId.Ticker} type from the repository for quotes");
        if (candidateInstrumentFileInfo.Count == 1)
        {
            quotesFileInfo = candidateInstrumentFileInfo[0]!;
            if (quotesFileInfo.Value.HasInstrument)
                quotesHistoricalRange
                    = new BoundedTimeRange
                        (quotesFileInfo.Value.EarliestEntry!.Value, quotesFileInfo.Value.LatestEntry!.Value.Min(now));
        }

        var existingRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest(tickerId.Ticker, PriceSummaryPeriod, period);
        candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new Exception($"Received More than one instrument type for {tickerId.Ticker} from the repository for period {period}");
        nextStorageCheckTime = TimeContext.UtcNow + cacheTimeSpan;
        if (candidateInstrumentFileInfo.Count == 1)
        {
            existingSummariesInfo = candidateInstrumentFileInfo[0];
            if (existingSummariesInfo.Value.HasInstrument)
                existingSummariesHistoricalRange = new BoundedTimeRange
                    (existingSummariesInfo.Value.EarliestEntry!.Value, existingSummariesInfo.Value.LatestEntry!.Value.Min(maxPreviousTimeRange));
        }

        if (existingSummariesHistoricalRange.ToTime < maxPreviousTimeRange)
        {
            if (buildPeriod is not (None or Tick))
            {
                var existingPeriodsRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest(tickerId.Ticker, PriceSummaryPeriod, buildPeriod);
                candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
                    (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingPeriodsRangeRequest);
                if (candidateInstrumentFileInfo.Count > 1)
                    throw new
                        Exception($"Received More than one instrument type for {tickerId.Ticker} from the repository for subPeriod {buildPeriod}");
                if (candidateInstrumentFileInfo.Count == 1)
                {
                    subPeriodFileInfo = candidateInstrumentFileInfo[0];
                    if (subPeriodFileInfo.Value.HasInstrument)
                    {
                        subPeriodsHistoricalRange = new BoundedTimeRange
                            (subPeriodFileInfo.Value.EarliestEntry!.Value, subPeriodFileInfo.Value.LatestEntry!.Value.Min(now));

                        var searchRange = new UnboundedTimeRange
                            (existingSummariesHistoricalRange.ToTime, subPeriodsHistoricalRange.ToTime.Min(maxPreviousTimeRange));

                        await CheckLaunchSubPeriodFactory
                            (new HistoricalPeriodStreamRequest
                                (searchRange, new ResponsePublishParams(tickerId.PersistAppendPeriodSummaryPublish(period))));
                    }
                    else
                    {
                        historicalSummariesUpToDate?.SetResult(true);
                        historicalSummariesUpToDate = null;
                    }
                }
            }
            else
            {
                subPeriodsHistoricalRange = quotesHistoricalRange;
                await GetHistoricalTicksToConvert
                    (new HistoricalPeriodStreamRequest
                        (new UnboundedTimeRange(quotesHistoricalRange.FromTime, existingSummariesHistoricalRange.ToTime.Min(maxPreviousTimeRange))
                       , new ResponsePublishParams(tickerId.PersistAppendPeriodSummaryPublish(period))));
            }
        }
        else
        {
            historicalSummariesUpToDate?.SetResult(true);
            historicalSummariesUpToDate = null;
        }
    }

    private async ValueTask CacheRecentlyCompletedSummariesHandler(IBusMessage<IPricePeriodSummary> recentlyCompletedSummaryMsg)
    {
        var summary = recentlyCompletedSummaryMsg.Payload.Body();
        summary.IncrementRefCount();
        cacheLatest.AddLast(summary);
        if (generateHistoricalComplete.IsCompleted)
        {
            var now = TimeContext.UtcNow;
            if (now > nextStorageCheckTime)
            {
                var maxHistoricalTimeRange = period.ContainingPeriodBoundaryStart(now);
                var existingRangeRequest   = new TimeSeriesRepositoryInstrumentFileInfoRequest(tickerId.Ticker, PriceSummaryPeriod, period);
                var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
                    (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingRangeRequest);

                if (candidateInstrumentFileInfo.Count == 1)
                {
                    existingSummariesInfo = candidateInstrumentFileInfo[0];
                    if (existingSummariesInfo!.HasValue)
                        existingSummariesHistoricalRange = new BoundedTimeRange
                            (existingSummariesInfo.Value.EarliestEntry!.Value
                           , existingSummariesInfo.Value.LatestEntry!.Value.Min(maxHistoricalTimeRange));
                }
                nextStorageCheckTime = now + cacheTimeSpan;
            }
            var keepFrom       = (TimeContext.UtcNow - cacheTimeSpan).Min(existingSummariesHistoricalRange.ToTime - cacheTimeSpan);
            var currentSummary = cacheLatest.Head;
            while (currentSummary != null && currentSummary.PeriodEndTime < keepFrom)
            {
                cacheLatest.Remove(currentSummary);
                currentSummary.DecrementRefCount();
            }
        }
    }

    public override async ValueTask StopAsync()
    {
        await historicalPriceStreamRequestSubscription.NullSafeUnsubscribe();
        await historicalPriceResponseRequestSubscription.NullSafeUnsubscribe();
        await base.StopAsync();
    }

    private async ValueTask<List<IPricePeriodSummary>> HistoricalPeriodResponseRequestHandler
        (IBusRespondingMessage<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>> reqMsg)
    {
        await generateHistoricalComplete;
        await CheckLoadCache();

        var timeRange = reqMsg.Payload.Body().RequestTimeRange;

        var dateFirstCached = cacheLatest.Head?.PeriodStartTime ?? DateTimeConstants.UnixEpoch;

        var currentSummary = cacheLatest.Head;
        var cacheResults   = new List<IPricePeriodSummary>();
        while (currentSummary != null)
        {
            if (currentSummary.PeriodEndTime >= timeRange.FromTime && currentSummary.PeriodEndTime < timeRange.ToTime)
                cacheResults.Add(currentSummary);
            currentSummary = currentSummary.Next;
        }
        if (timeRange.FromTime >= dateFirstCached) return cacheResults;

        var unmatchedRange = new BoundedTimeRange(timeRange.FromTime, dateFirstCached);

        if (period >= PricePeriodSummaryConstants.PersistPeriodsFrom)
        {
            var retrieveUncachedRequest = new HistoricalPricePeriodSummaryRequestResponse(tickerId, period, unmatchedRange);

            var remainingPeriods = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<IPricePeriodSummary>>
                (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoRequestResponse, retrieveUncachedRequest);
            remainingPeriods.AddRange(cacheResults);
            return remainingPeriods;
        }
        if (subPeriod > Tick)
        {
            var subPeriodRequest = new HistoricalPeriodResponseRequest(unmatchedRange);
            var subPeriodsInRange
                = await this.RequestAsync<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>>
                    (tickerId.HistoricalPeriodSummaryResponseRequest(subPeriod), subPeriodRequest);

            if (subPeriodsInRange.Count == 0) return cacheResults;
            var remainingPeriods = new List<IPricePeriodSummary>();
            var firstSubPeriod   = subPeriodsInRange[0];
            var currentPeriod    = new PricePeriodSummary(period, period.ContainingPeriodBoundaryStart(firstSubPeriod.PeriodStartTime));
            foreach (var subPeriodHistory in subPeriodsInRange)
            {
                if (!subPeriodHistory.IsWhollyBoundedBy(currentPeriod))
                {
                    remainingPeriods.Add(currentPeriod.Clone());
                    currentPeriod = new PricePeriodSummary(period, period.ContainingPeriodBoundaryStart(subPeriodHistory.PeriodStartTime));
                }
                currentPeriod.MergeBoundaries(subPeriodHistory);
            }
            remainingPeriods.AddRange(cacheResults.Select(pps => pps.Clone()));
            return remainingPeriods;
        }
        else
        {
            // request ticks build PricePeriodSummaries
            var limitedRecycler = Context.PooledRecycler.Borrow<LimitedBlockingRecycler>();
            limitedRecycler.MaxTypeBorrowLimit = 200;
            var remainingPeriods      = new List<IPricePeriodSummary>();
            var rebuildCompleteSource = new TaskCompletionSource<bool>();
            var rebuildCompleteTask   = rebuildCompleteSource.Task;

            var keepFrom = (TimeContext.UtcNow - cacheTimeSpan).Min(existingSummariesHistoricalRange.ToTime - cacheTimeSpan);
            var summaryPopulateChannel = this.CreateChannelFactory(ce =>
            {
                if (ce.IsLastEvent)
                {
                    rebuildCompleteSource.SetResult(true);
                }
                else
                {
                    if (ce.Event.PeriodEndTime > keepFrom) cacheLatest.AddReplace(ce.Event);
                    remainingPeriods.Add(ce.Event.Clone());
                }
                return !ce.IsLastEvent;
            }, limitedRecycler);
            var rebuildRequest     = summaryPopulateChannel.ToChannelPublishRequest();
            var tickChannel        = this.CreateChannelFactory<TQuote>(ReceiveHistoricalQuote, new LimitedBlockingRecycler(200));
            var tickChannelRequest = tickChannel.ToChannelPublishRequest(-1, 50);
            var req                = new HistoricalPeriodStreamRequest(unmatchedRange, new ResponsePublishParams(rebuildRequest));
            historicalPeriodsPublishLookup.Add(tickChannel.Id, new HistoricalPeriodRequestState(req, tickChannel));
            var request = tickChannelRequest.ToHistoricalQuotesRequest(tickerId, unmatchedRange);

            var expectResult
                = await this.RequestAsync<HistoricalQuotesRequest<TQuote>, bool>(request.RequestAddress, request);

            if (!expectResult)
            {
                Context.PooledRecycler.Recycle(limitedRecycler);
                return cacheResults;
            }

            await rebuildCompleteTask;

            remainingPeriods.AddRange(cacheResults);

            return remainingPeriods;
        }
    }

    private async ValueTask<bool> HistoricalPeriodStreamRequestHandler(IBusMessage<HistoricalPeriodStreamRequest> reqMsg)
    {
        await generateHistoricalComplete;
        await CheckLoadCache();

        if (Equals(quotesHistoricalRange, default)) return false;

        var req      = reqMsg.Payload.Body();
        var rangeReq = req.RequestTimeRange;

        var maxPreviousTimeRange = period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var requestMax           = maxPreviousTimeRange.Min(rangeReq?.ToTime);

        var dateFirstCached = cacheLatest.Head?.PeriodStartTime;

        var timeRange = new BoundedTimeRange(quotesHistoricalRange.FromTime.Max(rangeReq?.FromTime), requestMax);

        var hasCacheMatch  = false;
        var currentSummary = cacheLatest.Head;
        while (currentSummary != null && !hasCacheMatch)
        {
            if (currentSummary.PeriodEndTime >= timeRange.FromTime) hasCacheMatch = true;
            currentSummary = currentSummary.Next;
        }
        if (timeRange.FromTime >= dateFirstCached && hasCacheMatch)
        {
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Context.RegisteredOn.RunOn(this, () => PublishCacheToChannel(req, timeRange));
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            return true;
        }

        if (buildPeriod > Tick)
        {
            if (!subPeriodResolverStarted) await EnsureSubPeriodResolverRunning();

            await generateHistoricalComplete;
            var historicalRetrieveRange = new UnboundedTimeRange(rangeReq?.FromTime, dateFirstCached ?? maxPreviousTimeRange);

            var limitedRecycler = Context.PooledRecycler.Borrow<LimitedBlockingRecycler>();
            limitedRecycler.MaxTypeBorrowLimit = req.PublishParams.MaxInflightEvents;
            var tickChannel    = this.CreateChannelFactory(ReceiveHistoricalPeriod, limitedRecycler);
            var channelRequest = tickChannel.ToChannelPublishRequest(-1, 50);
            historicalPeriodsPublishLookup.Add(tickChannel.Id, new HistoricalPeriodRequestState(req, tickChannel));
            var request = new HistoricalPeriodStreamRequest(historicalRetrieveRange, new ResponsePublishParams(channelRequest));

            var expectResult
                = await this.RequestAsync<HistoricalPeriodStreamRequest, bool>
                    (tickerId.HistoricalPeriodSummaryStreamRequest(buildPeriod), request);

            return expectResult;
        }
        return await GetHistoricalTicksToConvert(req);
    }

    public async ValueTask PublishCacheToChannel(HistoricalPeriodStreamRequest req, BoundedTimeRange timeRange)
    {
        var sendMore       = true;
        var currentSummary = cacheLatest.Head;
        while (currentSummary != null && sendMore)
        {
            if (currentSummary.PeriodEndTime > timeRange.FromTime && currentSummary.PeriodEndTime <= timeRange.ToTime)
                sendMore = await PublishHistoricalSummary(currentSummary, req.PublishParams);
            currentSummary = currentSummary.Next;
        }
        if (req.PublishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
        {
            var publishChannel = (IChannel<IPricePeriodSummary>)req.PublishParams.ChannelRequest!.Channel;
            await publishChannel.PublishComplete(this);
        }
    }

    private async ValueTask<bool> PublishHistoricalSummary(int channelId, IPricePeriodSummary periodSummary)
    {
        var publishParams = historicalPeriodsPublishLookup[channelId].HistoricalPeriodStreamRequest.PublishParams;
        return await PublishHistoricalSummary(periodSummary, publishParams);
    }

    private async Task<bool> PublishHistoricalSummary(IPricePeriodSummary periodSummary, ResponsePublishParams publishParams)
    {
        var keepFrom = (TimeContext.UtcNow - cacheTimeSpan).Min(existingSummariesHistoricalRange.ToTime - cacheTimeSpan);
        if (periodSummary.PeriodEndTime > keepFrom) cacheLatest.AddReplace(periodSummary);
        if (publishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
        {
            await this.PublishAsync(tickerId.PeriodSummaryRepublish(period), periodSummary.Clone());
        }
        else if (publishParams.ResponsePublishMethod == ResponsePublishMethod.AlternativeBroadcastAddress)
        {
            this.Publish(publishParams.AlternativePublishAddress!, periodSummary.Clone(), publishParams.PublishDispatchOptions);
        }
        else
        {
            var publishChannel = (IChannel<IPricePeriodSummary>)publishParams.ChannelRequest!.Channel;
            var getMore        = await publishChannel.Publish(this, periodSummary.Clone());
            return getMore;
        }
        return true;
    }

    private async ValueTask<bool> ReceiveHistoricalPeriod(ChannelEvent<IPricePeriodSummary> channelEvent)
    {
        if (channelEvent.IsLastEvent)
        {
            await PublishCachedRangeAndCloseRequest(channelEvent.ChannelId);
            return false;
        }
        var subSummaryPeriod = channelEvent.Event;
        var sendMore         = await PublishHistoricalSummary(channelEvent.ChannelId, subSummaryPeriod);
        return sendMore;
    }

    private async ValueTask<bool> GetHistoricalTicksToConvert(HistoricalPeriodStreamRequest rangeReq)
    {
        if (Equals(quotesHistoricalRange, default)) return false;
        var endHistoricalPeriods   = period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var requestHistoricalRange = quotesHistoricalRange;
        if (existingSummariesHistoricalRange.ToTime >= requestHistoricalRange.ToTime) return false;

        if (existingSummariesHistoricalRange.ToTime < requestHistoricalRange.ToTime)
            requestHistoricalRange
                = new BoundedTimeRange(existingSummariesHistoricalRange.ToTime, requestHistoricalRange.ToTime.Min(endHistoricalPeriods));

        var historicalQuotesChannel = this.CreateChannelFactory<TQuote>(ReceiveHistoricalQuote, new LimitedBlockingRecycler(200));
        var channelRequest          = historicalQuotesChannel.ToChannelPublishRequest(-1, 50);
        historicalPeriodsPublishLookup.Add(historicalQuotesChannel.Id, new HistoricalPeriodRequestState(rangeReq, historicalQuotesChannel));
        var request = channelRequest.ToHistoricalQuotesRequest(tickerId, requestHistoricalRange);

        var retrieving = await this.RequestAsync<HistoricalQuotesRequest<TQuote>, bool>(request.RequestAddress, request);
        if (!retrieving)
        {
            historicalSummariesUpToDate?.SetResult(true);
            historicalSummariesUpToDate = null;
        }
        return retrieving;
    }

    private async ValueTask<bool> ReceiveHistoricalQuote(ChannelEvent<TQuote> channelEvent)
    {
        var publishState = historicalPeriodsPublishLookup[channelEvent.ChannelId];
        if (channelEvent.IsLastEvent)
        {
            if (publishState.PreviousPeriodState is { HasPublishedComplete: false })
                await PublishHistoricalSummary
                    (channelEvent.ChannelId, publishState.PreviousPeriodState.BuildPeriodSummary
                        (Context.PooledRecycler, publishState.PreviousPeriodState.PeriodEnd()));
            if (publishState.CurrentPeriodState is { HasPublishedComplete: false }
             && publishState.CurrentPeriodState.PeriodStartTime < period.ContainingPeriodBoundaryStart(TimeContext.UtcNow))
                await PublishHistoricalSummary
                    (channelEvent.ChannelId, publishState.CurrentPeriodState.BuildPeriodSummary
                        (Context.PooledRecycler, publishState.CurrentPeriodState.PeriodEnd()));
            await PublishCachedRangeAndCloseRequest(channelEvent.ChannelId);
            historicalSummariesUpToDate?.SetResult(true);
            historicalSummariesUpToDate = null;
            return false;
        }
        var quote = channelEvent.Event;

        var currentGeneratePeriodSubSummariesState = publishState.CurrentPeriodState ??=
            new PeriodSummaryState(period.ContainingPeriodBoundaryStart(quote.SourceTime), period);

        var quotePeriodWrapper = Context.PooledRecycler.Borrow<QuoteWrappingPricePeriodSummary>();
        quotePeriodWrapper.Configure(quote);
        var quoteStartPeriod = period.ContainingPeriodBoundaryStart(quote.SourceTime);

        var sendMore = true;
        if (currentGeneratePeriodSubSummariesState.PeriodStartTime == quoteStartPeriod)
        {
            currentGeneratePeriodSubSummariesState.SubSummaryPeriods.AddLast(quotePeriodWrapper);
        }
        else if (quoteStartPeriod > currentGeneratePeriodSubSummariesState.PeriodStartTime)
        {
            var previousGeneratePeriodSubSummariesState = publishState.PreviousPeriodState;

            var noLastSummaryPeriod = previousGeneratePeriodSubSummariesState == null;
            currentGeneratePeriodSubSummariesState.NextPeriodBidAskStart = quote.BidAskTop;

            if (noLastSummaryPeriod)
            {
                previousGeneratePeriodSubSummariesState = publishState.PreviousPeriodState = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState = publishState.CurrentPeriodState = new PeriodSummaryState(quoteStartPeriod, period)
                {
                    PreviousPeriodBidAskEnd = previousGeneratePeriodSubSummariesState.SubSummaryPeriods.Tail?.EndBidAsk
                };
            }
            else
            {
                var lastPeriodEnd = previousGeneratePeriodSubSummariesState!.SubSummaryPeriods.Tail?.EndBidAsk;
                if (previousGeneratePeriodSubSummariesState.HasPublishedComplete)
                {
                    sendMore = await PublishHistoricalSummary
                        (channelEvent.ChannelId, previousGeneratePeriodSubSummariesState.BuildPeriodSummary(Context.PooledRecycler
                       , previousGeneratePeriodSubSummariesState.PeriodEnd()));
                    previousGeneratePeriodSubSummariesState.HasPublishedComplete = true;
                }

                var newCurrentPeriodSummaryState = ClearExisting(previousGeneratePeriodSubSummariesState);
                publishState.PreviousPeriodState       = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState = publishState.CurrentPeriodState = newCurrentPeriodSummaryState;
                currentGeneratePeriodSubSummariesState.Configure(quoteStartPeriod, period);
                currentGeneratePeriodSubSummariesState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentGeneratePeriodSubSummariesState.SubSummaryPeriods.AddFirst(quotePeriodWrapper);
        }
        return sendMore;
    }

    private async ValueTask CheckLaunchSubPeriodFactory(HistoricalPeriodStreamRequest streamRequest)
    {
        await EnsureSubPeriodResolverRunning();
        var requestHistoricalRange = streamRequest.RequestTimeRange;

        var historicalQuotesChannel = this.CreateChannelFactory(ReceiveHistoricalSubPeriod, new LimitedBlockingRecycler(200));
        var channelRequest          = historicalQuotesChannel.ToChannelPublishRequest(-1, 50);
        var request                 = new HistoricalPeriodStreamRequest(requestHistoricalRange, new ResponsePublishParams(channelRequest));
        historicalPeriodsPublishLookup.Add(historicalQuotesChannel.Id, new HistoricalPeriodRequestState(streamRequest, historicalQuotesChannel));

        var expectResults
            = await this.RequestAsync<HistoricalPeriodStreamRequest, bool>(tickerId.HistoricalPeriodSummaryStreamRequest(buildPeriod), request);
        if (!expectResults)
        {
            historicalSummariesUpToDate?.SetResult(true);
            historicalSummariesUpToDate = null;
        }
    }

    private async ValueTask<bool> ReceiveHistoricalSubPeriod(ChannelEvent<IPricePeriodSummary> channelEvent)
    {
        var publishState = historicalPeriodsPublishLookup[channelEvent.ChannelId];
        if (channelEvent.IsLastEvent)
        {
            if (publishState.PreviousPeriodState is { HasPublishedComplete: false })
                await PublishHistoricalSummary
                    (channelEvent.ChannelId, publishState.PreviousPeriodState.BuildPeriodSummary
                        (Context.PooledRecycler, publishState.PreviousPeriodState.PeriodEnd()));
            if (publishState.CurrentPeriodState is { HasPublishedComplete: false }
             && publishState.CurrentPeriodState.PeriodStartTime < period.ContainingPeriodBoundaryStart(TimeContext.UtcNow))
                await PublishHistoricalSummary
                    (channelEvent.ChannelId, publishState.CurrentPeriodState.BuildPeriodSummary
                        (Context.PooledRecycler, publishState.CurrentPeriodState.PeriodEnd()));
            await PublishCachedRangeAndCloseRequest(channelEvent.ChannelId);
            historicalSummariesUpToDate?.SetResult(true);
            historicalSummariesUpToDate = null;
            return false;
        }
        var subSummaryPeriod = channelEvent.Event;

        var currentGeneratePeriodSubSummariesState = publishState.CurrentPeriodState ??=
            new PeriodSummaryState(period.ContainingPeriodBoundaryStart(subSummaryPeriod.PeriodStartTime), period);
        var quoteStartPeriod = period.ContainingPeriodBoundaryStart(subSummaryPeriod.PeriodStartTime);

        var sendMore = true;
        if (currentGeneratePeriodSubSummariesState.PeriodStartTime == quoteStartPeriod)
        {
            currentGeneratePeriodSubSummariesState.SubSummaryPeriods.AddLast(subSummaryPeriod);
        }
        else if (quoteStartPeriod > currentGeneratePeriodSubSummariesState.PeriodStartTime)
        {
            var previousGeneratePeriodSubSummariesState = publishState.PreviousPeriodState;
            var noLastSummaryPeriod                     = previousGeneratePeriodSubSummariesState == null;
            currentGeneratePeriodSubSummariesState.NextPeriodBidAskStart = subSummaryPeriod.EndBidAsk;

            if (noLastSummaryPeriod)
            {
                previousGeneratePeriodSubSummariesState = publishState.PreviousPeriodState = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState = publishState.CurrentPeriodState = new PeriodSummaryState(quoteStartPeriod, period)
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
                        (channelEvent.ChannelId, previousGeneratePeriodSubSummariesState.BuildPeriodSummary(Context.PooledRecycler
                       , previousGeneratePeriodSubSummariesState.PeriodEnd()));
                    previousGeneratePeriodSubSummariesState.HasPublishedComplete = true;
                }

                var newCurrentPeriodSummaryState = ClearExisting(previousGeneratePeriodSubSummariesState);
                publishState.PreviousPeriodState       = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState = publishState.CurrentPeriodState = newCurrentPeriodSummaryState;
                currentGeneratePeriodSubSummariesState.Configure(quoteStartPeriod, period);
                currentGeneratePeriodSubSummariesState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentGeneratePeriodSubSummariesState.SubSummaryPeriods.AddFirst(subSummaryPeriod);
        }
        return sendMore;
    }

    private async ValueTask<bool> PublishCachedRangeAndCloseRequest(int channelId, UnboundedTimeRange? timeRange = null)
    {
        if (!historicalPeriodsPublishLookup.TryGetValue(channelId, out var publishState)) return false;
        var cacheRange = timeRange ?? publishState.HistoricalPeriodStreamRequest.RequestTimeRange ?? new UnboundedTimeRange();

        var maxPreviousTimeRange = period.PreviousPeriodStart(TimeContext.UtcNow);
        var bounded              = cacheRange.CapUpperTime(maxPreviousTimeRange);
        var cacheCurrent         = cacheLatest.Head;
        var sendMore             = true;
        while (cacheCurrent != null && sendMore)
        {
            if (bounded.CompletelyContains(cacheCurrent)) sendMore = await PublishHistoricalSummary(channelId, cacheCurrent);
            cacheCurrent = cacheCurrent.Next;
        }
        var publishParams = publishState.HistoricalPeriodStreamRequest.PublishParams;
        if (publishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
        {
            var publishChannel = (IChannel<IPricePeriodSummary>)publishParams.ChannelRequest!.Channel;
            historicalPeriodsPublishLookup.Remove(channelId);
            await publishChannel.PublishComplete(this);
        }
        publishState.PublishChannel?.DecrementRefCount();
        return false;
    }

    private async ValueTask EnsureSubPeriodResolverRunning()
    {
        var tickerSubPeriodService = new TickerPeriodServiceRequest
            (RequestType.StartOrStatus, ServiceType.HistoricalPricePeriodSummaryResolver, tickerId, buildPeriod
           , PQQuoteExtensions.GetQuoteLevel<TQuote>());

        var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricePeriodIndicatorsServiceStartRequest, tickerSubPeriodService);

        if (response.RunStatus is not (ServiceRunStatus.ServiceStarted or ServiceRunStatus.ServiceRestarted
                                                                       or ServiceRunStatus.ServiceAlreadyRunning))
        {
            Logger.Warn("Could not start HistoricalPeriodSummariesResolverRule for subPeriod {0}", buildPeriod);
            throw new Exception($"Could not start HistoricalPeriodSummariesResolverRule for subPeriod {buildPeriod}");
        }
        subPeriodResolverStarted = true;
    }

    private async ValueTask CheckLoadCache()
    {
        var currentSummary = cacheLatest.Head;
        if (currentSummary != null) return;
        var maxPreviousTimeRange = period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);

        var cacheFrom = maxPreviousTimeRange - cacheTimeSpan;
        var timeRange = new BoundedTimeRange(cacheFrom, maxPreviousTimeRange);

        if (existingSummariesHistoricalRange.ToTime >= timeRange.FromTime)
            if (currentSummary == null)
            {
                var request = new HistoricalPricePeriodSummaryRequestResponse(tickerId, period, timeRange);
                var repoHistoricalSummaries = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
                    (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoRequestResponse, request);
                foreach (var repoHistoricalSummary in repoHistoricalSummaries) cacheLatest.AddLast(repoHistoricalSummary);
            }
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
