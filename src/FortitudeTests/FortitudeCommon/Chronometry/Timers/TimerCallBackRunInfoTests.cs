// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using static FortitudeCommon.Chronometry.Timers.UpdateableTimer;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

[TestClass]
public class TimerCallBackRunInfoTests
{
    private Timer timer = null!;

    private ConcreteTimerCallbackRunInfo timerCallBackRunInfo = null!;


    [TestInitialize]
    public void SetUp()
    {
        timer = new Timer(NoopTimeCallback, null, MaxTimerMs, MaxTimerMs);
        var firstScheduledTime = TimeContext.UtcNow;
        timerCallBackRunInfo = CreateTimerCallBackRunInfo(firstScheduledTime);
    }

    private ConcreteTimerCallbackRunInfo CreateTimerCallBackRunInfo(DateTime firstScheduledTime) =>
        new()
        {
            FirstScheduledTime = firstScheduledTime, LastRunTime = DateTime.MinValue

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
    public void LowestNextScheduleTimeComparesLessThanOther()
    {
        var higherNextScheduleTime = new ConcreteTimerCallbackRunInfo();
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
        var fromCopy = new ConcreteTimerCallbackRunInfo();
        fromCopy.CopyFrom(timerCallBackRunInfo);

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
    public void IncrementAndDecrementRefCountTest()
    {
        Assert.AreEqual(1, timerCallBackRunInfo.RefCount);
        Assert.AreEqual(2, timerCallBackRunInfo.IncrementRefCount());
        Assert.AreEqual(2, timerCallBackRunInfo.RefCount);
        timerCallBackRunInfo.AutoRecycleAtRefCountZero = false;
        Assert.AreEqual(1, timerCallBackRunInfo.DecrementRefCount());
        Assert.AreEqual(1, timerCallBackRunInfo.RefCount);
    }


    [TestMethod]
    public void RecycleTest()
    {
        var recycler                     = new Recycler();
        var borrowedTimerCallbackRunInfo = recycler.Borrow<ConcreteTimerCallbackRunInfo>();
        Assert.AreEqual(1, borrowedTimerCallbackRunInfo.RefCount);
        Assert.AreEqual(false, borrowedTimerCallbackRunInfo.IsInRecycler);
        Assert.AreEqual(1, borrowedTimerCallbackRunInfo.DecrementRefCount());
        Assert.AreEqual(1, borrowedTimerCallbackRunInfo.RefCount);
        Assert.AreEqual(true, borrowedTimerCallbackRunInfo.IsInRecycler);
    }

    private class ConcreteTimerCallbackRunInfo : TimerCallBackRunInfo
    {
        public override bool RunCallbackOnThreadPool() => throw new NotImplementedException();

        public override bool RunCallbackOnThisThread() => throw new NotImplementedException();

        public override TimerCallBackRunInfo Clone() => new ConcreteTimerCallbackRunInfo();
    }
}
