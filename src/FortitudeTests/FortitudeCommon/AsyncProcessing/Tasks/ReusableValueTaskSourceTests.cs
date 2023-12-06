#region

using FortitudeCommon.AsyncProcessing.Tasks;

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

    [TestMethod]
    public void ReusableValueTaskSourceClearsTaskOnReset()
    {
        RunReusableValueTaskAndTaskOperations(decimalReusableValueTaskSource, 123.456789m);
        decimalReusableValueTaskSource.Reset();
        RunReusableValueTaskAndTaskOperations(decimalReusableValueTaskSource, 123.456789m);
        RunReusableValueTaskAndTaskOperations(objectReusableValueTaskSource, "Some Expected String Obj");
        objectReusableValueTaskSource.Reset();
        RunReusableValueTaskAndTaskOperations(objectReusableValueTaskSource, "Some Expected String Obj");
    }

    [TestMethod]
    public void ResetClearsCompletionsAddedOnPriorTaskAdditions()
    {
        RunResetAfterSettingCompletionThenPerformNormalOperations(decimalReusableValueTaskSource, 123.456789m);
        RunResetAfterSettingCompletionThenPerformNormalOperations(objectReusableValueTaskSource
            , "Some Expected String Obj");
    }

    private void RunResetAfterSettingCompletionThenPerformNormalOperations<T>(
        ReusableValueTaskSource<T> reusableVTaskSource, T expectedResult)
    {
        var valueTask = reusableVTaskSource.GenerateValueTask();
        var toTask = valueTask.ToTask();

        Assert.IsFalse(valueTask.IsCompleted);
        Assert.IsFalse(toTask.IsCompleted);

        var shouldNeverRun = false;
        valueTask.GetAwaiter().OnCompleted(() => shouldNeverRun = true);
        toTask.ContinueWith((_, _) => shouldNeverRun = true, null);

        reusableVTaskSource.Reset();

        var valueTask2 = reusableVTaskSource.GenerateValueTask();
        var toTask2 = valueTask.ToTask();
        Assert.AreSame(toTask, toTask2);

        var valueTaskCompleteCalled = false;
        var taskCompleteCalled = false;

        valueTask2.GetAwaiter().OnCompleted(() => valueTaskCompleteCalled = true);
        toTask2.ContinueWith((_, _) => taskCompleteCalled = true, null);

        SetResultAssertComplete(reusableVTaskSource, expectedResult, valueTask2, toTask2, valueTaskCompleteCalled
            , taskCompleteCalled);
        Assert.IsFalse(shouldNeverRun);
    }


    private void RunReusableValueTaskAndTaskOperations<T>(ReusableValueTaskSource<T> reusableVTaskSource
        , T expectedResult)
    {
        var valueTask = reusableVTaskSource.GenerateValueTask();
        var toTask = valueTask.ToTask();

        Assert.IsFalse(valueTask.IsCompleted);
        Assert.IsFalse(toTask.IsCompleted);

        var valueTaskCompleteCalled = false;
        var taskCompleteCalled = false;

        valueTask.GetAwaiter().OnCompleted(() => valueTaskCompleteCalled = true);
        toTask.ContinueWith((_, _) => taskCompleteCalled = true, null);

        SetResultAssertComplete(reusableVTaskSource, expectedResult, valueTask, toTask, valueTaskCompleteCalled
            , taskCompleteCalled);
    }

    private static void SetResultAssertComplete<T>(ReusableValueTaskSource<T> reusableVTaskSource, T expectedResult
        , ValueTask<T> valueTask, Task<T> toTask, in bool valueTaskCompleteCalled, in bool taskCompleteCalled)
    {
        reusableVTaskSource.TrySetResult(expectedResult);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.IsTrue(toTask.IsCompleted);
        Assert.IsTrue(valueTaskCompleteCalled);
        Thread.Sleep(20); // time to run on thread pool
        Assert.IsTrue(taskCompleteCalled);

        Assert.AreEqual(expectedResult, valueTask.Result);
        Assert.AreEqual(expectedResult, toTask.Result);
    }
}
