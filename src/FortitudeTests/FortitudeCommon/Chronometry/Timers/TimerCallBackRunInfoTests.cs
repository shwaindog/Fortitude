#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using static FortitudeCommon.Chronometry.Timers.Timer;
using Timer = System.Threading.Timer;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

[TestClass]
public class TimerCallBackRunInfoTests
{
    private readonly AutoResetEvent autoResetEvent = new(false);
    private volatile int callbackCounter;
    private Timer timer = null!;
    private TimerCallBackRunInfo timerCallBackRunInfo = null!;
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

    private TimerCallBackRunInfo CreateTimerCallBackRunInfo(DateTime firstScheduledTime) =>
        new()
        {
            Callback = waitCallback, FirstScheduledTime = firstScheduledTime, LastRunTime = DateTime.MinValue
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
    public void LowestNextScheduleTimeComparesLessThanOther()
    {
        var higherNextScheduleTime = new TimerCallBackRunInfo();
        higherNextScheduleTime.CopyFrom(timerCallBackRunInfo);
        higherNextScheduleTime.NextScheduleTime += TimeSpan.FromSeconds(10);

        Assert.AreEqual(-1, timerCallBackRunInfo.CompareTo(higherNextScheduleTime));
        Assert.AreEqual(1, higherNextScheduleTime.CompareTo(timerCallBackRunInfo));
        Assert.AreEqual(0, timerCallBackRunInfo.CompareTo(timerCallBackRunInfo));
        Assert.AreEqual(0, higherNextScheduleTime.CompareTo(higherNextScheduleTime));
    }

    [TestMethod]
    public void CopyFromTest()
    {
        var fromCopy = new TimerCallBackRunInfo();
        fromCopy.CopyFrom((IStoreState)timerCallBackRunInfo, CopyMergeFlags.Default);

        Assert.AreEqual(timerCallBackRunInfo.Callback, fromCopy.Callback);
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
    public void IncrementAndDecrementRefCountTest()
    {
        Assert.AreEqual(0, timerCallBackRunInfo.RefCount);
        Assert.AreEqual(1, timerCallBackRunInfo.IncrementRefCount());
        Assert.AreEqual(1, timerCallBackRunInfo.RefCount);
        timerCallBackRunInfo.RecycleOnRefCountZero = false;
        Assert.AreEqual(0, timerCallBackRunInfo.DecrementRefCount());
        Assert.AreEqual(0, timerCallBackRunInfo.RefCount);
    }


    [TestMethod]
    public void RecycleTest()
    {
        var recycler = new Recycler();
        var borrowedTimerCallbackRunInfo = recycler.Borrow<TimerCallBackRunInfo>();
        Assert.AreEqual(1, borrowedTimerCallbackRunInfo.RefCount);
        Assert.AreEqual(false, borrowedTimerCallbackRunInfo.IsInRecycler);
        Assert.AreEqual(0, borrowedTimerCallbackRunInfo.DecrementRefCount());
        Assert.AreEqual(0, borrowedTimerCallbackRunInfo.RefCount);
        Assert.AreEqual(true, borrowedTimerCallbackRunInfo.IsInRecycler);
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
