#region

using System.Linq.Expressions;
using System.Threading.Tasks.Sources;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Tasks;

public interface IMessageResponseSource : IValueTaskSource, IRecyclableObject<IMessageResponseSource> { }

public interface IReusableMessageResponseSource<T> : IMessageResponseSource
{
    // ReSharper disable once UnusedMemberInSuper.Global
    Task<T> AsTask { get; }
}

// credit here to Microsoft.AspNetCore.Server.Kestrel.Core.Internal.ManualResetValueTaskSource
// copy taken from Microsoft.AspNetCore.Server.Kestrel.Core.Internal.ManualResetValueTaskSource in Microsoft.AspNetCore.Server.IIS
public class ReusableValueTaskSource<T> : Task<T>, IValueTaskSource<T>, IReusableMessageResponseSource<T>
    , IStoreState<ReusableValueTaskSource<T>>
{
    public static int AfterGetResultRecycleInstanceMs = 10_000;
    private static readonly Action<IMessageResponseSource?> DecrementUsageAction = DecrementUsage;
    private static readonly Action<Task<T>, object?> CheckTaskComplete = CheckAsTaskComplete;
    private static readonly Action<ReusableValueTaskSource<T>?> CheckTaskCompleteAgain = CheckAsTaskCompleteAgain;

    private static readonly Func<object?, T> NeverRunTask = _ =>
        throw new ArgumentException("This should never be called");

    private static readonly Action<Task<T>> ResetTaskAction;
    private static readonly Func<ValueTask<T>, object?> ExtractValueTaskObj;
    private readonly TaskCompletionSource<T> taskCompletionSource = new();
    protected ManualResetValueTaskSourceCore<T> Core; // mutable struct; do not make this readonly
    private ITimerUpdate? lastTimerActive;
    private int refCount;

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

        var paramValueTask = Expression.Parameter(typeof(ValueTask<T>));
        Expression mObj = Expression.Field(paramValueTask, "_obj");
        ExtractValueTaskObj = Expression.Lambda<Func<ValueTask<T>, object?>>(mObj, paramValueTask).Compile();
    }

    public ReusableValueTaskSource() : base(NeverRunTask, null) { }

    public bool RunContinuationsAsynchronously
    {
        get => Core.RunContinuationsAsynchronously;
        set => Core.RunContinuationsAsynchronously = value;
    }

    public short Version => Core.Version;
    public IActionTimer? RecycleTimer { get; set; }

    public Task<T> AsTask
    {
        get
        {
            var returnTask = taskCompletionSource.Task;
            returnTask.ContinueWith(CheckTaskComplete, this);
            return returnTask;
        }
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((ReusableValueTaskSource<T>)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; }
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }

    public int DecrementRefCount()
    {
        if (Interlocked.Decrement(ref refCount) == 0 && RecycleOnRefCountZero) Recycle();
        return refCount;
    }

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (!IsInRecycler && (refCount == 0 || !RecycleOnRefCountZero || AutoRecycledByProducer))
        {
            Reset();
            Recycler!.Recycle(this);
        }

        return IsInRecycler;
    }

    public void CopyFrom(IMessageResponseSource source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((ReusableValueTaskSource<T>)source, copyMergeFlags);
    }

    void IValueTaskSource.GetResult(short token)
    {
        StartRecycleTimer();
        Core.GetResult(token);
    }

    public virtual void CopyFrom(ReusableValueTaskSource<T> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Core = source.Core;
    }

    public T GetResult(short token) => Core.GetResult(token);
    public ValueTaskSourceStatus GetStatus(short token) => Core.GetStatus(token);

    public void OnCompleted(Action<object?> continuation, object? state, short token
        , ValueTaskSourceOnCompletedFlags flags) =>
        Core.OnCompleted(continuation, state, token, flags);

    private static void DecrementUsage(IMessageResponseSource? toDecrement)
    {
        toDecrement?.DecrementRefCount();
    }

    private static void CheckAsTaskComplete(Task<T> owningTask, object? state)
    {
        if (state is ReusableValueTaskSource<T> reusableValueTaskSource)
        {
            if (owningTask.IsCompleted)
            {
                reusableValueTaskSource.StartRecycleTimer();
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
            state.StartRecycleTimer();
        }
        else
        {
            state.lastTimerActive?.DecrementRefCount();
            state.lastTimerActive
                = state.RecycleTimer?.RunIn(AfterGetResultRecycleInstanceMs, state, CheckTaskCompleteAgain);
        }
    }

    private void StartRecycleTimer()
    {
        if (RecycleTimer != null)
        {
            if (lastTimerActive != null) lastTimerActive.DecrementRefCount();

            lastTimerActive = RecycleTimer.RunIn(AfterGetResultRecycleInstanceMs, this, DecrementUsageAction);
        }
    }

    public virtual void Reset()
    {
        ResetTaskAction(taskCompletionSource.Task);
        RunContinuationsAsynchronously = false;
        Core.Reset();
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

    // ReSharper disable once UnusedMember.Global
    public ValueTaskSourceStatus GetStatus()
    {
        var status = Core.GetStatus(Core.Version);
        if (status == ValueTaskSourceStatus.Canceled || status == ValueTaskSourceStatus.Faulted) StartRecycleTimer();
        return status;
    }

    public void TrySetResult(T result)
    {
        if (Core.GetStatus(Core.Version) == ValueTaskSourceStatus.Pending)
        {
            Core.SetResult(result);
            taskCompletionSource.TrySetResult(result);
        }
    }

    // ReSharper disable once UnusedMember.Global
    public static Task<T>? ExtractTask(ValueTask<T> toConvert)
    {
        var obj = ExtractValueTaskObj(toConvert);
        if (obj is ReusableValueTaskSource<T> reusableObj) return reusableObj.AsTask;

        return null;
    }

    public override string ToString() => $"{GetType()}{{ {nameof(refCount)}: {refCount} }}";
    // ReSharper disable StaticMemberInGenericType


    // ReSharper restore StaticMemberInGenericType
    // ReSharper disable StaticMemberInGenericType


    // ReSharper restore StaticMemberInGenericType
}
