// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;
using FortitudeMarketsCore.Pricing.Summaries;
using static FortitudeIO.TimeSeries.InstrumentType;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries.Construction;

public struct HistoricalPeriodParams(ISourceTickerIdentifier tickerId, TimeSeriesPeriod period, TimeLength cacheLength)
{
    public ISourceTickerIdentifier TickerId { get; set; } = tickerId;

    public TimeSeriesPeriod Period      { get; set; } = period;
    public TimeLength       CacheLength { get; set; } = cacheLength;
}

public struct HistoricalPeriodStreamRequest(UnboundedTimeRange? requestTimeRange, ResponsePublishParams publishParams)
{
    public UnboundedTimeRange?   RequestTimeRange { get; set; } = requestTimeRange;
    public ResponsePublishParams PublishParams    { get; set; } = publishParams;
}

public struct HistoricalPeriodResponseRequest(BoundedTimeRange requestTimeRange)
{
    public BoundedTimeRange RequestTimeRange { get; set; } = requestTimeRange;
}

public interface IHistoricalPricePeriodSummaryResolverRule : IListeningRule
{
    ResolverState State { get; }
    ValueTask     EnsureSubPeriodResolverRunning();
}

public class HistoricalPeriodSummariesResolverRule<TQuote> : Rule, IHistoricalPricePeriodSummaryResolverRule
    where TQuote : class, ITimeSeriesEntry<TQuote>, ILevel1Quote, new()
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalPeriodSummariesResolverRule<TQuote>));

    private readonly ISummaryConstructionRequestDispatcher attendantDispatcher;

    private readonly TimeSeriesPeriod buildPeriod;

    private ISubscription? historicalPriceResponseRequestSubscription;
    private ISubscription? historicalPriceStreamRequestSubscription;

    private DateTime       nextStorageCheckTime;
    private ISubscription? recentlyCompletedSummariesSubscription;

    private ISummaryStreamRequestAttendant? startupBuildSummariesForPersister;

    private bool subPeriodResolverStarted;

    public HistoricalPeriodSummariesResolverRule(HistoricalPeriodParams historicalPeriod)
    {
        State = new ResolverState(historicalPeriod.TickerId, historicalPeriod.Period);

        if (State.period is (None or > OneYear)) throw new Exception("Period to generate is greater than expected bounds");

        var subPeriod = State.period.GranularDivisiblePeriod();
        buildPeriod = subPeriod.RoundNonPersistPeriodsToTick();

        if (buildPeriod > Tick)
            attendantDispatcher = new SubSummaryConstructionRequestDispatcher(this, subPeriod);
        else
            attendantDispatcher = new QuoteToSummaryConstructionRequestDispatcher<TQuote>(this);

        State.cacheTimeSpan = State.period >= PricePeriodSummaryConstants.PersistPeriodsFrom
            ? historicalPeriod.CacheLength.LargerPeriodOf(State.period, 32)
            : historicalPeriod.CacheLength.ToTimeSpan().Max(TimeSpan.FromHours(4));
    }

    public ResolverState State { get; }

    public override async ValueTask StartAsync()
    {
        recentlyCompletedSummariesSubscription
            = await this.RegisterListenerAsync<PricePeriodSummary>
                (State.tickerId.CompletePeriodSummaryAddress(State.period), CacheRecentlyCompletedSummariesHandler);
        historicalPriceStreamRequestSubscription = await this.RegisterRequestListenerAsync<HistoricalPeriodStreamRequest, bool>
            (State.tickerId.HistoricalPeriodSummaryStreamRequest(State.period), HistoricalPeriodStreamRequestHandler);
        historicalPriceResponseRequestSubscription
            = await this.RegisterRequestListenerAsync<HistoricalPeriodResponseRequest, List<PricePeriodSummary>>
                (State.tickerId.HistoricalPeriodSummaryResponseRequest(State.period), HistoricalPeriodResponseRequestHandler);
        if (State.period is >= PricePeriodSummaryConstants.PersistPeriodsFrom and <= PricePeriodSummaryConstants.PersistPeriodsTo)
        {
            var tickerSubPeriodService = new TickerPeriodServiceRequest
                (RequestType.StartOrStatus, ServiceType.PricePeriodSummaryFilePersister, State.tickerId, State.period
               , PQQuoteExtensions.GetQuoteLevel<TQuote>());

            var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
                (IndicatorServiceConstants.PricePeriodIndicatorsServiceStartRequest, tickerSubPeriodService);
            if (response.RunStatus == ServiceRunStatus.Disabled)
                Logger.Info("File Persistence for PricePeriodSummary {0} for {1} is disable in this configuration"
                          , State.tickerId.ShortName(), State.period);
        }

        var now                  = TimeContext.UtcNow;
        var maxPreviousTimeRange = State.period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);

        await GetRepositoryQuoteInfo(now);

        await GetRepositoryExistingSummariesInfo(maxPreviousTimeRange);

        if (State.existingSummariesHistoricalRange.ToTime < maxPreviousTimeRange)
        {
            if (buildPeriod is not (None or Tick))
                await GetSubSummaryInfoAndBuildSummariesForPersister(now, maxPreviousTimeRange);
            else
                await BuildHistoricalSummariesForPersister(maxPreviousTimeRange);
        }
    }

    public override async ValueTask StopAsync()
    {
        await historicalPriceStreamRequestSubscription.NullSafeUnsubscribe();
        await historicalPriceResponseRequestSubscription.NullSafeUnsubscribe();
        await recentlyCompletedSummariesSubscription.NullSafeUnsubscribe();
        await base.StopAsync();
    }

    public async ValueTask EnsureSubPeriodResolverRunning()
    {
        if (subPeriodResolverStarted) return;
        var tickerSubPeriodService = new TickerPeriodServiceRequest
            (RequestType.StartOrStatus, ServiceType.HistoricalPricePeriodSummaryResolver, State.tickerId, buildPeriod
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

    private async Task GetRepositoryExistingSummariesInfo(DateTime maxPreviousTimeRange)
    {
        var existingRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest(State.tickerId.Ticker, PriceSummaryPeriod, State.period);
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new Exception($"Received More than one instrument type for {State.tickerId.Ticker} from the repository for period {State.period}");
        nextStorageCheckTime = TimeContext.UtcNow + State.cacheTimeSpan;
        if (candidateInstrumentFileInfo.Count == 1)
        {
            State.existingSummariesInfo = candidateInstrumentFileInfo[0];
            if (State.existingSummariesInfo.Value.HasInstrument)
                State.existingSummariesHistoricalRange = new BoundedTimeRange
                    (State.existingSummariesInfo.Value.EarliestEntry!.Value
                   , State.existingSummariesInfo.Value.LatestEntry!.Value.Min(maxPreviousTimeRange));
        }
    }

    private async Task GetRepositoryQuoteInfo(DateTime now)
    {
        var quotesRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest(State.tickerId.Ticker, Price, Tick);
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, quotesRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new Exception($"Received More than one instrument for {State.tickerId.Ticker} type from the repository for quotes");
        if (candidateInstrumentFileInfo.Count == 1)
        {
            State.quotesFileInfo = candidateInstrumentFileInfo[0];
            if (State.quotesFileInfo.Value.HasInstrument)
                State.quotesHistoricalRange
                    = new BoundedTimeRange
                        (State.quotesFileInfo.Value.EarliestEntry!.Value, State.quotesFileInfo.Value.LatestEntry!.Value.Min(now));
        }
    }

    private async Task BuildHistoricalSummariesForPersister(DateTime maxPreviousTimeRange)
    {
        State.subPeriodsHistoricalRange = State.quotesHistoricalRange;

        if (State.existingSummariesHistoricalRange.ToTime >= State.quotesHistoricalRange.ToTime.Min(maxPreviousTimeRange)) return;
        startupBuildSummariesForPersister = attendantDispatcher.GetStreamRequestAttendant
            (new HistoricalPeriodStreamRequest
                (new UnboundedTimeRange(State.existingSummariesHistoricalRange.ToTime.AddMicroseconds(1), State.quotesHistoricalRange.ToTime.Min(maxPreviousTimeRange))
               , new ResponsePublishParams(State.tickerId.PersistAppendPeriodSummaryPublish(State.period))));
        await startupBuildSummariesForPersister.BuildFromParts();
    }

    private async Task GetSubSummaryInfoAndBuildSummariesForPersister(DateTime now, DateTime maxPreviousTimeRange)
    {
        var existingPeriodsRangeRequest
            = new TimeSeriesRepositoryInstrumentFileInfoRequest(State.tickerId.Ticker, PriceSummaryPeriod, buildPeriod);
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingPeriodsRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new
                Exception($"Received More than one instrument type for {State.tickerId.Ticker} from the repository for subPeriod {buildPeriod}");
        if (candidateInstrumentFileInfo.Count == 1)
        {
            State.subPeriodFileInfo = candidateInstrumentFileInfo[0];
            if (State.subPeriodFileInfo.Value.HasInstrument)
            {
                State.subPeriodsHistoricalRange = new BoundedTimeRange
                    (State.subPeriodFileInfo.Value.EarliestEntry!.Value, State.subPeriodFileInfo.Value.LatestEntry!.Value.Min(now));

                var searchRange = new UnboundedTimeRange
                    (State.existingSummariesHistoricalRange.ToTime.AddMicroseconds(1)
                   , State.subPeriodsHistoricalRange.ToTime.Min(maxPreviousTimeRange));

                startupBuildSummariesForPersister = attendantDispatcher.GetStreamRequestAttendant
                    (new HistoricalPeriodStreamRequest
                        (searchRange, new ResponsePublishParams(State.tickerId.PersistAppendPeriodSummaryPublish(State.period))));
                await startupBuildSummariesForPersister.BuildFromParts();
            }
        }
    }

    private async ValueTask CacheRecentlyCompletedSummariesHandler(IBusMessage<PricePeriodSummary> recentlyCompletedSummaryMsg)
    {
        var summary = recentlyCompletedSummaryMsg.Payload.Body();
        summary.IncrementRefCount();
        State.cacheLatest.AddLast(summary);
        if (startupBuildSummariesForPersister?.HasCompleted == true)
        {
            var now = TimeContext.UtcNow;
            if (now > nextStorageCheckTime)
            {
                var maxHistoricalTimeRange = State.period.ContainingPeriodBoundaryStart(now);
                var existingRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest(State.tickerId.Ticker, PriceSummaryPeriod, State.period);
                var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
                    (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingRangeRequest);

                if (candidateInstrumentFileInfo.Count == 1)
                {
                    State.existingSummariesInfo = candidateInstrumentFileInfo[0];
                    if (State.existingSummariesInfo!.HasValue)
                        State.existingSummariesHistoricalRange = new BoundedTimeRange
                            (State.existingSummariesInfo.Value.EarliestEntry!.Value
                           , State.existingSummariesInfo.Value.LatestEntry!.Value.Min(maxHistoricalTimeRange));
                }
                nextStorageCheckTime = now + State.cacheTimeSpan;
            }
            var keepFrom       = (TimeContext.UtcNow - State.cacheTimeSpan).Min(State.existingSummariesHistoricalRange.ToTime - State.cacheTimeSpan);
            var currentSummary = State.cacheLatest.Head;
            while (currentSummary != null && currentSummary.PeriodEndTime < keepFrom)
            {
                State.cacheLatest.Remove(currentSummary);
                currentSummary.DecrementRefCount();
            }
        }
    }

    private async ValueTask<List<PricePeriodSummary>> HistoricalPeriodResponseRequestHandler
        (IBusRespondingMessage<HistoricalPeriodResponseRequest, List<PricePeriodSummary>> reqMsg)
    {
        await CheckLoadCache();

        var req       = reqMsg.Payload.Body();
        var timeRange = req.RequestTimeRange;

        var maxPreviousTimeRange = State.period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var requestMax           = maxPreviousTimeRange.Min(timeRange.ToTime);
        var dateFirstCached      = State.cacheLatest.Head?.PeriodStartTime ?? requestMax;

        var currentSummary = State.cacheLatest.Head;
        var cacheResults   = new List<PricePeriodSummary>();
        while (currentSummary != null)
        {
            if (currentSummary.PeriodEndTime >= timeRange.FromTime && currentSummary.PeriodEndTime <= requestMax)
                cacheResults.Add(new PricePeriodSummary(currentSummary));
            currentSummary = currentSummary.Next;
        }
        if (timeRange.FromTime >= dateFirstCached) return cacheResults;

        var unmatchedRange = new BoundedTimeRange(timeRange.FromTime, dateFirstCached);

        if (State.existingSummariesHistoricalRange.ToTime >= unmatchedRange.FromTime &&
            State.period >= PricePeriodSummaryConstants.PersistPeriodsFrom)
        {
            var retrieveUncachedRequest = new HistoricalPricePeriodSummaryRequestResponse(State.tickerId, State.period, unmatchedRange);

            var remainingPeriods = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
                (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoRequestResponse, retrieveUncachedRequest);
            remainingPeriods.AddRange(cacheResults);
            return remainingPeriods;
        }

        var responseAttendant    = attendantDispatcher.GetRequestResponseAttendant(req);
        var constructedSummaries = await responseAttendant.BuildFromParts(unmatchedRange);
        constructedSummaries.AddRange(cacheResults);
        return constructedSummaries;
    }

    private async ValueTask<bool> HistoricalPeriodStreamRequestHandler(IBusMessage<HistoricalPeriodStreamRequest> reqMsg)
    {
        await CheckLoadCache();

        if (Equals(State.quotesHistoricalRange, default)) return false;

        var req      = reqMsg.Payload.Body();
        var rangeReq = req.RequestTimeRange;

        var maxPreviousTimeRange = State.period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var requestMax           = maxPreviousTimeRange.Min(rangeReq?.ToTime);

        var dateFirstCached = State.cacheLatest.Head?.PeriodEndTime ?? requestMax;

        var timeRange         = new BoundedTimeRange(State.quotesHistoricalRange.FromTime.Max(rangeReq?.FromTime), requestMax);
        var responseAttendant = attendantDispatcher.GetStreamRequestAttendant(req);

        var hasCacheMatch  = false;
        var currentSummary = State.cacheLatest.Head;
        while (currentSummary != null && !hasCacheMatch)
        {
            if (currentSummary.PeriodEndTime >= timeRange.FromTime) hasCacheMatch = true;
            currentSummary = currentSummary.Next;
        }
        if (timeRange.FromTime >= dateFirstCached && hasCacheMatch) return await responseAttendant.PublishFromCache(timeRange);
        var unmatchedRange = new BoundedTimeRange(timeRange.FromTime, dateFirstCached);

        if (State.existingSummariesHistoricalRange.ToTime >= unmatchedRange.FromTime &&
            State.period >= PricePeriodSummaryConstants.PersistPeriodsFrom)
            return await responseAttendant.RetrieveFromRepository(unmatchedRange);
        responseAttendant.ReadCacheFromTime = unmatchedRange.ToTime;
        var constructedSummaries = await responseAttendant.BuildFromParts();
        return constructedSummaries;
    }

    private async ValueTask CheckLoadCache()
    {
        var currentSummary         = State.cacheLatest.Head;
        var endHistoricalTimeRange = State.period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var cacheFrom              = endHistoricalTimeRange - State.cacheTimeSpan;
        if (currentSummary != null && currentSummary.PeriodStartTime <= cacheFrom && State.cacheLatest.Tail!.PeriodEndTime >= endHistoricalTimeRange)
            return;

        if (currentSummary != null && currentSummary.PeriodStartTime > cacheFrom)
        {
            var timeRange = new BoundedTimeRange(cacheFrom, currentSummary.PeriodStartTime);
            if (State.existingSummariesHistoricalRange.ToTime >= timeRange.FromTime)
            {
                var request = new HistoricalPricePeriodSummaryRequestResponse(State.tickerId, State.period, timeRange);
                var repoHistoricalSummaries = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
                    (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoRequestResponse, request);
                foreach (var repoHistoricalSummary in repoHistoricalSummaries)
                    State.cacheLatest.AddReplace(repoHistoricalSummary, Context.PooledRecycler);
            }
        }
        if (currentSummary != null && State.cacheLatest.Tail!.PeriodEndTime >= endHistoricalTimeRange)
        {
            var timeRange = new BoundedTimeRange(State.cacheLatest.Tail!.PeriodEndTime, endHistoricalTimeRange);
            if (State.existingSummariesHistoricalRange.ToTime >= timeRange.FromTime)
            {
                var request = new HistoricalPricePeriodSummaryRequestResponse(State.tickerId, State.period, timeRange);
                var repoHistoricalSummaries = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
                    (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoRequestResponse, request);
                foreach (var repoHistoricalSummary in repoHistoricalSummaries)
                    State.cacheLatest.AddReplace(repoHistoricalSummary, Context.PooledRecycler);
            }
        }
        if (currentSummary == null)
        {
            var timeRange = new BoundedTimeRange(cacheFrom, endHistoricalTimeRange);
            if (State.existingSummariesHistoricalRange.ToTime >= timeRange.FromTime)
                if (currentSummary == null)
                {
                    var request = new HistoricalPricePeriodSummaryRequestResponse(State.tickerId, State.period, timeRange);
                    var repoHistoricalSummaries = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
                        (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoRequestResponse, request);
                    foreach (var repoHistoricalSummary in repoHistoricalSummaries)
                        State.cacheLatest.AddReplace(repoHistoricalSummary, Context.PooledRecycler);
                }
        }
    }
}
