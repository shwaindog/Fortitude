#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Trading.Executions;

#endregion

namespace FortitudeMarketsCore.Trading.Executions;

public class Executions : IExecutions
{
    private IList<IExecution> executions;
    private int refCount = 0;

    public Executions() => executions = new List<IExecution>();

    public Executions(IExecutions toClone)
    {
        executions = toClone.Select(e => e.Clone()).ToList();
    }

    public Executions(IList<IExecution> executions) => this.executions = executions;

    public int Count => executions.Count;

    public IExecution this[int index]
    {
        get => executions[index];
        set => executions[index] = value;
    }

    public void Add(IExecution execution)
    {
        executions.Add(execution);
    }

    public IExecutions Clone() => new Executions(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IExecution> GetEnumerator() => executions.GetEnumerator();

    public void CopyFrom(IExecutions source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        executions = source.ToList();
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IExecutions)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => Interlocked.Decrement(ref refCount);

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (refCount == 0 || !RecycleOnRefCountZero) Recycler!.Recycle(this);

        return IsInRecycler;
    }
}
