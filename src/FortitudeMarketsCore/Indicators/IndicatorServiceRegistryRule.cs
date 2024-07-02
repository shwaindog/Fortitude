// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Indicators.Config;
using FortitudeMarketsCore.Indicators.Persistence;
using FortitudeMarketsCore.Indicators.Pricing;
using FortitudeMarketsCore.Indicators.Pricing.MovingAverage;
using FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;
using FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;

#endregion

namespace FortitudeMarketsCore.Indicators;

public enum ServiceType
{
    LiveQuote
  , HistoricalQuoteResolver
  , LivePricePeriodSummary
  , HistoricalPricePeriodSummaryResolver
  , PricePeriodSummaryFilePersister
  , LiveMovingAverage
  , HistoricalMovingAverageResolver
}

public enum ServiceRunStatus
{
    NoServiceFound
  , Disabled
  , NotStarted
  , ServiceStarted
  , ServiceAlreadyRunning
  , ServiceRestarted
  , ServiceStartFailed
  , ServiceStopped
}

public struct SourceTickerService
{
    public SourceTickerService
    (ServiceType serviceType, ISourceTickerIdentifier tickerId, TimeSeriesPeriod period = Tick
      , QuoteLevel quoteLevel = QuoteLevel.Level1, bool usePqQuote = false)
    {
        ServiceType = serviceType;
        TickerId    = tickerId;
        Period      = period;
        QuoteLevel  = quoteLevel;
        UsePqQuote  = usePqQuote;
    }

    public ServiceType             ServiceType { get; }
    public ISourceTickerIdentifier TickerId    { get; }
    public TimeSeriesPeriod        Period      { get; }
    public QuoteLevel              QuoteLevel  { get; set; }
    public bool                    UsePqQuote  { get; set; }
}

public struct ServiceStartInfo
{
    public ServiceStartInfo(ServiceRunStatus runStatus = ServiceRunStatus.NoServiceFound) => RunStatus = runStatus;

    public ServiceStartInfo(IRule rule, ServiceRunStatus runStatus)
    {
        Rule      = rule;
        RunStatus = runStatus;
    }

    public ServiceStartInfo(IRule rule, ServiceRunStatus runStatus, DateTime? startTime)
    {
        Rule      = rule;
        RunStatus = runStatus;
        StartTime = startTime;
    }

    public IRule?           Rule      { get; }
    public ServiceRunStatus RunStatus { get; }
    public DateTime?        StartTime { get; }
}

public class IndicatorServiceRegistryRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(IndicatorServiceRegistryRule));

    private readonly IIndicatorServicesConfig config;

    private readonly Dictionary<SourceTickerService, ServiceStartInfo> serviceStartTimes = new();

    private ISubscription? serviceStartRequestListenSubscription;

    private Dictionary<ServiceType, Func<SourceTickerService, ServiceStartInfo>> tickerServiceFactoryLookup = new();

    public IndicatorServiceRegistryRule(IIndicatorServicesConfig config) : base(nameof(IndicatorServiceRegistryRule)) => this.config = config;

    public override ValueTask StartAsync()
    {
        tickerServiceFactoryLookup = new Dictionary<ServiceType, Func<SourceTickerService, ServiceStartInfo>>()
        {
            { ServiceType.LivePricePeriodSummary, LivePriceSummaryFactory }
          , { ServiceType.LiveMovingAverage, LiveMovingAverageFactory }
          , { ServiceType.HistoricalPricePeriodSummaryResolver, HistoricalPricePeriodSummaryResolverFactory }
          , { ServiceType.PricePeriodSummaryFilePersister, PricePeriodSummaryPersisterFactory }
        };
        serviceStartRequestListenSubscription = this.RegisterRequestListener<SourceTickerService, ServiceStartInfo>
            (IndicatorServiceConstants.IndicatorsServiceStartRequest, HandleServiceStartRequest);
        if (config.MarketsConfig != null)
        {
            // launch client pricing feeds
        }
        if (config.TimeSeriesFileRepositoryConfig != null)
        {
            var repoInfoRule = new TimeSeriesRepositoryInfoRule(config.TimeSeriesFileRepositoryConfig);
            Context.MessageBus.DeployRule(this, repoInfoRule, new DeploymentOptions(messageGroupType: MessageQueueType.Custom));
            var historicalQuoteRetriever = new HistoricalQuotesRetrievalRule(config.TimeSeriesFileRepositoryConfig);
            Context.MessageBus.DeployRule(this, historicalQuoteRetriever, new DeploymentOptions(messageGroupType: MessageQueueType.Custom));
        }
        return base.StartAsync();
    }

    private ServiceStartInfo LivePriceSummaryFactory(SourceTickerService tickerService)
    {
        if (tickerService.Period is Tick or > OneYear) return new ServiceStartInfo();
        switch (tickerService.QuoteLevel)
        {
            case QuoteLevel.Level1 when tickerService.UsePqQuote:
                if (tickerService.UsePqQuote)
                    return new ServiceStartInfo(new LivePricePeriodSummaryPublisherRule<PQLevel1Quote>
                                                    (new TimeSeriesPricePeriodParams(tickerService.Period, tickerService.TickerId))
                                              , ServiceRunStatus.NotStarted);
                return new ServiceStartInfo
                    (new LivePricePeriodSummaryPublisherRule<Level1PriceQuote>
                        (new TimeSeriesPricePeriodParams(tickerService.Period, tickerService.TickerId)), ServiceRunStatus.NotStarted);
            case QuoteLevel.Level2 when tickerService.UsePqQuote:
                if (tickerService.UsePqQuote)
                    return new ServiceStartInfo(new LivePricePeriodSummaryPublisherRule<PQLevel2Quote>
                                                    (new TimeSeriesPricePeriodParams(tickerService.Period, tickerService.TickerId))
                                              , ServiceRunStatus.NotStarted);
                return new ServiceStartInfo(new LivePricePeriodSummaryPublisherRule<Level2PriceQuote>
                                                (new TimeSeriesPricePeriodParams(tickerService.Period, tickerService.TickerId))
                                          , ServiceRunStatus.NotStarted);
            case QuoteLevel.Level3 when tickerService.UsePqQuote:
                if (tickerService.UsePqQuote)
                    return new ServiceStartInfo(new LivePricePeriodSummaryPublisherRule<PQLevel3Quote>
                                                    (new TimeSeriesPricePeriodParams(tickerService.Period, tickerService.TickerId))
                                              , ServiceRunStatus.NotStarted);
                return new ServiceStartInfo(new LivePricePeriodSummaryPublisherRule<Level3PriceQuote>
                                                (new TimeSeriesPricePeriodParams(tickerService.Period, tickerService.TickerId))
                                          , ServiceRunStatus.NotStarted);
        }
        return new ServiceStartInfo();
    }

    private ServiceStartInfo HistoricalPricePeriodSummaryResolverFactory(SourceTickerService tickerService)
    {
        if (tickerService.Period is Tick) return new ServiceStartInfo();
        switch (tickerService.QuoteLevel)
        {
            case QuoteLevel.Level1 when tickerService.UsePqQuote:
                if (tickerService.UsePqQuote)
                    return new ServiceStartInfo
                        (new HistoricalPeriodSummariesResolverRule<PQLevel1Quote>
                             (new HistoricalPeriodParams(tickerService.TickerId, tickerService.Period
                                                       , new TimeLength(config.DefaultCacheSummaryPeriodsPricesTimeSpan)))
                       , ServiceRunStatus.NotStarted);
                return new ServiceStartInfo
                    (new HistoricalPeriodSummariesResolverRule<Level1PriceQuote>
                         (new HistoricalPeriodParams(tickerService.TickerId, tickerService.Period
                                                   , new TimeLength(config.DefaultCacheSummaryPeriodsPricesTimeSpan)))
                   , ServiceRunStatus.NotStarted);
            case QuoteLevel.Level2 when tickerService.UsePqQuote:
                if (tickerService.UsePqQuote)
                    return new ServiceStartInfo
                        (new HistoricalPeriodSummariesResolverRule<PQLevel2Quote>
                             (new HistoricalPeriodParams(tickerService.TickerId, tickerService.Period
                                                       , new TimeLength(config.DefaultCacheSummaryPeriodsPricesTimeSpan)))
                       , ServiceRunStatus.NotStarted);
                return new ServiceStartInfo
                    (new HistoricalPeriodSummariesResolverRule<Level2PriceQuote>
                         (new HistoricalPeriodParams(tickerService.TickerId, tickerService.Period
                                                   , new TimeLength(config.DefaultCacheSummaryPeriodsPricesTimeSpan)))
                   , ServiceRunStatus.NotStarted);
            case QuoteLevel.Level3 when tickerService.UsePqQuote:
                if (tickerService.UsePqQuote)
                    return new ServiceStartInfo
                        (new HistoricalPeriodSummariesResolverRule<PQLevel3Quote>
                             (new HistoricalPeriodParams(tickerService.TickerId, tickerService.Period
                                                       , new TimeLength(config.DefaultCacheSummaryPeriodsPricesTimeSpan)))
                       , ServiceRunStatus.NotStarted);
                return new ServiceStartInfo
                    (new HistoricalPeriodSummariesResolverRule<Level3PriceQuote>
                         (new HistoricalPeriodParams(tickerService.TickerId, tickerService.Period
                                                   , new TimeLength(config.DefaultCacheSummaryPeriodsPricesTimeSpan)))
                   , ServiceRunStatus.NotStarted);
        }
        return new ServiceStartInfo();
    }

    private ServiceStartInfo PricePeriodSummaryPersisterFactory(SourceTickerService tickerService)
    {
        if (tickerService.Period is < PricePeriodSummaryConstants.PersistPeriodsFrom or > PricePeriodSummaryConstants.PersistPeriodsTo)
            return new ServiceStartInfo();
        if (!config.PersistenceConfig.PersistPriceSummaries) return new ServiceStartInfo(ServiceRunStatus.Disabled);
        if (tickerService.Period >= FifteenSeconds)
            return new ServiceStartInfo
                (new PriceSummarizingFilePersisterRule<IPricePeriodSummary>
                     (new SummarizingPricePersisterParams
                         (new TimeSeriesRepositoryParams(config.TimeSeriesFileRepositoryConfig!), tickerService.TickerId
                        , InstrumentType.PriceSummaryPeriod
                        , tickerService.Period, tickerService.TickerId.PersistAppendPeriodSummaryPublish(tickerService.Period)))
               , ServiceRunStatus.NotStarted);
        return new ServiceStartInfo();
    }

    private ServiceStartInfo LiveMovingAverageFactory(SourceTickerService tickerService)
    {
        if (tickerService.Period is >= Tick and <= FifteenMinutes)
        {
            // shared ticks moving Average
            var anyExisting =
                serviceStartTimes
                    .Keys.FirstOrDefault
                        (sts => Equals(sts.TickerId, tickerService.TickerId) && sts.Period is >= Tick and <= FifteenMinutes);
            if (!Equals(anyExisting, default)) return serviceStartTimes[anyExisting];

            return new ServiceStartInfo
                (new LiveSharedTicksMovingAveragePublisherRule
                    (new MovingAveragePublisherParams
                        (tickerService.TickerId.SourceId, tickerService.TickerId
                       , new PricePublishInterval(OneSecond), new MovingAverageParams(tickerService.Period!))), ServiceRunStatus.NotStarted);
        }
        if (tickerService.Period is > FifteenMinutes and <= OneYear)
        {
            // TODO implement period summary moving average calculator
        }
        return new ServiceStartInfo();
    }

    private async ValueTask<ServiceStartInfo> HandleServiceStartRequest
        (IBusRespondingMessage<SourceTickerService, ServiceStartInfo> serviceStartRequestMsg)
    {
        var serviceReq = serviceStartRequestMsg.Payload.Body();
        if (serviceStartTimes.TryGetValue(serviceReq, out var existing))
            if (existing.RunStatus == ServiceRunStatus.ServiceStarted)
                return new ServiceStartInfo(existing.Rule!, ServiceRunStatus.ServiceAlreadyRunning, existing.StartTime);
        if (tickerServiceFactoryLookup.TryGetValue(serviceReq.ServiceType, out var factory))
        {
            var serviceInfo = factory.Invoke(serviceReq);
            if (serviceInfo.RunStatus == ServiceRunStatus.NotStarted || Equals(existing, default))
                try
                {
                    await this.DeployRuleAsync(serviceInfo.Rule!);
                    if (Equals(existing, default)) return new ServiceStartInfo(serviceInfo.Rule!, ServiceRunStatus.ServiceStarted, DateTime.UtcNow);
                    if (existing.RunStatus == ServiceRunStatus.ServiceStopped)
                        return new ServiceStartInfo(serviceInfo.Rule!, ServiceRunStatus.ServiceRestarted, DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    Logger.Warn("When attempt to start rule {0}. Got {1}", serviceInfo.Rule!.FriendlyName, ex);
                    return new ServiceStartInfo(serviceInfo.Rule!, ServiceRunStatus.ServiceStartFailed, DateTime.UtcNow);
                }
            return serviceInfo;
        }
        return new ServiceStartInfo();
    }
}
