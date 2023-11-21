#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using Moq;
using static FortitudeCommon.Chronometry.Timers.Timer;
using Timer = System.Threading.Timer;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

[TestClass]
public class IntervalTimerUpdateTests
{
    private readonly AutoResetEvent autoResetEvent = new(false);
    private volatile int callbackCounter;
    private IntervalTimerUpdate intervalTimerUpdate = null!;
    private Mock<IUpdateableTimer> moqTimer = null!;
    private Timer timer = null!;
    private TimerCallBackRunInfo timerCallBackRunInfo = null!;
    private volatile int timerCounter;
    private WaitCallback waitCallback = null!;

    [TestInitialize]
    public void SetUp()
    {
        callbackCounter = 0;
        timerCounter = 0;
        waitCallback = _ =>
        {
            Interlocked.Increment(ref callbackCounter);
            autoResetEvent.Set();
        };
        moqTimer = new Mock<IUpdateableTimer>();
        timer = new Timer(TimerCallback, null, MaxTimerMs, MaxTimerMs);
        var firstScheduledTime = TimeContext.UtcNow;
        timerCallBackRunInfo = CreateTimerCallBackRunInfo(firstScheduledTime);
        intervalTimerUpdate = new IntervalTimerUpdate
        {
            UpdateableTimer = moqTimer.Object, CallBackRunInfo = timerCallBackRunInfo
        };
    }

    private TimerCallBackRunInfo CreateTimerCallBackRunInfo(DateTime firstScheduledTime) =>
        new()
        {
            Callback = waitCallback, FirstScheduledTime = firstScheduledTime, LastRunTime = DateTime.MinValue
            , MaxNumberOfCalls = 1, NextScheduleTime = firstScheduledTime
            , IntervalPeriodTimeSpan = TimeSpan.FromMilliseconds(50), State = null, RegisteredTimer = timer
        };

    [TestCleanup]
    public void TearDown()
    {
        timer.Dispose();
    }

    private void TimerCallback(object? state)
    {
        Interlocked.Increment(ref timerCounter);
        autoResetEvent.Set();
    }

    [TestMethod]
    public void Cancel()
    {
        moqTimer.Setup(ut => ut.Remove(timerCallBackRunInfo)).Returns(true).Verifiable();

        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(true, intervalTimerUpdate.Cancel());
        Assert.AreEqual(true, timerCallBackRunInfo.IsPaused);

        moqTimer.Verify();
    }

    [TestMethod]
    public void UpdateWaitPeriod()
    {
        Assert.AreEqual(0, timerCounter);
        Assert.AreEqual(true, intervalTimerUpdate.UpdateWaitPeriod(40));
        autoResetEvent.WaitOne(1_000);
        Assert.IsTrue(timerCounter >= 1);
        autoResetEvent.WaitOne(1_000);
        Assert.IsTrue(timerCounter >= 2);


        timerCallBackRunInfo.RunCallbackOnThisThread();
        Assert.AreEqual(false, intervalTimerUpdate.UpdateWaitPeriod(40));
    }


    [TestMethod]
    public void PauseTest()
    {
        Assert.AreEqual(0, timerCounter);
        Assert.AreEqual(true, intervalTimerUpdate.UpdateWaitPeriod(40));
        autoResetEvent.WaitOne(1_000);
        Assert.IsTrue(timerCounter >= 1);
        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(true, intervalTimerUpdate.Pause());
        Thread.Sleep(50);
        var postPauseTimerCounter = timerCounter;
        Assert.AreEqual(true, timerCallBackRunInfo.IsPaused);
        Thread.Sleep(50);
        Assert.AreEqual(postPauseTimerCounter, timerCounter);
        Thread.Sleep(50);
        Assert.AreEqual(postPauseTimerCounter, timerCounter);

        timerCallBackRunInfo.RunCallbackOnThisThread();
        Assert.AreEqual(false, intervalTimerUpdate.Pause());
    }

    [TestMethod]
    public void ResumeTest()
    {
        Assert.AreEqual(0, timerCounter);
        Assert.AreEqual(true, intervalTimerUpdate.UpdateWaitPeriod(40));
        autoResetEvent.WaitOne(1_000);
        Assert.IsTrue(timerCounter >= 1);
        Assert.AreEqual(false, timerCallBackRunInfo.IsPaused);
        Assert.AreEqual(true, intervalTimerUpdate.Pause());
        Thread.Sleep(50);
        var postPauseTimerCounter = timerCounter;
        Assert.AreEqual(true, timerCallBackRunInfo.IsPaused);
        Thread.Sleep(50);
        Assert.AreEqual(true, intervalTimerUpdate.Resume());
        autoResetEvent.WaitOne(1_000);
        Assert.IsTrue(timerCounter > postPauseTimerCounter);
        autoResetEvent.WaitOne(1_000);
        Assert.IsTrue(timerCounter > postPauseTimerCounter + 1);
    }
}
