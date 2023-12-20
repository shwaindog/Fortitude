#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQPublisher : IQuotePublisher<ILevel0Quote>
{
    void RegisterTickersWithServer(ISourceTickerPublicationConfigRepository tickersIdRef);
}
