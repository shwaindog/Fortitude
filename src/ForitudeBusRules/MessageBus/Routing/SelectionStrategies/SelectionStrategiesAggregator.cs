#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public class SelectionStrategiesAggregator : RecyclableObject, ISelectionStrategy
{
    private readonly ReusableList<ISelectionStrategy> backingList = new();

    private ISaveSelectionResult? CacheResult { get; set; }

    public string Name => "StrategySelector";

    public SelectionResult? Select(IReusableList<IEventQueue> availableEventQueues, IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        SelectionResult? found = null;
        foreach (var selectionStrategy in backingList)
            if (found == null)
            {
                var checkResult = selectionStrategy
                    .Select(availableEventQueues, senderRule, deployRule, deploymentOptions);
                if (checkResult.HasValue) found = checkResult;
            }

        if (found != null) CacheResult?.Save(senderRule, deployRule, deploymentOptions, found.Value);
        DecrementRefCount();
        return found;
    }

    public IDispatchSelectionResultSet Select(IReusableList<IEventQueue> availableEventQueues
        , IRule senderRule
        , DispatchOptions dispatchOptions
        , string? destinationAddress = null)
    {
        IDispatchSelectionResultSet? found = null;
        foreach (var selectionStrategy in backingList)
            if (found is not { HasFinished: true })
            {
                var checkResult = selectionStrategy
                    .Select(availableEventQueues, senderRule, dispatchOptions, destinationAddress);
                if (checkResult.HasItems) found = checkResult;
            }

        if (found is { HasFinished: true }) CacheResult?.Save(senderRule, dispatchOptions, destinationAddress, found);
        DecrementRefCount();
        return found ?? Recycler?.Borrow<DispatchSelectionResultSet>() ?? new DispatchSelectionResultSet();
    }

    public override void StateReset()
    {
        CacheResult = null;
        backingList.Clear();
        base.StateReset();
    }

    public SelectionStrategiesAggregator Add(ISelectionStrategy strategy)
    {
        backingList.Add(strategy);
        return this;
    }

    public SelectionStrategiesAggregator AddRange(IEnumerable<ISelectionStrategy> strategy)
    {
        backingList.AddRange(strategy);
        return this;
    }

    public SelectionStrategiesAggregator Remove(ISelectionStrategy toRemove)
    {
        backingList.Remove(toRemove);
        return this;
    }

    public SelectionStrategiesAggregator Clear()
    {
        backingList.Clear();
        return this;
    }
}
