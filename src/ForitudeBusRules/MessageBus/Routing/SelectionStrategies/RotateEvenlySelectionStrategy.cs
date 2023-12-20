#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Pipelines.Groups;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public class RotateEvenlySelectionStrategy : UsesRecycler, ISelectionStrategy
{
    public const string StrategyName = "RotateEvenlySelectionStrategy";
    private readonly IPreviouslyCachedSelections previouslyCachedSelections;

    public RotateEvenlySelectionStrategy(IRecycler recycler, IPreviouslyCachedSelections previouslyCachedSelections)
    {
        Recycler = recycler;
        this.previouslyCachedSelections = previouslyCachedSelections;
    }

    public string Name => StrategyName;

    public RouteSelectionResult? Select(IEventQueueGroupContainer availableEventQueues, IRule senderRule
        , IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        var flags = deploymentOptions.RoutingFlags;
        if (!flags.IsRotateEvenly() || flags.IsRecalculateCache()) return null;
        var previousSelect
            = previouslyCachedSelections.LastDeploySelectionResult(deploymentOptions.EventGroupType, deploymentOptions);
        if (previousSelect == null) return null;
        var availableQueues = availableEventQueues.SelectEventQueues(deploymentOptions.EventGroupType);
        var lastRequestIndex = availableQueues.IndexOf(previousSelect.Value.EventQueue);
        if (lastRequestIndex == -1) return null;
        var nextIndex = (lastRequestIndex + 1) % availableQueues.Count;
        var nextQueue = availableQueues[nextIndex];
        return new RouteSelectionResult(nextQueue, Name, flags, deployRule);
    }

    public IDispatchSelectionResultSet? Select(IEventQueueGroupContainer availableEventQueues, IRule senderRule
        , DispatchOptions dispatchOptions, string destinationAddress)
    {
        var flags = dispatchOptions.RoutingFlags;
        if (!flags.IsRotateEvenly() || flags.IsRecalculateCache()) return null;
        IDispatchSelectionResultSet? foundResult = null;
        if (!flags.IsSendToAll() &&
            (flags.IsSenderCacheLast() || (!flags.IsSenderCacheLast() && !flags.IsDestinationCacheLast())))
        {
            var previousDispatchSelectionResult
                = previouslyCachedSelections.LastSenderDestinationSelectionResultSet(senderRule, destinationAddress
                    , dispatchOptions);
            foundResult = GetNextDispatchSelectionResultSet(availableEventQueues, dispatchOptions, destinationAddress
                , previousDispatchSelectionResult);
        }

        if (foundResult != null || flags.IsSendToAll() || !flags.IsDestinationCacheLast()) return foundResult;
        var previousDestinationResult
            = previouslyCachedSelections.LastDestinationSelectionResultSet(destinationAddress, dispatchOptions);
        foundResult = GetNextDispatchSelectionResultSet(availableEventQueues, dispatchOptions, destinationAddress
            , previousDestinationResult);
        return foundResult;
    }

    private IDispatchSelectionResultSet? GetNextDispatchSelectionResultSet(
        IEventQueueGroupContainer availableEventQueues
        , DispatchOptions dispatchOptions, string destinationAddress
        , IDispatchSelectionResultSet? previousDispatchSelectionResult)
    {
        if (previousDispatchSelectionResult == null || previousDispatchSelectionResult.Count != 1) return null;
        var previousSelection = previousDispatchSelectionResult.First();
        var previousEventQueue = previousSelection.EventQueue;
        var previousRule = previousSelection.Rule;
        var currentListeningRules = Recycler?.Borrow<InsertionOrderSet<IRule>>() ?? new InsertionOrderSet<IRule>();
        previousEventQueue.RulesListeningToAddress(currentListeningRules, destinationAddress);
        var previousRuleIndex = previousRule != null ? currentListeningRules.IndexOf(previousRule) : -1;
        if (++previousRuleIndex < currentListeningRules.Count)
        {
            var returnDispatchResult = GetRuleAtIndexReturnDispatchSelectionResultSet(dispatchOptions
                , currentListeningRules
                , previousRuleIndex, previousEventQueue);
            currentListeningRules.DecrementRefCount();
            return returnDispatchResult;
        }

        currentListeningRules.Clear();
        var nextEventQueue = GetNextListeningEventQueue(availableEventQueues, dispatchOptions, destinationAddress
            , previousEventQueue);

        nextEventQueue.RulesListeningToAddress(currentListeningRules, destinationAddress);
        if (currentListeningRules.Count > 0)
        {
            var returnDispatchResult
                = GetRuleAtIndexReturnDispatchSelectionResultSet(dispatchOptions, currentListeningRules, 0
                    , nextEventQueue);
            currentListeningRules.DecrementRefCount();
            return returnDispatchResult;
        }

        currentListeningRules.DecrementRefCount();
        return null;
    }

    private static IEventQueue GetNextListeningEventQueue(IEventQueueGroupContainer availableEventQueues
        , DispatchOptions dispatchOptions, string destinationAddress, IEventQueue previousEventQueue)
    {
        var availableQueues
            = availableEventQueues.SelectEventQueues(dispatchOptions.TargetEventQueueType);

        var indexOfPrevious = availableQueues.IndexOf(previousEventQueue);
        var availableQueuesCount = availableQueues.Count;
        var attemptCount = 0;
        var indexOfNextQueue = indexOfPrevious % availableQueuesCount;
        IEventQueue nextEventQueue;
        do
        {
            indexOfNextQueue = (indexOfNextQueue + 1) % availableQueuesCount;
            nextEventQueue = availableQueues[indexOfNextQueue];
        } while (!nextEventQueue.IsListeningToAddress(destinationAddress) &&
                 ++attemptCount < availableQueuesCount);

        availableQueues.DecrementRefCount();
        return nextEventQueue;
    }

    private DispatchSelectionResultSet GetRuleAtIndexReturnDispatchSelectionResultSet(DispatchOptions dispatchOptions
        , InsertionOrderSet<IRule> currentListeningRules, int ruleAtIndex, IEventQueue processEventQueue)
    {
        var nextRule = currentListeningRules[ruleAtIndex];
        var returnDispatchResult = Recycler?.Borrow<DispatchSelectionResultSet>() ?? new DispatchSelectionResultSet();
        returnDispatchResult.MaxUniqueResults = 1;
        returnDispatchResult.DispatchOptions = dispatchOptions;
        returnDispatchResult.StrategyName = Name;
        returnDispatchResult.Add(new RouteSelectionResult(processEventQueue, Name
            , dispatchOptions.RoutingFlags, nextRule));
        return returnDispatchResult;
    }
}
