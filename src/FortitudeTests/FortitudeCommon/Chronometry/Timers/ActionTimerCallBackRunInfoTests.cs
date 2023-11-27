#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Types;
using static FortitudeCommon.Chronometry.Timers.Timer;
using Timer = System.Threading.Timer;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

[TestClass]
public class ActionTimerCallBackRunInfoTests
{
    private readonly AutoResetEvent autoResetEvent = new(false);
    private volatile int callbackCounter;
    private Timer timer = null!;
    private ActionStateTimerCallBackRunInfo<object> timerCallBackRunInfo = null!;
    private Action<object?> waitCallback = null!;

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

    private ActionStateTimerCallBackRunInfo<object> CreateTimerCallBackRunInfo(DateTime firstScheduledTime) =>
        new()
        {
            Action = waitCallback, State = new object(), FirstScheduledTime = firstScheduledTime
            , LastRunTime = DateTime.MinValue
            , MaxNumberOfCalls = 1, NextScheduleTime = firstScheduledTime
            , IntervalPeriodTimeSpan = TimeSpan.FromMilliseconds(500), RegisteredTimer = timer
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
        var fromCopy = new ActionStateTimerCallBackRunInfo<object>();
        fromCopy.CopyFrom((IStoreState)timerCallBackRunInfo, CopyMergeFlags.Default);

        Assert.AreEqual(timerCallBackRunInfo.Action, fromCopy.Action);
        Assert.AreEqual(timerCallBackRunInfo.State, fromCopy.State);
        Assert.AreEqual(timerCallBackRunInfo.FirstScheduledTime, fromCopy.FirstScheduledTime);
        Assert.AreEqual(timerCallBackRunInfo.LastRunTime, fromCopy.LastRunTime);
        Assert.AreEqual(timerCallBackRunInfo.NextScheduleTime, fromCopy.NextScheduleTime);
        Assert.AreEqual(timerCallBackRunInfo.IntervalPeriodTimeSpan, fromCopy.IntervalPeriodTimeSpan);
        Assert.AreEqual(timerCallBackRunInfo.CurrentNumberOfCalls, fromCopy.CurrentNumberOfCalls);
        Assert.AreEqual(timerCallBackRunInfo.MaxNumberOfCalls, fromCopy.MaxNumberOfCalls);
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
