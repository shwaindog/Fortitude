#region

using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using Moq;

#endregion

namespace FortitudeTests.FortitudeCommon.EventProcessing.Disruption.Rings.Batching;

[TestClass]
public class RingPollerBaseTests
{
    private const string ExpectedThreadName = "RingPollerBaseTests-Poller";
    private const uint NoDataPauseTimeout = 100U;
    private Action dispatchWorkerAction = null!;
    private DummyRingPollerBase dummyRingPollerBase = null!;
    private bool haveCalledDispatchWorkerAction;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPollingRing<StringContainer>> moqPollingRing = null!;
    private ThreadStart workerThreadMethod = null!;

    [TestInitialize]
    public void SetUp()
    {
        haveCalledDispatchWorkerAction = false;
        var firstStringContainer = new StringContainer
        {
            Payload = "firstStringContainer"
        };
        var secondStringContainer = new StringContainer
        {
            Payload = "secondStringContainer"
        };
        var thirdStringContainer = new StringContainer
        {
            Payload = "thirdStringContainer"
        };
        var batchedEnumerable = new List<StringContainer> { firstStringContainer, secondStringContainer, thirdStringContainer };
        // ReSharper disable once CollectionNeverUpdated.Local
        var emptyEnumerable = new List<StringContainer>();

        dispatchWorkerAction = () => { haveCalledDispatchWorkerAction = true; };
        moqPollingRing = new Mock<IPollingRing<StringContainer>>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelController = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;


        moqOsThread = new Mock<IOSThread>();
        moqParallelController.Setup(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>()))
            .Callback<ThreadStart>(workerMethod => { workerThreadMethod = workerMethod; })
            .Returns(moqOsThread.Object).Verifiable();

        moqPollingRing.Setup(pr => pr.Name).Returns("RingPollerBaseTests");
        moqPollingRing.Setup(pr => pr[0L]).Returns(firstStringContainer);
        moqPollingRing.Setup(pr => pr[1L]).Returns(secondStringContainer);
        moqPollingRing.Setup(pr => pr[2L]).Returns(thirdStringContainer);
        moqPollingRing.SetupSequence(pr => pr.GetEnumerator()).Returns(batchedEnumerable.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
        moqPollingRing.SetupGet(pr => pr.CurrentBatchSize).Returns(3);
        moqPollingRing.SetupSequence(pr => pr.StartOfBatch).Returns(true).Returns(false).Returns(false);
        moqPollingRing.SetupSequence(pr => pr.EndOfBatch).Returns(false).Returns(false).Returns(true);
        moqPollingRing.SetupSequence(pr => pr.CurrentSequence).Returns(0).Returns(1).Returns(2);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void NewRingPoller_New_SetsNameWithPoller()
    {
        dummyRingPollerBase
            = new DummyRingPollerBase(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);
        Assert.AreEqual("RingPollerBaseTests-Poller", dummyRingPollerBase.Name);
    }

    [TestMethod]
    public void UnstartedRingPoller_Start_IncrementsUsageCountLaunchesWorkerThread()
    {
        moqOsThread.Setup(ost => ost.Join()).Verifiable();
        dispatchWorkerAction = () =>
        {
            haveCalledDispatchWorkerAction = true;
            Assert.AreEqual(1, dummyRingPollerBase.UsageCount);
            Assert.IsTrue(NonPublicInvocator.GetInstanceField<bool>(dummyRingPollerBase, "isRunning"));
            dummyRingPollerBase.Stop();
        };
        dummyRingPollerBase
            = new DummyRingPollerBase(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);

        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadName)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();

        dummyRingPollerBase.Start();
        workerThreadMethod();

        Assert.IsTrue(haveCalledDispatchWorkerAction);
        Assert.IsFalse(NonPublicInvocator.GetInstanceField<bool>(dummyRingPollerBase, "isRunning"));
        moqOsThread.Verify();
        moqParallelController.Verify();
    }

    [TestMethod]
    public void StartedRingPoller_Start_IncrementsUsageCountDoesNotRelaunchWorkerThread()
    {
        dummyRingPollerBase
            = new DummyRingPollerBase(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);
        moqOsThread.SetupSet(ost => ost.Name = ExpectedThreadName).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        dummyRingPollerBase.Start();
        moqOsThread.Verify();

        moqOsThread.Reset();
        dummyRingPollerBase.Start();
        Assert.AreEqual(2, dummyRingPollerBase.UsageCount);
        moqOsThread.Verify(ost => ost.Start(), Times.Never);
    }

    [TestMethod]
    public void StartedRingPollerUsageCount1_Stop_DecrementsUsageCountStopsWorkThread()
    {
        dummyRingPollerBase
            = new DummyRingPollerBase(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadName)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        dummyRingPollerBase.Start();
        Assert.AreEqual(1, dummyRingPollerBase.UsageCount);

        moqOsThread.Setup(ost => ost.Join()).Verifiable();

        dummyRingPollerBase.Stop();

        Assert.AreEqual(0, dummyRingPollerBase.UsageCount);
        Assert.IsFalse(NonPublicInvocator.GetInstanceField<bool>(dummyRingPollerBase, "isRunning"));
        moqOsThread.Verify();
        moqParallelController.Verify();
    }

    [TestMethod]
    public void StartedRingPollerUsageCount2_Stop_DecrementsUsageCountLeavesWorkerThreadRunning()
    {
        dummyRingPollerBase
            = new DummyRingPollerBase(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadName)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        dummyRingPollerBase.Start();
        dummyRingPollerBase.Start();
        Assert.AreEqual(2, dummyRingPollerBase.UsageCount);

        dummyRingPollerBase.Stop();

        Assert.AreEqual(1, dummyRingPollerBase.UsageCount);
        moqOsThread.Setup(ost => ost.Join()).Verifiable(Times.Never);
    }

    public class StringContainer
    {
        public string? Payload { get; set; }
    }

    private class DummyRingPollerBase(IPollingRing<StringContainer> ring, uint noDataPauseTimeoutMs, Action dispatchWorkerAction
        , Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : RingPollerBase<StringContainer>(ring, noDataPauseTimeoutMs, threadStartInitialization
        , parallelController)
    {
        protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, StringContainer data, bool ringStartOfBatch
            , bool ringEndOfBatch)
        {
            dispatchWorkerAction();
        }
    }
}
