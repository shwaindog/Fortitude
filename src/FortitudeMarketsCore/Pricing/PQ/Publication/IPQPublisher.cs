using System;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public interface IPQPublisher : IQuotePublisher<ILevel0Quote>
    {
        void RegisterTickersWithServer(ISourceTickerPublicationConfigRepository tickersIdRef);
    }
}