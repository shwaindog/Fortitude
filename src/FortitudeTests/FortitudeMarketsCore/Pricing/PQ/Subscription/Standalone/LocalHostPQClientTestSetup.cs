#region

using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;
using FortitudeTests.FortitudeCommon.Types;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

[NoMatchingProductionClass]
public class LocalHostPQClientTestSetup : LocalHostPQTestSetupCommon
{
    public ISocketDispatcherResolver ClientDispatcherResolver = null!;
    public IMarketsConfig DefaultClientMarketsConfig = null!;
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
        DefaultClientMarketsConfig = DefaultServerMarketsConfig.ToggleProtocolDirection("LocalHostPQClientTestSetup");
    }

    public PQClient CreatePQClient(IMarketsConfig? overrideMarketsConfig = null)
    {
        PqClient = new PQClient(overrideMarketsConfig ?? DefaultClientMarketsConfig
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
