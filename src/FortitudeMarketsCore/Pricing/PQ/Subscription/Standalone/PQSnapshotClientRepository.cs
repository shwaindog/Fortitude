#region

using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

public class PQSnapshotClientRepository : PQConversationRepositoryBase<PQStandaloneSnapshotClient>
{
    private readonly ISocketDispatcherResolver socketDispatcherResolver;

    public PQSnapshotClientRepository(ISocketDispatcherResolver dispatcherResolver) => socketDispatcherResolver = dispatcherResolver;

    protected override PQStandaloneSnapshotClient
        CreateNewSocketSubscriptionType(INetworkTopicConnectionConfig networkConnectionConfig) =>
        PQStandaloneSnapshotClient.BuildTcpRequester(networkConnectionConfig, socketDispatcherResolver);
}
