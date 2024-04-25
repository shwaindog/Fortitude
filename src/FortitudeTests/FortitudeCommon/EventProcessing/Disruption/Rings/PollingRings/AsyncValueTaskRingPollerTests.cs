#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using Moq;

#endregion

namespace FortitudeTests.FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

[TestClass]
public class AsyncValueTaskRingPollerTests
{
    private const string ExpectedThreadName = "AsyncValueTaskRingPollerTests-AsyncValueTaskPoller";
    private const uint NoDataPauseTimeout = 100U;
    private AsyncValueTaskRingPoller<StringContainer> asyncValueTaskRingPoller = null!;
    private ValueTask<long> completeNoWorkDoneValueTask;
    private ValueTask<long> completeWorkDoneValueTask;
    private bool haveCalledThreadInitializeAction;
    private ValueTask<long> incompleteValueTask;
    private Mock<IIntraOSThreadSignal> moqIntraOsThreadSignal = null!;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IAsyncValueTaskPollingRing<StringContainer>> moqPollingRing = null!;
    private Action threadInitializeAction = null!;
    private ThreadStart workerThreadMethod = null!;

    [TestInitialize]
    public void SetUp()
    {
        haveCalledThreadInitializeAction = false;

        threadInitializeAction = () => { haveCalledThreadInitializeAction = true; };

        var incompleteTask = new Task<long>(_ =>
        {
            Thread.Sleep(20_000);
            return 1;
        }, this);
        incompleteValueTask = new ValueTask<long>(incompleteTask);
        completeWorkDoneValueTask = new ValueTask<long>(1);
        completeNoWorkDoneValueTask = new ValueTask<long>(-1);

        moqPollingRing = new Mock<IAsyncValueTaskPollingRing<StringContainer>>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelController = new Mock<IOSParallelController>();
        moqIntraOsThreadSignal = new Mock<IIntraOSThreadSignal>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        moqOsThread = new Mock<IOSThread>();
        moqOsThread.SetupGet(ost => ost.IsAlive).Returns(true);
        moqParallelController.Setup(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>()))
            .Callback<ThreadStart>(workerMethod => { workerThreadMethod = workerMethod; })
            .Returns(moqOsThread.Object).Verifiable();
        moqParallelController.Setup(pc => pc.SingleOSThreadActivateSignal(false))
            .Returns(moqIntraOsThreadSignal.Object).Verifiable();

        moqPollingRing.SetupAllProperties();
        moqPollingRing.Setup(pr => pr.Name).Returns("AsyncValueTaskRingPollerTests");
        moqPollingRing.Setup(pr => pr.Poll()).Returns(completeNoWorkDoneValueTask);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    }

    [TestMethod]
    public void NewRingPoller_New_SetsNameWithPoller()
    {
        asyncValueTaskRingPoller
            = new AsyncValueTaskRingPoller<StringContainer>(moqPollingRing.Object, NoDataPauseTimeout, threadInitializeAction
                , moqParallelController.Object);
        Assert.AreEqual("AsyncValueTaskRingPollerTests-AsyncValueTaskPoller", asyncValueTaskRingPoller.Name);
    }

    [TestMethod]
    public void UnstartedRingPoller_Start_IncrementsUsageCountLaunchesWorkerThread()
    {
        moqOsThread.Setup(ost => ost.Join()).Verifiable();
        moqPollingRing.Setup(pr => pr.Poll()).Callback(() =>
        {
            haveCalledThreadInitializeAction = true;
            Assert.AreEqual(1, asyncValueTaskRingPoller.UsageCount);
            Assert.IsTrue(NonPublicInvocator.GetInstanceField<bool>(asyncValueTaskRingPoller, "isRunning"));
            asyncValueTaskRingPoller.Stop();
        }).Returns(completeNoWorkDoneValueTask);
        asyncValueTaskRingPoller
            = new AsyncValueTaskRingPoller<StringContainer>(moqPollingRing.Object, NoDataPauseTimeout, threadInitializeAction
                , moqParallelController.Object);

        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadName)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();

        asyncValueTaskRingPoller.Start();
        workerThreadMethod();

        Assert.IsTrue(haveCalledThreadInitializeAction);
        Assert.IsFalse(NonPublicInvocator.GetInstanceField<bool>(asyncValueTaskRingPoller, "isRunning"));
        moqOsThread.Verify();
        moqParallelController.Verify();
    }

    [TestMethod]
    public void UnstartedRingPoller_ContinuesToPoll_WhenUncompleteTaskReturns()
    {
        moqOsThread.Setup(ost => ost.Join()).Verifiable();
        moqPollingRing.Setup(pr => pr.Poll()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.Poll()).Callback(() =>
            {
                moqPollingRing.Setup(pr => pr.Poll()).Callback(() =>
                {
                    haveCalledThreadInitializeAction = true;
                    Assert.AreEqual(1, asyncValueTaskRingPoller.UsageCount);
                    Assert.IsTrue(NonPublicInvocator.GetInstanceField<bool>(asyncValueTaskRingPoller, "isRunning"));
                    asyncValueTaskRingPoller.Stop();
                }).Returns(completeNoWorkDoneValueTask);
            }).Returns(incompleteValueTask);
        }).Returns(incompleteValueTask);
        asyncValueTaskRingPoller
            = new AsyncValueTaskRingPoller<StringContainer>(moqPollingRing.Object, NoDataPauseTimeout, threadInitializeAction
                , moqParallelController.Object);

        moqOsThread.Setup(ost => ost.Start()).Verifiable();

        asyncValueTaskRingPoller.Start();
        workerThreadMethod();

        Assert.IsTrue(haveCalledThreadInitializeAction);
        Assert.IsFalse(NonPublicInvocator.GetInstanceField<bool>(asyncValueTaskRingPoller, "isRunning"));
        moqOsThread.Verify();
        moqParallelController.Verify();
    }

    [TestMethod]
    public void StartedRingPoller_Start_IncrementsUsageCountDoesNotRelaunchWorkerThread()
    {
        asyncValueTaskRingPoller
            = new AsyncValueTaskRingPoller<StringContainer>(moqPollingRing.Object, NoDataPauseTimeout, threadInitializeAction
                , moqParallelController.Object);
        moqOsThread.SetupSet(ost => ost.Name = ExpectedThreadName).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        asyncValueTaskRingPoller.Start();
        moqOsThread.Verify();

        moqOsThread.Reset();
        asyncValueTaskRingPoller.Start();
        Assert.AreEqual(2, asyncValueTaskRingPoller.UsageCount);
        moqOsThread.Verify(ost => ost.Start(), Times.Never);
        asyncValueTaskRingPoller.Stop();
        asyncValueTaskRingPoller.Stop();
    }

    [TestMethod]
    public void StartedRingPollerUsageCount1_Stop_DecrementsUsageCountStopsWorkThread()
    {
        asyncValueTaskRingPoller
            = new AsyncValueTaskRingPoller<StringContainer>(moqPollingRing.Object, NoDataPauseTimeout, threadInitializeAction
                , moqParallelController.Object);
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadName)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        asyncValueTaskRingPoller.Start();
        Assert.AreEqual(1, asyncValueTaskRingPoller.UsageCount);

        moqOsThread.Setup(ost => ost.Join()).Verifiable();

        asyncValueTaskRingPoller.Stop();

        Assert.AreEqual(0, asyncValueTaskRingPoller.UsageCount);
        Assert.IsFalse(NonPublicInvocator.GetInstanceField<bool>(asyncValueTaskRingPoller, "isRunning"));
        moqOsThread.Verify();
        moqParallelController.Verify();
    }

    [TestMethod]
    public void StartedRingPollerUsageCount2_Stop_DecrementsUsageCountLeavesWorkerThreadRunning()
    {
        asyncValueTaskRingPoller
            = new AsyncValueTaskRingPoller<StringContainer>(moqPollingRing.Object, NoDataPauseTimeout, threadInitializeAction
                , moqParallelController.Object);
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadName)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        asyncValueTaskRingPoller.Start();
        asyncValueTaskRingPoller.Start();
        Assert.AreEqual(2, asyncValueTaskRingPoller.UsageCount);

        asyncValueTaskRingPoller.Stop();

        Assert.AreEqual(1, asyncValueTaskRingPoller.UsageCount);
        moqOsThread.Setup(ost => ost.Join()).Verifiable(Times.Never);
        asyncValueTaskRingPoller.Stop();
    }

    public class StringContainer : ICanCarryTaskCallbackPayload
    {
        public string? Payload { get; set; }

        public bool IsTaskCallbackItem => false;

        public void SetAsTaskCallbackItem(SendOrPostCallback callback, object? state) { }

        public void InvokeTaskCallback() { }
    }
}
