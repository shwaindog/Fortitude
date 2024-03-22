#region

using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

[TestClass]
public class PQSnapshotServerTests
{
    private Mock<IAcceptorControls> moqAcceptorControls = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<ISocketSessionContext> moqNewClientContext = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISocketSessionContext> moqPqSnapshotServerSessionConnection = null!;
    private Mock<ISerdesFactory> moqSerdesFactory = null!;
    private Mock<IOSSocket> moqSocket = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<ISocketDispatcher> moqSocketDispatcher = null!;
    private Mock<ISocketDispatcherListener> moqSocketDispatcherListener = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<ISocketFactories> moqSocketFactories = null!;
    private Mock<ISocketFactory> moqSocketFactory = null!;
    private PQSnapshotServer pqSnapshotServer = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqSocket = new Mock<IOSSocket>();
        moqSocket.SetupAllProperties();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqParallelController = new Mock<IOSParallelController>();
        moqSocketFactories = new Mock<ISocketFactories>();
        moqSocketFactory = new Mock<ISocketFactory>();
        moqSerdesFactory = new Mock<ISerdesFactory>();
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqSocketDispatcher = new Mock<ISocketDispatcher>();
        moqSocketDispatcherListener = new Mock<ISocketDispatcherListener>();
        moqAcceptorControls = new Mock<IAcceptorControls>();
        moqNewClientContext = new Mock<ISocketSessionContext>();
        moqSocketConnection = new Mock<ISocketConnection>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);

        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqAcceptorControls.SetupAdd(ac => ac.NewClient += It.IsAny<Action<IConversationRequester>>()).Verifiable();

        moqNewClientContext.SetupGet(ssc => ssc.Name).Returns("New Client Connection");
        moqNewClientContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object);
        moqNewClientContext.SetupGet(ssc => ssc.SocketFactories).Returns(moqSocketFactories.Object);
        moqSocketConnection.SetupGet(sc => sc.OSSocket).Returns(moqSocket.Object);
        moqSocketDispatcherResolver.Setup(sdr => sdr.Resolve(It.IsAny<ISocketConnectionConfig>()))
            .Returns(moqSocketDispatcher.Object);
        moqSocketDispatcher.SetupGet(sd => sd.Listener).Returns(moqSocketDispatcherListener.Object);

        var moqSocketConnectivityChanged = new Mock<ISocketConnectivityChanged>();
        Func<ISocketSessionContext, ISocketConnectivityChanged> moqCallback = context =>
            moqSocketConnectivityChanged.Object;
        moqSocketFactories.SetupGet(pcf => pcf.SocketFactory).Returns(moqSocketFactory.Object);
        moqSocketFactories.SetupGet(pcf => pcf.NetworkingController).Returns(moqNetworkingController.Object);
        moqSocketFactories.SetupGet(pcf => pcf.ConnectionChangedHandlerResolver).Returns(moqCallback);
        moqSocketFactories.SetupGet(pcf => pcf.SocketDispatcherResolver).Returns(moqSocketDispatcherResolver.Object);
        // PQSnapshotServer.SocketFactories = moqSocketFactories.Object;
        var socketConConfig = new SocketConnectionConfig("PQSnapshotServerTests", "PQSnapshotServerTests",
            SocketConnectionAttributes.None, 2_000_000, 2_000_000, "testHostName", null,
            false, 3333, 3333);
        var socketSessionContext = new SocketSessionContext(ConversationType.Responder
            , SocketConversationProtocol.TcpAcceptor, "PQSnapshotServerTests", socketConConfig
            , moqSocketFactories.Object, moqSerdesFactory.Object);
        pqSnapshotServer = new PQSnapshotServer(socketSessionContext, moqAcceptorControls.Object);

        moqPqSnapshotServerSessionConnection = new Mock<ISocketSessionContext>();
        moqPqSnapshotServerSessionConnection.SetupGet(ssc => ssc.SocketDispatcher).Returns(moqSocketDispatcher.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void NewPQSnapshotServer_SerdesForAcceptor_ReturnsNoSerdesFromFactory()
    {
        var registeredSerializers = pqSnapshotServer.SerdesFactory!;

        Assert.IsTrue(registeredSerializers.StreamEncoderFactory == null);
        Assert.IsTrue(registeredSerializers.StreamDecoderFactory == null);
    }
}
