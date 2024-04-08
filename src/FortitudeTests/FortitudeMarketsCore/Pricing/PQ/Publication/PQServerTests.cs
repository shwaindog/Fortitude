#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeTests.TestEnvironment;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

[TestClass]
public class PQServerTests
{
    private const string ExchangeName = "ComponentTestExchange";
    private const ushort ExchangeId = 1;
    private const string TestTicker1 = "EUR/USD";
    private const string TestTicker2 = "USD/JPY";
    private const string TestTicker3 = "GBP/USD";
    private const ushort TickerId1 = 1;
    private const ushort TickerId2 = 2;
    private const ushort TickerId3 = 3;
    private Mock<IPQServerHeartBeatSender> moqHeartBeatSender = null!;
    private Mock<IPQSnapshotServer> moqSnapshotService = null!;
    private Mock<ISocketDispatcher> moqSocketDispatcher = null!;
    private Mock<ISocketDispatcherListener> moqSocketDispatcherListener = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<ISocketDispatcherSender> moqSocketDispatcherSender = null!;
    private Mock<IPQUpdateServer> moqUpdateService = null!;
    private Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer> pqSnapshotFactory = null!;
    private Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer> pqUpdateFactory = null!;
    private SnapshotUpdatePricingServerConfig snapshotUpdatePricingServerConfig = null!;
    private SourceTickerPublicationConfigRepository sourceTickerPublicationConfigs = null!;
    private SourceTickerPublicationConfig sourceTickerQuoteInfo1 = null!;
    private SourceTickerPublicationConfig sourceTickerQuoteInfo2 = null!;
    private SourceTickerPublicationConfig sourceTickerQuoteInfo3 = null!;

    public void Setup(LayerFlags layerDetails, LastTradedFlags lastTradedFlags = LastTradedFlags.None)
    {
        sourceTickerQuoteInfo1 =
            new SourceTickerPublicationConfig(
                UniqueSourceTickerIdentifier.GenerateUniqueSourceTickerId(ExchangeId, TickerId1),
                ExchangeName, TestTicker1, 20, 0.00001m, 0.1m, 100, 0.1m, 250, layerDetails, lastTradedFlags);
        sourceTickerQuoteInfo2 =
            new SourceTickerPublicationConfig(
                UniqueSourceTickerIdentifier.GenerateUniqueSourceTickerId(ExchangeId, TickerId2),
                ExchangeName, TestTicker2, 20, 0.00001m, 0.1m, 100, 0.1m, 250, layerDetails, lastTradedFlags);
        sourceTickerQuoteInfo3 =
            new SourceTickerPublicationConfig(
                UniqueSourceTickerIdentifier.GenerateUniqueSourceTickerId(ExchangeId, TickerId3),
                ExchangeName, TestTicker3, 20, 0.00001m, 0.1m, 100, 0.1m, 250, layerDetails, lastTradedFlags);
        sourceTickerPublicationConfigs =
            new SourceTickerPublicationConfigRepository(new[]
                { sourceTickerQuoteInfo1, sourceTickerQuoteInfo2, sourceTickerQuoteInfo3 });
        snapshotUpdatePricingServerConfig = new SnapshotUpdatePricingServerConfig(ExchangeName,
            MarketServerType.MarketData,
            new[]
            {
                new NetworkTopicConnectionConfig("TestSnapshotServer", SocketConversationProtocol.TcpAcceptor, new[]
                {
                    new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                        , TestMachineConfig.ServerSnapshotPort)
                })
                , new NetworkTopicConnectionConfig("TestUpdateServer", SocketConversationProtocol.UdpPublisher, new[]
                {
                    new EndpointConfig(TestMachineConfig.LoopBackIpAddress, TestMachineConfig.ServerUpdatePort
                        , TestMachineConfig.NetworkSubAddress)
                }, connectionAttributes: SocketConnectionAttributes.Multicast | SocketConnectionAttributes.Fast)
            }, null, 9000, sourceTickerPublicationConfigs, false, false);

        moqHeartBeatSender = new Mock<IPQServerHeartBeatSender>();
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqSocketDispatcherListener = new Mock<ISocketDispatcherListener>();
        moqSocketDispatcherSender = new Mock<ISocketDispatcherSender>();
        moqSocketDispatcher = new Mock<ISocketDispatcher>();
        moqSocketDispatcherResolver.Setup(sdr => sdr.Resolve(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSocketDispatcher.Object);
        moqSocketDispatcher.SetupProperty(sd => sd.Listener, moqSocketDispatcherListener.Object);
        moqSocketDispatcher.SetupProperty(sd => sd.Sender, moqSocketDispatcherSender.Object);

        moqSnapshotService = new Mock<IPQSnapshotServer>();
        moqUpdateService = new Mock<IPQUpdateServer>();
        moqSnapshotService.Setup(sss => sss.Start()).Verifiable();

        moqHeartBeatSender.SetupSet(hbs => hbs.UpdateServer = moqUpdateService.Object).Verifiable();

        pqSnapshotFactory = (_, _) => moqSnapshotService.Object;
        pqUpdateFactory = (_, _) => moqUpdateService.Object;
    }

    [TestMethod]
    public void NewPQServer_StartServices_ConnectsUpdateServiceAndListensToSnapshotService()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQLevel0Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        moqSnapshotService.Verify();
        moqUpdateService.Verify();
    }

    [TestMethod]
    public void NewPQServer_StartServices_GivesHeartBeatServiceUpdateService()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        moqHeartBeatSender.SetupSet(hbs => hbs.UpdateServer = moqUpdateService.Object).Verifiable();

        var pqServer = new PQServer<PQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        moqHeartBeatSender.Verify();
    }

    [TestMethod]
    public void PQServerWithNullQuoteFactory_Register_CreateLevel0QuoteReadyForPublish()
    {
        TestLevelQuoteIsReturned<PQLevel0Quote>();
    }

    [TestMethod]
    public void PQServerWithNullQuoteFactory_Register_CreateLevel1QuoteReadyForPublish()
    {
        TestLevelQuoteIsReturned<PQLevel1Quote>();
    }

    [TestMethod]
    public void PQServerWithNullQuoteFactory_Register_CreateLevel2QuoteReadyForPublish()
    {
        TestLevelQuoteIsReturned<PQLevel2Quote>();
    }

    [TestMethod]
    public void PQServerWithNullQuoteFactory_Register_CreateLevel3QuoteReadyForPublish()
    {
        TestLevelQuoteIsReturned<PQLevel3Quote>();
    }

    [TestMethod]
    public void StartedPQServer_Register_PublishesEmptyQuoteToConnectedUpdateServer()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        moqUpdateService.SetupGet(us => us.IsStarted).Returns(true);
        moqUpdateService.Setup(us => us.Send(It.IsAny<PQLevel1Quote>())).Verifiable();

        pqServer.Register(TestTicker1);

        moqUpdateService.Verify();
    }

    [TestMethod]
    public void StartedPQServer_Register_DoesNotPublishedToUnconnectedUpdateServer()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        moqUpdateService.SetupGet(us => us.IsStarted).Returns(false);

        pqServer.Register(TestTicker1);

        moqUpdateService.Verify();
        moqUpdateService.Verify(us => us.Send(It.IsAny<PQLevel1Quote>()), Times.Never);
    }

    [TestMethod]
    public void StartedPQServer_Register_StartsUnStartedHeartBeatSender()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        moqHeartBeatSender.SetupGet(hbs => hbs.HasStarted).Returns(false);
        moqHeartBeatSender.Setup(hbs => hbs.StartSendingHeartBeats()).Verifiable();

        pqServer.Register(TestTicker1);

        moqHeartBeatSender.Verify();
    }


    [TestMethod]
    public void StartedPQServerHeartBeatingAlready_Register_LeavesHeartBeatSenderAlone()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        moqHeartBeatSender.SetupGet(hbs => hbs.HasStarted).Returns(true);

        pqServer.Register(TestTicker1);

        moqHeartBeatSender.Verify(hbs => hbs.StartSendingHeartBeats(), Times.Never);
    }

    [TestMethod]
    public void StartedPQServer_RegisterUnknownTicker_ReturnsNoQuoteForPublishing()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        var quoteToSubmit = pqServer.Register("UNK/OWN");

        Assert.IsNull(quoteToSubmit);
    }

    [TestMethod]
    public void RegisteredPQServer_Publish_ProtectsUpdatesToQuoteBehindQuoteSyncLock()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQLevel0Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel0Quote(info));

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQLevel0Quote>();
        var isInQuoteSyncLock = false;
        var moqSyncLock = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire()).Callback(() => { isInQuoteSyncLock = true; }).Verifiable();

        var moqRegisteredPqLevel0Quote = new Mock<IPQLevel0Quote>();
        moqRegisteredPqLevel0Quote.SetupGet(l0Q => l0Q.Lock).Returns(moqSyncLock.Object).Verifiable();
        moqRegisteredPqLevel0Quote.SetupGet(l0Q => l0Q.PQSequenceId)
            .Callback(() => { Assert.IsTrue(isInQuoteSyncLock); }).Returns(10u).Verifiable();

        // can't use moq because of property redefinition not handled properly
        var stubLevel1Quote = new PQLevel0QuoteTests.DummyPQLevel0Quote
        {
            SourceTickerQuoteInfo = sourceTickerQuoteInfo1, HasUpdates = true
        };
        moqRegisteredPqLevel0Quote.Setup(l0Q => l0Q.CopyFrom((ILevel0Quote)stubLevel1Quote, CopyMergeFlags.Default))
            .Callback(() => { Assert.IsTrue(isInQuoteSyncLock); }).Returns(moqRegisteredPqLevel0Quote.Object)
            .Verifiable();
        moqRegisteredPqLevel0Quote.SetupSet(l0Q => l0Q.PQSequenceId = 10)
            .Callback(() => { Assert.IsTrue(isInQuoteSyncLock); }).Verifiable();
        moqSyncLock.Setup(sl => sl.Release()).Callback(() => { isInQuoteSyncLock = false; }).Verifiable();

        replaceWithPQServerInstance.Add(sourceTickerQuoteInfo1.Id, moqRegisteredPqLevel0Quote.Object);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);

        Assert.IsFalse(isInQuoteSyncLock);
        pqServer.Publish(stubLevel1Quote);
        Assert.IsFalse(isInQuoteSyncLock);
        Assert.IsFalse(stubLevel1Quote.HasUpdates);
        moqRegisteredPqLevel0Quote.Verify();
        moqSyncLock.Verify();
    }

    [TestMethod]
    public void RegisteredPQServer_Publish_ReordersPublishedQuoteToEndOfHeartBeats()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQLevel0Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel0Quote(info));

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQLevel0Quote>();
        var isInHeartBeatSyncLock = false;
        var moqQuoteSyncLock = new Mock<ISyncLock>();
        moqQuoteSyncLock.Setup(sl => sl.Acquire());
        var ticker1RegisteredPqLevel0Quote = new PQLevel0Quote(sourceTickerQuoteInfo1);

        // can't use moq because of property hiding/redefinition not handled properly
        var stubLevel1Quote = new PQLevel0QuoteTests.DummyPQLevel0Quote
        {
            SourceTickerQuoteInfo = sourceTickerQuoteInfo1, HasUpdates = true
        };
        moqQuoteSyncLock.Setup(sl => sl.Release());

        replaceWithPQServerInstance.Add(sourceTickerQuoteInfo1.Id, ticker1RegisteredPqLevel0Quote);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);
        var moqHeartBeatSyncLock = new Mock<ISyncLock>();

        moqHeartBeatSyncLock.Setup(sl => sl.Acquire())
            .Callback(() => { isInHeartBeatSyncLock = true; }).Verifiable();
        moqHeartBeatSyncLock.Setup(sl => sl.Release())
            .Callback(() => { isInHeartBeatSyncLock = false; }).Verifiable();
        NonPublicInvocator.SetInstanceField(pqServer, "heartBeatSync", moqHeartBeatSyncLock.Object);

        var heartBeatQuotes = new DoublyLinkedList<IPQLevel0Quote>();
        var initialLastLvl0Quote = new PQLevel0QuoteTests.DummyPQLevel0Quote();
        var initialMiddleLvl0Quote = new PQLevel0QuoteTests.DummyPQLevel0Quote();
        heartBeatQuotes.AddLast(initialLastLvl0Quote);
        heartBeatQuotes.AddFirst(initialMiddleLvl0Quote);
        heartBeatQuotes.AddFirst(ticker1RegisteredPqLevel0Quote);
        NonPublicInvocator.SetInstanceField(pqServer, "heartbeatQuotes", heartBeatQuotes);

        Assert.IsFalse(isInHeartBeatSyncLock);
        pqServer.Publish(stubLevel1Quote);
        Assert.IsFalse(isInHeartBeatSyncLock);
        moqHeartBeatSyncLock.Verify();
        Assert.AreSame(initialMiddleLvl0Quote, heartBeatQuotes.Head);
        Assert.AreSame(ticker1RegisteredPqLevel0Quote, heartBeatQuotes.Tail);
    }

    [TestMethod]
    public void RegisteredPQServer_Publish_ProtectsReorderedHeartBeat()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQLevel0Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel0Quote(info));

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQLevel0Quote>();
        var moqQuoteSyncLock = new Mock<ISyncLock>();
        moqQuoteSyncLock.Setup(sl => sl.Acquire());
        var moqRegisteredPqLevel0Quote = new Mock<IPQLevel0Quote>();
        moqRegisteredPqLevel0Quote.SetupGet(l0Q => l0Q.Lock).Returns(moqQuoteSyncLock.Object).Verifiable();
        moqRegisteredPqLevel0Quote.SetupProperty(l0Q => l0Q.PQSequenceId, 10u);

        // can't use moq because of property hiding/redefinition not handled properly
        var stubLevel1Quote = new PQLevel0QuoteTests.DummyPQLevel0Quote
        {
            SourceTickerQuoteInfo = sourceTickerQuoteInfo1, HasUpdates = true
        };
        moqRegisteredPqLevel0Quote.Setup(l0Q => l0Q.CopyFrom((ILevel0Quote)stubLevel1Quote, CopyMergeFlags.Default));
        moqQuoteSyncLock.Setup(sl => sl.Release());

        replaceWithPQServerInstance.Add(sourceTickerQuoteInfo1.Id, moqRegisteredPqLevel0Quote.Object);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);
        var isInHeartBeatSyncLock = false;
        var moqHeartBeatSyncLock = new Mock<ISyncLock>();
        moqHeartBeatSyncLock.Setup(sl => sl.Acquire())
            .Callback(() => { isInHeartBeatSyncLock = true; }).Verifiable();
        moqHeartBeatSyncLock.Setup(sl => sl.Release())
            .Callback(() => { isInHeartBeatSyncLock = false; }).Verifiable();
        moqRegisteredPqLevel0Quote.SetupSet(rpql0Q => rpql0Q.LastPublicationTime = It.IsAny<DateTime>())
            .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();
        NonPublicInvocator.SetInstanceField(pqServer, "heartBeatSync", moqHeartBeatSyncLock.Object);

        var moqHeartBeatQuotes = new Mock<IDoublyLinkedList<IPQLevel0Quote>>();
        moqHeartBeatQuotes.Setup(hbq => hbq.Remove(moqRegisteredPqLevel0Quote.Object))
            .Callback(() => { Assert.IsTrue(isInHeartBeatSyncLock); })
            .Returns(moqRegisteredPqLevel0Quote.Object).Verifiable();
        moqHeartBeatQuotes.Setup(hbq => hbq.AddLast(moqRegisteredPqLevel0Quote.Object))
            .Callback(() => { Assert.IsTrue(isInHeartBeatSyncLock); })
            .Returns(moqRegisteredPqLevel0Quote.Object).Verifiable();
        NonPublicInvocator.SetInstanceField(pqServer, "heartbeatQuotes", moqHeartBeatQuotes.Object);

        Assert.IsFalse(isInHeartBeatSyncLock);
        pqServer.Publish(stubLevel1Quote);
        Assert.IsFalse(isInHeartBeatSyncLock);
        moqHeartBeatSyncLock.Verify();
        moqRegisteredPqLevel0Quote.Verify();
        moqHeartBeatQuotes.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PQServer_PublishUnregisteredQuote_ExpectArgumentException()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQLevel0Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel0Quote(info));


        // can't use moq because of property hiding/redefinition not handled properly
        var stubLevel1Quote = new PQLevel0QuoteTests.DummyPQLevel0Quote
        {
            SourceTickerQuoteInfo = sourceTickerQuoteInfo1, HasUpdates = true
        };
        pqServer.Publish(stubLevel1Quote);
    }

    [TestMethod]
    public void RegisteredPQServerDisconnectedUpdateServer_Publish_DoesNotSendReconnectDoesSend()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        moqUpdateService.SetupGet(us => us.IsStarted).Returns(false).Verifiable();

        var pqServer = new PQServer<IPQLevel0Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel0Quote(info));

        pqServer.StartServices();

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQLevel0Quote>();
        var moqSyncLock = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire());

        var moqRegisteredPqLevel0Quote = new Mock<IPQLevel0Quote>();
        moqRegisteredPqLevel0Quote.SetupGet(l0Q => l0Q.Lock).Returns(moqSyncLock.Object);

        // can't use moq because of property redefinition not handled properly
        var stubLevel1Quote = new PQLevel0QuoteTests.DummyPQLevel0Quote
        {
            SourceTickerQuoteInfo = sourceTickerQuoteInfo1, HasUpdates = true
        };
        moqRegisteredPqLevel0Quote.Setup(l0Q => l0Q.CopyFrom((ILevel0Quote)stubLevel1Quote, CopyMergeFlags.Default));
        moqRegisteredPqLevel0Quote.SetupProperty(l0Q => l0Q.PQSequenceId, 10u);
        moqSyncLock.Setup(sl => sl.Release());

        replaceWithPQServerInstance.Add(sourceTickerQuoteInfo1.Id, moqRegisteredPqLevel0Quote.Object);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);

        pqServer.Publish(stubLevel1Quote);
        moqUpdateService.Verify();
        moqUpdateService.Verify(us => us.Send(moqRegisteredPqLevel0Quote.Object), Times.Never);
        moqUpdateService.SetupGet(us => us.IsStarted).Returns(true).Verifiable();
        stubLevel1Quote.HasUpdates = true;
        pqServer.Publish(stubLevel1Quote);
        moqUpdateService.Verify();
        moqUpdateService.Verify(us => us.Send(moqRegisteredPqLevel0Quote.Object), Times.Once);
    }

    [TestMethod]
    public void StartedRegisteredPQServer_SnapshotRequestReceived_SendsSnapshotsForEachIdRequested()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var moqConvoRequester = new Mock<IConversationRequester>();

        moqConvoRequester.Setup(ssc => ssc.Send(It.IsAny<IVersionedMessage>())).Verifiable();
        var moqlevel0Quote = new Mock<PQLevel0Quote>();

        var moqConversationRequester = new Mock<IConversationRequester>();
        moqSnapshotService.SetupAdd(sss => sss.OnSnapshotRequest += (ssc, id) =>
            moqSnapshotService.Object.Send(moqConversationRequester.Object, moqlevel0Quote.Object));

        var pqServer = new PQServer<PQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        pqServer.Register(TestTicker1);
        pqServer.Register(TestTicker2);
        pqServer.Register(TestTicker3);

        moqSnapshotService.Raise(sss => sss.OnSnapshotRequest += null, moqConvoRequester.Object
            , new PQSnapshotIdsRequest(new[] { sourceTickerQuoteInfo1.Id, sourceTickerQuoteInfo2.Id, sourceTickerQuoteInfo3.Id }));

        moqConvoRequester.Verify();
    }

    [TestMethod]
    public void RegisteredNonEmptyQuotePQServer_Unregister_PublishesEmptyQuote()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel1Quote(info));

        pqServer.StartServices();

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQLevel1Quote>();
        var moqSyncLock = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire());

        var moqRegisteredPqLevel0Quote = new Mock<IPQLevel1Quote>();
        moqRegisteredPqLevel0Quote.SetupGet(l0Q => l0Q.Lock).Returns(moqSyncLock.Object);

        // can't use moq because of property redefinition not handled properly
        var stubLevel1Quote = new PQLevel1QuoteTests.DummyLevel1Quote
        {
            SourceTickerQuoteInfo = sourceTickerQuoteInfo1, HasUpdates = true
        };
        moqRegisteredPqLevel0Quote.Setup(l0Q => l0Q.CopyFrom((ILevel0Quote)stubLevel1Quote, CopyMergeFlags.Default));
        moqRegisteredPqLevel0Quote.SetupProperty(l0Q => l0Q.PQSequenceId, 10u);
        moqSyncLock.Setup(sl => sl.Release());

        replaceWithPQServerInstance.Add(sourceTickerQuoteInfo1.Id, moqRegisteredPqLevel0Quote.Object);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);

        moqUpdateService.SetupGet(us => us.IsStarted).Returns(true);
        moqUpdateService.Setup(us => us.Send(It.IsAny<IPQLevel1Quote>())).Verifiable();

        pqServer.Unregister(stubLevel1Quote);

        moqUpdateService.Verify();
    }

    [TestMethod]
    public void RegisteredNonEmptyQuotePQServer_Unregister_RemovesRegisteredQuote()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel1Quote(info));

        pqServer.StartServices();

        var moqSyncLock = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire());

        var moqRegisteredPqLevel0Quote = new Mock<IPQLevel1Quote>();
        moqRegisteredPqLevel0Quote.SetupGet(l0Q => l0Q.Lock).Returns(moqSyncLock.Object);

        // can't use moq because of property redefinition not handled properly
        var stubLevel1Quote = new PQLevel1QuoteTests.DummyLevel1Quote
        {
            SourceTickerQuoteInfo = sourceTickerQuoteInfo1, HasUpdates = true
        };
        moqRegisteredPqLevel0Quote.Setup(l0Q => l0Q.CopyFrom((ILevel0Quote)stubLevel1Quote, CopyMergeFlags.Default));
        moqRegisteredPqLevel0Quote.SetupProperty(l0Q => l0Q.PQSequenceId, 10u);
        moqSyncLock.Setup(sl => sl.Release());

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQLevel1Quote>
        {
            { sourceTickerQuoteInfo1.Id, moqRegisteredPqLevel0Quote.Object }
        };
        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);

        pqServer.Unregister(stubLevel1Quote);
        Assert.AreEqual(0, replaceWithPQServerInstance.Count);
    }

    [TestMethod]
    public void LastRegisteredQuote_Unregister_StopsStartedHeartBeatingSender()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel1Quote(info));

        pqServer.StartServices();

        var moqSyncLock = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire());

        var moqRegisteredPqLevel0Quote = new Mock<IPQLevel1Quote>();
        moqRegisteredPqLevel0Quote.SetupGet(l0Q => l0Q.Lock).Returns(moqSyncLock.Object);
        // can't use moq because of property redefinition not handled properly
        var stubLevel1Quote = new PQLevel1QuoteTests.DummyLevel1Quote
        {
            SourceTickerQuoteInfo = sourceTickerQuoteInfo1, HasUpdates = true
        };
        moqRegisteredPqLevel0Quote.Setup(l0Q => l0Q.CopyFrom((ILevel0Quote)stubLevel1Quote, CopyMergeFlags.Default));
        moqRegisteredPqLevel0Quote.SetupProperty(l0Q => l0Q.PQSequenceId, 10u);
        moqSyncLock.Setup(sl => sl.Release());

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQLevel1Quote>
        {
            { sourceTickerQuoteInfo1.Id, moqRegisteredPqLevel0Quote.Object }
        };
        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);

        moqHeartBeatSender.SetupGet(hbs => hbs.HasStarted).Returns(true).Verifiable();
        moqHeartBeatSender.Setup(hbs => hbs.StopAndWaitUntilFinished()).Verifiable();

        pqServer.Unregister(stubLevel1Quote);
        moqHeartBeatSender.Verify();
    }

    [TestMethod]
    public void StartedPublishingPQServer_Dispose_StopsUpdateAndSnapshotServerAndDiscards()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel1Quote(info));

        pqServer.StartServices();
        Assert.IsTrue(pqServer.IsStarted);

        moqUpdateService.Verify(us => us.Stop(), Times.Never);
        moqSnapshotService.Verify(ss => ss.Stop(), Times.Never);

        pqServer.Dispose();

        moqUpdateService.Verify(us => us.Stop(), Times.Once);
        moqSnapshotService.Verify(ss => ss.Stop(), Times.Once);
        var currentUpdateServerInstance =
            NonPublicInvocator.GetInstanceField<IPQUpdateServer>(pqServer, "updateServer");
        var currentSnapshotServerInstance =
            NonPublicInvocator.GetInstanceField<IPQSnapshotServer>(pqServer, "snapshotServer");
        Assert.IsNull(currentUpdateServerInstance);
        Assert.IsNull(currentSnapshotServerInstance);
        Assert.IsFalse(pqServer.IsStarted);
    }


    [TestMethod]
    public void StartedPublishingPQServer_Dispose_SyncProtectsClearingThenStopsHeartBeatService()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQLevel1Quote>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory, info => new PQLevel1Quote(info));
        pqServer.StartServices();

        var isInHeartBeatSyncLock = false;
        var moqHeartBeatSyncLock = new Mock<ISyncLock>();
        var moqHeartBeatQuotes = new Mock<IDoublyLinkedList<IPQLevel0Quote>>();
        moqHeartBeatSyncLock.Setup(sl => sl.Acquire())
            .Callback(() => { isInHeartBeatSyncLock = true; }).Verifiable();
        moqHeartBeatSyncLock.Setup(sl => sl.Release())
            .Callback(() => { isInHeartBeatSyncLock = false; }).Verifiable();
        moqHeartBeatQuotes.Setup(rpql0Q => rpql0Q.Clear())
            .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();
        NonPublicInvocator.SetInstanceField(pqServer, "heartBeatSync", moqHeartBeatSyncLock.Object);
        NonPublicInvocator.SetInstanceField(pqServer, "heartbeatQuotes", moqHeartBeatQuotes.Object);

        moqHeartBeatSender.SetupGet(hbs => hbs.HasStarted).Returns(true).Verifiable();
        moqHeartBeatSender.Setup(hbs => hbs.StopAndWaitUntilFinished()).Verifiable();

        pqServer.Dispose();
        moqHeartBeatSyncLock.Verify();
        moqHeartBeatQuotes.Verify();
        moqHeartBeatSender.Verify();
    }

    private void TestLevelQuoteIsReturned<T>() where T : class, IPQLevel0Quote
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<T>(snapshotUpdatePricingServerConfig, moqHeartBeatSender.Object,
            moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);
        pqServer.StartServices();
        var pqLevelTQuote = pqServer.Register(sourceTickerQuoteInfo1.Ticker);
        Assert.IsNotNull(pqLevelTQuote);
    }
}
