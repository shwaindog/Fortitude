#region

using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

public class PQSnapshotClientRepository : PQConversationRepositoryBase<PQSnapshotClient>
{
    private readonly ISocketDispatcherResolver socketDispatcherResolver;

    public PQSnapshotClientRepository(ISocketDispatcherResolver dispatcherResolver) => socketDispatcherResolver = dispatcherResolver;

    protected override PQSnapshotClient
        CreateNewSocketSubscriptionType(INetworkTopicConnectionConfig networkConnectionConfig) =>
        PQSnapshotClient.BuildTcpRequester(networkConnectionConfig, socketDispatcherResolver);
}
