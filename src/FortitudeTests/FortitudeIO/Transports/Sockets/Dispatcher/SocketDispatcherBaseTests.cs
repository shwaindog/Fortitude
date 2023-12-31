﻿#region

using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets.Dispatcher;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher;

[TestClass]
public class SocketDispatcherBaseTests
{
    private const string ExpectedThreadNameRegex = @"DummyWorkerThreadName\d+";
    private Action<IOSThread> cleanUpForStopAction = null!;
    private Action dispatchWorkerAction = null!;
    private DummySocketDispatcherBase dummySocketDispatcherBase = null!;
    private bool haveCalledCleanUpForStopAction;
    private bool haveCalledDispatchWorkerAction;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private ThreadStart workerThreadMethod = null!;

    [TestInitialize]
    public void SetUp()
    {
        haveCalledDispatchWorkerAction = false;
        haveCalledCleanUpForStopAction = false;

        dispatchWorkerAction = () => haveCalledDispatchWorkerAction = true;
        cleanUpForStopAction = iosThread => haveCalledCleanUpForStopAction = true;
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelController = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqOsThread = new Mock<IOSThread>();
        moqParallelController.Setup(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>()))
            .Callback<ThreadStart>(workerMethod => { workerThreadMethod = workerMethod; })
            .Returns(moqOsThread.Object).Verifiable();

        dummySocketDispatcherBase = new DummySocketDispatcherBase("TestDescription", dispatchWorkerAction,
            cleanUpForStopAction);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void NewDispatcherBase_New_SetsDispatcherDescription()
    {
        Assert.AreEqual("TestDescription", dummySocketDispatcherBase.DispatcherDescription);
    }

    [TestMethod]
    public void UnstartedDispatcher_Start_IncrementsUsageCountLaunchesWorkerThreadAsExpected()
    {
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadNameRegex)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();

        dummySocketDispatcherBase.Start();
        workerThreadMethod();

        Assert.AreEqual(1, dummySocketDispatcherBase.UsageCount);
        Assert.IsTrue(haveCalledDispatchWorkerAction);
        Assert.IsTrue(NonPublicInvocator.GetInstanceField<bool>(dummySocketDispatcherBase, "Running"));
        moqOsThread.Verify();
        moqParallelController.Verify();
    }

    [TestMethod]
    public void StartedDispatcher_Start_IncrementsUsageCountDoesntRelaunchWorkerThread()
    {
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadNameRegex)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        dummySocketDispatcherBase.Start();

        moqOsThread.Reset();

        dummySocketDispatcherBase.Start();

        Assert.AreEqual(2, dummySocketDispatcherBase.UsageCount);
        moqOsThread.Verify(ost => ost.Start(), Times.Never);
    }

    [TestMethod]
    public void StartedDispatcherUsageCount1_Stop_DecrementsUsageCountStopsWorkThread()
    {
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadNameRegex)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        dummySocketDispatcherBase.Start();
        Assert.AreEqual(1, dummySocketDispatcherBase.UsageCount);

        var moqCurrentOsThread = new Mock<IOSThread>();
        moqCurrentOsThread.SetupGet(ost => ost.Name).Returns("TestCurrentThread").Verifiable();
        moqParallelController.SetupGet(pc => pc.CurrentOSThread).Returns(moqCurrentOsThread.Object).Verifiable();
        Assert.IsFalse(haveCalledCleanUpForStopAction);

        dummySocketDispatcherBase.Stop();

        Assert.AreEqual(0, dummySocketDispatcherBase.UsageCount);
        Assert.IsFalse(NonPublicInvocator.GetInstanceField<bool>(dummySocketDispatcherBase, "Running"));
        moqCurrentOsThread.Verify();
        moqParallelController.Verify();
        Assert.IsTrue(haveCalledCleanUpForStopAction);
    }

    [TestMethod]
    public void StartedDispatcherUsageCount2_Stop_DecrementsUsageCountLeavesWorkerThreadRunning()
    {
        moqOsThread.SetupSet(ost => ost.Name = It.IsRegex(ExpectedThreadNameRegex)).Verifiable();
        moqOsThread.SetupSet(ost => ost.IsBackground = true).Verifiable();
        moqOsThread.Setup(ost => ost.Start()).Verifiable();
        dummySocketDispatcherBase.Start();
        dummySocketDispatcherBase.Start();
        Assert.AreEqual(2, dummySocketDispatcherBase.UsageCount);

        dummySocketDispatcherBase.Stop();

        Assert.AreEqual(1, dummySocketDispatcherBase.UsageCount);
        Assert.IsFalse(haveCalledCleanUpForStopAction);
    }

    private class DummySocketDispatcherBase : SocketDispatcherBase
    {
        private readonly Action<IOSThread> cleanUpForStopAction;
        private readonly Action dispatchWorkerAction;

        public DummySocketDispatcherBase(string dispatcherDescription, Action dispatchWorkerAction,
            Action<IOSThread> cleanUpForStopAction)
            : base(dispatcherDescription)
        {
            this.dispatchWorkerAction = dispatchWorkerAction;
            this.cleanUpForStopAction = cleanUpForStopAction;
        }

        protected override string WorkerThreadName => "DummyWorkerThreadName";

        protected override void DispatchWorker()
        {
            dispatchWorkerAction();
        }

        protected override void CleanupForStop(IOSThread workerThread)
        {
            cleanUpForStopAction(workerThread);
        }
    }
}
