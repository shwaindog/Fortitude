// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks.Sources;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.AsyncProcessing.Tasks;

public interface IAsyncResponseSource : IValueTaskSource, IRecyclableObject
{
    bool IsCompleted  { get; }
    Type ResponseType { get; }
    void SetException(Exception error);
}

public interface IReusableAsyncResponse<in T> : IAsyncResponseSource
{
    short Version { get; }
    void  TrySetResult(T result);
    void  SetResult(T result);
}

public interface IReusableAsyncResponseSource<T> : IReusableAsyncResponse<T>
{
    // ReSharper disable once UnusedMemberInSuper.Global
    Task<T>       AsTask                    { get; }
    bool          AutoDecrementOnGetResult  { get; set; }
    bool          AutoRecycleOnTaskComplete { get; set; }
    ValueTask<T>? AwaitingValueTask         { get; set; }
    void          TrySetResultFromAwaitingTask(ValueTask<T> awaitingValueTask);
    void          TrySetResultFromAwaitingTask(Task<T> awaitingTask);
    ValueTask<T>  GenerateValueTask(Action<object?>? continuation = null);
}

// credit here to Microsoft.AspNetCore.Server.Kestrel.Core.Internal.ManualResetValueTaskSource
// copy taken from Microsoft.AspNetCore.Server.Kestrel.Core.Internal.ManualResetValueTaskSource in Microsoft.AspNetCore.Server.IIS
public class ReusableValueTaskSource<T> : RecyclableObject, IValueTaskSource<T>, IReusableAsyncResponseSource<T>
{
    // ReSharper disable once UnusedMember.Local
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(ReusableValueTaskSource<T>));

    public static int AfterGetResultRecycleInstanceMs = 60_000;

    private static readonly Action<IAsyncResponseSource?>       DecrementUsageAction           = DecrementUsage;
    private static readonly Action<Task<T>, object?>            CheckTaskComplete              = CheckAsTaskComplete;
    private static readonly Action<ReusableValueTaskSource<T>?> CheckTaskCompleteAgain         = CheckAsTaskCompleteAgain;
    private static readonly Action<ReusableValueTaskSource<T>?> ResponseTimedOut               = SetResponseTimedOut;
    private static readonly Action<IAsyncResponseSource?>       RecycleReusableValueTaskSource = RecycleCompleted;
    private static readonly Action<Task<T>>                     ResetTaskAction;
    private static readonly Type                                NonContingentPropertyType;

    private static int totalInstances;

    // protected ManualResetValueTaskSourceCore<T> Core; // mutable struct; do not make this readonly
    protected RecyclingValueTaskSource<T> Core; // mutable struct; do not make this readonly

    private int decrementCountDownTimerSet;

    private   bool hasCreatedAsTask;
    protected int  InstanceNumber;

    private ITimerUpdate? lastTimerActive;

    private TaskCompletionSource<T>? taskCompletionSource;

    static ReusableValueTaskSource()
    {
        var paramTask = Expression.Parameter(typeof(Task<T>));
        NonContingentPropertyType = NonPublicInvocator.GetNonPublicType
            (typeof(Task), "System.Threading.Tasks.Task+ContingentProperties");

        Expression mStateFlags           = Expression.Field(paramTask, typeof(Task), "m_stateFlags");
        Expression setMStateFlags        = Expression.Assign(mStateFlags, Expression.Constant(0x2000400));
        Expression mStateObj             = Expression.Field(paramTask, typeof(Task), "m_stateObject");
        Expression setMStateObj          = Expression.Assign(mStateObj, Expression.Constant(null));
        Expression mContinuationObj      = Expression.Field(paramTask, typeof(Task), "m_continuationObject");
        Expression setContinuationObj    = Expression.Assign(mContinuationObj, Expression.Constant(null));
        Expression mContingentProperties = Expression.Field(paramTask, typeof(Task), "m_contingentProperties");
        Expression setMContingentProperties = Expression.Assign
            (mContingentProperties, Expression.Constant(null, NonContingentPropertyType));
        Expression mResult    = Expression.Field(paramTask, typeof(Task<T>), "m_result");
        Expression setMResult = Expression.Assign(mResult, Expression.Constant(default(T), typeof(T)));

        Expression actionBlock = Expression.Block
            (setMStateFlags, setMStateObj, setContinuationObj, setMContingentProperties, setMResult);

        ResetTaskAction = Expression.Lambda<Action<Task<T>>>(actionBlock, paramTask).Compile();
    }

    public ReusableValueTaskSource() => InstanceNumber = Interlocked.Increment(ref totalInstances);

    public bool RunContinuationsAsynchronously
    {
        get => Core.RunContinuationsAsynchronously;
        set => Core.RunContinuationsAsynchronously = value;
    }

    public IActionTimer? ResponseTimeoutAndRecycleTimer { get; set; }

    public TimeSpan? ResponseTimeout { get; set; }

    private bool ShouldStartDecrementCounter => Interlocked.CompareExchange(ref decrementCountDownTimerSet, 1, 0) == 0;

    public override IRecycler? Recycler
    {
        get => base.Recycler;
        set
        {
            base.Recycler = value;
            if (value != null) Core.Recycler = value;
        }
    }

    public Type ResponseType => typeof(T);

    public short Version => Core.Version;

    public bool IsCompleted => GetStatus() != ValueTaskSourceStatus.Pending;

    public ValueTask<T>? AwaitingValueTask { get; set; }

    public bool AutoDecrementOnGetResult  { get; set; }
    public bool AutoRecycleOnTaskComplete { get; set; }

    public void TrySetResultFromAwaitingTask(Task<T> awaitingTask)
    {
        if (awaitingTask.IsCompleted)
            try
            {
                TrySetResult(awaitingTask.Result);
            }
            catch (Exception e)
            {
                SetException(e);
            }
        else
            awaitingTask.ContinueWith((task, result) =>
            {
                try
                {
                    TrySetResult(task.Result);
                }
                catch (Exception e)
                {
                    SetException(e);
                }
            }, null);
    }

    public void TrySetResultFromAwaitingTask(ValueTask<T> awaitingValueTask)
    {
        if (awaitingValueTask.IsCompleted)
            try
            {
                TrySetResult(awaitingValueTask.Result);
            }
            catch (AggregateException ae)
            {
                SetException(ae.InnerException!);
            }
            catch (Exception e)
            {
                SetException(e);
            }
        else
            awaitingValueTask.ToTask().ContinueWith((task, result) =>
            {
                try
                {
                    TrySetResult(task.Result);
                }
                catch (Exception e)
                {
                    SetException(e);
                }
            }, null);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Task<T> AsTask
    {
        get
        {
            taskCompletionSource ??= new TaskCompletionSource<T>();
            var returnTask = taskCompletionSource.Task;
            AutoDecrementOnGetResult = false;
            hasCreatedAsTask         = true;
            IncrementRefCount();
            returnTask.ContinueWith(CheckTaskComplete, this);
            return returnTask;
        }
    }

    public override int DecrementRefCount()
    {
        // logger.Debug("instanceNumber: {0} DecrementRefCount with refCount {1} stack trace - {2}", InstanceNumber
        //     , refCount, new StackTrace());
        if (Interlocked.Decrement(ref refCount) <= 0 && !IsInRecycler && AutoRecycleAtRefCountZero) DirectOrTimerRecycle();

        return refCount;
    }

    public override int IncrementRefCount() =>
        // logger.Debug("instanceNumber: {0} IncrementRefCount with refCount {1} stack trace - {2}", InstanceNumber
        //     , refCount, new StackTrace());
        Interlocked.Increment(ref refCount);

    void IValueTaskSource.GetResult(short token)
    {
        try
        {
            Core.GetResult(token);
        }
        finally
        {
            if (AutoDecrementOnGetResult && !IsInRecycler) DecrementRefCount();
        }
    }

    public void SetResult(T result)
    {
        Core.SetResult(result);
        taskCompletionSource?.SetResult(result);
    }

    public void SetException(Exception error)
    {
        Core.SetException(error);
        taskCompletionSource?.SetException(error);
    }

    public void TrySetResult(T result)
    {
        if (Core.GetStatus(Core.Version) == ValueTaskSourceStatus.Pending)
        {
            Core.SetResult(result);
            taskCompletionSource?.TrySetResult(result);
        }
    }

    public override void StateReset()
    {
        decrementCountDownTimerSet = 0;
        AutoRecycleOnTaskComplete  = false;
        AutoDecrementOnGetResult   = false;
        hasCreatedAsTask           = false;
        AwaitingValueTask          = null;
        if (taskCompletionSource != null) ResetTaskAction(taskCompletionSource.Task);
        RunContinuationsAsynchronously = false;
        ResponseTimeoutAndRecycleTimer = null;
        ResponseTimeout                = null;
        lastTimerActive?.DecrementRefCount();
        lastTimerActive = null;
        Core.Reset();
        base.StateReset();
    }

    public ValueTask<T> GenerateValueTask(Action<object?>? continuation = null)
    {
        AutoDecrementOnGetResult = !hasCreatedAsTask;
        if (continuation != null) OnCompleted(continuation, null, Version, ValueTaskSourceOnCompletedFlags.UseSchedulingContext);
        return new ValueTask<T>(this, Version);
    }

    public virtual T GetResult(short token)
    {
        T result;
        try
        {
            result = Core.GetResult(token);
        }
        finally
        {
            if (AutoDecrementOnGetResult && !IsInRecycler) DecrementRefCount();
        }
        return result;
    }

    public ValueTaskSourceStatus GetStatus(short token)
    {
        var status = Core.GetStatus(token);
        return status;
    }

    public void OnCompleted
    (Action<object?> continuation, object? state, short token
      , ValueTaskSourceOnCompletedFlags flags) =>
        Core.OnCompleted(continuation, state, token, flags);

    public ValueTask ToValueTask(Action<object?>? continuation = null)
    {
        AutoDecrementOnGetResult = !hasCreatedAsTask;
        if (continuation != null) OnCompleted(continuation, null, Version, ValueTaskSourceOnCompletedFlags.UseSchedulingContext);
        return new ValueTask(this, Version);
    }

    ~ReusableValueTaskSource()
    {
        Console.Out.WriteLine($"GC {GetType().Name}<{GetType().GenericTypeArguments.Select(t => t.Name).JoinToString()}>");
    }

    public bool DirectOrTimerRecycle()
    {
        if (ResponseTimeoutAndRecycleTimer != null)
        {
            lastTimerActive?.DecrementRefCount();
            lastTimerActive = ResponseTimeoutAndRecycleTimer.RunIn
                (AfterGetResultRecycleInstanceMs, this, RecycleReusableValueTaskSource);
        }
        else
        {
            return Recycle();
        }
        return false;
    }

    public void SetResponseTimeout(TimeSpan responseTimeout, IActionTimer? actionTimer)
    {
        var resolvedActionTimer = ResponseTimeoutAndRecycleTimer ?? actionTimer;
        resolvedActionTimer?.RunIn(responseTimeout, this, ResponseTimedOut);
    }

    private static void SetResponseTimedOut(ReusableValueTaskSource<T>? taskTimeOut)
    {
        taskTimeOut?.SetException(new ValueTaskTimeoutException("Timeout exceeded!"));
    }

    private static void RecycleCompleted(IAsyncResponseSource? toRecycle)
    {
        toRecycle?.Recycle();
    }

    private static void DecrementUsage(IAsyncResponseSource? toDecrement)
    {
        toDecrement?.DecrementRefCount();
    }

    private static void CheckAsTaskComplete(Task<T> owningTask, object? state)
    {
        if (state is ReusableValueTaskSource<T> reusableValueTaskSource)
        {
            if (owningTask.IsCompleted)
            {
                if (reusableValueTaskSource.AutoRecycleOnTaskComplete)
                    reusableValueTaskSource.Recycle();
                else
                    reusableValueTaskSource.StartRecycleDecrementRefCountTimer();
            }
            else
            {
                reusableValueTaskSource.lastTimerActive?.DecrementRefCount();
                reusableValueTaskSource.lastTimerActive
                    = reusableValueTaskSource.ResponseTimeoutAndRecycleTimer?.RunIn
                        (AfterGetResultRecycleInstanceMs, reusableValueTaskSource, CheckTaskCompleteAgain);
            }
        }
    }

    private static void CheckAsTaskCompleteAgain(ReusableValueTaskSource<T>? state)
    {
        if (state == null) return;
        if (state.taskCompletionSource?.Task.IsCompleted == true)
        {
            state.StartRecycleDecrementRefCountTimer();
        }
        else
        {
            state.lastTimerActive?.DecrementRefCount();
            state.lastTimerActive
                = state.ResponseTimeoutAndRecycleTimer?.RunIn(AfterGetResultRecycleInstanceMs, state, CheckTaskCompleteAgain);
        }
    }

    private void StartRecycleDecrementRefCountTimer()
    {
        if (!ShouldStartDecrementCounter) return;
        if (ResponseTimeoutAndRecycleTimer != null)
            switch (lastTimerActive)
            {
                case { IsFinished: true }:
                    lastTimerActive.DecrementRefCount();
                    lastTimerActive = ResponseTimeoutAndRecycleTimer.RunIn(AfterGetResultRecycleInstanceMs, this, DecrementUsageAction);
                    break;
                case null:
                    lastTimerActive = ResponseTimeoutAndRecycleTimer.RunIn(AfterGetResultRecycleInstanceMs, this, DecrementUsageAction);
                    break;
            }
        else
            DecrementRefCount();
    }

    // ReSharper disable once UnusedMember.Global
    public ValueTaskSourceStatus GetStatus()
    {
        var status = Core.GetStatus(Core.Version);
        if (status == ValueTaskSourceStatus.Canceled || status == ValueTaskSourceStatus.Faulted) StartRecycleDecrementRefCountTimer();
        // logger.Debug("instanceNumber: {0} IncrementRefCount with refCount {1} has status {2}", InstanceNumber
        //     , refCount, status);
        return status;
    }

    public override string ToString() =>
        $"{GetType().Name}[{typeof(T).Name}]({nameof(InstanceNumber)}: {InstanceNumber}, {nameof(Version)}: {Version}, " +
        $"{nameof(refCount)}: {refCount}, " +
        $"{nameof(IsCompleted)}: {IsCompleted}, {nameof(IsInRecycler)}: {IsInRecycler})";
}
