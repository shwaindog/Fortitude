// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeMarketsCore.Indicators;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Indicators;

public class IndicatorServiceRegistryRuleTests
{
    public class IndicatorServiceRegistryRuleTestReplacementRule : IndicatorServiceRegistryRule
    {
        public IndicatorServiceRegistryRuleTestReplacementRule(IndicatorServiceRegistryParams overrideParams)
            : base(overrideParams) { }

        public List<TickerPeriodServiceRequest> TickerPeriodReceivedRequests { get; } = new();
        public List<GlobalServiceRequest>       GlobalReceivedRequests       { get; } = new();

        public List<ServiceRunStateResponse> Responses { get; } = new();

        public Dictionary<TickerPeriodServiceInfo, ServiceRuntimeState> TickerPeriodServiceRegistry => TickerPeriodServiceStateLookup;
        public Dictionary<ServiceType, ServiceRuntimeState>             GlobalServiceRegistry       => GlobalServiceStateLookup;

        public Func<TickerPeriodServiceRequest, ServiceRunStateResponse, bool> ShouldLaunchRule { get; set; } = (_, _) => true;

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
}
