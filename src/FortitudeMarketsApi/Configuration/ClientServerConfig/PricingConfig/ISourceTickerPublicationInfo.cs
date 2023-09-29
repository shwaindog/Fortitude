using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig
{
    public interface ISourceTickerPublicationConfig : ISourceTickerQuoteInfo
    {
        ISnapshotUpdatePricingServerConfig MarketPriceQuoteServer { get; }
        new ISourceTickerPublicationConfig Clone();
    }
}
