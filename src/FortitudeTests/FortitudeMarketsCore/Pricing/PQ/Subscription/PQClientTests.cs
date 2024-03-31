#region

using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQClientTests
{
    private readonly uint firstSourceTickerPublicationConfigId = 123;
    private readonly uint secondSourceTickerPublicationConfigId = 234;
    private bool defaultAllowCatchUps;
    private uint defaultSyncRetryInterval;
    private PQLevel0Quote dummyLevel0Quote = null!;
    private PQLevel1Quote dummyLevel1Quote = null!;
    private PQLevel2Quote dummyLevel2Quote = null!;
    private PQLevel3Quote dummyLevel3Quote = null!;
    private string firstTestSourceName = null!;
    private string firstTestTicker = null!;
    private Mock<IDisposable> moqDeserializerSubscription = null!;
    private Mock<ISocketDispatcher> moqFirstSocketDispatcher = null!;
    private Mock<IMutableSourceTickerPublicationConfig> moqFirstTestTickerPublicationConfig = null!;
    private Mock<IMessageStreamDecoder> moqMessageStreamDecoder = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQClientSyncMonitoring> moqPQClientSyncMonitoring = null!;
    private Mock<IPQDeserializer<PQLevel0Quote>> moqPQLvl0QuoteSerializer = null!;
    private Mock<IPQDeserializer<PQLevel1Quote>> moqPQLvl1QuoteSerializer = null!;
    private Mock<IPQDeserializer<PQLevel2Quote>> moqPQLvl2QuoteSerializer = null!;
    private Mock<IPQDeserializer<PQLevel3Quote>> moqPQLvl3QuoteSerializer = null!;
    private Mock<IPQDeserializer> moqPQQuoteSerializer = null!;
    private Mock<IPQQuoteSerializerRepository> moqPQQuoteSerializerRepo = null!;
    private Mock<IPricingClientConfigRepository> moqPricingServersConfigRepo = null!;
    private Mock<IMutableSourceTickerPublicationConfig> moqSecondTestTickerPublicationConfig = null!;
    private Mock<IIntraOSThreadSignal> moqSingleOsThreadSignal = null!;
    private Mock<IPQSnapshotClient> moqSnapshotClient = null!;
    private Mock<IPQConversationRepository<IPQSnapshotClient>> moqSnapshotClientFactory = null!;
    private Mock<IEndpointConfig> moqSnapshotServerConfig = null!;
    private Mock<INetworkTopicConnectionConfig> moqSnapshotTopicServerConfig = null!;
    private Mock<ISnapshotUpdatePricingServerConfig> moqSnapshotUpdatePricingServerConfig = null!;
    private Mock<ISocketDispatcherResolver> moqSocketDispatcherResolver = null!;
    private Mock<IPQUpdateClient> moqUpdateClient = null!;
    private Mock<IPQConversationRepository<IPQUpdateClient>> moqUpdateClientFactory = null!;
    private Mock<IEndpointConfig> moqUpdateServerConfig = null!;
    private Mock<INetworkTopicConnectionConfig> moqUpdateTopicServerConfig = null!;
    private PQClient pqClient = null!;
    private string secondTestSourceName = null!;
    private string secondTestTicker = null!;
    private List<ISourceTickerPublicationConfig> serverTickerConfigs = null!;

    [TestInitialize]
    public void SetUp()
    {
        PrepareInitialValues();
        PrepareNewPQClientInitializationMocks();

        PrepareQuoteLevelMocks();

        PrepareGetQuoteStreamMocks();
        pqClient = new PQClient(moqPricingServersConfigRepo.Object, moqSocketDispatcherResolver.Object,
            defaultAllowCatchUps, moqUpdateClientFactory.Object, moqSnapshotClientFactory.Object);

        NonPublicInvocator.SetInstanceField(pqClient, "pqClientSyncMonitoring", moqPQClientSyncMonitoring.Object);
        NonPublicInvocator.SetInstanceField(pqClient, "snapshotSerializationRepository"
            , moqPQQuoteSerializerRepo.Object);
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

        moqFirstTestTickerPublicationConfig = new Mock<IMutableSourceTickerPublicationConfig>();
        moqFirstTestTickerPublicationConfig.As<IMutableUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Source)
            .Returns(firstTestSourceName);
        moqFirstTestTickerPublicationConfig.As<IMutableUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Ticker)
            .Returns(firstTestTicker);
        moqFirstTestTickerPublicationConfig.As<IMutableUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Id)
            .Returns(firstSourceTickerPublicationConfigId);
        moqFirstTestTickerPublicationConfig.As<IUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Source)
            .Returns(firstTestSourceName);
        moqFirstTestTickerPublicationConfig.As<IUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Ticker)
            .Returns(firstTestTicker);
        moqFirstTestTickerPublicationConfig.As<IUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Id)
            .Returns(firstSourceTickerPublicationConfigId);
        moqFirstTestTickerPublicationConfig.SetupGet(stpc => stpc.Source)
            .Returns(firstTestSourceName);
        moqFirstTestTickerPublicationConfig.SetupGet(stpc => stpc.Ticker)
            .Returns(firstTestTicker);
        moqFirstTestTickerPublicationConfig.SetupGet(stpc => stpc.Id)
            .Returns(firstSourceTickerPublicationConfigId);
        moqSecondTestTickerPublicationConfig = new Mock<IMutableSourceTickerPublicationConfig>();
        moqSecondTestTickerPublicationConfig.As<IMutableUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Source)
            .Returns(secondTestSourceName);
        moqSecondTestTickerPublicationConfig.As<IMutableUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Ticker)
            .Returns(secondTestTicker);
        moqSecondTestTickerPublicationConfig.As<IMutableUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Id)
            .Returns(secondSourceTickerPublicationConfigId);
        moqSecondTestTickerPublicationConfig.As<IUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Source)
            .Returns(secondTestSourceName);
        moqSecondTestTickerPublicationConfig.As<IUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Ticker)
            .Returns(secondTestTicker);
        moqSecondTestTickerPublicationConfig.As<IUniqueSourceTickerIdentifier>().SetupGet(stpc => stpc.Id)
            .Returns(secondSourceTickerPublicationConfigId);
        moqSecondTestTickerPublicationConfig.SetupGet(stpc => stpc.Source)
            .Returns(secondTestSourceName);
        moqSecondTestTickerPublicationConfig.SetupGet(stpc => stpc.Ticker)
            .Returns(secondTestTicker);
        moqSecondTestTickerPublicationConfig.SetupGet(stpc => stpc.Id)
            .Returns(secondSourceTickerPublicationConfigId);
        serverTickerConfigs = new List<ISourceTickerPublicationConfig>
        {
            moqFirstTestTickerPublicationConfig.Object, moqSecondTestTickerPublicationConfig.Object
        };
        moqSnapshotUpdatePricingServerConfig = new Mock<ISnapshotUpdatePricingServerConfig>();
        moqSnapshotUpdatePricingServerConfig.SetupGet(supsc => supsc.SourceTickerPublicationConfigs)
            .Returns(serverTickerConfigs).Verifiable();
        moqPricingServersConfigRepo = new Mock<IPricingClientConfigRepository>();
        moqPricingServersConfigRepo.Setup(pscr => pscr.Find(It.IsAny<string>()))
            .Returns(moqSnapshotUpdatePricingServerConfig.Object);


        moqSnapshotClientFactory = new Mock<IPQConversationRepository<IPQSnapshotClient>>();
        moqSnapshotClient = new Mock<IPQSnapshotClient>();
        moqMessageStreamDecoder = new Mock<IMessageStreamDecoder>();
        moqSnapshotClient.SetupGet(sc => sc.MessageStreamDecoder).Returns(moqMessageStreamDecoder.Object);
        moqSnapshotClientFactory
            .Setup(scf => scf.RetrieveOrCreateConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSnapshotClient.Object);
        moqSnapshotClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSnapshotClient.Object);

        moqUpdateClientFactory = new Mock<IPQConversationRepository<IPQUpdateClient>>();
        moqUpdateClient = new Mock<IPQUpdateClient>();
        moqUpdateClient.SetupGet(sc => sc.MessageStreamDecoder).Returns(moqMessageStreamDecoder.Object);
        moqUpdateClientFactory.Setup(scf => scf.RetrieveOrCreateConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqUpdateClient.Object);
        moqUpdateClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqUpdateClient.Object);

        moqFirstSocketDispatcher = new Mock<ISocketDispatcher>();
        moqFirstSocketDispatcher.SetupAllProperties();
    }

    private void PrepareInitialValues()
    {
        defaultAllowCatchUps = true;
        defaultSyncRetryInterval = 60000;
        firstTestSourceName = "FirstTestSourceName";
        firstTestTicker = "FirstTestTicker";
        secondTestSourceName = "SecondTestSourceName";
        secondTestTicker = "SecondTestTicker";
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
        moqSnapshotUpdatePricingServerConfig.SetupGet(supsc => supsc.Name).Returns("TestServerName").Verifiable();

        moqSnapshotServerConfig = new Mock<IEndpointConfig>();
        moqSnapshotTopicServerConfig = new Mock<INetworkTopicConnectionConfig>();
        moqSnapshotTopicServerConfig.SetupGet(supsc => supsc.Current).Returns(moqSnapshotServerConfig.Object);
        moqSnapshotServerConfig.SetupGet(scc => scc.Hostname).Returns("SnapshotHostName");
        moqSnapshotUpdatePricingServerConfig.SetupGet(supsc => supsc.SnapshotConnectionConfig)
            .Returns(moqSnapshotTopicServerConfig.Object).Verifiable();
        moqSnapshotUpdatePricingServerConfig.SetupGet(supsc => supsc.UpdateConnectionConfig)
            .Returns(moqUpdateTopicServerConfig.Object);
    }

    private void PrepareQuoteLevelMocks()
    {
        dummyLevel0Quote = new PQLevel0Quote(moqFirstTestTickerPublicationConfig.Object);
        dummyLevel1Quote = new PQLevel1Quote(moqFirstTestTickerPublicationConfig.Object);
        dummyLevel2Quote = new PQLevel2Quote(moqFirstTestTickerPublicationConfig.Object);
        dummyLevel3Quote = new PQLevel3Quote(moqFirstTestTickerPublicationConfig.Object);

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

        moqPQQuoteSerializerRepo = new Mock<IPQQuoteSerializerRepository>();
        moqPQQuoteSerializerRepo.Setup(pqqsf => pqqsf.CreateQuoteDeserializer<PQLevel0Quote>(
                It.IsAny<ISourceTickerClientAndPublicationConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteSerializerRepo.Setup(pqqsf => pqqsf.CreateQuoteDeserializer<PQLevel1Quote>(
                It.IsAny<ISourceTickerClientAndPublicationConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteSerializerRepo.Setup(pqqsf => pqqsf.CreateQuoteDeserializer<PQLevel2Quote>(
                It.IsAny<ISourceTickerClientAndPublicationConfig>()))
            .Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteSerializerRepo.Setup(pqqsf => pqqsf.CreateQuoteDeserializer<PQLevel3Quote>(
                It.IsAny<ISourceTickerClientAndPublicationConfig>()))
            .Returns(moqPQQuoteSerializer.Object);

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
            Mock<IMutableSourceTickerPublicationConfig>? sourceTickerQuoteInfo = null) where T : PQLevel0Quote, new()
    {
        sourceTickerQuoteInfo ??= moqFirstTestTickerPublicationConfig;
        var result = pqClient.GetQuoteStream<T>(sourceTickerQuoteInfo.Object, defaultSyncRetryInterval);

        Assert.IsNotNull(result);
        sourceTickerQuoteInfo.Verify();
        moqPricingServersConfigRepo.Verify();
        moqPQQuoteSerializerRepo.Verify();
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
            = pqClient.GetQuoteStream<T>(moqFirstTestTickerPublicationConfig.Object, defaultSyncRetryInterval);

        PrepareUnsubscribeMocks();

        subQuoteStream!.Unsubscribe();

        moqPQQuoteSerializerRepo.Verify();
        moqUpdateClientFactory.Verify();
        moqSnapshotClientFactory.Verify();
        moqPQClientSyncMonitoring.Verify();
    }

    private void PrepareUnsubscribeMocks()
    {
        moqPQQuoteSerializerRepo.Setup(pqqsf => pqqsf.GetQuoteDeserializer(
            moqFirstTestTickerPublicationConfig.Object)).Returns(moqPQQuoteSerializer.Object);
        moqPQQuoteSerializerRepo.Setup(pqqsf => pqqsf.GetQuoteDeserializer(
            moqSecondTestTickerPublicationConfig.Object)).Returns(moqPQQuoteSerializer.Object);

        moqUpdateClientFactory.Setup(sscf => sscf.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqSnapshotClientFactory.Setup(sscf => sscf.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(pqcsm => pqcsm.UnregisterSerializer(It.IsAny<IPQDeserializer>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(pqcsm => pqcsm.CheckStopMonitoring()).Verifiable();
        moqPQQuoteSerializerRepo.SetupGet(pqqsf => pqqsf.HasPictureDeserializers).Returns(false).Verifiable();
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
            moqSecondTestTickerPublicationConfig);

        Assert.AreNotEqual(firstSub, secondSub);
    }

    [TestMethod]
    public void SubscribedLevel0Stream_GetQuoteStreamNoMulticastLevel1QuoteSameSourceDiffTicker_TwoDiffStreams()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel0Quote>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel1Quote>(
            moqSecondTestTickerPublicationConfig);

        Assert.AreNotEqual(firstSub, secondSub);
    }

    [TestMethod]
    public void SubscribedLevel0ForTwoDiffCcyStream_Unsubscribe1Stream_LeavesSyncMonitorRunning()
    {
        var firstSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel0Quote>();

        var secondSub = NewPQClientTyped_GetQuoteStreamNoMulticast_RegistersAndReturnsQuote<PQLevel1Quote>(
            moqSecondTestTickerPublicationConfig);

        Assert.AreNotEqual(firstSub, secondSub);
        moqPQQuoteSerializerRepo.Reset();
        moqPQQuoteSerializer.Reset();
        moqFirstTestTickerPublicationConfig.Reset();
        moqFirstTestTickerPublicationConfig.SetupGet(stpc => stpc.Ticker)
            .Returns(firstTestTicker);
        moqFirstTestTickerPublicationConfig.As<ISourceTickerPublicationConfig>().SetupGet(stpc => stpc.Id)
            .Returns(firstSourceTickerPublicationConfigId);
        moqFirstTestTickerPublicationConfig.As<ISourceTickerQuoteInfo>().SetupGet(stpc => stpc.Ticker)
            .Returns(secondTestTicker);
        moqFirstTestTickerPublicationConfig.As<ISourceTickerQuoteInfo>().SetupGet(stpc => stpc.Source)
            .Returns(secondTestSourceName);
        moqPQQuoteSerializerRepo.Setup(pqqsf => pqqsf.GetQuoteDeserializer(
            moqFirstTestTickerPublicationConfig.Object)).Returns(moqPQQuoteSerializer.Object).Verifiable();

        moqUpdateClientFactory.Setup(sscf => sscf.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqSnapshotClientFactory.Setup(sscf => sscf.RemoveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Verifiable();
        moqPQClientSyncMonitoring.Setup(pqcsm => pqcsm.UnregisterSerializer(It.IsAny<IPQDeserializer>()))
            .Verifiable();
        moqPQQuoteSerializerRepo.SetupGet(pqqsf => pqqsf.HasPictureDeserializers).Returns(true).Verifiable();

        firstSub.Unsubscribe();

        moqPQQuoteSerializerRepo.Verify();
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
            moqSecondTestTickerPublicationConfig);

        Assert.AreNotEqual(firstSub, secondSub);
        moqPQQuoteSerializerRepo.Reset();
        moqFirstTestTickerPublicationConfig.Reset();
        moqSecondTestTickerPublicationConfig.Reset();
        moqPQQuoteSerializer.Reset();
        moqFirstTestTickerPublicationConfig.SetupGet(stpc => stpc.Ticker)
            .Returns(firstTestTicker);
        moqFirstTestTickerPublicationConfig.As<ISourceTickerPublicationConfig>().SetupGet(stpc => stpc.Id)
            .Returns(firstSourceTickerPublicationConfigId);
        moqSecondTestTickerPublicationConfig.SetupGet(stpc => stpc.Ticker)
            .Returns(secondTestTicker);
        moqSecondTestTickerPublicationConfig.As<ISourceTickerPublicationConfig>().SetupGet(stpc => stpc.Id)
            .Returns(secondSourceTickerPublicationConfigId);
        moqPQQuoteSerializerRepo.SetupGet(pqqsf => pqqsf.HasPictureDeserializers).Returns(false).Verifiable();
        PrepareUnsubscribeMocks();

        pqClient.Dispose();

        moqPQQuoteSerializerRepo.Verify();
        moqUpdateClientFactory.Verify();
        moqSnapshotClientFactory.Verify();
        moqPQClientSyncMonitoring.Verify();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReturnsNullQueuesInfoRequestAttempt()
    {
        moqPricingServersConfigRepo.Reset();
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
            It.IsAny<WaitOrTimerCallback>(), 60000, true)).Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestTickerPublicationConfig.Object
                , defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqParallelController.Verify();
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReattemptFindsInfoNoMoreAttemptsAreQueued()
    {
        moqPricingServersConfigRepo.Reset();
        moqSnapshotUpdatePricingServerConfig.Reset();
        WaitOrTimerCallback? queuedReattemptHandler = null;
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((its, callback, timeout, callOnce) =>
            {
                queuedReattemptHandler = callback;
            }).Returns(new Mock<ITimerCallbackSubscription>().Object).Verifiable();
        var subQuoteStream
            = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestTickerPublicationConfig.Object
                , defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqParallelController.Verify();

        moqParallelController.Reset();
        moqPricingServersConfigRepo.Setup(pscr => pscr.Find(It.IsAny<string>()))
            .Returns(moqSnapshotUpdatePricingServerConfig.Object);
        Assert.IsNotNull(queuedReattemptHandler);
        queuedReattemptHandler(null, true);
        moqPricingServersConfigRepo.Verify();

        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
            It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Never);
    }

    [TestMethod]
    public void NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReattemptStillDoesntFindInfoMoreAttemptsAreQueued()
    {
        moqPricingServersConfigRepo.Reset();
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
            = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestTickerPublicationConfig.Object
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
        moqPricingServersConfigRepo.Reset();
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
            = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestTickerPublicationConfig.Object
                , defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqParallelController.Verify();

        moqParallelController.Reset();
        pqClient.Dispose();
        moqSingleOsThreadSignal.Verify();
        Assert.IsNotNull(queuedReattemptHandler);
        queuedReattemptHandler(null, true);
        moqPricingServersConfigRepo.Verify();

        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
            It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Never);
    }

    [TestMethod]
    public void NewPQClient_MultipleGetQuoteStreamNoQuoteInfos_OnlyOneReattemptIsQueued()
    {
        moqPricingServersConfigRepo.Reset();
        var scheduleCounter = 0;
        moqParallelController.Setup(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
                It.IsAny<WaitOrTimerCallback>(), 60000, true))
            .Callback<IIntraOSThreadSignal, WaitOrTimerCallback, uint, bool>((its, callback, timeout, callOnce) => { scheduleCounter++; })
            .Returns(new Mock<ITimerCallbackSubscription>().Object)
            .Verifiable();
        var subQuoteStream = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestTickerPublicationConfig.Object,
            defaultSyncRetryInterval);
        var subQuoteStream2 = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestTickerPublicationConfig.Object,
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

        var diffQuoteInfo = new Mock<IMutableSourceTickerPublicationConfig>();

        diffQuoteInfo.As<ISourceTickerQuoteInfo>().SetupGet(stqi => stqi.Ticker)
            .Returns("NewUnknownTicker").Verifiable();
        diffQuoteInfo.As<ISourceTickerQuoteInfo>().SetupGet(stqi => stqi.Source)
            .Returns(firstTestSourceName).Verifiable();

        var subQuoteStream = pqClient.GetQuoteStream<PQLevel0Quote>(diffQuoteInfo.Object,
            defaultSyncRetryInterval);

        Assert.IsNull(subQuoteStream);
        moqFirstTestTickerPublicationConfig.Verify();
        moqParallelController.Verify(pc => pc.ScheduleWithEarlyTrigger(moqSingleOsThreadSignal.Object,
            It.IsAny<WaitOrTimerCallback>(), 60000, true), Times.Never);
        Assert.AreEqual(0, scheduleCounter);
    }

    [TestMethod]
    public void SubscribedLevel0_GetSourceServerConfig_FindsSnapshotUpdatePricingServerConfig()
    {
        var subQuoteStream = pqClient.GetQuoteStream<PQLevel0Quote>(moqFirstTestTickerPublicationConfig.Object,
            defaultSyncRetryInterval);

        Assert.IsNotNull(subQuoteStream);
        var pricingServerConfig = pqClient.GetSourceServerConfig(moqFirstTestTickerPublicationConfig.Object.Source);
        Assert.IsNotNull(pricingServerConfig);
        Assert.AreEqual(moqSnapshotUpdatePricingServerConfig.Object, pricingServerConfig);
    }

    [TestMethod]
    public void NewPQClient_GetSourceServerConfig_DoesntFindSnapshotUpdatePricingServerConfig()
    {
        var pricingServerConfig = pqClient.GetSourceServerConfig(moqFirstTestTickerPublicationConfig.Object.Source);
        Assert.IsNull(pricingServerConfig);
    }

    [TestMethod]
    public void SubscribedLevel0Stream_RequestSnapshots_FindsSnapshotClientForwardsStreamIdRequests()
    {
        moqSnapshotClient.Setup(sc => sc.RequestSnapshots(It.IsAny<List<IUniqueSourceTickerIdentifier>>()))
            .Verifiable();
        moqSnapshotClientFactory.Reset();
        moqSnapshotClientFactory.Setup(scf => scf.RetrieveConversation(It.IsAny<INetworkTopicConnectionConfig>()))
            .Returns(moqSnapshotClient.Object).Verifiable();

        var moqClientSnapshotServerConfig = new Mock<INetworkTopicConnectionConfig>();
        pqClient.RequestSnapshots(moqClientSnapshotServerConfig.Object, new List<IUniqueSourceTickerIdentifier>());

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
        pqClient.RequestSnapshots(moqClientSnapshotServerConfig.Object, new List<IUniqueSourceTickerIdentifier>());

        moqSnapshotClientFactory.Verify();
    }
}
