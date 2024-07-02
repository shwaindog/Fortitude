// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;

#endregion

namespace FortitudeCommon.DataStructures.Collections;

public class FlowRateEnumerableWrapper<T> : IFlowRateEnumerable<T>
{
    private readonly FlowRate       flowRate;
    private readonly IEnumerable<T> underlyingEnumerable;

    public FlowRateEnumerableWrapper(FlowRate flowRate, IEnumerable<T> underlyingEnumerable)
    {
        this.flowRate             = flowRate;
        this.underlyingEnumerable = underlyingEnumerable;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ValueTask<T>> GetEnumerator() => new AsyncFlowEnumerator<T>(new FlowRegulator(flowRate), underlyingEnumerable.GetEnumerator());
}

public class AsyncFlowEnumerator<T> : IEnumerator<ValueTask<T>>
{
    private readonly FlowRegulator  flowRegulator;
    private readonly IEnumerator<T> underlyingEnumerator;

    public AsyncFlowEnumerator(FlowRegulator flowRegulator, IEnumerator<T> underlyingEnumerator)
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

    public ValueTask<T> Current => GetCurrentRegulated();

    private async ValueTask<T> GetCurrentRegulated()
    {
        await flowRegulator.IncrementEntry();
        return underlyingEnumerator.Current;
    }
}
