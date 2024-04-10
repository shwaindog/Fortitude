#region

using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQClientTests
{
    private const ushort FirstSourceId = 123;
    private const ushort FirstTickerId = 456;
    private const ushort SecondSourceId = 234;
    private const ushort SecondTickerId = 567;

    private const uint FirstSourceTickerId = ((uint)FirstSourceId << 16) | FirstTickerId;
    private const uint SecondSourceTickerId = ((uint)SecondSourceId << 16) | SecondTickerId;
    private readonly bool defaultAllowCatchUps = true;
    private readonly uint defaultSyncRetryInterval = 60000;
    private readonly string firstTestSourceName = "FirstTestSourceName";
    private readonly string firstTestTicker = "FirstTestTicker";
    private PQLevel0Quote dummyLevel0Quote = null!;
    private PQLevel1Quote dummyLevel1Quote = null!;
    private PQLevel2Quote dummyLevel2Quote = null!;
    private PQLevel3Quote dummyLevel3Quote = null!;
    private Mock<IDisposable> moqDeserializerSubscription = null!;
    private Mock<IMarketConnectionConfig> moqFirstMarketConnectionConfig = null!;
    private Mock<IPricingServerConfig> moqFirstPricingServerConfig = null!;
    private Mock<ISocketDispatcher> moqFirstSocketDispatcher = null!;
    private Mock<ISourceTickerQuoteInfo> moqFirstTestSourceTickerQuoteInfo = null!;
    private Mock<IMarketConnectionConfigRepository> moqMarketConnectionConfigRepo = null!;
    private Mock<IPQClientMessageStreamDecoder> moqMessageStreamDecoder = null!;
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
    private Mock<IEndpointConfig> moqSnapshotServerConfig = null!;
    private Mock<INetworkTopicConnectionConfig> moqSnapshotTopicServerConfig = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<IPQUpdateClient> moqUpdateClient = null!;
    private Mock<IPQConversationRepository<IPQUpdateClient>> moqUpdateClientFactory = null!;
    private Mock<IEndpointConfig> moqUpdateServerConfig = null!;
    private Mock<INetworkTopicConnectionConfig> moqUpdateTopicServerConfig = null!;
    private PQClient pqClient = null!;
    private string secondTestSourceName = "SecondTestSourceName";
    private string secondTestTicker = "SecondTestTicker";

    [TestInitialize]
    public void SetUp()
    {
        PrepareNewPQClientInitializationMocks();

        PrepareQuoteLevelMocks();

        PrepareGetQuoteStreamMocks();
        pqClient = new PQClient(moqMarketConnectionConfigRepo.Object, moqSocketDispatcherResolver.Object,
            defaultAllowCatchUps, moqUpdateClientFactory.Object, moqSnapshotClientFactory.Object);

        NonPublicInvocator.SetInstanceField(pqClient, "pqClientSyncMonitoring", moqPQClientSyncMonitoring.Object);
        NonPublicInvocator.SetInstanceField(pqClient, "snapshotSerializationRepository"
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
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Source).Returns(firstTestSourceName);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Ticker).Returns(firstTestTicker);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Id).Returns(FirstSourceTickerId);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.SourceId).Returns(FirstSourceId);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.TickerId).Returns(FirstTickerId);

        moqSecondTestSourceTickerQuoteInfo = new Mock<ISourceTickerQuoteInfo>();

        moqSecondTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Source).Returns(secondTestSourceName);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Ticker).Returns(secondTestTicker);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Id).Returns(SecondSourceTickerId);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.SourceId).Returns(SecondSourceId);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.TickerId).Returns(SecondTickerId);


        moqFirstMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqFirstPricingServerConfig = new Mock<IPricingServerConfig>();
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.PricingServerConfig).Returns(moqFirstPricingServerConfig.Object);
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.SourceId).Returns(FirstSourceId);
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.Name).Returns(firstTestSourceName);
        moqFirstMarketConnectionConfig.Setup(mcc => mcc.GetSourceTickerInfo(firstTestTicker)).Returns(moqFirstTestSourceTickerQuoteInfo.Object);

        moqSecondMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqSecondPricingServerConfig = new Mock<IPricingServerConfig>();
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.PricingServerConfig).Returns(moqSecondPricingServerConfig.Object);
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.SourceId).Returns(SecondSourceId);
        moqSecondMarketConnectionConfig.SetupGet(mcc => mcc.Name).Returns(secondTestSourceName);
        moqSecondMarketConnectionConfig.Setup(mcc => mcc.GetSourceTickerInfo(secondTestTicker)).Returns(moqSecondTestSourceTickerQuoteInfo.Object);


        moqSnapshotClientFactory = new Mock<IPQConversationRepository<IPQSnapshotClient>>();
        moqSnapshotClient = new Mock<IPQSnapshotClient>();
        moqMessageStreamDecoder = new Mock<IPQClientMessageStreamDecoder>();
        moqSnapshotClientFactory
            .Setup(scf => scf.RetrieveOrCreateConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSnapshotClient.Object);
        moqSnapshotClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSnapshotClient.Object);

        moqMarketConnectionConfigRepo = new Mock<IMarketConnectionConfigRepository>();
        moqMarketConnectionConfigRepo.Setup(pscr => pscr.Find(firstTestSourceName))
            .Returns(moqFirstMarketConnectionConfig.Object);
        moqMarketConnectionConfigRepo.Setup(pscr => pscr.Find(secondTestSourceName))
            .Returns(moqSecondMarketConnectionConfig.Object);
        var serverTickerConfigs = new List<IMarketConnectionConfig>
        {
            moqFirstMarketConnectionConfig.Object, moqSecondMarketConnectionConfig.Object
        };
        moqMarketConnectionConfigRepo.SetupGet(supsc => supsc.CurrentConfigs)
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
        moqPQClientSyncMonitoring.Setup(pqcsm => pqcsm.RegisterNewDeserializer(It.IsAny<IPQDeserializer>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(pqcsm => pqcsm.CheckStartMonitoring()).Verifiable();

        moqUpdateServerConfig = new Mock<IEndpointConfig>();
        moqUpdateTopicServerConfig = new Mock<INetworkTopicConnectionConfig>();
        moqUpdateTopicServerConfig.SetupGet(utsc => utsc.Current).Returns(moqUpdateServerConfig.Object);
        moqUpdateTopicServerConfig.Setup(utsc => utsc.GetEnumerator()).Returns(moqUpdateTopicServerConfig.Object);
        moqUpdateServerConfig.SetupGet(scc => scc.SubnetMask).Returns("123.0.0.123").Verifiable();
        moqUpdateServerConfig.SetupGet(scc => scc.Hostname).Returns("UpdateHostName").Verifiable();
        moqFirstMarketConnectionConfig.SetupGet(mcc => mcc.Name).Returns("TestServerName").Verifiable();

        moqSnapshotServerConfig = new Mock<IEndpointConfig>();
        moqSnapshotTopicServerConfig = new Mock<INetworkTopicConnectionConfig>();
        moqSnapshotTopicServerConfig.SetupGet(supsc => supsc.Current).Returns(moqSnapshotServerConfig.Object);
        moqSnapshotServerConfig.SetupGet(scc => scc.Hostname).Returns("SnapshotHostName");
        moqFirstPricingServerConfig.SetupGet(supsc => supsc.SnapshotConnectionConfig)
            .Returns(moqSnapshotTopicServerConfig.Object).Verifiable();
        moqFirstPricingServerConfig.SetupGet(supsc => supsc.UpdateConnectionConfig)
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
        moqPQQuoteDeserializerRepo.Setup(pqqsf => pqqsf.CreateQuoteDeserializer<PQLevel0Quote>(
                It.IsAny<ITickerPricingSubscriptionConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(pqqsf => pqqsf.CreateQuoteDeserializer<PQLevel1Quote>(
                It.IsAny<ITickerPricingSubscriptionConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(pqqsf => pqqsf.CreateQuoteDeserializer<PQLevel2Quote>(
                It.IsAny<ITickerPricingSubscriptionConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(pqqsf => pqqsf.CreateQuoteDeserializer<PQLevel3Quote>(
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
        moqMarketConnectionConfigRepo.Verify();
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
        moqPQQuoteDeserializerRepo.Setup(pqqsf => pqqsf.GetDeserializer(moqFirstTestSourceTickerQuoteInfo.Object))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteDeserializerRepo.Setup(pqqsf => pqqsf.GetDeserializer(moqFirstTestSourceTickerQuoteInfo.Object))
            .Returns(moqPQQuoteSerializer.Object);

        moqUpdateClientFactory.Setup(sscf => sscf.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqSnapshotClientFactory.Setup(sscf => sscf.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(pqcsm => pqcsm.UnregisterSerializer(It.IsAny<IPQDeserializer>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(pqcsm => pqcsm.CheckStopMonitoring()).Verifiable();
        moqPQQuoteDeserializerRepo.SetupGet(pqqsf => pqqsf.RegisteredMessageIds).Returns(Array.Empty<uint>()).Verifiable();
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
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Ticker)
            .Returns(firstTestTicker);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Id).Returns(FirstSourceTickerId);

        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Source).Returns(secondTestSourceName);
        moqPQQuoteDeserializerRepo.Setup(pqqsf => pqqsf.GetDeserializer(
            moqFirstTestSourceTickerQuoteInfo.Object)).Returns(moqPQQuoteSerializer.Object).Verifiable();

        moqUpdateClientFactory.Setup(sscf => sscf.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqSnapshotClientFactory.Setup(sscf => sscf.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(pqcsm => pqcsm.UnregisterSerializer(It.IsAny<IPQDeserializer>()))
            .Verifiable();
        moqPQQuoteDeserializerRepo.SetupGet(pqqsf => pqqsf.RegisteredMessageIds).Returns(new uint[] { 1 }).Verifiable();

        firstSub.Unsubscribe();

        moqPQQuoteDeserializerRepo.Verify();
        moqUpdateClientFactory.Verify();
        moqSnapshotClientFactory.Verify();
        moqPQClientSyncMonitoring.Verify();
        moqPQClientSyncMonitoring.Verify(pqcsm => pqcsm.CheckStopMonitoring(), Times.Never);
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
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Ticker)
            .Returns(firstTestTicker);
        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Id).Returns(FirstSourceTickerId);

        moqFirstTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Source).Returns(firstTestSourceName);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Ticker)
            .Returns(secondTestSourceName);
        moqSecondTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Id).Returns(SecondSourceTickerId);

        moqSecondTestSourceTickerQuoteInfo.SetupGet(stpc => stpc.Source).Returns(secondTestSourceName);
        moqPQQuoteDeserializerRepo.SetupGet(pqqsf => pqqsf.RegisteredMessageIds).Returns(Array.Empty<uint>()).Verifiable();
        moqPQQuoteDeserializerRepo.Setup(pqqsf => pqqsf.GetDeserializer(It.IsAny<uint>())).Returns(moqPQLvl1QuoteSerializer.Object);
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
        moqMarketConnectionConfigRepo.Reset();
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
        moqMarketConnectionConfigRepo.Reset();
        moqFirstPricingServerConfig.Reset();
        WaitOrTimerCallback? queuedReattemptHandler = null;
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((its, callback, timeout, callOnce) =>
            {
                queuedReattemptHandler = callback;
            }).Returns(new Mock<ITimerCallbackSubscription>().Object).Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestSourceTickerQuoteInfo.Object
                , defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqParallelController.Verify();

        moqParallelController.Reset();
        moqMarketConnectionConfigRepo.Setup(pscr => pscr.Find(It.IsAny<string>()))
            .Returns(moqFirstMarketConnectionConfig.Object);
        Assert.IsNotNull(queuedReattemptHandler);
        queuedReattemptHandler(null, true);
        moqMarketConnectionConfigRepo.Verify();

        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
            It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Never);
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReattemptStillDoesntFindInfoMoreAttemptsAreQueued()
    {
        moqMarketConnectionConfigRepo.Reset();
        WaitOrTimerCallback? queuedReattemptHandler = null;
        var scheduleCounter = 0;
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((its, callback, timeout, callOnce) =>
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
        moqMarketConnectionConfigRepo.Reset();
        WaitOrTimerCallback? queuedReattemptHandler = null;
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((its, callback, timeout, callOnce) =>
            {
                queuedReattemptHandler = callback;
            }).Returns(new Mock<ITimerCallbackSubscription>().Object)
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
        moqMarketConnectionConfigRepo.Verify();

        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
            It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Never);
    }

    [TestMethod]
    public void NewPQClient_MultipleGetQuoteStreamNoQuoteInfos_OnlyOneReattemptIsQueued()
    {
        moqMarketConnectionConfigRepo.Reset();
        var scheduleCounter = 0;
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((its, callback, timeout, callOnce) => { scheduleCounter++; })
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
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((its, callback, timeout, callOnce) => { scheduleCounter++; })
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
    public void NewPQClient_GetSourceServerConfig_DoesntFindSnapshotUpdatePricingServerConfig()
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
    public void NewPQClient_RequestSnapshots_DoesntFindSnapshotClientDoesNothing()
    {
        moqSnapshotClientFactory.Reset();
        moqSnapshotClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSnapshotClient.Object).Verifiable();


        var moqClientSnapshotServerConfig = new Mock<INetworkTopicConnectionConfig>();
        pqClient.RequestSnapshots(moqClientSnapshotServerConfig.Object, new List<ISourceTickerQuoteInfo>());

        moqSnapshotClientFactory.Verify();
    }
}
