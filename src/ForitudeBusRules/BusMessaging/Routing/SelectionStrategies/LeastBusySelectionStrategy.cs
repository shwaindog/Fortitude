// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

public class LeastBusySelectionStrategy : UsesRecycler, ISelectionStrategy
{
    public const string StrategyName = "LeastBusySelectionStrategy";
    public LeastBusySelectionStrategy(IRecycler recycler) => Recycler = recycler;
    public string Name => StrategyName;

    public RouteSelectionResult? Select
    (IMessageQueueGroupContainer availableMessageQueues, IRule senderRule
      , IRule deployRule
      , DeploymentOptions deploymentOptions)
    {
        RouteSelectionResult? found = null;

        var flags            = deploymentOptions.RoutingFlags;
        var selectFromQueues = availableMessageQueues.SelectEventQueues(deploymentOptions.MessageGroupType);

        selectFromQueues.Sort(); // IEventQueue has IComparable<IEventQueue> on RecentlyReceivedMessagesCount
        if (flags.IsPreferNotSenderQueue() && senderRule != Rule.NoKnownSender)
        {
            var indexOfSenderQueue = selectFromQueues.IndexOf(senderRule.Context.RegisteredOn);
            if (indexOfSenderQueue != -1) selectFromQueues.ShiftToEnd(indexOfSenderQueue);
        }

        foreach (var eventQueue in selectFromQueues)
            if (found == null)
                // need to exhaust Enumerator to get it to recycle and not create Garbage
                found = new RouteSelectionResult(eventQueue, Name, deploymentOptions.RoutingFlags, deployRule);
        selectFromQueues.DecrementRefCount();
        return found;
    }

    public IDispatchSelectionResultSet? Select
    (IMessageQueueGroupContainer availableMessageQueues
      , IRule senderRule
      , DispatchOptions dispatchOptions
      , string destinationAddress)
    {
        var flags = dispatchOptions.RoutingFlags;
        if (flags.IsSendToAll()) return null;

        IDispatchSelectionResultSet results = Recycler?.Borrow<DispatchSelectionResultSet>() ??
                                              new DispatchSelectionResultSet();
        results.DispatchOptions  = dispatchOptions;
        results.StrategyName     = Name;
        results.MaxUniqueResults = 1;
        var selectFromQueues = availableMessageQueues.SelectEventQueues(dispatchOptions.TargetMessageQueueType);
        selectFromQueues.Sort(); // IEventQueue has IComparable<IEventQueue> on RecentlyReceivedMessagesCount
        if (flags.IsPreferNotSenderQueue())
        {
            var indexOfSenderQueue = selectFromQueues.IndexOf(senderRule.Context.RegisteredOn);
            if (indexOfSenderQueue != -1) selectFromQueues.ShiftToEnd(indexOfSenderQueue);
        }

        foreach (var eventQueue in selectFromQueues)
            if (!results.HasFinished)
                if (eventQueue.IsListeningToAddress(destinationAddress))
                {
                    if (flags.IsSendToOne())
                    {
                        var currentListeningRules = Recycler?.Borrow<InsertionOrderSet<IRule>>() ??
                                                    new InsertionOrderSet<IRule>();
                        eventQueue.RulesListeningToAddress(currentListeningRules, destinationAddress);
                        if (currentListeningRules.Count > 0)
                        {
                            var firstRule = currentListeningRules[0];
                            currentListeningRules.DecrementRefCount();
                            results.Add(new RouteSelectionResult(eventQueue, Name
                                                               , dispatchOptions.RoutingFlags, firstRule));
                        }
                        else
                        {
                            currentListeningRules.DecrementRefCount();
                        }
                    }
                    else
                    {
                        // need to exhaust Enumerator to get it to recycle and not create Garbage
                        results.Add(
                                    new RouteSelectionResult(eventQueue, Name, dispatchOptions.RoutingFlags));
                    }
                }

        selectFromQueues.DecrementRefCount();
        return results;
    }
}
