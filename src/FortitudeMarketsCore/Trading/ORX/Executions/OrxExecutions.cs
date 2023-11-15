#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Executions;

public class OrxExecutions : IExecutions
{
    private int refCount = 0;
    public OrxExecutions() => ExecutionsList = new List<OrxExecution>();

    public OrxExecutions(IExecutions toClone)
    {
        ExecutionsList = toClone.Select(e => new OrxExecution(e)).ToList();
    }

    public OrxExecutions(IEnumerable<IExecution> executions)
    {
        ExecutionsList = executions.Select(e => new OrxExecution(e)).ToList();
    }

    [OrxMandatoryField(0)] public List<OrxExecution> ExecutionsList { get; set; }

    public void Add(IExecution execution)
    {
        if (execution is OrxExecution orxExecution)
            ExecutionsList.Add(orxExecution);
        else
            ExecutionsList.Add(new OrxExecution(execution));
    }

    public int Count => ExecutionsList.Count;

    public IExecution this[int index]
    {
        get => ExecutionsList[index];
        set => ExecutionsList[index] = (OrxExecution)value;
    }

    public IExecutions Clone() => new OrxExecutions(this);

    public IEnumerator<IExecution> GetEnumerator() => ExecutionsList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CopyFrom(IExecutions executions, CopyMergeFlags copyMergeFlags)
    {
        var executionsCount = executions.Count;
        if (executionsCount > 0)
        {
            var orxExecutionsList = Recycler!.Borrow<List<OrxExecution>>();
            orxExecutionsList.Clear();
            for (var i = 0; i < executionsCount; i++)
            {
                var orxExecution = Recycler!.Borrow<OrxExecution>();
                orxExecution.CopyFrom(executions[i], copyMergeFlags);
                orxExecutionsList.Add(orxExecution);
            }

            ExecutionsList = orxExecutionsList;
        }
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

    protected bool Equals(OrxExecutions other) =>
        ExecutionsList?.SequenceEqual(other.ExecutionsList) ?? other.ExecutionsList == null;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxExecutions)obj);
    }

    public override int GetHashCode() => ExecutionsList != null ? ExecutionsList.GetHashCode() : 0;
}
