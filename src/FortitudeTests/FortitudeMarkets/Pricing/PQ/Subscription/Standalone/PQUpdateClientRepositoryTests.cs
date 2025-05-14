// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Subscription.Standalone;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

[TestClass]
public class PQUpdateClientRepositoryTests
{
    private Mock<Action<SocketSessionState>> moqAction = null!;

    private Mock<IFLogger>  moqFlogger  = null!;
    private Mock<IOSSocket> moqOsSocket = null!;

    private Mock<IOSParallelController> moqParallelController = null!;

    private Mock<IOSParallelControllerFactory>         moqParallelControllerFactory = null!;
    private Mock<IPQClientQuoteDeserializerRepository> moqPQQuoteSerializationRepo  = null!;

    private Mock<IEndpointConfig> moqServerConnectionConfig = null!;

    private Mock<INotifyingMessageDeserializer<PQPublishableTickInstant>> moqSocketBinaryDeserializer = null!;

    private Mock<ISocketConnectivityChanged>    moqSocketConnectivityChanged   = null!;
    private Mock<ISocketDispatcherResolver>     moqSocketDispatcherResolver    = null!;
    private Mock<ISocketFactoryResolver>        moqSocketFactories             = null!;
    private Mock<INetworkTopicConnectionConfig> moqSocketTopicConnectionConfig = null!;

    private Mock<IStreamControls>        moqStreamControls        = null!;
    private Mock<IStreamControlsFactory> moqStreamControlsFactory = null!;

    private PQUpdateClientRepository pqUpdateClientRegFactory = null!;

    private string testHostName = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();

        moqParallelController = new Mock<IOSParallelController>();

        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
                                    .Returns(moqParallelController.Object);
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();

        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        moqSocketTopicConnectionConfig = new Mock<INetworkTopicConnectionConfig>();
        moqSocketConnectivityChanged   = new Mock<ISocketConnectivityChanged>();
        moqServerConnectionConfig      = new Mock<IEndpointConfig>();
        moqPQQuoteSerializationRepo    = new Mock<IPQClientQuoteDeserializerRepository>();
        moqSocketBinaryDeserializer    = new Mock<INotifyingMessageDeserializer<PQPublishableTickInstant>>();
        moqStreamControlsFactory       = new Mock<IStreamControlsFactory>();

        moqStreamControls  = new Mock<IStreamControls>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();
        moqOsSocket        = new Mock<IOSSocket>();
        moqAction          = new Mock<Action<SocketSessionState>>();

        moqSocketConnectivityChanged.Setup(scc => scc.GetOnConnectionChangedHandler()).Returns(moqAction.Object);

        ISocketConnectivityChanged ConnectivityChanged(ISocketSessionContext context) => moqSocketConnectivityChanged.Object;

        moqStreamControlsFactory.Setup(scf => scf.ResolveStreamControls(It.IsAny<ISocketSessionContext>()))
                                .Returns(moqStreamControls.Object);
        moqSocketFactories.SetupGet(sf => sf.ConnectionChangedHandlerResolver).Returns(ConnectivityChanged);
        moqSocketFactories.SetupGet(sf => sf.StreamControlsFactory).Returns(moqStreamControlsFactory.Object);
        moqSocketFactories.SetupGet(sf => sf.SocketDispatcherResolver).Returns(moqSocketDispatcherResolver.Object);
        PQUpdateClient.SocketFactories = moqSocketFactories.Object;
        testHostName                   = "TestHostname";
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicName).Returns("PQUpdateClientRepositoryTests");
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicDescription).Returns("PQUpdateClientRepositoryTests");
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        testHostPort = 1979;
        moqServerConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqPQQuoteSerializationRepo.Setup(pqqsf => pqqsf.GetDeserializer<PQPublishableTickInstant>(uint.MaxValue))
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
        moqStreamControls.Setup(ic => ic.Start()).Verifiable();
        var socketClient = pqUpdateClientRegFactory.RetrieveOrCreateConversation(moqSocketTopicConnectionConfig.Object);

        Assert.IsNotNull(socketClient);
        Assert.IsInstanceOfType(socketClient, typeof(PQUpdateClient));

        moqStreamControls.Verify();
        var foundSubscription = pqUpdateClientRegFactory.RetrieveConversation(moqSocketTopicConnectionConfig.Object);

        Assert.AreSame(socketClient, foundSubscription);
    }
}
