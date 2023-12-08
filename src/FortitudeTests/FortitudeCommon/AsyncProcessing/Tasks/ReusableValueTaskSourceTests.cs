#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using Timer = FortitudeCommon.Chronometry.Timers.Timer;

#endregion

namespace FortitudeTests.FortitudeCommon.AsyncProcessing.Tasks;

[TestClass]
public class ReusableValueTaskSourceTests
{
    private ReusableValueTaskSource<decimal> decimalReusableValueTaskSource = null!;
    private ReusableValueTaskSource<object> objectReusableValueTaskSource = null!;

    [TestInitialize]
    public void Setup()
    {
        decimalReusableValueTaskSource = new ReusableValueTaskSource<decimal>();
        objectReusableValueTaskSource = new ReusableValueTaskSource<object>();
    }

    [TestCleanup]
    public void TearDown()
    {
        ReusableValueTaskSource<decimal>.AfterGetResultRecycleInstanceMs = 10_000;
        ReusableValueTaskSource<object>.AfterGetResultRecycleInstanceMs = 10_000;
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task ReusableValueTaskSourceClearsTaskOnReset()
    {
        await RunReusableValueTaskAndTaskOperations(decimalReusableValueTaskSource, 123.456789m);
        decimalReusableValueTaskSource.Reset();
        await RunReusableValueTaskAndTaskOperations(decimalReusableValueTaskSource, 123.456789m);
        await RunReusableValueTaskAndTaskOperations(objectReusableValueTaskSource, "Some Expected String Obj");
        objectReusableValueTaskSource.Reset();
        await RunReusableValueTaskAndTaskOperations(objectReusableValueTaskSource, "Some Expected String Obj");
    }

    [TestMethod]
    [Timeout(10_000)]
    public async Task ResetClearsCompletionsAddedOnPriorTaskAdditions()
    {
        await RunResetAfterSettingCompletionThenPerformNormalOperations(decimalReusableValueTaskSource, 123.456789m);
        await RunResetAfterSettingCompletionThenPerformNormalOperations(objectReusableValueTaskSource
            , "Some Expected String Obj");
    }

    [TestMethod]
    public async Task ReusableDecimalTaskIsReturnedToRecyclerAfterValueIsRead()
    {
        var recycler = new Recycler();
        decimalReusableValueTaskSource.Recycler = recycler;
        objectReusableValueTaskSource.Recycler = recycler;
        var threadPoolTimer = new Timer();
        decimalReusableValueTaskSource.RecycleTimer = threadPoolTimer;
        objectReusableValueTaskSource.RecycleTimer = threadPoolTimer;
        ReusableValueTaskSource<decimal>.AfterGetResultRecycleInstanceMs = 10;
        ReusableValueTaskSource<object>.AfterGetResultRecycleInstanceMs = 10;

        Assert.IsFalse(decimalReusableValueTaskSource.IsInRecycler);
        Assert.IsFalse(objectReusableValueTaskSource.IsInRecycler);
        await RunReusableValueTaskAndTaskOperations(decimalReusableValueTaskSource, 123.456789m);
        await RunReusableValueTaskAndTaskOperations(objectReusableValueTaskSource, "Some Expected String Obj");
        await Task.Delay(50);

        Assert.IsTrue(decimalReusableValueTaskSource.IsInRecycler);
        Assert.IsTrue(objectReusableValueTaskSource.IsInRecycler);

        var checkDecimalReusableValueTaskSource = recycler.Borrow<ReusableValueTaskSource<decimal>>();
        var checkObjReusableValueTaskSource = recycler.Borrow<ReusableValueTaskSource<object>>();

        Assert.AreSame(decimalReusableValueTaskSource, checkDecimalReusableValueTaskSource);
        Assert.AreSame(objectReusableValueTaskSource, checkObjReusableValueTaskSource);

        Assert.IsFalse(checkDecimalReusableValueTaskSource.IsInRecycler);
        Assert.IsFalse(checkObjReusableValueTaskSource.IsInRecycler);

        await RunReusableValueTaskAndTaskOperations(checkDecimalReusableValueTaskSource, 123.456789m);
        await RunReusableValueTaskAndTaskOperations(checkObjReusableValueTaskSource, "Some Expected String Obj");

        await Task.Delay(50);
        Assert.IsTrue(checkDecimalReusableValueTaskSource.IsInRecycler);
        Assert.IsTrue(checkObjReusableValueTaskSource.IsInRecycler);
    }

    private async Task RunResetAfterSettingCompletionThenPerformNormalOperations<T>(
        ReusableValueTaskSource<T> reusableVTaskSource, T expectedResult)
    {
        var valueTask = reusableVTaskSource.GenerateValueTask();
        var toTask = valueTask.ToTask();

        Assert.IsFalse(valueTask.IsCompleted);
        Assert.IsFalse(toTask.IsCompleted);

        var shouldNeverRun = new bool[1];
        valueTask.GetAwaiter().OnCompleted(() => shouldNeverRun[0] = true);
        toTask.GetAwaiter().OnCompleted(() => shouldNeverRun[0] = true);

        reusableVTaskSource.Reset();

        var valueTask2 = reusableVTaskSource.GenerateValueTask();
        var toTask2 = valueTask.ToTask();
        Assert.AreSame(toTask, toTask2);

        var valueTaskCompleteCalled = new bool[1];
        var taskCompleteCalled = new bool[1];

        valueTask2.GetAwaiter().OnCompleted(() => valueTaskCompleteCalled[0] = true);
        toTask.GetAwaiter().OnCompleted(() => taskCompleteCalled[0] = true);

        await SetResultAssertComplete(reusableVTaskSource, expectedResult, valueTask2, toTask2, valueTaskCompleteCalled
            , taskCompleteCalled);
        Assert.IsFalse(shouldNeverRun[0]);
    }

    private async Task RunReusableValueTaskAndTaskOperations<T>(ReusableValueTaskSource<T> reusableVTaskSource
        , T expectedResult)
    {
        var valueTask = reusableVTaskSource.GenerateValueTask();
        var toTask = valueTask.ToTask();

        Assert.IsFalse(valueTask.IsCompleted);
        Assert.IsFalse(toTask.IsCompleted);

        var valueTaskCompleteCalled = new bool[1];
        var taskCompleteCalled = new bool[1];

        valueTask.GetAwaiter().OnCompleted(() => valueTaskCompleteCalled[0] = true);
        toTask.GetAwaiter().OnCompleted(() => taskCompleteCalled[0] = true);

        await SetResultAssertComplete(reusableVTaskSource, expectedResult, valueTask, toTask, valueTaskCompleteCalled
            , taskCompleteCalled);
    }

    private static async Task SetResultAssertComplete<T>(ReusableValueTaskSource<T> reusableVTaskSource
        , T expectedResult
        , ValueTask<T> valueTask, Task<T> toTask, bool[] valueTaskCompleteCalled, bool[] taskCompleteCalled)
    {
        reusableVTaskSource.TrySetResult(expectedResult);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.IsTrue(toTask.IsCompleted);
        Assert.IsTrue(valueTaskCompleteCalled[0]);
        var valueTaskResult = await valueTask;
        var toTaskResult = await toTask;
        Assert.AreEqual(expectedResult, valueTaskResult);
        Assert.AreEqual(expectedResult, toTaskResult);
        await Task.Delay(20); // time for the completion to finish
        Assert.IsTrue(taskCompleteCalled[0]);
    }
}
