#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

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

    public RouteSelectionResult? Select(IMessageQueueGroupContainer availableMessageQueues, IRule senderRule
        , IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        var flags = deploymentOptions.RoutingFlags;
        if (!flags.IsRotateEvenly() || flags.IsRecalculateCache()) return null;
        var previousSelect
            = previouslyCachedSelections.LastDeploySelectionResult(deploymentOptions.MessageGroupType, deploymentOptions);
        if (previousSelect == null) return null;
        var availableQueues = availableMessageQueues.SelectEventQueues(deploymentOptions.MessageGroupType);
        var lastRequestIndex = availableQueues.IndexOf(previousSelect.Value.MessageQueue);
        if (lastRequestIndex == -1) return null;
        var nextIndex = (lastRequestIndex + 1) % availableQueues.Count;
        var nextQueue = availableQueues[nextIndex];
        return new RouteSelectionResult(nextQueue, Name, flags, deployRule);
    }

    public IDispatchSelectionResultSet? Select(IMessageQueueGroupContainer availableMessageQueues, IRule senderRule
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
            foundResult = GetNextDispatchSelectionResultSet(availableMessageQueues, dispatchOptions, destinationAddress
                , previousDispatchSelectionResult);
        }

        if (foundResult != null || flags.IsSendToAll() || !flags.IsDestinationCacheLast()) return foundResult;
        var previousDestinationResult
            = previouslyCachedSelections.LastDestinationSelectionResultSet(destinationAddress, dispatchOptions);
        foundResult = GetNextDispatchSelectionResultSet(availableMessageQueues, dispatchOptions, destinationAddress
            , previousDestinationResult);
        return foundResult;
    }

    private IDispatchSelectionResultSet? GetNextDispatchSelectionResultSet(
        IMessageQueueGroupContainer availableMessageQueues
        , DispatchOptions dispatchOptions, string destinationAddress
        , IDispatchSelectionResultSet? previousDispatchSelectionResult)
    {
        if (previousDispatchSelectionResult == null || previousDispatchSelectionResult.Count != 1) return null;
        var previousSelection = previousDispatchSelectionResult.First();
        var previousEventQueue = previousSelection.MessageQueue;
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
        var nextEventQueue = GetNextListeningEventQueue(availableMessageQueues, dispatchOptions, destinationAddress
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

    private static IMessageQueue GetNextListeningEventQueue(IMessageQueueGroupContainer availableMessageQueues
        , DispatchOptions dispatchOptions, string destinationAddress, IMessageQueue previousMessageQueue)
    {
        var availableQueues
            = availableMessageQueues.SelectEventQueues(dispatchOptions.TargetMessageQueueType);

        var indexOfPrevious = availableQueues.IndexOf(previousMessageQueue);
        var availableQueuesCount = availableQueues.Count;
        var attemptCount = 0;
        var indexOfNextQueue = indexOfPrevious % availableQueuesCount;
        IMessageQueue nextMessageQueue;
        do
        {
            indexOfNextQueue = (indexOfNextQueue + 1) % availableQueuesCount;
            nextMessageQueue = availableQueues[indexOfNextQueue];
        } while (!nextMessageQueue.IsListeningToAddress(destinationAddress) &&
                 ++attemptCount < availableQueuesCount);

        availableQueues.DecrementRefCount();
        return nextMessageQueue;
    }

    private DispatchSelectionResultSet GetRuleAtIndexReturnDispatchSelectionResultSet(DispatchOptions dispatchOptions
        , InsertionOrderSet<IRule> currentListeningRules, int ruleAtIndex, IMessageQueue processMessageQueue)
    {
        var nextRule = currentListeningRules[ruleAtIndex];
        var returnDispatchResult = Recycler?.Borrow<DispatchSelectionResultSet>() ?? new DispatchSelectionResultSet();
        returnDispatchResult.MaxUniqueResults = 1;
        returnDispatchResult.DispatchOptions = dispatchOptions;
        returnDispatchResult.StrategyName = Name;
        returnDispatchResult.Add(new RouteSelectionResult(processMessageQueue, Name
            , dispatchOptions.RoutingFlags, nextRule));
        return returnDispatchResult;
    }
}
