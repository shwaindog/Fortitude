#region

using System.Collections;
using FortitudeBusRules.Messaging;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public interface IDispatchSelectionResultSet : IReusableObject<IDispatchSelectionResultSet>
    , IEnumerable<RouteSelectionResult>
{
    bool HasFinished { get; }
    bool HasItems { get; }
    int Count { get; }
    int MaxUniqueResults { get; set; }
    public string? StrategyName { get; set; }
    DispatchOptions DispatchOptions { get; set; }
    bool Add(RouteSelectionResult routeSelectionResult);
    bool AddRange(IEnumerable<RouteSelectionResult> addRange);
    void Clear();
}

public class DispatchSelectionResultSet : ReusableObject<IDispatchSelectionResultSet>, IDispatchSelectionResultSet
{
    private readonly ReusableList<RouteSelectionResult> backingList = new();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<RouteSelectionResult> GetEnumerator()
    {
        var enumerator = backingList.GetEnumerator();
        return enumerator;
    }

    public string? StrategyName { get; set; }
    public int MaxUniqueResults { get; set; }

    public DispatchOptions DispatchOptions { get; set; }
    public bool HasItems => backingList.Count > 0;
    public int Count => backingList.Count;

    public bool HasFinished => backingList.Count >= MaxUniqueResults;

    public bool Add(RouteSelectionResult routeSelectionResult)
    {
        if (backingList.Count >= MaxUniqueResults) return false;
        var alreadyExists = false;
        foreach (var existing in backingList)
            if (!alreadyExists && routeSelectionResult.EventQueue == existing.EventQueue)
                alreadyExists = true;

        if (!alreadyExists) backingList.Add(routeSelectionResult);
        return !HasFinished;
    }

    public bool AddRange(IEnumerable<RouteSelectionResult> selectionResultRange)
    {
        var keepGoing = backingList.Count < MaxUniqueResults;
        foreach (var toAdd in selectionResultRange)
            if (keepGoing)
                keepGoing = Add(toAdd);
        return keepGoing;
    }

    public void Clear()
    {
        backingList.Clear();
    }

    public override void StateReset()
    {
        DispatchOptions = default;
        backingList.Clear();
        MaxUniqueResults = 0;
        StrategyName = null;
        base.StateReset();
    }

    public override IDispatchSelectionResultSet CopyFrom(IDispatchSelectionResultSet source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is DispatchSelectionResultSet dispatchSelectionResultSet)
        {
            MaxUniqueResults = dispatchSelectionResultSet.MaxUniqueResults;
            StrategyName = dispatchSelectionResultSet.StrategyName;
            DispatchOptions = dispatchSelectionResultSet.DispatchOptions;
            backingList.Clear();
            backingList.AddRange(dispatchSelectionResultSet.backingList);
        }

        return this;
    }

    public override IDispatchSelectionResultSet Clone() =>
        Recycler?.Borrow<DispatchSelectionResultSet>().CopyFrom(this) ??
        new DispatchSelectionResultSet().CopyFrom(this);
}
