// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeTests.FortitudeCommon.AsyncProcessing.Tasks;

[TestClass]
public class ReusableValueTaskSourceTests
{
    private ReusableValueTaskSource<decimal> decimalReusableValueTaskSource = null!;
    private ReusableValueTaskSource<object>  objectReusableValueTaskSource  = null!;

    [TestInitialize]
    public void Setup()
    {
        decimalReusableValueTaskSource = new ReusableValueTaskSource<decimal>();
        objectReusableValueTaskSource  = new ReusableValueTaskSource<object>();
    }

    [TestCleanup]
    public void TearDown()
    {
        ReusableValueTaskSource<decimal>.AfterGetResultRecycleInstanceMs = 10_000;
        ReusableValueTaskSource<object>.AfterGetResultRecycleInstanceMs  = 10_000;
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task ReusableValueTaskSourceClearsTaskOnReset()
    {
        await RunReusableValueTaskAndTaskOperations(decimalReusableValueTaskSource, 123.456789m);
        decimalReusableValueTaskSource.StateReset();
        await RunReusableValueTaskAndTaskOperations(decimalReusableValueTaskSource, 123.456789m);
        await RunReusableValueTaskAndTaskOperations(objectReusableValueTaskSource, "Some Expected String Obj");
        objectReusableValueTaskSource.StateReset();
        await RunReusableValueTaskAndTaskOperations(objectReusableValueTaskSource, "Some Expected String Obj");
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task ResetClearsCompletionsAddedOnPriorTaskAdditions()
    {
        await RunResetAfterSettingCompletionThenPerformNormalOperations(decimalReusableValueTaskSource, 123.456789m);
        await RunResetAfterSettingCompletionThenPerformNormalOperations
            (objectReusableValueTaskSource, "Some Expected String Obj");
    }

    [TestMethod]
    public async Task ReusableDecimalTaskIsReturnedToRecyclerAfterValueIsRead()
    {
        var recycler = new Recycler();
        decimalReusableValueTaskSource.AutoRecycleAtRefCountZero = true;
        decimalReusableValueTaskSource.Recycler                  = recycler;
        Assert.AreEqual(1, decimalReusableValueTaskSource.RefCount);
        var threadPoolTimer = TimerContext.CreateUpdateableTimer();
        decimalReusableValueTaskSource.ResponseTimeoutAndRecycleTimer    = threadPoolTimer;
        ReusableValueTaskSource<decimal>.AfterGetResultRecycleInstanceMs = 10;

        Assert.IsFalse(decimalReusableValueTaskSource.IsInRecycler);
        await RunReusableValueTaskAndTaskOperations(decimalReusableValueTaskSource, 123.456789m);
        await Task.Delay(200);
        Assert.IsTrue(decimalReusableValueTaskSource.IsInRecycler);

        var checkDecimalReusableValueTaskSource = recycler.Borrow<ReusableValueTaskSource<decimal>>();
        Assert.AreSame(decimalReusableValueTaskSource, checkDecimalReusableValueTaskSource);
        Assert.IsFalse(checkDecimalReusableValueTaskSource.IsInRecycler);

        await RunReusableValueTaskAndTaskOperations(checkDecimalReusableValueTaskSource, 123.456789m);
        await Task.Delay(200);
        Assert.IsTrue(checkDecimalReusableValueTaskSource.IsInRecycler);
    }

    [TestMethod]
    public async Task ReusableObjTaskIsReturnedToRecyclerAfterValueIsRead()
    {
        var recycler = new Recycler();
        objectReusableValueTaskSource.AutoRecycleAtRefCountZero = true;
        objectReusableValueTaskSource.Recycler                  = recycler;
        Assert.AreEqual(1, objectReusableValueTaskSource.RefCount);
        var threadPoolTimer = TimerContext.CreateUpdateableTimer();
        objectReusableValueTaskSource.ResponseTimeoutAndRecycleTimer    = threadPoolTimer;
        ReusableValueTaskSource<object>.AfterGetResultRecycleInstanceMs = 10;

        Assert.IsFalse(objectReusableValueTaskSource.IsInRecycler);
        await RunReusableValueTaskAndTaskOperations(objectReusableValueTaskSource, "Some Expected String Obj");
        await Task.Delay(200);
        Assert.IsTrue(objectReusableValueTaskSource.IsInRecycler);

        var checkObjReusableValueTaskSource = recycler.Borrow<ReusableValueTaskSource<object>>();
        Assert.AreSame(objectReusableValueTaskSource, checkObjReusableValueTaskSource);
        Assert.IsFalse(checkObjReusableValueTaskSource.IsInRecycler);

        await RunReusableValueTaskAndTaskOperations(checkObjReusableValueTaskSource, "Some Expected String Obj");
        await Task.Delay(200);
        Assert.IsTrue(checkObjReusableValueTaskSource.IsInRecycler);
    }

    private async Task RunResetAfterSettingCompletionThenPerformNormalOperations<T>
    (
        ReusableValueTaskSource<T> reusableVTaskSource, T expectedResult)
    {
        var valueTask = reusableVTaskSource.GenerateValueTask();
        var toTask    = valueTask.ToTask();

        Assert.IsFalse(valueTask.IsCompleted);
        Assert.IsFalse(toTask.IsCompleted);

        var shouldNeverRun = new bool[1];
        valueTask.GetAwaiter().OnCompleted(() => shouldNeverRun[0] = true);
        toTask.GetAwaiter().OnCompleted(() => shouldNeverRun[0]    = true);

        reusableVTaskSource.StateReset();

        var valueTask2 = reusableVTaskSource.GenerateValueTask();
        var toTask2    = valueTask.ToTask();
        Assert.AreSame(toTask, toTask2);

        var valueTaskCompleteCalled = new bool[1];
        var taskCompleteCalled      = new bool[1];

        valueTask2.GetAwaiter().OnCompleted(() => valueTaskCompleteCalled[0] = true);
        toTask.GetAwaiter().OnCompleted(() => taskCompleteCalled[0]          = true);

        await SetResultAssertComplete
            (reusableVTaskSource, expectedResult, valueTask2, toTask2, valueTaskCompleteCalled, taskCompleteCalled);
        Assert.IsFalse(shouldNeverRun[0]);
    }

    private async Task RunReusableValueTaskAndTaskOperations<T>
    (ReusableValueTaskSource<T> reusableVTaskSource
      , T expectedResult)
    {
        var valueTask = reusableVTaskSource.GenerateValueTask();
        var toTask    = valueTask.ToTask();
        reusableVTaskSource.AutoRecycleOnTaskComplete = true;

        Assert.IsFalse(valueTask.IsCompleted);
        Assert.IsFalse(toTask.IsCompleted);

        var valueTaskCompleteCalled = new bool[1];
        var taskCompleteCalled      = new bool[1];

        valueTask.GetAwaiter().OnCompleted(() => valueTaskCompleteCalled[0] = true);
        toTask.GetAwaiter().OnCompleted(() => taskCompleteCalled[0]         = true);

        await SetResultAssertComplete
            (reusableVTaskSource, expectedResult, valueTask, toTask, valueTaskCompleteCalled, taskCompleteCalled);
    }

    private static async Task SetResultAssertComplete<T>
    (ReusableValueTaskSource<T> reusableVTaskSource
      , T expectedResult
      , ValueTask<T> valueTask, Task<T> toTask, bool[] valueTaskCompleteCalled, bool[] taskCompleteCalled)
    {
        reusableVTaskSource.TrySetResult(expectedResult);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.IsTrue(toTask.IsCompleted);
        Assert.IsTrue(valueTaskCompleteCalled[0]);
        var valueTaskResult = await valueTask;
        var toTaskResult    = await toTask;
        Assert.AreEqual(expectedResult, valueTaskResult);
        Assert.AreEqual(expectedResult, toTaskResult);
        await Task.Delay(20); // time for the completion to finish
        Assert.IsTrue(taskCompleteCalled[0]);
    }
}
