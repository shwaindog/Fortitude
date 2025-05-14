// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Publication;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using Moq;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;
using static FortitudeTests.TestEnvironment.TestMachineConfig;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[TestClass]
public class PQServerTests
{
    private const string ExchangeName = "ComponentTestExchange";
    private const ushort ExchangeId   = 1;
    private const string TestTicker1  = "EUR/USD";
    private const string TestTicker2  = "USD/JPY";
    private const string TestTicker3  = "GBP/USD";
    private const ushort TickerId1    = 1;
    private const ushort TickerId2    = 2;
    private const ushort TickerId3    = 3;

    private IMarketConnectionConfig marketConnectionConfig = null!;

    private Mock<IPQServerHeartBeatSender>  moqHeartBeatSender          = null!;
    private Mock<IPQSnapshotServer>         moqSnapshotService          = null!;
    private Mock<ISocketDispatcher>         moqSocketDispatcher         = null!;
    private Mock<ISocketDispatcherListener> moqSocketDispatcherListener = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<ISocketDispatcherSender>   moqSocketDispatcherSender   = null!;
    private Mock<IPQUpdateServer>           moqUpdateService            = null!;

    private Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer> pqSnapshotFactory = null!;
    private Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer>   pqUpdateFactory   = null!;

    private IPricingServerConfig pricingServerConfig = null!;
    private ITickerConfig        sourceTickerConfig1 = null!;
    private ITickerConfig        sourceTickerConfig2 = null!;
    private ITickerConfig        sourceTickerConfig3 = null!;
    private ISourceTickersConfig sourceTickerConfigs = null!;
    private IPQSourceTickerInfo    sourceTickerInfo1   = null!;
    private IPQSourceTickerInfo    sourceTickerInfo2   = null!;
    private IPQSourceTickerInfo    sourceTickerInfo3   = null!;

    public void Setup(LayerFlags layerDetails, LastTradedFlags lastTradedFlags = LastTradedFlags.None)
    {
        sourceTickerConfig1 =
            new TickerConfig
                (TickerId1, TestTicker1, TickerAvailability.PricingAndTradingEnabled, Level3Quote, Unknown
               , 0.00001m, 0.0001m, 0.1m, 100, 0.1m, 250, 10_000
               , layerDetails, 20, lastTradedFlags);
        sourceTickerConfig2 =
            new TickerConfig
                (TickerId2, TestTicker2, TickerAvailability.PricingAndTradingEnabled, Level3Quote, Unknown
               , 0.00001m, 0.0001m, 0.1m, 100, 0.1m, 250, 10_000
               , layerDetails, 20, lastTradedFlags);
        sourceTickerConfig3 =
            new TickerConfig(TickerId3, TestTicker3, TickerAvailability.PricingAndTradingEnabled, Level3Quote, Unknown
                           , 0.00001m, 0.0001m, 0.1m, 100, 0.1m, 250, 10_000
                           , layerDetails, 20, lastTradedFlags);
        sourceTickerConfigs = new SourceTickersConfig(sourceTickerConfig1, sourceTickerConfig2, sourceTickerConfig3);
        pricingServerConfig =
            new PricingServerConfig
                (new NetworkTopicConnectionConfig
                     ("TestSnapshotServer", SocketConversationProtocol.TcpAcceptor
                    , new[]
                      {
                          new EndpointConfig(LoopBackIpAddress, ServerSnapshotPort)
                      })
               , new NetworkTopicConnectionConfig
                     ("TestUpdateServer", SocketConversationProtocol.UdpPublisher
                    , new[]
                      {
                          new EndpointConfig(LoopBackIpAddress, ServerUpdatePort, NetworkSubAddress)
                      }
                    , connectionAttributes: SocketConnectionAttributes.Multicast | SocketConnectionAttributes.Fast));
        marketConnectionConfig
            = new MarketConnectionConfig(ExchangeId, ExchangeName, MarketConnectionType.Pricing, sourceTickerConfigs, pricingServerConfig);
        sourceTickerInfo1 = new PQSourceTickerInfo(marketConnectionConfig.GetSourceTickerInfo(TestTicker1)!);
        sourceTickerInfo2 = new PQSourceTickerInfo(marketConnectionConfig.GetSourceTickerInfo(TestTicker2)!);
        sourceTickerInfo3 = new PQSourceTickerInfo(marketConnectionConfig.GetSourceTickerInfo(TestTicker3)!);

        moqHeartBeatSender          = new Mock<IPQServerHeartBeatSender>();
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqSocketDispatcherListener = new Mock<ISocketDispatcherListener>();
        moqSocketDispatcherSender   = new Mock<ISocketDispatcherSender>();
        moqSocketDispatcher         = new Mock<ISocketDispatcher>();
        moqSocketDispatcherResolver.Setup(sdr => sdr.Resolve(It.IsAny<INetworkTopicConnectionConfig>()))
                                   .Returns(moqSocketDispatcher.Object);
        moqSocketDispatcher.SetupProperty(sd => sd.Listener, moqSocketDispatcherListener.Object);
        moqSocketDispatcher.SetupProperty(sd => sd.Sender, moqSocketDispatcherSender.Object);

        moqSnapshotService = new Mock<IPQSnapshotServer>();
        moqUpdateService   = new Mock<IPQUpdateServer>();
        moqSnapshotService.Setup(sss => sss.Start()).Verifiable();

        moqHeartBeatSender.SetupSet(hbs => hbs.UpdateServer = moqUpdateService.Object).Verifiable();

        pqSnapshotFactory = (_, _) => moqSnapshotService.Object;
        pqUpdateFactory   = (_, _) => moqUpdateService.Object;
    }

    [TestMethod]
    public void NewPQServer_StartServices_ConnectsUpdateServiceAndListensToSnapshotService()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQPublishableTickInstant>
            (marketConnectionConfig, moqHeartBeatSender.Object,
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

        var pqServer = new PQServer<PQPublishableLevel1Quote>(marketConnectionConfig, moqHeartBeatSender.Object,
                                                   moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        moqHeartBeatSender.Verify();
    }

    [TestMethod]
    public void PQServerWithNullQuoteFactory_Register_CreateTickInstantReadyForPublish()
    {
        TestLevelQuoteIsReturned<PQPublishableTickInstant>();
    }

    [TestMethod]
    public void PQServerWithNullQuoteFactory_Register_CreateLevel1QuoteReadyForPublish()
    {
        TestLevelQuoteIsReturned<PQPublishableLevel1Quote>();
    }

    [TestMethod]
    public void PQServerWithNullQuoteFactory_Register_CreateLevel2QuoteReadyForPublish()
    {
        TestLevelQuoteIsReturned<PQPublishableLevel2Quote>();
    }

    [TestMethod]
    public void PQServerWithNullQuoteFactory_Register_CreateLevel3QuoteReadyForPublish()
    {
        TestLevelQuoteIsReturned<PQPublishableLevel3Quote>();
    }

    [TestMethod]
    public void StartedPQServer_Register_PublishesEmptyQuoteToConnectedUpdateServer()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        moqUpdateService.SetupGet(us => us.IsStarted).Returns(true);
        moqUpdateService.Setup(us => us.Send(It.IsAny<PQPublishableLevel1Quote>())).Verifiable();

        pqServer.Register(TestTicker1);

        moqUpdateService.Verify();
    }

    [TestMethod]
    public void StartedPQServer_Register_DoesNotPublishedToUnconnectedUpdateServer()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        moqUpdateService.SetupGet(us => us.IsStarted).Returns(false);

        pqServer.Register(TestTicker1);

        moqUpdateService.Verify();
        moqUpdateService.Verify(us => us.Send(It.IsAny<PQPublishableLevel1Quote>()), Times.Never);
    }

    [TestMethod]
    public void StartedPQServer_Register_StartsUnStartedHeartBeatSender()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<PQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
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

        var pqServer = new PQServer<PQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
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

        var pqServer = new PQServer<PQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        var quoteToSubmit = pqServer.Register("UNK/OWN");

        Assert.IsNull(quoteToSubmit);
    }

    [TestMethod]
    public void RegisteredPQServer_Publish_ProtectsUpdatesToQuoteBehindQuoteSyncLock()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQPublishableTickInstant>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableTickInstant(info));

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQPublishableTickInstant>();
        var isInQuoteSyncLock           = false;
        var moqSyncLock                 = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire()).Callback(() => { isInQuoteSyncLock = true; }).Verifiable();

        var moqRegisteredPqTickInstant = new Mock<IPQPublishableTickInstant>();
        var moqTickInstantAsPqMutMessage        = moqRegisteredPqTickInstant.As<IPQMutableMessage>();
        moqTickInstantAsPqMutMessage.SetupGet(l1Q => l1Q.Lock).Returns(moqSyncLock.Object);
        moqRegisteredPqTickInstant.SetupProperty(ti => ti.PQSequenceId, 10u);

        // can't use moq because of property redefinition not handled properly
        var stubTickInstant = new PQTickInstantTests.DummyPQTickInstant
        {
            SourceTickerInfo = sourceTickerInfo1, HasUpdates = true
        };
        moqTickInstantAsPqMutMessage.Setup(ti => ti.CopyFrom(stubTickInstant, CopyMergeFlags.Default))
                                    .Callback(() => { Assert.IsTrue(isInQuoteSyncLock); }).Returns(moqRegisteredPqTickInstant.Object)
                                    .Verifiable();
        moqTickInstantAsPqMutMessage.SetupSet(ti => ti.PQSequenceId = 10)
                                    .Callback(() => { Assert.IsTrue(isInQuoteSyncLock); }).Verifiable();
        moqSyncLock.Setup(sl => sl.Release()).Callback(() => { isInQuoteSyncLock = false; }).Verifiable();

        replaceWithPQServerInstance.Add(sourceTickerInfo1.SourceTickerId, moqRegisteredPqTickInstant.Object);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);

        Assert.IsFalse(isInQuoteSyncLock);
        pqServer.Publish(stubTickInstant);
        Assert.IsFalse(isInQuoteSyncLock);
        Assert.IsFalse(stubTickInstant.HasUpdates);
        moqRegisteredPqTickInstant.Verify();
        moqSyncLock.Verify();
    }

    [TestMethod]
    public void RegisteredPQServer_Publish_ReordersPublishedQuoteToEndOfHeartBeats()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQPublishableTickInstant>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableTickInstant(info));

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQPublishableTickInstant>();
        var isInHeartBeatSyncLock       = false;
        var moqQuoteSyncLock            = new Mock<ISyncLock>();
        moqQuoteSyncLock.Setup(sl => sl.Acquire());
        var ticker1RegisteredPqTickInstant = new PQPublishableTickInstant(sourceTickerInfo1);

        // can't use moq because of property hiding/redefinition not handled properly
        var stubLevel1Quote = new PQTickInstantTests.DummyPQTickInstant
        {
            SourceTickerInfo = sourceTickerInfo1, HasUpdates = true
        };
        moqQuoteSyncLock.Setup(sl => sl.Release());

        replaceWithPQServerInstance.Add(sourceTickerInfo1.SourceTickerId, ticker1RegisteredPqTickInstant);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);
        var moqHeartBeatSyncLock = new Mock<ISyncLock>();

        moqHeartBeatSyncLock.Setup(sl => sl.Acquire())
                            .Callback(() => { isInHeartBeatSyncLock = true; }).Verifiable();
        moqHeartBeatSyncLock.Setup(sl => sl.Release())
                            .Callback(() => { isInHeartBeatSyncLock = false; }).Verifiable();
        NonPublicInvocator.SetInstanceField(pqServer, "heartBeatSync", moqHeartBeatSyncLock.Object);

        var heartBeatQuotes        = new DoublyLinkedList<IPQMutableMessage>();
        var initialLastLvl1Quote   = new PQTickInstantTests.DummyPQTickInstant();
        var initialMiddleLvl1Quote = new PQTickInstantTests.DummyPQTickInstant();
        heartBeatQuotes.AddLast(initialLastLvl1Quote);
        heartBeatQuotes.AddFirst(initialMiddleLvl1Quote);
        heartBeatQuotes.AddFirst(ticker1RegisteredPqTickInstant);
        NonPublicInvocator.SetInstanceField(pqServer, "heartbeatQuotes", heartBeatQuotes);

        Assert.IsFalse(isInHeartBeatSyncLock);
        pqServer.Publish(stubLevel1Quote);
        Assert.IsFalse(isInHeartBeatSyncLock);
        moqHeartBeatSyncLock.Verify();
        Assert.AreSame(initialMiddleLvl1Quote, heartBeatQuotes.Head);
        Assert.AreSame(ticker1RegisteredPqTickInstant, heartBeatQuotes.Tail);
    }

    [TestMethod]
    public void RegisteredPQServer_Publish_ProtectsReorderedHeartBeat()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQPublishableTickInstant>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableTickInstant(info));

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQPublishableTickInstant>();
        var moqQuoteSyncLock            = new Mock<ISyncLock>();
        moqQuoteSyncLock.Setup(sl => sl.Acquire());
        var moqRegisteredPqTickInstant   = new Mock<IPQPublishableTickInstant>();
        var moqTickInstantAsPqMutMessage = moqRegisteredPqTickInstant.As<IPQMutableMessage>();
        moqTickInstantAsPqMutMessage.SetupGet(l1Q => l1Q.Lock).Returns(moqQuoteSyncLock.Object).Verifiable();
        moqRegisteredPqTickInstant.SetupProperty(ti => ti.PQSequenceId, 10u);

        // can't use moq because of property hiding/redefinition not handled properly
        var stubLevel1Quote = new PQTickInstantTests.DummyPQTickInstant
        {
            SourceTickerInfo = sourceTickerInfo1, HasUpdates = true
        };
        moqRegisteredPqTickInstant.Setup(ti => ti.CopyFrom((IPublishableTickInstant)stubLevel1Quote, CopyMergeFlags.Default));
        moqQuoteSyncLock.Setup(sl => sl.Release());

        replaceWithPQServerInstance.Add(sourceTickerInfo1.SourceTickerId, moqRegisteredPqTickInstant.Object);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);
        var isInHeartBeatSyncLock = false;
        var moqHeartBeatSyncLock  = new Mock<ISyncLock>();
        moqHeartBeatSyncLock.Setup(sl => sl.Acquire())
                            .Callback(() => { isInHeartBeatSyncLock = true; }).Verifiable();
        moqHeartBeatSyncLock.Setup(sl => sl.Release())
                            .Callback(() => { isInHeartBeatSyncLock = false; }).Verifiable();
        moqTickInstantAsPqMutMessage.SetupSet(ti => ti.LastPublicationTime = It.IsAny<DateTime>())
                                    .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();
        NonPublicInvocator.SetInstanceField(pqServer, "heartBeatSync", moqHeartBeatSyncLock.Object);

        var moqHeartBeatQuotes = new Mock<IDoublyLinkedList<IPQMutableMessage>>();
        moqHeartBeatQuotes.Setup(hbq => hbq.Remove(moqRegisteredPqTickInstant.Object))
                          .Callback(() => { Assert.IsTrue(isInHeartBeatSyncLock); })
                          .Returns(moqRegisteredPqTickInstant.Object).Verifiable();
        moqHeartBeatQuotes.Setup(hbq => hbq.AddLast(moqRegisteredPqTickInstant.Object))
                          .Callback(() => { Assert.IsTrue(isInHeartBeatSyncLock); })
                          .Returns(moqRegisteredPqTickInstant.Object).Verifiable();
        NonPublicInvocator.SetInstanceField(pqServer, "heartbeatQuotes", moqHeartBeatQuotes.Object);

        Assert.IsFalse(isInHeartBeatSyncLock);
        pqServer.Publish(stubLevel1Quote);
        Assert.IsFalse(isInHeartBeatSyncLock);
        moqHeartBeatSyncLock.Verify();
        moqRegisteredPqTickInstant.Verify();
        moqHeartBeatQuotes.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PQServer_PublishUnregisteredQuote_ExpectArgumentException()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQPublishableTickInstant>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableTickInstant(info));


        // can't use moq because of property hiding/redefinition not handled properly
        var stubLevel1Quote = new PQTickInstantTests.DummyPQTickInstant
        {
            SourceTickerInfo = sourceTickerInfo1, HasUpdates = true
        };
        pqServer.Publish(stubLevel1Quote);
    }

    [TestMethod]
    public void RegisteredPQServerDisconnectedUpdateServer_Publish_DoesNotSendReconnectDoesSend()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        moqUpdateService.SetupGet(us => us.IsStarted).Returns(false).Verifiable();

        var pqServer = new PQServer<IPQPublishableTickInstant>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableTickInstant(info));

        pqServer.StartServices();

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQPublishableTickInstant>();
        var moqSyncLock                 = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire());

        var moqRegisteredPqTickInstant   = new Mock<IPQPublishableTickInstant>();
        var moqTickInstantAsPqMutMessage = moqRegisteredPqTickInstant.As<IPQMutableMessage>();
        moqTickInstantAsPqMutMessage.SetupGet(l1Q => l1Q.Lock).Returns(moqSyncLock.Object);

        // can't use moq because of property redefinition not handled properly
        var stubLevel1Quote = new PQTickInstantTests.DummyPQTickInstant
        {
            SourceTickerInfo = sourceTickerInfo1, HasUpdates = true
        };
        moqRegisteredPqTickInstant.Setup(ti => ti.CopyFrom((IPublishableTickInstant)stubLevel1Quote, CopyMergeFlags.Default));
        moqRegisteredPqTickInstant.SetupProperty(ti => ti.PQSequenceId, 10u);
        moqRegisteredPqTickInstant.SetupProperty(ti => ti.HasUpdates, true);
        moqSyncLock.Setup(sl => sl.Release());

        replaceWithPQServerInstance.Add(sourceTickerInfo1.SourceTickerId, moqRegisteredPqTickInstant.Object);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);

        pqServer.Publish(stubLevel1Quote);
        moqUpdateService.Verify();
        moqUpdateService.Verify(us => us.Send(moqRegisteredPqTickInstant.Object), Times.Never);
        moqUpdateService.SetupGet(us => us.IsStarted).Returns(true).Verifiable();
        stubLevel1Quote.HasUpdates = true;
        pqServer.Publish(stubLevel1Quote);
        moqUpdateService.Verify();
        moqUpdateService.Verify(us => us.Send(moqRegisteredPqTickInstant.Object), Times.Once);
    }

    [TestMethod]
    public void StartedRegisteredPQServer_SnapshotRequestReceived_SendsSnapshotsForEachIdRequested()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var moqConvoRequester = new Mock<IConversationRequester>();

        moqConvoRequester.Setup(ssc => ssc.Send(It.IsAny<IVersionedMessage>())).Verifiable();
        var moqTickInstant = new Mock<PQPublishableTickInstant>();

        var moqConversationRequester = new Mock<IConversationRequester>();
        moqSnapshotService.SetupAdd(sss => sss.OnSnapshotRequest += (ssc, id) =>
                                        moqSnapshotService.Object.Send(moqConversationRequester.Object, moqTickInstant.Object));

        var pqServer = new PQServer<PQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);

        pqServer.StartServices();

        pqServer.Register(TestTicker1);
        pqServer.Register(TestTicker2);
        pqServer.Register(TestTicker3);

        moqSnapshotService.Raise
            (sss => sss.OnSnapshotRequest += null, moqConvoRequester.Object
           , new PQSnapshotIdsRequest(new[]
             {
                 sourceTickerInfo1.SourceTickerId, sourceTickerInfo2.SourceTickerId
               , sourceTickerInfo3.SourceTickerId
             }));

        moqConvoRequester.Verify();
    }

    [TestMethod]
    public void RegisteredNonEmptyQuotePQServer_Unregister_PublishesEmptyQuote()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableLevel1Quote(info));

        pqServer.StartServices();

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQPublishableLevel1Quote>();
        var moqSyncLock                 = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire());

        var moqRegisteredPqLevel1Quote = new Mock<IPQPublishableLevel1Quote>();
        var moqL1AsPqMutMessage        = moqRegisteredPqLevel1Quote.As<IPQMutableMessage>();
        moqL1AsPqMutMessage.SetupGet(l1Q => l1Q.Lock).Returns(moqSyncLock.Object);

        // can't use moq because of property redefinition not handled properly
        var stubLevel1Quote = new PQLevel1QuoteTests.DummyLevel1Quote
        {
            SourceTickerInfo = sourceTickerInfo1, HasUpdates = true, SingleTickValue = 123, IsSingleValueUpdated = true
        };
        moqRegisteredPqLevel1Quote.Setup(l1Q => l1Q.CopyFrom((IPublishableTickInstant)stubLevel1Quote, CopyMergeFlags.Default));
        moqRegisteredPqLevel1Quote.SetupProperty(l1Q => l1Q.PQSequenceId, 10u);
        moqRegisteredPqLevel1Quote.SetupProperty(l1Q => l1Q.HasUpdates, true);
        moqSyncLock.Setup(sl => sl.Release());

        replaceWithPQServerInstance.Add(sourceTickerInfo1.SourceTickerId, moqRegisteredPqLevel1Quote.Object);

        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);

        moqUpdateService.SetupGet(us => us.IsStarted).Returns(true);
        moqUpdateService.Setup(us => us.Send(It.IsAny<IPQPublishableLevel1Quote>())).Verifiable();

        pqServer.Unregister(stubLevel1Quote);

        moqUpdateService.Verify();
    }

    [TestMethod]
    public void RegisteredNonEmptyQuotePQServer_Unregister_RemovesRegisteredQuote()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableLevel1Quote(info));

        pqServer.StartServices();

        var moqSyncLock = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire());

        var moqRegisteredPqLevel1Quote = new Mock<IPQPublishableLevel1Quote>();
        var moqL1AsPqMutMessage        = moqRegisteredPqLevel1Quote.As<IPQMutableMessage>();
        moqL1AsPqMutMessage.SetupGet(l1Q => l1Q.Lock).Returns(moqSyncLock.Object);

        // can't use moq because of property redefinition not handled properly
        var stubLevel1Quote = new PQLevel1QuoteTests.DummyLevel1Quote
        {
            SourceTickerInfo = sourceTickerInfo1, HasUpdates = true
        };
        moqRegisteredPqLevel1Quote.Setup(l1Q => l1Q.CopyFrom((IPublishableTickInstant)stubLevel1Quote, CopyMergeFlags.Default));
        moqRegisteredPqLevel1Quote.SetupProperty(l1Q => l1Q.PQSequenceId, 10u);
        moqSyncLock.Setup(sl => sl.Release());

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQPublishableLevel1Quote>
        {
            { sourceTickerInfo1.SourceTickerId, moqRegisteredPqLevel1Quote.Object }
        };
        NonPublicInvocator.SetInstanceField(pqServer, "entities", replaceWithPQServerInstance);

        pqServer.Unregister(stubLevel1Quote);
        Assert.AreEqual(0, replaceWithPQServerInstance.Count);
    }

    [TestMethod]
    public void LastRegisteredQuote_Unregister_StopsStartedHeartBeatingSender()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<IPQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableLevel1Quote(info));

        pqServer.StartServices();

        var moqSyncLock = new Mock<ISyncLock>();
        moqSyncLock.Setup(sl => sl.Acquire());

        var moqRegisteredPqLevel1Quote = new Mock<IPQPublishableLevel1Quote>();
        var moqL1AsPqMutMessage        = moqRegisteredPqLevel1Quote.As<IPQMutableMessage>();
        moqL1AsPqMutMessage.SetupGet(l1Q => l1Q.Lock).Returns(moqSyncLock.Object);
        // can't use moq because of property redefinition not handled properly
        var stubLevel1Quote = new PQLevel1QuoteTests.DummyLevel1Quote
        {
            SourceTickerInfo = sourceTickerInfo1, HasUpdates = true
        };
        moqRegisteredPqLevel1Quote.Setup(l1Q => l1Q.CopyFrom((IPublishableTickInstant)stubLevel1Quote, CopyMergeFlags.Default));
        moqRegisteredPqLevel1Quote.SetupProperty(l1Q => l1Q.PQSequenceId, 10u);
        moqSyncLock.Setup(sl => sl.Release());

        var replaceWithPQServerInstance = new ConcurrentMap<uint, IPQPublishableLevel1Quote>
        {
            { sourceTickerInfo1.SourceTickerId, moqRegisteredPqLevel1Quote.Object }
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

        var pqServer = new PQServer<IPQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableLevel1Quote(info));

        pqServer.StartServices();
        Assert.IsTrue(pqServer.IsStarted);

        moqUpdateService.Verify(us => us.Stop(It.IsAny<CloseReason>(), It.IsAny<string?>()), Times.Never);
        moqSnapshotService.Verify(ss => ss.Stop(It.IsAny<CloseReason>(), It.IsAny<string?>()), Times.Never);

        pqServer.Dispose();

        moqUpdateService.Verify(us => us.Stop(It.IsAny<CloseReason>(), It.IsAny<string?>()), Times.Once);
        moqSnapshotService.Verify(ss => ss.Stop(It.IsAny<CloseReason>(), It.IsAny<string?>()), Times.Once);
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

        var pqServer = new PQServer<IPQPublishableLevel1Quote>
            (marketConnectionConfig, moqHeartBeatSender.Object,
             moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory
           , info => new PQPublishableLevel1Quote(info));
        pqServer.StartServices();

        var isInHeartBeatSyncLock = false;
        var moqHeartBeatSyncLock  = new Mock<ISyncLock>();
        var moqHeartBeatQuotes    = new Mock<IDoublyLinkedList<IPQMutableMessage>>();
        moqHeartBeatSyncLock.Setup(sl => sl.Acquire())
                            .Callback(() => { isInHeartBeatSyncLock = true; }).Verifiable();
        moqHeartBeatSyncLock.Setup(sl => sl.Release())
                            .Callback(() => { isInHeartBeatSyncLock = false; }).Verifiable();
        moqHeartBeatQuotes.Setup(pqti => pqti.Clear())
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

    private void TestLevelQuoteIsReturned<T>() where T : class, IPQPublishableTickInstant
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServer = new PQServer<T>(marketConnectionConfig, moqHeartBeatSender.Object,
                                       moqSocketDispatcherResolver.Object, pqSnapshotFactory, pqUpdateFactory);
        pqServer.StartServices();
        var pqLevelTQuote = pqServer.Register(sourceTickerConfig1.Ticker!);
        Assert.IsNotNull(pqLevelTQuote);
    }
}
