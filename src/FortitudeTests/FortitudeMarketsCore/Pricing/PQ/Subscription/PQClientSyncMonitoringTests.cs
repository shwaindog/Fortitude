#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQClientSyncMonitoringTests
{
    private readonly string nonExistentSource = "TestNonExistentSource";
    private DateTime baseTime;
    private Func<string, IMarketConnectionConfig?> getSourceServerConfigFunc = null!;
    private List<ISourceTickerQuoteInfo> historyOfCalledSourceTickerIds = null!;
    private bool inSequencerSerialize;
    private INetworkTopicConnectionConfig? lastConnectionConfig;
    private string? lastSourceName;
    private List<ISourceTickerQuoteInfo> lastUniqueSourceTickerIdentifiers = null!;
    private ThreadStart monitorAction = null!;
    private Mock<IPQDeserializer> moqFirstQuoteDeserializer = null!;
    private Mock<ISourceTickerQuoteInfo> moqFirstQuoteDeserializerIdentifier = null!;
    private Mock<IFLogger> moqLogger = null!;
    private Mock<IMarketConnectionConfig> moqMarketConnectionConfig = null!;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQDeserializer> moqSecondQuoteDeserializer = null!;
    private Mock<ISourceTickerQuoteInfo> moqSecondQuoteDeserializerIdentifier = null!;
    private Mock<ISequencer> moqSequencer = null!;
    private Mock<INetworkTopicConnectionConfig> moqSnapshotServerConnectionConfig = null!;
    private Mock<IPricingServerConfig> moqSnapshotUpdatePricingServerConfig = null!;
    private Mock<IIntraOSThreadSignal> moqStopSignal = null!;
    private Mock<IDoublyLinkedList<IPQDeserializer>> moqSyncKo = null!;
    private Mock<IDoublyLinkedList<IPQDeserializer>> moqSyncOk = null!;
    private Mock<ITimeContext> moqTimeContext = null!;
    private Mock<IEndpointConfig> moqUpdateServerConnectionConfig = null!;
    private PQClientSyncMonitoring pqClientSyncMonitoring = null!;
    private long sequencerSequence;

    private Action<INetworkTopicConnectionConfig, List<ISourceTickerQuoteInfo>>
        snapShotRequestActionFunc = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqStopSignal = new Mock<IIntraOSThreadSignal>();
        moqParallelController = new Mock<IOSParallelController>();
        moqParallelController.Setup(pc => pc.SingleOSThreadActivateSignal(false)).Returns(moqStopSignal.Object)
            .Verifiable();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object).Verifiable();
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        moqSnapshotUpdatePricingServerConfig = new Mock<IPricingServerConfig>();
        moqMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqSnapshotServerConnectionConfig = new Mock<INetworkTopicConnectionConfig>();
        moqUpdateServerConnectionConfig = new Mock<IEndpointConfig>();
        moqMarketConnectionConfig.SetupGet(mcc => mcc.PricingServerConfig).Returns(moqSnapshotUpdatePricingServerConfig.Object);
        moqSnapshotUpdatePricingServerConfig.SetupGet(supsc => supsc.SnapshotConnectionConfig)
            .Returns(moqSnapshotServerConnectionConfig.Object).Callback(() => { pqClientSyncMonitoring.CheckStopMonitoring(); })
            .Verifiable();
        lastSourceName = null;
        getSourceServerConfigFunc = srcName =>
        {
            lastSourceName = srcName;
            return srcName != nonExistentSource ? moqMarketConnectionConfig.Object : null;
        };

        historyOfCalledSourceTickerIds = new List<ISourceTickerQuoteInfo>();
        lastConnectionConfig = null;
        snapShotRequestActionFunc = (scc, stList) =>
        {
            lastConnectionConfig = scc;
            historyOfCalledSourceTickerIds.AddRange(stList);
            lastUniqueSourceTickerIdentifiers = stList;
        };
        pqClientSyncMonitoring = new PQClientSyncMonitoring(getSourceServerConfigFunc, snapShotRequestActionFunc);

        moqSyncKo = new Mock<IDoublyLinkedList<IPQDeserializer>>();
        moqSyncKo.SetupAllProperties();
        moqSyncOk = new Mock<IDoublyLinkedList<IPQDeserializer>>();
        moqSyncOk.SetupAllProperties();

        NonPublicInvocator.SetInstanceField(pqClientSyncMonitoring, "syncKo", moqSyncKo.Object);
        NonPublicInvocator.SetInstanceField(pqClientSyncMonitoring, "syncOk", moqSyncOk.Object);

        moqOsThread = new Mock<IOSThread>();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        moqParallelController.Setup(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>())).Returns(moqOsThread.Object)
            .Callback<ThreadStart>(ts => { monitorAction = ts; }).Verifiable();

        moqFirstQuoteDeserializer = new Mock<IPQDeserializer>();
        moqFirstQuoteDeserializer.SetupAllProperties();
        moqSecondQuoteDeserializer = new Mock<IPQDeserializer>();
        moqSecondQuoteDeserializer.SetupAllProperties();

        sequencerSequence = 1;
        moqSequencer = new Mock<ISequencer>();
        moqSequencer.Setup(s => s.Claim()).Returns(sequencerSequence).Verifiable();
        inSequencerSerialize = false;
        moqSequencer.Setup(s => s.Serialize(sequencerSequence))
            .Callback(() => inSequencerSerialize = true).Verifiable();
        moqSequencer.Setup(s => s.Release(sequencerSequence))
            .Callback(() => inSequencerSerialize = false).Verifiable();
        NonPublicInvocator.SetInstanceField(pqClientSyncMonitoring, "pqSeq", moqSequencer.Object);

        moqTimeContext = new Mock<ITimeContext>();
        TimeContext.Provider = moqTimeContext.Object;
        baseTime = new DateTime(2017, 06, 12, 15, 50, 39);

        moqLogger = new Mock<IFLogger>();
        NonPublicInvocator.SetStaticField(typeof(PQClientSyncMonitoring), "Logger", moqLogger.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
        TimeContext.Provider = new HighPrecisionTimeContext();
    }

    [TestMethod]
    public void NewClientSyncMonitor_New_CreatesStopThreadSignal()
    {
        moqParallelController.Verify(pc => pc.SingleOSThreadActivateSignal(false), Times.Once);
    }

    [TestMethod]
    public void NewClientSyncMonitor_RegisterNewDeserializer_AddsCallbacksToDeserializerAddsDeserializerToKoList()
    {
        moqSyncKo.Setup(sok => sok.AddFirst(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();

        pqClientSyncMonitoring.RegisterNewDeserializer(moqFirstQuoteDeserializer.Object);
        Assert.IsFalse(inSequencerSerialize);
        moqSyncKo.Verify();
    }

    [TestMethod]
    public void RegisteredKoDeserializer_UnregisterDeserializer_RemovesFromKoList()
    {
        moqSyncKo.Setup(sko => sko.SafeContains(moqFirstQuoteDeserializer.Object)).Returns(true).Verifiable();
        moqSyncKo.Setup(sko => sko.Remove(moqFirstQuoteDeserializer.Object)).Returns(moqFirstQuoteDeserializer.Object)
            .Verifiable();
        moqSyncOk.Setup(sok => sok.SafeContains(moqFirstQuoteDeserializer.Object)).Returns(false).Verifiable();
        pqClientSyncMonitoring.UnregisterSerializer(moqFirstQuoteDeserializer.Object);
        moqSyncKo.Verify();
        moqSyncOk.Verify();
        moqSequencer.Verify();
    }

    [TestMethod]
    public void RegisteredOkDeserializer_UnregisterDeserializer_RemovesFromKoList()
    {
        moqSyncOk.Setup(sok => sok.SafeContains(moqFirstQuoteDeserializer.Object)).Returns(true).Verifiable();
        moqSyncOk.Setup(sok => sok.Remove(moqFirstQuoteDeserializer.Object)).Returns(moqFirstQuoteDeserializer.Object)
            .Verifiable();
        moqSyncKo.Setup(sko => sko.SafeContains(moqFirstQuoteDeserializer.Object)).Returns(false).Verifiable();
        pqClientSyncMonitoring.UnregisterSerializer(moqFirstQuoteDeserializer.Object);
        moqSyncKo.Verify();
        moqSyncOk.Verify();
        moqSequencer.Verify();
    }

    [TestMethod]
    public void TwoDeserializersWithKnownOrder_OnUpdate_SyncProtectsMovesUpdatedDeserializerToEndOfSyncOk()
    {
        pqClientSyncMonitoring.RegisterNewDeserializer(moqFirstQuoteDeserializer.Object);
        moqSyncOk.Setup(sok => sok.Remove(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();
        moqSyncOk.Setup(sok => sok.AddLast(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();
        moqFirstQuoteDeserializer.Raise(qu => qu.ReceivedUpdate += NoOp, moqFirstQuoteDeserializer.Object);
        moqSyncOk.Verify();
        moqSequencer.Verify();
    }

    [TestMethod]
    public void UnSyncedSerializer_OnSyncOk_SyncProtectsMovesUpdatedSerializerToEndOfSyncOk()
    {
        pqClientSyncMonitoring.RegisterNewDeserializer(moqFirstQuoteDeserializer.Object);
        moqSyncKo.Setup(sko => sko.Remove(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();
        moqSyncOk.Setup(sok => sok.AddLast(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();
        moqFirstQuoteDeserializer.Raise(qu => qu.SyncOk += NoOp, moqFirstQuoteDeserializer.Object);
        moqSyncOk.Verify();
        moqSyncKo.Verify();
        moqSequencer.Verify();
    }

    [TestMethod]
    public void SyncedDeserializer_OnSyncKo_SyncProtectsMovesDeserializerToFromOkToKo()
    {
        pqClientSyncMonitoring.RegisterNewDeserializer(moqFirstQuoteDeserializer.Object);
        moqSyncOk.Setup(sok => sok.Remove(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();
        moqSyncKo.Setup(sko => sko.AddFirst(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();
        moqFirstQuoteDeserializer.Raise(qu => qu.OutOfSync += NoOp, moqFirstQuoteDeserializer.Object);
        moqSyncOk.Verify();
        moqSyncKo.Verify();
        moqSequencer.Verify();
    }

    [TestMethod]
    public void NewUnstartedClientSyncMonitoring_CheckStartMonitoring_CreateStartsNewBackgroundThread()
    {
        pqClientSyncMonitoring.CheckStartMonitoring();

        Assert.IsNotNull(monitorAction);
        moqOsThread.Verify();
    }

    [TestMethod]
    public void StartedClientSyncMonitoring_CheckStartMonitoring_DoesNotRestartThread()
    {
        pqClientSyncMonitoring.CheckStartMonitoring();
        Assert.IsNotNull(monitorAction);
        moqOsThread.Verify();

        pqClientSyncMonitoring.CheckStartMonitoring();

        moqOsThread.Verify(ost => ost.Start(), Times.Once);
        moqParallelController.Verify(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>()), Times.Once);
    }

    [TestMethod]
    public void StartedClientSyncMonitoring_CheckStopMonitoring_FlagsMonitoringToFinishWaitsForThreadToJoin()
    {
        moqOsThread.Setup(ost => ost.Join()).Verifiable();
        moqStopSignal.Setup(iosts => iosts.Set()).Verifiable();
        pqClientSyncMonitoring.CheckStartMonitoring();

        pqClientSyncMonitoring.CheckStopMonitoring();

        moqOsThread.Verify();
        moqStopSignal.Verify();
    }

    [TestMethod]
    public void StartedThenStopedClientSyncMonitoring_CheckStopMonitoring_DoesNothing()
    {
        moqOsThread.Setup(ost => ost.Join()).Verifiable();
        moqStopSignal.Setup(iosts => iosts.Set()).Verifiable();
        pqClientSyncMonitoring.CheckStartMonitoring();
        pqClientSyncMonitoring.CheckStopMonitoring();
        moqOsThread.Verify(ost => ost.Join(), Times.Once);
        moqStopSignal.Verify(iosts => iosts.Set(), Times.Once);

        pqClientSyncMonitoring.CheckStopMonitoring();

        moqOsThread.Verify(ost => ost.Join(), Times.Once);
        moqStopSignal.Verify(iosts => iosts.Set(), Times.Once);
    }

    [TestMethod]
    public void NewClientSyncMonitoring_MonitorDeserializersForSnapshotResync_RequestsSnapshotValues()
    {
        pqClientSyncMonitoring.CheckStartMonitoring();
        PrepareMonitorDeserializersForSnapshotResyncMoqs();

        monitorAction();

        moqStopSignal.Verify();
        moqSequencer.Verify(s => s.Claim(), Times.Exactly(5));
        moqSequencer.Verify(s => s.Serialize(sequencerSequence), Times.Exactly(5));
        moqSequencer.Verify(s => s.Release(sequencerSequence), Times.Exactly(5));
        moqSyncOk.Verify();
        moqSyncKo.Verify();
        moqFirstQuoteDeserializer.Verify();
        moqSecondQuoteDeserializer.Verify();
        Assert.AreEqual("TestFirstSource", lastSourceName);
        Assert.AreEqual(moqSnapshotServerConnectionConfig.Object, lastConnectionConfig);
        Assert.IsNotNull(lastUniqueSourceTickerIdentifiers);
        Assert.AreEqual(1, lastUniqueSourceTickerIdentifiers.Count);
        Assert.AreEqual(moqFirstQuoteDeserializerIdentifier.Object, lastUniqueSourceTickerIdentifiers[0]);
    }

    [TestMethod]
    public void OneIterationTakesTooLong_MonitorDeserializersForSnapshotResync_LogsWarningThatTasksAreSlow()
    {
        pqClientSyncMonitoring.CheckStartMonitoring();
        PrepareMonitorDeserializersForSnapshotResyncMoqs();
        moqTimeContext.SetupSequence(tc => tc.UtcNow).Returns(baseTime).Returns(baseTime.AddMilliseconds(200))
            .Returns(baseTime.AddMilliseconds(1001)).Returns(baseTime.AddMilliseconds(1002))
            .Returns(baseTime.AddMilliseconds(1003)).Returns(baseTime.AddMilliseconds(1004))
            .Returns(baseTime.AddMilliseconds(2002)).Returns(baseTime.AddMilliseconds(2003))
            .Returns(baseTime.AddMilliseconds(2004)).Returns(baseTime.AddMilliseconds(2005));
        moqSyncKo.SetupSequence(sko => sko.Head).Returns(moqFirstQuoteDeserializer.Object)
            .Returns(moqSecondQuoteDeserializer.Object).Returns(moqFirstQuoteDeserializer.Object)
            .Returns(moqFirstQuoteDeserializer.Object).Returns(moqSecondQuoteDeserializer.Object)
            .Returns(moqFirstQuoteDeserializer.Object);
        moqSyncOk.SetupSequence(sok => sok.Head).Returns(moqFirstQuoteDeserializer.Object)
            .Returns(null as IPQDeserializer)
            .Returns(moqFirstQuoteDeserializer.Object).Returns(null as IPQDeserializer);
        var count = 0;
        moqSnapshotUpdatePricingServerConfig.SetupGet(supsc => supsc.SnapshotConnectionConfig)
            .Returns(moqSnapshotServerConnectionConfig.Object).Callback(() =>
            {
                if (count++ >= 1) pqClientSyncMonitoring.CheckStopMonitoring();
            }).Verifiable();
        moqLogger.Setup(fl => fl.Warn("Tasks scheduler slow time in Ms:{0}", 1001)).Verifiable();

        monitorAction();

        moqLogger.Verify();
        moqStopSignal.Verify();
        moqSequencer.Verify(s => s.Claim(), Times.Exactly(10));
        moqSequencer.Verify(s => s.Serialize(sequencerSequence), Times.Exactly(10));
        moqSequencer.Verify(s => s.Release(sequencerSequence), Times.Exactly(10));
        moqSyncOk.Verify();
        moqSyncKo.Verify();
        moqFirstQuoteDeserializer.Verify();
        moqSecondQuoteDeserializer.Verify();
        Assert.AreEqual("TestFirstSource", lastSourceName);
        Assert.AreEqual(moqSnapshotServerConnectionConfig.Object, lastConnectionConfig);
        Assert.IsNotNull(lastUniqueSourceTickerIdentifiers);
        Assert.AreEqual(1, lastUniqueSourceTickerIdentifiers.Count);
        Assert.AreEqual(moqFirstQuoteDeserializerIdentifier.Object, lastUniqueSourceTickerIdentifiers[0]);
    }

    [TestMethod]
    public void TwoTickersFromSameSource_MonitorDeserializersForSnapshotResync_RequestBothAtSameTime()
    {
        pqClientSyncMonitoring.CheckStartMonitoring();
        PrepareMonitorDeserializersForSnapshotResyncMoqs();
        moqSecondQuoteDeserializer.Setup(qu => qu.CheckResync(It.IsAny<DateTime>())).Returns(true).Verifiable();
        moqSecondQuoteDeserializer.SetupGet(qu => qu.Identifier)
            .Returns(moqSecondQuoteDeserializerIdentifier.Object);
        moqSecondQuoteDeserializerIdentifier.SetupGet(usti => usti.Source).Returns("TestFirstSource");

        monitorAction();

        moqStopSignal.Verify();
        moqSequencer.Verify(s => s.Claim(), Times.Exactly(5));
        moqSequencer.Verify(s => s.Serialize(sequencerSequence), Times.Exactly(5));
        moqSequencer.Verify(s => s.Release(sequencerSequence), Times.Exactly(5));
        moqSyncOk.Verify();
        moqSyncKo.Verify();
        moqFirstQuoteDeserializer.Verify();
        moqSecondQuoteDeserializer.Verify();
        Assert.AreEqual("TestFirstSource", lastSourceName);
        Assert.AreEqual(moqSnapshotServerConnectionConfig.Object, lastConnectionConfig);
        Assert.IsNotNull(lastUniqueSourceTickerIdentifiers);
        Assert.AreEqual(2, lastUniqueSourceTickerIdentifiers.Count);
        Assert.AreEqual(moqFirstQuoteDeserializerIdentifier.Object, lastUniqueSourceTickerIdentifiers[0]);
        Assert.AreEqual(moqSecondQuoteDeserializerIdentifier.Object, lastUniqueSourceTickerIdentifiers[1]);
    }

    [TestMethod]
    public void UnknownSnapshotServer_MonitorDeserializersForSnapshotResync_SkipsRequestToSendSnapshotRequest()
    {
        pqClientSyncMonitoring.CheckStartMonitoring();
        PrepareMonitorDeserializersForSnapshotResyncMoqs();
        moqSecondQuoteDeserializer.Setup(qu => qu.CheckResync(It.IsAny<DateTime>())).Returns(true).Verifiable();
        moqFirstQuoteDeserializerIdentifier.SetupGet(usti => usti.Source).Returns(nonExistentSource);

        monitorAction();

        moqStopSignal.Verify();
        moqSequencer.Verify(s => s.Claim(), Times.Exactly(5));
        moqSequencer.Verify(s => s.Serialize(sequencerSequence), Times.Exactly(5));
        moqSequencer.Verify(s => s.Release(sequencerSequence), Times.Exactly(5));
        moqSyncOk.Verify();
        moqSyncKo.Verify();
        moqFirstQuoteDeserializer.Verify();
        moqSecondQuoteDeserializer.Verify();
        Assert.AreEqual("TestSecondSource", lastSourceName);
        Assert.AreEqual(moqSnapshotServerConnectionConfig.Object, lastConnectionConfig);
        Assert.IsNotNull(lastUniqueSourceTickerIdentifiers);
        Assert.AreEqual(1, lastUniqueSourceTickerIdentifiers.Count);
        Assert.AreEqual(moqSecondQuoteDeserializerIdentifier.Object, lastUniqueSourceTickerIdentifiers[0]);
    }

    [TestMethod]
    public void NoTickersRequireSnapshot_MonitorDeserializersForSnapshotResync_SkipsRequestToSendSnapshotRequest()
    {
        pqClientSyncMonitoring.CheckStartMonitoring();
        PrepareMonitorDeserializersForSnapshotResyncMoqs();
        moqFirstQuoteDeserializer.Setup(qu => qu.CheckResync(It.IsAny<DateTime>())).Returns(false).Verifiable();
        moqSecondQuoteDeserializer.Setup(qu => qu.CheckResync(It.IsAny<DateTime>())).Callback(() => { pqClientSyncMonitoring.CheckStopMonitoring(); })
            .Returns(false).Verifiable();

        monitorAction();

        moqStopSignal.Verify();
        moqSequencer.Verify(s => s.Claim(), Times.Exactly(4));
        moqSequencer.Verify(s => s.Serialize(sequencerSequence), Times.Exactly(4));
        moqSequencer.Verify(s => s.Release(sequencerSequence), Times.Exactly(4));
        moqSyncOk.Verify();
        moqSyncKo.Verify();
        moqFirstQuoteDeserializer.Verify();
        moqSecondQuoteDeserializer.Verify();
        Assert.AreEqual(null, lastSourceName);
        Assert.AreEqual(null, lastConnectionConfig);
        Assert.IsNull(null);
    }

    [TestMethod]
    public void TwoSourcesRequireSnapshoting_MonitorDeserializersForSnapshotResync_SendsTwoRequests()
    {
        pqClientSyncMonitoring.CheckStartMonitoring();
        PrepareMonitorDeserializersForSnapshotResyncMoqs();
        moqSecondQuoteDeserializer.Setup(qu => qu.CheckResync(It.IsAny<DateTime>())).Returns(true).Verifiable();
        moqSecondQuoteDeserializer.SetupGet(qu => qu.Identifier)
            .Returns(moqSecondQuoteDeserializerIdentifier.Object);

        monitorAction();

        moqStopSignal.Verify();
        moqSequencer.Verify(s => s.Claim(), Times.Exactly(5));
        moqSequencer.Verify(s => s.Serialize(sequencerSequence), Times.Exactly(5));
        moqSequencer.Verify(s => s.Release(sequencerSequence), Times.Exactly(5));
        moqSyncOk.Verify();
        moqSyncKo.Verify();
        moqFirstQuoteDeserializer.Verify();
        moqSecondQuoteDeserializer.Verify();
        Assert.AreEqual("TestSecondSource", lastSourceName);
        Assert.AreEqual(moqSnapshotServerConnectionConfig.Object, lastConnectionConfig);
        Assert.IsNotNull(lastUniqueSourceTickerIdentifiers);
        Assert.AreEqual(1, lastUniqueSourceTickerIdentifiers.Count);
        Assert.AreEqual(moqSecondQuoteDeserializerIdentifier.Object, lastUniqueSourceTickerIdentifiers[0]);
        Assert.AreEqual(2, historyOfCalledSourceTickerIds.Count);
        Assert.AreEqual(moqFirstQuoteDeserializerIdentifier.Object, historyOfCalledSourceTickerIds[0]);
        Assert.AreEqual(moqSecondQuoteDeserializerIdentifier.Object, historyOfCalledSourceTickerIds[1]);
    }

    [TestMethod]
    public void NewSyncMonitor_MonitorDeserializersForSnapshotResyncReceivesException_CarriesOn()
    {
        pqClientSyncMonitoring.CheckStartMonitoring();
        PrepareMonitorDeserializersForSnapshotResyncMoqs();
        moqTimeContext.SetupSequence(tc => tc.UtcNow).Returns(baseTime).Throws<Exception>()
            .Returns(baseTime.AddMilliseconds(200)).Returns(baseTime.AddMilliseconds(1001))
            .Returns(baseTime.AddMilliseconds(1002)).Returns(baseTime.AddMilliseconds(1003))
            .Returns(baseTime.AddMilliseconds(1004));
        moqLogger.Setup(fl => fl.Error("Unexpected error in task scheduler: {0}", It.IsAny<Exception>()))
            .Verifiable();

        monitorAction();

        moqLogger.Verify();
        moqStopSignal.Verify();
        moqSequencer.Verify(s => s.Claim(), Times.Exactly(5));
        moqSequencer.Verify(s => s.Serialize(sequencerSequence), Times.Exactly(5));
        moqSequencer.Verify(s => s.Release(sequencerSequence), Times.Exactly(5));
        moqSyncOk.Verify();
        moqSyncKo.Verify();
        moqFirstQuoteDeserializer.Verify();
        moqSecondQuoteDeserializer.Verify();
        Assert.AreEqual("TestFirstSource", lastSourceName);
        Assert.AreEqual(moqSnapshotServerConnectionConfig.Object, lastConnectionConfig);
        Assert.IsNotNull(lastUniqueSourceTickerIdentifiers);
        Assert.AreEqual(1, lastUniqueSourceTickerIdentifiers.Count);
        Assert.AreEqual(moqFirstQuoteDeserializerIdentifier.Object, lastUniqueSourceTickerIdentifiers[0]);
    }

    private void PrepareMonitorDeserializersForSnapshotResyncMoqs()
    {
        moqTimeContext.SetupSequence(tc => tc.UtcNow).Returns(baseTime).Returns(baseTime.AddMilliseconds(200))
            .Returns(baseTime.AddMilliseconds(1001)).Returns(baseTime.AddMilliseconds(1002))
            .Returns(baseTime.AddMilliseconds(1003)).Returns(baseTime.AddMilliseconds(1004));

        moqStopSignal.Setup(iosts => iosts.WaitOne(It.IsAny<int>())).Returns(false).Verifiable();
        moqSyncOk.SetupSequence(sok => sok.Head).Returns(moqFirstQuoteDeserializer.Object)
            .Returns(null as IPQDeserializer);
        moqFirstQuoteDeserializer.Setup(qu => qu.HasTimedOutAndNeedsSnapshot(It.IsAny<DateTime>())).Returns(true)
            .Verifiable();

        moqSyncOk.Setup(sok => sok.Remove(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();
        moqSyncKo.Setup(sko => sko.AddFirst(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();

        moqSyncKo.SetupSequence(sko => sko.Head).Returns(moqFirstQuoteDeserializer.Object)
            .Returns(moqSecondQuoteDeserializer.Object).Returns(moqFirstQuoteDeserializer.Object);
        moqFirstQuoteDeserializer.Setup(qu => qu.CheckResync(It.IsAny<DateTime>())).Returns(true).Verifiable();
        moqSecondQuoteDeserializer.Setup(qu => qu.CheckResync(It.IsAny<DateTime>())).Returns(false).Verifiable();

        moqSyncKo.Setup(sko => sko.Remove(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();
        moqSyncKo.Setup(sko => sko.AddLast(moqFirstQuoteDeserializer.Object)).Callback(() => { Assert.IsTrue(inSequencerSerialize); })
            .Returns(moqFirstQuoteDeserializer.Object).Verifiable();

        moqFirstQuoteDeserializerIdentifier = new Mock<ISourceTickerQuoteInfo>();
        moqFirstQuoteDeserializer.SetupGet(qu => qu.Identifier)
            .Returns(moqFirstQuoteDeserializerIdentifier.Object);
        moqFirstQuoteDeserializerIdentifier.SetupGet(usti => usti.Source).Returns("TestFirstSource");
        moqSecondQuoteDeserializerIdentifier = new Mock<ISourceTickerQuoteInfo>();
        moqSecondQuoteDeserializer.SetupGet(qu => qu.Identifier)
            .Returns(moqSecondQuoteDeserializerIdentifier.Object);
        moqSecondQuoteDeserializerIdentifier.SetupGet(usti => usti.Source).Returns("TestSecondSource");
    }

    private void NoOp(IPQDeserializer obj) { }
}
