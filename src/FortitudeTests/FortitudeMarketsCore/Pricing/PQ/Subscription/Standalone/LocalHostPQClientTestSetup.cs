#region

using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

[NoMatchingProductionClass]
public class LocalHostPQClientTestSetup : LocalHostPQTestSetupCommon
{
    public IMarketConnectionConfigRepository ClientConnectionsConfigRepository = null!;
    public ISocketDispatcherResolver ClientDispatcherResolver = null!;
    public PQClient PqClient = null!;
    public IPQConversationRepository<IPQSnapshotClient> SnapshotClientFactory = null!;
    public IPQConversationRepository<IPQUpdateClient> UpdateClientFactory = null!;

    public void InitializeClientPrereqs()
    {
        InitializeCommonConfig();
        ClientDispatcherResolver = new SimpleSocketDispatcherResolver(new SocketDispatcher(
            new SimpleSocketAsyncValueTaskRingPollerListener("PQClient", 1
                , new SocketSelector(1, NetworkingController), ThreadPoolTimer),
            new SimpleAsyncValueTaskSocketRingPollerSender("PQClient", 1)));
        SnapshotClientFactory = new PQSnapshotClientRepository(ClientDispatcherResolver);
        UpdateClientFactory = new PQUpdateClientRepository(ClientDispatcherResolver);
        ClientConnectionsConfigRepository =
            new MarketConnectionConfigRepository(MarketConnectionConfig).ToggleProtocolDirection();
    }

    public PQClient CreatePQClient()
    {
        PqClient = new PQClient(ClientConnectionsConfigRepository
            , SingletonSocketDispatcherResolver.Instance, UpdateClientFactory, SnapshotClientFactory);
        return PqClient;
    }

    [TestInitialize]
    public void Setup()
    {
        CreatePQClient();
    }

    [TestCleanup]
    public void TearDown()
    {
        PqClient.Dispose();
    }
}
