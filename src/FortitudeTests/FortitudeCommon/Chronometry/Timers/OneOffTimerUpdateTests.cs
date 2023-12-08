#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using Moq;
using static FortitudeCommon.Chronometry.Timers.Timer;
using Timer = System.Threading.Timer;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

[TestClass]
public class OneOffTimerUpdateTests
{
    private readonly AutoResetEvent autoResetEvent = new(false);
    private volatile int callbackCounter;
    private Mock<IUpdateableTimer> moqTimer = null!;
    private OneOffTimerUpdate oneOffTimerUpdate = null!;
    private Timer timer = null!;
    private WaitCallbackTimerCallBackRunInfo timerCallBackRunInfo = null!;
    private WaitCallback waitCallback = null!;


    [TestInitialize]
    public void SetUp()
    {
        callbackCounter = 0;
        waitCallback = _ =>
        {
            Interlocked.Increment(ref callbackCounter);
            autoResetEvent.Set();
        };
        moqTimer = new Mock<IUpdateableTimer>();
        timer = new Timer(TimerCallback, null, MaxTimerMs, MaxTimerMs);
        var firstScheduledTime = TimeContext.UtcNow;
        timerCallBackRunInfo = CreateTimerCallBackRunInfo(firstScheduledTime);
        oneOffTimerUpdate = new OneOffTimerUpdate
        {
            UpdateableTimer = moqTimer.Object, CallBackRunInfo = timerCallBackRunInfo
        };
    }

    private WaitCallbackTimerCallBackRunInfo CreateTimerCallBackRunInfo(DateTime firstScheduledTime) =>
        new()
        {
            WaitCallback = waitCallback, FirstScheduledTime = firstScheduledTime, LastRunTime = DateTime.MinValue
            , MaxNumberOfCalls = 1, NextScheduleTime = firstScheduledTime
            , IntervalPeriodTimeSpan = TimeSpan.FromMilliseconds(500), State = null, RegisteredTimer = timer
        };

    [TestCleanup]
    public void TearDown()
    {
        timer.Dispose();
    }

    private void TimerCallback(object? state) { }

    [TestMethod]
    public void CopyFromTest()
    {
        var fromCopy = new OneOffTimerUpdate();
        fromCopy.CopyFrom((IStoreState)oneOffTimerUpdate, CopyMergeFlags.Default);

        Assert.AreEqual(oneOffTimerUpdate.CallBackRunInfo, fromCopy.CallBackRunInfo);
        Assert.AreEqual(oneOffTimerUpdate.UpdateableTimer, fromCopy.UpdateableTimer);
        Assert.AreEqual(oneOffTimerUpdate.RegisteredTimer, fromCopy.RegisteredTimer);
    }

    [TestMethod]
    public void IncrementAndDecrementRefCountTest()
    {
        Assert.AreEqual(0, oneOffTimerUpdate.RefCount);
        Assert.AreEqual(1, oneOffTimerUpdate.IncrementRefCount());
        Assert.AreEqual(1, oneOffTimerUpdate.RefCount);
        oneOffTimerUpdate.AutoRecycleAtRefCountZero = false;
        Assert.AreEqual(0, oneOffTimerUpdate.DecrementRefCount());
        Assert.AreEqual(0, oneOffTimerUpdate.RefCount);
    }

    [TestMethod]
    public void RecycleTest()
    {
        var recycler = new Recycler();
        var borrowed = recycler.Borrow<OneOffTimerUpdate>();
        var borrowCallBackRunInfo = recycler.Borrow<WaitCallbackTimerCallBackRunInfo>();
        borrowed.CallBackRunInfo = borrowCallBackRunInfo;
        Assert.AreEqual(1, borrowed.RefCount);
        Assert.AreEqual(2, borrowCallBackRunInfo.RefCount);
        Assert.AreEqual(false, borrowed.IsInRecycler);
        Assert.AreEqual(false, borrowCallBackRunInfo.IsInRecycler);
        Assert.AreEqual(0, borrowed.DecrementRefCount());
        Assert.AreEqual(0, borrowed.RefCount);
        Assert.AreEqual(0, borrowCallBackRunInfo.RefCount);
        Assert.AreEqual(true, borrowed.IsInRecycler);
        Assert.AreEqual(true, borrowCallBackRunInfo.IsInRecycler);
    }

    [TestMethod]
    public void CancelTest()
    {
        moqTimer.Setup(ut => ut.Remove(timerCallBackRunInfo)).Returns(true).Verifiable();

        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(true, oneOffTimerUpdate.Cancel());
        Assert.AreEqual(true, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(true, timerCallBackRunInfo.IsFinished);

        moqTimer.Verify();
    }

    [TestMethod]
    public void ExecuteNowOnThreadPoolTest()
    {
        moqTimer.Setup(ut => ut.Remove(timerCallBackRunInfo)).Returns(true).Verifiable();
        moqTimer.Setup(ut => ut.CheckNextOneOffLaunchTimeStillCorrect(timerCallBackRunInfo)).Verifiable();

        Assert.AreEqual(0, callbackCounter);
        oneOffTimerUpdate.ExecuteNowOnThreadPool();
        autoResetEvent.WaitOne(1_000);
        Assert.AreEqual(1, callbackCounter);

        var pausedTimerCallbackInfo = CreateTimerCallBackRunInfo(timerCallBackRunInfo.FirstScheduledTime);
        pausedTimerCallbackInfo.IsPaused = true;
        var pausedOneOffTimerUpdate = new OneOffTimerUpdate
        {
            CallBackRunInfo = pausedTimerCallbackInfo, UpdateableTimer = moqTimer.Object
        };
        moqTimer.Setup(ut => ut.Remove(pausedTimerCallbackInfo)).Returns(true).Verifiable();
        moqTimer.Setup(ut => ut.CheckNextOneOffLaunchTimeStillCorrect(pausedTimerCallbackInfo)).Verifiable();

        Assert.AreEqual(1, callbackCounter);
        pausedOneOffTimerUpdate.ExecuteNowOnThreadPool();
        autoResetEvent.WaitOne(1_000);
        Assert.AreEqual(2, callbackCounter);
        Assert.AreEqual(true, pausedTimerCallbackInfo.IsPaused);

        pausedOneOffTimerUpdate.ExecuteNowOnThreadPool();
        Assert.AreEqual(2, callbackCounter);

        moqTimer.VerifyAll();
    }

    [TestMethod]
    public void ExecuteNowOnThisThreadTest()
    {
        moqTimer.Setup(ut => ut.Remove(timerCallBackRunInfo)).Returns(true).Verifiable();
        moqTimer.Setup(ut => ut.CheckNextOneOffLaunchTimeStillCorrect(timerCallBackRunInfo)).Verifiable();

        Assert.AreEqual(0, callbackCounter);
        oneOffTimerUpdate.ExecuteNowOnThisThread();
        Assert.AreEqual(1, callbackCounter);

        var pausedTimerCallbackInfo = CreateTimerCallBackRunInfo(timerCallBackRunInfo.FirstScheduledTime);
        pausedTimerCallbackInfo.IsPaused = true;
        var pausedOneOffTimerUpdate = new OneOffTimerUpdate
        {
            CallBackRunInfo = pausedTimerCallbackInfo, UpdateableTimer = moqTimer.Object
        };
        moqTimer.Setup(ut => ut.Remove(pausedTimerCallbackInfo)).Returns(true).Verifiable();
        moqTimer.Setup(ut => ut.CheckNextOneOffLaunchTimeStillCorrect(pausedTimerCallbackInfo)).Verifiable();

        Assert.AreEqual(1, callbackCounter);
        pausedOneOffTimerUpdate.ExecuteNowOnThisThread();
        Assert.AreEqual(2, callbackCounter);
        Assert.AreEqual(true, pausedTimerCallbackInfo.IsPaused);

        pausedOneOffTimerUpdate.ExecuteNowOnThisThread();
        Assert.AreEqual(2, callbackCounter);

        moqTimer.VerifyAll();
    }

    [TestMethod]
    public void UpdateWaitPeriodTest()
    {
        moqTimer.Setup(ut => ut.CheckNextOneOffLaunchTimeStillCorrect(timerCallBackRunInfo)).Verifiable();

        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(true, oneOffTimerUpdate.UpdateWaitPeriod(40));
        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);
        moqTimer.VerifyAll();

        timerCallBackRunInfo.IsPaused = true;
        Assert.AreEqual(true, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(true, oneOffTimerUpdate.UpdateWaitPeriod(40));
        Assert.AreEqual(true, timerCallBackRunInfo.IsPaused);
        moqTimer.VerifyAll();
    }

    [TestMethod]
    public void PauseTest()
    {
        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(true, oneOffTimerUpdate.Pause());
        Assert.AreEqual(true, timerCallBackRunInfo.IsPaused);

        timerCallBackRunInfo.RunCallbackOnThisThread();
        timerCallBackRunInfo.IsPaused = false;
        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(false, oneOffTimerUpdate.Pause());
        Assert.AreEqual(true, oneOffTimerUpdate.IsPaused);
    }

    [TestMethod]
    public void ResumeTest()
    {
        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(true, oneOffTimerUpdate.Pause());
        Assert.AreEqual(true, oneOffTimerUpdate.Resume());
        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);

        timerCallBackRunInfo.RunCallbackOnThisThread();
        timerCallBackRunInfo.IsPaused = false;
        Assert.AreEqual(false, oneOffTimerUpdate.IsPaused);
        Assert.AreEqual(true, oneOffTimerUpdate.IsFinished);
        Assert.AreEqual(DateTime.MaxValue, oneOffTimerUpdate.NextScheduleDateTime);
        Assert.AreEqual(false, oneOffTimerUpdate.Resume());
        Assert.AreEqual(false, oneOffTimerUpdate.IsPaused);
    }
}
