// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface ITickerPricingSubscriptionConfig
{
    ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; }
    IPricingServerConfig   PricingServerConfig   { get; }
}

public class TickerPricingSubscriptionConfig : ITickerPricingSubscriptionConfig
{
    public TickerPricingSubscriptionConfig(ISourceTickerQuoteInfo sourceTickerQuoteInfo, IPricingServerConfig pricingServerConfig)
    {
        SourceTickerQuoteInfo = sourceTickerQuoteInfo;
        PricingServerConfig   = pricingServerConfig;
    }

    public ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; }
    public IPricingServerConfig   PricingServerConfig   { get; }
}
