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
using FortitudeCommon.Types.StringsOfPower;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using FortitudeMarkets.Indicators.Persistence;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Converters;
using FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.BusRules;
using static FortitudeIO.Storage.TimeSeries.InstrumentType;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Candles.Construction;

public struct HistoricalCandleParams(SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period, TimeLength cacheLength)
{
    public HistoricalCandleParams
        (PricingInstrumentIdValue pricingInstrumentId, TimeLength cacheLength) :
        this(pricingInstrumentId, pricingInstrumentId.CoveringPeriod.Period, cacheLength) { }

    public SourceTickerIdentifier SourceTickerIdentifier { get; set; } = sourceTickerIdentifier;
    public TimeBoundaryPeriod     Period                 { get; set; } = period;
    public PricingInstrumentIdValue PricingInstrumentId =>
        new(SourceTickerIdentifier, new PeriodInstrumentTypePair(InstrumentType.Candle, new DiscreetTimePeriod(Period)));

    public TimeLength CacheLength { get; set; } = cacheLength;
    
    public static StringBearerRevealState<HistoricalCandleParams> Styler { get; } =
        (bap, stsa) =>
            stsa.StartComplexType(bap, nameof(bap))
                .Field.AlwaysAdd(nameof(bap.SourceTickerIdentifier), bap.SourceTickerIdentifier, SourceTickerIdentifier.Styler)
                .Field.AlwaysAdd(nameof(bap.Period), bap.Period)
                .Field.AlwaysAdd(nameof(bap.PricingInstrumentId), bap.PricingInstrumentId, PricingInstrumentIdValue.Styler)
                .Field.AlwaysAdd(nameof(bap.CacheLength), bap.CacheLength, TimeLength.Styler)
                .Complete();
}

public struct HistoricalCandleStreamRequest(UnboundedTimeRange? requestTimeRange, ResponsePublishParams publishParams)
{
    public UnboundedTimeRange?   RequestTimeRange { get; set; } = requestTimeRange;
    public ResponsePublishParams PublishParams    { get; set; } = publishParams;
}

public struct HistoricalCandleResponseRequest(BoundedTimeRange requestTimeRange)
{
    public BoundedTimeRange RequestTimeRange { get; set; } = requestTimeRange;
    
    public static StringBearerRevealState<HistoricalCandleResponseRequest> Styler { get; } =
        (bap, stsa) =>
            stsa.StartComplexType(bap, nameof(bap))
                .Field.AlwaysAdd(nameof(bap.RequestTimeRange), bap.RequestTimeRange, BoundedTimeRange.Styler)
                .Complete();
}

public interface IHistoricalCandleResolverRule : IListeningRule
{
    ResolverState State { get; }
    ValueTask     EnsureSubPeriodResolverRunning();
}

public class HistoricalCandlesResolverRule<TQuote> : Rule, IHistoricalCandleResolverRule
    where TQuote : class, ITimeSeriesEntry, IPublishableLevel1Quote, new()
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalCandlesResolverRule<TQuote>));

    private readonly ICandleConstructionRequestDispatcher attendantDispatcher;

    private readonly TimeBoundaryPeriod  buildPeriod;
    private readonly PricingInstrumentIdValue priceInstrumentId;

    private ISubscription? historicalPriceResponseRequestSubscription;
    private ISubscription? historicalPriceStreamRequestSubscription;

    private DateTime nextStorageCheckTime;

    private bool           persisterRunning;
    private ISubscription? prepareForPersisterRepublishSubscription;

    private ISubscription? recentlyCompletedCandlesSubscription;

    private ICandleStreamRequestAttendant? startupBuildSummariesForPersister;

    private bool subPeriodResolverStarted;

    public HistoricalCandlesResolverRule(HistoricalCandleParams historicalCandleParams)
    {
        State = new ResolverState(historicalCandleParams.PricingInstrumentId);

        if (State.PricingInstrumentId.CoveringPeriod.Period is (Tick or > OneYear))
            throw new Exception("Period to generate is greater than expected bounds");

        var subPeriod = State.PricingInstrumentId.CoveringPeriod.Period.GranularDivisiblePeriod();
        buildPeriod = subPeriod.RoundNonPersistPeriodsToTick();

        if (buildPeriod > Tick)
            attendantDispatcher = new SubCandleConstructionRequestDispatcher(this, subPeriod);
        else
            attendantDispatcher = new QuoteToCandleConstructionRequestDispatcher<TQuote>(this);

        State.CacheTimeSpan = State.PricingInstrumentId.CoveringPeriod.Period >= CandleConstants.PersistPeriodsFrom
            ? historicalCandleParams.CacheLength.LargerPeriodOf(State.PricingInstrumentId.CoveringPeriod.Period, 32)
            : historicalCandleParams.CacheLength.ToTimeSpan().Max(TimeSpan.FromHours(4));

        priceInstrumentId = historicalCandleParams.PricingInstrumentId;
    }

    private bool CanTrimCache => !persisterRunning || startupBuildSummariesForPersister?.HasCompleted == true;

    public ResolverState State { get; }

    public override async ValueTask StartAsync()
    {
        recentlyCompletedCandlesSubscription
            = await this.RegisterListenerAsync<Candle>
                (State.PricingInstrumentId.CompleteCandleAddress(), CacheRecentlyCompletedSummariesHandler);
        historicalPriceStreamRequestSubscription = await this.RegisterRequestListenerAsync<HistoricalCandleStreamRequest, bool>
            (State.PricingInstrumentId.HistoricalCandleStreamRequest(), HistoricalCandleStreamRequestHandler);
        historicalPriceResponseRequestSubscription
            = await this.RegisterRequestListenerAsync<HistoricalCandleResponseRequest, List<Candle>>
                (State.PricingInstrumentId.HistoricalCandleResponseRequest(), HistoricalCandlesResponseRequestHandler);
        prepareForPersisterRepublishSubscription
            = await this.RegisterListenerAsync<Candle>
                (State.PricingInstrumentId.PersistPrepareCandlePublish(), PersisterRepublisherHandler);
        if (State.CandlePeriod is >= CandleConstants.PersistPeriodsFrom and <= CandleConstants.PersistPeriodsTo)
        {
            var subPeriodPersisterService = new GlobalServiceRequest
                (RequestType.StartOrStatus, ServiceType.CandleFilePersister);

            var response = await this.RequestAsync<GlobalServiceRequest, ServiceRunStateResponse>
                (IndicatorServiceConstants.GlobalIndicatorsServiceStartRequest, subPeriodPersisterService);
            if (response.RunStatus == ServiceRunStatus.Disabled)
                Logger.Info("File Persistence for Candle {0} for {1} is disable in this configuration"
                          , State.PricingInstrumentId.GetReferenceShortName(), State.CandlePeriod);
            persisterRunning = response.IsRunning();
            if (persisterRunning)
            {
                var tickerSubPeriodService = new TickerPeriodServiceRequest
                    (RequestType.StartOrStatus, ServiceType.LiveCandle, State.PricingInstrumentId
                   , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>(), PQQuoteConverterExtensions.IsPQQuoteType<TQuote>());

                response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
                    (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);
                if (!response.IsRunning())
                    Logger.Warn("Problem starting LiveCandlePublisherRule for ticker {0} sub period {1} got {2}",
                                State.PricingInstrumentId.GetReferenceShortName(), State.CandlePeriod.GranularDivisiblePeriod().ShortName()
                              , response.RunStatus);
            }
        }

        var now = TimeContext.UtcNow;

        var maxPreviousTimeRange = State.CandlePeriod.ContainingPeriodBoundaryStart(TimeContext.UtcNow);

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
        await recentlyCompletedCandlesSubscription.NullSafeUnsubscribe();
        await prepareForPersisterRepublishSubscription.NullSafeUnsubscribe();
        await base.StopAsync();
    }

    public async ValueTask EnsureSubPeriodResolverRunning()
    {
        if (subPeriodResolverStarted) return;
        var tickerSubPeriodService = new TickerPeriodServiceRequest
            (RequestType.StartOrStatus, ServiceType.HistoricalCandlesResolver, State.PricingInstrumentId
           , new DiscreetTimePeriod(buildPeriod)
           , PQQuoteConverterExtensions.GetQuoteLevel<TQuote>());

        var response = await this.RequestAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, tickerSubPeriodService);

        if (response.RunStatus is not (ServiceRunStatus.ServiceStarted or ServiceRunStatus.ServiceRestarted
                                                                       or ServiceRunStatus.ServiceAlreadyRunning))
        {
            Logger.Warn("Could not start HistoricalCandlesResolverRule for subPeriod {0}", buildPeriod);
            throw new Exception($"Could not start HistoricalCandlesResolverRule for subPeriod {buildPeriod}");
        }
        subPeriodResolverStarted = true;
    }

    private async ValueTask GetRepositoryExistingSummariesInfo(DateTime maxPreviousTimeRange)
    {
        var existingRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest
            (State.PricingInstrumentId.InstrumentName, State.PricingInstrumentId.SourceName, InstrumentType.Candle, new DiscreetTimePeriod(State.CandlePeriod));
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new
                Exception($"Received More than one instrument type for {State.PricingInstrumentId.InstrumentName} from the repository for period {State.CandlePeriod}");
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
            (State.PricingInstrumentId.InstrumentName, State.PricingInstrumentId.SourceName, Price, new DiscreetTimePeriod(Tick));
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, quotesRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new Exception($"Received More than one instrument for {State.PricingInstrumentId.InstrumentName} type from the repository for quotes");
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
        State.SubCandleRepoRange = State.QuotesRepoRange;

        if (State.ExistingRepoRange.ToTime >= State.QuotesRepoRange.ToTime.Min(maxPreviousTimeRange)) return;
        startupBuildSummariesForPersister = attendantDispatcher.GetStreamRequestAttendant
            (new HistoricalCandleStreamRequest
                (new UnboundedTimeRange(State.ExistingRepoRange.ToTime.AddMicroseconds(1), null)
               , new ResponsePublishParams
                     (State.PricingInstrumentId.PersistPrepareCandlePublish()
                    , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: this))));
        startupBuildSummariesForPersister.ReadCacheFromTime = State.QuotesRepoRange.ToTime.Min(maxPreviousTimeRange);
        await startupBuildSummariesForPersister.BuildFromParts();
    }

    private async ValueTask GetSubSummaryInfoAndBuildSummariesForPersister(DateTime now, DateTime maxPreviousTimeRange)
    {
        var existingPeriodsRangeRequest
            = new TimeSeriesRepositoryInstrumentFileInfoRequest
                (State.PricingInstrumentId.InstrumentName, State.PricingInstrumentId.SourceName, InstrumentType.Candle, new DiscreetTimePeriod(buildPeriod));
        var candidateInstrumentFileInfo = await this.RequestAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
            (TimeSeriesRepositoryConstants.TimeSeriesInstrumentFileInfoRequestResponse, existingPeriodsRangeRequest);
        if (candidateInstrumentFileInfo.Count > 1)
            throw new
                Exception($"Received More than one instrument type for {State.PricingInstrumentId.InstrumentName} from the repository for subPeriod {buildPeriod}");
        if (candidateInstrumentFileInfo.Count == 1)
        {
            State.SubCandleRepoInfo = candidateInstrumentFileInfo[0];
            if (State.SubCandleRepoInfo.Value.HasInstrument)
            {
                State.SubCandleRepoRange = new BoundedTimeRange
                    (State.SubCandleRepoInfo.Value.EarliestEntry!.Value, State.SubCandleRepoInfo.Value.LatestEntry!.Value.Min(now));

                var searchRange = new UnboundedTimeRange(State.ExistingRepoRange.ToTime.AddMicroseconds(1), null);

                startupBuildSummariesForPersister = attendantDispatcher.GetStreamRequestAttendant
                    (new HistoricalCandleStreamRequest
                        (searchRange, new ResponsePublishParams
                            (State.PricingInstrumentId.PersistPrepareCandlePublish()
                           , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: this))));
                startupBuildSummariesForPersister.ReadCacheFromTime = State.SubCandleRepoRange.ToTime.Min(maxPreviousTimeRange);
                await startupBuildSummariesForPersister.BuildFromParts();
            }
        }
    }

    private async ValueTask CacheRecentlyCompletedSummariesHandler(IBusMessage<Candle> recentlyCompletedSummaryMsg)
    {
        var summary = recentlyCompletedSummaryMsg.Payload.Body();
        State.Cache.AddLast(summary);
        if (CanTrimCache)
        {
            if (persisterRunning)
                await this.PublishAsync
                    (State.PricingInstrumentId.PersistPrepareCandlePublish(), summary
                   , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: this));
            var now = TimeContext.UtcNow;
            if (now > nextStorageCheckTime)
            {
                var maxHistoricalTimeRange = State.CandlePeriod.ContainingPeriodBoundaryStart(now);
                var existingRangeRequest = new TimeSeriesRepositoryInstrumentFileInfoRequest
                    (State.PricingInstrumentId.InstrumentName, State.PricingInstrumentId.SourceName, InstrumentType.Candle, new DiscreetTimePeriod(State.CandlePeriod));
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

    private async ValueTask PersisterRepublisherHandler(IBusMessage<Candle> recentlyCompletedSummaryMsg)
    {
        if (!persisterRunning) return;
        var summary = recentlyCompletedSummaryMsg.Payload.Body();
        summary.IncrementRefCount(); // struct payloads on message bus won't duplicate or increment ref count of contained items;
        await this.PublishAsync(CandleConstants.PersistAppendCandlePublish()
                              , new ChainableInstrumentPayload<Candle>(priceInstrumentId, summary));
    }

    private async ValueTask<List<Candle>> HistoricalCandlesResponseRequestHandler
        (IBusRespondingMessage<HistoricalCandleResponseRequest, List<Candle>> reqMsg)
    {
        await CheckLoadCache();

        var req       = reqMsg.Payload.Body();
        var timeRange = req.RequestTimeRange;

        var maxPreviousTimeRange = State.CandlePeriod.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var requestMax           = maxPreviousTimeRange.Min(timeRange.ToTime);
        var dateFirstCached      = State.Cache.Head?.PeriodStartTime ?? requestMax;

        var currentCandle = State.Cache.Head;
        var cacheResults   = new List<Candle>();
        while (currentCandle != null)
        {
            if (currentCandle.PeriodEndTime >= timeRange.FromTime && currentCandle.PeriodEndTime <= requestMax)
                cacheResults.Add(new Candle(currentCandle));
            currentCandle = currentCandle.Next;
        }
        if (timeRange.FromTime >= dateFirstCached) return cacheResults;

        var unmatchedRange = new BoundedTimeRange(timeRange.FromTime, dateFirstCached);

        if (State.ExistingRepoRange.ToTime >= unmatchedRange.FromTime &&
            State.CandlePeriod >= CandleConstants.PersistPeriodsFrom)
        {
            var retrieveUncachedRequest = new HistoricalCandleRequestResponse(State.PricingInstrumentId, unmatchedRange);

            var remainingCandles = await this.RequestAsync<HistoricalCandleRequestResponse, List<Candle>>
                (HistoricalQuoteTimeSeriesRepositoryConstants.CandleRepoRequestResponse, retrieveUncachedRequest);
            remainingCandles.AddRange(cacheResults);
            return remainingCandles;
        }

        var responseAttendant    = attendantDispatcher.GetRequestResponseAttendant(req);
        var constructedSummaries = await responseAttendant.BuildFromParts(unmatchedRange);
        constructedSummaries.AddRange(cacheResults);
        return constructedSummaries;
    }

    private async ValueTask<bool> HistoricalCandleStreamRequestHandler(IBusMessage<HistoricalCandleStreamRequest> reqMsg)
    {
        await CheckLoadCache();

        if (Equals(State.QuotesRepoRange, default)) return false;

        var req      = reqMsg.Payload.Body();
        var rangeReq = req.RequestTimeRange;

        var maxPreviousTimeRange = State.CandlePeriod.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
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
            State.CandlePeriod >= CandleConstants.PersistPeriodsFrom)
            return await responseAttendant.RetrieveFromRepository(unmatchedRange);
        responseAttendant.ReadCacheFromTime = unmatchedRange.ToTime;
        var constructedSummaries = await responseAttendant.BuildFromParts();
        return constructedSummaries;
    }

    private async ValueTask CheckLoadCache()
    {
        var currentCandle         = State.Cache.Head;
        var endHistoricalTimeRange = State.CandlePeriod.ContainingPeriodBoundaryStart(TimeContext.UtcNow);
        var cacheFrom              = endHistoricalTimeRange - State.CacheTimeSpan;
        if (currentCandle != null && currentCandle.PeriodStartTime <= cacheFrom && State.Cache.Tail!.PeriodEndTime >= endHistoricalTimeRange)
            return;

        if (currentCandle != null && currentCandle.PeriodStartTime > cacheFrom)
        {
            var timeRange = new BoundedTimeRange(cacheFrom, currentCandle.PeriodStartTime);
            if (State.ExistingRepoRange.ToTime >= timeRange.FromTime)
            {
                var request = new HistoricalCandleRequestResponse(State.PricingInstrumentId, timeRange);
                var repoHistoricalSummaries = await this.RequestAsync<HistoricalCandleRequestResponse, List<Candle>>
                    (HistoricalQuoteTimeSeriesRepositoryConstants.CandleRepoRequestResponse, request);
                foreach (var repoHistoricalSummary in repoHistoricalSummaries) State.Cache.AddReplace(repoHistoricalSummary);
            }
        }
        if (currentCandle != null && State.Cache.Tail!.PeriodEndTime >= endHistoricalTimeRange)
        {
            var timeRange = new BoundedTimeRange(State.Cache.Tail!.PeriodEndTime, endHistoricalTimeRange);
            if (State.ExistingRepoRange.ToTime >= timeRange.FromTime)
            {
                var request = new HistoricalCandleRequestResponse(State.PricingInstrumentId, timeRange);
                var repoHistoricalSummaries = await this.RequestAsync<HistoricalCandleRequestResponse, List<Candle>>
                    (HistoricalQuoteTimeSeriesRepositoryConstants.CandleRepoRequestResponse, request);
                foreach (var repoHistoricalSummary in repoHistoricalSummaries) State.Cache.AddReplace(repoHistoricalSummary);
            }
        }
        if (currentCandle == null)
        {
            var timeRange = new BoundedTimeRange(cacheFrom, endHistoricalTimeRange);
            if (State.ExistingRepoRange.ToTime >= timeRange.FromTime)
                if (currentCandle == null)
                {
                    var request = new HistoricalCandleRequestResponse(State.PricingInstrumentId, timeRange);
                    var repoHistoricalSummaries = await this.RequestAsync<HistoricalCandleRequestResponse, List<Candle>>
                        (HistoricalQuoteTimeSeriesRepositoryConstants.CandleRepoRequestResponse, request);
                    foreach (var repoHistoricalSummary in repoHistoricalSummaries) State.Cache.AddReplace(repoHistoricalSummary);
                }
        }
    }
}
