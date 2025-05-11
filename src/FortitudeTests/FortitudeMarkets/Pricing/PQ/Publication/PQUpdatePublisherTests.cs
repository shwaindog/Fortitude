// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Publication;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[TestClass]
public class PQUpdatePublisherTests
{
    private Mock<IMessageSerializationRepository> moqMessageSerializationRepo  = null!;
    private Mock<ISocketSessionContext>           moqNewClientContext          = null!;
    private Mock<IOSNetworkingController>         moqNeworkingController       = null!;
    private Mock<IOSParallelController>           moqParallelController        = null!;
    private Mock<IOSParallelControllerFactory>    moqParallelControllerFactory = null!;
    private Mock<IMessageSerdesRepositoryFactory> moqSerdesFactory             = null!;

    private Mock<IOSSocket>         moqSocket           = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<ISocketDispatcher> moqSocketDispatcher = null!;

    private Mock<ISocketDispatcherListener> moqSocketDispatcherListener = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<ISocketFactoryResolver>    moqSocketFactories          = null!;

    private Mock<ISocketFactory>  moqSocketFactory  = null!;
    private Mock<IStreamControls> moqStreamControls = null!;
    private PQUpdatePublisher     pqUpdatePublisher = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();

        moqNeworkingController = new Mock<IOSNetworkingController>();
        moqParallelController  = new Mock<IOSParallelController>();
        moqSocketFactories     = new Mock<ISocketFactoryResolver>();
        moqSocketFactory       = new Mock<ISocketFactory>();
        moqSerdesFactory       = new Mock<IMessageSerdesRepositoryFactory>();

        moqMessageSerializationRepo = new Mock<IMessageSerializationRepository>();
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();

        moqSocketDispatcher = new Mock<ISocketDispatcher>();
        moqSocket           = new Mock<IOSSocket>();

        moqSocketDispatcherListener = new Mock<ISocketDispatcherListener>();

        moqStreamControls   = new Mock<IStreamControls>();
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

        moqSerdesFactory.Setup(sf => sf.MessageSerializationRepository).Returns(moqMessageSerializationRepo.Object);
        moqMessageSerializationRepo.SetupGet(sf => sf.RegisteredMessageIds).Returns(new uint[] { 0, 1 });

        var moqSocketConnectivityChanged = new Mock<ISocketConnectivityChanged>();
        Func<ISocketSessionContext, ISocketConnectivityChanged> moqCallback = context =>
            moqSocketConnectivityChanged.Object;
        moqSocketFactories.SetupGet(pcf => pcf.SocketFactory).Returns(moqSocketFactory.Object);
        moqSocketFactories.SetupGet(pcf => pcf.NetworkingController).Returns(moqNeworkingController.Object);
        moqSocketFactories.SetupGet(pcf => pcf.ConnectionChangedHandlerResolver).Returns(moqCallback);
        moqSocketFactories.SetupGet(pcf => pcf.SocketDispatcherResolver).Returns(moqSocketDispatcherResolver.Object);
        // PQSnapshotServer.SocketFactories = moqSocketFactories.Object;
        var socketConConfig = new NetworkTopicConnectionConfig
            ("PQUpdatePublisherTests", SocketConversationProtocol.UdpPublisher
           , new[]
             {
                 new EndpointConfig("testHostName", 3333, "127.0.0.1")
             }, connectionAttributes: SocketConnectionAttributes.Multicast | SocketConnectionAttributes.Fast);
        var socketSessionContext = new SocketSessionContext
            ("PQUpdatePublisherTests", ConversationType.Responder, SocketConversationProtocol.TcpAcceptor
           , socketConConfig, moqSocketFactories.Object, moqSerdesFactory.Object);

        pqUpdatePublisher = new PQUpdatePublisher(socketSessionContext, moqStreamControls.Object);
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

        Assert.IsTrue(registeredSerializers.MessageSerializationRepository != null);
        Assert.AreEqual(2, registeredSerializers.MessageSerializationRepository.RegisteredMessageIds.Count());
    }

    [TestMethod]
    public void NewPQUpdateServer_GetFactory_ReturnsSerializationFactory()
    {
        var registeredSerializers = pqUpdatePublisher.SerdesFactory!;

        Assert.IsTrue(registeredSerializers.MessageSerializationRepository != null);
        Assert.AreEqual(2, registeredSerializers.MessageSerializationRepository.RegisteredMessageIds.Count());

        Assert.IsNotNull(registeredSerializers);
        var pqMessageSerializer = registeredSerializers.MessageSerializationRepository.RegisterSerializer<PQPublishableTickInstant>();

        Assert.IsNotNull(pqMessageSerializer);
        var pqHeartBeatSerializer
            = registeredSerializers.MessageSerializationRepository.RegisterSerializer<PQHeartBeatQuotesMessage>();

        Assert.IsNotNull(pqHeartBeatSerializer);
    }
}
