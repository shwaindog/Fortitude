#region

using System.Reactive.Subjects;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQSnapshotClientRegistrationFactoryTests
{
    private ISubject<IConnectionUpdate> configUpdateSubject = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQQuoteSerializerFactory> moqPQQuoteSerializationFactory = null!;
    private Mock<IConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<ICallbackBinaryDeserializer<PQLevel0Quote>> moqSocketBinaryDeserializer = null!;
    private PQSnapshotClientRegistrationFactory pqSnapshotClientRegFactory = null!;
    private string testHostName = null!;
    private int testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqServerConnectionConfig = new Mock<IConnectionConfig>();
        moqPQQuoteSerializationFactory = new Mock<IPQQuoteSerializerFactory>();
        moqSocketBinaryDeserializer = new Mock<ICallbackBinaryDeserializer<PQLevel0Quote>>();
        moqOsSocket = new Mock<IOSSocket>();
        configUpdateSubject = new Subject<IConnectionUpdate>();

        testHostName = "TestHostname";
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        testHostPort = 1979;
        moqServerConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqServerConnectionConfig.SetupProperty(scc => scc.Updates, configUpdateSubject);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqPQQuoteSerializationFactory.Setup(pqqsf => pqqsf.GetDeserializer<PQLevel0Quote>(uint.MaxValue))
            .Returns(moqSocketBinaryDeserializer.Object).Verifiable();

        pqSnapshotClientRegFactory = new PQSnapshotClientRegistrationFactory(moqNetworkingController.Object);
    }

    [TestMethod]
    public void EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
    {
        var socketClient = pqSnapshotClientRegFactory.RegisterSocketSubscriber("TestSocketDescription",
            moqServerConnectionConfig.Object, uint.MaxValue, moqDispatcher.Object, 50,
            moqPQQuoteSerializationFactory.Object, "multicastInterfaceIP");

        Assert.IsNotNull(socketClient);
        Assert.IsInstanceOfType(socketClient, typeof(PQSnapshotClient));

        var foundSubscription = pqSnapshotClientRegFactory.FindSocketSubscription(moqServerConnectionConfig.Object);
        Assert.AreSame(socketClient, foundSubscription);
    }
}
