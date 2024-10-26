#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;
using FortitudeMarkets.Pricing.PQ.Publication;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[TestClass]
public class PQSnapshotServerTests
{
    private Mock<IAcceptorControls> moqAcceptorControls = null!;
    private Mock<IMessageSerializationRepository> moqMessageSerializationFactory = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<ISocketSessionContext> moqNewClientContext = null!;
    private Mock<ISocketSessionContext> moqPqSnapshotServerSessionConnection = null!;
    private Mock<IMessageSerdesRepositoryFactory> moqSerdesFactory = null!;
    private Mock<IOSSocket> moqSocket = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<ISocketDispatcher> moqSocketDispatcher = null!;
    private Mock<ISocketDispatcherListener> moqSocketDispatcherListener = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactories = null!;
    private Mock<ISocketFactory> moqSocketFactory = null!;
    private PQSnapshotServer pqSnapshotServer = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqSocket = new Mock<IOSSocket>();
        moqSocket.SetupAllProperties();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();
        moqSocketFactory = new Mock<ISocketFactory>();
        moqSerdesFactory = new Mock<IMessageSerdesRepositoryFactory>();
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqSocketDispatcher = new Mock<ISocketDispatcher>();
        moqSocketDispatcherListener = new Mock<ISocketDispatcherListener>();
        moqAcceptorControls = new Mock<IAcceptorControls>();
        moqNewClientContext = new Mock<ISocketSessionContext>();
        moqSocketConnection = new Mock<ISocketConnection>();
        moqMessageSerializationFactory = new Mock<IMessageSerializationRepository>();

        moqAcceptorControls.SetupAdd(ac => ac.NewClient += It.IsAny<Action<IConversationRequester>>()).Verifiable();

        moqNewClientContext.SetupGet(ssc => ssc.Name).Returns("New Client Connection");
        moqNewClientContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object);
        moqNewClientContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactories.Object);
        moqSocketConnection.SetupGet(sc => sc.OSSocket).Returns(moqSocket.Object);
        moqSocketDispatcherResolver.Setup(sdr => sdr.Resolve(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSocketDispatcher.Object);
        moqSocketDispatcher.SetupGet(sd => sd.Listener).Returns(moqSocketDispatcherListener.Object);

        var moqSocketConnectivityChanged = new Mock<ISocketConnectivityChanged>();
        Func<ISocketSessionContext, ISocketConnectivityChanged> moqCallback = context =>
            moqSocketConnectivityChanged.Object;
        moqSocketFactories.SetupGet(pcf => pcf.SocketFactory).Returns(moqSocketFactory.Object);
        moqSocketFactories.SetupGet(pcf => pcf.NetworkingController).Returns(moqNetworkingController.Object);
        moqSocketFactories.SetupGet(pcf => pcf.ConnectionChangedHandlerResolver).Returns(moqCallback);
        moqSocketFactories.SetupGet(pcf => pcf.SocketDispatcherResolver).Returns(moqSocketDispatcherResolver.Object);

        moqSerdesFactory.SetupGet(msrf => msrf.MessageSerializationRepository).Returns(moqMessageSerializationFactory.Object);
        // PQSnapshotServer.SocketFactories = moqSocketFactories.Object;
        var socketConConfig = new NetworkTopicConnectionConfig("PQSnapshotServerTests"
            , SocketConversationProtocol.TcpAcceptor, new[]
            {
                new EndpointConfig("testHostName", 3333)
            });

        var socketSessionContext = new SocketSessionContext("PQSnapshotServerTests", ConversationType.Responder
            , SocketConversationProtocol.TcpAcceptor, socketConConfig
            , moqSocketFactories.Object, moqSerdesFactory.Object);
        pqSnapshotServer = new PQSnapshotServer(socketSessionContext, moqAcceptorControls.Object);

        moqPqSnapshotServerSessionConnection = new Mock<ISocketSessionContext>();
        moqPqSnapshotServerSessionConnection.SetupGet(ssc => ssc.SocketDispatcher).Returns(moqSocketDispatcher.Object);
    }

    [TestMethod]
    public void NewPQSnapshotServer_MessageSerializationRepo_IsNotNull()
    {
        Assert.IsNotNull(pqSnapshotServer.MessageSerializationRepository);
    }
}
