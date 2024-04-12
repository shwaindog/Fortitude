#region

using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

public class PQUpdateClientRepository : PQConversationRepositoryBase<PQUpdateClient>
{
    private readonly ISocketDispatcherResolver socketDispatcherResolver;

    public PQUpdateClientRepository(ISocketDispatcherResolver dispatcherResolver) => socketDispatcherResolver = dispatcherResolver;

    protected override PQUpdateClient CreateNewSocketSubscriptionType(
        INetworkTopicConnectionConfig networkConnectionConfig)
    {
        var pqUpdateClient = PQUpdateClient.BuildUdpSubscriber(networkConnectionConfig, socketDispatcherResolver);
        pqUpdateClient.Start();
        return pqUpdateClient;
    }
}
