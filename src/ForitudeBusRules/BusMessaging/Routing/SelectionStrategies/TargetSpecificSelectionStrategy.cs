﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

public class TargetSpecificSelectionStrategy : ISelectionStrategy
{
    public const string StrategyName = "TargetSpecificSelectionStrategy";

    private readonly IRecycler recycler;
    public TargetSpecificSelectionStrategy(IRecycler recycler) => this.recycler = recycler;
    public string Name => StrategyName;

    public RouteSelectionResult? Select
        (IMessageQueueGroupContainer availableMessageQueues, IRule senderRule, IRule deployRule, DeploymentOptions deploymentOptions)
    {
        var flags = deploymentOptions.RoutingFlags;
        if (!flags.IsTargetSpecific() || deploymentOptions.SpecificEventQueueName.IsNullOrEmpty()) return null;
        var candidates = availableMessageQueues.SelectEventQueues(deploymentOptions.MessageGroupType);
        foreach (var eventQueue in candidates)
            if (eventQueue.Name == deploymentOptions.SpecificEventQueueName)
            {
                candidates.DecrementRefCount();
                return new RouteSelectionResult(eventQueue, Name, flags, deployRule);
            }

        candidates.DecrementRefCount();
        return null;
    }

    public IDispatchSelectionResultSet? Select
        (IMessageQueueGroupContainer availableMessageQueues, IRule senderRule, DispatchOptions dispatchOptions, string destinationAddress)
    {
        var flags = dispatchOptions.RoutingFlags;
        if (!flags.IsTargetSpecific() || dispatchOptions.TargetRule == null) return null;
        var targetRule       = dispatchOptions.TargetRule;
        var targetEventQueue = targetRule.Context.RegisteredOn;
        if (!availableMessageQueues.Contains(targetEventQueue) ||
            targetRule.LifeCycleState is not (RuleLifeCycle.Started or RuleLifeCycle.Starting))
            return null;
        var routeSelectionResult       = new RouteSelectionResult(targetEventQueue, Name, flags, targetRule);
        var dispatchSelectionResultSet = recycler.Borrow<DispatchSelectionResultSet>();
        dispatchSelectionResultSet.StrategyName     = Name;
        dispatchSelectionResultSet.DispatchOptions  = dispatchOptions;
        dispatchSelectionResultSet.MaxUniqueResults = 1;
        dispatchSelectionResultSet.Add(routeSelectionResult);
        return dispatchSelectionResultSet;
    }
}
