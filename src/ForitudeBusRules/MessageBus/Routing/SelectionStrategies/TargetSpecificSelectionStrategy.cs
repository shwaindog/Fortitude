#region

using FortitudeBusRules.MessageBus.Pipelines.Groups;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public class TargetSpecificSelectionStrategy : ISelectionStrategy
{
    public const string StrategyName = "TargetSpecificSelectionStrategy";
    private readonly IRecycler recycler;
    public TargetSpecificSelectionStrategy(IRecycler recycler) => this.recycler = recycler;
    public string Name => StrategyName;

    public RouteSelectionResult? Select(IEventQueueGroupContainer availableEventQueues, IRule senderRule
        , IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        var flags = deploymentOptions.RoutingFlags;
        if (!flags.IsTargetSpecific() || deploymentOptions.SpecificEventQueueName.IsNullOrEmpty()) return null;
        var candidates = availableEventQueues.SelectEventQueues(deploymentOptions.EventGroupType);
        foreach (var eventQueue in candidates)
            if (eventQueue.Name == deploymentOptions.SpecificEventQueueName)
            {
                candidates.DecrementRefCount();
                return new RouteSelectionResult(eventQueue, Name, flags, deployRule);
            }

        candidates.DecrementRefCount();
        return null;
    }

    public IDispatchSelectionResultSet? Select(IEventQueueGroupContainer availableEventQueues, IRule senderRule
        , DispatchOptions dispatchOptions, string destinationAddress)
    {
        var flags = dispatchOptions.RoutingFlags;
        if (!flags.IsTargetSpecific() || dispatchOptions.TargetRule == null) return null;
        var targetRule = dispatchOptions.TargetRule;
        var targetEventQueue = targetRule.Context.RegisteredOn;
        if (!availableEventQueues.Contains(targetEventQueue) ||
            targetRule.LifeCycleState != RuleLifeCycle.Started) return null;
        var routeSelectionResult = new RouteSelectionResult(targetEventQueue, Name, flags, targetRule);
        var dispatchSelectionResultSet = recycler.Borrow<DispatchSelectionResultSet>();
        dispatchSelectionResultSet.StrategyName = Name;
        dispatchSelectionResultSet.DispatchOptions = dispatchOptions;
        dispatchSelectionResultSet.MaxUniqueResults = 1;
        dispatchSelectionResultSet.Add(routeSelectionResult);
        return dispatchSelectionResultSet;
    }
}
