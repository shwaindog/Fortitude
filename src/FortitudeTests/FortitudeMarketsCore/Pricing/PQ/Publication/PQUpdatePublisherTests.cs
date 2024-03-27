#region

using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.NewSocketAPI.State;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

[TestClass]
public class PQUpdatePublisherTests
{
    private Mock<IInitiateControls> moqInitiatorControls = null!;
    private Mock<ISocketSessionContext> moqNewClientContext = null!;
    private Mock<IOSNetworkingController> moqNeworkingController = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISerdesFactory> moqSerdesFactory = null!;
    private Mock<IOSSocket> moqSocket = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<ISocketDispatcher> moqSocketDispatcher = null!;
    private Mock<ISocketDispatcherListener> moqSocketDispatcherListener = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactories = null!;
    private Mock<ISocketFactory> moqSocketFactory = null!;
    private PQUpdatePublisher pqUpdatePublisher = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqNeworkingController = new Mock<IOSNetworkingController>();
        moqParallelController = new Mock<IOSParallelController>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();
        moqSocketFactory = new Mock<ISocketFactory>();
        moqSerdesFactory = new Mock<ISerdesFactory>();
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqSocketDispatcher = new Mock<ISocketDispatcher>();
        moqSocket = new Mock<IOSSocket>();
        moqSocketDispatcherListener = new Mock<ISocketDispatcherListener>();
        moqInitiatorControls = new Mock<IInitiateControls>();
        moqNewClientContext = new Mock<ISocketSessionContext>();
        moqSocketConnection = new Mock<ISocketConnection>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);

        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        moqNewClientContext.SetupGet(ssc => ssc.Name).Returns("New Client Connection");
        moqNewClientContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object);
        moqNewClientContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactories.Object);
        moqSocketConnection.SetupGet(sc => sc.OSSocket).Returns(moqSocket.Object);
        moqSocketDispatcherResolver.Setup(sdr => sdr.Resolve(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSocketDispatcher.Object);
        moqSocketDispatcher.SetupGet(sd => sd.Listener).Returns(moqSocketDispatcherListener.Object);

        moqSerdesFactory.SetupProperty(sf => sf.StreamEncoderFactory);

        var moqSocketConnectivityChanged = new Mock<ISocketConnectivityChanged>();
        Func<ISocketSessionContext, ISocketConnectivityChanged> moqCallback = context =>
            moqSocketConnectivityChanged.Object;
        moqSocketFactories.SetupGet(pcf => pcf.SocketFactory).Returns(moqSocketFactory.Object);
        moqSocketFactories.SetupGet(pcf => pcf.NetworkingController).Returns(moqNeworkingController.Object);
        moqSocketFactories.SetupGet(pcf => pcf.ConnectionChangedHandlerResolver).Returns(moqCallback);
        moqSocketFactories.SetupGet(pcf => pcf.SocketDispatcherResolver).Returns(moqSocketDispatcherResolver.Object);
        // PQSnapshotServer.SocketFactories = moqSocketFactories.Object;
        var socketConConfig = new NetworkTopicConnectionConfig("PQUpdatePublisherTests"
            , SocketConversationProtocol.UdpPublisher, new[]
            {
                new EndpointConfig("testHostName", 3333, "127.0.0.1")
            }, connectionAttributes: SocketConnectionAttributes.Multicast | SocketConnectionAttributes.Fast);
        var socketSessionContext = new SocketSessionContext(ConversationType.Responder
            , SocketConversationProtocol.TcpAcceptor, "PQUpdatePublisherTests", socketConConfig
            , moqSocketFactories.Object, moqSerdesFactory.Object);

        pqUpdatePublisher = new PQUpdatePublisher(socketSessionContext, moqInitiatorControls.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }


    [TestMethod]
    public void NewPQUpdateServer_RegisterSerializer_ForPQFullSnapshot()
    {
        var registeredSerializers = pqUpdatePublisher.SerdesFactory!;

        Assert.IsTrue(registeredSerializers.StreamEncoderFactory != null);
        Assert.AreEqual(2, registeredSerializers.StreamEncoderFactory.RegisteredSerializerCount);
        Assert.IsTrue(registeredSerializers.StreamDecoderFactory == null);
    }

    [TestMethod]
    public void NewPQUpdateServer_GetFactory_ReturnsSerializationFactory()
    {
        var registeredSerializers = pqUpdatePublisher.SerdesFactory!;

        Assert.IsTrue(registeredSerializers.StreamEncoderFactory != null);
        Assert.AreEqual(2, registeredSerializers.StreamEncoderFactory.RegisteredSerializerCount);

        Assert.IsNotNull(registeredSerializers);
        var pqMessageSerializer = registeredSerializers.StreamEncoderFactory.MessageEncoder<IPQLevel0Quote>(0u);

        Assert.IsNotNull(pqMessageSerializer);
        var pqHeartBeatSerializer
            = registeredSerializers.StreamEncoderFactory.MessageEncoder<PQHeartBeatQuotesMessage>(1u);

        Assert.IsNotNull(pqHeartBeatSerializer);
    }
}
