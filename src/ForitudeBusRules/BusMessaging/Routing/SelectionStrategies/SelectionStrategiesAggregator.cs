#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

public class SelectionStrategiesAggregator : AutoRecycledObject, ISelectionStrategy
{
    private readonly ReusableList<ISelectionStrategy> backingList = new();

    public ISaveSelectionResult? CacheResult { get; set; }

    public string Name => "StrategySelector";

    public RouteSelectionResult? Select(IMessageQueueGroupContainer availableMessageQueues, IRule senderRule
        , IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        RouteSelectionResult? found = null;
        foreach (var selectionStrategy in backingList)
            if (found == null)
            {
                var checkResult = selectionStrategy
                    .Select(availableMessageQueues, senderRule, deployRule, deploymentOptions);
                if (checkResult.HasValue) found = checkResult;
            }

        if (found != null) CacheResult?.Save(senderRule, deployRule, deploymentOptions, found.Value);
        DecrementRefCount();
        return found;
    }

    public IDispatchSelectionResultSet Select(IMessageQueueGroupContainer availableMessageQueues
        , IRule senderRule
        , DispatchOptions dispatchOptions
        , string destinationAddress)
    {
        IDispatchSelectionResultSet? found = null;
        foreach (var selectionStrategy in backingList)
            if (found is not { HasItems: true })
            {
                var checkResult = selectionStrategy
                    .Select(availableMessageQueues, senderRule, dispatchOptions, destinationAddress);
                if (checkResult?.HasItems == true) found = checkResult;
            }

        if (found is { HasItems: true }) CacheResult?.Save(senderRule, dispatchOptions, destinationAddress, found);
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
