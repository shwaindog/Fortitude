#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public class EventQueueTypeOrderSelectionStrategy : RecyclableObject, ISelectionStrategy
{
    public string Name => "EventQueueTypeOrderStrategy";

    public SelectionResult? Select(IReusableList<IEventQueue> availableEventQueues, IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        SelectionResult? found = null;
        foreach (var eventQueue in availableEventQueues)
            if (found == null && (eventQueue.QueueType & deploymentOptions.EventGroupType) != 0)
                // need to exhaust Enumerator to get it to recycle and not create Garbage
                found = new SelectionResult(eventQueue, Name, deploymentOptions.RoutingStrategySelectionFlags);
        return found;
    }

    public IDispatchSelectionResultSet Select(IReusableList<IEventQueue> availableEventQueues
        , IRule senderRule
        , DispatchOptions dispatchOptions
        , string? destinationAddress = null)
    {
        IDispatchSelectionResultSet results = Recycler?.Borrow<DispatchSelectionResultSet>() ??
                                              new DispatchSelectionResultSet();
        var flags = dispatchOptions.RoutingStrategySelectionFlags;

        results.MaxUniqueResults = flags.IsSelectAllMatching() && flags.IsSendToAll() ? availableEventQueues.Count : 1;
        foreach (var eventQueue in availableEventQueues)
            if (!results.HasFinished)
                if ((eventQueue.QueueType & dispatchOptions.TargetEventQueueType) != 0 &&
                    (destinationAddress == null || eventQueue.IsListeningToAddress(destinationAddress)))
                    // need to exhaust Enumerator to get it to recycle and not create Garbage
                    results.Add(new SelectionResult(eventQueue, Name, dispatchOptions.RoutingStrategySelectionFlags));

        return results;
    }
}
