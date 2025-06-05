// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Indicators.Config;
using FortitudeMarkets.Indicators.Persistence;
using FortitudeMarkets.Indicators.Pricing;
using FortitudeMarkets.Indicators.Pricing.Candles;
using FortitudeMarkets.Indicators.Pricing.Candles.Construction;
using FortitudeMarkets.Indicators.Pricing.MovingAverage.TimeWeighted;
using FortitudeMarkets.Indicators.Pricing.Parameters;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;

#endregion

namespace FortitudeMarkets.Indicators;

public enum ServiceType
{
    ServiceRegistry
  , TimeSeriesFileRepositoryInfo
  , HistoricalQuotesRetriever
  , LiveQuote
  , HistoricalQuoteResolver
  , LiveCandle
  , HistoricalCandlesRetriever
  , HistoricalCandlesResolver
  , CandleFilePersister
  , LiveMovingAverage
  , HistoricalMovingAverageResolver
}

public enum RequestType
{
    StartOrStatus
  , StatusOnly
  , RegisterUsageCount
  , ShutdownRequest
}

public enum ServiceRunStatus
{
    NoServiceFound
  , Disabled
  , NotStarted
  , AwaitingHeartbeatResponse
  , HeartbeatResponseReceived
  , ServiceStarted
  , ServiceAlreadyRunning
  , ServiceRestarted
  , ServiceStartFailed
  , ServiceStopped
}

public struct TickerPeriodServiceRequest
{
    public TickerPeriodServiceRequest
    (RequestType requestType, ServiceType serviceType, PricingInstrumentIdValue pricingInstrumentId
      , TickerQuoteDetailLevel tickerQuoteDetailLevel = TickerQuoteDetailLevel.Level1Quote, bool usePqQuote = false)
    {
        RequestType = requestType;
        TickerPeriodServiceInfo
            = new TickerPeriodServiceInfo(serviceType, pricingInstrumentId, pricingInstrumentId.CoveringPeriod, tickerQuoteDetailLevel, usePqQuote);
    }

    public TickerPeriodServiceRequest
    (RequestType requestType, ServiceType serviceType, SourceTickerIdentifier sourceTickerIdentifier, DiscreetTimePeriod? period = null
      , TickerQuoteDetailLevel tickerQuoteDetailLevel = TickerQuoteDetailLevel.Level1Quote, bool usePqQuote = false)
    {
        RequestType             = requestType;
        TickerPeriodServiceInfo = new TickerPeriodServiceInfo(serviceType, sourceTickerIdentifier, period, tickerQuoteDetailLevel, usePqQuote);
    }

    public TickerPeriodServiceRequest
        (RequestType requestType, TickerPeriodServiceInfo tickerPeriodServiceInfo)
    {
        RequestType             = requestType;
        TickerPeriodServiceInfo = tickerPeriodServiceInfo;
    }

    public TickerPeriodServiceInfo TickerPeriodServiceInfo { get; }

    public RequestType RequestType { get; }
}

public struct GlobalServiceRequest
{
    public GlobalServiceRequest
        (RequestType requestType, ServiceType serviceType)
    {
        RequestType = requestType;
        ServiceType = serviceType;
    }

    public ServiceType ServiceType { get; }
    public RequestType RequestType { get; }
}

public struct TickerPeriodServiceInfo
{
    public TickerPeriodServiceInfo
    (ServiceType serviceType, PricingInstrumentIdValue pricingInstrumentId
      , TickerQuoteDetailLevel tickerQuoteDetailLevel = TickerQuoteDetailLevel.Level1Quote, bool usePqQuote = false)
    {
        ServiceType         = serviceType;
        PricingInstrumentId = pricingInstrumentId;
        TickerQuoteDetailLevel   = tickerQuoteDetailLevel;
        UsePqQuote          = usePqQuote;
    }

    public TickerPeriodServiceInfo
    (ServiceType serviceType, SourceTickerIdentifier sourceTickerIdentifier, DiscreetTimePeriod? period = null
      , TickerQuoteDetailLevel tickerQuoteDetailLevel = TickerQuoteDetailLevel.Level1Quote, bool usePqQuote = false)
    {
        ServiceType = serviceType;
        PricingInstrumentId = new PricingInstrumentIdValue(sourceTickerIdentifier
                                                    , new PeriodInstrumentTypePair(InstrumentType.Custom, period ?? new DiscreetTimePeriod(Tick)));
        TickerQuoteDetailLevel = tickerQuoteDetailLevel;
        UsePqQuote        = usePqQuote;
    }

    public ServiceType ServiceType { get; }

    public PricingInstrumentIdValue PricingInstrumentId { get; }
    public TickerQuoteDetailLevel   TickerQuoteDetailLevel   { get; set; }

    public bool UsePqQuote { get; set; }
}

public class ServiceRuntimeState
{
    private ServiceRunStateResponse lastStartResult;

    public ServiceRuntimeState
        (ServiceRunStatus runStatus = ServiceRunStatus.NoServiceFound) =>
        LastStartResult = new ServiceRunStateResponse(runStatus);

    public ServiceRuntimeState() => LastStartResult = new ServiceRunStateResponse();
    public ServiceRuntimeState(ServiceRunStateResponse lastStartResult) => LastStartResult = lastStartResult;

    public ServiceRunStatus RunStatus => LastStatusUpdate?.RunStatus ?? LastStartResult.RunStatus;

    public IRule? Rule => LastStatusUpdate?.Rule ?? LastStartResult.Rule;

    public ServiceStatusUpdate? LastStatusUpdate { get; set; }
    public ServiceRunStateResponse LastStartResult
    {
        get => lastStartResult;
        set
        {
            if (LastStatusUpdate != null)
                LastStatusUpdate = new ServiceStatusUpdate
                    (LastStatusUpdate.Value.TickerPeriodServiceRequestInfo, LastStatusUpdate.Value.Rule, value.RunStatus
                   , LastStatusUpdate.Value.AtTime);
            lastStartResult = value;
        }
    }
}

public struct ServiceRunStateResponse
{
    public ServiceRunStateResponse(ServiceRunStatus runStatus = ServiceRunStatus.NoServiceFound)
    {
        RunStatus = runStatus;

        LastUpdateTime = DateTime.UtcNow;
    }

    public ServiceRunStateResponse(IRule rule, ServiceRunStatus runStatus)
    {
        Rule      = rule;
        RunStatus = runStatus;

        LastUpdateTime = DateTime.UtcNow;
    }

    public ServiceRunStateResponse(IRule rule, ServiceRunStatus runStatus, DateTime? lastUpdateTime = null)
    {
        Rule      = rule;
        RunStatus = runStatus;

        LastUpdateTime = lastUpdateTime ?? DateTime.UtcNow;
    }

    public IRule? Rule { get; }

    public ServiceRunStatus RunStatus { get; }

    public DateTime LastUpdateTime { get; }
}

public static class ServiceRunStateResponseExtensions
{
    public static bool IsRunning(this ServiceRunStateResponse response) =>
        response.RunStatus is
            ServiceRunStatus.ServiceRestarted or ServiceRunStatus.HeartbeatResponseReceived or ServiceRunStatus.AwaitingHeartbeatResponse or
            ServiceRunStatus.ServiceAlreadyRunning or ServiceRunStatus.ServiceStarted;
}

public struct ServiceStatusUpdate
{
    public ServiceStatusUpdate(TickerPeriodServiceRequest tickerPeriodServiceRequestInfo, IRule rule, ServiceRunStatus runStatus, DateTime? atTime)
    {
        TickerPeriodServiceRequestInfo = tickerPeriodServiceRequestInfo;

        Rule      = rule;
        RunStatus = runStatus;
        AtTime    = atTime ?? DateTime.UtcNow;
    }

    public TickerPeriodServiceRequest TickerPeriodServiceRequestInfo { get; }

    public IRule Rule { get; }

    public ServiceRunStatus RunStatus { get; }

    public DateTime? AtTime { get; }
}

public struct IndicatorServiceRegistryParams
{
    public IndicatorServiceRegistryParams
    (IIndicatorServicesConfig indicatorServiceConfig
      , Dictionary<ServiceType, Func<TickerPeriodServiceRequest, ServiceRuntimeState>>? overrideTickerPeriodFactory = null
      , Dictionary<ServiceType, Func<GlobalServiceRequest, ServiceRuntimeState>>? overrideGlobalServiceFactory = null)
    {
        IndicatorServiceConfig = indicatorServiceConfig;
        TickerPeriodServiceFactoryOverrides
            = overrideTickerPeriodFactory ?? new Dictionary<ServiceType, Func<TickerPeriodServiceRequest, ServiceRuntimeState>>();
        GlobalServiceFactoryOverrides
            = overrideGlobalServiceFactory ?? new Dictionary<ServiceType, Func<GlobalServiceRequest, ServiceRuntimeState>>();
    }

    public Dictionary<ServiceType, Func<TickerPeriodServiceRequest, ServiceRuntimeState>> TickerPeriodServiceFactoryOverrides { get; }
    public Dictionary<ServiceType, Func<GlobalServiceRequest, ServiceRuntimeState>>       GlobalServiceFactoryOverrides       { get; }

    public IIndicatorServicesConfig IndicatorServiceConfig { get; }
}

public class IndicatorServiceRegistryRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(IndicatorServiceRegistryRule));

    private readonly IIndicatorServicesConfig config;

    protected readonly List<IRuleDeploymentLifeTime> DeployedServices = new();

    protected readonly Dictionary<ServiceType, Func<GlobalServiceRequest, ServiceRuntimeState>> GlobalServiceFactoryLookup;

    protected readonly Dictionary<ServiceType, ServiceRuntimeState> GlobalServiceStateLookup = new();

    private readonly IndicatorServiceRegistryParams indicatorServiceParams;

    protected readonly Dictionary<TickerPeriodServiceInfo, ServiceRuntimeState> TickerPeriodServiceStateLookup = new();

    protected readonly Dictionary<ServiceType, Func<TickerPeriodServiceRequest, ServiceRuntimeState>> TickerServiceFactoryLookup;

    private ISubscription? globalServiceRequestSubscription;

    private ISubscription? tickerPeriodServiceRequestSubscription;

    public IndicatorServiceRegistryRule(IndicatorServiceRegistryParams indicatorServiceRegistryParams) : base(nameof(IndicatorServiceRegistryRule))
    {
        indicatorServiceParams = indicatorServiceRegistryParams;

        config = indicatorServiceRegistryParams.IndicatorServiceConfig;

        GlobalServiceFactoryLookup = new Dictionary<ServiceType, Func<GlobalServiceRequest, ServiceRuntimeState>>
        {
            { ServiceType.ServiceRegistry, SimpleGlobalServiceLookup }
          , { ServiceType.TimeSeriesFileRepositoryInfo, SimpleGlobalServiceLookup }
          , { ServiceType.HistoricalQuotesRetriever, SimpleGlobalServiceLookup }
          , { ServiceType.HistoricalCandlesRetriever, SimpleGlobalServiceLookup }
          , { ServiceType.CandleFilePersister, SimpleGlobalServiceLookup }
        };
        TickerServiceFactoryLookup = new Dictionary<ServiceType, Func<TickerPeriodServiceRequest, ServiceRuntimeState>>
        {
            { ServiceType.LiveCandle, LiveCandleFactory }
          , { ServiceType.LiveMovingAverage, LiveMovingAverageFactory }
          , { ServiceType.HistoricalCandlesResolver, HistoricalCandlesResolverFactory }
        };

        foreach (var kvp in indicatorServiceParams.GlobalServiceFactoryOverrides) GlobalServiceFactoryLookup[kvp.Key]       = kvp.Value;
        foreach (var kvp in indicatorServiceParams.TickerPeriodServiceFactoryOverrides) TickerServiceFactoryLookup[kvp.Key] = kvp.Value;
    }

    public override async ValueTask StartAsync()
    {
        tickerPeriodServiceRequestSubscription = await this.RegisterRequestListenerAsync<TickerPeriodServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.PricingIndicatorsServiceStartRequest, HandleTickerPeriodServiceStartRequest);
        globalServiceRequestSubscription = await this.RegisterRequestListenerAsync<GlobalServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.GlobalIndicatorsServiceStartRequest, HandleGlobalServiceStartRequest);
        if (config.MarketsConfig != null)
        {
            // launch client pricing feeds
        }
        if (config.TimeSeriesFileRepositoryConfig != null)
        {
            await LaunchGlobalService(ServiceType.TimeSeriesFileRepositoryInfo);
            await LaunchGlobalService(ServiceType.HistoricalQuotesRetriever);
            await LaunchGlobalService(ServiceType.HistoricalCandlesRetriever);
        }
        await base.StartAsync();
    }

    protected async ValueTask<ServiceRunStateResponse> LaunchGlobalService(ServiceType serviceType)
    {
        var launchResult = await this.RequestAsync<GlobalServiceRequest, ServiceRunStateResponse>
            (IndicatorServiceConstants.GlobalIndicatorsServiceStartRequest, new GlobalServiceRequest(RequestType.StartOrStatus, serviceType));
        if (launchResult.RunStatus is ServiceRunStatus.NoServiceFound or ServiceRunStatus.ServiceStartFailed)
            Logger.Warn("Start of Global Service {0} returned {1}", serviceType, launchResult.RunStatus);
        return launchResult;
    }

    public override async ValueTask StopAsync()
    {
        await tickerPeriodServiceRequestSubscription.NullSafeUnsubscribe();
        await globalServiceRequestSubscription.NullSafeUnsubscribe();
        await base.StopAsync();
    }

    protected ServiceRuntimeState SimpleGlobalServiceLookup(GlobalServiceRequest globalServiceRequest)
    {
        switch (globalServiceRequest.ServiceType)
        {
            case ServiceType.TimeSeriesFileRepositoryInfo:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new TimeSeriesRepositoryInfoRule(config.TimeSeriesFileRepositoryConfig!), ServiceRunStatus.NotStarted));
            case ServiceType.HistoricalQuotesRetriever:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new HistoricalQuotesRetrievalRule(config.TimeSeriesFileRepositoryConfig!), ServiceRunStatus.NotStarted));
            case ServiceType.HistoricalCandlesRetriever:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new HistoricalCandlesRetrievalRule(config.TimeSeriesFileRepositoryConfig!), ServiceRunStatus.NotStarted));
            case ServiceType.CandleFilePersister:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new CyclingInstrumentChainingEntryPersisterRule<Candle>
                             (new CyclingInstrumentChainingEntryPersisterParams
                                 (new TimeSeriesRepositoryParams(config.TimeSeriesFileRepositoryConfig!)
                                , CandleConstants.PersistAppendCandlePublish()))
                       , ServiceRunStatus.NotStarted));
        }
        return new ServiceRuntimeState();
    }

    protected ServiceRuntimeState LiveCandleFactory(TickerPeriodServiceRequest tickerPeriodServiceRequest)
    {
        var tickerServiceInfo = tickerPeriodServiceRequest.TickerPeriodServiceInfo;
        if (tickerServiceInfo.PricingInstrumentId.CoveringPeriod < Tick || tickerServiceInfo.PricingInstrumentId.CoveringPeriod > OneYear)
            return new ServiceRuntimeState();
        switch (tickerServiceInfo.TickerQuoteDetailLevel)
        {
            case TickerQuoteDetailLevel.Level1Quote when tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new LiveCandlePublisherRule<PQPublishableLevel1Quote>
                             (new LivePublishCandleParams(tickerServiceInfo.PricingInstrumentId))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level1Quote when !tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new LiveCandlePublisherRule<PublishableLevel1PriceQuote>
                             (new LivePublishCandleParams(tickerServiceInfo.PricingInstrumentId))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level2Quote when tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new LiveCandlePublisherRule<PQPublishableLevel2Quote>
                             (new LivePublishCandleParams(tickerServiceInfo.PricingInstrumentId))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level2Quote when !tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new LiveCandlePublisherRule<PublishableLevel2PriceQuote>
                             (new LivePublishCandleParams(tickerServiceInfo.PricingInstrumentId))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level3Quote when tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new LiveCandlePublisherRule<PQPublishableLevel3Quote>
                             (new LivePublishCandleParams(tickerServiceInfo.PricingInstrumentId))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level3Quote when !tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new LiveCandlePublisherRule<PublishableLevel3PriceQuote>
                             (new LivePublishCandleParams(tickerServiceInfo.PricingInstrumentId))
                       , ServiceRunStatus.NotStarted));
        }
        return new ServiceRuntimeState();
    }

    protected ServiceRuntimeState HistoricalCandlesResolverFactory(TickerPeriodServiceRequest tickerPeriodServiceRequest)
    {
        var tickerServiceInfo = tickerPeriodServiceRequest.TickerPeriodServiceInfo;
        if (tickerServiceInfo.PricingInstrumentId.CoveringPeriod == Tick) return new ServiceRuntimeState();
        switch (tickerServiceInfo.TickerQuoteDetailLevel)
        {
            case TickerQuoteDetailLevel.Level1Quote when tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new HistoricalCandlesResolverRule<PQPublishableLevel1Quote>
                             (new HistoricalCandleParams
                                 (tickerServiceInfo.PricingInstrumentId
                                , new TimeLength(config.DefaultCacheCandlesTimeSpan)))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level1Quote when !tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new HistoricalCandlesResolverRule<PublishableLevel1PriceQuote>
                             (new HistoricalCandleParams
                                 (tickerServiceInfo.PricingInstrumentId
                                , new TimeLength(config.DefaultCacheCandlesTimeSpan)))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level2Quote when tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new HistoricalCandlesResolverRule<PQPublishableLevel2Quote>
                             (new HistoricalCandleParams
                                 (tickerServiceInfo.PricingInstrumentId
                                , new TimeLength(config.DefaultCacheCandlesTimeSpan)))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level2Quote when !tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new HistoricalCandlesResolverRule<PublishableLevel2PriceQuote>
                             (new HistoricalCandleParams
                                 (tickerServiceInfo.PricingInstrumentId
                                , new TimeLength(config.DefaultCacheCandlesTimeSpan)))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level3Quote when tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new HistoricalCandlesResolverRule<PQPublishableLevel3Quote>
                             (new HistoricalCandleParams
                                 (tickerServiceInfo.PricingInstrumentId
                                , new TimeLength(config.DefaultCacheCandlesTimeSpan)))
                       , ServiceRunStatus.NotStarted));
            case TickerQuoteDetailLevel.Level3Quote when !tickerServiceInfo.UsePqQuote:
                return new ServiceRuntimeState
                    (new ServiceRunStateResponse
                        (new HistoricalCandlesResolverRule<PublishableLevel3PriceQuote>
                             (new HistoricalCandleParams
                                 (tickerServiceInfo.PricingInstrumentId
                                , new TimeLength(config.DefaultCacheCandlesTimeSpan)))
                       , ServiceRunStatus.NotStarted));
        }
        return new ServiceRuntimeState();
    }

    protected ServiceRuntimeState LiveMovingAverageFactory(TickerPeriodServiceRequest tickerPeriodServiceRequest)
    {
        var tickerServiceInfo = tickerPeriodServiceRequest.TickerPeriodServiceInfo;
        if (tickerServiceInfo.PricingInstrumentId.CoveringPeriod.Period is >= Tick and <= FifteenMinutes)
        {
            // shared ticks moving Average
            var anyExisting =
                TickerPeriodServiceStateLookup
                    .FirstOrDefault
                        (kvp => Equals(kvp.Key.PricingInstrumentId, tickerServiceInfo.PricingInstrumentId) &&
                                kvp.Key.PricingInstrumentId.CoveringPeriod.Period is >= Tick and <= FifteenMinutes);
            if (!Equals(anyExisting, default(KeyValuePair<TickerPeriodServiceInfo, ServiceRuntimeState>)))
            {
                TickerPeriodServiceStateLookup[anyExisting.Key] = anyExisting.Value;
                return anyExisting.Value;
            }

            return new ServiceRuntimeState
                (new ServiceRunStateResponse
                    (new LiveShortPeriodMovingAveragePublisherRule
                         (new LiveShortPeriodMovingAveragePublishParams
                             (new PricingIndicatorId(IndicatorConstants.MovingAverageTimeWeightedBidAskId, tickerServiceInfo.PricingInstrumentId)))
                   , ServiceRunStatus.NotStarted));
        }
        if (tickerServiceInfo.PricingInstrumentId.CoveringPeriod.Period is > FifteenMinutes and <= OneYear)
        {
            // TODO implement period summary moving average calculator
        }
        return new ServiceRuntimeState();
    }

    protected virtual async ValueTask<ServiceRunStateResponse> HandleGlobalServiceStartRequest
        (IBusRespondingMessage<GlobalServiceRequest, ServiceRunStateResponse> serviceStartRequestMsg)
    {
        var serviceReq = serviceStartRequestMsg.Payload.Body();
        if (GlobalServiceStateLookup.TryGetValue(serviceReq.ServiceType, out var existing))
            if (existing.LastStartResult.IsRunning())
                return GlobalServiceStateLookup[serviceReq.ServiceType].LastStartResult
                    = new ServiceRunStateResponse(existing.Rule!, ServiceRunStatus.ServiceAlreadyRunning, existing.LastStartResult.LastUpdateTime);
        if (GlobalServiceFactoryLookup.TryGetValue(serviceReq.ServiceType, out var factory))
        {
            var serviceInfo = factory.Invoke(serviceReq);
            GlobalServiceStateLookup[serviceReq.ServiceType] = serviceInfo;
            if (serviceInfo.RunStatus is ServiceRunStatus.NotStarted or ServiceRunStatus.ServiceStopped &&
                existing is null or { RunStatus: ServiceRunStatus.ServiceStopped })
                try
                {
                    var ruleDeployment = await this.DeployChildRuleAsync(serviceInfo.Rule!);
                    DeployedServices.Add(ruleDeployment);
                    if (Equals(existing, default))
                        return GlobalServiceStateLookup[serviceReq.ServiceType].LastStartResult
                            = new ServiceRunStateResponse(serviceInfo.Rule!, ServiceRunStatus.ServiceStarted, DateTime.UtcNow);
                    if (existing.RunStatus == ServiceRunStatus.ServiceStopped)
                        return GlobalServiceStateLookup[serviceReq.ServiceType].LastStartResult
                            = new ServiceRunStateResponse(serviceInfo.Rule!, ServiceRunStatus.ServiceRestarted, DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    Logger.Warn("When attempt to start rule {0}. Got {1}", serviceInfo.Rule!.FriendlyName, ex);
                    return GlobalServiceStateLookup[serviceReq.ServiceType].LastStartResult
                        = new ServiceRunStateResponse(serviceInfo.Rule!, ServiceRunStatus.ServiceStartFailed, DateTime.UtcNow);
                }
            return serviceInfo.LastStartResult;
        }
        return existing?.LastStartResult ?? new ServiceRunStateResponse();
    }

    protected virtual async ValueTask<ServiceRunStateResponse> HandleTickerPeriodServiceStartRequest
        (IBusRespondingMessage<TickerPeriodServiceRequest, ServiceRunStateResponse> serviceStartRequestMsg)
    {
        var serviceReq = serviceStartRequestMsg.Payload.Body();
        if (TickerPeriodServiceStateLookup.TryGetValue(serviceReq.TickerPeriodServiceInfo, out var existing))
            if (existing.LastStartResult.IsRunning())
                return TickerPeriodServiceStateLookup[serviceReq.TickerPeriodServiceInfo].LastStartResult
                    = new ServiceRunStateResponse(existing.Rule!, ServiceRunStatus.ServiceAlreadyRunning, existing.LastStartResult.LastUpdateTime);
        if (TickerServiceFactoryLookup.TryGetValue(serviceReq.TickerPeriodServiceInfo.ServiceType, out var factory))
        {
            var serviceInfo = factory.Invoke(serviceReq);
            TickerPeriodServiceStateLookup[serviceReq.TickerPeriodServiceInfo] = serviceInfo;
            if (serviceInfo.RunStatus is ServiceRunStatus.NotStarted or ServiceRunStatus.ServiceStopped &&
                existing is null or { RunStatus: ServiceRunStatus.ServiceStopped })
                try
                {
                    var ruleDeployment = await this.DeployChildRuleAsync(serviceInfo.Rule!);
                    DeployedServices.Add(ruleDeployment);
                    if (Equals(existing, default))
                        return TickerPeriodServiceStateLookup[serviceReq.TickerPeriodServiceInfo].LastStartResult
                            = new ServiceRunStateResponse(serviceInfo.Rule!, ServiceRunStatus.ServiceStarted, DateTime.UtcNow);
                    if (existing.RunStatus == ServiceRunStatus.ServiceStopped)
                        return TickerPeriodServiceStateLookup[serviceReq.TickerPeriodServiceInfo].LastStartResult
                            = new ServiceRunStateResponse(serviceInfo.Rule!, ServiceRunStatus.ServiceRestarted, DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    Logger.Warn("When attempt to start rule {0}. Got {1}", serviceInfo.Rule!.FriendlyName, ex);
                    return TickerPeriodServiceStateLookup[serviceReq.TickerPeriodServiceInfo].LastStartResult
                        = new ServiceRunStateResponse(serviceInfo.Rule!, ServiceRunStatus.ServiceStartFailed, DateTime.UtcNow);
                }
            return serviceInfo.LastStartResult;
        }
        return existing?.LastStartResult ?? new ServiceRunStateResponse();
    }
}
