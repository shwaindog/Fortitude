#region

using System.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public interface IDispatchSelectionResultSet : IAutoRecycledObject, IEnumerable<SelectionResult>
{
    bool HasFinished { get; }
    bool HasItems { get; }
    int Count { get; }
    int MaxUniqueResults { get; set; }
    bool Add(SelectionResult selectionResult);
    bool AddRange(IEnumerable<SelectionResult> addRange);
    void Clear();
}

public class DispatchSelectionResultSet : AutoRecycledObject, IDispatchSelectionResultSet
{
    private readonly ReusableList<SelectionResult> backingList = new();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<SelectionResult> GetEnumerator()
    {
        var enumerator = backingList.GetEnumerator();

        return enumerator;
    }

    public int MaxUniqueResults { get; set; }
    public bool HasItems => backingList.Count > 0;
    public int Count => backingList.Count;

    public bool HasFinished => backingList.Count >= MaxUniqueResults;


    public bool Add(SelectionResult selectionResult)
    {
        if (backingList.Count >= MaxUniqueResults) return false;
        var alreadyExists = false;
        foreach (var existing in backingList)
            if (!alreadyExists && selectionResult.EventQueue == existing.EventQueue)
                alreadyExists = true;

        if (!alreadyExists) backingList.Add(selectionResult);
        return !HasFinished;
    }

    public bool AddRange(IEnumerable<SelectionResult> selectionResultRange)
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
        backingList.Clear();
        MaxUniqueResults = 0;
        base.StateReset();
    }
}
