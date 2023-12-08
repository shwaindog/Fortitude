#region

using System.Linq.Expressions;
using System.Threading.Tasks.Sources;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.AsyncProcessing.Tasks;

public interface IAsyncResponseSource : IValueTaskSource, IRecyclableObject
{
    bool IsCompleted { get; }
    void SetException(Exception error);
}

public interface IReusableAsyncResponseSource<T> : IAsyncResponseSource
    , IReusableObject<IReusableAsyncResponseSource<T>>
{
    // ReSharper disable once UnusedMemberInSuper.Global
    Task<T> AsTask { get; }

    short Version { get; }
    ValueTask<T>? AwaitingValueTask { get; set; }
    void TrySetResultFromAwaitingTask(ValueTask<T> awaitingValueTask);
    void TrySetResultFromAwaitingTask(Task<T> awaitingTask);
    void TrySetResult(T result);
    void SetResult(T result);
    ValueTask<T> GenerateValueTask();
}

// credit here to Microsoft.AspNetCore.Server.Kestrel.Core.Internal.ManualResetValueTaskSource
// copy taken from Microsoft.AspNetCore.Server.Kestrel.Core.Internal.ManualResetValueTaskSource in Microsoft.AspNetCore.Server.IIS
public class ReusableValueTaskSource<T> : IValueTaskSource<T>, IReusableAsyncResponseSource<T>
    , IStoreState<ReusableValueTaskSource<T>>
{
    // ReSharper disable once UnusedMember.Local
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(ReusableValueTaskSource<T>));

    public static int AfterGetResultRecycleInstanceMs = 10_000;
    private static readonly Action<IAsyncResponseSource?> DecrementUsageAction = DecrementUsage;
    private static readonly Action<Task<T>, object?> CheckTaskComplete = CheckAsTaskComplete;
    private static readonly Action<ReusableValueTaskSource<T>?> CheckTaskCompleteAgain = CheckAsTaskCompleteAgain;
    private static readonly Action<IAsyncResponseSource?> RecycleReusableValueTaskSource = RecycleCompleted;

    private static readonly Action<Task<T>> ResetTaskAction;
    private static int totalInstances;
    private readonly TaskCompletionSource<T> taskCompletionSource = new();
    protected ManualResetValueTaskSourceCore<T> Core; // mutable struct; do not make this readonly
    private int decrementCountDownTimerSet;
    protected int InstanceNumber;
    private ITimerUpdate? lastTimerActive;
    private int refCount;
    private int shouldRecycle;

    static ReusableValueTaskSource()
    {
        var paramTask = Expression.Parameter(typeof(Task<T>));

        Expression mStateFlags = Expression.Field(paramTask, typeof(Task), "m_stateFlags");
        Expression setMStateFlags = Expression.Assign(mStateFlags, Expression.Constant(0x2000400));
        Expression mStateObj = Expression.Field(paramTask, typeof(Task), "m_stateObject");
        Expression setMStateObj = Expression.Assign(mStateObj, Expression.Constant(null));
        Expression mContinuationObj = Expression.Field(paramTask, typeof(Task), "m_continuationObject");
        Expression setContinuationObj = Expression.Assign(mContinuationObj, Expression.Constant(null));
        Expression mContingentProperties = Expression.Field(paramTask, typeof(Task), "m_contingentProperties");
        Expression setMContingentProperties = Expression.Assign(mContingentProperties
            , Expression.Constant(null,
                NonPublicInvocator.GetNonPublicType(typeof(Task), "System.Threading.Tasks.Task+ContingentProperties")));
        Expression mResult = Expression.Field(paramTask, typeof(Task<T>), "m_result");
        Expression setMResult = Expression.Assign(mResult, Expression.Constant(default(T), typeof(T)));

        Expression actionBlock = Expression.Block(setMStateFlags, setMStateObj, setContinuationObj
            , setMContingentProperties, setMResult);

        ResetTaskAction = Expression.Lambda<Action<Task<T>>>(actionBlock, paramTask).Compile();
    }

    public ReusableValueTaskSource() => InstanceNumber = Interlocked.Increment(ref totalInstances);

    private ReusableValueTaskSource(ReusableValueTaskSource<T> toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public bool RunContinuationsAsynchronously
    {
        get => Core.RunContinuationsAsynchronously;
        set => Core.RunContinuationsAsynchronously = value;
    }

    public IActionTimer? RecycleTimer { get; set; }

    private bool ShouldPerformRecycle => Interlocked.CompareExchange(ref shouldRecycle, 1, 0) == 0;

    private bool ShouldStartDecrementCounter => Interlocked.CompareExchange(ref decrementCountDownTimerSet, 1, 0) == 0;

    public short Version => Core.Version;

    public bool IsCompleted => GetStatus() != ValueTaskSourceStatus.Pending;

    public ValueTask<T>? AwaitingValueTask { get; set; }

    public ValueTask<T> GenerateValueTask() => new(this, Version);

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
            throw new ArgumentException("Expected awaitingTask to be completed");
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
            throw new ArgumentException("Expected awaitingValueTask to be completed");
    }

    public Task<T> AsTask
    {
        get
        {
            var returnTask = taskCompletionSource.Task;
            returnTask.ContinueWith(CheckTaskComplete, this);
            return returnTask;
        }
    }

    public int RefCount => refCount;
    public bool AutoRecycleAtRefCountZero { get; set; } = true;
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }

    public virtual int DecrementRefCount()
    {
        // logger.Debug("instanceNumber: {0} DecrementRefCount with refCount {1} stack trace - {2}", InstanceNumber
        //     , refCount, new StackTrace());
        if (AutoRecycleAtRefCountZero && Interlocked.Decrement(ref refCount) <= 0)
            if (ShouldPerformRecycle)
            {
                if (RecycleTimer != null)
                {
                    if (lastTimerActive != null) lastTimerActive.DecrementRefCount();

                    lastTimerActive = RecycleTimer.RunIn(AfterGetResultRecycleInstanceMs, this
                        , RecycleReusableValueTaskSource);
                }
                else
                {
                    Recycle();
                }
            }

        return refCount;
    }

    public int IncrementRefCount() =>
        // logger.Debug("instanceNumber: {0} IncrementRefCount with refCount {1} stack trace - {2}", InstanceNumber
        //     , refCount, new StackTrace());
        Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (!IsInRecycler && (refCount <= 0 || !AutoRecycleAtRefCountZero)) Recycler?.Recycle(this);

        return IsInRecycler;
    }

    void IValueTaskSource.GetResult(short token)
    {
        StartRecycleDecrementRefCountTimer();
        Core.GetResult(token);
    }

    public void SetResult(T result)
    {
        Core.SetResult(result);
        taskCompletionSource.SetResult(result);
    }

    public void SetException(Exception error)
    {
        Core.SetException(error);
        taskCompletionSource.SetException(error);
    }

    public void TrySetResult(T result)
    {
        if (Core.GetStatus(Core.Version) == ValueTaskSourceStatus.Pending)
        {
            Core.SetResult(result);
            taskCompletionSource.TrySetResult(result);
        }
    }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((ReusableValueTaskSource<T>)source, copyMergeFlags);

    public virtual void Reset()
    {
        refCount = 0;
        shouldRecycle = 0;
        decrementCountDownTimerSet = 0;
        AwaitingValueTask = null;
        ResetTaskAction(taskCompletionSource.Task);
        RunContinuationsAsynchronously = false;
        Core.Reset();
    }


    public IReusableAsyncResponseSource<T> CopyFrom(IReusableAsyncResponseSource<T> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ReusableValueTaskSource<T> reusableValueTaskSource)
        {
            AwaitingValueTask = reusableValueTaskSource.AwaitingValueTask;
            RunContinuationsAsynchronously = reusableValueTaskSource.RunContinuationsAsynchronously;
            Core = reusableValueTaskSource.Core;
        }

        return this;
    }

    object ICloneable.Clone() => Clone();

    public IReusableAsyncResponseSource<T> Clone() =>
        Recycler?.Borrow<ReusableValueTaskSource<T>>().CopyFrom(this) ?? new ReusableValueTaskSource<T>(this);

    public virtual ReusableValueTaskSource<T> CopyFrom(ReusableValueTaskSource<T> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Core = source.Core;
        return this;
    }

    public T GetResult(short token) => Core.GetResult(token);
    public ValueTaskSourceStatus GetStatus(short token) => Core.GetStatus(token);

    public void OnCompleted(Action<object?> continuation, object? state, short token
        , ValueTaskSourceOnCompletedFlags flags) =>
        Core.OnCompleted(continuation, state, token, flags);

    public void CopyFrom(IAsyncResponseSource source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((ReusableValueTaskSource<T>)source, copyMergeFlags);
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
                reusableValueTaskSource.StartRecycleDecrementRefCountTimer();
            }
            else
            {
                reusableValueTaskSource.lastTimerActive?.DecrementRefCount();
                reusableValueTaskSource.lastTimerActive
                    = reusableValueTaskSource.RecycleTimer?.RunIn(AfterGetResultRecycleInstanceMs
                        , reusableValueTaskSource, CheckTaskCompleteAgain);
            }
        }
    }

    private static void CheckAsTaskCompleteAgain(ReusableValueTaskSource<T>? state)
    {
        if (state == null) return;
        if (state.taskCompletionSource.Task.IsCompleted)
        {
            state.StartRecycleDecrementRefCountTimer();
        }
        else
        {
            state.lastTimerActive?.DecrementRefCount();
            state.lastTimerActive
                = state.RecycleTimer?.RunIn(AfterGetResultRecycleInstanceMs, state, CheckTaskCompleteAgain);
        }
    }

    private void StartRecycleDecrementRefCountTimer()
    {
        if (!ShouldStartDecrementCounter) return;
        if (RecycleTimer != null)
            switch (lastTimerActive)
            {
                case { IsFinished: true }:
                    lastTimerActive.DecrementRefCount();
                    lastTimerActive = RecycleTimer.RunIn(AfterGetResultRecycleInstanceMs, this, DecrementUsageAction);
                    break;
                case null:
                    lastTimerActive = RecycleTimer.RunIn(AfterGetResultRecycleInstanceMs, this, DecrementUsageAction);
                    break;
            }
        else
            DecrementRefCount();
    }

    // ReSharper disable once UnusedMember.Global
    public ValueTaskSourceStatus GetStatus()
    {
        var status = Core.GetStatus(Core.Version);
        if (status == ValueTaskSourceStatus.Canceled || status == ValueTaskSourceStatus.Faulted)
            StartRecycleDecrementRefCountTimer();
        // logger.Debug("instanceNumber: {0} IncrementRefCount with refCount {1} has status {2}", InstanceNumber
        //     , refCount, status);
        return status;
    }


    public override string ToString() =>
        $"{GetType().Name}[{typeof(T).Name}]({nameof(InstanceNumber)}: {InstanceNumber}, {nameof(Version)}: {Version}, " +
        $"{nameof(refCount)}: {refCount}, " +
        $"{nameof(IsCompleted)}: {IsCompleted}, {nameof(IsInRecycler)}: {IsInRecycler})";
}
