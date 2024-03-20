#region

using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public class PQSnapshotClientRepository : PQConversationRepositoryBase<PQSnapshotClient>
{
    private readonly ISocketDispatcherResolver socketDispatcherResolver;

    public PQSnapshotClientRepository(ISocketDispatcherResolver dispatcherResolver) =>
        socketDispatcherResolver = dispatcherResolver;

    protected override PQSnapshotClient
        CreateNewSocketSubscriptionType(ISocketConnectionConfig socketConnectionConfig) =>
        PQSnapshotClient.BuildTcpRequester(socketConnectionConfig, socketDispatcherResolver);
}
