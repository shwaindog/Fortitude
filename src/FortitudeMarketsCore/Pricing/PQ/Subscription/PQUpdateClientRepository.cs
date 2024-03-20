#region

using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public class PQUpdateClientRepository : PQConversationRepositoryBase<PQUpdateClient>
{
    private readonly ISocketDispatcherResolver socketDispatcherResolver;

    public PQUpdateClientRepository(ISocketDispatcherResolver dispatcherResolver) =>
        socketDispatcherResolver = dispatcherResolver;

    protected override PQUpdateClient CreateNewSocketSubscriptionType(ISocketConnectionConfig socketConnectionConfig)
    {
        var pqUpdateClient = PQUpdateClient.BuildUdpSubscriber(socketConnectionConfig, socketDispatcherResolver);
        pqUpdateClient.Start();
        return pqUpdateClient;
    }
}
