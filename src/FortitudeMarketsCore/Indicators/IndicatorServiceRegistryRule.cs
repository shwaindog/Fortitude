// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Rules;
using FortitudeMarketsCore.Indicators.Config;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;

#endregion

namespace FortitudeMarketsCore.Indicators;

public class IndicatorServiceRegistryRule : Rule
{
    private readonly IIndicatorServicesConfig config;

    public IndicatorServiceRegistryRule(IIndicatorServicesConfig config) : base(nameof(IndicatorServiceRegistryRule)) => this.config = config;


    public override ValueTask StartAsync()
    {
        if (config.MarketsConfig != null)
        {
            // launch client pricing feeds
        }
        if (config.TimeSeriesFileRepositoryConfig != null)
        {
            var historicalQuoteRetriever = new HistoricalQuotesRetrievalRule(config.TimeSeriesFileRepositoryConfig);
            Context.MessageBus.DeployRule(this, historicalQuoteRetriever, new DeploymentOptions(messageGroupType: MessageQueueType.Custom));
        }
        return base.StartAsync();
    }
}
