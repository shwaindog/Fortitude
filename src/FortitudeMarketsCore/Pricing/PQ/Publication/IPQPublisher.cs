#region

using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQPublisher : IQuotePublisher<ILevel0Quote>
{
    void RegisterTickersWithServer(IMarketConnectionConfig marketConnectionConfig);
}
