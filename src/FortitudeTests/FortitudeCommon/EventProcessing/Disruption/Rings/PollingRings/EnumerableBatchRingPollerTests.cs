#region

using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using Moq;

#endregion

namespace FortitudeTests.FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

[TestClass]
public class EnumerableBatchRingPollerTests
{
    private const string ExpectedThreadName = "EnumerableBatchRingPollerTests-EnumerableBatchRingPoller";
    private const uint NoDataPauseTimeout = 100U;
    private Action dispatchWorkerAction = null!;
    private DummyEnumerableBatchRingPoller dummyEnumerableBatchRingPoller = null!;
    private bool haveCalledDispatchWorkerAction;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IEnumerableBatchPollingRing<StringContainer>> moqPollingRing = null!;
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
        moqPollingRing = new Mock<IEnumerableBatchPollingRing<StringContainer>>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelController = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;


        moqOsThread = new Mock<IOSThread>();
        moqParallelController.Setup(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>()))
            .Callback<ThreadStart>(workerMethod => { workerThreadMethod = workerMethod; })
            .Returns(moqOsThread.Object).Verifiable();

        moqPollingRing.Setup(pr => pr.Name).Returns("EnumerableBatchRingPollerTests");
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
        dummyEnumerableBatchRingPoller
            = new DummyEnumerableBatchRingPoller(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);
        Assert.AreEqual("EnumerableBatchRingPollerTests-EnumerableBatchRingPoller", dummyEnumerableBatchRingPoller.Name);
    }

    [TestMethod]
    public void UnstartedRingPoller_Start_IncrementsUsageCountLaunchesWorkerThread()
    {
        moqOsThread.Setup(ost => ost.Join()).Verifiable();
        dispatchWorkerAction = () =>
        {
            haveCalledDispatchWorkerAction = true;
            Assert.AreEqual(1, dummyEnumerableBatchRingPoller.UsageCount);
            Assert.IsTrue(NonPublicInvocator.GetInstanceField<bool>(dummyEnumerableBatchRingPoller, "isRunning"));
            dummyEnumerableBatchRingPoller.Stop();
        };
        dummyEnumerableBatchRingPoller
            = new DummyEnumerableBatchRingPoller(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);

        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadName)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();

        dummyEnumerableBatchRingPoller.Start();
        workerThreadMethod();

        Assert.IsTrue(haveCalledDispatchWorkerAction);
        Assert.IsFalse(NonPublicInvocator.GetInstanceField<bool>(dummyEnumerableBatchRingPoller, "isRunning"));
        moqOsThread.Verify();
        moqParallelController.Verify();
    }

    [TestMethod]
    public void StartedRingPoller_Start_IncrementsUsageCountDoesNotRelaunchWorkerThread()
    {
        dummyEnumerableBatchRingPoller
            = new DummyEnumerableBatchRingPoller(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);
        moqOsThread.SetupSet(ost => ost.Name = ExpectedThreadName).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        dummyEnumerableBatchRingPoller.Start();
        moqOsThread.Verify();

        moqOsThread.Reset();
        dummyEnumerableBatchRingPoller.Start();
        Assert.AreEqual(2, dummyEnumerableBatchRingPoller.UsageCount);
        moqOsThread.Verify(ost => ost.Start(), Times.Never);
        dummyEnumerableBatchRingPoller.Stop();
    }

    [TestMethod]
    public void StartedRingPollerUsageCount1_Stop_DecrementsUsageCountStopsWorkThread()
    {
        dummyEnumerableBatchRingPoller
            = new DummyEnumerableBatchRingPoller(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadName)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        dummyEnumerableBatchRingPoller.Start();
        Assert.AreEqual(1, dummyEnumerableBatchRingPoller.UsageCount);

        moqOsThread.Setup(ost => ost.Join()).Verifiable();

        dummyEnumerableBatchRingPoller.Stop();

        Assert.AreEqual(0, dummyEnumerableBatchRingPoller.UsageCount);
        Assert.IsFalse(NonPublicInvocator.GetInstanceField<bool>(dummyEnumerableBatchRingPoller, "isRunning"));
        moqOsThread.Verify();
        moqParallelController.Verify();
    }

    [TestMethod]
    public void StartedRingPollerUsageCount2_Stop_DecrementsUsageCountLeavesWorkerThreadRunning()
    {
        dummyEnumerableBatchRingPoller
            = new DummyEnumerableBatchRingPoller(moqPollingRing.Object, NoDataPauseTimeout, dispatchWorkerAction, null, moqParallelController.Object);
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadName)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        dummyEnumerableBatchRingPoller.Start();
        dummyEnumerableBatchRingPoller.Start();
        Assert.AreEqual(2, dummyEnumerableBatchRingPoller.UsageCount);

        dummyEnumerableBatchRingPoller.Stop();

        Assert.AreEqual(1, dummyEnumerableBatchRingPoller.UsageCount);
        moqOsThread.Setup(ost => ost.Join()).Verifiable(Times.Never);
        dummyEnumerableBatchRingPoller.Stop();
    }

    public class StringContainer
    {
        public string? Payload { get; set; }
    }

    private class DummyEnumerableBatchRingPoller(IEnumerableBatchPollingRing<StringContainer> ring, uint noDataPauseTimeoutMs
        , Action dispatchWorkerAction
        , Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : EnumerableBatchRingPoller<StringContainer>(ring, noDataPauseTimeoutMs
        , threadStartInitialization
        , parallelController)
    {
        protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, StringContainer data, bool ringStartOfBatch
            , bool ringEndOfBatch)
        {
            dispatchWorkerAction();
        }
    }
}
