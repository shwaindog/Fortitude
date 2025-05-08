// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Indicators;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeMarkets.Indicators;

[NoMatchingProductionClass]
public class IndicatorServiceRegistryStubRule : IndicatorServiceRegistryRule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(IndicatorServiceRegistryStubRule));

    public IndicatorServiceRegistryStubRule(IndicatorServiceRegistryParams overrideParams)
        : base(overrideParams) { }

    public List<TickerPeriodServiceRequest> TickerPeriodReceivedRequests { get; } = new();

    public List<GlobalServiceRequest> GlobalReceivedRequests { get; } = new();

    public List<ServiceRunStateResponse> Responses { get; } = new();

    public Dictionary<TickerPeriodServiceInfo, ServiceRuntimeState> TickerPeriodServiceRegistry => TickerPeriodServiceStateLookup;

    public Dictionary<ServiceType, ServiceRuntimeState> GlobalServiceRegistry => GlobalServiceStateLookup;

    public Func<TickerPeriodServiceRequest, ServiceRunStateResponse, bool> ShouldLaunchRule { get; set; } = (_, _) => true;

    public override async ValueTask StopAsync()
    {
        foreach (var asyncDisposable in DeployedServices) await asyncDisposable.DisposeAsync();
    }

    public async ValueTask RegisterAndDeployGlobalService(ServiceType service, IRule rule)
    {
        try
        {
            var deployLifeTime = await this.DeployChildRuleAsync(rule);
            DeployedServices.Add(deployLifeTime);
            var serviceRunStateResponse = new ServiceRunStateResponse(rule, ServiceRunStatus.ServiceStarted);
            GlobalServiceRegistry.Add(service, new ServiceRuntimeState(serviceRunStateResponse));
        }
        catch (Exception ex)
        {
            Logger.Warn("Deployment of rule {0} failed.  Got {1}", rule.FriendlyName, ex);
            var serviceRunStateResponse = new ServiceRunStateResponse(rule, ServiceRunStatus.ServiceStartFailed);
            GlobalServiceRegistry.Add(service, new ServiceRuntimeState(serviceRunStateResponse));
        }
    }

    public void RegisterGlobalServiceStatus(ServiceType service, ServiceRunStatus setStatus)
    {
        var serviceRunStateResponse = new ServiceRunStateResponse(setStatus);
        GlobalServiceRegistry.Add(service, new ServiceRuntimeState(serviceRunStateResponse));
    }

    public async ValueTask RegisterAndDeployTickerPeriodService
    (SourceTickerIdentifier sourceTickerIdentifier, DiscreetTimePeriod period, ServiceType service, IRule rule
      , TickerDetailLevel tickerDetailLevel = TickerDetailLevel.Level1Quote, bool usePQQuote = false)
    {
        try
        {
            var deployLifeTime = await this.DeployChildRuleAsync(rule);
            DeployedServices.Add(deployLifeTime);
            var tickerPeriodServiceInfo = new TickerPeriodServiceInfo(service, sourceTickerIdentifier, period, tickerDetailLevel, usePQQuote);
            var serviceRunStateResponse = new ServiceRunStateResponse(rule, ServiceRunStatus.ServiceStarted);
            TickerPeriodServiceStateLookup.Add(tickerPeriodServiceInfo, new ServiceRuntimeState(serviceRunStateResponse));
        }
        catch (Exception ex)
        {
            Logger.Warn("Deployment of rule {0} failed.  Got {1}", rule.FriendlyName, ex);
            var serviceRunStateResponse = new ServiceRunStateResponse(rule, ServiceRunStatus.ServiceStartFailed);
            GlobalServiceRegistry.Add(service, new ServiceRuntimeState(serviceRunStateResponse));
        }
    }

    public void RegisterTickerPeriodServiceStatus
    (SourceTickerIdentifier sourceTickerIdentifier, DiscreetTimePeriod period, ServiceType service, ServiceRunStatus setStatus
      , TickerDetailLevel tickerDetailLevel = TickerDetailLevel.Level1Quote, bool usePQQuote = false)
    {
        var tickerPeriodServiceInfo = new TickerPeriodServiceInfo(service, sourceTickerIdentifier, period, tickerDetailLevel, usePQQuote);
        var serviceRunStateResponse = new ServiceRunStateResponse(setStatus);
        TickerPeriodServiceStateLookup.Add(tickerPeriodServiceInfo, new ServiceRuntimeState(serviceRunStateResponse));
    }

    protected override async ValueTask<ServiceRunStateResponse> HandleGlobalServiceStartRequest
        (IBusRespondingMessage<GlobalServiceRequest, ServiceRunStateResponse> serviceStartRequestMsg)
    {
        var serviceReq = serviceStartRequestMsg.Payload.Body();
        GlobalReceivedRequests.Add(serviceReq);
        var result = await base.HandleGlobalServiceStartRequest(serviceStartRequestMsg);
        Responses.Add(result);
        return result;
    }

    protected override async ValueTask<ServiceRunStateResponse> HandleTickerPeriodServiceStartRequest
        (IBusRespondingMessage<TickerPeriodServiceRequest, ServiceRunStateResponse> serviceStartRequestMsg)
    {
        var serviceReq = serviceStartRequestMsg.Payload.Body();
        TickerPeriodReceivedRequests.Add(serviceReq);
        var result = await base.HandleTickerPeriodServiceStartRequest(serviceStartRequestMsg);
        Responses.Add(result);
        return result;
    }
}
