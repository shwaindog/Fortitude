// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

[TestClass]
public class PQSnapshotClientRepositoryTests
{
    private Mock<IEnumerator<IEndpointConfig>> moqEndpointEnumerator = null!;

    private Mock<IFLogger>  moqFlogger  = null!;
    private Mock<IOSSocket> moqOsSocket = null!;

    private Mock<IOSParallelController> moqParallelControler = null!;

    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;

    private Mock<IPQClientQuoteDeserializerRepository>         moqPQQuoteDeserializationFactory = null!;
    private Mock<INotifyingMessageDeserializer<PQTickInstant>> moqSocketBinaryDeserializer      = null!;

    private Mock<IEndpointConfig>               moqSocketConnectionConfig      = null!;
    private Mock<ISocketDispatcherResolver>     moqSocketDispatcherResolver    = null!;
    private Mock<INetworkTopicConnectionConfig> moqSocketTopicConnectionConfig = null!;

    private PQSnapshotClientRepository pqSnapshotClientRegRepo = null!;

    private string testHostName = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger           = new Mock<IFLogger>();
        moqParallelControler = new Mock<IOSParallelController>();

        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
                                    .Returns(moqParallelControler.Object);

        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();

        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        moqSocketTopicConnectionConfig   = new Mock<INetworkTopicConnectionConfig>();
        moqEndpointEnumerator            = new Mock<IEnumerator<IEndpointConfig>>();
        moqSocketConnectionConfig        = new Mock<IEndpointConfig>();
        moqPQQuoteDeserializationFactory = new Mock<IPQClientQuoteDeserializerRepository>();
        moqSocketBinaryDeserializer      = new Mock<INotifyingMessageDeserializer<PQTickInstant>>();
        moqOsSocket                      = new Mock<IOSSocket>();

        testHostName = "TestHostname";
        moqEndpointEnumerator.SetupGet(stcc => stcc.Current).Returns(moqSocketConnectionConfig.Object);
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicName).Returns("PQSnapshotClientRepositoryTests");
        moqSocketTopicConnectionConfig.SetupGet(scc => scc.TopicDescription).Returns("PQSnapshotClientRepositoryTests");
        moqSocketConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        testHostPort = 1979;
        moqSocketConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqPQQuoteDeserializationFactory
            .Setup(pqqsf => pqqsf.GetDeserializer<PQTickInstant>(uint.MaxValue))
            .Returns(moqSocketBinaryDeserializer.Object).Verifiable();

        pqSnapshotClientRegRepo = new PQSnapshotClientRepository(moqSocketDispatcherResolver.Object);
    }

    [TestMethod]
    public void EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance()
    {
        var socketClient = pqSnapshotClientRegRepo.RetrieveOrCreateConversation(moqSocketTopicConnectionConfig.Object);

        Assert.IsNotNull(socketClient);
        Assert.IsInstanceOfType(socketClient, typeof(PQStandaloneSnapshotClient));

        var foundSubscription = pqSnapshotClientRegRepo.RetrieveConversation(moqSocketTopicConnectionConfig.Object);
        Assert.AreSame(socketClient, foundSubscription);
    }
}
