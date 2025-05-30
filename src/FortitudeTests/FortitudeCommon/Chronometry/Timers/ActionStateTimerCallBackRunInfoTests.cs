﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Types.Mutable;
using static FortitudeCommon.Chronometry.Timers.UpdateableTimer;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

[TestClass]
public class ActionStateTimerCallBackRunInfoTests

{
    private readonly AutoResetEvent autoResetEvent = new(false);

    private volatile int   callbackCounter;
    private          Timer timer = null!;

    private ActionTimerCallBackRunInfo timerCallBackRunInfo = null!;

    private Action waitCallback = null!;

    [TestInitialize]
    public void SetUp()
    {
        callbackCounter = 0;
        waitCallback = () =>
        {
            Interlocked.Increment(ref callbackCounter);
            autoResetEvent.Set();
        };
        timer = new Timer(NoopTimeCallback, null, MaxTimerMs, MaxTimerMs);
        var firstScheduledTime = TimeContext.UtcNow;
        timerCallBackRunInfo = CreateTimerCallBackRunInfo(firstScheduledTime);
    }

    private ActionTimerCallBackRunInfo CreateTimerCallBackRunInfo(DateTime firstScheduledTime) =>
        new()
        {
            Action      = waitCallback, FirstScheduledTime = firstScheduledTime
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
        var fromCopy = new ActionTimerCallBackRunInfo();
        fromCopy.CopyFrom(timerCallBackRunInfo);

        Assert.AreEqual(timerCallBackRunInfo.Action, fromCopy.Action);
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
    [Timeout(10_000)]
    public async Task RunCallbackOnThisThreadTest()
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
}
