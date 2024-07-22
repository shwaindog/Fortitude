// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface ITickerPricingSubscriptionConfig
{
    ISourceTickerInfo    SourceTickerInfo    { get; }
    IPricingServerConfig PricingServerConfig { get; }
}

public class TickerPricingSubscriptionConfig : ITickerPricingSubscriptionConfig
{
    public TickerPricingSubscriptionConfig(ISourceTickerInfo sourceTickerInfo, IPricingServerConfig pricingServerConfig)
    {
        SourceTickerInfo    = sourceTickerInfo;
        PricingServerConfig = pricingServerConfig;
    }

    public ISourceTickerInfo    SourceTickerInfo    { get; }
    public IPricingServerConfig PricingServerConfig { get; }
}
