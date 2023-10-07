#region

using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface ISourceTickerPublicationConfig : ISourceTickerQuoteInfo
{
    ISnapshotUpdatePricingServerConfig? MarketPriceQuoteServer { get; }
    new ISourceTickerPublicationConfig Clone();
}

public interface IMutableSourceTickerPublicationConfig : ISourceTickerPublicationConfig, IMutableSourceTickerQuoteInfo
{
    new ISnapshotUpdatePricingServerConfig? MarketPriceQuoteServer { get; set; }
    new IMutableSourceTickerPublicationConfig Clone();
}
