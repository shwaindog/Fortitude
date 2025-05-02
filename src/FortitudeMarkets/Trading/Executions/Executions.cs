// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Executions;

public class Executions : ReusableObject<IExecutions>, IExecutions
{
    private IList<IExecution> executions;
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

    public override void StateReset()
    {
        base.StateReset();
        executions.Clear();
    }

    public override IExecutions Clone() => Recycler?.Borrow<Executions>().CopyFrom(this) ?? new Executions(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IExecution> GetEnumerator() => executions.GetEnumerator();

    public override IExecutions CopyFrom(IExecutions source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        executions = source.ToList();
        return this;
    }
}
