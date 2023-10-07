#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Executions;

public class OrxExecutions : IExecutions
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

    public IExecutions Clone() => new OrxExecutions(this);

    public IEnumerator<IExecution> GetEnumerator() => ExecutionsList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CopyFrom(IExecutions executions, IRecycler recycler)
    {
        var executionsCount = executions.Count;
        if (executionsCount > 0)
        {
            var orxExecutionsList = recycler.Borrow<List<OrxExecution>>();
            orxExecutionsList.Clear();
            for (var i = 0; i < executionsCount; i++)
            {
                var orxExecution = recycler.Borrow<OrxExecution>();
                orxExecution.CopyFrom(executions[i], recycler);
                orxExecutionsList.Add(orxExecution);
            }

            ExecutionsList = orxExecutionsList;
        }
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
