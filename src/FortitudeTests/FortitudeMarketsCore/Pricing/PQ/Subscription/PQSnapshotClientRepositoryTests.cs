#region

using System.Reactive.Subjects;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.Sockets;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;
using ISocketDispatcher = FortitudeIO.Transports.Sockets.Dispatcher.ISocketDispatcher;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQSnapshotClientRepositoryTests
{
    private ISubject<IConnectionUpdate> configUpdateSubject = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQQuoteSerializerRepository> moqPQQuoteSerializationFactory = null!;
    private Mock<ISocketConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<ICallbackMessageDeserializer<PQLevel0Quote>> moqSocketBinaryDeserializer = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private PQSnapshotClientRepository pqSnapshotClientRegRepo = null!;
    private string testHostName = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqServerConnectionConfig = new Mock<ISocketConnectionConfig>();
        moqPQQuoteSerializationFactory = new Mock<IPQQuoteSerializerRepository>();
        moqSocketBinaryDeserializer = new Mock<ICallbackMessageDeserializer<PQLevel0Quote>>();
        moqOsSocket = new Mock<IOSSocket>();
        configUpdateSubject = new Subject<IConnectionUpdate>();

        testHostName = "TestHostname";
        moqServerConnectionConfig.SetupGet(scc => scc.InstanceName).Returns("PQSnapshotClientRepositoryTests");
        moqServerConnectionConfig.SetupGet(scc => scc.SocketDescription).Returns("PQSnapshotClientRepositoryTests");
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        testHostPort = 1979;
        moqServerConnectionConfig.SetupGet(scc => scc.PortStartRange).Returns(testHostPort);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqPQQuoteSerializationFactory.Setup(pqqsf => pqqsf.GetDeserializer<PQLevel0Quote>(uint.MaxValue))
            .Returns(moqSocketBinaryDeserializer.Object).Verifiable();

        pqSnapshotClientRegRepo = new PQSnapshotClientRepository(moqSocketDispatcherResolver.Object);
    }

    [TestMethod]
    public void EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
    {
        var socketClient = pqSnapshotClientRegRepo.RetrieveOrCreateConversation(moqServerConnectionConfig.Object);

        Assert.IsNotNull(socketClient);
        Assert.IsInstanceOfType(socketClient, typeof(PQSnapshotClient));

        var foundSubscription = pqSnapshotClientRegRepo.RetrieveConversation(moqServerConnectionConfig.Object);
        Assert.AreSame(socketClient, foundSubscription);
    }
}
