// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarkets.Indicators.Persistence;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.PQ.Converters;
using FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Summaries;
using static FortitudeIO.TimeSeries.InstrumentType;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.PeriodSummaries.Construction;

public struct HistoricalPeriodParams(SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period, TimeLength cacheLength)
{
    public HistoricalPeriodParams
        (PricingInstrumentId pricingInstrumentId, TimeLength cacheLength) :
        this(pricingInstrumentId, pricingInstrumentId.CoveringPeriod.Period, cacheLength) { }

    public SourceTickerIdentifier SourceTickerIdentifier { get; set; } = sourceTickerIdentifier;
    public TimeBoundaryPeriod     Period                 { get; set; } = period;
    public PricingInstrumentId PricingInstrumentId =>
        new(SourceTickerIdentifier, new PeriodInstrumentTypePair(PriceSummaryPeriod, new DiscreetTimePeriod(Period)));

    public TimeLength CacheLength { get; set; } = cacheLength;
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
    where TQuote : class, ITimeSeriesEntry, ILevel1Quote, new()
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalPeriodSummariesResolverRule<TQuote>));

    private readonly ISummaryConstructionRequestDispatcher attendantDispatcher;

    private readonly TimeBoundaryPeriod  buildPeriod;
    private readonly PricingInstrumentId priceInstrumentId;

    private ISubscription? historicalPriceResponseRequestSubscription;
    private ISubscription? historicalPriceStreamRequestSubscription;

    private DateTime nextStorageCheckTime;

    private bool           persisterRunning;
    private ISubscription? prepareForPersisterRepublishSubscription;

    private ISubscription? recentlyCompletedSummariesSubscription;

    private ISummaryStreamRequestAttendant? startupBuildSummariesForPersister;

    private bool subPeriodResolverStarted;

    public HistoricalPeriodSummariesResolverRule(HistoricalPeriodParams historicalPeriod)
    {
        State = new ResolverState(historicalPeriod.PricingInstrumentId);

        if (State.PricingInstrumentId.CoveringPeriod.Period is (Tick or > OneYear))
            throw new Exception("Period to generate is greater than expected bounds");

        var subPeriod = State.PricingInstrumentId.CoveringPeriod.Period.GranularDivisiblePeriod();
        buildPeriod = subPeriod.RoundNonPersistPeriodsToTick();

        if (buildPeriod > Tick)
            attendantDispatcher = new SubSummaryConstructionRequestDispatcher(this, subPeriod);
        else
            attendantDispatcher = new QuoteToSummaryConstructionRequestDispatcher<TQuote>(this);

        State.CacheTimeSpan = State.PricingInstrumentId.CoveringPeriod.Period >= PricePeriodSummaryConstants.PersistPeriodsFrom
            ? historicalPeriod.CacheLength.LargerPeriodOf(State.PricingInstrumentId.CoveringPeriod.Period, 32)
            : historicalPeriod.CacheLength.ToTimeSpan().Max(TimeSpan.FromHours(4));

        priceInstrumentId = historicalPeriod.PricingInstrumentId;
    }

    private bool CanTrimCache => !persisterRunning || startupBuildSummariesForPersister?.HasCompleted == true;

    public ResolverState State { get; }

    public override async ValueTask StartAsync()
    {
        recentlyCompletedSummariesSubscription
            = await this.RegisterListenerAsync<PricePeriodSummary>
                (State.PricingInstrumentId.CompletePeriodSummaryAddress(), CacheRecentlyCompletedSummariesHandler);
        historicalPriceStreamRequestSubscription = await this.RegisterRequestListenerAsync<HistoricalPeriodStreamRequest, bool>
            (State.PricingInstrumentId.HistoricalPeriodSummaryStreamRequest(), HistoricalPeriodStreamRequestHandler);
        historicalPriceResponseRequestSubscription
            = await this.RegisterRequestListenerAsync<HistoricalPeriodResponseRequest, List<PricePeriodSummary>>
                (State.PricingInstrumentId.HistoricalPeriodSummaryResponseRequest(), HistoricalPeriodResponseRequestHandler);
        prepareForPersisterRepublishSubscription
            = await this.RegisterListenerAsync<PricePeriodSummary>
                (State.PricingInstrumentId.PersistPreparePeriodSummaryPublish(), PersisterRepublisherHandler);
        if (State.Period is >= PricePeriodSummaryConstants.PersistPeriodsFrom and <= PricePeriodSummaryConstants.PersistPeriodsTo)
        {
            var subPeriodPersisterService = new GlobalServiceRequest
                (RequestType.StartOrStatus, ServiceType.PricePeriodSummaryFilePersister);

            var response = await this.RequestAsync<GlobalServiceRequest, ServiceRunStateResponse>
                (IndicatorServiceConstants.GlobalIndicatorsServiceStartRequest, subPeriodPersisterService);
            if (response.RunStatus == ServiceRunStatus.Disabled)
                Logger.Info("File Persistence for PricePeriodSummary {0} for {1} is disable in this configuration"
                          , State.PricingInstrumentId.GetReferenceShortName(), State.Period);
            persisterRunning = response.IsRunning();
            if (persisterRunning)
            {
                var tickerSubPeriodService = new TickerPeriodServiceRequest
                    (RequestType.StartOrStatus, ServiceType.LivePricePeriodSummary, State.PricingInstrumentId
                   , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>(), PQQuoteConverterExtensions.IsPQQuoteType<TQuote>());

                response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
                    (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);
                if (!response.IsRunning())
                    Logger.Warn("Problem starting LivePricePeriodSummaryPublisherRule for ticker {0} sub period {1} got {2}",
                                State.PricingInstrumentId.GetReferenceShortName(), State.Period.GranularDivisiblePeriod().ShortName()
                              , response.RunStatus);
            }
        }

        var now = TimeContext.UtcNow;

        var maxPreviousTimeRange = State.Period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);

        await GetRepositoryQuoteInfo(now);

        await GetRepositoryExistingSummariesInfo(maxPreviousTimeRange);

        if (persisterRunning && State.ExistingRepoRange.ToTime < maxPreviousTimeRange)
        {
            if (buildPeriod is not Tick)
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
        await prepareForPersisterRepublishSubscription.NullSafeUnsubscribe();
        await base.StopAsync();
    }

    public async ValueTask EnsureSubPeriodResolverRunning()
    {
        if (subPeriodResolverStarted) return;
        var tickerSubPeriodService = new TickerPeriodServiceRequest
            (RequestType.StartOrStatus, ServiceType.HistoricalPricePeriodSummaryResolver, State.PricingInstrumentId
           , new DiscreetTimePeriod(buildPeriod)
           , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>());

        var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);

        if (response.RunStatus is not (ServiceRunStatus.ServiceStarted or ServiceRunStatus.ServiceRestarted
                                                                       or ServiceRunStatus.ServiceAlreadyRunning))
        {
            Logger.Warn("Could not start HistoricalPeriodSummariesResolverRule for subPeriod {0}", buildPeriod);
            throw new Exception($"Could not start HistoricalPeriodSummariesResolverRule for subPeriod {buildPeriod}");
        }
        subPeriodResolverStarted = true;
    }

    private async ValueTask GetRepositoryExistingSummariesInfo(DateTime maxPreviousTimeRange)
    {
        var existingRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest
            (State.PricingInstrumentId.Ticker, State.PricingInstrumentId.Source, PriceSummaryPeriod, new DiscreetTimePeriod(State.Period));
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new
                Exception($"Received More than one instrument type for {State.PricingInstrumentId.Ticker} from the repository for period {State.Period}");
        nextStorageCheckTime = TimeContext.UtcNow + State.CacheTimeSpan;
        if (candidateInstrumentFileInfo.Count == 1)
        {
            State.ExistingRepoInfo = candidateInstrumentFileInfo[0];
            if (State.ExistingRepoInfo.Value.HasInstrument)
                State.ExistingRepoRange = new BoundedTimeRange
                    (State.ExistingRepoInfo.Value.EarliestEntry!.Value
                   , State.ExistingRepoInfo.Value.LatestEntry!.Value.Min(maxPreviousTimeRange));
        }
    }

    private async ValueTask GetRepositoryQuoteInfo(DateTime now)
    {
        var quotesRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest
            (State.PricingInstrumentId.Ticker, State.PricingInstrumentId.Source, Price, new DiscreetTimePeriod(Tick));
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, quotesRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new Exception($"Received More than one instrument for {State.PricingInstrumentId.Ticker} type from the repository for quotes");
        if (candidateInstrumentFileInfo.Count == 1)
        {
            State.QuotesRepoInfo = candidateInstrumentFileInfo[0];
            if (State.QuotesRepoInfo.Value.HasInstrument)
                State.QuotesRepoRange
                    = new BoundedTimeRange
                        (State.QuotesRepoInfo.Value.EarliestEntry!.Value, State.QuotesRepoInfo.Value.LatestEntry!.Value.Min(now));
        }
    }

    private async ValueTask BuildHistoricalSummariesForPersister(DateTime maxPreviousTimeRange)
    {
        State.SubPeriodsRepoRange = State.QuotesRepoRange;

        if (State.ExistingRepoRange.ToTime >= State.QuotesRepoRange.ToTime.Min(maxPreviousTimeRange)) return;
        startupBuildSummariesForPersister = attendantDispatcher.GetStreamRequestAttendant
            (new HistoricalPeriodStreamRequest
                (new UnboundedTimeRange(State.ExistingRepoRange.ToTime.AddMicroseconds(1), null)
               , new ResponsePublishParams
                     (State.PricingInstrumentId.PersistPreparePeriodSummaryPublish()
                    , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: this))));
        startupBuildSummariesForPersister.ReadCacheFromTime = State.QuotesRepoRange.ToTime.Min(maxPreviousTimeRange);
        await startupBuildSummariesForPersister.BuildFromParts();
    }

    private async ValueTask GetSubSummaryInfoAndBuildSummariesForPersister(DateTime now, DateTime maxPreviousTimeRange)
    {
        var existingPeriodsRangeRequest
            = new TimeSeriesRepositoryInstrumentFileInfoRequest
                (State.PricingInstrumentId.Ticker, State.PricingInstrumentId.Source, PriceSummaryPeriod, new DiscreetTimePeriod(buildPeriod));
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingPeriodsRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new
                Exception($"Received More than one instrument type for {State.PricingInstrumentId.Ticker} from the repository for subPeriod {buildPeriod}");
        if (candidateInstrumentFileInfo.Count == 1)
        {
            State.SubPeriodRepoInfo = candidateInstrumentFileInfo[0];
            if (State.SubPeriodRepoInfo.Value.HasInstrument)
            {
                State.SubPeriodsRepoRange = new BoundedTimeRange
                    (State.SubPeriodRepoInfo.Value.EarliestEntry!.Value, State.SubPeriodRepoInfo.Value.LatestEntry!.Value.Min(now));

                var searchRange = new UnboundedTimeRange(State.ExistingRepoRange.ToTime.AddMicroseconds(1), null);

                startupBuildSummariesForPersister = attendantDispatcher.GetStreamRequestAttendant
                    (new HistoricalPeriodStreamRequest
                        (searchRange, new ResponsePublishParams
                            (State.PricingInstrumentId.PersistPreparePeriodSummaryPublish()
                           , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: this))));
                startupBuildSummariesForPersister.ReadCacheFromTime = State.SubPeriodsRepoRange.ToTime.Min(maxPreviousTimeRange);
                await startupBuildSummariesForPersister.BuildFromParts();
            }
        }
    }

    private async ValueTask CacheRecentlyCompletedSummariesHandler(IBusMessage<PricePeriodSummary> recentlyCompletedSummaryMsg)
    {
        var summary = recentlyCompletedSummaryMsg.Payload.Body();
        State.Cache.AddLast(summary);
        if (CanTrimCache)
        {
            if (persisterRunning)
                await this.PublishAsync
                    (State.PricingInstrumentId.PersistPreparePeriodSummaryPublish(), summary
                   , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: this));
            var now = TimeContext.UtcNow;
            if (now > nextStorageCheckTime)
            {
                var maxHistoricalTimeRange = State.Period.ContainingPeriodBoundaryStart(now);
                var existingRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest
                    (State.PricingInstrumentId.Ticker, State.PricingInstrumentId.Source, PriceSummaryPeriod, new DiscreetTimePeriod(State.Period));
                var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
                    (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingRangeRequest);

                if (candidateInstrumentFileInfo.Count == 1)
                {
                    State.ExistingRepoInfo = candidateInstrumentFileInfo[0];
                    if (State.ExistingRepoInfo!.HasValue)
                        State.ExistingRepoRange = new BoundedTimeRange
                            (State.ExistingRepoInfo.Value.EarliestEntry!.Value
                           , State.ExistingRepoInfo.Value.LatestEntry!.Value.Min(maxHistoricalTimeRange));
                }
                nextStorageCheckTime = now + State.CacheTimeSpan;
            }
            var keepFrom       = (TimeContext.UtcNow - State.CacheTimeSpan).Min(State.ExistingRepoRange.ToTime - State.CacheTimeSpan);
            var currentSummary = State.Cache.Head;
            while (currentSummary != null && currentSummary.PeriodEndTime < keepFrom)
            {
                State.Cache.Remove(currentSummary);
                currentSummary.DecrementRefCount();
            }
        }
    }

    private async ValueTask PersisterRepublisherHandler(IBusMessage<PricePeriodSummary> recentlyCompletedSummaryMsg)
    {
        if (!persisterRunning) return;
        var summary = recentlyCompletedSummaryMsg.Payload.Body();
        summary.IncrementRefCount(); // struct payloads on message bus won't duplicate or increment ref count of contained items;
        await this.PublishAsync(PricePeriodSummaryConstants.PersistAppendPeriodSummaryPublish()
                              , new ChainableInstrumentPayload<PricePeriodSummary>(priceInstrumentId, summary));
    }

    private async ValueTask<List<PricePeriodSummary>> HistoricalPeriodResponseRequestHandler
        (IBusRespondingMessage<HistoricalPeriodResponseRequest, List<PricePeriodSummary>> reqMsg)
    {
        await CheckLoadCache();

        var req       = reqMsg.Payload.Body();
        var timeRange = req.RequestTimeRange;

        var maxPreviousTimeRange = State.Period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var requestMax           = maxPreviousTimeRange.Min(timeRange.ToTime);
        var dateFirstCached      = State.Cache.Head?.PeriodStartTime ?? requestMax;

        var currentSummary = State.Cache.Head;
        var cacheResults   = new List<PricePeriodSummary>();
        while (currentSummary != null)
        {
            if (currentSummary.PeriodEndTime >= timeRange.FromTime && currentSummary.PeriodEndTime <= requestMax)
                cacheResults.Add(new PricePeriodSummary(currentSummary));
            currentSummary = currentSummary.Next;
        }
        if (timeRange.FromTime >= dateFirstCached) return cacheResults;

        var unmatchedRange = new BoundedTimeRange(timeRange.FromTime, dateFirstCached);

        if (State.ExistingRepoRange.ToTime >= unmatchedRange.FromTime &&
            State.Period >= PricePeriodSummaryConstants.PersistPeriodsFrom)
        {
            var retrieveUncachedRequest = new HistoricalPricePeriodSummaryRequestResponse(State.PricingInstrumentId, unmatchedRange);

            var remainingPeriods = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
                (HistoricalQuoteTimeSeriesRepositoryConstants.PricePeriodSummaryRepoRequestResponse, retrieveUncachedRequest);
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

        if (Equals(State.QuotesRepoRange, default)) return false;

        var req      = reqMsg.Payload.Body();
        var rangeReq = req.RequestTimeRange;

        var maxPreviousTimeRange = State.Period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var requestMax           = maxPreviousTimeRange.Min(rangeReq?.ToTime);

        var dateFirstCached = State.Cache.Head?.PeriodEndTime ?? requestMax;

        var timeRange         = new BoundedTimeRange(State.QuotesRepoRange.FromTime.Max(rangeReq?.FromTime), requestMax);
        var responseAttendant = attendantDispatcher.GetStreamRequestAttendant(req);

        var hasCacheMatch  = false;
        var currentSummary = State.Cache.Head;
        while (currentSummary != null && !hasCacheMatch)
        {
            if (currentSummary.PeriodEndTime >= timeRange.FromTime) hasCacheMatch = true;
            currentSummary = currentSummary.Next;
        }
        if (timeRange.FromTime >= dateFirstCached && hasCacheMatch) return await responseAttendant.PublishFromCache(timeRange);
        var unmatchedRange = new BoundedTimeRange(timeRange.FromTime, dateFirstCached);

        if (State.ExistingRepoRange.ToTime >= unmatchedRange.FromTime &&
            State.Period >= PricePeriodSummaryConstants.PersistPeriodsFrom)
            return await responseAttendant.RetrieveFromRepository(unmatchedRange);
        responseAttendant.ReadCacheFromTime = unmatchedRange.ToTime;
        var constructedSummaries = await responseAttendant.BuildFromParts();
        return constructedSummaries;
    }

    private async ValueTask CheckLoadCache()
    {
        var currentSummary         = State.Cache.Head;
        var endHistoricalTimeRange = State.Period.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var cacheFrom              = endHistoricalTimeRange - State.CacheTimeSpan;
        if (currentSummary != null && currentSummary.PeriodStartTime <= cacheFrom && State.Cache.Tail!.PeriodEndTime >= endHistoricalTimeRange)
            return;

        if (currentSummary != null && currentSummary.PeriodStartTime > cacheFrom)
        {
            var timeRange = new BoundedTimeRange(cacheFrom, currentSummary.PeriodStartTime);
            if (State.ExistingRepoRange.ToTime >= timeRange.FromTime)
            {
                var request = new HistoricalPricePeriodSummaryRequestResponse(State.PricingInstrumentId, timeRange);
                var repoHistoricalSummaries = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
                    (HistoricalQuoteTimeSeriesRepositoryConstants.PricePeriodSummaryRepoRequestResponse, request);
                foreach (var repoHistoricalSummary in repoHistoricalSummaries) State.Cache.AddReplace(repoHistoricalSummary);
            }
        }
        if (currentSummary != null && State.Cache.Tail!.PeriodEndTime >= endHistoricalTimeRange)
        {
            var timeRange = new BoundedTimeRange(State.Cache.Tail!.PeriodEndTime, endHistoricalTimeRange);
            if (State.ExistingRepoRange.ToTime >= timeRange.FromTime)
            {
                var request = new HistoricalPricePeriodSummaryRequestResponse(State.PricingInstrumentId, timeRange);
                var repoHistoricalSummaries = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
                    (HistoricalQuoteTimeSeriesRepositoryConstants.PricePeriodSummaryRepoRequestResponse, request);
                foreach (var repoHistoricalSummary in repoHistoricalSummaries) State.Cache.AddReplace(repoHistoricalSummary);
            }
        }
        if (currentSummary == null)
        {
            var timeRange = new BoundedTimeRange(cacheFrom, endHistoricalTimeRange);
            if (State.ExistingRepoRange.ToTime >= timeRange.FromTime)
                if (currentSummary == null)
                {
                    var request = new HistoricalPricePeriodSummaryRequestResponse(State.PricingInstrumentId, timeRange);
                    var repoHistoricalSummaries = await this.RequestAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
                        (HistoricalQuoteTimeSeriesRepositoryConstants.PricePeriodSummaryRepoRequestResponse, request);
                    foreach (var repoHistoricalSummary in repoHistoricalSummaries) State.Cache.AddReplace(repoHistoricalSummary);
                }
        }
    }
}
