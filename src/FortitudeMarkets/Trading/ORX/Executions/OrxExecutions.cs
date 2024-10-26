#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Executions;

#endregion

namespace FortitudeMarkets.Trading.ORX.Executions;

public class OrxExecutions : ReusableObject<IExecutions>, IExecutions
{
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

    public override IExecutions Clone() => Recycler?.Borrow<OrxExecutions>().CopyFrom(this) ?? new OrxExecutions(this);

    public IEnumerator<IExecution> GetEnumerator() => ExecutionsList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override void StateReset()
    {
        ExecutionsList.Clear();
        base.StateReset();
    }

    public override IExecutions CopyFrom(IExecutions executions, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
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

        return this;
    }

    protected bool Equals(OrxExecutions other) => ExecutionsList?.SequenceEqual(other.ExecutionsList) ?? other.ExecutionsList == null;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxExecutions)obj);
    }

    public override int GetHashCode() => ExecutionsList != null ? ExecutionsList.GetHashCode() : 0;
}
