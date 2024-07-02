// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;

#endregion

namespace FortitudeCommon.DataStructures.Collections;

public class BatchFlowRateEnumerableWrapper<TCollection, TEntry> : IFlowRateEnumerable<TCollection> where TCollection : IReadOnlyCollection<TEntry>
{
    private readonly FlowRate                 flowRate;
    private readonly IEnumerable<TCollection> underlyingEnumerable;

    public BatchFlowRateEnumerableWrapper(FlowRate flowRate, IEnumerable<TCollection> underlyingEnumerable)
    {
        this.flowRate             = flowRate;
        this.underlyingEnumerable = underlyingEnumerable;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ValueTask<TCollection>> GetEnumerator() =>
        new AsyncBatchFlowEnumerator<TCollection, TEntry>(new FlowRegulator(flowRate), underlyingEnumerable.GetEnumerator());
}

public class AsyncBatchFlowEnumerator<TCollection, TEntry> : IEnumerator<ValueTask<TCollection>> where TCollection : IReadOnlyCollection<TEntry>
{
    private readonly FlowRegulator            flowRegulator;
    private readonly IEnumerator<TCollection> underlyingEnumerator;

    public AsyncBatchFlowEnumerator(FlowRegulator flowRegulator, IEnumerator<TCollection> underlyingEnumerator)
    {
        this.flowRegulator        = flowRegulator;
        this.underlyingEnumerator = underlyingEnumerator;
    }

    public bool MoveNext() => underlyingEnumerator.MoveNext();

    public void Reset()
    {
        underlyingEnumerator.Reset();
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        underlyingEnumerator.Dispose();
    }

    public ValueTask<TCollection> Current => GetCurrentRegulated();

    private async ValueTask<TCollection> GetCurrentRegulated()
    {
        var current = underlyingEnumerator.Current;
        for (var i = 0; i < current.Count; i++) await flowRegulator.IncrementEntry();
        return current;
    }
}
