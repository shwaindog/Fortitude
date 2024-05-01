#region

using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

[TestClass]
public class PQClientTests
{
    private const ushort FirstSourceId = 123;
    private const ushort FirstTickerId = 456;
    private const ushort SecondSourceId = 234;
    private const ushort SecondTickerId = 567;

    private const uint FirstSourceTickerId = ((uint)FirstSourceId << 16) | FirstTickerId;
    private const uint SecondSourceTickerId = ((uint)SecondSourceId << 16) | SecondTickerId;
    private readonly uint defaultSyncRetryInterval = 60000;
    private readonly string firstTestSourceName = "FirstTestSourceName";
    private readonly string firstTestTicker = "FirstTestTicker";
    private readonly string secondTestSourceName = "SecondTestSourceName";
    private readonly string secondTestTicker = "SecondTestTicker";
    private PQLevel0Quote dummyLevel0Quote = null!;
    private PQLevel1Quote dummyLevel1Quote = null!;
    private PQLevel2Quote dummyLevel2Quote = null!;
    private PQLevel3Quote dummyLevel3Quote = null!;
    private Mock<IDisposable> moqDeserializerSubscription = null!;
    private Mock<IMarketConnectionConfig> moqFirstMarketConnectionConfig = null!;
    private Mock<IPricingServerConfig> moqFirstPricingServerConfig = null!;
    private Mock<ISocketDispatcher> moqFirstSocketDispatcher = null!;
    private Mock<ISourceTickerQuoteInfo> moqFirstTestSourceTickerQuoteInfo = null!;
    private Mock<IMarketsConfig> moqMarketsConfig = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQClientSyncMonitoring> moqPQClientSyncMonitoring = null!;
    private Mock<IPQDeserializer<PQLevel0Quote>> moqPQLvl0QuoteSerializer = null!;
    private Mock<IPQDeserializer<PQLevel1Quote>> moqPQLvl1QuoteSerializer = null!;
    private Mock<IPQDeserializer<PQLevel2Quote>> moqPQLvl2QuoteSerializer = null!;
    private Mock<IPQDeserializer<PQLevel3Quote>> moqPQLvl3QuoteSerializer = null!;
    private Mock<IPQClientQuoteDeserializerRepository> moqPQQuoteDeserializerRepo = null!;
    private Mock<IPQDeserializer> moqPQQuoteSerializer = null!;
    private Mock<IMarketConnectionConfig> moqSecondMarketConnectionConfig = null!;
    private Mock<IPricingServerConfig> moqSecondPricingServerConfig = null!;
    private Mock<ISourceTickerQuoteInfo> moqSecondTestSourceTickerQuoteInfo = null!;
    private Mock<IIntraOSThreadSignal> moqSingleOsThreadSignal = null!;
    private Mock<IPQSnapshotClient> moqSnapshotClient = null!;
    private Mock<IPQConversationRepository<IPQSnapshotClient>> moqSnapshotClientFactory = null!;
    private Mock<IEnumerator<IEndpointConfig>> moqSnapshotEndpointEnumerator = null!;
    private Mock<IEndpointConfig> moqSnapshotServerConfig = null!;
    private Mock<INetworkTopicConnectionConfig> moqSnapshotTopicServerConfig = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<IPQUpdateClient> moqUpdateClient = null!;
    private Mock<IPQConversationRepository<IPQUpdateClient>> moqUpdateClientFactory = null!;
    private Mock<IEnumerator<IEndpointConfig>> moqUpdateEndpointEnumerator = null!;
    private Mock<IEndpointConfig> moqUpdateServerConfig = null!;
    private Mock<INetworkTopicConnectionConfig> moqUpdateTopicServerConfig = null!;
    private PQClient pqClient = null!;

    [TestInitialize]
    public void SetUp()
    {
        PrepareNewPQClientInitializationMocks();

        PrepareQuoteLevelMocks();

        PrepareGetQuoteStreamMocks();
        pqClient = new PQClient(moqMarketsConfig.Object, moqSocketDispatcherResolver.Object,
            moqUpdateClientFactory.Object, moqSnapshotClientFactory.Object);

        NonPublicInvocator.SetInstanceField(pqClient, "pqClientSyncMonitoring", moqPQClientSyncMonitoring.Object);
        NonPublicInvocator.SetInstanceField(pqClient, "deserializationRepository"
            , moqPQQuoteDeserializerRepo.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    private void PrepareNewPQClientInitializationMocks()
    {
        moqSingleOsThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqParallelController = new Mock<IOSParallelController>();
        moqSocketDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqParallelController.Setup(pc => pc.SingleOSThreadActivateSignal(false)).Returns(
            moqSingleOsThreadSignal.Object);
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;


        moqFirstTestSourceTickerQuoteInfo = new Mock<ISourceTickerQuoteInfo>();
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Source).Returns(firstTestSourceName);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Ticker).Returns(firstTestTicker);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Id).Returns(FirstSourceTickerId);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.SourceId).Returns(FirstSourceId);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.TickerId).Returns(FirstTickerId);

        moqSecondTestSourceTickerQuoteInfo = new Mock<ISourceTickerQuoteInfo>();

        moqSecondTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Source).Returns(secondTestSourceName);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Ticker).Returns(secondTestTicker);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Id).Returns(SecondSourceTickerId);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.SourceId).Returns(SecondSourceId);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.TickerId).Returns(SecondTickerId);


        moqFirstMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqFirstPricingServerConfig = new Mock<IPricingServerConfig>();
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.PricingServerConfig).Returns(moqFirstPricingServerConfig.Object);
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.SourceId).Returns(FirstSourceId);
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.Name).Returns(firstTestSourceName);
        moqFirstMarketConnectionConfig.Setup(mcc => mcc.GetSourceTickerInfo(firstTestTicker)).Returns(moqFirstTestSourceTickerQuoteInfo.Object);
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.AllSourceTickerInfos).Returns(new[] { moqFirstTestSourceTickerQuoteInfo.Object });

        moqSecondMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqSecondPricingServerConfig = new Mock<IPricingServerConfig>();
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.PricingServerConfig).Returns(moqSecondPricingServerConfig.Object);
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.SourceId).Returns(SecondSourceId);
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.Name).Returns(secondTestSourceName);
        moqSecondMarketConnectionConfig.Setup(mcc => mcc.GetSourceTickerInfo(secondTestTicker)).Returns(moqSecondTestSourceTickerQuoteInfo.Object);
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.AllSourceTickerInfos).Returns(new[] { moqSecondTestSourceTickerQuoteInfo.Object });


        moqSnapshotClientFactory = new Mock<IPQConversationRepository<IPQSnapshotClient>>();
        moqSnapshotClient = new Mock<IPQSnapshotClient>();
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
        var serverTickerConfigs = new List<IMarketConnectionConfig>
        {
            moqFirstMarketConnectionConfig.Object, moqSecondMarketConnectionConfig.Object
        };
        moqMarketsConfig.SetupGet(mccr => mccr.Markets)
            .Returns(serverTickerConfigs);

        moqUpdateClientFactory = new Mock<IPQConversationRepository<IPQUpdateClient>>();
        moqUpdateClient = new Mock<IPQUpdateClient>();
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
        moqPQClientSyncMonitoring.Setup(csm => csm.RegisterNewDeserializer(It.IsAny<IPQDeserializer>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(csm => csm.CheckStartMonitoring()).Verifiable();

        moqUpdateServerConfig = new Mock<IEndpointConfig>();
        moqUpdateTopicServerConfig = new Mock<INetworkTopicConnectionConfig>();
        moqUpdateEndpointEnumerator = new Mock<IEnumerator<IEndpointConfig>>();
        moqUpdateEndpointEnumerator.SetupGet(uee => uee.Current).Returns(moqUpdateServerConfig.Object);
        moqUpdateTopicServerConfig.Setup(uee => uee.GetEnumerator()).Returns(moqUpdateEndpointEnumerator.Object);
        moqUpdateServerConfig.SetupGet(scc => scc.SubnetMask).Returns("123.0.0.123").Verifiable();
        moqUpdateServerConfig.SetupGet(scc => scc.Hostname).Returns("UpdateHostName").Verifiable();
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.Name).Returns("TestServerName").Verifiable();

        moqSnapshotServerConfig = new Mock<IEndpointConfig>();
        moqSnapshotTopicServerConfig = new Mock<INetworkTopicConnectionConfig>();
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
        dummyLevel0Quote = new PQLevel0Quote(moqFirstTestSourceTickerQuoteInfo.Object);
        dummyLevel1Quote = new PQLevel1Quote(moqFirstTestSourceTickerQuoteInfo.Object);
        dummyLevel2Quote = new PQLevel2Quote(moqFirstTestSourceTickerQuoteInfo.Object);
        dummyLevel3Quote = new PQLevel3Quote(moqFirstTestSourceTickerQuoteInfo.Object);

        moqPQQuoteSerializer = new Mock<IPQDeserializer>();
        moqPQLvl0QuoteSerializer = moqPQQuoteSerializer.As<IPQDeserializer<PQLevel0Quote>>();
        moqPQLvl1QuoteSerializer = moqPQQuoteSerializer.As<IPQDeserializer<PQLevel1Quote>>();
        moqPQLvl2QuoteSerializer = moqPQQuoteSerializer.As<IPQDeserializer<PQLevel2Quote>>();
        moqPQLvl3QuoteSerializer = moqPQQuoteSerializer.As<IPQDeserializer<PQLevel3Quote>>();
        moqDeserializerSubscription = new Mock<IDisposable>();
        moqPQLvl0QuoteSerializer.Setup(pql0Qs =>
                pql0Qs.Subscribe(It.IsAny<PQTickerFeedSubscriptionQuoteStream<PQLevel0Quote>>()))
            .Returns(moqDeserializerSubscription.Object).Verifiable();
        moqPQLvl1QuoteSerializer.Setup(pql0Qs =>
                pql0Qs.Subscribe(It.IsAny<PQTickerFeedSubscriptionQuoteStream<PQLevel1Quote>>()))
            .Returns(moqDeserializerSubscription.Object).Verifiable();
        moqPQLvl2QuoteSerializer.Setup(pql0Qs =>
                pql0Qs.Subscribe(It.IsAny<PQTickerFeedSubscriptionQuoteStream<PQLevel2Quote>>()))
            .Returns(moqDeserializerSubscription.Object).Verifiable();
        moqPQLvl3QuoteSerializer.Setup(pql0Qs =>
                pql0Qs.Subscribe(It.IsAny<PQTickerFeedSubscriptionQuoteStream<PQLevel3Quote>>()))
            .Returns(moqDeserializerSubscription.Object).Verifiable();

        moqPQQuoteDeserializerRepo = new Mock<IPQClientQuoteDeserializerRepository>();
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.CreateQuoteDeserializer<PQLevel0Quote>(
                It.IsAny<ITickerPricingSubscriptionConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.CreateQuoteDeserializer<PQLevel1Quote>(
                It.IsAny<ITickerPricingSubscriptionConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.CreateQuoteDeserializer<PQLevel2Quote>(
                It.IsAny<ITickerPricingSubscriptionConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.CreateQuoteDeserializer<PQLevel3Quote>(
                It.IsAny<ITickerPricingSubscriptionConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqUpdateClient.SetupGet(sc => sc.DeserializerRepository).Returns(moqPQQuoteDeserializerRepo.Object);
        moqSnapshotClient.SetupGet(sc => sc.DeserializerRepository).Returns(moqPQQuoteDeserializerRepo.Object);

        moqPQLvl0QuoteSerializer.SetupGet(pql0Qs => pql0Qs.PublishedQuote).Returns(dummyLevel0Quote).Verifiable();
        moqPQLvl1QuoteSerializer.SetupGet(pql0Qs => pql0Qs.PublishedQuote).Returns(dummyLevel1Quote).Verifiable();
        moqPQLvl2QuoteSerializer.SetupGet(pql0Qs => pql0Qs.PublishedQuote).Returns(dummyLevel2Quote).Verifiable();
        moqPQLvl3QuoteSerializer.SetupGet(pql0Qs => pql0Qs.PublishedQuote).Returns(dummyLevel3Quote).Verifiable();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMulticastLevel0Quote_RegistersAndReturnsQuote()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel0Quote>();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMulticastLevel1Quote_RegistersAndReturnsQuote()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel1Quote>();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMulticastLevel2Quote_RegistersAndReturnsQuote()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel2Quote>();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMulticastLevel3Quote_RegistersAndReturnsQuote()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel3Quote>();
    }

    private IPQTickerFeedSubscriptionQuoteStream<T>
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<T>(
            Mock<ISourceTickerQuoteInfo>? sourceTickerQuoteInfo = null) where T : PQLevel0Quote, new()
    {
        sourceTickerQuoteInfo ??= moqFirstTestSourceTickerQuoteInfo;
        var result = pqClient.GetQuoteStream<T>(sourceTickerQuoteInfo.Object, defaultSyncRetryInterval);

        Assert.IsNotNull(result);
        sourceTickerQuoteInfo.Verify();
        moqMarketsConfig.Verify();
        moqPQQuoteDeserializerRepo.Verify();
        moqPQClientSyncMonitoring.Verify();
        moqFirstSocketDispatcher.Verify();
        moqUpdateClientFactory.Verify();
        moqSnapshotClientFactory.Verify();

        return result;
    }

    [TestMethod]
    public void SubscribedLevel0QuoteStream_Unsubscribe_RegistersAndReturnsQuote()
    {
        SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<PQLevel0Quote>();
    }

    [TestMethod]
    public void SubscribedLevel1QuoteStream_Unsubscribe_RegistersAndReturnsQuote()
    {
        SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<PQLevel1Quote>();
    }

    [TestMethod]
    public void SubscribedLevel2QuoteStream_Unsubscribe_RegistersAndReturnsQuote()
    {
        SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<PQLevel2Quote>();
    }

    [TestMethod]
    public void SubscribedLevel3QuoteStream_Unsubscribe_RegistersAndReturnsQuote()
    {
        SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<PQLevel3Quote>();
    }

    public void SubscribedQuoteStreamTyped_Unsubscribe_RegistersAndReturnsQuote<T>() where T : PQLevel0Quote, new()
    {
        var subQuoteStream
            = pqClient.GetQuoteStream<T>(moqFirstTestSourceTickerQuoteInfo.Object, defaultSyncRetryInterval);

        PrepareUnsubscribeMocks();

        subQuoteStream!.Unsubscribe();

        moqPQQuoteDeserializerRepo.Verify();
        moqUpdateClientFactory.Verify();
        moqSnapshotClientFactory.Verify();
        moqPQClientSyncMonitoring.Verify();
    }

    private void PrepareUnsubscribeMocks()
    {
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.GetDeserializer(moqFirstTestSourceTickerQuoteInfo.Object))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.GetDeserializer(moqFirstTestSourceTickerQuoteInfo.Object))
            .Returns(moqPQQuoteSerializer.Object);

        moqUpdateClientFactory.Setup(cr => cr.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqSnapshotClientFactory.Setup(cr => cr.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(cr => cr.UnregisterSerializer(It.IsAny<IPQDeserializer>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(csm => csm.CheckStopMonitoring()).Verifiable();
        moqPQQuoteDeserializerRepo.SetupGet(qdr => qdr.RegisteredMessageIds).Returns(Array.Empty<uint>()).Verifiable();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void SubscribedLevel0Stream_GetQuoteStreamNoMulticastLevel1QuoteSameSourceTicker_ThrowsException()
    {
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel0Quote>();
        NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel1Quote>();
        Assert.Fail("Should not reach here");
    }

    [TestMethod]
    public void SubscribedLevel0Stream_GetQuoteStreamNoMulticastLevel1QuoteDiffSourceSameTicker_TwoDiffStreams()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel0Quote>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel1Quote>(
            moqSecondTestSourceTickerQuoteInfo);

        Assert.AreNotEqual(firstSub, secondSub);
    }

    [TestMethod]
    public void SubscribedLevel0Stream_GetQuoteStreamNoMulticastLevel1QuoteSameSourceDiffTicker_TwoDiffStreams()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel0Quote>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel1Quote>(
            moqSecondTestSourceTickerQuoteInfo);

        Assert.AreNotEqual(firstSub, secondSub);
    }

    [TestMethod]
    public void SubscribedLevel0ForTwoDiffCcyStream_Unsubscribe1Stream_LeavesSyncMonitorRunning()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel0Quote>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel1Quote>(
            moqSecondTestSourceTickerQuoteInfo);

        Assert.AreNotEqual(firstSub, secondSub);
        moqPQQuoteDeserializerRepo.Reset();
        moqPQQuoteSerializer.Reset();
        moqFirstTestSourceTickerQuoteInfo.Reset();
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Ticker)
            .Returns(firstTestTicker);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Id).Returns(FirstSourceTickerId);

        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Source).Returns(secondTestSourceName);
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.GetDeserializer(
            moqFirstTestSourceTickerQuoteInfo.Object)).Returns(moqPQQuoteSerializer.Object).Verifiable();

        moqUpdateClientFactory.Setup(cr => cr.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqSnapshotClientFactory.Setup(cr => cr.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(csm => csm.UnregisterSerializer(It.IsAny<IPQDeserializer>()))
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
    public void SubscribedLevel0ForTwoDiffCcyStream_Dispose_UnsubscribesBothSubscriptions()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel0Quote>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel1Quote>(
            moqSecondTestSourceTickerQuoteInfo);

        Assert.AreNotEqual(firstSub, secondSub);
        moqPQQuoteDeserializerRepo.Reset();
        moqFirstTestSourceTickerQuoteInfo.Reset();
        moqSecondTestSourceTickerQuoteInfo.Reset();
        moqPQQuoteSerializer.Reset();
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Ticker)
            .Returns(firstTestTicker);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Id).Returns(FirstSourceTickerId);

        moqFirstTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Source).Returns(firstTestSourceName);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Ticker)
            .Returns(secondTestSourceName);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Id).Returns(SecondSourceTickerId);

        moqSecondTestSourceTickerQuoteInfo.SetupGet(stqi => stqi.Source).Returns(secondTestSourceName);
        moqPQQuoteDeserializerRepo.SetupGet(qdr => qdr.RegisteredMessageIds).Returns(Array.Empty<uint>()).Verifiable();
        moqPQQuoteDeserializerRepo.Setup(qdr => qdr.GetDeserializer(It.IsAny<uint>())).Returns(moqPQLvl1QuoteSerializer.Object);
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
            = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestSourceTickerQuoteInfo.Object
                , defaultSyncRetryInterval);

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
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, callback, _, _) => { queuedReattemptHandler = callback; })
            .Returns(new Mock<ITimerCallbackSubscription>().Object).Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestSourceTickerQuoteInfo.Object
                , defaultSyncRetryInterval);

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
        var scheduleCounter = 0;
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, callback, _, _) =>
            {
                queuedReattemptHandler = callback;
                scheduleCounter++;
            }).Returns(new Mock<ITimerCallbackSubscription>().Object)
            .Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestSourceTickerQuoteInfo.Object
                , defaultSyncRetryInterval);

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
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, callback, _, _) => { queuedReattemptHandler = callback; })
            .Returns(new Mock<ITimerCallbackSubscription>().Object)
            .Verifiable();
        moqSingleOsThreadSignal.Setup(sts => sts.Set()).Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestSourceTickerQuoteInfo.Object
                , defaultSyncRetryInterval);

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
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, _, _, _) => { scheduleCounter++; })
            .Returns(new Mock<ITimerCallbackSubscription>().Object)
            .Verifiable();
        var subQuoteStream = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestSourceTickerQuoteInfo.Object,
            defaultSyncRetryInterval);
        var subQuoteStream2 = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestSourceTickerQuoteInfo.Object,
            defaultSyncRetryInterval);

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
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((_, _, _, _) => { scheduleCounter++; })
            .Returns(new Mock<ITimerCallbackSubscription>().Object)
            .Verifiable();

        var diffQuoteInfo = new Mock<ISourceTickerQuoteInfo>();

        diffQuoteInfo.SetupGet(stqi => stqi.Ticker).Returns("SomeUnknownTicker").Verifiable();
        diffQuoteInfo.SetupGet(stqi => stqi.Source).Returns(firstTestSourceName).Verifiable();

        var subQuoteStream = pqClient.GetQuoteStream<PQLevel0Quote>(diffQuoteInfo.Object,
            defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqFirstTestSourceTickerQuoteInfo.Verify();
        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
            It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Never);
        Assert.AreEqual(0, scheduleCounter);
    }

    [TestMethod]
    public void SubscribedLevel0_GetSourceServerConfig_FindsSnapshotUpdatePricingServerConfig()
    {
        var subQuoteStream = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestSourceTickerQuoteInfo.Object,
            defaultSyncRetryInterval);

        Assert.IsNotNull(subQuoteStream);
        var marketConnConfig = pqClient.GetSourceServerConfig(moqFirstTestSourceTickerQuoteInfo.Object.Source);
        Assert.IsNotNull(marketConnConfig);
        Assert.AreEqual(moqFirstMarketConnectionConfig.Object, marketConnConfig);
    }

    [TestMethod]
    public void NewPQClient_GetSourceServerConfig_DoesNotFindSnapshotUpdatePricingServerConfig()
    {
        var pricingServerConfig = pqClient.GetSourceServerConfig(moqFirstTestSourceTickerQuoteInfo.Object.Source);
        Assert.IsNull(pricingServerConfig);
    }

    [TestMethod]
    public void SubscribedLevel0Stream_RequestSnapshots_FindsSnapshotClientForwardsStreamIdRequests()
    {
        moqSnapshotClient.Setup(sc => sc.RequestSnapshots(It.IsAny<List<ISourceTickerQuoteInfo>>()))
            .Verifiable();
        moqSnapshotClientFactory.Reset();
        moqSnapshotClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSnapshotClient.Object).Verifiable();

        var moqClientSnapshotServerConfig = new Mock<INetworkTopicConnectionConfig>();
        pqClient.RequestSnapshots(moqClientSnapshotServerConfig.Object, new List<ISourceTickerQuoteInfo>());

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
        pqClient.RequestSnapshots(moqClientSnapshotServerConfig.Object, new List<ISourceTickerQuoteInfo>());

        moqSnapshotClientFactory.Verify();
    }
}
