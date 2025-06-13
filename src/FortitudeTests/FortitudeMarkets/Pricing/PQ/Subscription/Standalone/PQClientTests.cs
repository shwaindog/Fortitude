// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarkets.Config;
using FortitudeMarkets.Config.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Subscription.Standalone;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

[TestClass]
public class PQClientTests
{
    private const ushort FirstSourceId        = 123;
    private const ushort FirstTickerId        = 456;
    private const ushort SecondSourceId       = 234;
    private const ushort SecondTickerId       = 567;
    private const uint   FirstSourceTickerId  = ((uint)FirstSourceId << 16) | FirstTickerId;
    private const uint   SecondSourceTickerId = ((uint)SecondSourceId << 16) | SecondTickerId;

    private readonly uint   defaultSyncRetryInterval = 60000;
    private readonly string firstTestSourceName      = "FirstTestSourceName";
    private readonly string firstTestTicker          = "FirstTestTicker";
    private readonly string secondTestSourceName     = "SecondTestSourceName";
    private readonly string secondTestTicker         = "SecondTestTicker";

    private PQPublishableLevel1Quote dummyLevel1Quote = null!;
    private PQPublishableLevel2Quote dummyLevel2Quote = null!;
    private PQPublishableLevel3Quote dummyLevel3Quote = null!;

    private PQPublishableTickInstant dummyTickInstant = null!;

    private Mock<IDisposable>                  moqDeserializerSubscription    = null!;
    private Mock<IMarketConnectionConfig>      moqFirstMarketConnectionConfig = null!;
    private Mock<IPricingServerConfig>         moqFirstPricingServerConfig    = null!;
    private Mock<ISocketDispatcher>            moqFirstSocketDispatcher       = null!;
    private Mock<ISourceTickerInfo>            moqFirstTestSourceTickerInfo   = null!;
    private Mock<IMarketsConfig>               moqMarketsConfig               = null!;
    private Mock<IOSParallelController>        moqParallelController          = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory   = null!;
    private Mock<IPQClientSyncMonitoring>      moqPQClientSyncMonitoring      = null!;

    private Mock<IPQMessageDeserializer<PQPublishableLevel1Quote>>  moqPQLvl1QuoteDeserializer = null!;
    private Mock<IPQMessageDeserializer<PQPublishableLevel2Quote>>  moqPQLvl2QuoteDeserializer = null!;
    private Mock<IPQMessageDeserializer<PQPublishableLevel3Quote>>  moqPQLvl3QuoteDeserializer = null!;
    private Mock<IPQClientQuoteDeserializerRepository> moqPQQuoteDeserializerRepo = null!;

    private Mock<IPQMessageDeserializer> moqPQQuoteSerializer = null!;

    private Mock<IPQMessageDeserializer<PQPublishableTickInstant>> moqPQTickInstantDeserializer    = null!;
    private Mock<IMarketConnectionConfig>             moqSecondMarketConnectionConfig = null!;
    private Mock<IPricingServerConfig>                moqSecondPricingServerConfig    = null!;
    private Mock<ISourceTickerInfo>                   moqSecondTestSourceTickerInfo   = null!;
    private Mock<IIntraOSThreadSignal>                moqSingleOsThreadSignal         = null!;
    private Mock<IPQSnapshotClient>                   moqSnapshotClient               = null!;

    private Mock<IPQConversationRepository<IPQSnapshotClient>> moqSnapshotClientFactory = null!;

    private Mock<IEnumerator<IEndpointConfig>>  moqSnapshotEndpointEnumerator = null!;
    private Mock<IEndpointConfig>               moqSnapshotServerConfig       = null!;
    private Mock<INetworkTopicConnectionConfig> moqSnapshotTopicServerConfig  = null!;
    private Mock<ISocketDispatcherResolver>     moqSocketDispatcherResolver   = null!;
    private Mock<IPQUpdateClient>               moqUpdateClient               = null!;

    private Mock<IPQConversationRepository<IPQUpdateClient>> moqUpdateClientFactory = null!;

    private Mock<IEnumerator<IEndpointConfig>>  moqUpdateEndpointEnumerator = null!;
    private Mock<IEndpointConfig>               moqUpdateServerConfig       = null!;
    private Mock<INetworkTopicConnectionConfig> moqUpdateTopicServerConfig  = null!;
    private PQClient                            pqClient                    = null!;

    [TestInitialize]
    public void SetUp()
    {
        PrepareNewPQClientInitializationMocks();

        PrepareQuoteLevelMocks();

        PrepareGetQuoteStreamMocks();
        pqClient = new PQClient(moqMarketsConfig.Object, moqSocketDispatcherResolver.Object,
                                moqUpdateClientFactory.Object, moqSnapshotClientFactory.Object);

        NonPublicInvocator.SetInstanceField(pqClient, "pqClientSyncMonitoring", moqPQClientSyncMonitoring.Object);
        NonPublicInvocator.SetInstanceField
            (pqClient, "deserializationRepository", moqPQQuoteDeserializerRepo.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    private void PrepareNewPQClientInitializationMocks()
    {
        moqSingleOsThreadSignal     = new Mock<IIntraOSThreadSignal>();
        moqParallelController       = new Mock<IOSParallelController>();
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqParallelController.Setup(pc => pc.SingleOSThreadActivateSignal(false)).Returns(
                                                                                          moqSingleOsThreadSignal.Object);
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
                                    .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;


        moqFirstTestSourceTickerInfo = new Mock<ISourceTickerInfo>();
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.FilledAttributes).Returns([]);
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.SourceName).Returns(firstTestSourceName);
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.InstrumentName).Returns(firstTestTicker);
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.SourceInstrumentId).Returns(FirstSourceTickerId);
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.SourceId).Returns(FirstSourceId);
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.InstrumentId).Returns(FirstTickerId);

        moqSecondTestSourceTickerInfo = new Mock<ISourceTickerInfo>();

        moqSecondTestSourceTickerInfo.SetupGet(stqi => stqi.SourceName).Returns(secondTestSourceName);
        moqSecondTestSourceTickerInfo.SetupGet(stqi => stqi.InstrumentName).Returns(secondTestTicker);
        moqSecondTestSourceTickerInfo.SetupGet(stqi => stqi.SourceInstrumentId).Returns(SecondSourceTickerId);
        moqSecondTestSourceTickerInfo.SetupGet(stqi => stqi.SourceId).Returns(SecondSourceId);
        moqSecondTestSourceTickerInfo.SetupGet(stqi => stqi.InstrumentId).Returns(SecondTickerId);


        moqFirstMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqFirstPricingServerConfig    = new Mock<IPricingServerConfig>();
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.PricingServerConfig).Returns(moqFirstPricingServerConfig.Object);
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.SourceId).Returns(FirstSourceId);
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.SourceName).Returns(firstTestSourceName);
        moqFirstMarketConnectionConfig.Setup(mcc => mcc.GetSourceTickerInfo(firstTestTicker)).Returns(moqFirstTestSourceTickerInfo.Object);
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.AllSourceTickerInfos).Returns(new[] { moqFirstTestSourceTickerInfo.Object });

        moqSecondMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqSecondPricingServerConfig    = new Mock<IPricingServerConfig>();
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.PricingServerConfig).Returns(moqSecondPricingServerConfig.Object);
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.SourceId).Returns(SecondSourceId);
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.SourceName).Returns(secondTestSourceName);
        moqSecondMarketConnectionConfig.Setup(mcc => mcc.GetSourceTickerInfo(secondTestTicker)).Returns(moqSecondTestSourceTickerInfo.Object);
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.AllSourceTickerInfos).Returns(new[] { moqSecondTestSourceTickerInfo.Object });


        moqSnapshotClientFactory = new Mock<IPQConversationRepository<IPQSnapshotClient>>();
        moqSnapshotClient        = new Mock<IPQSnapshotClient>();
        moqSnapshotClientFactory
            .Setup(scf => scf.RetrieveOrCreateConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSnapshotClient.Object);
        moqSnapshotClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
                                .Returns(moqSnapshotClient.Object);

        moqMarketsConfig = new Mock<IMarketsConfig>();
        moqMarketsConfig.Setup(mccr => mccr.Find(firstTestSourceName))
                        .Returns(moqFirstMarketConnectionConfig.Object);
        moqMarketsConfig.Setup(mccr => mccr.Find(secondTestSourceName))
                        .Returns(moqSecondMarketConnectionConfig.Object);
        var serverTickerConfigs = new Dictionary<string, IMarketConnectionConfig>
        {
            { firstTestSourceName, moqFirstMarketConnectionConfig.Object }, { secondTestSourceName, moqSecondMarketConnectionConfig.Object }
        };
        moqMarketsConfig.SetupGet(mccr => mccr.Markets)
                        .Returns(serverTickerConfigs);

        moqUpdateClientFactory = new Mock<IPQConversationRepository<IPQUpdateClient>>();
        moqUpdateClient        = new Mock<IPQUpdateClient>();
        moqUpdateClientFactory.Setup(scf => scf.RetrieveOrCreateConversation(It.IsAny<INetworkTopicConnectionConfig>()))
                              .Returns(moqUpdateClient.Object);
        moqUpdateClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
                              .Returns(moqUpdateClient.Object);

        moqFirstSocketDispatcher = new Mock<ISocketDispatcher>();
        moqFirstSocketDispatcher.SetupAllProperties();
    }

    private void PrepareGetQuoteStreamMocks()
    {
        moqPQClientSyncMonitoring = new Mock<IPQClientSyncMonitoring>();
        moqPQClientSyncMonitoring.Setup(csm => csm.RegisterNewDeserializer(It.IsAny<IPQMessageDeserializer>()))
                                 .Verifiable();
        moqPQClientSyncMonitoring.Setup(csm => csm.CheckStartMonitoring()).Verifiable();

        moqUpdateServerConfig       = new Mock<IEndpointConfig>();
        moqUpdateTopicServerConfig  = new Mock<INetworkTopicConnectionConfig>();
        moqUpdateEndpointEnumerator = new Mock<IEnumerator<IEndpointConfig>>();
        moqUpdateEndpointEnumerator.SetupGet(uee => uee.Current).Returns(moqUpdateServerConfig.Object);
        moqUpdateTopicServerConfig.Setup(uee => uee.GetEnumerator()).Returns(moqUpdateEndpointEnumerator.Object);
        moqUpdateServerConfig.SetupGet(scc => scc.SubnetMask).Returns("123.0.0.123").Verifiable();
        moqUpdateServerConfig.SetupGet(scc => scc.Hostname).Returns("UpdateHostName").Verifiable();
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.SourceName).Returns("TestServerName" as string).Verifiable();

        moqSnapshotServerConfig       = new Mock<IEndpointConfig>();
        moqSnapshotTopicServerConfig  = new Mock<INetworkTopicConnectionConfig>();
        moqSnapshotEndpointEnumerator = new Mock<IEnumerator<IEndpointConfig>>();
        moqSnapshotEndpointEnumerator.SetupGet(uee => uee.Current).Returns(moqSnapshotServerConfig.Object);
        moqSnapshotServerConfig.SetupGet(scc => scc.Hostname).Returns("SnapshotHostName");
        moqFirstPricingServerConfig.SetupGet(psc => psc.SnapshotConnectionConfig)
                                   .Returns(moqSnapshotTopicServerConfig.Object).Verifiable();
        moqFirstPricingServerConfig.SetupGet(psc => psc.UpdateConnectionConfig)
                                   .Returns(moqUpdateTopicServerConfig.Object);
    }

    private void PrepareQuoteLevelMocks()
    {
        dummyTickInstant = new PQPublishableTickInstant(moqFirstTestSourceTickerInfo.Object);
        dummyLevel1Quote = new PQPublishableLevel1Quote(moqFirstTestSourceTickerInfo.Object);
        dummyLevel2Quote = new PQPublishableLevel2Quote(moqFirstTestSourceTickerInfo.Object);
        dummyLevel3Quote = new PQPublishableLevel3Quote(moqFirstTestSourceTickerInfo.Object);

        moqPQQuoteSerializer         = new Mock<IPQMessageDeserializer>();
        moqPQTickInstantDeserializer = moqPQQuoteSerializer.As<IPQMessageDeserializer<PQPublishableTickInstant>>();
        moqPQLvl1QuoteDeserializer   = moqPQQuoteSerializer.As<IPQMessageDeserializer<PQPublishableLevel1Quote>>();
        moqPQLvl2QuoteDeserializer   = moqPQQuoteSerializer.As<IPQMessageDeserializer<PQPublishableLevel2Quote>>();
        moqPQLvl3QuoteDeserializer   = moqPQQuoteSerializer.As<IPQMessageDeserializer<PQPublishableLevel3Quote>>();
        moqDeserializerSubscription  = new Mock<IDisposable>();
        moqPQTickInstantDeserializer.Setup(pqti =>
                                               pqti.Subscribe(It.IsAny<PQTickerFeedSubscriptionQuoteStream<PQPublishableTickInstant>>()))
                                    .Returns(moqDeserializerSubscription.Object).Verifiable();
        moqPQLvl1QuoteDeserializer.Setup(pql1Qs =>
                                             pql1Qs.Subscribe(It.IsAny<PQTickerFeedSubscriptionQuoteStream<PQPublishableLevel1Quote>>()))
                                  .Returns(moqDeserializerSubscription.Object).Verifiable();
        moqPQLvl2QuoteDeserializer.Setup(pql2Qs =>
                                             pql2Qs.Subscribe(It.IsAny<PQTickerFeedSubscriptionQuoteStream<PQPublishableLevel2Quote>>()))
                                  .Returns(moqDeserializerSubscription.Object).Verifiable();
        moqPQLvl3QuoteDeserializer.Setup(pql3Qs =>
                                             pql3Qs.Subscribe(It.IsAny<PQTickerFeedSubscriptionQuoteStream<PQPublishableLevel3Quote>>()))
                                  .Returns(moqDeserializerSubscription.Object).Verifiable();

        moqPQQuoteDeserializerRepo = new Mock<IPQClientQuoteDeserializerRepository>();
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.CreateQuoteDeserializer<PQPublishableTickInstant>(
                                                                                           It.IsAny<ITickerPricingSubscriptionConfig>()))
                                  .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.CreateQuoteDeserializer<PQPublishableLevel1Quote>(
                                                                                           It.IsAny<ITickerPricingSubscriptionConfig>()))
                                  .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.CreateQuoteDeserializer<PQPublishableLevel2Quote>(
                                                                                           It.IsAny<ITickerPricingSubscriptionConfig>()))
                                  .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.CreateQuoteDeserializer<PQPublishableLevel3Quote>(
                                                                                           It.IsAny<ITickerPricingSubscriptionConfig>()))
                                  .Returns(moqPQQuoteSerializer.Object);
        moqUpdateClient.SetupGet(sc => sc.DeserializerRepository).Returns(moqPQQuoteDeserializerRepo.Object);
        moqSnapshotClient.SetupGet(sc => sc.DeserializerRepository).Returns(moqPQQuoteDeserializerRepo.Object);

        moqPQTickInstantDeserializer.SetupGet(pqti => pqti.PublishedQuote).Returns(dummyTickInstant).Verifiable();
        moqPQLvl1QuoteDeserializer.SetupGet(pql1Qs => pql1Qs.PublishedQuote).Returns(dummyLevel1Quote).Verifiable();
        moqPQLvl2QuoteDeserializer.SetupGet(pql2Qs => pql2Qs.PublishedQuote).Returns(dummyLevel2Quote).Verifiable();
        moqPQLvl3QuoteDeserializer.SetupGet(pql3Qs => pql3Qs.PublishedQuote).Returns(dummyLevel3Quote).Verifiable();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMulticastTickInstant_RegistersAndReturnsTickInstant()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableTickInstant>();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMulticastLevel1Quote_RegistersAndReturnsQuote()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableLevel1Quote>();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMulticastLevel2Quote_RegistersAndReturnsQuote()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableLevel2Quote>();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMulticastLevel3Quote_RegistersAndReturnsQuote()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableLevel3Quote>();
    }

    private IPQTickerFeedSubscriptionQuoteStream<T>
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<T>
        (
            Mock<ISourceTickerInfo>? sourceTickerInfo = null) where T : PQPublishableTickInstant, new()
    {
        sourceTickerInfo ??= moqFirstTestSourceTickerInfo;
        var result = pqClient.GetQuoteStream<T>(sourceTickerInfo.Object, defaultSyncRetryInterval);

        Assert.IsNotNull(result);
        sourceTickerInfo.Verify();
        moqMarketsConfig.Verify();
        moqPQQuoteDeserializerRepo.Verify();
        moqPQClientSyncMonitoring.Verify();
        moqFirstSocketDispatcher.Verify();
        moqUpdateClientFactory.Verify();
        moqSnapshotClientFactory.Verify();

        return result;
    }

    [TestMethod]
    public void SubscribedTickInstantStream_Unsubscribe_RegistersAndReturnsTickInstant()
    {
        SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<PQPublishableTickInstant>();
    }

    [TestMethod]
    public void SubscribedLevel1QuoteStream_Unsubscribe_RegistersAndReturnsQuote()
    {
        SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<PQPublishableLevel1Quote>();
    }

    [TestMethod]
    public void SubscribedLevel2QuoteStream_Unsubscribe_RegistersAndReturnsQuote()
    {
        SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<PQPublishableLevel2Quote>();
    }

    [TestMethod]
    public void SubscribedLevel3QuoteStream_Unsubscribe_RegistersAndReturnsQuote()
    {
        SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<PQPublishableLevel3Quote>();
    }

    public void SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<T>() where T : PQPublishableTickInstant, new()
    {
        var subQuoteStream
            = pqClient.GetQuoteStream<T>(moqFirstTestSourceTickerInfo.Object, defaultSyncRetryInterval);

        PrepareUnsubscribeMocks();

        subQuoteStream!.Unsubscribe();

        moqPQQuoteDeserializerRepo.Verify();
        moqUpdateClientFactory.Verify();
        moqSnapshotClientFactory.Verify();
        moqPQClientSyncMonitoring.Verify();
    }

    private void PrepareUnsubscribeMocks()
    {
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.GetDeserializer(moqFirstTestSourceTickerInfo.Object))
                                  .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.GetDeserializer(moqFirstTestSourceTickerInfo.Object))
                                  .Returns(moqPQQuoteSerializer.Object);

        moqUpdateClientFactory.Setup(cr => cr.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
                              .Verifiable();
        moqSnapshotClientFactory.Setup(cr => cr.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
                                .Verifiable();
        moqPQClientSyncMonitoring.Setup(cr => cr.UnregisterSerializer(It.IsAny<IPQMessageDeserializer>()))
                                 .Verifiable();
        moqPQClientSyncMonitoring.Setup(csm => csm.CheckStopMonitoring()).Verifiable();
        moqPQQuoteDeserializerRepo.SetupGet(qdr => qdr.RegisteredMessageIds).Returns(Array.Empty<uint>()).Verifiable();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void SubscribedTickInstantStream_GetQuoteStreamNoMulticastLevel1QuoteSameSourceTicker_ThrowsException()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableTickInstant>();
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableLevel1Quote>();
        Assert.Fail("Should not reach here");
    }

    [TestMethod]
    public void SubscribedTickInstantStream_GetQuoteStreamNoMulticastLevel1QuoteDiffSourceSameTicker_TwoDiffStreams()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableTickInstant>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableLevel1Quote>(
         moqSecondTestSourceTickerInfo);

        Assert.AreNotEqual(firstSub, secondSub);
    }

    [TestMethod]
    public void SubscribedTickInstantStream_GetQuoteStreamNoMulticastLevel1QuoteSameSourceDiffTicker_TwoDiffStreams()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableTickInstant>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableLevel1Quote>(
         moqSecondTestSourceTickerInfo);

        Assert.AreNotEqual(firstSub, secondSub);
    }

    [TestMethod]
    public void SubscribedTickInstantForTwoDiffCcyStream_Unsubscribe1Stream_LeavesSyncMonitorRunning()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableTickInstant>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableLevel1Quote>(
         moqSecondTestSourceTickerInfo);

        Assert.AreNotEqual(firstSub, secondSub);
        moqPQQuoteDeserializerRepo.Reset();
        moqPQQuoteSerializer.Reset();
        moqFirstTestSourceTickerInfo.Reset();
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.InstrumentName)
                                    .Returns(firstTestTicker);
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.SourceInstrumentId).Returns(FirstSourceTickerId);

        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.SourceName).Returns(secondTestSourceName);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.GetDeserializer(
                                                                    moqFirstTestSourceTickerInfo.Object)).Returns(moqPQQuoteSerializer.Object)
                                  .Verifiable();

        moqUpdateClientFactory.Setup(cr => cr.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
                              .Verifiable();
        moqSnapshotClientFactory.Setup(cr => cr.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
                                .Verifiable();
        moqPQClientSyncMonitoring.Setup(csm => csm.UnregisterSerializer(It.IsAny<IPQMessageDeserializer>()))
                                 .Verifiable();
        moqPQQuoteDeserializerRepo.SetupGet(qdr => qdr.RegisteredMessageIds).Returns(new uint[] { 1 }).Verifiable();

        firstSub.Unsubscribe();

        moqPQQuoteDeserializerRepo.Verify();
        moqUpdateClientFactory.Verify();
        moqSnapshotClientFactory.Verify();
        moqPQClientSyncMonitoring.Verify();
        moqPQClientSyncMonitoring.Verify(csm => csm.CheckStopMonitoring(), Times.Never);
    }

    [TestMethod]
    public void SubscribedTickInstantForTwoDiffCcyStream_Dispose_UnsubscribesBothSubscriptions()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableTickInstant>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQPublishableLevel1Quote>(
         moqSecondTestSourceTickerInfo);

        Assert.AreNotEqual(firstSub, secondSub);
        moqPQQuoteDeserializerRepo.Reset();
        moqFirstTestSourceTickerInfo.Reset();
        moqSecondTestSourceTickerInfo.Reset();
        moqPQQuoteSerializer.Reset();
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.InstrumentName)
                                    .Returns(firstTestTicker);
        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.SourceInstrumentId).Returns(FirstSourceTickerId);

        moqFirstTestSourceTickerInfo.SetupGet(stqi => stqi.SourceName).Returns(firstTestSourceName);
        moqSecondTestSourceTickerInfo.SetupGet(stqi => stqi.InstrumentName)
                                     .Returns(secondTestSourceName);
        moqSecondTestSourceTickerInfo.SetupGet(stqi => stqi.SourceInstrumentId).Returns(SecondSourceTickerId);

        moqSecondTestSourceTickerInfo.SetupGet(stqi => stqi.SourceName).Returns(secondTestSourceName);
        moqPQQuoteDeserializerRepo.SetupGet(qdr => qdr.RegisteredMessageIds).Returns(Array.Empty<uint>()).Verifiable();
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.GetDeserializer(It.IsAny<uint>())).Returns(moqPQLvl1QuoteDeserializer.Object);
        PrepareUnsubscribeMocks();

        pqClient.Dispose();

        moqPQQuoteDeserializerRepo.Verify();
        moqUpdateClientFactory.Verify();
        moqSnapshotClientFactory.Verify();
        moqPQClientSyncMonitoring.Verify();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReturnsNullQueuesInfoRequestAttempt()
    {
        moqMarketsConfig.Reset();
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                                      It.IsAny<WaitOrTimerCallback>(), 60000, true)).Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQPublishableTickInstant>(moqFirstTestSourceTickerInfo.Object, defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqParallelController.Verify();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReattemptFindsInfoNoMoreAttemptsAreQueued()
    {
        moqMarketsConfig.Reset();
        moqFirstPricingServerConfig.Reset();
        WaitOrTimerCallback? queuedReattemptHandler = null;
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                                      It.IsAny<WaitOrTimerCallback>(), 60000, true))
                             .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, callback, _, _) =>
                             {
                                 queuedReattemptHandler = callback;
                             })
                             .Returns(new Mock<ITimerCallbackSubscription>().Object).Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQPublishableTickInstant>(moqFirstTestSourceTickerInfo.Object, defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqParallelController.Verify();

        moqParallelController.Reset();
        moqMarketsConfig.Setup(mccr => mccr.Find(It.IsAny<string>()))
                        .Returns(moqFirstMarketConnectionConfig.Object);
        Assert.IsNotNull(queuedReattemptHandler);
        queuedReattemptHandler(null, true);
        moqMarketsConfig.Verify();

        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                                       It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Never);
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReattemptStillDoesNotFindInfoMoreAttemptsAreQueued()
    {
        moqMarketsConfig.Reset();
        WaitOrTimerCallback? queuedReattemptHandler = null;
        var                  scheduleCounter        = 0;
        moqParallelController
            .Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                     It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, callback, _, _) =>
            {
                queuedReattemptHandler = callback;
                scheduleCounter++;
            }).Returns(new Mock<ITimerCallbackSubscription>().Object)
            .Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQPublishableTickInstant>(moqFirstTestSourceTickerInfo.Object, defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqParallelController.Verify();

        Assert.IsNotNull(queuedReattemptHandler);
        queuedReattemptHandler(null, true);

        Assert.AreEqual(2, scheduleCounter);
        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                                       It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Exactly(2));
    }


    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfoThenDispose_UnsubscribeOnReattemptNoInfosRequired()
    {
        moqMarketsConfig.Reset();
        WaitOrTimerCallback? queuedReattemptHandler = null;
        moqParallelController
            .Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                     It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, callback, _, _) => { queuedReattemptHandler = callback; })
            .Returns(new Mock<ITimerCallbackSubscription>().Object)
            .Verifiable();
        moqSingleOsThreadSignal.Setup(sts => sts.Set()).Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQPublishableTickInstant>(moqFirstTestSourceTickerInfo.Object, defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqParallelController.Verify();

        moqParallelController.Reset();
        pqClient.Dispose();
        moqSingleOsThreadSignal.Verify();
        Assert.IsNotNull(queuedReattemptHandler);
        queuedReattemptHandler(null, true);
        moqMarketsConfig.Verify();

        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                                       It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Never);
    }

    [TestMethod]
    public void NewPQClient_MultipleGetQuoteStreamNoQuoteInfos_OnlyOneReattemptIsQueued()
    {
        moqMarketsConfig.Reset();
        var scheduleCounter = 0;
        moqParallelController
            .Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                     It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, _, _, _) => { scheduleCounter++; })
            .Returns(new Mock<ITimerCallbackSubscription>().Object)
            .Verifiable();
        var subQuoteStream = pqClient.GetQuoteStream<PQPublishableTickInstant>
            (moqFirstTestSourceTickerInfo.Object, defaultSyncRetryInterval);
        var subQuoteStream2 = pqClient.GetQuoteStream<PQPublishableTickInstant>
            (moqFirstTestSourceTickerInfo.Object, defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        Assert.IsNull(subQuoteStream2);
        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                                       It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Exactly(1));
        Assert.AreEqual(1, scheduleCounter);
    }

    [TestMethod]
    public void NewPQClientTickerNotAvailableInSource_GetQuoteStreamNoMultiCast_ReturnsNullNoQuoteRepoIsQueued()
    {
        var scheduleCounter = 0;
        moqParallelController
            .Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                     It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, _, _, _) => { scheduleCounter++; })
            .Returns(new Mock<ITimerCallbackSubscription>().Object)
            .Verifiable();

        var diffQuoteInfo = new Mock<ISourceTickerInfo>();

        diffQuoteInfo.SetupGet(stqi => stqi.InstrumentName).Returns("SomeUnknownTicker").Verifiable();
        diffQuoteInfo.SetupGet(stqi => stqi.SourceName).Returns(firstTestSourceName).Verifiable();

        var subQuoteStream = pqClient.GetQuoteStream<PQPublishableTickInstant>(diffQuoteInfo.Object, defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqFirstTestSourceTickerInfo.Verify();
        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                                                                       It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Never);
        Assert.AreEqual(0, scheduleCounter);
    }

    [TestMethod]
    public void SubscribedTickInstant_GetSourceServerConfig_FindsSnapshotUpdatePricingServerConfig()
    {
        var subQuoteStream = pqClient.GetQuoteStream<PQPublishableTickInstant>
            (moqFirstTestSourceTickerInfo.Object, defaultSyncRetryInterval);

        Assert.IsNotNull(subQuoteStream);
        var marketConnConfig = pqClient.GetSourceServerConfig(moqFirstTestSourceTickerInfo.Object.SourceName);
        Assert.IsNotNull(marketConnConfig);
        Assert.AreEqual(moqFirstMarketConnectionConfig.Object, marketConnConfig);
    }

    [TestMethod]
    public void NewPQClient_GetSourceServerConfig_DoesNotFindSnapshotUpdatePricingServerConfig()
    {
        var pricingServerConfig = pqClient.GetSourceServerConfig(moqFirstTestSourceTickerInfo.Object.SourceName);
        Assert.IsNull(pricingServerConfig);
    }

    [TestMethod]
    public void SubscribedTickInstantStream_RequestSnapshots_FindsSnapshotClientForwardsStreamIdRequests()
    {
        moqSnapshotClient.Setup(sc => sc.RequestSnapshots(It.IsAny<List<ISourceTickerInfo>>()))
                         .Verifiable();
        moqSnapshotClientFactory.Reset();
        moqSnapshotClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
                                .Returns(moqSnapshotClient.Object).Verifiable();

        var moqClientSnapshotServerConfig = new Mock<INetworkTopicConnectionConfig>();
        pqClient.RequestSnapshots(moqClientSnapshotServerConfig.Object, new List<ISourceTickerInfo>());

        moqSnapshotClientFactory.Verify();
        moqSnapshotClient.Verify();
    }

    [TestMethod]
    public void NewPQClient_RequestSnapshots_DoesNotFindSnapshotClientDoesNothing()
    {
        moqSnapshotClientFactory.Reset();
        moqSnapshotClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
                                .Returns(moqSnapshotClient.Object).Verifiable();


        var moqClientSnapshotServerConfig = new Mock<INetworkTopicConnectionConfig>();
        pqClient.RequestSnapshots(moqClientSnapshotServerConfig.Object, new List<ISourceTickerInfo>());

        moqSnapshotClientFactory.Verify();
    }
}
