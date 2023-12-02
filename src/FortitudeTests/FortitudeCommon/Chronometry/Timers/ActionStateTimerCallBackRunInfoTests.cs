#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Types;
using static FortitudeCommon.Chronometry.Timers.Timer;
using Timer = System.Threading.Timer;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

[TestClass]
public class ActionStateTimerCallBackRunInfoTests
{
    private readonly AutoResetEvent autoResetEvent = new(false);
    private volatile int callbackCounter;
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
        timer = new Timer(NoopTimeCallback, null, MaxTimerMs, MaxTimerMs);
        var firstScheduledTime = TimeContext.UtcNow;
        timerCallBackRunInfo = CreateTimerCallBackRunInfo(firstScheduledTime);
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

    private void NoopTimeCallback(object? state) { }


    [TestMethod]
    public void CopyFromTest()
    {
        var fromCopy = new WaitCallbackTimerCallBackRunInfo();
        fromCopy.CopyFrom((IStoreState)timerCallBackRunInfo, CopyMergeFlags.Default);

        Assert.AreEqual(timerCallBackRunInfo.WaitCallback, fromCopy.WaitCallback);
        Assert.AreEqual(timerCallBackRunInfo.FirstScheduledTime, fromCopy.FirstScheduledTime);
        Assert.AreEqual(timerCallBackRunInfo.LastRunTime, fromCopy.LastRunTime);
        Assert.AreEqual(timerCallBackRunInfo.NextScheduleTime, fromCopy.NextScheduleTime);
        Assert.AreEqual(timerCallBackRunInfo.IntervalPeriodTimeSpan, fromCopy.IntervalPeriodTimeSpan);
        Assert.AreEqual(timerCallBackRunInfo.CurrentNumberOfCalls, fromCopy.CurrentNumberOfCalls);
        Assert.AreEqual(timerCallBackRunInfo.MaxNumberOfCalls, fromCopy.MaxNumberOfCalls);
        Assert.AreEqual(timerCallBackRunInfo.State, fromCopy.State);
        Assert.AreEqual(timerCallBackRunInfo.IsFinished, fromCopy.IsFinished);
        Assert.AreEqual(timerCallBackRunInfo.RegisteredTimer, fromCopy.RegisteredTimer);
    }

    [TestMethod]
    public void RunCallbackOnThreadPoolTest()
    {
        Assert.AreEqual(0, callbackCounter);
        timerCallBackRunInfo.RunCallbackOnThreadPool();
        autoResetEvent.WaitOne(1_000);
        Assert.AreEqual(1, callbackCounter);

        var pausedTimerCallbackInfo = CreateTimerCallBackRunInfo(timerCallBackRunInfo.FirstScheduledTime);
        pausedTimerCallbackInfo.IsPaused = true;
        Assert.AreEqual(1, callbackCounter);
        pausedTimerCallbackInfo.RunCallbackOnThreadPool();
        autoResetEvent.WaitOne(1_000);
        Assert.AreEqual(2, callbackCounter);
    }

    [TestMethod]
    public void RunCallbackOnThisThreadTest()
    {
        Assert.AreEqual(0, callbackCounter);
        timerCallBackRunInfo.RunCallbackOnThisThread();
        Assert.AreEqual(1, callbackCounter);

        var pausedTimerCallbackInfo = CreateTimerCallBackRunInfo(timerCallBackRunInfo.FirstScheduledTime);
        pausedTimerCallbackInfo.IsPaused = true;
        Assert.AreEqual(1, callbackCounter);
        pausedTimerCallbackInfo.RunCallbackOnThisThread();
        Assert.AreEqual(2, callbackCounter);
    }
}
