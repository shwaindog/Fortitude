#region

using FortitudeCommon.Chronometry;
using Timer = FortitudeCommon.Chronometry.Timers.Timer;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry.Timers;

[TestClass]
public class TimerTests
{
    private Action actionCallback = null!;
    private volatile int actionCallbackCounter;
    private AutoResetEvent actionCallbackResetEvent = null!;
    private volatile int nonNullStateWaitCallbackCounter;
    private volatile int nullStateWaitCallbackCounter;
    private Timer timer = null!;
    private WaitCallback waitCallback = null!;
    private AutoResetEvent waitCallbackResetEvent = null!;

    [TestInitialize]
    public void SetUp()
    {
        actionCallbackResetEvent = new AutoResetEvent(false);
        waitCallbackResetEvent = new AutoResetEvent(false);
        nullStateWaitCallbackCounter = 0;
        nonNullStateWaitCallbackCounter = 0;
        actionCallbackCounter = 0;
        waitCallback = state =>
        {
            if (state == null)
                Interlocked.Increment(ref nullStateWaitCallbackCounter);
            else
                Interlocked.Increment(ref nonNullStateWaitCallbackCounter);
            waitCallbackResetEvent.Set();
        };
        actionCallback = () =>
        {
            Interlocked.Increment(ref actionCallbackCounter);
            actionCallbackResetEvent.Set();
        };
        timer = new Timer();
    }

    [TestCleanup]
    public void TearDown()
    {
        timer.StopAllTimers();
    }

    [TestMethod]
    public void RunInExecutesExpectedNumberOfCallbacks()
    {
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        var statelessWaitUpdate = timer.RunIn(20, waitCallback);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.AreEqual(1, nullStateWaitCallbackCounter);
        Assert.AreEqual(1, statelessWaitUpdate.RefCount);
        Assert.AreEqual(true, statelessWaitUpdate.IsFinished);

        Assert.AreEqual(0, actionCallbackCounter);
        var actionCallbackUpdate = timer.RunIn(20, actionCallback);
        actionCallbackResetEvent.WaitOne(1_000);
        Assert.AreEqual(1, actionCallbackCounter);
        Assert.AreEqual(1, actionCallbackUpdate.RefCount);
        Assert.AreEqual(true, actionCallbackUpdate.IsFinished);

        Assert.AreEqual(0, nonNullStateWaitCallbackCounter);
        var stateWaitUpdate = timer.RunIn(20, new object(), waitCallback);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.AreEqual(1, nonNullStateWaitCallbackCounter);
        Assert.AreEqual(1, stateWaitUpdate.RefCount);
        Assert.AreEqual(true, stateWaitUpdate.IsFinished);

        Assert.AreEqual(1, nullStateWaitCallbackCounter);
        Assert.AreEqual(0, statelessWaitUpdate.DecrementRefCount());
        statelessWaitUpdate = timer.RunIn(TimeSpan.FromMilliseconds(20), waitCallback);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.AreEqual(2, nullStateWaitCallbackCounter);
        Assert.AreEqual(1, statelessWaitUpdate.RefCount);
        Assert.AreEqual(true, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(0, statelessWaitUpdate.DecrementRefCount());

        Assert.AreEqual(1, actionCallbackCounter);
        Assert.AreEqual(0, actionCallbackUpdate.DecrementRefCount());
        actionCallbackUpdate = timer.RunIn(TimeSpan.FromMilliseconds(20), actionCallback);
        actionCallbackResetEvent.WaitOne(1_000);
        Assert.AreEqual(2, actionCallbackCounter);
        Assert.AreEqual(1, actionCallbackUpdate.RefCount);
        Assert.AreEqual(true, actionCallbackUpdate.IsFinished);
        Assert.AreEqual(0, actionCallbackUpdate.DecrementRefCount());

        Assert.AreEqual(1, nonNullStateWaitCallbackCounter);
        Assert.AreEqual(0, stateWaitUpdate.DecrementRefCount());
        stateWaitUpdate = timer.RunIn(TimeSpan.FromMilliseconds(20), new object(), waitCallback);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.AreEqual(2, nonNullStateWaitCallbackCounter);
        Assert.AreEqual(1, stateWaitUpdate.RefCount);
        Assert.AreEqual(true, stateWaitUpdate.IsFinished);
        Assert.AreEqual(0, stateWaitUpdate.DecrementRefCount());
    }


    [TestMethod]
    public void RunEveryIntMsStatelessWaitCallbackWorksAsExpected()
    {
        var intervalMs = 20;
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        var statelessWaitUpdate = timer.RunEvery(intervalMs, waitCallback);
        Assert.AreEqual(1, statelessWaitUpdate.RefCount);
        var firstScheduledTime = statelessWaitUpdate.NextScheduleDateTime;
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nullStateWaitCallbackCounter >= 1);
        Assert.IsTrue(statelessWaitUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs));
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nullStateWaitCallbackCounter >= 2);
        Assert.IsTrue(statelessWaitUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs * 2));
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nullStateWaitCallbackCounter >= 3);
        Assert.IsTrue(statelessWaitUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs * 3));
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        statelessWaitUpdate.Pause();
        Assert.AreEqual(true, statelessWaitUpdate.IsPaused);
        Thread.Sleep(intervalMs);
        var lastNullStateCounter = nullStateWaitCallbackCounter;
        Thread.Sleep(intervalMs * 2);
        Assert.AreEqual(lastNullStateCounter, nullStateWaitCallbackCounter);
        Thread.Sleep(intervalMs * 2);
        Assert.AreEqual(lastNullStateCounter, nullStateWaitCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        statelessWaitUpdate.Resume();
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nullStateWaitCallbackCounter >= lastNullStateCounter + 1);
        Assert.IsTrue(statelessWaitUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs * nullStateWaitCallbackCounter));
        var now = TimeContext.UtcNow;
        Assert.AreEqual(true, statelessWaitUpdate.UpdateWaitPeriod(200));
        Assert.IsTrue(now + TimeSpan.FromMilliseconds(200) <= statelessWaitUpdate.NextScheduleDateTime);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nullStateWaitCallbackCounter >= lastNullStateCounter + 2);
        Assert.AreEqual(true, statelessWaitUpdate.Cancel());
        Assert.AreEqual(true, statelessWaitUpdate.IsFinished);
        Thread.Sleep(200);
        lastNullStateCounter = nullStateWaitCallbackCounter;
        Thread.Sleep(200);
        Assert.AreEqual(lastNullStateCounter, nullStateWaitCallbackCounter);
        Thread.Sleep(intervalMs * 2);
        Assert.AreEqual(lastNullStateCounter, nullStateWaitCallbackCounter);
        Assert.AreEqual(0, statelessWaitUpdate.DecrementRefCount());
    }

    [TestMethod]
    public void RunEveryIntMsActionCallbackWorksAsExpected()
    {
        var intervalMs = 20;
        Assert.AreEqual(0, actionCallbackCounter);
        var actionUpdate = timer.RunEvery(intervalMs, actionCallback);
        var firstScheduledTime = actionUpdate.NextScheduleDateTime;
        Assert.AreEqual(1, actionUpdate.RefCount);
        actionCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(actionCallbackCounter >= 1);
        Assert.IsTrue(actionUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs));
        Assert.AreEqual(false, actionUpdate.IsFinished);
        actionCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(actionCallbackCounter >= 2);
        Assert.IsTrue(actionUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs * 2));
        Assert.AreEqual(false, actionUpdate.IsFinished);
        actionCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(actionCallbackCounter >= 3);
        Assert.IsTrue(actionUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs * 3));
        Assert.AreEqual(false, actionUpdate.IsFinished);
        actionUpdate.Pause();
        Assert.AreEqual(true, actionUpdate.IsPaused);
        Thread.Sleep(intervalMs);
        var lastNullStateCounter = actionCallbackCounter;
        Thread.Sleep(intervalMs * 2);
        Assert.AreEqual(lastNullStateCounter, actionCallbackCounter);
        Thread.Sleep(intervalMs * 2);
        Assert.AreEqual(lastNullStateCounter, actionCallbackCounter);
        Assert.AreEqual(false, actionUpdate.IsFinished);
        actionUpdate.Resume();
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(actionCallbackCounter >= lastNullStateCounter + 1);
        Assert.IsTrue(actionUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs * actionCallbackCounter));
        var now = TimeContext.UtcNow;
        Assert.AreEqual(true, actionUpdate.UpdateWaitPeriod(200));
        Assert.IsTrue(now + TimeSpan.FromMilliseconds(200) <= actionUpdate.NextScheduleDateTime);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(actionCallbackCounter >= lastNullStateCounter + 2);
        Assert.AreEqual(true, actionUpdate.Cancel());
        Assert.AreEqual(true, actionUpdate.IsFinished);
        Thread.Sleep(200);
        lastNullStateCounter = actionCallbackCounter;
        Thread.Sleep(200);
        Assert.AreEqual(lastNullStateCounter, actionCallbackCounter);
        Thread.Sleep(intervalMs * 2);
        Assert.AreEqual(lastNullStateCounter, actionCallbackCounter);
        Assert.AreEqual(0, actionUpdate.DecrementRefCount());
    }

    [TestMethod]
    public void RunEveryIntMsStatefulWaitCallbackWorksAsExpected()
    {
        var intervalMs = 20;
        Assert.AreEqual(0, nonNullStateWaitCallbackCounter);
        var statefulTimerUpdate = timer.RunEvery(intervalMs, new object(), waitCallback);
        Assert.AreEqual(1, statefulTimerUpdate.RefCount);
        var firstScheduledTime = statefulTimerUpdate.NextScheduleDateTime;
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nonNullStateWaitCallbackCounter >= 1);
        Assert.IsTrue(statefulTimerUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs));
        Assert.AreEqual(false, statefulTimerUpdate.IsFinished);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nonNullStateWaitCallbackCounter >= 2);
        Assert.IsTrue(statefulTimerUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs * 2));
        Assert.AreEqual(false, statefulTimerUpdate.IsFinished);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nonNullStateWaitCallbackCounter >= 3);
        Assert.IsTrue(statefulTimerUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs * 3));
        Assert.AreEqual(false, statefulTimerUpdate.IsFinished);
        statefulTimerUpdate.Pause();
        Assert.AreEqual(true, statefulTimerUpdate.IsPaused);
        Thread.Sleep(intervalMs);
        var lastNullStateCounter = nonNullStateWaitCallbackCounter;
        Thread.Sleep(intervalMs * 2);
        Assert.AreEqual(lastNullStateCounter, nonNullStateWaitCallbackCounter);
        Thread.Sleep(intervalMs * 2);
        Assert.AreEqual(lastNullStateCounter, nonNullStateWaitCallbackCounter);
        Assert.AreEqual(false, statefulTimerUpdate.IsFinished);
        statefulTimerUpdate.Resume();
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nonNullStateWaitCallbackCounter >= lastNullStateCounter + 1);
        Assert.IsTrue(statefulTimerUpdate.NextScheduleDateTime >=
                      firstScheduledTime + TimeSpan.FromMilliseconds(intervalMs * nonNullStateWaitCallbackCounter));
        var now = TimeContext.UtcNow;
        Assert.AreEqual(true, statefulTimerUpdate.UpdateWaitPeriod(200));
        Assert.IsTrue(now + TimeSpan.FromMilliseconds(200) <= statefulTimerUpdate.NextScheduleDateTime);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.IsTrue(nonNullStateWaitCallbackCounter >= lastNullStateCounter + 2);
        Assert.AreEqual(true, statefulTimerUpdate.Cancel());
        Assert.AreEqual(true, statefulTimerUpdate.IsFinished);
        Thread.Sleep(200);
        lastNullStateCounter = nonNullStateWaitCallbackCounter;
        Thread.Sleep(200);
        Assert.AreEqual(lastNullStateCounter, nonNullStateWaitCallbackCounter);
        Thread.Sleep(intervalMs * 2);
        Assert.AreEqual(lastNullStateCounter, nonNullStateWaitCallbackCounter);
        Assert.AreEqual(0, statefulTimerUpdate.DecrementRefCount());
    }


    [TestMethod]
    public void RunAtExecutesExpectedNumberOfCallbacks()
    {
        var soon = TimeSpan.FromMilliseconds(20);
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        var statelessWaitUpdate = timer.RunAt(TimeContext.UtcNow + soon, waitCallback);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.AreEqual(1, nullStateWaitCallbackCounter);
        Assert.AreEqual(true, statelessWaitUpdate.IsFinished);

        Assert.AreEqual(0, actionCallbackCounter);
        var actionCallbackUpdate = timer.RunAt(TimeContext.UtcNow + soon, actionCallback);
        actionCallbackResetEvent.WaitOne(1_000);
        Assert.AreEqual(1, actionCallbackCounter);
        Assert.AreEqual(true, actionCallbackUpdate.IsFinished);

        Assert.AreEqual(0, nonNullStateWaitCallbackCounter);
        var stateWaitUpdate = timer.RunAt(TimeContext.UtcNow + soon, new object(), waitCallback);
        waitCallbackResetEvent.WaitOne(1_000);
        Assert.AreEqual(1, nonNullStateWaitCallbackCounter);
        Assert.AreEqual(true, stateWaitUpdate.IsFinished);
    }

    [TestMethod]
    public void PauseAllThenResumeAllTimersStopsCallbacksThenResumesThem()
    {
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.AreEqual(0, actionCallbackCounter);
        var statelessWaitUpdate = timer.RunIn(20, waitCallback);
        var actionIntvlCallbackUpdate = timer.RunEvery(2_000, actionCallback);
        Thread.Sleep(40);
        timer.PauseAllTimers();
        Assert.AreEqual(1, nullStateWaitCallbackCounter);
        Assert.AreEqual(0, actionCallbackCounter);
        Assert.AreEqual(true, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(false, actionIntvlCallbackUpdate.IsFinished);
        Assert.AreEqual(true, actionIntvlCallbackUpdate.IsPaused);

        Assert.AreEqual(0, nonNullStateWaitCallbackCounter);
        var statefulTimerUpdate = timer.RunIn(20, new object(), waitCallback);
        Thread.Sleep(40);
        Assert.AreEqual(0, nonNullStateWaitCallbackCounter);
        Assert.AreEqual(false, statefulTimerUpdate.IsFinished);
        Assert.AreEqual(true, statefulTimerUpdate.IsPaused);

        timer.ResumeAllTimers();
        actionCallbackResetEvent.WaitOne(2_200);
        Assert.IsTrue(actionCallbackCounter >= 1);
        Assert.IsTrue(nonNullStateWaitCallbackCounter >= 1);
        Assert.AreEqual(1, nullStateWaitCallbackCounter);
    }

    [TestMethod]
    public void IndividualOneOffTimerUpdatePauseDoesNotAffectOtherRequests()
    {
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.AreEqual(0, actionCallbackCounter);
        var statelessWaitUpdate = timer.RunIn(200, waitCallback);
        var actionIntvlCallbackUpdate = timer.RunEvery(200, actionCallback);
        statelessWaitUpdate.Pause();
        actionCallbackResetEvent.WaitOne(400);
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.AreEqual(1, actionCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(true, statelessWaitUpdate.IsPaused);
        Assert.AreEqual(false, actionIntvlCallbackUpdate.IsFinished);
        Assert.AreEqual(false, actionIntvlCallbackUpdate.IsPaused);
        actionCallbackResetEvent.WaitOne(400);
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.IsTrue(actionCallbackCounter >= 2);
        statelessWaitUpdate.Resume();
        waitCallbackResetEvent.WaitOne(400);
        actionCallbackResetEvent.WaitOne(400);
        Assert.AreEqual(1, nullStateWaitCallbackCounter);
        Assert.IsTrue(actionCallbackCounter >= 3);
    }

    [TestMethod]
    public void IndividualIntervalTimerUpdatePauseDoesNotAffectOtherRequests()
    {
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.AreEqual(0, actionCallbackCounter);
        var statelessWaitUpdate = timer.RunEvery(200, waitCallback);
        var actionIntvlCallbackUpdate = timer.RunEvery(200, actionCallback);
        statelessWaitUpdate.Pause();
        actionCallbackResetEvent.WaitOne(400);
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.AreEqual(1, actionCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(true, statelessWaitUpdate.IsPaused);
        Assert.AreEqual(false, actionIntvlCallbackUpdate.IsFinished);
        Assert.AreEqual(false, actionIntvlCallbackUpdate.IsPaused);
        actionCallbackResetEvent.WaitOne(400);
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.IsTrue(actionCallbackCounter >= 2);
        statelessWaitUpdate.Resume();
        waitCallbackResetEvent.WaitOne(400);
        actionCallbackResetEvent.WaitOne(400);
        Assert.IsTrue(nullStateWaitCallbackCounter >= 1);
        Assert.IsTrue(actionCallbackCounter >= 3, $"actionCallbackCounter: {actionCallbackCounter}");
    }


    [TestMethod]
    public void OnOneOffTimerRunNextScheduledOneOffCallbackNowOnThisThread()
    {
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        var statelessWaitUpdate = timer.RunIn(1_000, waitCallback);
        statelessWaitUpdate.Pause();
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        statelessWaitUpdate.ExecuteNowOnThisThread();
        Assert.AreEqual(1, nullStateWaitCallbackCounter);
        Assert.AreEqual(true, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(DateTime.MaxValue, statelessWaitUpdate.NextScheduleDateTime);
    }

    [TestMethod]
    public void OnIntervalTimerRunNextScheduledOneOffCallbackNowOnThisThread()
    {
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        var statelessWaitUpdate = timer.RunEvery(1_000, waitCallback);
        statelessWaitUpdate.Pause();
        var nextScheduledRunTime = statelessWaitUpdate.NextScheduleDateTime;
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        statelessWaitUpdate.ExecuteNowOnThisThread();
        Assert.AreEqual(1, nullStateWaitCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(nextScheduledRunTime + TimeSpan.FromSeconds(1), statelessWaitUpdate.NextScheduleDateTime);
        statelessWaitUpdate.ExecuteNowOnThisThread();
        Assert.AreEqual(2, nullStateWaitCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(nextScheduledRunTime + TimeSpan.FromSeconds(2), statelessWaitUpdate.NextScheduleDateTime);
    }


    [TestMethod]
    public void OnOneOffTimerRunNextScheduledOneOffCallbackNowOnThreadPool()
    {
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        var statelessWaitUpdate = timer.RunIn(1_000, waitCallback);
        statelessWaitUpdate.Pause();
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        statelessWaitUpdate.ExecuteNowOnThreadPool();
        waitCallbackResetEvent.WaitOne(1_200);
        Assert.AreEqual(1, nullStateWaitCallbackCounter);
        Assert.AreEqual(true, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(DateTime.MaxValue, statelessWaitUpdate.NextScheduleDateTime);
    }

    [TestMethod]
    public void OnIntervalTimerRunNextScheduledOneOffCallbackNowOnThreadPool()
    {
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        var statelessWaitUpdate = timer.RunEvery(1_000, waitCallback);
        statelessWaitUpdate.Pause();
        var nextScheduledRunTime = statelessWaitUpdate.NextScheduleDateTime;
        Assert.AreEqual(0, nullStateWaitCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        statelessWaitUpdate.ExecuteNowOnThreadPool();
        waitCallbackResetEvent.WaitOne(1_200);
        Assert.AreEqual(1, nullStateWaitCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(nextScheduledRunTime + TimeSpan.FromSeconds(1), statelessWaitUpdate.NextScheduleDateTime);
        statelessWaitUpdate.ExecuteNowOnThreadPool();
        waitCallbackResetEvent.WaitOne(1_200);
        Assert.AreEqual(2, nullStateWaitCallbackCounter);
        Assert.AreEqual(false, statelessWaitUpdate.IsFinished);
        Assert.AreEqual(nextScheduledRunTime + TimeSpan.FromSeconds(2), statelessWaitUpdate.NextScheduleDateTime);
    }
}
