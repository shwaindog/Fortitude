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

    public ISourceTickerIdentifier TickerId    { get; set; }
    public TimeSeriesPeriod        Period      { get; set; }
    public TimeLength              CacheLength { get; set; }
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

public struct HistoricalPeriodRequestState
{
    public HistoricalPeriodRequestState(HistoricalPeriodStreamRequest periodStreamRequest, IChannel publishChannel)
    {
        HistoricalPeriodStreamRequest = periodStreamRequest;
        PublishChannel                = publishChannel;
    }

    public HistoricalPeriodStreamRequest HistoricalPeriodStreamRequest { get; set; }
    public IChannel?                     PublishChannel                { get; set; }
}

public class HistoricalPeriodSummariesResolverRule<TQuote> : Rule where TQuote : class, ITimeSeriesEntry<TQuote>, ILevel1Quote
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalPeriodSummariesResolverRule<TQuote>));
    private readonly        TimeSeriesPeriod buildPeriod;
    private readonly        IDoublyLinkedList<IPricePeriodSummary> cacheLatest = new DoublyLinkedList<IPricePeriodSummary>();
    private readonly        Task<bool> generateHistoricalComplete;

    private readonly HistoricalPeriodParams historicalPeriodParams;

    private readonly Dictionary<int, HistoricalPeriodRequestState> historicalPeriodsPublishLookup = new();

    private readonly TaskCompletionSource<bool> historicalSummariesUpToDate;

    private readonly TimeSeriesPeriod        period;
    private readonly TimeSeriesPeriod        subPeriod;
    private readonly ISourceTickerIdentifier tickerId;

    private TimeSpan cacheTimeSpan;

    private PeriodSummaryState? currentGeneratePeriodSubSummariesState;
    private BoundedTimeRange    existingSummariesHistoricalRange;
    private InstrumentFileInfo? existingSummariesInfo;
    private ISubscription?      historicalPriceResponseRequestSubscription;

    private ISubscription? historicalPriceStreamRequestSubscription;
    private DateTime       nextStorageCheckTime;

    private PeriodSummaryState? previousGeneratePeriodSubSummariesState;

    private InstrumentFileInfo? quotesFileInfo;
    private BoundedTimeRange    quotesHistoricalRange;
    private ISubscription?      recentlyCompletedSummariesSubscription;

    private InstrumentFileInfo? subPeriodFileInfo;
    private bool                subPeriodResolverStarted;
    private BoundedTimeRange    subPeriodsHistoricalRange;

    public HistoricalPeriodSummariesResolverRule(HistoricalPeriodParams historicalPeriod)
    {
        historicalPeriodParams = historicalPeriod;
        tickerId               = historicalPeriod.TickerId;
        period                 = historicalPeriod.Period;
        if (period is (None or > OneYear)) throw new Exception("Period to generate is greater than expected bounds");
        subPeriod                   = period.GranularDivisiblePeriod();
        buildPeriod                 = subPeriod.RoundNonPersistPeriodsToTick();
        historicalSummariesUpToDate = new TaskCompletionSource<bool>();
        generateHistoricalComplete  = historicalSummariesUpToDate.Task;
        cacheTimeSpan = buildPeriod != Tick
            ? historicalPeriod.CacheLength.LargerPeriodOf(period, 32)
            : historicalPeriod.CacheLength.ToTimeSpan().Max(TimeSpan.FromHours(4));
    }

    public override async ValueTask StartAsync()
    {
        recentlyCompletedSummariesSubscription
            = await this.RegisterListenerAsync<IPricePeriodSummary>(tickerId.CompletePeriodSummaryAddress(period)
                                                                  , CacheRecentlyCompletedSummariesHandler);
        if (period is >= PricePeriodSummaryConstants.PersistPeriodsFrom and <= PricePeriodSummaryConstants.PersistPeriodsTo)
        {
            var tickerSubPeriodService = new TickerPeriodServiceRequest
                (RequestType.StartOrStatus, ServiceType.PricePeriodSummaryFilePersister, tickerId, period, PQQuoteExtensions.GetQuoteLevel<TQuote>());

            var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
                (IndicatorServiceConstants.PricePeriodIndicatorsServiceStartRequest, tickerSubPeriodService);
            if (response.RunStatus == ServiceRunStatus.Disabled)
                Logger.Info("File Persistence for PricePeriodSummary {0} for {1} is disable in this configuration", tickerId.ShortName(), period);
        }
        var maxPreviousTimeRange = period.PreviousPeriodStart(DateTime.UtcNow);
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
                    = new BoundedTimeRange(quotesFileInfo.Value.EarliestEntry!.Value
                                         , quotesFileInfo.Value.LatestEntry!.Value.Min(maxPreviousTimeRange));
        }

        var existingRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest(tickerId.Ticker, PriceSummaryPeriod, period);
        candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new Exception($"Received More than one instrument type for {tickerId.Ticker} from the repository for period {period}");
        nextStorageCheckTime = DateTime.UtcNow + cacheTimeSpan;
        if (candidateInstrumentFileInfo.Count == 1)
        {
            existingSummariesInfo = candidateInstrumentFileInfo[0];
            if (existingSummariesInfo.Value.HasInstrument)
                existingSummariesHistoricalRange = new BoundedTimeRange
                    (existingSummariesInfo.Value.EarliestEntry!.Value, existingSummariesInfo.Value.LatestEntry!.Value.Min(maxPreviousTimeRange));
        }

        if (buildPeriod is not (None or Tick))
        {
            var existingPeriodsRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest(tickerId.Ticker, PriceSummaryPeriod, buildPeriod);
            candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
                (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingPeriodsRangeRequest);
            if (candidateInstrumentFileInfo.Count > 1)
                throw new Exception($"Received More than one instrument type for {tickerId.Ticker} from the repository for subPeriod {buildPeriod}");
            if (candidateInstrumentFileInfo.Count == 1)
            {
                subPeriodFileInfo = candidateInstrumentFileInfo[0];
                if (subPeriodFileInfo.Value.HasInstrument)
                {
                    subPeriodsHistoricalRange = new BoundedTimeRange
                        (subPeriodFileInfo.Value.EarliestEntry!.Value, subPeriodFileInfo.Value.LatestEntry!.Value.Min(maxPreviousTimeRange));
                    await CheckLaunchSubPeriodFactory
                        (new HistoricalPeriodStreamRequest
                            (new UnboundedTimeRange(quotesHistoricalRange.FromTime, existingSummariesHistoricalRange.ToTime.Min(maxPreviousTimeRange))
                           , new ResponsePublishParams()));
                }
                else
                {
                    historicalSummariesUpToDate.SetResult(true);
                }
            }
        }
        else
        {
            subPeriodsHistoricalRange = quotesHistoricalRange;
            await GetHistoricalTicksToConvert
                (new HistoricalPeriodStreamRequest
                    (new UnboundedTimeRange(quotesHistoricalRange.FromTime, existingSummariesHistoricalRange.ToTime.Min(maxPreviousTimeRange))
                   , new ResponsePublishParams()));
        }
        historicalPriceStreamRequestSubscription = await this.RegisterRequestListenerAsync<HistoricalPeriodStreamRequest, bool>
            (tickerId.HistoricalPeriodSummaryStreamRequest(period), HistoricalPeriodStreamRequestHandler);
        historicalPriceResponseRequestSubscription
            = await this.RegisterRequestListenerAsync<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>>
                (tickerId.HistoricalPeriodSummaryResponseRequest(period), HistoricalPeriodResponseRequestHandler);
    }

    private async ValueTask CacheRecentlyCompletedSummariesHandler(IBusMessage<IPricePeriodSummary> recentlyCompletedSummaryMsg)
    {
        var summary = recentlyCompletedSummaryMsg.Payload.Body();
        summary.IncrementRefCount();
        cacheLatest.AddLast(summary);
        if (generateHistoricalComplete.IsCompleted)
        {
            var now = DateTime.UtcNow;
            if (now > nextStorageCheckTime)
            {
                var maxPreviousTimeRange = period.PreviousPeriodStart(now);
                var existingRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest(tickerId.Ticker, PriceSummaryPeriod, period);
                var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
                    (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingRangeRequest);

                if (candidateInstrumentFileInfo.Count == 1)
                {
                    existingSummariesInfo = candidateInstrumentFileInfo[0];
                    if (existingSummariesInfo!.HasValue)
                        existingSummariesHistoricalRange = new BoundedTimeRange
                            (existingSummariesInfo.Value.EarliestEntry!.Value
                           , existingSummariesInfo.Value.LatestEntry!.Value.Min(maxPreviousTimeRange));
                }
                nextStorageCheckTime = now + cacheTimeSpan;
            }
            var keepFrom       = (DateTime.UtcNow - cacheTimeSpan).Min(existingSummariesHistoricalRange.ToTime - cacheTimeSpan);
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
        if (historicalPriceStreamRequestSubscription != null) await historicalPriceStreamRequestSubscription.UnsubscribeAsync();
        if (historicalPriceResponseRequestSubscription != null) await historicalPriceResponseRequestSubscription.UnsubscribeAsync();
        await base.StopAsync();
    }

    private async ValueTask<List<IPricePeriodSummary>> HistoricalPeriodResponseRequestHandler
        (IBusRespondingMessage<HistoricalPeriodResponseRequest, List<IPricePeriodSummary>> reqMsg)
    {
        await generateHistoricalComplete;

        var timeRange       = reqMsg.Payload.Body().RequestTimeRange;
        var dateFirstCached = cacheLatest.Head?.PeriodStartTime ?? DateTimeConstants.UnixEpoch;
        var cacheResults    = new List<IPricePeriodSummary>();
        var currentSummary  = cacheLatest.Head;
        while (currentSummary != null)
        {
            if (currentSummary.PeriodEndTime >= timeRange.FromTime) cacheResults.Add(currentSummary);
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
                    remainingPeriods.Add(currentPeriod);
                    currentPeriod = new PricePeriodSummary(period, period.ContainingPeriodBoundaryStart(subPeriodHistory.PeriodStartTime));
                }
                currentPeriod.MergeBoundaries(subPeriodHistory);
            }
            remainingPeriods.AddRange(cacheResults);
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
            var summaryPopulateChannel = this.CreateChannelFactory(ce =>
            {
                if (ce.IsLastEvent)
                    rebuildCompleteSource.SetResult(true);
                else
                    remainingPeriods.Add(ce.Event);
                return !ce.IsLastEvent;
            }, limitedRecycler);
            var rebuildRequest     = summaryPopulateChannel.ToChannelPublishRequest();
            var tickChannel        = this.CreateChannelFactory<TQuote>(ReceiveHistoricalQuote, new LimitedBlockingRecycler(200));
            var tickChannelRequest = tickChannel.ToChannelPublishRequest(-1, 50);
            var req                = new HistoricalPeriodStreamRequest(unmatchedRange, new ResponsePublishParams(rebuildRequest));
            historicalPeriodsPublishLookup.Add(tickChannel.Id, new HistoricalPeriodRequestState(req, tickChannel));
            var request = new HistoricalPeriodStreamRequest(unmatchedRange, new ResponsePublishParams(tickChannelRequest));

            var expectResult
                = await this.RequestAsync<HistoricalPeriodStreamRequest, bool>
                    (tickerId.HistoricalPeriodSummaryStreamRequest(buildPeriod), request);

            if (!expectResult) return cacheResults;

            await rebuildCompleteTask;

            remainingPeriods.AddRange(cacheResults);

            return remainingPeriods;
        }
    }

    private async ValueTask<bool> HistoricalPeriodStreamRequestHandler(IBusMessage<HistoricalPeriodStreamRequest> reqMsg)
    {
        if (Equals(quotesHistoricalRange, default)) return false;

        var req      = reqMsg.Payload.Body();
        var rangeReq = req.RequestTimeRange;

        var maxPreviousTimeRange = period.PreviousPeriodStart(DateTime.UtcNow);
        var requestMax           = rangeReq?.ToTime ?? maxPreviousTimeRange;

        var dateFirstCached = cacheLatest.Head?.PeriodStartTime;

        if (!subPeriodResolverStarted) await EnsureSubPeriodResolverRunning();

        if ((rangeReq?.FromTime == null && subPeriodsHistoricalRange.FromTime < dateFirstCached)
         || (rangeReq?.FromTime != null && rangeReq.Value.FromTime.Value < requestMax))
        {
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

            if (!expectResult && dateFirstCached == null) return false;
            if (!expectResult && dateFirstCached != null) await PublishCachedRangeAndCloseRequest(tickChannel.Id, historicalRetrieveRange);
        }
        return true;
    }

    private async ValueTask<bool> PublishHistoricalSummary(int channelId, IPricePeriodSummary periodSummary)
    {
        var publishParams = historicalPeriodsPublishLookup[channelId].HistoricalPeriodStreamRequest.PublishParams;
        var keepFrom      = (DateTime.UtcNow - cacheTimeSpan).Min(existingSummariesHistoricalRange.ToTime - cacheTimeSpan);
        if (periodSummary.PeriodEndTime > keepFrom) cacheLatest.AddReplace(periodSummary);
        if (publishParams.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
        {
            await this.PublishAsync(tickerId.PersistAppendPeriodSummaryPublish(period), periodSummary);
        }
        else if (publishParams.ResponsePublishMethod == ResponsePublishMethod.AlternativeBroadcastAddress)
        {
            this.Publish(publishParams.AlternativePublishAddress!, periodSummary, publishParams.PublishDispatchOptions);
        }
        else
        {
            var publishChannel = (IChannel<IPricePeriodSummary>)publishParams.ChannelRequest!.Channel;
            var getMore        = await publishChannel.Publish(this, periodSummary);
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

    private async ValueTask GetHistoricalTicksToConvert(HistoricalPeriodStreamRequest rangeReq)
    {
        if (Equals(quotesHistoricalRange, default)) return;
        var requestHistoricalRange = quotesHistoricalRange;
        if (existingSummariesHistoricalRange.ToTime >= requestHistoricalRange.ToTime) return;

        if (existingSummariesHistoricalRange.ToTime < requestHistoricalRange.ToTime)
            requestHistoricalRange = new BoundedTimeRange(existingSummariesHistoricalRange.ToTime, requestHistoricalRange.ToTime);

        var historicalQuotesChannel = this.CreateChannelFactory<TQuote>(ReceiveHistoricalQuote, new LimitedBlockingRecycler(200));
        var channelRequest          = historicalQuotesChannel.ToChannelPublishRequest(-1, 50);
        historicalPeriodsPublishLookup.Add(historicalQuotesChannel.Id, new HistoricalPeriodRequestState(rangeReq, historicalQuotesChannel));
        var request = channelRequest.ToHistoricalQuotesRequest(tickerId, requestHistoricalRange);

        var retrieving = await this.RequestAsync<HistoricalQuotesRequest<TQuote>, bool>(request.RequestAddress, request);
        if (!retrieving) historicalSummariesUpToDate.SetResult(true);
    }

    private async ValueTask<bool> ReceiveHistoricalQuote(ChannelEvent<TQuote> channelEvent)
    {
        if (channelEvent.IsLastEvent)
        {
            await PublishCachedRangeAndCloseRequest(channelEvent.ChannelId);
            historicalSummariesUpToDate.SetResult(true);
            return false;
        }
        var quote = channelEvent.Event;
        currentGeneratePeriodSubSummariesState ??= new PeriodSummaryState(period.ContainingPeriodBoundaryStart(quote.SourceTime), period);
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
            var noLastSummaryPeriod = previousGeneratePeriodSubSummariesState == null;
            currentGeneratePeriodSubSummariesState.NextPeriodBidAskStart = quote.BidAskTop;

            if (noLastSummaryPeriod)
            {
                previousGeneratePeriodSubSummariesState = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState = new PeriodSummaryState(quoteStartPeriod, period)
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
                previousGeneratePeriodSubSummariesState = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState  = newCurrentPeriodSummaryState;
                currentGeneratePeriodSubSummariesState.Configure(quoteStartPeriod, period);
                currentGeneratePeriodSubSummariesState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentGeneratePeriodSubSummariesState.SubSummaryPeriods.AddFirst(quotePeriodWrapper);
        }
        return sendMore;
    }

    private async ValueTask CheckLaunchSubPeriodFactory(HistoricalPeriodStreamRequest rangeReq)
    {
        await EnsureSubPeriodResolverRunning();
        var requestHistoricalRange = subPeriodsHistoricalRange;
        if (existingSummariesHistoricalRange.ToTime >= requestHistoricalRange.ToTime) return;

        if (existingSummariesHistoricalRange.ToTime < requestHistoricalRange.ToTime)
            requestHistoricalRange = new BoundedTimeRange(existingSummariesHistoricalRange.ToTime, requestHistoricalRange.ToTime);

        var historicalQuotesChannel = this.CreateChannelFactory(ReceiveHistoricalSubPeriod, new LimitedBlockingRecycler(200));
        var channelRequest          = historicalQuotesChannel.ToChannelPublishRequest(-1, 50);
        historicalPeriodsPublishLookup.Add(historicalQuotesChannel.Id, new HistoricalPeriodRequestState(rangeReq, historicalQuotesChannel));
        var request = new HistoricalPeriodStreamRequest(requestHistoricalRange, new ResponsePublishParams(channelRequest));

        var expectResults
            = await this.RequestAsync<HistoricalPeriodStreamRequest, bool>(tickerId.HistoricalPeriodSummaryStreamRequest(buildPeriod), request);
        if (!expectResults) historicalSummariesUpToDate.SetResult(true);
    }

    private async ValueTask<bool> ReceiveHistoricalSubPeriod(ChannelEvent<IPricePeriodSummary> channelEvent)
    {
        if (channelEvent.IsLastEvent)
        {
            await PublishCachedRangeAndCloseRequest(channelEvent.ChannelId);
            historicalSummariesUpToDate.SetResult(true);
            return false;
        }
        var subSummaryPeriod = channelEvent.Event;
        currentGeneratePeriodSubSummariesState
            ??= new PeriodSummaryState(period.ContainingPeriodBoundaryStart(subSummaryPeriod.PeriodStartTime), period);
        var quoteStartPeriod = period.ContainingPeriodBoundaryStart(subSummaryPeriod.PeriodStartTime);

        var sendMore = true;
        if (currentGeneratePeriodSubSummariesState.PeriodStartTime == quoteStartPeriod)
        {
            currentGeneratePeriodSubSummariesState.SubSummaryPeriods.AddLast(subSummaryPeriod);
        }
        else if (quoteStartPeriod > currentGeneratePeriodSubSummariesState.PeriodStartTime)
        {
            var noLastSummaryPeriod = previousGeneratePeriodSubSummariesState == null;
            currentGeneratePeriodSubSummariesState.NextPeriodBidAskStart = subSummaryPeriod.EndBidAsk;

            if (noLastSummaryPeriod)
            {
                previousGeneratePeriodSubSummariesState = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState = new PeriodSummaryState(quoteStartPeriod, period)
                {
                    PreviousPeriodBidAskEnd = previousGeneratePeriodSubSummariesState.SubSummaryPeriods.Tail?.EndBidAsk
                };
            }
            else
            {
                var lastPeriodEnd = previousGeneratePeriodSubSummariesState!.SubSummaryPeriods.Tail?.EndBidAsk;
                if (!previousGeneratePeriodSubSummariesState.HasPublishedComplete)
                {
                    sendMore = await PublishHistoricalSummary(channelEvent.ChannelId
                                                            , previousGeneratePeriodSubSummariesState.BuildPeriodSummary(Context.PooledRecycler
                                                             , previousGeneratePeriodSubSummariesState.PeriodEnd()));
                    previousGeneratePeriodSubSummariesState.HasPublishedComplete = true;
                }

                var newCurrentPeriodSummaryState = ClearExisting(previousGeneratePeriodSubSummariesState);
                previousGeneratePeriodSubSummariesState = currentGeneratePeriodSubSummariesState;
                currentGeneratePeriodSubSummariesState  = newCurrentPeriodSummaryState;
                currentGeneratePeriodSubSummariesState.Configure(quoteStartPeriod, period);
                currentGeneratePeriodSubSummariesState.PreviousPeriodBidAskEnd = lastPeriodEnd;
            }
            currentGeneratePeriodSubSummariesState.SubSummaryPeriods.AddFirst(subSummaryPeriod);
        }
        return sendMore;
    }

    private async ValueTask<bool> PublishCachedRangeAndCloseRequest(int channelId, UnboundedTimeRange? timeRange = null)
    {
        var publishState = historicalPeriodsPublishLookup[channelId];
        var cacheRange   = timeRange ?? publishState.HistoricalPeriodStreamRequest.RequestTimeRange ?? new UnboundedTimeRange();

        var maxPreviousTimeRange = period.PreviousPeriodStart(DateTime.UtcNow);
        var bounded              = cacheRange.CapUpperTime(maxPreviousTimeRange);
        var cacheCurrent         = cacheLatest.Head;
        var sendMore             = true;
        while (cacheCurrent != null)
        {
            if (bounded.CompletelyContains(cacheCurrent)) sendMore = await PublishHistoricalSummary(channelId, cacheCurrent);
            cacheCurrent = cacheCurrent.Next;
        }
        var publishParams = publishState.HistoricalPeriodStreamRequest.PublishParams;
        if (publishParams.ResponsePublishMethod == ResponsePublishMethod.ReceiverChannel)
        {
            var publishChannel = (IChannel<IPricePeriodSummary>)publishParams.ChannelRequest!.Channel;
            await publishChannel.PublishComplete(this);
        }
        publishState.PublishChannel?.DecrementRefCount();
        historicalPeriodsPublishLookup.Remove(channelId);
        return sendMore;
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
