// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Types.Mutable;
using static FortitudeCommon.Chronometry.Timers.UpdateableTimer;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

[TestClass]
public class WaitCallbackTimerCallBackRunInfoTests
{
    private readonly AutoResetEvent autoResetEvent = new(false);

    private volatile int   callbackCounter;
    private          Timer timer = null!;

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
            WaitCallback     = waitCallback, FirstScheduledTime = firstScheduledTime, LastRunTime = DateTime.MinValue
          , MaxNumberOfCalls = 1, NextScheduleTime              = firstScheduledTime

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
        fromCopy.CopyFrom((ITransferState)timerCallBackRunInfo, CopyMergeFlags.Default);

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
    [Timeout(10_000)]
    public async Task RunCallbackOnThreadPoolTest()
    {
        Assert.AreEqual(0, callbackCounter);
        timerCallBackRunInfo.MaxNumberOfCalls = 2;
        var nextExecutionVTask = timerCallBackRunInfo.NextThreadPoolExecutionAsync();
        var wasScheduled       = timerCallBackRunInfo.RunCallbackOnThreadPool();
        Assert.IsTrue(wasScheduled);
        Assert.AreEqual(2, timerCallBackRunInfo.RefCount);
        var firstTime = await nextExecutionVTask;
        var now       = DateTime.Now;
        Assert.IsTrue(firstTime + TimeSpan.FromMilliseconds(100) > now
                    , $"firstTime {firstTime} is more than 100ms less than {now}");
        Assert.AreEqual(1, callbackCounter);
        await Task.Delay(50);
        Assert.AreEqual(1, timerCallBackRunInfo.RefCount);
        var secondExecutionTask = timerCallBackRunInfo.NextThreadPoolExecutionAsync();
        wasScheduled = timerCallBackRunInfo.RunCallbackOnThreadPool();
        Assert.IsTrue(wasScheduled);
        Assert.AreEqual(2, timerCallBackRunInfo.RefCount);
        var secondTime = await secondExecutionTask;
        now = DateTime.Now;
        Assert.IsTrue(secondTime + TimeSpan.FromMilliseconds(100) > now
                    , $"secondTime {secondTime} is more than 100ms less than {now}");
        await Task.Delay(50);
        Assert.AreEqual(1, timerCallBackRunInfo.RefCount);
        Assert.AreEqual(2, callbackCounter);

        // Paused callbacks can still be run manually
        var pausedTimerCallbackInfo = CreateTimerCallBackRunInfo(timerCallBackRunInfo.FirstScheduledTime);
        pausedTimerCallbackInfo.IsPaused = true;
        Assert.AreEqual(2, callbackCounter);
        var thirdExecutionTask = pausedTimerCallbackInfo.NextThreadPoolExecutionAsync();
        wasScheduled = pausedTimerCallbackInfo.RunCallbackOnThreadPool();
        Assert.IsTrue(wasScheduled);
        Assert.AreEqual(2, pausedTimerCallbackInfo.RefCount);
        var thirdTime = await thirdExecutionTask;
        now = DateTime.Now;
        Assert.IsTrue(thirdTime + TimeSpan.FromMilliseconds(100) > now
                    , $"thirdTime {thirdTime} is more than 100ms less than {now}");
        await Task.Delay(50);
        Assert.AreEqual(1, pausedTimerCallbackInfo.RefCount);
        Assert.AreEqual(3, callbackCounter);
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
