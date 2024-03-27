#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.State;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQUpdateClientRepositoryTests
{
    private Mock<Action<SocketSessionState>> moqAction = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IInitiateControls> moqInitiateControls = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQQuoteSerializerRepository> moqPQQuoteSerializationRepo = null!;
    private Mock<IEndpointConfig> moqServerConnectionConfig = null!;
    private Mock<ICallbackMessageDeserializer<PQLevel0Quote>> moqSocketBinaryDeserializer = null!;
    private Mock<ISocketConnectivityChanged> moqSocketConnectivityChanged = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactories = null!;
    private Mock<INetworkTopicConnectionConfig> moqSocketTopicConnectionConfig = null!;
    private Mock<IStreamControlsFactory> moqStreamControlsFactory = null!;
    private PQUpdateClientRepository pqUpdateClientRegFactory = null!;
    private string testHostName = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqSocketTopicConnectionConfig = new Mock<INetworkTopicConnectionConfig>();
        moqServerConnectionConfig = new Mock<IEndpointConfig>();
        moqPQQuoteSerializationRepo = new Mock<IPQQuoteSerializerRepository>();
        moqStreamControlsFactory = new Mock<IStreamControlsFactory>();
        moqInitiateControls = new Mock<IInitiateControls>();
        moqSocketBinaryDeserializer = new Mock<ICallbackMessageDeserializer<PQLevel0Quote>>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();
        moqOsSocket = new Mock<IOSSocket>();
        moqSocketConnectivityChanged = new Mock<ISocketConnectivityChanged>();
        moqAction = new Mock<Action<SocketSessionState>>();

        moqSocketConnectivityChanged.Setup(scc => scc.GetOnConnectionChangedHandler()).Returns(moqAction.Object);

        ISocketConnectivityChanged ConnectivityChanged(ISocketSessionContext context) =>
            moqSocketConnectivityChanged.Object;

        moqStreamControlsFactory.Setup(scf => scf.ResolveStreamControls(It.IsAny<ISocketSessionContext>()))
            .Returns(moqInitiateControls.Object);
        moqSocketFactories.SetupGet(sf => sf.ConnectionChangedHandlerResolver).Returns(ConnectivityChanged);
        moqSocketFactories.SetupGet(sf => sf.StreamControlsFactory).Returns(moqStreamControlsFactory.Object);
        moqSocketFactories.SetupGet(sf => sf.SocketDispatcherResolver).Returns(moqSocketDispatcherResolver.Object);
        PQUpdateClient.SocketFactories = moqSocketFactories.Object;
        testHostName = "TestHostname";
        moqSocketTopicConnectionConfig.SetupGet(stcc => stcc.Current).Returns(moqServerConnectionConfig.Object);
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicName).Returns("PQUpdateClientRepositoryTests");
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicDescription).Returns("PQUpdateClientRepositoryTests");
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        testHostPort = 1979;
        moqServerConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqPQQuoteSerializationRepo.Setup(pqqsf => pqqsf.GetDeserializer<PQLevel0Quote>(uint.MaxValue))
            .Returns(moqSocketBinaryDeserializer.Object).Verifiable();

        pqUpdateClientRegFactory = new PQUpdateClientRepository(moqSocketDispatcherResolver.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        PQUpdateClient.SocketFactories = SocketFactoryResolver.GetRealSocketFactories();
    }

    [TestMethod]
    public void EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
    {
        moqInitiateControls.Setup(ic => ic.Start()).Verifiable();
        var socketClient = pqUpdateClientRegFactory.RetrieveOrCreateConversation(moqSocketTopicConnectionConfig.Object);

        Assert.IsNotNull(socketClient);
        Assert.IsInstanceOfType(socketClient, typeof(PQUpdateClient));

        moqInitiateControls.Verify();
        var foundSubscription = pqUpdateClientRegFactory.RetrieveConversation(moqSocketTopicConnectionConfig.Object);

        Assert.AreSame(socketClient, foundSubscription);
    }
}
